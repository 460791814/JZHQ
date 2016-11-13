var arrLink = document.links;
var thunderLen = arrLink.length;
var thunderSufix = ".chm;.asf;.avi;.exe;.iso;.mp3;.mpeg;.mpg;.mpga;.ra;.rar;.rm;.rmvb;.tar;.wma;.wmv;.zip;.swf;.mp4;.3gp;.torrent;.txt;.jar;.mov;.wav;"
var arrSufix = thunderSufix.split(";");

for(var i=0;i<thunderLen;i++)
{	
	var temp =arrLink[i].href;
	var post = temp.lastIndexOf(".");
	var p = temp.substring(post,temp.length).toLowerCase();
	var thun=temp.substring(post,8).toLowerCase();

	var k = arrSufix.length;
	var flag =false;
	var thunder_url = arrLink[i].href;
	var protocol=arrLink[i].protocol;
	var pathname=arrLink[i].pathname;
	var path=pathname.substring(pathname.lastIndexOf("/"),pathname.length);
	var parameterPrefix;
	var parameterPosix;
    var thunderPrefix=temp.substring(0,10)
   if(thunderPrefix=="thunder://")
	{
		var s = document.createElement("anchor");
		s.innerHTML+="<a href='#' thunderHref='"+thunder_url+"' thunderPid='"+thunderPid+"' thunderType='' thunderResTitle='' onClick='return OnDownloadClick_Simple(this,2,4)' oncontextmenu='ThunderNetwork_SetHref(this)'>"+arrLink[i].innerHTML+"</a>";
		arrLink[i].replaceNode(s);
	}

	parameterPrefix=path.split('.');

	if(parameterPrefix[1]!=null)
	{
	      parameterPosix=parameterPrefix[1].split("?");
		
	
	if(parameterPosix[0]!=null)
	{
	 var kkkk="."+parameterPosix[0];
	for(var k=0;k<arrSufix.length;k++)
	{
		if(p==arrSufix[k] || kkkk==arrSufix[k])
		{
			flag=true;
			break;
		}
	}
	
	}
	}

	if(thunderPath == null)
	{
	   thunderPath="";
	}
	if(path == thunderPath && thunderPath != "")
	{
		flag=true;
	}
	//
	for(var k=0;k<arrSufix.length;k++)
	{
		if(p==arrSufix[k])
		{
			flag=true;
			break;
		}
	}
	if(protocol!="http:"&protocol!="ftp:"&protocol!="mms:"&protocol!="rtsp:")
	{
		flag=false;
	}	
	if(flag)
		{
		var s = document.createElement("anchor");
		s.innerHTML+="<a href='#' thunderHref='"+ThunderEncode(thunder_url)+"' thunderPid='"+thunderPid+"' thunderType='' thunderResTitle='' onClick='return OnDownloadClick_Simple(this,2,4)' oncontextmenu='ThunderNetwork_SetHref(this)'>"+arrLink[i].innerHTML+"</a>";
		arrLink[i].replaceNode(s);
		}
}