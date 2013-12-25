<%@ Page Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true" CodeFile="Default.aspx.cs"
    Inherits="_10_Piano_Default" Title="支持录音和回放的钢琴" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" src="../Silverlight.js"></script>

    <script type="text/javascript" src="Default.aspx.js"></script>

    <script type="text/javascript" src="Piano.xaml.js"></script>

    <script type="text/javascript" src="Piano.js"></script>

    <style type="text/css">
        .silverlightHost
        {
            height: 500px;
            width: 1024px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="SilverlightControlHost" class="silverlightHost">

        <script type="text/javascript">
            var txtInput = '<%= txtInput.ClientID %>';
            var txtName = '<%= txtName.ClientID %>';
			createSilverlight();
        </script>

        <asp:UpdateProgress ID="UpdateProgress1" runat="server">
            <ProgressTemplate>
                <div style="position: absolute; top: 0px; background-color: Red; color: White; z-index: 999">
                    Loading...</div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <asp:GridView ID="GridView1" runat="server" Style="position: absolute; top: 0px;
                    width: 1024px" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                    DataKeyNames="ID" DataSourceID="LinqDataSource1" RowStyle-HorizontalAlign="Center"
                    PageSize="2" OnSelectedIndexChanged="GridView1_SelectedIndexChanged">
                    <Columns>
                        <asp:CommandField ShowSelectButton="True" HeaderText="选择" ItemStyle-Width="40px"></asp:CommandField>
                        <asp:CommandField ShowDeleteButton="True" HeaderText="删除" ItemStyle-Width="40px"></asp:CommandField>
                        <asp:BoundField DataField="ID" HeaderText="ID" InsertVisible="False" ReadOnly="True"
                            SortExpression="ID" ItemStyle-Width="40px"></asp:BoundField>
                        <asp:BoundField DataField="Name" HeaderText="名称" SortExpression="Name"></asp:BoundField>
                        <asp:TemplateField HeaderText="乐谱" SortExpression="Details" ItemStyle-Width="700px">
                            <ItemTemplate>
                                <div style="overflow: hidden; width: 666px">
                                    <asp:Label ID="lblDetails" runat="server" Text='<%# Bind("Details") %>'></asp:Label>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:TextBox ID="txtInput" runat="server" Style="position: absolute; left: 200px;
                    top: 434px; width: 300px" />
                <asp:TextBox ID="txtName" runat="server" Style="position: absolute; left: 566px;
                    top: 434px;" />
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnAdd" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:LinqDataSource ID="LinqDataSource1" runat="server" ContextTypeName="Data.MusicBookDataContext"
            EnableDelete="True" TableName="MusicBook">
        </asp:LinqDataSource>
        <asp:Button ID="btnAdd" runat="server" Text="添加" Style="position: absolute; left: 710px;
            top: 434px;" OnClick="btnAdd_Click" />
    </div>
</asp:Content>
