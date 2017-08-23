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
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Util.Geral.grid_RowDataBound_Confirmacao(sender, e, 1, "Deseja excluir a operadora?");
        }

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
    }
}