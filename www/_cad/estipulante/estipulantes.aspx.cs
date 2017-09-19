namespace cadben.www._cad.estipulante
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
    using cadben.Entidades.Enuns;

    public partial class estipulantes : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            txtTaxaValor.Attributes.Add("onKeyUp", "mascara('" + txtTaxaValor.ClientID + "')");
            if (!IsPostBack)
            {
                //this.carregar();
            }
        }

        void carregar()
        {
            var estipulantes = EstipulanteFacade.Instancia.CarregarTodos(Util.UsuarioLogado.IDContratante, txtNome.Text);
            grid.DataSource = estipulantes;
            grid.DataBind();
        }

        protected void cmdProcurar_Click(object sender, EventArgs e)
        {
            this.carregar();
        }

        protected void lnkNovo_Click(object sender, EventArgs e)
        {
            Response.Redirect("estipulante.aspx");
        }

        protected void grid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                long id = Util.Geral.ObterDataKeyValDoGrid<long>(grid, e, 0);

                string urisegura = Util.Geral.PreparaQueryStringSegura(new string[] { Util.Keys.IdKey }, new string[] { id.ToString() });

                Response.Redirect("estipulante.aspx?" + urisegura);
            }
            else if (e.CommandName.Equals("Taxas"))
            {
                //mpeTaxas.Show();
                //Util.Geral.JSScript(this, "document.getElementById('popSpan').click();");

                string id = Util.Geral.ObterDataKeyValDoGrid<string>(grid, e, 0);
                txtIdEstipulante.Text = id; //todo: denis, encriptar
                this.carregarTaxas();
                Util.Geral.JSScript(this, "showModalTaxas();");
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
                    Util.Geral.Alerta(null, this, "_err", "Não foi possível remover o estipulante. Talvez ele esteja em uso.");
                }
            }
        }

        protected void grid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //Util.Geral.grid_RowDataBound_Confirmacao(sender, e, 1, "Deseja excluir o estipulante?");
        }

        /*****************************************************************************/

        //void exibeModalTaxas__(bool fclick = false)
        //{
        //    Util.Geral.JSScript(this, "showModalTaxas()");
        //}

        void carregarTaxas()
        {
            GridTaxa.DataSource = EstipulanteFacade.Instancia.CarregarTaxas(
                Util.CTipos.CToLong(txtIdEstipulante.Text),
                Util.UsuarioLogado.IDContratante);
            GridTaxa.DataBind();
            GridTaxa.UseAccessibleHeader = true;

            if (GridTaxa.DataSource != null && GridTaxa.Rows.Count > 0)
                GridTaxa.HeaderRow.TableSection = TableRowSection.TableHeader;
        }

        protected void gridTaxa_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Editar"))
            {
                //long id = Util.Geral.ObterDataKeyValDoGrid<long>(GridTaxa, e, 0);
            }
            //else if (e.CommandName.Equals("Taxas"))
            //{
            //    Util.Geral.JSScript(this, "showModalTaxas()");
            //}
            else if (e.CommandName.Equals("Excluir"))
            {
                long id = Util.Geral.ObterDataKeyValDoGrid<long>(GridTaxa, e, 0);

                EstipulanteFacade.Instancia.ExcluirTaxa(id);

                //Util.Geral.JSScript(this, "showModalTaxas();alert('Taxa excluída com sucesso.');");
                Util.Geral.Alerta(this, "Taxa excluída com sucesso.");
                this.carregarTaxas();
            }
        }

        protected void gridTaxa_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Util.Geral.grid_RowDataBound_Confirmacao(sender, e, 3, "Deseja excluir a taxa?");

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Util.Geral.grid_AdicionaToolTip<LinkButton>(e, 3, 0, "Excluir");

                var taxa = e.Row.DataItem as EstipulanteTaxa;
                if (taxa != null)
                {
                    if (taxa.Tipo == TipoTaxa.PorBeneficiario)
                        e.Row.Cells[2].Text = "Por beneficiário";
                    else
                        e.Row.Cells[2].Text = "Por proposta";
                }
            }
        }

        protected void cmdSalvar_Click(object sender, EventArgs e)
        {
            #region validacoes

            long estipulanteId = Util.CTipos.CToLong(txtIdEstipulante.Text);

            DateTime vigencia = Util.CTipos.CStringToDateTime(txtVigencia.Text);
            if (vigencia == DateTime.MinValue)
            {
                //Util.Geral.JSScript(this, "showModalTaxas();alert('Data de vigência não informada ou inválida.');");
                Util.Geral.Alerta(this, "Data de vigência não informada ou inválida.");
                return;
            }

            if (cboTaxaTipo.SelectedIndex == 0)
            {
                //Util.Geral.JSScript(this, "showModalTaxas();alert('Tipo de taxa não informado.');");
                Util.Geral.Alerta(this, "Tipo de taxa não informado.");
                return;
            }

            bool vigenciaOK = EstipulanteFacade.Instancia.ValidarVigenciaTaxa(null, estipulanteId, vigencia);
            if (!vigenciaOK)
            {
                //Util.Geral.JSScript(this, "showModalTaxas();alert('Vigência inválida. Certifique-se de não haver outra taxa com mesma data.');");
                Util.Geral.Alerta(this, "Vigência inválida. Certifique-se de não haver outra taxa com mesma data.");
                return;
            }

            #endregion

            EstipulanteTaxa taxa = new EstipulanteTaxa();
            taxa.EstipulanteId = estipulanteId;
            taxa.Tipo = (TipoTaxa)Enum.Parse(typeof(TipoTaxa), cboTaxaTipo.SelectedValue);
            taxa.Valor = Util.CTipos.ToDecimal(txtTaxaValor.Text);
            taxa.Vigencia = vigencia;

            EstipulanteFacade.Instancia.SalvarTaxa(taxa);
            this.carregarTaxas();

            //Util.Geral.JSScript(this, "showModalTaxas();alert('Taxa salva com sucesso.');");
            Util.Geral.Alerta(this, "Taxa salva com sucesso.");
        }

        protected void cmd1_Click(object sender, EventArgs e)
        {
            txt1.Text = DateTime.Now.ToLongDateString();
        }

        protected void cmd2_Click(object sender, EventArgs e)
        {
            lit1.Text = txt1.Text;
        }
    }
}