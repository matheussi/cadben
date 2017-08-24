namespace cadben.www._cad.operadora
{
    using System;
    using System.Web;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Collections.Generic;
    using System.Collections.Specialized;

    using cadben.Facade;
    using cadben.Entidades;
    using cadben.www.seguranca;

    public partial class operadoras : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.carregar();
                this.carregarEstipulantes();
            }
        }

        void carregar()
        {
            var operadoras = OperadoraFacade.Instancia.CarregarTodas(Util.UsuarioLogado.IDContratante, txtNome.Text);
            grid.DataSource = operadoras;
            grid.DataBind();
        }

        void carregarEstipulantes()
        {
            cboContratoAdmEstipulante.Items.Clear();
            var estipulantes = EstipulanteFacade.Instancia.CarregarTodos(Util.UsuarioLogado.IDContratante);

            if (estipulantes != null)
            {
                estipulantes.ForEach(e => cboContratoAdmEstipulante.Items.Add(new ListItem(e.Nome, e.ID.ToString())));
            }

            cboContratoAdmEstipulante.Items.Insert(0, new ListItem("selecione", "0"));
        }

        protected void cmdProcurar_Click(object sender, EventArgs e)
        {
            this.carregar();
        }

        protected void lnkNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("contratante.aspx");
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                long id = Util.Geral.ObterDataKeyValDoGrid<long>(grid, e, 0);

                string urisegura = Util.Geral.PreparaQueryStringSegura(new string[] { Util.Keys.IdKey }, new string[] { id.ToString() });

                Response.Redirect("operadora.aspx?" + urisegura);
            }
            else if (e.CommandName.Equals("Excluir"))
            {
                long id = Util.Geral.ObterDataKeyValDoGrid<long>(grid, e, 0);

                try
                {
                    //AssociadoPJFacade.Instance.Excluir(id);
                    this.carregar();
                }
                catch
                {
                    Util.Geral.Alerta(null, this, "_err", "Não foi possível remover a operadora. Talvez ele esteja em uso.");
                }
            }
            else if (e.CommandName.Equals("ContratosAdm"))
            {
                long id = Util.Geral.ObterDataKeyValDoGrid<long>(grid, e, 0);
                txtContratoAdmOperadoraId.Text = id.ToString();
                this.carregarContratosAdm();
                this.exibeModalContratoAdm();
            }
            else if (e.CommandName.Equals("Planos"))
            {
                long id = Util.Geral.ObterDataKeyValDoGrid<long>(grid, e, 0);
                txtPlanoOperadoraId.Text = id.ToString();
                this.carregaPlanos();
                this.exibeModalPlanos();
            }
            else if (e.CommandName.Equals("Adicionais"))
            {
                long id = Util.Geral.ObterDataKeyValDoGrid<long>(grid, e, 0);
                txtAdicionalOperadoraId.Text = id.ToString();
                this.carregarAdicionais();
                this.exibeModalAdicional();
            }
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Util.Geral.grid_RowDataBound_Confirmacao(sender, e, 1, "Deseja excluir a operadora?");
        }

        //CONTRATO ADM
        /**************************************************************/

        /// <summary>
        /// Abre ou mantém o modal de contrato adm
        /// </summary>
        void exibeModalContratoAdm(string alert = null)
        {
            if (string.IsNullOrEmpty(alert))
                Util.Geral.JSScript(this, "showmodalContratoAdm();");
            else
                Util.Geral.JSScript(this, string.Concat("showmodalContratoAdm();alert('", alert, "');"));
        }

        protected void cmdContratoAdmNovo_Click(object sender, EventArgs e)
        {
            txtContratoAdmId.Text = "";
            cboContratoAdmEstipulante.SelectedIndex = 0;
            txtContratoAdmDescricao.Text = "";
            txtContratoAdmNumero.Text = "";
            txtContratoAdmCodFilial.Text = "";
            txtContratoAdmCodUnidade.Text = "";
            txtContratoAdmCodAdministradora.Text = "";
            txtContratoAdmSaude.Text = "";
            txtContratoAdmDental.Text = "";
            chkContratoAdmAtivo.Checked = true;
            this.setaVisibilidadePaineis(true, false);
            this.exibeModalContratoAdm();
        }

        void carregarContratosAdm()
        {
            var contratos = OperadoraFacade.Instancia.CarregarContratosAdm(Convert.ToInt64(txtContratoAdmOperadoraId.Text), Util.UsuarioLogado.IDContratante);
            GridContratoAdm.DataSource = contratos;
            GridContratoAdm.DataBind();
            GridContratoAdm.UseAccessibleHeader = true;

            if (GridContratoAdm.DataSource != null && GridContratoAdm.Rows.Count > 0) //if (contratos != null && contratos.Count > 0)
                GridContratoAdm.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void cmdContratoAdmCancelar_Click(object sender, EventArgs e)
        {
            this.setaVisibilidadePaineis(false, true);
            this.exibeModalContratoAdm();
        }

        protected void cmdContratoAdmSalvar_Click(object sender, EventArgs e)
        {
            #region validacoes 

            if(cboContratoAdmEstipulante.Items.Count <= 1 || cboContratoAdmEstipulante.SelectedIndex <= 0)
            {
                this.exibeModalContratoAdm("Estipulante não informado.");
                return;
            }
            if(txtContratoAdmDescricao.Text.Trim().Length <= 3)
            {
                this.exibeModalContratoAdm("Nome do contrato não informado.");
                return;
            }

            #endregion

            ContratoADM contrato = new ContratoADM();

            long id = Util.CTipos.CToLong(txtContratoAdmId.Text);
            if (id > 0)
            {
                contrato = OperadoraFacade.Instancia.CarregarContratoAdm(id, Util.UsuarioLogado.IDContratante);
                contrato.DataAlteracao = DateTime.Now;
                txtContratoAdmId.Text = "";
            }
            else
            {
                contrato.DataCadastro = DateTime.Now;
                contrato.Operadora = new Operadora(Util.CTipos.CTipo<long>(txtContratoAdmOperadoraId.Text));
                contrato.AssociadoPJ = new AssociadoPJ(Util.CTipos.CTipo<long>(cboContratoAdmEstipulante.SelectedValue));
            }

            contrato.Descricao = txtContratoAdmDescricao.Text;
            contrato.Numero = txtContratoAdmNumero.Text;
            contrato.CodigoFilial = txtContratoAdmCodFilial.Text;
            contrato.CodigoUnidade = txtContratoAdmCodUnidade.Text;
            contrato.CodigoAdministradora = txtContratoAdmCodAdministradora.Text;

            contrato.ContratoSaude = txtContratoAdmSaude.Text;
            contrato.ContratoDental = txtContratoAdmDental.Text;
            contrato.Ativo = chkContratoAdmAtivo.Checked;

            OperadoraFacade.Instancia.SalvarContratoAdm(contrato);

            this.setaVisibilidadePaineis(false, true);
            this.carregarContratosAdm();
            this.exibeModalContratoAdm("Contrato administrativo salvo com sucesso.");
        }

        protected void gridContratoAdm_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                long id = Util.Geral.ObterDataKeyValDoGrid<long>(GridContratoAdm, e, 0);

                ContratoADM c = OperadoraFacade.Instancia.CarregarContratoAdm(id, Util.UsuarioLogado.IDContratante);

                cboContratoAdmEstipulante.SelectedValue = c.AssociadoPJ.ID.ToString();
                txtContratoAdmDescricao.Text = c.Descricao;
                txtContratoAdmNumero.Text = c.Numero;
                txtContratoAdmCodFilial.Text = c.CodigoFilial;
                txtContratoAdmCodUnidade.Text = c.CodigoUnidade;
                txtContratoAdmCodAdministradora.Text = c.CodigoAdministradora;
                txtContratoAdmSaude.Text = c.ContratoSaude;
                txtContratoAdmDental.Text = c.ContratoDental;
                chkContratoAdmAtivo.Checked = c.Ativo;

                txtContratoAdmId.Text = c.ID.ToString();
                this.setaVisibilidadePaineis(true, false);

                this.exibeModalContratoAdm();
            }
            else if (e.CommandName.Equals("Excluir"))
            {
                long id = Util.Geral.ObterDataKeyValDoGrid<long>(GridContratoAdm, e, 0);

                try
                {
                    OperadoraFacade.Instancia.ExcluirContratoAdm(id);
                    this.carregarContratosAdm();
                    this.exibeModalContratoAdm("Contrado excluído com sucesso.");
                }
                catch
                {
                    this.exibeModalContratoAdm("Não foi possível excluir o contrato pois ele está sendo usado.");
                }
            }
        }

        protected void gridContratoAdm_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Util.Geral.grid_RowDataBound_Confirmacao(sender, e, 4, "Deseja excluir o contrato?");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Util.Geral.grid_AdicionaToolTip<LinkButton>(e, 4, 0, "Excluir");
                Util.Geral.grid_AdicionaToolTip<LinkButton>(e, 5, 0, "Alterar");
            }
        }

        void setaVisibilidadePaineis(bool detalhe, bool lista)
        {
            pnlContratosLista.Visible = lista;
            pnlContratosAdmDetalhe.Visible = detalhe;
        }

        #region ADICIONAIS 
        /**************************************************************/

        List<AdicionalFaixa> Faixas
        {
            get
            {
                return ViewState["_f"] as List<AdicionalFaixa>;
            }
            set
            {
                ViewState["_f"] = value;
            }
        }

        void exibirFaixas()
        {
            gridItemAdicional.DataSource = this.Faixas;
            gridItemAdicional.DataBind();

            gridItemAdicional.UseAccessibleHeader = true;
            if (gridItemAdicional.DataSource != null && gridItemAdicional.Rows.Count > 0) 
                gridItemAdicional.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        void carregarAdicionais()
        {
            var adicionais = OperadoraFacade.Instancia.CarregarAdicionais(Convert.ToInt64(txtAdicionalOperadoraId.Text), Util.UsuarioLogado.IDContratante);
            gridAdicionais.DataSource = adicionais;
            gridAdicionais.DataBind();
            gridAdicionais.UseAccessibleHeader = true;

            if (gridAdicionais.DataSource != null && gridAdicionais.Rows.Count > 0) //if (contratos != null && contratos.Count > 0)
                gridAdicionais.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        void exibeModalAdicional(string alert = null)
        {
            if (string.IsNullOrEmpty(alert))
                Util.Geral.JSScript(this, "showmodalAdicional();");
            else
                Util.Geral.JSScript(this, string.Concat("showmodalAdicional();alert('", alert, "');"));
        }
        void adicionalSetaVisibilidadePaineis(bool detalhe, bool lista)
        {
            pnlAdicionalLista.Visible = lista;
            pnlAdicionalDetalhe.Visible = detalhe;
        }

        protected void cmdAdicionalNovo_Click(object sender, EventArgs e)
        {
            txtAdicionalId.Text = "";
            txtAdicionalCodigo.Text = "";
            txtAdicionalDescricao.Text = "";
            chkAdicionalAtivo.Checked = true;
            chkAdicionalIndividual.Checked = false;

            this.Faixas = null;
            this.exibirFaixas();

            this.adicionalSetaVisibilidadePaineis(true, false);
            this.exibeModalAdicional();
        }

        protected void cmdAdicionalCancelar_Click(object sender, EventArgs e)
        {
            this.adicionalSetaVisibilidadePaineis(false, true);
            this.exibeModalAdicional();
        }

        protected void cmdAdicionalSalvar_Click(object sender, EventArgs e)
        {
            #region validacoes 

            if (txtAdicionalDescricao.Text.Trim().Length <= 3)
            {
                this.exibeModalAdicional("Descrição do adicional não informada.");
                return;
            }

            List<AdicionalFaixa> faixas = null;

            if (gridItemAdicional.Rows.Count > 0)
            {
                TextBox txtaux = null;
                DateTime dtaux = DateTime.MinValue;
                int auxIdadeInicio = 0, auxIdadeFim = 0;
                int? auxidade = null; decimal? auxvalor = null;

                faixas = this.Faixas;

                for (int i = 0; i < gridItemAdicional.Rows.Count; i++)
                {
                    txtaux = (TextBox)gridItemAdicional.Rows[i].Cells[0].FindControl("txtAdicionalItemVigencia");
                    dtaux = Util.CTipos.CStringToDateTime(txtaux.Text);
                    if(dtaux == DateTime.MinValue)
                    {
                        Util.Geral.Alerta(this, "Uma ou mais faixas do adicional estão sem data de vigência.");
                        return;
                    }

                    txtaux = (TextBox)gridItemAdicional.Rows[i].Cells[1].FindControl("txtAdicionalItemIdadeInicio");
                    auxidade = Util.CTipos.CToIntNullable(txtaux.Text);
                    if(auxidade == null)
                    {
                        Util.Geral.Alerta(this, "Uma ou mais faixas do adicional estão sem idade de início.");
                        return;
                    }
                    auxIdadeInicio = auxidade.Value;

                    txtaux = (TextBox)gridItemAdicional.Rows[i].Cells[2].FindControl("txtAdicionalItemIdadeFim");
                    auxidade = Util.CTipos.CToIntNullable(txtaux.Text);
                    if (auxidade == null)
                    {
                        Util.Geral.Alerta(this, "Uma ou mais faixas do adicional estão sem idade final.");
                        return;
                    }
                    auxIdadeFim = auxidade.Value;

                    if(auxIdadeFim < auxIdadeInicio)
                    {
                        Util.Geral.Alerta(this, "A idade final não pode ser menor que a idade inicial.\nVerifique as faixas do adicional.");
                        return;
                    }

                    txtaux = (TextBox)gridItemAdicional.Rows[i].Cells[3].FindControl("txtAdicionalItemValor");
                    auxvalor = Util.CTipos.CToDecimalNullable(txtaux.Text);
                    if (auxvalor == null)
                    {
                        Util.Geral.Alerta(this, "Uma ou mais faixas do adicional estão com valor inválido.");
                        return;
                    }

                    faixas[i].IdadeFim = auxIdadeFim;
                    faixas[i].IdadeInicio = auxIdadeInicio;
                    faixas[i].Valor = auxvalor.Value;
                    faixas[i].Vigencia = dtaux;
                }
            }

            #endregion

            Adicional adicional = new Adicional();

            long id = Util.CTipos.CToLong(txtAdicionalId.Text);
            if (id > 0)
            {
                adicional = OperadoraFacade.Instancia.CarregarAdicional(id, Util.UsuarioLogado.IDContratante);
                adicional.DataAlteracao = DateTime.Now;
                txtAdicionalId.Text = "";
            }
            else
            {
                adicional.DataCadastro = DateTime.Now;
                adicional.Operadora = new Operadora(Util.CTipos.CTipo<long>(txtAdicionalOperadoraId.Text));
            }

            adicional.Descricao = txtAdicionalDescricao.Text;
            adicional.Codigo = txtAdicionalCodigo.Text;
            adicional.Ativo = chkAdicionalAtivo.Checked;
            adicional.ParaTodaProposta = !chkAdicionalIndividual.Checked;

            OperadoraFacade.Instancia.SalvarAdicional(adicional, faixas);

            this.adicionalSetaVisibilidadePaineis(false, true);
            this.carregarAdicionais();
            this.exibeModalAdicional("Adicional salvo com sucesso.");
        }

        protected void cmdAdicionalAddItem_Click(object sender, EventArgs e)
        {
            if (this.Faixas == null) this.Faixas = new List<AdicionalFaixa>();

            this.Faixas.Add(new AdicionalFaixa
                {
                    Valor = 0,
                    Vigencia = DateTime.Now
                }
            );

            this.exibirFaixas();
        }

        //Grids
        protected void gridAdicionais_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                long id = Util.Geral.ObterDataKeyValDoGrid<long>(gridAdicionais, e, 0);

                Adicional a = OperadoraFacade.Instancia.CarregarAdicional(id, Util.UsuarioLogado.IDContratante);

                txtAdicionalCodigo.Text = a.Codigo;
                txtAdicionalDescricao.Text = a.Descricao;
                chkAdicionalAtivo.Checked = a.Ativo;
                chkAdicionalIndividual.Checked = !a.ParaTodaProposta;

                //Faixas
                this.Faixas = OperadoraFacade.Instancia.CarregarAdicionailFaixas(a.ID, Util.UsuarioLogado.IDContratante);
                this.exibirFaixas();

                txtAdicionalId.Text = a.ID.ToString();
                this.adicionalSetaVisibilidadePaineis(true, false);

                //this.exibeModalContratoAdm();
            }
            else if (e.CommandName.Equals("Excluir"))
            {
                long id = Util.Geral.ObterDataKeyValDoGrid<long>(gridAdicionais, e, 0);

                try
                {
                    OperadoraFacade.Instancia.ExcluirAdicional(id);
                    this.carregarAdicionais();
                    this.exibeModalAdicional("Adicional excluído com sucesso.");
                }
                catch
                {
                    this.exibeModalAdicional("Não foi possível excluir o adicional.\nVerifique se ele possui itens vigentes e exclua-os.");
                }
            }
        }
        protected void gridAdicionais_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Util.Geral.grid_RowDataBound_Confirmacao(sender, e, 2, "Deseja excluir o adicional?");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Util.Geral.grid_AdicionaToolTip<LinkButton>(e, 2, 0, "Excluir");
                Util.Geral.grid_AdicionaToolTip<LinkButton>(e, 3, 0, "Alterar");
            }
        }

        protected void gridItemAdicional_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                long id = Util.Geral.ObterDataKeyValDoGrid<long>(gridAdicionais, e, 0);

                //Adicional a = OperadoraFacade.Instancia.CarregarAdicional(id, Util.UsuarioLogado.IDContratante);

                //txtAdicionalCodigo.Text = a.Codigo;
                //txtAdicionalDescricao.Text = a.Descricao;
                //chkAdicionalAtivo.Checked = a.Ativo;
                //chkAdicionalIndividual.Checked = !a.ParaTodaProposta;

                //txtAdicionalId.Text = a.ID.ToString();
                //this.adicionalSetaVisibilidadePaineis(true, false);
            }
            else if (e.CommandName.Equals("Excluir"))
            {
                try
                {
                    int index = Util.CTipos.CTipo<int>(e.CommandArgument);

                    AdicionalFaixa faixa = this.Faixas[index]; //gridItemAdicional.Rows[index].DataItem as AdicionalFaixa;

                    if(faixa.TemId)
                    {
                        OperadoraFacade.Instancia.ExcluirAdicionalFaixa(faixa.ID);
                    }

                    this.Faixas.RemoveAt(index);
                    this.exibirFaixas();
                }
                catch
                {
                    this.exibeModalAdicional("Não foi possível excluir a faixa adicional.");
                }
            }
        }
        protected void gridItemAdicional_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                Util.Geral.grid_AdicionaToolTip<LinkButton>(e, 4, 0, "Excluir");
                Util.Geral.grid_RowDataBound_Confirmacao(sender, e, 4, "Deseja excluir a faixa de adicional?");

                TextBox txt = (TextBox)e.Row.Cells[3].FindControl("txtAdicionalItemValor");
                txt.Attributes.Add("onKeyUp", "mascara('" + txt.ClientID + "')");
            }
        }

        #endregion

        //PLANOS
        /**************************************************************/

        void exibeModalPlanos(string alert = null)
        {
            if (string.IsNullOrEmpty(alert))
                Util.Geral.JSScript(this, "showmodalPlano();");
            else
                Util.Geral.JSScript(this, string.Concat("showmodalPlano();alert('", alert, "');"));
        }
        void planoSetaVisibilidadePaineis(bool detalhe, bool lista)
        {
            pnlPlanoLista.Visible = lista;
            pnlPlanoDetalhe.Visible = detalhe;
        }
        void carregaPlanos()
        {
        }

        protected void cmdPlanoCancelar_Click(object sender, EventArgs e)
        {
            this.planoSetaVisibilidadePaineis(false, true);
        }

        protected void cmdPlanoSalvar_Click(object sender, EventArgs e)
        {
            this.planoSetaVisibilidadePaineis(false, true);
        }

        protected void cmdPlanoNovo_Click(object sender, EventArgs e)
        {
            txtPlanoId.Text = "";
            this.planoSetaVisibilidadePaineis(true, false);
        }

        //Grids
        protected void gridPlano_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //if (e.CommandName.Equals("Editar"))
            //{
            //    long id = Util.Geral.ObterDataKeyValDoGrid<long>(GridContratoAdm, e, 0);

            //    ContratoADM c = OperadoraFacade.Instancia.CarregarContratoAdm(id, Util.UsuarioLogado.IDContratante);

            //    cboContratoAdmEstipulante.SelectedValue = c.AssociadoPJ.ID.ToString();
            //    txtContratoAdmDescricao.Text = c.Descricao;
            //    txtContratoAdmNumero.Text = c.Numero;
            //    txtContratoAdmCodFilial.Text = c.CodigoFilial;
            //    txtContratoAdmCodUnidade.Text = c.CodigoUnidade;
            //    txtContratoAdmCodAdministradora.Text = c.CodigoAdministradora;
            //    txtContratoAdmSaude.Text = c.ContratoSaude;
            //    txtContratoAdmDental.Text = c.ContratoDental;
            //    chkContratoAdmAtivo.Checked = c.Ativo;

            //    txtContratoAdmId.Text = c.ID.ToString();
            //    this.setaVisibilidadePaineis(true, false);

            //    this.exibeModalContratoAdm();
            //}
            //else if (e.CommandName.Equals("Excluir"))
            //{
            //    long id = Util.Geral.ObterDataKeyValDoGrid<long>(GridContratoAdm, e, 0);

            //    try
            //    {
            //        OperadoraFacade.Instancia.ExcluirContratoAdm(id);
            //        this.carregarContratosAdm();
            //        this.exibeModalContratoAdm("Contrado excluído com sucesso.");
            //    }
            //    catch
            //    {
            //        this.exibeModalContratoAdm("Não foi possível excluir o contrato pois ele está sendo usado.");
            //    }
            //}
        }
        protected void gridPlano_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Util.Geral.grid_RowDataBound_Confirmacao(sender, e, 4, "Deseja excluir o contrato?");

            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    Util.Geral.grid_AdicionaToolTip<LinkButton>(e, 4, 0, "Excluir");
            //    Util.Geral.grid_AdicionaToolTip<LinkButton>(e, 5, 0, "Alterar");
            //}
        }


    }
}