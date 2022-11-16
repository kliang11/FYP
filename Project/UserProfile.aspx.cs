using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Text.RegularExpressions;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Globalization;

namespace FYP.Project
{
    public partial class UserProfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) //IsPostBack = false
            {
                // return;
                ResetAllDefault();
                //load database and update to panel
                string id = "";
                id = Request.QueryString["id"];   //temp
                if (id != null)
                {
                    BindData(id);
                }
                else
                {
                    //do something
                }

                if (txtName.Text != "-")
                {
                    lblTitle.Text = txtName.Text;
                }



            }
        }

        private void BindData(String id)
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
                if (rd["Gender"].ToString() != "")
                    ddlGender.SelectedValue = rd["Gender"].ToString();
                txtNationality.Text = rd["Nationality"].ToString();
                txtIC.Text = rd["IcNo"].ToString();
                txtPhoneNo.Text = rd["PhoneNo"].ToString();
                txtAddress.Text = rd["Address"].ToString();
                if (rd["MaritalStatus"].ToString() != "")
                    ddlMaritalStatus.SelectedValue = rd["MaritalStatus"].ToString();
                if (rd["SpouseWorking"].ToString() != "")
                    ddlSpouseWork.SelectedValue = rd["SpouseWorking"].ToString();
                if (rd["NoChild"].ToString() != "")
                    txtNoChild.Text = rd["NoChild"].ToString();
                if (rd["SpouseDisable"].ToString() != "")
                    ddlSpouseDisable.SelectedValue = rd["SpouseDisable"].ToString();
                if (rd["SelfDisable"].ToString() != "")
                    ddlSelfDisable.SelectedValue = rd["SelfDisable"].ToString();

                if (rd["DateJoined"].ToString() != "")
                {
                    string s = rd["DateJoined"].ToString();
                    var date = DateTime.ParseExact(s, "d/M/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                    txtDateJoin.Text = date.ToString("yyyy-MM-dd");
                }
                txtDepartment.Text = rd["Department"].ToString();
                txtPosition.Text = rd["Position"].ToString();
                if (rd["JobType"].ToString() != "")
                    ddlJobType.SelectedValue = rd["JobType"].ToString();

                if (rd["SalaryEffectiveDate"].ToString() != "")
                {
                    string s = rd["SalaryEffectiveDate"].ToString();
                    var date = DateTime.ParseExact(s, "d/M/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                    txtSalaryEffectiveDate.Text = date.ToString("yyyy-MM-dd");
                }
                if (rd["Salary"].ToString() != "")
                    txtBasicSalary.Text = rd["Salary"].ToString();
                if (rd["HourlyRate"].ToString() != "")
                    txtHourlyRate.Text = rd["HourlyRate"].ToString();
                if (rd["OvertimeRate"].ToString() != "")
                    txtOvertimeRate.Text = rd["OvertimeRate"].ToString();
                txtBank.Text = rd["Bank"].ToString();
                txtBankAccNo.Text = rd["BankAccNo"].ToString();
                if (rd["PaymentPeriod"].ToString() != "")
                    ddlPayPeriod.SelectedValue = rd["PaymentPeriod"].ToString();
                if (rd["PaymentMethod"].ToString() != "")
                    ddlPayMethod.SelectedValue = rd["PaymentMethod"].ToString();
                if (rd["EmployerEpfRate"].ToString() != "")
                    ddlEmployerEpfRate.SelectedValue = rd["EmployerEpfRate"].ToString();
                if (rd["EmployeeEpfRate"].ToString() != "")
                    ddlEmployeeEpfRate.SelectedValue = rd["EmployeeEpfRate"].ToString();
                txtEpfNo.Text = rd["EpfNo"].ToString();
                if (rd["SocsoCategory"].ToString() != "")
                    ddlSocsoCategory.SelectedValue = rd["SocsoCategory"].ToString();
                if (rd["EisContribution"].ToString() != "")
                    ddlEisContribution.SelectedValue = rd["EisContribution"].ToString();
                txtTaxNo.Text = rd["TaxNo"].ToString();
                if (rd["TaxStatus"].ToString() != "")
                    ddlTaxStatus.SelectedValue = rd["TaxStatus"].ToString();
            }
            con.Close();
        }

        private void ResetAllDefault()
        {

            btnEditt.Visible = true;
            btnSave.Visible = false;

            imgProfile.ImageUrl = "~/Image/defaultProfileImg.png";
            txtStaffID.Text = "";
            txtName.Text = "";
            txtEmail.Text = "";
            ddlGender.SelectedValue = "Male";
            txtNationality.Text = "";
            txtIC.Text = "";
            txtPhoneNo.Text = "";
            txtAddress.Text = "";
            ddlMaritalStatus.SelectedValue = "Single";
            ddlSpouseWork.SelectedValue = "No";
            txtNoChild.Text = "0";
            ddlSpouseDisable.SelectedValue = "No";
            ddlSelfDisable.SelectedValue = "No";

            txtDateJoin.Text = "";
            txtDepartment.Text = "";
            txtPosition.Text = "";
            ddlJobType.SelectedValue = "Permenant";

            txtSalaryEffectiveDate.Text = "";
            txtBasicSalary.Text = "0";
            txtHourlyRate.Text = "0";
            txtOvertimeRate.Text = "0";
            txtBank.Text = "";
            txtBankAccNo.Text = "";
            ddlPayPeriod.SelectedValue = "Monthly";
            ddlPayMethod.SelectedValue = "Bank";
            ddlEmployerEpfRate.SelectedValue = "No Contribution";
            ddlEmployeeEpfRate.SelectedValue = "No Contribution";
            txtEpfNo.Text = "";
            ddlSocsoCategory.SelectedValue = "No Contribution";
            ddlEisContribution.SelectedValue = "No";
            txtTaxNo.Text = "";
            ddlTaxStatus.SelectedValue = "No Contribution";

            //===========================

            //lblFileUpload.Visible = false;
            //btnDelete.Visible = false;

            ResetDisable();
        }

        private void ResetDisable()
        {
            txtName.Enabled = false;
            txtEmail.Enabled = false;
            ddlGender.Enabled = false;
            txtNationality.Enabled = false;
            txtIC.Enabled = false;
            txtPhoneNo.Enabled = false;
            txtAddress.Enabled = false;
            ddlMaritalStatus.Enabled = false;
            ddlSpouseWork.Enabled = false;
            txtNoChild.Enabled = false;
            ddlSpouseDisable.Enabled = false;
            ddlSelfDisable.Enabled = false;

            txtDateJoin.Enabled = false;
            txtDepartment.Enabled = false;
            txtPosition.Enabled = false;
            ddlJobType.Enabled = false;

            txtSalaryEffectiveDate.Enabled = false;
            txtBasicSalary.Enabled = false;
            txtHourlyRate.Enabled = false;
            txtOvertimeRate.Enabled = false;
            txtBank.Enabled = false;
            txtBankAccNo.Enabled = false;
            ddlPayPeriod.Enabled = false;
            ddlPayMethod.Enabled = false;
            ddlEmployerEpfRate.Enabled = false;
            ddlEmployeeEpfRate.Enabled = false;
            txtEpfNo.Enabled = false;
            ddlSocsoCategory.Enabled = false;
            ddlEisContribution.Enabled = false;
            txtTaxNo.Enabled = false;
            ddlTaxStatus.Enabled = false;
        }

        private void SetEnable()
        {
            btnEditt.Visible = false;
            btnSave.Visible = true;

            //RegularExpressionValidator1.ControlToValidate = "FileUpLoad1";
            //lblFileUpload.Visible = true;
            FileUpload1.Visible = true;
            string defaultImageUrl = "data:Image/png;base64,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";
            //if (imgProfile.ImageUrl.ToString() == defaultImageUrl)
                //btnDelete.Visible = false;
            //else
                //btnDelete.Visible = true;
            txtName.Enabled = true;
           // txtEmail.Enabled = true;
            ddlGender.Enabled = true;
            txtNationality.Enabled = true;
            txtIC.Enabled = true;
            txtPhoneNo.Enabled = true;
            txtAddress.Enabled = true;
            ddlMaritalStatus.Enabled = true;
            if (ddlMaritalStatus.SelectedValue == "Single")
            {
                ddlSpouseWork.Enabled = false;
                ddlSpouseDisable.Enabled = false;
                txtNoChild.Enabled = false;
            }
            else if (ddlMaritalStatus.SelectedValue == "Married")
            {
                ddlSpouseWork.Enabled = true;
                ddlSpouseDisable.Enabled = true;
                txtNoChild.Enabled = true;
            }
            else
            {
                ddlSpouseWork.Enabled = false;
                ddlSpouseDisable.Enabled = false;
                txtNoChild.Enabled = true;
            }
            ddlSelfDisable.Enabled = true;

            txtDateJoin.Enabled = true;
            txtDepartment.Enabled = true;
            txtPosition.Enabled = true;
            ddlJobType.Enabled = true;

            txtSalaryEffectiveDate.Enabled = true;
            txtBasicSalary.Enabled = true;
            txtHourlyRate.Enabled = true;
            txtOvertimeRate.Enabled = true;
            txtBank.Enabled = true;
            txtBankAccNo.Enabled = true;
            ddlPayPeriod.Enabled = true;
            ddlPayMethod.Enabled = true;
            ddlEmployerEpfRate.Enabled = true;
            ddlEmployeeEpfRate.Enabled = true;
            txtEpfNo.Enabled = true;
            ddlSocsoCategory.Enabled = true;
            ddlEisContribution.Enabled = true;
            txtTaxNo.Enabled = true;
            ddlTaxStatus.Enabled = true;
        }

        protected void btnProfile_Click(object sender, EventArgs e)
        {
            personalPanel.Visible = true;
            jobPanel.Visible = false;
            salaryPanel.Visible = false;
            btnProfile.CssClass = "profileMenuButton profileMenuButtonActive";
            btnJob.CssClass = "profileMenuButton";
            btnSalary.CssClass = "profileMenuButton";
            tempSaveProfileImage();
        }

        protected void btnJob_Click(object sender, EventArgs e)
        {
            personalPanel.Visible = false;
            jobPanel.Visible = true;
            salaryPanel.Visible = false;
            btnProfile.CssClass = "profileMenuButton";
            btnJob.CssClass = "profileMenuButton profileMenuButtonActive";
            btnSalary.CssClass = "profileMenuButton";
            tempSaveProfileImage();
        }

        protected void btnSalary_Click(object sender, EventArgs e)
        {
            personalPanel.Visible = false;
            jobPanel.Visible = false;
            salaryPanel.Visible = true;
            btnProfile.CssClass = "profileMenuButton";
            btnJob.CssClass = "profileMenuButton";
            btnSalary.CssClass = "profileMenuButton profileMenuButtonActive";
            tempSaveProfileImage();
        }

        private void tempSaveProfileImage()
        {
            //if(txtStoreImageURL.Text == "default")
            //{
            //    return;
            //}
            string folderPath = Server.MapPath("~/Image/");
            string fileName = "";

            //Check whether Directory (Folder) exists.
            if (!Directory.Exists(folderPath))
            {
                //If Directory (Folder) does not exists Create it.
                Directory.CreateDirectory(folderPath);
            }

            if (Path.GetFileName(FileUpload1.FileName) == "" || Path.GetFileName(FileUpload1.FileName) == null)
            {
                fileName = imgProfile.ImageUrl;
            }
            else
            {
                fileName = Path.GetFileName(FileUpload1.FileName);
                //Save the File to the Directory (Folder). 
                var result = fileName.Substring(fileName.Length - 3);
                if (result == "jpg" || result == "JPG" || result == "jpeg" || result == "JPEG" || result == "png" || result == "PNG")
                {
                    FileUpload1.SaveAs(folderPath + fileName);
                    string checkFileExist = Server.MapPath("~/Image/tempProfile.png");
                    if (File.Exists(checkFileExist))
                    {
                        //If folder exists, Delete it.
                        System.IO.File.Delete(checkFileExist);
                    }
                    System.IO.File.Move(folderPath + fileName, folderPath + "tempProfile.png");
                    fileName = "~/Image/tempProfile.png";
                }
                else
                {
                    return;
                }
            }


            //Display the Picture in Image control.
            imgProfile.ImageUrl = fileName;
        }

        protected void ddlMaritalStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            string result = ddlMaritalStatus.SelectedValue.ToString();
            if (result == "Single")
            {
                ddlSpouseWork.Enabled = false;
                ddlSpouseDisable.Enabled = false;
                txtNoChild.Enabled = false;
                ddlSpouseWork.SelectedValue = "No";
                ddlSpouseDisable.SelectedValue = "No";
                txtNoChild.Text = "0";
            }
            else if (result == "Married")
            {
                ddlSpouseWork.Enabled = true;
                ddlSpouseDisable.Enabled = true;
                txtNoChild.Enabled = true;
            }
            else
            {
                ddlSpouseWork.Enabled = false;
                ddlSpouseDisable.Enabled = false;
                txtNoChild.Enabled = true;
                ddlSpouseWork.SelectedValue = "No";
                ddlSpouseDisable.SelectedValue = "No";
            }
        }

        protected void btnEditt_Click(object sender, EventArgs e)
        {
            SetEnable();
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string staff_id = txtStaffID.Text.ToString();


            byte[] bytes = null;
            byte[] imageBytes = null;
            using (BinaryReader br = new BinaryReader(FileUpload1.PostedFile.InputStream))
            {
                imageBytes = br.ReadBytes(FileUpload1.PostedFile.ContentLength);
            }
            if (imageBytes.Length == 0)
            {
                string abc = "~/Image/defaultProfileImg.png";
                imageBytes = System.IO.File.ReadAllBytes(HttpContext.Current.Server.MapPath(abc));
            }
            bytes = imageBytes;
            string name = txtName.Text.ToString();
            string email = txtEmail.Text.ToString();
            string gender = ddlGender.SelectedValue.ToString();
            string nationality = txtNationality.Text.ToString();
            string ic = txtIC.Text.ToString();
            string phoneNo = txtPhoneNo.Text.ToString();
            string address = txtAddress.Text.ToString();
            string maritalStatus = ddlMaritalStatus.SelectedValue.ToString();
            string spousework = ddlSpouseWork.SelectedValue.ToString();
            string noChild = txtNoChild.Text.ToString();
            string spouseDisable = ddlSpouseDisable.SelectedValue.ToString();
            string selfDisable = ddlSelfDisable.SelectedValue.ToString();


            //string s = txtDateJoin.Text.ToString();
            //var dateJoin = DateTime.ParseExact(s, "d/M/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            //txtDateJoin.Text = dateJoin.ToString("dd/MM/yyyy");
            //dateJoin = DateTime.ParseExact(txtDateJoin.Text.ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture);
            



            DateTime dateJoin = DateTime.ParseExact(txtDateJoin.Text.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            string department = txtDepartment.Text.ToString();
            string position = txtPosition.Text.ToString();
            string jobtype = ddlJobType.SelectedValue.ToString();

            DateTime salaryEffectiveDate = DateTime.ParseExact(txtSalaryEffectiveDate.Text.ToString(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
            decimal salary = decimal.Parse(txtBasicSalary.Text.ToString()); salary = decimal.Round(salary, 2);
            decimal hourlyRate = decimal.Parse(txtHourlyRate.Text.ToString()); hourlyRate = decimal.Round(hourlyRate, 2);
            decimal overtimeRate = decimal.Parse(txtOvertimeRate.Text.ToString()); overtimeRate = decimal.Round(overtimeRate, 2);
            string bank = txtBank.Text.ToString();
            string bankAccNo = txtBankAccNo.Text.ToString();
            string payPeriod = ddlPayPeriod.SelectedValue.ToString();
            string payMethod = ddlPayMethod.SelectedValue.ToString();
            decimal employerEpfRate = decimal.Parse(ddlEmployerEpfRate.SelectedValue.ToString()); employerEpfRate = decimal.Round(employerEpfRate, 2);
            decimal employeeEpfRate = decimal.Parse(ddlEmployeeEpfRate.SelectedValue.ToString()); employeeEpfRate = decimal.Round(employeeEpfRate, 2);
            string epfNo = txtEpfNo.Text.ToString();
            string socsoCategory = ddlSocsoCategory.SelectedValue.ToString();
            string eisContribution = ddlEisContribution.SelectedValue.ToString();
            string taxNo = txtTaxNo.Text.ToString();
            string taxStatus = ddlTaxStatus.SelectedValue.ToString();


            //database
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Staff_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "UPDATE");
                    cmd.Parameters.AddWithValue("@Staff_ID", Int16.Parse(staff_id)); 
                    cmd.Parameters.AddWithValue("@Photo", bytes);
                    cmd.Parameters.AddWithValue("@Name", name);
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Gender", gender);
                    cmd.Parameters.AddWithValue("@Nationality", nationality);
                    cmd.Parameters.AddWithValue("@IcNo", ic);
                    cmd.Parameters.AddWithValue("@PhoneNo", phoneNo);
                    cmd.Parameters.AddWithValue("@Address", address);
                    cmd.Parameters.AddWithValue("@MaritalStatus", maritalStatus);
                    cmd.Parameters.AddWithValue("@SpouseWorking", spousework);
                    cmd.Parameters.AddWithValue("@NoChild", noChild);
                    cmd.Parameters.AddWithValue("@SpouseDisable", spouseDisable);
                    cmd.Parameters.AddWithValue("@SelfDisable", selfDisable);
                    cmd.Parameters.AddWithValue("@DateJoined", dateJoin);
                    cmd.Parameters.AddWithValue("@Department", department);
                    cmd.Parameters.AddWithValue("@Position", position);
                    cmd.Parameters.AddWithValue("@JobType", jobtype);
                    cmd.Parameters.AddWithValue("@SalaryEffectiveDate", salaryEffectiveDate);
                    cmd.Parameters.AddWithValue("@Salary", salary);
                    cmd.Parameters.AddWithValue("@HourlyRate", hourlyRate);
                    cmd.Parameters.AddWithValue("@OvertimeRate", overtimeRate);
                    cmd.Parameters.AddWithValue("@Bank", bank);
                    cmd.Parameters.AddWithValue("@BankAccNo", bankAccNo);
                    cmd.Parameters.AddWithValue("@PaymentPeriod", payPeriod);
                    cmd.Parameters.AddWithValue("@PaymentMethod", payMethod);
                    cmd.Parameters.AddWithValue("@EmployerEpfRate", employerEpfRate);
                    cmd.Parameters.AddWithValue("@EmployeeEpfRate", employeeEpfRate);
                    cmd.Parameters.AddWithValue("@EpfNo", epfNo);
                    cmd.Parameters.AddWithValue("@SocsoCategory", socsoCategory);
                    cmd.Parameters.AddWithValue("@EisContribution", eisContribution);
                    cmd.Parameters.AddWithValue("@TaxNo", taxNo);
                    cmd.Parameters.AddWithValue("@TaxStatus", taxStatus);
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
                    }
                }
            }
            ResetDisable();
            btnSave.Visible = false;
            btnEditt.Visible = true;
            BindData(txtStaffID.Text);
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            imgProfile.ImageUrl = "deleted!)^%$(!";
            //btnDelete.Visible = false;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("~/Project/EmployeeList.aspx");
        }

        //protected void Button1_Click(object sender, EventArgs e)
        //{
        //    string abc = "";
        //}
    }
}