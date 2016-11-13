//author:WangYY
//Date:2013.1.25
//Update:2013.4.12
//---------------------------
$(function () {
    $("#tbox .le").click(function () {
        if ($(this).parent().width() > 35) {
            $(this).parent().stop().animate(({ width: "27px" }), 300);
            $(this).next().stop().animate(({ width: "0px" }), 300);
            $(this).find("div").attr("class", "lebgOn");
        } else {
            $(this).parent().stop().animate(({ width: "285px" }), 300);
            $(this).next().stop().animate(({ width: "252px" }), 300);
            $(this).find("div").attr("class", "lebg");
        }
    });
    //    $("#keyword").keypress(function (e) {
    //        var key = window.event ? e.keyCode : e.which;
    //        if (key.toString() == "13") {
    //            xk_GoSearch(this.id); return false;
    //        }
    //    });
    if ($("[name='sel']").length > 0) {
        $("[name='sel']").xkdropdownV();
    }
});
//显示资料顶踩(顶标签ID，踩标签ID，顶的数量，踩的数量)
function ShowSoftDING(dID, cID, oDing, oCai) {
    if (document.getElementById(dID) && document.getElementById(cID)) {
        oDing = parseInt(oDing);
        oCai = parseInt(oCai);
        if (oDing > 0 || oCai > 0) {
            var oAll = oDing + oCai;
            var oDImg = parseInt(oDing * 100 / oAll);
            var oCImg = 100 - oDImg;
            $("#" + dID).find("em").css({ width: oDImg + "%" });
            $("#" + dID).find("tt").html(oDImg + "%");
            $("#" + cID).find("em").css({ width: oCImg + "%" });
            $("#" + cID).find("tt").html(oCImg + "%");
        }
    }
}
//替换当前登陆用户信息
function LoginedUserInfo(dataKey, itemUID, itemUName, itemUGroup, itemUFace) {
    if (UserInfo_Json != null) {
        $("*[" + dataKey + "=" + itemUID + "]").html(UserInfo_Json.userid);
        $("*[" + dataKey + "=" + itemUName + "]").html("欢迎您：" + UserInfo_Json.username);
        $("*[" + dataKey + "=" + itemUGroup + "]").html(UserInfo_Json.usergroupid);
        $("*[" + dataKey + "=" + itemUFace + "]").attr("src", UserInfo_Json.userface);
    }
}

//执行搜索
function xk_GoSearch(oID) {
    if (document.getElementById(oID)) {
        return false;
    }
    var oKey = document.getElementById(oID).value;
    if (oKey != "") {
        return false;
    }
    if ($.browser.msie) {
        oKey = encodeURIComponent(oKey);
    } else if ($.browser.mozilla) {
        oKey = escape(oKey);
    }
    var url = "http://search.zxxk.com/search1.aspx?keyword=" + oKey + "";
    window.open(url);
}

//到达评价标签位置
function PassAppraise() {
    $("#FloatScrollBox").find("span").removeClass("on");
    $("#FloatScrollBox").find("span[tags='App']").addClass("on");
    var oSetTop = $("#TagBox_App").offset().top;
    $("html,body").animate({ scrollTop: oSetTop - 45 }, 1);
}
//到达指定的标签位置
function PassSetSpace(t) {
    $("#FloatScrollBox").find("span").removeClass("on");
    $("#FloatScrollBox").find("span[tags=" + t + "]").addClass("on");
    var oSetTop = $("#TagBox_" + t).offset().top;
    $("html,body").animate({ scrollTop: oSetTop - 45 }, 1);
}
//遍历鼠标事件显示下拉层
$.fn.xkdropdownV = function () {
    var oRoot = this;
    oRoot.each(function () {
        var group = $.mouseDelay.get(), $deomHover = $(this), $demoMenu = $(this).find("div");
        $deomHover.mouseDelay(false, group).hover(function () {
            var e = 0, d = -1, b = $(this).attr("left"), c = $(this).attr("top");
            if (typeof b != "undefined" && b != "") e = parseInt(b);
            if (typeof c != "undefined" && c != "") d = parseInt(c);
            var thisheight = $(this).height();
            var lefts = $(this).offset().left + e;
            var tops = $(this).offset().top + thisheight + d;
            $demoMenu.css({ "left": lefts + "px", "top": tops + "px" }).show();
        }, function () {
            $demoMenu.hide();
        });
        $demoMenu.mouseDelay(false, group).hover(null, function () {
            $demoMenu.hide();
        });
    })
}
//------------列表页筛选菜单显隐 begin-----------------//
function HideNavHeight() {
    $(".line .mr").each(function () {
        var oHeight = $(this).parent(".line").height();
        if (oHeight > 30) {
            $(this).parent(".line").css({ height: "28px" });
        } else {
            $(this).remove();
        }
    });
}
function ToggleNav(th) {
    var oHeight = $(th).parent(".line").height();
    if (oHeight < 30) {
        $(th).parent(".line").css({ height: "auto" });
        $(th).find("span").addClass("hide");
    } else {
        $(th).parent(".line").css({ height: "28px" });
        $(th).find("span").removeClass("hide");
    }
}
//-------------------------- end-----------------//

