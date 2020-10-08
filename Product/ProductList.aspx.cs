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

                string categoryqry = "SELECT CategoryId,CategoryName FROM Category where isnull(IsDeleted,0)=0 order by Sequence asc";
                DataTable dtcategory = new DataTable();
                dtcategory = dbc.GetDataTable(categoryqry);
                dtcategory.Rows.Add("0","Select Category");
                DataView dv = dtcategory.DefaultView;
                dv.Sort = "CategoryId asc";
                DataTable sortedDT = dv.ToTable();

                ddlCategoryName.DataSource = sortedDT;
                ddlCategoryName.DataTextField = "CategoryName";
                ddlCategoryName.DataValueField = "CategoryId";
                ddlCategoryName.DataBind();

                string SubCategoryQry = "SELECT Id,SubCategory FROM tblSubCategory where isnull(IsActive,0)=1  AND CategoryId = '" + ddlCategoryName.SelectedValue + "' order by Id asc";
                DataTable dtSubCategory = new DataTable();
                dtSubCategory = dbc.GetDataTable(SubCategoryQry);
                dtSubCategory.Rows.Add("0", "Select SubCategory");
                DataView dvSubCategory = dtSubCategory.DefaultView;
                dvSubCategory.Sort = "Id asc";
                DataTable sortedSubCategoryDT = dvSubCategory.ToTable();
                ddlSubCategoryName.DataSource = sortedSubCategoryDT;
                ddlSubCategoryName.DataTextField = "SubCategory";
                ddlSubCategoryName.DataValueField = "Id";
                ddlSubCategoryName.DataBind();

                string productQry = "SELECT Id,Name FROM Product where isnull(IsActive,0)=1 AND CategoryId = '" + ddlCategoryName.SelectedValue + "' AND SubCategoryId = '" + ddlSubCategoryName.SelectedValue + "' order by Id asc";
                DataTable dtproduct = new DataTable();
                dtproduct = dbc.GetDataTable(productQry);
                dtproduct.Rows.Add("0", "Select Product");
                DataView dvProd = dtproduct.DefaultView;
                dvProd.Sort = "Id asc";
                DataTable sortedProdDT = dvProd.ToTable();
                ddlProduct.DataSource = sortedProdDT;
                ddlProduct.DataTextField = "Name";
                ddlProduct.DataValueField = "Id";
                ddlProduct.DataBind();

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
        int categoryId = Convert.ToInt32(ddlCategoryName.SelectedValue);
        int productId = Convert.ToInt32(ddlProduct.SelectedValue);
        //string query = "SELECT [Id],[Name],[StartDate],[EndDate],[IsActive],[Mrp] as MRP,[Offer] as Offer,[BuyWith1FriendExtraDiscount] as BuyWith1Friend,[BuyWith5FriendExtraDiscount] as BuyWith5Friend FROM [SalebhaiOnePage_Staging].[dbo].[Product] where IsDeleted=0  and DOC>='" + txtdt.Text + " 00:00:00' and DOC<='" + txtdt1.Text + " 23:59:59' order by EndDate desc ";
        string query = "SELECT [Id],[Name],[StartDate],[EndDate],[IsActive],[Mrp] as MRP,[Offer] as Offer,[BuyWith1FriendExtraDiscount] as BuyWith1Friend,[BuyWith5FriendExtraDiscount] as BuyWith5Friend FROM [dbo].[Product] where IsDeleted=0 and IsApproved = 1 and convert(date,DOC,103)>='" + StrPart[2] + "-" + StrPart[1] + "-" + StrPart[0] + "' and convert(date,DOC,103)<='" + StrPart1[2] + "-" + StrPart1[1] + "-" + StrPart1[0] + "'";
        if (IsAdmin == "False")
        {
            query += " and JurisdictionId=" + sJurisdictionId;
        }
        if(categoryId > 0)
        {
            query += " and CategoryID=" + categoryId;
        }
        if (productId > 0)
        {
            query += " and Id =" + productId;
        }
        query += " order by EndDate desc ";

        DataTable dtbannerlist = dbc.GetDataTable(query);
        if (dtbannerlist.Rows.Count > 0)
        {
            gvproductlist.DataSource = dtbannerlist;
            gvproductlist.DataBind();
        }
    }

    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddlSubCategoryName.Items.Clear();
        string SubCategoryQry = "SELECT Id,SubCategory FROM tblSubCategory where isnull(IsActive,0)=1  AND CategoryId = '" + ddlCategoryName.SelectedValue + "' order by Id asc";
        DataTable dtSubCategory = new DataTable();
        dtSubCategory = dbc.GetDataTable(SubCategoryQry);
        dtSubCategory.Rows.Add("0", "Select SubCategory");
        DataView dvSubCategory = dtSubCategory.DefaultView;
        dvSubCategory.Sort = "Id asc";
        DataTable sortedSubCategoryDT = dvSubCategory.ToTable();
        ddlSubCategoryName.DataSource = sortedSubCategoryDT;
        ddlSubCategoryName.DataTextField = "SubCategory";
        ddlSubCategoryName.DataValueField = "Id";
        ddlSubCategoryName.DataBind();
    }

    protected void OnSelectedIndexSubCategoryChanged(object sender, EventArgs e)
    {
        ddlProduct.Items.Clear();
        string productQry = "SELECT Id,Name FROM Product where isnull(IsActive,0)=1  AND CategoryId = '" + ddlCategoryName.SelectedValue + "' order by Id asc";
        DataTable dtproduct = new DataTable();
        dtproduct = dbc.GetDataTable(productQry);
        dtproduct.Rows.Add("0", "Select Product");
        DataView dvProd = dtproduct.DefaultView;
        dvProd.Sort = "Id asc";
        DataTable sortedProdDT = dvProd.ToTable();
        ddlProduct.DataSource = sortedProdDT;
        ddlProduct.DataTextField = "Name";
        ddlProduct.DataValueField = "Id";
        ddlProduct.DataBind();
        
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