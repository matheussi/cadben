<%@ Page Theme="metronic1"  Language="C#" MasterPageFile="~/layout2.Master" AutoEventWireup="true" CodeBehind="default2.aspx.cs" Inherits="cadben.www.default2" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>

<asp:Content ID="topo" ContentPlaceHolderID="title" runat="server">
    Início
    <!--<small>Você está na página inicial do sistema CadBen</small>-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="content" runat="server">
    <div class="note note-info">
        <p>Você está na página inicial do sistema CadBen</p>
    </div>
    <asp:UpdatePanel ID="upTaxas" runat="server" EnableViewState="true">
                <ContentTemplate>
    <asp:TextBox runat="server" ID="temp"></asp:TextBox>
    <asp:Button ID="cmdteste" runat="server" OnClick="cmdteste_Click" Text="teste" />
    <asp:Button ID="cmdteste2" runat="server" OnClick="cmdteste2_Click" Text="teste2" />
    <asp:Literal ID="litteste" runat="server" />
                    </ContentTemplate>
        </asp:UpdatePanel>
</asp:Content>