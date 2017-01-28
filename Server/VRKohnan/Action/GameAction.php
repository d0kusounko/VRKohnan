<?php
namespace VRKohnan\Action;

/**
 * ゲームクライアントからのリクエスト処理用Action基底.
 * 
 * @author dO
 */
abstract class GameAction extends \VRKohnan\Action\Action {
	/** リクエストデータ */
	private $request = array();
	
	public function __construct($controller)
	{
		parent::__construct($controller);
		
		// リクエストデータをバイナリから連想配列に変換.
		if (defined('STDIN')) {
			global $argv;
			$binaryParam	= hex2bin( $argv[3] );
		}
		else
		{
			$binaryParam	= filter_input(INPUT_POST, 'data');
		}

		$this->request = $this->unpackRequestData($binaryParam);
	}
	
	/**
	 * バイナリのリクエストデータを連想配列に変換.
	 * @param binary $requestBinary バイナリのリクエストデータ.
	 */
	abstract protected function unpackRequestData($requestBinary);
	
	/**
	 * リクエストデータの取得
	 * @return array リクエストデータ
	 */
	protected function getRequest()
	{
		return $this->request;
	}
}
