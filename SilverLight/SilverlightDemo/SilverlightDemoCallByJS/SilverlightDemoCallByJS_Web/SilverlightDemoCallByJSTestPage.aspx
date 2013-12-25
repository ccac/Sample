<%@ Page Language="C#" AutoEventWireup="true" %>

<%@ Register Assembly="System.Web.Silverlight" Namespace="System.Web.UI.SilverlightControls"
    TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" style="height:100%;">
<head runat="server">
    <title>Test Page For SilverlightDemoCallByJS</title>
    <script type="text/javascript">
        
        function callSilverlight()
        {
            var slPlugin = document.getElementById('Xaml1');
            
            slPlugin.content.Calculator.Add(document.getElementById('txt1').value,document.getElementById('txt2').value);
        }
        
        function callSilverlight1()
        {
            var slPlugin = document.getElementById('Xaml1');
            
            var c = slPlugin.content.services.createObject("cal");
            
            alert(c.Add(document.getElementById('txt1').value,document.getElementById('txt2').value));
        }

    </script>
</head>
<body style="height:100%;margin:0;">
    <form id="form1" runat="server" style="height:100%;">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <div class="main">
            <input id="txt1" type="text" />
            <input id="txt2" type="text" />
            <input id="Button1" type="button" value="确 定" onclick="callSilverlight();callSilverlight1()" />
        </div>
        <div  style="height:100%;">
            <asp:Silverlight ID="Xaml1" runat="server" Source="~/ClientBin/SilverlightDemoCallByJS.xap" Version="2.0" Width="100%" Height="100%" />
        </div>
    </form>
</body>
</html>