namespace cadben.www
{
    using System;
    using System.Web;
    using System.Data;
    using System.Linq;
    using System.Web.UI;
    using System.Web.Security;
    using System.Data.OleDb;
    using System.Web.UI.WebControls;
    using System.Collections.Generic;

    //using cadben.Entidades;

    using cadben.Entity;
    using cadben.Facade;
    using Ent = cadben.Entidades;
    //using LC.SmartTools.Lead.www.SiteUtil.Seguranca;
    using System.Configuration;
    using System.Globalization;
    //using LC.Web.PadraoSeguros.Facade;
    using System.Net.Mail;
    //using LC.Framework.Phantom;
    using System.Text;

    public partial class login2 : System.Web.UI.Page
    {
        /// <summary>
        /// Retorna o alias da url
        /// </summary>
        string alias
        {
            get
            {
                return string.Empty;
            }
        }

        protected string baseThemePath
        {
            get
            {
                //return Page.ResolveClientUrl(string.Concat("~/App_Themes/", this.Theme, "/"));
                //return "http://localhost:10657/App_Themes/metronic1/";
                return @"~/App_Themes/metronic1/";
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session.Remove("logado");
                FormsAuthentication.SignOut();
            }
        }

        protected void cmdEntrar_Click(object sender, EventArgs e)
        {

#if DEBUG
            string[] arr = new string[] { "admin", "admin" };
            Util.UsuarioLogado.IDContratante = 1;

            //string erro = "";
            //Util.Geral.Mail.Enviar("assunto", "corpo", "denis.goncalves@wedigi.com.br", true, out erro);
#else
            string[] arr = new string[] { txtLogin.Value, txtSenha.Value };
#endif
            Ent.Usuario usuario = UsuarioFacade.Instancia.LogOn(arr[0], arr[1], this.alias);

            if (usuario != null)
            {
                Session["logado"] = 1;
                Util.UsuarioLogado.ID = usuario.ID.ToString();
                Util.UsuarioLogado.Nome = usuario.Nome;

                switch (usuario.Tipo)
                {
                    case Ent.Enuns.TipoUsuario.Administrador:
                    {
                        Util.UsuarioLogado.TipoUsuario = Util.UsuarioLogado.Tipo.Administrador; break;
                    }
                    case Ent.Enuns.TipoUsuario.AdministradorContratante:
                    {
                        Util.UsuarioLogado.TipoUsuario = Util.UsuarioLogado.Tipo.AdministradorContratante;
                        //Util.UsuarioLogado.Nome = usuario.Unidade.Owner.Nome;
                        //Util.UsuarioLogado.IDUnidade = usuario.Unidade.ID;
                        //Util.UsuarioLogado.NomeUnidade = usuario.Unidade.Nome;
                        //Util.UsuarioLogado.EnderecoUnidade = string.Concat(usuario.Unidade.Endereco, ", ", usuario.Unidade.Numero, " - ", usuario.Unidade.Bairro, " - ", usuario.Unidade.Cidade, " - ", usuario.Unidade.UF);
                        //Util.UsuarioLogado.FoneUnidade = usuario.Unidade.Telefone;
                        //Util.UsuarioLogado.EmailUnidade = usuario.Unidade.Email;
                        break;
                    }
                    case Ent.Enuns.TipoUsuario.OperadorContratante:
                    {
                        Util.UsuarioLogado.TipoUsuario = Util.UsuarioLogado.Tipo.OperadorContratante; break;
                    }
                    default:
                    {
                        Util.UsuarioLogado.TipoUsuario = Util.UsuarioLogado.Tipo.Indefinido; break;
                    }
                }

                Response.Redirect("default2.aspx");
            }
            else
            {
                Util.Geral.Alerta(this, "Usuário ou senha incorreto(s).");
            }

            #region comentado

            //Requisicao<string[]> requisicao = new Requisicao<string[]>(arr);
            //RetornoComplex<Usuario, DateTime> retorno = UsuarioFacade.Instancia.LogOn(requisicao);

            //if (retorno == null)
            //    litErro.Text = "Senha ou login incorreto(s)";
            //else
            //{
            //    UsuarioAutenticado.UsuarioNome = retorno.Objeto1.Nome;
            //    UsuarioAutenticado.UsuarioId = retorno.Objeto1.ID;
            //    UsuarioAutenticado.UsuarioEmail = retorno.Objeto1.Email;
            //    UsuarioAutenticado.UsuarioTipo = (int)retorno.Objeto1.Tipo;

            //    if (retorno.Objeto1.UltimoAcesso.HasValue)
            //        UsuarioAutenticado.UsuarioDataUltimoAcesso = retorno.Objeto2.ToString("dd/MM/yyyy HH:mm");
            //    else
            //        UsuarioAutenticado.UsuarioDataUltimoAcesso = DateTime.Now.ToString("dd/MM/yyyy HH:mm");

            //    if (retorno.Objeto1.Tipo != Entity.Enuns.TipoUsuario.Master)
            //    {
            //        UsuarioAutenticado.ContratanteId = retorno.Objeto1.Contratante.ID;

            //        if (!string.IsNullOrEmpty(retorno.Objeto1.Contratante.Logo))
            //            UsuarioAutenticado.ContratanteLogo = ConfigurationManager.AppSettings["logoVirtualBasePath"] + retorno.Objeto1.Contratante.Logo;
            //    }

            //    FormsAuthentication.SetAuthCookie(retorno.Objeto1.Email, false);

            //    Response.Redirect("default.aspx");
            //}
            #endregion
        }
    }
}