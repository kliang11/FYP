﻿using System;
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
                if (Session["email"] != null)
                {
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

                if(txtCurrentPw.Text != lblStorePassword.Text)
                {
                    lblErrorMsg.Text = "'Current Password' is not match.";
                    lblErrorMsg.Visible = true;
                    txtCurrentPw.Focus();
                    return;
                }
            }
            //if wrong then will return, all fine then go line 97

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
                            string redirect = Request.QueryString["redirect"];
                            if (redirect == "") 
                                url = "~/Project/attendance.aspx"; //happen when login then direct to this page
                            else
                            {
                                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                                url = ViewState["RefUrl"].ToString(); //happen when when user click the masterpage button
                            }
                        }
                        con.Close();
                    }
                }
            }
            catch
            {
                message = "Update unsuccessful. Please try again.";
                url = "~/Project/ChangePassword.aspx";
            }
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');", true);            
            Response.Redirect(url);
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            string url = "";
            string redirect = Request.QueryString["redirect"];
            if (redirect == "")
                url = "~/Project/attendance.aspx";
            else
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                url = ViewState["RefUrl"].ToString(); 
            }
            Response.Redirect(url);
        }
    }
}