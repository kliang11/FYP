using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FYP.Project
{
    public partial class attendanceStaff : System.Web.UI.Page
    {
        //private string staffID = "2";  //staff id 
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

                    if (Session["role"].ToString() == "Normal Staff")
                    {
                        txtSelectMonth.Text = DateTime.Now.ToString("yyyy-MM");
                        BindGrid();
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

        private void BindGrid()
        {
            string staffID = "";
            if (Session["id"] != null)
            {
                 staffID = Session["id"].ToString();
            }
            DateTime date = Convert.ToDateTime(txtSelectMonth.Text);
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Attendance_CRUD"))
                {
                    cmd.Parameters.AddWithValue("@Action", "SELECT_STAFF");
                    cmd.Parameters.AddWithValue("@AttendanceDate", date);
                    cmd.Parameters.AddWithValue("@Staff_ID", staffID);
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
            presentCount(constr, date, staffID);
            absentCount(constr, date, staffID);
            onLeaveCount(constr, date, staffID);

        }

        private void absentCount(string constr, DateTime date, string staffID)
        {
            using (SqlConnection mycon = new SqlConnection(constr))
            {

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) from Attendance where MONTH(AttendanceDate) = MONTH(@date) AND YEAR(AttendanceDate) = Year(@date) and StaffID = @staffID and AttendanceStatus = 'Absent'", mycon))
                {
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Parameters.AddWithValue("@staffID", staffID);
                    cmd.CommandType = CommandType.Text;
                    mycon.Open();

                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    countAbsent.InnerText = count.ToString();
                }
            }
        }

        private void presentCount(string constr, DateTime date, string staffID)
        {
            using (SqlConnection mycon = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) from Attendance where MONTH(AttendanceDate) = MONTH(@date) AND YEAR(AttendanceDate) = Year(@date) and StaffID = @staffID and AttendanceStatus = 'Present'", mycon))
                {
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Parameters.AddWithValue("@staffID", staffID);
                    cmd.CommandType = CommandType.Text;
                    mycon.Open();

                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    countPresent.InnerText = count.ToString();
                }
            }
        }

        private void onLeaveCount(string constr, DateTime date, string staffID)
        {
            using (SqlConnection mycon = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) from Attendance where MONTH(AttendanceDate) = MONTH(@date) AND YEAR(AttendanceDate) = Year(@date) and StaffID = @staffID and AttendanceStatus = 'On Leave'", mycon))
                {
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Parameters.AddWithValue("@staffID", staffID);
                    cmd.CommandType = CommandType.Text;
                    mycon.Open();

                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    countOnLeave.InnerText = count.ToString();
                }
            }
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label arrival = e.Row.FindControl("lblStaffArrival") as Label;
                if (arrival.Text.Equals("N"))
                {
                    arrival.Text = "On Time";
                    arrival.CssClass = "label_green";
                }
                else if (arrival.Text.Equals("Y"))
                {
                    arrival.Text = "Late";
                    arrival.CssClass = "label_red";
                }

                Label status = e.Row.FindControl("lblAttendanceStatus") as Label;
                if (status.Text.Equals("Present"))
                    status.CssClass = "label_green";
                else if (status.Text.Equals("On Leave"))
                    status.CssClass = "label_grey";
                else if (status.Text.Equals("Absent"))
                {
                    status.CssClass = "label_redAbsent";
                    arrival.Text = "-";
                    arrival.CssClass = "";
                }
                else
                    status.CssClass = "label_yellow";

                var timeIn = (e.Row.FindControl("lblTimeIn") as Label).Text;
                var timeOut = (e.Row.FindControl("lblTimeOut") as Label).Text;
                var workingHour = (e.Row.FindControl("lblStaffWorkingHour") as Label).Text;
                if (timeIn.Equals(""))
                {
                    (e.Row.FindControl("lblTimeIn") as Label).Text = "-";
                }
                if (timeOut.Equals(""))
                {
                    (e.Row.FindControl("lblTimeOut") as Label).Text = "-";
                }
                if (workingHour.Equals(""))
                {
                    (e.Row.FindControl("lblStaffWorkingHour") as Label).Text = "-";
                }
            }
        }

        protected void minusMonth(object sender, EventArgs e)
        {
            DateTime date = Convert.ToDateTime(txtSelectMonth.Text).AddMonths(-1);
            txtSelectMonth.Text = date.ToString("yyyy-MM");
            BindGrid();

        }

        protected void addMonth(object sender, EventArgs e)
        {
            DateTime date = Convert.ToDateTime(txtSelectMonth.Text).AddMonths(1);
            txtSelectMonth.Text = date.ToString("yyyy-MM");
            BindGrid();
        }

        protected void txtSelectMonth_TextChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void OnRowSelect(object sender, EventArgs e)
        {
            Label a = (Label)gvList.SelectedRow.FindControl("lblAttendanceID");
            string attendanceID = a.Text.ToString();
            Response.Redirect(string.Format("~/Project/attendanceDetails.aspx?id={0}", attendanceID));
        }

        protected void btnReport_Click(object sender, EventArgs e)
        {
            string staffID = "";
            if (Session["id"] != null)
            {
                staffID = Session["id"].ToString();
            }
            string date = txtSelectMonth.Text;
            string url = string.Format("/Project/staffReport.aspx?date={0}&id={1}", date, staffID);
            StringBuilder sb = new StringBuilder();
            sb.Append("<script type = 'text/javascript'>");
            sb.Append("window.open('");
            sb.Append(url);
            sb.Append("');");
            sb.Append("</script>");
            ClientScript.RegisterStartupScript(this.GetType(), "script", sb.ToString());


        }
    }
}