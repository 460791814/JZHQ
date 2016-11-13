	/*
	 *	自动更新首页计数
	 */
	
	(function(){
		var ignous={}

		ignous.countdown=function(node,fn){
			var _daysize = 263526558;
			//var target=node.getAttribute('data-size');

			function create(n,o){
				var num=document.createElement('b');
				num.innerHTML=n;
				o.appendChild(num);
				return true;
			}

			function setSpan(n,o,l){
				o.innerHTML='';
				var l=l||3;
				var n=n.toString().split('');
				if(n.length<l){
					var nlne=l-n.length;
					for(var i=0;i<nlne;i++){
						n.splice(0,0,0);
					}
				}
				for(var i=0;i<n.length;i++){
					create(n[i]*1,o);
				}
			}

			function show(n){
				if(n<=0){clearInterval(showTimeSet);if(fn) fn();return;}				
				var _sizes=Math.floor(n)				
				setSpan(Math.floor(_sizes/1000000),node.getElementsByTagName('span')[0]);
				setSpan(Math.floor(_sizes%1000000/1000),node.getElementsByTagName('span')[1]);
				setSpan(Math.floor(_sizes%1000000%1000),node.getElementsByTagName('span')[3]);
			}

			function showSize(){
				/*	显示当前容量
				*	_daysize:	前一天统计容量数
				*	计算方法：前一天统计容量数_daysize + 当天0：00到目前的秒数*5.78 + 1-6随机数
				*/
				var sNow = _daysize + (new Date().getHours()*3600 + new Date().getMinutes()*60 + new Date().getSeconds())*5.78 + parseInt(6*Math.random());
				//var sNow = _daysize*1000;
				show(sNow);
			}
			showSize();
			var showTimeSet=setInterval(showSize,3000);
		}

		var sizeBox=document.getElementById('item_size')
		ignous.countdown(sizeBox)
	})()