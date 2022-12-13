using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
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
                if (Session["email"] != null)
                {
                    if (Request.UrlReferrer != null)
                    {
                        try
                        {
                            ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                        }
                        catch
                        {
                            Response.Redirect(string.Format("~/Project/ContactAdmin.html"));
                        }
                    }
                    else
                    {
                        if (Session["role"].ToString() == "HR Staff")
                            ViewState["RefUrl"] = "~/Project/attendance.aspx";
                        else if (Session["role"].ToString() == "Normal Staff")
                            ViewState["RefUrl"] = "~/Project/attendanceStaff.aspx";
                    }

                    txtCurrentPw.Attributes["type"] = "password";
                    txtNewPw.Attributes["type"] = "password";
                    txtConfirmNewPw.Attributes["type"] = "password";

                    SqlConnection con = new SqlConnection();
                    string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    con = new SqlConnection(strCon);
                    con.Open();
                    string strSqlQuery = "Select * from Staff Where Staff_ID = " + Int16.Parse(Session["id"].ToString());
                    SqlCommand cmdSelect = new SqlCommand(strSqlQuery, con);
                    SqlDataReader rd = cmdSelect.ExecuteReader();
                    while (rd.Read())
                    {
                        lblStorePassword.Text = rd["Password"].ToString();
                    }
                    con.Close();
                    if (Session["role"].ToString() == "Admin")
                        lblStorePassword.Text = Application["password"].ToString();
                    if (Session["resetPW"].ToString() == "yes")
                    {
                        lblTitle.Text = "Reset Password";
                        lblCurrentPassword.Visible = false;
                        txtCurrentPw.Visible = false;
                        lblStar.Visible = false;
                        lblEye.Visible = false;
                        btnCancel.Visible = false;
                    }
                }
                else
                {
                    Response.Redirect("~/Project/Login.aspx?ReturnUrl=%2fChangePassword.aspx");
                }
            }
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            if (Session["resetPW"].ToString() == "yes")
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

                if (txtCurrentPw.Text != lblStorePassword.Text)
                {
                    lblErrorMsg.Text = "'Current Password' is not match.";
                    lblErrorMsg.Visible = true;
                    txtCurrentPw.Focus();
                    return;
                }
            }
            //if wrong then will return, all fine then go line below

            if (txtNewPw.Text != txtConfirmNewPw.Text)
            {
                txtConfirmNewPw.Focus();
                lblErrorMsg.Text = "'New Password' and 'Confirm New Password' is not match.";
                lblErrorMsg.Visible = true;
                return;
            }
            else
            {
                if (txtNewPw.Text == lblStorePassword.Text && txtConfirmNewPw.Text == lblStorePassword.Text)
                {
                    txtNewPw.Focus();
                    lblErrorMsg.Text = "New Password cannot be match as old password.";
                    lblErrorMsg.Visible = true;
                    return;
                }
            }
            Regex validateGuidRegex = new Regex("^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,20}$");
            if (txtConfirmNewPw.Text.Length < 8 || txtConfirmNewPw.Text.Length > 20)
            {
                txtNewPw.Focus();
                lblErrorMsg.Text = "Password length must within 8-20 characters.";
                lblErrorMsg.Visible = true;
                return;
            }
            else if (!validateGuidRegex.IsMatch(txtConfirmNewPw.Text))
            {
                txtNewPw.Focus();
                lblErrorMsg.Text = "Password need at least 1 upper & lower character, number and special character.";
                lblErrorMsg.Visible = true;
                return;
            }
            lblErrorMsg.Visible = false;

            string message = "Password has been updated.";
            string url = "";
            if (Session["role"].ToString() == "Admin")
            {
                Application["password"] = txtConfirmNewPw.Text;
                string redirect = Request.QueryString["redirect"];
                if (redirect == null && !ViewState["RefUrl"].ToString().Contains("ReturnUrl"))
                {                    
                   url = "EmployeeList.aspx";
                }
                else
                {
                    url = ViewState["RefUrl"].ToString(); //happen when when user click the masterpage button
                }
            }
            else
            {
                try
                {
                    string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    using (SqlConnection con = new SqlConnection(constr))
                    {
                        string sql = "UPDATE Staff SET Password = @Password, RenewPassword = @RenewPassword  WHERE Staff_ID = @Staff_ID";
                        using (SqlCommand cmd = new SqlCommand(sql, con))
                        {
                            cmd.Parameters.AddWithValue("@Password", txtConfirmNewPw.Text);
                            cmd.Parameters.AddWithValue("@RenewPassword", "no");
                            cmd.Parameters.AddWithValue("@Staff_ID", Int16.Parse(Session["id"].ToString()));
                            cmd.Connection = con;
                            con.Open();
                            int a = cmd.ExecuteNonQuery();
                            if (a > 0)
                            {
                                Session["resetPW"] = "no";
                                string redirect = Request.QueryString["redirect"];
                                if (redirect == null && !ViewState["RefUrl"].ToString().Contains("ReturnUrl"))
                                {
                                    if (Session["role"].ToString() == "HR Staff")
                                    {
                                        //url = "~/Project/attendance.aspx";
                                        url = "attendance.aspx";

                                    }
                                    else if (Session["role"].ToString() == "Normal Staff")
                                    {
                                        //url = "~/Project/attendanceStaff.aspx";
                                        url = "attendanceStaff.aspx";
                                    }
                                }
                                else
                                {
                                    url = ViewState["RefUrl"].ToString(); //happen when when user click the masterpage button
                                }
                            }
                            con.Close();
                        }
                    }
                }
                catch
                {
                    message = "Password update unsuccessful. Please try again.";
                    //url = "~/Project/ChangePassword.aspx";
                    url = "ChangePassword.aspx";
                }
            }

            //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');", false);


            var page = HttpContext.Current.CurrentHandler as Page;
            ScriptManager.RegisterStartupScript(page, page.GetType(), "alert", "alert('" + message + "');window.location ='" + url + "';", true);
            //if (txtCurrentPw.Visible == false)                                
            //    Response.Redirect(url);
            //else
            //    Response.Redirect(url);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string url = "";
            string redirect = Request.QueryString["redirect"];
            if (redirect == "")
            {
                if (Session["role"].ToString() == "HR Staff")
                    url = "~/Project/attendance.aspx";
                else if (Session["role"].ToString() == "Normal Staff")
                    url = "~/Project/attendanceStaff.aspx";
            }
            else
            {
                //ViewState["RefUrl"] = Request.UrlReferrer.ToString();asd
                url = ViewState["RefUrl"].ToString();
            }
            Response.Redirect(url);
        }
    }
}