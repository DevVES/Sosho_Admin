using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;
using System.Net;
using System.Web;
using System.IO;

namespace WebApplication1
{
    public class dbConnection
    {
        //public static string ServiceUrl = "http://api.motorz.co.in";
        //public static string ServiceUrl = "http://192.168.1.122";

        public static string ServiceUrl = "http://sosho.in/";
	public  string consString = @"Data Source=KHUSHBU\SQLEXPRESS;Initial Catalog=SalebhaiOnePageStaging;Persist Security Info=True;User Id=sa;Password=ves123;";
        //public string consString = @"Data Source=S97-74-232-233\SQLEXPRESS;Initial Catalog=SalebhaiOnePage_Testing;Integrated Security=True;Persist Security Info=False";

        //CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30'))
        //public string consString = @"Data Source=DISHA\SQLEXPRESS;Initial Catalog=SalebhaiOnePage_Staging;User Id=sa;Password=123";
        //public string consString = @"Data Source=S97-74-229-95\SQLEXPRESS;Initial Catalog=MOTORZ;Integrated Security=True;Persist Security Info=False";
        // public string consString = getConnectionString();
        //public static string getConnectionString()
        //{
        //    string txtpath = HttpContext.Current.Server.MapPath("../") + "\\setting.txt";
        //    StreamReader sr = new StreamReader(txtpath);
        //    String line = sr.ReadToEnd();
        //    return line;
        //}
        SqlConnection conn = new SqlConnection();
        string ErrorMsgPrefix = "Error in dbConnection.cs  ";
        public SqlConnection GetConnectionForAdapter()
        {
            if (conn.State == ConnectionState.Open)
            {
                return conn;
            }
            else
            {
                conn.ConnectionString = consString;
                return conn;
            }
        }

        public int ExecuteQueryWithParams(string query, string[] parameters)
        {
            try
            {
                query = query.ToLower();
                String CheckQry = query;
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                
                for (int counter = 1; counter <= parameters.Length; counter++)
                {
                    cmd.Parameters.AddWithValue("@" + counter.ToString(), parameters[counter - 1]);
                    string s1 = "@" + counter.ToString();
                    string s2 = parameters[counter - 1];
                    CheckQry = CheckQry.Replace(s1, s2);

                }
                conn = openConnection();
                cmd.Connection = conn;
                int val = cmd.ExecuteNonQuery();
                conn.Close();
                return val;
            }
            catch (Exception e)
            {
                InsertLogs(ErrorMsgPrefix + " SavePartDetail Msg:" + e.Message, "", e.StackTrace);
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }

        public int ExecuteQueryWithParamsId(string query, string[] parameters)
        {
            try
            {
                query = query.ToLower();
                String CheckQry = query;
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;

                for (int counter = 1; counter <= parameters.Length; counter++)
                {
                    cmd.Parameters.AddWithValue("@" + counter.ToString(), parameters[counter - 1]);
                    string s1 = "@" + counter.ToString();
                    string s2 = parameters[counter - 1];
                    CheckQry = CheckQry.Replace(s1, s2);

                }
                conn = openConnection();
                cmd.Connection = conn;
                //int val = cmd.ExecuteNonQuery();
                int val1 = Convert.ToInt32(cmd.ExecuteScalar());
                conn.Close();
                return val1;
            }
            catch (Exception e)
            {
                InsertLogs(ErrorMsgPrefix + " SavePartDetail Msg:" + e.Message, "", e.StackTrace);
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
        public object ExecuteSQLScaler(string sqlStatement)
        {
            object obj = null;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlStatement;
                cmd.Connection = openConnection();

                //objcmd.Connection = objcon;
                cmd.CommandText = sqlStatement;
                obj = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                cmd.Dispose();
                return obj;
            }
            catch (Exception e)
            {
                InsertLogs(ErrorMsgPrefix + " SavePartDetail Msg:" + e.Message, "", e.StackTrace);
                return obj;
            }
        }
        public int ExecuteScalarQueryWithParams(string query, string[] parameters)
        {
            try
            {
                String CheckQry = query;
                query = query.ToLower();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;

                for (int counter = 1; counter <= parameters.Length; counter++)
                {
                    cmd.Parameters.AddWithValue("@" + counter.ToString(), parameters[counter - 1]);
                    string s1 = "@" + counter.ToString();
                    string s2 = parameters[counter - 1];
                    CheckQry = CheckQry.Replace(s1, s2);
                }
                conn = openConnection();
                cmd.Connection = conn;
                object value = cmd.ExecuteScalar();
                int val = Convert.ToInt32(value);
                conn.Close();
                return val;
            }
            catch (Exception e)
            {
                InsertLogs(ErrorMsgPrefix + " ExecuteScalarQueryWithParams Msg:" + e.Message, "", e.StackTrace);
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
        public SqlConnection openConnection()
        {
            if (conn.State == ConnectionState.Open)
            {
                return conn;
            }
            else
            {
                conn.ConnectionString = consString;
                conn.Open();
            }
            return conn;
        }
        public void closeConnection()
        {
            if (conn.State == ConnectionState.Closed)
            {
                return;
            }
            else
            {
                conn.Close();
            }

        }

        public DataTable GetDataTable(string query)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlDataAdapter adap = new SqlDataAdapter(query, GetConnectionForAdapter());
                adap.SelectCommand.CommandTimeout = 180;
                adap.Fill(dt);
            }
            catch (Exception e) {
                InsertLogs(ErrorMsgPrefix + " SavePartDetail Msg:" + e.Message, "", e.StackTrace);
            }
            return dt;
        }
        public DataTable GetDataTableWithParams(string query, string[] parameters)
        {
            try
            {
                query = query.ToLower();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                for (int counter = 1; counter <= parameters.Length; counter++)
                {
                    cmd.Parameters.AddWithValue("@" + counter.ToString(), parameters[counter - 1]);
                }
                conn = openConnection();
                cmd.Connection = conn;
                SqlDataAdapter adap = new SqlDataAdapter();
                adap.SelectCommand = cmd;
                adap.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                InsertLogs(ErrorMsgPrefix + " GetDataTableWithParams Msg:" + e.Message, "", e.StackTrace);
                return null;
            }
            finally
            {
                conn.Close();
            }

        }
        public System.Web.UI.WebControls.DropDownList FillCombo(System.Web.UI.WebControls.DropDownList combo, string query, string textField, string valueField)
        {
            DataTable dt = new DataTable();
            SqlDataAdapter adap = new SqlDataAdapter(query, GetConnectionForAdapter());
            adap.Fill(dt);
            combo.DataSource = dt;
            combo.DataTextField = textField;
            combo.DataValueField = valueField;
            combo.DataBind();
            return combo;
        }
        public int ExecuteQuery(string query)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = openConnection();//Open
                int i = cmd.ExecuteNonQuery();
                return i;
            }
            catch (Exception e) {
                //InsertLogs(ErrorMsgPrefix + " ExecuteQuery Msg:" + e.Message, "", e.StackTrace + query);
                InsertLogs(ErrorMsgPrefix + " ExecuteQuery Msg:" + e.Message, "", query);
            }
            finally {
                closeConnection();
            }
            return 0;
            //Close
        }

