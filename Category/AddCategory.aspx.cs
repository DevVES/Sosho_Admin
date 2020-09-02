using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using WebApplication1;

public partial class Category_AddCategory : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    string FileName = "", id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblmsg.Text = "";
            if (!IsPostBack)
            {
                chkisactive.Checked = true;
                id = Request.QueryString["Id"];
                if (id != null && !id.Equals(""))
                {
                    BtnSave.Text = "Update";

                    string query = "SELECT CategoryID,CategoryName,CategoryDescription,IsActive,CreatedOn,CategoryImage FROM Category where isnull(IsDeleted,0)=0  and CategoryID = " + id;
                    DataTable dtUpdate = dbc.GetDataTable(query);
                    if (dtUpdate.Rows.Count > 0)
                    {
                        txtCategoryName.Text = dtUpdate.Rows[0]["CategoryName"].ToString();
                        txtDescription.Text = dtUpdate.Rows[0]["CategoryDescription"].ToString();
                        if (dtUpdate.Rows[0]["IsActive"].ToString() == "True")
                            chkisactive.Checked = true;
                        else
                            chkisactive.Checked = false;

                        CategoryImage.ImageUrl = "../CategoryImage/" + dtUpdate.Rows[0]["CategoryImage"].ToString();
                    }

                }
            }
        }
        catch (Exception ee)
        {
            lblmsg.Text = "Error:" + ee.Message + " ::: " + ee.StackTrace + " :::: " + ee.InnerException;
        }
    }
    protected void FileUpload1_DataBinding(object sender, EventArgs e)
    {
        try
        {
            if (FileUpload1.HasFile)
            {
                CategoryImage.ImageUrl = FileUpload1.FileName;
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
                CategoryImage.ImageUrl = FileUpload1.FileName;
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
                CategoryImage.ImageUrl = FileUpload1.FileName;
            }
        }
        catch (Exception ex)
        {

        }
    }
    protected void BtnRemoveImage_Click(object sender, EventArgs e)
    {
        try
        {
            filedel();
        }
        catch (Exception aa)
        {

        }
    }
    public void filedel()
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
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string userId = Request.Cookies["TUser"]["Id"].ToString();
            int IsActive = 0;
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
                string path = Server.MapPath("/CategoryImage");
                FileUpload1.SaveAs(path + "/" + imgnamenew);
            }

            string fileName = "";
            fileName = imgnamenew;


            if (chkisactive.Checked)
            {
                IsActive = 1;
            }
            DateTime dt = DateTime.Now;

            if (BtnSave.Text.Equals("Update"))
            {
                string id = Request.QueryString["id"].ToString();
                if (fileName != "")
                {
                    string[] para1 = { txtCategoryName.Text, txtDescription.Text, IsActive.ToString(), fileName, dt.ToString(),userId, id };
                    string query = "UPDATE [Category] SET [CategoryName]=@1,[CategoryDescription]=@2,[IsActive]=@3,[CategoryImage]=@4,[ModifiedOn]=@5,[ModifiedBy]=@6 where [CategoryID]=@7";
                    int v1 = dbc.ExecuteQueryWithParams(query, para1);
                    if (v1 > 0)
                    {
                        sweetMessage("", "Category Updated Successfully", "success");
                        Response.Redirect("Category.aspx", true);
                    }
                }
                else if (fileName == "")
                {
                    string[] para1 = { txtCategoryName.Text, txtDescription.Text,  IsActive.ToString(), dt.ToString(), userId, id };

                    string query = "UPDATE [Category] SET [CategoryName]=@1,[CategoryDescription]=@2,[IsActive]=@3,[ModifiedOn]=@4,[ModifiedBy]=@5 where [CategoryID]=@6";
                    int v1 = dbc.ExecuteQueryWithParams(query, para1);
                    if (v1 > 0)
                    {
                        sweetMessage("", "Category Updated Successfully", "success");
                        Response.Redirect("Category.aspx", true);
                    }
                    else
                    {
                        sweetMessage("", "Please Try Again!!", "warning");
                    }
                }
            }
            else
            {
                string query = "INSERT INTO [dbo].[Category] ([CategoryName] ,[CategoryDescription],[CategoryImage],[IsActive],[IsDeleted],[CreatedOn],[CreatedBy]) VALUES ('" + txtCategoryName.Text.ToString().Replace("'", "''") + "','" + txtDescription.Text.ToString().Replace("'", "''") + "','" + fileName + "'," + IsActive + ",0,'" + dt.ToString() + "'," + userId + ")";
                int VAL = dbc.ExecuteQuery(query);

                if (VAL > 0)
                {
                    sweetMessage("", "Category Added Successfully", "success");
                    Response.Redirect("Category.aspx", true);
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
            sb.Append("window.location.href = 'Category.aspx'");
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

}