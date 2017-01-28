<?php
namespace VRKohnan\Controller;

/**
 * Gameクライアントからの処理用Controller
 * 
 * @author dO
 */
class GameController extends \VRKohnan\Controller\Controller {
	/**
	 * コンストラクタ.
	 * @param string $actionName アクション名.
	 */
	public function __construct($actionName)
	{
		parent::__construct($actionName);
	}
	
	/**
	 * Actionクラスの生成
	 * @param string $actionName アクション名.
	 */
	protected function createAction($actionName)
	{
		$actionClass = 'VRKohnan\\Action\\' . $actionName;
		
		return new $actionClass($this);
	}
	/**
	 * Viewクラスの生成
	 * @param string $actionName アクション名.
	 */
	protected function createView($actionName)
	{
		return new \VRKohnan\View\Binary();
	}
}

