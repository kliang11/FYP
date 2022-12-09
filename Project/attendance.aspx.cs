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
    public partial class attendance : System.Web.UI.Page
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
                        txtSelectDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                        BindGrid();
                    }
                    else
                    {
                        Response.Redirect(string.Format("~/Project/403error.html"));
                    }
                }
                else
                {
                    Response.Redirect("~/Project/Login.aspx?ReturnUrl=%2fEmployeeList.aspx");
                }

            }
            else if (gvList.Rows.Count != 0)
            {
                gvList.UseAccessibleHeader = true;
                gvList.HeaderRow.TableSection = TableRowSection.TableHeader;
                gvList.FooterRow.TableSection = TableRowSection.TableFooter;
            }
        }
        private void BindGrid()
        {

            DateTime date = Convert.ToDateTime(txtSelectDate.Text);
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Attendance_CRUD"))
                {
                    cmd.Parameters.AddWithValue("@Action", "SELECT");
                    cmd.Parameters.AddWithValue("@AttendanceDate", date);
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
            presentCount(constr, date);
            pendingCount(constr, date);
            onLeaveCount(constr, date);

        }

        private void pendingCount(string constr, DateTime date)
        {
            using (SqlConnection mycon = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Attendance WHERE AttendanceStatus= 'Pending' AND AttendanceDate =@date", mycon))
                {
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.CommandType = CommandType.Text;
                    mycon.Open();

                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    countPending.InnerText = count.ToString();
                }
            }
        }

        private void presentCount(string constr, DateTime date)
        {
            using (SqlConnection mycon = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Attendance WHERE (AttendanceStatus= 'Present') AND AttendanceDate =@date", mycon))
                {
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.CommandType = CommandType.Text;
                    mycon.Open();

                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    countPresent.InnerText = count.ToString();
                }
            }
        }

        private void onLeaveCount(string constr, DateTime date)
        {
            using (SqlConnection mycon = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Attendance WHERE (AttendanceStatus= 'On Leave') AND AttendanceDate =@date", mycon))
                {
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.CommandType = CommandType.Text;
                    mycon.Open();

                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    CountOnLeave.InnerText = count.ToString();
                }
            }
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != gvList.EditIndex)
            {

                if ((e.Row.FindControl("lblAttendanceStatus") as Label).Text.Equals("Present"))
                    (e.Row.FindControl("lblAttendanceStatus") as Label).CssClass = "label_green";
                else if ((e.Row.FindControl("lblAttendanceStatus") as Label).Text.Equals("Pending"))
                    (e.Row.FindControl("lblAttendanceStatus") as Label).CssClass = "label_yellow";
                else if ((e.Row.FindControl("lblAttendanceStatus") as Label).Text.Equals("On Leave"))
                    (e.Row.FindControl("lblAttendanceStatus") as Label).CssClass = "label_grey";
                else if ((e.Row.FindControl("lblAttendanceStatus") as Label).Text.Equals("Absent"))
                    (e.Row.FindControl("lblAttendanceStatus") as Label).CssClass = "label_redAbsent";

                var timeIn = (e.Row.FindControl("lblTimeIn") as Label).Text;
                var timeOut = (e.Row.FindControl("lblTimeOut") as Label).Text;
                if (timeIn.Equals(""))
                {
                    (e.Row.FindControl("lblTimeIn") as Label).Text = "-";
                }
                if (timeOut.Equals(""))
                {
                    (e.Row.FindControl("lblTimeOut") as Label).Text = "-";
                }
                (e.Row.Cells[5].Controls[2] as ImageButton).Attributes["onclick"] = "if(!confirm('Are you sure you want to delete?')) return false;";
            }

        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = gvList.Rows[e.RowIndex];

            string attendanceID = gvList.DataKeys[e.RowIndex]["AttendanceID"].ToString();
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Attendance_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "DELETE");
                    cmd.Parameters.AddWithValue("@AttendanceID", attendanceID);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            this.BindGrid();
        }

        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            gvList.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void OnRowCancelingEdit(object sender, EventArgs e)
        {
            gvList.EditIndex = -1;
            this.BindGrid();
        }

        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {

            GridViewRow row = gvList.Rows[e.RowIndex];
            string checkTimeIn = (row.FindControl("txtTimeIn") as TextBox).Text;
            string checkTimeOut = (row.FindControl("txtTimeOut") as TextBox).Text;
            string attendanceStatus = "Pending";
            string isLate = "N";
            double workingHour = 0;
            double overtime = 0;
            Boolean yesTimeIn = false;
            Boolean yesTimeOut = false;

            if (checkTimeIn != "-" && checkTimeIn != "")
            {
                DateTime timeIn = Convert.ToDateTime(checkTimeIn);
                if (!checkOntime(timeIn))
                {
                    isLate = "Y";
                }
                yesTimeIn = true;
                attendanceStatus = "Present";
            }

            if (checkTimeOut != "-" && checkTimeOut != "")
            {
                calculateWorkingHour(checkTimeIn, checkTimeOut, ref workingHour, ref overtime);
                yesTimeOut = true;
            }

            string attendanceID = gvList.DataKeys[e.RowIndex]["AttendanceID"].ToString();
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Attendance_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "UPDATE");
                    cmd.Parameters.AddWithValue("@AttendanceID", attendanceID);
                    cmd.Parameters.AddWithValue("@AttendanceStatus", attendanceStatus);
                    if (yesTimeIn) { cmd.Parameters.AddWithValue("@AttendanceTimeIn", Convert.ToDateTime(checkTimeIn)); }
                    else { cmd.Parameters.AddWithValue("@AttendanceTimeIn", null); }
                    if (yesTimeOut) { cmd.Parameters.AddWithValue("@AttendanceTimeOut", Convert.ToDateTime(checkTimeOut)); }
                    else { cmd.Parameters.AddWithValue("@AttendanceTimeOut", null); }
                    cmd.Parameters.AddWithValue("@WorkingHour", Math.Round(workingHour, 2));
                    cmd.Parameters.AddWithValue("@Overtime", overtime);
                    cmd.Parameters.AddWithValue("@IsLate", isLate);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            gvList.EditIndex = -1;
            this.BindGrid();
        }

        private bool checkOntime(DateTime timeIn)
        {
            int workHour = 9;
            int workMinutes = 1;
            if (timeIn.Hour > workHour)
            {
                return false;
            }
            else if (timeIn.Hour == workHour && timeIn.Minute >= workMinutes)
            {
                return false;
            }
            return true;
        }

        private void calculateWorkingHour(string cIn, string cOut, ref double workingHour, ref double overtime)
        {

            int otHour = 9;

            DateTime timeIn = Convert.ToDateTime(cIn);
            DateTime timeOut = Convert.ToDateTime(cOut);
            TimeSpan span = timeOut.Subtract(timeIn);

            workingHour = Math.Round((double)(span.Hours + (span.Minutes / 60.00)), 2);

            if (workingHour > otHour)
            {
                overtime = Math.Round((workingHour - otHour), 2);
                workingHour = otHour;
            }
        }

        protected void addAttendance(object sender, EventArgs e)
        {
            string staffID = ddlStaffName.SelectedItem.Value.ToString();
            string date = txtDate.Text.ToString();
            string timeIn = txtTimeIn.Text.ToString();
            string timeOut = txtTimeOut.Text.ToString();
            string attendanceStatus = "Present";
            string isLate = "N";
            double workingHour = 0;
            double overtime = 0;


            if (!checkOntime(Convert.ToDateTime(timeIn)))
            {
                isLate = "Y";
            }

            calculateWorkingHour(timeIn, timeOut, ref workingHour, ref overtime);

            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Attendance_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "INSERT");
                    cmd.Parameters.AddWithValue("@AttendanceStatus", attendanceStatus);
                    cmd.Parameters.AddWithValue("@AttendanceDate", Convert.ToDateTime(date));
                    cmd.Parameters.AddWithValue("@AttendanceTimeIn", Convert.ToDateTime(timeIn));
                    cmd.Parameters.AddWithValue("@AttendanceTimeOut", Convert.ToDateTime(timeOut));
                    cmd.Parameters.AddWithValue("@WorkingHour", workingHour);
                    cmd.Parameters.AddWithValue("@Overtime", overtime);
                    cmd.Parameters.AddWithValue("@IsLate", isLate);
                    cmd.Parameters.AddWithValue("@Staff_ID", staffID);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            BindGrid();
            txtDate.Text = "";
            txtTimeIn.Text = "";
            txtTimeOut.Text = "";
        }

        protected void minusDate(object sender, EventArgs e)
        {
            DateTime date = Convert.ToDateTime(txtSelectDate.Text).AddDays(-1);
            txtSelectDate.Text = date.ToString("yyyy-MM-dd");
            BindGrid();

        }

        protected void addDate(object sender, EventArgs e)
        {
            DateTime date = Convert.ToDateTime(txtSelectDate.Text).AddDays(1);
            txtSelectDate.Text = date.ToString("yyyy-MM-dd");
            BindGrid();
        }

        protected void txtSelectDate_TextChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void OnRowSelect(object sender, EventArgs e)
        {
            Label a = (Label)gvList.SelectedRow.FindControl("lblAttendanceID");
            string attendanceID = a.Text.ToString().Remove(0, 1);
            Response.Redirect(string.Format("~/Project/attendanceDetails.aspx?id={0}", attendanceID));
        }
    }
}