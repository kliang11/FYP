using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FYP.Project
{
    public partial class PayrollDetailList : System.Web.UI.Page
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
                        ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                        string id = Request.QueryString["id"];
                        string payperiod = Request.QueryString["payperiod"];
                        string date = Request.QueryString["date"];
                        if (id != null && payperiod != null && date != null)
                        {
                            DateTime dateforlabel = Convert.ToDateTime(date);
                            string strDate = "";
                            if (payperiod == "Weekly")
                                strDate = dateforlabel.ToShortDateString() + " - " + dateforlabel.AddDays(6).ToShortDateString();
                            else
                                strDate = dateforlabel.AddMonths(1).AddDays(-1).ToString("MMMM yyyy");
                            lblTitle.Text = lblTitle.Text + strDate.ToString();
                            List<int> staffIDList = new List<int>();
                            List<int> payslipIDList = new List<int>();
                            List<String> checkIsUpdate = new List<String>();
                            string paymentMethod = ""; double basicSalary = 0.0, unpaidLeaveSalary = 0.0; int i = 0;
                            //update staff info to payslip
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
                                        int staffID = Int16.Parse(rd["Staff_ID"].ToString());
                                        staffIDList.Add(staffID);
                                        int payslipID = Int16.Parse(rd["PayslipID"].ToString());
                                        payslipIDList.Add(payslipID);
                                        string IsUpdate = rd["NetSalary"].ToString();
                                        checkIsUpdate.Add(IsUpdate);
                                    }
                                    con.Close();
                                }
                            }

                            foreach (int staffID in staffIDList)
                            {
                                if (checkIsUpdate[i].ToString() == "")
                                {
                                    constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                                    using (SqlConnection con = new SqlConnection(constr))
                                    {
                                        using (SqlCommand cmd = new SqlCommand("Staff_CRUD"))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@Action", "SELECTSTAFF");
                                            cmd.Parameters.AddWithValue("@Staff_ID", staffID);
                                            cmd.Connection = con;
                                            con.Open();
                                            SqlDataReader rd = cmd.ExecuteReader();
                                            while (rd.Read())
                                            {
                                                paymentMethod = rd["PaymentMethod"].ToString();
                                                basicSalary = Convert.ToDouble(rd["Salary"].ToString());
                                            }
                                            con.Close();
                                        }
                                    }

                                    int totalWorkingDay = 0, IntPresent = 0, IntUnpaidLeave = 0;
                                    string unpaidleave = "";
                                    DateTime firstDay = new DateTime(1975, 1, 1);
                                    DateTime lastDay = DateTime.MaxValue;
                                    if (payperiod == "Weekly")
                                    {
                                        firstDay = DateTime.Parse(date);
                                        lastDay = firstDay.AddDays(6);
                                    }
                                    else
                                    {
                                        firstDay = DateTime.Parse(date);
                                        lastDay = firstDay.AddMonths(1).AddDays(-1);
                                    }
                                    constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                                    using (SqlConnection con = new SqlConnection(constr))
                                    {
                                        using (SqlCommand cmd = new SqlCommand("Attendance_CRUD"))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@Action", "COUNT");
                                            cmd.Parameters.AddWithValue("@Staff_ID", staffID);
                                            cmd.Parameters.AddWithValue("@Date1", firstDay);
                                            cmd.Parameters.AddWithValue("@Date2", lastDay);
                                            cmd.Connection = con;
                                            con.Open();
                                            SqlDataReader rd = cmd.ExecuteReader();
                                            while (rd.Read())
                                            {
                                                string present = rd["Present"].ToString();
                                                unpaidleave = rd["Absent"].ToString();
                                                if (present != "")
                                                    IntPresent = Int16.Parse(present);
                                                if (unpaidleave != "")
                                                    IntUnpaidLeave = Int16.Parse(unpaidleave);

                                                totalWorkingDay = IntPresent + IntUnpaidLeave;
                                            }
                                            con.Close();
                                        }
                                    }
                                    if (totalWorkingDay != 0)
                                        unpaidLeaveSalary = (IntUnpaidLeave / totalWorkingDay) * basicSalary;
                                    else
                                    {
                                        basicSalary = 0;
                                    }

                                    constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                                    using (SqlConnection con = new SqlConnection(constr))
                                    {
                                        using (SqlCommand cmd = new SqlCommand("Payslip_CRUD"))
                                        {
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@Action", "UPDATE");
                                            cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                                            cmd.Parameters.AddWithValue("@BasicSalary", basicSalary);
                                            cmd.Parameters.AddWithValue("@UnpaidLeaveSalary", unpaidLeaveSalary);
                                            cmd.Parameters.AddWithValue("@PayslipID", payslipIDList[i]);
                                            cmd.Connection = con;
                                            con.Open();
                                            int a = cmd.ExecuteNonQuery();
                                            con.Close();
                                        }
                                    }
                                }
                                i++;
                            }

                            List<String> checkIsAllProcess = new List<String>();
                            bool IsAllProcess = true; int IntIsAllProcess = 0;
                            constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
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
                                        string status = rd["ProcessStatus"].ToString();
                                        checkIsAllProcess.Add(status);
                                    }
                                    con.Close();
                                }
                            }
                            foreach (string status in checkIsAllProcess)
                            {
                                if (status != "Processed" && IntIsAllProcess == 0)
                                {
                                    IsAllProcess = false;
                                    IntIsAllProcess++;
                                }
                            }

                            if (IsAllProcess)
                            {
                                constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                                using (SqlConnection con = new SqlConnection(constr))
                                {
                                    using (SqlCommand cmd = new SqlCommand("PayrollList_CRUD"))
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@Action", "UPDATE");
                                        cmd.Parameters.AddWithValue("@Status", "Processed");
                                        cmd.Parameters.AddWithValue("@PayrollListID", id);
                                        cmd.Connection = con;
                                        con.Open();
                                        int a = cmd.ExecuteNonQuery();
                                        con.Close();
                                    }
                                }
                            }
                            this.BindGrid(Int16.Parse(id));
                        }
                    }
                    else
                    {
                        Response.Redirect(string.Format("~/Project/403error.html"));
                    }
                }
                else
                {
                    Response.Redirect("~/Project/Login.aspx?ReturnUrl=%2fPayrollDetailList.aspx");
                }
            }
        }


        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != gvList.EditIndex)
            {
                if ((e.Row.FindControl("lblProcessStatus") as Label).Text.Equals("Processed"))
                {
                    (e.Row.FindControl("lblProcessStatus") as Label).CssClass = "label_green";
                    (e.Row.FindControl("btnProcess") as ImageButton).Visible = false;
                    (e.Row.FindControl("btnEdit") as ImageButton).Visible = false;
                    (e.Row.FindControl("btnViewDetail") as ImageButton).Visible = true;
                    (e.Row.FindControl("txtBonus") as TextBox).Visible = false;
                    (e.Row.FindControl("lblBonus") as Label).Visible = true;
                }
                else if ((e.Row.FindControl("lblProcessStatus") as Label).Text.Equals("Pending"))
                {
                    (e.Row.FindControl("lblProcessStatus") as Label).CssClass = "label_yellow";
                    (e.Row.FindControl("btnProcess") as ImageButton).Visible = true;
                    (e.Row.FindControl("btnEdit") as ImageButton).Visible = true;
                    (e.Row.FindControl("btnViewDetail") as ImageButton).Visible = false;
                    (e.Row.FindControl("txtBonus") as TextBox).Visible = true;
                    (e.Row.FindControl("lblBonus") as Label).Visible = false;
                }
                else if ((e.Row.FindControl("lblProcessStatus") as Label).Text.Equals("Overdue"))
                {
                    (e.Row.FindControl("lblProcessStatus") as Label).CssClass = "label_redAbsent";
                    (e.Row.FindControl("btnProcess") as ImageButton).Visible = true;
                    (e.Row.FindControl("btnEdit") as ImageButton).Visible = true;
                    (e.Row.FindControl("btnViewDetail") as ImageButton).Visible = false;
                    (e.Row.FindControl("txtBonus") as TextBox).Visible = true;
                    (e.Row.FindControl("lblBonus") as Label).Visible = false;
                }

                //if ((e.Row.FindControl("lblSendStatus") as Label).Text.Equals("Sent"))
                //{
                //    (e.Row.FindControl("lblSendStatus") as Label).CssClass = "label_green";
                //    (e.Row.FindControl("btnSend") as ImageButton).Visible = false;
                //}
                //else if ((e.Row.FindControl("lblSendStatus") as Label).Text.Equals("Pending"))
                //{
                //    (e.Row.FindControl("lblSendStatus") as Label).CssClass = "label_yellow";
                //    (e.Row.FindControl("btnSend") as ImageButton).Visible = true;
                //}
                //else if ((e.Row.FindControl("lblSendStatus") as Label).Text.Equals("Waiting"))
                //{
                //    (e.Row.FindControl("lblSendStatus") as Label).CssClass = "label_grey";
                //    (e.Row.FindControl("btnSend") as ImageButton).Visible = false;
                //}

                if ((e.Row.FindControl("txtBonus") as TextBox).Text.Equals(""))
                {
                    (e.Row.FindControl("txtBonus") as TextBox).Text = "0.0";
                }
                if ((e.Row.FindControl("lblUnpaidLeaveSalary") as Label).Text.Equals(""))
                {
                    (e.Row.FindControl("lblUnpaidLeaveSalary") as Label).Text = "0.0";
                }
            }
        }

        protected void FireRowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridViewRow row = (GridViewRow)(((ImageButton)e.CommandSource).NamingContainer);
            int id = (int)gvList.DataKeys[row.RowIndex].Value; //payslip id
            //int rowIndex = Convert.ToInt32(e.CommandArgument);
            GridViewRow rows = gvList.Rows[row.RowIndex];
            double salary = Convert.ToDouble((rows.FindControl("lblBasicSalary") as Label).Text);
            double bonus = Convert.ToDouble((rows.FindControl("txtBonus") as TextBox).Text);
            double unpaidLeaveSalary = Convert.ToDouble((rows.FindControl("lblUnpaidLeaveSalary") as Label).Text);
            string payperiod = Request.QueryString["payperiod"];

            string command = e.CommandName;
            switch (command)
            {
                case "Processing":
                    {
                        int staffID = getStaffID(id);
                        double hourlyRate = 0.0, overtimeRate = 0.0, employerEpfRate = 0.0, employeeEpfRate = 0.0;
                        string paymentMethod = "", socsoCategory = "", eisContribution = "", taxStatus = "", jobType = "",
                            selfDisable = "", spouseDisable = "", spouseWorking = "", maritalStatus = "";
                        int numChild = 0, numberOfLate = 0;

                        string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand("Staff_CRUD"))
                            {
                                cmd.Parameters.AddWithValue("@Action", "SELECTSTAFF");
                                cmd.Parameters.AddWithValue("@Staff_ID", staffID);
                                using (SqlDataAdapter sda = new SqlDataAdapter())
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Connection = con;
                                    con.Open();
                                    SqlDataReader rd = cmd.ExecuteReader();
                                    while (rd.Read())
                                    {
                                        paymentMethod = rd["PaymentMethod"].ToString();
                                        hourlyRate = Convert.ToDouble(rd["HourlyRate"].ToString());
                                        overtimeRate = Convert.ToDouble(rd["OvertimeRate"].ToString());
                                        socsoCategory = rd["SocsoCategory"].ToString();
                                        eisContribution = rd["EisContribution"].ToString();
                                        employerEpfRate = Convert.ToDouble(rd["EmployerEpfRate"].ToString());
                                        employeeEpfRate = Convert.ToDouble(rd["EmployeeEpfRate"].ToString());
                                        taxStatus = rd["TaxStatus"].ToString();
                                        jobType = rd["JobType"].ToString();
                                        staffID = (int)rd["Staff_ID"];
                                        selfDisable = rd["SelfDisable"].ToString();
                                        spouseDisable = rd["SpouseDisable"].ToString();
                                        numChild = (int)rd["NoChild"];
                                        spouseWorking = rd["SpouseWorking"].ToString();
                                        maritalStatus = rd["MaritalStatus"].ToString();
                                    }
                                    con.Close();
                                }
                            }
                        }

                        double totalEpfPaid = 0.0, totalPcbPaid = 0.0, totalSalaryGet = 0.0;
                        totalEpfPaid = getTotalEpfPaid(staffID);
                        totalPcbPaid = getTotalPcbPaid(staffID);

                        string abc = "", xyz = "", qwe = "";
                        constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand("Payslip_CRUD"))
                            {
                                cmd.Parameters.AddWithValue("@Action", "VALUEGETBEFORE");
                                cmd.Parameters.AddWithValue("@Staff_ID", staffID);
                                using (SqlDataAdapter sda = new SqlDataAdapter())
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Connection = con;
                                    con.Open();
                                    SqlDataReader rd = cmd.ExecuteReader();
                                    while (rd.Read())
                                    {
                                        abc = rd["Epf"].ToString();
                                        xyz = rd["Pcb"].ToString();
                                        qwe = rd["Salary"].ToString();
                                    }
                                    con.Close();
                                }
                            }
                        }
                        if (abc != "" && abc != null)                        
                            totalEpfPaid = Convert.ToDouble(abc);                        
                        if (xyz != "" && xyz != null)                        
                            totalPcbPaid = Convert.ToDouble(xyz);                        
                        if (qwe != "" && qwe != null)                        
                            totalSalaryGet = Convert.ToDouble(qwe);

                        abc = "";
                        constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand("Payslip_CRUD"))
                            {
                                cmd.Parameters.AddWithValue("@Action", "LATESTSALARY");
                                cmd.Parameters.AddWithValue("@Staff_ID", staffID);
                                using (SqlDataAdapter sda = new SqlDataAdapter())
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Connection = con;
                                    con.Open();
                                    SqlDataReader rd = cmd.ExecuteReader();
                                    while (rd.Read())
                                    {
                                        abc = rd["BasicSalary"].ToString();                                        
                                    }
                                    con.Close();
                                }
                            }
                        }
                        if (abc != "" && abc != null)
                            totalSalaryGet = totalSalaryGet - Convert.ToDouble(abc);

                        //calculation
                        bool firstCategory = false;
                        double pcb = 0.0, lateDeduction = 0.0, totalWorkingHour = 0.0, totalOvertime = 0.0, totalClaimAmount = 0.0;
                        bool isSelfDisable = false, isSpouseDisable = false, IsSpouseWorking = false, isSingle = false, IsAdditionalRemuneration = false;
                        string date = Request.QueryString["date"];
                        DateTime month = new DateTime(1975, 1, 1);
                        month = DateTime.Parse(date);
                        date = month.ToString("MMMM");

                        DateTime firstDay = new DateTime(1975, 1, 1);
                        DateTime lastDay = DateTime.MaxValue;
                        if (payperiod == "Weekly")
                        {
                            firstDay = month;
                            lastDay = firstDay.AddDays(6);
                        }
                        else
                        {
                            firstDay = month;
                            lastDay = firstDay.AddMonths(1).AddDays(-1);
                        }

                        constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand("Attendance_CRUD"))
                            {
                                cmd.Parameters.AddWithValue("@Action", "COUNT_HOUR");
                                cmd.Parameters.AddWithValue("@Staff_ID", staffID);
                                cmd.Parameters.AddWithValue("@date1 ", firstDay);
                                cmd.Parameters.AddWithValue("@date2", lastDay);
                                using (SqlDataAdapter sda = new SqlDataAdapter())
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Connection = con;
                                    con.Open();
                                    SqlDataReader rd = cmd.ExecuteReader();
                                    while (rd.Read())
                                    {
                                        string aaa = rd["TotalWorkingHour"].ToString();
                                        string bbb = rd["TotalOvertime"].ToString();
                                        if(aaa!="")
                                            totalWorkingHour = Convert.ToDouble(abc);
                                        if (bbb != "")
                                            totalOvertime = Convert.ToDouble(bbb);
                                    }
                                    con.Close();
                                }
                            }
                        }
                        totalWorkingHour = totalWorkingHour * hourlyRate;
                        totalOvertime = totalOvertime * overtimeRate;

                        if (jobType == "Permenant")
                        {
                            //late working amount = number of late x RM5
                            //salary - late working amount
                            //hour x overtimeRate
                            //sum and get salary
                            constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                            using (SqlConnection con = new SqlConnection(constr))
                            {
                                using (SqlCommand cmd = new SqlCommand("Attendance_CRUD"))
                                {
                                    cmd.Parameters.AddWithValue("@Action", "COUNT_LATE");
                                    cmd.Parameters.AddWithValue("@Staff_ID", staffID);
                                    cmd.Parameters.AddWithValue("@date1 ", firstDay);
                                    cmd.Parameters.AddWithValue("@date2", lastDay);
                                    using (SqlDataAdapter sda = new SqlDataAdapter())
                                    {
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Connection = con;
                                        con.Open();
                                        SqlDataReader rd = cmd.ExecuteReader();
                                        while (rd.Read())
                                        {
                                            string abcd  = rd["LateCount"].ToString();
                                            if (abcd != "")
                                                numberOfLate = Int16.Parse(abc);
                                        }
                                        con.Close();
                                    }
                                }
                            }
                            lateDeduction = numberOfLate * 5;
                            totalWorkingHour = 0.0;
                        }
                        if (salary == 0)
                            salary = totalWorkingHour;
                        EpfCalculation(salary, employeeEpfRate, employerEpfRate);  //EPF
                        if (socsoCategory != "No Contribution")  //SOCSO
                        {
                            if (socsoCategory == "Employment Injury & Invalidity")
                            {
                                firstCategory = true;
                            }
                            SocsoCalculation(firstCategory, salary);
                        }
                        if (eisContribution == "Yes")   //EIS
                        {
                            EisCalculation(salary);
                        }
                        if (taxStatus != "No Contribution")  //TAX
                        {
                            if (taxStatus == "Tax Resident")
                                pcb = salary * 0.3;
                            if (selfDisable == "Yes")
                                isSelfDisable = true;
                            if (spouseDisable == "Yes")
                                isSpouseDisable = true;
                            if (maritalStatus == "Single")
                                isSingle = true;
                            else if (maritalStatus == "Married")
                                isSingle = false;
                            else
                                isSingle = false;
                            if (spouseWorking == "Yes")
                                IsSpouseWorking = true;
                            if (bonus != 0.0)
                                IsAdditionalRemuneration = true;
                            pcb = TaxCalculation(salary, isSelfDisable, isSpouseDisable, IsSpouseWorking, isSingle, IsAdditionalRemuneration, date, totalEpfPaid, numChild, totalPcbPaid, totalSalaryGet, bonus);
                        }

                        constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand("Claim_CRUD"))
                            {
                                cmd.Parameters.AddWithValue("@Action", "TOTALCLAIMAMOUNT");
                                cmd.Parameters.AddWithValue("@Staff_ID", staffID);
                                using (SqlDataAdapter sda = new SqlDataAdapter())
                                {
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Connection = con;
                                    con.Open();
                                    SqlDataReader rd = cmd.ExecuteReader();
                                    while (rd.Read())
                                    {
                                        string temp = rd["TotalClaimAmount"].ToString();
                                        if (temp != "")
                                            totalClaimAmount = Convert.ToDouble(temp);
                                    }
                                    con.Close();
                                }
                            }
                        }

                        //Update claim receive status wiht (id+firstDay)
                        constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                        using (SqlConnection con = new SqlConnection(constr))
                        {
                            using (SqlCommand cmd = new SqlCommand("Claim_CRUD"))
                            {
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@Action", "UPDATECLAIMRECEIVE");
                                cmd.Parameters.AddWithValue("@Staff_ID", staffID);
                                cmd.Parameters.AddWithValue("@ClaimReceiveDate", staffID + id);
                                cmd.Connection = con;
                                con.Open();
                                int a = cmd.ExecuteNonQuery();
                                con.Close();
                            }
                        }
                        if (lblEmployeeEPF.Text.Trim() == "")
                            lblEmployeeEPF.Text = "0.0";
                        if (lblEmployeeSocso.Text.Trim() == "")
                            lblEmployeeSocso.Text = "0.0";
                        if (lblEmployeeEIS.Text.Trim() == "")
                            lblEmployeeEIS.Text = "0.0";
                        if (lblEmployerEPF.Text.Trim() == "")
                            lblEmployerEPF.Text = "0.0";
                        if (lblEmployerSocso.Text.Trim() == "")
                            lblEmployerSocso.Text = "0.0";
                        if (lblEmployerEIS.Text.Trim() == "")
                            lblEmployerEIS.Text = "0.0";

                        insertPayslipDatabase(id, DateTime.Now, paymentMethod, salary, bonus, unpaidLeaveSalary, lateDeduction,
                             Convert.ToDouble(lblEmployeeEPF.Text.Trim()), Convert.ToDouble(lblEmployeeSocso.Text.Trim()), Convert.ToDouble(lblEmployeeEIS.Text.Trim()),
                             pcb, Convert.ToDouble(lblEmployerEPF.Text.Trim()), Convert.ToDouble(lblEmployerSocso.Text.Trim()), Convert.ToDouble(lblEmployerEIS.Text.Trim())
                             , totalClaimAmount, totalWorkingHour, totalOvertime);

                        Response.Redirect(string.Format("~/Project/PayslipPage.aspx?id={0}&payperiod={1}&date1={2}&date2={3}&staffID={4}", id, payperiod, firstDay, lastDay, staffID));
                        break;
                    }

                //case "Sending":
                //    {
                //        break;
                //    }

                case "Editing":
                    {
                        int staffID = getStaffID(id);
                        string payrollListID = Request.QueryString["id"];
                        string date = Request.QueryString["date"];
                        Response.Redirect(string.Format("~/Project/UserProfile.aspx?id={0}&payrollListID={1}&payperiod={2}&date={3}", staffID, payrollListID, payperiod, date));
                        break;
                    }

                case "Detailing":
                    {
                        int staffID = getStaffID(id);
                        string firstDay = Request.QueryString["date"];
                        DateTime lastDay = Convert.ToDateTime(firstDay);
                        if (payperiod == "Weekly")
                            lastDay = lastDay.AddDays(6);
                        else
                            lastDay = lastDay.AddMonths(1).AddDays(-1);
                        Response.Redirect(string.Format("~/Project/PayslipPage.aspx?id={0}&payperiod={1}&date1={2}&date2={3}&staffID={4}", id, payperiod, firstDay, lastDay.ToString(), staffID));
                        break; //payslip 
                    }
            }
        }

        private double getTotalPcbPaid(int staffID)
        {
            string abc = "";
            double pcb = 0.0;
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Payslip_CRUD"))
                {
                    cmd.Parameters.AddWithValue("@Action", "PCBPAIDBEFORE");
                    cmd.Parameters.AddWithValue("@Staff_ID", staffID);
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        con.Open();
                        SqlDataReader rd = cmd.ExecuteReader();
                        while (rd.Read())
                        {
                            abc = rd["Pcb"].ToString();
                        }
                        con.Close();
                    }
                }
            }
            if (abc != "" && abc != null)
            {
                pcb = Convert.ToDouble(abc);
            }
            return pcb;
        }

        private void insertPayslipDatabase(int id, DateTime now, string paymentMethod, double salary, double bonus, double unpaidLeaveSalary, double lateDeduction, double employeeEpf, double employeeSocso, double employeeEIS, double pcb, double employerEPF, double employerSocso, double employerEIS, double totalClaimAmount, double totalWorkingHour, double totalOvertime)
        {
            double netSalary = 0.0, statutory = 0.0;
            //string day = now.ToString();
            //now = DateTime.ParseExact(day, "yyyy-MM-dd", CultureInfo.InvariantCulture);

            if (salary == 0.0)
            {
                salary = totalWorkingHour;
            }
            statutory = employeeEIS + employeeSocso + employeeEpf + pcb;
            netSalary = salary + bonus + totalOvertime + totalClaimAmount - unpaidLeaveSalary - lateDeduction - statutory;

            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Payslip_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "UPDATEMANUAL");
                    cmd.Parameters.AddWithValue("@DateGenerated", now);
                    cmd.Parameters.AddWithValue("@PaymentMethod", paymentMethod);
                    cmd.Parameters.AddWithValue("@BasicSalary", decimal.Round(Convert.ToDecimal(salary), 2));
                    cmd.Parameters.AddWithValue("@Bonus", decimal.Round(Convert.ToDecimal(bonus), 2));
                    cmd.Parameters.AddWithValue("@OT", decimal.Round(Convert.ToDecimal(totalOvertime), 2));
                    cmd.Parameters.AddWithValue("@TotalClaimAmount", decimal.Round(Convert.ToDecimal(totalClaimAmount), 2));
                    cmd.Parameters.AddWithValue("@UnpaidLeaveSalary", decimal.Round(Convert.ToDecimal(unpaidLeaveSalary), 2));
                    cmd.Parameters.AddWithValue("@LateDeduction", decimal.Round(Convert.ToDecimal(lateDeduction), 2));
                    cmd.Parameters.AddWithValue("@EpfEmployee", decimal.Round(Convert.ToDecimal(employeeEpf), 2));
                    cmd.Parameters.AddWithValue("@SocsoEmployee", decimal.Round(Convert.ToDecimal(employeeSocso), 2));
                    cmd.Parameters.AddWithValue("@EisEmployee", decimal.Round(Convert.ToDecimal(employeeEIS), 2));
                    cmd.Parameters.AddWithValue("@PcbEmployee", decimal.Round(Convert.ToDecimal(pcb), 2));
                    cmd.Parameters.AddWithValue("@EpfEmployer", decimal.Round(Convert.ToDecimal(employerEPF), 2));
                    cmd.Parameters.AddWithValue("@SocsoEmployer", decimal.Round(Convert.ToDecimal(employerSocso), 2));
                    cmd.Parameters.AddWithValue("@EisEmployer", decimal.Round(Convert.ToDecimal(employerEIS), 2));
                    cmd.Parameters.AddWithValue("@NetSalary", decimal.Round(Convert.ToDecimal(netSalary), 2));
                    cmd.Parameters.AddWithValue("@ProcessStatus", "Processed");
                    cmd.Parameters.AddWithValue("@PayslipID", id);
                    cmd.Connection = con;
                    con.Open();
                    int a = cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
        }

        private double getTotalEpfPaid(int staffID)
        {
            string abc = "";
            double epf = 0.0;
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Payslip_CRUD"))
                {
                    cmd.Parameters.AddWithValue("@Action", "EPFPAIDBEFORE");
                    cmd.Parameters.AddWithValue("@Staff_ID", staffID);
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        con.Open();
                        SqlDataReader rd = cmd.ExecuteReader();
                        while (rd.Read())
                        {
                            abc = rd["Epf"].ToString();
                        }
                        con.Close();
                    }
                }
            }
            if (abc != "" && abc != null)
            {
                epf = Convert.ToDouble(abc);
            }
            return epf;
        }

        private int getStaffID(int id)
        {
            int staffID = 0;
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Payslip_CRUD"))
                {
                    cmd.Parameters.AddWithValue("@Action", "SELECTSTAFF");
                    cmd.Parameters.AddWithValue("@PayslipID", id);
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        con.Open();
                        SqlDataReader rd = cmd.ExecuteReader();
                        while (rd.Read())
                        {
                            staffID = (int)rd["Staff_ID"];
                        }
                        con.Close();
                    }
                }
            }
            return staffID;
        }

        private void BindGrid(int id)
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Payslip_CRUD"))
                {
                    cmd.Parameters.AddWithValue("@Action", "SELECT");
                    cmd.Parameters.AddWithValue("@PayrollListID", id);
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







        //=================================================================================

        private double pcbRoundUp(double value)
        {
            double roundUpValue = value;

            double dblTemp = roundUpValue; // 6.375
            int intTemp = (int)roundUpValue; // 6
            int intTemp2 = 0;
            double dblTemp2 = intTemp; // 6
            dblTemp = dblTemp - intTemp; //6.375 - 6 = 0.375
            dblTemp = (int)(dblTemp * 100); //0.375 = 37
            intTemp2 = (int)dblTemp; //37
            dblTemp = (int)(dblTemp % 10); //37 = 7
            if (dblTemp == 0 || dblTemp == 5)
            {
                //do nothing
            }
            else if (dblTemp > 5)
            {
                int x = 10 - (int)dblTemp; //10 - 7 = 3
                intTemp2 = intTemp2 + x; //37 + 3 = 40 
            }
            else
            {
                int x = 5 - (int)dblTemp; //5 - 1 = 4
                intTemp2 = intTemp2 + x; // 41 + 4 = 45
            }
            dblTemp = (double)intTemp2 / 100; //40 / 100 = 0.40  
            dblTemp2 += dblTemp; // 6 + 0.40 = 6.40
            roundUpValue = dblTemp2;

            return roundUpValue;
        }
        private void EpfCalculation(double basicSalary, double EmployeePercentage, double EmployerPercentage)
        {
            //epf calculation
            bool exit = false;
            double epfEmployer = 0.0, epfEmployee = 0.0;
            double salary = basicSalary; //Convert.ToDouble(TextBox1.Text); //temp
            double increaseValue = 20.00;
            double bottomValue = 20.01, topValue = 40.00;
            double range = 0;
            double epfEmployeePercentage = EmployeePercentage, epfEmployerPercentage = EmployerPercentage;
            int age = 0;

            if (salary == 0)
                return;
            if (salary >= 0.01 && salary <= 10.00)
            {
                epfEmployer = 0.00;
                epfEmployee = 0.00;
            }
            else if (salary >= 10.01 && salary <= 20.00)
            {
                epfEmployer = 3.00;
                epfEmployee = 3.00;
            }
            else
            {
                //this is default percentage follow the kwsp website
                //if later i let the user choose their percentage, then need to add a boolean value like "true" then skip this step
                if (salary <= 5000 && age >= 60)
                {
                    //epfEmployeePercentage = 0.055;
                    //epfEmployerPercentage = 0.065;
                }
                else if (salary > 5000 && age >= 60)
                {
                    //epfEmployeePercentage = 0.055;
                    //epfEmployerPercentage = 0.06;
                }
                else if (salary > 5000 && age <= 60)
                {
                    increaseValue = 100.00;
                    //epfEmployeePercentage = 0.11;
                    //epfEmployerPercentage = 0.12;
                }
                //else if (salary >= 20000) //if salary more than 20000  //temp
                //{
                //epfEmployeePercentage & epfEmployerPercentage, is depend on your percentage selected
                //}

                range = salary / 1000; //get the range; 0 = below 1000, 1 = 1000$, 10 = 10000$

                if (range > 1 && (range % 1) != 0) //for value > 1000
                {
                    range = (int)range * 1000;
                    bottomValue += range - 20.00;
                    topValue += range - 20.00;
                    if (topValue > 5000)
                    {
                        //bottomValue = bottomValue + 0.01;
                        topValue = topValue - 20.00 + 100.00;
                    }
                }
                else if (range > 0 && (range % 1) == 0) //for value like 1000,2000,5000
                {
                    epfEmployee = salary * epfEmployeePercentage;
                    epfEmployer = salary * epfEmployerPercentage;
                    exit = true;
                }

                while (exit == false)
                {
                    if (salary >= bottomValue && salary <= topValue)
                    {
                        epfEmployee = Math.Ceiling(topValue * epfEmployeePercentage);
                        epfEmployer = Math.Ceiling(topValue * epfEmployerPercentage);
                        exit = true;
                    }
                    bottomValue += increaseValue;
                    topValue += increaseValue;
                }
            }
            lblEmployerEPF.Text = epfEmployer.ToString();
            lblEmployeeEPF.Text = epfEmployee.ToString();
        }
        private void EisCalculation(double basicSalary)
        {
            //EIS calculation
            bool exit = false;
            double eisEmployer = 0.0, eisEmployee = 0.0;
            double salary = basicSalary;
            double increaseValue = 100.00;
            double bottomValue = 200.01, topValue = 300.00;
            double eisPercentage = 0.002;

            if (salary == 0)
                return;
            if (salary >= 0.01 && salary <= 30.00)
                eisEmployer = eisEmployee = 0.05;
            else if (salary >= 30.01 && salary <= 50.00)
                eisEmployer = eisEmployee = 0.10;
            else if (salary >= 50.01 && salary <= 70.00)
                eisEmployer = eisEmployee = 0.15;
            else if (salary >= 70.01 && salary <= 100.00)
                eisEmployer = eisEmployee = 0.20;
            else if (salary >= 100.01 && salary <= 140.00)
                eisEmployer = eisEmployee = 0.25;
            else if (salary >= 140.01 && salary <= 200.00)
                eisEmployer = eisEmployee = 0.35;
            else if (salary >= 5000.01)
                eisEmployer = eisEmployee = 9.90;
            else
            {
                while (exit == false)
                {
                    if (salary >= bottomValue && salary <= topValue)
                    {
                        eisEmployer = eisEmployee = ((((int)bottomValue + topValue) / 2) * eisPercentage);
                        exit = true;
                    }
                    bottomValue += increaseValue;
                    topValue += increaseValue;
                }
            }
            lblEmployerEIS.Text = eisEmployer.ToString();
            lblEmployeeEIS.Text = eisEmployee.ToString();
        }
        private void SocsoCalculation(bool IsfirstCategory, double basicSalary)
        {
            //socso calculation
            bool exit = false;
            double socsoEmployer = 0.0, socsoEmployee = 0.0;
            double salary = basicSalary;
            double increaseValue = 100.00;
            double bottomValue = 200.01, topValue = 300.00;
            double socsoEmployeePercentage = 0.005, socsoEmployerPercentage = 0.0175;
            bool firstCategory = IsfirstCategory;

            if (salary == 0)
                return;
            if (salary >= 0.01 && salary <= 30.00)
            {
                socsoEmployer = 0.40;
                socsoEmployee = 0.10;
            }
            else if (salary >= 30.01 && salary <= 50.00)
            {
                socsoEmployer = 0.70;
                socsoEmployee = 0.20;
            }
            else if (salary >= 50.01 && salary <= 70.00)
            {
                socsoEmployer = 1.10;
                socsoEmployee = 0.30;
            }
            else if (salary >= 70.01 && salary <= 100.00)
            {
                socsoEmployer = 1.50;
                socsoEmployee = 0.40;
            }
            else if (salary >= 100.01 && salary <= 140.00)
            {
                socsoEmployer = 2.10;
                socsoEmployee = 0.60;
            }
            else if (salary >= 140.01 && salary <= 200.00)
            {
                socsoEmployer = 2.95;
                socsoEmployee = 0.85;
            }
            else if (salary >= 5000.01)
            {
                socsoEmployer = 86.65;
                socsoEmployee = 24.75;
            }
            else
            {
                while (exit == false)
                {
                    if (salary >= bottomValue && salary <= topValue)
                    {
                        double dblTemp = ((((int)bottomValue + topValue) / 2) * socsoEmployerPercentage); // 6.375
                        int intTemp = (int)((((int)bottomValue + topValue) / 2) * socsoEmployerPercentage); // 6
                        int intTemp2 = 0;
                        double dblTemp2 = intTemp; // 6
                        dblTemp = dblTemp - intTemp; //6.375 - 6 = 0.375
                        dblTemp = (int)(dblTemp * 100); //0.375 = 37
                        intTemp2 = (int)dblTemp; //37
                        dblTemp = (int)(dblTemp % 10); //37 = 7
                        if (dblTemp > 5)
                            intTemp2 = intTemp2 - 2; //37 - 2 = 35 
                        else
                            intTemp2 = intTemp2 + 3;
                        dblTemp = (double)intTemp2 / 100; //35 / 100 = 0.35  
                        dblTemp2 += dblTemp; // 6 + 0.35 = 6.35

                        socsoEmployer = dblTemp2;
                        socsoEmployee = ((((int)bottomValue + topValue) / 2) * socsoEmployeePercentage);
                        exit = true;
                    }
                    bottomValue += increaseValue;
                    topValue += increaseValue;
                }
            }
            if (firstCategory == false)
            {
                socsoEmployer = socsoEmployer - socsoEmployee;
                socsoEmployee = 0.00;
            }
            lblEmployerSocso.Text = socsoEmployer.ToString();
            lblEmployeeSocso.Text = socsoEmployee.ToString();
        }
        private static double RoundUp(double input, int places)
        {
            double multiplier = Math.Pow(10, Convert.ToDouble(places));
            return Math.Ceiling(input * multiplier) / multiplier;
        }
        private double TaxCalculation(double basicSalary, bool IsSelfDisable, bool IsSpouseDisable, bool IsMarriedSpouseWork, bool IsSingle, bool IsAdditionalRemuneration, string month, double totalEpfPaidBefore, int numChild, double totalPcbPaidBefore, double totalSalaryGetBefore, double bonus)
        {
            if (basicSalary == 0)
                return 0;
            //tax, pcb or mtb
            //PCB for the current month = [ [ ( P – M ) R + B ] – ( Z + X ) ] / (n + 1)

            //https://blog.talenox.com/malaysia-tax-guide-how-do-i-calculate-pcb-mtd-part-2-of-3/            

            //if MTD for current month < 10$ , then MTD = 0
            //if MTD for current month >= 10$ , then MTD != 0
            //if MTD for current month after minus "zakat" < 10$ , then MTD != 0

            //Net PCB = PCB for the current month – zakat for the current month
            if (totalEpfPaidBefore > 4000)
                totalEpfPaidBefore = 4000;

            double P = 0.0, M = 0.0, R = 0.0, B = 0.0, Z = 0.0, X = totalPcbPaidBefore;
            int n = 0;
            bool category3 = false;
            double pcbCurrentMonth = 0.0, netPCB = 0.0;
            double Y = totalSalaryGetBefore, K = totalEpfPaidBefore, Y1 = 0.0, K1 = 0.0, Y2 = 0.0, K2 = 0.0, D = 0.0, S = 0.0, Du = 0.0, Su = 0.0, Q = 0.0, LP = 0.0, LP1 = 0.0, Kt = 0.0;
            int C = numChild;
            double zakat = 0.0;
            double salary = basicSalary;
            bool selfDisabled = IsSelfDisable, spouseDisable = IsSpouseDisable, married_spouseWork = IsMarriedSpouseWork;
            bool additionalRemuneration =  IsAdditionalRemuneration;

            //calculation for P
            //[∑(Y - K) + (Y1 - K1) +[(Y2 - K2)n] ]-(D + S + Du + Su + QC +∑LP + LP1)
            //Y = sum up of monthly employee salary, eg. january = 4000, K=0  , then next month february, K = 4000, next month march K = 8000
            //K = sum up of monthly employee epf, eg. january = 440, K=0  , then next month february, K = 440, next month march K = 880
            //Y1 = employee salary
            //K1 = employee epf
            //Y2 = estimated next month employee salary
            //K2 = formula =>  ( 4000 - (K + K1 + Kt) ) / n      ; n = Remaining working month in a year ; Kt = ?????

            //Y take from database accumulated Y
            //K take from database accumulated K
            //LP take from database accumulated LP
            //LP1 take from textfield; LP1 = Other allowable deductions for current month ;  //maybe i will not do for this, shit of deduction

            string monthName = month; //"January"; // temp
            n = 12 - DateTime.ParseExact(monthName, "MMMM", CultureInfo.CurrentCulture).Month;
            salary = basicSalary;
            Y1 = salary;
            K1 = Convert.ToDouble(lblEmployeeEPF.Text.ToString().Trim());// EpfCalculation(); //employee epf  
            Y2 = salary;
            K2 = Math.Truncate(100 * ((4000 - (K + K1 + Kt)) / n)) / 100;  //temp  //Need check K2 is accumulated or not
            if (K2 > K1)
                K2 = K1;
            D = 9000;
            Q = 2000;

            if (selfDisabled)
                Du = 6000;
            if (spouseDisable && !married_spouseWork)
            {
                S = 4000;
                Su = 5000;
            }
            else if (!IsSingle && !married_spouseWork)
                S = 4000;

            P = ((Y - K) + (Y1 - K1) + ((Y2 - K2) * n)) - (D + S + Du + Su + (Q * C) + LP + LP1);
            if (P <= 0) //just added 30/11/2022 2:18AM
            {
                P = 0;
                M = 0;
                R = 0;
            }
            //for Normal Remuneration
            else if (P >= 5001 && P <= 20000.99)
            {
                M = 5000;
                R = 0.01;
                if (category3)
                    B = -800;
                else
                    B = -400;
            }
            else if (P >= 20001 && P <= 35000.99)
            {
                M = 20000;
                R = 0.03;
                if (category3)
                    B = -650;
                else
                    B = -250;
            }
            else if (P >= 35001 && P <= 50000.99)
            {
                M = 35000;
                R = 0.08;
                B = 600;
            }
            else if (P >= 50001 && P <= 70000.99)
            {
                M = 50000;
                R = 0.14;
                B = 1800;
            }
            else if (P >= 70001 && P <= 100000.99)
            {
                M = 70000;
                R = 0.21;
                B = 4600;
            }
            else if (P >= 100001 && P <= 250000.99)
            {
                M = 100000;
                R = 0.24;
                B = 10900;
            }
            else if (P >= 250001 && P <= 400000.99)
            {
                M = 250000;
                R = 0.245;
                B = 46900;
            }
            else if (P >= 400001 && P <= 600000.99)
            {
                M = 400000;
                R = 0.25;
                B = 836500;
            }
            else if (P >= 600001 && P <= 1000000.99)
            {
                M = 600000;
                R = 0.26;
                B = 133650;
            }
            else
            {
                M = 1000000;
                R = 0.28;
                B = 237650;
            }

            //"Z" take from database accumulated Z (zakat);    "X" take from database accumulated X (total epf);    "n" take month change to digit
            pcbCurrentMonth = ((((P - M) * R) + B) - (Z + X)) / (n + 1);
            if (pcbCurrentMonth < 10)
                pcbCurrentMonth = 0.0;
            netPCB = pcbCurrentMonth - zakat;
            if (netPCB <= 0)
                netPCB = 0;
            else if (additionalRemuneration == false)
            {
                netPCB = pcbRoundUp(netPCB);
            }

            //normal pcb done for above code


            if (additionalRemuneration) 
            {
                //calculation for pcb(b)
                double pcb_B = 0.0;
                netPCB = Math.Truncate(100 * netPCB) / 100;
                pcb_B = (X) + (netPCB * (n + 1));

                //calculation for CS
                double CS = 0.0;
                double Yt = bonus; //temp //Yt = any additional income eg. bonus, commissions, ...       //Kt = maybe is extra pay of EPF for current month
                P = ((Y - K) + (Y1 - K1) + ((Y2 - K2) * n) + (Yt - Kt)) - (D + S + Du + Su + (Q * C) + LP + LP1);
                if (P <= 0) //just added 1/12/2022 12:30AM
                {
                    P = 0;
                    M = 0;
                    R = 0;
                }
                else if (P >= 5001 && P <= 20000.99)
                {
                    M = 5000;
                    R = 0.01;
                    if (category3)
                        B = -800;
                    else
                        B = -400;
                }
                else if (P >= 20001 && P <= 35000.99)
                {
                    M = 20000;
                    R = 0.03;
                    if (category3)
                        B = -650;
                    else
                        B = -250;
                }
                else if (P >= 35001 && P <= 50000.99)
                {
                    M = 35000;
                    R = 0.08;
                    B = 600;
                }
                else if (P >= 50001 && P <= 70000.99)
                {
                    M = 50000;
                    R = 0.14;
                    B = 1800;
                }
                else if (P >= 70001 && P <= 100000.99)
                {
                    M = 70000;
                    R = 0.21;
                    B = 4600;
                }
                else if (P >= 100001 && P <= 250000.99)
                {
                    M = 100000;
                    R = 0.24;
                    B = 10900;
                }
                else if (P >= 250001 && P <= 400000.99)
                {
                    M = 250000;
                    R = 0.245;
                    B = 46900;
                }
                else if (P >= 400001 && P <= 600000.99)
                {
                    M = 400000;
                    R = 0.25;
                    B = 836500;
                }
                else if (P >= 600001 && P <= 1000000.99)
                {
                    M = 600000;
                    R = 0.26;
                    B = 133650;
                }
                else
                {
                    M = 1000000;
                    R = 0.28;
                    B = 237650;
                }
                CS = ((P - M) * R) + B;

                //calculation for Additional Remuneration MTD 
                double pcb_C = 0.0;
                pcb_B = Math.Truncate(100 * pcb_B) / 100;
                pcb_C = CS - (pcb_B + Z);

                double totalPCB = 0.0;
                totalPCB = netPCB + pcb_C;

                totalPCB = pcbRoundUp(totalPCB);
                return totalPCB;
            }
            return netPCB;
        }
        //RoundUp(189.182, 2);    189.182 => "189.19"

        //the place have "temp" need change

        //=================================================================================

    }
}