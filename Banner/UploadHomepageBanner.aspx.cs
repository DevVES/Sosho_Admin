using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Banner_UploadHomepageBanner : System.Web.UI.Page
{
    public class ActionType
    {
        // Auto-Initialized properties  
        public string Text { get; set; }
        public int Value { get; set; }
    }
    dbConnection dbc = new dbConnection();
    string FileName = "",id="", intermediateFileName="";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                string bannerqry = "Select Id as Id,BannerName as Name from BannerTypeMaster where IsActive = 1 order by Id";
                DataTable dtbanner = dbc.GetDataTable(bannerqry);
                ddlintermediateType.DataSource = dtbanner;
                ddlintermediateType.DataTextField = "Name";
                ddlintermediateType.DataValueField = "Id";
                ddlintermediateType.DataBind();
                ddlintermediateType.Items.Insert(0, new ListItem("Select Banner Type", ""));

                string categoryqry = "SELECT CategoryId,CategoryName FROM Category where isnull(IsDeleted,0)=0 order by CategoryId asc";
                DataTable dtcategory = dbc.GetDataTable(categoryqry);
                ddlintermedicateCategory.DataSource = dtcategory;
                ddlintermedicateCategory.DataTextField = "CategoryName";
                ddlintermedicateCategory.DataValueField = "CategoryId";
                ddlintermedicateCategory.DataBind();

                string productqry = "SELECT Id,Name FROM Product where isnull(IsDeleted,0)=0 order by Id asc";
                DataTable dtproduct = dbc.GetDataTable(productqry);
                ddlintermedicateProduct.DataSource = dtproduct;
                ddlintermedicateProduct.DataTextField = "Name";
                ddlintermedicateProduct.DataValueField = "Id";
                ddlintermedicateProduct.DataBind();

                List<ActionType> ActionList = new List<ActionType>
                {
                                    new ActionType { Text = "Select Action Name", Value = 0},
                                    new ActionType { Text = clsCommon.BannerActionType.OpenUrl.ToString(), Value = clsCommon.BannerActionType.OpenUrl.GetHashCode()},
                    new ActionType { Text = clsCommon.BannerActionType.NavigateToCategory.ToString(), Value = clsCommon.BannerActionType.NavigateToCategory.GetHashCode()},
                    new ActionType { Text = clsCommon.BannerActionType.AddToCart.ToString(), Value = clsCommon.BannerActionType.AddToCart.GetHashCode()},
                    new ActionType { Text = clsCommon.BannerActionType.None.ToString(), Value = clsCommon.BannerActionType.None.GetHashCode()},
                };
                ddlintermedicateAction.DataTextField = "Text";
                ddlintermedicateAction.DataValueField = "Value";
                ddlintermedicateAction.DataSource = ActionList;
                ddlintermedicateAction.DataBind();

                string IsAdmin = Request.Cookies["TUser"]["IsAdmin"].ToString();
                string sJurisdictionId = Request.Cookies["TUser"]["JurisdictionID"].ToString();
                if (IsAdmin == "True")
                {
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
                    divJurisdictionIncharge.Visible = false;
                }

                txtdt.Text = dbc.getindiantime().ToString("dd/MMM/yyyy");
                txtdt1.Text = dbc.getindiantime().ToString("dd/MMM/yyyy");
                txtintermediateStartDate.Text = dbc.getindiantime().ToString("dd/MMM/yyyy");
                txtintermediateEndDate.Text = dbc.getindiantime().ToString("dd/MMM/yyyy");
                chkisactive.Checked = true;
                ChkintermediateisActive.Checked = true;
                id = Request.QueryString["Id"];
                if (id != null && !id.Equals(""))
                {
                    BtnSave.Text = "Update";

                    DataTable dt1 = dbc.GetDataTable("SELECT  [Title],[AltText],[Link],[StartDate],[EndDate],[IsActive],[ImageName] FROM [dbo].[HomepageBanner] where IsDeleted=0  and Id=" + id);
                    //DataTable intermediatedt = dbc.GetDataTable("SELECT  [Title],[AltText],[Link],[StartDate],[EndDate],[IsActive],[ImageName] FROM [dbo].[IntermediateBanners] where IsDeleted=0  and Id=" + id);
                    if (dt1.Rows.Count > 0)
                    {
                        txtTitle1.Text = dt1.Rows[0]["Title"].ToString();
                        txtAltText.Text = dt1.Rows[0]["AltText"].ToString();
                        txtLink.Text = dt1.Rows[0]["Link"].ToString();

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
                            if (dt.Length ==3)
                            {
                                txtdt.Text = dt[0].ToString();
                                txttime.Text = dt[1].ToString() + " "+dt[2].ToString();
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
                    }
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

            string FROM1 = startdate + " " +starttime;
            string TO1 = enddate +" " + endtime ;

            string[] validFileTypes = { "png", "jpg", "jpeg" };
            Stream fs = FileUpload1.PostedFile.InputStream;
            BinaryReader br = new BinaryReader(fs);
            Byte[] image = br.ReadBytes((Int32)fs.Length);

            string exten = System.IO.Path.GetExtension(FileUpload1.FileName);
            string name = System.IO.Path.GetFileName(FileUpload1.FileName);

            string ext = System.IO.Path.GetExtension(FileUpload1.FileName);
            
            bool isValidFile = false;
            string imgname = "", imgnamenew="";
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

            if (BtnSave.Text.Equals("Update"))
            {
                string id = Request.QueryString["id"].ToString();

                if (fileName != "")
                {
                    string[] para1 = { txtTitle1.Text, txtAltText.Text, txtLink.Text ,FROM1 , TO1,IsActive.ToString(),fileName, dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss"),id1};
                    string query = "UPDATE [HomepageBanner] SET [Title]=@1,[AltText]=@2,[Link]=@3,[StartDate]=@4,[EndDate]=@5,[IsActive]=@6,[ImageName]=@7,[DOM]=@8 where [Id]=@9";
                    int v1 = dbc.ExecuteQueryWithParams(query, para1);
                    if (v1 > 0)
                    {
                        sweetMessage("", "Banner Updated Successfully", "success");
                        Response.Redirect("HomePageBannerList.aspx", true);
                    }
                }
                else if (fileName == "")
                {
                    string[] para1 = { txtTitle1.Text, txtAltText.Text, txtLink.Text, FROM1, TO1, IsActive.ToString(), dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss"), id1 };

                    string query = "UPDATE [HomepageBanner] SET [Title]=@1,[AltText]=@2,[Link]=@3,[StartDate]=@4,[EndDate]=@5,[IsActive]=@6,[DOM]=@7 where [Id]=@8";
                    int v1 = dbc.ExecuteQueryWithParams(query, para1);
                    if (v1 > 0)
                    {
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
                string query = "INSERT INTO [dbo].[HomepageBanner] ([Title] ,[AltText],[Link],[StartDate],[EndDate],[IsActive],[ImageName],[IsDeleted],[DOC],[DOM]) VALUES ('" + txtTitle1.Text + "','" + txtAltText.Text + "','" + txtLink.Text + "','" + FROM1 + "','" + TO1 + "','" + IsActive + "','" + fileName + "',0,'" + dbc.getindiantimeString() + "','" + dbc.getindiantimeString() + "')";
                int VAL = dbc.ExecuteQuery(query);

                if (VAL > 0)
                {
                    sweetMessage("", "Banner Uploaded Successfully", "success");
                    Response.Redirect("HomePageBannerList.aspx", true);
                }
                else
                {
                    sweetMessage("", "Please Try Again!!", "warning");
                }
            }
        }
        catch (Exception E) {
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
    protected void FileUpload2_DataBinding(object sender, EventArgs e)
    {
        try
        {
            if (FileUpload2.HasFile)
            {
                intermediateimage.ImageUrl = FileUpload2.FileName;
                intermediateFileName = FileUpload2.FileName;
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
                intermediateimage.ImageUrl = FileUpload2.FileName;
                intermediateFileName = FileUpload2.FileName;
            }
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
                intermediateimage.ImageUrl = FileUpload2.FileName;
            }
        }
        catch (Exception ex)
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
    protected void BtnintermediateRemoveImage_Click(object sender, EventArgs e)
    {
        try
        {
            intermediatefiledel();
        }
        catch (Exception aa)
        {

        }
    }
    public void intermediatefiledel()
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

    protected void BtnintermediateSave_Click(object sender, EventArgs e)
    {
        try
        {
            string userId = Request.Cookies["TUser"]["Id"].ToString();
            int IsActive = 0;
            string id1 = Request.QueryString["Id"];
            string startdate = txtintermediateStartDate.Text.ToString();
            string starttime = txtintermediateStartDatetimepicker.Text.ToString();
            string enddate = txtintermediateEndDate.Text.ToString();
            string endtime = txtintermediateEndDatetimepicker.Text.ToString();

            string FROM1 = startdate + " " + starttime;
            string TO1 = enddate + " " + endtime;

            string[] validFileTypes = { "png", "jpg", "jpeg" };
            Stream fs = FileUpload2.PostedFile.InputStream;
            BinaryReader br = new BinaryReader(fs);
            Byte[] image = br.ReadBytes((Int32)fs.Length);

            string exten = System.IO.Path.GetExtension(FileUpload2.FileName);
            string name = System.IO.Path.GetFileName(FileUpload2.FileName);

            string ext = System.IO.Path.GetExtension(FileUpload2.FileName);

            bool isValidFile = false;
            string imgname = "", imgnamenew = "";
            DateTime dtCreatedon = DateTime.Now;
            if (FileUpload2.HasFile)
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
                imgnamenew = FileUpload2.FileName;
                string path = Server.MapPath("/IntermediateBannerImage");
                FileUpload2.SaveAs(path + "/" + imgnamenew);
            }
            
            string fileName = "";
            fileName = imgnamenew;


            if (ChkintermediateisActive.Checked)
            {
                IsActive = 1;
            }

            if (BtnintermediateSave.Text.Equals("Update"))
            {
                string id = Request.QueryString["id"].ToString();

                if (fileName != "")
                {
                    string[] para1 = { txtintermediateTitle.Text, txtintermediateAltText.Text, txtintermediateLink.Text, FROM1, TO1, IsActive.ToString(), fileName, dtCreatedon.ToString(),userId, id1 };
                    string query = "UPDATE [IntermediateBanners] SET [Title]=@1,[AltText]=@2,[Link]=@3,[StartDate]=@4,[EndDate]=@5,[IsActive]=@6,[ImageName]=@7,[ModifiedOn]=@8,[ModifiedBy]=@9 where [Id]=@10";
                    int v1 = dbc.ExecuteQueryWithParams(query, para1);
                    if (v1 > 0)
                    {
                        sweetMessage("", "Intermediate Updated Successfully", "success");
                        Response.Redirect("HomePageBannerList.aspx", true);
                    }
                }
                else if (fileName == "")
                {
                    string[] para1 = { txtintermediateTitle.Text, txtintermediateAltText.Text, txtintermediateLink.Text, FROM1, TO1, IsActive.ToString(), dtCreatedon.ToString(),userId, id1 };

                    string query = "UPDATE [IntermediateBanners] SET [Title]=@1,[AltText]=@2,[Link]=@3,[StartDate]=@4,[EndDate]=@5,[IsActive]=@6,[ModifiedOn]=@7,[ModifiedBy]=@8 where [Id]=@9";
                    int v1 = dbc.ExecuteQueryWithParams(query, para1);
                    if (v1 > 0)
                    {
                        sweetMessage("", "Intermediate Banner Updated Successfully", "success");
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
                string sCategoryId = "";
                if (string.IsNullOrEmpty(ddlintermedicateCategory.SelectedValue))
                    sCategoryId = "0";
                else
                    sCategoryId = ddlintermedicateCategory.SelectedValue.ToString();

                string sProductId = "";
                if (string.IsNullOrEmpty(ddlintermedicateProduct.SelectedValue))
                    sProductId = "0";
                else
                    sProductId = ddlintermedicateProduct.SelectedValue.ToString();

                int intermediateActionId = 0;
                if (ddlintermedicateAction.SelectedIndex > 0)
                    intermediateActionId = ddlintermedicateAction.SelectedIndex;

                List<ListItem> selectedIncharge = new List<ListItem>();
                selectedIncharge = chklstJurisdictionIncharge.Items.Cast<ListItem>().Where(n => n.Selected).ToList();

                
                string[] para1 = { txtintermediateTitle.Text.ToString().Replace("'", "''"),
            txtintermediateAltText.Text.ToString().Replace("'", "''"),
            txtintermediateLink.Text.ToString().Replace("'", "''"),
            FROM1,
            TO1,
            IsActive.ToString(),
            fileName,
            "0",
            ddlintermediateType.SelectedValue,
            dtCreatedon.ToString(),
            userId,
            ddlintermedicateAction.SelectedItem.ToString(),
            sCategoryId,
            intermediateActionId.ToString(),
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

                        string Jurisdictionquery = "INSERT INTO [dbo].[JurisdictionBanner] ([JurisdictionId] ,[BannerId],[CreatedOn],[CreatedBy])";
                        Jurisdictionquery += " VALUES ('" + sJurisdictionId + "','" + VAL + "','" + dtCreatedon.ToString() + "'," + userId + ")";
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
        catch (Exception E)
        {
            sweetMessage("", "Please Try Again!!", "warning");
        }
    }

    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    {
            ddlintermedicateCategory.Visible = false;
            lblintermediateCategory.Visible = false;

            txtintermediateLink.Visible = false;
            lblintermediateLink.Visible = false;

            ddlintermedicateProduct.Visible = false;
            lblintermediateProduct.Visible = false;

            dvintermediateLink.Style.Add("display", "none");
            dvintermediateCategory.Style.Add("display", "none");
            dvintermediateProduct.Style.Add("display", "none");

            if (ddlintermedicateAction.SelectedIndex == clsCommon.BannerActionType.NavigateToCategory.GetHashCode())
            {
                ddlintermedicateCategory.Visible = true;
                lblintermediateCategory.Visible = true;
                dvintermediateCategory.Style.Add("display", "block");
            }

            if (ddlintermedicateAction.SelectedIndex == clsCommon.BannerActionType.OpenUrl.GetHashCode())
            {
                txtintermediateLink.Visible = true;
                lblintermediateLink.Visible = true;
                dvintermediateLink.Style.Add("display", "block");
            }

            if (ddlintermedicateAction.SelectedIndex == clsCommon.BannerActionType.AddToCart.GetHashCode())
            {
                ddlintermedicateProduct.Visible = true;
                lblintermediateProduct.Visible = true;
                dvintermediateProduct.Style.Add("display", "block");
            }
    }

}
