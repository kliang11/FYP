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
    public partial class attendanceDetails : System.Web.UI.Page
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
                    try
                    {
                        ViewState["RefUrl"] = Request.UrlReferrer.ToString();
                    }
                    catch
                    {
                        Response.Redirect(string.Format("~/Project/ContactAdmin.html"));
                    }
                    string id = Request.QueryString["id"];
                    if (id != null)
                    {
                        databind(id);
                    }
                    else
                    {
                        Response.Redirect(string.Format("~/Project/ContactAdmin.html"));
                    }
                }
                else
                {
                    Response.Redirect("~/Project/Login.aspx?ReturnUrl=%2fApplyClaim.aspx");
                }
            }
        }

        private void databind(string id)
        {

            SqlConnection con = new SqlConnection();
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            con = new SqlConnection(strCon);
            con.Open();

            string strSqlQuery = "SELECT Attendance.AttendanceID,Attendance.AttendanceStatus,Attendance.AttendanceTimeIn,Attendance.AttendanceTimeOut,Attendance.Overtime,Attendance.IsLate,Attendance.WorkingHour,Attendance.StaffID,Attendance.AttendanceDate,Staff.Name, Staff.Photo FROM Attendance INNER JOIN Staff ON Attendance.StaffID = Staff.Staff_ID WHERE Attendance.AttendanceID =" + id;
            SqlCommand cmdSelect = new SqlCommand(strSqlQuery, con);
            SqlDataReader rd = cmdSelect.ExecuteReader();

            while (rd.Read())
            {
                if (rd["Photo"].ToString() != "")
                {
                    imgProfile.ImageUrl = "data:Image/png;base64," + Convert.ToBase64String((byte[])rd["Photo"]);
                }

                txtAttdID.Text = "A" + rd["AttendanceID"].ToString();
                string attdStatus = rd["AttendanceStatus"].ToString();
                txtAttdStatus.Text = attdStatus;
                string date = rd["AttendanceDate"].ToString();
                txtAttedDate.Text = Convert.ToDateTime(date).ToShortDateString().ToString();

                txtStaffID.Text = rd["StaffID"].ToString();
                txtStaffName.Text = rd["Name"].ToString();

                string timeIn = rd["AttendanceTimeIn"].ToString();
                txtTimeIn.Text = checkTime(timeIn);
                string timeOut = rd["AttendanceTimeOut"].ToString();
                txtTimeOut.Text = checkTime(timeOut);


                string workingHour = rd["WorkingHour"].ToString();
                string overtime = rd["Overtime"].ToString();
                txtWorkHour.Text = checkHour(workingHour);
                txtOvertime.Text = checkHour(overtime);

                string isLate = rd["IsLate"].ToString();
                txtIsLate.Text = checkIsLate(isLate, attdStatus);
            }
        }
        private string checkHour(string time)
        {
            if (time == "")
            {
                return "-";
            }
            else
            {
                return time + " hrs";
            }
        }

        private string checkTime(string time)
        {
            if (time == "")
            {
                return "-";
            }
            return time;
        }

        private string checkIsLate(string isLate, string atdStatus)
        {
            if(atdStatus == "Pending" || atdStatus=="On Leave" || atdStatus=="Absent")
            {
                return "-";
            }
            if(isLate == "N")
            {
                return "On Time";
            }
            return "Late"; 
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {

            object refUrl = ViewState["RefUrl"];
            if (refUrl != null)
                Response.Redirect((string)refUrl);
        }
    }
}