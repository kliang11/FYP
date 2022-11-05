using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FYP.Project
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            bool bolFilled = false;
            if (txtEmail.Text == "")
                txtEmail.Focus();
            else if (txtPassword.Text == "")
                txtPassword.Focus();
            else if (txtEmail.Text != "" && txtPassword.Text != "")
                bolFilled = true;


            //search database


            //temp
            if (txtEmail.Text != "Ali" && txtPassword.Text != "123" && bolFilled == true)
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Sorry, you have entered an invalid email address or password.";
                txtEmail.Text = "";
                txtPassword.Text = "";
            }

        }

    }
}