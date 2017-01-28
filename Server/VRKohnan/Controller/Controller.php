<?php
namespace VRKohnan\Controller;

/**
 * VRKohnanのController基底.
 * 
 * @author dO
 */
abstract class Controller extends \FW\Core\Controller {
	/** ロガー */
	// private $logger = null;

	/** 開始時間 */
	private $startTime = 0;
	
	/**
	 * コンストラクタ
	 * @param string $actionName アクション名.
	 */
	public function __construct($actionName)
	{
		$this->startTime = \VRKohnan\Utility::microtimeFloat();
		
		parent::__construct($actionName);
		
		$process_id = getmypid();
	}
	
	public function __get($name)
	{
		$ret = parent::__get($name);
		if (empty($ret)) {
			$ret = $this->$name;
		}
		return $ret;
	}
	
	/** デストラクタ */
	public function __destruct()
	{
		$endTime = \VRKohnan\Utility::microtimeFloat();
		$diffTime = $endTime - $this->startTime;
		
		$process_id = getmypid();
	}
}

