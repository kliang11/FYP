using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FYP.Project
{
    public partial class PayrollReport1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] != null)
            {
                if (Session["resetPW"].ToString() == "yes")
                {
                    Response.Redirect("~/Project/ChangePassword.aspx");
                }
                if (Session["role"].ToString() != "HR Staff")
                {
                    Response.Redirect(string.Format("~/Project/403error.html"));
                }
            }
            else
            {
                Response.Redirect("~/Project/Login.aspx?ReturnUrl=%2fPayrollReport.aspx");
            }
        }

        protected void btnGenerateReport_Click(object sender, EventArgs e)
        {
            string week = txtDate.Text.Trim();
            string month = txtMonth.Text.Trim();
            string date = "";
            DateTime firstDay = new DateTime(1975, 1, 1);
            DateTime lastDay = DateTime.MaxValue;
            string dateForPass = "";
            string title = "";

            if (week != "")
            {
                date = week;
                int year = Int16.Parse(date.Substring(0, date.IndexOf('-')));
                week = date.Substring(date.LastIndexOf('W') + 1);
                firstDay = FirstDateOfWeek(year, Int16.Parse(week), CalendarWeekRule.FirstFullWeek);
                lastDay = firstDay.AddDays(6);
                dateForPass = firstDay.ToShortDateString() + " - " + lastDay.ToShortDateString();
                title = "Weekly Payroll Report";
            }
            else
            {
                date = month;
                firstDay = Convert.ToDateTime(date);
                lastDay = firstDay.AddMonths(1).AddDays(-1);
                dateForPass = firstDay.ToString("MMMM yyyy");
                title = "Monthly Payroll Report";
            }

            string payrollListID = "";

            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("PayrollList_CRUD"))
                {
                    if (txtMonth.Text != "")
                        cmd.Parameters.AddWithValue("@Action", "SELECTFORREPORTMONTH");
                    else
                        cmd.Parameters.AddWithValue("@Action", "SELECTFORREPORTWEEK");
                    cmd.Parameters.AddWithValue("@Date", firstDay);
                    cmd.Parameters.AddWithValue("@DateTwo", lastDay);
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        con.Open();
                        SqlDataReader rd = cmd.ExecuteReader();
                        while (rd.Read())
                        {
                            payrollListID = rd["PayrollListID"].ToString();
                        }
                        con.Close();
                    }
                }
            }            

            if(payrollListID != "")
            {
                constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("Payslip_CRUD"))
                    {
                        cmd.Parameters.AddWithValue("@Action", "SELECTFORREPORT");
                        cmd.Parameters.AddWithValue("@PayrollListID", Int16.Parse(payrollListID));
                        using (SqlDataAdapter sda = new SqlDataAdapter())
                        {
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Connection = con;
                            con.Open();
                            SqlDataReader rd = cmd.ExecuteReader();
                            while (rd.Read())
                            {
                                payrollListID = rd["PayrollListID"].ToString();
                            }
                            con.Close();
                        }
                    }
                }
                if (payrollListID != "")
                {
                    //Response.Redirect(string.Format("~/Project/PayrollReportReal.aspx?payrollListID={0}&date={1}&title={2}", payrollListID, dateForPass, title));
                    string url = string.Format("~/Project/PayrollReportReal.aspx?payrollListID={0}&date={1}&title={2}", payrollListID, dateForPass, title);
                    StringBuilder sb = new StringBuilder();
                    sb.Append("<script type = 'text/javascript'>");
                    sb.Append("window.open('");
                    sb.Append(url);
                    sb.Append("');");
                    sb.Append("</script>");
                    ClientScript.RegisterStartupScript(this.GetType(), "script", sb.ToString());
                }
            }
            else
            {
                Response.Write("<script>alert('No record found. Please choose another date.');</script>");
            }


        }

        private static DateTime FirstDateOfWeek(int year, int weekNum, CalendarWeekRule rule)
        {
            Debug.Assert(weekNum >= 1);

            DateTime jan1 = new DateTime(year, 1, 1);

            int daysOffset = DayOfWeek.Monday - jan1.DayOfWeek;
            DateTime firstMonday = jan1.AddDays(daysOffset);
            Debug.Assert(firstMonday.DayOfWeek == DayOfWeek.Monday);

            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstMonday, rule, DayOfWeek.Monday);

            if (firstWeek <= 1)
            {
                weekNum -= 1;
            }

            DateTime result = firstMonday.AddDays(weekNum * 7);

            return result;
        }


    }
}