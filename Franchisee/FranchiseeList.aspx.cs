using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Franchisee_FranchiseeList : System.Web.UI.Page
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

                string masterFranchiseeqry = "Select MasterFranchiseeID as Id,MasterFranchiseeName as Name from MasterFranchisee where IsActive = 1 order by MasterFranchiseeID";
                DataTable dtMFranchisee = dbc.GetDataTable(masterFranchiseeqry);
                ddlMasterFranchisee.DataSource = dtMFranchisee;
                ddlMasterFranchisee.DataTextField = "Name";
                ddlMasterFranchisee.DataValueField = "Id";
                ddlMasterFranchisee.DataBind();
                ddlMasterFranchisee.Items.Insert(0, new ListItem("Select Master Franchisee", ""));

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

        //string IsAdmin = Request.Cookies["TUser"]["IsAdmin"].ToString();
        //string sJurisdictionId = Request.Cookies["TUser"]["JurisdictionID"].ToString();
        int masterFranchiseeId = 0, superFranchiseeId = 0;
        if(ddlMasterFranchisee.SelectedValue != "")
            masterFranchiseeId = Convert.ToInt32(ddlMasterFranchisee.SelectedValue);

        if (ddlSuperFranchisee.SelectedValue != "")
            superFranchiseeId = Convert.ToInt32(ddlSuperFranchisee.SelectedValue);

        string query = "SELECT F.FranchiseeID AS [Id],F.[FranchiseeName],F.[FranchiseeContactNumber],F.[FranchiseeEmailAddress],F.[FranchiseeCustomerCode], " +
                       " F.[ShortUrl], SF.[SuperFranchiseeName], MF.[MasterFranchiseeName]  " +
                       " FROM [dbo].[Franchisee] F " +
                       " LEFT JOIN SuperFranchisee SF ON SF.SuperFranchiseeID= F.SuperFranchiseeID " +
                       " LEFT JOIN MasterFranchisee MF ON MF.MasterFranchiseeID = F.MasterFranchiseeID " +
                       " where F.IsDeleted=0 " +
                       " AND F.IsActive = " + isActive;
        if (masterFranchiseeId > 0)
        {
            query += " and MF.MasterFranchiseeID=" + masterFranchiseeId;
        }
        if (superFranchiseeId > 0)
        {
            query += " and SF.SuperFranchiseeID=" + superFranchiseeId;
        }
        
        query += " order by F.FranchiseeID ";

        DataTable dtFranchiseelist = dbc.GetDataTable(query);
        gvFranchiseelist.DataSource = dtFranchiseelist;
        gvFranchiseelist.DataBind();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        DataList();
    }

    
}