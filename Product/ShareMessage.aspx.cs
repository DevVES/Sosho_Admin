using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Product_ShareMessage : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
        string qry = "SELECT [Id],[Key],[Value] FROM [tblShareSocialMessage]";
        DataTable dt = dbc.GetDataTable(qry);

        if (dt != null && dt.Rows.Count > 0)
        {
            HiddenField1.Value = dt.Rows[0]["Key"].ToString();
            TextBox1.Text = dt.Rows[0]["Value"].ToString();

            HiddenField2.Value = dt.Rows[1]["Key"].ToString();
            TextBox2.Text = dt.Rows[1]["Value"].ToString();
        }
    }

    [WebMethod]
    public static string HomepageMsg(string Msg, string Key)
    {
        dbConnection dbc = new dbConnection();
        try
        {
            string responce = "";
            string Message = Msg;
            string KeyVal = Key;
            if (Key != "" && Key != null)
            {
                string Update = "UPDATE [dbo].[tblShareSocialMessage] SET [Value] = '" + Message + "' WHERE [Key] ='" + KeyVal + "'";
                int res = dbc.ExecuteQuery(Update);
                if (res > 0)
                {
                    return responce = "Success";
                }
                else
                {
                    return responce = "Fail";
                }
            }
            return responce = "Fail";
        }
        catch (Exception)
        {
            return null;
        }
    }
}