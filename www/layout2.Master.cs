﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace cadben.www
{
    public partial class layout2 : System.Web.UI.MasterPage
    {
        protected string baseThemePath
        {
            get
            {
                return Page.ResolveUrl(string.Concat("~/App_Themes/", "metronic1", "/"));
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            if (Session["logado"] == null) Response.Redirect("~/login2.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
        }
    }
}