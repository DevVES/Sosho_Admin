using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;
using System.Data;
using OfficeOpenXml;

public partial class RoleManagement_User : System.Web.UI.Page
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
                        DataTable dt = dbc.GetDataTable("Select * from [Users] where id=" + Request.QueryString["ID"].ToString() + "");
                        if (dt.Rows.Count > 0)
                        {
                            txtusername.Text = dt.Rows[0]["UserName"].ToString();
                            txtpwd.Text = dt.Rows[0]["Password"].ToString();
                            txtnumber.Text = dt.Rows[0]["mobile_number"].ToString();
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
                    if (txtusername.Text.Trim() != "" && txtpwd.Text.Trim() != "" && txtnumber.Text.Trim() != "")
                    {
                        DataTable dtcheck = dbc.GetDataTable("select * from Users where UserName = '" + txtusername.Text.Trim() + "' and id <> "+Request.QueryString["ID"].ToString()+"");
                        if (dtcheck.Rows.Count > 0)
                        {
                            ltrErr.Text = "Username already exist, Please use different username.";
                            return;
                        }
                        string[] ins = { txtusername.Text.Trim(), txtpwd.Text.Trim(), txtnumber.Text.Trim() };
                        int i = dbc.ExecuteQueryWithParams("update Users set Username=@1,Password=@2,mobile_number=@3 where id=" + Request.QueryString["ID"].ToString() + "", ins);
                        if (i > 0)
                        {
                            Response.Redirect("~/RoleManagement/User.aspx", false);
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
                if (txtusername.Text.Trim() != "" && txtpwd.Text.Trim() != "" && txtnumber.Text.Trim() != "")
                {
                    DataTable dtcheck = dbc.GetDataTable("select * from Users where UserName = '" + txtusername.Text.Trim() + "' ");
                    if(dtcheck.Rows.Count>0)
                    {
                        ltrErr.Text = "Username already exist, Please use different username.";
                        return;
                    }
                    string[] ins = { txtusername.Text.Trim(), txtpwd.Text.Trim(), txtnumber.Text.Trim() };
                    int i = dbc.ExecuteQueryWithParams("insert into Users (Username,Password,mobile_number) values(@1,@2,@3)", ins);
                    if (i > 0)
                    {
                        getdata();
                        ltrErr.Text = "Inserted Successfully";
                        txtusername.Text = "";
                        txtpwd.Text = "";
                        txtnumber.Text = "";
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
            DataTable dtdata = dbc.GetDataTable("Select Id,Username,Password,mobile_number from Users");
            if (dtdata.Rows.Count > 0)
            {
                grd.Caption = "Total User: " + dtdata.Rows.Count;
                grd.DataSource = dtdata;
                grd.DataBind();
            }
            else
            {
                dtdata = new DataTable();
                grd.Caption = "Total User: " + dtdata.Rows.Count;
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
            int i = dbc.ExecuteQuery("Delete from Users where id=" + id + "");
            if (i > 0)
            {
                Response.Redirect("~/RoleManagement/User.aspx", false);
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