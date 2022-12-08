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
    public partial class EmployeeList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["email"] != null)
                {
                    if (Session["resetPW"].ToString() == "yes")
                    {
                        Response.Redirect("~/Project/ChangePassword.aspx");
                    }
                    if (Session["role"].ToString() != "Normal Staff")
                    {
                        if (Session["role"].ToString() == "Admin")
                        {
                            this.gvList.Columns[5].Visible = true;
                            this.gvList.Columns[6].Visible = false;
                        }
                        else if (Session["role"].ToString() == "HR Staff")
                        {
                            this.gvList.Columns[5].Visible = false;
                            this.gvList.Columns[6].Visible = true;
                        }
                        this.BindGrid();
                    }
                    else
                    {
                        Response.Redirect(string.Format("~/Project/403error.html"));
                    }
                }
                else
                {
                    Response.Redirect("~/Project/Login.aspx?ReturnUrl=%2fEmployeeList.aspx");
                }
            }
        }
        protected void gvList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label a = (Label)gvList.SelectedRow.FindControl("lblStaffID");
            string id = a.Text.ToString();
            Response.Redirect(string.Format("~/Project/UserProfile.aspx?id={0}", id));
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != gvList.EditIndex)
            {
                (e.Row.Cells[5].Controls[0] as ImageButton).Attributes["onclick"] = "if(!confirm('Are you sure you want to delete?')) return false;";
            }
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = gvList.Rows[e.RowIndex];
            string staff_id = gvList.DataKeys[e.RowIndex]["Staff_ID"].ToString();
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Staff_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "DELETE");
                    cmd.Parameters.AddWithValue("@Staff_ID", staff_id);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            this.BindGrid();
        }

        private void BindGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Staff_CRUD"))
                {
                    string staff_id = Session["id"].ToString(); //this id is the id of the admin or hr
                    cmd.Parameters.AddWithValue("@Action", "SELECT");
                    cmd.Parameters.AddWithValue("@Staff_ID", staff_id);
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            gvList.DataSource = dt;
                            gvList.DataBind();
                            if (dt.Rows.Count > 0)
                            {
                                gvList.UseAccessibleHeader = true;
                                gvList.HeaderRow.TableSection = TableRowSection.TableHeader;
                                gvList.FooterRow.TableSection = TableRowSection.TableFooter;
                            }
                        }
                    }
                }
            }
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            rejectReasonPopup.Visible = true;
            resetDefault();
            this.BindGrid();
        }
        protected void btnRejectClose_Click(object sender, EventArgs e)
        {
            rejectReasonPopup.Visible = false;
            resetDefault();
            this.BindGrid();
        }

        private void resetDefault()
        {
            txtStaffEmailPopUp.Text = "";
            txtStaffEmailPopUp.Attributes["style"] = "borderColor: \"\"; display:none";
            lbl_EmailError.Attributes["style"] = "display:none;";
            ddlRole.SelectedValue = "Normal Staff";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            //database
            //check email exist or not
            string inputEmail = txtStaffEmailPopUp.Text.ToString();
            bool emailExist = false;
            string message = "Account has been successfully created.";

            SqlConnection con = new SqlConnection();
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            con = new SqlConnection(strCon);
            con.Open();

            string strSqlQuery = "Select * from Staff Where Email = '" + inputEmail + "'";
            SqlCommand cmdSelect = new SqlCommand(strSqlQuery, con);
            SqlDataReader rd = cmdSelect.ExecuteReader();

            if (rd.HasRows)
                emailExist = true;
            else
                emailExist = false;
            con.Close();

            if (emailExist == true) //input email is exists in database
            {
                //message = "Email has been registered. Please enter another email.";
                //ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');", true);
                txtStaffEmailPopUp.Attributes["style"] = "borderColor: \"\"; display:none;";
                rejectReasonPopup.Visible = true;
                lbl_EmailError.Text = "Email has been registered. Please enter another email.";
                txtStaffEmailPopUp.Attributes["style"] = "borderColor: \"\"; display:block";
                this.BindGrid();
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
                    resetDefault();
                    rejectReasonPopup.Visible = false;
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + message + "');", true);
                    this.BindGrid();
                }
                else //insert database have problem
                {
                    resetDefault();
                    ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + databaseMsg + "');", true);
                    this.BindGrid();
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
                    cmd.Parameters.AddWithValue("@Email", txtStaffEmailPopUp.Text.ToString());
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

            string to = txtStaffEmailPopUp.Text.Trim();//To address  
            string from = "fyptarc99@gmail.com"; //From address  
            MailMessage message = new MailMessage(from, to);

            string mailbody = "<div style=\"padding: 20px; background-color: rgb(255, 255, 255);\">" +
            " <div style=\"color: rgb(0, 0, 0); text-align: left;\">" +
            " <h1 style=\"margin: 1rem 0\">Your New Account </h1>" +
            " <p style=\"padding-bottom: 16px\">Please use the email address and temporary password stated below to sign in.</p>" +
            " <p style=\"padding-bottom: 2px\">Your account(email): <strong style=\"font-size: 130 %\">"+ inputEmail + "</strong></p>"+
            " <p style=\"padding-bottom: 2px\">Your temporary password: <strong style=\"font-size: 130 %\">"+ password + "</strong></p>"+
            " <p style =\"padding-bottom: 2px\">Your will be a &quot;<strong style=\"font-size: 130 %\">"+ role + "</strong>&quot; in this system.</p>"+
            " </div>" +
            " </div>";

            //string mailbody = "Your account(email): " + inputEmail + "<br>" + "Your temporary password: " + password + "<br>" + "Your will be a &quot;" + role + "&quot; in this system.";     //temp
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