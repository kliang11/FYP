using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace FYP.Project
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) //IsPostBack = false
            {
                //do something
            }
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //for focus the textbox that not fill up
            bool bolFilled = false;
            if (txtEmail.Text == "")
                txtEmail.Focus();
            else if (txtPassword.Text == "")
                txtPassword.Focus();
            else if (txtEmail.Text != "" && txtPassword.Text != "")
                bolFilled = true;


            //search database
            //do something


            //for reset password purpose in next page
            if (lblStoreResetEmail.Text != "" && lblStoreResetEmail.Text == txtEmail.Text) //may change by using database check "true" then do something
            {
                //do something 
                //maybe need add a field in database as ResetPassword = true/false
                //database show true then display reset password screen
                //this for avoid the user close the webpage and relogin again with random password
                //also good for, when admin create account for new staff

                //set true to database
                //then when navigate to next page can show reset password page
            }

            //for wrong email or password   //temp
            if (txtEmail.Text != "Ali" && txtPassword.Text != "123" && bolFilled == true)
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Sorry, you have entered an invalid email address or password.";
                txtEmail.Text = "";
                txtPassword.Text = "";
            }
            else  //successful login
            {
                Session["userEmail"] = txtEmail.Text.ToString();
                //Session["usertype"] = usertype;
                if (chkbxRememberMe.Checked)
                {
                    //do something
                    //cookies???
                }

                //Response.Redirect("home.aspx");
            }

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            lblStoreResetEmail.Text = txtEmailPopUp.Text;
            String pw = Membership.GeneratePassword(6, 2);
            email(pw);
        }

        private void email(String password)
        {
            //SmtpFYP
            //afouvomjvphfcksn

            //fyptarc99@gmail.com
            //Abc12345!

            string to = "kuanliang176@gmail.com";//To address    //temp
            string from = "fyptarc99@gmail.com"; //From address  
            MailMessage message = new MailMessage(from, to);

            string mailbody = "Here is your temporary password: " + password;     //temp
            message.Subject = "Forgot Password";
            message.Body = mailbody;
            message.BodyEncoding = Encoding.UTF8;
            message.IsBodyHtml = true;
            SmtpClient client = new SmtpClient("smtp.gmail.com", 587); //Gmail smtp    
            System.Net.NetworkCredential basicCredential1 = new
            System.Net.NetworkCredential("fyptarc99@gmail.com", "afouvomjvphfcksn"); // put the email and password
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = true;
            client.UseDefaultCredentials = false;
            client.Credentials = basicCredential1;
            message.BodyEncoding = Encoding.Default;
            try
            {
                client.Send(message);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}