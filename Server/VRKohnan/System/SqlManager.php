<?php
namespace VRKohnan\System;

/**
 * SQL管理クラス.
 * 
 * @author dO
 */
class SqlManager {
	/** キャッシュ操作なしフラグ */
	const CACHE_NONE		= 0x00;
	/** キャッシュの設定&取得フラグ */
	const CACHE_GETSET	= 0x01;
	/** キャッシュの削除フラグ */
	const CACHE_DELETE	= 0x02;
	
	private $dsnTable = array();
	
	/** DSN名 */
	private $dsnName = '';
	
	/** PDOオブジェクト */
	private $pdo = null;
	
	public function __construct()
	{
		$this->dsnTable = parse_ini_file( 'VRKohnan/Config/db.ini', true );
	}
	
	/** DBへの接続 */
	public function connect($dsnName)
	{
		$this->close();
		
		try {
			$this->dsnName = $dsnName;
			
			$dsn = $this->dsnTable[$dsnName];
			$dsn_string	= 'mysql:host='.$dsn['host'] .'; dbname='.$dsn['database'] .'; charset='.$dsn['charset'];
			$this->pdo = new \PDO(
					$dsn_string,
					$dsn['user'], $dsn['pass']);
			
			// PDO設定.
			$this->pdo->setAttribute(\PDO::ATTR_EMULATE_PREPARES, false);
			$this->pdo->setAttribute(\PDO::ATTR_ERRMODE, \PDO::ERRMODE_EXCEPTION);

		} catch(\Exception $e) {
			// TOOD: ロギング.
			throw $e;
		}
	}
	
	/** DBへの接続解除 */
	public function close()
	{
		$this->pdo = null;
	}
	
	/**
	 * DBへのSQLリクエスト.
	 * 
	 * @param string $tableName テーブル名.
	 * @param string $queryName クエリ名.
	 * @param array $params バインドパラメータ.
	 * @return array クエリ結果配列.
	 * @return null 取得データなし.
	 */
	public function execute( $tableName, $queryName, $params = array() )
	{
		// TODO: キャッシュ処理.

		// クエリ取得.
		$sql = $this->buildListQuery( \VRKohnan\SqlTable::get($tableName, $queryName) , $params);
		
		// プリペアドステートメントの作成.
		$statement = $this->pdo->prepare($sql);
		$this->bindParameter($statement, $params, $tableName, $queryName);

		if (!$statement->execute()) {
			// エラー情報を取得.
			ob_start();
			var_dump($statement->errorInfo());
			$errInfo =ob_get_contents();
			ob_end_clean();
			throw new \Exception('QueryError('.$this->dsnName.') Table:'.$tableName.' Query:'.$queryName.' ErrInfo:'.$errInfo);
		}
		$fetch = $statement->fetchAll(\PDO::FETCH_ASSOC);
		$cnt = count($fetch);
		if ($cnt == 1) {
			$fetch = $fetch[0];
		} else if ($cnt == 0) {
			$fetch = null;
		}

		return $fetch;
	}
	
	/** トランザクションの開始 */
	public function beginTransaction()
	{
		try {
			$this->pdo->beginTransaction();
		} catch (\Exception $ex) {
			throw $ex;
		}
	}
	
	/** トランザクション結果をコミット */
	public function commit()
	{
		try {
			$this->pdo->commit();
		} catch (\Exception $ex) {
			throw $ex;
		}
	}
	
	/** トランザクションのロールバック */
	public function rollback()
	{
		if (!is_null($this->pdo)) {
			$this->pdo->rollBack();
		}
	}
	
	/**
	 * プリペアドステートメントのパラメータをバインド.
	 * @param PDOStatement $statement プリペアドステートメント
	 * @param array $params パラメータ配列
	 * @throws \Exception
	 */
	private function bindParameter($statement, $params, $tableName, $queryName)
	{
		foreach ($params as $key => $val) {
			// パラメータ値が配列の場合は複数行対応処理.
			if (is_array($val)) {
				// 先頭からバインドすると小さい番号に大きい番号が食いつぶされるので逆順に処理
				// 例： :user_id1 = 100
				//      :user_id1, :user_id10 →(bind) 100, 1000
				for ($i = count($val) - 1; $i >= 0; --$i) {
					$rowParams = $val[$i];
					foreach ($rowParams as $rowKey => $rowVal) {
						if (!$statement->bindValue(':'.$rowKey.$i, $rowVal)) {
							throw new \Exception('Statement bindValue failed. Table:'.$tableName.' Query:'.$queryName.' Value:'.$rowKey.'='.$rowVal);
						}
					}
				}
			} else {
				if (!$statement->bindValue(':'.$key, $val)) {
					throw new \Exception('Statement bindValue failed. Table:'.$tableName.' Query:'.$queryName.' Value:'.$key.'='.$val);
				}
			}
		}
	}
	
	/**
	 * 複数行構文を処理してクエリ文を構築.
	 * 
	 * @param string $query 変換前のクエリ
	 * @param array $params パラメータ配列
	 * @return string 複数行クエリへ変換したクエリ文
	 */
	private function buildListQuery($query, $params)
	{
		$loop_start = strpos($query, ':list<');
		$loop_end = strpos($query, '>');
		if (($loop_start !== false) && ($loop_end !== false)) {
			// :list<,>を除いた範囲を取得
			$start = ($loop_start + 6);
			$length = $loop_end - $start;

			$loopQuery = substr($query, $start, $length);

			$conbineLoopQuery = '';

			$loopCnt = count($params['list']);
			if ($loopCnt > 0) {
				$keys = array_keys($params['list'][0]);
			}
			for ($i = 0; $i < $loopCnt; ++$i)
			{
				$replaceLoopQuery = $loopQuery;
				foreach ($keys as $key) {
					$replaceLoopQuery = str_replace(':'.$key, ':'.$key.$i, $replaceLoopQuery);
				}
				if ($i > 0) { $conbineLoopQuery .= ', '; }
				$conbineLoopQuery .= '('.$replaceLoopQuery.')';
			}
			
			$loopQuery = substr($query, $loop_start, ($loop_end - $loop_start + 1));
			$query = str_replace($loopQuery, $conbineLoopQuery, $query);
		}

		return $query;
	}
	
}
