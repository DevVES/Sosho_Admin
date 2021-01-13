using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Product_ProductAssignToCategory : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    string id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                string categoryqry = "SELECT CategoryId,CategoryName FROM Category where isnull(IsDeleted,0)=0 AND isnull(IsActive,0)=1 order by Sequence asc";
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

                ddlToCategoryName.DataSource = sortedDT;
                ddlToCategoryName.DataTextField = "CategoryName";
                ddlToCategoryName.DataValueField = "CategoryId";
                ddlToCategoryName.DataBind();

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

                ddlToSubCategoryName.DataSource = sortedSubCategoryDT;
                ddlToSubCategoryName.DataTextField = "SubCategory";
                ddlToSubCategoryName.DataValueField = "Id";
                ddlToSubCategoryName.DataBind();


                string JurisdictionInchargeqry = "Select Distinct JurisdictionId,JurisdictionIncharge From JurisdictionMaster where IsActive = 1 order by JurisdictionId";
                DataTable dtIncharge = dbc.GetDataTable(JurisdictionInchargeqry);
                ddlJurisdiction.DataSource = dtIncharge;
                ddlJurisdiction.DataTextField = "JurisdictionIncharge";
                ddlJurisdiction.DataValueField = "JurisdictionId";
                ddlJurisdiction.DataBind();
                DataList();
            }
            catch (Exception W) { }
        }
    }

    private void DataList()
    {
        string IsAdmin = Request.Cookies["TUser"]["IsAdmin"].ToString();
        int categoryId = Convert.ToInt32(ddlCategoryName.SelectedValue);
        int subcategoryId = Convert.ToInt32(ddlSubCategoryName.SelectedValue);
        int sJurisdictionId = Convert.ToInt32(ddlJurisdiction.SelectedValue);
        int toCategoryId = Convert.ToInt32(ddlToCategoryName.SelectedValue);
        int toSubcategoryId = Convert.ToInt32(ddlToSubCategoryName.SelectedValue);

        string query = " SELECT P.[Id],P.[Name],P.[StartDate],P.[EndDate],P.[IsActive],P.[Mrp] as MRP,P.[Offer] as Offer, " +
                       " P.[BuyWith1FriendExtraDiscount] as BuyWith1Friend, P.[BuyWith5FriendExtraDiscount] as BuyWith5Friend, PT.Name AS ProductType  " +
                       " FROM [dbo].[Product] P " +
                       " LEFT JOIN ProductTemplate PT ON PT.Id = P.ProductTemplateID " +
                       " LEFT JOIN tblCategoryProductLink L ON L.ProductId = P.Id  " +
                       " where P.IsDeleted=0 " +
                       " and P.IsApproved = 1 " +
                       " and ISNULL(P.ProductMasterId,0) = 0" +
                       " and L.CategoryID=" + categoryId +
                       " and L.SubCategoryID=" + subcategoryId +
                       " and P.JurisdictionId =" + sJurisdictionId +
                       " and ISNULL(P.JurisdictionId,0) > 0 " +
                       " and l.ProductId Not In(select productid from tblCategoryProductLink il " +
                       " inner join product ip on ip.id = il.ProductId " +
                       " where il.CategoryID = "+ toCategoryId + "  and il.SubCategoryID = "+ toSubcategoryId + " and ip.JurisdictionID = "+ sJurisdictionId + ")";
        query += " order by EndDate desc ";

        DataTable dtbannerlist = dbc.GetDataTable(query);
        gvproductlist.DataSource = dtbannerlist;
        gvproductlist.DataBind();
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        DataList();
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

    protected void OnToSelectedIndexChanged(object sender, EventArgs e)
    {
        ddlToSubCategoryName.Items.Clear();
        string SubCategoryQry = "SELECT Id,SubCategory FROM tblSubCategory where isnull(IsActive,0)=1  AND CategoryId = '" + ddlToCategoryName.SelectedValue + "' order by Id asc";
        DataTable dtSubCategory = new DataTable();
        dtSubCategory = dbc.GetDataTable(SubCategoryQry);
        dtSubCategory.Rows.Add("0", "Select SubCategory");
        DataView dvSubCategory = dtSubCategory.DefaultView;
        dvSubCategory.Sort = "Id asc";
        DataTable sortedSubCategoryDT = dvSubCategory.ToTable();
        ddlToSubCategoryName.DataSource = sortedSubCategoryDT;
        ddlToSubCategoryName.DataTextField = "SubCategory";
        ddlToSubCategoryName.DataValueField = "Id";
        ddlToSubCategoryName.DataBind();
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

    protected void BtnAdd_Click(object sender, EventArgs e)
    {
        bool flag = false;
        foreach (GridViewRow gr in gvproductlist.Rows)
        {
            string userId = Request.Cookies["TUser"]["Id"].ToString();
            bool isChecked = ((CheckBox)gr.FindControl("chkProduct")).Checked;
           // int sProductId = Convert.ToInt32(((HiddenField)gr.Cells[1].FindControl("HiddenFieldgrpid")));
            

            int sJurisdictionId = Convert.ToInt32(ddlJurisdiction.SelectedValue);
            int categoryId = Convert.ToInt32(ddlToCategoryName.SelectedValue);
            int subcategoryId = Convert.ToInt32(ddlToSubCategoryName.SelectedValue);

            if (isChecked)
            {
                HiddenField hdngrpId = ((HiddenField)gr.Cells[1].FindControl("HiddenFieldgrpid"));
                string productId = hdngrpId.Value;

                //string InsertProduct = " Insert into Product " +
                //                        " Select Name, GSTTaxId, Unit, UnitId, StartDate, EndDate, IsActive, Metatags, Metadesc, Mrp, Offer, BuyWith1FriendExtraDiscount, " +
                //                        " BuyWith5FriendExtraDiscount, FixedShipRate, KeyFeatures, Note, IsDeleted, DOC, DOM, VideoName, OGImage, ProductDiscription, JustBougth, " +
                //                        " sold, IsOrderDone, ProductMRP, DisplayNameInMsg, ShowMrpInMsg, IsQtyFreeze, DisplayOrder, CategoryID, ProductTemplateID, ProductBanner, " +
                //                        " Recommended, '" + dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss") + "' As CreatedOn, " + userId + " as CreatedBy, '' As ModifiedOn, 0 as ModifiedBy, " + sJurisdictionId + " As JurisdictionID, IsApproved, RejectedReason, ApproverID, DiscountType, " +
                //                        " Discount, SoshoPrice, MaxQty, MinQty, IsProductDescription, IsFreeShipping, IsFixedShipping, id as ProductMasterId, SubCategoryId " +
                //                        " From product where id = " + productId + "; SELECT SCOPE_IDENTITY(); ";
                //int Val = dbc.ExecuteQueryWithScalarId(InsertProduct);

                //string SelectAttrIdQry = " SELECT Id FROM Product_ProductAttribute_Mapping WHERE ProductId = " + productId;
                //DataTable dtAttributelist = dbc.GetDataTable(SelectAttrIdQry);
                //if (dtAttributelist.Rows.Count > 0)
                //{
                //    for (int nCtr = 0; nCtr < dtAttributelist.Rows.Count; nCtr++)
                //    {
                //        int AttributeId = Convert.ToInt32(dtAttributelist.Rows[nCtr]["Id"]);
                //        string prodAttrqry = " SELECT Unit, UnitId, Mrp, DiscountType, ISNULL(Discount,0) AS Discount, ISNULL(SoshoPrice,0) AS SoshoPrice, PackingType, ProductImage, " +
                //                             " ISNULL(IsActive,0) AS IsActive, ISNULL(IsDeleted,0) IsDeleted, ISNULL(isOutOfStock,0) AS isOutOfStock, " +
                //                             " ISNULL(isSelected,0) AS isSelected, ISNULL(MaxQty,0) AS MaxQty, IsNull(MinQty,0) AS MinQty, " +
                //                             " ISNULL(IsQtyFreeze,0) AS IsQtyFreeze, ISNULL(IsBestBuy,0) AS IsBestBuy, ISNULL(FreezeQty,0) AS FreezeQty" +
                //                             " FROM Product_ProductAttribute_Mapping WHERE Id =" + AttributeId;
                //        DataTable dtAttributeDetail = dbc.GetDataTable(prodAttrqry);
                //        int isDeleted = 0;
                //        int isActive = 1;
                //        int isOutOfStock = 0;
                //        int isSelected = 0;
                //        int IsQtyFreeze = 0;
                //        int isBestBuy = 0;

                //        if (Convert.ToBoolean(dtAttributeDetail.Rows[0]["IsDeleted"]) == true)
                //            isDeleted = 1;

                //        if (Convert.ToBoolean(dtAttributeDetail.Rows[0]["IsActive"]) == false)
                //            isActive = 0;

                //        if (Convert.ToBoolean(dtAttributeDetail.Rows[0]["isOutOfStock"]) == true)
                //            isOutOfStock = 1;

                //        if (Convert.ToBoolean(dtAttributeDetail.Rows[0]["isSelected"]) == true)
                //            isSelected = 1;

                //        if (Convert.ToBoolean(dtAttributeDetail.Rows[0]["IsQtyFreeze"]) == true)
                //            IsQtyFreeze = 1;

                //        if (Convert.ToBoolean(dtAttributeDetail.Rows[0]["IsBestBuy"]) == true)
                //            isBestBuy = 1;

                //        string InserAttrProduct = " Insert into Product_ProductAttribute_Mapping(ProductId, Unit, UnitId, Mrp, DiscountType, Discount, SoshoPrice, " +
                //                                  " PackingType, ProductImage, IsActive, IsDeleted, CreatedOn, CreatedBy, isOutOfStock, " +
                //                                  " isSelected, MaxQty, MinQty, IsQtyFreeze, IsBestBuy, FreezeQty, AttributeMasterId) VALUES(" +
                //                                   Val + ", '" + dtAttributeDetail.Rows[0]["Unit"] + "', " + dtAttributeDetail.Rows[0]["UnitId"] + ", " +
                //                                    dtAttributeDetail.Rows[0]["Mrp"] + ",'" + dtAttributeDetail.Rows[0]["DiscountType"] + "'," + dtAttributeDetail.Rows[0]["Discount"] +
                //                                    "," + dtAttributeDetail.Rows[0]["SoshoPrice"] + ",'" + dtAttributeDetail.Rows[0]["PackingType"] + "'," +
                //                                    "'" + dtAttributeDetail.Rows[0]["ProductImage"] + "'," + isActive + "," +
                //                                     isDeleted + ",'" + dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss") + "'," +
                //                                     userId + "," + isOutOfStock + "," + isSelected + "," +
                //                                     dtAttributeDetail.Rows[0]["MaxQty"] + "," + dtAttributeDetail.Rows[0]["MinQty"] + "," +
                //                                     IsQtyFreeze + "," + isBestBuy + "," +
                //                                     dtAttributeDetail.Rows[0]["FreezeQty"] + "," + AttributeId + ")";
                //        dbc.ExecuteQuery(InserAttrProduct);
                //    }
                //}
                DateTime dtCreatedon = DateTime.Now;
                string ProductCategoryqry = "INSERT INTO [dbo].[tblCategoryProductLink] ([ProductId],[CategoryId],[SubCategoryId],[IsActive],[IsDeleted],[CreatedDate],[CreatedBy]) VALUES (" + productId + ",'" + categoryId + "'," + subcategoryId + ",1,0,'" + dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss") + "'," + userId + ")";
                dbc.ExecuteQuery(ProductCategoryqry);
                flag = true;

            }

        }
        DataList();
        if (flag)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Product Assign to Jurisdiction Successfully')", true);
        }

    }
}