(function(){
	var $j = jQuery.noConflict(); //自定义一个比较短快捷方式
		//第一个下拉
		$j("#liNav_1").bind("mouseover",function(){			
			$j("#divNavBox_1").css("display","block");
			$j(this).addClass("tablist_hover");
		}).bind("mouseout",function(){
			$j("#divNavBox_1").css("display","none");
			$j(this).removeClass("tablist_hover");
		});
		
		$j("#divNavBox_1").bind("mouseover",function(){
			$j("#liNav_1").addClass("tablist_hover");
			$j(this).css("display","block");
		}).bind("mouseout",function(){
			$j("#liNav_1").removeClass("tablist_hover");
			$j(this).css("display","none");
		});
		
		$j("#ulRouteAppList li .app_item").bind("mouseover",function(){
			$j(this).addClass("on");
		}).bind("mouseout",function(){
			$j(this).removeClass("on");
		});
		
		//第二个下拉
		$j("#liNav_2").bind("mouseover",function(){			
			$j("#divNavBox_2").css("display","block");
			$j(this).addClass("tablist_hover");
		}).bind("mouseout",function(){
			$j("#divNavBox_2").css("display","none");
			$j(this).removeClass("tablist_hover");
		});		
		
		$j("#divNavBox_2").bind("mouseover",function(){
			$j("#liNav_2").addClass("tablist_hover");
			$j(this).css("display","block");
		}).bind("mouseout",function(){
			$j("#liNav_2").removeClass("tablist_hover");
			$j(this).css("display","none");
		});
		
		$j("#ulAppList li").bind("mouseover",function(){
			$j(this).addClass("cur");
		}).bind("mouseout",function(){
			$j(this).removeClass("cur");
		});
		
		
		//个人设置
		$j("#divUserManage").bind("mouseover",function(){			
			$j("#divUserMenu").css("display","block");
			$j(this).addClass("tablist_hover");
		}).bind("mouseout",function(){
			$j("#divUserMenu").css("display","none");
			$j(this).removeClass("tablist_hover");
		});
		
		//系统消息
		$j("#divMsgFriend").bind("mouseover",function(){			
			$j("#divFriendMsgMenu").css("display","block");
			$j(this).addClass("tablist_hover");
		}).bind("mouseout",function(){
			$j("#divFriendMsgMenu").css("display","none");
			$j(this).removeClass("tablist_hover");
		});
		
	$("#page a").click(function(){
    $.get($(this).attr('href'),function(html){
          $("#list").html(html);
    })
    return false; //防止跳转
 })	
})()
function finish_loading(){
$('loading-icon').className = 'h';
$('loading-cover').className = 'h';
setTimeout(function(){ $('loading-icon').className = 'hidden'; }, 250);
setTimeout(function(){ $('loading-cover').className = 'hidden'; }, 250);
finish_loading = function(){};
}
function ShowTab(theA,Small,main){
	for(var i=Small;i< main;i++ ){
		document.getElementById('Tab'+i).style.display='none';
		document.getElementById('Span'+i).className='';
	}
	document.getElementById('Tab'+theA).style.display='';
	document.getElementById('Span'+theA).className='choose';
}