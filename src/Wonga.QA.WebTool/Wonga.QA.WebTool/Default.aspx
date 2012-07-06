<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.master" AutoEventWireup="true"
    CodeBehind="Default.aspx.cs" Inherits="Wonga.QA.WebTool._Default" %>

<asp:Content ID="HeaderContent" runat="server" ContentPlaceHolderID="HeadContent">
</asp:Content>
<asp:Content ID="BodyContent" runat="server" ContentPlaceHolderID="MainContent">
    <div class="content-wrapper">
        <div id="centralWrapper">
            <div class="aut">
                <asp:RadioButtonList ID="_aut" runat="server">
                </asp:RadioButtonList>
            </div>
            <div class="sut">
                <asp:RadioButtonList ID="_sut" runat="server">
                </asp:RadioButtonList>
            </div>
            <div class="customer">
                <asp:RadioButtonList ID="_customer" runat="server" OnSelectedIndexChanged="_customer_SelectedIndexChanged">
                </asp:RadioButtonList>
            </div>
            <div class="parameters" style="display: none;">
                <asp:TextBox ID="_parameters" runat="server"></asp:TextBox>
            </div>
        </div>
        <div id="bottomWrapper">
            <asp:Button ID="runButton" runat="server" Text="Make Customer" OnClick="runButton_Click" />
        </div>
        <div id="tableWrapper">
            <img src="../img/processing.gif" runat="server" ID="progressing_icon" style="display: none;"/>
            <asp:Table ID="resultTable" runat="server" Style="display:none;">
            </asp:Table>
        </div>
    </div>
    <div class="popup-wrapper" ID="wrapperp" style="display: none;" runat="server">
        <div class="popup">
            <asp:Label ID="errorLabel" runat="server" Font-Names="Times New Roman" Font-Size="X-Large"
                ForeColor="#FF3300" OnUnload="errorLabel_Unload"></asp:Label>
            <a href="#" id="ok_button" style="text-decoration: none;"><img src="../img/button_ok.png" /></a>
        </div>
    </div>
</asp:Content>
