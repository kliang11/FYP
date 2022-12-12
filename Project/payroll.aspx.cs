using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Runtime.Serialization;
using System.Globalization;
using System.Threading;
using System.Diagnostics;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace FYP.Project
{
    public partial class payroll : System.Web.UI.Page
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
                        btnAdd.Visible = false;
                        btnLeftWeek.Visible = false;
                        txtSelectWeek.Visible = false;
                        btnRightWeek.Visible = false;
                        btnLeftMonth.Visible = false;
                        txtSelectMonth.Visible = false;
                        btnRightMonth.Visible = false;

                        var cultureInfo = Thread.CurrentThread.CurrentCulture;
                        DateTime currentDate = DateTime.Today;
                        System.Globalization.Calendar cal = cultureInfo.Calendar;
                        int currentWeek = cal.GetWeekOfYear(currentDate, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
                        int currentYear = DateTime.Now.Year;
                        txtSelectWeek.Text = currentYear.ToString() + "-W" + currentWeek.ToString();
                        txtSelectMonth.Text = DateTime.Now.ToString("yyyy-MM");
                        txtDatePopUp.Text = DateTime.Now.ToString("yyyy-MM");
                        loadPayrollList();
                        this.BindGrid(ddlMonthWeek.SelectedValue);
                    }
                    else
                    {
                        Response.Redirect(string.Format("~/Project/403error.html"));
                    }
                }
                else
                {
                    Response.Redirect("~/Project/Login.aspx?ReturnUrl=%2fpayroll.aspx");
                }
            }
        }

        private void loadPayrollList()
        {
            int checkMonth = 0, checkWeek = 0;
            DateTime dateForMonth = DateTime.MinValue;
            string payPeriodForMonth = "";
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("PayrollList_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "SELECTLATESTMONTHLYRECORD");
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        DateTime.TryParse(rd["Date"].ToString(), out dateForMonth);
                        payPeriodForMonth = rd["Payperiod"].ToString();
                        checkMonth = 1;
                    }
                    con.Close();
                }
            }

            DateTime dateForWeek = DateTime.MinValue;
            string payPeriodForWeek = "";
            constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("PayrollList_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "SELECTLATESTWEEKLYRECORD");
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        DateTime.TryParse(rd["Date"].ToString(), out dateForWeek);
                        payPeriodForWeek = rd["Payperiod"].ToString();
                        checkWeek = 1;
                    }
                    con.Close();
                }
            }

            int MonthNotInList = 0, WeekNotInList = 0;
            DateTime dateForMonth2 = dateForMonth, dateForWeek2 = dateForWeek;

            if (checkMonth != 1 && checkWeek == 1)
            {
                constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("Staff_CRUD"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "SELECTSTAFFIDFORPAYROLL");
                        cmd.Parameters.AddWithValue("@PaymentPeriod", "Monthly");
                        cmd.Connection = con;
                        con.Open();
                        SqlDataReader rd = cmd.ExecuteReader();
                        if (rd.Read())
                        {
                            MonthNotInList = 1;
                        }
                        con.Close();
                    }
                }
            }

            if (checkWeek != 1 && checkMonth == 1)
            {
                constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                using (SqlConnection con = new SqlConnection(constr))
                {
                    using (SqlCommand cmd = new SqlCommand("Staff_CRUD"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Action", "SELECTSTAFFIDFORPAYROLL");
                        cmd.Parameters.AddWithValue("@PaymentPeriod", "Weekly");
                        cmd.Connection = con;
                        con.Open();
                        SqlDataReader rd = cmd.ExecuteReader();
                        if (rd.Read())
                        {
                            WeekNotInList = 1;
                        }
                        con.Close();
                    }
                }
            }

            if (checkMonth == 1 || MonthNotInList == 1) //month exist only
            {
                if (MonthNotInList == 1)
                {
                    int week = 0, currentWeeks = 0;
                    week = GetWeekNumber(dateForWeek2);
                    currentWeeks = GetWeekNumber(DateTime.Now);
                    DateTime firstDayofThatWeek = FirstDateOfWeek(dateForWeek2.Year, week, CalendarWeekRule.FirstFullWeek);
                    dateForMonth = firstDayofThatWeek;
                }

                while ((dateForMonth.Year == DateTime.Now.Year && dateForMonth.Month < DateTime.Now.Month) || dateForMonth.Year < DateTime.Now.Year) //your month smaller than month now
                {
                    //update database
                    int payrollListID = 0;
                    payrollListID = GetLatestPayrollListID(payrollListID);
                    bool IsHavePending = false;
                    IsHavePending = checkHavePending(payrollListID);
                    if (IsHavePending)
                    {
                        updatePayrollListStatus(payrollListID, "Overdue");
                    }
                    dateForMonth = dateForMonth.AddMonths(1);
                    string stringDate = dateForMonth.ToString("MMMM yyyy");
                    insertPayrollList(stringDate, dateForMonth, "Monthly");

                    payrollListID = GetLatestPayrollListID(payrollListID);
                    List<int> staffIDList = new List<int>();
                    staffIDList = GetStaffID("Monthly");
                    foreach (int staffID in staffIDList)
                    {
                        CreatePayslip(staffID, payrollListID);
                    }
                }
            }
            if (checkWeek == 1 || WeekNotInList == 1) //week exist only
            {
                int week = 0, currentWeeks = 0;
                week = GetWeekNumber(dateForWeek);
                currentWeeks = GetWeekNumber(DateTime.Now);
                DateTime firstDayofThatWeek = FirstDateOfWeek(dateForWeek.Year, week, CalendarWeekRule.FirstFullWeek);

                if (WeekNotInList == 1)
                {
                    week = GetWeekNumber(dateForMonth2);
                    firstDayofThatWeek = FirstDateOfWeek(dateForMonth2.Year, week, CalendarWeekRule.FirstFullWeek);
                }

                while ((firstDayofThatWeek.Year == DateTime.Now.Year && week < currentWeeks) || firstDayofThatWeek.Year < DateTime.Now.Year) //your week smaller than week now
                {
                    //update database
                    int payrollListID = 0;
                    payrollListID = GetLatestPayrollListID(payrollListID);
                    bool IsHavePending = false;
                    IsHavePending = checkHavePending(payrollListID);
                    if (IsHavePending)
                    {
                        updatePayrollListStatus(payrollListID, "Overdue");
                    }
                    firstDayofThatWeek = firstDayofThatWeek.AddDays(7);
                    week += 1;
                    string stringDate = firstDayofThatWeek.ToShortDateString().ToString() + " - " + ToDateTime(firstDayofThatWeek.Year, week, DayOfWeek.Sunday).ToShortDateString().ToString();
                    insertPayrollList(stringDate, firstDayofThatWeek, "Weekly");

                    payrollListID = GetLatestPayrollListID(payrollListID);
                    List<int> staffIDList = new List<int>();
                    staffIDList = GetStaffID("Weekly");
                    foreach (int staffID in staffIDList)
                    {
                        CreatePayslip(staffID, payrollListID);
                    }
                }
            }

            //latest month or week update pending   /SELECTLATESTIDPAYPERIOD
            int idforMonth = GetLatestPayrollListIDByPayPeriod("Monthly");
            int idforWeek = GetLatestPayrollListIDByPayPeriod("Weekly");
            bool IsPending = false;
            if (idforMonth != 0)
            {
                IsPending = checkHavePending(idforMonth);
                if (IsPending)
                    updatePayrollListStatus(idforMonth, "Pending");
            }
            if (idforWeek != 0)
            {
                IsPending = checkHavePending(idforWeek);
                if (IsPending)
                    updatePayrollListStatus(idforWeek, "Pending");
            }
        }

        private void updatePayrollListStatus(int id, string status)
        {            
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("PayrollList_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "UPDATE");
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@PayrollListID", id);
                    cmd.Connection = con;
                    con.Open();
                    int a = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        private bool checkHavePending(int id)
        {
            bool IsHavePending = false;
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Payslip_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "SELECT");
                    cmd.Parameters.AddWithValue("@PayrollListID", id);
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        if (rd["ProcessStatus"].ToString() == "Pending")
                        {
                            IsHavePending = true;
                        }
                    }
                    con.Close();
                }
            }
            return IsHavePending;
        }

        

        










        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != gvList.EditIndex)
            {
                if ((e.Row.FindControl("lblStatus") as Label).Text.Equals("Processed"))
                    (e.Row.FindControl("lblStatus") as Label).CssClass = "label_green";
                else if ((e.Row.FindControl("lblStatus") as Label).Text.Equals("Pending"))
                    (e.Row.FindControl("lblStatus") as Label).CssClass = "label_yellow";
                else if ((e.Row.FindControl("lblStatus") as Label).Text.Equals("Overdue"))
                    (e.Row.FindControl("lblStatus") as Label).CssClass = "label_redAbsent";
            }
        }

        //==========================================================================================
        protected void minusMonth(object sender, EventArgs e)
        {
            DateTime date = Convert.ToDateTime(txtSelectMonth.Text).AddMonths(-1);
            txtSelectMonth.Text = date.ToString("yyyy-MM");
            this.BindGrid("selectMonth");
        }
        protected void addMonth(object sender, EventArgs e)
        {
            DateTime date = Convert.ToDateTime(txtSelectMonth.Text).AddMonths(1);
            txtSelectMonth.Text = date.ToString("yyyy-MM");
            this.BindGrid("selectMonth");
        }
        protected void txtSelectMonth_TextChanged(object sender, EventArgs e)
        {
            this.BindGrid("selectMonth");
        }
        protected void minusWeek(object sender, EventArgs e)
        {
            string str = txtSelectWeek.Text.ToString();
            int year = Int16.Parse(str.Substring(0, str.IndexOf('-')));
            string week = str.Substring(str.LastIndexOf('W') + 1);
            week = (Int16.Parse(week) - Int16.Parse("1")).ToString();
            if (Int16.Parse(week) < 10)
            {
                week = 0 + week;
            }
            if (year != 2021 && year != 2027 && Int16.Parse(week) > 52)
            {
                year -= 1;
                week = "52";
            }
            else if ((year == 2021 || year == 2027) && Int16.Parse(week) < 1)
            {
                year -= 1;
                week = "53";
            }
            txtSelectWeek.Text = year.ToString() + "-W" + week.ToString();
            this.BindGrid("selectWeek");
        }
        protected void addWeek(object sender, EventArgs e)
        {
            string str = txtSelectWeek.Text.ToString();
            int year = Int16.Parse(str.Substring(0, str.IndexOf('-')));
            string week = str.Substring(str.LastIndexOf('W') + 1);
            week = (Int16.Parse(week) + Int16.Parse("1")).ToString();
            if (Int16.Parse(week) < 10)
            {
                week = 0 + week;
            }
            if (year != 2020 && year != 2026 && Int16.Parse(week) > 52)
            {
                year += 1;
                week = "01";
            }
            else if ((year == 2020 || year == 2026) && Int16.Parse(week) > 53)
            {
                year += 1;
                week = "01";
            }
            txtSelectWeek.Text = year.ToString() + "-W" + week.ToString();
            this.BindGrid("selectWeek");
        }
        protected void txtSelectWeek_TextChanged(object sender, EventArgs e)
        {
            this.BindGrid("selectWeek");
        }
        //==========================================================================================

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
        //DateTime firstDayofThatWeek = FirstDateOfWeek(currentYear, GetWeekNumber(date), CalendarWeekRule.FirstFullWeek);

        private static DateTime ToDateTime(int year, int week, DayOfWeek dayOfWeek)
        {
            //if (year < MinYear || year > MaxYear)
            //{
            //    throw new ArgumentOutOfRangeException(nameof(year), SR.ArgumentOutOfRange_Year);
            //}

            //if (week < MinWeek || week > MaxWeek)
            //{
            //    throw new ArgumentOutOfRangeException(nameof(week), SR.ArgumentOutOfRange_Week_ISO);
            //}

            // We allow 7 for convenience in cases where a user already has a valid ISO
            // day of week value for Sunday. This means that both 0 and 7 will map to Sunday.
            // The GetWeekday method will normalize this into the 1-7 range required by ISO.
            //if ((int)dayOfWeek < 0 || (int)dayOfWeek > 7)
            //{
            //    throw new ArgumentOutOfRangeException(nameof(dayOfWeek), SR.ArgumentOutOfRange_DayOfWeek);
            //}

            var jan4 = new DateTime(year, month: 1, day: 4);

            int correction = GetWeekday(jan4.DayOfWeek) + 3;

            int ordinal = (week * 7) + GetWeekday(dayOfWeek) - correction;

            return new DateTime(year, month: 1, day: 1).AddDays(ordinal - 1);
        }
        private static int GetWeekday(DayOfWeek dayOfWeek)
        {
            return dayOfWeek == DayOfWeek.Sunday ? 7 : (int)dayOfWeek;
        }
        private static int GetWeekNumber(DateTime date)
        {
            return (date.DayOfYear - GetWeekday(date.DayOfWeek) + 10) / 7;
        }

        private void BindGrid(string action)
        {
            DateTime firstDay= new DateTime(1975, 1, 1);
            DateTime lastDay = DateTime.MaxValue;
            string day1 = "";
            string day2 = "";

            if (action == "All")
            {
                action = "SELECTALL";
            }
            else if (action == "Monthly")
            {
                action = "SELECTMONTH";
            }
            else if (action == "Weekly")
            {
                action = "SELECTWEEK";
            }
            else if (action == "selectWeek")
            {
                action = "SELECTCERTAINDATE";
                string str = txtSelectWeek.Text.ToString();
                int year = Int16.Parse(str.Substring(0, str.IndexOf('-')));
                string week = str.Substring(str.LastIndexOf('W') + 1);
                firstDay = FirstDateOfWeek(year, Int16.Parse(week), CalendarWeekRule.FirstFullWeek);
                lastDay = firstDay.AddDays(6);
                day1 = firstDay.ToString("yyyy/M/d");
                day2 = lastDay.ToString("yyyy/M/d");
                firstDay = DateTime.ParseExact(day1, "yyyy/M/d", CultureInfo.InvariantCulture);
                lastDay = DateTime.ParseExact(day2, "yyyy/M/d", CultureInfo.InvariantCulture);
            }
            else if (action == "selectMonth")
            {
                action = "SELECTCERTAINDATE";
                firstDay = Convert.ToDateTime(txtSelectMonth.Text);
                lastDay = firstDay.AddMonths(1).AddDays(-1);
                day1 = firstDay.ToString("yyyy/M/d");
                day2 = lastDay.ToString("yyyy/M/d");
                firstDay = DateTime.ParseExact(day1, "yyyy/M/d", CultureInfo.InvariantCulture);
                lastDay = DateTime.ParseExact(day2, "yyyy/M/d", CultureInfo.InvariantCulture);
            }

            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("PayrollList_CRUD"))
                {
                    cmd.Parameters.AddWithValue("@Action", action);
                    cmd.Parameters.AddWithValue("@Date", firstDay);
                    cmd.Parameters.AddWithValue("@DateTwo", lastDay);
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
                            else
                            {
                                btnAdd.Visible = true;
                            }
                        }
                    }
                }
            }
        }

        protected void ddlMonthWeek_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlMonthWeek.SelectedValue == "Monthly")
            {
                btnLeftWeek.Visible = false;
                txtSelectWeek.Visible = false;
                btnRightWeek.Visible = false;
                btnLeftMonth.Visible = true;
                txtSelectMonth.Visible = true;
                btnRightMonth.Visible = true;
            }
            else if (ddlMonthWeek.SelectedValue == "Weekly")
            {
                btnLeftWeek.Visible = true;
                txtSelectWeek.Visible = true;
                btnRightWeek.Visible = true;
                btnLeftMonth.Visible = false;
                txtSelectMonth.Visible = false;
                btnRightMonth.Visible = false;
            }
            else
            {
                btnLeftWeek.Visible = false;
                txtSelectWeek.Visible = false;
                btnRightWeek.Visible = false;
                btnLeftMonth.Visible = false;
                txtSelectMonth.Visible = false;
                btnRightMonth.Visible = false;
            }
            this.BindGrid(ddlMonthWeek.SelectedValue);
        }

        protected void ddlPayperiodPopUp_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPayperiodPopUp.SelectedValue == "Monthly")
            {
                txtDatePopUp.TextMode = TextBoxMode.Month;
                txtDatePopUp.Text = DateTime.Now.ToString("yyyy-MM");
            }
            else
            {
                var cultureInfo = Thread.CurrentThread.CurrentCulture;
                DateTime currentDate = DateTime.Today;
                System.Globalization.Calendar cal = cultureInfo.Calendar;
                int currentWeek = cal.GetWeekOfYear(currentDate, CalendarWeekRule.FirstFullWeek, DayOfWeek.Monday);
                int currentYear = DateTime.Now.Year;

                txtDatePopUp.TextMode = TextBoxMode.Week;
                txtDatePopUp.Text = currentYear.ToString() + "-W" + currentWeek.ToString();
            }
        }

        protected void gvList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label a = (Label)gvList.SelectedRow.FindControl("lblPayrollListID");
            string payrollListID = a.Text.ToString();
            Label b = (Label)gvList.SelectedRow.FindControl("lblPayperiod");
            string payPeriod = b.Text.ToString();
            Label c = (Label)gvList.SelectedRow.FindControl("lblRealDate");
            string date = c.Text.ToString();
            Response.Redirect(string.Format("~/Project/PayrollDetailList.aspx?id={0}&payperiod={1}&date={2}", payrollListID, payPeriod, date));
        }

        protected void btnAdd_Click(object sender, EventArgs e)
        {
            addPayrollListPopup.Visible = true;
            resetDefault();
            this.BindGrid(ddlMonthWeek.SelectedValue);
        }

        protected void btnRejectClose_Click(object sender, EventArgs e)
        {
            addPayrollListPopup.Visible = false;
            resetDefault();
            this.BindGrid(ddlMonthWeek.SelectedValue);
        }

        private void resetDefault()
        {
            txtDatePopUp.Text = DateTime.Now.ToString("yyyy-MM");
            txtDatePopUp.Attributes["style"] = "borderColor: \"\"; display:none";
            lbl_DatePopUp.Attributes["style"] = "display:none;";
            ddlPayperiodPopUp.SelectedValue = "Monthly";
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string outputMsg = "";
            DateTime date;
            string stringDate = "";
            if (txtDatePopUp.TextMode == TextBoxMode.Month)
            {
                string abc = txtDatePopUp.Text.Trim() + "-01";
                abc = Convert.ToDateTime(abc).ToString("dd-MM-yyyy");
                date = DateTime.Parse(abc);
                stringDate = date.ToString("MMMM yyyy");
            }
            else
            {
                string abc = txtDatePopUp.Text.Trim();
                int year = Int16.Parse(abc.Substring(0, abc.IndexOf('-')));
                int week = Int16.Parse(abc.Substring(abc.LastIndexOf('W') + 1));
                date = ToDateTime(year, week, DayOfWeek.Monday);
                stringDate = date.ToShortDateString().ToString() + " - " + ToDateTime(year, week, DayOfWeek.Sunday).ToShortDateString().ToString();
            }

            outputMsg = insertPayrollList(stringDate, date, ddlPayperiodPopUp.SelectedValue.Trim());

            //Create payslip database           
            int payrollListID = 0;
            payrollListID = GetLatestPayrollListID(payrollListID);
            List<int> staffIDList = new List<int>();
            staffIDList = GetStaffID(ddlPayperiodPopUp.SelectedValue.Trim());
            foreach (int staffID in staffIDList)
            {
                CreatePayslip(staffID, payrollListID);
            }
            resetDefault();
            addPayrollListPopup.Visible = false;
            btnAdd.Visible = false;
            if (outputMsg == "")
                outputMsg = "Payroll List Created Successfully.";
            ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('" + outputMsg + "');", true);
            this.BindGrid(ddlMonthWeek.SelectedValue);
        }

        private string insertPayrollList(string stringDate, DateTime date, string payPeriod)
        {
            string outputMsg = "";
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("PayrollList_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "INSERT");
                    cmd.Parameters.AddWithValue("@StringDate", stringDate);
                    cmd.Parameters.AddWithValue("@Status", "Pending");
                    cmd.Parameters.AddWithValue("@Date", date);
                    cmd.Parameters.AddWithValue("@Payperiod", payPeriod);
                    cmd.Connection = con;
                    con.Open();
                    int a = cmd.ExecuteNonQuery();
                    if (a == 0)
                    {
                        outputMsg = "Upload Failed. Please Try Again.";
                    }
                    con.Close();
                }
            }
            return outputMsg;
        }

        private void CreatePayslip(int staffID, int payrollListID)
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Payslip_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "INSERT");
                    cmd.Parameters.AddWithValue("@ProcessStatus", "Pending");
                    cmd.Parameters.AddWithValue("@Staff_ID", staffID);
                    cmd.Parameters.AddWithValue("@PayrollListID", payrollListID);
                    cmd.Connection = con;
                    con.Open();
                    int a = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        private List<int> GetStaffID(string payPeriod)
        {
            List<int> idList = new List<int>();
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Staff_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "SELECTSTAFFIDFORPAYROLL");
                    cmd.Parameters.AddWithValue("@PaymentPeriod", payPeriod);
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        int abc = Int16.Parse(rd["Staff_ID"].ToString());
                        idList.Add(abc);
                    }
                    con.Close();
                }
            }
            return idList;
        }

        private int GetLatestPayrollListID(int payrollListID)
        {
            int id = 0;
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("PayrollList_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "SELECTLATESTID");
                    cmd.Connection = con;
                    con.Open();
                    id = (int)cmd.ExecuteScalar();
                    con.Close();
                }
            }
            return id;
        }

        private int GetLatestPayrollListIDByPayPeriod(string payperiod)
        {
            int id = 0;
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("PayrollList_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "SELECTLATESTIDPAYPERIOD");
                    cmd.Parameters.AddWithValue("@Payperiod", payperiod);
                    cmd.Connection = con;
                    con.Open();
                    var abc = cmd.ExecuteScalar();
                    if (abc != null)
                        id = (int)abc;
                    con.Close();
                }
            }
            return id;
        }
    }
}