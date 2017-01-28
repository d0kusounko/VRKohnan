<?php
namespace VRKohnan\Action;

/**
 * ランキング取得アクション.
 * 
 * @author dO
 */
class GetRanking extends GameAction {
	/**
	 * バイナリのリクエストデータを連想配列に変換.
	 * @param binary $requestBinary バイナリのリクエストデータ.
     * @return array
	 */
	protected function unpackRequestData($requestBinary)
	{
		$res = array(
			'num'	=> 0,	// 取得数.
		);

		if (!is_null($requestBinary)) {
			$decode = unpack('Vnum',  $requestBinary);

			$res['num']	= $decode['num'];
		}
		
		return $res;
	}
	
	/** 実行処理 */
	protected function onExecute($view)
	{
		$request = $this->getRequest();
		
		$sqlManager = new \VRKohnan\System\SqlManager();
		$sqlManager->connect('slave');
		
		// ランキングデータの取得.
		$ranking_array = $sqlManager->execute('ranking', 'get', $request);
		
		// 多次元配列に統一.
		if (empty($ranking_array)) { $ranking_array = array(); }
		if (array_values($ranking_array) !== $ranking_array) {
			$ranking_array = array($ranking_array);
		}

		// スレーブ接続解除.
		$sqlManager->close();

		// レスポンスデータ設定.
		$view->addPackData( strval( count( $ranking_array ) ) , 'V');
		foreach ( $ranking_array as $ranking )
		{
			$view->addPackData( $ranking['score'], 'V' );
			$view->addPackData( $ranking['name'], 'A3');
		}
	}
}
