using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Mail;

/// <summary>
/// Summary description for EmailClass
/// </summary>
public class EmailClass
{
    public string SendMail( string ToEmailAddress, string EmailSubject, string EmailMessage)
    {
        string FromEmailAddress = "info@taazafood.com";

        EmailSubject = "Taaza Food - " + EmailSubject;
        string str = "";
        SmtpClient Client = new SmtpClient();
        Client.Credentials = new NetworkCredential("info@taazafood.com", "TZ@2016");
        Client.Host = "smtpout.asia.secureserver.net";
        Client.Timeout = 200000;
        Client.Port = 80;
        Client.DeliveryMethod = SmtpDeliveryMethod.Network;
        MailMessage message = new MailMessage(FromEmailAddress, ToEmailAddress);
        try
        {
            message.Subject = EmailSubject;
            message.IsBodyHtml = true;
            message.Priority = MailPriority.High;


            str = str + EmailMessage;
            message.Body = str;
            Client.Send(message);
            return "Email sent successfully.";
        }
        catch (Exception ex)
        {
            str = str + ex.Message + "-" + ex.StackTrace;
            return "Email Sending failed: " + str;
        }
        finally
        {
            Client.Dispose();
            message.Dispose();
            SendMessage();
        }
    }

    public void SendMessage()
    {
        try
        {
            string Sms = "Taaza Food: Problem with Server. Please check your email.";
            string Mobile = "9726351115";
            try
            {
                HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create
                ("https://hapi.smsapi.org/SendSMS.aspx?UserName=TaazaFood&password=TaazaFood2016&MobileNo=" + Mobile
                + "&SenderID=TaazaF&CDMAHeader=TaazaF&Message=" + Sms);
                HttpWebResponse myResp = (HttpWebResponse)myReq.GetResponse();

                System.IO.StreamReader respStreamReader = new System.IO.StreamReader(myResp.GetResponseStream());
                string responseString = respStreamReader.ReadToEnd();
                string correct = responseString.Substring(0, 2);
                respStreamReader.Close();
                myResp.Close();

            }
            catch (Exception ex)
            {
            }
        }
        catch (Exception)
        {
        }


    }

}