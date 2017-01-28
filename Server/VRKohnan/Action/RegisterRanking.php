<?php
namespace VRKohnan\Action;

/**
 * ランキング登録のアクション.
 * 
 * @author dO
 */
class RegisterRanking extends GameAction {

	/**
	 * バイナリのリクエストデータを連想配列に変換.
	 * @param binary $requestBinary バイナリのリクエストデータ.
	 */
	protected function unpackRequestData($requestBinary)
	{
		$res = array(
			'score'	=> 0,	// スコア.
			'name'	=> '',	// 名前.
		);
		
		if (!is_null($requestBinary)) {
			$decode = unpack('Vscore/a3name', $requestBinary);

			$res['score']				= $decode['score'];
			$res['name']				= $decode['name'];
		}
		
		return $res;
	}
	
	/** 実行処理 */
	protected function onExecute($view)
	{
		$request = $this->getRequest();

		$isValidData = true;
		{
			// リクエストデータの正規チェック.
			if( strlen( $request['name'] ) != 3 )
			{
				// 名前が三文字で無い.
				$isValidData	= false;
			}
		}
		
		// 正常なリクエストデータの時だけ登録.
		$isRegistered	= false;

		if ($isValidData) {
			// マスター接続.
			$sqlManager = new \VRKohnan\System\SqlManager();
			$sqlManager->connect('master');

			try
			{
				// トランザクション開始.
				$sqlManager->beginTransaction();

				// ランキングデータ登録.
				$sqlManager->execute( 'ranking', 'register', $request );

				// トランザクションコミット.
				$sqlManager->commit();

				$isRegistered	= true;
			}
			catch ( \Exception $e )
			{
				// トランザクション失敗によるロールバック.
				$sqlManager->rollback();
				throw $e;
			}
		}
		
		// レスポンスデータ設定.
		if( $isRegistered )
		{
			// ランキング登録成功.
			$view->addPackData( 1, 'c' );
		}
		else
		{
			// ランキング登録失敗.
			$view->addPackData( 0, 'c' );
		}
	}
}
