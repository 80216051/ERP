<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="login.aspx.cs" Inherits="ERP.SystemPage.login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
         <div class="Content" style="z-index: 200">
        <table style="width: 300px;">
     
            <tr>
                <td>
                    <label class="label" style="padding-left: 22px;">
                        账号<asp:TextBox ID="txtUserName" runat="server"></asp:TextBox></label>
                </td>
            </tr>
            <tr>
                <td>
                    <label class="label" style="padding-left: 22px;">
                        密码<asp:TextBox ID="txtPass" TextMode="Password" runat="server"></asp:TextBox>
                    </label>
                </td>
            </tr>
            <tr>
                <td>
                    <label class="label" style="float: left;">
                        验证码
                        <asp:TextBox ID="txtvalidateCode" MaxLength="6" Style="width: 100px;" runat="server"></asp:TextBox>
                    </label>
                    <span style="float: left;">
                        <script type="text/javascript" language="JavaScript">
                            var numkey = Math.random();
                            numkey = Math.round(numkey * 10000);
                            document.write("<img style='width:70px;height:35px;' src=\"/Shared/imagecode.aspx?bgcolor=%23B66268&fontcolor=%23FFFFFF&weight=15&h=35&w=70&x=7&y=6&k=" + numkey + "\" onClick=\"this.src+=Math.random()\" title=\"图片看不清？点击重新得到验证码\" style=\"cursor:pointer;\" hspace=\"4\" />");
                        </script>
                    </span>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="diverror" style="background-color: #ECDADA; padding: 10px; display: none;"
                        class="box">
                    </div>
                    <asp:Button ID="btnInput" runat="server" Text="登录" OnClick="btnInput_Click" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
