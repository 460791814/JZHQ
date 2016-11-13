//Time: 2013-8-29
//Author:WangYY
//Update: 2013-9-13

//加载样式
var xkCommon_css = document.createElement('link');
xkCommon_css.rel = 'stylesheet';
xkCommon_css.type = 'text/css';
xkCommon_css.href = 'xkcommon/xkCommon.css';
document.getElementsByTagName('head')[0].appendChild(xkCommon_css);

//是否为IE6
function isIE6() {
    var ua = navigator.userAgent.toLowerCase();
    //如果是IE6则直接显示浮动层
    if (ua.indexOf("msie 6") > -1) {
        return true;
    } else {
        return false;
    }
}
//关闭弹出层
function closeDialog() {
    $("div[name=xk_dialog]").stop(false, true).animate({ marginTop: "-=100px", opacity: "0" }, 300, function () {
        lockShade(false);
        $(this).remove();
    });
}
//====================浮动下拉框(通用)======================//
(function ($) {
    $.fn.xkDropdown = function (options) {
        if (!this.length) {
            return;
        }
        var option = $.extend({
            event: "mouseover",   //事件类型
            current: "curr",      //样式名称
            delay: 150            //延时时间
        }, options || {});
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
                    }, option.delay)
                }
            }).bind(b, function () {
                if (isShow) {
                    var root = $(this);
                    timer2 = setTimeout(function () {
                        root.removeClass(option.current);
                        isShow = false
                    }, option.delay)
                } else {
                    clearTimeout(timer1)
                }
            });
        });
    }
})(jQuery);

//================选项卡切换（通用）==================//
(function ($) {
    $.fn.xkTabs = function (options) {
        if (!this.length) {
            return;
        }
        var option = $.extend({
            event: "mouseover",             //标签的事件
            currClass: "curr",              //选中状态Class
            hookKey: "data-tab",            //特定标签属性
            hookItemVal: "tab-item",        //切换标签属性参数
            hookContentVal: "tab-content"   //切换内容的标签属性参数
        }, options || {});
        $(this).each(function () {
            var item = $(this).find("*[" + option.hookKey + "=" + option.hookItemVal + "]");
            var con = $(this).find("*[" + option.hookKey + "=" + option.hookContentVal + "]");
            if (item.length != con.length) {
                return false;
            }
            item.each(function () {
                $(this).bind(option.event, function () {
                    item.removeClass(option.currClass);
                    $(this).addClass(option.currClass);
                    var _index = item.index(this);
                    con.hide();
                    con.eq(_index).show();
                })
            })
        });
    }
})(jQuery);

//=================选项卡切换（动画） (通用)===============//
(function ($) {
    $.fn.xkTabsMove = function (options) {
        if (!this.length) {
            return;
        }
        var option = $.extend({
            event: "mouseover",               //标签的事件
            currClass: "curr",                //选中状态Class
            hookKey: "data-move",             //特定标签属性
            hookItemVal: "move-item",         //切换标签属性参数
            hookContentVal: "move-content",   //切换内容的标签属性参数
            hookLineVal: "move-line",         //移动标签属性参数
            itemWidth: 160,                   //移动的宽度
            speed: 200                        //移动速度
        }, options || {});
        $(this).each(function () {
            var item = $(this).find("*[" + option.hookKey + "=" + option.hookItemVal + "]");
            var con = $(this).find("*[" + option.hookKey + "=" + option.hookContentVal + "]");
            var line = $(this).find("*[" + option.hookKey + "=" + option.hookLineVal + "]");
            if (item.length != con.length) {
                return false;
            }
            item.each(function (i) {
                $(this).bind(option.event, function () {
                    item.removeClass(option.currClass);
                    $(this).addClass(option.currClass);
                    var a1 = option.itemWidth * i;
                    line.animate({ left: a1 }, option.delay);
                    var _index = item.index(this);
                    con.hide();
                    con.eq(_index).show();
                })
            })
        });
    }
})(jQuery);

