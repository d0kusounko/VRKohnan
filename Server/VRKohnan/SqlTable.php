<?php
namespace VRKohnan;

/**
 * SQLクエリテーブル.
 * 
 * テーブルごとにクエリを定義しています.
 * 基本的にはPDOのプリペアドステートメントに従ったクエリ構文になっています.
 * 特殊処理として:list<>で囲んだ部分をパラメータ数分繰り返し、複数行のプリペアドステートメントに変換します.
 * 
 * 例：
 * query = INSERT table (val, create_at) VALUES :list<:val now()>
 * param = array('link' => array( array('val' = 0), array('val' = 1) ))
 * ↓
 * INSERT table (val, create_at) VALUES (:val0, now()), (:val1, now())
 * 
 * @author dO
 */
class SqlTable {
	
	/** クエリテーブル */
	private static $table = array(
		//--------------------------------------------------
		// マスタデータ.
		//--------------------------------------------------
		
		//--------------------------------------------------
		// トランザクションデータ.
		//--------------------------------------------------
		'ranking' => array(
			// 指定数のランキングデータをスコア降順で取得.
			'get'					=> 'SELECT * FROM t_ranking ORDER BY score DESC LIMIT :num',
			// 全ランキングデータをスコア降順で取得.
			'get_all'				=> 'SELECT * FROM t_ranking ORDER BY score DESC',
			// ランキングデータを登録.
			'register'				=> 'INSERT t_ranking ( score, name ) VALUES( :score, :name )',
			// 'register'				=> 'INSERT t_ranking ( score, name ) VALUES :list< :score, :name >',
		),
	);
	
	/**
	 * クエリを取得.
	 * 
	 * @param string $tableName テーブル名.
	 * @param string $query クエリ名.
	 * @return string クエリ.
	 */
	public static function get($tableName, $query)
	{
		if (key_exists($tableName, self::$table)) {
			$queries = self::$table[$tableName];
			if (key_exists($query, $queries)) {
				return $queries[$query];
			}
		}
		throw new \Exception('NotFoundQuery table:'.$tableName.' query:'.$query);
	}
}

