<%@ Page Language="C#" CodeBehind="rdlc.aspx.cs" Inherits="Easyman.Web.Rdlc.rdlc" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="../Scripts/jquery-2.2.4.min.js"></script>
    <style>
        table{
            width:100%
        }
        body{
            padding:0px;
            margin:0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:HiddenField runat="server" ID="code" />
            <asp:HiddenField runat="server" ID="queryParams" />
            <asp:HiddenField runat="server" ID="rpName" />
            <asp:HiddenField runat="server" ID="xmlStr" />
            <asp:ScriptManager ID="ScriptManager1" runat="server">
            </asp:ScriptManager>
            <rsweb:ReportViewer ID="reportViewer1" runat="server"  DocumentMapWidth="100%" Font-Names="Verdana" 
                Font-Size="8pt" WaitMessageFont-Names="Verdana"  AsyncRendering="False" SizeToReportContent="True" 
                WaitMessageFont-Size="14pt" Width="100%" Height="100%" ZoomMode="FullPage" >
            </rsweb:ReportViewer>
        </div>
        <div style="display:none">
            <asp:Button ID="SearchBtn" runat="server" OnClick="SearchBtn_Click" Text="查询" />
        </div>
        <script>
            $(function () {
                FullScreen();                
            });
            //列表全屏显示
            var FullScreen = function () {
                $("table").each(function () {
                    var strClass = $(this).attr("class");
                    var strLang = $(this).attr("lang");
                    if (strClass != null && strLang != null && strLang == "zh-CN") {//查找目标列表
                        //if (strClass != null && strLang != null && strClass.indexOf("_r10") >= 0 && strLang == "zh-CN") {//查找目标列表
                        $(this).attr("style", "width:100%");
                        var isRemoveGo = false;
                        var objTD = "";//存储目标列表
                        $("." + $(this).attr("class") + " tr td").each(function () {
                            if (isRemoveGo) {//移出非首个单元格
                                $(this).remove();
                            } else {
                                objTD = $(this).html();
                            }
                            isRemoveGo = true;
                        });
                        $("." + $(this).attr("class") + " tr td").eq(0).html(objTD);//重新将目标列表存回对应的位置
                        $("." + $(this).attr("class") + " tr td table tr").eq(0).remove();//移出空白的行
                        $("." + $(this).attr("class") + " tr td table tr").each(function () {
                            $(this).children("td").eq(0).remove();//移出首个空白的单元格
                        });
                        return false;
                    }
                });
            }
        </script>
    </form>
</body>
</html>