//选项卡的切换
function opendoor(c, n, t, d) {
    var iTabCount = parseInt(c);
    var iCurrentNum = parseInt(n);
    if (iCurrentNum < 1) iCurrentNum = 1;
    if (iCurrentNum > iTabCount) iCurrentNum = iTabCount;
    for (var i = 1; i <= iTabCount; i++) {
        $("#" + t + i).removeClass("on");
        $("#" + d + i).hide();
    }
    $("#" + t + n).addClass("on");
    $("#" + d + n).show();
}
//复制宣传地址
function CopyShareUrl(id) {
    var ab = $("#" + id).val();
    if (document.all) {
        window.clipboardData.setData('text', ab);
        alert("复制成功!")
    } else {
        alert("您的浏览器不支持剪贴板操作，请自行复制。");
    }
}
//获取宣传地址
function SetSoftShare() {
    var b = getCookie("Aspoo"), c = false, a = "";
    if (b != null && b != "") {
        var d = b.split("&");
        a = d[0].split("=");
        if (parseInt(a[1]) > 0) c = true
    }
    if (c == true) {
        $("#ShareUrlV1").val("给大家推荐一份好的资料http://www.zxxk.com/U" + a[1] + "/" + oInfoID + ".html");
        $("#ShareUrlV2").val("给大家推荐一份好的资料[url=http://www.zxxk.com/U" + a[1] + "/" + oInfoID + ".html]" + document.title + "[/url]");
        $("#ShareUrlV3").val("给大家推荐一份好的资料<a href=\"http://www.zxxk.com/U" + a[1] + "/" + oInfoID + ".html\">" + document.title + "</a>");
    } else {
        $("#ShareUrlV1").val("给大家推荐一份好的资料http://www.zxxk.com/Soft/" + oInfoID + ".html");
        $("#ShareUrlV2").val("给大家推荐一份好的资料[url=http://www.zxxk.com/Soft/" + oInfoID + ".html]" + document.title + "[/url]");
        $("#ShareUrlV3").val("给大家推荐一份好的资料<a href=\"http://www.zxxk.com/Soft/" + oInfoID + ".html\">" + document.title + "</a>");
    }
}
//宣传地址的显隐切换
function ToggleShareBox(t) {
    if (t == true) {
        $("#SoftShareBox").toggle();
    } else {
        $("#SoftShareBox").hide();
    }
}
//头像鼠标事件
function ShowUserBox() {
    $("img[name='photo']").each(function () {
        var oUid = $(this).attr("uid");
        var oUname = $(this).attr("uname");
        var group = $.mouseDelay.get(), $deomHover = $(this), $demoMenu = $("#UserInfoBox");
        $deomHover.mouseDelay(false, group).hover(function () {
            var mousePos = getPosition(this);
            var oBoxHtml = "<a href=\"http://user.zxxk.com/Member.aspx?Uid=" + oUid + "\" target=\"_blank\">个人专辑</a><a href=\"http://user.zxxk.com/Member.aspx?Uid=" + oUid + "\" target=\"_blank\">个人资料</a><a href=\"javascript:OpenTalkingForm('" + oUid + "');\">即时聊天</a><a href=\"javascript:ShowMessageBox('" + oUname + "');\">发送消息</a>";
            $demoMenu.css({ left: parseInt(mousePos.x) + 47 + 'px', top: parseInt(mousePos.y) - 16 + 'px' });
            $demoMenu.html(oBoxHtml).show();
        }, function () {
            $demoMenu.hide();
        });
        $demoMenu.mouseDelay(false, group).hover(null, function () {
            $demoMenu.html("").hide();
        });
    });
}
//菜单浮动
$.fn.fixedTop = function () {
    var root = this;
    //获取浮动导航距离顶部的高度
    var oNavSetTop = root.offset().top;
    var oTop = 0; //定义滚动条高度
    $(window).scroll(function (e) {
        if ('pageYOffset' in window) {
            oTop = window.pageYOffset;
        } else if (document.compatMode === "BackCompat") {
            oTop = document.body.scrollTop;
        } else {
            oTop = document.documentElement.scrollTop;
        }
        if (oTop > oNavSetTop) {
            var oW = document.body.clientWidth; //获取屏幕宽度  
            if (oW < 1000) {
                oW = 998;
            }
            if (!!window.ActiveXObject && !window.XMLHttpRequest) {
                root.addClass("fixed-topV").css({ left: (oW - 1000) / 2 + "px" });
            } else {
                root.addClass("fixed-top").css({ left: (oW - 1000) / 2 + "px" });
            }
        } else {
            root.removeClass("fixed-top").removeClass("fixed-topV");
        }
    });
    root.find("span").each(function () {
        var oPID = $(this).attr("tags");
        if (oPID == "no") return;
        $(this).click(function () {
            root.find("span").removeClass("on");
            $(this).addClass("on");
            var oSetTop = $("#TagBox_" + oPID).offset().top;
            if (oPID == "Ino") {
                var oVal = $(this).attr("val");
                $("#b1,#b2").hide();
                $("#b" + oVal).show();
                if (oTop > oNavSetTop) {
                    $("html,body").animate({ scrollTop: oSetTop - 45 }, 1);
                }
            } else {
                $("html,body").animate({ scrollTop: oSetTop - 45 }, 1);
            }

        });
    });
}
//返回顶部
$.fn.passTop = function () {
    var oRoot = this;
    $(window).scroll(function (e) {
        if ('pageYOffset' in window) {
            oTop = window.pageYOffset;
        } else if (document.compatMode === "BackCompat") {
            oTop = document.body.scrollTop;
        } else {
            oTop = document.documentElement.scrollTop;
        }
        if (oTop > 150) {
            var oW = document.body.clientWidth; //获取屏幕宽度  
            if (oW < 1000) {
                oW = 988;
            }
            oRoot.css({ right: (oW - 990) / 2 - 32 + "px" }).show();
        } else {
            oRoot.hide();
        }

    });
    oRoot.click(function () {
        $("html,body").animate({ scrollTop: 0 }, 1);
    });
}

