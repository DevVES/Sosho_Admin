using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Area_AddArea : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    string id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            id = Request.QueryString["Id"];

            string stateQuery = "SELECT Id,StateName FROM [dbo].[StateMaster] where isnull(IsActive,0)=1 order by Id asc";
            DataTable dtState = dbc.GetDataTable(stateQuery);
            ddlStateName.DataSource = dtState;
            ddlStateName.DataTextField = "StateName";
            ddlStateName.DataValueField = "Id";
            ddlStateName.DataBind();

            string cityQuery = "SELECT Id, CityName FROM [dbo].[CityMaster] where isnull(IsActive,0)=1 order by Id asc";
            DataTable dtCity = dbc.GetDataTable(cityQuery);
            ddlCityName.DataSource = dtCity;
            ddlCityName.DataTextField = "CityName";
            ddlCityName.DataValueField = "Id";
            ddlCityName.DataBind();

            if (id != null && !id.Equals(""))
            {
                string query = "SELECT Id,Area,zipcode,State,District AS City,IsActive,CreatedOn FROM ZipCode where isnull(IsDeleted,0)=0  and Id = " + id;
                DataTable dtUpdate = dbc.GetDataTable(query);
                if (dtUpdate.Rows.Count > 0)
                {
                    BtnSave.Text = "Update";
                    txtArea.Text = dtUpdate.Rows[0]["Area"].ToString();
                    txtZipCode.Text = dtUpdate.Rows[0]["zipcode"].ToString();
                    ddlCityName.Items.FindByText(dtUpdate.Rows[0]["City"].ToString()).Selected = true;
                    ddlStateName.Items.FindByText(dtUpdate.Rows[0]["State"].ToString()).Selected = true;
                    if (dtUpdate.Rows[0]["IsActive"].ToString() == "True")
                        chkisactive.Checked = true;
                    else
                        chkisactive.Checked = false;

                }
            }

        }
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string userId = Request.Cookies["TUser"]["Id"].ToString();
            int IsActive = 0;
            string zipcode = txtZipCode.Text.ToString();
            string area = txtArea.Text.ToString();
            string state = ddlStateName.SelectedItem.ToString();
            string city = ddlCityName.SelectedItem.ToString();
            DateTime dt = DateTime.Now;
            if (chkisactive.Checked)
            {
                IsActive = 1;
            }
            if (BtnSave.Text.Equals("Update"))
            {

                string id = Request.QueryString["id"].ToString();
                string query = "UPDATE [Zipcode] SET [Area]='" + area + "',[zipcode]='" + zipcode + "',[State]='" + state + "',[District]='" + city + "',[IsActive] = " + IsActive + ",[ModifiedOn]='" + dt.ToString() + "',[ModifiedBy]=" + userId + " Where  Id = " + id;
                int v1 = dbc.ExecuteQuery(query);
                if (v1 > 0)
                {
                    sweetMessage("", "Area Updated Successfully", "success");
                    Response.Redirect("Area.aspx", true);
                }
                else
                {
                    sweetMessage("", "Please Try Again!!", "warning");
                }
            }
            else
            {
                string query = "INSERT INTO [dbo].[Zipcode] ([Area] ,[zipcode],[State],[District],[IsActive],[IsDeleted],[CreatedOn],[CreatedBy]) VALUES ('" + area + "','" + zipcode + "','" + state + "','" + city + "'," + IsActive + ",0,'" + dt.ToString() + "'," + userId + ")";
                int VAL = dbc.ExecuteQuery(query);

                if (VAL > 0)
                {
                    sweetMessage("", "Area Added Successfully", "success");
                    Response.Redirect("Area.aspx", true);
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
            sb.Append("window.location.href = 'Location.aspx'");
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