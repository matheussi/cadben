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
            txtTabelaOver.Attributes.Add("onKeyUp", "mascara('" + txtTabelaOver.ClientID + "')");
            txtTabelaFixo.Attributes.Add("onKeyUp", "mascara('" + txtTabelaFixo.ClientID + "')");
            txtTabelaTarifa.Attributes.Add("onKeyUp", "mascara('" + txtTabelaTarifa.ClientID + "')");

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
                this.carregarContratosAdm_ParaPlanos();
                this.carregarPlanos();
                this.exibeModalPlanos();
            }
            else if (e.CommandName.Equals("Tabelas"))
            {
                long id = Util.Geral.ObterDataKeyValDoGrid<long>(grid, e, 0);
                txtTabelaOperadoraId.Text = id.ToString();
                this.carregarContratosAdm_ParaTabela();
                //this.carregarPlanos();
                this.exibeModalTabela();
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
            //this.exibeModalContratoAdm();
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
            //this.exibeModalContratoAdm();
        }

        protected void cmdContratoAdmSalvar_Click(object sender, EventArgs e)
        {
            #region validacoes 

            if(cboContratoAdmEstipulante.Items.Count <= 1 || cboContratoAdmEstipulante.SelectedIndex <= 0)
            {
                Util.Geral.Alerta(this, "Estipulante não informado."); //this.exibeModalContratoAdm("");
                return;
            }
            if(txtContratoAdmDescricao.Text.Trim().Length <= 2)
            {
                Util.Geral.Alerta(this, "Nome do contrato não informado."); //this.exibeModalContratoAdm("Nome do contrato não informado.");
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
            Util.Geral.Alerta(this, "Contrato administrativo salvo com sucesso."); //this.exibeModalContratoAdm("Contrato administrativo salvo com sucesso.");
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

                //this.exibeModalContratoAdm();
            }
            else if (e.CommandName.Equals("Excluir"))
            {
                long id = Util.Geral.ObterDataKeyValDoGrid<long>(GridContratoAdm, e, 0);

                try
                {
                    OperadoraFacade.Instancia.ExcluirContratoAdm(id);
                    this.carregarContratosAdm();
                    Util.Geral.Alerta(this, "Contrado excluído com sucesso."); //this.exibeModalContratoAdm("Contrado excluído com sucesso.");
                }
                catch
                {
                    Util.Geral.Alerta(this, "Não foi possível excluir o contrato pois ele está sendo usado."); //this.exibeModalContratoAdm("Não foi possível excluir o contrato pois ele está sendo usado.");
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

        void exibeModalAdicional()
        {
            //if (string.IsNullOrEmpty(alert))
                Util.Geral.JSScript(this, "showmodalAdicional();");
            //else
            //    Util.Geral.JSScript(this, string.Concat("showmodalAdicional();alert('", alert, "');"));
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
            //this.exibeModalAdicional();
        }

        protected void cmdAdicionalCancelar_Click(object sender, EventArgs e)
        {
            this.adicionalSetaVisibilidadePaineis(false, true);
            //this.exibeModalAdicional();
        }

        protected void cmdAdicionalSalvar_Click(object sender, EventArgs e)
        {
            #region validacoes 

            if (txtAdicionalDescricao.Text.Trim().Length <= 3)
            {
                Util.Geral.Alerta(this, "Descrição do adicional não informada.");
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
            Util.Geral.Alerta(this, "Adicional salvo com sucesso.");
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

                //this.exibeModalAdicional();
            }
            else if (e.CommandName.Equals("Excluir"))
            {
                long id = Util.Geral.ObterDataKeyValDoGrid<long>(gridAdicionais, e, 0);

                try
                {
                    OperadoraFacade.Instancia.ExcluirAdicional(id);
                    this.carregarAdicionais();
                    Util.Geral.Alerta(this, "Adicional excluído com sucesso.");
                }
                catch
                {
                    Util.Geral.Alerta(this, "Não foi possível excluir o adicional.\nVerifique se ele possui itens vigentes e exclua-os.");
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
                    Util.Geral.Alerta(this, "Não foi possível excluir a faixa adicional.");
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

        #region PLANOS 
        /**************************************************************/

        void carregarContratosAdm_ParaPlanos()
        {
            var contratos = OperadoraFacade.Instancia.CarregarContratosAdm(Convert.ToInt64(txtPlanoOperadoraId.Text), Util.UsuarioLogado.IDContratante);

            cboPlanoContratoAdm.Items.Clear();
            cboPlanoContratoAdm.Items.Add(new ListItem("selecione", "-1"));

            cboPlanoContratoAdmLista.Items.Clear();

            if (contratos != null && contratos.Count > 0)
            {
                foreach(var contrato in contratos)
                {
                    cboPlanoContratoAdm.Items.Add(new ListItem(contrato.Descricao, contrato.ID.ToString()));
                    cboPlanoContratoAdmLista.Items.Add(new ListItem(contrato.Descricao, contrato.ID.ToString()));
                }
            }
        }

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
        void carregarPlanos()
        {
            if(cboPlanoContratoAdmLista.Items.Count > 0)
            {
                gridPlano.DataSource = OperadoraFacade.Instancia.CarregarPlanos(
                    Util.CTipos.CTipo<long>(cboPlanoContratoAdmLista.SelectedValue), Util.UsuarioLogado.IDContratante);

                gridPlano.DataBind();
                gridPlano.UseAccessibleHeader = true;

                if (gridPlano.DataSource != null && gridPlano.Rows.Count > 0) 
                    gridPlano.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void cmdPlanoCancelar_Click(object sender, EventArgs e)
        {
            this.planoSetaVisibilidadePaineis(false, true);
        }

        protected void cmdPlanoSalvar_Click(object sender, EventArgs e)
        {
            #region valicacoes

            if(cboPlanoContratoAdm.SelectedIndex <= 0)
            {
                Util.Geral.Alerta(this, "Contrato administrativo não informada.");
                return;
            }

            if(txtPlanoDescicao.Text.Trim().Length <= 1)
            {
                Util.Geral.Alerta(this, "Descrição do plano não informada.");
                return;
            }

            #endregion

            Plano plano = new Plano();

            long id = Util.CTipos.CToLong(txtPlanoId.Text);
            if (id > 0)
            {
                plano = OperadoraFacade.Instancia.CarregarPlano(id, Util.UsuarioLogado.IDContratante);
                plano.ContratoAdm.ID = Util.CTipos.CToLong(cboPlanoContratoAdm.SelectedValue);

                txtPlanoId.Text = "";
            }

            plano.Data = DateTime.Now;
            plano.Ativo = chkPlanoAtivo.Checked;
            plano.ContratoAdm = new ContratoADM(Util.CTipos.CToLong(cboPlanoContratoAdm.SelectedValue));
            plano.Descricao = txtPlanoDescicao.Text;

            plano.QuartoComum = chkPlanoQuartoColetivo.Checked;
            plano.QuartoComumCodigo = txtPlanoColetivoCodigo.Text;
            plano.QuartoComumCodigoANS = txtPlanoColetivoCodigoAns.Text;
            plano.QuartoComumInicio = Util.CTipos.CStringToDateTimeG(txtPlanoColetivoInicio.Text);
            plano.QuartoComumSubplano = txtPlanoColetivoSubplano.Text;

            plano.QuartoParticular = chkPlanoQuartoParticular.Checked;
            plano.QuartoParticularCodigo = txtPlanoParticularCodigo.Text;
            plano.QuartoParticularCodigoANS = txtPlanoParticularCodigoAns.Text;
            plano.QuartoParticularInicio = Util.CTipos.CStringToDateTimeG(txtPlanoParticularInicio.Text);
            plano.QuartoParticularSubplano = txtPlanoParticularSubplano.Text;

            OperadoraFacade.Instancia.SalvarPlano(plano);

            this.carregarPlanos();
            this.planoSetaVisibilidadePaineis(false, true);
            Util.Geral.Alerta(this, "Plano salvo com sucesso.");
        }

        protected void cmdPlanoNovo_Click(object sender, EventArgs e)
        {
            txtPlanoId.Text = "";

            txtPlanoColetivoCodigo.Text = "";
            txtPlanoColetivoCodigoAns.Text = "";
            txtPlanoColetivoInicio.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtPlanoColetivoSubplano.Text = "";

            txtPlanoDescicao.Text = "";
            txtPlanoParticularCodigo.Text = "";
            txtPlanoParticularCodigoAns.Text = "";
            txtPlanoParticularInicio.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtPlanoParticularSubplano.Text = "";

            chkPlanoAtivo.Checked = true;
            chkPlanoQuartoColetivo.Checked = false;
            chkPlanoQuartoParticular.Checked = false;

            this.planoSetaVisibilidadePaineis(true, false);
        }

        //Grids
        protected void gridPlano_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                long id = Util.Geral.ObterDataKeyValDoGrid<long>(gridPlano, e, 0);

                Plano p = OperadoraFacade.Instancia.CarregarPlano(id, Util.UsuarioLogado.IDContratante);

                cboPlanoContratoAdm.SelectedValue = p.ContratoAdm.ID.ToString();
                chkPlanoAtivo.Checked = p.Ativo;
                txtPlanoDescicao.Text = p.Descricao;

                chkPlanoQuartoColetivo.Checked = p.QuartoComum;
                txtPlanoColetivoCodigo.Text = p.QuartoComumCodigo;
                txtPlanoColetivoCodigoAns.Text = p.QuartoComumCodigoANS;
                txtPlanoColetivoSubplano.Text = p.QuartoComumSubplano;

                if (p.QuartoComumInicio.HasValue)
                    txtPlanoColetivoInicio.Text = p.QuartoComumInicio.Value.ToString("dd/MM/yyyy");

                chkPlanoQuartoParticular.Checked = p.QuartoParticular;
                txtPlanoParticularCodigo.Text = p.QuartoParticularCodigo;
                txtPlanoParticularCodigoAns.Text = p.QuartoParticularCodigoANS;
                txtPlanoParticularSubplano.Text = p.QuartoParticularSubplano;

                if (p.QuartoParticularInicio.HasValue)
                    txtPlanoParticularInicio.Text = p.QuartoParticularInicio.Value.ToString("dd/MM/yyyy");

                txtPlanoId.Text = p.ID.ToString();
                this.planoSetaVisibilidadePaineis(true, false);
            }
            else if (e.CommandName.Equals("Excluir"))
            {
                long id = Util.Geral.ObterDataKeyValDoGrid<long>(gridPlano, e, 0);

                try
                {
                    OperadoraFacade.Instancia.ExcluirPlano(id);
                    this.carregarPlanos();
                    Util.Geral.Alerta(this, "Plano excluído com sucesso.");
                }
                catch
                {
                    this.exibeModalPlanos("Não foi possível excluir o plano pois ele está sendo usado.");
                }
            }
        }
        protected void gridPlano_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Util.Geral.grid_RowDataBound_Confirmacao(sender, e, 3, "Deseja excluir o plano?");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Util.Geral.grid_AdicionaToolTip<LinkButton>(e, 3, 0, "Excluir");
                Util.Geral.grid_AdicionaToolTip<LinkButton>(e, 4, 0, "Alterar");
            }
        }

        protected void cboPlanoContratoAdmLista_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.carregarPlanos();
        }
        #endregion

        //TABELA DE VALOR 
        /**************************************************************/

        void carregarContratosAdm_ParaTabela()
        {
            var contratos = OperadoraFacade.Instancia.CarregarContratosAdm(Convert.ToInt64(txtTabelaOperadoraId.Text), Util.UsuarioLogado.IDContratante);

            cboTabelaContratoAdm.Items.Clear();
            cboTabelaContratoAdm.Items.Add(new ListItem("selecione", "-1"));

            cboTabelaContratoAdmLista.Items.Clear();

            if (contratos != null && contratos.Count > 0)
            {
                foreach (var contrato in contratos)
                {
                    cboTabelaContratoAdm.Items.Add(new ListItem(contrato.Descricao, contrato.ID.ToString()));
                    cboTabelaContratoAdmLista.Items.Add(new ListItem(contrato.Descricao, contrato.ID.ToString()));
                }
            }
        }

        void exibeModalTabela(string alert = null)
        {
            if (string.IsNullOrEmpty(alert))
                Util.Geral.JSScript(this, "showmodalTabela();");
            else
                Util.Geral.JSScript(this, string.Concat("showmodalTabela();alert('", alert, "');"));
        }
        void tabelaSetaVisibilidadePaineis(bool detalhe, bool lista)
        {
            pnlTabelaLista.Visible = lista;
            pnlTabelaDetalhe.Visible = detalhe;
        }
        void carregarTabelas()
        {
            if (cboTabelaContratoAdmLista.Items.Count > 0)
            {
                //gridPlano.DataSource = OperadoraFacade.Instancia.CarregarPlanos(
                //    Util.CTipos.CTipo<long>(cboTabelaContratoAdmLista.SelectedValue), Util.UsuarioLogado.IDContratante);

                //gridPlano.DataBind();
                //gridPlano.UseAccessibleHeader = true;

                //if (gridPlano.DataSource != null && gridPlano.Rows.Count > 0)
                //    gridPlano.HeaderRow.TableSection = TableRowSection.TableHeader;
            }
        }

        protected void cmdTabelaCancelar_Click(object sender, EventArgs e)
        {
            this.tabelaSetaVisibilidadePaineis(false, true);
        }

        protected void cmdTabelaSalvar_Click(object sender, EventArgs e)
        {
            #region valicacoes

            if (cboTabelaContratoAdm.SelectedIndex <= 0)
            {
                Util.Geral.Alerta(this, "Contrato administrativo não informado.");
                return;
            }

            //if (txtPlanoDescicao.Text.Trim().Length <= 1)
            //{
            //    Util.Geral.Alerta(this, "Descrição do plano não informada.");
            //    return;
            //}

            #endregion

            //Plano plano = new Plano();

            //long id = Util.CTipos.CToLong(txtPlanoId.Text);
            //if (id > 0)
            //{
            //    plano = OperadoraFacade.Instancia.CarregarPlano(id, Util.UsuarioLogado.IDContratante);
            //    plano.ContratoAdm.ID = Util.CTipos.CToLong(cboPlanoContratoAdm.SelectedValue);

            //    txtPlanoId.Text = "";
            //}

            //plano.Data = DateTime.Now;
            //plano.Ativo = chkPlanoAtivo.Checked;
            //plano.ContratoAdm = new ContratoADM(Util.CTipos.CToLong(cboPlanoContratoAdm.SelectedValue));
            //plano.Descricao = txtPlanoDescicao.Text;

            //plano.QuartoComum = chkPlanoQuartoColetivo.Checked;
            //plano.QuartoComumCodigo = txtPlanoColetivoCodigo.Text;
            //plano.QuartoComumCodigoANS = txtPlanoColetivoCodigoAns.Text;
            //plano.QuartoComumInicio = Util.CTipos.CStringToDateTimeG(txtPlanoColetivoInicio.Text);
            //plano.QuartoComumSubplano = txtPlanoColetivoSubplano.Text;

            //plano.QuartoParticular = chkPlanoQuartoParticular.Checked;
            //plano.QuartoParticularCodigo = txtPlanoParticularCodigo.Text;
            //plano.QuartoParticularCodigoANS = txtPlanoParticularCodigoAns.Text;
            //plano.QuartoParticularInicio = Util.CTipos.CStringToDateTimeG(txtPlanoParticularInicio.Text);
            //plano.QuartoParticularSubplano = txtPlanoParticularSubplano.Text;

            //OperadoraFacade.Instancia.SalvarPlano(plano);

            this.carregarTabelas();
            this.planoSetaVisibilidadePaineis(false, true);
            Util.Geral.Alerta(this, "Plano salvo com sucesso.");
        }

        protected void cmdTabelaNova_Click(object sender, EventArgs e)
        {
            //txtPlanoId.Text = "";

            //txtPlanoColetivoCodigo.Text = "";
            //txtPlanoColetivoCodigoAns.Text = "";
            //txtPlanoColetivoInicio.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //txtPlanoColetivoSubplano.Text = "";

            //txtPlanoDescicao.Text = "";
            //txtPlanoParticularCodigo.Text = "";
            //txtPlanoParticularCodigoAns.Text = "";
            //txtPlanoParticularInicio.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //txtPlanoParticularSubplano.Text = "";

            //chkPlanoAtivo.Checked = true;
            //chkPlanoQuartoColetivo.Checked = false;
            //chkPlanoQuartoParticular.Checked = false;

            this.tabelaSetaVisibilidadePaineis(true, false);
        }

        //Grids
        protected void gridTabela_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                long id = Util.Geral.ObterDataKeyValDoGrid<long>(gridTabela, e, 0);

                //Plano p = OperadoraFacade.Instancia.CarregarPlano(id, Util.UsuarioLogado.IDContratante);

                //cboPlanoContratoAdm.SelectedValue = p.ContratoAdm.ID.ToString();
                //chkPlanoAtivo.Checked = p.Ativo;
                //txtPlanoDescicao.Text = p.Descricao;

                //chkPlanoQuartoColetivo.Checked = p.QuartoComum;
                //txtPlanoColetivoCodigo.Text = p.QuartoComumCodigo;
                //txtPlanoColetivoCodigoAns.Text = p.QuartoComumCodigoANS;
                //txtPlanoColetivoSubplano.Text = p.QuartoComumSubplano;

                //if (p.QuartoComumInicio.HasValue)
                //    txtPlanoColetivoInicio.Text = p.QuartoComumInicio.Value.ToString("dd/MM/yyyy");

                //chkPlanoQuartoParticular.Checked = p.QuartoParticular;
                //txtPlanoParticularCodigo.Text = p.QuartoParticularCodigo;
                //txtPlanoParticularCodigoAns.Text = p.QuartoParticularCodigoANS;
                //txtPlanoParticularSubplano.Text = p.QuartoParticularSubplano;

                //if (p.QuartoParticularInicio.HasValue)
                //    txtPlanoParticularInicio.Text = p.QuartoParticularInicio.Value.ToString("dd/MM/yyyy");

                //txtTabelaId.Text = p.ID.ToString();
                this.tabelaSetaVisibilidadePaineis(true, false);
            }
            else if (e.CommandName.Equals("Excluir"))
            {
                long id = Util.Geral.ObterDataKeyValDoGrid<long>(gridTabela, e, 0);

                try
                {
                    //OperadoraFacade.Instancia.ExcluirPlano(id);
                    //this.carregarPlanos();
                    Util.Geral.Alerta(this, "Tabela excluída com sucesso.");
                }
                catch
                {
                    this.exibeModalTabela("Não foi possível excluir a tabela pois ela está em uso.");
                }
            }
        }
        protected void gridTabela_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Util.Geral.grid_RowDataBound_Confirmacao(sender, e, 3, "Deseja excluir a tabela?");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Util.Geral.grid_AdicionaToolTip<LinkButton>(e, 3, 0, "Excluir");
                Util.Geral.grid_AdicionaToolTip<LinkButton>(e, 4, 0, "Alterar");
            }
        }

        protected void cboTabelaContratoAdmLista_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.carregarTabelas();
        }
    }
}