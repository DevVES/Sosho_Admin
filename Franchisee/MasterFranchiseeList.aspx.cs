using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Franchisee_MasterFranchiseeList : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    string id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {

                string superFranchiseeqry = "Select SuperFranchiseeID as Id,SuperFranchiseeName as Name from SuperFranchisee where IsActive = 1 order by SuperFranchiseeID";
                DataTable dtSFranchisee = dbc.GetDataTable(superFranchiseeqry);
                ddlSuperFranchisee.DataSource = dtSFranchisee;
                ddlSuperFranchisee.DataTextField = "Name";
                ddlSuperFranchisee.DataValueField = "Id";
                ddlSuperFranchisee.DataBind();
                ddlSuperFranchisee.Items.Insert(0, new ListItem("Select Super Franchisee", ""));


                DataList();
            }
            catch (Exception W) { }
        }
    }

    private void DataList()
    {
        int isActive = 0;
        if (chkisactive.Checked)
        {
            isActive = 1;
        }

        int superFranchiseeId = 0;
        if (ddlSuperFranchisee.SelectedValue != "")
            superFranchiseeId = Convert.ToInt32(ddlSuperFranchisee.SelectedValue);

        string query = "SELECT F.MasterFranchiseeID AS [Id],F.[MasterFranchiseeName],F.[MasterFranchiseeContactNumber],F.[MasterFranchiseeCustomerCode],F.[MasterFranchiseeEmailAddress], " +
                       " F.[ShortUrl], SF.[SuperFranchiseeName] " +
                       " FROM [dbo].[MasterFranchisee] F " +
                       " LEFT JOIN SuperFranchisee SF ON SF.SuperFranchiseeID= F.SuperFranchiseeID " +
                       " where F.IsDeleted=0 " +
                       " AND F.IsActive = " + isActive;
        if (superFranchiseeId > 0)
        {
            query += " and SF.SuperFranchiseeID=" + superFranchiseeId;
        }

        query += " order by F.MasterFranchiseeID ";

        DataTable dtFranchiseelist = dbc.GetDataTable(query);
        gvFranchiseelist.DataSource = dtFranchiseelist;
        gvFranchiseelist.DataBind();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        DataList();
    }
}