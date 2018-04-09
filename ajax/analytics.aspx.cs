using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class pages_ajax_analytics : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Request.QueryString.AllKeys.Length == 1)
        {
            try
            {
                new Analytics().recordVisit(Request.UserHostName, Request.QueryString["page"]);
            }
            catch
            {
                Response.Write("500");
            }
        }
        else
        {
            Response.Write("403");
        }
    }
}