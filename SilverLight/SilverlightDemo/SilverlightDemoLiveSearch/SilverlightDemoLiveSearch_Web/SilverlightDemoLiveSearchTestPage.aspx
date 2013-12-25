<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" style="height:100%;">
<head runat="server">
    <title>Test Page For SilverlightDemoLiveSearch</title>
    <style type="text/css">
        #result
        {
            margin-left:20px;
        }
        .urlstyle
        {
            color:#59990E;
        }
        .itemstyle
        {
            border-bottom:dotted 1px #59990E;
            margin-bottom:20px;
        }
    </style>
</head>
<body style="height:100%;margin:0;">
    <form id="form1" runat="server" style="height:100%;">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div  style="height:100%;">
            <asp:Silverlight ID="Xaml1" runat="server" Source="~/ClientBin/SilverlightDemoLiveSearch.xap" Version="2.0" Width="100%" Height="150" />
            <div id="result"></div>
        </div>
    </form>
</body>
</html>