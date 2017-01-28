<?php
namespace FW\Core;

/**
 * コントローラ基底.
 * 
 * @author dO
 */
abstract class Controller {
	
	/** アクション名 */
	private $actionName;
	
	/** アクションクラス */
	private $action = null;
	
	/** ビュークラス */
	private $view = null;
	
	/**
	 * コンストラクタ
	 * @param string $actionName アクション名.
	 */
	public function __construct($actionName)
	{
		$this->actionName = $actionName;
		$this->action = $this->createAction($actionName);
		$this->view = $this->createView($actionName);
	}
	
	public function __get($name)
	{
		if (property_exists($this, $name)) {
			return $this->$name;
		}
		return null;
	}
	
	/**
	 * Actionクラスの生成
	 * @param string $actionName アクション名.
	 */
	abstract protected function createAction($actionName);
	/**
	 * Viewクラスの生成
	 * @param string $actionName アクション名.
	 */
	abstract protected function createView($actionName);

	/** 実行処理 */
	public function execute()
	{	
		$this->action->execute($this->view);

		$this->view->render();
	}
}
