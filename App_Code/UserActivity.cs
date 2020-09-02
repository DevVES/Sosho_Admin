using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for UserActivity
/// </summary>
public class UserActivity
{
	public UserActivity()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public void InsertUserActivity(string Activity,string UserId,string WorkshopId,string IP,string Action,string ActivityType)
    {
        string[] strarr={Activity,UserId,WorkshopId,IP,Action,ActivityType};
        WebApplication1.dbConnection dbcon=new WebApplication1.dbConnection();
        dbcon.ExecuteQueryWithParams("INSERT INTO [dbo].[UserActivity]([Activity],[UserId],[WorkshopId],[DOC],[IP],[Action],[ActivityType]) VALUES (@1,@2,@3,'"+dbcon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss tt")+"',@4,@5,@6)",strarr);
    }
}