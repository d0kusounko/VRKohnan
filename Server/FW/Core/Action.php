<?php
namespace FW\Core;

/**
 * アクション基底
 * 
 * @author dO
 */
abstract class Action {
	/** Controllerのインスタンス */
	private $controller;

	/**
	 * コンストラクタ.
	 * @param Controller $controller Controller
	 */
	public function __construct($controller)
	{
		$this->controller = $controller;
	}

	public function __get($name)
	{
		return $this->$name;
	}
	
	/** 実行関数 */
	abstract public function execute($view);

	
	/** Controllerの取得 */
	protected function getController()
	{
		return $this->controller;
	}
}
