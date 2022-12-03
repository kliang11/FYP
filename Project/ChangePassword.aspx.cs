using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FYP.Project
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) //IsPostBack = false
            {
                txtCurrentPw.Attributes["type"] = "password";
                txtNewPw.Attributes["type"] = "password";
                txtConfirmNewPw.Attributes["type"] = "password";

                //if from database check if reset Password = true, meaning the user forget their password & when they use the temp code to login,
                //then they will see different design for this page


                SqlConnection con = new SqlConnection();
                string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                con = new SqlConnection(strCon);
                con.Open();

                string strSqlQuery = "Select * from Staff Where Staff_ID = " + "6"; //temp id
                SqlCommand cmdSelect = new SqlCommand(strSqlQuery, con);
                SqlDataReader rd = cmdSelect.ExecuteReader();

                while (rd.Read())
                {
                    lblStorePassword.Text = rd["Password"].ToString();
                    lblNeedRenew.Text = rd["RenewPassword"].ToString();
                }

                if (lblNeedRenew.Text == "yes")
                {
                    lblTitle.Text = "Reset Password";
                    lblCurrentPassword.Visible = false;
                    txtCurrentPw.Visible = false;
                    lblStar.Visible = false;
                    lblEye.Visible = false;
                    btnCancel.Visible = false;
                }
            }
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            if (lblNeedRenew.Text == "yes")
            {
                if (txtNewPw.Text == "")
                {
                    txtNewPw.Focus();
                    return;
                }
                else if (txtConfirmNewPw.Text == "")
                {
                    txtConfirmNewPw.Focus();
                    return;
                }
            }
            else
            {
                if (txtCurrentPw.Text == "")
                {
                    txtCurrentPw.Focus();
                    return;
                }
                else if (txtNewPw.Text == "")
                {
                    txtNewPw.Focus();
                    return;
                }
                else if (txtConfirmNewPw.Text == "")
                {
                    txtConfirmNewPw.Focus();
                    return;
                }

                if(txtCurrentPw.Text != lblStorePassword.Text)
                {
                    lblErrorMsg.Text = "'Current Password' is not match.";
                    lblErrorMsg.Visible = true;
                    txtCurrentPw.Focus();
                    return;
                }
            }

            if (txtNewPw.Text != txtConfirmNewPw.Text)
            {
                lblErrorMsg.Text = "'New Password' and 'Confirm New Password' is not match.";
                lblErrorMsg.Visible = true;
                return;
            }
            else 
            {
                if(txtNewPw.Text == lblStorePassword.Text && txtConfirmNewPw.Text == lblStorePassword.Text)
                {
                    lblErrorMsg.Text = "New Password cannot be match as old password.";
                    lblErrorMsg.Visible = true;
                    return;
                }
            }
            lblErrorMsg.Visible = false;

            string message = "Update successful.";
            string url = "";
            try
            {
                string s = "6"; //temp                
                int id = Int16.Parse(s);
                //database
                string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    string sql = "UPDATE Staff SET Password = @Password, RenewPassword = @RenewPassword  WHERE Staff_ID = @Staff_ID";
                    using (SqlCommand cmd = new SqlCommand(sql, con))
                    {
                        cmd.Parameters.AddWithValue("@Password", txtConfirmNewPw.Text);
                        cmd.Parameters.AddWithValue("@RenewPassword", "no");
                        cmd.Parameters.AddWithValue("@Staff_ID", id);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            catch
            {
                message = "Update unsuccessful. Please try again.";
                url = "ChangePassword.aspx";
            }

            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');", true);
            //back to home page  //temp
            //Response.Redirect(url);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            //back to home page  //temp
            //Response.Redirect(".aspx");
        }
    }
}