//===================轮换图(通用)==================//
(function ($) {
    $.fn.xkSlider = function (options) {
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
        }, options || {});
        var item = null,
        con = null,
        timer = null,
        timer1 = null,
        pt = null;
        var dafault = function () {
            con = oRoot.find("*[" + d.hookKey + "=" + d.hookContentVal + "]");
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
                oRoot.append(slNum);
                item = oRoot.find("span");
            } else {
                item = oRoot.find("*[" + d.hookKey + "=" + d.hookItemVal + "]");
                if (con.length != item.length) {
                    return false;
                }
            }
            pt = con.eq(0).parent();
            oRoot.find(".shade").css({ opacity: "0.3" });
            Mevent();
            if (d.auto) {
                ltime();
            }
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
            timer1 = setInterval(function () {
                var index = d.defaultIndex;
                index++;
                if (index == con.length) {
                    index = 0
                }
                SlideS(index);
            }, d.delay);
        };
        dafault();
    }
})(jQuery);
//===================带按钮的轮换图(通用)================//
(function ($) {
    $.fn.xkScrollPic = function (options) {
        if (!this.length) {
            return;
        }
        var oRoot = this;
        var d = $.extend({
            auto: true,                       //默认是否切换
            hoolKey: "data",                 //自定义标签属性
            hoolCont: 'scroll-cont',         //容器标识名称                                        
            hoolPrev: "scroll-prev",         //上一张标识名称
            hoolNext: "scroll-next",         //下一张标识名称
            hoolItem: "scroll-item",         //要移动的标识名称
            scrollCount: 1,                  //每次移动的数量
            itemWidth: 360,                  //要移动标签的宽度
            opacity: 0.8,                     //上下张按钮透明度
            speed: 500,                      //滑动时间（毫秒）
            delay: 3000                      //切换时间间隔（毫秒）
        }, options || {});
        var prev = null,
        next = null,
        cont = null,
        item = null,
        timer = null;
        var iNum = 0;
        var dafault = function () {
            prev = oRoot.find("*[" + d.hoolKey + "=" + d.hoolPrev + "]");
            next = oRoot.find("*[" + d.hoolKey + "=" + d.hoolNext + "]");
            cont = oRoot.find("*[" + d.hoolKey + "=" + d.hoolCont + "]");
            item = oRoot.find("*[" + d.hoolKey + "=" + d.hoolItem + "]");
            if (item.length < 1) return false;
            cont.css({ width: item.length * d.itemWidth + "px" });
            prev.css({ opacity: d.opacity });
            next.css({ opacity: d.opacity });
            Mevent();
            if (d.auto) {
                ltime();
            }
        },
        Mevent = function () {
            prev.bind("click", function () {
                Scroll('prev');
            });
            next.bind("click", function () {
                Scroll('next');
            });
            oRoot.bind("mouseover", function () {
                clearInterval(timer);
            }).bind("mouseleave", function () {
                ltime();
            });
        },
        Scroll = function (t) {
            var u = d.itemWidth * d.scrollCount; //移动的宽度
            if (t == 'prev') {
                for (var i = 0; i < d.scrollCount; i++) {
                    item = oRoot.find("*[" + d.hoolKey + "=" + d.hoolItem + "]");
                    item.last().insertBefore(item.first());
                }
                cont.css({ left: -u + "px" });
                cont.animate({
                    left: "0px"
                }, d.speed);
            } else {
                cont.animate({
                    left: -u + "px"
                }, d.speed, function () {
                    for (var i = 0; i < d.scrollCount ; i++) {
                        item = oRoot.find("*[" + d.hoolKey + "=" + d.hoolItem + "]");
                        item.first().insertAfter(item.last());
                    }
                    cont.css({ left: "0px" });
                });
            }
        },
        ltime = function () {
            timer = setInterval(function () {
                Scroll('next');
            }, d.delay);
        };
        dafault();
    }
})(jQuery);

