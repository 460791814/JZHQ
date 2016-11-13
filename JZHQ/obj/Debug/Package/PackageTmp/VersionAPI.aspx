<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="VersionAPI.aspx.cs" Inherits="JZHQ.VersionAPI" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>

    <form id="form1" action="VersionAPI.aspx" runat="server">
    <div>
        <table border="0" cellpadding="0" cellspacing="0">
        <tr>
            <td>当前版本</td><td>下载地址</td>
            
        </tr>
                <tr>
            <td>
                <input type="hidden" name="method" value="update" />
                <input type="text" name="version" value="<%=Version %>" /></td><td><input type="text" style="width:400px"  name="down" value="<%=DownString %>" /></td>
        </tr>
        <tr>
         <td>
             <input type="submit" name="name" value="修改" /></td>
        </tr>
    </table>
    </div>
    </form>
</body>
</html>
