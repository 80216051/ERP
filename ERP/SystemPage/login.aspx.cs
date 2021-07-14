using BLL;
using BLL.Sys;
using Model;
using Model.Sys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Utility;

namespace ERP.SystemPage
{
    public partial class login : System.Web.UI.Page
    {
        SysAdminManager adminManager = SysAdminManager.GetInstrance();
        SystemManager sysManager = SystemManager.GetInstrance();
        protected void Page_Load(object sender, EventArgs e)
        {
       
        }
        protected void btnInput_Click(object sender, EventArgs e)
        {
            try
            {
                ClientScriptManager cs = Page.ClientScript;
                string password = this.txtPass.Text.Replace(" ", ""); ;
                if (password.Length > 0)
                {
                    Sys_Admin admin2 = new Sys_Admin(this.txtUserName.Text, password);
                    Sys_Admin adminResult2 = new Sys_Admin();
                    adminResult2 = adminManager.getSaltLogin(admin2);
                    string salt = adminResult2.Asalt;
                    //程序固定盐值+密码+数据库随机盐值
                    Sys_Admin admin = new Sys_Admin(this.txtUserName.Text, EncryptService.Getsha512("HK51*J#8_@ldj9#6%89k" + password + salt));
                    Sys_Admin adminResult = new Sys_Admin();
               
                }
                else
                {
                    cs.RegisterStartupScript(this.GetType(), "", "art.dialog.tips(\"用户名或密码错误\", 3, \"warning\");", true);
                }
            }
            catch (Exception ex)
            {
            }
        }
    }
}