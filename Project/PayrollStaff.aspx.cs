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
    public partial class PayrollStaff : System.Web.UI.Page
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
                    if (Session["role"].ToString() == "Normal Staff")
                    {
                        this.BindGrid();
                    }
                }
                else
                {
                    Response.Redirect("~/Project/Login.aspx?ReturnUrl=%2fPayrollStaff.aspx");
                }
            }
        }

        private void BindGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Payslip_CRUD"))
                {
                    cmd.Parameters.AddWithValue("@Action", "SELECTWITHSTAFFID");
                    cmd.Parameters.AddWithValue("@Staff_ID", Session["id"].ToString());
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

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gvList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label a = (Label)gvList.SelectedRow.FindControl("lblPayslipID"); 
            string id = a.Text.ToString();
            Label b = (Label)gvList.SelectedRow.FindControl("lblPayrollListID");
            string payrollListID = b.Text.ToString();
            Label c = (Label)gvList.SelectedRow.FindControl("lblStaffID");
            string staffID = c.Text.ToString();

            string firstDay = "", payperiod="";

            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("PayrollList_CRUD"))
                {
                    cmd.Parameters.AddWithValue("@Action", "SELECTWITHID");
                    cmd.Parameters.AddWithValue("@PayrollListID", payrollListID);
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        con.Open();
                        SqlDataReader rd = cmd.ExecuteReader();
                        while (rd.Read())
                        {
                            firstDay = rd["Date"].ToString();
                            payperiod = rd["Payperiod"].ToString();
                        }
                        con.Close();
                    }
                }
            }

            Response.Redirect(string.Format("~/Project/PayslipPage.aspx?id={0}&payperiod={1}&date1={2}&date2={3}&staffID={4}", id, payperiod, firstDay, "", staffID));

        }
    }
}