using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FYP.Project
{
    public partial class AddEmployee : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) //IsPostBack = false
            {
                //do something
            }
        }

        protected void btnCreate_Click(object sender, EventArgs e)
        {
            //database
            //check email exist or not
            string inputEmail = txtEmail.Text.ToString();
            bool emailExist = false;
            string message = "Account has been successfully created.";

            SqlConnection con = new SqlConnection();
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            con = new SqlConnection(strCon);
            con.Open();

            string strSqlQuery = "Select * from Staff Where Email = '" + inputEmail +"'";
            SqlCommand cmdSelect = new SqlCommand(strSqlQuery, con);
            SqlDataReader rd = cmdSelect.ExecuteReader();

            if (rd.HasRows)
                emailExist = true;
            else
                emailExist = false;
            con.Close();         

            if (emailExist == true) //input email is exists in database
            {
                message = "Email has been registered. Please enter another email.";
                ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');", true);
                txtEmail.Style.Add("border-color", "red");
            }
            else //input email is not exists in database
            {
                String pw = Membership.GeneratePassword(6, 2);
                string role = ddlRole.SelectedValue.ToString();
                string databaseMsg = "";
                databaseMsg = insertDatabase(pw);
                if (databaseMsg == "") //end, all good condition
                {
                    email(inputEmail, pw, role);
                    txtEmail.Text = "";
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');", true);
                }
                else //insert database have problem
                {
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + databaseMsg + "');", true);
                }                
            }

        }

        private string insertDatabase(String password)
        {
            byte[] bytes = null;
            string abc = "~/Image/defaultProfileImg.png";
            byte[] imageBytes = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(abc));
            bytes = imageBytes;

            string outputMsg = "";
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Staff_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "INSERT");
                    cmd.Parameters.AddWithValue("@Role", ddlRole.SelectedValue.ToString());
                    cmd.Parameters.AddWithValue("@Email", txtEmail.Text.ToString());
                    cmd.Parameters.AddWithValue("@Password", password);
                    cmd.Parameters.AddWithValue("@Photo", bytes);
                    cmd.Parameters.AddWithValue("@Name", "-");
                    cmd.Parameters.AddWithValue("@RenewPassword", "yes");
                    cmd.Connection = con;
                    con.Open();
                    int a = cmd.ExecuteNonQuery();
                    if (a == 0)
                    {
                        outputMsg = "Upload Failed. Please Try Again.";
                    }
                    con.Close();
                }
            }
            return outputMsg;
        }
        private void email(String inputEmail, String password, String role)
        {
            //SmtpFYP
            //afouvomjvphfcksn

            //fyptarc99@gmail.com
            //Abc12345!

            string to = "kuanliang176@gmail.com";//To address    //temp
            string from = "fyptarc99@gmail.com"; //From address  
            MailMessage message = new MailMessage(from, to);

            string mailbody = "Your account(email): " + inputEmail + "<br>" + "Your temporary password: " + password + "<br>" + "Your will be a &quot;" + role + "&quot; in this system.";     //temp
            message.Subject = "Your New Account";
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