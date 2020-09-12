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
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            lblmsg.Text = "";
            if (!IsPostBack)
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

                chkisactive.Checked = true;

                string PinCodeqry = "Select Distinct Zipcode from Zipcode Where zipcode not in ( SELECT PinCodeID FROM JurisdictionDetail where IsActive = 1)";
                DataTable dtPincode = dbc.GetDataTable(PinCodeqry);
                chklstPincode.DataSource = dtPincode;
                chklstPincode.DataTextField = "zipcode";
                chklstPincode.DataValueField = "zipcode";
                chklstPincode.DataBind();

                
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
        string PinCodeqry = "Select Location,zipcode from Zipcode Where State = '" + ddlState.SelectedItem.Text + "' and District = '" + ddlCity.SelectedItem.Text + "' order by zipcode";
        DataTable dtPincode = dbc.GetDataTable(PinCodeqry);
        chklstPincode.DataSource = dtPincode;
        chklstPincode.DataTextField = "zipcode";
        chklstPincode.DataValueField = "Location";
        chklstPincode.DataBind();
    }
    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string userId = Request.Cookies["TUser"]["Id"].ToString();
            int IsActive = 0;
            
            DateTime dt = DateTime.Now;
            if (chkisactive.Checked)
                IsActive = 1;

            string[] para1 = { txtJurisdictionIncharge.Text.ToString().Replace("'", "''"),
            txtContact.Text.ToString().Replace("'", "''"),
            txtEmailId.Text.ToString().Replace("'", "''"),
            ddlState.SelectedItem.Value,
            ddlCity.SelectedItem.Value,
            txtComments.Text.ToString().Replace("'", "''"),
            IsActive.ToString(),
            dt.ToString(),
            userId
            };

            //string InsertJurisdictionMasterqry = "INSERT INTO [JurisdictionMaster] ([JurisdictionIncharge],[Contact],[EmalID],[StateId],[CityId],[Comments],[IsActive],[CreatedOn],[CreatedBy]) VALUES ('" + txtJurisdictionIncharge.Text.ToString().Replace("'", "''") + "','" + txtContact.Text.ToString().Replace("'", "''") + "','" + txtEmailId.Text.ToString().Replace("'", "''") + "'," + ddlState.SelectedItem.Value + "," + ddlCity.SelectedItem.Value + ",'" + txtComments.Text.ToString().Replace("'", "''") + "'," + IsActive + ",'" + dt + "'," + userId + ")";
            //int VAL = dbc.ExecuteQuery(InsertJurisdictionMasterqry);

            string query1 = "INSERT INTO [JurisdictionMaster] ([JurisdictionIncharge],[Contact],[EmalID],[StateId],[CityId],[Comments],[IsActive],[CreatedOn],[CreatedBy]) VALUES (@1,@2,@3,@4,@5,@6,@7,@8,@9); SELECT SCOPE_IDENTITY();";
            int VAL = dbc.ExecuteQueryWithParamsId(query1, para1);

            if (VAL > 0)
            {
            
                string InsertJDetailqry = "";
                List<ListItem> selectedPincode = new List<ListItem>();
                selectedPincode = chklstPincode.Items.Cast<ListItem>().Where(n => n.Selected).ToList();
                foreach (ListItem item in selectedPincode)
                {
                    InsertJDetailqry = "INSERT INTO [JurisdictionDetail] ([JurisdictionID],[PinCodeID],[IsActive],[CreatedOn],[CreatedBy]) VALUES ('" + VAL + "','" + item + "'," + IsActive + ",'" + dt + "'," + userId + ")";
                    int VAL1 = dbc.ExecuteQuery(InsertJDetailqry);
                }
                if (Convert.ToInt32(VAL) > 0)
                {
                    hdnJurisdictionID.Value = VAL.ToString();
                    hdnContact.Value = txtContact.Text.ToString().Replace("'", "''");
                    sweetMessage("", "Jurisdiction Added Successfully", "success");
                    Response.Redirect("Jurisdiction.aspx");
                }
                else
                {
                    hdnJurisdictionID.Value = "0";
                    sweetMessage("", "Please Try Again!!", "warning");
                }
            }
            else
            {
                sweetMessage("", "Please Try Again!!", "warning");
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
    protected void BtnUserSave_Click(object sender, EventArgs e)
    {
        try
        {
            if(hdnJurisdictionID.Value.ToString() != "0")
            {
                string InsertUserqry = "INSERT INTO [Users] ([UserName],[Password],[Name],[UserType],[JurisdictionID],[Mobile]) VALUES ('" + txtUserName.Text.ToString().Replace("'", "''") + "','" + txtPassword.Text.ToString().Replace("'", "''") + "','" + txtUserName.Text.ToString().Replace("'", "''") + "',2," + hdnJurisdictionID.Value + "," + hdnContact.Value + ")";
                int VAL = dbc.ExecuteQuery(InsertUserqry);
                if (VAL > 0)
                {
                    sweetMessage("", "Jurisdiction User Added Successfully", "success");
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
}