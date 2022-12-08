using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FYP
{
    public partial class ClaimType : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["email"] != null)
                {
                    if (Session["role"].ToString() == "Admin")
                    {
                        this.BindGrid();
                    }
                    else
                    {
                        Response.Redirect(string.Format("~/Project/403error.html"));
                    }
                }
                else
                {
                    Response.Redirect("~/Project/Login.aspx?ReturnUrl=%2fClaimType.aspx");
                }
            }
        }

        private void BindGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("ClaimType_CRUD"))
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

        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("ClaimType_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "INSERT");
                    cmd.Parameters.AddWithValue("@ClaimTypeDesc", txt_ClaimType.Text);
                    cmd.Parameters.AddWithValue("@MaxClaim", txt_Amount.Text);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            BindGrid();
            txt_ClaimType.Text = "";
            txt_Amount.Text = "";
        }

        protected void OnRowEditing(object sender, GridViewEditEventArgs e)
        {
            gvList.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void OnRowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = gvList.Rows[e.RowIndex];

            string claimTypeID = gvList.DataKeys[e.RowIndex]["ClaimTypeID"].ToString();
            string claimType = (row.FindControl("txtClaimType") as TextBox).Text;
            string amount = (row.FindControl("txtAmount") as TextBox).Text;

            if(claimType == null || claimType=="" || amount == null || amount == "")
            {
                return;
            }

            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("ClaimType_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "UPDATE");
                    cmd.Parameters.AddWithValue("@ClaimTypeID", claimTypeID);
                    cmd.Parameters.AddWithValue("@ClaimTypeDesc", claimType);
                    cmd.Parameters.AddWithValue("@MaxClaim", amount);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            gvList.EditIndex = -1;
            this.BindGrid();
        }

        protected void OnRowCancelingEdit(object sender, EventArgs e)
        {
            gvList.EditIndex = -1;
            this.BindGrid();
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = gvList.Rows[e.RowIndex];

            string claimTypeID = gvList.DataKeys[e.RowIndex]["ClaimTypeID"].ToString();
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("ClaimType_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "DELETE");
                    cmd.Parameters.AddWithValue("@ClaimTypeID", claimTypeID);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            this.BindGrid();
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != gvList.EditIndex)
            {                
                (e.Row.Cells[3].Controls[2] as ImageButton).Attributes["onclick"] = "if(!confirm('Are you sure you want to delete?')) return false;";
            }
        }

    }
}