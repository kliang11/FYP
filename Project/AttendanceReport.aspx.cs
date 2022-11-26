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
    public partial class AttendanceReport1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }

        }

        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {

            if (ddlSelect.Items[ddlSelect.SelectedIndex].Value == "M")
            {
                string date = txtMonth.Text;
                string selected = "M";
                Response.Redirect(string.Format("~/Project/AttendanceReportG.aspx?date={0}&option={1}", date, selected));

            }
            else if (ddlSelect.Items[ddlSelect.SelectedIndex].Value == "W")
            {
                string date = txtDate.Text;
                string selected = "W";
                Response.Redirect(string.Format("~/Project/AttendanceReportG.aspx?date={0}&option={1}", date, selected));
            }



            //if (ddlSelect.Items[ddlSelect.SelectedIndex].Value == "M")
            //{
            //    DateTime date = Convert.ToDateTime(txtMonth.Text);
            //    string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            //    SqlConnection con = new SqlConnection(constr);
            //    SqlCommand cmd = new SqlCommand("SELECT Staff.Name, sum(case when AttendanceStatus = 'Present' then 1 else 0 end) as Present, sum(case when AttendanceStatus = 'Absent' then 1 else 0 end) as Absent, sum(case when AttendanceStatus = 'On Leave' then 1 else 0 end) as OnLeave, sum(case when IsLate = 'Y' then 1 else 0 end) as Late, sum(WorkingHour) as TotalWorkingHour, sum(Overtime) as TotalOvertime, (cast(sum(case when AttendanceStatus = 'Present' then 1 else 0 end) as float) / (cast(sum(case when AttendanceStatus = 'Present' then 1 else 0 end)as float) +cast(sum(case when AttendanceStatus = 'Absent' then 1 else 0 end)as float)))*100 as AttendanceRate,cast(sum(case when IsLate = 'Y' then 1 else 0 end) as float) / cast(sum(case when AttendanceStatus = 'Present' then 1 else 0 end)as float) *100 as LatePercentage, DATENAME(month,AttendanceDate) as Month, Year(AttendanceDate) as Year from Attendance LEFT JOIN STAFF on attendance.StaffID = Staff.Staff_ID WHERE MONTH(AttendanceDate) = MONTH(@date) AND YEAR(AttendanceDate) = Year(@date) GROUP BY StaffID,Name,DATENAME(month,AttendanceDate),Year(AttendanceDate)", con);
            //    cmd.Parameters.AddWithValue("@date", date);
            //    SqlDataAdapter sda = new SqlDataAdapter(cmd);

            //    DataSet ds = new DataSet();
            //    sda.SelectCommand = cmd;
            //    try
            //    {
            //        sda.Fill(ds);
            //    }
            //    catch
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No attendance records found.')", true);
            //    }

            //    ReportDocument crp = new ReportDocument();
            //    crp.Load(Server.MapPath("~/Project/AttendanceReport.rpt"));
            //    crp.SetDataSource(ds.Tables["table"]);
            //    crp.SummaryInfo.ReportTitle = "Monthly Overall Staff Attendance Report ";
            //    CrystalReportViewer1.ReportSource = crp;
            //    crp.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "Staff Attendance");

            //}
            //else if (ddlSelect.Items[ddlSelect.SelectedIndex].Value == "W")
            //{
            //    DateTime startDate = Convert.ToDateTime(txtDate.Text);
            //    DateTime endDate = startDate.AddDays(7);
            //    string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            //    SqlConnection con = new SqlConnection(constr);
            //    SqlCommand cmd = new SqlCommand("SELECT Staff.Name, sum(case when AttendanceStatus = 'Present' then 1 else 0 end) as Present, sum(case when AttendanceStatus = 'Absent' then 1 else 0 end) as Absent, sum(case when AttendanceStatus = 'On Leave' then 1 else 0 end) as OnLeave, sum(case when IsLate = 'Y' then 1 else 0 end) as Late, sum(WorkingHour) as TotalWorkingHour, sum(Overtime) as TotalOvertime, (cast(sum(case when AttendanceStatus = 'Present' then 1 else 0 end) as float) / (cast(sum(case when AttendanceStatus = 'Present' then 1 else 0 end)as float) +cast(sum(case when AttendanceStatus = 'Absent' then 1 else 0 end)as float)))*100 as AttendanceRate,cast(sum(case when IsLate = 'Y' then 1 else 0 end) as float) / cast(sum(case when AttendanceStatus = 'Present' then 1 else 0 end)as float) *100 as LatePercentage from Attendance LEFT JOIN STAFF on attendance.StaffID = Staff.Staff_ID WHERE AttendanceDate between @startDate and @endDate GROUP BY StaffID,Name", con);
            //    cmd.Parameters.AddWithValue("@startDate", startDate);
            //    cmd.Parameters.AddWithValue("@endDate", endDate);
            //    SqlDataAdapter sda = new SqlDataAdapter(cmd);

            //    DataSet ds = new DataSet();
            //    sda.SelectCommand = cmd;
            //    try
            //    {
            //        sda.Fill(ds);
            //    }
            //    catch
            //    {
            //        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('No attendance records found.')", true);
            //    }


            //    ReportDocument crp = new ReportDocument();
            //    crp.Load(Server.MapPath("~/Project/WeekAttendanceReport.rpt"));
            //    crp.SetDataSource(ds.Tables["table"]);
            //    crp.SetParameterValue("StartDate", startDate.ToShortDateString());
            //    crp.SetParameterValue("EndDate", endDate.ToShortDateString());

            //    crp.SummaryInfo.ReportTitle = "Weekly Overall Staff Attendance Report ";
            //    CrystalReportViewer1.ReportSource = crp;

            //    crp.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "Staff Attendance");
            //}



        }

        //protected void ddlSelect1_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    string selectedValue = ddlSelect1.SelectedValue.ToString();

        //    if (selectedValue == "M")
        //    {
        //        txtDate.Enabled = false;
        //        rqvDate.Enabled = false;
        //        rqvDate.Visible = false;

        //        txtMonth.Enabled = true;
        //        rqvMonth.Enabled = true;
        //        rqvMonth.Visible = true;

        //    }
        //    else if (selectedValue == "W")
        //    {
        //        txtDate.Enabled = true;
        //        rqvDate.Enabled = true;
        //        rqvDate.Visible = true;

        //        txtMonth.Enabled = false;
        //        rqvMonth.Enabled = false;
        //        rqvMonth.Visible = false;
        //    }
        //    else
        //    {
        //        txtDate.Enabled = true;
        //        txtMonth.Enabled = true;
        //    }
        //}

        //protected void btnReset_Click(object sender, EventArgs e)
        //{

        //    txtDate.Enabled = true;
        //    txtMonth.Enabled = true;
        //    rqvDate.Enabled = true;
        //    rqvMonth.Enabled = true;
        //    rqvSelect.Enabled = true;
        //    rqvDate.Visible = true;
        //    rqvMonth.Visible = true;
        //    rqvSelect.Visible = true;
        //    txtDate.Text = "";
        //    txtMonth.Text = "";
        //    ddlSelect1.SelectedIndex = 0;
        //    lbl.Text = "Y";
        //}
    }
}