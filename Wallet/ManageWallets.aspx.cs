using System;
using System.Data;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Wallet_ManageWallets : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    string id = "";
    static DataTable dtcustomerlist;
    protected void Page_Load(object sender, EventArgs e)
    {
        dtcustomerlist = new DataTable("customerlist");
        dtcustomerlist.Columns.Add("Apply", typeof(Boolean));
        dtcustomerlist.Columns.Add("Id", typeof(Int32));
        dtcustomerlist.Columns.Add("Mobile", typeof(string));
        dtcustomerlist.Columns.Add("FirstName", typeof(string));
        dtcustomerlist.Columns.Add("LastName", typeof(string));
        dtcustomerlist.Columns.Add("Email", typeof(string));
        dtcustomerlist.Columns.Add("Sex", typeof(string));
        dtcustomerlist.Columns.Add("Address", typeof(string));
        dtcustomerlist.Columns.Add("Pincode", typeof(string));

        if (!IsPostBack)
        {
            id = Request.QueryString["Id"];
            txtdt.Text = dbc.getindiantime().ToString("dd/MMM/yyyy");
            txtdt1.Text = dbc.getindiantime().ToString("dd/MMM/yyyy");
            chkisactive.Checked = false;
            CustomerDataList();
            if (id != null && !id.Equals(""))
            {
                BtnSave.Text = "Update";
                
                DataTable dtcustomertbl = dbc.GetDataTable("SELECT Id, Mobile,FirstName,LastName,Email,Sex,Address,PinCode  FROM [dbo].[Customer]");
                if (dtcustomertbl.Rows.Count > 0)
                {
                    ViewState["dt"] = dtcustomertbl;
                    gvcustomerlist.DataSource = dtcustomertbl;
                    gvcustomerlist.DataBind();
                }

                DataTable dt1;
                dt1 = dbc.GetDataTable("SELECT  [campaign_name],[wallet_amount],[coupon_code],[is_applicable_first_order],[is_apply_all_customer],[per_type],[per_amount],[min_order_amount],[start_date],[end_date],[is_active] FROM [dbo].[WalletMaster] where ISNULL(is_deleted,0)=0  and wallet_id=" + id);
                if (dt1.Rows.Count > 0)
                {
                    txtcname.Text = dt1.Rows[0]["campaign_name"].ToString();
                    txtcouponcode.Text = dt1.Rows[0]["coupon_code"].ToString();
                    string IsApplicable = dt1.Rows[0]["is_applicable_first_order"].ToString();
                    if (IsApplicable == "True")
                    {
                        chkIsFirstOrderApplicable.Checked = true;
                    }
                    else
                    {
                        chkIsFirstOrderApplicable.Checked = false;
                    }
                  
                    string IsApplyAllCust = dt1.Rows[0]["is_apply_all_customer"].ToString();
                    if (IsApplyAllCust == "True")
                    {
                        chkApplyAllCustomer.Checked = true;
                        customertab.Visible = false;
                    }
                    else
                    {
                        chkApplyAllCustomer.Checked = false;
                        customertab.Visible = true;
                    }
                    ddlgrpType.SelectedValue = dt1.Rows[0]["per_type"].ToString();
                    txtgrpTypeValue.Text = dt1.Rows[0]["per_amount"].ToString();
                    txtgrpMinOrderAmt.Text = dt1.Rows[0]["min_order_amount"].ToString();
                    txtWalletAmount.Text = dt1.Rows[0]["wallet_amount"].ToString();
                    string strtdate = dt1.Rows[0]["start_date"].ToString();
                    string enddate = dt1.Rows[0]["end_date"].ToString();

                    DateTime oDate = Convert.ToDateTime(strtdate);
                    string datetime = oDate.ToString("dd/MMM/yyyy hh:mm tt");
                    if (!String.IsNullOrEmpty(datetime))
                    {
                        string[] dt = datetime.Split(' ');
                        if (dt.Length == 3)
                        {
                            txtdt.Text = dt[0].ToString();
                            txttime.Text = dt[1].ToString() + " " + dt[2].ToString();
                        }
                    }

                    DateTime oDate1 = Convert.ToDateTime(enddate);
                    string datetime1 = oDate1.ToString("dd/MMM/yyyy hh:mm tt");
                    if (!String.IsNullOrEmpty(datetime1))
                    {
                        string[] dt11 = datetime1.Split(' ');
                        if (dt11.Length == 3)
                        {
                            txtdt1.Text = dt11[0].ToString();
                            txttime1.Text = dt11[1].ToString() + " " + dt11[2].ToString();
                        }
                    }

                    string IsActive = dt1.Rows[0]["is_active"].ToString();
                    if (IsActive == "True")
                    {
                        chkisactive.Checked = true;
                    }
                    else
                    {
                        chkisactive.Checked = false;
                    }
                }
            }

        }
    }

    private void CustomerDataList()
    {
        String from = txtdt.Text.ToString();
        String to = txtdt1.Text.ToString();

        String[] StrPart = from.Split('/');

        String[] StrPart1 = to.Split('/');

        string IsAdmin = Request.Cookies["TUser"]["IsAdmin"].ToString();

        string query = "SELECT [Id],[Mobile],[FirstName],[LastName],[Email],[Sex],[Address],[CityId],[StateId],[Pincode] FROM [dbo].[Customer] ";
        query += " order by Id desc ";

        DataTable dtcustomerlist = dbc.GetDataTable(query);
        if (dtcustomerlist.Rows.Count > 0)
        {
            gvcustomerlist.DataSource = dtcustomerlist;
            gvcustomerlist.DataBind();
        }
    }

    protected void chkCustomer_Clicked(Object sender, EventArgs e)
    {
        bool isApplyAllCustomer = chkApplyAllCustomer.Checked;
        customertab.Visible = true;
        CustomerDataList();
        if (isApplyAllCustomer)
        {
            customertab.Visible = false;
        }
    }

    protected void BtnGenerate_Click(object sender, EventArgs e)
    {

        var chars = "0123456789";
        var stringChars = new char[6];
        var random = new Random();

        for (int i = 0; i < stringChars.Length; i++)
        {
            stringChars[i] = chars[random.Next(chars.Length)];
        }

        var finalString = new String(stringChars);
        txtcouponcode.Text = finalString;
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            string userId = Request.Cookies["TUser"]["Id"].ToString();
            string id1 = Request.QueryString["Id"];
            int IsActive = 0, IsFirstOrderApplicable = 0, IsApplyAllCustomer = 0;
            
            string startdate = txtdt.Text.ToString();
            string starttime = txttime.Text.ToString();
            string enddate = txtdt1.Text.ToString();
            string endtime = txttime1.Text.ToString();

            string FROM1 = startdate + " " + starttime;
            string TO1 = enddate + " " + endtime;
            string campaignname = txtcname.Text.ToString();
            string couponcode = txtcouponcode.Text.ToString();
            string type = ddlgrpType.SelectedValue.ToString();
            string perValue = txtgrpTypeValue.Text.ToString();
            string minOrderAmt = txtgrpMinOrderAmt.Text.ToString();
            string walletAmt = txtWalletAmount.Text.ToString();
            if (chkIsFirstOrderApplicable.Checked)
                IsFirstOrderApplicable = 1;

            if (chkApplyAllCustomer.Checked)
                IsApplyAllCustomer = 1;

            if (chkisactive.Checked)
                IsActive = 1;

            DateTime dtCreatedon = DateTime.Now;
            if (gvcustomerlist.Rows.Count > 0)
            {
                foreach (GridViewRow gvrow in gvcustomerlist.Rows)
                {
                    CheckBox chk = (CheckBox)gvrow.FindControl("Apply");
                    HiddenField hdncustomerid = (HiddenField)gvrow.FindControl("HiddenFieldCustomerId");
                    string custid = hdncustomerid.Value.ToString();
                    string mobileno = gvrow.Cells[1].Text;
                    string firstname = gvrow.Cells[2].Text;
                    string lastname = gvrow.Cells[3].Text;
                    string email = gvrow.Cells[4].Text;
                    string sex = gvrow.Cells[5].Text;
                    string address = gvrow.Cells[6].Text;
                    string pincode = gvrow.Cells[7].Text;
                    if (chk != null & chk.Checked)
                    {
                        dtcustomerlist.Rows.Add(
                            true,
                            custid
                            , mobileno
                            , firstname
                            , lastname
                            , email
                            , sex
                            , address
                            , pincode);

                    }
                }
            }
            if (BtnSave.Text.Equals("Update"))
            {
                string id = Request.QueryString["id"].ToString();
                string query = "UPDATE [dbo].[WalletMaster]  SET [campaign_name] = '" + campaignname + "',[wallet_amount]="+ walletAmt +",[coupon_code]='" + couponcode + "',[is_applicable_first_order]=" + IsFirstOrderApplicable + ",[is_apply_all_customer]=" + IsApplyAllCustomer + ",[per_type]='" + type + "',[per_amount]=" + perValue + ",[min_order_amount]=" + minOrderAmt + ",[start_date]='" + startdate + "',[end_date]='" + enddate + "',[is_active]=" + IsActive + ",[created_date]='" + dtCreatedon + "',[created_by]=" + userId + " where [wallet_id]=" + id;
                int VAL = dbc.ExecuteQuery(query);
                if (IsApplyAllCustomer == 1)
                {
                    string linkquerycount = " SELECT COUNT(Id) AS linkCount  FROM [dbo].[tblWalletCustomerLink] WHERE wallet_id= " + id + " AND customer_id = -1 AND is_active = 1";
                    DataTable dtcountlist = dbc.GetDataTable(linkquerycount);
                    if (dtcountlist.Rows[0]["linkCount"].ToString() == "0")
                    {
                        string customerlinkquery = "INSERT INTO [dbo].[tblWalletCustomerLink] ([wallet_id],[customer_id],[is_active],[created_date],[created_by]) VALUES (" + id + ",-1," + IsActive + ",'" + dtCreatedon.ToString() + "'," + userId + ")";
                        dbc.ExecuteQuery(customerlinkquery);
                    }
                    
                }
                else
                {
                    if (gvcustomerlist.Rows.Count > 0)
                    {
                        foreach (DataRow item in dtcustomerlist.Rows)
                        {
                            string linkquerycount = " SELECT COUNT(Id) AS linkCount  FROM [dbo].[tblWalletCustomerLink] WHERE wallet_id= " + id + " AND customer_id = " + item.ItemArray[1] + " AND is_active = 1";
                            DataTable dtcountlist = dbc.GetDataTable(linkquerycount);
                            if (dtcountlist.Rows[0]["linkCount"].ToString() == "0")
                            {
                                string customerlinkinsertquery = "INSERT INTO [dbo].[tblWalletCustomerLink] ([wallet_id],[customer_id],[is_active],[created_date],[created_by]) VALUES (" + id + "," + item.ItemArray[1] + "," + IsActive + ",'" + dtCreatedon.ToString() + "'," + userId + ")";
                                dbc.ExecuteQuery(customerlinkinsertquery);
                            }
                        }
                    }
                }
                if (VAL > 0)
                {
                    sweetMessage("", "Wallet Updated Successfully", "success");
                    Response.Redirect("WalletList.aspx");
                }
                else
                {
                    sweetMessage("", "Please Try Again!!", "warning");
                }

            }
            else
            {
                string[] para1 = { campaignname.Replace("'", "''"),
                    walletAmt.ToString(),
                                    couponcode.ToString().Replace("'", "''"),
                                     IsFirstOrderApplicable.ToString(),
                                    IsApplyAllCustomer.ToString(),
                                     type,
                                     perValue,
                                     minOrderAmt,
                                    startdate,
                                    enddate,
                                    IsActive.ToString(),
                                    dtCreatedon.ToString(),
                                    userId
                };
                string query = "INSERT INTO [dbo].[WalletMaster] ([campaign_name],[wallet_amount],[coupon_code],[is_applicable_first_order],[is_apply_all_customer],[per_type], " +
                                " [per_amount],[min_order_amount],[start_date],[end_date],[is_active],[created_date],[created_by]) " +
                                " VALUES (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13)  SELECT SCOPE_IDENTITY(); ";
                int VAL = dbc.ExecuteQueryWithParamsId(query, para1);
                if (IsApplyAllCustomer == 1)
                {
                    string customerlinkquery = "INSERT INTO [dbo].[tblWalletCustomerLink] ([wallet_id],[customer_id],[is_active],[created_date],[created_by]) VALUES (" + VAL + ",-1," + IsActive + ",'" + dtCreatedon.ToString() + "'," + userId + ")";
                    dbc.ExecuteQuery(customerlinkquery);
                }
                else
                {
                    if (gvcustomerlist.Rows.Count > 0)
                    {
                        foreach (DataRow item in dtcustomerlist.Rows)
                        {
                            string[] para2 = { VAL.ToString(),item.ItemArray[1].ToString(),IsActive.ToString(),dtCreatedon.ToString(), userId };
                            string customerlinkinsertquery = "INSERT INTO [dbo].[tblWalletCustomerLink] ([wallet_id],[customer_id],[is_active],[created_date],[created_by]) VALUES (@1,@2,@3,@4,@5) SELECT SCOPE_IDENTITY();";
                            int linkVAL = dbc.ExecuteQueryWithParamsId(customerlinkinsertquery, para2);

                            string[] para3 = { VAL.ToString(), item.ItemArray[1].ToString(), linkVAL.ToString(), startdate,campaignname,walletAmt.ToString(), "", "", 0.ToString(), walletAmt.ToString(), IsActive.ToString(), dtCreatedon.ToString(), userId };
                            //string customerwallethistoryQuery = "INSERT INTO [dbo].[tblWalletCustomerHistory] ([wallet_id],[customer_id],[wallet_link_id],[Cr_date],[Cr_description],[Cr_amount],[Dr_date],[Dr_description],[Dr_amount],[balance],[is_active],[created_date],[created_by]) VALUES (@1,@2,@3,@4,@5,@6,@7,@8,@9,@10,@11,@12,@13) SELECT SCOPE_IDENTITY();";
                            string customerwallethistoryQuery = "INSERT INTO [dbo].[tblWalletCustomerHistory] ([wallet_id],[customer_id],[wallet_link_id],[Cr_date],[Cr_description],[Cr_amount],[Dr_date],[Dr_description],[Dr_amount],[balance],[is_active],[created_date],[created_by]) VALUES ("+VAL.ToString()+", " +item.ItemArray[1].ToString()+","+ linkVAL.ToString()+", '"+startdate+"','"+campaignname+"',"+ walletAmt.ToString()+", '', '', 0, " + walletAmt.ToString() +","+ IsActive.ToString()+",'"+ dtCreatedon.ToString()+"',"+ userId +");";
                            dbc.ExecuteQuery(customerwallethistoryQuery);
                        }
                    }
                }
                if (VAL > 0)
                {
                    sweetMessage("", "Wallet Inserted Successfully", "success");
                    Response.Redirect("WalletList.aspx");
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

    public void BindgrpCustomerData()
    {
        if (dtcustomerlist.Rows.Count > 0)
        {
            gvcustomerlist.DataSource = dtcustomerlist;
        }
        else
        {
            gvcustomerlist.DataSource = ViewState["dt"];

        }
        gvcustomerlist.DataBind();
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

    protected void gvcustomerlist_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }
    protected void gvcustomerlist_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.TableSection = TableRowSection.TableHeader;
        }
    }
}