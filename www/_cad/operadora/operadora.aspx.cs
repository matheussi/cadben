namespace cadben.www._cad.operadora
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

    public partial class operadora : System.Web.UI.Page
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

            var operadora = OperadoraFacade.Instancia.Carregar(Convert.ToInt64(ret[0, 1]), UsuarioLogado.IDContratante);

            txtCNPJ.Text = operadora.CNPJ;
            txtNome.Text = operadora.Nome;
            chkAtivo.Checked = !operadora.Inativa;

            litCadastro.Text = operadora.DataCadastro.ToString("dd/MM/yyyy HH:mm");

            if (operadora.DataAlteracao.HasValue)
            {
                litAlteracao.Text = operadora.DataAlteracao.Value.ToString("dd/MM/yyyy HH:mm");
            }
        }

        protected void cmdVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("operadoras.aspx");
        }

        protected void cmdSalvar_Click(object sender, EventArgs e)
        {
            #region validacoes 

            if (txtNome.Text.Trim().Length == 0)
            {
                Geral.Alerta(this, "Informe o nome da operadora.");
                return;
            }

            #endregion

            Operadora operadora = new Operadora();
            operadora.ContratanteId = UsuarioLogado.IDContratante;

            if (!string.IsNullOrEmpty(Request[Keys.rKey]))
            {
                long id = Geral.ProcessaQueryStringSegura<long>(Request[Keys.rKey], Keys.IdKey);
                operadora = OperadoraFacade.Instancia.Carregar(id, UsuarioLogado.IDContratante);

                if(operadora == null)
                {
                    throw new ApplicationException("Security exception.");
                }
            }

            operadora.Inativa = !chkAtivo.Checked;

            if (operadora.TemId)
                operadora.DataAlteracao = DateTime.Now;
            else
                operadora.DataCadastro = DateTime.Now;

            operadora.CNPJ = txtCNPJ.Text;
            operadora.Nome = txtNome.Text;

            OperadoraFacade.Instancia.Salvar(operadora);
            Response.Redirect("operadoras.aspx");
        }
    }
}