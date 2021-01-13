using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Product_ManageProducts : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    string id = "", grpFileName = "", MainFileName = "";
    static DataTable dtgrpProduct;
    static DataTable dtProductCategory;
    static DataTable Jurisdictiondt;
    public static int _lastIndex = -1;
    public static int _lastIndexCategory = -1;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblmsg.Text = "";
            if (!IsPostBack)
            {
                _lastIndex = -1;
                _lastIndexCategory = -1;
                id = Request.QueryString["Id"];
                string grpId = Request.QueryString["grpId"];
                txtFixedShipRate.Enabled = false;
                txtFreezeQty.Enabled = false;
                dtgrpProduct = new DataTable("GrpProduct");
                dtgrpProduct.Columns.Add("grpUnitName", typeof(string));
                dtgrpProduct.Columns.Add("grpUnit", typeof(string));
                dtgrpProduct.Columns.Add("grpMrp", typeof(string));
                dtgrpProduct.Columns.Add("grpDiscountType", typeof(string));
                dtgrpProduct.Columns.Add("grpDiscount", typeof(string));
                dtgrpProduct.Columns.Add("grpSoshoPrice", typeof(string));
                dtgrpProduct.Columns.Add("grpPackingType", typeof(string));
                dtgrpProduct.Columns.Add("Id", typeof(string));
                dtgrpProduct.Columns.Add("grpId", typeof(string));
                dtgrpProduct.Columns.Add("grpUnitId", typeof(string));
                dtgrpProduct.Columns.Add("grpImage", typeof(string));
                dtgrpProduct.Columns.Add("grpisOutOfStock", typeof(string));
                dtgrpProduct.Columns.Add("grpisSelected", typeof(string));
                dtgrpProduct.Columns.Add("Status", typeof(string));
                dtgrpProduct.Columns.Add("grpMinQty", typeof(string));
                dtgrpProduct.Columns.Add("grpMaxQty", typeof(string));
                dtgrpProduct.Columns.Add("grpIsQtyFreeze", typeof(string));
                dtgrpProduct.Columns.Add("grpisBestBuy", typeof(string));
                dtgrpProduct.Columns.Add("grpFreezeQty", typeof(string));

                dtProductCategory = new DataTable("CATEGORY");
                dtProductCategory.Columns.Add("hdnlinkCategoryId", typeof(string));
                dtProductCategory.Columns.Add("linkProdCategory", typeof(string));
                dtProductCategory.Columns.Add("hdnlinkSubCategoryId", typeof(string));
                dtProductCategory.Columns.Add("linkProdSubCategory", typeof(string));
                
                string categoryqry = "SELECT CategoryId,CategoryName FROM Category where isnull(IsDeleted,0)=0 AND isnull(IsActive,0)=1 order by Sequence asc";
                DataTable dtcategory = dbc.GetDataTable(categoryqry);
                //ddlCategoryName.DataSource = dtcategory;
                //ddlCategoryName.DataTextField = "CategoryName";
                //ddlCategoryName.DataValueField = "CategoryId";
                //ddlCategoryName.DataBind();

                ddlLinkCategoryName.DataSource = dtcategory;
                ddlLinkCategoryName.DataTextField = "CategoryName";
                ddlLinkCategoryName.DataValueField = "CategoryId";
                ddlLinkCategoryName.DataBind();

                string producttype = "Select Id,Name From ProductTemplate order by Id asc";
                DataTable dtproducttype = dbc.GetDataTable(producttype);
                ddlProductType.DataSource = dtproducttype;
                ddlProductType.DataTextField = "Name";
                ddlProductType.DataValueField = "Id";
                ddlProductType.DataBind();

                string gstqry = "select Id as Id,TaxName as Name from [GstTaxCategory] where Isdeleted=0 order by Id asc";
                DataTable dtgst = dbc.GetDataTable(gstqry);
                ddlgst.DataSource = dtgst;
                ddlgst.DataTextField = "Name";
                ddlgst.DataValueField = "Id";
                ddlgst.DataBind();

                string unitnameqry = "select Id as Id,UnitName as Name from [UnitMaster] where Isdeleted=0 order by Id asc";
                DataTable dtunitname = dbc.GetDataTable(unitnameqry);
                ddlunitname.DataSource = dtunitname;
                ddlunitname.DataTextField = "Name";
                ddlunitname.DataValueField = "Id";
                ddlunitname.DataBind();

                ddlgrpUnitName.DataSource = dtunitname;
                ddlgrpUnitName.DataTextField = "Name";
                ddlgrpUnitName.DataValueField = "Id";
                ddlgrpUnitName.DataBind();

                txtdt.Text = dbc.getindiantime().ToString("dd/MMM/yyyy");
                txtdt1.Text = dbc.getindiantime().ToString("dd/MMM/yyyy");
                chkisactive.Checked = true;
                chkgrpIsSelected.Checked = true;
                chkgrpIsBestBuy.Checked = true;

                //txtFullDescription.Text = "<ul><li>When winters knock at your door, you must ensure that your skin remains healthy and vibrant as ever. With this body lotion, you can acquire smooth and soft skin. Its deep acting natural ingredients keep your skin moisturized throughout the day.</li></ul>";

                //ckkey.Text = "<li>Anti-ageing &amp; skin whitening properties</li><li>Gives a shiny glow</li><li>For all types of skin</li><li>UV filter protects the skin from damaging effects of the sun</li>";

                //cknotes.Text = "<ul><li style=\"box-sizing: border-box; color: rgb(116, 115, 115); font-size: 18px;\"><p><span style=\"color:#000000;\"><span style=\"font-size:12px;\"><span style=\"font-family:arial,helvetica,sans-serif;\">The more friends buy this product within offer period, the higher discount for everyone. Discount balance amount will be credited to your original mode of payment.</span></span></span></p></li><li style=\"box-sizing: border-box; color: rgb(116, 115, 115); font-size: 18px;\"><p><span style=\"color:#000000;\"><span style=\"font-size:12px;\"><span style=\"font-family:arial,helvetica,sans-serif;\">If less or no friends buy, the pending balance amount will be collected at time of delivery.</span></span></span></p></li></ul>";
                string IsAdmin = Request.Cookies["TUser"]["IsAdmin"].ToString();
                string sJurisdictionId = Request.Cookies["TUser"]["JurisdictionID"].ToString();
                if (IsAdmin == "True")
                {
                    hdnIsAdmin.Value = "1";
                    //ChkIsApproved.Checked = true;
                    divIsApproved.Visible = true;
                    divRejectedReason.Visible = true;
                    divJurisdictionIncharge.Visible = true;

                    string JurisdictionInchargeqry = "Select Distinct JurisdictionId,JurisdictionIncharge From JurisdictionMaster where IsActive = 1 order by JurisdictionId";
                    DataTable dtIncharge = dbc.GetDataTable(JurisdictionInchargeqry);
                    chklstJurisdictionIncharge.DataSource = dtIncharge;
                    chklstJurisdictionIncharge.DataTextField = "JurisdictionIncharge";
                    chklstJurisdictionIncharge.DataValueField = "JurisdictionId";
                    chklstJurisdictionIncharge.DataBind();
                }
                else
                {
                    hdnIsAdmin.Value = "0";
                    divIsApproved.Visible = false;
                    divRejectedReason.Visible = false;
                    divJurisdictionIncharge.Visible = false;
                    ChkIsApproved.Disabled = true;
                    txtRejectedReason.Enabled = false;
                    ChkIsApproved.Checked = false;
                }

                if (id != null && !id.Equals(""))
                {
                    BtnSave.Text = "Update";
                    chklstJurisdictionIncharge.Enabled=false;
                    //ddlSubCategoryName.Items.Clear(true
                    ImageData();
                    DataTable dt1;
                    if (IsAdmin == "True")
                    {
                        dt1 = dbc.GetDataTable("SELECT  [Name],[GSTTaxId],[Unit],[UnitId],[StartDate],[EndDate],[IsActive],[Metatags],[Metadesc],[sold],[Mrp],[Offer],[BuyWith1FriendExtraDiscount],[BuyWith5FriendExtraDiscount],[FixedShipRate],[KeyFeatures],[Note],[IsDeleted],[DOC],[DOM],[VideoName],[OGImage],[ProductDiscription],ProductMRP,IsQtyFreeze,DisplayOrder,[ProductTemplateID],[ProductBanner],[Recommended],[Createdby],[DiscountType],[Discount],[SoshoPrice],[MinQty],[MaxQty],[JurisdictionId],[IsFreeShipping],[IsFixedShipping],[IsApproved],[RejectedReason],[IsProductDescription] FROM [dbo].[Product] where IsDeleted=0  and Id=" + id);
                        Jurisdictiondt = dbc.GetDataTable("Select JurisdictionId From Product Where Id=" + id + " OR ProductMasterId=" + id);
                    }
                    else
                    {
                        dt1 = dbc.GetDataTable("SELECT  [Name],[GSTTaxId],[Unit],[UnitId],[StartDate],[EndDate],[IsActive],[Metatags],[Metadesc],[sold],[Mrp],[Offer],[BuyWith1FriendExtraDiscount],[BuyWith5FriendExtraDiscount],[FixedShipRate],[KeyFeatures],[Note],[IsDeleted],[DOC],[DOM],[VideoName],[OGImage],[ProductDiscription],ProductMRP,IsQtyFreeze,DisplayOrder,[ProductTemplateID],[ProductBanner],[Recommended],[Createdby],[DiscountType],[Discount],[SoshoPrice],[MinQty],[MaxQty],[JurisdictionId],[IsFreeShipping],[IsFixedShipping],[IsApproved],[RejectedReason],[IsProductDescription] FROM [dbo].[Product] where IsDeleted=0  and Id=" + id + " and JurisdictionId=" + sJurisdictionId);
                        Jurisdictiondt = dbc.GetDataTable("Select JurisdictionId From Product Where Id=" + id);
                    }


                    if (dt1.Rows.Count > 0)
                    {
                        txtpname.Text = dt1.Rows[0]["Name"].ToString();
                        hdnProductCreatedBy.Value = dt1.Rows[0]["Createdby"].ToString();
                        //chklstJurisdictionIncharge.SelectedValue = dt1.Rows[0]["JurisdictionId"].ToString();
                        txtFullDescription.Text = dt1.Rows[0]["ProductDiscription"].ToString();

                        txtunit.Text = dt1.Rows[0]["Unit"].ToString();
                        ddlgst.SelectedValue = dt1.Rows[0]["GSTTaxId"].ToString();
                        ddlunitname.SelectedValue = dt1.Rows[0]["UnitId"].ToString();
                        txtmetatag.Text = dt1.Rows[0]["Metatags"].ToString();
                        txtmetadisc.Text = dt1.Rows[0]["Metadesc"].ToString();
                        txtNoOfSoldItems.Text = dt1.Rows[0]["sold"].ToString();
                        txtvdo.Text = dt1.Rows[0]["VideoName"].ToString();
                        txtprice.Text = dt1.Rows[0]["Mrp"].ToString();
                        txtoffer.Text = dt1.Rows[0]["Offer"].ToString();
                        txtBuyWith1Friend.Text = dt1.Rows[0]["BuyWith1FriendExtraDiscount"].ToString();
                        txtBuyWith5Friend.Text = dt1.Rows[0]["BuyWith5FriendExtraDiscount"].ToString();
                        txtFixedShipRate.Text = dt1.Rows[0]["FixedShipRate"].ToString();
                        ckkey.Text = dt1.Rows[0]["KeyFeatures"].ToString();
                        cknotes.Text = dt1.Rows[0]["Note"].ToString();
                        txtMRP.Text = dt1.Rows[0]["ProductMRP"].ToString();
                        txtDisplayOrder.Text = dt1.Rows[0]["DisplayOrder"].ToString();
                        txtRejectedReason.Text = dt1.Rows[0]["RejectedReason"].ToString();
                        string IsQuantity = dt1.Rows[0]["IsQtyFreeze"].ToString();

                        if (Jurisdictiondt.Rows.Count > 0)
                        {
                            foreach (ListItem li in chklstJurisdictionIncharge.Items)
                            {
                                for (int i = 0; i < Jurisdictiondt.Rows.Count; i++)
                                {
                                    if (li.Value == Jurisdictiondt.Rows[i]["JurisdictionId"].ToString())
                                    {
                                        li.Selected = true;
                                    }
                                }
                            }
                        }
                        if (IsQuantity == "True")
                        {
                            chkIsQuantityFreez.Checked = true;
                            txtFreezeQty.Enabled = true;
                        }
                        else
                        {
                            chkIsQuantityFreez.Checked = false;
                            txtFreezeQty.Enabled = false;
                        }
                        string IsFreeShipping = dt1.Rows[0]["IsFreeShipping"].ToString();
                        if(IsFreeShipping == "True")
                        {
                            chkIsFreeShipping.Checked = true;
                        }
                        else
                        {
                            chkIsFreeShipping.Checked = false;
                        }
                        string IsFixedShipping = dt1.Rows[0]["IsFixedShipping"].ToString();
                        if (IsFixedShipping == "True")
                        {
                            chkIsFixedShipping.Checked = true;
                            txtFixedShipRate.Enabled = true;
                        }
                        else
                        {
                            chkIsFixedShipping.Checked = false;
                            txtFixedShipRate.Enabled = false;
                        }
                       
                        string IsApproved = dt1.Rows[0]["IsApproved"].ToString();
                        if (IsApproved == "True")
                        {
                            ChkIsApproved.Checked = true;
                        }
                        else
                        {
                            ChkIsApproved.Checked = false;
                        }

                        string IsProdDescription = dt1.Rows[0]["IsProductDescription"].ToString();
                        if (IsProdDescription == "True")
                        {
                            chkIsProductDescription.Checked = true;
                        }
                        else
                        {
                            chkIsProductDescription.Checked = false;
                        }
                        string SoldTime = txtNoOfSoldItems.Text.ToString();
                        string solditems = "Sold " + SoldTime + " Times";
                        if (!String.IsNullOrEmpty(solditems))
                        {
                            string[] dt7 = solditems.Split(' ');
                            if (dt7.Length > 0)
                            {
                                txtNoOfSoldItems.Text = dt7[02].ToString();

                            }
                        }

                        if (dt1.Rows[0]["IsActive"].ToString() == "True")
                            chkisactive.Checked = true;
                        else
                            chkisactive.Checked = false;

                        string strtdate = dt1.Rows[0]["StartDate"].ToString();
                        string enddate = dt1.Rows[0]["EndDate"].ToString();

                        //ddlCategoryName.SelectedValue = dt1.Rows[0]["CategoryID"].ToString();
                        string subcategoryqry = "SELECT Id,SubCategory FROM tblSubCategory where isnull(IsDeleted,0)=0 AND CategoryId = '" + ddlLinkCategoryName.SelectedValue + "' order by Sequence asc";
                        DataTable dtsubcategory = dbc.GetDataTable(subcategoryqry);
                        //ddlSubCategoryName.DataSource = dtsubcategory;
                        //ddlSubCategoryName.DataTextField = "SubCategory";
                        //ddlSubCategoryName.DataValueField = "Id";
                        //ddlSubCategoryName.DataBind();
                        //ddlSubCategoryName.SelectedValue = dt1.Rows[0]["SubCategoryID"].ToString();
                        ddlLinkSubCategoryName.DataSource = dtsubcategory;
                        ddlLinkSubCategoryName.DataTextField = "SubCategory";
                        ddlLinkSubCategoryName.DataValueField = "Id";
                        ddlLinkSubCategoryName.DataBind();
                        //ddlLinkSubCategoryName.SelectedValue = dt1.Rows[0]["SubCategoryID"].ToString();

                        ddlProductType.SelectedValue = dt1.Rows[0]["ProductTemplateID"].ToString();
                        txtProductBanner.Text = dt1.Rows[0]["ProductBanner"].ToString();
                        txtRecommended.Text = dt1.Rows[0]["Recommended"].ToString();


                        DateTime oDate = Convert.ToDateTime(strtdate);
                        string datetime = oDate.ToString("dd/MMM/yyyy hh:mm tt");
                        if (!String.IsNullOrEmpty(datetime))
                        {
                            string[] dt = datetime.Split(' ');
                            if (dt.Length == 3)
                            {
                                txtdt.Text = dt[0].ToString();
                                txttime.Text = dt[1].ToString() + " " + dt[2].ToString();
                            }
                        }

                        DateTime oDate1 = Convert.ToDateTime(enddate);
                        string datetime1 = oDate1.ToString("dd/MMM/yyyy hh:mm tt");
                        if (!String.IsNullOrEmpty(datetime1))
                        {
                            string[] dt11 = datetime1.Split(' ');
                            if (dt11.Length == 3)
                            {
                                txtdt1.Text = dt11[0].ToString();
                                txttime1.Text = dt11[1].ToString() + " " + dt11[2].ToString();
                            }
                        }

                        ddlDiscountType.SelectedValue = dt1.Rows[0]["DiscountType"].ToString();
                        txtDiscount.Text = dt1.Rows[0]["Discount"].ToString();
                        txtSoshoPrice.Text = dt1.Rows[0]["SoshoPrice"].ToString();
                        txtMaxQty.Text = dt1.Rows[0]["MaxQty"].ToString();
                        txtMinQty.Text = dt1.Rows[0]["MinQty"].ToString();

                        DataTable dt2 = dbc.GetDataTable("SELECT top 1 [ImageFileName] FROM [dbo].[ProductImages] where IsDeleted=0 and ProductId=" + id);
                        if (dt2.Rows.Count > 0)
                        {
                            productimg.ImageUrl = "../ProductImage/" + dt2.Rows[0]["ImageFileName"].ToString();
                        }

                        productimg1.ImageUrl = "../ProductOGImage/" + dt1.Rows[0]["OGImage"].ToString();

                        DataTable dtProductAttribute = dbc.GetDataTable("SELECT UnitId as grpUnitId,um.UnitName as grpUnitName,Unit as grpUnit,Mrp as grpMrp,DiscountType as grpDiscountType,Discount as grpDiscount,SoshoPrice as grpSoshoPrice,PackingType as grpPackingType,ProductImage as grpImage,ProductId as Id,pam.Id as grpId,pam.isOutofStock as grpisOutOfStock,pam.isSelected as grpisSelected,'Update' as Status, ISNULL(pam.MinQty,1) as grpMinQty, ISNULL(pam.MaxQty,99) as grpMaxQty, ISNULL(pam.IsQtyFreeze,0) as grpIsQtyFreeze, ISNULL(pam.IsBestBuy,0) AS grpisBestBuy, ISNULL(pam.FreezeQty,0) AS grpFreezeQty  FROM [dbo].[Product_ProductAttribute_Mapping] pam INNER JOIN [UnitMaster] um on pam.UnitId=um.Id  where pam.IsDeleted=0 and ProductId=" + id);
                        if (dtProductAttribute.Rows.Count > 0)
                        {
                            ViewState["dt"] = dtProductAttribute;
                            grdgProduct.DataSource = dtProductAttribute;
                            grdgProduct.DataBind();

                        }

                        DataTable dtProductCategoryLink = dbc.GetDataTable("Select PL.CategoryId AS hdnlinkCategoryId, C.CategoryName AS linkProdCategory, PL.SubCategoryId AS hdnlinkSubCategoryId, S.SubCategory AS linkProdSubCategory FROM tblCategoryProductLink PL LEFT JOIN Category C ON C.CategoryID = PL.CategoryId LEFT JOIN tblSubCategory S ON S.Id = PL.SubCategoryId where PL.IsDeleted=0 and PL.ProductId=" + id);
                        if (dtProductCategoryLink.Rows.Count > 0)
                        {
                            ViewState["dtProductCategory"] = dtProductCategoryLink;
                            grdProductCategory.DataSource = dtProductCategoryLink;
                            grdProductCategory.DataBind();

                        }
                    }
                    else
                    {
                        sweetMessage("", "Jurisdiction Product Not Match", "warning");
                        return;
                    }
                }


                if (grpId != null && !grpId.Equals(""))
                {
                    BtnAdd.Visible = false;
                    BtnUpdate.Visible = true;

                    if (id != null && !id.Equals(""))
                    {
                        DataTable dtProductAttribute = dbc.GetDataTable("SELECT UnitId as grpUnitId,um.UnitName as grpUnitName,Unit as grpUnit,Mrp as grpMrp,DiscountType as grpDiscountType,Discount as grpDiscount,SoshoPrice as grpSoshoPrice,PackingType as grpPackingType,ProductImage as grpImage,ProductId as Id,pam.Id as grpId, ISNULL(pam.MinQty,1) as grpMinQty, ISNULL(pam.MaxQty,99) as grpMaxQty, ISNULL(pam.IsQtyFreeze,0) as grpIsQtyFreeze, ISNULL(pam.IsBestBuy,0) as grpIsBestBuy, ISNULL(pam.FreezeQty,0) AS grpFreezeQty  FROM [dbo].[Product_ProductAttribute_Mapping] pam INNER JOIN [UnitMaster] um on pam.UnitId=um.Id  where pam.IsDeleted=0 and ProductId=" + id + " and pam.Id=" + grpId);
                        if (dtProductAttribute.Rows.Count > 0)
                        {
                            ddlgrpUnitName.SelectedValue = dtProductAttribute.Rows[0]["grpUnitId"].ToString();
                            txtgrpUnit.Text = dtProductAttribute.Rows[0]["grpUnit"].ToString();
                            txtgrpMRP.Text = dtProductAttribute.Rows[0]["grpMrp"].ToString();
                            ddlgrpDiscountType.SelectedValue = dtProductAttribute.Rows[0]["grpDiscountType"].ToString();
                            txtgrpDiscount.Text = dtProductAttribute.Rows[0]["grpDiscount"].ToString();
                            txtgrpSoshoPrice.Text = dtProductAttribute.Rows[0]["grpSoshoPrice"].ToString();
                            txtgrpPackingType.Text = dtProductAttribute.Rows[0]["grpPackingType"].ToString();
                            txtMinQty.Text = dtProductAttribute.Rows[0]["grpMinQty"].ToString();
                            txtMaxQty.Text = dtProductAttribute.Rows[0]["grpMaxQty"].ToString();
                            txtFreezeQty.Text = dtProductAttribute.Rows[0]["grpFreezeQty"].ToString();
                            int isQtyFreez = Convert.ToInt32(dtProductAttribute.Rows[0]["grpIsQtyFreeze"]);

                            if (isQtyFreez == 1) {
                                chkIsQuantityFreez.Checked = true;
                                txtFreezeQty.Enabled = true;
                                    }
                            else
                            {
                                chkIsQuantityFreez.Checked = false;
                                txtFreezeQty.Enabled = false;
                            }
                            int isBestBuy = Convert.ToInt32(dtProductAttribute.Rows[0]["grpIsBestBuy"]);
                            if (isBestBuy == 1)
                            {
                                chkgrpIsBestBuy.Checked = true;
                            }
                            else
                            {
                                chkgrpIsBestBuy.Checked = false;
                            }
                            GrpImage.ImageUrl =  dtProductAttribute.Rows[0]["grpImage"].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ee)
        {
            lblmsg.Text = "Error:" + ee.Message + " ::: " + ee.StackTrace + " :::: " + ee.InnerException;
        }
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            int index = _lastIndex;
            string fileName_OG = string.Empty; string fileName = string.Empty;
            string userId = Request.Cookies["TUser"]["Id"].ToString();
            string IsAdmin = Request.Cookies["TUser"]["IsAdmin"].ToString();
            int IsActive = 0, IsApproved = 0, iMaxQty = 0, iMinQty = 0;
            if (IsAdmin == "True")
            {
                IsApproved = 1;
            }
                int IsFreeShipping = 0;
            int IsFixedShipping = 0, IsProductDescription = 0;
            string id1 = Request.QueryString["Id"];
            string sCreatedJurisdictionId = Request.Cookies["TUser"]["JurisdictionID"].ToString();
            string startdate = txtdt.Text.ToString();
            string starttime = txttime.Text.ToString();
            string enddate = txtdt1.Text.ToString();
            string endtime = txttime1.Text.ToString();

            string FROM1 = startdate + " " + starttime;
            string TO1 = enddate + " " + endtime;
            string productname = txtpname.Text.ToString();
            string fulldescription = txtFullDescription.Text.ToString();
            string unit = txtunit.Text.ToString();
            string vdolink = txtvdo.Text.ToString();
            string SoldTime = txtNoOfSoldItems.Text.ToString();
            string solditems = "Sold " + SoldTime + " Times";
            string gstid = ddlgst.SelectedValue.ToString();
            string unitid = ddlunitname.SelectedValue.ToString();

            string metatag = txtmetatag.Text.ToString();
            string metadisc = txtmetadisc.Text.ToString();

            string price = txtprice.Text.ToString();
            string offer = txtoffer.Text.ToString();
            string Buywith1 = txtBuyWith1Friend.Text.ToString();
            string Buywith5 = txtBuyWith5Friend.Text.ToString();
            string shiprate = txtFixedShipRate.Text.ToString();

            string key = ckkey.Text.ToString();
            string notes = cknotes.Text.ToString();

            string displayorder = txtDisplayOrder.Text.ToString();
            string ProductTemplateID = ddlProductType.SelectedValue.ToString();
            string ProductBanner = txtProductBanner.Text.ToString();
            string Recommended = txtRecommended.Text.ToString();
            string rejectedReason = "";
            if (!string.IsNullOrEmpty(txtRejectedReason.Text))
            {
                rejectedReason = Convert.ToString(txtRejectedReason.Text);
            }
                
            DateTime dtCreatedon = DateTime.Now;

            string spDiscountType = "", spDiscount = "", spSoshoPrice = "", spmrp = "";

            if (string.IsNullOrEmpty(ddlDiscountType.SelectedValue.ToString()))
                spDiscountType = "";
            else
                spDiscountType = ddlDiscountType.SelectedValue.ToString();

            if (string.IsNullOrEmpty(txtDiscount.Text.ToString()))
                spDiscount = "0";
            else
                spDiscount = txtDiscount.Text.ToString();


            if (string.IsNullOrEmpty(txtSoshoPrice.Text.ToString()))
                spSoshoPrice = "0";
            else
                spSoshoPrice = txtSoshoPrice.Text.ToString();

            if (string.IsNullOrEmpty(txtMRP.Text.ToString()))
                spmrp = "0";
            else
                spmrp = txtMRP.Text.ToString();
            if (string.IsNullOrEmpty(txtMaxQty.Text))
                iMaxQty = 99;
            else
                iMaxQty = Convert.ToInt32(txtMaxQty.Text);

            if (string.IsNullOrEmpty(txtMinQty.Text))
                iMinQty = 1;
            else
                iMinQty = Convert.ToInt32(txtMinQty.Text);

            if (chkIsProductDescription.Checked)
                IsProductDescription = 1;

            Stream fs = FileUploadMainImages.PostedFile.InputStream;
            BinaryReader br = new BinaryReader(fs);
            Byte[] image = br.ReadBytes((Int32)fs.Length);
            List<string> filenamearray = new List<string>();

            if (FileUploadMainImages.HasFile)
            {
                int filecount = 0;
                int fileuploadcount = 0;
                filecount = FileUploadMainImages.PostedFiles.Count();


                if (filecount <= 10)
                {
                    foreach (HttpPostedFile postfiles in FileUploadMainImages.PostedFiles)
                    {
                        string filetype = Path.GetExtension(postfiles.FileName);

                        string serverfolder = string.Empty;
                        string serverpath = string.Empty;
                        var supportedTypes = new[] { ".png", ".jpg", ".jpeg", ".gif", ".bmp" };
                        if (supportedTypes.Contains(filetype))
                        {
                            fileName = DateTime.Now.Ticks + filetype;
                            serverfolder = Server.MapPath(@"/ProductImage");
                            serverpath = serverfolder + "\\" + Path.GetFileName(fileName);
                            postfiles.SaveAs(serverpath);


                            fileuploadcount++;
                            filenamearray.Add(fileName);
                        }
                    }
                }

            }

            string[] validFileTypes_OG = { "png", "jpg", "jpeg" };
            Stream fs_OG = FileUpload2.PostedFile.InputStream;
            BinaryReader br_OG = new BinaryReader(fs_OG);
            Byte[] image_OG = br_OG.ReadBytes((Int32)fs_OG.Length);

            string exten_OG = System.IO.Path.GetExtension(FileUpload2.FileName);
            string name_OG = System.IO.Path.GetFileName(FileUpload2.FileName);

            string ext_OG = System.IO.Path.GetExtension(FileUpload2.FileName);

            bool isValidFile_OG = false;
            string imgname_OG = "", imgnamenew_OG = "";

            decimal size = Math.Round(((decimal)FileUpload2.PostedFile.ContentLength / (decimal)1024), 2);

            if (FileUpload2.HasFile)
            {
                if (size >= 10 && size <= 150)
                {
                    for (int i = 0; i < validFileTypes_OG.Length; i++)
                    {
                        if (ext_OG == "." + validFileTypes_OG[i])
                        {
                            isValidFile_OG = true;
                            break;
                        }
                        else
                        {

                        }
                    }
                    if (!isValidFile_OG)
                    {
                        return;
                    }
                    ext_OG = ext_OG.Replace(".", "images/");
                    imgname_OG = DateTime.Now.Ticks + ext_OG;
                    imgnamenew_OG = FileUpload2.FileName;
                    string path_OG = Server.MapPath("/ProductOGImage");
                    FileUpload2.SaveAs(path_OG + "/" + imgnamenew_OG);
                    fileName_OG = imgnamenew_OG;
                }
                else
                {
                    sweetMessage("", "OG Image Size should be 10KB to 150KB", "warning");
                    return;
                }

            }
            if (chkisactive.Checked)
                IsActive = 1;

            List<ListItem> selectedIncharge = new List<ListItem>();

            string struserqry = "Select * From users where Id = " + hdnProductCreatedBy.Value;
            DataTable dtUserQry = dbc.GetDataTable(struserqry);
            string sNCreatedJurisdictionId = "";

            if (dtUserQry.Rows.Count > 0)
            {
                sNCreatedJurisdictionId = dtUserQry.Rows[0]["JurisdictionID"].ToString();
            }
            if (ChkIsApproved.Checked)
            {
                IsApproved = 1;
            }
            if (chkIsFreeShipping.Checked)
            {
                IsFreeShipping = 1;
            }
            if (chkIsFixedShipping.Checked)
            {
                IsFixedShipping = 1;
            }

            selectedIncharge = chklstJurisdictionIncharge.Items.Cast<ListItem>().Where(n => n.Selected).ToList();

            string id11 = "";
            int showmrpmsg = 1;
            DataTable dt = dbc.GetDataTable("select Id from [Product] where [Name] ='" + productname + "' Order by Id desc");
            if (BtnSave.Text.Equals("Update"))
            {
                string id = Request.QueryString["id"].ToString();
                if (fileName != "" && fileName_OG != "")
                {
                    int gstid1 = ddlgst.SelectedIndex + 1;
                    int unitname = ddlunitname.SelectedIndex + 1;
                    string isqty = "";
                    if (chkIsQuantityFreez.Checked == true)
                    {
                        isqty = "1";
                    }
                    else
                    {
                        isqty = "0"; ;
                    }

                    string[] para1 = { productname, gstid1.ToString(), unit, unitname.ToString(), FROM1, TO1, IsActive.ToString(), metatag, metadisc, price, offer, Buywith1, Buywith5, shiprate, key, notes, "0", dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss"), vdolink, fileName_OG, fulldescription, id1 };
                    string query = "UPDATE [Product] SET [Name]=@1,[GSTTaxId]=@2,[Unit]=@3,[UnitId]=@4,[StartDate]=@5,[EndDate]=@6,[IsActive]=@7,[Metatags]=@8,[Metadesc]=@9,[Mrp]=@10,[Offer]=@11,[BuyWith1FriendExtraDiscount]=@12,[BuyWith5FriendExtraDiscount]=@13,[FixedShipRate]=@14,[KeyFeatures]=@15,[Note]=@16,[IsDeleted]=@17,[DOM]=@18,[VideoName]=@19,[OGImage]=@20,[ProductDiscription]=@21,ProductMRP='" + txtMRP.Text + "',IsQtyFreeze='" + isqty + "',sold='" + solditems + "',DisplayOrder='" + displayorder + "',ModifiedOn='" + dtCreatedon.ToString() + "',ModifiedBy=" + userId + ",DiscountType='" + spDiscountType.ToString() + "',Discount=" + spDiscount.ToString() + ",SoshoPrice=" + spSoshoPrice.ToString() + ", Recommended = '" + Recommended + "', ShowMrpInMsg = " + showmrpmsg + ", ProductBanner = '" + ProductBanner + "', IsFreeShipping ="+IsFreeShipping+ ",IsFixedShipping = "+IsFixedShipping+", RejectedReason = '"+rejectedReason+ "',IsProductDescription = "+IsProductDescription.ToString()+" where [Id]=@22";
                    int v1 = dbc.ExecuteQueryWithParams(query, para1);
                    // int imgorder = 0;
                    if (v1 > 0)
                    {
                        //product image update
                        int imgorder = 0;
                        DataTable dtcount = dbc.GetDataTable("select Id from ProductImages where ProductId=" + id1);
                        if (dtcount != null & dtcount.Rows.Count > 0)
                        {
                            imgorder = dtcount.Rows.Count;
                        }

                        for (int i = 0; i < filenamearray.Count; i++)
                        {
                            imgorder++;
                            string[] para11 = { id1, filenamearray[i], "0", imgorder.ToString(), dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss"), dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss") };
                            string query1 = "INSERT INTO [dbo].[ProductImages]([ProductId],[ImageFileName],[IsDeleted],[DisplayOrder],[Doc],[Dom]) VALUES (@1,@2,@3,@4,@5,@6)";
                            int v11 = dbc.ExecuteQueryWithParams(query1, para11);
                            if (v11 > 0)
                            {
                                sweetMessage("", "Product Updated Successfully", "success");
                                Response.Redirect("ProductList.aspx");
                            }
                            else
                            {
                                sweetMessage("", "Please Try Again!!", "warning");
                            }
                        }
                    }
                    else
                    {
                        sweetMessage("", "Please Try Again!!", "warning");
                    }
                }
                else if (fileName_OG != "" && fileName == "")
                {
                    int gstid1 = ddlgst.SelectedIndex + 1;
                    int unitname = ddlunitname.SelectedIndex + 1;

                    string[] para1 = { productname, gstid1.ToString(), unit, unitname.ToString(), FROM1, TO1, IsActive.ToString(), metatag, metadisc, price, offer, Buywith1, Buywith5, shiprate, key, notes, "0", dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss"), vdolink, fileName_OG, fulldescription, id1 };
                    string isqty = "";
                    if (chkIsQuantityFreez.Checked == true)
                    {
                        isqty = "1";
                    }
                    else
                    {
                        isqty = "0"; ;
                    }

                    string query = "UPDATE [Product] SET [Name]=@1,[GSTTaxId]=@2,[Unit]=@3,[UnitId]=@4,[StartDate]=@5,[EndDate]=@6,[IsActive]=@7,[Metatags]=@8,[Metadesc]=@9,[Mrp]=@10,[Offer]=@11,[BuyWith1FriendExtraDiscount]=@12,[BuyWith5FriendExtraDiscount]=@13,[FixedShipRate]=@14,[KeyFeatures]=@15,[Note]=@16,[IsDeleted]=@17,[DOM]=@18,[VideoName]=@19,[OGImage]=@20,[ProductDiscription]=@21,ProductMRP='" + txtMRP.Text + "',IsQtyFreeze='" + isqty + "',sold='" + solditems + "',DisplayOrder='" + displayorder + "',ModifiedOn='" + dtCreatedon.ToString() + "',ModifiedBy=" + userId + ",DiscountType='" + spDiscountType.ToString() + "',Discount=" + spDiscount.ToString() + ",SoshoPrice=" + spSoshoPrice.ToString() + ", Recommended = '" + Recommended + "', ShowMrpInMsg = " + showmrpmsg + ", ProductBanner = '" + ProductBanner + "', IsFreeShipping =" + IsFreeShipping + ",IsFixedShipping = " + IsFixedShipping + ",RejectedReason = '"+rejectedReason+"' where [Id]=@22";
                    int v1 = dbc.ExecuteQueryWithParams(query, para1);
                    if (v1 > 0)
                    {
                        sweetMessage("", "Product Updated Successfully", "success");
                        Response.Redirect("ProductList.aspx");

                    }
                    else
                    {
                        sweetMessage("", "Please Try Again!!", "warning");
                    }
                }

                else if (fileName_OG == "" && fileName != "")
                {
                    int gstid1 = ddlgst.SelectedIndex + 1;
                    int unitname = ddlunitname.SelectedIndex + 1;

                    string[] para1 = { productname, gstid1.ToString(), unit, unitname.ToString(), FROM1, TO1, IsActive.ToString(), metatag, metadisc, price, offer, Buywith1, Buywith5, shiprate, key, notes, "0", dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss"), vdolink, fulldescription, id1 };

                    string query = "UPDATE [Product] SET [Name]=@1,[GSTTaxId]=@2,[Unit]=@3,[UnitId]=@4,[StartDate]=@5,[EndDate]=@6,[IsActive]=@7,[Metatags]=@8,[Metadesc]=@9,[Mrp]=@10,[Offer]=@11,[BuyWith1FriendExtraDiscount]=@12,[BuyWith5FriendExtraDiscount]=@13,[FixedShipRate]=@14,[KeyFeatures]=@15,[Note]=@16,[IsDeleted]=@17,[DOM]=@18,[VideoName]=@19,[ProductDiscription]=@20,ProductMRP='" + txtMRP.Text + "',sold='" + solditems + "',DisplayOrder='" + displayorder + "',ModifiedOn='" + dtCreatedon.ToString() + "',ModifiedBy=" + userId + ",DiscountType='" + spDiscountType.ToString() + "',Discount=" + spDiscount.ToString() + ",SoshoPrice=" + spSoshoPrice.ToString() + ", Recommended = '" + Recommended + "', ShowMrpInMsg = " + showmrpmsg + ", ProductBanner = '" + ProductBanner + "', IsFreeShipping =" + IsFreeShipping + ",IsFixedShipping = " + IsFixedShipping + ",RejectedReason='"+rejectedReason+ "',IsProductDescription="+ IsProductDescription .ToString()+ " where [Id]=@21";
                    int v1 = dbc.ExecuteQueryWithParams(query, para1);
                    if (v1 > 0)
                    {
                        //product image update
                        int imgorder = 0;
                        DataTable dtcount = dbc.GetDataTable("select Id from ProductImages where ProductId=" + id1);
                        if (dtcount != null & dtcount.Rows.Count > 0)
                        {
                            imgorder = dtcount.Rows.Count;
                        }

                        for (int i = 0; i < filenamearray.Count; i++)
                        {
                            imgorder++;
                            string[] para11 = { id1, filenamearray[i], "0", imgorder.ToString(), dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss"), dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss") };

                            string query1 = "INSERT INTO [dbo].[ProductImages]([ProductId],[ImageFileName],[IsDeleted],[DisplayOrder],[Doc],[Dom]) VALUES (@1,@2,@3,@4,@5,@6)";
                            int v11 = dbc.ExecuteQueryWithParams(query1, para11);
                        }
                    }
                    else
                    {
                        sweetMessage("", "Please Try Again!!", "warning");
                    }
                }

                else if (fileName == "" && fileName_OG == "")
                {
                    string isqty = "";
                    if (chkIsQuantityFreez.Checked == true)
                    {
                        isqty = "1";
                    }
                    else
                    {
                        isqty = "0"; ;
                    }

                    int gstid1 = ddlgst.SelectedIndex + 1;
                    int unitname = ddlunitname.SelectedIndex + 1;

                    string[] para1 = { productname, gstid1.ToString(), unit, unitname.ToString(), FROM1, TO1, IsActive.ToString(), metatag, metadisc, price, offer, Buywith1, Buywith5, shiprate, key, notes, "0", dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss"), vdolink, fulldescription, id1 };

                    string query = "UPDATE [Product] SET [Name]=@1,[GSTTaxId]=@2,[Unit]=@3,[UnitId]=@4,[StartDate]=@5,[EndDate]=@6,[IsActive]=@7,[Metatags]=@8,[Metadesc]=@9,[Mrp]=@10,[Offer]=@11,[BuyWith1FriendExtraDiscount]=@12,[BuyWith5FriendExtraDiscount]=@13,[FixedShipRate]=@14,[KeyFeatures]=@15,[Note]=@16,[IsDeleted]=@17,[DOM]=@18,[VideoName]=@19,[ProductDiscription]=@20,ProductMRP='" + txtMRP.Text + "',IsQtyFreeze='" + isqty + "',sold='" + solditems + "',DisplayOrder='" + displayorder + "',ModifiedOn='" + dtCreatedon.ToString() + "',ModifiedBy=" + userId + ", Recommended = '" + Recommended + "', ShowMrpInMsg = " + showmrpmsg + ", ProductBanner = '" + ProductBanner + "', IsFreeShipping =" + IsFreeShipping + ",IsFixedShipping = " + IsFixedShipping + ",RejectedReason='"+rejectedReason+ "',IsProductDescription="+ IsProductDescription.ToString() + ", ISApproved = "+IsApproved+" where [Id]=@21";
                    int v1 = dbc.ExecuteQueryWithParams(query, para1);
                    if (v1 > 0)
                    {
                        //Insert Product Entry
                        foreach (ListItem item in selectedIncharge)
                        {
                            string sJurisdictionId = item.Value.ToString();

                            if (!string.IsNullOrEmpty(sNCreatedJurisdictionId))
                            {
                                if (sJurisdictionId == sNCreatedJurisdictionId)
                                {
                                    //Update Product
                                    string queryup = "UPDATE [Product] SET [JurisdictionID] = " + sNCreatedJurisdictionId + ",[ApproverID]=" + userId + ",[IsApproved] = " + IsApproved + " where Id = " + id1;
                                    int v1up = dbc.ExecuteQuery(queryup);
                                }
                                else
                                {
                                    //Insert Product
                                    if (chkIsQuantityFreez.Checked == true)
                                    {
                                        isqty = "1";
                                    }
                                    else
                                    {
                                        isqty = "0"; ;
                                    }
                                    string querym = "INSERT INTO [dbo].[Product] ([Name],[GSTTaxId],[Unit],[UnitId],[StartDate],[EndDate],[IsActive],[Metatags],[Metadesc]," + 
                                                    " [Mrp],[Offer],[BuyWith1FriendExtraDiscount],[BuyWith5FriendExtraDiscount],[FixedShipRate]," + 
                                                    " [KeyFeatures],[Note],[IsDeleted],[DOC],[DOM],[VideoName],[OGImage],[ProductDiscription]," +
                                                    " [JustBougth],[sold],ProductMRP,[ShowMrpInMsg],IsQtyFreeze,DisplayOrder,ProductTemplateID, " + 
                                                    " ProductBanner,Recommended,[CreatedOn],[CreatedBy],[IsApproved],[DiscountType],[Discount],[SoshoPrice],[MaxQty], " + 
                                                    " [MinQty],[JurisdictionID],[ApproverID],[IsFreeShipping],[IsFixedShipping],RejectedReason) " + 
                                                    " VALUES ('" + productname.Replace("'", "''") + "','" + gstid1.ToString().Replace("'", "''") + "','" + unit.Replace("'", "''") + "','" + unitname.ToString().Replace("'", "''") + "','" + FROM1.Replace("'", "''") + "','" + TO1.Replace("'", "''") + "','" + IsActive + "','" + metatag.Replace("'", "''") + "','" + metadisc.Replace("'", "''") + "','" + price.Replace("'", "''") + "','" + offer.Replace("'", "''") + "','" + Buywith1.Replace("'", "''") + "','" + Buywith5.Replace("'", "''") + "','" + shiprate.Replace("'", "''") + "','" + key.Replace("'", "''") + "','" + notes.Replace("'", "''") + "',0,'" + dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss").Replace("'", "''") + "','" + dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss").Replace("'", "''") + "','" + vdolink.Replace("'", "''") + "','" + fileName_OG.Replace("'", "''") + "','" + fulldescription.Replace("'", "''") + "','Hardik Just bought!','" + solditems + "','" + txtMRP.Text + "'," + showmrpmsg + ",'" + isqty + "','" + displayorder + "'," + ProductTemplateID + ",'" + ProductBanner + "','" + Recommended + "','" + dtCreatedon.ToString() + "'," + userId + "," + IsApproved + ",'" + spDiscountType.ToString() + "'," + spDiscount.ToString() + "," + spSoshoPrice.ToString() + "," + iMaxQty + "," + iMinQty + "," + sJurisdictionId + "," + userId + ","+IsFreeShipping+","+IsFixedShipping+",'"+rejectedReason+"')";
                                    int VAL = dbc.ExecuteQuery(querym);
                                }
                            }
                            
                        }
                        string strUnitId = "", strImage = "", strisOutOfStock = "", strisSelected = "", strPackingType = "", strisBestBuy = "";
                        string MinQty = "", isQtyFreezeVal = "", FreezeQty = "", sGrpId = string.Empty;
                        
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            id11 = dt.Rows[0]["Id"].ToString();

                        }
                        //string delProductAttribute = " Delete FROM [dbo].[Product_ProductAttribute_Mapping]  where Productid=" + Convert.ToInt32(id1);
                        //dbc.ExecuteQuery(delProductAttribute);

                        string delProductCategory = " Delete FROM [dbo].[tblcategoryproductlink]  where Productid=" + Convert.ToInt32(id1);
                        dbc.ExecuteQuery(delProductCategory);

                        decimal selectedSoshoPrice = 0;
                        decimal selectedDiscount = 0;
                        decimal selectedMRP = 0;
                        foreach (GridViewRow g1 in grdgProduct.Rows)
                        {
                            HiddenField hdnMinQty = (HiddenField)g1.FindControl("HiddenFieldMinQty");
                            MinQty = hdnMinQty.Value;
                            HiddenField hdnMaxQty = (HiddenField)g1.FindControl("HiddenFieldMaxQty");
                            string MaxQty = hdnMaxQty.Value;

                            HiddenField hdnisQtyFreeze = (HiddenField)g1.FindControl("HiddenFieldIsQtyFreeze");
                            isQtyFreezeVal = hdnisQtyFreeze.Value;

                            if (isQtyFreezeVal == "True")
                            {
                                isQtyFreezeVal = "1";

                            }
                            else
                            {
                                isQtyFreezeVal = "0";
                            }
                            HiddenField hdnFreezeQty = (HiddenField)g1.FindControl("HiddenFieldFreezeQty");
                            FreezeQty = hdnFreezeQty.Value;

                            HiddenField hdnUnitId = (HiddenField)g1.FindControl("HiddenFieldgrpUnitId");
                            strUnitId = hdnUnitId.Value;
                            HiddenField hdnImage = (HiddenField)g1.FindControl("HiddenFieldgrpImage");
                            strImage = hdnImage.Value;
                            HiddenField hdnisOutOfStock = (HiddenField)g1.FindControl("HiddenFieldgrpisOutOfStock");
                            strisOutOfStock = hdnisOutOfStock.Value;

                            HiddenField hdnGrpId = (HiddenField)g1.FindControl("HiddenFieldgrpid");
                            sGrpId = hdnGrpId.Value;
                            if (strisOutOfStock == "True" || strisOutOfStock == "1")
                            {
                                strisOutOfStock = "1";
                            }
                            else
                            {
                                strisOutOfStock = "0";
                            }
                            
                            HiddenField hdnisSelected = (HiddenField)g1.FindControl("HiddenFieldgrpisSelected");
                            strisSelected = hdnisSelected.Value;
                            if (strisSelected == "True")
                            {
                                strisSelected = "1";
                                selectedSoshoPrice = Convert.ToDecimal(g1.Cells[5].Text);
                                selectedDiscount = Convert.ToDecimal(g1.Cells[4].Text);
                                selectedMRP = Convert.ToDecimal(g1.Cells[2].Text);
                            }
                            else
                            {
                                strisSelected = "0";
                            }
                            HiddenField hdnisBestBuy = (HiddenField)g1.FindControl("HiddenFieldIsBestBuy");
                            strisBestBuy = hdnisBestBuy.Value;
                            if (strisBestBuy == "True")
                                strisBestBuy = "1";
                            else
                                strisBestBuy = "0";

                            Label name = (Label)g1.FindControl("lblname");

                            strPackingType = g1.Cells[6].Text;
                            if (!string.IsNullOrEmpty(strPackingType))
                            {
                                if (strPackingType.ToString() == "&nbsp;")
                                    strPackingType = "";
                                else
                                    strPackingType = g1.Cells[6].Text;
                            }
                            else
                            {
                                strPackingType = "";
                            }

                            if (string.IsNullOrEmpty(sGrpId))
                            {
                                string ProductAttrqry = "INSERT INTO [dbo].[Product_ProductAttribute_Mapping] ([ProductId],[Unit],[UnitId],[Mrp],[DiscountType],[Discount],[SoshoPrice],[PackingType],[ProductImage],[IsActive],[IsDeleted],[CreatedOn],[CreatedBy],[isOutOfStock],[isSelected],[MinQty],[MaxQty],[IsQtyFreeze],[IsBestBuy],[FreezeQty]) VALUES (" + id1 + ",'" + g1.Cells[1].Text + "'," + strUnitId.ToString() + "," + g1.Cells[2].Text + ",'" + g1.Cells[3].Text + "'," + g1.Cells[4].Text + "," + g1.Cells[5].Text + ",'" + strPackingType.ToString() + "','" + strImage.ToString() + "',1,0,'" + dtCreatedon.ToString() + "'," + userId + "," + strisOutOfStock + "," + strisSelected + "," + MinQty + "," + MaxQty + "," + isQtyFreezeVal + "," + strisBestBuy + "," + FreezeQty + ")";
                                int VALatt = dbc.ExecuteQuery(ProductAttrqry);
                            }
                            else
                            {
                                string updateqty = "UPDATE [Product_ProductAttribute_Mapping] set Unit = '" + g1.Cells[1].Text + "',UnitId='" + strUnitId.ToString() + "',Mrp='" + g1.Cells[2].Text + "',DiscountType='" + g1.Cells[3].Text + "',Discount='" + g1.Cells[4].Text + "',SoshoPrice='" + g1.Cells[5].Text + "',PackingType='" + strPackingType.ToString() + "',ProductImage='" + strImage.ToString() + "',isOutOfStock='" + strisOutOfStock + "',isSelected='" + strisSelected + "',MinQty='" + MinQty + "',MaxQty='" + MaxQty + "',IsQtyFreeze='" + isQtyFreezeVal + "',IsBestBuy='" + strisBestBuy + "',FreezeQty='" + FreezeQty + "' Where Id=" + sGrpId + " and ProductId = " + id1;
                                int VALatt = dbc.ExecuteQuery(updateqty);
                            }
                            
                        }
                        string updateProductPrice = "UPDATE Product SET SoshoPrice = " + selectedSoshoPrice + ", MRP = "+ selectedMRP +
                                                   ",Discount = " +selectedDiscount+ " Where id=" + Convert.ToInt32(id1);
                        dbc.ExecuteQuery(updateProductPrice);

                        foreach (GridViewRow g2 in grdProductCategory.Rows)
                        {
                            HiddenField hdnLinkCategoryId = (HiddenField)g2.FindControl("HiddenFieldlinkCategoryId");
                            string linkCategoryId = hdnLinkCategoryId.Value;
                            HiddenField hdnLinkSubCategoryId = (HiddenField)g2.FindControl("HiddenFieldlinkSubCategoryId");
                            string linkSubCategoryId = hdnLinkSubCategoryId.Value;

                            string ProductCategoryqry = "INSERT INTO [dbo].[tblCategoryProductLink] ([ProductId],[CategoryId],[SubCategoryId],[IsActive],[IsDeleted],[CreatedDate],[CreatedBy]) VALUES (" + id1 + ",'" + linkCategoryId + "'," + linkSubCategoryId + ",1,0,'" + dtCreatedon.ToString() + "'," + userId + ")";
                            dbc.ExecuteQuery(ProductCategoryqry);
                        }
                        sweetMessage("", "Product Updated Successfully", "success");
                        Response.Redirect("ProductList.aspx");
                    }
                    else
                    {
                        sweetMessage("", "Please Try Again!!", "warning");
                    }
                }
            }
            else
            {
                int gstid1 = ddlgst.SelectedIndex + 1;
                int unitname = ddlunitname.SelectedIndex + 1;
                string isqty = "";
                if (chkIsQuantityFreez.Checked == true)
                {
                    isqty = "1";
                }
                else
                {
                    isqty = "0"; ;
                }
                int VAL = 0;
                int ProductMasterId = 0;
                int AttributeMasterId = 0;
                int iCtr = 0;
               
                if (selectedIncharge.Count > 0)
                {
                    List<int> AttributeMasterIds = new List<int>();
                    foreach (ListItem item in selectedIncharge)
                    {
                        ++iCtr;
                        string sJurisdictionId = item.Value.ToString();
                        string[] para2 = { productname.Replace("'", "''") , gstid1.ToString().Replace("'", "''"), unit.Replace("'", "''"), unitname.ToString().Replace("'", "''"),
                        FROM1.Replace("'", "''") , TO1.Replace("'", "''"),IsActive.ToString(),metatag.Replace("'", "''"),metadisc.Replace("'", "''"),
                        price.Replace("'", "''"),offer.Replace("'", "''"),Buywith1.Replace("'", "''"),Buywith5.Replace("'", "''"),shiprate.Replace("'", "''"),
                        key.Replace("'", "''"),notes.Replace("'", "''"),"0",dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss").Replace("'", "''"),
                        dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss").Replace("'", "''"), vdolink.Replace("'", "''"),fileName_OG.Replace("'", "''")
                        ,fulldescription.Replace("'", "''"),"Hardik Just bought!",solditems,spmrp,showmrpmsg.ToString(),isqty,displayorder,ProductTemplateID,
                        ProductBanner,Recommended,dtCreatedon.ToString(),userId,IsApproved.ToString(),spDiscountType.ToString(),spDiscount.ToString(),spSoshoPrice.ToString(),
                        iMaxQty.ToString() ,iMinQty.ToString(),IsProductDescription.ToString(), sJurisdictionId , IsFreeShipping.ToString(), IsFixedShipping.ToString(), rejectedReason.ToString().Replace("'", "''") };
                        string query = "INSERT INTO [dbo].[Product] ([Name],[GSTTaxId],[Unit],[UnitId],[StartDate],[EndDate],[IsActive],[Metatags],[Metadesc]," +
                                        " [Mrp],[Offer],[BuyWith1FriendExtraDiscount],[BuyWith5FriendExtraDiscount],[FixedShipRate],[KeyFeatures],[Note]," + 
                                        " [IsDeleted],[DOC],[DOM],[VideoName],[OGImage],[ProductDiscription],[JustBougth],[sold],ProductMRP,[ShowMrpInMsg], " + 
                                        " IsQtyFreeze,DisplayOrder,ProductTemplateID,ProductBanner,Recommended,[CreatedOn],[CreatedBy],[IsApproved],[DiscountType]," +
                                        " [Discount],[SoshoPrice],[MaxQty],[MinQty],[IsProductDescription],[JurisdictionId],[IsFreeShipping],[IsFixedShipping],RejectedReason) " +
                                        "VALUES (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15,@16,@17,@18,@19,@20,@21,@22,@23,@24,@25,@26,@27,@28,@29,@30,@31,@32, " +
                                        " @33,@34,@35,@36,@37,@38,@39,@40,@41,@42,@43,@44); SELECT SCOPE_IDENTITY();";
                        
                        VAL = dbc.ExecuteQueryWithParamsId(query, para2);
                        //UPDATE productMasterId in Product table
                        string UpdateQry = " UPDATE [dbo].[Product]  SET ProductMasterId = " + ProductMasterId + " WHERE id = " + VAL;
                        dbc.ExecuteQuery(UpdateQry);
                        if (iCtr == 1)
                        {
                            ProductMasterId = VAL;
                        }

                        dt = dbc.GetDataTable("select Id from [Product] where [Name] ='" + productname + "' Order by Id desc");
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            id11 = dt.Rows[0]["Id"].ToString();
                        }

                        int imgorder = 0;
                        for (int i = 0; i < filenamearray.Count; i++)
                        {
                            imgorder++;
                            string[] para1 = { id11, filenamearray[i], "0", imgorder.ToString(), dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss"), dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss") };
                            string query1 = "INSERT INTO [dbo].[ProductImages]([ProductId],[ImageFileName],[IsDeleted],[DisplayOrder],[Doc],[Dom]) VALUES (@1,@2,@3,@4,@5,@6)";
                            int v1 = dbc.ExecuteQueryWithParams(query1, para1);
                        }

                        //Product Attribute Mapping Save
                        string strUnitId = "", strImage = "", strisOutOfStock = "", strisSelected = "", strPackingType = "", strisBestBuy = "";
                        string MinQty = "", isQtyFreezeVal = "", MaxQty = "", FreezeQty = "";

                        decimal selectedSoshoPrice = 0;
                        decimal selectedDiscount = 0;
                        decimal selectedMRP = 0;
                        int iAtr = 0;
                        foreach (GridViewRow g1 in grdgProduct.Rows)
                        {
                            ++iAtr;
                            HiddenField hdnMinQty = (HiddenField)g1.FindControl("HiddenFieldMinQty");
                            MinQty = hdnMinQty.Value;
                            HiddenField hdnMaxQty = (HiddenField)g1.FindControl("HiddenFieldMaxQty");
                            MaxQty = hdnMaxQty.Value;
                            HiddenField hdnisQtyFreeze = (HiddenField)g1.FindControl("HiddenFieldIsQtyFreeze");
                            isQtyFreezeVal = hdnisQtyFreeze.Value;
                            if (isQtyFreezeVal == "True")
                            {
                                isQtyFreezeVal = "1";
                            }
                            else
                            {
                                isQtyFreezeVal = "0";
                            }
                            HiddenField hdnFreezeQty = (HiddenField)g1.FindControl("HiddenFieldFreezeQty");
                            FreezeQty = hdnFreezeQty.Value;
                            HiddenField hdnUnitId = (HiddenField)g1.FindControl("HiddenFieldgrpUnitId");
                            strUnitId = hdnUnitId.Value;
                            HiddenField hdnImage = (HiddenField)g1.FindControl("HiddenFieldgrpImage");
                            strImage = hdnImage.Value;
                            HiddenField hdnisOutOfStock = (HiddenField)g1.FindControl("HiddenFieldgrpisOutOfStock");
                            strisOutOfStock = hdnisOutOfStock.Value;
                            if (strisOutOfStock == "True")
                            {
                                strisOutOfStock = "1";
                            }
                            else
                            {
                                strisOutOfStock = "0";
                            }
                            HiddenField hdnisSelected = (HiddenField)g1.FindControl("HiddenFieldgrpisSelected");
                            strisSelected = hdnisSelected.Value;
                            if (strisSelected == "True")
                            {
                                strisSelected = "1";
                                selectedSoshoPrice = Convert.ToDecimal(g1.Cells[5].Text);
                                selectedDiscount = Convert.ToDecimal(g1.Cells[4].Text);
                                selectedMRP = Convert.ToDecimal(g1.Cells[2].Text);
                            }
                            else { 
                            strisSelected = "0";
                        }
                            HiddenField hdnisBestBuy = (HiddenField)g1.FindControl("HiddenFieldIsBestBuy");
                            strisBestBuy = hdnisBestBuy.Value;
                            if (strisBestBuy == "True")
                                strisBestBuy = "1";
                            else
                                strisBestBuy = "0";
                           
                            Label name = (Label)g1.FindControl("lblname");
                            strPackingType = g1.Cells[6].Text;
                            if (!string.IsNullOrEmpty(strPackingType))
                            {
                                if (strPackingType.ToString() == "&nbsp;")
                                    strPackingType = "";
                                else
                                    strPackingType = g1.Cells[6].Text;
                            }
                            else
                            {
                                strPackingType = "";
                            }

                            string ProductAttrqry = "INSERT INTO [dbo].[Product_ProductAttribute_Mapping] ([ProductId],[Unit],[UnitId],[Mrp],[DiscountType],[Discount],[SoshoPrice],[PackingType],[ProductImage],[IsActive],[IsDeleted],[CreatedOn],[CreatedBy],[isOutOfStock],[isSelected],[MinQty],[MaxQty],[IsQtyFreeze],[IsBestBuy],[FreezeQty]) VALUES (" + VAL + ",'" + g1.Cells[1].Text + "'," + strUnitId.ToString() + "," + g1.Cells[2].Text + ",'" + g1.Cells[3].Text + "'," + g1.Cells[4].Text + "," + g1.Cells[5].Text + ",'" + strPackingType.ToString() + "','" + strImage.ToString() + "',1,0,'" + dtCreatedon.ToString() + "'," + userId + "," + strisOutOfStock + "," + strisSelected + "," + MinQty + "," + MaxQty + "," + isQtyFreezeVal + ","+strisBestBuy+","+ FreezeQty + "); SELECT SCOPE_IDENTITY();";
                            //int VALatt = dbc.ExecuteQuery(ProductAttrqry);
                            object att = dbc.ExecuteSQLScaler(ProductAttrqry);
                            int VALatt = Convert.ToInt32(att);
                            string UpdateAttrQry = string.Empty;
                            if (iCtr == 1)
                            {
                                AttributeMasterIds.Add(VALatt);
                                //AttributeMasterId = VALatt;
                            }
                            if (AttributeMasterIds.Count > 0 && iCtr != 1)
                            {
                                UpdateAttrQry = " UPDATE [dbo].[Product_ProductAttribute_Mapping]  SET AttributeMasterId = " + AttributeMasterIds[iAtr - 1].ToString() + " WHERE id = " + VALatt;

                            }
                            else
                            {
                               UpdateAttrQry = " UPDATE [dbo].[Product_ProductAttribute_Mapping]  SET AttributeMasterId = " + AttributeMasterId + " WHERE id = " + VALatt;
                            }
                            dbc.ExecuteQuery(UpdateAttrQry);
                           
                        }
                        string updateProductPrice = "UPDATE Product SET SoshoPrice = " + selectedSoshoPrice + ", MRP = " + selectedMRP +
                                                   ",Discount = " + selectedDiscount + " Where id=" + Convert.ToInt32(VAL);
                        dbc.ExecuteQuery(updateProductPrice);

                        foreach (GridViewRow g2 in grdProductCategory.Rows)
                        {
                            HiddenField hdnLinkCategoryId = (HiddenField)g2.FindControl("HiddenFieldlinkCategoryId");
                            string linkCategoryId = hdnLinkCategoryId.Value;
                            HiddenField hdnLinkSubCategoryId = (HiddenField)g2.FindControl("HiddenFieldlinkSubCategoryId");
                            string linkSubCategoryId = hdnLinkSubCategoryId.Value;

                            string ProductCategoryqry = "INSERT INTO [dbo].[tblCategoryProductLink] ([ProductId],[CategoryId],[SubCategoryId],[IsActive],[IsDeleted],[CreatedDate],[CreatedBy]) VALUES (" + VAL + ",'" + linkCategoryId + "'," + linkSubCategoryId + ",1,0,'" + dtCreatedon.ToString() + "'," + userId + ")";
                            dbc.ExecuteQuery(ProductCategoryqry);
                        }
                    }
                }
                else
                {
                    string query = "INSERT INTO [dbo].[Product] ([Name],[GSTTaxId],[Unit],[UnitId],[StartDate],[EndDate],[IsActive],[Metatags] " + 
                                   " ,[Metadesc],[Mrp],[Offer],[BuyWith1FriendExtraDiscount],[BuyWith5FriendExtraDiscount],[FixedShipRate], " + 
                                   " [KeyFeatures],[Note],[IsDeleted],[DOC],[DOM],[VideoName],[OGImage],[ProductDiscription],[JustBougth],[sold],ProductMRP, " + 
                                   " [ShowMrpInMsg],IsQtyFreeze,DisplayOrder,ProductTemplateID,ProductBanner,Recommended,[CreatedOn],[CreatedBy],[IsApproved], " + 
                                   " [DiscountType],[Discount],[SoshoPrice],[MaxQty],[MinQty],[IsProductDescription],[JurisdictionId], " + 
                                   " [IsFreeShipping],[IsFixedShipping],[RejectedReason]) " + 
                                   " VALUES ('" + productname.Replace("'", "''") + "','" + gstid1.ToString().Replace("'", "''") + "','" + 
                                   unit.Replace("'", "''") + "','" + unitname.ToString().Replace("'", "''") + "','" + FROM1.Replace("'", "''") + "','" + 
                                   TO1.Replace("'", "''") + "','" + IsActive + "','" + metatag.Replace("'", "''") + "','" + 
                                   metadisc.Replace("'", "''") + "','" + price.Replace("'", "''") + "','" + offer.Replace("'", "''") + "','" + 
                                   Buywith1.Replace("'", "''") + "','" + Buywith5.Replace("'", "''") + "','" + shiprate.Replace("'", "''") + "','" + 
                                   key.Replace("'", "''") + "','" + notes.Replace("'", "''") + "',0,'" + dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss").Replace("'", "''") + 
                                   "','" + dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss").Replace("'", "''") + "','" + 
                                   vdolink.Replace("'", "''") + "','" + fileName_OG.Replace("'", "''") + "','" + fulldescription.Replace("'", "''") +
                                   "','Hardik Just bought!','" + solditems + "','" + spmrp + "'," + showmrpmsg + ",'" + isqty + "','" + 
                                   displayorder + "'," + ProductTemplateID + ",'" + ProductBanner + "','" + Recommended + "','" + 
                                   dtCreatedon.ToString() + "'," + userId + "," + IsApproved + ",'" + spDiscountType.ToString() + "'," + 
                                   spDiscount.ToString() + "," + spSoshoPrice.ToString() + "," + iMaxQty + "," + iMinQty + "," + 
                                   IsProductDescription + "," + sCreatedJurisdictionId + "," + IsFreeShipping + "," + IsFixedShipping + ",'" + rejectedReason.Replace("'", "''") + "')";
                    VAL = dbc.ExecuteQuery(query);

                    dt = dbc.GetDataTable("select Id from [Product] where [Name] ='" + productname + "' Order by Id desc");
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        id11 = dt.Rows[0]["Id"].ToString();

                    }

                    int imgorder = 0;
                    for (int i = 0; i < filenamearray.Count; i++)
                    {
                        imgorder++;
                        string[] para1 = { id11, filenamearray[i], "0", imgorder.ToString(), dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss"), dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss") };

                        string query1 = "INSERT INTO [dbo].[ProductImages]([ProductId],[ImageFileName],[IsDeleted],[DisplayOrder],[Doc],[Dom]) VALUES (@1,@2,@3,@4,@5,@6)";
                        int v1 = dbc.ExecuteQueryWithParams(query1, para1);
                    }

                    //Product Attribute Mapping Save
                    string strUnitId = "", strImage = "", strisOutOfStock = "", strisSelected = "", strPackingType = "", strisBestBuy = "";
                    string MinQty = "", isQtyFreezeVal = "", MaxQty = "", FreezeQty="";

                    decimal selectedSoshoPrice = 0;
                    decimal selectedDiscount = 0;
                    decimal selectedMRP = 0;
                    foreach (GridViewRow g1 in grdgProduct.Rows)
                    {
                        HiddenField hdnMinQty = (HiddenField)g1.FindControl("HiddenFieldMinQty");
                        MinQty = hdnMinQty.Value;
                        HiddenField hdnMaxQty = (HiddenField)g1.FindControl("HiddenFieldMaxQty");
                        MaxQty = hdnMaxQty.Value;
                        HiddenField hdnisQtyFreeze = (HiddenField)g1.FindControl("HiddenFieldIsQtyFreeze");
                        isQtyFreezeVal = hdnisQtyFreeze.Value;

                        HiddenField hdnFreezeQty = (HiddenField)g1.FindControl("HiddenFieldFreezeQty");
                        FreezeQty = hdnFreezeQty.Value;
                        HiddenField hdnUnitId = (HiddenField)g1.FindControl("HiddenFieldgrpUnitId");
                        strUnitId = hdnUnitId.Value;
                        HiddenField hdnImage = (HiddenField)g1.FindControl("HiddenFieldgrpImage");
                        strImage = hdnImage.Value;
                        HiddenField hdnisOutOfStock = (HiddenField)g1.FindControl("HiddenFieldgrpisOutOfStock");
                        strisOutOfStock = hdnisOutOfStock.Value;

                        HiddenField hdnisSelected = (HiddenField)g1.FindControl("HiddenFieldgrpisSelected");
                        strisSelected = hdnisSelected.Value;
                        if (strisSelected == "True")
                        {
                            strisSelected = "1";
                            selectedSoshoPrice = Convert.ToDecimal(g1.Cells[5].Text);
                            selectedDiscount = Convert.ToDecimal(g1.Cells[4].Text);
                            selectedMRP = Convert.ToDecimal(g1.Cells[2].Text);
                        }
                        else { 
                        strisSelected = "0";
                    }
                        HiddenField hdnisBestBuy = (HiddenField)g1.FindControl("HiddenFieldIsBestBuy");
                        strisBestBuy = hdnisBestBuy.Value;
                        if (strisBestBuy == "True")
                            strisBestBuy = "1";
                        else
                            strisBestBuy = "0";
                        if (strisOutOfStock == "True")
                            strisOutOfStock = "1";
                        else
                            strisOutOfStock = "0";

                        Label name = (Label)g1.FindControl("lblname");

                        strPackingType = g1.Cells[6].Text;
                        if (!string.IsNullOrEmpty(strPackingType))
                        {
                            if (strPackingType.ToString() == "&nbsp;")
                                strPackingType = "";
                            else
                                strPackingType = g1.Cells[6].Text;
                        }
                        else
                        {
                            strPackingType = "";
                        }


                        string ProductAttrqry = "INSERT INTO [dbo].[Product_ProductAttribute_Mapping] ([ProductId],[Unit],[UnitId],[Mrp],[DiscountType],[Discount],[SoshoPrice],[PackingType],[ProductImage],[IsActive],[IsDeleted],[CreatedOn],[CreatedBy],[isOutOfStock],[isSelected],[MinQty],[MaxQty],[IsQtyFreeze],[IsBestBuy],[FreezeQty]) VALUES (" + id11 + ",'" + g1.Cells[1].Text + "'," + strUnitId.ToString() + "," + g1.Cells[2].Text + ",'" + g1.Cells[3].Text + "'," + g1.Cells[4].Text + "," + g1.Cells[5].Text + ",'" + strPackingType.ToString() + "','" + strImage.ToString() + "',1,0,'" + dtCreatedon.ToString() + "'," + userId + "," + strisOutOfStock + "," + strisSelected + "," + MinQty + "," + MaxQty + "," + isQtyFreezeVal + ","+strisBestBuy+","+ FreezeQty + ")";
                        int VALatt = dbc.ExecuteQuery(ProductAttrqry);
                    }
                    string updateProductPrice = "UPDATE Product SET SoshoPrice = " + selectedSoshoPrice + ", MRP = " + selectedMRP +
                                                   ",Discount = " + selectedDiscount + " Where id=" + Convert.ToInt32(id11);
                    dbc.ExecuteQuery(updateProductPrice);
                    foreach (GridViewRow g2 in grdProductCategory.Rows)
                    {
                        HiddenField hdnLinkCategoryId = (HiddenField)g2.FindControl("HiddenFieldlinkCategoryId");
                        string linkCategoryId = hdnLinkCategoryId.Value;
                        HiddenField hdnLinkSubCategoryId = (HiddenField)g2.FindControl("HiddenFieldlinkSubCategoryId");
                        string linkSubCategoryId = hdnLinkSubCategoryId.Value;

                        string ProductCategoryqry = "INSERT INTO [dbo].[tblCategoryProductLink] ([ProductId],[CategoryId],[SubCategoryId],[IsActive],[IsDeleted],[CreatedDate],[CreatedBy]) VALUES (" + id11 + ",'" + linkCategoryId + "'," + linkSubCategoryId + ",1,0,'" + dtCreatedon.ToString() + "'," + userId + ")";
                        dbc.ExecuteQuery(ProductCategoryqry);
                    }

                }
                if (VAL > 0)
                {
                    sweetMessage("", "Product Uploaded Successfully", "success");
                    Response.Redirect("ProductList.aspx");
                }
                else
                {
                    sweetMessage("", "Please Try Again!!", "warning");
                }

            }

        }
        catch (Exception E)
        {
            sweetMessage("", "Please Try Again!!", "warning");
        }
    }



    protected void Button11_Click(object sender, EventArgs e)
    {
        try
        {
            string gst = txtgst1.Text.ToString();
            string gstval = txtvalue.Text.ToString();
            string[] para1 = { gst, gstval, "0", dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss"), dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss") };
            string query = "INSERT INTO [dbo].[GstTaxCategory]([TaxName],[TaxValue],[IsDeleted],[DOM],[DOC]) VALUES (@1,@2,@3,@4,@5)";
            int v1 = dbc.ExecuteQueryWithParams(query, para1);
            if (v1 > 0)
            {
                string gstqry = "select Id as Id,TaxName as Name from [GstTaxCategory] where Isdeleted=0 order by Id asc";
                DataTable dtgst = dbc.GetDataTable(gstqry);
                ddlgst.DataSource = dtgst;
                ddlgst.DataTextField = "Name";
                ddlgst.DataValueField = "Id";
                ddlgst.DataBind();
            }
            else
            {
                sweetMessage("", "Please Try Again!!", "warning");
            }
        }
        catch (Exception aa)
        {

        }
    }

    protected void Button22_Click(object sender, EventArgs e)
    {
        try
        {
            string unitname = txtunitvalue1.Text.ToString();

            string[] para1 = { unitname, "int", "0", dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss"), dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss") };

            string query = "INSERT INTO [dbo].[UnitMaster]([UnitName],[UnitType],[IsDeleted],[DOC],[DOM]) VALUES (@1,@2,@3,@4,@5)";
            int v1 = dbc.ExecuteQueryWithParams(query, para1);
            if (v1 > 0)
            {
                string unitnameqry = "select Id as Id,UnitName as Name from [UnitMaster] where Isdeleted=0 order by Id asc";
                DataTable dtunitname = dbc.GetDataTable(unitnameqry);
                ddlunitname.DataSource = dtunitname;
                ddlunitname.DataTextField = "Name";
                ddlunitname.DataValueField = "Id";
                ddlunitname.DataBind();
            }
            else
            {
                sweetMessage("", "Please Try Again!!", "warning");
            }
        }
        catch (Exception aa)
        {

        }
    }

    private void sweetMessage(string message, string message1, string type)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<script type = 'text/javascript'>");
        sb.Append("swal({");
        sb.Append("title: \"" + message + "\",");
        sb.Append("text: \"" + message1 + "\",");
        sb.Append("icon: \"" + type + "\" ");
        sb.Append("}).then(result => {");
        sb.Append("if (result.value) {");
        sb.Append("");//For Yes
        sb.Append("} else {");
        if (type.Equals("success"))
        {
            sb.Append("window.location.href = 'HomePageBannerList.aspx'");
        }
        else
        {
            sb.Append("");//For No
        }
        sb.Append("}");
        sb.Append("});");

        sb.Append("</script>");
        ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", sb.ToString());
    }

    protected void ImageData()
    {
        string query = "SELECT Id as ImageId,[ImageFileName] as ImageName,[DisplayOrder] as ImageDisplayOrder from ProductImages where ProductId= " + Request.QueryString["Id"];
        DataTable dtimage = dbc.GetDataTable(query);
        string ImagePath = "Image";

        if (dtimage.Rows.Count > 0)
        {
            dtimage.Columns.Add("Image", typeof(string));
            for (int i = 0; i < dtimage.Rows.Count; i++)
            {
                string file_name = dtimage.Rows[i]["ImageName"].ToString();
                string imgpath = "/ProductImage/" + file_name;
                dtimage.Rows[i]["Image"] = imgpath;
            }
            // (e.Row.FindControl("Image1") as Image).ImageUrl = imageUrl;
            gvImage.DataSource = dtimage;
            gvImage.DataBind();
        }
    }
    protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow && e.Row.RowIndex != gvImage.EditIndex)
        {
            (e.Row.Cells[0].Controls[2] as LinkButton).Attributes["onclick"] = "return confirm('Do you want to delete this row?');";
        }
    }

    protected void gvImage_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.TableSection = TableRowSection.TableHeader;
        }
    }

    // edit event    
    protected void gvImage_RowEditing(object sender, GridViewEditEventArgs e)
    {
        gvImage.EditIndex = e.NewEditIndex;
        ImageData();

    }

    protected void gvImage_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    // update event    
    protected void gvImage_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        //find image id of edit row    
        string imageId = gvImage.DataKeys[e.RowIndex].Value.ToString();
        // find values for update    
        TextBox name1 = (TextBox)gvImage.Rows[e.RowIndex].FindControl("txt_ImageName");

        TextBox displayorder = (TextBox)gvImage.Rows[e.RowIndex].FindControl("txt_ImageDisplayOrder");
        FileUpload FileUpload1 = (FileUpload)gvImage.Rows[e.RowIndex].FindControl("FileUpload1");
        // con = new SqlConnection(connStr);  
        // string path = "/ProductImage/";
        string pid = Request.QueryString["Id"];
        string flname = FileUpload1.FileName;
        if (FileUpload1.HasFile)
        {
            //path += FileUpload1.FileName;  
            //save image in folder    
            // FileUpload1.SaveAs(MapPath(path));
            DataTable dtimgname = dbc.GetDataTable("select [ImageFileName] from ProductImages where Id=" + imageId);
            string oldfile = dtimgname.Rows[0]["ImageFileName"].ToString();
            ImageDeleteFromFolder(oldfile);

            string querydel = "DELETE from [dbo].[ProductImages] where Id=" + imageId;
            int vdel = dbc.ExecuteQuery(querydel);

            string[] validFileTypes = { "png", "jpg", "jpeg" };
            Stream fs = FileUpload1.PostedFile.InputStream;
            BinaryReader br = new BinaryReader(fs);
            Byte[] image = br.ReadBytes((Int32)fs.Length);

            string exten = System.IO.Path.GetExtension(FileUpload1.FileName);
            string name = System.IO.Path.GetFileName(FileUpload1.FileName);

            string ext = System.IO.Path.GetExtension(FileUpload1.FileName);

            bool isValidFile = false;
            string imgname = "", imgnamenew = "";
            if (FileUpload1.HasFile)
            {
                for (int i = 0; i < validFileTypes.Length; i++)
                {
                    if (ext == "." + validFileTypes[i])
                    {
                        isValidFile = true;
                        break;
                    }
                    else
                    {
                        //lblmsg.Text = "Line : 349 : " + aa.Message;
                    }
                }
                if (!isValidFile)
                {
                    return;
                }
                ext = ext.Replace(".", "images/");
                imgname = DateTime.Now.Ticks + ext;
                imgnamenew = FileUpload1.FileName;
                string path = Server.MapPath("/ProductImage");
                FileUpload1.SaveAs(path + "/" + imgnamenew);
                flname = imgnamenew;
            }

            string[] para1 = { pid, flname.ToString(), "0", displayorder.Text.ToString(), dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss"), dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss") };

            string query1 = "INSERT INTO [dbo].[ProductImages]([ProductId],[ImageFileName],[IsDeleted],[DisplayOrder],[Doc],[Dom]) VALUES (@1,@2,@3,@4,@5,@6)";
            int v1 = dbc.ExecuteQueryWithParams(query1, para1);

            //string[] para11 = { name.ToString(), displayorder.Text.ToString(), dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss"), pid, imageId };

            //string query = "UPDATE [ProductImages] SET [ImageFileName]=@1,[DisplayOrder]=@2,[Dom]=@3 where [ProductId]=@4 and [Id]=@5";
            //int v11 = dbc.ExecuteQueryWithParams(query, para11);
        }
        else
        {
            // use previous user image if new image is not changed    
            //Image img = (Image)gvImage.Rows[e.RowIndex].FindControl("Image");
            //path = "~/ProductImage/" + name;
            string[] para11 = { displayorder.Text.ToString(), dbc.getindiantime().ToString("dd-MMM-yyyy HH:mmm:ss"), pid, imageId };

            string query = "UPDATE [ProductImages] SET [DisplayOrder]=@1,[Dom]=@2 where [ProductId]=@3 and [Id]=@4";
            int v11 = dbc.ExecuteQueryWithParams(query, para11);
        }


        gvImage.EditIndex = -1;
        ImageData();
    }
    // cancel edit event    
    protected void gvImage_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        gvImage.EditIndex = -1;
        ImageData();
    }
    //delete event    
    protected void gvImage_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        try
        {
            GridViewRow row = (GridViewRow)gvImage.Rows[e.RowIndex];
            Label lbldeleteid = (Label)row.FindControl("lblImgId");
            Label lblDeleteImageName = (Label)row.FindControl("lblImageName");

            // string[] para1 = {  name.Text.ToString(), displayorder.Text.ToString() , dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss"), id1 };
            int id = int.Parse(gvImage.DataKeys[e.RowIndex].Value.ToString());
            string query = "delete from [ProductImages] where Id=" + id;
            int v1 = dbc.ExecuteQuery(query);

            ImageDeleteFromFolder(lblDeleteImageName.Text);
            ImageData();
        }
        catch (Exception ex)
        {

        }
    }
    /// <summary>  
    /// This function is used to delete image from folder when deleting in gridview.  
    /// </summary>  
    /// <param name="imagename">image name</param>  
    protected void ImageDeleteFromFolder(string imagename)
    {
        string file_name = imagename;
        string path = Server.MapPath("/ProductImage/" + imagename);
        FileInfo file = new FileInfo(path);
        if (file.Exists) //check file exsit or not  
        {
            file.Delete();
            lblResult.Text = file_name + " file deleted successfully";
            lblResult.ForeColor = System.Drawing.Color.Green;
        }
        else
        {
            lblResult.Text = file_name + " This file does not exists ";
            lblResult.ForeColor = System.Drawing.Color.Red;
        }
    }
    protected void FileUploadMainImages_DataBinding(object sender, EventArgs e)
    {
        try
        {
            if (FileUploadMainImages.HasFile)
            {
                productimg.ImageUrl = FileUploadMainImages.FileName;
                MainFileName = FileUploadMainImages.FileName;
            }
        }
        catch (Exception ex)
        {

        }

    }
    protected void FileUpload2_DataBinding(object sender, EventArgs e)
    {
        try
        {
            if (FileUpload2.HasFile)
            {
                productimg1.ImageUrl = FileUpload2.FileName;
                //FileName = FileUpload1.FileName;
            }
        }
        catch (Exception ex)
        {

        }

    }

    protected void FileUploadMainImages_Load(object sender, EventArgs e)
    {
        try
        {
            if (FileUploadMainImages.HasFile)
            {
                productimg.ImageUrl = FileUploadMainImages.FileName;
                MainFileName = FileUploadMainImages.FileName;
            }
        }
        catch (Exception ex)
        {

        }
    }

    protected void FileUpload2_Load(object sender, EventArgs e)
    {
        try
        {
            if (FileUpload2.HasFile)
            {
                productimg1.ImageUrl = FileUpload2.FileName;
                // FileName = FileUpload1.FileName;
            }
        }
        catch (Exception ex)
        {
        }
    }
    protected void FileUploadMainImagesControl_Init(object sender, EventArgs e)
    {
        try
        {
            if (FileUploadMainImages.HasFile)
            {
                productimg.ImageUrl = FileUploadMainImages.FileName;
            }
            //if (FileUpload2.HasFile)
            //{
            //    productimg1.ImageUrl = FileUpload2.FileName;
            //}
        }
        catch (Exception ex)
        {
        }
    }
    protected void FileUpload2Control_Init(object sender, EventArgs e)
    {
        try
        {

            if (FileUpload2.HasFile)
            {
                productimg1.ImageUrl = FileUpload2.FileName;
            }
        }
        catch (Exception ex)
        {
        }
    }
    public void filedel()
    {
        try
        {
            if (!String.IsNullOrWhiteSpace(Request.QueryString["Id"]))
            {
                DataTable dt = dbc.GetDataTable("select [Id],[ImageFileName] from [ProductImages] where ProductId=" + Request.QueryString["ID"].ToString() + "");

                if (dt != null && dt.Rows.Count > 0)
                {

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string path = Server.MapPath("/ProductImage/" + dt.Rows[i]["ImageFileName"].ToString());
                        FileInfo file = new FileInfo(path);
                        if (file.Exists) //check file exsit or not  
                        {
                            file.Delete();
                        }
                    }
                    string querydel = "DELETE from [dbo].[ProductImages] where ProductId=" + Request.QueryString["Id"];
                    int vdel = dbc.ExecuteQuery(querydel);
                }
                DataTable dt2 = dbc.GetDataTable("SELECT top 1 [ImageFileName] FROM [SalebhaiOnePage].[dbo].[ProductImages] where IsDeleted=0 and ProductId=" + Request.QueryString["Id"]);
                if (dt2.Rows.Count == 0)
                {
                    //productimg.ImageUrl = "../ProductImage/" + dt2.Rows[0]["ImageFileName"].ToString();
                }
                ImageData();
            }
        }
        catch (Exception ee)
        {
        }
    }

    public void filedel1()
    {
        try
        {
            if (!String.IsNullOrWhiteSpace(Request.QueryString["Id"]))
            {
                DataTable dt = dbc.GetDataTable("select OGImage from Product where Id=" + Request.QueryString["ID"].ToString() + "");

                if (dt != null && dt.Rows.Count > 0)
                {
                    string path = Server.MapPath("/ProductOGImage/" + dt.Rows[0]["OGImage"].ToString());
                    FileInfo file = new FileInfo(path);
                    if (file.Exists) //check file exsit or not  
                    {
                        file.Delete();
                    }
                    else
                    {
                    }
                    string querydel = "Update [dbo].[Product] set [OGImage]=''  where Id=" + Request.QueryString["ID"].ToString();
                    int vdel = dbc.ExecuteQuery(querydel);
                    DataTable dt1 = dbc.GetDataTable("SELECT [OGImage] FROM [SalebhaiOnePage].[dbo].[Product] where IsDeleted=0 and Id=" + Request.QueryString["Id"]);
                    if (dt1.Rows.Count > 0)
                    {
                        productimg1.ImageUrl = "../ProductOGImage/" + dt1.Rows[0]["OGImage"].ToString();
                    }

                }
            }
        }
        catch (Exception ee)
        {
        }
    }

    protected void ogbtnremoveimage_Click(object sender, EventArgs e)
    {
        try
        {
            filedel1();
        }
        catch (Exception aa)
        {
        }
    }

    protected void bannerremoveImage_Click1(object sender, EventArgs e)
    {
        try
        {
            filedel();
        }
        catch (Exception aa)
        {
        }
    }

    protected void BtnAdd_Click(object sender, EventArgs e)
    {
        try
        {
            string[] validFileTypes = { "png", "jpg", "jpeg" };
            Stream fs = FileUploadgrpImages.PostedFile.InputStream;
            BinaryReader br = new BinaryReader(fs);
            Byte[] image = br.ReadBytes((Int32)fs.Length);

            string exten = System.IO.Path.GetExtension(FileUploadgrpImages.FileName);
            string name = System.IO.Path.GetFileName(FileUploadgrpImages.FileName);

            string ext = System.IO.Path.GetExtension(FileUploadgrpImages.FileName);

            bool isValidFile = false, bisSelected = false, bisbestbuy = false;
            string imgname = "", imgnamenew = "";
            if (FileUploadgrpImages.HasFile)
            {
                for (int i = 0; i < validFileTypes.Length; i++)
                {
                    if (ext == "." + validFileTypes[i])
                    {
                        isValidFile = true;
                        break;
                    }
                    else
                    {
                    }
                }

                if (!isValidFile)
                {
                    return;
                }


                ext = ext.Replace(".", "images/");

                imgname = DateTime.Now.Ticks + exten;
                imgnamenew = FileUploadgrpImages.FileName;
                string path = Server.MapPath("/ProductAttributeImage");
                FileUploadgrpImages.SaveAs(path + "/" + imgnamenew);
            }
            string fileName = "", sPackingType = "";
            fileName = imgnamenew;
            bool isOutOfStock = false; int igrpIndex = 0, isQtyFreeze = 0;
            if (!string.IsNullOrEmpty(txtgrpPackingType.Text))
                sPackingType = txtgrpPackingType.Text;

            if (chkgrpIsOutOfStock.Checked)
                isOutOfStock = true;
            if (chkgrpIsSelected.Checked)
                bisSelected = true;

            if (chkgrpIsBestBuy.Checked)
                bisbestbuy = true;


            if (chkIsQuantityFreez.Checked)
                isQtyFreeze = 1;
           

            foreach (DataRow row in dtgrpProduct.Rows)
            {
                if (bisSelected)
                {
                    row[12] = "False";
                }
                if (bisbestbuy)
                {
                    row[17] = "False";
                }
                igrpIndex++;
            }


            this.BindgrpProductData();
            if (_lastIndex > -1)
            {
                dtgrpProduct.Rows[_lastIndex]["grpUnitId"] = ddlgrpUnitName.SelectedValue.ToString();
                dtgrpProduct.Rows[_lastIndex]["grpUnitName"] = ddlgrpUnitName.SelectedItem.ToString();
                dtgrpProduct.Rows[_lastIndex]["grpUnit"] = txtgrpUnit.Text;
                dtgrpProduct.Rows[_lastIndex]["grpMrp"] = txtgrpMRP.Text;
                dtgrpProduct.Rows[_lastIndex]["grpDiscountType"] = ddlgrpDiscountType.SelectedValue.ToString();
                dtgrpProduct.Rows[_lastIndex]["grpDiscount"] = txtgrpDiscount.Text;
                dtgrpProduct.Rows[_lastIndex]["grpSoshoPrice"] = txtgrpSoshoPrice.Text;
                dtgrpProduct.Rows[_lastIndex]["grpPackingType"] = sPackingType.ToString();
                dtgrpProduct.Rows[_lastIndex]["grpisSelected"] = bisSelected;
                dtgrpProduct.Rows[_lastIndex]["grpImage"] = GrpImage.ImageUrl.Replace("../ProductAttributeImage/", "");
                dtgrpProduct.Rows[_lastIndex]["grpMinQty"] = txtMinQty.Text;
                dtgrpProduct.Rows[_lastIndex]["grpMaxQty"] = txtMaxQty.Text;
                dtgrpProduct.Rows[_lastIndex]["grpIsQtyFreeze"] = isQtyFreeze;
                dtgrpProduct.Rows[_lastIndex]["grpisBestBuy"] = bisbestbuy;
                dtgrpProduct.Rows[_lastIndex]["grpFreezeQty"] = txtFreezeQty.Text;
                dtgrpProduct.Rows[_lastIndex]["grpisOutOfStock"] = isOutOfStock;
                //dtgrpProduct.Rows[_lastIndex]["HiddenFieldgrpid"] = grpId;
            }
            else
            {
                dtgrpProduct.Rows.Add(
                     ddlgrpUnitName.SelectedItem.Text
                    , txtgrpUnit.Text
                    , txtgrpMRP.Text
                    , ddlgrpDiscountType.SelectedValue.ToString()
                    , txtgrpDiscount.Text
                    , txtgrpSoshoPrice.Text
                    , sPackingType.ToString()
                    , "", "", ddlgrpUnitName.SelectedValue.ToString(), fileName, isOutOfStock, bisSelected, "", txtMinQty.Text,
                     txtMaxQty.Text,isQtyFreeze,bisbestbuy,txtFreezeQty.Text
                    );

                DataTable dtgrpProduct1 = ViewState["dt"] as DataTable;
                if (dtgrpProduct1 != null)
                {
                    dtgrpProduct1 = ViewState["dt"] as DataTable;
                    foreach (DataRow row in dtgrpProduct1.Rows)
                    {
                        if (bisSelected)
                        {
                            row[12] = "False";
                        }
                        if (bisbestbuy)
                        {
                            row[17] = "False";
                        }
                        //if (Convert.ToBoolean(row["grpisOutOfStock"]) == true)
                        //    isOutOfStock = true;

                        //if (Convert.ToBoolean(row["grpisSelected"]) == true)
                        //    bisSelected = true;

                        //if (Convert.ToBoolean(row["grpisBestBuy"]) == true)
                        //    bisbestbuy = true;

                        isOutOfStock = Convert.ToBoolean(row["grpisOutOfStock"]);
                        var isSelected = Convert.ToBoolean(row["grpisSelected"]);
                        var isbestbuy = Convert.ToBoolean(row["grpisBestBuy"]);

                        if (Convert.ToBoolean(row["grpIsQtyFreeze"]) == true)
                            isQtyFreeze = 1;

                    //    dtgrpProduct.Rows.Add(
                    //     row["grpunitname"].ToString()
                    //    , row["grpUnit"]
                    //    , row["grpMrp"]
                    //    , row["grpDiscountType"]
                    //    , row["grpDiscount"]
                    //    , row["grpSoshoPrice"]
                    //    , row["grpPackingType"].ToString()
                    //    , row["Id"]
                    //    , row["grpId"]
                    //    , row["grpUnitId"]
                    //    , row["grpimage"].ToString()
                    //     , isOutOfStock, bisSelected, "",
                    //     row["grpMinQty"],
                    //     row["grpMaxQty"],
                    //     isQtyFreeze,
                    //     bisbestbuy,
                    //     row["grpFreezeQty"]
                    //);
                        dtgrpProduct.Rows.Add(
                       row["grpunitname"].ToString()
                      , row["grpUnit"]
                      , row["grpMrp"]
                      , row["grpDiscountType"]
                      , row["grpDiscount"]
                      , row["grpSoshoPrice"]
                      , row["grpPackingType"].ToString()
                      , row["Id"]
                      , row["grpId"]
                      , row["grpUnitId"]
                      , row["grpimage"].ToString()
                       , isOutOfStock, isSelected, "",
                       row["grpMinQty"],
                       row["grpMaxQty"],
                       isQtyFreeze,
                       isbestbuy,
                       row["grpFreezeQty"]
                  );
                    }
                }
            }
            BindgrpProductData();

            txtgrpUnit.Text = String.Empty;
            txtgrpMRP.Text = String.Empty;
            txtgrpDiscount.Text = String.Empty;
            txtgrpPackingType.Text = String.Empty;
            txtgrpSoshoPrice.Text = String.Empty;
            txtMinQty.Text = "1";
            txtMaxQty.Text = "99";
            txtFreezeQty.Text = "1";
            chkIsQuantityFreez.Checked = false;
            chkgrpIsOutOfStock.Checked = false;
            chkgrpIsSelected.Checked = false;
            chkgrpIsBestBuy.Checked = false;

            if (BtnAdd.Text.Equals("Update"))
            {
                //int isQtyFreeze = 0;
                isQtyFreeze = Convert.ToInt32(dtgrpProduct.Rows[_lastIndex]["grpIsQtyFreeze"]);

                //GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;
                if (_lastIndex >= 0)
                {
                    if (isQtyFreeze == 1)
                        chkIsQuantityFreez.Checked = true;

                    ddlgrpDiscountType.SelectedValue = dtgrpProduct.Rows[_lastIndex]["grpDiscountType"].ToString();
                    txtgrpMRP.Text = dtgrpProduct.Rows[_lastIndex]["grpMrp"].ToString();
                    txtgrpUnit.Text = dtgrpProduct.Rows[_lastIndex]["grpUnit"].ToString();
                    txtgrpDiscount.Text = dtgrpProduct.Rows[_lastIndex]["grpDiscount"].ToString();
                    txtgrpSoshoPrice.Text = dtgrpProduct.Rows[_lastIndex]["grpSoshoPrice"].ToString();
                    txtgrpPackingType.Text = dtgrpProduct.Rows[_lastIndex]["grpPackingType"].ToString();
                    chkgrpIsSelected.Checked = Convert.ToBoolean(dtgrpProduct.Rows[_lastIndex]["grpisSelected"]);
                    chkgrpIsBestBuy.Checked = Convert.ToBoolean(dtgrpProduct.Rows[_lastIndex]["grpisBestBuy"]);
                    //GrpImage.ImageUrl = "../ProductAttributeImage/" + dtgrpProduct.Rows[_lastIndex]["grpImage"].ToString();
                    GrpImage.ImageUrl = dtgrpProduct.Rows[_lastIndex]["grpImage"].ToString();
                    ddlgrpUnitName.SelectedValue = dtgrpProduct.Rows[_lastIndex]["grpUnitId"].ToString();

                    txtMinQty.Text = dtgrpProduct.Rows[_lastIndex]["grpMinQty"].ToString();
                    txtMaxQty.Text = dtgrpProduct.Rows[_lastIndex]["grpMaxQty"].ToString();
                    txtFreezeQty.Text = dtgrpProduct.Rows[_lastIndex]["grpFreezeQty"].ToString();
                    //chkIsQuantityFreez.Checked = true;//Convert.ToBoolean(dtgrpProduct.Rows[_lastIndex]["grpIsQtyFreeze"]); 

                    grdgProduct.EditIndex = -1;
                    this.BindgrpProductData();

                    txtgrpUnit.Text = String.Empty;
                    txtgrpMRP.Text = String.Empty;
                    txtgrpDiscount.Text = String.Empty;
                    txtgrpPackingType.Text = String.Empty;
                    txtgrpSoshoPrice.Text = String.Empty;
                    txtMinQty.Text = "1";
                    txtMaxQty.Text = "99";
                    txtFreezeQty.Text = "1";
                    chkIsQuantityFreez.Checked = false;
                    chkgrpIsOutOfStock.Checked = false;
                    chkgrpIsSelected.Checked = false;
                    chkgrpIsBestBuy.Checked = false;
                    GrpImage.ImageUrl = "";
                    BtnAdd.Text = "Add";
                    _lastIndex = -1;
                }

            }
        }
        catch (Exception aa)
        {
            sweetMessage("", "Please Try Again!!", "warning");
        }
    }

    protected void BtnCategoryAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (_lastIndexCategory > -1)
            {
                dtProductCategory.Rows[_lastIndexCategory]["hdnlinkCategoryId"] = ddlLinkCategoryName.SelectedValue.ToString();
                dtProductCategory.Rows[_lastIndexCategory]["linkProdCategory"] = ddlLinkCategoryName.SelectedItem.ToString();
                dtProductCategory.Rows[_lastIndexCategory]["hdnlinkSubCategoryId"] = ddlLinkSubCategoryName.SelectedValue.ToString();
                dtProductCategory.Rows[_lastIndexCategory]["linkProdSubCategory"] = ddlLinkSubCategoryName.SelectedItem.ToString();
            }
            else
            {
                dtProductCategory.Rows.Add(
                     ddlLinkCategoryName.SelectedValue.ToString()
                     ,ddlLinkCategoryName.SelectedItem.Text
                     , ddlLinkSubCategoryName.SelectedValue.ToString()
                     ,ddlLinkSubCategoryName.SelectedItem.Text
                    );

                DataTable dtProductCategory1 = ViewState["dtProductCategory"] as DataTable;
                if (dtProductCategory1 != null)
                {
                    dtProductCategory1 = ViewState["dtProductCategory"] as DataTable;
                    foreach (DataRow row in dtProductCategory1.Rows)
                    {

                        dtProductCategory.Rows.Add(
                            row["hdnlinkCategoryId"]
                         ,row["linkProdCategory"].ToString()
                        , row["hdnlinkSubCategoryId"]
                        , row["linkProdSubCategory"]
                    );
                    }
                }
            }
            BindProductCategoryData();

        }
        catch (Exception aa)
        {
            sweetMessage("", "Please Try Again!!", "warning");
        }
    }
    protected void OnUpdate(object sender, EventArgs e)
    {
        int isQtyFreeze = 0;
        isQtyFreeze = Convert.ToInt32(dtgrpProduct.Rows[_lastIndex]["grpIsQtyFreeze"]);
        
        GridViewRow row = (sender as LinkButton).NamingContainer as GridViewRow;
        if (_lastIndex >= 0)
        {
            if (isQtyFreeze == 1)
                chkIsQuantityFreez.Checked = true;

            ddlgrpDiscountType.SelectedValue = dtgrpProduct.Rows[_lastIndex]["grpDiscountType"].ToString();
            txtgrpMRP.Text = dtgrpProduct.Rows[_lastIndex]["grpMrp"].ToString();
            txtgrpUnit.Text = dtgrpProduct.Rows[_lastIndex]["grpUnit"].ToString();
            txtgrpDiscount.Text = dtgrpProduct.Rows[_lastIndex]["grpDiscount"].ToString();
            txtgrpSoshoPrice.Text = dtgrpProduct.Rows[_lastIndex]["grpSoshoPrice"].ToString();
            txtgrpPackingType.Text = dtgrpProduct.Rows[_lastIndex]["grpPackingType"].ToString();
            chkgrpIsSelected.Checked = Convert.ToBoolean(dtgrpProduct.Rows[_lastIndex]["grpisSelected"]);
            chkgrpIsBestBuy.Checked = Convert.ToBoolean(dtgrpProduct.Rows[_lastIndex]["grpisBestBuy"]);
            //GrpImage.ImageUrl = "../ProductAttributeImage/" + dtgrpProduct.Rows[_lastIndex]["grpImage"].ToString();
            GrpImage.ImageUrl = dtgrpProduct.Rows[_lastIndex]["grpImage"].ToString();
            ddlgrpUnitName.SelectedValue= dtgrpProduct.Rows[_lastIndex]["grpUnitId"].ToString();

            txtMinQty.Text = dtgrpProduct.Rows[_lastIndex]["grpMinQty"].ToString();
            txtMaxQty.Text = dtgrpProduct.Rows[_lastIndex]["grpMaxQty"].ToString();
            txtFreezeQty.Text = dtgrpProduct.Rows[_lastIndex]["grpFreezeQty"].ToString();
            //chkIsQuantityFreez.Checked = true;//Convert.ToBoolean(dtgrpProduct.Rows[_lastIndex]["grpIsQtyFreeze"]); 

            grdgProduct.EditIndex = -1;
            this.BindgrpProductData();

            txtgrpUnit.Text = String.Empty;
            txtgrpMRP.Text = String.Empty;
            txtgrpDiscount.Text = String.Empty;
            txtgrpPackingType.Text = String.Empty;
            txtgrpSoshoPrice.Text = String.Empty;
            txtMinQty.Text = "1";
            txtMaxQty.Text = "99";
            txtFreezeQty.Text = "1";
            chkgrpIsOutOfStock.Checked = false;
            chkgrpIsSelected.Checked = false;
            chkgrpIsBestBuy.Checked = false;
            GrpImage.ImageUrl = "";
            BtnAdd.Text = "Add";
            _lastIndex = -1;
        }
    }

    protected void OnCancel(object sender, EventArgs e)
    {
        BtnAdd.Text = "Add";
        grdgProduct.EditIndex = -1;
        this.BindgrpProductData();
    }

    protected void OnRow_Editing(object sender, GridViewEditEventArgs e)
    {
        e.NewEditIndex = e.NewEditIndex;
        grdgProduct.EditIndex = e.NewEditIndex;
        BindgrpProductData();

        _lastIndex = e.NewEditIndex;
        int isQtyFreeze = 0;
        

        //Bind data to the GridView control.
        if (dtgrpProduct.Rows.Count == 0)
        {
            dtgrpProduct = ViewState["dt"] as DataTable;
        }
        if (_lastIndex >= 0)
        {
            isQtyFreeze = Convert.ToInt32(dtgrpProduct.Rows[_lastIndex]["grpIsQtyFreeze"]);
            if (isQtyFreeze == 1)
            {
                chkIsQuantityFreez.Checked = true;
                txtFreezeQty.Enabled = true;
            }
            else{
                chkIsQuantityFreez.Checked = false;
                txtFreezeQty.Enabled = false;
            }
               

            ddlgrpDiscountType.SelectedValue = dtgrpProduct.Rows[_lastIndex]["grpDiscountType"].ToString();
            txtgrpMRP.Text = dtgrpProduct.Rows[_lastIndex]["grpMrp"].ToString();
            txtgrpUnit.Text = dtgrpProduct.Rows[_lastIndex]["grpUnit"].ToString();
            txtgrpDiscount.Text = dtgrpProduct.Rows[_lastIndex]["grpDiscount"].ToString();
            txtgrpSoshoPrice.Text = dtgrpProduct.Rows[_lastIndex]["grpSoshoPrice"].ToString();
            txtgrpPackingType.Text = dtgrpProduct.Rows[_lastIndex]["grpPackingType"].ToString();
            chkgrpIsSelected.Checked = Convert.ToBoolean(dtgrpProduct.Rows[_lastIndex]["grpisSelected"]);
            chkgrpIsBestBuy.Checked = Convert.ToBoolean(dtgrpProduct.Rows[_lastIndex]["grpisBestBuy"]);
            GrpImage.ImageUrl = "../ProductAttributeImage/" + dtgrpProduct.Rows[_lastIndex]["grpImage"].ToString();
            ddlgrpUnitName.SelectedValue = dtgrpProduct.Rows[_lastIndex]["grpUnitId"].ToString();
            txtMinQty.Text = dtgrpProduct.Rows[_lastIndex]["grpMinQty"].ToString();
            txtMaxQty.Text = dtgrpProduct.Rows[_lastIndex]["grpMaxQty"].ToString();
            txtFreezeQty.Text = dtgrpProduct.Rows[_lastIndex]["grpFreezeQty"].ToString();
            chkgrpIsOutOfStock.Checked = Convert.ToBoolean(dtgrpProduct.Rows[_lastIndex]["grpisOutOfStock"]);

            //chkIsQuantityFreez.Checked = Convert.ToBoolean(dtgrpProduct.Rows[_lastIndex]["grpIsQtyFreeze"]);

            BtnAdd.Text = "Update";
        }

    }

    protected void OnRowCategory_Editing(object sender, GridViewEditEventArgs e)
    {
        e.NewEditIndex = e.NewEditIndex;
        grdProductCategory.EditIndex = e.NewEditIndex;
        BindProductCategoryData();

        _lastIndexCategory = e.NewEditIndex;
        
        //Bind data to the GridView control.
        if (dtProductCategory.Rows.Count == 0)
        {
            dtProductCategory = ViewState["dtProductCategory"] as DataTable;
        }
        if (_lastIndexCategory >= 0)
        {
            
            ddlLinkCategoryName.SelectedValue = dtProductCategory.Rows[_lastIndexCategory]["hdnlinkCategoryId"].ToString();
            string subcategoryqry = "SELECT Id,SubCategory FROM tblSubCategory where isnull(IsDeleted,0)=0 AND CategoryId = '" + ddlLinkCategoryName.SelectedValue + "' order by Sequence asc";
            DataTable dtsubcategory = dbc.GetDataTable(subcategoryqry);
            ddlLinkSubCategoryName.DataSource = dtsubcategory;
            ddlLinkSubCategoryName.DataTextField = "SubCategory";
            ddlLinkSubCategoryName.DataValueField = "Id";
            ddlLinkSubCategoryName.DataBind();
            ddlLinkSubCategoryName.SelectedValue = dtProductCategory.Rows[_lastIndexCategory]["hdnlinkSubCategoryId"].ToString();
            
            btnCategoryAdd.Text = "Update";
        }

    }

    public void BindgrpProductData()
    {
        if (dtgrpProduct.Rows.Count > 0)
        {
            grdgProduct.DataSource = dtgrpProduct;
        }
        else
        {
            grdgProduct.DataSource = ViewState["dt"];

        }
        grdgProduct.DataBind();
    }

    public void BindProductCategoryData()
    {
        if (dtProductCategory.Rows.Count > 0)
        {
            grdProductCategory.DataSource = dtProductCategory;
        }
        else
        {
            grdProductCategory.DataSource = ViewState["dtProductCategory"];

        }
        grdProductCategory.DataBind();
    }

    protected void BtnRemoveImage_Click(object sender, EventArgs e)
    {
        try
        {
            filedelgrp();
        }
        catch (Exception aa)
        {

        }
    }
    public void filedelgrp()
    {
        try
        {
            if (!String.IsNullOrWhiteSpace(Request.QueryString["Id"]))
            {

                DataTable dt = dbc.GetDataTable("select CategoryImage from Category where Id=" + Request.QueryString["ID"].ToString() + "");


                if (dt != null && dt.Rows.Count > 0)
                {

                    string path = dt.Rows[0]["CategoryImage"].ToString();


                    if (File.Exists(path))
                    {
                        File.Delete(path);
                    }
                    else
                    {

                    }
                }
            }
        }
        catch (Exception ee)
        {
        }
    }
    protected void FileUploadgrpImages_DataBinding(object sender, EventArgs e)
    {
        try
        {
            if (FileUploadgrpImages.HasFile)
            {
                GrpImage.ImageUrl = FileUploadgrpImages.FileName;
                grpFileName = FileUploadgrpImages.FileName;
            }
        }
        catch (Exception ex)
        {

        }

    }
    protected void FileUploadgrpImages_Load(object sender, EventArgs e)
    {
        try
        {
            if (FileUploadgrpImages.HasFile)
            {
                GrpImage.ImageUrl = FileUploadgrpImages.FileName;
                grpFileName = FileUploadgrpImages.FileName;
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void FileUploadgrpImagesControl_Init(object sender, EventArgs e)
    {
        try
        {
            if (FileUploadgrpImages.HasFile)
            {
                GrpImage.ImageUrl = FileUploadgrpImages.FileName;
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            string userId = Request.Cookies["TUser"]["Id"].ToString();
            int IsDeleted = 0, iisOutOfStock = 0;
            if (chkgrpIsDeleted.Checked)
                IsDeleted = 1;

            id = Request.QueryString["Id"];
            string grpId = Request.QueryString["grpId"];
            string sUnit = txtgrpUnit.Text.ToString();
            string sUnitId = ddlgrpUnitName.SelectedValue.ToString();
            string sMrp = txtgrpMRP.Text.ToString();
            string sDiscountType = ddlgrpDiscountType.SelectedValue.ToString();
            string sDiscount = txtgrpDiscount.Text.ToString();
            string sSoshoPrice = txtgrpSoshoPrice.Text.ToString();
            string sPackingType = txtgrpPackingType.Text.ToString();
            string grpImage = FileUploadgrpImages.FileName;
            string FreezeQty = txtFreezeQty.Text;

            if (chkgrpIsOutOfStock.Checked)
                iisOutOfStock = 1;
            if (id != null && !id.Equals("") && grpId != null && !grpId.Equals(""))
            {
                string updateqty = "UPDATE [Product_ProductAttribute_Mapping] set Unit = '" + sUnit + "',UnitId=" + sUnitId + ",Mrp='" + sMrp + "',DiscountType='" + sDiscountType + "',Discount='" + sDiscount + "',SoshoPrice='" + sSoshoPrice + "',PackingType='" + sPackingType + "',isOutOfStock=" + iisOutOfStock + ",IsDeleted = " + IsDeleted + ", ProductImage = '" + grpImage + "', FreezeQty = " + FreezeQty + " Where Id=" + grpId + " and ProductId = " + id;
                int VALupdate = dbc.ExecuteQuery(updateqty);
                if (VALupdate > 0)
                {
                    sweetMessage("", "Product Updated Successfully", "success");
                    Response.Redirect("ManageProducts.aspx?Id=" + id);
                }
                else
                {
                    sweetMessage("", "Please Try Again!!", "warning");
                }
            }

        }
        catch (Exception aa)
        {
            sweetMessage("", "Please Try Again!!", "warning");
        }
    }

    protected void ChckedChanged(Object sender, EventArgs e)
    {
        if(chkIsFixedShipping.Checked == true)
            txtFixedShipRate.Enabled = true;
        else
            txtFixedShipRate.Enabled = false;

    }
    protected void FreezeQtyChckedChanged(Object sender, EventArgs e)
    {
        if (chkIsQuantityFreez.Checked == true)
            txtFreezeQty.Enabled = true;
        else
            txtFreezeQty.Enabled = false;

    }

    //protected void OnSelectedCategoryChanged(object sender, EventArgs e)
    //{
    //    ddlSubCategoryName.Items.Clear();
    //    string subcategoryqry = "SELECT Id,SubCategory FROM tblSubCategory where isnull(IsDeleted,0)=0 AND CategoryId = '" + ddlCategoryName.SelectedValue + "' order by Sequence asc";
    //    DataTable dtsubcategory = dbc.GetDataTable(subcategoryqry);
    //    ddlSubCategoryName.DataSource = dtsubcategory;
    //    ddlSubCategoryName.DataTextField = "SubCategory";
    //    ddlSubCategoryName.DataValueField = "Id";
    //    ddlSubCategoryName.DataBind();
    //}

    protected void OnSelectedLinkCategoryChanged(object sender, EventArgs e)
    {
        ddlLinkSubCategoryName.Items.Clear();
        string subcategoryqry = "SELECT Id,SubCategory FROM tblSubCategory where isnull(IsDeleted,0)=0 AND CategoryId = '" + ddlLinkCategoryName.SelectedValue + "' order by Sequence asc";
        DataTable dtsubcategory = dbc.GetDataTable(subcategoryqry);
        ddlLinkSubCategoryName.DataSource = dtsubcategory;
        ddlLinkSubCategoryName.DataTextField = "SubCategory";
        ddlLinkSubCategoryName.DataValueField = "Id";
        ddlLinkSubCategoryName.DataBind();
    }
    [WebMethod]
    public static List<string> GetProductName(string prefixText)
    {
        dbConnection dbc = new dbConnection();
        using (SqlConnection conn = new SqlConnection())
        {
            conn.ConnectionString = dbc.consString;
            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandText = "select Name, Id from Product where Name like @SearchText + '%'";
                cmd.Parameters.AddWithValue("@SearchText", prefixText);
                cmd.Connection = conn;
                conn.Open();
                List<string> products = new List<string>();
                using (SqlDataReader sdr = cmd.ExecuteReader())
                {
                    while (sdr.Read())
                    {
                        products.Add(sdr["Name"].ToString());
                    }
                }
                conn.Close();
                return products;
            }
        }
    }
    
}