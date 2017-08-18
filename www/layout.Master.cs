﻿namespace MedProj.www
{
    using System;
    using System.Web;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.WebControls;
    using System.Collections.Generic;

    using CUtil = cadben.www.Util;

    public partial class layout : System.Web.UI.MasterPage
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            //if (UsuarioAutenticado.UsuarioId == 0) { Response.Redirect("~/Login.aspx"); }
            if (Session["logado"] == null) Response.Redirect("~/Login.aspx");
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            Page.Header.DataBind();

            this.montarMenu();
            this.exibirDados();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                litNomeUsuario.Text = cadben.www.Util.UsuarioLogado.Nome;
            }
        }

        void montarMenu()
        {
            if (CUtil.UsuarioLogado.TipoUsuario == CUtil.UsuarioLogado.Tipo.Administrador)
            {
                pnlMenu.Visible = true;
                pnlMenuAtendimento.Visible = true;
                liSelPrest.Visible = true;
                liConsultaAvancada.Visible = true;
            }
            //else if (CUtil.UsuarioLogado.TipoUsuario == CUtil.UsuarioLogado.Tipo.ContratoDePrestador)
            //{
            //    pnlMenu.Visible = false;
            //    liSelPrest.Visible = false;
            //    pnlMenuAtendimento.Visible = true;
            //    liConsultaAvancada.Visible = false;
            //}
            else
            {
                pnlMenu.Visible = true;
                pnlMenuAtendimento.Visible = true;
                liSelPrest.Visible = true;
                liConsultaAvancada.Visible = true;
            }
            //if (UsuarioAutenticado.UsuarioTipo != (int)TipoUsuario.Master)
            //{
            //    liClientes.Visible = false;
            //}

            //if (UsuarioAutenticado.UsuarioTipo == (int)TipoUsuario.Vendedor)
            //{
            //    liSecoes.Visible = false;
            //    liOrigens.Visible = false;
            //    liUsuarios.Visible = false;
            //    liServicos.Visible = false;
            //}
        }

        void exibirDados()
        {
            //if(!string.IsNullOrWhiteSpace(UsuarioAutenticado.UsuarioNome))
            //    litNomeUsuario.Text = UsuarioAutenticado.UsuarioNome.Split(' ')[0];
            //else
            //    litNomeUsuario.Text = "Usuário";

            //if (!string.IsNullOrEmpty(UsuarioAutenticado.ContratanteLogo))
            //    imglogo.Src = UsuarioAutenticado.ContratanteLogo;
        }
    }
}