<?php
namespace VRKohnan\Action;

/**
 * VRKohnanのAction基底.
 * 
 * @author dO
 */
abstract class Action extends \FW\Core\Action {
	/** 実行処理 */
	public function execute($view)
	{
		$this->onExecute($view);
	}
	
	/**
	 * 内部実行処理
	 * @param View $view ビュークラス
	 */
	abstract protected function onExecute($view);
}
