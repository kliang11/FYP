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
    public partial class LeaveDetailsStaff : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                string id = Request.QueryString["id"];
                if (id != null)
                {
                    databind(id);
                }
                ResetAllDefault();
            }
        }

        private void ResetAllDefault()
        {
            string url = ViewState["RefUrl"].ToString();
            if (url.Contains("LeaveStaff"))
            {
                btnApproves.Visible = false;
                btnReject.Visible = false;
            }
            else if (url.Contains("LeaveList"))
            {
                if (txtStatus.Text.Trim() == "Pending")
                {
                    btnApproves.Visible = true;
                    btnReject.Visible = true;
                }
                else
                {
                    btnApproves.Visible = false;
                    btnReject.Visible = false;
                }
            }

        }

        private void databind(string id)
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Leave_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "SELECT_DETAILS");
                    cmd.Parameters.AddWithValue("@LeaveAppID", id);
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        if (rd["LeaveAttachments"].ToString() != "")
                        {
                            imgBtnClaimAttachment.ImageUrl = "data:Image/png;base64," + Convert.ToBase64String((byte[])rd["LeaveAttachments"]);
                        }
                        txtLeaveID.Text = rd["LeaveAppID"].ToString();
                        string applicationDate = rd["ApplicationDate"].ToString();
                        txtLeaveAppDate.Text = Convert.ToDateTime(applicationDate).ToShortDateString().ToString();
                        txtStaffID.Text = rd["StaffID"].ToString();
                        txtStaffName.Text = rd["Name"].ToString();
                        string dateFrom = rd["LeaveDateStart"].ToString();
                        txtLeaveFrom.Text = Convert.ToDateTime(dateFrom).ToShortDateString().ToString();
                        string dateTo = rd["LeaveDateEnd"].ToString();
                        txtLeaveTo.Text = Convert.ToDateTime(dateTo).ToShortDateString().ToString();
                        txtLeaveType.Text = rd["LeaveType"].ToString();
                        txtTotalDay.Text = rd["TotalDay"].ToString()+" Days";
                        txtLeaveReason.Text = rd["LeaveReason"].ToString();
                        txtStatus.Text = rd["LeaveStatus"].ToString();
                        string rejectReason = rd["PullBackReason"].ToString();
                        if(rejectReason != "")
                        {
                            txtRejectReason.Text = rejectReason;
                        }
                        else 
                        {
                            txtRejectReason.Text = "-";
                        }
                    }
                    con.Close();
                }
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {

            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
                Response.Redirect((string)refUrl);
        }

        protected void btnRejectClose_Click(object sender, EventArgs e)
        {
            rejectReasonPopup.Visible = false;
            lblPopUpID.Text = "";
            txtRejectReasonPopUp.Attributes["style"] = "borderColor: \"\"; display:none";
        }
        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            rejectReasonPopup.Visible = false;
            UpdateApproveReject("Rejected", txtRejectReasonPopUp.Text.Trim(),"","","");
            databind(txtLeaveID.Text.Trim());
            ResetAllDefault();
        }
        protected void btnApproves_Click(object sender, EventArgs e)
        {
            UpdateApproveReject("Approved", "-",txtLeaveType.Text,txtTotalDay.Text,txtStaffID.Text);
            databind(txtLeaveID.Text.Trim());
            ResetAllDefault();
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            lblPopUpID.Text = lblPopUpID.Text+" "+txtLeaveID.Text;
            rejectReasonPopup.Visible = true;
        }

        private void UpdateApproveReject(String approveReject, String reason,string leaveType,string leaveCount,string staffID)
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Leave_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "UPDATE");
                    cmd.Parameters.AddWithValue("@LeaveStatus", approveReject);
                    cmd.Parameters.AddWithValue("@PullBackReason", reason);
                    cmd.Parameters.AddWithValue("@LeaveAppID", txtLeaveID.Text.Trim());
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                 if (approveReject == "Approved")
                {
                    using (SqlCommand cmd = new SqlCommand("Staff_CRUD"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        if (leaveType == "Sick Leave")
                        {
                            cmd.Parameters.AddWithValue("@Action", "UPDATE_SICK");
                        }
                        else
                        {
                            cmd.Parameters.AddWithValue("@Action", "UPDATE_CASUAL");
                        }
                        leaveCount = leaveCount.Remove(leaveCount.Length - 4);
                        cmd.Parameters.AddWithValue("@Count", leaveCount);
                        cmd.Parameters.AddWithValue("@Staff_ID", staffID);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
            }
            databind(txtLeaveID.Text.Trim());
        }
    }
}