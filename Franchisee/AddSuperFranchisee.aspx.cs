﻿using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Web;
using System.Web.Services;
using WebApplication1;
using ZXing;

public partial class Franchisee_AddSuperFranchisee : System.Web.UI.Page
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
                string IsAdmin = Request.Cookies["TUser"]["IsAdmin"].ToString();
                string sJurisdictionId = Request.Cookies["TUser"]["JurisdictionID"].ToString();

                txtQRCodeURL.Visible = false;
                lblQRCodeURL.Visible = false;
                txtShortUrl.Enabled = true;
                txtCustomerCode.Enabled = true;
                btnGenerate.Enabled = true;
                QRCodeImage.Visible = false;
                lblQRCodeImages.Visible = false;
                id = Request.QueryString["Id"];
                if (id != null && !id.Equals(""))
                {
                    BtnSave.Text = "Update";
                    DataTable dt1 = new DataTable();
                    dt1 = dbc.GetDataTable(" SELECT F.SuperFranchiseeID, F.SuperFranchiseeID, F.SuperFranchiseeName, F.SuperFranchiseeContactNumber, F.SuperFranchiseeEmailAddress, F.SuperFranchiseeCustomerCode, F.ShortUrl,F.QRCodeURL, F.IsActive, F.QrCodeImage,U.UserName,U.Password FROM [SuperFranchisee] F INNER JOIN Users U on U.FranchiseeId = F.SuperFranchiseeID where F.IsDeleted = 0 and F.SuperFranchiseeID=" + id);

                    if (dt1.Rows.Count > 0)
                    {
                        txtSuperFranchiseeName.Text = dt1.Rows[0]["SuperFranchiseeName"].ToString();
                        txtContactNumber.Text = dt1.Rows[0]["SuperFranchiseeContactNumber"].ToString();
                        txtEmailAddress.Text = dt1.Rows[0]["SuperFranchiseeEmailAddress"].ToString();
                        txtCustomerCode.Text = dt1.Rows[0]["SuperFranchiseeCustomerCode"].ToString();
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

            string qrCodeURL = _QRCODEURL.Replace("#QRCODE#", txtCustomerCode.Text);
            qrCodeURL = qrCodeURL.Replace("#FNAME#", "&utm_campaign=" + txtSuperFranchiseeName.Text);
            qrCodeURL = _QRCodeStaticURL + "&referrer=utm_source=" + HttpUtility.UrlEncode(qrCodeURL);

            
            if (BtnSave.Text.Equals("Update"))
            {
                string[] para1 = { txtSuperFranchiseeName.Text, txtContactNumber.Text, txtEmailAddress.Text, txtCustomerCode.Text, txtShortUrl.Text, IsActive.ToString(), dbc.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss"), qrCodeURL, id1 };
                string query = "UPDATE [SuperFranchisee] SET [SuperFranchiseeName]=@1,[SuperFranchiseeContactNumber]=@2,[SuperFranchiseeEmailAddress]=@3,[SuperFranchiseeCustomerCode]=@4," +
                               " [ShortUrl]=@5,[IsActive]=@6,[CreatedOn]=@7,[QRCodeUrl]=@8 where [SuperFranchiseeID]=@9";
                int v1 = dbc.ExecuteQueryWithParams(query, para1);
                if (v1 > 0)
                {
                    string updateUser = " UPDATE  [Users] SET [UserName]='" + txtUserName.Text.ToString() + "',[Password]='" + txtPassword.Text.ToString() + "'," +
                                       " [Name]='" + txtUserName.Text.ToString() + "',[UserType]=6,[FranchiseeId]=" + id1 + ",[Mobile] = '" + txtContactNumber.Text.ToString() + "'" +
                                       " WHERE FranchiseeId = " + id1;
                    int userVAL = dbc.ExecuteQuery(updateUser);
                    sweetMessage("", "Super Franchisee Updated Successfully", "success");
                    Response.Redirect("SuperFranchiseeList.aspx", true);
                }
            }
            else
            {
                
                byte[] QRCodebyteImage = GenerateQRCode(qrCodeURL);
                string newfileName = txtSuperFranchiseeName.Text + "_" + txtCustomerCode.Text;
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


                string[] para1 = { txtSuperFranchiseeName.Text.ToString().Replace("'", "''"),
            txtContactNumber.Text.ToString().Replace("'", "''"),
            txtEmailAddress.Text.ToString().Replace("'", "''"),
            txtCustomerCode.Text.ToString().Replace("'", "''"),
            txtShortUrl.Text.ToString(),
            IsActive.ToString(),
            dbc.getindiantimeString(),
            "0",
            qrCodeURL,
            filepath
            };
                string query = "INSERT INTO [dbo].[SuperFranchisee] ([SuperFranchiseeName] ,[SuperFranchiseeContactNumber],[SuperFranchiseeEmailAddress],[SuperFranchiseeCustomerCode]," +
                               " [ShortUrl],[IsActive],[CreatedOn],[IsDeleted],[QRCodeUrl],[QrCodeImage])" +
                               " VALUES (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10); SELECT SCOPE_IDENTITY();";
                int VAL = dbc.ExecuteQueryWithParamsId(query, para1);
                if (VAL > 0)
                {
                    hdnFranchiseeID.Value = VAL.ToString();
                    hdnContact.Value = txtContactNumber.Text.ToString().Replace("'", "''");

                    string InsertUserqry = "INSERT INTO [Users] ([UserName],[Password],[Name],[UserType],[FranchiseeId],[Mobile]) VALUES ('" + txtUserName.Text.ToString().Replace("'", "''") + "','" + txtPassword.Text.ToString().Replace("'", "''") + "','" + txtUserName.Text.ToString().Replace("'", "''") + "',6," + VAL.ToString() + ",'" + txtContactNumber.Text.ToString() + "')";
                    int userVAL = dbc.ExecuteQuery(InsertUserqry);

                    sweetMessage("", "Super Franchisee Uploaded Successfully", "success");
                    Response.Redirect("SuperFranchiseeList.aspx", true);
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
            sb.Append("window.location.href = 'SuperFranchiseeList.aspx'");
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
        if (dtFdata.Rows.Count > 0)
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