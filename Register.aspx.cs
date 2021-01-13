using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Register : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                txtdt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtdt1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                DataList();
            }
            catch (Exception W) { }
        }
    }

    private void DataList()
    {
        string UserType = Request.Cookies["TUser"]["UserType"].ToString();
        string FranchiseeId = Request.Cookies["TUser"]["FranchiseeId"].ToString();
        String from = txtdt.Text.ToString();
        String to = txtdt1.Text.ToString();

        String[] StrPart = from.Split('/');

        String[] StrPart1 = to.Split('/');
        string query = string.Empty;
        if (UserType == "4")
        {
            query = "SELECT C.Id, C.DOC as [RegistrationDate], C.Mobile, isnull(C.FirstName, '') as FirstName, isnull(C.LastName, '') as LastName " +
                    " FROM [customer_franchise_link] FL " + 
                    " INNER JOIN Franchisee F ON F.FranchiseeCustomerCode = FL.fcode" +
                    " LEFT OUTER JOIN Customer C ON C.Mobile = FL.Mobile " +
                    " where convert(date, FL.CreatedOn, 103) >= '" + StrPart[2] + "-" + StrPart[1] + "-" + StrPart[0] + "' and convert(date, FL.CreatedOn,103)<= '" + StrPart1[2] + "-" + StrPart1[1] + "-" + StrPart1[0] + "'" +
                    " AND F.FranchiseeId = " + FranchiseeId +
                    " order by C.Id desc ";
        }
        else if (UserType == "5")
        {
            query = "SELECT C.Id, C.DOC as [RegistrationDate], C.Mobile, isnull(C.FirstName, '') as FirstName, isnull(C.LastName, '') as LastName " +
                    " FROM [customer_franchise_link] FL " +
                    " LEFT OUTER JOIN Customer C ON C.Mobile = FL.Mobile " +
                    " where convert(date, FL.CreatedOn, 103) >= '" + StrPart[2] + "-" + StrPart[1] + "-" + StrPart[0] + "' and convert(date, FL.CreatedOn,103)<= '" + StrPart1[2] + "-" + StrPart1[1] + "-" + StrPart1[0] + "'" +
                    "   AND (FL.fcode in (select M.MasterFranchiseeCustomerCode from MasterFranchisee M where M.MasterFranchiseeId = " + FranchiseeId + ") " +
                    "        OR FL.fcode in (select F.FranchiseeCustomerCode from Franchisee F where F.MasterFranchiseeId = " + FranchiseeId + ")) "+
            " order by C.Id desc ";
        }
        else if (UserType == "6")
        {
            query = "SELECT C.Id, C.DOC as [RegistrationDate], C.Mobile, isnull(C.FirstName, '') as FirstName, isnull(C.LastName, '') as LastName " +
                    " FROM [customer_franchise_link] FL " +
                    " LEFT OUTER JOIN Customer C ON C.Mobile = FL.Mobile " +
                    " where convert(date, FL.CreatedOn, 103) >= '" + StrPart[2] + "-" + StrPart[1] + "-" + StrPart[0] + "' and convert(date, FL.CreatedOn,103)<= '" + StrPart1[2] + "-" + StrPart1[1] + "-" + StrPart1[0] + "'" +
                    "   AND (FL.fcode in (select S.SuperFranchiseeCustomerCode from SuperFranchisee S where S.SuperFranchiseeId = " + FranchiseeId + ") " +
                        "        OR FL.fcode in (select M.MasterFranchiseeCustomerCode from MasterFranchisee M where M.SuperFranchiseeId = " + FranchiseeId + ") " +
                        "        OR FL.fcode in (select FL.FranchiseeCustomerCode from Franchisee FL where FL.MasterFranchiseeId IN (select ML.MasterFranchiseeId from MasterFranchisee ML where ML.SuperFranchiseeId = " + FranchiseeId + ")) " +
                    "        OR FL.fcode in (select F.FranchiseeCustomerCode from Franchisee F where F.SuperFranchiseeId = " + FranchiseeId + ")) "+
                   " order by C.Id desc ";
        }
        else
        {
            query = "SELECT Id, DOC as [RegistrationDate], Mobile, isnull(FirstName, '') as FirstName, isnull(LastName, '') as LastName " +
                    " from customer " +
                    " where convert(date, DOC, 103) >= '" + StrPart[2] + "-" + StrPart[1] + "-" + StrPart[0] + "' and convert(date, DOC,103)<= '" + StrPart1[2] + "-" + StrPart1[1] + "-" + StrPart1[0] + "'" +
                    " order by Id desc ";
        }
        DataTable dtReglist = dbc.GetDataTable(query);
        if (dtReglist.Rows.Count > 0)
        {
            gvRegisterlist.DataSource = dtReglist;
            gvRegisterlist.DataBind();
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        DataList();
    }

    protected void gvRegisterlist_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.TableSection = TableRowSection.TableHeader;
        }
    }
}