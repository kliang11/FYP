using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FYP.Project
{
    public partial class ApplyClaim : System.Web.UI.Page
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
                        ddlBind();
                        LoadMaxClaim();
                    }
                    else
                    {
                        Response.Redirect(string.Format("~/Project/403error.html"));
                    }
                }
                else
                {
                    Response.Redirect("~/Project/Login.aspx?ReturnUrl=%2fApplyClaim.aspx");
                }
            }
        }

        private void LoadMaxClaim()
        {
            List<string[]> rowList = new List<string[]>();
            List<string> claimIDList = new List<string>();
            string abc = "";
            string value = "Type=";
            string claimTypeId = "";
            string xyz = "";

            SqlConnection con = new SqlConnection();
            string strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            con = new SqlConnection(strCon);
            con.Open();
            string strSqlQuery = "SELECT * FROM ClaimType";
            SqlCommand cmdSelect = new SqlCommand(strSqlQuery, con);
            SqlDataReader rd = cmdSelect.ExecuteReader();
            while (rd.Read())
            {
                string[] values = new string[2];
                values[0] = rd["ClaimTypeDesc"].ToString();
                values[1] = rd["MaxClaim"].ToString();
                rowList.Add(values);

                claimTypeId = rd["ClaimTypeID"].ToString();
                claimIDList.Add(claimTypeId);
            }
            con.Close();
            string[,] storeValueArray = new string[rowList.Count, 2];
            for (int i = 0; i < rowList.Count; ++i)
            {
                for (int j = 0; j < 2; ++j)
                {
                    storeValueArray[i, j] = rowList[i][j];
                    abc += value + rowList[i][j];
                    if (value == "Type=")
                        value = " Max=";
                    else
                        value = "Type=";
                }
                abc += ",";
            }
            lblStoreClaimType.Text = abc;

            con = new SqlConnection();
            strCon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            con = new SqlConnection(strCon);
            con.Open();
            for (int i = 0; i < rowList.Count; ++i)
            {
                strSqlQuery = "SELECT SUM(ClaimAmount) FROM Claim  WHERE Staff_ID ="+ Session["id"].ToString()+ " AND " + "ClaimTypeID = " + claimIDList[i] + ";";
                cmdSelect = new SqlCommand(strSqlQuery, con);
                object amount = cmdSelect.ExecuteScalar();
                if (amount.ToString() == "")
                    amount = "0";
                xyz += "Id=" + claimIDList[i] + " Total=" + amount.ToString()+", ";
            }
            con.Close();
            lblStoreTotalClaimAmount.Text = xyz;
        }

        private void ddlBind()
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlDataAdapter adpt = new SqlDataAdapter("SELECT ClaimTypeID, ClaimTypeDesc FROM ClaimType", con);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            ddlClaimType.DataSource = dt;
            ddlClaimType.DataBind();
            ddlClaimType.DataTextField = "ClaimTypeDesc";
            ddlClaimType.DataValueField = "ClaimTypeID";
            ddlClaimType.DataBind();
        }

        protected void ddlClaimType_DataBound(object sender, EventArgs e)
        {

        }

        private void BindGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Claim_CRUD"))
                {
                    cmd.Parameters.AddWithValue("@Action", "SELECT");
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

        protected void gvList_SelectedIndexChanged(object sender, EventArgs e)
        {
            Label a = (Label)gvList.SelectedRow.FindControl("lblClaimID");
            string claimID = a.Text.ToString();
            Response.Redirect(string.Format("~/Project/ClaimDetails.aspx?id={0}", claimID));
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != gvList.EditIndex)
            {
                if ((e.Row.FindControl("lblStatus") as Label).Text.Equals("Reject"))
                    (e.Row.FindControl("lblStatus") as Label).CssClass = "label_red";
                else if ((e.Row.FindControl("lblStatus") as Label).Text.Equals("Approve"))
                    (e.Row.FindControl("lblStatus") as Label).CssClass = "label_green";
                else if ((e.Row.FindControl("lblStatus") as Label).Text.Equals("Pending"))
                    (e.Row.FindControl("lblStatus") as Label).CssClass = "label_yellow";
            }
        }
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            DateTime dateFrom = Convert.ToDateTime(txt_dateFrom.Text.ToString());
            DateTime dateTo = Convert.ToDateTime(txt_dateTo.Text.ToString());
            var claimDate = DateTime.Now.ToString("yyyy-MM-dd");
            decimal amount = decimal.Parse(txt_Amount.Text.ToString()); amount = decimal.Round(amount, 2);
            string description = txt_Reason.Text.ToString();
            byte[] bytes = null;
            byte[] imageBytes = null;
            using (BinaryReader br = new BinaryReader(FileUpload1.PostedFile.InputStream))
            {
                imageBytes = br.ReadBytes(FileUpload1.PostedFile.ContentLength);
            }
            bytes = imageBytes;
            string claimTypeID = ddlClaimType.SelectedItem.Value.ToString();
            string status = "Pending";
            string staffId = Session["id"].ToString();

            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Claim_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "INSERT");
                    cmd.Parameters.AddWithValue("@DateFrom", dateFrom);
                    cmd.Parameters.AddWithValue("@DateTo", dateTo);
                    cmd.Parameters.AddWithValue("@ClaimDate", claimDate);
                    cmd.Parameters.AddWithValue("@ClaimAmount", amount);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@ClaimAttachment", bytes);
                    cmd.Parameters.AddWithValue("@ClaimTypeID", claimTypeID);
                    cmd.Parameters.AddWithValue("@ClaimStatus", status);
                    cmd.Parameters.AddWithValue("@RejectReason", "-");
                    cmd.Parameters.AddWithValue("@Staff_ID", staffId);
                    cmd.Parameters.AddWithValue("@ClaimReceive", "Pending");
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            BindGrid();
            LoadMaxClaim();
            txt_dateFrom.Text = "";
            txt_dateTo.Text = "";
            ddlClaimType.SelectedIndex = 0;
            txt_Amount.Text = "";
            txt_Reason.Text = "";
            FileUpload1.PostedFile.InputStream.Dispose();
        }
    }
}