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
    public partial class EmployeeList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) //IsPostBack = false
            {
                string role = "admin";// Session["role"].ToString();
                if(role == "admin")
                {
                    this.gvList.Columns[5].Visible = true;
                    this.gvList.Columns[6].Visible = false;
                }
                else if(role == "hr")
                {
                    this.gvList.Columns[5].Visible = false;
                    this.gvList.Columns[6].Visible = true;
                }
                this.BindGrid();

            }           
        }
        protected void gvList_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            Label a = (Label)gvList.SelectedRow.FindControl("lblStaffID"); //need wait database then only can test  //temp
            string id = a.Text.ToString();
            Response.Redirect(string.Format("~/Project/UserProfile.aspx?id={0}", id));
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != gvList.EditIndex)
            {
                (e.Row.Cells[5].Controls[0] as ImageButton).Attributes["onclick"] = "if(!confirm('Are you sure you want to delete?')) return false;";
            }
        }

        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = gvList.Rows[e.RowIndex];

            string staff_id = gvList.DataKeys[e.RowIndex]["Staff_ID"].ToString();
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Staff_CRUD")) 
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "DELETE");
                    cmd.Parameters.AddWithValue("@Staff_ID", staff_id);    
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            this.BindGrid();
        }

        private void BindGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("Staff_CRUD"))
                {
                    string staff_id = "2";  //session[id] //temp //this id is the id of the admin or hr
                    cmd.Parameters.AddWithValue("@Action", "SELECT");
                    cmd.Parameters.AddWithValue("@Staff_ID", staff_id);  //temp
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

    }
}