<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        // Code that runs on application startup
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //  Code that runs on application shutdown
    }
    
    void Application_Error(object sender, EventArgs e) 
    { 
        // Code that runs when an unhandled error occurs
        Exception exceptionObject = Server.GetLastError();
        WebApplication1.dbConnection objDbConnection = new WebApplication1.dbConnection();
        //objDbConnection.InsertLogs(exceptionObject.Message,(exceptionObject!=null?exceptionObject.InnerException.ToString():""),exceptionObject.StackTrace);
        objDbConnection.ExecuteQuery("INSERT INTO [dbo].[UnHandelledExaptionsLog]([ErrorMessage],[InnerException],[StackTrace],[DOC] ,[WorkshopId]) VALUES ('" + exceptionObject.Message.Replace("'", "''") + "','" + (exceptionObject.InnerException != null ? exceptionObject.InnerException.ToString().Replace("'", "''") : "") + "','" + exceptionObject.StackTrace.Replace("'", "''") + "','" + objDbConnection.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss") + "' ,1)");
    }

    void Session_Start(object sender, EventArgs e) 
    {
        // Code that runs when a new session is started

    }

    void Session_End(object sender, EventArgs e) 
    {
        // Code that runs when a session ends. 
        // Note: The Session_End event is raised only when the sessionstate mode
        // is set to InProc in the Web.config file. If session mode is set to StateServer 
        // or SQLServer, the event is not raised.

    }
       
</script>
