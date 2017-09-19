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
    <asp:TextBox runat="server" ID="temp"></asp:TextBox>
</asp:Content>