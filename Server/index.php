<?php

// POSTデバッグ有効定義.
define('POST_DEBUG', FALSE);

if( POST_DEBUG )
{
    // POSTデバッグ用html出力.
    echo '
<html lang = "ja">
<head>
<meta charset = "UFT-8">
</head>
<body>
<form action = "index.php" method = "post">
<input type = "text" name ="c" value="Game"><br/>
<input type = "text" name ="act" value="get_ranking"><br/>
<input type = "text" name ="data" value="0a000000"><br/>
<input type = "submit" value ="submit">
	';

    {
        $controllerName = filter_input(INPUT_POST, 'c');
        $actionName		= filter_input(INPUT_POST, 'act');
        $binaryParam	= filter_input(INPUT_POST, 'data');

        echo 'c = ' . $controllerName . '<br/>';
        echo 'act = ' . $actionName . '<br/>';
        echo 'data = ' . $binaryParam . '<br/>';
    }

    echo '
</form>
</body>
</html>
	';
}

$api_execute	= true;

if( POST_DEBUG &&
    is_null($controllerName) && is_null($actionName) && is_null($binaryParam) )
{
    // POSTデバッグ有効時はPOSTパラメータが無い時はAPIを実行しない.
    $api_execute	= false;
}

if( $api_execute )
{
    // TOOD: 簡易POSTパラメーターログ.
    {
        $controllerName = filter_input(INPUT_POST, 'c');
        $actionName		= filter_input(INPUT_POST, 'act');
        $binaryParam	= filter_input(INPUT_POST, 'data');

        $format	= '['.date('YmdHis')."]\n". 'c='.$controllerName . "\nact=".$actionName . "\ndata=". bin2hex( $binaryParam ) . "\n\n";
        file_put_contents( '/var/www/html/VRKohnan/log/post_param.log', $format, FILE_APPEND );
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
}