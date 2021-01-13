using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Banner_HomePageBannerList : System.Web.UI.Page
{
    public class BannerType
    {
        // Auto-Initialized properties  
        public string Text { get; set; }
        public int Value { get; set; }
    }
    dbConnection dbc = new dbConnection();
    string id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                List<BannerType> ActionList = new List<BannerType>
                {
                    new BannerType { Text = "Select Banner Type", Value = 0},
                    new BannerType { Text = "Home Page Banner", Value = 1},
                    new BannerType { Text = "Intermediate Banner", Value = 2},
                    new BannerType { Text = "All", Value = 3}
                };
                ddlBannerType.DataTextField = "Text";
                ddlBannerType.DataValueField = "Value";
                ddlBannerType.DataSource = ActionList;
                ddlBannerType.DataBind();

                // txtdt.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                //  txtdt1.Text = DateTime.Now.ToString("dd/MMM/yyyy");

                DataList();
            }
            catch (Exception W) { }
        }
    }
    private void DataList()
    {
        //String from = txtdt.Text.ToString();
        //String to = txtdt1.Text.ToString();
        string type = ddlBannerType.SelectedValue;

        int isActive = 0;
        if (chkisactive.Checked)
        {
            isActive = 1;
        }


        // String[] StrPart = from.Split('/');

        //   String[] StrPart1 = to.Split('/');
        string query = "";
        if (type == "1")
        {
            query = " SELECT  'HomePage' AS Type,'1' AS TypeId,[Id],[Title],[Link],[StartDate],[EndDate],[IsActive],[ImageName] FROM [dbo].[HomepageBanner] " + 
                    " where IsDeleted=0  AND IsActive = " + isActive + 
                    " order by sequence ";
            //  and StartDate>=CONVERT(DATETIME, '" + StrPart[2] + "-" + StrPart[1] + "-" + StrPart[0] + " 00:00:00', 102) and StartDate<=CONVERT(DATETIME, '" + StrPart1[2] + "-" + StrPart1[1] + "-" + StrPart1[0] + " 23:59:59', 102)  
        }
        if (type == "2")
        {
            query = "SELECT 'Intermediate' AS Type,'2' AS TypeId, [Id],[Title],[AltText],[Link],[StartDate],[EndDate],[IsActive],[ImageName],[TypeId],[CategoryID],[ProductId] FROM [dbo].[IntermediateBanners] " +
                    " where IsDeleted=0 AND IsActive = " + isActive  + 
                    //and StartDate>=CONVERT(DATETIME, '" + StrPart[2] + "-" + StrPart[1] + "-" + StrPart[0] + " 00:00:00', 102) and StartDate<=CONVERT(DATETIME, '" + StrPart1[2] + "-" + StrPart1[1] + "-" + StrPart1[0] + " 23:59:59', 102)  
                    " order by sequence "; 
        }
        if (type == "3" || type == "0")
        {
            query = "SELECT 'HomePage' AS Type,'1' AS TypeId, [Id],[Title],[Link],[StartDate],[EndDate],[IsActive],[ImageName] FROM [dbo].[HomepageBanner] " +
                    " where IsDeleted=0 AND IsActive = " + isActive  + 
                    //and StartDate>=CONVERT(DATETIME, '" + StrPart[2] + "-" + StrPart[1] + "-" + StrPart[0] + " 00:00:00', 102) and StartDate<=CONVERT(DATETIME, '" + StrPart1[2] + "-" + StrPart1[1] + "-" + StrPart1[0] + " 23:59:59', 102) " +
                    " Union All "+
                    "SELECT  'Intermediate' AS Type,'2' AS TypeId, [Id],[Title],[Link],[StartDate],[EndDate],[IsActive],[ImageName] FROM [dbo].[IntermediateBanners] " +
                    " where IsDeleted=0 AND IsActive = " + isActive + 
                    //and StartDate>=CONVERT(DATETIME, '" + StrPart[2] + "-" + StrPart[1] + "-" + StrPart[0] + " 00:00:00', 102) and StartDate<=CONVERT(DATETIME, '" + StrPart1[2] + "-" + StrPart1[1] + "-" + StrPart1[0] + " 23:59:59', 102)  
                    " order by EndDate desc ";
}

       DataTable dtbannerlist = dbc.GetDataTable(query);
        if (dtbannerlist.Rows.Count > 0)
        {
            gvbannerlist.DataSource = dtbannerlist;
            gvbannerlist.DataBind();
        }
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        DataList();
    }
    protected void gvbannerlist_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }
    protected void gvbannerlist_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.TableSection = TableRowSection.TableHeader;
        }
    }
}