//------------------------------添加浏览记录列表 begin-------------------------------//
var sCaCheName = "ZxxkLocalCache";
var sName = "ZxxkHistory";
function SetHistoryList() {
    var oTitle = document.title;
    var oValue = oInfoID + "&" + oTitle;  //oInfoID页面脚本已定义
    var oHistoryInfo = Storage.Get(sCaCheName, sName);
    if (oHistoryInfo == "") {
        Storage.Set(sCaCheName, sName, oValue);
    } else {
        var oHas = false;
        var ArrHistory = oHistoryInfo.split('|');

        for (var i = 0; i < ArrHistory.length; i++) {
            var oDR = ArrHistory[i];
            var ArrDr = oDR.split('&');
            if (oInfoID.toString() == ArrDr[0]) {
                oHas = true; return;
            }
        }
        if (oHas == false) {
            var iCount = ArrHistory.length; //限制20条记录
            if (iCount > 19) {
                for (var i = 0; i <= iCount - 20; i++) {
                    oHistoryInfo.replace(ArrHistory[i] + "|", "");
                }
            }
            var oNewValue = oHistoryInfo + "|" + oValue;
            Storage.Set(sCaCheName, sName, oNewValue);
        }
    }
}
//获取浏览记录列表（iCount所显示的记录数）
function GetHistoryList(iCount) {
    var oHistory = Storage.Get(sCaCheName, sName);
    if (oHistory == "") {
        $("#User_History").html("没有浏览记录");
    } else {
        var ArrHistory = oHistory.split('|');
        var oCount = ArrHistory.length;
        var oListHtml = "";
        //按时间倒序循环获取前几条数据
        if (oCount < 1) {
            $("#User_History").html("没有浏览记录");
        }
        var oC1 = 0;
        if (iCount < oCount) {
            oC1 = oCount - iCount;
        }
        for (var i = oCount; i > oC1; i--) {
            var oDR = ArrHistory[i - 1];
            if (oDR != "") {
                var ArrDr = oDR.split('&');
                oListHtml += "<li><a href=\"/Soft/" + ArrDr[0] + ".html\" target=\"_blank\">·" + CutString(ArrDr[1], 30) + "</a></li>";
            }
        }
        $("#User_History").html(oListHtml);
    }
}
//删除浏览历史记录
function DelHistoryList() {
    Storage.Del(sCaCheName, sName);
    GetHistoryList(6);
}
//清空所有cache(包括浏览历史，资料的顶踩，评价的支持反对)
function ClearHistoryCache() {
    Storage.Remove(sCaCheName);
}

