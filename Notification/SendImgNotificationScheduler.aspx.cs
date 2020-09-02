using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;
using System.Data;
using System.Drawing;
using System.Web.Hosting;
using System.Web.Script.Serialization;
using System.Web.Script.Services;
using System.Web.Services;
using System.Net;
using System.Data.SqlClient;
using System.IO;

public partial class App_Management_SendImgNotificationScheduler : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            try
            {
                txtSDate.Value = dbc.getindiantime().ToString("dd/MMM/yyyy");
                //bindsociety();
                //txtpwd.Text = "";
                //txtimg.Text = " ";

                ddlProduct = dbc.FillCombo(ddlProduct, "Select Id , Name from Product", "Name", "Id");

                if (rdbsendall.Checked == true)
                {
                    txtmob.Enabled = false;
                }
                else
                {
                    txtmob.Enabled = true;
                }
                
                //gvbind();


                // txtexpdate.Value = dbc.getindiantime().AddDays(1).ToString("dd/MMM/yyyy");
            }
            catch (Exception ex)
            {
            }
        }

    }

   
    protected void rdbsendall_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (rdbsendall.Checked == true)
            {
                txtmob.Text = "";
                txtmsg.Text = "";
                txtmob.Enabled = false;
            }
            else
            {
                txtmob.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            ltrerr.Text = ex.Message + "Stack: " + ex.StackTrace;
        }
    }
    protected void rdbsendsel_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (rdbsendsel.Checked == true)
            {
                txtmob.Text = "";
                txtmob.Enabled = true;
            }
            else
            {
                txtmob.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            ltrerr.Text = ex.Message + "Stack: " + ex.StackTrace;
        }
    }
    protected void btnsend_Click(object sender, EventArgs e)
    {
        // txtimg.Text = txtimg.Text.Trim();
        try
        {
            string[] validFileTypes = { "png", "jpg", "jpeg" };
            Stream fs = FileUpload2.PostedFile.InputStream;
            BinaryReader br = new BinaryReader(fs);
            Byte[] image = br.ReadBytes((Int32)fs.Length);

            string exten = System.IO.Path.GetExtension(FileUpload2.FileName);
            string name = System.IO.Path.GetFileName(FileUpload2.FileName);


            string ext = System.IO.Path.GetExtension(FileUpload2.FileName);

            string fileName = "";
            bool isValidFile = false;
            string imgname = "";
            if (FileUpload2.HasFile)
            {

                for (int i = 0; i < validFileTypes.Length; i++)
                {
                    if (ext == "." + validFileTypes[i])
                    {
                        isValidFile = true;
                        break;
                    }
                }

                if (!isValidFile)
                {
                    ltrerr.Text = "Invalid File. Please upload valid Image file (.png, .jpg, .jpeg).";
                    return;
                }


                ext = ext.Replace(".", "image/");

                imgname = DateTime.Now.Ticks + exten;
                string path = Server.MapPath("~/content/images/thumbs") + "\\" + imgname;

            

                FileUpload2.SaveAs(path);
                if (Request.Url.ToString().Contains("localhost"))
                {
                    imgname = "http://" + Request.Url.Authority + "/content/images/thumbs/" + imgname;
                }
                else
                {
                    imgname = "http://admin.salebhai.in/content/images/thumbs/" + imgname;
                }
            }
            fileName = imgname;











            string uid = Request.Cookies["TUser"]["Id"];
            string type = "";
            string NotificationType = ddlcategory.SelectedValue;
            bool isoffer = false;

            //if (ddlcategory.SelectedValue == "0")
            //{
            //    if (chkoffer.Checked == false)
            //    {
            //        isoffer = false;

            //            type = "0";
            //            NotificationType = "0";

            //    }
            //    else
            //    {
            //        isoffer = true;

            //            type = "11";
            //            NotificationType = "11";

            //    }
            //}
            //else
            //{

            //        type = "14";
            //        NotificationType = "14";

            //}


            ltrerr.Text = "";
            int sendtype = 0; ;
            string NotificationTo = "";
            string SendTo = "";
            //if (txtpwd.Text != "" && txtpwd.Text.ToLower() == "secret")
            //{
            string mobile = "";

            //if (rdbhotel.Checked == true)
            //{
            //    NotificationTo = "1"; // Hotel
            //}
            //else if (rdbretail.Checked == true)
            //{
            //    NotificationTo = "0"; // Retail
            //}
            //else
            //{
            NotificationTo = "2"; // Both
            //}


            if (rdbsendall.Checked == true)
            {
                sendtype = 0;///// have to set 0 afterwards


                SendTo = "0"; // All
            }
            else
            {
                sendtype = 1;
                SendTo = "1"; // Selected

                if (txtmsg.Text != "" && txtmob.Text != "")
                {
                    txtmob.Text = txtmob.Text.Replace("\r\n", ",");
                    txtmob.Text = txtmob.Text.Replace(",,,", ",");
                    txtmob.Text = txtmob.Text.Replace(",,", ",");
                    mobile = txtmob.Text;
                    mobile = "'" + mobile.Replace(",", "','") + "'";
                }
                else
                {
                    ltrerr.Text = "Enter all values";
                    return;
                }
            }

            //Notification nf = new Notification();
            string category = "";
            string product = "";
            if (ddlcategory.SelectedValue == "0")
            {
                category = "";
            }
            else
            {
                category = ddlcategory.SelectedValue;
            }

            if (ddlProduct.SelectedValue == "0")
            {
                product = "";
            }
            else
            {
                product = ddlProduct.SelectedValue.ToString();
            }

            string ScheduleTime = txtSDate.Value + " " + txttime.Value;
            //   string exptime = txtexpdate.Value + " " + txtexptime.Value;

            int sent = StoreNotificationScheduler(NotificationTo, SendTo, category, product, txtmsg.Text.Trim(), mobile, NotificationType, ScheduleTime, ScheduleTime, fileName);// nf.SendNotification(type, uid, txtimg.Text, txtmsg.Text, isoffer, category, mobile, product, sendtype);
            if (sent > 0)
            {
                ltrerr.Text = "Notification Scheduler Set Successfully.";
                txtmob.Text = "";
                txtmsg.Text = "";
                // txtpwd.Text = "";
                // txtimg.Text = " ";

                //gvbind();
            }
            else
            {
                ltrerr.Text = "Notification Scheduler NOT Set.";
                //txtpwd.Text = "";
                // txtimg.Text = " ";
            }

            //}
            //else
            //{
            //    txtpwd.Text = "";
            //    txtimg.Text = " ";
            //    ltrerr.Text = "Invalid Password";
            //}
        }
        catch (Exception ex)
        {
            // txtimg.Text = " ";
            //txtpwd.Text = "";
            ltrerr.Text = ex.Message + "Stack: " + ex.StackTrace;
        }
    }

    public int StoreNotificationScheduler(string NotificationTo, string SendTo, string Category, string Product, string Message, string MobileNumber, string NotificationType, string ScheduleTime, string exptime, string ImageUrl)
    {
        try
        {
            //string NotificationType = "15";

            int userId = 0;
            int.TryParse(Request.Cookies["TUser"]["Id"], out userId);

            string ScheduleQuery = "INSERT INTO [dbo].[Sosho_Notification_Schedule] ([NotificationTo],[SendTo],[CategoryId],[ProductId],[ImageUrl],[Message],[NotificationType],[IsSend],[DOC],[DOM],[ExpiredTime],[SendBy],[ScheduleTime],DisplayExpiredTime) VALUES(@1,@2,@3,@4,@5,@6,@7,@8,CONVERT(DATETIME, '" + dbc.getDOCMtime() + "', 102),CONVERT(DATETIME, '" + dbc.getDOCMtime() + "', 102),CONVERT(DATETIME, '" + dbc.getindiantime().AddHours(24).ToString("yyyy-MM-dd HH:mm:ss") + "', 102),@9,'" + ScheduleTime + "','" + exptime + "')";
            string[] parm = { NotificationTo, SendTo, "0", Product, ImageUrl, Message, Category, "0", userId.ToString() };
            int rest = dbc.ExecuteQueryWithParams(ScheduleQuery, parm);
            if (rest > 0)
            {
                string qs2 = "SELECT IDENT_CURRENT('Sosho_Notification_Schedule')";

                object objbid = dbc.ExecuteSQLScaler(qs2);

                int scheduleId = 0;
                int.TryParse(objbid.ToString(), out scheduleId);


                if (scheduleId > 0)
                {
                    if (SendTo.Equals("1"))
                    {
                        string MobileQuery = "select *,iif(IsIOS = 1,1,0) as DeviceType from (select isnull(deviceid,'') as deviceid, isnull(fcmregistrationid,'') as fcmregistrationid, mobilenumber, [doc], dom, Id, IsIOS, (select top 1 isnull(Id,0) from Customer where Mobile = mobilenumber) as CustomerId from [dbo].[DevicedetailsCustomer] where mobilenumber in (" + MobileNumber + ")) as finalData where CustomerId <> 0 AND deviceid <> '' AND fcmregistrationid <> '' Order by mobilenumber";
                        DataTable dtMob = dbc.GetDataTable(MobileQuery);

                        if (dtMob.Rows.Count > 0)
                        {
                            for (int i = 0; i < dtMob.Rows.Count; i++)
                            {

                                string InertScheduleDetail = "INSERT INTO [dbo].[Sosho_Notification_Schedule_Detail] ([ScheduleID],[DeviceId],[FCMRegistrationId],[Message],[ResponseId],[Type],[DOC],[Mobile],[isSend],[isSuccess],[delivered],[opentime],[DeviceType],[CustomerId],[ExpiredTime]) VALUES (@1,@2,@3,@4,'',@5,CONVERT(DATETIME, '" + dbc.getDOCMtime() + "', 102),@6,0,0,'','',@7,@8,@9)";

                                string[] parmdata = { scheduleId.ToString(), dtMob.Rows[i]["deviceid"].ToString(), dtMob.Rows[i]["fcmregistrationid"].ToString(), Message, NotificationType, dtMob.Rows[i]["mobilenumber"].ToString(), dtMob.Rows[i]["DeviceType"].ToString(), dtMob.Rows[i]["CustomerId"].ToString(), exptime };

                                int restdet = dbc.ExecuteQueryWithParams(InertScheduleDetail, parmdata);



                            }
                            return 1;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                    else
                    {
                        return 1; // Send To All Mobile Details Store Via Scheduler
                    }
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return 0;
            }
        }
        catch (Exception ex)
        {
            return -1;
        }
    }

    

    protected void gvoffer_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName.ToString().Equals("endoffer"))
            {
                string[] id = { e.CommandArgument.ToString() };
                DataTable dt = (DataTable)ViewState["table"];
                int i = dbc.ExecuteQueryWithParams("Update Sosho_Offer_Notification set EndDate=DATEADD(MINUTE,330,GETUTCDATE()),OfferStatus=0 where Id=@1", id);
                if (i > 0)
                {
                    //  gvbind();
                    ltrerr.Text = "Offer Closed";
                }
            }
        }
        catch (Exception ex)
        {
            ltrerr.Text = "";
        }
    }

    
    private void sweetMessage(string message, string time, string type)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("<script type = 'text/javascript'>");
        sb.Append("alert('" + message + "');");
        sb.Append("</script>");
        ClientScript.RegisterClientScriptBlock(this.GetType(), "Message", sb.ToString());
    }
}