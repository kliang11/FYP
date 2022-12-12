using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
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
    public partial class PayrollReportReal : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["email"] != null)
            {
                if (Session["resetPW"].ToString() == "yes")
                {
                    Response.Redirect("~/Project/ChangePassword.aspx");
                }
                string date = Request.QueryString["date"];
                string payrollListID = Request.QueryString["payrollListID"];
                string title = Request.QueryString["title"];
                if (date != null && payrollListID != null && title != null)
                {
                    bindReport(date, payrollListID, title);
                }
                else
                {
                    Response.Redirect(string.Format("~/Project/ContactAdmin.html"));
                }
            }
            else
            {
                Response.Redirect("~/Project/Login.aspx?ReturnUrl=%2fPayrollReportReal.aspx");
            }          
        }

        private void bindReport(string date, string payrollListID, string title)
        {
            string BasicSalaryTotal = "", OTTotal = "", BonusTotal = "", ClaimTotal = "", UnpaidTotal = "", LateTotal = "", EpfTotal_e = "", SocsoTotal_e = "", EisTotal_e = "", PcbTotal = "", EpfTotal_r = "", SocsoTotal_r = "", EisTotal_r = "", NetTotal = "";

            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Payslip_CRUD"))
                {
                    cmd.Parameters.AddWithValue("@Action", "SELECTSUMFORREPORT");
                    cmd.Parameters.AddWithValue("@PayrollListID", Int16.Parse(payrollListID));
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        con.Open();
                        SqlDataReader rd = cmd.ExecuteReader();
                        while (rd.Read())
                        {
                            BasicSalaryTotal = rd["BasicSalaryTotal"].ToString();
                            OTTotal = rd["OTTotal"].ToString();
                            BonusTotal = rd["BonusTotal"].ToString();
                            ClaimTotal = rd["ClaimTotal"].ToString();
                            UnpaidTotal = rd["UnpaidTotal"].ToString();
                            LateTotal = rd["LateTotal"].ToString();
                            EpfTotal_e = rd["EpfTotal_e"].ToString();
                            SocsoTotal_e = rd["SocsoTotal_e"].ToString();
                            EisTotal_e = rd["EisTotal_e"].ToString();
                            PcbTotal = rd["PcbTotal"].ToString();
                            EpfTotal_r = rd["EpfTotal_r"].ToString();
                            SocsoTotal_r = rd["SocsoTotal_r"].ToString();
                            EisTotal_r = rd["EisTotal_r"].ToString();
                            NetTotal = rd["NetTotal"].ToString();
                        }
                        con.Close();
                    }
                }
            }

            constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Payslip_CRUD"))
                {
                    cmd.Parameters.AddWithValue("@Action", "SELECTFORREPORTWITHNAME");
                    cmd.Parameters.AddWithValue("@PayrollListID", Int16.Parse(payrollListID));
                    using (SqlDataAdapter sda = new SqlDataAdapter())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Connection = con;
                        sda.SelectCommand = cmd;
                        using (DataSet ds = new DataSet())
                        {
                            sda.SelectCommand = cmd;
                            sda.Fill(ds);
                            con.Open();
                            SqlDataReader rd = cmd.ExecuteReader();
                            con.Close();

                            ReportDocument crp = new ReportDocument();
                            crp.Load(Server.MapPath("~/Project/Payrollreport.rpt"));
                            crp.SetDataSource(ds.Tables["table"]);
                            crp.SetParameterValue("Title", title);
                            crp.SetParameterValue("StartDate", date);
                            crp.SetParameterValue("BasicSalaryTotal", BasicSalaryTotal);
                            crp.SetParameterValue("OTTotal", OTTotal);
                            crp.SetParameterValue("BonusTotal", BonusTotal);
                            crp.SetParameterValue("ClaimTotal", ClaimTotal);
                            crp.SetParameterValue("UnpaidTotal", UnpaidTotal);
                            crp.SetParameterValue("LateTotal", LateTotal);
                            crp.SetParameterValue("EpfTotal_e", EpfTotal_e);
                            crp.SetParameterValue("SocsoTotal_e", SocsoTotal_e);
                            crp.SetParameterValue("EisTotal_e", EisTotal_e);
                            crp.SetParameterValue("PcbTotal", PcbTotal);
                            crp.SetParameterValue("EpfTotal_r", EpfTotal_r);
                            crp.SetParameterValue("SocsoTotal_r", SocsoTotal_r);
                            crp.SetParameterValue("EisTotal_r", EisTotal_r);
                            crp.SetParameterValue("NetTotal", NetTotal);

                            crp.SummaryInfo.ReportTitle = "Payroll Report";
                            CrystalReportViewer1.ReportSource = crp;
                            crp.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "Payroll Report");
                        }
                    }
                }
            }

        }
    }
}