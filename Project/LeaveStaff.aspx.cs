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
    public partial class LeaveStaff : System.Web.UI.Page
    {
        private string staffID = "2"; //staff id 
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
            else if (gvList.Rows.Count != 0)
            {
                gvList.UseAccessibleHeader = true;
                gvList.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvList.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }

        private void BindGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Leave_CRUD"))
                {
                    cmd.Parameters.AddWithValue("@Action", "SELECT_STAFF");
                    cmd.Parameters.AddWithValue("@StaffID", staffID);
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
            casualLeaveCount(constr);
            sickLeaveCount(constr);
        }

        private void casualLeaveCount(string constr)
        {
            using (SqlConnection mycon = new SqlConnection(constr))
            {

                using (SqlCommand cmd = new SqlCommand("SELECT TotalCasualLeave from Staff where Staff_ID = @staffID ", mycon))
                {
                    cmd.Parameters.AddWithValue("@staffID", staffID);
                    cmd.CommandType = CommandType.Text;
                    mycon.Open();

                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    countCasualLeave.InnerText = count.ToString();
                }
            }
        }

        private void sickLeaveCount(string constr)
        {
            using (SqlConnection mycon = new SqlConnection(constr))
            {

                using (SqlCommand cmd = new SqlCommand("SELECT TotalSickLeave from Staff where Staff_ID = @staffID ", mycon))
                {
                    cmd.Parameters.AddWithValue("@staffID", staffID);
                    cmd.CommandType = CommandType.Text;
                    mycon.Open();

                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    countSickLeave.InnerText = count.ToString();
                }
            }
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label status = e.Row.FindControl("lblLeaveStatus") as Label;
                if (status.Text.Equals("Approved"))
                    status.CssClass = "label_green";
                else if (status.Text.Equals("Pending"))
                    status.CssClass = "label_yellow";
                else if (status.Text.Equals("Rejected"))
                    status.CssClass = "label_redAbsent";
            }
        }
        protected void OnRowSelect(object sender, EventArgs e)
        {
            Label a = (Label)gvList.SelectedRow.FindControl("lblLeaveID");
            string leaveID = a.Text.ToString();
            Response.Redirect(string.Format("~/Project/LeaveDetailsStaff.aspx?id={0}", leaveID));
        }

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            //check the count 
            string leaveType = ddlLeaveType.SelectedValue.ToString();
            int dayCount = int.Parse(txtLeaveDay.Text.ToString());

            if (leaveType == "Casual Leave" && dayCount > int.Parse(countCasualLeave.InnerHtml))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Your Available Casual Leave not sufficient. Please Try Again.')", true);
                txtDate.Text = "";
                txtLeaveDay.Text = "";
                txtReason.Text = "";
                ddlLeaveType.SelectedIndex = 0;
                rqvField2.Attributes.Add("style", "display:none");
                return;
            }
            else if (leaveType == "Sick Leave" && dayCount > int.Parse(countSickLeave.InnerHtml))
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Your Available Sick Leave not sufficient. Please Try Again.')", true);
                txtDate.Text = "";
                txtLeaveDay.Text = "";
                txtReason.Text = "";
                ddlLeaveType.SelectedIndex = 0;
                rqvField2.Attributes.Add("style", "display:none");
                return;
            }

            DateTime startDate = Convert.ToDateTime(txtDate.Text.ToString());
            DateTime endDate = startDate.AddDays(dayCount - 1);
            string reason = txtReason.Text.ToString();
            DateTime applicationDate = DateTime.Now;

            byte[] imageBytes = null;
            using (BinaryReader br = new BinaryReader(attachment.PostedFile.InputStream))
            {
                imageBytes = br.ReadBytes(attachment.PostedFile.ContentLength);
            }

            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Leave_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "INSERT");
                    cmd.Parameters.AddWithValue("@LeaveType", leaveType);
                    cmd.Parameters.AddWithValue("@LeaveDateStart", startDate);
                    cmd.Parameters.AddWithValue("@LeaveDateEnd", endDate);
                    cmd.Parameters.AddWithValue("@TotalDay", dayCount);
                    cmd.Parameters.AddWithValue("@LeaveReason", reason);
                    cmd.Parameters.AddWithValue("@LeaveAttachments", imageBytes);
                    cmd.Parameters.AddWithValue("@ApplicationDate", applicationDate);
                    cmd.Parameters.AddWithValue("@StaffID", staffID);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    
                }
            }
            BindGrid();
            txtDate.Text = "";
            txtLeaveDay.Text = "";
            txtReason.Text = "";
            ddlLeaveType.SelectedIndex = 0;
            rqvField2.Attributes.Add("style", "display:none");
            attachment.Dispose();
        }
    }
}