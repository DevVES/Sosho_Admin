using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Product_ProductList : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    string id = "";
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
        String from = txtdt.Text.ToString();
        String to = txtdt1.Text.ToString();

        String[] StrPart = from.Split('/');

        String[] StrPart1 = to.Split('/');

        string IsAdmin = Request.Cookies["TUser"]["IsAdmin"].ToString();
        string sJurisdictionId = Request.Cookies["TUser"]["JurisdictionID"].ToString();

        //string query = "SELECT [Id],[Name],[StartDate],[EndDate],[IsActive],[Mrp] as MRP,[Offer] as Offer,[BuyWith1FriendExtraDiscount] as BuyWith1Friend,[BuyWith5FriendExtraDiscount] as BuyWith5Friend FROM [SalebhaiOnePage_Staging].[dbo].[Product] where IsDeleted=0  and DOC>='" + txtdt.Text + " 00:00:00' and DOC<='" + txtdt1.Text + " 23:59:59' order by EndDate desc ";
        string query = "SELECT [Id],[Name],[StartDate],[EndDate],[IsActive],[Mrp] as MRP,[Offer] as Offer,[BuyWith1FriendExtraDiscount] as BuyWith1Friend,[BuyWith5FriendExtraDiscount] as BuyWith5Friend FROM [dbo].[Product] where IsDeleted=0 and IsApproved = 1 and convert(date,DOC,103)>='" + StrPart[2] + "-" + StrPart[1] + "-" + StrPart[0] + "' and convert(date,DOC,103)<='" + StrPart1[2] + "-" + StrPart1[1] + "-" + StrPart1[0] + "'";
        if (IsAdmin == "False")
        {
            query += " and JurisdictionId=" + sJurisdictionId;
        }
        query += " order by EndDate desc ";

        DataTable dtbannerlist = dbc.GetDataTable(query);
        if (dtbannerlist.Rows.Count > 0)
        {
            gvproductlist.DataSource = dtbannerlist;
            gvproductlist.DataBind();
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        DataList();
    }
    protected void gvproductlist_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }
    protected void gvproductlist_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.TableSection = TableRowSection.TableHeader;
        }
    }
}