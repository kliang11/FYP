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
    public partial class ClaimDetails : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                
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
                ResetAllDefault();
            }
        }

        private void ResetAllDefault()
        {
            string url = ViewState["RefUrl"].ToString();
            if (url.Contains("ApplyClaim"))
            {
                btnApproves.Visible = false;
                btnReject.Visible = false;
            }
            else if (url.Contains("ClaimList"))
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

        private void BindData(string id)
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Claim_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "SELECTDETAIL");
                    cmd.Parameters.AddWithValue("@ClaimID", id);
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        if (rd["ClaimAttachment"].ToString() != "")
                        {
                            imgBtnClaimAttachment.ImageUrl = "data:Image/png;base64," + Convert.ToBase64String((byte[])rd["ClaimAttachment"]);
                        }
                        txtClaimID.Text = rd["ClaimID"].ToString();
                        string claimDate = rd["ClaimDate"].ToString();
                        txtClaimDate.Text = Convert.ToDateTime(claimDate).ToShortDateString().ToString();
                        txtStaffID.Text = rd["Staff_ID"].ToString();
                        txtStaffName.Text = rd["Name"].ToString();
                        string dateFrom = rd["DateFrom"].ToString();
                        txtDateFrom.Text = Convert.ToDateTime(dateFrom).ToShortDateString().ToString();
                        string dateTo = rd["DateTo"].ToString();
                        txtDateTo.Text = Convert.ToDateTime(dateTo).ToShortDateString().ToString();
                        txtClaimType.Text = rd["ClaimTypeDesc"].ToString();
                        txtAmount.Text = rd["ClaimAmount"].ToString();
                        txtDescription.Text = rd["Description"].ToString();
                        txtStatus.Text = rd["ClaimStatus"].ToString();
                        txtClaimReceive.Text = rd["ClaimReceive"].ToString();
                        txtReason.Text = rd["RejectReason"].ToString();
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
            UpdateApproveReject("Reject", txtRejectReasonPopUp.Text.Trim());
            BindData(txtClaimID.Text.Trim());
            ResetAllDefault();
        }
        protected void btnApproves_Click(object sender, EventArgs e)
        {
            UpdateApproveReject("Approve", "-");
            BindData(txtClaimID.Text.Trim());
            ResetAllDefault();
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            rejectReasonPopup.Visible = true;
        }

        private void UpdateApproveReject(String approveReject, String reason)
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Claim_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "UPDATE");
                    cmd.Parameters.AddWithValue("@ClaimStatus", approveReject);
                    cmd.Parameters.AddWithValue("@RejectReason", reason);
                    cmd.Parameters.AddWithValue("@ClaimID", txtClaimID.Text.Trim());
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }
    }
}