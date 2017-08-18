namespace cadben.www.master.contratante
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

    public partial class contratante : Page
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

            var contratante = ContratanteFacade.Instancia.Carregar(Convert.ToInt64(ret[0, 1]));

            txtAlias.Text = contratante.Alias;
            txtEmail.Text = contratante.Email;
            txtNome.Text = contratante.Nome;
            chkAtivo.Checked = contratante.Ativo;

            litCadastro.Text = contratante.DataCadastro.ToString("dd/MM/yyyy HH:mm");

            if (contratante.DataAlteracao.HasValue)
            {
                litAlteracao.Text = contratante.DataAlteracao.Value.ToString("dd/MM/yyyy HH:mm");
            }
        }

        protected void cmdVoltar_Click(object sender, EventArgs e)
        {
            Response.Redirect("contratantes.aspx");
        }

        protected void cmdSalvar_Click(object sender, EventArgs e)
        {
            #region validacoes 

            if (txtNome.Text.Trim().Length == 0)
            {
                Geral.Alerta(this, "Informe o nome do contratante.");
                return;
            }

            if (txtAlias.Text.Trim().Length == 0)
            {
                Geral.Alerta(this, "Informe o alias do contratante.");
                return;
            }

            #endregion

            Contratante c = new Contratante();

            if (!string.IsNullOrEmpty(Request[Keys.rKey]))
            {
                long id = Geral.ProcessaQueryStringSegura<long>(Request[Keys.rKey], Keys.IdKey);
                c = ContratanteFacade.Instancia.Carregar(id);
            }

            c.Alias = txtAlias.Text; //TODO: validar alias
            c.Ativo = chkAtivo.Checked;

            if (c.Id > 0)
                c.DataAlteracao = DateTime.Now;
            else
                c.DataCadastro = DateTime.Now;

            c.Email = txtEmail.Text;
            c.Nome  = txtNome.Text;

            ContratanteFacade.Instancia.Salvar(c);
            Response.Redirect("contratantes.aspx");
        }
    }
}