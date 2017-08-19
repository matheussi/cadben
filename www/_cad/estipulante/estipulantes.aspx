﻿<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="estipulantes.aspx.cs" Inherits="cadben.www._cad.estipulante.estipulantes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <style>
        .mfade {
          opacity:    0.3; 
          background: #000; 
          width:      100%;
          height:     100%; 
          z-index:    0;
          top:        0; 
          left:       0; 
          position:   fixed; 
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    Estipulantes
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <div class="panel panel-default">
                <div class="panel-heading text-right" style="position:relative;">
                    <div style="position:absolute; right:0; top:-70px;"><asp:Button ID="lnkNovo" Text="Novo estipulante" runat="server" EnableViewState="false" SkinID="botaoPadrao1" OnClick="lnkNovo_Click" /></div>
                    <div class="col-md-12">
                        <div class="row">
                            <label class="col-md-12 text-left">Filtro:</label>
                        </div>
                        <div class="row">
                            <div class="col-md-12">
                                <div class="col-md-9"  style="padding-left:0px;">
                                      <asp:TextBox ID="txtNome" Width="100%" SkinID="txtPadrao" runat="server" />
                                </div>
                                <div class="col-md-3">
                                    <div class="text-right col-md-12"><asp:Button ID="cmdProcurar" Text="Procurar" SkinID="botaoPadrao1" EnableViewState="false" runat="server" OnClick="cmdProcurar_Click" /></div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="clearfix"></div>
                </div>
                <div class="panel-body">
                    <div class="space"></div>
                    <asp:GridView ID="grid" runat="server" SkinID="gridPadraoProp" Width="100%" AutoGenerateColumns="False" AllowPaging="true" PageSize="100" DataKeyNames="ID" OnRowCommand="grid_RowCommand" OnRowDataBound="grid_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="Nome" HeaderText="Estipulante" />
                            
                            <asp:ButtonField ButtonType="Link" Text="Taxas" CommandName="Taxas">
                                <ItemStyle Width="1%" />
                                <ControlStyle Width="1%" />
                                <ControlStyle />
                            </asp:ButtonField>

                            <asp:ButtonField ButtonType="Link" Text="" CommandName="Excluir">
                                <ItemStyle Width="1%" />
                                <ControlStyle Width="1%" />
                                <ControlStyle CssClass="glyphicon glyphicon-remove" />
                            </asp:ButtonField>
                            <asp:ButtonField ButtonType="Link" Text="" CommandName="Editar">
                                <ItemStyle Width="1%" />
                                <ControlStyle Width="1%" />
                                <ControlStyle CssClass="glyphicon glyphicon-pencil" />
                            </asp:ButtonField>
                        </Columns>
                    </asp:GridView>
                    <asp:Literal ID="litMensagem" EnableViewState="false" runat="server" />
                </div>
            </div>

            <ajaxToolkit:ModalPopupExtender ID="mpeTaxas" runat="server" PopupControlID="pnlTaxas" CancelControlID="cmdFechar" TargetControlID="targetTaxas"></ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="pnlTaxas" runat="server">
                <asp:LinkButton runat="server" EnableViewState="false" ID="targetTaxas" />
                <asp:TextBox ID="txtIdEstipulante" Visible="false" runat="server" />
                    <div class="mfade"></div>
                    <div class="col-xs-10 alert alert-warning ">
                        <div class="form-group">
                            <label class="col-xs-2 control-label">Valor</label>
                            <div class="col-xs-2"><asp:TextBox ID="txtTaxaValor" runat="server" Width="100%" SkinID="txtPadrao" /></div>
                            <label class="col-xs-2 control-label">Tipo</label>
                            <div class="col-xs-2"><asp:DropDownList ID="cboTaxaTipo" runat="server" Width="100%" SkinID="comboPadrao1" /></div>
                            <label class="col-xs-2 control-label">Vigência</label>
                            <div class="col-xs-2"><asp:TextBox ID="txtVigencia" runat="server" Width="100%" SkinID="txtPadrao" /></div>
                        </div>
                        <div class="form-group">
                            <div class="col-xs-6 text-center"><asp:Button ID="cmdFechar" Text="Fechar" runat="server" SkinID="botaoPadraoINFO_Small" /></div>
                            <div class="col-xs-6 text-center"><asp:Button ID="cmdSalvar" Text="Gravar" runat="server" SkinID="botaoPadraoDANGER_Small" /></div>
                        </div>
                    </div>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>