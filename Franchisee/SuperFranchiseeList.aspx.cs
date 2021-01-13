using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Franchisee_SuperFranchiseeList : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    string id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
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

        string query = "SELECT F.SuperFranchiseeID AS [Id],F.[SuperFranchiseeName],F.[SuperFranchiseeContactNumber],F.[SuperFranchiseeCustomerCode],F.[SuperFranchiseeEmailAddress], " +
                       " F.[ShortUrl] " +
                       " FROM [dbo].[SuperFranchisee] F " +
                       " where F.IsDeleted=0 " +
                       " AND F.IsActive = " + isActive;
        query += " order by F.SuperFranchiseeID ";

        DataTable dtFranchiseelist = dbc.GetDataTable(query);
        gvFranchiseelist.DataSource = dtFranchiseelist;
        gvFranchiseelist.DataBind();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        DataList();
    }
}