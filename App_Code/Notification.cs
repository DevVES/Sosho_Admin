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
/// <summary>
/// Summary description for Notification
/// </summary>
public class Notification
{
    public Notification()
    {

    }
   
    public int SendNotification(string type, string uid, string image, string msg, bool Isoffer, string category, string mobile,string productid,int sendtype)
    {
        SqlConnection connection = new SqlConnection();
        try
        {
            dbConnection dbc = new dbConnection();

            SqlTransaction transaction;
            connection.ConnectionString = dbc.consString;
            connection.Open();
            SqlCommand command = connection.CreateCommand();
            command.CommandTimeout = 1200;
            transaction = connection.BeginTransaction();
            command.Connection = connection;
            command.Transaction = transaction;

            mobile = mobile.Replace("\r\n", ",");
            mobile = mobile.Replace(",,,", ",");
            mobile = mobile.Replace(",,", ",");
            int sendcount = 0;
            string[] mobarray = mobile.Split(',');


            string qry = "insert into Taaza_NotificationBatchMaster ([Mesage],[imgurl],[userid],[type],[doc]) values (@msg,@url,@userid,@typ,DATEADD(MINUTE, 330, GETUTCDATE()))";

            command.CommandText = qry;
            command.Parameters.Clear();
            command.Parameters.AddWithValue("@msg", msg);
            command.Parameters.AddWithValue("@url", image);
            command.Parameters.AddWithValue("@userid", uid);
            command.Parameters.AddWithValue("@typ", type);

            int j = command.ExecuteNonQuery();

            if (j > 0)
            {
                string qs2 = "SELECT IDENT_CURRENT('Taaza_NotificationBatchMaster')";

                object objbid = dbc.ExecuteSQLScaler(qs2);
                int bid = objbid == null ? 0 : int.Parse(objbid.ToString());

                if (Isoffer == true)
                {
                    int ii = InsertIsOffer(bid, image, msg);
                    if (ii <= 0)
                    {
                        return 0;
                    }
                }
               
              callImageNotificationService(mobile, bid, type, msg, image, category,productid,sendtype );
                transaction.Commit();
                return 1;
            }
            else
            {
                return 0;
            }
        }
        catch (Exception ex)
        {
            return 0;
        }
        finally
        {
            connection.Close();
        }
    }
  
    private void callImageNotificationService(string mobile, int batchId, string type, string txtMessage, string txtImage, string notificationCategory,string productid,int sendtype)
    {
        //com.taazafood.api.ProductSearch service = new com.taazafood.api.ProductSearch();
        ////localhost.ProductSearch service = new localhost.ProductSearch();
        //service.Timeout = -1;
        //if(txtImage != "")
        //{
        //    service.SendImageNotification(mobile, batchId, type, sendtype, txtMessage, txtImage, notificationCategory, productid);    
        //}
        //else
        //{
        //    service.SendNotification(mobile, batchId, type, sendtype, txtMessage, notificationCategory, productid);    
        //}
           

    }
    public int InsertIsOffer(int bid, string img, string msg)
    {
        try
        {
            dbConnection dbc = new dbConnection();
            int display = 0;
            if (img.Trim().Equals(""))
            {
                display = 0;
            }
            else
            {
                display = 1;
            }
            string[] ins = { img.Trim(), display.ToString(), msg.Trim(), bid.ToString() };
            int i = dbc.ExecuteQueryWithParams("insert into Taaza_Offer_Notification (Imageurl,ImageDisplay,OfferStatus,OfferMessage,StartDate,Notification_Id) Values (@1,@2,1,@3,DATEADD(MINUTE, 330, GETUTCDATE()),@4 )", ins);
            if (i > 0)
            {
                return 1;
            }
            else
            {
                return 0;
            }

        }
        catch (Exception ex)
        {
            return 0;
        }
    }


}