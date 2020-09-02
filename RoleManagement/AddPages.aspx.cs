using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;
using System.Data;
using OfficeOpenXml;

public partial class RoleManagement_AddPages : System.Web.UI.Page
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
                        DataTable dt = dbc.GetDataTable("Select * from Pages where id=" + Request.QueryString["ID"].ToString() + "");
                        if (dt.Rows.Count > 0)
                        {
                            txtname.Text = dt.Rows[0]["Name"].ToString();
                            txturl.Text = dt.Rows[0]["PageUrl"].ToString();
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
            ltrerr.Text = ex.Message;
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            if (Request.QueryString.AllKeys.Contains("ID"))
            {
                if (!Request.QueryString["ID"].ToString().Equals(""))
                {
                    if (txtname.Text.Trim() != "" && txturl.Text.Trim() != "")
                    {
                        string[] ins = { txtname.Text.Trim(), txturl.Text.Trim() };
                        int i = dbc.ExecuteQueryWithParams("update Pages set Name=@1,PageUrl=@2 where id=" + Request.QueryString["ID"].ToString() + "", ins);
                        if (i > 0)
                        {
                            Response.Redirect("~/RoleManagement/AddPages.aspx", false);
                        }
                    }
                    else
                    {
                        getdata();
                        ltrerr.Text = "Please fill all details";                        
                    }
                }
            }
            else
            {
                if (txtname.Text.Trim() != "" && txturl.Text.Trim() != "")
                {
                    string[] ins = { txtname.Text.Trim(), txturl.Text.Trim() };
                    int i = dbc.ExecuteQueryWithParams("insert into Pages (Name,PageUrl,doc) values(@1,@2,Dateadd(Minute,330,Getutcdate()))", ins);
                    if (i > 0)
                    {
                        getdata();
                        ltrerr.Text = "Inserted Successfully";
                        txtname.Text = "";
                        txturl.Text = "";
                       
                    }
                    else
                    {
                        getdata();
                        ltrerr.Text = "Not Inserted";                       
                        
                    }
                }
                else
                {
                    getdata();
                    ltrerr.Text = "Please fill all details";
                    
                }
            }
        }
        catch (Exception ex)
        {            
           ltrerr.Text = ex.Message;
        }
    }

    public void getdata()
    {
        try
        {
            ltrerr.Text = "";
            DataTable dtdata = dbc.GetDataTable("Select Id,Name,PageUrl,Convert(varchar,doc,106) as doc from Pages");
            if (dtdata.Rows.Count > 0)
            {
                GridView1.Caption = "Total Pages: " + dtdata.Rows.Count;
                GridView1.DataSource = dtdata;
                GridView1.DataBind();
            }
            else
            {
                dtdata = new DataTable();
                GridView1.Caption = "Total Pages: " + dtdata.Rows.Count;
                GridView1.DataSource = dtdata;
                GridView1.DataBind();
            }
        }
        catch (Exception ex)
        {
            ltrerr.Text = ex.Message;
        }
    }

    public void Delete(string id)
    {
        try
        {
            int i = dbc.ExecuteQuery("Delete from Pages where id=" + id + "");
            if (i > 0)
            {
                Response.Redirect("~/RoleManagement/AddPages.aspx", false);
            }
            else
            {
                getdata();
                ltrerr.Text = "Not Deleted";
               
            }
        }
        catch (Exception ex)
        {
            ltrerr.Text = ex.Message;
        }
    }
}