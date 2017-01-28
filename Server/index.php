<?php

// TOOD: 簡易POSTパラメーターログ.
{
	$controllerName = filter_input(INPUT_POST, 'c');
	$actionName		= filter_input(INPUT_POST, 'act');
	$binaryParam	= filter_input(INPUT_POST, 'data');
	// if( !is_null($controllerName) && !is_null($actionName) && is_null($binaryParam) )
	{
		$format	= '['.date('YmdHis')."]\n". 'c='.$controllerName . "\nact=".$actionName . "\ndata=". bin2hex( $binaryParam ) . "\n\n";
		file_put_contents( '/var/www/html/VRKohnan/log/post_param.log', $format, FILE_APPEND );
	}
}

set_include_path(get_include_path() . PATH_SEPARATOR . dirname(__FILE__));

date_default_timezone_set('Asia/Tokyo');

require_once 'FW/Autoload/ClassLoader.php';

$loader = new FW\Autoload\ClassLoader();
$loader->registor();
$loader->setUseIncludePath(true);

try {
	$dispatcher = new FW\Core\Dispatcher('VRKohnan\Controller', 'Game');
	$controller = $dispatcher->dispatch();

	$controller->execute();
} catch (\Exception $e) {
	$msg = $e->getMessage();
	if ($controller) {
		// TODO: ロギング.
		// $controller->logger->error($msg);
	}
	header('HTTP/1.1 503');
}

$loader->unregister();