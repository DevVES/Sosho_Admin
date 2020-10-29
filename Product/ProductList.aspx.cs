using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
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
                dtcategory.Rows.Add("0", "Select Category");
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
        string query = "SELECT P.[Id],P.[Name],P.[StartDate],P.[EndDate],P.[IsActive],P.[Mrp] as MRP,P.[Offer] as Offer, " +
                       " P.[BuyWith1FriendExtraDiscount] as BuyWith1Friend, P.[BuyWith5FriendExtraDiscount] as BuyWith5Friend, PT.Name AS ProductType  " +
                       " FROM [dbo].[Product] P " +
                       " LEFT JOIN ProductTemplate PT ON PT.Id = P.ProductTemplateID " +
                       " where IsDeleted=0 " +
                       " and IsApproved = 1 and convert(date,DOC,103)>='" +
                       StrPart[2] + "-" + StrPart[1] + "-" + StrPart[0] + "' and convert(date,DOC,103)<='"
                       + StrPart1[2] + "-" + StrPart1[1] + "-" + StrPart1[0] + "'";
        if (IsAdmin == "False")
        {
            query += " and JurisdictionId=" + sJurisdictionId;
        }
        if (categoryId > 0)
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

    protected void GridView1_RowEditing(object sender, System.Web.UI.WebControls.GridViewEditEventArgs e)
    {
        grdgProduct.EditIndex = e.NewEditIndex;
        this.BindGrid();
       
    }
    protected void OnUpdate(object sender, EventArgs e)
    {
        GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;
        HiddenField hdngrpId = ((HiddenField)row.Cells[1].FindControl("HiddenFieldgrpid"));
        string grpId = hdngrpId.Value;
        string unit = (row.Cells[1].Controls[0] as TextBox).Text;
        decimal mrp = Convert.ToDecimal((row.Cells[2].Controls[0] as TextBox).Text);
        string discountType = (row.Cells[3].Controls[0] as TextBox).Text;
        decimal grpDiscount = Convert.ToDecimal((row.Cells[4].Controls[0] as TextBox).Text);
        decimal grpSoshoPrice = Convert.ToDecimal((row.Cells[5].Controls[0] as TextBox).Text);
        string grpPackingType = (row.Cells[6].Controls[0] as TextBox).Text;
        DataTable dt = ViewState["dt"] as DataTable;
        string productid = dt.Rows[0]["Id"].ToString();

        if (discountType == "%")
        {
            grpSoshoPrice = mrp - (( mrp * grpDiscount) / 100);
        }
        else if (discountType == "Fixed")
        {
            grpSoshoPrice = mrp - grpDiscount;
        }
       

        dt.Rows[row.RowIndex]["grpUnit"] = unit;
        dt.Rows[row.RowIndex]["grpMrp"] = mrp;
        dt.Rows[row.RowIndex]["grpDiscountType"] = discountType;
        dt.Rows[row.RowIndex]["grpDiscount"] = grpDiscount;
        dt.Rows[row.RowIndex]["grpSoshoPrice"] = grpSoshoPrice;
        dt.Rows[row.RowIndex]["grpPackingType"] = grpPackingType;
        dt.Rows[row.RowIndex]["grpId"] = grpId;
        ViewState["dt"] = dt;
        string updateqty = "UPDATE [Product_ProductAttribute_Mapping] set Unit = '" + unit + "',Mrp='" + mrp + "',DiscountType='" + discountType + "',Discount='" + grpDiscount + "',SoshoPrice='" + grpSoshoPrice + "',PackingType='" + grpPackingType + "' Where Id=" + grpId + " and ProductId = " + productid;
        int VALupdate = dbc.ExecuteQuery(updateqty);
        grdgProduct.EditIndex = -1;
        this.BindGrid();
        
    }
    protected void BindGrid()
    {
        grdgProduct.DataSource = ViewState["dt"] as DataTable;
        grdgProduct.DataBind();
        ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal();", true);
        
    }
    protected void gvproductlist_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Price")
        {
            LinkButton lnkView = (LinkButton)e.CommandSource;
            string productId = lnkView.CommandArgument;
            DataTable dtProductAttribute = dbc.GetDataTable("SELECT UnitId as grpUnitId,um.UnitName as grpUnitName,Unit as grpUnit,Mrp as grpMrp,DiscountType as grpDiscountType,Discount as grpDiscount,SoshoPrice as grpSoshoPrice,PackingType as grpPackingType,ProductImage as grpImage,ProductId as Id,pam.Id as grpId,pam.isOutofStock as grpisOutOfStock,pam.isSelected as grpisSelected,'Update' as Status, ISNULL(pam.MinQty,1) as grpMinQty, ISNULL(pam.MaxQty,99) as grpMaxQty, ISNULL(pam.IsQtyFreeze,0) as grpIsQtyFreeze, ISNULL(pam.IsBestBuy,0) AS grpisBestBuy, ISNULL(pam.FreezeQty,0) AS grpFreezeQty  FROM [dbo].[Product_ProductAttribute_Mapping] pam INNER JOIN [UnitMaster] um on pam.UnitId=um.Id  where pam.IsDeleted=0 and ProductId=" + productId);
            if (dtProductAttribute.Rows.Count > 0)
            {
                ViewState["dt"] = dtProductAttribute;
                grdgProduct.DataSource = dtProductAttribute;
                grdgProduct.DataBind();
                string productid = dtProductAttribute.Rows[0]["Id"].ToString();
                DataTable dtProductName = dbc.GetDataTable("select Name from Product where Id=" + productid);
                if (dtProductName != null & dtProductName.Rows.Count > 0)
                {
                    lblProdName.InnerHtml = dtProductName.Rows[0]["Name"].ToString(); ;
                }
            }
            ClientScript.RegisterStartupScript(this.GetType(), "Pop", "openModal();", true);
        }

    }
    protected void gvproductlist_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.TableSection = TableRowSection.TableHeader;
        }
    }

    //protected void OnTextChangedMRP(object sender, EventArgs e)
    //{
    //    TextBox textBox = sender as TextBox;
    //    string mrp = textBox.Text;
        
    //    BindGrid();
    //}
}