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
            if (!IsPostBack)
            {
                if (Session["email"] != null)
                {
                    profileImg.Src = Session["profileImg"].ToString();
                    name.InnerText = Session["name"].ToString();
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
                    }
                    else if(Session["role"].ToString() == "Admin")
                    {
                        report.Style.Add("display", "none");
                        payroll.Style.Add("display", "none");
                        claim.HRef = "~/Project/ClaimType.aspx";
                        staffList.HRef = "~/Project/EmployeeList.aspx";
                    }
                }
            }
        }
    }
}