//======================弹出层======================//
(function ($) {
    var xkDialog = function (options) {
        var s = $.extend({
            id: "xk_dialog",                //弹出层ID                  
            title: "弹出层",                //标题
            width: 420,                     //默认宽度
            height: 220,                    //默认高度
            dialogclass: "xk_dialog",       //样式名称
            type: "text",                   //显示类型 text/iframe
            content: "这是一个弹出层",      //显示内容，text输入文本，iframe时url地址
            isRadius: true,                 //是否圆角
            hasclose: true,                 //是否显示关闭按钮
            hassubmit: true,                //是否显示提交按钮
            hascancel: true,                //是否显示取消按钮
            submitname: "确认",             //提交按钮名称    
            cancelname: "取消",             //取消按钮名称
            submitclass: "submit",          //提交按钮样式
            cancelclass: "cancel",          //取消按钮样式
            submitcallback: null,           //提交按钮回调事件
            cancelcallback: null,           //取消按钮回调事件
            ismove: true,                   //是否允许移动
            zindex: 21000,                  //定位高度
            times: 0                        //延时时间
        }, options || {});
        if ($("." + s.dialogclass).length > 0) {
            $("." + s.dialogclass).remove();
        }
        //初始化标签
        var main = $("<div name=\"xk_dialog\"></div>"),
            titlebox = $("<div class=\"xk_title\"></div>"),
            title = $("<span></span>"),
            closebox = $("<a href=\"javascript:;\" title=\"关闭\">关闭</a>"),
            content = $("<div class=\"xk_content\"></div>"),
            iframe = $("<iframe id=\"dialog_iframe\" width=\"100%\" scrolling=\"auto\" frameborder=\"0\"></iframe>"),
            inputbox = $("<div class=\"input_box\"></div>"),
            submitbutton = $("<input type=\"button\" >").addClass(s.submitclass).val(s.submitname),
            cancelbutton = $("<input type=\"button\" >").addClass(s.cancelclass).val(s.cancelname);
        //是否显示submit按钮
        if (s.hassubmit) {
            inputbox.append(submitbutton);
        }
        //是否显示cancel按钮
        if (s.hascancel) {
            inputbox.append(cancelbutton);
        }
        //定义弹出层外层的宽度和高度
        var oMainHeight = s.height + 30, oMainWidth = s.width;
        //添加title到弹出层
        titlebox.append(title.html(s.title)).appendTo(main);
        //定义内容框样式并添加到弹出层中
        content.css({ height: s.height + "px", width: s.width + "px" }).appendTo(main);
        //验证是否添加按钮标签
        if (s.hassubmit || s.hascancel) {
            oMainHeight += 28;
            inputbox.css({ width: s.width + "px" }).appendTo(main);
        }
        //定义弹出层样式
        main.attr("class", s.dialogclass).css({ height: oMainHeight + "px", width: oMainWidth + "px" });
        //添加弹出层到body中
        $("body").append(main.css({ opacity: "0.2" })); lockShade(true);
        //定位弹出层
        var main_left = ($(window).width()) / 2 - (parseInt(oMainWidth) / 2) - 5;  //距离左边距离
        var main_top = ($(window).height() - oMainHeight) / 2 - 150;  //距离顶部距离

        var cw = document.documentElement.clientWidth, ch = document.documentElement.clientHeight, est = document.documentElement.scrollTop;
        if (isIE6()) {
            main.css({left:"50%", top: (parseInt((ch) / 2) + est) + "px", marginTop: -(oMainHeight / 2 + 150) + "px", marginLeft: -(oMainWidth / 2) + "px"});
        } else {
            main.css({ left: "50%", top:"50%", marginTop: -(oMainHeight / 2 + 150) + "px", marginLeft: -(oMainWidth / 2) + "px" });
        };
        //定义弹出层圆角
        if (s.isRadius) {
            main.addClass('xk_dialogRadius');
            if (window.curvyCorners && typeof (window.curvyCorners) == "function") {
                var setting = {
                    tl: { radius: 6 },
                    tr: { radius: 6 },
                    bl: { radius: 6 },
                    br: { radius: 6 },
                    antiAlias: true
                }
                curvyCorners(setting, "div[name='xk_dialog']");
            }
        }
        //弹出内容类型
        switch (s.type) {
            case "id":
                content.append($("<p class=\"xk_text\"></p>").html($("#" + s.content).html()));
                break;
            case "text":
                content.append($("<p class=\"xk_text\"></p>").html(s.content));
                break;
            case "iframe":
                var loading = $("<div class=\"load\">loading...</div>");
                content.append(loading).append(iframe);
                iframe.attr("src", s.content);
                if (navigator.appVersion.indexOf("MSIE 6") < 0) {
                    iframe.bind("load", function () {
                        loading.remove();
                        iframe.css({ height: "100%" });
                    });
                } else {
                    iframe.bind("readystatechange", function () {
                        if (this.readyState && this.readyState == 'complete') {
                            loading.remove(); iframe.css({ height: "100%" });
                        }
                    });
                }
                break;
        }
        //动画显示弹出层
        main.stop(false, true).animate({ marginTop: "+=100px", opacity: "1" }, 500);

        //close按钮关闭事件
        if (s.hasclose) {
            titlebox.append(closebox);
            closebox.click(closeDialog);
        }
        //添加按钮事件
        if (s.hassubmit) {
            submitbutton.click(function () { closeCallback(s.submitcallback); });
        }
        if (s.hascancel) {
            cancelbutton.click(function () { closeCallback(s.cancelcallback); });
        }
        //延时事件
        if (s.times > 0) {
            var dialogTimer = setTimeout(function () {
                closeDialog();
                clearTimeout(dialogTimer);
            }, s.times);
        }
        //拖动
        if (s.ismove) {
            moveDialog($("div[name=xk_dialog]"));
        }
    }
    //回调事件关闭方法
    function closeCallback(callback) {
        if (callback != null && typeof (callback) == "function") {
            callback();
        }
        closeDialog();
    }
    //拖动事件
    function moveDialog(_this) {
        if (typeof (_this) == 'object') {
            _this = _this;
        } else {
            _this = $("#" + _this);
        }
        if (!_this) { return false; }      
        var cw = document.documentElement.clientWidth, ch = document.documentElement.clientHeight, est = document.documentElement.scrollTop;        
        var Drag_ID = _this[0], DragHead = _this.find(".xk_title")[0];

        var moveX = 0, moveY = 0, moveTop, moveLeft = 0, moveable = false;
        if (isIE6() == 6.0) {
            moveTop = est;
        } else {
            moveTop = 0;
        }
        var sw = Drag_ID.scrollWidth, sh = Drag_ID.scrollHeight;
        DragHead.onmouseover = function (e) {
            DragHead.style.cursor = "move";
        };
        DragHead.onmousedown = function (e) {
            moveable = true;
            e = window.event ? window.event : e;
            var ol = Drag_ID.offsetLeft, ot = Drag_ID.offsetTop - moveTop;
            moveX = e.clientX - ol;
            moveY = e.clientY - ot;
            document.onmousemove = function (e) {
                if (moveable) {
                    e = window.event ? window.event : e;
                    var x = e.clientX - moveX;
                    var y = e.clientY - moveY;
                    if (x > 0 && (x + sw < cw) && y > 0 && (y + sh < ch)) {
                        Drag_ID.style.left = x + "px";
                        Drag_ID.style.top = parseInt(y + moveTop) + "px";
                        Drag_ID.style.margin = "auto";
                    }
                }
            }
            document.onmouseup = function () { moveable = false; };
            Drag_ID.onselectstart = function (e) { return false; }
        }
    }
    $.xkDialog = function (o) { return new xkDialog(o); }
    return $.xkDialog;
})(jQuery);

