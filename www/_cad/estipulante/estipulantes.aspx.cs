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
    using cadben.www.seguranca;

    public partial class estipulantes : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.carregar();
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
                Util.Geral.JSScript(this, "showModalTaxas()");
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
    }
}