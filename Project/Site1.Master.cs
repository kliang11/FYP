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
                        payrollStaff.Style.Add("display", "none");
                        applyClaim.Style.Add("display", "none");
                        claimType.Style.Add("display", "none");
                    }
                    else if (Session["role"].ToString() == "Normal Staff")
                    {
                        attendance.HRef = "~/Project/attendanceStaff.aspx";
                        leave.HRef = "~/Project/LeaveStaff.aspx";
                        rfid.Style.Add("display", "none");
                        report.Style.Add("display", "none");
                        staffList.Style.Add("display", "none");
                        payrollList.Style.Add("display", "none");
                        payrollReport.Style.Add("display", "none");
                        claimType.Style.Add("display", "none");
                        claimList.Style.Add("display", "none");
                    }
                    else if(Session["role"].ToString() == "Admin")
                    {
                        payrollList.Style.Add("display", "none");
                        payrollReport.Style.Add("display", "none");
                        payrollStaff.Style.Add("display", "none");
                        applyClaim.Style.Add("display", "none");
                        claimList.Style.Add("display", "none");
                    }
                }
            }
        }
    }
}