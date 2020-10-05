using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Banner_UploadBanner : System.Web.UI.Page
{
    public class ActionType
    {
        // Auto-Initialized properties  
        public string Text { get; set; }
        public int Value { get; set; }
    }
    dbConnection dbc = new dbConnection();
    string FileName = "", id = "", intermediateFileName = ""; string typeid = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                string bannerqry = "Select Id as Id,BannerName as Name from BannerTypeMaster where IsActive = 1 order by Id";
                DataTable dtbanner = dbc.GetDataTable(bannerqry);
                ddlBannerType.DataSource = dtbanner;
                ddlBannerType.DataTextField = "Name";
                ddlBannerType.DataValueField = "Id";
                ddlBannerType.DataBind();
                ddlBannerType.Items.Insert(0, new ListItem("Select Banner Type", ""));

                string categoryqry = "SELECT CategoryId,CategoryName FROM Category where isnull(IsDeleted,0)=0 order by Sequence asc";
                DataTable dtcategory = dbc.GetDataTable(categoryqry);

                ddlbasicCategory.DataSource = dtcategory;
                ddlbasicCategory.DataTextField = "CategoryName";
                ddlbasicCategory.DataValueField = "CategoryId";
                ddlbasicCategory.DataBind();

                string productqry = "SELECT Id,Name FROM Product where isnull(IsDeleted,0)=0 order by Id asc";
                DataTable dtproduct = dbc.GetDataTable(productqry);

                ddlbasicProduct.DataSource = dtproduct;
                ddlbasicProduct.DataTextField = "Name";
                ddlbasicProduct.DataValueField = "Id";
                ddlbasicProduct.DataBind();

                List<ActionType> ActionList = new List<ActionType>
                {
                                    new ActionType { Text = "Select Action Name", Value = 0},
                                    new ActionType { Text = clsCommon.BannerActionType.OpenUrl.ToString(), Value = clsCommon.BannerActionType.OpenUrl.GetHashCode()},
                    new ActionType { Text = clsCommon.BannerActionType.NavigateToCategory.ToString(), Value = clsCommon.BannerActionType.NavigateToCategory.GetHashCode()},
                    new ActionType { Text = clsCommon.BannerActionType.AddToCart.ToString(), Value = clsCommon.BannerActionType.AddToCart.GetHashCode()},
                    new ActionType { Text = clsCommon.BannerActionType.None.ToString(), Value = clsCommon.BannerActionType.None.GetHashCode()},
                };

                ddlbasicAction.DataTextField = "Text";
                ddlbasicAction.DataValueField = "Value";
                ddlbasicAction.DataSource = ActionList;
                ddlbasicAction.DataBind();

                string IsAdmin = Request.Cookies["TUser"]["IsAdmin"].ToString();
                string sJurisdictionId = Request.Cookies["TUser"]["JurisdictionID"].ToString();
                if (IsAdmin == "True")
                {
                    divBasicJurisdictionIncharge.Visible = true;

                    string JurisdictionInchargeqry = "Select Distinct JurisdictionId,JurisdictionIncharge From JurisdictionMaster where IsActive = 1 order by JurisdictionId";
                    DataTable dtIncharge = dbc.GetDataTable(JurisdictionInchargeqry);
                    chklstBasicJurisdictionIncharge.DataSource = dtIncharge;
                    chklstBasicJurisdictionIncharge.DataTextField = "JurisdictionIncharge";
                    chklstBasicJurisdictionIncharge.DataValueField = "JurisdictionId";
                    chklstBasicJurisdictionIncharge.DataBind();

                    chklstBasicJurisdictionIncharge.DataSource = dtIncharge;
                    chklstBasicJurisdictionIncharge.DataTextField = "JurisdictionIncharge";
                    chklstBasicJurisdictionIncharge.DataValueField = "JurisdictionId";
                    chklstBasicJurisdictionIncharge.DataBind();
                }
                else
                {
                    divBasicJurisdictionIncharge.Visible = false;
                }

                txtdt.Text = dbc.getindiantime().ToString("dd/MMM/yyyy");
                txtdt1.Text = dbc.getindiantime().ToString("dd/MMM/yyyy");
                chkisactive.Checked = true;

                txtbasicLink.Visible = false;
                lblbasicLink.Visible = false;
                ddlbasicCategory.Visible = false;
                lblbasicCategory.Visible = false;
                ddlbasicProduct.Visible = false;
                lblbasicProduct.Visible = false;
                dvbasicLink.Style.Add("display", "none");
                dvbasicCategory.Style.Add("display", "none");
                dvbasicProduct.Style.Add("display", "none");

                id = Request.QueryString["Id"];
                typeid = Request.QueryString["TypeId"];
                if (id != null && !id.Equals(""))
                {
                    BtnSave.Text = "Update";
                    DataTable dt1 = new DataTable();
                    DataTable Jurisdictiondt = new DataTable();
                    if (typeid == "1")
                    {
                        dt1 = dbc.GetDataTable("SELECT  [Title],[AltText],[Link],[StartDate],[EndDate],[IsActive],[ImageName],[ActionId],[CategoryId],[ProductId] FROM [dbo].[HomepageBanner] where IsDeleted=0  and Id=" + id);
                        Jurisdictiondt = dbc.GetDataTable("SELECT  [JurisdictionId] FROM [dbo].[JurisdictionBanner] where BannerType = 'HomePage' AND BannerId=" + id);
                    }
                    else
                    {
                        dt1 = dbc.GetDataTable("SELECT  [Title],[AltText],[Link],[StartDate],[EndDate],[IsActive],[ImageName],[ActionId],[CategoryId],[ProductId] FROM [dbo].[IntermediateBanners] where IsDeleted=0  and Id=" + id);
                        Jurisdictiondt = dbc.GetDataTable("SELECT  [JurisdictionId] FROM [dbo].[JurisdictionBanner] where BannerType = 'Intermediate' AND BannerId=" + id);
                    }
                        if (dt1.Rows.Count > 0)
                        {
                        ddlBannerType.SelectedIndex = Convert.ToInt32(typeid);
                        txtTitle1.Text = dt1.Rows[0]["Title"].ToString();
                            txtAltText.Text = dt1.Rows[0]["AltText"].ToString();
                            //txtLink.Text = dt1.Rows[0]["Link"].ToString();
                            ddlbasicAction.SelectedIndex = Convert.ToInt32(dt1.Rows[0]["ActionId"]);
                            txtbasicLink.Text = dt1.Rows[0]["Link"].ToString();
                            ddlbasicCategory.SelectedValue = Convert.ToInt32(dt1.Rows[0]["CategoryId"]).ToString();
                            ddlbasicProduct.SelectedValue = Convert.ToInt32(dt1.Rows[0]["ProductId"]).ToString();

                            if (ddlbasicAction.SelectedIndex == clsCommon.BannerActionType.OpenUrl.GetHashCode())
                            {
                                if (!string.IsNullOrEmpty(txtbasicLink.Text))
                                {
                                    txtbasicLink.Visible = true;
                                    lblbasicLink.Visible = true;
                                    dvbasicLink.Style.Add("display", "block");
                                }
                            }
                            if (ddlbasicAction.SelectedIndex == clsCommon.BannerActionType.NavigateToCategory.GetHashCode())
                            {
                                if (ddlbasicCategory.SelectedIndex > 0)
                                {
                                    ddlbasicCategory.Visible = true;
                                    lblbasicCategory.Visible = true;
                                    dvbasicCategory.Style.Add("display", "block");
                                }
                            }
                            if (ddlbasicAction.SelectedIndex == clsCommon.BannerActionType.AddToCart.GetHashCode())
                            {
                                if (ddlbasicProduct.SelectedIndex > 0)
                                {
                                    ddlbasicProduct.Visible = true;
                                    lblbasicProduct.Visible = true;
                                    dvbasicProduct.Style.Add("display", "block");
                                }
                            }


                            if (dt1.Rows[0]["IsActive"].ToString() == "True")
                                chkisactive.Checked = true;
                            else
                                chkisactive.Checked = false;

                            string strtdate = dt1.Rows[0]["StartDate"].ToString();
                            string enddate = dt1.Rows[0]["EndDate"].ToString();


                            DateTime oDate = Convert.ToDateTime(strtdate);
                            string datetime = oDate.ToString("dd/MMM/yyyy HH:mm tt");
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
                            string datetime1 = oDate1.ToString("dd/MMM/yyyy HH:mm tt");
                            if (!String.IsNullOrEmpty(datetime1))
                            {
                                string[] dt11 = datetime1.Split(' ');
                                if (dt11.Length == 3)
                                {
                                    txtdt1.Text = dt11[0].ToString();
                                    txttime1.Text = dt11[1].ToString() + " " + dt11[2].ToString();
                                }

                            }


                            productimg.ImageUrl = "../BannerImage/" + dt1.Rows[0]["ImageName"].ToString();
                            
                            
                            if (Jurisdictiondt.Rows.Count > 0)
                            {
                                foreach (ListItem li in chklstBasicJurisdictionIncharge.Items)
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
                        }
                    
                    //if (typeid == "2")
                    //{
                    //    DataTable intermediatedt = dbc.GetDataTable("SELECT  [TypeId],[Title],[AltText],[Link],[StartDate],[EndDate],[IsActive],[ImageName],[ActionId],[CategoryId],[ProductId] FROM [dbo].[IntermediateBanners] where IsDeleted=0  and Id=" + id);
                    //    if (intermediatedt.Rows.Count > 0)
                    //    {
                    //        ddlBannerType.SelectedIndex = Convert.ToInt32(intermediatedt.Rows[0]["TypeId"]);
                    //        txtintermediateTitle.Text = intermediatedt.Rows[0]["Title"].ToString();
                    //        ddlintermedicateAction.SelectedIndex = Convert.ToInt32(intermediatedt.Rows[0]["ActionId"]);
                    //        txtintermediateLink.Text = intermediatedt.Rows[0]["Link"].ToString();
                    //        ddlintermedicateCategory.SelectedValue = Convert.ToInt32(intermediatedt.Rows[0]["CategoryId"]).ToString();
                    //        ddlintermedicateProduct.SelectedValue = Convert.ToInt32(intermediatedt.Rows[0]["ProductId"]).ToString();
                    //        txtintermediateAltText.Text = intermediatedt.Rows[0]["AltText"].ToString();
                    //        string strtdate = intermediatedt.Rows[0]["StartDate"].ToString();
                    //        string enddate = intermediatedt.Rows[0]["EndDate"].ToString();

                    //        if (ddlintermedicateAction.SelectedIndex == clsCommon.BannerActionType.OpenUrl.GetHashCode())
                    //        {
                    //            if (!string.IsNullOrEmpty(txtintermediateLink.Text))
                    //            {
                    //                txtintermediateLink.Visible = true;
                    //                lblintermediateLink.Visible = true;
                    //                dvintermediateLink.Style.Add("display", "block");
                    //            }
                    //        }
                    //        if (ddlintermedicateAction.SelectedIndex == clsCommon.BannerActionType.NavigateToCategory.GetHashCode())
                    //        {
                    //            if (ddlintermedicateCategory.SelectedIndex > 0)
                    //            {
                    //                ddlintermedicateCategory.Visible = true;
                    //                lblintermediateCategory.Visible = true;
                    //                dvintermediateCategory.Style.Add("display", "block");
                    //            }
                    //        }
                    //        if (ddlintermedicateAction.SelectedIndex == clsCommon.BannerActionType.AddToCart.GetHashCode())
                    //        {
                    //            if (ddlintermedicateProduct.SelectedIndex > 0)
                    //            {
                    //                ddlintermedicateProduct.Visible = true;
                    //                lblintermediateProduct.Visible = true;
                    //                dvintermediateProduct.Style.Add("display", "block");
                    //            }
                    //        }

                    //        DateTime oDate = Convert.ToDateTime(strtdate);
                    //        string datetime = oDate.ToString("dd/MMM/yyyy HH:mm tt");
                    //        if (!String.IsNullOrEmpty(datetime))
                    //        {
                    //            string[] dt = datetime.Split(' ');
                    //            if (dt.Length == 3)
                    //            {
                    //                txtintermediateStartDate.Text = dt[0].ToString();
                    //                txtintermediateStartDatetimepicker.Text = dt[1].ToString() + " " + dt[2].ToString();
                    //            }

                    //        }

                    //        DateTime oDate1 = Convert.ToDateTime(enddate);
                    //        string datetime1 = oDate1.ToString("dd/MMM/yyyy HH:mm tt");
                    //        if (!String.IsNullOrEmpty(datetime1))
                    //        {
                    //            string[] dt11 = datetime1.Split(' ');
                    //            if (dt11.Length == 3)
                    //            {
                    //                txtintermediateEndDate.Text = dt11[0].ToString();
                    //                txtintermediateEndDatetimepicker.Text = dt11[1].ToString() + " " + dt11[2].ToString();
                    //            }
                    //        }

                    //        intermediateimage.ImageUrl = "../IntermediateBannerImage/" + intermediatedt.Rows[0]["ImageName"].ToString();

                    //        DataTable Jurisdictiondt = dbc.GetDataTable("SELECT  [JurisdictionId] FROM [dbo].[JurisdictionBanner] where BannerId=" + id);
                    //        if (Jurisdictiondt.Rows.Count > 0)
                    //        {

                    //            foreach (ListItem li in chklstJurisdictionIncharge.Items)
                    //            {
                    //                for (int i = 0; i < Jurisdictiondt.Rows.Count; i++)
                    //                {
                    //                    if (li.Value == Jurisdictiondt.Rows[i]["JurisdictionId"].ToString())
                    //                    {
                    //                        li.Selected = true;
                    //                    }

                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                }
            }
            catch (Exception W) { }
        }
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string userId = Request.Cookies["TUser"]["Id"].ToString();
            int IsActive = 0;
            string id1 = Request.QueryString["Id"];
            string startdate = txtdt.Text.ToString();
            string starttime = txttime.Text.ToString();
            string enddate = txtdt1.Text.ToString();
            string endtime = txttime1.Text.ToString();

            string FROM1 = startdate + " " + starttime;
            string TO1 = enddate + " " + endtime;

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

                imgname = DateTime.Now.Ticks + exten;
                imgnamenew = FileUpload1.FileName;
                string path = Server.MapPath("/BannerImage");
                FileUpload1.SaveAs(path + "/" + imgnamenew);
            }

            string fileName = "";
            fileName = imgnamenew;


            if (chkisactive.Checked)
            {
                IsActive = 1;
            }

            string sCategoryId = "";
            if (string.IsNullOrEmpty(ddlbasicCategory.SelectedValue))
                sCategoryId = "0";
            else
                sCategoryId = ddlbasicCategory.SelectedValue.ToString();

            string sProductId = "";
            if (string.IsNullOrEmpty(ddlbasicProduct.SelectedValue))
                sProductId = "0";
            else
                sProductId = ddlbasicProduct.SelectedValue.ToString();

            int sActionId = 0;
            if (Convert.ToInt32(ddlbasicAction.SelectedValue) != 0)
                sActionId = Convert.ToInt32(ddlbasicAction.SelectedValue);



            List<ListItem> selectedIncharge = new List<ListItem>();
            selectedIncharge = chklstBasicJurisdictionIncharge.Items.Cast<ListItem>().Where(n => n.Selected).ToList();



            if (BtnSave.Text.Equals("Update"))
            {
                string id = Request.QueryString["id"].ToString();
                if (ddlBannerType.SelectedValue == "1")
                {
                    string delJurisdictionBanner = " Delete FROM [dbo].[JurisdictionBanner]  where BannerType = 'HomePage' AND BannerId=" + Convert.ToInt32(id);
                    dbc.ExecuteQuery(delJurisdictionBanner);

                    if (fileName != "")
                    {
                        string[] para1 = { txtTitle1.Text, txtAltText.Text, txtbasicLink.Text, FROM1, TO1, IsActive.ToString(), fileName, dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss"), sCategoryId, sProductId, sActionId.ToString(), id1 };
                        string query = "UPDATE [HomepageBanner] SET [Title]=@1,[AltText]=@2,[Link]=@3,[StartDate]=@4," +
                                       " [EndDate]=@5,[IsActive]=@6,[ImageName]=@7,[DOM]=@8,[CategoryId]=@9,[ProductId]=@10, [ActionId]=@11 where [Id]=@12";
                        int v1 = dbc.ExecuteQueryWithParams(query, para1);
                        if (v1 > 0)
                        {

                            foreach (ListItem item in selectedIncharge)
                            {
                                string sJurisdictionId = item.Value.ToString();

                                string Jurisdictionquery = "INSERT INTO [dbo].[JurisdictionBanner] ([JurisdictionId] ,[BannerId],[CreatedOn],[CreatedBy],[BannerType])";
                                Jurisdictionquery += " VALUES ('" + sJurisdictionId + "','" + id + "','" + dbc.getindiantimeString() + "'," + userId + ",'HomePage')";
                                dbc.ExecuteQuery(Jurisdictionquery);
                            }

                            sweetMessage("", "Banner Updated Successfully", "success");
                            Response.Redirect("HomePageBannerList.aspx", true);
                        }
                    }
                    else if (fileName == "")
                    {
                        string[] para1 = { txtTitle1.Text, txtAltText.Text, txtbasicLink.Text, FROM1, TO1, IsActive.ToString(), dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss"), sCategoryId, sProductId, sActionId.ToString(), id1 };

                        string query = "UPDATE [HomepageBanner] SET [Title]=@1,[AltText]=@2,[Link]=@3,[StartDate]=@4," +
                                       " [EndDate]=@5,[IsActive]=@6,[DOM]=@7,[CategoryId]=@8,[ProductId]=@9, [ActionId]=@10 where [Id]=@11";
                        int v1 = dbc.ExecuteQueryWithParams(query, para1);
                        if (v1 > 0)
                        {
                            foreach (ListItem item in selectedIncharge)
                            {
                                string sJurisdictionId = item.Value.ToString();

                                string Jurisdictionquery = "INSERT INTO [dbo].[JurisdictionBanner] ([JurisdictionId] ,[BannerId],[CreatedOn],[CreatedBy],[BannerType])";
                                Jurisdictionquery += " VALUES ('" + sJurisdictionId + "','" + id + "','" + dbc.getindiantimeString() + "'," + userId + ",'HomePage')";
                                dbc.ExecuteQuery(Jurisdictionquery);
                            }

                            sweetMessage("", "Banner Updated Successfully", "success");
                            Response.Redirect("HomePageBannerList.aspx", true);
                        }
                        else
                        {
                            sweetMessage("", "Please Try Again!!", "warning");
                        }
                    }
                }
                else
                {
                    string delJurisdictionBanner = " Delete FROM [dbo].[JurisdictionBanner]  where BannerType='Intermediate' AND BannerId=" + Convert.ToInt32(id);
                    dbc.ExecuteQuery(delJurisdictionBanner);


                    if (fileName != "")
                    {
                        string[] para1 = { txtTitle1.Text, txtAltText.Text, txtbasicLink.Text, FROM1, TO1, IsActive.ToString(), fileName, dbc.getindiantimeString(), userId, sCategoryId, sActionId.ToString(), sProductId, id1 };
                        string query = "UPDATE [IntermediateBanners] SET [Title]=@1,[AltText]=@2,[Link]=@3,[StartDate]=@4,[EndDate]=@5,[IsActive]=@6,[ImageName]=@7,[ModifiedOn]=@8,[ModifiedBy]=@9," +
                                        " [CategoryID]=@10,[ActionId]=@11,[ProductId]=@12" +
                                       " where [Id]=@13";
                        int v1 = dbc.ExecuteQueryWithParams(query, para1);
                        if (v1 > 0)
                        {
                            foreach (ListItem item in selectedIncharge)
                            {
                                string sJurisdictionId = item.Value.ToString();

                                string Jurisdictionquery = "INSERT INTO [dbo].[JurisdictionBanner] ([JurisdictionId] ,[BannerId],[CreatedOn],[CreatedBy],[BannerType])";
                                Jurisdictionquery += " VALUES ('" + sJurisdictionId + "','" + id1 + "','" + dbc.getindiantimeString() + "'," + userId + ",'Intermediate')";
                                dbc.ExecuteQuery(Jurisdictionquery);
                            }
                            sweetMessage("", "Intermediate Updated Successfully", "success");
                            Response.Redirect("HomePageBannerList.aspx", true);
                        }
                    }
                    else if (fileName == "")
                    {
                        string[] para1 = { txtTitle1.Text, txtAltText.Text, txtbasicLink.Text, FROM1, TO1, IsActive.ToString(), dbc.getindiantimeString(), userId, sCategoryId, sActionId.ToString(), sProductId, id1 };

                        string query = "UPDATE [IntermediateBanners] SET [Title]=@1,[AltText]=@2,[Link]=@3,[StartDate]=@4,[EndDate]=@5,[IsActive]=@6,[ModifiedOn]=@7,[ModifiedBy]=@8, " +
                                        " [CategoryID]=@9,[ActionId]=@10,[ProductId]=@11" +
                                        " where[Id] =@12";
                        int v1 = dbc.ExecuteQueryWithParams(query, para1);
                        if (v1 > 0)
                        {
                            foreach (ListItem item in selectedIncharge)
                            {
                                string sJurisdictionId = item.Value.ToString();

                                string Jurisdictionquery = "INSERT INTO [dbo].[JurisdictionBanner] ([JurisdictionId] ,[BannerId],[CreatedOn],[CreatedBy],[BannerType])";
                                Jurisdictionquery += " VALUES ('" + sJurisdictionId + "','" + id1 + "','" + dbc.getindiantimeString() + "'," + userId + ",'Intermediate')";
                                dbc.ExecuteQuery(Jurisdictionquery);
                            }
                            sweetMessage("", "Intermediate Banner Updated Successfully", "success");
                            Response.Redirect("HomePageBannerList.aspx", true);
                        }
                        else
                        {
                            sweetMessage("", "Please Try Again!!", "warning");
                        }
                    }
                }

            }
            else
            {
                if (ddlBannerType.SelectedValue == "1")
                {
                    string[] para1 = { txtTitle1.Text.ToString().Replace("'", "''"),
            txtAltText.Text.ToString().Replace("'", "''"),
            txtbasicLink.Text.ToString().Replace("'", "''"),
            FROM1,
            TO1,
            IsActive.ToString(),
            fileName,
            "0",
            dbc.getindiantimeString(),
            dbc.getindiantimeString(),
            sActionId.ToString(),
            sCategoryId,
            sProductId,
            };
                    //string query = "INSERT INTO [dbo].[HomepageBanner] ([Title] ,[AltText],[Link],[StartDate],[EndDate],[IsActive],[ImageName],[IsDeleted],[DOC],[DOM],[ActionId],[CategoryId],[ProductId]) VALUES ('" + txtTitle1.Text + "','" + txtAltText.Text + "','" + txtbasicLink.Text + "','" + FROM1 + "','" + TO1 + "','" + IsActive + "','" + fileName + "',0,'" + dbc.getindiantimeString() + "','" + dbc.getindiantimeString() + "',"+sActionId+","+sCategoryId+","+sProductId+")";
                    string query = "INSERT INTO [dbo].[HomepageBanner] ([Title] ,[AltText],[Link],[StartDate],[EndDate]," +
                                   " [IsActive],[ImageName],[IsDeleted],[DOC],[DOM],[ActionId],[CategoryId],[ProductId])" +
                                   " VALUES (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13); SELECT SCOPE_IDENTITY();";
                    //int VAL = dbc.ExecuteQuery(query);
                    int VAL = dbc.ExecuteQueryWithParamsId(query, para1);
                    if (VAL > 0)
                    {
                        foreach (ListItem item in selectedIncharge)
                        {
                            string sJurisdictionId = item.Value.ToString();

                            string Jurisdictionquery = "INSERT INTO [dbo].[JurisdictionBanner] ([JurisdictionId] ,[BannerId],[CreatedOn],[CreatedBy],[BannerType])";
                            Jurisdictionquery += " VALUES ('" + sJurisdictionId + "','" + VAL + "','" + dbc.getindiantimeString() + "'," + userId + ",'HomePage')";
                            dbc.ExecuteQuery(Jurisdictionquery);
                        }
                        sweetMessage("", "Banner Uploaded Successfully", "success");
                        Response.Redirect("HomePageBannerList.aspx", true);
                    }
                    else
                    {
                        sweetMessage("", "Please Try Again!!", "warning");
                    }
                }
                else
                {
                    string[] para1 = { txtTitle1.Text.ToString().Replace("'", "''"),
            txtAltText.Text.ToString().Replace("'", "''"),
            txtbasicLink.Text.ToString().Replace("'", "''"),
            FROM1,
            TO1,
            IsActive.ToString(),
            fileName,
            "0",
            ddlBannerType.SelectedValue,
            dbc.getindiantimeString(),
            userId,
            ddlbasicAction.SelectedItem.ToString(),
            sCategoryId,
            sActionId.ToString(),
            sProductId,
            };

                string query = "INSERT INTO [dbo].[IntermediateBanners] ([Title] ,[AltText],[Link],[StartDate],[EndDate],[IsActive],[ImageName],";
                query += "[IsDeleted],[TypeId],[CreatedOn],[CreatedBy],[Action],[CategoryID],[ActionId],[ProductId]) VALUES (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13,@14,@15); SELECT SCOPE_IDENTITY();";
                //query +=  ddlintermediateType.SelectedValue.ToString() + "','" + dtCreatedon.ToString() + "'," + userId + ",'" + ddlintermedicateAction.SelectedItem.ToString() + "'," + sCategoryId + "," + intermediateActionId + "," + sProductId + "); SELECT SCOPE_IDENTITY();";
                //int VAL = dbc.ExecuteQuery(query);
                int VAL = dbc.ExecuteQueryWithParamsId(query, para1);
                //query += "','" + FROM1 + "','" + TO1 + "','" + IsActive + "','" + fileName + "',0,'";


                if (VAL > 0)
                {
                    foreach (ListItem item in selectedIncharge)
                    {
                        string sJurisdictionId = item.Value.ToString();

                        string Jurisdictionquery = "INSERT INTO [dbo].[JurisdictionBanner] ([JurisdictionId] ,[BannerId],[CreatedOn],[CreatedBy],[BannerType])";
                        Jurisdictionquery += " VALUES ('" + sJurisdictionId + "','" + VAL + "','" + dbc.getindiantimeString() + "'," + userId + ",'Intermediate')";
                        dbc.ExecuteQuery(Jurisdictionquery);
                    }
                    sweetMessage("", "Intermediate Banner Added Successfully", "success");
                    Response.Redirect("HomePageBannerList.aspx", true);
                }
                else
                {
                    sweetMessage("", "Please Try Again!!", "warning");
                }
                }
            }
        }
        catch (Exception E)
        {
            sweetMessage("", "Please Try Again!!", "warning");
        }
    }
    protected void FileUpload1_DataBinding(object sender, EventArgs e)
    {
        try
        {
            if (FileUpload1.HasFile)
            {
                productimg.ImageUrl = FileUpload1.FileName;
                FileName = FileUpload1.FileName;
            }
        }
        catch (Exception ex)
        {

        }

    }
    protected void FileUpload1_Load(object sender, EventArgs e)
    {
        try
        {
            if (FileUpload1.HasFile)
            {
                productimg.ImageUrl = FileUpload1.FileName;
                FileName = FileUpload1.FileName;
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void FileUploadControl_Init(object sender, EventArgs e)
    {
        try
        {
            if (FileUpload1.HasFile)
            {
                productimg.ImageUrl = FileUpload1.FileName;
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

                DataTable dt = dbc.GetDataTable("select ImageName from HomepageBanner where Id=" + Request.QueryString["ID"].ToString() + "");


                if (dt != null && dt.Rows.Count > 0)
                {

                    string path = dt.Rows[0]["ImageName"].ToString();


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
    protected void Button1_Click(object sender, EventArgs e)
    {
        try
        {
            filedel();
        }
        catch (Exception aa)
        {

        }
    }
    protected void Button2_Click(object sender, EventArgs e)
    {

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

    protected void OnSelectedBasicActionChanged(object sender, EventArgs e)
    {
        ddlbasicCategory.Visible = false;
        lblbasicCategory.Visible = false;

        txtbasicLink.Visible = false;
        lblbasicLink.Visible = false;

        ddlbasicProduct.Visible = false;
        lblbasicProduct.Visible = false;

        dvbasicLink.Style.Add("display", "none");
        dvbasicCategory.Style.Add("display", "none");
        dvbasicProduct.Style.Add("display", "none");

        if (ddlbasicAction.SelectedIndex == clsCommon.BannerActionType.NavigateToCategory.GetHashCode())
        {
            ddlbasicCategory.Visible = true;
            lblbasicCategory.Visible = true;
            dvbasicCategory.Style.Add("display", "block");
        }

        if (ddlbasicAction.SelectedIndex == clsCommon.BannerActionType.OpenUrl.GetHashCode())
        {
            txtbasicLink.Visible = true;
            lblbasicLink.Visible = true;
            dvbasicLink.Style.Add("display", "block");
        }

        if (ddlbasicAction.SelectedIndex == clsCommon.BannerActionType.AddToCart.GetHashCode())
        {
            ddlbasicProduct.Visible = true;
            lblbasicProduct.Visible = true;
            dvbasicProduct.Style.Add("display", "block");
        }
    }
}