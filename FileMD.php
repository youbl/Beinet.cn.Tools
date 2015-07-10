<?php
//$whiteIpList =  ";172.17.150.134;";

if (isset($whiteIpList) && !strpos($whiteIpList, GetClientIP()) >= 0) {
    echo '对不起，页面请求失败！';
    return;
}
//$proxyIp

DoGetMd5();

function DoGetMd5() {
    if (!empty($_POST['d'])) {
        $dir = $_POST['d'];
    } else if (!empty($_GET['d'])) {
        $dir = $_GET['d'];
    } else {
        echo 'no input';
        return;
    }
    if (!empty($_POST['ignoreDir'])) {
        $ignoreDir = $_POST['ignoreDir'];
    } else if (!empty($_GET['ignoreDir'])) {
        $ignoreDir = $_GET['ignoreDir'];
    } else {
        $ignoreDir = '';
    }
    $ignoreDir = explode('|', $ignoreDir);

    $ret = array();
    LoopDirMd5($dir, $ret, $dir, $ignoreDir);

    header('Content-type:text/plain');
    foreach ($ret as $key => $value) {
        echo str_replace('/', '\\', $key) . ',' . strtoupper($value) . "\n";
    }
}

function LoopDirMd5($dir, &$ret, $root, $ignoreDir) {
    if (!is_dir($dir)) {
        return;
    }

    // 是否要忽略的目录判断
    $isIgnore = false;
    foreach ($ignoreDir as $val) {
        if ($val === '') {
            continue;
        }
        $compare = '/' . str_replace('\\', '/', $val);
        $idx = stripos($dir, $compare);
        if ($idx === false) {
            $idx = stripos($dir, str_replace('/', '\\', $val));
        }
        if ($idx !== false) {
            $isIgnore = true;
            break;
        }
    }
    if ($isIgnore) {
        return;
    }

    if (!($dh = opendir($dir))) {
        return;
    }
    // 移除最后一个斜杠
    $chEnd = $dir[strlen($dir) - 1];
    if ($chEnd == '/' || $chEnd == '\\') {
        $dir = substr($dir, 0, strlen($dir) - 1);
    }
    while (($file = readdir($dh)) !== false) {
        if ($file == '.' || $file == '..') {
            continue;
        }
        $showname = iconv('gb2312', 'utf-8', $file);
        $fullpath = $dir . '/' . $file;
        if (is_file($fullpath)) {
            $path = str_replace($root, '', $fullpath);
            $md5 = md5_file($fullpath);
            $ret[$path] = $md5;
        } else if (is_dir($fullpath)) {
            LoopDirMd5($fullpath, $ret, $root, $ignoreDir);
        }
    }
}

function GetClientIP() {
    $cip = '';
    if (!empty($_SERVER['HTTP_CLIENT_IP'])) {
        $cip = $_SERVER['HTTP_CLIENT_IP'];
    } elseif (!empty($_SERVER['HTTP_X_FORWARDED_FOR'])) {
        // 注意，客户端可以伪造 X-Forwarded-For: 192.168.156.45，从而导致我们得到假IP
        $cip = $_SERVER['HTTP_X_FORWARDED_FOR'];
    } elseif (!empty($_SERVER['REMOTE_ADDR'])) {
        $cip = $_SERVER['REMOTE_ADDR'];
    }
    return $cip;
}
