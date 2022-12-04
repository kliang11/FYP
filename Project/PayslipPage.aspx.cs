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
    public partial class PayslipPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string id = Request.QueryString["id"];
            string payperiod = Request.QueryString["payperiod"];
            string firstday= Request.QueryString["date1"];
            string lastday = Request.QueryString["date2"];            
            string staffID = Request.QueryString["staffID"];            

            if (id != null && payperiod != null && firstday != null)
            {
                DateTime date = Convert.ToDateTime(firstday);
                if (payperiod == "Weekly")
                {
                    firstday = date.ToString("d/M/yyyy");
                    date = date.AddDays(6);
                    firstday = firstday +" - "+ date.ToString("d/M/yyyy");
                }
                else
                {
                    firstday = date.ToString("MMMM yyyy");
                }

                bindReport(id, payperiod, firstday, staffID);
            }
        }

        private void bindReport(string payslipID, string payperiod, string firstday, string staffID)
        {
            string dateGenerated = "", paymentMethod = "", basicSalary = "", bonus = "", unpaidLeaveSalary = "", lateDeduction = "", netSalary = "", OT = "",
               totalClaimAmount = "", epfEmployee = "", socsoEmployee = "", eisEmployee = "", pcbEmployee = "", epfEmployer = "", socsoEmployer = "", eisEmployer = "";
            string staffName = "", NRIC = "";

            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Payslip_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "SELECTSTAFF");
                    cmd.Parameters.AddWithValue("@PayslipID", payslipID);
                    cmd.Connection = con;
                    con.Open();
                    SqlDataReader rd = cmd.ExecuteReader();
                    while (rd.Read())
                    {
                        dateGenerated = (rd["DateGenerated"].ToString());                    
                        paymentMethod = (rd["PaymentMethod"].ToString());                    
                        basicSalary = (rd["BasicSalary"].ToString());                    
                        bonus = (rd["Bonus"].ToString());                    
                        OT = (rd["OT"].ToString());
                        totalClaimAmount = (rd["TotalClaimAmount"].ToString());                        
                        unpaidLeaveSalary = (rd["UnpaidLeaveSalary"].ToString());                    
                        lateDeduction = (rd["LateDeduction"].ToString());                    
                        epfEmployee = (rd["EpfEmployee"].ToString());                    
                        socsoEmployee = (rd["SocsoEmployee"].ToString());                    
                        eisEmployee = (rd["EisEmployee"].ToString());                    
                        pcbEmployee = (rd["PcbEmployee"].ToString());                    
                        epfEmployer = (rd["EpfEmployer"].ToString());                    
                        socsoEmployer = (rd["SocsoEmployer"].ToString());                    
                        eisEmployer = (rd["EisEmployer"].ToString());                    
                        netSalary = (rd["NetSalary"].ToString());                    
                    }
                    con.Close();
                }
            }

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
                        staffName = rd["Name"].ToString();
                        NRIC = rd["IcNo"].ToString();
                    }
                    con.Close();
                }
            }

            constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Claim_CRUD"))
                {
                    cmd.Parameters.AddWithValue("@Action", "SELECTFORPAYSLIP");
                    cmd.Parameters.AddWithValue("@Staff_ID", staffID);
                    cmd.Parameters.AddWithValue("@ClaimReceiveDate", staffID+payslipID);
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

                            double statutory = (Convert.ToDouble(epfEmployee) + Convert.ToDouble(socsoEmployee) + Convert.ToDouble(eisEmployee) + Convert.ToDouble(pcbEmployee));
                            double employerTotal = (Convert.ToDouble(epfEmployer) + Convert.ToDouble(socsoEmployer) + Convert.ToDouble(eisEmployer));
                            double totalStatutory = (Convert.ToDouble(epfEmployee) + Convert.ToDouble(socsoEmployee) + Convert.ToDouble(eisEmployee) + Convert.ToDouble(pcbEmployee) + Convert.ToDouble(epfEmployee) + Convert.ToDouble(socsoEmployee) + Convert.ToDouble(eisEmployee) + Convert.ToDouble(pcbEmployee));
                            double epfTotal = (Convert.ToDouble(epfEmployee) + Convert.ToDouble(epfEmployer));
                            double socsoTotal = (Convert.ToDouble(socsoEmployee) + Convert.ToDouble(socsoEmployer));
                            double eisTotal = (Convert.ToDouble(eisEmployee) + Convert.ToDouble(eisEmployer));
                            
                            ReportDocument crp = new ReportDocument();
                            crp.Load(Server.MapPath("~/Project/Payslip.rpt"));
                            crp.SetDataSource(ds.Tables["table"]);
                            crp.SetParameterValue("StartDate", firstday);
                            crp.SetParameterValue("StaffID", staffID);
                            crp.SetParameterValue("Name", staffName);
                            crp.SetParameterValue("NRIC", NRIC);
                            crp.SetParameterValue("PaymentMethod", paymentMethod);
                            crp.SetParameterValue("BasicSalary", basicSalary);
                            crp.SetParameterValue("Bonus", bonus);
                            crp.SetParameterValue("OT", OT);                                           
                            crp.SetParameterValue("TotalClaimAmount", totalClaimAmount);
                            crp.SetParameterValue("UnpaidLeaveDeduction", "(" + unpaidLeaveSalary + ")");
                            crp.SetParameterValue("LatenessDeduction", "(" + lateDeduction + ")");
                            crp.SetParameterValue("StatutoryContribution", "(" + statutory.ToString() + ")");
                            crp.SetParameterValue("NetSalary", netSalary);
                            crp.SetParameterValue("Epf_employer", epfEmployer);
                            crp.SetParameterValue("Socso_employer", socsoEmployer);
                            crp.SetParameterValue("EIS_employer", eisEmployer);
                            crp.SetParameterValue("Employer_total", employerTotal.ToString());
                            crp.SetParameterValue("StatutoryBelow", statutory.ToString());
                            crp.SetParameterValue("Epf_employee", epfEmployee);
                            crp.SetParameterValue("Socso_employee", socsoEmployee);
                            crp.SetParameterValue("EIS_employee", eisEmployee);
                            crp.SetParameterValue("Tax_employee", pcbEmployee);                                 
                            crp.SetParameterValue("Epf_total", epfTotal.ToString());
                            crp.SetParameterValue("Socso_total", socsoTotal.ToString());
                            crp.SetParameterValue("EIS_total", eisTotal.ToString());
                            crp.SetParameterValue("Tax_total", pcbEmployee);
                            crp.SetParameterValue("total_statutory", totalStatutory.ToString());
                            crp.SummaryInfo.ReportTitle = "Payslip";
                            CrystalReportViewer1.ReportSource = crp;
                            crp.ExportToHttpResponse(ExportFormatType.PortableDocFormat, Response, false, "Payslip");
                        }
                    }
                }
            }
        }
    }
}