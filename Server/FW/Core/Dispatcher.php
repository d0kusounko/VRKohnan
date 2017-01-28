<?php
namespace FW\Core;

/**
 * コントローラーの振り分けクラス.
 * 
 * @author dO
 */
class Dispatcher {
	
	/** コントローラのルート名前空間 */
	private $ctrlNamespace;
	
	/** デフォルトコントローラ */
	private $defaultController;
	
	/**
	 * コンストラクタ
	 * 
	 * @param string $rootNs	コントローラのルート名前空間.
	 * @param string $default	デフォルトのコントローラ名.
	 */
	public function __construct($rootNs, $default) 
	{
		$this->ctrlNamespace = $rootNs;
		$this->defaultController = $default;
	}
	
	/**
	 * 振り分け処理.
	 * 
	 * @return FW\Core\Controller 振り分けられたコントローラ.
	 */
	public function dispatch()
	{
		if (defined('STDIN')) {
			global $argv;
			$controllerName = $argv[1];
			$actionName		= $argv[2];
		}
		else
		{
			// POSTパラメータより取得.

			// コントローラーの種別チェック.
			$controllerName = filter_input(INPUT_POST, 'c');
			if (is_null($controllerName))
			{
				// 指定無しの場合はデフォルト.
				$controllerName = $this->defaultController;
			}

			// アクションAPI取得.
			$actionName = filter_input(INPUT_POST, 'act');
		}		

		$controllerName = ucfirst($controllerName);

		if (is_null($actionName)) {
			$actionName = ucfirst('index');
		} else {
			$splitArray = explode('_', $actionName);
			$actionName = '';
			foreach ($splitArray as $val) {
				$actionName .= ucfirst($val);
			}
		}
		
		$className = $this->ctrlNamespace . '\\' . $controllerName . 'Controller';
		
		$class = new $className($actionName);
		return $class;
	}
}
