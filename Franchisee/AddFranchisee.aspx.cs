﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Services;
using System.Web.UI.WebControls;
using WebApplication1;
using ZXing;

public partial class Franchisee_AddFranchisee : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    private readonly string _QRCodeStaticURL = ConfigurationManager.AppSettings["QRCodeStaticURL"].ToString();
    private readonly string _QRCODEURL = ConfigurationManager.AppSettings["QRCodeURL"].ToString();
    private readonly string _ShortURL = ConfigurationManager.AppSettings["ShortURL"].ToString();
    string id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                string superFranchiseeqry = "Select SuperFranchiseeID as Id,SuperFranchiseeName as Name from SuperFranchisee where IsActive = 1 order by SuperFranchiseeID";
                DataTable dtSFranchisee = dbc.GetDataTable(superFranchiseeqry);
                ddlSuperFranchisee.DataSource = dtSFranchisee;
                ddlSuperFranchisee.DataTextField = "Name";
                ddlSuperFranchisee.DataValueField = "Id";
                ddlSuperFranchisee.DataBind();
                ddlSuperFranchisee.Items.Insert(0, new ListItem("Select Super Franchisee", ""));

                string masterFranchiseeqry = "Select MasterFranchiseeID as Id,MasterFranchiseeName as Name from MasterFranchisee where IsActive = 1 order by MasterFranchiseeID";
                DataTable dtMFranchisee = dbc.GetDataTable(masterFranchiseeqry);
                ddlMasterFranchisee.DataSource = dtMFranchisee;
                ddlMasterFranchisee.DataTextField = "Name";
                ddlMasterFranchisee.DataValueField = "Id";
                ddlMasterFranchisee.DataBind();
                ddlMasterFranchisee.Items.Insert(0, new ListItem("Select Master Franchisee", ""));

                txtQRCodeURL.Visible = false;
                lblQRCodeURL.Visible = false;

                txtShortUrl.Enabled = true;
                txtCustomerCode.Enabled = true;
                btnGenerate.Enabled = true;
                QRCodeImage.Visible = false;
                lblQRCodeImages.Visible = false;
                string IsAdmin = Request.Cookies["TUser"]["IsAdmin"].ToString();
                string sJurisdictionId = Request.Cookies["TUser"]["JurisdictionID"].ToString();

                id = Request.QueryString["Id"];
                if (id != null && !id.Equals(""))
                {
                    BtnSave.Text = "Update";
                    DataTable dt1 = new DataTable();
                    dt1 = dbc.GetDataTable(" SELECT F.FranchiseeID, F.SuperFranchiseeID, F.MasterFranchiseeID, F.FranchiseeName, F.FranchiseeContactNumber, F.FranchiseeEmailAddress, F.FranchiseeCustomerCode, F.ShortUrl,F.QRCodeURL, F.IsActive,F.QrCodeImage,U.UserName,U.Password FROM Franchisee F INNER JOIN Users U on U.FranchiseeId = F.FranchiseeId where F.IsDeleted = 0 and F.FranchiseeID=" + id);

                    if (dt1.Rows.Count > 0)
                    {
                        ddlSuperFranchisee.SelectedValue = Convert.ToInt32(dt1.Rows[0]["SuperFranchiseeID"]).ToString();
                        ddlMasterFranchisee.SelectedValue = Convert.ToInt32(dt1.Rows[0]["MasterFranchiseeID"]).ToString();
                        txtFranchiseeName.Text = dt1.Rows[0]["FranchiseeName"].ToString();
                        txtContactNumber.Text = dt1.Rows[0]["FranchiseeContactNumber"].ToString();
                        txtEmailAddress.Text = dt1.Rows[0]["FranchiseeEmailAddress"].ToString();
                        txtCustomerCode.Text = dt1.Rows[0]["FranchiseeCustomerCode"].ToString();
                        txtShortUrl.Text = dt1.Rows[0]["ShortUrl"].ToString();
                        txtQRCodeURL.Text = dt1.Rows[0]["QRCodeURL"].ToString();

                        if (dt1.Rows[0]["IsActive"].ToString() == "True")
                            chkisactive.Checked = true;
                        else
                            chkisactive.Checked = false;

                        txtQRCodeURL.Visible = true;
                        lblQRCodeURL.Visible = true;
                        txtQRCodeURL.Enabled = false;
                        lblQRCodeURL.Enabled = false;
                        txtShortUrl.Enabled = false;
                        txtCustomerCode.Enabled = false;
                        btnGenerate.Enabled = false;
                        QRCodeImage.Visible = true;
                        lblQRCodeImages.Visible = true;
                        QRCodeImage.ImageUrl = dt1.Rows[0]["QrCodeImage"].ToString();

                        txtUserName.Text = dt1.Rows[0]["UserName"].ToString();
                        txtPassword.Text = "Password";
                        txtPassword.Attributes["value"] = dt1.Rows[0]["Password"].ToString();
                        txtConfirmPassword.Text = "Password";
                        txtConfirmPassword.Attributes["value"] = dt1.Rows[0]["Password"].ToString();
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

            if (chkisactive.Checked)
            {
                IsActive = 1;
            }

            string sSuperFranchiseeId = "";
            if (string.IsNullOrEmpty(ddlSuperFranchisee.SelectedValue))
                sSuperFranchiseeId = "0";
            else
                sSuperFranchiseeId = ddlSuperFranchisee.SelectedValue.ToString();

            string sMasterFranchiseeId = "";
            if (string.IsNullOrEmpty(ddlMasterFranchisee.SelectedValue))
                sMasterFranchiseeId = "0";
            else
                sMasterFranchiseeId = ddlMasterFranchisee.SelectedValue.ToString();

            string qrCodeURL = _QRCODEURL.Replace("#QRCODE#", txtCustomerCode.Text);
            qrCodeURL = qrCodeURL.Replace("#FNAME#", "&utm_campaign=" + txtFranchiseeName.Text);
            qrCodeURL = _QRCodeStaticURL + "&referrer=utm_source=" + HttpUtility.UrlEncode(qrCodeURL);

            

            if (BtnSave.Text.Equals("Update"))
            {
                string[] para1 = { txtFranchiseeName.Text, txtContactNumber.Text, txtEmailAddress.Text, txtCustomerCode.Text, txtShortUrl.Text, IsActive.ToString(), sSuperFranchiseeId, sMasterFranchiseeId, dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss"), qrCodeURL, id1 };
                string query = "UPDATE [Franchisee] SET [FranchiseeName]=@1,[FranchiseeContactNumber]=@2,[FranchiseeEmailAddress]=@3,[FranchiseeCustomerCode]=@4," +
                               " [ShortUrl]=@5,[IsActive]=@6,[SuperFranchiseeID]=@7,[MasterFranchiseeID]=@8,[CreatedOn]=@9,[QRCodeUrl]=@10 where [FranchiseeID]=@11";
                int v1 = dbc.ExecuteQueryWithParams(query, para1);
                if (v1 > 0)
                {
                    string updateUser = " UPDATE  [Users] SET [UserName]='" + txtUserName.Text.ToString() + "',[Password]='" + txtPassword.Text.ToString() + "'," +
                                        " [Name]='" + txtUserName.Text.ToString() + "',[UserType]=4,[FranchiseeId]=" + id1 + ",[Mobile] = '" + txtContactNumber.Text.ToString() + "'" +
                                        " WHERE FranchiseeId = " + id1;
                    int userVAL = dbc.ExecuteQuery(updateUser);

                    sweetMessage("", "Franchisee Updated Successfully", "success");
                    Response.Redirect("FranchiseeList.aspx", true);
                }
            }
            else
            {
                byte[] QRCodebyteImage = GenerateQRCode(qrCodeURL);
                string newfileName = txtFranchiseeName.Text + "_" + txtCustomerCode.Text;
                newfileName = newfileName.Replace(" ", String.Empty);
                string filepath = "~/Content/images/QRCodeImages/" + newfileName + ".png";
                string tempFile = HttpContext.Current.Server.MapPath(filepath);
                using (var ms = new MemoryStream(QRCodebyteImage))
                {
                    using (FileStream fs = File.Create(tempFile, 1024))
                    {
                        ms.WriteTo(fs);
                    }
                    ms.Close();
                    ms.Flush();
                }

                string[] para1 = { txtFranchiseeName.Text.ToString().Replace("'", "''"),
            txtContactNumber.Text.ToString().Replace("'", "''"),
            txtEmailAddress.Text.ToString().Replace("'", "''"),
            txtCustomerCode.Text.ToString().Replace("'", "''"),
            txtShortUrl.Text.ToString(),
            IsActive.ToString(),
            sSuperFranchiseeId,
            sMasterFranchiseeId,
            dbc.getindiantimeString(),
            "0",
            qrCodeURL,
            filepath
            };
                //string query = "INSERT INTO [dbo].[HomepageBanner] ([Title] ,[AltText],[Link],[StartDate],[EndDate],[IsActive],[ImageName],[IsDeleted],[DOC],[DOM],[ActionId],[CategoryId],[ProductId]) VALUES ('" + txtTitle1.Text + "','" + txtAltText.Text + "','" + txtbasicLink.Text + "','" + FROM1 + "','" + TO1 + "','" + IsActive + "','" + fileName + "',0,'" + dbc.getindiantimeString() + "','" + dbc.getindiantimeString() + "',"+sActionId+","+sCategoryId+","+sProductId+")";
                string query = "INSERT INTO [dbo].[Franchisee] ([FranchiseeName] ,[FranchiseeContactNumber],[FranchiseeEmailAddress],[FranchiseeCustomerCode]," +
                               " [ShortUrl],[IsActive],[SuperFranchiseeID],[MasterFranchiseeID],[CreatedOn],[IsDeleted],[QRCodeUrl],[QrCodeImage])" +
                               " VALUES (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12); SELECT SCOPE_IDENTITY();";
                //int VAL = dbc.ExecuteQuery(query);
                int VAL = dbc.ExecuteQueryWithParamsId(query, para1);
                if (VAL > 0)
                {
                    hdnFranchiseeID.Value = VAL.ToString();
                    hdnContact.Value = txtContactNumber.Text.ToString().Replace("'", "''");

                    string InsertUserqry = "INSERT INTO [Users] ([UserName],[Password],[Name],[UserType],[FranchiseeId],[Mobile]) VALUES ('" + txtUserName.Text.ToString().Replace("'", "''") + "','" + txtPassword.Text.ToString().Replace("'", "''") + "','" + txtUserName.Text.ToString().Replace("'", "''") + "',4," + VAL.ToString() + ",'" + txtContactNumber.Text.ToString() + "')";
                    int userVAL = dbc.ExecuteQuery(InsertUserqry);

                    sweetMessage("", "Franchisee Uploaded Successfully", "success");
                    Response.Redirect("FranchiseeList.aspx", true);
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
            sb.Append("window.location.href = 'FranchiseeList.aspx'");
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

    protected void BtnGenerate_Click(object sender, EventArgs e)
    {
        var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var stringChars = new char[6];
        var random = new Random();

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }
        var finalString = new String(stringChars);
        txtCustomerCode.Text = finalString;
        txtShortUrl.Text = _ShortURL + txtCustomerCode.Text;
    }


    [WebMethod]
    public static bool CheckCustomerCode(string codeText)
    {
        dbConnection dbc = new dbConnection();
        bool flag = true;
        DataTable dtFdata = dbc.GetDataTable("select * FROM Franchisee WHERE ISActive = 1 AND [FranchiseeCustomerCode] = '" + codeText + "'");
        if(dtFdata.Rows.Count > 0)
        {
            flag = false;
            return flag;
        }
        DataTable dtMFdata = dbc.GetDataTable("select * FROM MasterFranchisee WHERE ISActive = 1 AND [MasterFranchiseeCustomerCode] = '" + codeText + "'");
        if (dtMFdata.Rows.Count > 0)
        {
            flag = false;
            return flag;
        }
        DataTable dtSFdata = dbc.GetDataTable("select * FROM SuperFranchisee WHERE ISActive = 1 AND [SuperFranchiseeCustomerCode] = '" + codeText + "'");
        if (dtSFdata.Rows.Count > 0)
        {
            flag = false;
            return flag;
        }
        return flag;
    }

    private byte[] GenerateQRCode(string qrcodeText)
    {
        SqlConnection con = new SqlConnection(dbc.consString);
        con.Open();
        var sql = string.Empty;
        string folderPath = HttpContext.Current.Server.MapPath("~/content/images/");
        string imagePath = HttpContext.Current.Server.MapPath("~/content/images/QrCode.png");
        var barcodeWriter = new BarcodeWriter();
        barcodeWriter.Format = BarcodeFormat.QR_CODE;
        var result = barcodeWriter.Write(qrcodeText);
        int width = 200, height = 200;
        string barcodePath = imagePath;
        var barcodeBitmap = new Bitmap(result, height, width);
        byte[] bytes = new byte[1024];

        try
        {
            using (MemoryStream memory = new MemoryStream())
            {
                using (FileStream fs = File.Create(barcodePath, 1024))
                {
                    barcodeBitmap.Save(memory, ImageFormat.Png);
                    bytes = memory.ToArray();
                    fs.Write(bytes, 0, bytes.Length);
                }
                memory.Flush();
                memory.Close();
            }
        }
        catch (Exception ex)
        {

        }
        con.Close();
        return bytes;
    }
}