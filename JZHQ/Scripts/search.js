$(function () {

    $("#keyWord").keydown(function (event) {
        var e = event || window.event || arguments.callee.caller.arguments[0];
        if (e && e.keyCode == 27) { // 按 Esc 
            //要做的事情
        }
        if (e && e.keyCode == 113) { // 按 F2 
            //要做的事情
        }
        if (e && e.keyCode == 13) { // enter 键
            //要做的事情
            LoadKey(encodeURIComponent(validateStr($("#keyWord").val().replace(/[ ]/g, ""))));
        }

    })

    $("#btnSouba").click(function () {

        LoadKey(encodeURIComponent(validateStr($("#keyWord").val().replace(/[ ]/g, ""))));
    })


})
//过滤危险字符
function validateStr(str) {
    str = str.replace(/$/gi, "");
    str = str.replace(/</gi, "《");
    str = str.replace(/>/gi, "》");
    //str=str.replace(/ /gi,"");
    str = str.replace(/%/gi, "");
    str = str.replace(/--/gi, "");
    str = str.replace(/;/gi, "");
    str = str.replace(/\./gi, "");
    str = str.replace(/\*/gi, "");
    str = str.replace(/\&/gi, "");
    str = str.replace(/\-/gi, "");
    str = str.replace(/\//gi, "");
    return str;
}
function LoadKey(keyWord) {
    if(keyWord.replace(/[ ]/g,"")!=""){
    $.ajax({
        url: "/Handler/AddToKeyWord.ashx",
        data: "KeyWord=" + keyWord,
        dataType: "json",
        type: "post",
        contentType: "application/x-www-form-urlencoded; charset=utf-8"
    });

    window.location.href = "/list/" + keyWord + "/1";
	}
}
function random() {

    LoadKey(encodeURIComponent(validateStr("波多野结衣")));
}

function hitSoft(h) {
    $.ajax({
        url: "/Handler/HitSoft.ashx",
        data: "Hash=" + h,
        dataType: "json",
        type: "post",
        contentType: "application/x-www-form-urlencoded; charset=utf-8"
    });
}