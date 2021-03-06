﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Building_AddBuilding : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    string id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            id = Request.QueryString["Id"];

            string zipcodeQuery = "SELECT Id,ZipCode FROM [dbo].[ZipCode] where isnull(IsActive,0)=1 order by Id asc";
            DataTable dtZipCode = dbc.GetDataTable(zipcodeQuery);
            ddlZipCode.DataSource = dtZipCode;
            ddlZipCode.DataTextField = "ZipCode";
            ddlZipCode.DataValueField = "Id";
            ddlZipCode.DataBind();

            //string locationQuery = "SELECT Id, Location FROM [dbo].[ZipCode] where isnull(IsActive,0)=1 order by Id asc";
            //DataTable dtLocation = dbc.GetDataTable(locationQuery);
            //ddlLocation.DataSource = dtLocation;
            //ddlLocation.DataTextField = "Location";
            //ddlLocation.DataValueField = "Id";
            //ddlLocation.DataBind();

            if (id != null && !id.Equals(""))
            {

                string query = "SELECT A.Id,A.Building, Z.Area, Z.ZipCode,A.ZipcodeId,A.IsActive,A.CreatedOn FROM tblBuilding A LEFT JOIN [dbo].[ZipCode] Z ON Z.Id = A.ZipCodeId where isnull(A.IsDeleted,0)=0  and A.Id = " + id;
                DataTable dtUpdate = dbc.GetDataTable(query);
                if (dtUpdate.Rows.Count > 0)
                {
                    ddlArea.Items.Clear();
                    string locationQuery = "SELECT Id, Area FROM [dbo].[ZipCode] where isnull(IsActive,0)=1 AND ZipCode = '" + dtUpdate.Rows[0]["ZipCode"].ToString() + "' order by Id asc";
                    DataTable dtLocation = dbc.GetDataTable(locationQuery);
                    ddlArea.DataSource = dtLocation;
                    ddlArea.DataTextField = "Area";
                    ddlArea.DataValueField = "Id";
                    ddlArea.DataBind();

                    BtnSave.Text = "Update";
                    txtBuilding.Text = dtUpdate.Rows[0]["Building"].ToString();
                    ddlZipCode.Items.FindByText(dtUpdate.Rows[0]["ZipCode"].ToString()).Selected = true;
                    ddlArea.Items.FindByText(dtUpdate.Rows[0]["Area"].ToString()).Selected = true;
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
            string building = txtBuilding.Text.ToString();
            string area = ddlArea.SelectedItem.ToString();
            string zipcode = ddlZipCode.SelectedItem.ToString();
            DateTime dt = DateTime.Now;
            if (chkisactive.Checked)
            {
                IsActive = 1;
            }
            string zipcodeid = "";
            if (BtnSave.Text.Equals("Update"))
            {

                string id = Request.QueryString["id"].ToString();
                string getZipCodeId = "SELECT Id FROM [dbo].[ZipCode] WHERE ZipCode = '" + zipcode + "' AND Area=" + "'" + area + "'";
                DataTable dtZipCodeId = dbc.GetDataTable(getZipCodeId);
                if (dtZipCodeId.Rows.Count > 0)
                {
                    zipcodeid = dtZipCodeId.Rows[0]["Id"].ToString();
                    string query = "UPDATE [tblBuilding] SET [Building]='" + building + "',[zipcode]='" + zipcode + "',[Area]='" + area + "',[ZipCodeId]='" + zipcodeid + "',[IsActive] = " + IsActive + ",[ModifiedOn]='" + dt.ToString() + "',[ModifiedBy]=" + userId + " Where  Id = " + id;
                    int v1 = dbc.ExecuteQuery(query);
                    if (v1 > 0)
                    {
                        sweetMessage("", "Society/Building Updated Successfully", "success");
                        Response.Redirect("Building.aspx", true);
                    }
                    else
                    {
                        sweetMessage("", "Please Try Again!!", "warning");
                    }
                }
            }
            else
            {

                string getZipCodeId = "SELECT Id FROM [dbo].[ZipCode] WHERE ZipCode = '" + zipcode + "' AND Area=" + "'" + area + "'";
                DataTable dtZipCodeId = dbc.GetDataTable(getZipCodeId);
                if (dtZipCodeId.Rows.Count > 0)
                {
                    zipcodeid = dtZipCodeId.Rows[0]["Id"].ToString();
                    string query = "INSERT INTO [dbo].[tblBuilding] ([Building] ,[zipcode],[Area],[ZipCodeId],[IsActive],[IsDeleted],[CreatedOn],[CreatedBy]) VALUES ('" + building + "','" + zipcode + "','" + area + "'," + zipcodeid + "," + IsActive + ",0,'" + dt.ToString() + "'," + userId + ")";
                    int VAL = dbc.ExecuteQuery(query);

                    if (VAL > 0)
                    {
                        sweetMessage("", "Society/Building Added Successfully", "success");
                        Response.Redirect("Building.aspx", true);
                    }
                    else
                    {
                        sweetMessage("", "Please Try Again!!", "warning");
                    }
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
            sb.Append("window.location.href = 'Area.aspx'");
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

    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    {
        ddlArea.Items.Clear();
        string locationQuery = "SELECT Id, Area FROM [dbo].[ZipCode] where isnull(IsActive,0)=1 AND ZipCode = '" + ddlZipCode.SelectedItem + "' order by Id asc";
        DataTable dtLocation = dbc.GetDataTable(locationQuery);
        ddlArea.DataSource = dtLocation;
        ddlArea.DataTextField = "Area";
        ddlArea.DataValueField = "Id";
        ddlArea.DataBind();
    }
}