//============================浮动层（通用）===========================//
(function ($) {
    $.fn.xkPosition = function (options) {
        if (!this.length) {
            return;
        }
        var option = $.extend({
            type: 1,          //浮动类型(1:左上；2：左下；3右上；4：右下)
            ismove: true,     //是否动画
            num1: 10,         //距离上或下的距离
            num2: 10,         //距离左或右的距离
            scroll: 0,        //滚动条滚动距离，控制显隐   为0则不控制
            height: 100,      //距离底部高度
            timer: 3000
        }, options || {});
        var oRoot = this;
        oRoot.css({ position: "fixed" });
        var oScrollTop = 0;
        if (isIE6())
        {
            oScrollTop = document.documentElement.scrollTop;
        }
        switch (option.type) {
            case 1:
                oRoot.css({ left: option.num1 + "px", top: option.num2 + "px" });
                break;
            case 2:
                oRoot.css({ left: option.num1 + "px", bottom: option.num2 + "px" });
                break;
            case 3:
                oRoot.css({ right: option.num1 + "px", top: option.num2 + "px" });
                break;
            case 4:
                oRoot.css({ right: option.num1 + "px", bottom: option.num2 + "px" });
                break;
        }
        //如果滚动条控制显隐      
        if (option.scroll > 0) {
            $(window).scroll(function (e) {
                if ('pageYOffset' in window) {
                    oTop = window.pageYOffset;
                } else if (document.compatMode === "BackCompat") {
                    oTop = document.body.scrollTop;
                } else {
                    oTop = document.documentElement.scrollTop;
                }
                if (oTop > option.scroll) {
                    //如果有动画效果
                    if (option.ismove) {
                        if (oRoot.not(":animated")) {
                            oRoot.show().stop(true).animate({ top: ($(window).height() - oRoot.height() - option.height) + "px" }, option.timer);
                        }
                    } else {
                        oRoot.show();
                    }
                } else {
                    //如果有动画效果
                    if (option.ismove) {
                        oRoot.stop(false, true).hide().css({ top: option.num2 + "px" });
                    } else {
                        oRoot.hide();
                    }
                }
            });
        }
    }
})(jQuery);

