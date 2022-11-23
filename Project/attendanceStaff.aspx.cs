using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
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
    public partial class attendanceStaff : System.Web.UI.Page
    {
        private string staffID = "1";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtSelectMonth.Text = DateTime.Now.ToString("yyyy-MM");
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
            presentCount(constr, date);
            absentCount(constr, date);
            onLeaveCount(constr, date);

        }

        private void absentCount(string constr, DateTime date)
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

        private void presentCount(string constr, DateTime date)
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

        private void onLeaveCount(string constr, DateTime date)
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
                Label status = e.Row.FindControl("lblAttendanceStatus") as Label;
                if (status.Text.Equals("Present"))
                    status.CssClass = "label_green";
                else if (status.Text.Equals("On Leave"))
                    status.CssClass = "label_grey";
                else if (status.Text.Equals("Absent"))
                    status.CssClass = "label_redAbsent";
                else
                    status.CssClass = "label_yellow";

                Label arrival = e.Row.FindControl("lblStaffArrival") as Label;
                if (arrival.Text.Equals("N"))
                {
                    arrival.Text = "On Time";
                    arrival.CssClass = "label_green";
                }
                else if (arrival.Text.Equals("Y")) {
                    arrival.Text = "Late";
                    arrival.CssClass = "label_red";
                }

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

            DateTime date = Convert.ToDateTime(txtSelectMonth.Text);
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("SELECT Staff.Name, sum(case when AttendanceStatus = 'Present' then 1 else 0 end) as Present, sum(case when AttendanceStatus = 'Absent' then 1 else 0 end) as Absent, sum(case when AttendanceStatus = 'On Leave' then 1 else 0 end) as OnLeave, sum(case when IsLate = 'Y' then 1 else 0 end) as Late, sum(WorkingHour) as TotalWorkingHour, sum(Overtime) as TotalOvertime, (cast(sum(case when AttendanceStatus = 'Present' then 1 else 0 end) as float) / (cast(sum(case when AttendanceStatus = 'Present' then 1 else 0 end)as float) +cast(sum(case when AttendanceStatus = 'Absent' then 1 else 0 end)as float)))*100 as AttendanceRate,cast(sum(case when IsLate = 'Y' then 1 else 0 end) as float) / cast(sum(case when AttendanceStatus = 'Present' then 1 else 0 end)as float) *100 as LatePercentage, DATENAME(month,AttendanceDate) as Month, Year(AttendanceDate) as Year from Attendance LEFT JOIN STAFF on attendance.StaffID = Staff.Staff_ID WHERE MONTH(AttendanceDate) = MONTH(@date) AND YEAR(AttendanceDate) = Year(@date) AND StaffID=@staffID GROUP BY StaffID,Name,DATENAME(month,AttendanceDate),Year(AttendanceDate)", con);
            cmd.Parameters.AddWithValue("@date", date);
            cmd.Parameters.AddWithValue("@staffID", staffID);
            SqlDataAdapter sda = new SqlDataAdapter(cmd);

            DataSet ds = new DataSet();
            sda.SelectCommand = cmd;
            try
            {
                sda.Fill(ds);
            }
            catch
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No attendance records found.')", true);
            }

            ReportDocument crp = new ReportDocument();
            crp.Load(Server.MapPath("~/Project/StaffAttendanceReport.rpt"));
            crp.SetDataSource(ds.Tables["table"]);
            crp.SummaryInfo.ReportTitle = "Monthly Overall Staff Attendance Report ";
            CrystalReportViewer1.ReportSource = crp;
            crp.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "Staff Attendance");
        }
    }
}