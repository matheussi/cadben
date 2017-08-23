<%@ Page Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="operadoras.aspx.cs" Inherits="cadben.www._cad.operadora.operadoras" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    Operadoras
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <div class="panel panel-default">
                <div class="panel-heading text-right" style="position:relative;">
                    <div style="position:absolute; right:0; top:-70px;"><asp:Button ID="lnkNovo" Text="Novo associado" runat="server" EnableViewState="false" SkinID="botaoPadrao1" OnClick="lnkNovo_Click" /></div>
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
                    <asp:GridView ID="grid" runat="server" SkinID="gridPadraoProp" Width="100%" 
                        AutoGenerateColumns="False" AllowPaging="true" PageSize="100" 
                        DataKeyNames="ID" OnRowCommand="grid_RowCommand" OnRowDataBound="grid_RowDataBound">
                        <Columns>
                            <asp:BoundField DataField="Nome" HeaderText="Operadora" />

                            <asp:ButtonField ButtonType="Link" Text="Contratos" CommandName="ContratosAdm">
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
        </ContentTemplate>
    </asp:UpdatePanel>


    <!--Modal Contrato Adm-->
    <div class="modal" id="modalContratoAdm" tabindex="-1" role="dialog" aria-labelledby="myModalLabelContratoAdm" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header text-left">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h2 class="modal-title">Contratos Administrativos</h2>
                </div>
                <div class="modal-body text-center">
                    <asp:UpdatePanel ID="upContratoAdm" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtContratoAdmId" Visible="false" runat="server" />
                            <asp:TextBox ID="txtContratoAdmOperadoraId" Visible="false" runat="server" />
                            <asp:Panel ID="pnlContratosAdmDetalhe" runat="server" Visible="false">
                                <div class="alert alert-warning">
                                    <div class="row">
                                        <div class="col-md-3 text-left">
                                            Estipulante<br />
                                            <asp:DropDownList ID="cboContratoAdmEstipulante" runat="server" Width="100%" SkinID="comboPadrao1"/>
                                        </div>
                                        <div class="col-md-3 text-left">
                                            Descrição do contrato<br />
                                            <asp:TextBox ID="txtContratoAdmDescricao" runat="server" Width="100%" MaxLength="250" SkinID="txtPadrao" />
                                        </div>
                                        <div class="col-md-3 text-left">
                                            Número do contrato<br />
                                            <asp:TextBox ID="txtContratoAdmNumero" runat="server" Width="50%" MaxLength="60" SkinID="txtPadrao" />
                                        </div>
                                    </div><!---->
                                    <div class="row" style="margin-top:20px">
                                        <div class="col-md-3 text-left">
                                            Código da Filial<br />
                                            <asp:TextBox ID="txtContratoAdmCodFilial" runat="server" Width="100%" MaxLength="50" SkinID="txtPadrao" />
                                        </div>
                                        <div class="col-md-3 text-left">
                                            Código da Unidade<br />
                                            <asp:TextBox ID="txtContratoAdmCodUnidade" runat="server" Width="100%" MaxLength="50" SkinID="txtPadrao" />
                                        </div>
                                        <div class="col-md-3 text-left">
                                            Código da Administradora<br />
                                            <asp:TextBox ID="txtContratoAdmCodAdministradora" runat="server" Width="100%" MaxLength="50" SkinID="txtPadrao" />
                                        </div>
                                    </div><!---->
                                    <div class="row" style="margin-top:20px">
                                        <div class="col-md-3 text-left">
                                            Contrato saúde<br />
                                            <asp:TextBox ID="txtContratoAdmSaude" runat="server" Width="100%" MaxLength="50" SkinID="txtPadrao" />
                                        </div>
                                        <div class="col-md-3 text-left">
                                            Contrato dental<br />
                                            <asp:TextBox ID="txtContratoAdmDental" runat="server" Width="100%" MaxLength="50" SkinID="txtPadrao" />
                                        </div>
                                        <div class="col-md-3 text-left">
                                            Status<br />
                                            <asp:CheckBox ID="chkContratoAdmAtivo" Checked="true" runat="server" style="font-weight:normal" Text="Este contrato está ativo" />
                                        </div>
                                        <div class="col-md-3 text-center">
                                            <asp:Button style="margin-bottom:-40px" ID="cmdContratoAdmCancelar" Text="Cancelar" runat="server" SkinID="botaoPadraoWarning_Small" OnClick="cmdContratoAdmCancelar_Click" />
                                            &nbsp;
                                            <asp:Button style="margin-bottom:-40px" ID="cmdContratoAdmSalvar" Text="Gravar" runat="server" SkinID="botaoPadraoWarning_Small" OnClick="cmdContratoAdmSalvar_Click" />
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>
                            <asp:Panel ID="pnlContratosLista" runat="server" Visible="true">
                                <div class="col-md-12">
                                    <div class="form-group">
                                        <div class="col-md-12 text-center">
                                            <asp:Button ID="cmdContratoAdmNovo" Text="Novo Contrato Administrativo" runat="server" SkinID="botaoPadraoWarning_Small" OnClick="cmdContratoAdmNovo_Click" />
                                        </div>
                                    </div>
                                </div>
                                <div class="col-md-12">
                                    <asp:GridView ID="GridContratoAdm" runat="server" SkinID="gridStrib" Width="100%" 
                                        AutoGenerateColumns="False" AllowPaging="true" PageSize="100" DataKeyNames="ID" 
                                        OnRowCommand="gridContratoAdm_RowCommand" OnRowDataBound="gridContratoAdm_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="Numero" HeaderText="Número"  />
                                            <asp:BoundField DataField="Descricao" HeaderText="Contrato" DataFormatString="{0:C}" />
                                            <asp:TemplateField HeaderText="Estipulante">
                                                <itemtemplate>
                                                    <%#DataBinder.Eval(Container.DataItem, "AssociadoPJ.Nome")%>
                                                </itemtemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DataCadastro" HeaderText="Data" DataFormatString="{0:dd/MM/yyyy}" />
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
                                        <RowStyle HorizontalAlign="Left" />
                                    </asp:GridView>
                                    <div class="clearfix"></div>
                                </div>
                                <div class="clearfix"></div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-info" data-dismiss="modal" id="cmdFecharModalContratoAdm">Fechar</button>
                </div>
            </div>
        </div>
    </div>

    <script>
        function showmodalContratoAdm()
        {
            $('#modalContratoAdm').modal(true);
        }
    </script>
</asp:Content>