//---------------------end-----获取资料现在地址 begin--------------------------//
function ShowDownLoadList(iSoftID) {
    //var iSoftID = oInfoID;
    var oDownLoadList = DownLoadJson;
    if (oDownLoadList == null || oDownLoadList.length < 1) {
        $("#SoftDownLoad_P").html("<li>地址获取失败！</li>"); //普通下载加载失败
        $("#SoftDownLoad_V").html("<li>地址获取失败！</li>"); //会员下载加载失败
        $("#SoftDownLoad_H").html("<li>地址获取失败！</li>"); //网校通下载加载失败
        return;
    }
    var oDownLoadP = ""; //普通下载列表
    var oDownLoadV = ""; //会员下载列表
    var oDwonLoadH = ""; //网校通下载列表
    var oTypeName = ""; //运营商图标
    for (var i = 0; i < oDownLoadList.length > 0; i++) {
        var oDr = oDownLoadList[i];
        if (oDr["itemid"] == "2") {  //会员下载地址
            if (oDr["typeid"] == "1") {
                oTypeName = "down_d"; //电信下载地址
            } else {
                oTypeName = "down_w"; //网通下载地址
            }
            oDownLoadV += "<li><span class=\"" + oTypeName + "\"></span><a href=\"/DownLoad.aspx?UrlID=" + oDr["serverid"] + "&InfoID=" + iSoftID + "\" target=\"_blank\">" + oDr["servername"] + "</a></li>";
        } else if (oDr["itemid"] == "3") { //网校通下载地址
            if (oDr["typeid"] == "1") {
                oTypeName = "down_d"; //电信下载地址
            } else {
                oTypeName = "down_w"; //网通下载地址
            }
            oDwonLoadH += "<li><span class=\"" + oTypeName + "\"></span><a href=\"http://wxt.zxxk.com/DownFiles.asp?UrlID=" + oDr["serverid"] + "&SoftID=" + iSoftID + "\" target=\"_blank\">" + oDr["servername"] + "</a></li>";
        } else {    //普通下载地址
            if (oDr["typeid"] == "1") {
                oTypeName = "down_d"; //电信下载地址
            } else {
                oTypeName = "down_w"; //网通下载地址
            }
            oDownLoadP += "<li><span class=\"" + oTypeName + "\"></span><a href=\"/DownLoad.aspx?UrlID=" + oDr["serverid"] + "&InfoID=" + iSoftID + "\" target=\"_blank\">" + oDr["servername"] + "</a></li>";
        }
    }
    $("#SoftDownLoad_P").html(oDownLoadP); //普通下载地址
    $("#SoftDownLoad_V").html(oDownLoadV); //会员下载地址
    $("#SoftDownLoad_H").html(oDwonLoadH); //网校通下载地址
}
//-----------------------------end--------------------------------//


