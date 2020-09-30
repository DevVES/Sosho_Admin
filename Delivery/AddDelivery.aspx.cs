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

                string PinCodeqry = "Select Distinct Zipcode from Zipcode Where zipcode not in ( SELECT PinCodeID FROM DeliveryDetail where IsActive = 1)";
                DataTable dtPincode = dbc.GetDataTable(PinCodeqry);
                ddlPinCode.DataSource = dtPincode;
                ddlPinCode.DataTextField = "zipcode";
                ddlPinCode.DataValueField = "zipcode";
                ddlPinCode.DataBind();

                string Areaqry = "Select Id as Id,Area as Name from [ZipCode] where IsActive = 1 AND zipcode =" + ddlPinCode.SelectedItem.Text + "  order by Id";
                DataTable dtArea = dbc.GetDataTable(Areaqry);
                ddlArea.DataSource = dtArea;
                ddlArea.DataTextField = "Name";
                ddlArea.DataValueField = "Id";
                ddlArea.DataBind();

                id = Request.QueryString["Id"];

                if (id != null && !id.Equals(""))
                {
                    BtnSave.Text = "Update";
                    string query = "SELECT JM.DeliveryID,JM.DeliveryIncharge,JM.Contact,JM.EmalID,JM.StateId,JM.CityId,JM.Comments," +
                                   " JM.IsActive, U.UserName,U.Password FROM DeliveryMaster Jm " +
                                   " INNER JOIN StateMaster sm on JM.StateId = sm.Id " +
                                   " INNER JOIN CityMaster cm on JM.CityId = cm.Id " +
                                   " INNER JOIN Users U on U.DeliveryId = Jm.DeliveryId " +
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

                        txtUserName.Text = dtUpdate.Rows[0]["UserName"].ToString();
                        txtPassword.Text = "Password";
                        txtPassword.Attributes["value"] = dtUpdate.Rows[0]["Password"].ToString();
                        txtConfirmPassword.Text = "Password";
                        txtConfirmPassword.Attributes["value"] = dtUpdate.Rows[0]["Password"].ToString();

                    }

                    PinCodeqry = "Select Distinct Zipcode from Zipcode Where zipcode not in ( SELECT PinCodeID FROM DeliveryDetail where IsActive = 1 AND DeliveryId not in ("+id+"))";
                    dtPincode = dbc.GetDataTable(PinCodeqry);
                    ddlPinCode.DataSource = dtPincode;
                    ddlPinCode.DataTextField = "zipcode";
                    ddlPinCode.DataValueField = "zipcode";
                    ddlPinCode.DataBind();

                    Areaqry = "Select Id as Id,Area as Name from [ZipCode] where IsActive = 1  order by Id";
                    dtArea = dbc.GetDataTable(Areaqry);
                    ddlArea.DataSource = dtArea;
                    ddlArea.DataTextField = "Name";
                    ddlArea.DataValueField = "Id";
                    ddlArea.DataBind();


                    DataTable dtAreaQuery = dbc.GetDataTable("SELECT DeliveryDetailID,PinCodeID,LocationId, Z.Location, A.Area,DD.AreaId FROM DeliveryDetail DD INNER JOIN Zipcode Z ON DD.LocationId = Z.Id INNER JOIN tblArea A ON A.Id = DD.AreaId WHERE DeliveryID = " + id);
                    if (dtAreaQuery.Rows.Count > 0)
                    {
                        string societyqry = "Select Id,Area from tblArea Where Location = '" + dtAreaQuery.Rows[0]["Location"].ToString() + "' order by Id";
                        DataTable dtSociety = dbc.GetDataTable(societyqry);
                        chklstArea.DataSource = dtSociety;
                        chklstArea.DataTextField = "Area";
                        chklstArea.DataValueField = "Id";
                        chklstArea.DataBind();

                        ddlArea.Items.FindByText(dtAreaQuery.Rows[0]["Location"].ToString()).Selected = true;
                        ddlPinCode.Items.FindByText(dtAreaQuery.Rows[0]["PinCodeID"].ToString()).Selected = true;
                        ddlArea.Enabled = false;
                        ddlPinCode.Enabled = false;
                        chklstArea.Enabled = false;
                        List<ListItem> selectedArea = new List<ListItem>();
                        foreach (DataRow row in dtAreaQuery.Rows)
                        {
                            selectedArea.Add(new ListItem(row["Area"].ToString(), row["AreaId"].ToString()));
                        }

                        foreach (ListItem item in chklstArea.Items)
                        {
                            foreach (ListItem itemArea in selectedArea)
                            {
                                if (item.Text == itemArea.Text)
                                {
                                    item.Selected = true;
                                }
                            }
                        }
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
        string PinCodeqry = "Select Area,zipcode from Zipcode Where State = '" + ddlState.SelectedItem.Text + "' and District = '" + ddlCity.SelectedItem.Text + "' order by zipcode";
        DataTable dtPincode = dbc.GetDataTable(PinCodeqry);
        ddlPinCode.DataSource = dtPincode;
        ddlPinCode.DataTextField = "zipcode";
        ddlPinCode.DataValueField = "Area";
        ddlPinCode.DataBind();
    }

    protected void OnLocationSelectedIndexChanged(object sender, EventArgs e)
    {
        string Areaqry = "Select Id,Area from tblArea Where Location = '" + ddlArea.SelectedItem.Text + "' order by Id";
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
            if (BtnSave.Text.Equals("Update"))
            {
                string id = Request.QueryString["id"].ToString();
                string query = " UPDATE [DeliveryMaster] SET [DeliveryIncharge]=@1,[Contact]=@2,[EmalID]=@3,[StateId]=@4,[CityId]=@5,[Comments]=@6, " +
                               " [IsActive] = @7,[CreatedOn] = @8,[CreatedBy] = @9 WHERE DeliveryId = " + id;
                int v1 = dbc.ExecuteQueryWithParams(query, para1);
                if (v1 > 0)
                {
                    string updateUser = " UPDATE  [Users] SET [UserName]='" + txtUserName.Text.ToString() + "',[Password]='" + txtPassword.Text.ToString() + "'," +
                                        " [Name]='" + txtUserName.Text.ToString() + "',[UserType]=3,[DeliveryID]=" + id + ",[Mobile] = '" + txtContact.Text.ToString() + "'" +
                                        " WHERE DeliveryId = " + id;
                    int userVAL = dbc.ExecuteQuery(updateUser);
                    sweetMessage("", "Delivery Updated Successfully", "success");
                    Response.Redirect("Delivery.aspx");
                }
                }
            else
            {
                

                string query1 = "INSERT INTO [DeliveryMaster] ([DeliveryIncharge],[Contact],[EmalID],[StateId],[CityId],[Comments],[IsActive],[CreatedOn],[CreatedBy]) VALUES (@1,@2,@3,@4,@5,@6,@7,@8,@9); SELECT SCOPE_IDENTITY();";
                int VAL = dbc.ExecuteQueryWithParamsId(query1, para1);

                if (VAL > 0)
                {
                    int areaId = Convert.ToInt32(ddlArea.SelectedValue);
                    string pincode = string.Empty;
                    string pincodeQry = " SELECT Top 1 zipCode FROM ZipCode WHERE Id = " + areaId;
                    DataTable dtPincode = dbc.GetDataTable(pincodeQry);
                    if (dtPincode.Rows.Count > 0)
                    {
                        pincode = dtPincode.Rows[0]["zipcode"].ToString();
                    }
                    string InsertDetailqry = "";
                    List<ListItem> selectedArea = new List<ListItem>();
                    selectedArea = chklstArea.Items.Cast<ListItem>().Where(n => n.Selected).ToList();
                    foreach (ListItem item in selectedArea)
                    {
                        InsertDetailqry = "INSERT INTO [DeliveryDetail] ([DeliveryID],[PinCodeID],[AreaId],[BuildingId],[IsActive],[CreatedOn],[CreatedBy]) VALUES ('" + VAL + "','" + pincode + "'," + areaId + "," + item.Value + "," + IsActive + ",'" + dt + "'," + userId + ")";
                        int VAL1 = dbc.ExecuteQuery(InsertDetailqry);
                    }
                    if (Convert.ToInt32(VAL) > 0)
                    {
                        hdnDeliveryID.Value = VAL.ToString();
                        hdnContact.Value = txtContact.Text.ToString().Replace("'", "''");

                        string InsertUserqry = "INSERT INTO [Users] ([UserName],[Password],[Name],[UserType],[DeliveryID],[Mobile]) VALUES ('" + txtUserName.Text.ToString().Replace("'", "''") + "','" + txtPassword.Text.ToString().Replace("'", "''") + "','" + txtUserName.Text.ToString().Replace("'", "''") + "',3," + VAL.ToString() + "," + txtContact.Text.ToString() + ")";
                        int userVAL = dbc.ExecuteQuery(InsertUserqry);
                       
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
        }
        catch (Exception E)
        {
            sweetMessage("", "Please Try Again!!", "warning");
        }
    }

    protected void OnZipCodeSelectedIndexChanged(object sender, EventArgs e)
    {

        string Locationqry = "Select Id as Id,Area as Name from [ZipCode] where IsActive = 1 AND zipcode = '" + ddlPinCode.SelectedItem.Text + "' order by Id";
        DataTable dtLocation = dbc.GetDataTable(Locationqry);
        ddlArea.DataSource = dtLocation;
        ddlArea.DataTextField = "Name";
        ddlArea.DataValueField = "Id";
        ddlArea.DataBind();

        string Areaqry = "Select Id,Area from tblArea Where Location = '" + ddlArea.SelectedItem.Text + "' order by Id";
        DataTable dtArea = dbc.GetDataTable(Areaqry);
        chklstArea.DataSource = dtArea;
        chklstArea.DataTextField = "Area";
        chklstArea.DataValueField = "Id";
        chklstArea.DataBind();

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