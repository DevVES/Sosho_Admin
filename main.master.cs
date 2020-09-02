using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WebApplication1;
public partial class main : System.Web.UI.MasterPage
{
    dbConnection dbc = new dbConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
        //clsDataSourse checkInvoice = new clsDataSourse();
        //checkInvoice.GetGstInvoiceNumber();
        lblCopyrightYear.Text = (DateTime.Now.Year - 1).ToString() + " - " + DateTime.Now.Year.ToString();

        if (!Request.Cookies.AllKeys.Contains("TUser"))
        {

            string Url = Request.Url.ToString();
            Session["URL"] = Url;
            Response.Redirect("~/login.aspx");

        }
        else
        {
            int userId = 0;
            int.TryParse(Request.Cookies["TUser"]["Id"], out userId);
            //if (userId != 1)
            //    if (Session["KeepAlive"] == null)
            //    {
            //        string Url = Request.Url.ToString();
            //        Session["URL"] = Url;
            //        Response.Redirect("~/login.aspx");
            //    }
            int workshopId = 0;
            int.TryParse(Request.Cookies["TUser"]["WorkshopId"], out workshopId);
            try
            {
                dbConnection dbcon = new dbConnection();

                if (Application[workshopId + "-pages-" + userId] == null)
                {
                    string StrPages = "";
                    DataTable dt = dbcon.GetDataTable("SELECT Pages.PageUrl FROM Role INNER JOIN User_Role_Mapping ON Role.Id = User_Role_Mapping.RoleId INNER JOIN Users ON User_Role_Mapping.UserId = Users.Id INNER JOIN Role_Page_Mapping ON Role.Id = Role_Page_Mapping.RoleId INNER JOIN Pages ON Role_Page_Mapping.PageId = Pages.Id where Users.Id=" + userId + " and Users.WorkshopId=" + workshopId);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        StrPages += dt.Rows[i][0].ToString();
                    }
                    Application[workshopId + "-pages-" + userId] = StrPages;
                }
            }
            catch (Exception E)
            { }
            Label1.Text = Request.Cookies["TUser"]["UserName"];
            Label2.Text = Request.Cookies["TUser"]["UserName"];
            hdnmainIsAdmin.Value = Request.Cookies["TUser"]["IsAdmin"];
            //403
            try
            {
                if (Request.Url.ToString().ToLower().Contains("chatkaro"))
                    return;
                if (Request.Url.ToString().ToLower().Contains("jsplum"))
                    return;
                //if (!Request.Url.ToString().Contains("403.aspx"))
                //{
                if (Application[workshopId + "-pages-" + userId] != null)
                {
                    string strpath = "";
                    string url = Request.Url.ToString();
                    string path = (url.Contains("?") ? url.Substring(0, url.IndexOf("?")) : url);
                    string[] strp = path.Split('/');
                    if (strp.Length > 0)
                    {
                        for (int j = 3; j < strp.Length; j++)
                        {
                            strpath += "/" + strp[j];
                        }
                        string strVal = Application[workshopId + "-pages-" + userId].ToString();
                        if (Application[workshopId + "-pages-" + userId].ToString().ToLower().Contains(strpath.ToLower()))
                        {
                        }
                        else
                        {
                            //if (userId != 1)
                            //    Response.Redirect("/403.aspx");
                        }
                    }
                }
                else
                {
                    if (userId != 1)
                        Response.Redirect("/403.aspx");
                }

            }
            catch (Exception E) { }
        }
    }
}
