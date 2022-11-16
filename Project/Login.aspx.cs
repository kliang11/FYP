﻿using System;
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
using System.Data.SqlClient;
using System.Configuration;

namespace FYP.Project
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) //IsPostBack = false
            {
                if (Request.Cookies["userEmail"] != null && Request.Cookies["userPassword"] != null)
                {
                    txtEmail.Text = Request.Cookies["userEmail"].Value;                   
                    txtPassword.Text = Request.Cookies["userPassword"].Value;
                    txtPassword.Attributes["type"] = "password";
                    chkbxRememberMe.Checked = true;
                    btnLogin_Click(btnLogin, EventArgs.Empty);
                }


                txtPassword.Attributes["type"] = "password";
            }
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            //for focus the textbox that not fill up
            bool IsWrong = true;

            if (txtEmail.Text == "")
            {
                txtEmail.Focus();
                return;
            }
            else if (txtPassword.Text == "")
            {
                txtPassword.Focus();
                return;
            }

            SqlConnection con = new SqlConnection();
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            con = new SqlConnection(strCon);
            con.Open();
            string strSqlQuery = "Select * from Staff Where Email = '" + txtEmail.Text.Trim() + "'" + " And Password = '" + txtPassword.Text + "'"; //temp id
            SqlCommand cmdSelect = new SqlCommand(strSqlQuery, con);
            SqlDataReader rd = cmdSelect.ExecuteReader();

            while (rd.Read())
            {
                IsWrong = false;
                Session["email"] = txtEmail.Text.ToString();
                Session["role"] = rd["Role"].ToString();
                Session["id"] = rd["Staff_ID"].ToString();
                
                if (chkbxRememberMe.Checked == true)
                {
                    Response.Cookies["userEmail"].Value = txtEmail.Text;
                    Response.Cookies["userPassword"].Value = txtPassword.Text;
                    Response.Cookies["userEmail"].Expires = DateTime.Now.AddDays(15);
                    Response.Cookies["userPassword"].Expires = DateTime.Now.AddDays(15);
                }
                else
                {
                    Response.Cookies["userEmail"].Expires = DateTime.Now.AddDays(-1);
                    Response.Cookies["userPassword"].Expires = DateTime.Now.AddDays(-1);
                }                
            }
            con.Close();

            //for wrong email or password   //temp
            if (IsWrong == true)
            {
                lblErrorMsg.Visible = true;
                lblErrorMsg.Text = "Sorry, you have entered an invalid email address or password.";
            }
            else //successful login
            {
                //Response.Redirect("home.aspx");  //temp
            }

        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            bool IsEmailExist = true;
            lblStoreResetEmail.Text = txtEmailPopUp.Text;

            SqlConnection con = new SqlConnection();
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            con = new SqlConnection(strCon);
            con.Open();
            string strSqlQuery = "Select * from Staff Where Email = '" + txtEmailPopUp.Text.Trim() + "'"; //temp id
            SqlCommand cmdSelect = new SqlCommand(strSqlQuery, con);
            SqlDataReader rd = cmdSelect.ExecuteReader();

            while (rd.Read())
            {
                IsEmailExist = false;
                lblStoreResetEmail.Text = "0";
            }
            con.Close();

            if(IsEmailExist == true)
            {
                lblStoreResetEmail.Text = "1";
                txtEmailPopUp.Focus();
            }
            else 
            {
                String pw = Membership.GeneratePassword(6, 2);
                email(pw);
                string message = "A temporary password has been sent to the entered email address.";
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');", true);
            }
            
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

            string mailbody = "Here is your temporary password: " + password + "<br>" + "Use this temporary password to log into your account.";   //temp
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