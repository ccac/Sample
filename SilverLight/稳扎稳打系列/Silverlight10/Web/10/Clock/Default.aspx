<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs"
    Inherits="_10_Clock_Default" Title="电子表" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="../Silverlight.js"></script>

    <script type="text/javascript" src="Default.aspx.js"></script>

    <script type="text/javascript" src="Clock.xaml.js"></script>

    <script type="text/javascript" src="Clock.js"></script>

    <style type="text/css">
        .silverlightHost
        {
            height: 150px;
            width: 180px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div id="SilverlightControlHost" class="silverlightHost">

        <script type="text/javascript">
			createSilverlight();
        </script>

    </div>
</asp:Content>
