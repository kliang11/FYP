using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace FYP.Project
{
    public partial class rfid : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                this.BindGrid();
                ddlBind();
            }
        }
        private void BindGrid()
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("RFID_CRUD"))
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

            string card_id = gvList.DataKeys[e.RowIndex]["TagID"].ToString();  
            string card_status;
            string dateString = (row.FindControl("txtDate") as TextBox).Text;
            string staff_id = (row.FindControl("ddlEditName") as DropDownList).SelectedItem.Value.ToString();

            if (staff_id == "")
            {
                card_status = "Available";
            }
            else
            {
                card_status = "In Use";
            }

            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("RFID_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "UPDATE");
                    cmd.Parameters.AddWithValue("@Tag_ID", card_id);
                    cmd.Parameters.AddWithValue("@Tag_Status", card_status);
                    cmd.Parameters.AddWithValue("@Staff_ID", staff_id);
                    cmd.Parameters.AddWithValue("@Date", Convert.ToDateTime(dateString));
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            gvList.EditIndex = -1;
            this.BindGrid();
            ddlBind();
        }
        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != gvList.EditIndex)
            {
                if ((e.Row.FindControl("lblCardStatus") as Label).Text.Equals("In Use"))
                    (e.Row.FindControl("lblCardStatus") as Label).CssClass = "label_red";
                else if ((e.Row.FindControl("lblCardStatus") as Label).Text.Equals("Available"))
                    (e.Row.FindControl("lblCardStatus") as Label).CssClass = "label_green";
                if ((e.Row.FindControl("lblStaffName") as Label).Text.Equals(""))
                    (e.Row.FindControl("lblStaffName") as Label).Text = "-";
                (e.Row.Cells[4].Controls[2] as ImageButton).Attributes["onclick"] = "if(!confirm('Are you sure you want to delete?')) return false;";

            }
            if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex == gvList.EditIndex)
            {
                (e.Row.FindControl("txtDate") as TextBox).Text = DateTime.Now.ToString("yyyy-MM-dd");
                DropDownList ddList = (DropDownList)e.Row.FindControl("ddlEditName");
                string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                SqlConnection con = new SqlConnection(constr);
                SqlDataAdapter adpt = new SqlDataAdapter("SELECT Staff.name, Staff.Staff_ID FROM Staff LEFT JOIN RFID ON RFID.StaffID = Staff.Staff_ID WHERE RFID.StaffID IS NULL", con);
                DataTable dt = new DataTable();
                adpt.Fill(dt);
                ddList.DataSource = dt;
                ddList.DataBind();
                ddList.DataTextField = "name";
                ddList.DataValueField = "Staff_ID";
                ddList.DataBind();
                ddList.Items.Insert(0, new ListItem("- None -", ""));

            }
        }


        protected void OnRowDeleting(object sender, GridViewDeleteEventArgs e)
        {

            GridViewRow row = gvList.Rows[e.RowIndex];

            string card_id = gvList.DataKeys[e.RowIndex]["TagID"].ToString();
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("RFID_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "DELETE");
                    cmd.Parameters.AddWithValue("@Tag_ID", card_id);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
            }
            this.BindGrid();
            ddlBind();
        }
        //need to handle the duplicate primary key exception 
        protected void btnConfirm_Click(object sender, EventArgs e)
        {
            string card_id = txt_rfidid.Text.ToString();
            DateTime reg_date = Convert.ToDateTime(txt_date.Text.ToString());
            string card_status;
            string staff_id = ddlStaffName.SelectedItem.Value.ToString();

            if (staff_id == "")
            {
                card_status = "Available";
                staff_id = "-";
            }
            else
            {
                card_status = "In Use";
            }

            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand("RFID_CRUD"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Action", "INSERT");
                    cmd.Parameters.AddWithValue("@Tag_ID", card_id);
                    cmd.Parameters.AddWithValue("@Tag_Status", card_status);
                    cmd.Parameters.AddWithValue("@Staff_ID", staff_id);
                    cmd.Parameters.AddWithValue("@Date", reg_date);
                    cmd.Connection = con;
                    con.Open();
                    try{
                        cmd.ExecuteNonQuery();
                    }
                    catch (System.Data.SqlClient.SqlException) 
                    {
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('The Card ID is existed! Please try agian.')", true);
                    }
                    con.Close();
                }
            }
            BindGrid();
            ddlBind();
            txt_date.Text = "";
            txt_rfidid.Text = "";
            ddlStaffName.SelectedIndex = 0;

        }

        private void ddlBind()
        {
            string constr = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            SqlDataAdapter adpt = new SqlDataAdapter("SELECT Staff.name, Staff.Staff_ID FROM Staff LEFT JOIN RFID ON RFID.StaffID = Staff.Staff_ID WHERE RFID.StaffID IS NULL", con);
            DataTable dt = new DataTable();
            adpt.Fill(dt);
            ddlStaffName.DataSource = dt;
            ddlStaffName.DataBind();
            ddlStaffName.DataTextField = "name";
            ddlStaffName.DataValueField = "Staff_ID";
            ddlStaffName.DataBind();
        }

        protected void ddlStaffName_DataBound(object sender, EventArgs e)
        {
            ddlStaffName.Items.Insert(0, new ListItem("- Select -", ""));
        }

    }

}