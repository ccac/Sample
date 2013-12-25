<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" style="height: 100%;">
<head runat="server">
    <title>Silverlight20</title>
</head>
<body style="height: 100%; margin: 0;">
    <form id="form1" runat="server" style="height: 100%;">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div style="height: 100%;">
        <!--
        ScaleMode - 指定插件如何缩放到所属的 object 标记（None - 不缩放；Stretch - 完全填充；Zoom - 等比填充）
        PluginNotInstalledTemplate - 指定浏览器在未安装 Silverlight 插件时所呈现的 HTML 标记
        -->
        <asp:Silverlight ID="Xaml1" runat="server" Source="~/ClientBin/Silverlight20.xap"
            MinimumVersion="2.0.31005.0" Width="100%" Height="100%" ScaleMode="Stretch">
            <PluginNotInstalledTemplate>
                <div style="width: 100%; margin: 100px 0px; text-align: center">
                    <a href="http://go.microsoft.com/fwlink/?LinkID=124807">
                        <img src="http://go2.microsoft.com/fwlink/?LinkID=108181" alt="获取 Microsoft Silverlight"
                            style="border-width: 0px" />
                    </a>
                </div>
            </PluginNotInstalledTemplate>
        </asp:Silverlight>
    </div>
    </form>
</body>
</html>
