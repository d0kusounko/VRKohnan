<?php
namespace FW\Autoload;

/**
 * オートローダ.
 * 
 * @author dO
 */
class ClassLoader
{
	/** ファイルの拡張子 */
	private $fileExtention = '.php';
	
	/** クラス名のプレフィックスリスト */
	private $prefixes = array();

	/** クラス読み込みにインクルードパスを使用するフラグ */
	private $useIncludePath = false;

	/** 名前空間のセパレータ */
	private $namespaceSeparator = '\\';

	/**
     * プレフィックスリストを取得.
     *
     * @return array
     */
    public function getPrefixes()
    {
        return $this->prefixes;
    }

    /**
     * プレフィックスを追加.
     *
     * @param string        $prefix クラスプレフィックス.
     * @param array|string  $paths クラスパス.
     */
    public function addPrefix($prefix, $paths)
    {
		if (!$prefix) { return; }

        if (isset($this->prefixes[$prefix])) {
            $this->prefixes[$prefix]
                = array_merge($this->prefixes[$prefix], (array) $paths);
        } else {
            $this->prefixes[$prefix] = (array) $paths;
        }
    }

    /**
     * インクルードパス使用フラグを設定.
     *
     * @param boolean $flg フラグ.
     */
    public function setUseIncludePath($flg)
    {
        $this->useIncludePath = $flg;
    }

    /**
     * インクルードパス使用フラグを取得.
     *
     * @return boolean フラグ.
     */
    public function getUseIncludePath()
    {
        return $this->useIncludePath;
    }

    /**
     * autoloadスタックに登録する.
     *
     * @param boolean $prepend autoloadスタックの先頭に追加するか.
     */
    public function registor($prepend = false)
    {
        spl_autoload_register(array($this, 'loadClass'), true, $prepend);
    }

    /**
     * autoloadスタックから登録解除.
     */
    public function unregister()
    {
        spl_autoload_unregister(array($this, 'loadClass'));
    }

    /**
     * クラス読み込みインタフェース.
     *
     * @param string $class クラス名.
     * @return boolean 読み込めたらtrue
     */
    public function loadClass($class)
    {
        if (!is_null($file = $this->findFile($class))) {
            require_once $file;

            return true;
        }
    }

    /**
     * ファイル検索.
     *
     * @param string $class クラス名.
     * @return string ファイルが見つかったらファイル名.
     */
	private function findFile($class)
	{
		$class_path = str_replace($this->namespaceSeparator, DIRECTORY_SEPARATOR, $class) . $this->fileExtention;

		foreach ($this->prefixes as $prefix => $dirs) {
			if (0 === str_pos($class, $prefix)) {
				foreach ($dirs as $dir) {
					if (file_exists($dir.DIRECTORY_SEPARATOR.$class_path)) {
						return $dir.DIRECTORY_SEPARATOR.$class_path;
					}
				}
			}
		}

		if ($this->useIncludePath && $file = stream_resolve_include_path($class_path)) {
			return $file;
		}
    }
}
