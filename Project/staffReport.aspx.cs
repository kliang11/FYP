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
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            string date = Request.QueryString["date"];
            if(id!=null && date != null)
            {
                 bindReport(id,date);
            }
           
        }

        private void bindReport(string id, string date) {
            DateTime date2 = Convert.ToDateTime(date);
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlCommand cmd = new SqlCommand("SELECT Staff.Name, sum(case when AttendanceStatus = 'Present' then 1 else 0 end) as Present, sum(case when AttendanceStatus = 'Absent' then 1 else 0 end) as Absent, sum(case when AttendanceStatus = 'On Leave' then 1 else 0 end) as OnLeave, sum(case when IsLate = 'Y' then 1 else 0 end) as Late, sum(WorkingHour) as TotalWorkingHour, sum(Overtime) as TotalOvertime, (cast(sum(case when AttendanceStatus = 'Present' then 1 else 0 end) as float) / (cast(sum(case when AttendanceStatus = 'Present' then 1 else 0 end)as float) +cast(sum(case when AttendanceStatus = 'Absent' then 1 else 0 end)as float)))*100 as AttendanceRate,cast(sum(case when IsLate = 'Y' then 1 else 0 end) as float) / cast(sum(case when AttendanceStatus = 'Present' then 1 else 0 end)as float) *100 as LatePercentage, DATENAME(month,AttendanceDate) as Month, Year(AttendanceDate) as Year from Attendance LEFT JOIN STAFF on attendance.StaffID = Staff.Staff_ID WHERE MONTH(AttendanceDate) = MONTH(@date) AND YEAR(AttendanceDate) = Year(@date) AND StaffID=@staffID GROUP BY StaffID,Name,DATENAME(month,AttendanceDate),Year(AttendanceDate)", con);
            cmd.Parameters.AddWithValue("@date", date2);
            cmd.Parameters.AddWithValue("@staffID", id);
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