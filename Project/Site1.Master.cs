using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FYP
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] != null)
            {
                profileImg.Src = Session["profileImg"].ToString();
                name.InnerText = Session["name"].ToString();
                role.InnerText = Session["role"].ToString();                                
            }            
        }
    }
}