<%@ Page Language="C#" Theme="metronic1" MasterPageFile="~/layout2.Master" AutoEventWireup="true" CodeBehind="estipulantes.aspx.cs" Inherits="cadben.www._cad.estipulante.estipulantes" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="title" runat="server">
    Estipulantes
    <!--<a class="btn red btn-outline sbold" data-toggle="modal" href="#basic"> View Demo </a><br />-->
    <a id="popSpan" style="display:none" class="btn btn-outline dark" data-target="#stack1" data-toggle="modal"> View Demo </a>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="content" runat="server">
    <asp:UpdatePanel ID="up" runat="server">
        <ContentTemplate>
            <div class="panel panel-default">
                <div class="panel-heading text-right" style="position:relative;">
                    <div style="position:absolute; right:0; top:-50px;"><asp:Button ID="lnkNovo" Text="Novo estipulante" runat="server" EnableViewState="false" SkinID="botaoPadrao1" OnClick="lnkNovo_Click" /></div>
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
                    <asp:GridView ID="grid" runat="server" SkinID="gridPadraoProp" Width="100%" AutoGenerateColumns="False" AllowPaging="true" PageSize="20" DataKeyNames="ID" OnRowCommand="grid_RowCommand" OnRowDataBound="grid_RowDataBound">
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
        </ContentTemplate>
    </asp:UpdatePanel>

    <!--Modal Taxas-->
    <div class="modal" id="modalTaxas" tabindex="-1" role="dialog" style="margin-top:15px !important" aria-labelledby="myModalLabelTaxas" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header text-left">
                    <button type="button" class="close" data-dismiss="modal" aria-hidden="true">&times;</button>
                    <h2 class="modal-title">Taxas</h2>
                </div>
                <div class="modal-body text-center">
                    <asp:UpdatePanel ID="upTaxas" runat="server">
                        <ContentTemplate>
                            <asp:TextBox ID="txtIdEstipulante" Visible="false" runat="server" />
                            <div class="col-md-12 alert alert-warning"><div class="clearfix"></div>
                                <div class="form-group">
                                    <label class="col-md-1 control-label">Vigência</label>
                                    <div class="col-md-2"><asp:TextBox ID="txtVigencia" runat="server" Width="100%" MaxLength="10" onkeypress="filtro_SoNumeros(event); mascara_DATA(this, event);" SkinID="txtPadrao" /></div>
                                    <label class="col-md-1 control-label">Valor</label>
                                    <div class="col-md-2"><asp:TextBox ID="txtTaxaValor" onkeypress="filtro_SoNumeros(event);" MaxLength="10" runat="server" Width="100%" SkinID="txtPadrao" /></div>
                                    <label class="col-md-1 control-label">Tipo</label>
                                    <div class="col-md-2">
                                        <asp:DropDownList ID="cboTaxaTipo" runat="server" Width="100%" SkinID="comboPadrao1">
                                            <asp:ListItem Value="-1" Text="selecione" Selected="True"></asp:ListItem>
                                            <asp:ListItem Value="0" Text="Por beneficiário"></asp:ListItem>
                                            <asp:ListItem Value="1" Text="Por proposta"></asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                    <div class="col-md-2"><asp:Button ID="cmdSalvar" Text="Gravar" runat="server" SkinID="botaoPadraoWarning_Small" OnClick="cmdSalvar_Click" /></div>
                                    <div class="clearfix"></div>
                                </div>
                                <div class="clearfix"></div>

                            </div>
                            <div class="col-md-12">
                                <asp:GridView ID="GridTaxa" runat="server" SkinID="gridStrib" Width="100%" 
                                    AutoGenerateColumns="False" AllowPaging="true" PageSize="100" DataKeyNames="ID" 
                                    OnRowCommand="gridTaxa_RowCommand" OnRowDataBound="gridTaxa_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="Vigencia" HeaderText="Vigência" DataFormatString="{0:dd/MM/yyyy}" />
                                        <asp:BoundField DataField="Valor" HeaderText="Valor" DataFormatString="{0:C}" />
                                        <asp:BoundField DataField="Tipo" HeaderText="Tipo" />
                                        <asp:ButtonField ButtonType="Link" Text="" CommandName="Excluir">
                                            <ItemStyle Width="1%" />
                                            <ControlStyle Width="1%" />
                                            <ControlStyle CssClass="glyphicon glyphicon-remove" />
                                        </asp:ButtonField>
                                    </Columns>
                                    <RowStyle HorizontalAlign="Left" />
                                </asp:GridView>
                                <div class="clearfix"></div>
                            </div>
                            <div class="clearfix"></div>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-info" data-dismiss="modal" id="cmdFecharModal">Fechar</button>
                </div>
            </div>
        </div>
    </div>

    <!-- stackable -->
    <div id="stack1" class="modal fade" tabindex="-1" data-focus-on="input:first">
        <div class="modal-header">
            <button type="button" class="close" data-dismiss="modal" aria-hidden="true"></button>
            <h4 class="modal-title">Stack One</h4>
        </div>
        <div class="modal-body">
            <p> One fine body… </p>
            <p> One fine body… </p>
            <p> One fine body… </p>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <p><asp:LinkButton ID="lnk" runat="server" Text="teste"></asp:LinkButton></p>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div class="form-group">
                <input class="form-control" type="text" data-tabindex="1"> </div>
            <div class="form-group">
                <input class="form-control" type="text" data-tabindex="2"> </div>
            <button class="btn blue" data-toggle="modal" href="#stack2">Launch modal</button>
        </div>
        <div class="modal-footer">
            <button type="button" data-dismiss="modal" class="btn btn-outline dark">Close</button>
            <button type="button" class="btn green">Ok</button>
        </div>
    </div>

    <script>
        $(document).ready(function () {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(configAutocomplete);
        });

        function showModalTaxas()
        {
            $('#modalTaxas').modal(true);
        }
    </script>
</asp:Content>
