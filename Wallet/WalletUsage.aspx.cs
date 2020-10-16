using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Wallet_WalletUsage : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    string id = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {

                DataTable dt1;
                dt1 = dbc.GetDataTable("SELECT [per_type],[per_amount],[min_order_amount] FROM [dbo].[tblWalletUsageMaster] where ISNULL(is_deleted,0)=0 ");
                if (dt1.Rows.Count > 0)
                {
                    ddlgrpType.SelectedValue = dt1.Rows[0]["per_type"].ToString();
                    txtgrpTypeValue.Text = dt1.Rows[0]["per_amount"].ToString();
                    txtgrpMinOrderAmt.Text = dt1.Rows[0]["min_order_amount"].ToString();
                }
        }
    }

    protected void BtnSave_Click(object sender, EventArgs e)
    {
        try
        {
            bool lvalid = true;
            string id1 = Request.QueryString["Id"];
            if (lvalid)
            {
                string userId = Request.Cookies["TUser"]["Id"].ToString();
                string type = ddlgrpType.SelectedValue.ToString();
                string perValue = txtgrpTypeValue.Text.ToString();
                string minOrderAmt = txtgrpMinOrderAmt.Text.ToString();
                DateTime dtCreatedon = DateTime.Now;
                string countqry = " SELECT COUNT(wallet_usage_id) AS walletCount FROM tblWalletUsageMaster ";
                DataTable dtcountlist = dbc.GetDataTable(countqry);
                if (dtcountlist.Rows[0]["walletCount"].ToString() == "0")
                {
                    string[] para1 = {
                                     type,
                                     perValue,
                                     minOrderAmt,
                                     "1",
                                    dtCreatedon.ToString(),
                                    userId,
                                    "0"
                };
                    string query = "INSERT INTO [dbo].[tblWalletUsageMaster] ([per_type],[per_amount],[min_order_amount],[is_active],[created_date],[created_by],[is_deleted]) " +
                                    " VALUES (@1,@2,@3,@4,@5,@6,@7)  SELECT SCOPE_IDENTITY(); ";
                    int VAL = dbc.ExecuteQueryWithParamsId(query, para1);

                    if (VAL > 0)
                    {
                        sweetMessage("", "Wallet Usage Inserted Successfully", "success");
                        Response.Redirect("WalletUsage.aspx");
                    }
                    else
                    {
                        sweetMessage("", "Please Try Again!!", "warning");
                    }
                }
                else
                {
                    string query = "UPDATE [dbo].[tblWalletUsageMaster]  SET [per_type]='" + type + "',[per_amount]=" + perValue + ",[min_order_amount]=" + minOrderAmt + ",[is_active]=1,[modified_date]='" + dtCreatedon + "',[modified_by]=" + userId;
                    int VAL = dbc.ExecuteQuery(query);

                    if (VAL > 0)
                    {
                        sweetMessage("", "Wallet Usage Updated Successfully", "success");
                        Response.Redirect("WalletUsage.aspx");
                    }
                    else
                    {
                        sweetMessage("", "Please Try Again!!", "warning");
                    }
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
            sb.Append("window.location.href = 'WalletList.aspx'");
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