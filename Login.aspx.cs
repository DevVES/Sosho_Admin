using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["Id"] != null && !Request.QueryString["Id"].ToString().Equals(""))
                {
                    try
                    {
                        dbConnection dbcon = new dbConnection();
                        string WorkshopId = HttpContext.Current.Request.Cookies["TUser"]["WorkshopId"];
                        string UserId = HttpContext.Current.Request.Cookies["TUser"]["Id"];
                        UserActivity objUserAct = new UserActivity();
                        objUserAct.InsertUserActivity(HttpContext.Current.Request.Cookies["TUser"]["UserName"] + " Loged Out from system on : " + dbcon.getindiantime().ToString("MM-dd-yyyy HH:mm:ss tt"), UserId, WorkshopId, "", "Common", "LogedOut");
                    }
                    catch (Exception E) { }
                    HttpCookie aCookie;
                    string cookieName;
                    int limit = Request.Cookies.Count;
                    for (int i = 0; i < limit; i++)
                    {
                        cookieName = Request.Cookies[i].Name;
                        aCookie = new HttpCookie(cookieName);
                        aCookie.Expires = DateTime.Now.AddHours(24);  // make it expire yesterday
                        Response.Cookies.Add(aCookie); // overwrite it                    
                    }
                }
                if (Request.Cookies.AllKeys.Contains("TUser") && Session["KeepAlive"] != null)
                {
                    int userId = 0;
                    int.TryParse(Request.Cookies["TUser"]["Id"], out userId);
                    Response.Redirect("Home.aspx");
                }
                if (Request.Cookies.AllKeys.Contains("TUser") && Session["KeepAlive"] == null)
                {
                    try
                    {
                       txtId.Text= Request.Cookies["TUser"]["UserName"].ToString();
                    }
                    catch (Exception E) { }
                }
            }
        }
        catch (Exception ex)
        { }
        
        //SELECT [Id],[UserName],[Password] FROM [dbo].[Users] where [UserName]='' And [Password]=''
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        WebApplication1.dbConnection bdc = new WebApplication1.dbConnection();
        String Str = "SELECT [Name],[Id],[UserName],[Password],[IsAdmin],[UserType],[VendorID],[Deleted],WorkshopId,isnull(JurisdictionID,0) as JurisdictionID FROM [dbo].[Users] where [UserName]='" + txtId.Text.Replace("'", "''") + "' And [Password]='" + txtpass.Text.Replace("'", "''") + "' And Deleted = 0";
        DataTable st = bdc.GetDataTable(Str);
        if (st.Rows.Count > 0)
        {
            Session["KeepAlive"] = true;
            if (!Response.Cookies.AllKeys.Contains("TUser"))
            {
                HttpCookie aCookie = new HttpCookie("TUser");
                aCookie["Id"] = st.Rows[0]["Id"].ToString();
                aCookie["UserName"] = st.Rows[0]["Name"].ToString();
               // aCookie["mobile_number"] = st.Rows[0]["mobile_number"].ToString();
                aCookie["VendorId"] = st.Rows[0]["VendorID"].ToString();
                aCookie["WorkshopId"] = st.Rows[0]["WorkshopId"].ToString();
                aCookie["IsAdmin"] = st.Rows[0]["IsAdmin"].ToString();
                aCookie["JurisdictionID"] = st.Rows[0]["JurisdictionID"].ToString();
                aCookie["UserType"] = st.Rows[0]["UserType"].ToString();
                aCookie.Expires = DateTime.Now.AddHours(24);

                Response.Cookies.Add(aCookie);
                
                int userId = 0;
                int.TryParse(Request.Cookies["TUser"]["Id"], out userId);
                try
                {
                    dbConnection dbcon = new dbConnection();
                    int workshopId = 0;
                    int.TryParse(Request.Cookies["TUser"]["WorkshopId"], out workshopId);
                    string StrPages = "";
                    DataTable dt = dbcon.GetDataTable("SELECT Pages.PageUrl FROM Role INNER JOIN User_Role_Mapping ON Role.Id = User_Role_Mapping.RoleId INNER JOIN Users ON User_Role_Mapping.UserId = Users.Id INNER JOIN Role_Page_Mapping ON Role.Id = Role_Page_Mapping.RoleId INNER JOIN Pages ON Role_Page_Mapping.PageId = Pages.Id where Users.Id=" + userId + " and Users.WorkshopId=" + workshopId);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        StrPages += dt.Rows[i][0].ToString();
                    }
                    Application[workshopId + "-pages-" + userId] = StrPages;
                    try
                    {
                       // string WorkshopId = HttpContext.Current.Request.Cookies["TUser"]["WorkshopId"];
                        //string UserId = HttpContext.Current.Request.Cookies["TUser"]["Id"];
                        UserActivity objUserAct = new UserActivity();
                        objUserAct.InsertUserActivity(st.Rows[0]["UserName"].ToString() + " Loged In to system on : " + dbcon.getindiantime().ToString("MM-dd-yyyy HH:mm:ss tt"), userId.ToString(), workshopId.ToString(), "", "Common", "LogedIn");
                    }
                    catch (Exception E) { }
                }
                catch (Exception E)
                { }
                if (Session["URL"] != null)
                   {
                        string str = Session["URL"].ToString();
                        Session["URL"] = null;
                        Response.Redirect(str);
                   }
                   Response.Redirect("Home.aspx");
            }
            else
            {
                var aCookie = Response.Cookies.Get("TUser");
                aCookie.Value = st.Rows[0]["Id"].ToString();
                aCookie.Expires = DateTime.Now.AddHours(24);
                // Response.Cookies.Add(aCookie);
            }
                Literal1.Text = "";
            }
            else
            {
                Literal1.Text = "<p class='login-box-msg'>Incorrect username or password</p>";
            }
        }
    
}
