using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Product_ProductRemoveFromJuridiction : System.Web.UI.Page
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
                       " and ISNULL(P.JurisdictionId,0) > 0 ";
        query += " order by EndDate desc ";

        DataTable dtbannerlist = dbc.GetDataTable(query);
        //if (dtbannerlist.Rows.Count > 0)
        //{
        gvproductlist.DataSource = dtbannerlist;
        gvproductlist.DataBind();
        //}
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

    protected void BtnRemove_Click(object sender, EventArgs e)
    {
        bool flag = false;
        foreach (GridViewRow gr in gvproductlist.Rows)
        {
            string userId = Request.Cookies["TUser"]["Id"].ToString();
            bool isChecked = ((CheckBox)gr.FindControl("chkProduct")).Checked;
            int sJurisdictionId = Convert.ToInt32(ddlJurisdiction.SelectedValue);
            int categoryId = Convert.ToInt32(ddlCategoryName.SelectedValue);
            int subcategoryId = Convert.ToInt32(ddlSubCategoryName.SelectedValue);

            if (isChecked)
            {
                HiddenField hdngrpId = ((HiddenField)gr.Cells[1].FindControl("HiddenFieldgrpid"));
                string productId = hdngrpId.Value;
                string delcategoryLinkQry = " DELETE FROM tblCategoryProductLink WHERE ProductId = " + productId +
                                            " and CategoryId = " + categoryId +
                                            " and SubCategoryId = " + subcategoryId;
                dbc.ExecuteQuery(delcategoryLinkQry);

            }

        }
        DataList();
        if (flag)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Product Assign to Jurisdiction Successfully')", true);
        }

    }

}