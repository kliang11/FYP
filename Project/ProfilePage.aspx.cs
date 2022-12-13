using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FYP.Project
{
    public partial class ProfilePage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                imgProfile.ImageUrl = "~/Image/defaultProfileImg.png";
                if (Session["email"] != null)
                {
                    if (Session["resetPW"].ToString() == "yes")
                    {
                        Response.Redirect("~/Project/ChangePassword.aspx");
                    }
                    if (Session["role"].ToString() != "Normal Staff")
                    {
                        ResetDisable();
                        string id = "";
                        id = Request.QueryString["id"];
                        string redirect = Request.QueryString["redirect"]; //from masterpage
                        if (redirect != null)
                            id = Session["id"].ToString();
                        if (id != null)
                            BindData(id);
                        if (txtName.Text != "-")
                            lblTitle.Text = txtName.Text;
                    }
                    else
                    {
                        Response.Redirect(string.Format("~/Project/403error.html"));
                    }
                }
                else
                {
                    Response.Redirect("~/Project/Login.aspx?ReturnUrl=%2fUserProfile.aspx");
                }                
            }
        }

        private void BindData(string id)
        {
            SqlConnection con = new SqlConnection();
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            con = new SqlConnection(strCon);
            con.Open();
            string strSqlQuery = "Select * from Staff Where Staff_ID = " + id;
            SqlCommand cmdSelect = new SqlCommand(strSqlQuery, con);
            SqlDataReader rd = cmdSelect.ExecuteReader();
            while (rd.Read())
            {
                imgProfile.ImageUrl = "data:Image/png;base64," + Convert.ToBase64String((byte[])rd["Photo"]);
                txtStaffID.Text = rd["Staff_ID"].ToString();
                txtName.Text = rd["Name"].ToString();
                txtEmail.Text = rd["Email"].ToString();
            }
            con.Close();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            imgProfile.ImageUrl = "deleted!)^%$(!";
        }

        protected void btnEditt_Click(object sender, EventArgs e)
        {
            SetEnable();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            string staff_id = txtStaffID.Text.ToString();
            string action = "";

            byte[] bytes = null;
            byte[] imageBytes = null;
            var azzz = FileUpload1.PostedFile;
            if (azzz == null)
            {
                if (imgProfile.ImageUrl.ToString().Contains("data:Image/png;base64,"))
                {
                    string aaqq = imgProfile.ImageUrl.ToString().Remove(0, 22);
                    bytes = System.Convert.FromBase64String(aaqq);
                    action = "UPDATE";
                }
                else
                {
                    bytes = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(imgProfile.ImageUrl));
                    action = "UPDATE";
                }

            }
            else
            {
                string defaultImageUrl = "data:Image/png;base64,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";

                if (azzz != null)
                {
                    using (BinaryReader br = new BinaryReader(FileUpload1.PostedFile.InputStream))
                    {
                        imageBytes = br.ReadBytes(FileUpload1.PostedFile.ContentLength);
                    }
                    if (imageBytes.Length == 0)
                    {
                        if (imgProfile.ImageUrl.ToString() != defaultImageUrl)
                        {
                            if (imgProfile.ImageUrl.ToString().Contains("data:Image/png;base64,"))
                            {
                                defaultImageUrl = imgProfile.ImageUrl.ToString().Remove(0, 22);
                                imageBytes = System.Convert.FromBase64String(defaultImageUrl);
                            }
                            else
                            {
                                string abc = "~/Image/defaultProfileImg.png";
                                imageBytes = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(abc));
                            }
                        }
                        else
                        {
                            string abc = "~/Image/defaultProfileImg.png";
                            imageBytes = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(abc));
                        }
                    }
                    bytes = imageBytes;
                    action = "UPDATE";
                }
                else if (imgProfile.ImageUrl.ToString() != defaultImageUrl)
                {
                    defaultImageUrl = imgProfile.ImageUrl.ToString().Remove(0, 22);
                    imageBytes = System.Convert.FromBase64String(defaultImageUrl);
                    bytes = imageBytes;
                    action = "UPDATE";
                }
                else
                {
                    action = "UPDATEWITHOUTPHOTO";
                }
            }
            string name = txtName.Text.ToString();
            string email = txtEmail.Text.ToString();
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Staff_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", action);
                    cmd.Parameters.AddWithValue("@Staff_ID", Int16.Parse(staff_id));
                    if (action == "UPDATE")
                        cmd.Parameters.AddWithValue("@Photo", bytes);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Connection = con;
                    con.Open();
                    int a = cmd.ExecuteNonQuery();
                    con.Close();
                    if (a == 0)
                    {
                        string script = "alert('Save Unsuccessful. Please Try Again');";
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
                    }
                    else
                    {
                        string script = "alert('Save Successfully.');";
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "Alert", script, true);
                        RegularExpressionValidator1.Enabled = false;
                        Session["profileImg"] = "data:Image/png;base64," + Convert.ToBase64String(bytes);
                        Session["name"] = name;
                    }
                }
            }
            ResetDisable();
            BindData(txtStaffID.Text);

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            string payrollListID, payperiod, date;
            payrollListID = Request.QueryString["payrollListID"];
            payperiod = Request.QueryString["payperiod"];
            date = Request.QueryString["date"];
            string redirect = Request.QueryString["redirect"]; //from masterpage
            if (redirect != null)
            {
                if (Session["role"].ToString() == "HR Staff")
                    Response.Redirect("~/Project/attendance.aspx");
                else if (Session["role"].ToString() == "Normal Staff")
                    Response.Redirect("~/Project/attendanceStaff.aspx");
            }
            if (payrollListID == null)
            {
                Response.Redirect("~/Project/EmployeeList.aspx");
            }
            else
            {
                Response.Redirect(string.Format("~/Project/PayrollDetailList.aspx?id={0}&payperiod={1}&date={2}", payrollListID, payperiod, date));
            }
        }

        private void ResetDisable()
        {
            txtName.Enabled = false;
            btnEditt.Visible = true;
            btnSave.Visible = false;
        }

        private void SetEnable()
        {
            txtName.Enabled = true;
            btnEditt.Visible = false;
            btnSave.Visible = true;
        }
    }
}