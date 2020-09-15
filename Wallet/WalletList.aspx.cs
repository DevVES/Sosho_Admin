using System;
using System.Data;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Wallet_WalletList : System.Web.UI.Page
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

        string query = "SELECT [wallet_id] AS Id,[campaign_name],[coupon_code],[per_type],[per_amount],[min_order_amount],[start_date],[end_date],[is_active] FROM [dbo].[WalletMaster] where ISNULL(is_deleted,0)=0 and convert(date,start_date,103)>='" + StrPart[2] + "-" + StrPart[1] + "-" + StrPart[0] + "' and convert(date,start_date,103)<='" + StrPart1[2] + "-" + StrPart1[1] + "-" + StrPart1[0] + "'";
        query += " order by end_date desc ";

        DataTable dtwalletlist = dbc.GetDataTable(query);
        if (dtwalletlist.Rows.Count > 0)
        {
            gvwalletlist.DataSource = dtwalletlist;
            gvwalletlist.DataBind();
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        DataList();
    }
    protected void gvwalletlist_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }
    protected void gvwalletlist_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.TableSection = TableRowSection.TableHeader;
        }
    }
}