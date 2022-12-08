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
    public partial class ClaimList : System.Web.UI.Page
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
                    if (Session["role"].ToString() == "HR Staff")
                    {
                        this.BindGrid();
                    }
                    else
                    {
                        Response.Redirect(string.Format("~/Project/403error.html"));
                    }
                }
                else
                {
                    Response.Redirect("~/Project/Login.aspx?ReturnUrl=%2fClaimList.aspx");
                }
            }
            else if (gvList.Rows.Count != 0)
            {
                gvList.UseAccessibleHeader = true;
                gvList.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvList.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != gvList.EditIndex)
            {
                if ((e.Row.FindControl("lblStatus") as Label).Text.Equals("Reject"))
                {
                    (e.Row.FindControl("lblStatus") as Label).CssClass = "label_red";
                    (e.Row.FindControl("btnApprove") as ImageButton).Visible = false;
                    (e.Row.FindControl("btnReject") as ImageButton).Visible = false;
                }
                else if ((e.Row.FindControl("lblStatus") as Label).Text.Equals("Approve"))
                {
                    (e.Row.FindControl("lblStatus") as Label).CssClass = "label_green";
                    (e.Row.FindControl("btnApprove") as ImageButton).Visible = false;
                    (e.Row.FindControl("btnReject") as ImageButton).Visible = false;
                }

                else if ((e.Row.FindControl("lblStatus") as Label).Text.Equals("Pending"))
                {
                    (e.Row.FindControl("lblStatus") as Label).CssClass = "label_yellow";
                    (e.Row.FindControl("btnApprove") as ImageButton).Visible = true;
                    (e.Row.FindControl("btnReject") as ImageButton).Visible = true;
                }
            }
        }
        private void BindGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Claim_CRUD"))
                {
                    cmd.Parameters.AddWithValue("@Action", "SELECT");
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

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            rejectReasonPopup.Visible = false;
            string id = lblPopUpID.Text.Remove(0, 10);
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Claim_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "UPDATE");
                    cmd.Parameters.AddWithValue("@ClaimStatus", "Reject");
                    cmd.Parameters.AddWithValue("@RejectReason", txtRejectReasonPopUp.Text.Trim());
                    cmd.Parameters.AddWithValue("@ClaimReceive", "Reject");
                    cmd.Parameters.AddWithValue("@ClaimID", id);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            this.BindGrid();

        }

        protected void FireRowCommand(object sender, GridViewCommandEventArgs e)
        {

            GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            int id = (int)gvList.DataKeys[row.RowIndex].Value;

            string command = e.CommandName;
            switch (command)
            {
                case "Approving":
                    {
                        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand("Claim_CRUD"))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@Action", "UPDATE");
                                cmd.Parameters.AddWithValue("@ClaimStatus", "Approve");
                                cmd.Parameters.AddWithValue("@RejectReason", "-");
                                cmd.Parameters.AddWithValue("@ClaimReceive", "Pending");
                                cmd.Parameters.AddWithValue("@ClaimID", id);
                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                        this.BindGrid();
                        break;
                    }


                case "Rejecting":
                    {
                        rejectReasonPopup.Visible = true;
                        lblPopUpID.Text = "Claim ID: " + id;
                        break;
                    }

                case "Selecting":
                    {
                        Response.Redirect(string.Format("~/Project/ClaimDetails.aspx?id={0}", id));
                        break;
                    }
            }
        }

        protected void btnRejectClose_Click(object sender, EventArgs e)
        {
            rejectReasonPopup.Visible = false;
            lblPopUpID.Text = "";
            txtRejectReasonPopUp.Attributes["style"] = "borderColor: \"\"; display:none";
        }
    }
}