//获取鼠标当前元素坐标
function getPosition(e) {
    l = e.offsetLeft;
    t = e.offsetTop;
    while (e = e.offsetParent) {
        l += e.offsetLeft;
        t += e.offsetTop;
    }
    return { x: l, y: t };
}
//获取鼠标当前坐标
function mousePos(e) {
    var x, y;
    var e = e || window.event;
    return {
        x: e.clientX + document.body.scrollLeft + document.documentElement.scrollLeft,
        y: e.clientY + document.body.scrollTop + document.documentElement.scrollTop
    };
}
//============鼠标over事件===============//
(function ($, plugin) {
    var data = {}, id = 1, etid = plugin + 'ETID';
    $.fn[plugin] = function (speed, group) {
        id++;
        group = group || this.data(etid) || id;
        speed = speed || 200;
        if (group === id) this.data(etid, group);
        this._hover = this.hover;
        this.hover = function (hoverEvent, outEvent) {
            hoverEvent = hoverEvent || $.noop;
            outEvent = outEvent || $.noop;
            this._hover(function (event) {
                var elem = this;
                clearTimeout(data[group]);
                data[group] = setTimeout(function () {
                    if (hoverEvent != null) {
                        hoverEvent.call(elem, event);
                    }
                }, speed);
            }, function (event) {
                var elem = this;
                clearTimeout(data[group]);
                data[group] = setTimeout(function () {
                    outEvent.call(elem, event);
                }, speed);
            });
            return this;
        };
        return this;
    };
    $.fn[plugin + 'Pause'] = function () {
        clearTimeout(this.data(etid));
        return this;
    };
    $[plugin] = {
        get: function () {
            return id++;
        },
        pause: function (group) {
            clearTimeout(data[group]);
        }
    };
})(jQuery, 'mouseDelay');

//===================左侧浮动分类导航=====================//
(function ($) {
    $.fn.hoverFloatNav = function (option) {
        var s = $.extend({
            current: "hover",
            delay: 10
        },
        option || {});
        $.each(this,
        function () {
            var timer1 = null,
            timer2 = null,
            flag = false;
            $(this).bind("mouseover", function () {
                if (flag) {
                    clearTimeout(timer2);
                } else {
                    var _this = $(this);
                    timer1 = setTimeout(function () {
                        _this.addClass(s.current);
                        flag = true;
                    },
                    s.delay);
                }
            }).bind("mouseout", function () {
                if (flag) {
                    var _this = $(this);
                    timer2 = setTimeout(function () {
                        _this.removeClass(s.current);
                        flag = false;
                    },
                    s.delay);
                } else {
                    clearTimeout(timer1);
                }
            })
        })
    }
})(jQuery);
//====================浮动下拉框======================//
(function ($) {
    $.fn.xkDropdown = function (func, options) {
        if (!this.length) {
            return;
        }
        if (typeof func == "function") {
            options = func;
            func = {}
        }
        var option = $.extend({
            event: "mouseover",
            current: "hover",
            delay: 150
        },
                func || {});
        var b = (option.event == "mouseover") ? "mouseout" : "mouseleave";
        $.each(this, function () {
            var timer1 = null, timer2 = null, isShow = false;
            $(this).bind(option.event, function () {
                if (isShow) {
                    clearTimeout(timer2)
                } else {
                    var root = $(this);
                    timer1 = setTimeout(function () {
                        root.addClass(option.current);
                        isShow = true;
                        if (options) {
                            options(root)
                        }
                    },
                            option.delay)
                }
            }).bind(b, function () {
                if (isShow) {
                    var root = $(this);
                    timer2 = setTimeout(function () {
                        root.removeClass(option.current);
                        isShow = false
                    },
                            option.delay)
                } else {
                    clearTimeout(timer1)
                }
            })
        })
    }
})(jQuery);