        public int ExecuteQueryWithScalarId(string query)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = openConnection();//Open
                //int i = cmd.ExecuteNonQuery();
                int val1 = Convert.ToInt32(cmd.ExecuteScalar());
                return val1;
            }
            catch (Exception e)
            {
                //InsertLogs(ErrorMsgPrefix + " ExecuteQuery Msg:" + e.Message, "", e.StackTrace + query);
                InsertLogs(ErrorMsgPrefix + " ExecuteQuery Msg:" + e.Message, "", query);
            }
            finally
            {
                closeConnection();
            }
            return 0;
            //Close
        }

        public int ExecuteQuery1(string query)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Connection = openConnection();//Open
                int i = cmd.ExecuteNonQuery();
                return i;
            }
            catch (Exception E)
            {
                //InsertLogs(ErrorMsgPrefix + " SavePartDetail Msg:" + e.Message, "", e.StackTrace);
            }
            finally
            {
                closeConnection();
            }
            return 0;
            //Close
        }
        public object ExecuteSQLScalerWithTrn(string sqlStatement, SqlConnection con, SqlTransaction Trn)
        {
            object obj = null;
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = sqlStatement;
                cmd.Connection = con;
                cmd.Transaction = Trn;
                cmd.CommandText = sqlStatement;
                obj = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                cmd.Dispose();
                return obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public DataTable GetDataTableWithParamsWithTrn(string query, string[] parameters, SqlConnection con, SqlTransaction Trn)
        {
            try
            {
                query = query.ToLower();
                DataTable dt = new DataTable();
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                for (int counter = 1; counter <= parameters.Length; counter++)
                {
                    cmd.Parameters.AddWithValue("@" + counter.ToString(), parameters[counter - 1]);
                }
                conn = openConnection();
                cmd.Connection = conn;
                SqlDataAdapter adap = new SqlDataAdapter();
                adap.SelectCommand = cmd;
                adap.Fill(dt);
                return dt;
            }
            catch (Exception e)
            {
                InsertLogs(ErrorMsgPrefix + " GetDataTableWithParams Msg:" + e.Message, "", e.StackTrace);
                return null;
            }
            finally
            {
                conn.Close();
            }

        }
        public int ExecuteQueryWithTrn(string query, SqlConnection con, SqlTransaction Trn)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Transaction = Trn;
                cmd.Connection = con;
                int i = cmd.ExecuteNonQuery();
                closeConnection();
                return i;
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
        public DataTable GetDataTableWithTrn(string query, SqlConnection con, SqlTransaction Trn)
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = query;
                cmd.Transaction = Trn;
                cmd.Connection = con;
                SqlDataAdapter adap = new SqlDataAdapter(cmd);
                adap.Fill(dt);
                //closeConnection();
            }
            catch (Exception ex)
            {

            }
            return dt;
        }
        public void InsertLogs(string ErrorMessage, string InnerException="", string StackTrace="")
        {
            string SqlQuery = "INSERT INTO [dbo].[SystemLog] ([ErrorMessage],[InnerException],[StackTrace],[DOC]) VALUES ('" + ErrorMessage.Replace("'", "''") + "','" + InnerException.Replace("'", "''") + "','" + StackTrace.Replace("'", "''") + "',SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30'))";
            ExecuteQuery1(SqlQuery);
        }
        public DateTime getindiantime()
        {
            try
            {
                DateTime nonISD = DateTime.Now;
                //Change Time zone to ISD timezone
                TimeZoneInfo myTZ = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                //DateTime ISDTime = TimeZoneInfo.ConvertTime(nonISD, TimeZoneInfo.Local, myTZ);
                DateTime ISDTime = TimeZoneInfo.ConvertTime(nonISD, myTZ);
                //ISDTime = DateTime.ParseExact(ISDTime,"dd/MM/yyyy",System.Globalization.CultureInfo.InvariantCulture);
                return ISDTime;
            }
            catch (Exception ex)
            {
                return DateTime.Now;
            }
        }

       public string getindiantimeString(bool needTimeToo=false)
    {
        try
        {
            DateTime nonISD = DateTime.Now;
            //Change Time zone to ISD timezone
            TimeZoneInfo myTZ = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            //DateTime ISDTime = TimeZoneInfo.ConvertTime(nonISD, TimeZoneInfo.Local, myTZ);
            DateTime ISDTime = TimeZoneInfo.ConvertTime(nonISD, myTZ);
            //ISDTime = DateTime.ParseExact(ISDTime,"dd/MM/yyyy",System.Globalization.CultureInfo.InvariantCulture);
            if (needTimeToo)
                return ISDTime.ToString("dd-MMM-yyyy HH:mm:ss");
            return ISDTime.ToString("dd-MMM-yyyy");
        }
        catch (Exception ex)
        {
            return DateTime.Now.ToString("dd-MMM-yyyy");
        }
    }

        public string getDOCMtime()
        {
            try
            {
                DateTime nonISD = DateTime.Now;

                //Change Time zone to ISD timezone
                TimeZoneInfo myTZ = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                //DateTime ISDTime = TimeZoneInfo.ConvertTime(nonISD, TimeZoneInfo.Local, myTZ);
                DateTime ISDTime = TimeZoneInfo.ConvertTime(nonISD, myTZ);
                //ISDTime = DateTime.ParseExact(ISDTime,"dd/MM/yyyy",System.Globalization.CultureInfo.InvariantCulture);
                string currentDateString = ISDTime.ToString("dd-MM-yyyy HH:mm:ss");

                string[] dt1 = currentDateString.Split(' ');
                string[] date = dt1[0].Split('-');
                string[] time = dt1[1].Split(':');

                return date[2] + "-" + date[1] + "-" + date[0] + " " + time[0] + ":" + time[1] + ":" + time[2];
            }
            catch (Exception ex)
            {
                return DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss");
            }
        }

    public void SendSMS(String Mobile, String Sms)
        {
           
            try
            {
                {
                   
                    Sms = System.Web.HttpUtility.UrlEncode(Sms);
                    HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create
                    ("http://hapi.smsapi.org/SendSMS.aspx?UserName=sms_salebhai&password=240955&MobileNo=" + Mobile
                    + "&SenderID=ESOSHO&CDMAHeader=ESOSHO&Message=" + Sms);

                    HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();

                    System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
                    string responseString = respStreamReader.ReadToEnd();
                    string correct = responseString.Substring(0, 2);
                    respStreamReader.Close();
                    myResp.Close();
                }
            }
            catch (Exception ex)
            {

            }
        }


       
        public void InsertLogs(string  Logtype, string shortmsg, string detailmsg, string custId = "0")
        {
            try
            {
                string cust_ip = "";
                DateTime curr_time = getindiantime();
                SqlConnection sqlcon = GetConnectionForAdapter();
                string[] insert = { Logtype.ToString(), shortmsg, detailmsg, custId };
               // string cmd = "INSERT INTO [dbo].[Logs_Application]([LogType],[DOC],[LogShortMsg],[LogDetailedMsg],[CustomerId],[CustomerIP])VALUES(@1,getdate(),@2,@3,@4,'" + GetIP4Address() + "')";
                //SqlCommand sqlcmd = new SqlCommand(cmd, sqlcon);
                //sqlcon.Open();
                //int value = sqlcmd.ExecuteNonQuery();
                //sqlcon.Close();
                openConnection();
                //int value = ExecuteQueryWithParams(cmd, insert);
                closeConnection();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                conn.Close();
            }
        }

        public string GetIP4Address()
        {
            try
            {
                string hostName = Dns.GetHostName(); // Retrive the Name of HOST
                Console.WriteLine(hostName);
                // Get the IP
                string myIP = Dns.GetHostByName(hostName).AddressList[0].ToString();
                return myIP;
            }
            catch (Exception err)
            {
                //InsertLogs(LOGS.LogLevel.Error, "GetIP4Address", err.Message.ToString());
                return null;
            }
        }

      

    }

      
}
