using System;
using System.Web.UI.WebControls;
using WebApplication1;
using System.Data;
public partial class Product_ApprovedProductList : System.Web.UI.Page
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

                DataList(false);
            }
            catch (Exception W) { }
        }
    }
    private void DataList(bool bclick)
    {
        String from = txtdt.Text.ToString();
        String to = txtdt1.Text.ToString();

        String[] StrPart = from.Split('/');

        String[] StrPart1 = to.Split('/');
        string query = "SELECT [Id],[Name],[StartDate],[EndDate],Product.IsActive AS IsActive,[Mrp] as MRP,[Offer] as Offer,[BuyWith1FriendExtraDiscount] as BuyWith1Friend,[BuyWith5FriendExtraDiscount] as BuyWith5Friend,Product.JurisdictionId,JM.JurisdictionIncharge FROM [dbo].[Product] LEFT JOIN JurisdictionMaster JM on JM.JurisdictionId = Product.JurisdictionId where Product.IsDeleted=0 and isnull(IsApproved,0) = 0 ";
        if (bclick)
            query += " and convert(date,DOC,103)>='" + StrPart[2] + "-" + StrPart[1] + "-" + StrPart[0] + "' and convert(date,DOC,103)<='" + StrPart1[2] + "-" + StrPart1[1] + "-" + StrPart1[0] + "'";

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
        DataList(true);
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