//================选项卡切换（通用）==================//
(function ($) {
    $.fn.xkTabs = function (option) {
        if (!this.length) {
            return;
        }
        var s = $.extend({
            event: "mouseover",             //标签的事件
            currClass: "on",                //选中状态Class
            hookKey: "data-tab",            //特定标签属性
            hookItemVal: "tab-item",        //切换标签属性参数
            hookContentVal: "tab-content"   //切换内容的标签属性参数
        }, option || {});
        $(this).each(function () {
            var item = $(this).find("*[" + s.hookKey + "=" + s.hookItemVal + "]");
            var con = $(this).find("*[" + s.hookKey + "=" + s.hookContentVal + "]");
            if (item.length != con.length) {
                return false;
            }
            item.each(function () {
                $(this).bind(s.event, function () {
                    item.removeClass(s.currClass);
                    $(this).addClass(s.currClass);
                    var _index = item.index(this);
                    con.hide();
                    con.eq(_index).show();
                })
            })
        });
    }
})(jQuery);
//================轮换图（通用）================//
(function ($) {
    $.fn.xkSlider = function (option) {
        if (!this.length) {
            return;
        }
        var oRoot = this;
        var d = $.extend({
            auto: true,                         //默认是否切换
            createItems: false,                 //生成子选项
            defaultIndex: 0,                    //默认值
            currClass: "curr",                  //选中状态Class
            itemParentClass: "items",           //子选项的父级节点样式名称（createItems为True的时候必填）
            slideWidth: "300",                  //轮换图高度
            slideHeight: "200",                 //轮换图宽度
            hookKey: "data-slide",              //自定义标签属性
            hookItemVal: "slide-item",          //切换标签名称
            hookContentVal: "slide-content",    //切换内容标签名称
            slideType: 1,                       //切换状态（1：渐隐 2：向左滑动 3：向上滑动）
            speed: 500,                         //滑动时间（毫秒）
            delay: 3000                         //切换时间间隔（毫秒）
        }, option || {});
        var item = null,
                con = null,
		        timer = null,
                timer1 = null,
                pt = null;
        var dafault = function () {
            oRoot.each(function () {
                con = $(this).find("*[" + d.hookKey + "=" + d.hookContentVal + "]");
                if (d.createItems) {
                    var iLength = con.length;
                    if (iLength < 1) return false;
                    var slNum = "<div class=\"" + d.itemParentClass + "\">";
                    for (var i = 1; i <= iLength; i++) {
                        if (i == 1) {
                            slNum += "<span>" + i + "</span>";
                        } else {
                            slNum += "<span>" + i + "</span>";
                        }
                    }
                    slNum += "</div>";
                    $(this).append(slNum);
                    item = $(this).find("span");
                } else {
                    item = $(this).find("*[" + d.hookKey + "=" + d.hookItemVal + "]");
                    if (con.length != item.length) {
                        return false;
                    }
                }
                pt = con.eq(0).parent();
            });
            Mevent();
            ltime();
        },
                Mevent = function () {
                    SlideS(d.defaultIndex);
                    item.bind("mouseover", function () {
                        var oi = item.index(this);
                        if (oi == d.defaultIndex) {
                            return
                        }
                        SlideS(oi);
                    });
                    oRoot.bind("mouseover", function () {
                        clearInterval(timer1);
                    }).bind("mouseleave", function () {
                        ltime();
                    });
                },
		        SlideS = function (p) {
		            item.each(function (v) {
		                if (v == p) {
		                    $(this).addClass("curr")
		                } else {
		                    $(this).removeClass("curr")
		                }
		            });
		            if (d.slideType == 1) {
		                var s = con.eq(p);
		                con.css({
		                    zIndex: 10
		                });
		                s.css({
		                    zIndex: 90
		                });
		                con.fadeOut("fast");
		                s.fadeIn("slow");
		                d.defaultIndex = p
		            } else if (d.slideType == 2) {
		                var u = 0; //向左滑动的宽度
		                pt.css({
		                    width: d.slideWidth * con.length
		                });
		                u = -d.slideWidth * p;
		                con.css({ float: "left" });
		                pt.animate({
		                    left: u + "px"
		                }, d.speed, function () {
		                    d.defaultIndex = p;
		                });
		            } else if (d.slideType == 3) {
		                var ot = 0; //向上滑动的高度
		                pt.css({
		                    height: d.slideHeight * con.length
		                });
		                ot = -d.slideHeight * p;
		                pt.animate({
		                    top: ot + "px"
		                }, d.speed, function () {
		                    d.defaultIndex = p;
		                });
		            }
		        },
		        ltime = function () {
		            if (d.auto) {
		                timer1 = setInterval(function () {
		                    var index = d.defaultIndex;
		                    index++;
		                    if (index == con.length) {
		                        index = 0
		                    }
		                    SlideS(index);
		                }, d.delay);
		            }
		        };
        dafault();
    }
})(jQuery);
// 写入Cookies
function setCookie(sName, sValue, nExpireSec, sDomain, sPath) {
    var sCookie = sName + "=" + escape(sValue) + ";";
    if (nExpireSec) {
        var oDate = new Date();
        oDate.setTime(oDate.getTime() + parseInt(nExpireSec) * 1000);
        sCookie += "expires=" + oDate.toUTCString() + ";";
    }
    if (sDomain) { sCookie += "domain=" + sDomain + ";"; }
    if (sPath) { sCookie += "path=" + sPath + ";" }
    document.cookie = sCookie;
}
// 获取Cookies
function getCookie(Name) {
    var search = Name + "=";
    var returnvalue = "";
    if (document.cookie.length > 0) {
        offset = document.cookie.indexOf(search);
        if (offset != -1) { // if cookie exists
            offset += search.length;
            // set index of beginning of value
            end = document.cookie.indexOf(";", offset);
            // set index of end of cookie value
            if (end == -1)
            { end = document.cookie.length; }
            returnvalue = unescape(document.cookie.substring(offset, end));
        }
    }
    return returnvalue;
}
//验证用户是否登陆
function isLogined() {
    var oCookie = getCookie('Aspoo');
    if (oCookie == null || oCookie == '') {
        return false;
    }
    var arrS = oCookie.split('&');
    var arrID = arrS[0].split('=');
    var arrPass = arrS[1].split('=');
    if (parseInt(arrID[1]) < 1 || arrPass[1] == '') {
        return false;
    }
}
//遮罩层
function lockShade(t) {
    var conDiv = document.getElementById('Shade_Div');
    if (t == true) {
        if (!conDiv) {
            conDiv = document.createElement("div");
            conDiv.id = "Shade_Div";
            conDiv.style.top = 0;
            conDiv.style.left = 0;
            conDiv.style.width = "100%";
            conDiv.style.height = $(document).height() + "px";
            conDiv.style.position = "absolute";
            conDiv.style.background = "#000000";
            conDiv.style.zIndex = 10000;
            if (typeof conDiv.style.opacity == "undefined") {
                conDiv.style.filter = "alpha(opacity=20)";
            } else {
                conDiv.style.opacity = "0.2";
            }
            document.body.appendChild(conDiv);
        }
        conDiv.style.display = "block";
    }
    else { if (conDiv) { conDiv.style.display = "none"; } }
}
//列表页中资料的收藏方法
function ShowFavoriteBoxList(oid, otil) {
    if (isLogined() == false) {
        AlertMsg('notice', '未登录，请登陆后进行收藏', 1500); return;
    }
    var oLinkUrl = "http://www.zxxk.com/s" + oid + ".html";
    var options = { id: 'layerFavorite', ftitle: otil, furl: oLinkUrl, width: 460, height: 270 };
    popFavorite(options);
}
//列表页中资料的收藏方法
function popFavorite(options) {
    var defaults = {
        id: 'layerFavorite',
        title: '添加收藏',
        ftitle:'中学学科网',
        furl:'http://www.zxxk.com',
        width: 500,
        height: 400
    };
    var options = $.extend(defaults, options);
    if ($("#" + options.id)) {
        $("#" + options.id).remove();
    }
    lockShade(true);
    var oBoxHTML = "<div id=\"" + options.id + "\" class=\"layerbox\"><div class=\"t\"><span>" + options.title + "</span><img src=\"http://www.zxxk.com/Skins/images/Info/close_icon.png\" alt=\"关闭\" title=\"关闭\" onclick=\"popClose();\"/></div><div id=\"Content" + options.id + "\"></div></div>";
    $("body").append(oBoxHTML);
    var $layer = $("#" + options.id);
    var $oContent = $("#Content" + options.id);
    $oContent.css({ height: parseInt(options.height) - 25 + 'px' });
    var oHTML = "<table cellpadding=\"0\" cellspacing=\"0\" width=\"100%\" class=\"fbox\">";
    oHTML += "<tr style=\"background-color:#ffffff;height:35px\">";
    oHTML += "<td style=\"width:80px;text-align:right\">标题：</td><td>&nbsp;<input id=\"FavoriteTitle\" type=\"text\" class=\"text\" maxlength=\"100\" value=\"" + options.ftitle + "\" /></td>";
    oHTML += "</tr>";
    oHTML += "<tr style=\"background-color:#ffffff;height:35px\">";
    oHTML += "<td align=\"right\">地址：</td><td>&nbsp;<input id=\"FavoriteUrl\" type=\"text\" class=\"text\" maxlength=\"100\" value=\"" + options.furl + "\" /></td>";
    oHTML += "</tr>";
    oHTML += "<tr style=\"background-color:#ffffff;height:35px\">";
    oHTML += "<td align=\"right\">备注：</td><td>&nbsp;<textarea id=\"FavoriteRemark\" cols=\"20\" rows=\"2\" class=\"textarea\"></textarea></td>";
    oHTML += "</tr>";
    oHTML += "<tr style=\"background-color:#ffffff;height:35px\">";
    oHTML += "<td align=\"right\">共享：</td><td>&nbsp;<input id=\"rdUnPub\" type=\"radio\" name=\"share\" value=\"0\"/><label for=\"rdUnPub\">私有</label>&nbsp;&nbsp;<input id=\"rdPub\" type=\"radio\" name=\"share\" value=\"1\" checked=\"checked\" /><label for=\"rdPub\">共享</label></td>";
    oHTML += "</tr>";
    oHTML += "<tr style=\"background-color:#ffffff;height:30px\">";
    oHTML += "<td></td><td>&nbsp;<input type=\"button\" value=\"收 藏\" class=\"btn\" onclick=\"addFavoriteSave(this);\" /></td>";
    oHTML += "</tr></table>";
    $oContent.html(oHTML);
    $layer.css({ left: (($(document).width()) / 2 - (parseInt(options.width) / 2) - 5) + "px", width: options.width, height: options.height });

    var isIE6 = navigator.appVersion.indexOf("MSIE 6") > -1;
    if (isIE6) {
        $layer.css({ position: 'absolute' });
    }
}
//关闭弹出框
function popClose() {
    lockShade(false);
    $(".layerbox").remove();
}
//添加收藏
function addFavoriteSave(th) {
    var oTitle = $("#FavoriteTitle").val();
    var oUrl = $("#FavoriteUrl").val();
    var oRemark = $("#FavoriteRemark").val();
    var oIsPublic = $("input[name='share']:checked").val();
    if (oTitle == "") {
        AlertMsg('notice', '标题不能为空', 1200); $("#FavoriteTitle").focus(); return;
    }
    if (oUrl == "") {
        AlertMsg('notice', '地址不能为空', 1200); $("#FavoriteUrl").focus(); return;
    }
    var postdata = "action=api&Title=" + escape(oTitle) + "&Url=" + escape(oUrl) + "&Remark=" + escape(oRemark) + "&IsPublic=" + oIsPublic + "&t=" + Math.random();
    $(th).val("正在提交");
    $.ajax({
        type: 'get',
        url: 'http://user.zxxk.com/MainFUNC/MyFavorite.aspx',
        dataType: "jsonp",
        jsonp: "callback",
        data: postdata,
        success: function (data) {
            if (data.msg == "ok") {
                AlertMsg('success', '收藏成功', 1500);
                popClose();
            } else {
                AlertMsg('notice', data.msg, 1500);
                $(th).val("收 藏");
            }
        }, error: function () {
            AlertMsg('error', '收藏失败，请稍候重试', 1500);
            popClose();
        }
    });
}
//消息提示信息
//(t:类型){success:'成功',error:'失败',notice:'提示',load:'等待'}
//(msg:要提示的内容)
//(time:消息框消失时间(毫秒))
function AlertMsg(t, msg, time) {
    var tipHtml = '';
    var oHeight = $(document).height();
    var oScoreHeight = $(document).scrollTop();
    if (t == 'load') {
        tipHtml = '<img alt="" src="/images/layer/load.gif">' + (msg ? msg : '正在提交您的请求，请稍候...');
    } else if (t == 'notice') {
        tipHtml = '<span class="gtl_ico_hits"></span>' + msg
    } else if (t == 'error') {
        tipHtml = '<span class="gtl_ico_fail"></span>' + msg
    } else if (t == 'success') {
        tipHtml = '<span class="gtl_ico_succ"></span>' + msg
    }
    if ($('.msgbox_layer_wrap').length > 0) {
        $('.msgbox_layer_wrap').remove();
    }
    if (st) {
        clearTimeout(st);
    }
    if ($("#viewerPlaceHolder").length > 0) {
        $("body").prepend("<div class='msgbox_layer_wrap'><span id='mode_tips_v2' style='z-index: 10000;' class='msgbox_layer'><span class='gtl_ico_clear'></span>" + tipHtml + "<span class='gtl_end'></span></span></div>");
        $(".msgbox_layer_wrap").css({ top: ($("#viewerPlaceHolder").offset().top + 610 - oScoreHeight) + 'px' }).show();
    } else {
        $("body").prepend("<div class='msgbox_layer_wrap'><span id='mode_tips_v2' style='z-index: 10000;' class='msgbox_layer'><span class='gtl_ico_clear'></span>" + tipHtml + "<span class='gtl_end'></span></span></div>");
        $(".msgbox_layer_wrap").css({ _top: oHeight / 2 + oScoreHeight + 'px' }).show();
    }

    var st = setTimeout(function () {
        $(".msgbox_layer_wrap").hide();
        clearTimeout(st);
    }, time);
}