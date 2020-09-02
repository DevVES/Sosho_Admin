using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class AppManagement_NotificationList : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DataTable dttype11 = new DataTable();
            dttype11.Columns.Add("TypeId");
            dttype11.Columns.Add("Name");

            dttype11.Rows.Add("0", "Last 7 Days");
            dttype11.Rows.Add("1", "Last 15 Days");
            dttype11.Rows.Add("2","Current Month");
            dttype11.Rows.Add("3", "All");

            //ddlDateType.DataSource = dttype11;
            //ddlDateType.DataTextField = "Name";
            //ddlDateType.DataValueField = "TypeId";
            //ddlDateType.DataBind();
            //ddlDateType.SelectedIndex = 0;

            GetData();
        }
    }



    public void GetData()
    {
        try
        {
            dbConnection dbc = new dbConnection();

            string datewhere = "";
            string query = "SELECT [Sosho_Notification_Schedule].[Id],iif(len(ImageUrl)>5 ,ImageUrl,'') as View1,iif(len(ImageUrl)>5 ,'View','') as View2 ,iif([NotificationTo] = 0,'Retail','Hotel Or Both') as [NotificationTo] ,iif([SendTo]= 0, 'All','Selected') as [SendTo] ,(select Name from Product where Id = [ProductId]) as ProductName ,[ImageUrl] ,[Message] ,[NotificationType] ,iif([IsSend] = 1,'Done','Pending') as IsSend ,Convert(varchar(6),DOC,106)+' '+ Convert(varchar(5),DOC,108) as DateOfCreate ,Convert(varchar(6),DOM,106)+' '+ Convert(varchar(5),DOM,108) as DateOfMotific ,Convert(varchar(6),ExpiredTime,106)+' '+ Convert(varchar(5),ExpiredTime,108) as DateOfExpiredTime, Convert(varchar(6),scheduletime,106)+' '+ Convert(varchar(5),scheduletime,108) as DateOfScheduleTime ,(select count(*) from [Sosho_Notification_Schedule_Detail] where ScheduleID = [Sosho_Notification_Schedule].Id) as AllMobile ,(select count(*) from [Sosho_Notification_Schedule_Detail] where ScheduleID = [Sosho_Notification_Schedule].Id AND [Sosho_Notification_Schedule_Detail].IsSend = 0) as Pending ,(select count(*) from [Sosho_Notification_Schedule_Detail] where ScheduleID = [Sosho_Notification_Schedule].Id AND [Sosho_Notification_Schedule_Detail].IsSend = 1) as Processed ,(select count(*) from [Sosho_Notification_Schedule_Detail] where ScheduleID = [Sosho_Notification_Schedule].Id AND [Sosho_Notification_Schedule_Detail].IsSend = 1 AND isSuccess = 0) as Fail ,(select count(*) from [Sosho_Notification_Schedule_Detail] where ScheduleID = [Sosho_Notification_Schedule].Id AND [Sosho_Notification_Schedule_Detail].IsSend = 1 AND isSuccess = 1) as Success,isnull((Select UserName from Users where Id = [Sosho_Notification_Schedule].SendBy),'') as UserName FROM [dbo].[Sosho_Notification_Schedule] " + datewhere + " Order By [Id] desc";

            //string query = "SELECT [Sosho_Notification_Schedule].[Id],iif(len(ImageUrl)>5 ,ImageUrl,'') as View1,iif(len(ImageUrl)>5 ,'View','') as View2 ,iif([NotificationTo] = 0,'Retail','Hotel Or Both') as [NotificationTo] ,iif([SendTo]= 0, 'All','Selected') as [SendTo] ,(select Name from Category where id =  [Sosho_Notification_Schedule].CategoryId) as CategoryName ,(select Name from Product where Id = [ProductId]) as ProductName ,[ImageUrl] ,[Message] ,[NotificationType] ,iif([IsSend] = 1,'Done','Pending') as IsSend ,Convert(varchar(6),DOC,106)+' '+ Convert(varchar(5),DOC,108) as DateOfCreate ,Convert(varchar(6),DOM,106)+' '+ Convert(varchar(5),DOM,108) as DateOfMotific ,Convert(varchar(6),ExpiredTime,106)+' '+ Convert(varchar(5),ExpiredTime,108) as DateOfExpiredTime, Convert(varchar(6),scheduletime,106)+' '+ Convert(varchar(5),scheduletime,108) as DateOfScheduleTime ,(select count(*) from [Sosho_Notification_Schedule_Detail] where ScheduleID = [Sosho_Notification_Schedule].Id) as AllMobile ,(select count(*) from [Sosho_Notification_Schedule_Detail] where ScheduleID = [Sosho_Notification_Schedule].Id AND [Sosho_Notification_Schedule_Detail].IsSend = 0) as Pending ,(select count(*) from [Sosho_Notification_Schedule_Detail] where ScheduleID = [Sosho_Notification_Schedule].Id AND [Sosho_Notification_Schedule_Detail].IsSend = 1) as Processed ,(select count(*) from [Sosho_Notification_Schedule_Detail] where ScheduleID = [Sosho_Notification_Schedule].Id AND [Sosho_Notification_Schedule_Detail].IsSend = 1 AND isSuccess = 0) as Fail ,(select count(*) from [Sosho_Notification_Schedule_Detail] where ScheduleID = [Sosho_Notification_Schedule].Id AND [Sosho_Notification_Schedule_Detail].IsSend = 1 AND isSuccess = 1) as Success,isnull((Select UserName from Sosho_Users where Id = [Sosho_Notification_Schedule].SendBy),'') as UserName FROM [dbo].[Sosho_Notification_Schedule] " + datewhere + " Order By [Id] desc";

            DataTable dtData = dbc.GetDataTable(query);

            if (dtData.Rows.Count > 0)
            {
                grd.DataSource = dtData;
                grd.Caption = "Notification List: " + dtData.Rows.Count;
                grd.DataBind();
                grd.Visible = true;
            }
            else
            {
                grd.DataSource = null;
                grd.DataBind();
                grd.Visible = false;
            }

            string CallOnQuery = "Select top 1 *,Convert(varchar(6),DOC,106)+' '+ Convert(varchar(5),DOC,108) as DispDoc from Logs_Application where CustomerId = -12 AND LogDetailedMsg like '%Scheduler End ON%' order by id desc";
            DataTable dtcall = dbc.GetDataTable(CallOnQuery);
            if (dtcall != null && dtcall.Rows.Count > 0)
            {
                alitlastcall.Text = "Last Scheduler Called On " + dtcall.Rows[0]["DispDoc"].ToString();
            }
        }
        catch (Exception ex)
        {
            ltrErr.Text = "Error: " + ex.Message + ":::" + ex.StackTrace;
        }
    }

    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {

    }
    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }
    //protected void ddlDateType_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    GetData();
    //}
}