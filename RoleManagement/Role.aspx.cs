using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;
using System.Data;
using OfficeOpenXml;


public partial class RoleManagement_Role : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                lblDateTime.Text = String.Format("{0:ddd, MMM d, yyyy}", dbc.getindiantime());
                btnsave.Text = "Save";
                if (Request.QueryString.AllKeys.Contains("ID"))
                {
                    if (!Request.QueryString["ID"].ToString().Equals(""))
                    {
                        btnsave.Text = "Update";
                        DataTable dt = dbc.GetDataTable("Select * from [Role] where id=" + Request.QueryString["ID"].ToString() + "");
                        if (dt.Rows.Count > 0)
                        {
                            txtrole.Text = dt.Rows[0]["RoleName"].ToString();                           
                        }
                    }
                }
                else if (Request.QueryString.AllKeys.Contains("DID"))
                {
                    if (!Request.QueryString["DID"].ToString().Equals(""))
                    {
                        Delete(Request.QueryString["DID"].ToString());
                    }
                }
                getdata();
            }
        }
        catch (Exception ex)
        {
            ltrErr.Text = ex.Message;
        }
    }
    protected void cmdSave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString.AllKeys.Contains("ID"))
            {
                if (!Request.QueryString["ID"].ToString().Equals(""))
                {
                    if (txtrole.Text.Trim() != "")
                    {
                        string[] ins = { txtrole.Text.Trim() };
                        int i = dbc.ExecuteQueryWithParams("update Role set RoleName=@1 where id=" + Request.QueryString["ID"].ToString() + "", ins);
                        if (i > 0)
                        {
                            Response.Redirect("~/RoleManagement/Role.aspx", false);
                        }
                    }
                    else
                    {
                        getdata();
                        ltrErr.Text = "Please fill all details";
                    }
                }
            }
            else
            {
                if (txtrole.Text.Trim() != "")
                {
                    string[] ins = { txtrole.Text.Trim()};
                    int i = dbc.ExecuteQueryWithParams("insert into Role (RoleName) values(@1)", ins);
                    if (i > 0)
                    {
                        getdata();
                        ltrErr.Text = "Inserted Successfully";
                        txtrole.Text = "";                       
                    }
                    else
                    {
                        getdata();
                        ltrErr.Text = "Not Inserted";

                    }
                }
                else
                {
                    getdata();
                    ltrErr.Text = "Please fill all details";

                }
            }
        }
        catch (Exception ex)
        {
            ltrErr.Text = ex.Message;
        }
    }

    public void getdata()
    {
        try
        {
            ltrErr.Text = "";
            DataTable dtdata = dbc.GetDataTable("Select * from Role");
            if (dtdata.Rows.Count > 0)
            {
                grd.Caption = "Total Role: " + dtdata.Rows.Count;
                grd.DataSource = dtdata;
                grd.DataBind();
            }
            else
            {
                dtdata = new DataTable();
                grd.Caption = "Total Role: " + dtdata.Rows.Count;
                grd.DataSource = dtdata;
                grd.DataBind();
            }
        }
        catch (Exception ex)
        {
            ltrErr.Text = ex.Message;
        }
    }

    public void Delete(string id)
    {
        try
        {
            int i = dbc.ExecuteQuery("Delete from Role where id=" + id + "");
            if (i > 0)
            {
                Response.Redirect("~/RoleManagement/Role.aspx", false);
            }
            else
            {
                getdata();
                ltrErr.Text = "Not Deleted";

            }
        }
        catch (Exception ex)
        {
            ltrErr.Text = ex.Message;
        }
    }
}