using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class RunQuery : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {

                DataTable dtstatus = dbc.GetDataTable("SELECT name, database_id, create_date FROM sys.databases where name not in ('master','tempdb','model','msdb') Order by name");
                ddldbname.DataSource = dtstatus;
                ddldbname.DataTextField = "name";
                ddldbname.DataValueField = "database_id";
                ddldbname.DataBind();
                ddldbname.Items.Insert(0, new ListItem("Select Database", "0"));
            }
        }
        catch (Exception ex)
        {
            Response.Redirect("~/login.aspx", false);
        }
        
    }
    protected void btnsearch_Click(object sender, EventArgs e)
    {
        string text = "";
        string dbname = "";
        try
        {
            ltrErr.Text = "";
            int did = 0;
            
            dbname = ddldbname.SelectedItem.Text.ToString();
            
            text = txtName.Text.ToString();
            string firstWord = text.Split(' ').First();
            string pass = txtpass.Value;
            if (dbname != null && dbname != "" && text != null && text != "")
            {
                if (dbname != "Select Database")
                {
                    if (pass.Equals("123"))
                    {
                        if (!firstWord.ToLower().Equals("select") && !firstWord.ToLower().Equals("show"))
                        {
                            ltrErr.Text = "Please Use Only Extarct Data Query";
                            grd.DataSource = null;
                            grd.DataBind();
                        }
                        else
                        {
                            string query = "Use " + dbname.ToString().Trim() + "; " + text;
                            DataTable dt = dbc.GetDataTable(query);
                            if (dt != null && dt.Rows.Count > 0)
                            {
                                grd.DataSource = dt;
                                ViewState["griddata"] = dt;
                                grd.DataBind();
                                grd.Caption = "Total Records: " + dt.Rows.Count;
                            }
                            else
                            {
                                ltrErr.Text = "No Record Found";
                                grd.DataSource = null;
                                grd.DataBind();
                            }
                        }
                    }
                    else
                    {
                        ltrErr.Text = "Please Enter Correct Password";
                        grd.DataSource = null;
                        grd.DataBind();
                    }
                }
                else
                {
                    ltrErr.Text = "Please Select Database";
                    grd.DataSource = null;
                    grd.DataBind();
                }
            }
            else
            {
                ltrErr.Text = "Please Fill All Details";
                grd.DataSource = null;
                grd.DataBind();
            }

        }
        catch (Exception ex)
        {
            ltrErr.Text = ex.Message;
        }
        finally
        {
        }
    }
    protected void btnExport_Click(object sender, EventArgs e)
    {
        try
        {
            DataTable dt = (DataTable)ViewState["griddata"];
            DataSet ds = new DataSet("table");
            ds.Tables.Add(dt);
            ExportToExcel e2e = new ExportToExcel();
            e2e.GenerateExcel(ds, "Report", false);//"From:" + startdate.Value + " To " + enddate.Value + " Generated On "
            
        }
        catch (Exception ex)
        {
        }
    }
    
    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.TableSection = TableRowSection.TableHeader;
        }
    }
}