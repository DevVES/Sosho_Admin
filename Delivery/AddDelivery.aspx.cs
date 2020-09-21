using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Delivery_AddDelivery : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    string id = "";
    private string pincode = "";
    private int iCtr = 0;
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

                List<ListItem> selectedPincode = new List<ListItem>();
                selectedPincode = chklstPincode.Items.Cast<ListItem>().Where(n => n.Selected).ToList();
                string pincode = string.Empty;
                int iCtr = 0;
                foreach (ListItem item in selectedPincode)
                {
                    if (iCtr == 1)
                    {
                        pincode += ",";
                    }
                    pincode += item.Text;
                    ++iCtr;
                }
                string Locationqry = "Select Id as Id,Location as Name from [ZipCode] where IsActive = 1 AND zipcode in('" + pincode + "')  order by Id";
                DataTable dtLocation = dbc.GetDataTable(Locationqry);
                ddlLocation.DataSource = dtLocation;
                ddlLocation.DataTextField = "Name";
                ddlLocation.DataValueField = "Id";
                ddlLocation.DataBind();

                id = Request.QueryString["Id"];

                if (id != null && !id.Equals(""))
                {
                    
                    string query = "SELECT JM.DeliveryID,JM.DeliveryIncharge,JM.Contact,JM.EmalID,JM.StateId,JM.CityId,JM.Comments," + 
                                   " JM.IsActive FROM DeliveryMaster Jm " + 
                                   " INNER JOIN StateMaster sm on JM.StateId = sm.Id " + 
                                   " INNER JOIN CityMaster cm on JM.CityId = cm.Id " +
                                   " where isnull(Jm.IsDeleted,0)=0  and JM.DeliveryID=" + id;
                    DataTable dtUpdate = dbc.GetDataTable(query);
                    if (dtUpdate.Rows.Count > 0)
                    {
                        hdnDeliveryID.Value = dtUpdate.Rows[0]["DeliveryID"].ToString();
                        txtDeliveryIncharge.Text = dtUpdate.Rows[0]["DeliveryIncharge"].ToString();
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

                    Locationqry = "Select Id as Id,Location as Name from [ZipCode] where IsActive = 1  order by Id";
                    dtLocation = dbc.GetDataTable(Locationqry);
                    ddlLocation.DataSource = dtLocation;
                    ddlLocation.DataTextField = "Name";
                    ddlLocation.DataValueField = "Id";
                    ddlLocation.DataBind();

                    DataTable dtAreaQuery = dbc.GetDataTable("SELECT DeliveryDetailID,PinCodeID,LocationId, Z.Location, A.Area,DD.AreaId FROM DeliveryDetail DD INNER JOIN Zipcode Z ON DD.LocationId = Z.Id INNER JOIN tblArea A ON A.Id = DD.AreaId WHERE DeliveryID = " + id);
                    if (dtAreaQuery.Rows.Count > 0)
                    {
                        string Areaqry = "Select Id,Area from tblArea Where Location = '" + dtAreaQuery.Rows[0]["Location"].ToString() + "' order by Id";
                        DataTable dtArea = dbc.GetDataTable(Areaqry);
                        chklstArea.DataSource = dtArea;
                        chklstArea.DataTextField = "Area";
                        chklstArea.DataValueField = "Id";
                        chklstArea.DataBind();

                        ddlLocation.Items.FindByText(dtAreaQuery.Rows[0]["Location"].ToString()).Selected = true;
                        chklstPincode.SelectedValue = dtAreaQuery.Rows[0]["PinCodeID"].ToString();
                        //List<ListItem> selectedIncharge = new List<ListItem>();
                        
                        //selectedIncharge = chklstArea.Items.Cast<ListItem>().Where(n => n.Selected).ToList();
                        //foreach (var item in dtAreaQuery.Rows)
                        //{
                        //    chklstArea.SelectedValue = item;
                        //}

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
        string PinCodeqry = "Select Location,zipcode from Zipcode Where State = '" + ddlState.SelectedItem.Text + "' and District = '" + ddlCity.SelectedItem.Text + "' order by zipcode";
        DataTable dtPincode = dbc.GetDataTable(PinCodeqry);
        chklstPincode.DataSource = dtPincode;
        chklstPincode.DataTextField = "zipcode";
        chklstPincode.DataValueField = "Location";
        chklstPincode.DataBind();
    }

    protected void OnLocationSelectedIndexChanged(object sender, EventArgs e)
    {
        string Areaqry = "Select Id,Area from tblArea Where Location = '" + ddlLocation.SelectedItem.Text + "' order by Id";
        DataTable dtArea = dbc.GetDataTable(Areaqry);
        chklstArea.DataSource = dtArea;
        chklstArea.DataTextField = "Area";
        chklstArea.DataValueField = "Id";
        chklstArea.DataBind();
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

            string[] para1 = { txtDeliveryIncharge.Text.ToString().Replace("'", "''"),
            txtContact.Text.ToString().Replace("'", "''"),
            txtEmailId.Text.ToString().Replace("'", "''"),
            ddlState.SelectedItem.Value,
            ddlCity.SelectedItem.Value,
            txtComments.Text.ToString().Replace("'", "''"),
            IsActive.ToString(),
            dt.ToString(),
            userId
            };

            string query1 = "INSERT INTO [DeliveryMaster] ([DeliveryIncharge],[Contact],[EmalID],[StateId],[CityId],[Comments],[IsActive],[CreatedOn],[CreatedBy]) VALUES (@1,@2,@3,@4,@5,@6,@7,@8,@9); SELECT SCOPE_IDENTITY();";
            int VAL = dbc.ExecuteQueryWithParamsId(query1, para1);

            if (VAL > 0)
            {
                int locationId = Convert.ToInt32(ddlLocation.SelectedValue);
                string pincode = string.Empty;
                string pincodeQry = " SELECT Top 1 zipCode FROM ZipCode WHERE Id = " + locationId;
                DataTable dtPincode = dbc.GetDataTable(pincodeQry);
                if(dtPincode.Rows.Count > 0)
                {
                    pincode = dtPincode.Rows[0]["zipcode"].ToString();
                }
                string InsertDetailqry = "";
                List<ListItem> selectedArea = new List<ListItem>();
                selectedArea = chklstArea.Items.Cast<ListItem>().Where(n => n.Selected).ToList();
                foreach (ListItem item in selectedArea)
                {
                    InsertDetailqry = "INSERT INTO [DeliveryDetail] ([DeliveryID],[PinCodeID],[LocationId],[AreaId],[IsActive],[CreatedOn],[CreatedBy]) VALUES ('" + VAL + "','" + pincode + "',"+ locationId + ","+item.Value+"," + IsActive + ",'" + dt + "'," + userId + ")";
                    int VAL1 = dbc.ExecuteQuery(InsertDetailqry);
                }
                if (Convert.ToInt32(VAL) > 0)
                {
                    hdnDeliveryID.Value = VAL.ToString();
                    hdnContact.Value = txtContact.Text.ToString().Replace("'", "''");
                    sweetMessage("", "Delivery Added Successfully", "success");
                    Response.Redirect("Delivery.aspx");
                }
                else
                {
                    hdnDeliveryID.Value = "0";
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

    protected void OnCheckBox_Changed(object sender, EventArgs e)
    {
        
        foreach (ListItem item in chklstPincode.Items)
        {
            if (item.Selected)
            {
                if (!string.IsNullOrEmpty(pincode))
                {
                    pincode += ",";
                }
                pincode += item.Value;
                ++iCtr;
            }
        }
        if (!string.IsNullOrEmpty(pincode))
        {
            string Locationqry = "Select Id as Id,Location as Name from [ZipCode] where IsActive = 1 AND zipcode in(" + pincode + ")  order by Id";
            DataTable dtLocation = dbc.GetDataTable(Locationqry);
            ddlLocation.DataSource = dtLocation;
            ddlLocation.DataTextField = "Name";
            ddlLocation.DataValueField = "Id";
            ddlLocation.DataBind();
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
            if (hdnDeliveryID.Value.ToString() != "0")
            {
                string InsertUserqry = "INSERT INTO [Users] ([UserName],[Password],[Name],[UserType],[JurisdictionID],[Mobile]) VALUES ('" + txtUserName.Text.ToString().Replace("'", "''") + "','" + txtPassword.Text.ToString().Replace("'", "''") + "','" + txtUserName.Text.ToString().Replace("'", "''") + "',2," + hdnDeliveryID.Value + "," + hdnContact.Value + ")";
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