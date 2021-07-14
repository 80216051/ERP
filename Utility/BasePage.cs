using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
namespace Utility
{
    public class BasePage : System.Web.UI.Page
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.Load += new EventHandler(Page_Load);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Session["admin"] == null)
                {
                    Response.Write("<script type='text/javascript'>parent.location.href='/SystemPage/Error.aspx?error=未登录或登录超时，请登录后操作。';</script>");
                }
            }
            catch (Exception)
            {
                Response.Write("<script type='text/javascript'>parent.location.href='/SystemPage/Error.aspx?error=未登录或登录超时，请登录后操作。';</script>");
            }
        }
    }
}
