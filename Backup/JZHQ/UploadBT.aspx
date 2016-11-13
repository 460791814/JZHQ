<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UploadBT.aspx.cs" Inherits="JZHQ.UploadBT" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=UTF-8">
<title> 磁力转换种子,Hash转换成BT,磁力,种子</title>

<meta name="viewport" content="minimal-ui">
<meta name="apple-mobile-web-app-capable" content="yes">
    <link href="/Styles/hashtobt.css" rel="stylesheet" type="text/css" />
    <link href="Styles/head.css" rel="stylesheet" type="text/css" />
    <link href="/Styles/default.css" rel="stylesheet" type="text/css" />

<link rel="shortcut icon" type="image/x-icon" href="http://dianying.fm/static/images/favicon.ico?v=4fdcf" />


<meta name="description" content="最新种子，最热门种子">
<meta name="keywords" content="最新种子，最热门种子，迅雷免费下载">
<meta name="title" content="磁力搜吧 近期热门种子，最新影视资源，在线下载，云点播">

  
    <script src="/Scripts/jquery.js" type="text/javascript"></script>
  
    <script src="/Scripts/search.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(function () {
            $("textarea[name='magnet']").focus(function () {
                if ($(this).val() == '请在此输入磁力链接...')
                    $(this).val('');

            });

            $("#ToBT").click(function () {
                $(".create i").removeClass().addClass("load");
                var hash = $("#hash").val();
                if (hash.indexOf("magnet:?xt=urn:btih:") == -1) {
                    alert("请输入正确的磁力链接");
                    return;
                }
                hash = hash.replace("magnet:?xt=urn:btih:", "");
                if (hash.indexOf("&") > -1) {
                    hash = hash.substr(0, 40);
                }
                if (hash.length != 40) {
                    alert("请输入正确的磁力链接");
                    return;
                }
                alert(hash);

                $.ajax({
                    url: "/HashToBt.aspx",
                    data: "Action=GetKey&Hash=" + hash,
                    type: "post",
                    success: function (r) {
                        var r = "<p class='tname'><a href='" + "http://torrent-cache.bitcomet.org:36869/get_torrent?info_hash=" + hash + "&key=" + r + "'>" + hash + ".torrent <i class='dld' alt='下载'></i></a></p>";
                        $("#result").html(r);
                        $(".create i").removeClass().addClass("done");
                    }

                });

            })



        })



    </script>

<script type="text/javascript">
    function addFavorite(url, title) { try { window.external.addFavorite(url, title) } catch (e) { try { window.sidebar.addPanel(title, url, '') } catch (e) { alert("加入收藏失败，请使用Ctrl+D进行添加!") } } }
</script>
</head>
<body >
 <div class="navbar navbar-fixed-top" >
<div class="navbar-inner" >
<div class="container">
<a class="btn btn-navbar" data-toggle="collapse" data-target=".nav-collapse"><span class="icon-bar"></span><span class="icon-bar"></span><span class="icon-bar"></span></a>
<a class="brand" href="/">HashBT</a>
<div class="nav-collapse">
<ul class="nav x-top-nav">
<li >
<a href="/"><i class="icon-home icon-white"></i> 首页</a></li>
<li class="">
<a href="/keyword--p1.html"><i class="icon-fire icon-white"></i> 搜吧</a></li>
<li >
<a href="/hashtobt.html"><i class="icon-th-list icon-white"></i> 磁力转换</a></li>

</ul>
<ul class="nav pull-right" >
<li class="x-li-nickname" >

<a id="login" class="login-register-dialog" style="cursor:pointer;">
<i class="icon-user icon-white"></i><font id="nav_nickname" onclick="addFavorite(this.href,'HashBT磁力种子,磁力搜索')">
收藏本页
</font>
</a>

</li>

</ul>

<div class="pull-left">
<div class="navbar-search input-append">
<input id="keyWord" type="text"
name="key" placeholder="来自星星的你" data-provide="typeahead"
class="typeahead search-query" autocomplete="off" style="margin:0;border-radius:15px 0 0 15px;  color:#666;">
<!--<button style="border-radius:0 15px 15px 0 ;" type="submit" class="btn btn-primary"><i class="icon-search icon-white"></i></button>-->

    <input type="button" id="btnSouba" style="width:55px;height:30px;border-radius:0 15px 15px 0 ;" name="name" value="搜一下" />
</div>
</div>

</div>
</div>
</div>
</div>
<div id="main_content" style="width:960px;" class="container">



<div class="row-fluid" style="margin-bottom:5px; height:100px">

</div>

<div class="mainbox">
        <div class="conbox pdlr20 cl">

            <div class="fl" style="width: 600px;">
            <div>
                <form id="form1" runat="server" action="/Handler/Handler.ashx" method="post" enctype="multipart/form-data">
    <input type="file" name="name" value=" " />
    <input type="submit" name="tijiao" value="上传" />
    </form>
            </div>
                <div class="link">
                    <textarea name="magnet" id="hash" class="ta magta"><%=str %></textarea>
                    </div>
              
                   
                <div id="result" style="margin-top: 20px;">
                    </div>
            </div>
        </div>
        <!--/conbox end/-->
    </div>




<!-- modal -->




<div class="well" style="margin-top: 20px; font-size: 13px;">
<div class="" style="height: 20px;">

<div class="pull-right " style="color: #888;">
本站不提供任何视听上传服务，所有内容均来自视频分享站点所提供的公开引用资源
</div>
</div>
<div style="height: 20px;">

<div style="color: #888; " class="pull-right">Copyright &copy;  A</div>
</div>
</div>

<div style="display:none;">

</div>

</div>


<div class="btn-group btn-group-vertical z-rightedge-container" style="left:50%;">
<a href="javascript:void(0);" class="btn btn-small z-rightedge-buttom" style="display:none;" id="gotop">
<i class="icon-arrow-up"></i>
</br>回</br>到</br>顶</br>部
</a>
<a href="/opinion" style="" class="btn btn-small z-rightedge-buttom" alt="">
反</br>馈</br>意</br>见
</a>
</div>
</body>
</html>