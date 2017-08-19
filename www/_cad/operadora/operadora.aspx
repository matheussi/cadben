﻿<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="operadora.aspx.cs" Inherits="cadben.www._cad.operadora.operadora" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    Operadora
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">&nbsp;</h3>
                </div>
                <div class="panel-body">

                    <div class="form-group">
                        <label class="col-xs-2 control-label">Nome</label>
                        <div class="col-xs-10"><asp:TextBox ID="txtNome" runat="server" Width="70%" SkinID="txtPadrao" /></div>
                    </div>

                    <div class="form-group">
                        <label class="col-xs-2 control-label">CNPJ</label>
                        <div class="col-xs-10"><asp:TextBox ID="txtCNPJ" runat="server" onkeypress="filtro_SoNumeros(event);mascara_CNPJ(this,event);" Width="70%" SkinID="txtPadrao" MaxLength="18" /></div>
                    </div>

                    <div class="form-group">
                        <label class="col-xs-2 control-label pull-left">Situação</label>
                        <div class="col-xs-10"><asp:CheckBox ID="chkAtivo" Checked="true" Text="Ativa" runat="server" /> </div>
                    </div>

                    <div class="form-group">
                        <label class="col-xs-2 control-label pull-left">Cadastro</label>
                        <div class="col-xs-10"><asp:Literal ID="litCadastro" Text="" runat="server" /> </div>
                    </div>
                    <div class="form-group">
                        <label class="col-xs-2 control-label pull-left">Alteração</label>
                        <div class="col-xs-10"><asp:Literal ID="litAlteracao" Text="" runat="server" /> </div>
                    </div>

                    <hr />
                    <div class="col-xs-12 text-right">
                        <asp:Button ID="cmdVoltar" Text="Voltar" runat="server" OnClick="cmdVoltar_Click" EnableViewState="false" SkinID="botaoPadrao1" />
                        <asp:Button ID="cmdSalvar" Text="Salvar" runat="server" OnClick="cmdSalvar_Click" EnableViewState="false" SkinID="botaoPadrao1" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>