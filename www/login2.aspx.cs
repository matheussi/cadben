namespace cadben.www
{
    using System;
    using System.Web;
    using System.Linq;
    using System.Web.UI;
    using System.Collections.Generic;
    using System.Web.UI.WebControls;

    public partial class login2 : System.Web.UI.Page
    {
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
        }
    }
}