//' (c) Copyright Microsoft Corporation.
//' This source is subject to the Microsoft Public License.
//' See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
//' All other rights reserved.
//'*-------------------------------*
//'*                               *
//'*      Mahsa Hassankashi        *
//'*     info@mahsakashi.com       *
//'*   http://www.mahsakashi.com   * 
//'*     kashi_mahsa@yahoo.com     * 
//'*                               *
//'*                               *
//'*-------------------------------*
//' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
using System;
using System.Collections;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using WebApplication1;

/// <summary>
/// Summary description for AutoComplete
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class AutoComplete : System.Web.Services.WebService {

    public AutoComplete () {

        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }

    [WebMethod]
     

    public string[] GetCompletionList(string prefixText, int count)
    {
        List<string> txtItems=new List<string>();
        dbConnection db = new dbConnection();
        DataTable dt = db.GetDataTable("SELECT [Id] ,[Name] FROM [dbo].[Category]");
        for (int i = 0; i < dt.Rows.Count;i++)
        {
            //String From DataBase(dbValues)
           string dbValues = dt.Rows[1][0].ToString();
            dbValues = dbValues.ToLower();
            txtItems.Add(dbValues);
        }
        return txtItems.ToArray();
    }
}

