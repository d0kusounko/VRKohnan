<?php
namespace VRKohnan;

/**
 * ユーティリティクラス.
 * 
 * @author dO
 */
class Utility {
	/**
	 * 配列を特定のkeyによる連想配列に変換します.
	 * 
	 * 例:
	 * array(
	 *  0 => array('user_id' => 100, 'name'=>'foo'),
	 *  1 => array('user_id' => 200, 'name'=>'bar'),
	 * )
	 * ↓
	 * array(
	 *  100 => array('user_id' => 100, 'name'=>'foo'),
	 *  200 => array('user_id' => 200, 'name'=>'bar'),
	 * ),
	 * 
	 * @param array $array 作業対象の配列.
	 * @param string $key keyとなる要素
	 * @return array 変換後の配列
	 */
	static public function arrayToKeyArray($array, $key)
	{		
		$ret = array();
		if (is_array($array)) {
			foreach ($array as $val) {
				if (!is_array($val) || !key_exists($key, $val)) { continue; }

				$ret[ $val[$key] ] = $val;
			}
		}
		
		return $ret;
	}
	
	/** 連想配列判定 */
	static public function isKeyArray($array)
	{
		return (array_values($array) !== $array);
	}
	
	/** UNIXタイムスタンプをマイクロ秒までfloat値で返す */
	static public function microtimeFloat()
	{
		list($usec, $sec) = explode(" ", microtime());
		return ((float)$usec + (float)$sec);
	}
	
	/**
	 * HTTPステータスコードの設定取得.
	 * @param int $code HTTPステータスコードの設定値。省略すると現在値を返す.
	 * @return int HTTPステータスコード.
	 */
	static public function httpResponseCode($code = NULL)
	{
		if ($code !== NULL) {
			switch ($code) {
				case 100: $text = 'Continue'; break;
				case 101: $text = 'Switching Protocols'; break;
				case 200: $text = 'OK'; break;
				case 201: $text = 'Created'; break;
				case 202: $text = 'Accepted'; break;
				case 203: $text = 'Non-Authoritative Information'; break;
				case 204: $text = 'No Content'; break;
				case 205: $text = 'Reset Content'; break;
				case 206: $text = 'Partial Content'; break;
				case 300: $text = 'Multiple Choices'; break;
				case 301: $text = 'Moved Permanently'; break;
				case 302: $text = 'Moved Temporarily'; break;
				case 303: $text = 'See Other'; break;
				case 304: $text = 'Not Modified'; break;
				case 305: $text = 'Use Proxy'; break;
				case 400: $text = 'Bad Request'; break;
				case 401: $text = 'Unauthorized'; break;
				case 402: $text = 'Payment Required'; break;
				case 403: $text = 'Forbidden'; break;
				case 404: $text = 'Not Found'; break;
				case 405: $text = 'Method Not Allowed'; break;
				case 406: $text = 'Not Acceptable'; break;
				case 407: $text = 'Proxy Authentication Required'; break;
				case 408: $text = 'Request Time-out'; break;
				case 409: $text = 'Conflict'; break;
				case 410: $text = 'Gone'; break;
				case 411: $text = 'Length Required'; break;
				case 412: $text = 'Precondition Failed'; break;
				case 413: $text = 'Request Entity Too Large'; break;
				case 414: $text = 'Request-URI Too Large'; break;
				case 415: $text = 'Unsupported Media Type'; break;
				case 500: $text = 'Internal Server Error'; break;
				case 501: $text = 'Not Implemented'; break;
				case 502: $text = 'Bad Gateway'; break;
				case 503: $text = 'Service Unavailable'; break;
				case 504: $text = 'Gateway Time-out'; break;
				case 505: $text = 'HTTP Version not supported'; break;
				default:
					exit('Unknown http status code "' . htmlentities($code) . '"');
					break;
			}

			$protocol = filter_input(INPUT_SERVER, 'SERVER_PROTOCOL');
			if (!$protocol) {
				$protocol = 'HTTP/1.0';
			}
			header($protocol . ' ' . $code . ' ' . $text);
			$GLOBALS['http_response_code'] = $code;
		} else {
			$code = (isset($GLOBALS['http_response_code']) ? $GLOBALS['http_response_code'] : 200);
		}

		return $code;
	}
}
