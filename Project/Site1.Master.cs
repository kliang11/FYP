using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FYP
{
    public partial class Site1 : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //temp 
            //abc.Style.Add("display", "none");
            if (!IsPostBack)
            {
                Session["role"] = "HR Staff";

                if (Session["role"].ToString() == "HR Staff")
                {
                    attendance.HRef = "~/Project/attendance.aspx";                   
                    leave.HRef = "~/Project/LeaveList.aspx";
                }
                else if (Session["role"].ToString() == "Normal Staff")
                {
                    attendance.HRef = "~/Project/attendanceStaff.aspx";
                    leave.HRef = "~/Project/LeaveStaff.aspx";
                    rfid.Style.Add("display", "none");
                    report.Style.Add("display", "none");
                }

            }
        }
    }
}