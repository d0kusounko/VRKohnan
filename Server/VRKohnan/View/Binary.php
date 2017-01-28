<?php
namespace VRKohnan\View;

/**
 * バイナリ出力ビュー.
 * 
 * @author dO
 */
class Binary extends \FW\Core\View {
	
	private $data = "";
	
	/**
	 * パックするデータを追加.
	 * 
	 * @param type $data		バイナリ変換して追加するデータ.
	 * @param type $packFormat バイナリ変換フォーマット.
	 */
	public function addPackData($data, $packFormat)
	{
		$this->data .= pack($packFormat, $data);
	}
	
	/** レンダリング */
	public function render()
	{
		\VRKohnan\Utility::httpResponseCode(200);
		header('Access-Control-Allow-Origin:*');
		header('Content-Type: application/octet-stream');
		header('Content-Transfer-Encoding: binary');
		echo $this->data;

		// TODO: バイナリ出力ロギング.
		// echo bin2hex( $this->data );
	}
}
