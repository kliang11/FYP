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
    public partial class LeaveList : System.Web.UI.Page
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
                    Response.Redirect("~/Project/Login.aspx?ReturnUrl=%2fEmployeeList.aspx");
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
                if ((e.Row.FindControl("lblStatus") as Label).Text.Equals("Rejected"))
                {
                    (e.Row.FindControl("lblStatus") as Label).CssClass = "label_redAbsent";
                    (e.Row.FindControl("btnApprove") as ImageButton).Visible = false;
                    (e.Row.FindControl("btnReject") as ImageButton).Visible = false;
                }
                else if ((e.Row.FindControl("lblStatus") as Label).Text.Equals("Approved"))
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
                using (SqlCommand cmd = new SqlCommand("Leave_CRUD"))
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
            count(constr);
        }

        private void count(string constr)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Leave_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "COUNT");
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        countApproved.InnerText = rd["Approved"].ToString();
                        countPending.InnerText = rd["Pending"].ToString();
                        countRejected.InnerText = rd["Rejected"].ToString();
                    }
                    con.Close();
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
                using (SqlCommand cmd = new SqlCommand("Leave_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "UPDATE");
                    cmd.Parameters.AddWithValue("@LeaveStatus", "Rejected");
                    cmd.Parameters.AddWithValue("@PullBackReason", txtRejectReasonPopUp.Text.Trim());
                    cmd.Parameters.AddWithValue("@LeaveAppID", id);
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
            string leaveType = (string)gvList.DataKeys[row.RowIndex]["LeaveType"].ToString();
            string staffID = (string)gvList.DataKeys[row.RowIndex]["StaffID"].ToString();
            string leaveCount = (string)gvList.DataKeys[row.RowIndex]["TotalDay"].ToString();
            DateTime leaveFrom = Convert.ToDateTime((string)gvList.DataKeys[row.RowIndex]["LeaveDateStart"].ToString());

            string command = e.CommandName;
            switch (command)
            {
                case "Approving":
                    {
                        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand("Leave_CRUD"))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@Action", "UPDATE");
                                cmd.Parameters.AddWithValue("@LeaveStatus", "Approved");
                                cmd.Parameters.AddWithValue("@PullBackReason", "-");
                                cmd.Parameters.AddWithValue("@LeaveAppID", id);
                                cmd.Connection = con;
                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                        updateCount(constr, leaveCount, staffID, leaveType);
                        if (leaveType == "Sick Leave" && DateTime.Compare(leaveFrom,DateTime.Now) <= 0)
                        {
                            int count = int.Parse(leaveCount)-1;
                            DateTime leaveTo = DateTime.Now;
                            if (DateTime.Compare(leaveFrom.AddDays(count), DateTime.Now) < 0)
                            {
                                leaveTo = leaveFrom.AddDays(count);
                            }
                            updateAttendanceStatus(constr, leaveFrom, leaveTo ,staffID);
                        }                                  
                           
                        this.BindGrid();
                        break;
                    }


                case "Rejecting":
                    {
                        rejectReasonPopup.Visible = true;
                        lblPopUpID.Text = "Leave ID: " + id;
                        break;
                    }

                case "Selecting":
                    {
                        Response.Redirect(string.Format("~/Project/LeaveDetailsStaff.aspx?id={0}", id));
                        break;
                    }
            }
        }
        
        private void updateCount(string constr,string leaveCount,string staffID, string leaveType)
        {
            using (SqlConnection con = new SqlConnection(constr))
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
                    cmd.Parameters.AddWithValue("@Count", leaveCount);
                    cmd.Parameters.AddWithValue("@Staff_ID", staffID);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        private void updateAttendanceStatus(string constr, DateTime leaveFrom,DateTime leaveTo, string staffID)
        {
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Attendance_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "UPDATE_STATUS");
                    cmd.Parameters.AddWithValue("@Staff_ID", staffID);
                    cmd.Parameters.AddWithValue("@date1", leaveFrom);
                    cmd.Parameters.AddWithValue("@date2", leaveTo);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
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