//====================无缝滚动事件====================//
(function ($) {
    $.fn.xkRolling = function (options) {
        if (!this.length) {
            return;
        }
        var option = $.extend({
            labelName: 'li',    //要滚动的标签名称
            count: 4,           //要显示的个数
            times: 1000,        //渐隐时间（毫秒）
            delay: 4000         //滚动时间（毫秒）
        }, options || {});
        $(this).each(function () {
            var root = $(this),
                arrItems = [],
                items = null,
                height = 0,
                outerHeight = 0,
                current = option.count,
                total = 0;
            items = root.find(option.labelName);
            items.each(function () {
                arrItems.push($(this));
            });
            height = items.eq(0).height();
            outerHeight = items.eq(0).outerHeight(true);
            total = arrItems.length - 1;
            root.height(outerHeight * option.count);
            items.filter(':gt(' + (option.count - 1) + ')').remove();
            function spy() {
                var $insert = $(arrItems[current]).css({ height: "0px", opacity: "0" }).prependTo(root);
                items = root.find(option.labelName);
                items.last().stop(false, true).animate({ opacity: "0" }, option.times, function () {
                    $(this).remove();
                    $insert.animate({ height: height + "px" }, option.times).animate({ opacity: "1" }, option.times);
                });
                current++;
                if (current >= total) {
                    current = 0;
                }
                setTimeout(spy, option.delay);
            };
            spy();
        });
    }
})(jQuery);

//=====================手风琴效果======================//
(function ($) {
    $.fn.xkAccordion = function (options) {
        if (!this.length) {
            return;
        }
        var option = $.extend({
            labelName: "li",
            width: 250,
            times: 500
        }, options || {});
        $(this).each(function () {
            var root = $(this),
                w = 0,
                index = 0,
                count = 0,
                t = false,
                _this = null;
            count = root.find(option.labelName).length;
            w = (root.width() - option.width) / (count - 1);
            root.find(option.labelName).first().animate({ width: option.width + "px" });
            root.find(option.labelName).hover(function () {
                _this = $(this);
                index = root.find(option.labelName).index(_this[0]);
                var act = function () {
                    _this.siblings("li").removeClass("cur");
                    _this.animate({ "width": option.width + "px" }, { duration: option.times, easing: "easeOutQuart" }).siblings("li").animate({ "width": w + "px" }, { duration: option.times })
                }
                t = setTimeout(act, 200);
            }, function () {
                if (t) {
                    clearTimeout(t);
                }
            });
        });
    }
})(jQuery)

