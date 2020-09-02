using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Jurisdiction_AddJurisdiction : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    string  id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            
            lblmsg.Text = "";
            if (!IsPostBack)
            {
                id = Request.QueryString["Id"];

                if (id != null && !id.Equals(""))
                {
                    string Stateqry = "Select Id as Id,StateName as Name from [StateMaster] where IsActive = 1 order by Id";
                    DataTable dtState = dbc.GetDataTable(Stateqry);
                    ddlState.DataSource = dtState;
                    ddlState.DataTextField = "Name";
                    ddlState.DataValueField = "Id";
                    ddlState.DataBind();

                    string Cityqry = "Select Id as Id,CityName as Name from [CityMaster] where IsActive = 1 order by Id";
                    DataTable dtCity = dbc.GetDataTable(Cityqry);
                    ddlCity.DataSource = dtCity;
                    ddlCity.DataTextField = "Name";
                    ddlCity.DataValueField = "Id";
                    ddlCity.DataBind();

                    string query = "SELECT JM.JurisdictionID,JM.JurisdictionIncharge,JM.Contact,JM.EmalID,JM.StateId,JM.CityId,JM.Comments,JM.IsActive FROM JurisdictionMaster Jm INNER JOIN StateMaster sm on JM.StateId = sm.Id INNER JOIN CityMaster cm on JM.CityId = cm.Id where isnull(Jm.IsDeleted,0)=0  and JM.JurisdictionID=" + id;
                    DataTable dtUpdate = dbc.GetDataTable(query);
                    if(dtUpdate.Rows.Count > 0)
                    {
                        hdnJurisdictionID.Value = dtUpdate.Rows[0]["JurisdictionID"].ToString();
                        txtJurisdictionIncharge.Text = dtUpdate.Rows[0]["JurisdictionIncharge"].ToString();
                        txtContact.Text = dtUpdate.Rows[0]["Contact"].ToString();
                        txtEmailId.Text = dtUpdate.Rows[0]["EmalID"].ToString();
                        ddlState.SelectedValue = dtUpdate.Rows[0]["StateId"].ToString();
                        ddlCity.SelectedValue = dtUpdate.Rows[0]["CityId"].ToString();
                        txtComments.Text = dtUpdate.Rows[0]["Comments"].ToString();
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
    protected void OnStateSelectedIndexChanged(object sender, EventArgs e)
    {
        string Cityqry = "Select Id as Id,CityName as Name from [CityMaster] where IsActive = 1 and StateId = " + ddlState.SelectedItem.Value + " order by Id";
        DataTable dtCity = dbc.GetDataTable(Cityqry);
        ddlCity.DataSource = dtCity;
        ddlCity.DataTextField = "Name";
        ddlCity.DataValueField = "Id";
        ddlCity.DataBind();

     
    }
    protected void OnCitySelectedIndexChanged(object sender, EventArgs e)
    {
      
    }
    protected void BtnUpdate_Click(object sender, EventArgs e)
    {
        try
        {
            string userId = Request.Cookies["TUser"]["Id"].ToString();
            DateTime dt = DateTime.Now;
            string[] para1 = { 
            txtContact.Text.ToString().Replace("'", "''"),
            txtEmailId.Text.ToString().Replace("'", "''"),
            txtComments.Text.ToString().Replace("'", "''"),
            dt.ToString(),
            userId
            };
            id = Request.QueryString["Id"];
            if (id != null && !id.Equals(""))
            {
                string queryupdate = "UPDATE [JurisdictionMaster] SET [Contact]=@1,[EmalID]=@2,[Comments]=@3,[ModifiedOn]=@4,[ModifiedBy]=@5 Where JurisdictionID = " + id;
                int v1 = dbc.ExecuteQueryWithParams(queryupdate, para1);
                if (v1 > 0)
                {
                    sweetMessage("", "Juurisdiction Updated Successfully", "success");
                    Response.Redirect("Jurisdiction.aspx", true);
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
   
}