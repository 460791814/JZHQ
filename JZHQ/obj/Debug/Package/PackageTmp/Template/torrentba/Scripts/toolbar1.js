(function(){
		//第一个下拉
		$("#liNav_1").bind("mouseover",function(){			
			$("#divNavBox_1").css("display","block");
			$(this).addClass("tablist_hover");
		}).bind("mouseout",function(){
			$("#divNavBox_1").css("display","none");
			$(this).removeClass("tablist_hover");
		});
		
		$("#divNavBox_1").bind("mouseover",function(){
			$("#liNav_1").addClass("tablist_hover");
			$(this).css("display","block");
		}).bind("mouseout",function(){
			$("#liNav_1").removeClass("tablist_hover");
			$(this).css("display","none");
		});
		
		$("#ulRouteAppList li .app_item").bind("mouseover",function(){
			$(this).addClass("on");
		}).bind("mouseout",function(){
			$(this).removeClass("on");
		});
		
		//第二个下拉
		$("#liNav_2").bind("mouseover",function(){			
			$("#divNavBox_2").css("display","block");
			$(this).addClass("tablist_hover");
		}).bind("mouseout",function(){
			$("#divNavBox_2").css("display","none");
			$(this).removeClass("tablist_hover");
		});		
		
		$("#divNavBox_2").bind("mouseover",function(){
			$("#liNav_2").addClass("tablist_hover");
			$(this).css("display","block");
		}).bind("mouseout",function(){
			$("#liNav_2").removeClass("tablist_hover");
			$(this).css("display","none");
		});
		
		$("#ulAppList li").bind("mouseover",function(){
			$(this).addClass("cur");
		}).bind("mouseout",function(){
			$(this).removeClass("cur");
		});
		
		
		//个人设置
		$("#divUserManage").bind("mouseover",function(){			
			$("#divUserMenu").css("display","block");
			$(this).addClass("tablist_hover");
		}).bind("mouseout",function(){
			$("#divUserMenu").css("display","none");
			$(this).removeClass("tablist_hover");
		});
		
		//系统消息
		$("#divMsgFriend").bind("mouseover",function(){			
			$("#divFriendMsgMenu").css("display","block");
			$(this).addClass("tablist_hover");
		}).bind("mouseout",function(){
			$("#divFriendMsgMenu").css("display","none");
			$(this).removeClass("tablist_hover");
		});
		
		
		
		
		
		
		
})()