//=======================拖动（通用）===================//
(function ($) {
    $.Move = function (_this) {
        if (typeof (_this) == 'object') {
            _this = _this;
        } else {
            _this = $("#" + _this);
        }
        if (!_this) { return false; }

        _this.css({ 'position': 'absolute' }).hover(function () { $(this).css("cursor", "move"); }, function () { $(this).css("cursor", "default"); })
        _this.mousedown(function (e) {//e鼠标事件
            var offset = $(this).offset();
            var x = e.pageX - offset.left;
            var y = e.pageY - offset.top;
            $(document).bind("mousemove", function (ev) {//绑定鼠标的移动事件，因为光标在DIV元素外面也要有效果，所以要用doucment的事件，而不用DIV元素的事件
                _this.bind('selectstart', function () { return false; });
                var _x = ev.pageX - x;//获得X轴方向移动的值
                var _y = ev.pageY - y;//获得Y轴方向移动的值
                _this.css({ 'left': _x + "px", 'top': _y + "px" });
            });
        });
        $(document).mouseup(function () {
            $(this).unbind("mousemove");
        })
    };
})(jQuery);

//===================遮罩层================//
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
            conDiv.style.zIndex = 20000;
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

//===================消息提示信息====================//
var alert_Timer;
(function ($) {
    var xkAlertLayer = function (options) {
        var option = $.extend({
            type: "load",                             //类型(success:'成功',error:'失败',notice:'提示',load:'等待')
            msg: "正在提交您的请求，请稍候...",       //消息内容
            timer: 3000                               //显示时间（毫秒）
        }, options || {});
        var tipHtml = '';
        switch (option.type) {
            case "load":
                tipHtml = '<img alt="" src="/xkcommon/image/load.gif">' + option.msg;
                break;
            case "notice":
                tipHtml = '<span class="gtl_ico_hits"></span>' + option.msg;
                break;
            case "error":
                tipHtml = '<span class="gtl_ico_fail"></span>' + option.msg;
                break;
            case "success":
                tipHtml = '<span class="gtl_ico_succ"></span>' + option.msg;
                break;
        }
        if ($('.msgbox_layer_wrap').length > 0) {
            $('.msgbox_layer_wrap').remove();
        }
        if (alert_Timer) {
            clearTimeout(alert_Timer);
        }
        var oMain = $("<div class='msgbox_layer_wrap'></div>");
        var oContent = $("<span id='mode_tips_v2' style='z-index: 10000;' class='msgbox_layer'><span class='gtl_ico_clear'></span>" + tipHtml + "<span class='gtl_end'></span></span>");
        var main_top = $(window).height() / 2 - 260;
        if (isIE6()) {
            var oScoreHeight = document.documentElement.scrollTop;
            oMain.css({ position: "absolute", top: main_top + oScoreHeight + "px", opacity: "0" }).append(oContent);
        } else {
            oMain.css({ top: main_top + "px", opacity: "0" }).append(oContent);
        }        
        $("body").append(oMain);
        $(".msgbox_layer_wrap").show().animate({ top: "+=200px", opacity: "1" }, 500);

        alert_Timer = setTimeout(function () {
            $(".msgbox_layer_wrap").stop(false,true).animate({ top: "-=200px", opacity: "0" }, 500, function () {
                $(this).remove();
            });
            clearTimeout(alert_Timer);
        }, option.timer);
    }
    $.xkAlertLayer = function (o) { return new xkAlertLayer(o); }
    return $.xkAlertLayer;
})(jQuery);
//关闭alert浮动层
function closeAlertLayer() {
    if (alert_Timer) {
        clearTimeout(alert_Timer);
    }
    $(".msgbox_layer_wrap").animate({ top: "-=200px", opacity: "0" }, 500, function () {
        $(this).remove();
    });
}