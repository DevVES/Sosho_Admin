using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class SubCategory_AddSubCategory : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    string id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblmsg.Text = "";
            if (!IsPostBack)
            {
                string categoryqry = "SELECT CategoryId,CategoryName FROM Category where isnull(IsDeleted,0)=0 order by Sequence asc";
                DataTable dtcategory = dbc.GetDataTable(categoryqry);
                ddlCategoryName.DataSource = dtcategory;
                ddlCategoryName.DataTextField = "CategoryName";
                ddlCategoryName.DataValueField = "CategoryId";
                ddlCategoryName.DataBind();

                chkisactive.Checked = true;
                id = Request.QueryString["Id"];
                if (id != null && !id.Equals(""))
                {
                    BtnSave.Text = "Update";

                    

                    string query = "SELECT CategoryID,SubCategory,Description,IsActive,CreatedOn,Sequence " + 
                                   " FROM tblSubCategory where isnull(IsDeleted,0)=0  and Id = " + id;
                    DataTable dtUpdate = dbc.GetDataTable(query);
                    if (dtUpdate.Rows.Count > 0)
                    {
                        ddlCategoryName.SelectedValue = dtUpdate.Rows[0]["CategoryID"].ToString();
                        txtSubCategoryName.Text = dtUpdate.Rows[0]["SubCategory"].ToString();
                        txtDescription.Text = dtUpdate.Rows[0]["Description"].ToString();
                        txtSequence.Text = dtUpdate.Rows[0]["Sequence"].ToString();
                        if (dtUpdate.Rows[0]["IsActive"].ToString() == "True")
                            chkisactive.Checked = true;
                        else
                            chkisactive.Checked = false;

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
            string userId = Request.Cookies["TUser"]["Id"].ToString();
            string categoryId= ddlCategoryName.SelectedValue.ToString();
            int IsActive = 0;

            if (chkisactive.Checked)
            {
                IsActive = 1;
            }
            DateTime dt = DateTime.Now;

            if (BtnSave.Text.Equals("Update"))
            {
                string id = Request.QueryString["id"].ToString();
       
                    string[] para1 = { txtSubCategoryName.Text, txtDescription.Text, IsActive.ToString(), dt.ToString(), userId, id, txtSequence.Text, categoryId };

                    string query = "UPDATE [tblSubCategory] SET [SubCategory]=@1,[Description]=@2,[IsActive]=@3,[ModifiedOn]=@4,[ModifiedBy]=@5,[sequence]=@7,[CategoryId]=@8 where [Id]=@6";
                    int v1 = dbc.ExecuteQueryWithParams(query, para1);
                    if (v1 > 0)
                    {
                        sweetMessage("", "SubCategory Updated Successfully", "success");
                        Response.Redirect("SubCategory.aspx", true);
                    }
                    else
                    {
                        sweetMessage("", "Please Try Again!!", "warning");
                    }
            }
            else
            {
                string query = "INSERT INTO [dbo].[tblSubCategory] ([SubCategory] ,[Description],[CategoryId],[IsActive],[IsDeleted],[CreatedOn],[CreatedBy],[sequence]) " +
                                " VALUES ('" + txtSubCategoryName.Text.ToString().Replace("'", "''") + "','" + txtDescription.Text.ToString().Replace("'", "''") + "','" + categoryId + "'," + IsActive + ",0,'" + dt.ToString() + "'," + userId + "," + txtSequence.Text + ")";
                int VAL = dbc.ExecuteQuery(query);

                if (VAL > 0)
                {
                    sweetMessage("", "SubCategory Added Successfully", "success");
                    Response.Redirect("SubCategory.aspx", true);
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
            sb.Append("window.location.href = 'SubCategory.aspx'");
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