$(function () {

    $("#input").keydown(function (event) {
        var e = event || window.event || arguments.callee.caller.arguments[0];
        if (e && e.keyCode == 27) { // 按 Esc 
            //要做的事情
        }
        if (e && e.keyCode == 113) { // 按 F2 
            //要做的事情
        }
        if (e && e.keyCode == 13) { // enter 键
            //要做的事情
            LoadKey(encodeURIComponent(validateStr($("#input").val().replace(/[ ]/g, ""))));
        }

    })

    $("#search-button").click(function () {
        LoadKey(encodeURIComponent(validateStr($("#input").val().replace(/[ ]/g, ""))));
     
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


    window.location.href = "/list/" + keyWord + "/1";
}
function random() {

    LoadKey(encodeURIComponent(validateStr("波多野结衣")));
}

function search() {

    LoadKey(encodeURIComponent(validateStr(document.getElementById("input").value.replace(/[ ]/g, ""))));
}