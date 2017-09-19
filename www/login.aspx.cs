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

    public partial class login : System.Web.UI.Page
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

        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("login2.aspx");
            if (!IsPostBack)
            {
                Session.Remove("logado");
                //Util.UsuarioLogado.Encerrar();
                FormsAuthentication.SignOut();

                //decimal taxa = Convert.ToDecimal(System.Configuration.ConfigurationManager.AppSettings["taxaBoleto"], new System.Globalization.CultureInfo("pt-Br"));
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

        protected void cmdEnviarSenha_Click(object sender, EventArgs e)
        {
            //if (string.IsNullOrWhiteSpace(EmailSenha.Text))
            //{
            //    Util.Geral.Alerta(this, "Informe seu login.");
            //    return;
            //}

            //Usuario usuario = UsuarioFacade.Instance.Carregar(EmailSenha.Text);

            //if (usuario == null)
            //{
            //    Util.Geral.Alerta(this, "Login não encontrado.");
            //}
            //else
            //{
            //    System.Text.StringBuilder sb = new System.Text.StringBuilder();

            //    sb.Append("Olá!");
            //    sb.Append(Environment.NewLine);
            //    sb.Append(Environment.NewLine);
            //    sb.Append("Você está recebendo este e-mail por ter solicitado um lembrete de sua senha no sistema Clube Azul.");
            //    sb.Append(Environment.NewLine);
            //    sb.Append("Essa solicitação ocorreu em "); sb.Append(DateTime.Now.ToString("dd/MM/yyyy", new CultureInfo("pt-Br")));
            //    sb.Append(" às ");
            //    sb.Append(DateTime.Now.ToString("HH:mm", new CultureInfo("pt-Br"))); sb.Append(".");
            //    sb.Append(Environment.NewLine);
            //    sb.Append(Environment.NewLine);
            //    sb.Append("Seus dados de acesso são:");
            //    sb.Append(Environment.NewLine);
            //    sb.Append("Login: "); sb.Append(usuario.Login);
            //    sb.Append(Environment.NewLine);
            //    sb.Append("Senha: "); sb.Append(usuario.Senha);
            //    sb.Append(Environment.NewLine);
            //    sb.Append(Environment.NewLine);
            //    sb.Append(Environment.NewLine);
            //    sb.Append("Este é um e-mail automático, por favor não o responda.");


            //    string err = "";
            //    //int? porta = null;
            //    //bool seguro = false;


            //    //if (ConfigurationManager.AppSettings["appEmailSecure"].ToUpper() == "TRUE") seguro = true;
            //    //if (ConfigurationManager.AppSettings["appEmailSmtpPort"] != "") porta = Convert.ToInt32(ConfigurationManager.AppSettings["appEmailSmtpPort"]);

            //    bool ok = Util.Geral.Mail.Enviar("[CLUBE AZUL] - Senha", sb.ToString(), usuario.Login, false, out err);

            //    if (ok)
            //        Util.Geral.Alerta(this, "Lembrete de senha enviado.");
            //    else
            //    {
            //        Util.Geral.Alerta(this, "Não foi possível enviar o lembrete de senha.");
            //    }
            //}
        }
    }
}