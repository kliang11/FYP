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
    public partial class AttendanceReportG : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string date = Request.QueryString["date"];
            string option = Request.QueryString["option"];
            if (date != null && option != null)
            {
                bindReport(date,option);
            }
        }
        
        private void bindReport(string date, string option)
        {
            if(option == "M")
            {
                DateTime dateConvert = Convert.ToDateTime(date);
                string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand("SELECT Staff.Name, sum(case when AttendanceStatus = 'Present' then 1 else 0 end) as Present, sum(case when AttendanceStatus = 'Absent' then 1 else 0 end) as Absent, sum(case when AttendanceStatus = 'On Leave' then 1 else 0 end) as OnLeave, sum(case when IsLate = 'Y' then 1 else 0 end) as Late, sum(WorkingHour) as TotalWorkingHour, sum(Overtime) as TotalOvertime, (cast(sum(case when AttendanceStatus = 'Present' then 1 else 0 end) as float) / (cast(sum(case when AttendanceStatus = 'Present' then 1 else 0 end)as float) +cast(sum(case when AttendanceStatus = 'Absent' then 1 else 0 end)as float)))*100 as AttendanceRate,cast(sum(case when IsLate = 'Y' then 1 else 0 end) as float) / cast(sum(case when AttendanceStatus = 'Present' then 1 else 0 end)as float) *100 as LatePercentage, DATENAME(month,AttendanceDate) as Month, Year(AttendanceDate) as Year from Attendance LEFT JOIN STAFF on attendance.StaffID = Staff.Staff_ID WHERE MONTH(AttendanceDate) = MONTH(@date) AND YEAR(AttendanceDate) = Year(@date) GROUP BY StaffID,Name,DATENAME(month,AttendanceDate),Year(AttendanceDate)", con);
                cmd.Parameters.AddWithValue("@date", dateConvert);
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
                crp.Load(Server.MapPath("~/Project/AttendanceReport.rpt"));
                crp.SetDataSource(ds.Tables["table"]);
                crp.SummaryInfo.ReportTitle = "Monthly Overall Staff Attendance Report ";
                CrystalReportViewer1.ReportSource = crp;
                crp.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "Staff Attendance");
            }
            else if (option == "W")
            {
                DateTime startDate = Convert.ToDateTime(date);
                DateTime endDate = startDate.AddDays(7);
                string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                SqlConnection con = new SqlConnection(constr);
                SqlCommand cmd = new SqlCommand("SELECT Staff.Name, sum(case when AttendanceStatus = 'Present' then 1 else 0 end) as Present, sum(case when AttendanceStatus = 'Absent' then 1 else 0 end) as Absent, sum(case when AttendanceStatus = 'On Leave' then 1 else 0 end) as OnLeave, sum(case when IsLate = 'Y' then 1 else 0 end) as Late, sum(WorkingHour) as TotalWorkingHour, sum(Overtime) as TotalOvertime, (cast(sum(case when AttendanceStatus = 'Present' then 1 else 0 end) as float) / (cast(sum(case when AttendanceStatus = 'Present' then 1 else 0 end)as float) +cast(sum(case when AttendanceStatus = 'Absent' then 1 else 0 end)as float)))*100 as AttendanceRate,cast(sum(case when IsLate = 'Y' then 1 else 0 end) as float) / cast(sum(case when AttendanceStatus = 'Present' then 1 else 0 end)as float) *100 as LatePercentage from Attendance LEFT JOIN STAFF on attendance.StaffID = Staff.Staff_ID WHERE AttendanceDate >= @startDate and AttendanceDate <= @endDate GROUP BY StaffID,Name", con);
                cmd.Parameters.AddWithValue("@startDate", startDate);
                cmd.Parameters.AddWithValue("@endDate", endDate);
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
                crp.Load(Server.MapPath("~/Project/WeekAttendanceReport.rpt"));
                crp.SetDataSource(ds.Tables["table"]);
                crp.SetParameterValue("StartDate", startDate.ToShortDateString());
                crp.SetParameterValue("EndDate", endDate.ToShortDateString());

                crp.SummaryInfo.ReportTitle = "Weekly Overall Staff Attendance Report ";
                CrystalReportViewer1.ReportSource = crp;

                crp.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "Staff Attendance");
            }
        }
    }
}