using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FYP
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] != null)
            {
                if (Session["role"].ToString() == "Admin")
                {
                    name.InnerText = "Admin";
                }
                else
                {
                    profileImg.Src = Session["profileImg"].ToString();
                    name.InnerText = Session["name"].ToString();
                }

            }

            if (!IsPostBack)
            {
                if (Session["email"] != null)
                {

                    role.InnerText = Session["role"].ToString();

                    if (Session["role"].ToString() == "HR Staff")
                    {
                        attendance.HRef = "~/Project/attendance.aspx";
                        leave.HRef = "~/Project/LeaveList.aspx";
                        staffList.HRef = "~/Project/EmployeeList.aspx";
                        payroll.HRef = "~/Project/payroll.aspx";
                        claim.HRef = "~/Project/ClaimList.aspx";
                    }
                    else if (Session["role"].ToString() == "Normal Staff")
                    {
                        attendance.HRef = "~/Project/attendanceStaff.aspx";
                        leave.HRef = "~/Project/LeaveStaff.aspx";
                        rfid.Style.Add("display", "none");
                        report.Style.Add("display", "none");
                        staffList.Style.Add("display", "none");
                        payroll.HRef = "~/Project/PayrollStaff.aspx";
                        claim.HRef = "~/Project/ApplyClaim.aspx";
                        dropdownprofile.HRef = "~/Project/UserProfile.aspx?redirect=yes";
                    }
                    else if (Session["role"].ToString() == "Admin")
                    {
                        report.Style.Add("display", "none");
                        payroll.Style.Add("display", "none");
                        attendance.Style.Add("display", "none");
                        leave.Style.Add("display", "none");
                        rfid.Style.Add("display", "none");
                        //dropdownprofile.Style.Add("display", "none");
                        //dropdownchangePW.Style.Add("display", "none");
                        claim.HRef = "~/Project/ClaimType.aspx";
                        staffList.HRef = "~/Project/EmployeeList.aspx";
                    }
                }
            }
        }
    }
}