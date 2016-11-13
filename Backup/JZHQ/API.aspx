<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="API.aspx.cs" Inherits="JZHQ.API" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="/Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript">
        function json() {
            var m = $("#s").val();
            var postdata = "method=" + m;
            if (m == "get_info") {
                postdata = postdata + "&Hash=" + $("#Hash").val();
            }
            if (m == "get_list") {
                postdata = postdata + "&KeyWord=" + $("#KeyWord").val() + "&PageIndex=" + $("#PageIndex").val() + "&PageSize=" + $("#PageSize").val();
            }

            $.ajax({
                url: "/API.aspx",
                data: postdata,
                type: "Post",
                success: function (data) {

                    $("#txt").text(data);

                },
                error: function () {

                }
            });
        }
    </script>
</head>
<body>
    <form id="form1" action="API.aspx" method="get" runat="server">
    <div>
        <select id="s">
            <option value="get_list">获取列表数据</option>
             <option value="get_info">获取详情数据</option>
        </select>
        <br />
        <div>
        <label>输入关键词</label> <input type="text" id="KeyWord" name="KeyWord" value="" />
           <br />
         <label>输入PageIndex</label> <input type="text" id="PageIndex" name="PageIndex" value="1" />
            <br />
          <label>输入PageSize</label> <input type="text" id="PageSize" name="PageSize" value="10" />
          </div>
        <br />
        <label>输入HASH</label> <input type="text" id="Hash" name="Hash" value="" />
           <br />
        <input type="button" name="name" value="API查询"  onclick="json()" />
    </div>
<textarea id="txt"></textarea>
    </form>
</body>
</html>
