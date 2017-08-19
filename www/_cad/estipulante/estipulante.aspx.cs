namespace cadben.www._cad.estipulante
{
    using System;
    using System.Web;
    using System.Linq;
    using System.Web.UI;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    using cadben.Entidades;
    using cadben.Facade;
    using System.Text;
    using cadben.www.Util;

    public partial class estipulante : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!string.IsNullOrEmpty(Request[Keys.rKey]))
                {
                    this.carregar();
                }
            }
        }

        void carregar()
        {
            string[,] ret = Util.Geral.ProcessaQueryStringSegura(Request[Keys.rKey]);

            var estipulante = EstipulanteFacade.Instancia.Carregar(Convert.ToInt64(ret[0, 1]), UsuarioLogado.IDContratante);

            txtNome.Text = estipulante.Nome;
            chkAtivo.Checked = estipulante.Ativo;

            litCadastro.Text = estipulante.DataCadastro.ToString("dd/MM/yyyy HH:mm");

            if (estipulante.DataAlteracao.HasValue)
            {
                litAlteracao.Text = estipulante.DataAlteracao.Value.ToString("dd/MM/yyyy HH:mm");
            }
        }

        protected void cmdVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("estipulantes.aspx");
        }

        protected void cmdSalvar_Click(object sender, EventArgs e)
        {
            #region validacoes 

            if (txtNome.Text.Trim().Length == 0)
            {
                Geral.Alerta(this, "Informe o nome do estipulante.");
                return;
            }

            #endregion

            Estipulante estipulante = new Estipulante();
            estipulante.ContratanteId = UsuarioLogado.IDContratante;

            if (!string.IsNullOrEmpty(Request[Keys.rKey]))
            {
                long id = Geral.ProcessaQueryStringSegura<long>(Request[Keys.rKey], Keys.IdKey);
                estipulante = EstipulanteFacade.Instancia.Carregar(id, UsuarioLogado.IDContratante);

                if (estipulante == null)
                {
                    throw new ApplicationException("Security exception.");
                }
            }

            estipulante.Ativo = chkAtivo.Checked;

            if (estipulante.TemId)
                estipulante.DataAlteracao = DateTime.Now;
            else
                estipulante.DataCadastro = DateTime.Now;

            estipulante.Nome = txtNome.Text;

            EstipulanteFacade.Instancia.Salvar(estipulante);
            Response.Redirect("estipulantes.aspx");
        }
    }
}