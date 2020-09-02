using WebApplication1;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Web.Script.Services;
using MotorzService;
using System.Web.Hosting;
using System.IO;
using System.Text;
using System.Net;
using System.Globalization;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
/// <summary>
/// Summary description for MotorzInner
/// </summary>
[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
// To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
[System.Web.Script.Services.ScriptService]
public class MotorzInner : System.Web.Services.WebService
{
    string urlLink = dbConnection.ServiceUrl + "/MotorzService.asmx/";
    dbConnection dbCon = new dbConnection();
    ServiceMethods ServiceMethod = new ServiceMethods();
    string ErrorMsgPrefix = "Error in MotorzInner.cs Method Name : ";
    public MotorzInner()
    {
        //Uncomment the following line if using designed components 
        //InitializeComponent(); 
    }
    #region Invoice Comment
    //SELECT [Comment] FROM [dbo].[InvoiceComment] where JobCardid=5455
    [WebMethod]
    public void getInvoiceCommentById(string jobcardid)
    {
        var objectToSerilize = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            DataTable dt = dbCon.GetDataTable("SELECT [Comment] FROM [dbo].[InvoiceComment] where JobCardid=" + jobcardid);
            if (dt != null && dt.Rows.Count > 0)
            {
               objectToSerilize.Name = dt.Rows[0][0].ToString();
            }
        }
        catch (Exception E) { }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }
    //INSERT INTO [dbo].[InvoiceComment] ([JobcardId] ,[Comment]) VALUES ( ,'')
    [WebMethod]
    public void addUpdateInvoiceComment(string jobcardid,string comment)
    {
        var objectToSerilize = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            DataTable dt = dbCon.GetDataTable("SELECT [Comment] FROM [dbo].[InvoiceComment] where JobCardid=" + jobcardid);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dbCon.ExecuteQuery("Update [dbo].[InvoiceComment] Set [Comment]='" + comment.Replace("'", "''") + "' where JobcardId=" + jobcardid) > 0)
                    objectToSerilize.Id = "1";
                else
                    objectToSerilize.Id = "0";
            }
            else
            {
                if(dbCon.ExecuteQuery("INSERT INTO [dbo].[InvoiceComment] ([JobcardId] ,[Comment]) VALUES ( "+jobcardid+",'"+comment.Replace("'","''")+"')")>0)
                    objectToSerilize.Id = "1";
                else
                    objectToSerilize.Id = "0";
            }
        }
        catch (Exception E) { }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }
    #endregion

    #region Insurance Detail


    // Allow Generate Invoice
    // JobCard.Type in (2,8,9)
    //[WebMethod]
    //public void ShouldAllowFertherInvoiceProcess(string jobcardid)
    //{
    //    var objectToSerilize = new Objs();
    //    JavaScriptSerializer js = new JavaScriptSerializer();
    //    Context.Response.Clear();
    //    Context.Response.ContentType = "application/json";
    //    objectToSerilize.Id = "0";
    //    try
    //    {
    //        if (dbCon.GetDataTable("Select jobcard.Id from jobcard where JobCard.Type in (2,8,9) and isnull(IsGatePassGenerated,0)=0 and jobcard.id=" + jobcardid).Rows.Count > 0)
    //        {
    //            //Check Insurance Detail

    //            if (dbCon.GetDataTable("Select Id from JobCard_Insurance_Mapping where JobCardId=" + jobcardid + " and len(isnull(Insurance_Number,''))>2 ").Rows.Count > 0)
    //            {
    //                //Check Rc Detail
    //                if (dbCon.GetDataTable("Select Id from RC_Details where JobCardId=" + jobcardid + " ").Rows.Count > 0)
    //                {
    //                    // Allow Further
    //                    objectToSerilize.Id = "1";
    //                }
    //            }
    //            //Surveyor Detail
    //        }
    //        else
    //        {
    //            objectToSerilize.Id = "1";
    //        }
    //    }
    //    catch (Exception E)
    //    {
    //        objectToSerilize.Id = "0";
    //    }
    //    Context.Response.Write(js.Serialize(objectToSerilize));
    //}

    [WebMethod]
    public void UpdatePayable(string jobcardid, string CustomerPayable,string InsurancePayable)
    {
        var objectToSerilize = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        objectToSerilize.Id = "0";
        try
        {
            if (dbCon.ExecuteQuery(" Update Jobcard set CustomerPayableAmount=" + CustomerPayable + (!String.IsNullOrWhiteSpace(InsurancePayable) ? ",InsuranceCompanyPayableAmount=" + InsurancePayable : " ") + ",PriGatepassProcessDone=1 where id=" + jobcardid) > 0)
            {
                objectToSerilize.Id = "1";
            }
        }
        catch (Exception E)
        {
            dbCon.InsertLogs(E.Message,"",E.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }
    [WebMethod]
    public void CheckPriGatepassProcessDone(string jobcardid)
    {
        var objectToSerilize = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        objectToSerilize.Id = "0";
        try
        {
            DataTable dt = dbCon.GetDataTable("Select isnull(PriGatepassProcessDone,0) from Jobcard where id=" + jobcardid);
            if (dt!=null && dt.Rows.Count>0)
            {
                objectToSerilize.Id = dt.Rows[0][0].ToString();
            }
        }
        catch (Exception E)
        {
            dbCon.InsertLogs(E.Message, "", E.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    #endregion

    [WebMethod]
    public void InvoiceHistory(string jobcardid, string type)
    {
        var objectToSerilize = new List<Objs2>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";

        try
        {
            DataTable dt = dbCon.GetDataTable("Select Id,Convert(varchar(17),Doc,113) as Doc,iif(isnull(IsCancelled,0)=0,iif(GstInvoiceNumber is not null,'Tax Invoice Generated, Invoice No. '+GstInvoiceNumber,'Proforma Invoice'),'Cancelled') as Cancelled,CancelledReason from [dbo].[Invoice] WHERE Type=" + type + " and JobCardId=" + jobcardid);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Objs2 objectToSerilize1 = new Objs2();
                    objectToSerilize1.Id = dt.Rows[i]["Id"].ToString();
                    objectToSerilize1.Name = dt.Rows[i]["Doc"].ToString();
                    objectToSerilize1.Id1 = dt.Rows[i]["Cancelled"].ToString();
                    objectToSerilize1.Id2 = dt.Rows[i]["CancelledReason"].ToString();
                    objectToSerilize.Add(objectToSerilize1);
                }
            }
        }
        catch (Exception E)
        {
            //objectToSerilize.Id = "0";
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }



    [WebMethod]
    public void IsAnyActiveProforma(string jobcardid, string type)
    {
        var objectToSerilize = new Objs();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        objectToSerilize.Id = "0";
        try
        {
            if (dbCon.GetDataTable("Select Id from [dbo].[Invoice] WHERE InvoiceNumber is not null and isnull(IsCancelled,0)=0 and Type=" + type + " and JobCardId=" + jobcardid).Rows.Count > 0)
            {
                objectToSerilize.Id = "1";
            }
        }
        catch (Exception E)
        {
            objectToSerilize.Id = "0";
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }
    [WebMethod]
    public void ReturnItem(string id, string qty)
    {
        var objectToSerilize = new Objs();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        objectToSerilize.Id = "0";
        try
        {
            if (dbCon.ExecuteQuery("UPDATE [dbo].[GRNDetail] SET [ReturnQty] = " + qty + " WHERE Id=" + id) > 0)
                objectToSerilize.Id = "1";
            else
                objectToSerilize.Id = "0";
        }
        catch (Exception E)
        {
            objectToSerilize.Id = "0";
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }
    [WebMethod]
    public void CancelInvoiceByJobcardId(string UserId, string jobcardid, string Reason)
    {
        var objectToSerilize = new Objs();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        objectToSerilize.Id = "0";
        try
        {
            bool IsInvoiceNumberGenerated = false;
            DataTable dtInvoice = dbCon.GetDataTable("SELECT [Id] FROM [dbo].[Invoice] where JobCardId=" + jobcardid + " and isnull(IsCancelled,0)=0 and (GstInvoiceNumber is not null )");
            if (dtInvoice != null && dtInvoice.Rows.Count > 0)
            {
                IsInvoiceNumberGenerated = true;
            }
            if (!IsInvoiceNumberGenerated)
            {
                //Get Ref Invoices
                DataTable dtCustomerRefInvoice = dbCon.GetDataTable("SELECT [Id] FROM [dbo].[Invoice] where isnull(IsCancelled,0)= 0 And Type=1 And JobCardId=" + jobcardid);
                DataTable dtInsuranceRefInvoice = dbCon.GetDataTable("SELECT [Id] FROM [dbo].[Invoice] where isnull(IsCancelled,0)= 0 And Type=2 And JobCardId=" + jobcardid);
                int customerInvoiceId = (dtCustomerRefInvoice != null && dtCustomerRefInvoice.Rows.Count > 0 ? (int)dtCustomerRefInvoice.Rows[0][0] : 0);
                int invoiceInvoiceId = (dtInsuranceRefInvoice != null && dtInsuranceRefInvoice.Rows.Count > 0 ? (int)dtInsuranceRefInvoice.Rows[0][0] : 0);

                //Copy Customer Invoice
                string CurrentDate = dbCon.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss");
                if (customerInvoiceId > 0)
                {
                    int customerInvoiceIdNew = (int)
                    dbCon.ExecuteSQLScaler("INSERT INTO [dbo].[Invoice] ([Id],[JobCardId],[TotalSpareAmount],[TotalServiceAmount],[TotalSGST],[TotalCGST],[TotalDiscount],[FinalTotal],[WorkshopId],[DOC],[DOM],[Type],[IncludeInGst],[Taxable18],[SGST18],[CGST18],[IGST18],[Taxable28],[SGST28],[CGST28],[IGST28]) select (Select max(Id)+1 from invoice),[JobCardId],[TotalSpareAmount],[TotalServiceAmount],[TotalSGST],[TotalCGST],[TotalDiscount],[FinalTotal],[WorkshopId],'" + CurrentDate + "','" + CurrentDate + "',[Type],[IncludeInGst],[Taxable18],[SGST18],[CGST18],[IGST18],[Taxable28],[SGST28],[CGST28],[IGST28] from Invoice where id=" + customerInvoiceId + ";(Select max(Id) from invoice)");

                    //Add Parts
                    dbCon.ExecuteQuery("Insert into Invoice_Spare_Mapping ([InvoiceId],[SpareId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId],[GrnDetailId],[Mrp],[RefId],[Depreciation],CancelledRefId,PurchaseAmount) SELECT " + customerInvoiceIdNew + ",[SpareId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId],[GrnDetailId],[Mrp],[RefId],[Depreciation],Id,PurchaseAmount FROM [dbo].[Invoice_Spare_Mapping] where [Invoice_Spare_Mapping].InvoiceId=" + customerInvoiceId);
                    //Add Service
                    dbCon.ExecuteQuery("Insert into Invoice_Service_Mapping ([InvoiceId],[ServiceId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId],[Mrp],[RefId],[Depreciation],CancelledRefId) SELECT " + customerInvoiceIdNew + ",[ServiceId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId],[Mrp],[RefId],[Depreciation],Id FROM [dbo].[Invoice_Service_Mapping] where [Invoice_Service_Mapping].InvoiceId=" + customerInvoiceId);
                }
                //if (customerInvoiceId > 0 || invoiceInvoiceId > 0)
                //{
                //    dbCon.ExecuteQuery("UPDATE [dbo].[Invoice] SET [IsCancelled] = 1,[CancelledBy] =" + UserId + "  ,[CancelledReason] ='" + Reason + "'  WHERE Id in (" + invoiceInvoiceId + "," + customerInvoiceId + ")");
                //}
                //Copy Insurance Invoice
                if (invoiceInvoiceId > 0)
                {
                    int invoiceInvoiceIdNew = (int)
                    dbCon.ExecuteSQLScaler("INSERT INTO [dbo].[Invoice] ([Id],[JobCardId],[TotalSpareAmount],[TotalServiceAmount],[TotalSGST],[TotalCGST],[TotalDiscount],[FinalTotal],[WorkshopId],[DOC],[DOM],[Type],[IncludeInGst],[Taxable18],[SGST18],[CGST18],[IGST18],[Taxable28],[SGST28],[CGST28],[IGST28]) select (Select max(Id)+1 from invoice),[JobCardId],[TotalSpareAmount],[TotalServiceAmount],[TotalSGST],[TotalCGST],[TotalDiscount],[FinalTotal],[WorkshopId],'" + CurrentDate + "','" + CurrentDate + "',[Type],[IncludeInGst],[Taxable18],[SGST18],[CGST18],[IGST18],[Taxable28],[SGST28],[CGST28],[IGST28] from Invoice where id=" + invoiceInvoiceId + ";(Select max(Id) from invoice)");

                    //Add Parts
                    dbCon.ExecuteQuery("Insert into Invoice_Spare_Mapping ([InvoiceId],[SpareId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId],[GrnDetailId],[Mrp],[RefId],[Depreciation],PurchaseAmount) SELECT " + invoiceInvoiceIdNew + ",[SpareId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId],[GrnDetailId],[Mrp],(Select a.id from Invoice_Spare_Mapping as a where a.CancelledRefId=[Invoice_Spare_Mapping].RefId),[Depreciation],PurchaseAmount FROM [dbo].[Invoice_Spare_Mapping] where [Invoice_Spare_Mapping].InvoiceId=" + invoiceInvoiceId);
                    //Add Service
                    dbCon.ExecuteQuery("Insert into Invoice_Service_Mapping ([InvoiceId],[ServiceId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId],[GrnDetailId],[Mrp],[RefId],[Depreciation]) SELECT " + invoiceInvoiceIdNew + ",[ServiceId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId],[GrnDetailId],[Mrp],(Select a.id from Invoice_Service_Mapping as a where a.CancelledRefId=[Invoice_Service_Mapping].RefId),[Depreciation] FROM [dbo].[Invoice_Service_Mapping] where [Invoice_Service_Mapping].InvoiceId=" + invoiceInvoiceId);
                }
                //Cancel Referance Invoice
                if (customerInvoiceId > 0 || invoiceInvoiceId > 0)
                {
                    dbCon.ExecuteQuery("UPDATE [dbo].[Invoice] SET [IsCancelled] = 1,[CancelledBy] =" + UserId + "  ,[CancelledReason] ='" + Reason + "'  WHERE Id in (" + invoiceInvoiceId + "," + customerInvoiceId + ")");
                }
            }
        }
        catch (Exception E)
        {
            objectToSerilize.Id = "0";
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void GenerateInvoiceByJobcardId(string jobcardid)
    {
        var objectToSerilize = new Objs();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        objectToSerilize.Id = "0";
        try
        {
            if (dbCon.ExecuteQuery("UPDATE [dbo].[Invoice] SET [InvoiceNumber] = [Id],Doc='"+dbCon.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss")+"' WHERE JobCardId=" + jobcardid) > 0)
            {
                objectToSerilize.Id = "1";
            }
        }
        catch (Exception E)
        {
            objectToSerilize.Id = "0";
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }
    [WebMethod]
    public void CountNoOfInvoiceCancelled(string jobcardid)
    {
        var objectToSerilize = new Objs();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        objectToSerilize.Id = "0";
        try
        {
            DataTable dt = dbCon.GetDataTable("Select Count(id) from [dbo].[Invoice]  WHERE type=1 and JobCardId=" + jobcardid);

            if (dt != null && dt.Rows.Count > 0)
            {
                objectToSerilize.Id = "History ( " + dt.Rows[0][0].ToString() + " )";
            }
            dt = dbCon.GetDataTable("Select Count(id) from [dbo].[Invoice]  WHERE type=2 and JobCardId=" + jobcardid);

            if (dt != null && dt.Rows.Count > 0)
            {
                objectToSerilize.Name = "History ( " + dt.Rows[0][0].ToString() + " )";
            }
        }
        catch (Exception E)
        {
            objectToSerilize.Id = "0";
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void GetSpareInvoiceListListFromGrn(string JobcardId)
    {
        var objectToSerilize = new List<ServiceClass.SparewithPrice>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string Str = " Select (Select isnull(ActualAmountPerUnit,0) from grndetail where id=GrnDetailId ) as GrnMrp,isnull(Depreciation,100) as Depreciation,Invoice_Spare_Mapping.ActualAmountPerUnit as CustomerAmount,isnull((Select b.ActualAmountPerUnit from [Invoice_Spare_Mapping] as b where b.refid=[Invoice_Spare_Mapping].Id),0) as InsuranceAmount,[Invoice_Spare_Mapping].[Id],[SpareId] ,(Select code from spare where id= [SpareId]) as Name, isnull([Invoice_Spare_Mapping].Mrp,0) as Amount ,[Quantity],'N' as AllowWithoutAllocation,isnull(Invoice_Spare_Mapping.DiscountPerUnit,0) as DiscountPerUnit,iif(isnull(Invoice_Spare_Mapping.IsMrpChanged,0)=0,0,1) as IsMrpChanged  From [dbo].[Invoice_Spare_Mapping] as Invoice_Spare_Mapping inner join [Invoice] as Invoice on [Invoice_Spare_Mapping].InvoiceId=Invoice.Id where [JobCardId]=" + JobcardId + " and  isnull([Invoice_Spare_Mapping].GrnDetailId,0)>0  and [Invoice].Type=1 and isnull(Invoice.IsCancelled,0)=0 ";
            DataTable dt = dbCon.GetDataTable(Str);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var obj = new ServiceClass.SparewithPrice();

                    obj.MappingId = dt.Rows[i]["Id"].ToString();
                    obj.Name = dt.Rows[i]["Name"].ToString();
                    obj.Id = dt.Rows[i]["SpareId"].ToString();
                    obj.Price = dt.Rows[i]["Amount"].ToString();
                    obj.Quantity = dt.Rows[i]["Quantity"].ToString();
                    obj.AllowWithoutAllocation = dt.Rows[i]["AllowWithoutAllocation"].ToString();

                    obj.Dep = dt.Rows[i]["Depreciation"].ToString();
                    obj.Cust = dt.Rows[i]["CustomerAmount"].ToString();
                    obj.Ins = dt.Rows[i]["InsuranceAmount"].ToString();
                    obj.Discount = dt.Rows[i]["DiscountPerUnit"].ToString();
                    obj.IsMrpChanged = dt.Rows[i]["IsMrpChanged"].ToString();
                    obj.GrnMrp = dt.Rows[i]["GrnMrp"].ToString();

                    objectToSerilize.Add(obj);
                }
            }
        }
        catch (Exception E)
        {
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void GetSpareInvoiceListListFromGrn1(string JobcardId)
    {
        var objectToSerilize = new List<ServiceClass.SparewithPrice>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string Str = " Select isnull(Invoice_Spare_Mapping.Depreciation,100) as Depreciation,Invoice_Spare_Mapping.ActualAmountPerUnit as CustomerAmount,isnull((Select b.ActualAmountPerUnit from [Invoice_Service_Mapping] as b where b.refid=[Invoice_Spare_Mapping].Id),0) as InsuranceAmount,[Invoice_Spare_Mapping].[Id],[Serviceid] as SpareId ,(Select name from Service where id= [Serviceid]) as Name, isnull([Invoice_Spare_Mapping].Mrp,0) as Amount ,[Quantity],'N' as AllowWithoutAllocation,isnull(Invoice_Spare_Mapping.DiscountPerUnit,0) as DiscountPerUnit From [dbo].[Invoice_Service_Mapping] as Invoice_Spare_Mapping inner join [Invoice] as Invoice on [Invoice_Spare_Mapping].InvoiceId=Invoice.Id where [JobCardId]=" + JobcardId + "   and [Invoice].Type=1  and isnull(Invoice.IsCancelled,0)=0 ";
            DataTable dt = dbCon.GetDataTable(Str);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var obj = new ServiceClass.SparewithPrice();
                    obj.MappingId = dt.Rows[i]["Id"].ToString();
                    obj.Name = dt.Rows[i]["Name"].ToString();
                    obj.Id = dt.Rows[i]["SpareId"].ToString();
                    obj.Price = dt.Rows[i]["Amount"].ToString();
                    obj.Quantity = dt.Rows[i]["Quantity"].ToString();
                    obj.AllowWithoutAllocation = dt.Rows[i]["AllowWithoutAllocation"].ToString();

                    obj.Dep = dt.Rows[i]["Depreciation"].ToString();
                    obj.Cust = dt.Rows[i]["CustomerAmount"].ToString();
                    obj.Ins = dt.Rows[i]["InsuranceAmount"].ToString();
                    obj.Discount = dt.Rows[i]["DiscountPerUnit"].ToString();

                    objectToSerilize.Add(obj);
                }
            }
        }
        catch (Exception E)
        {
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void InsertUpdateSpares_Web(string partId, string Jobcardid)
    {
        var objectToSerilize = new ServiceClass.InsertVehicleSpare();
        JavaScriptSerializer js = new JavaScriptSerializer();
        js.MaxJsonLength = Int32.MaxValue;
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            AddMotorzLog("Part Id = " + partId + " Jobcardid = " + Jobcardid);
            #region  Add Items Into Customers Invoice Change
            int InvoiceId = 0;
            DataTable dtInvoice = dbCon.GetDataTable("SELECT [Id] FROM [dbo].[Invoice] where JobCardId=" + Jobcardid + " and Type=1 and isnull(IsCancelled,0)=0");
            if (dtInvoice != null && dtInvoice.Rows.Count > 0)
            {
                InvoiceId = Convert.ToInt32(dtInvoice.Rows[0][0].ToString());
            }
            else
            {
                try
                {
                    InvoiceId = Convert.ToInt32(dbCon.GetDataTable("Select Max(Id)+1 from Invoice").Rows[0][0].ToString());
                }
                catch (Exception E)
                {
                    InvoiceId = 1;
                }
                dbCon.ExecuteQuery("INSERT INTO [dbo].[Invoice] ([Id],[JobCardId],[WorkshopId],[DOC],[DOM],[Type],[IncludeInGst]) values (" + InvoiceId + "," + Jobcardid + ",1,'" + dbCon.getindiantime().ToString("dd-MMM-yyyy HH:ss:mm") + "','" + dbCon.getindiantime().ToString("dd-MMM-yyyy HH:ss:mm") + "',1,1)");
            }
            AddMotorzLog("Part Id = " + partId + " Jobcardid = " + Jobcardid + " Step 1");
            DataTable dtCalc = dbCon.GetDataTable("Select Price,Convert(decimal(18,2),(isnull(TaxValue,0)/2)) as TaxVal,Convert(decimal(18,2),((Spare.Price*TaxValue)/(100+TaxValue))/2) as TaxAmt,Convert(decimal(18,2),(Price -((Spare.Price*TaxValue)/(100+TaxValue)))) as Taxable  FROM TaxCategory RIGHT OUTER JOIN Spare ON Spare.TaxId = TaxCategory.Id where Spare.id=" + partId);

            AddMotorzLog("Part Id = " + partId + " Jobcardid = " + Jobcardid + " Step 2");
            //dbCon.ExecuteQuery("INSERT INTO [dbo].[Invoice_Spare_Mapping]([InvoiceId],[SpareId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId],[GrnDetailId]) VALUES (" + InvoiceId + "," + partId + ",'" + dtCalc.Rows[0]["TaxVal"].ToString() + "','" + dtCalc.Rows[0]["TaxAmt"].ToString() + "','" + dtCalc.Rows[0]["TaxVal"].ToString() + "','" + dtCalc.Rows[0]["TaxAmt"].ToString() + "',0,'1','" + 1 + "',0,(" + dtCalc.Rows[0]["Price"].ToString() + "*" + 1 + "),'" + dbCon.getindiantime().ToString("dd-MMM-yyyy HH:ss:mm") + "',1,0,0)");
            AddMotorzLog("Part Id = " + partId + " Jobcardid = " + Jobcardid + " Step 3");
            dbCon.ExecuteQuery("INSERT INTO [dbo].[Invoice_Spare_Mapping]([InvoiceId],[SpareId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId],[GrnDetailId]) VALUES (" + InvoiceId + "," + partId + ",'" + dtCalc.Rows[0]["TaxVal"].ToString() + "','" + dtCalc.Rows[0]["TaxAmt"].ToString() + "','" + dtCalc.Rows[0]["TaxVal"].ToString() + "','" + dtCalc.Rows[0]["TaxAmt"].ToString() + "',0,'" + dtCalc.Rows[0]["Price"].ToString() + "','" + 1 + "',0,(" + dtCalc.Rows[0]["Price"].ToString() + "*" + 1 + "),'" + dbCon.getindiantime().ToString("dd-MMM-yyyy HH:ss:mm") + "',1,0," + 0 + ")");
            AddMotorzLog("Part Id = " + partId + " Jobcardid = " + Jobcardid + " Step 4");
            #endregion
        }
        catch (Exception ex)
        {
            objectToSerilize.resultflag = "0";
            objectToSerilize.Message = Constant.Message.ErrorMessage;
            dbCon.InsertLogs(ex.ToString(), ex.StackTrace.ToString());
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void InsertUpdateSpares_Web1(string partId, string Jobcardid)
    {
        var objectToSerilize = new ServiceClass.InsertVehicleSpare();
        JavaScriptSerializer js = new JavaScriptSerializer();
        js.MaxJsonLength = Int32.MaxValue;
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            #region  Add Items Into Customers Invoice Change
            int InvoiceId = 0;
            DataTable dtInvoice = dbCon.GetDataTable("SELECT [Id] FROM [dbo].[Invoice] where JobCardId=" + Jobcardid + " and Type=1  and isnull(IsCancelled,0)=0");
            if (dtInvoice != null && dtInvoice.Rows.Count > 0)
            {
                InvoiceId = Convert.ToInt32(dtInvoice.Rows[0][0].ToString());
            }
            else
            {
                try
                {
                    InvoiceId = Convert.ToInt32(dbCon.GetDataTable("Select Max(Id)+1 from Invoice").Rows[0][0].ToString());
                }
                catch (Exception E)
                {
                    InvoiceId = 1;
                }
                dbCon.ExecuteQuery("INSERT INTO [dbo].[Invoice] ([Id],[JobCardId],[WorkshopId],[DOC],[DOM],[Type],[IncludeInGst]) values (" + InvoiceId + "," + Jobcardid + ",1,'" + dbCon.getindiantime().ToString("dd-MMM-yyyy HH:ss:mm") + "','" + dbCon.getindiantime().ToString("dd-MMM-yyyy HH:ss:mm") + "',1,1)");
            }
            DataTable dtCalc = dbCon.GetDataTable("Select Spare.Id,Price,Convert(decimal(18,2),(isnull(TaxValue,0)/2)) as TaxVal,Convert(decimal(18,2),((Spare.Price*TaxValue)/(100))/2) as TaxAmt,Convert(decimal(18,2),(Price)) as Taxable  FROM TaxCategory RIGHT OUTER JOIN Service as Spare ON Spare.TaxId = TaxCategory.Id  where Spare.Id=" + partId);

            dbCon.ExecuteQuery("INSERT INTO [dbo].[Invoice_Service_Mapping]([InvoiceId],[ServiceId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId],Mrp,RefId,Depreciation) VALUES (" + InvoiceId + "," + partId + ",'" + dtCalc.Rows[0]["TaxVal"].ToString() + "','" + dtCalc.Rows[0]["TaxAmt"].ToString() + "','" + dtCalc.Rows[0]["TaxVal"].ToString() + "','" + dtCalc.Rows[0]["TaxAmt"].ToString() + "',0,'" + dtCalc.Rows[0]["Price"].ToString() + "','" + 1 + "',0,(" + dtCalc.Rows[0]["Price"].ToString() + "*" + 1 + "),'" + dbCon.getindiantime().ToString("dd-MMM-yyyy HH:ss:mm") + "',1,0," + dtCalc.Rows[0]["Price"].ToString() + ",0,100)");

            #endregion
        }
        catch (Exception ex)
        {
            objectToSerilize.resultflag = "0";
            objectToSerilize.Message = Constant.Message.ErrorMessage;
            dbCon.InsertLogs(ex.ToString(), ex.StackTrace.ToString());
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    //"Id":MappingId,"CustomerAmount":Amount,"InsuranseAmount":"<%=strJobcardId %>"
    public void UpdateInvoiceSpareLanding(string Id, string Landing,string JobcardId)
    {
        var objectToSerilize = new ServiceClass.InsertVehicleSpare();
        JavaScriptSerializer js = new JavaScriptSerializer();
        js.MaxJsonLength = Int32.MaxValue;
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            bool IsInvoiceNumberGenerated = false;
            DataTable dtInvoice = dbCon.GetDataTable("SELECT [Id] FROM [dbo].[Invoice] where JobCardId=" + JobcardId + " and isnull(IsCancelled,0)=0 and (GstInvoiceNumber is not null or InvoiceNumber is not null )");
            if (dtInvoice != null && dtInvoice.Rows.Count > 0)
            {
                IsInvoiceNumberGenerated = true;
            }
            if (!IsInvoiceNumberGenerated)
            {
                dbCon.ExecuteQuery("UPDATE [dbo].[Invoice_Spare_Mapping] SET  PurchaseAmount=iif(" + Landing + "=0,Null," + Landing + ")  WHERE Id=" + Id);
                dbCon.ExecuteQuery("UPDATE [dbo].[Invoice_Spare_Mapping] SET  PurchaseAmount=iif(" + Landing + "=0,Null," + Landing + ")  WHERE RefId=" + Id);
            }
        }
        catch (Exception ex)
        {
            objectToSerilize.resultflag = "0";
            objectToSerilize.Message = Constant.Message.ErrorMessage;
            dbCon.InsertLogs(ex.ToString(), ex.StackTrace.ToString());
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    //"Id":MappingId,"CustomerAmount":Amount,"InsuranseAmount":"<%=strJobcardId %>"
    public void UpdateInvoiceSpare(string Id, string CustomerAmount, string InsuranseAmount, string Depreciation, string JobcardId, string Discount, string Qty, string Mrp, int nOverride)
    {
        var objectToSerilize = new ServiceClass.InsertVehicleSpare();
        JavaScriptSerializer js = new JavaScriptSerializer();
        js.MaxJsonLength = Int32.MaxValue;
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            bool IsInvoiceNumberGenerated = false;

            DataTable dtInvoice = dbCon.GetDataTable("SELECT [Id] FROM [dbo].[Invoice] where JobCardId=" + JobcardId + " and isnull(IsCancelled,0)=0 and (GstInvoiceNumber is not null or InvoiceNumber is not null )");
            if (dtInvoice != null && dtInvoice.Rows.Count > 0)
            {
                IsInvoiceNumberGenerated = true;
            }
            if (!IsInvoiceNumberGenerated)
            {
                DataTable dtGetCustomerInvoiceDetail = dbCon.GetDataTable("SELECT isnull([CGSTValue],0) as CGST,Convert(Decimal(18,2),[Quantity]) as Qty FROM [dbo].[Invoice_Spare_Mapping] where id=" + Id);
                DataTable dtGetInsuranceInvoiceDetail = dbCon.GetDataTable("SELECT [CGSTValue],[SGSTValue],Convert(Decimal(18,2),[Quantity]) as Qty FROM [dbo].[Invoice_Spare_Mapping] where RefId=" + Id);


                if (dtGetCustomerInvoiceDetail != null && dtGetCustomerInvoiceDetail.Rows.Count > 0)
                {
                    decimal cgst = (decimal)dtGetCustomerInvoiceDetail.Rows[0]["CGST"];
                    decimal qty = (decimal)Convert.ToDecimal(Qty);
                    decimal CustAmount = Convert.ToDecimal(CustomerAmount);
                    decimal nDiscount = Convert.ToDecimal(Discount);
                    //CustAmount = CustAmount-nDiscount;
                    decimal cgstAmount = (((CustAmount - nDiscount) * (cgst * 2)) / (100 + (cgst * 2))) / 2;
                    decimal TotalAmount = CustAmount * qty;
                    decimal TotalDiscount = nDiscount * qty;
                    dbCon.ExecuteQuery("UPDATE [dbo].[Invoice_Spare_Mapping] SET  " + (nOverride == 1 ? " IsMrpChanged=1, " : "") + "Mrp=" + Mrp + ",Quantity=" + qty + ",[CGSTAmount] =" + cgstAmount + " ,[SGSTAmount] =" + cgstAmount + " ,[TotalActualAmount] =" + TotalAmount + " ,[Depreciation] =" + Depreciation + ",ActualAmountPerUnit=" + CustAmount + ",DiscountPerUnit=" + nDiscount + ",TotalDiscount=" + TotalDiscount + "  WHERE Id=" + Id);
                }
                if (dtGetInsuranceInvoiceDetail != null && dtGetInsuranceInvoiceDetail.Rows.Count > 0)
                {
                    decimal cgst = (decimal)dtGetCustomerInvoiceDetail.Rows[0]["CGST"];
                    decimal qty = (decimal)Convert.ToDecimal(Qty);
                    decimal CustAmount = Convert.ToDecimal(InsuranseAmount);
                    decimal cgstAmount = (((CustAmount) * (cgst * 2)) / (100 + (cgst * 2))) / 2;
                    decimal TotalAmount = CustAmount * qty;
                    dbCon.ExecuteQuery("UPDATE [dbo].[Invoice_Spare_Mapping] SET " + (nOverride == 1 ? " IsMrpChanged=1, " : "") + "Mrp=" + Mrp + ",Quantity=" + qty + ",[CGSTAmount] =" + cgstAmount + " ,[SGSTAmount] =" + cgstAmount + " ,[TotalActualAmount] =" + TotalAmount + " ,[Depreciation] =" + Depreciation + ",ActualAmountPerUnit=" + InsuranseAmount + "  WHERE RefId=" + Id);
                }
                else
                {
                    if (Depreciation != "100")
                    {
                        int InvoiceId = 0;
                        dtInvoice = dbCon.GetDataTable("SELECT [Id] FROM [dbo].[Invoice] where JobCardId=" + JobcardId + " and Type=2  and isnull(IsCancelled,0)=0");
                        if (dtInvoice != null && dtInvoice.Rows.Count > 0)
                        {
                            InvoiceId = Convert.ToInt32(dtInvoice.Rows[0][0].ToString());
                        }
                        else
                        {
                            try
                            {
                                InvoiceId = Convert.ToInt32(dbCon.GetDataTable("Select Max(Id)+1 from Invoice").Rows[0][0].ToString());
                            }
                            catch (Exception E)
                            {
                                InvoiceId = 1;
                            }
                            dbCon.ExecuteQuery("INSERT INTO [dbo].[Invoice] ([Id],[JobCardId],[WorkshopId],[DOC],[DOM],[Type],[IncludeInGst]) values (" + InvoiceId + "," + JobcardId + ",1,'" + dbCon.getindiantime().ToString("dd-MMM-yyyy HH:ss:mm") + "','" + dbCon.getindiantime().ToString("dd-MMM-yyyy HH:ss:mm") + "',2,1)");
                        }
                        dbCon.ExecuteQuery("INSERT INTO [dbo].[Invoice_Spare_Mapping] ([InvoiceId],[SpareId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId],[GrnDetailId],[Mrp],[RefId],[Depreciation],PurchaseAmount)( SELECT " + InvoiceId + ",[SpareId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],0,[ActualAmountPerUnit],[Quantity],0,[TotalActualAmount],[DOC],[WorkshopId],[MappingId],0,[Mrp]," + Id + ",0,PurchaseAmount FROM [dbo].[Invoice_Spare_Mapping] where id=" + Id + ")");

                        decimal cgst = (decimal)dtGetCustomerInvoiceDetail.Rows[0]["CGST"];
                        decimal qty = (decimal)Convert.ToDecimal(Qty);
                        decimal CustAmount = Convert.ToDecimal(InsuranseAmount);
                        decimal cgstAmount = (((CustAmount) * (cgst * 2)) / (100 + (cgst * 2))) / 2;
                        decimal TotalAmount = CustAmount * qty;
                        dbCon.ExecuteQuery("UPDATE [dbo].[Invoice_Spare_Mapping] SET Mrp=" + Mrp + ",Quantity=" + qty + ",[CGSTAmount] =" + cgstAmount + " ,[SGSTAmount] =" + cgstAmount + " ,[TotalActualAmount] =" + TotalAmount + " ,[Depreciation] =" + Depreciation + ",ActualAmountPerUnit=" + InsuranseAmount + "  WHERE RefId=" + Id);
                    }
                }
                objectToSerilize.resultflag = "1";
            }
        }
        catch (Exception ex)
        {
            objectToSerilize.resultflag = "0";
            objectToSerilize.Message = Constant.Message.ErrorMessage;
            dbCon.InsertLogs(ex.ToString(), ex.StackTrace.ToString());
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void DeleteSpareOrServiceMapping(string type, string jobcardid, string mappingid)
    {
        var objectToSerilize = new Objs();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            bool IsInvoiceNumberGenerated = false;
            DataTable dtInvoice = dbCon.GetDataTable("SELECT [Id] FROM [dbo].[Invoice] where JobCardId=" + jobcardid + " and isnull(IsCancelled,0)=0 and (GstInvoiceNumber is not null or InvoiceNumber is not null )");
            if (dtInvoice != null && dtInvoice.Rows.Count > 0)
            {
                IsInvoiceNumberGenerated = true;
            }
            if (!IsInvoiceNumberGenerated)
            {
                string[] strArr = { jobcardid, mappingid };
                if (type == "1")
                {
                    dbCon.ExecuteQueryWithParams("Delete from [dbo].[Invoice_Spare_Mapping]  WHERE id=@2 or refid=@2 ", strArr);
                }
                if (type == "2")
                {
                    dbCon.ExecuteQueryWithParams("Delete from  [dbo].[Invoice_Service_Mapping]  WHERE id=@2 or refid=@2 ", strArr);
                }
            }
            objectToSerilize.Id = "1";
        }
        catch (Exception E)
        {
            objectToSerilize.Id = "0";
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }


    [WebMethod]
    public void ApproveInvoiceByManager(string type, string jobcardid)
    {
        var objectToSerilize = new Objs();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        objectToSerilize.Id = "0";
        try
        {
            string UserId = HttpContext.Current.Request.Cookies["TUser"]["Id"];
            string[] strArr = { jobcardid };
            if (type == "1")
            {
                if (dbCon.ExecuteQueryWithParams("UPDATE [dbo].[Invoice] SET [Approved] = 1 ,[ApprovedBy] =" + UserId + "  WHERE JobCardId=@1 and Type=1 and isnull(IsCancelled,0)=0 ", strArr) > 0)
                {
                    objectToSerilize.Id = "1";
                }
            }
            if (type == "2")
            {
                if (dbCon.ExecuteQueryWithParams("UPDATE [dbo].[Invoice] SET [Approved] = 1 ,[ApprovedBy] =" + UserId + "  WHERE JobCardId=@1 and Type=2 and isnull(IsCancelled,0)=0 ", strArr) > 0)
                {
                    objectToSerilize.Id = "1";
                }
            }
            //objectToSerilize.Id = "1";
        }
        catch (Exception E)
        {
            objectToSerilize.Id = "0";
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }
    [WebMethod]
    public void IsInvoiceApproved(string type, string jobcardid)
    {
        var objectToSerilize = new Objs();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        objectToSerilize.Id = "0";
        try
        {
            string[] strArr = { jobcardid };
            if (type == "1")
            {
                if (dbCon.GetDataTableWithParams("Select id from [dbo].[Invoice]  WHERE JobCardId=@1 and Type=1 and isnull(IsCancelled,0)=0 and isnull(Approved,0)=0   and isnull(IsCancelled,0)=0 ", strArr).Rows.Count > 0)
                {
                    objectToSerilize.Id = "1";
                }
            }
            if (type == "2")
            {
                if (dbCon.GetDataTableWithParams("Select id from [dbo].[Invoice]  WHERE JobCardId=@1 and Type=2 and isnull(IsCancelled,0)=0 and isnull(Approved,0)=0  and isnull(IsCancelled,0)=0 ", strArr).Rows.Count > 0)
                {
                    objectToSerilize.Id = "1";
                }
            }
            //objectToSerilize.Id = "1";
        }
        catch (Exception E)
        {
            objectToSerilize.Id = "0";
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void IsJobcardHaveInsurance(string jobcardid)
    {
        var objectToSerilize = new Objs();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        objectToSerilize.Id = "0";
        try
        {
            string[] strArr = { jobcardid };
            if (dbCon.GetDataTableWithParams("SELECT  HavingInsurance FROM [dbo].[JobCard] where HavingInsurance=1 and JobCard.Type in (2,8,9,12,13) and JobCard.Id =@1", strArr).Rows.Count > 0)
            {
                if (dbCon.GetDataTable("Select id from JobCard_Insurance_Mapping where JobCardId=" + jobcardid + " and Insurance_Type=1").Rows.Count > 0)
                {
                    objectToSerilize.Id = "1";
                }
            }
        }
        catch (Exception E)
        {
            objectToSerilize.Id = "0";
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }
    [WebMethod]
    public void IsInvoiceNumberGenerated1(string jobcardid)
    {
        var objectToSerilize = new Objs();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        objectToSerilize.Id = "0";
        try
        {
            string[] strArr = { jobcardid };
            if (dbCon.GetDataTableWithParams("Select id from [dbo].[Invoice]  WHERE JobCardId=@1 and Type=1 and InvoiceNumber is not null  and isnull(IsCancelled,0)=0 ", strArr).Rows.Count > 0)
            {
                objectToSerilize.Id = "1";
            }
            if (dbCon.GetDataTableWithParams("Select id from [dbo].[Invoice]  WHERE JobCardId=@1 and Type=2 and InvoiceNumber is not null  and isnull(IsCancelled,0)=0 ", strArr).Rows.Count > 0)
            {
                objectToSerilize.Id = "1";
            }
        }
        catch (Exception E)
        {
            objectToSerilize.Id = "0";
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void IsInvoiceNumberGenerated(string jobcardid)
    {
        var objectToSerilize = new Objs();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        objectToSerilize.Id = "0";
        try
        {
            string[] strArr = { jobcardid };
            if (dbCon.GetDataTableWithParams("Select id from [dbo].[Invoice]  WHERE JobCardId=@1 and Type=1 and isnull(IsCancelled,0)=0 and InvoiceNumber is not null  and isnull(IsCancelled,0)=0 ", strArr).Rows.Count > 0)
            {
                objectToSerilize.Id = "1";
            }
            if (dbCon.GetDataTableWithParams("Select id from [dbo].[Invoice]  WHERE JobCardId=@1 and Type=2 and isnull(IsCancelled,0)=0 and InvoiceNumber is not null  and isnull(IsCancelled,0)=0", strArr).Rows.Count > 0)
            {
                objectToSerilize.Id = "1";
            }
            //if (dbCon.GetDataTableWithParams("Select id from [dbo].[Jobcard]  WHERE JobCardId=@1 and isnull(IsGatePassGenerated,0)=0 ", strArr).Rows.Count > 0)
            //{
            //    objectToSerilize.Id = "1";
            //}
        }
        catch (Exception E)
        {
            objectToSerilize.Id = "0";
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    
    [WebMethod]
    public void IsGatepassGenerated(string jobcardid)
    {
        var objectToSerilize = new Objs();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        objectToSerilize.Id = "0";
        try
        {
            string[] strArr = { jobcardid };
            //if (dbCon.GetDataTableWithParams("Select id from [dbo].[Invoice]  WHERE JobCardId=@1 and Type=1 and isnull(IsCancelled,0)=0 and InvoiceNumber is not null  and isnull(IsCancelled,0)=0 ", strArr).Rows.Count > 0)
            //{
            //    objectToSerilize.Id = "1";
            //}
            //if (dbCon.GetDataTableWithParams("Select id from [dbo].[Invoice]  WHERE JobCardId=@1 and Type=2 and isnull(IsCancelled,0)=0 and InvoiceNumber is not null  and isnull(IsCancelled,0)=0", strArr).Rows.Count > 0)
            //{
            //    objectToSerilize.Id = "1";
            //}
            if (dbCon.GetDataTableWithParams("Select id from [dbo].[Jobcard]  WHERE JobCardId=@1 and isnull(IsGatePassGenerated,0)=0 ", strArr).Rows.Count > 0)
            {
                objectToSerilize.Id = "1";
            }
        }
        catch (Exception E)
        {
            objectToSerilize.Id = "0";
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    //"Id":MappingId,"CustomerAmount":Amount,"InsuranseAmount":"<%=strJobcardId %>"
    public void UpdateInvoiceSpare1(string Id, string CustomerAmount, string InsuranseAmount, string Depreciation, string JobcardId, string Discount, string Qty, string Mrp)
    {
        var objectToSerilize = new ServiceClass.InsertVehicleSpare();
        JavaScriptSerializer js = new JavaScriptSerializer();
        js.MaxJsonLength = Int32.MaxValue;
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            bool IsInvoiceNumberGenerated = false;

            DataTable dtInvoice = dbCon.GetDataTable("SELECT [Id] FROM [dbo].[Invoice] where JobCardId=" + JobcardId + " and isnull(IsCancelled,0)=0 and (GstInvoiceNumber is not null or InvoiceNumber is not null )");
            if (dtInvoice != null && dtInvoice.Rows.Count > 0)
            {
                IsInvoiceNumberGenerated = true;
            }
            if (!IsInvoiceNumberGenerated)
            {
                DataTable dtGetCustomerInvoiceDetail = dbCon.GetDataTable("SELECT isnull([CGSTValue],0) as CGST,Convert(Decimal(18,2),[Quantity]) as Qty FROM [dbo].[Invoice_Service_Mapping] where id=" + Id);
                DataTable dtGetInsuranceInvoiceDetail = dbCon.GetDataTable("SELECT [CGSTValue],[SGSTValue],Convert(Decimal(18,2),[Quantity]) as Qty FROM [dbo].[Invoice_Service_Mapping] where RefId=" + Id);


                if (dtGetCustomerInvoiceDetail != null && dtGetCustomerInvoiceDetail.Rows.Count > 0)
                {
                    decimal cgst = (decimal)dtGetCustomerInvoiceDetail.Rows[0]["CGST"];
                    decimal qty = (decimal)Convert.ToDecimal(Qty);
                    decimal CustAmount = Convert.ToDecimal(CustomerAmount);
                    decimal nDiscount = Convert.ToDecimal(Discount);
                    //CustAmount = CustAmount-nDiscount;
                    decimal cgstAmount = (((CustAmount - nDiscount) * (cgst * 2)) / (100)) / 2;
                    decimal TotalAmount = CustAmount * qty;
                    decimal TotalDiscount = nDiscount * qty;
                    dbCon.ExecuteQuery("UPDATE [dbo].[Invoice_Service_Mapping] SET Mrp=" + Mrp + ",Quantity=" + qty + ",[CGSTAmount] =" + cgstAmount + " ,[SGSTAmount] =" + cgstAmount + " ,[TotalActualAmount] =" + TotalAmount + " ,[Depreciation] =" + Depreciation + ",ActualAmountPerUnit=" + CustAmount + ",DiscountPerUnit=" + nDiscount + ",TotalDiscount=" + TotalDiscount + "  WHERE Id=" + Id);
                }
                if (dtGetInsuranceInvoiceDetail != null && dtGetInsuranceInvoiceDetail.Rows.Count > 0)
                {
                    decimal cgst = (decimal)dtGetCustomerInvoiceDetail.Rows[0]["CGST"];
                    decimal qty = (decimal)Convert.ToDecimal(Qty);
                    decimal CustAmount = Convert.ToDecimal(InsuranseAmount);
                    decimal cgstAmount = (((CustAmount) * (cgst * 2)) / (100)) / 2;
                    decimal TotalAmount = CustAmount * qty;
                    dbCon.ExecuteQuery("UPDATE [dbo].[Invoice_Service_Mapping] SET Mrp=" + Mrp + ",Quantity=" + qty + ",[CGSTAmount] =" + cgstAmount + " ,[SGSTAmount] =" + cgstAmount + " ,[TotalActualAmount] =" + TotalAmount + " ,[Depreciation] =" + Depreciation + ",ActualAmountPerUnit=" + InsuranseAmount + "  WHERE RefId=" + Id);
                }
                else
                {
                    if (Depreciation != "100")
                    {
                        int InvoiceId = 0;
                        dtInvoice = dbCon.GetDataTable("SELECT [Id] FROM [dbo].[Invoice] where JobCardId=" + JobcardId + " and Type=2  and isnull(IsCancelled,0)=0");
                        if (dtInvoice != null && dtInvoice.Rows.Count > 0)
                        {
                            InvoiceId = Convert.ToInt32(dtInvoice.Rows[0][0].ToString());
                        }
                        else
                        {
                            try
                            {
                                InvoiceId = Convert.ToInt32(dbCon.GetDataTable("Select Max(Id)+1 from Invoice").Rows[0][0].ToString());
                            }
                            catch (Exception E)
                            {
                                InvoiceId = 1;
                            }
                            dbCon.ExecuteQuery("INSERT INTO [dbo].[Invoice] ([Id],[JobCardId],[WorkshopId],[DOC],[DOM],[Type],[IncludeInGst]) values (" + InvoiceId + "," + JobcardId + ",1,'" + dbCon.getindiantime().ToString("dd-MMM-yyyy HH:ss:mm") + "','" + dbCon.getindiantime().ToString("dd-MMM-yyyy HH:ss:mm") + "',2,1)");
                        }
                        dbCon.ExecuteQuery("INSERT INTO [dbo].[Invoice_Service_Mapping] ([InvoiceId],[ServiceId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId],[Mrp],[RefId],[Depreciation])( SELECT " + InvoiceId + ",[ServiceId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],0,[ActualAmountPerUnit],[Quantity],0,[TotalActualAmount],[DOC],[WorkshopId],[MappingId],[Mrp]," + Id + ",0 FROM [dbo].[Invoice_Service_Mapping] where id=" + Id + ")");

                        decimal cgst = (decimal)dtGetCustomerInvoiceDetail.Rows[0]["CGST"];
                        decimal qty = (decimal)Convert.ToDecimal(Qty);
                        decimal CustAmount = Convert.ToDecimal(InsuranseAmount);
                        decimal cgstAmount = (((CustAmount) * (cgst * 2)) / (100)) / 2;
                        decimal TotalAmount = CustAmount * qty;
                        dbCon.ExecuteQuery("UPDATE [dbo].[Invoice_Service_Mapping] SET Mrp=" + Mrp + ",Quantity=" + qty + ",[CGSTAmount] =" + cgstAmount + " ,[SGSTAmount] =" + cgstAmount + " ,[TotalActualAmount] =" + TotalAmount + " ,[Depreciation] =" + Depreciation + ",ActualAmountPerUnit=" + InsuranseAmount + "  WHERE RefId=" + Id);
                    }
                }
                objectToSerilize.resultflag = "1";
            }
        }
        catch (Exception ex)
        {
            objectToSerilize.resultflag = "0";
            objectToSerilize.Message = Constant.Message.ErrorMessage;
            dbCon.InsertLogs(ex.ToString(), ex.StackTrace.ToString());
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void GetSpareInvoiceListListWithoutGrn(string JobcardId)
    {
        var objectToSerilize = new List<ServiceClass.SparewithPrice>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string Str = " Select isnull(Depreciation,100) as Depreciation,isnull(Invoice_Spare_Mapping.PurchaseAmount,0) as PurchaseAmount,Invoice_Spare_Mapping.ActualAmountPerUnit as CustomerAmount,isnull((Select b.ActualAmountPerUnit from [Invoice_Spare_Mapping] as b where b.refid=[Invoice_Spare_Mapping].Id),0) as InsuranceAmount,[Invoice_Spare_Mapping].[Id],[SpareId] ,(Select code from spare where id= [SpareId]) as Name, isnull([Invoice_Spare_Mapping].Mrp,0) as Amount ,[Quantity],'N' as AllowWithoutAllocation,isnull(Invoice_Spare_Mapping.DiscountPerUnit,0) as DiscountPerUnit From [dbo].[Invoice_Spare_Mapping] as Invoice_Spare_Mapping inner join [Invoice] as Invoice on [Invoice_Spare_Mapping].InvoiceId=Invoice.Id where [JobCardId]=" + JobcardId + " and  isnull([Invoice_Spare_Mapping].GrnDetailId,0)=0  and [Invoice].Type=1 and isnull(Invoice.IsCancelled,0)=0";
            DataTable dt = dbCon.GetDataTable(Str);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var obj = new ServiceClass.SparewithPrice();
                    obj.MappingId = dt.Rows[i]["Id"].ToString();
                    obj.Name = dt.Rows[i]["Name"].ToString();
                    obj.Id = dt.Rows[i]["SpareId"].ToString();
                    obj.Price = dt.Rows[i]["Amount"].ToString();
                    obj.Quantity = dt.Rows[i]["Quantity"].ToString();
                    obj.AllowWithoutAllocation = dt.Rows[i]["AllowWithoutAllocation"].ToString();
                    obj.Dep = dt.Rows[i]["Depreciation"].ToString();
                    obj.Cust = dt.Rows[i]["CustomerAmount"].ToString();
                    obj.Ins = dt.Rows[i]["InsuranceAmount"].ToString();
                    obj.Discount = dt.Rows[i]["DiscountPerUnit"].ToString();
                    obj.PurchaseAmount = dt.Rows[i]["PurchaseAmount"].ToString();
                    objectToSerilize.Add(obj);
                }
            }
        }
        catch (Exception E)
        {
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void GetBarcode(string Id)
    {
        var obj = new List<GrnItem>();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            DataTable dt = dbCon.GetDataTable("SELECT [Id],(Select code from Spare where id= [EntityId]) as Name,[Qty] FROM [dbo].[GRNDetail] where GRNId=" + Id);
            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    GrnItem item = new GrnItem();
                    item.Id = dt.Rows[i]["Id"].ToString();
                    item.Name = dt.Rows[i]["Name"].ToString();
                    item.Qty = dt.Rows[i]["Qty"].ToString();
                    obj.Add(item);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetModelByBrandId Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }
    [WebMethod]
    public void MarkAsAvailable(string Id, string Tp)
    {
        var obj = new Objs1();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        obj.Id = "0";
        try
        {
            dbCon.ExecuteQuery("UPDATE [dbo].[Requisition_Spare] SET [IsAvailable] =" + (Tp.Equals("1") ? "1" : "0") + "  WHERE Id=" + Id);
            obj.Id = "1";
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetModelByBrandId Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }


    public class GrnItem
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Qty { get; set; }
    }
    [WebMethod]
    public void ClosePaymentRequest(string Id)
    {
        var obj = new Objs();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        obj.Id = "0";
        obj.Name = "fail";
        try
        {
            string UserId = HttpContext.Current.Request.Cookies["TUser"]["Id"];
            string[] strArr = { Id, UserId };
            if (dbCon.ExecuteQueryWithParams("UPDATE [dbo].[JobCard] SET [IsClosePaymentRequested] = 1,[ClosePaymentRequestedOn] ='" + dbCon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss") + "' ,[ClosePaymentRequestBy] =@2  WHERE id=@1", strArr) > 0)
            {
                obj.Id = "1";
                obj.Name = "success";
                try
                {
                    dbCon.ExecuteQuery("INSERT INTO [dbo].[JobcardPaymentCloseRequestHistory] ([RequestByUserId],[RequestDate],JobcardId) VALUES (" + UserId + ",'" + dbCon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss") + "'," + Id + ") ");
                    string WorkshopId = HttpContext.Current.Request.Cookies["TUser"]["WorkshopId"];
                    UserActivity objUserAct = new UserActivity();
                    objUserAct.InsertUserActivity("Request for close payment on : " + dbCon.getindiantime().ToString("MM-dd-yyyy HH:mm:ss tt") + " jobcard Id - " + Id, UserId, WorkshopId, "", "Close", "JobcardPayment");
                }
                catch (Exception E) { }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetModelByBrandId Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void GetValueFromBarcode(string code, string reqId)
    {
        var obj = new List<Objs2>();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string[] strArr = { code, reqId };
            DataTable dt = dbCon.GetDataTableWithParams("Select EntityId,(Select code from spare where id=EntityId)+' , '+(Select name from Spare_Brand where id=brandid) as SpareBrandName,brandid from GRNDetail where id=@1", strArr);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataTable dt1 = dbCon.GetDataTableWithParams("Select Id from Requisition_Spare where RequisitionId=@2 and SpareId=" + dt.Rows[0]["EntityId"].ToString(), strArr);
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        var objSub = new Objs2();
                        objSub.Name = dt.Rows[0]["SpareBrandName"].ToString();
                        objSub.Id = dt1.Rows[0]["Id"].ToString();
                        objSub.Id1 = dt.Rows[0]["brandid"].ToString();
                        obj.Add(objSub);
                    }
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetModelByBrandId Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }


    [WebMethod]
    public void GetAllBrand()
    {
        var obj = new List<Objs>();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            DataTable dt = dbCon.GetDataTable("Select Id,name from Vehicle_Brand  where IsDelete=0 --and id in ( select Vehicle_Brand_Id from Vehicle_Model where IsDelete=0)");
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var item = new Objs();
                    item.Id = dt.Rows[i]["Id"].ToString();
                    item.Name = dt.Rows[i]["Name"].ToString();
                    //Model
                    string[] strArr = { item.Id };
                    DataTable dt1 = dbCon.GetDataTableWithParams("Select Id,name from Vehicle_Model  where IsDelete=0  and Vehicle_Brand_Id=@1", strArr);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt1.Rows.Count; j++)
                        {
                            var item1 = new Objs1();
                            item1.Id = dt1.Rows[j]["Id"].ToString();
                            item1.Name = dt1.Rows[j]["Name"].ToString();
                            //variant
                            string[] strArr2 = { item1.Id };
                            DataTable dt2 = dbCon.GetDataTableWithParams("Select Id,name from Vehicle_Variant  where IsDelete=0 and Vehicle_Model_id=@1", strArr2);
                            if (dt1 != null && dt1.Rows.Count > 0)
                            {
                                for (int k = 0; k < dt2.Rows.Count; k++)
                                {
                                    var item2 = new Objs2();
                                    item2.Id = dt2.Rows[k]["Id"].ToString();
                                    item2.Name = dt2.Rows[k]["Name"].ToString();
                                    item1.Items.Add(item2);
                                }
                            }
                            //End
                            item.Items.Add(item1);
                        }
                    }
                    //End
                    obj.Add(item);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetAllBrand Msg:" + e.Message, "", e.StackTrace);

        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void GetModelByBrandId(string Id)
    {
        var obj = new List<Objs>();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string[] strArr = { Id };
            DataTable dt = dbCon.GetDataTableWithParams("Select Id,name from Vehicle_Model  where IsDelete=0  and Vehicle_Brand_Id=@1", strArr);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var item = new Objs();
                    item.Id = dt.Rows[i]["Id"].ToString();
                    item.Name = dt.Rows[i]["Name"].ToString();
                    obj.Add(item);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetModelByBrandId Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void GetVariantByModelId(string Id)
    {
        var obj = new List<Objs>();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string[] strArr = { Id };
            DataTable dt = dbCon.GetDataTableWithParams("Select Id,name from Vehicle_Variant  where IsDelete=0 and Vehicle_Model_id=@1", strArr);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var item = new Objs();
                    item.Id = dt.Rows[i]["Id"].ToString();
                    item.Name = dt.Rows[i]["Name"].ToString();
                    obj.Add(item);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetVariantByModelId Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void AddNewBrand(string Brands)
    {
        try
        {
            string strids = "0";
            string[] strArrBrand = Brands.Split(',');
            for (int i = 0; i < strArrBrand.Length; i++)
            {
                string[] strArr = { strArrBrand[i] };
                strids += "," + dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[Vehicle_Brand] ([Name],[IsActive],[IsDelete],[DisplayOrder],[DOC],[DOM]) VALUES (@1,1,0,0,SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30'),SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')) SELECT SCOPE_IDENTITY();", strArr).ToString();
            }
            var obj = new List<Objs>();
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            try
            {
                DataTable dt = dbCon.GetDataTable("Select Id,name from Vehicle_Brand  where IsDelete=0 and id in (" + strids + ")");
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        var item = new Objs();
                        item.Id = dt.Rows[i]["Id"].ToString();
                        item.Name = dt.Rows[i]["Name"].ToString();
                        //Model
                        string[] strArr = { item.Id };
                        DataTable dt1 = dbCon.GetDataTableWithParams("Select Id,name from Vehicle_Model  where IsDelete=0  and Vehicle_Brand_Id=@1", strArr);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            for (int j = 0; j < dt1.Rows.Count; j++)
                            {
                                var item1 = new Objs1();
                                item1.Id = dt1.Rows[j]["Id"].ToString();
                                item1.Name = dt1.Rows[j]["Name"].ToString();
                                //variant
                                string[] strArr2 = { item1.Id };
                                DataTable dt2 = dbCon.GetDataTableWithParams("Select Id,name from Vehicle_Variant  where IsDelete=0 and Vehicle_Model_id=@1", strArr2);
                                if (dt1 != null && dt1.Rows.Count > 0)
                                {
                                    for (int k = 0; k < dt2.Rows.Count; k++)
                                    {
                                        var item2 = new Objs2();
                                        item2.Id = dt2.Rows[k]["Id"].ToString();
                                        item2.Name = dt2.Rows[k]["Name"].ToString();
                                        item1.Items.Add(item2);
                                    }
                                }
                                //End
                                item.Items.Add(item1);
                            }
                        }
                        //End
                        obj.Add(item);
                    }
                }
            }
            catch (Exception e)
            {
                dbCon.InsertLogs(ErrorMsgPrefix + " GetAllBrand Msg:" + e.Message, "", e.StackTrace);

            }
            Context.Response.Write(js.Serialize(obj));
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetModelByBrandId Msg:" + e.Message, "", e.StackTrace);
        }
    }

    [WebMethod]
    public void AddNewModel(string Models, string BrandId)
    {
        try
        {
            string strids = "0";
            string[] strArrBrand = Models.Split(',');
            for (int i = 0; i < strArrBrand.Length; i++)
            {
                string[] strArr = { strArrBrand[i], BrandId };
                strids += "," + dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[Vehicle_Model] ([Name],[IsActive],[IsDelete],[DisplayOrder],[DOC],[DOM],Vehicle_Brand_Id) VALUES (@1,1,0,0,SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30'),SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30'),@2) SELECT SCOPE_IDENTITY();", strArr).ToString();
            }
            var obj = new List<Objs1>();
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            try
            {
                DataTable dt1 = dbCon.GetDataTable("Select Id,name from Vehicle_Model  where IsDelete=0 and id in(" + strids + ")");
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    for (int j = 0; j < dt1.Rows.Count; j++)
                    {
                        var item1 = new Objs1();
                        item1.Id = dt1.Rows[j]["Id"].ToString();
                        item1.Name = dt1.Rows[j]["Name"].ToString();
                        obj.Add(item1);
                    }
                }
            }
            catch (Exception e)
            {
                dbCon.InsertLogs(ErrorMsgPrefix + " AddNewModel Msg:" + e.Message, "", e.StackTrace);
            }
            Context.Response.Write(js.Serialize(obj));
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetModelByBrandId Msg:" + e.Message, "", e.StackTrace);
        }
    }

    [WebMethod]
    public void AddNewVariant(string Variants, string ModelId)
    {
        try
        {
            string strids = "0";
            string[] strArrBrand = Variants.Split(',');
            for (int i = 0; i < strArrBrand.Length; i++)
            {
                string[] strArr = { strArrBrand[i], ModelId };
                strids += "," + dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[Vehicle_Variant] ([Name],[IsActive],[IsDelete],[DisplayOrder],[DOC],[DOM],Vehicle_Model_Id) VALUES (@1,1,0,0,SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30'),SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30'),@2) SELECT SCOPE_IDENTITY();", strArr).ToString();
            }
            var obj = new List<Objs1>();
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
            Context.Response.Clear();
            Context.Response.ContentType = "application/json";
            try
            {
                DataTable dt1 = dbCon.GetDataTable("Select Id,name from Vehicle_Variant  where IsDelete=0 and id in(" + strids + ")");
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    for (int j = 0; j < dt1.Rows.Count; j++)
                    {
                        var item1 = new Objs1();
                        item1.Id = dt1.Rows[j]["Id"].ToString();
                        item1.Name = dt1.Rows[j]["Name"].ToString();
                        obj.Add(item1);
                    }
                }
            }
            catch (Exception e)
            {
                dbCon.InsertLogs(ErrorMsgPrefix + " AddNewVariant Msg:" + e.Message, "", e.StackTrace);
            }
            Context.Response.Write(js.Serialize(obj));
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetModelByBrandId Msg:" + e.Message, "", e.StackTrace);
        }
    }


    [WebMethod]
    public void GetTaxCategory()
    {
        var obj = new List<Objs>();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            DataTable dt = dbCon.GetDataTable("SELECT [Id],Name+',Rate:'+Convert(varchar,TaxValue) as Name FROM [dbo].[TaxCategory] where IsDelete=0");
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var item = new Objs();
                    item.Id = dt.Rows[i]["Id"].ToString();
                    item.Name = dt.Rows[i]["Name"].ToString();
                    obj.Add(item);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetModelByBrandId Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void GetCategory()
    {
        var obj = new List<Objs>();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            DataTable dt = dbCon.GetDataTable("SELECT ca.[Id] ,isnull((Select c.name+' >> ' from Category as c where c.Id=ca.ParentCategoryId),'')+[Name]  As Name FROM [dbo].[Category] ca");
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var item = new Objs();
                    item.Id = dt.Rows[i]["Id"].ToString();
                    item.Name = dt.Rows[i]["Name"].ToString();
                    obj.Add(item);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetModelByBrandId Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void SavePartDetail(string partId, string modelIds, string variantIds, string Name, string Code, string CatId, string TaxId, string Hsn, string IsGeneric)
    {
        var obj = new Objs();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            if (partId == "0")
            {
                String[] StrArr = { Name, "0", Code, "1", CatId, TaxId, "0", Hsn, (IsGeneric.ToLower() == "true" ? "1" : "0") };
                // DataTable dtchk = dbCon.GetDataTableWithParams("Select [Code] from Spare where Code=@", StrArr);
                int Id = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[Spare] ([Name],[Number],[Code],[IsActive],[Category_Id],[IsDeleted],[DOC],[DOM],TaxId,price,HSNNumber,IsGeneric) OUTPUT INSERTED.ID VALUES (@1,@2,@3,@4,@5,0,getdate(),getdate(),@6,@7,@8,@9)", StrArr);
                string SpareId = Id.ToString();
                try
                {
                    dbCon.ExecuteQuery("INSERT INTO [dbo].[SpareSpareBrandMapping] ([SpareId],[BrandId],[Qty],[MinQty],[MaxQty],[IsActive],[IsDeleted],[DOC],[DOM],[HSNNumber],[Mrp],[WorkshopId]) VALUES (" + SpareId + ",34,0,1,5000,1,0,getdate(),getdate(),'',0,1)");
                }
                catch (Exception E) { }
                if (IsGeneric.ToLower().Equals("false"))
                {
                    //Mapping
                    List<string> StrLst = new List<string>();
                    StrLst = modelIds.Split(',').ToList();
                    foreach (var s in StrLst)
                    {
                        if (int.Parse(s) > 0)
                        {
                            dbCon.ExecuteQuery("INSERT INTO [dbo].[SpareModelMapping] ([SpareId] ,[VehicleModelId] , [DOC] ,[DOM] ,[IsDelete]) VALUES (" + Id + "," + s + ",getdate(),getdate(),0)");
                        }
                    }
                    //StrLst = new List<string>();
                    //StrLst = (List<string>)ViewState["SelectedBrands"];
                    //foreach (var s in StrLst)
                    //{
                    //    if (int.Parse(s) > 0)
                    //    {
                    //        dbCon.ExecuteQuery("INSERT INTO [dbo].[SpareBrandMapping] ([SpareId] ,[VehicleBrandId] , [DOC] ,[DOM] ,[IsDelete]) VALUES (" + Id + "," + s + ",getdate(),getdate(),0)");
                    //    }
                    //}
                    StrLst = new List<string>();
                    StrLst = variantIds.Split(',').ToList();
                    foreach (var s in StrLst)
                    {
                        if (int.Parse(s) > 0)
                        {
                            dbCon.ExecuteQuery("INSERT INTO [dbo].[SpareVariantMapping] ([SpareId] ,[VehicleVariantId] , [DOC] ,[DOM] ,[IsDelete]) VALUES (" + Id + "," + s + ",getdate(),getdate(),0)");
                        }
                    }
                }
                //Emd Mapping

            }
            else
            {
                string SpareId = partId;
                string Id = SpareId;
                String[] StrArr = { Name, "", Code, "1", CatId, SpareId, TaxId, "0", (IsGeneric.ToLower() == "true" ? "1" : "0"), Hsn };
                dbCon.ExecuteQueryWithParams("Update [dbo].[Spare] Set [Name]=@1,[Number]=@2,[Code]=@3,[IsActive]=@4,[Category_Id]=@5,TaxId=@7,IsGeneric=@9,HSNNumber=@10 where id=@6 ", StrArr);

                if (IsGeneric.ToLower().Equals("false"))
                {
                    //Mapping
                    DataTable dtModel = dbCon.GetDataTable("Select * From SpareModelMapping where  SpareId=" + Id);
                    List<string> StrLst1 = new List<string>();
                    StrLst1 = modelIds.Split(',').ToList();
                    for (int i = 0; i < dtModel.Rows.Count; i++)
                    {
                        if (!StrLst1.Contains(dtModel.Rows[i]["VehicleModelId"].ToString()))
                            dbCon.ExecuteQuery("Delete from [dbo].[SpareModelMapping] where id=" + dtModel.Rows[i]["Id"].ToString());
                    }

                    //DataTable dtBrand = dbCon.GetDataTable("Select * From SpareBrandMapping where  SpareId=" + Id);
                    //List<string> StrLst2 = new List<string>();
                    //StrLst2 = (List<string>)ViewState["SelectedBrands"];
                    //for (int i = 0; i < dtBrand.Rows.Count; i++)
                    //{
                    //    if (!StrLst2.Contains(dtBrand.Rows[i]["VehicleBrandId"].ToString()))
                    //        dbCon.ExecuteQuery("Delete from [dbo].[SpareBrandMapping] where id=" + dtBrand.Rows[i]["Id"].ToString());
                    //}

                    DataTable dtVariant = dbCon.GetDataTable("Select * From SpareVariantMapping where  SpareId=" + Id);
                    List<string> StrLst3 = new List<string>();
                    StrLst3 = variantIds.Split(',').ToList();
                    for (int i = 0; i < dtVariant.Rows.Count; i++)
                    {
                        if (!StrLst3.Contains(dtVariant.Rows[i]["VehicleVariantId"].ToString()))
                            dbCon.ExecuteQuery("Delete from [dbo].[SpareVariantMapping] where id=" + dtVariant.Rows[i]["Id"].ToString());
                    }

                    //Repeated Please remove 
                    List<string> StrLst = new List<string>();
                    StrLst = modelIds.Split(',').ToList();
                    foreach (var s in StrLst)
                    {
                        if (int.Parse(s) > 0)
                        {
                            dbCon.ExecuteQuery("INSERT INTO [dbo].[SpareModelMapping] ([SpareId] ,[VehicleModelId] , [DOC] ,[DOM] ,[IsDelete]) VALUES (" + Id + "," + s + ",getdate(),getdate(),0)");
                        }
                    }
                    //StrLst = new List<string>();
                    //StrLst = (List<string>)ViewState["SelectedBrands"];
                    //foreach (var s in StrLst)
                    //{
                    //    if (int.Parse(s) > 0)
                    //    {
                    //        dbCon.ExecuteQuery("INSERT INTO [dbo].[SpareBrandMapping] ([SpareId] ,[VehicleBrandId] , [DOC] ,[DOM] ,[IsDelete]) VALUES (" + Id + "," + s + ",getdate(),getdate(),0)");
                    //    }
                    //}
                    StrLst = new List<string>();
                    StrLst = variantIds.Split(',').ToList();
                    foreach (var s in StrLst)
                    {
                        if (int.Parse(s) > 0)
                        {
                            dbCon.ExecuteQuery("INSERT INTO [dbo].[SpareVariantMapping] ([SpareId] ,[VehicleVariantId] , [DOC] ,[DOM] ,[IsDelete]) VALUES (" + Id + "," + s + ",getdate(),getdate(),0)");
                        }
                    }
                }
                //Emd Mapping
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " SavePartDetail Msg:" + e.Message, "", e.StackTrace);
        }
        obj.Id = "1";
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void GetPartDetailById(string id)
    {
        var obj = new PartDetail();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string[] strArr = { id };
            DataTable dt = dbCon.GetDataTableWithParams("SELECT [Name],[Code],[Category_Id],[TaxId],HSNNumber,isnull(IsGeneric,0) as IsGeneric FROM [dbo].[Spare] where id=@1", strArr);
            if (dt != null && dt.Rows.Count > 0)
            {
                obj.name = dt.Rows[0]["Name"].ToString();
                obj.code = dt.Rows[0]["Code"].ToString();
                obj.catid = dt.Rows[0]["Category_Id"].ToString();
                obj.taxid = dt.Rows[0]["TaxId"].ToString();
                obj.hsn = dt.Rows[0]["HSNNumber"].ToString();
                obj.isgeneric = dt.Rows[0]["IsGeneric"].ToString();
            }
            dt = dbCon.GetDataTableWithParams("SELECT distinct [VehicleModelId] FROM [dbo].[SpareModelMapping] where [IsDelete]=0  and SpareId=@1", strArr);
            var strModelIds = "0";
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strModelIds += "," + dt.Rows[i][0].ToString();
                }
            }
            dt = dbCon.GetDataTableWithParams("SELECT distinct [VehiclevariantId] FROM [dbo].[SparevariantMapping] where [IsDelete]=0 and SpareId=@1", strArr);
            var strVariantIds = "0";
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strVariantIds += "," + dt.Rows[i][0].ToString();
                }
            }
            obj.modelids = strModelIds;
            obj.variantids = strVariantIds;
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetModelByBrandId Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void GetAllForPurchase(string VendorId)
    {
        var objectToSerilize = new List<SpareNConsumable>();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };

        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            DataTable dt = new DataTable();
            String query = "SELECT Spare.Id,[Code]  AS Spares,PurchasePrice FROM [dbo].[Spare] where isnull(IsDeleted,0)=0 ";
            //if (!String.IsNullOrEmpty(VendorId))
            //{
            //    int vendorid = Convert.ToInt32(VendorId);
            //    if (vendorid > 0)
            //    {
            //        query += " inner join [Spare_Vendor_Mapping] on [Spare_Vendor_Mapping].SpareId=Spare.Id where VendorId =@1";
            //        string[] Vendorid = { vendorid.ToString() };
            //        dt = dbCon.GetDataTableWithParams(query, Vendorid);
            //    }
            //    else
            //    {
            //        dt = dbCon.GetDataTable(query);
            //    }
            //}
            //else
            //{
            //    dt = dbCon.GetDataTable(query);
            //}
            dt = dbCon.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var model = new SpareNConsumable();
                    string id = dr["Id"].ToString();
                    model.Id = id;
                    model.Name = dr["Spares"].ToString();
                    model.PurchasePrice = dr["PurchasePrice"].ToString();
                    model.type = "Spare";
                    objectToSerilize.Add(model);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetAllForPurchase Msg:" + e.Message, "", e.StackTrace);
        }
        //try
        //{
        //    DataTable dt = new DataTable();
        //    String query = "SELECT [Id],[Name],[PurchasePrice] FROM [dbo].[Consumables]";
        //    dt = dbCon.GetDataTable(query);
        //    if (dt != null && dt.Rows.Count > 0)
        //    {
        //        foreach (DataRow dr in dt.Rows)
        //        {
        //            var model = new SpareNConsumable();
        //            string id = dr["Id"].ToString();
        //            model.Id = id;
        //            model.Name = dr["Name"].ToString();
        //            model.PurchasePrice = dr["PurchasePrice"].ToString();
        //            model.type = "Consumable";
        //            objectToSerilize.Add(model);
        //        }
        //    }
        //}
        //catch (Exception e)
        //{
        //    dbCon.InsertLogs(ErrorMsgPrefix + " GetAllForPurchases Msg:" + e.Message, "", e.StackTrace);
        //}
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void GetVendors()
    {
        var objectToSerilize = new List<Objs>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string query = "SELECT [Id],[Name]  FROM [dbo].[Vendor] where isnull(IsDeleted,0)=0";
            DataTable dt = dbCon.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var Spares = new Objs();
                    Spares.Id = dr["Id"].ToString();
                    Spares.Name = dr["Name"].ToString();
                    objectToSerilize.Add(Spares);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetVendors Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }
    [WebMethod]
    public void GetMrp(string id)
    {
        var objectToSerilize = new Objs();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string query = "SELECT  Spare.Price, TaxCategory.Name FROM Spare INNER JOIN TaxCategory ON Spare.TaxId = TaxCategory.Id where Spare.id=" + id;
            DataTable dt = dbCon.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    objectToSerilize.Id = dr[0].ToString();
                    objectToSerilize.Name = dr[1].ToString();
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetVendors Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void GetSpareBrand()
    {
        var objectToSerilize = new List<Objs>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string query = "SELECT [Id],[Name]  FROM [dbo].[Spare_Brand] where IsDelete=0";
            DataTable dt = dbCon.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var Spares = new Objs();
                    Spares.Id = dr["Id"].ToString();
                    Spares.Name = dr["Name"].ToString();
                    objectToSerilize.Add(Spares);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetSpareBrand Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void GetConsumableBrand()
    {
        var objectToSerilize = new List<Objs>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string query = "SELECT [Id],[Name]  FROM [dbo].[Consumables_Brand] where IsDelete=0";
            DataTable dt = dbCon.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var Spares = new Objs();
                    Spares.Id = dr["Id"].ToString();
                    Spares.Name = dr["Name"].ToString();
                    objectToSerilize.Add(Spares);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetConsumableBrand Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void GetAllJobcardsForGrn()
    {
        var objectToSerilize = new List<Objs>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string query = "SELECT [Id],Convert(varchar,[Id])+' ,'+isnull((SELECT  Vehicle_Model.Name + ' ,' + Vehicle.Number AS Expr1 FROM  Vehicle_Model RIGHT OUTER JOIN Vehicle ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id WHERE (Vehicle.Id = [JobCard].Vehicle_Id)),'') As [Name] FROM [dbo].[JobCard]  where  isnull(IsDelete,0)=0   order by id desc";
            DataTable dt = dbCon.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var Spares = new Objs();
                    Spares.Id = dr["Id"].ToString();
                    Spares.Name = dr["Name"].ToString();
                    objectToSerilize.Add(Spares);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetAllJobcardsForGrn Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }


    [WebMethod]
    public void AddGrn(string VendorId, string strSpare, string strQty, string strPri, string strType, string strMrp, string strBMrp, string strbrand, string inv, string recby, string rmk, string strDis, string strDt, string dcn, string userId, string workshopId)
    {
        var objectToSerilize = new Objs();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            DateTime date = DateTime.ParseExact(strDt, "dd-MM-yyyy hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);
            string[] strArr = { };
            int grnid = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[GRN]([DOC],[DOM],[ReceivedBy],[VendorId],[RefNumber],[Remark],[InvoiceNo],DeliveryDate,dcno,UserId,WorkshopId) VALUES('" + dbCon.getindiantime().ToString("MM-dd-yyyy hh:mm tt") + "','" + dbCon.getindiantime().ToString("MM-dd-yyyy hh:mm tt") + "','" + recby + "','" + VendorId + "',0,'" + rmk + "','" + inv + "','" + date.ToString("MM-dd-yyyy hh:mm tt") + "','" + dcn + "'," + userId + "," + workshopId + ") SELECT SCOPE_IDENTITY();", strArr);
            string[] strArrSpare = strSpare.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrQty = strQty.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrPri = strPri.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrMrp = strMrp.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrBMrp = strBMrp.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrBrand = strbrand.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrDis = strDis.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrtype = strType.Split(new string[] { "$#@" }, StringSplitOptions.None);

            for (int i = 0; i < strArrSpare.Length; i++)
            {
                int grnDid = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[GRNDetail] (GRNId,[EntityId] , [EntityType] ,[Qty]  ,[Price],[Discount],[BrandId],[DOC],UserId,WorkshopId) VALUES ('" + grnid + "','" + strArrSpare[i] + "' , '" + strArrtype[i] + "' ,'" + strArrQty[i] + "','" + strArrPri[i] + "','" + strArrDis[i] + "','" + strArrBrand[i] + "','" + dbCon.getindiantime().ToString("MM-dd-yyyy hh:mm tt") + "'," + userId + "," + workshopId + ") SELECT SCOPE_IDENTITY();", strArr);
                if (strArrtype[i] == "Consumable")
                {
                    DataTable dt = dbCon.GetDataTable("SELECT id,[Qty] FROM [dbo].[ConsumableConsumableBrandMapping] where [ConsumableId]=" + strArrSpare[i] + " and [BrandId]=" + strArrBrand[i]);
                    int ide = 0;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dbCon.ExecuteQuery("update ConsumableConsumableBrandMapping set qty=(qty+" + strArrQty[i] + ") where id=" + dt.Rows[0][0].ToString());
                        ide = int.Parse(dt.Rows[0][0].ToString());
                    }
                    else
                    {
                        ide = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[ConsumableConsumableBrandMapping] ([ConsumableId],[BrandId],[Qty],[MinQty],[MaxQty],[IsActive],[IsDeleted],[DOC],[DOM],[HSNNumber])VALUES('" + strArrSpare[i] + "','" + strArrBrand[i] + "','" + strArrQty[i] + "',1,1000,1,0,getdate(),getdate(),'')  SELECT SCOPE_IDENTITY();", strArr);
                    }
                    string qry = "INSERT INTO [dbo].[ConsumableInventaryHistory]([ConsumableConsumableBrandMappingId],[Cr],[Dr],[DOC],[JobCardId],[GRNDetailId])VALUES (" + strArrSpare[i] + "," + strArrQty[i] + ",0,CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')),0," + grnDid + ")";
                    dbCon.ExecuteQueryWithParams(qry, strArr);

                    dbCon.ExecuteQuery("Update Consumables Set Price=" + strArrMrp[i] + " where id=" + strArrSpare[i]);

                }
                else
                {

                    DataTable dt = dbCon.GetDataTable("SELECT id,[Qty] FROM [dbo].[SpareSpareBrandMapping] where [SpareId]=" + strArrSpare[i] + " and [BrandId]=" + strArrBrand[i]);
                    int ide = 0;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dbCon.ExecuteQuery("update SpareSpareBrandMapping set Mrp='" + strArrBMrp[i] + "', qty=(qty+" + strArrQty[i] + ") where id=" + dt.Rows[0][0].ToString());
                        ide = int.Parse(dt.Rows[0][0].ToString());
                    }
                    else
                    {
                        ide = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[SpareSpareBrandMapping] ([SpareId],[BrandId],[Qty],[MinQty],[MaxQty],[IsActive],[IsDeleted],[DOC],[DOM],[HSNNumber],Mrp)VALUES(" + strArrSpare[i] + "," + strArrBrand[i] + ",'" + strArrQty[i] + "',1,1000,1,0,getdate(),getdate(),'','" + strArrBMrp[i] + "')  SELECT SCOPE_IDENTITY();", strArr);
                    }
                    string msg = strArrQty[i] + " Qty Credited in invetory >> GrnId=" + grnid + " Invoice Number:" + inv + " Received By:" + recby;
                    string qry = "INSERT INTO [dbo].[SpareInventaryHistory]([SpareSpareBrandMappingId],[Cr],[Dr],[DOC],[JobCardId],[GRNDetailId],[Message]) VALUES (" + ide + ",'" + strArrQty[i] + "',0,CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')),0," + grnDid + ",'" + msg.Replace("'", "''") + "')";
                    dbCon.ExecuteQueryWithParams(qry, strArr);
                    dbCon.ExecuteQuery("Update Spare Set Price='" + strArrMrp[i] + "' where id=" + strArrSpare[i]);
                }
            }
            String msgstr = "Inventary Updated GrnId - " + grnid + " ";
            int nItems = 0;
            try
            {
                nItems = int.Parse(dbCon.GetDataTable("select count(id) from grndetail where grnid=" + grnid).Rows[0][0].ToString());
                if (nItems > 0)
                    msgstr += " Items Inserted - " + nItems.ToString();
                else
                    msgstr = "Something went wrong please contact I.T. Team";
            }
            catch (Exception E) { }
            try
            {
                UserActivity objUserAct = new UserActivity();
                objUserAct.InsertUserActivity("Added New Grn On : " + dbCon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss tt") + " GRN ID Is : " + grnid + " & Items Inserted : " + nItems.ToString(), userId, workshopId, "", "Insert", "GRN");
            }
            catch (Exception E) { }
            objectToSerilize.Name = msgstr;
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " AddGrn Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void AddGrnWithAllocation(string VendorId, string strSpare, string strQty, string strPri, string strType, string strMrp, string strBMrp, string strbrand, string inv, string recby, string rmk, string strDis, string strDt, string dcn, string userId, string workshopId, string strFor, string strConsumption, string Rate,
string Sgstp, string SgstA, string Igstp, string IgstA, string Taxable, string Total)
    {

        //Should Use Transaction
        var objectToSerilize = new Objs();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            DateTime date = DateTime.ParseExact(strDt, "dd-MM-yyyy hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);
            string[] strArr = { };
            int grnid = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[GRN]([DOC],[DOM],[ReceivedBy],[VendorId],[RefNumber],[Remark],[InvoiceNo],DeliveryDate,dcno,UserId,WorkshopId) VALUES('" + dbCon.getindiantime().ToString("MM-dd-yyyy hh:mm tt") + "','" + dbCon.getindiantime().ToString("MM-dd-yyyy hh:mm tt") + "','" + recby + "','" + VendorId + "',0,'" + rmk + "','" + inv + "','" + date.ToString("MM-dd-yyyy hh:mm tt") + "','" + dcn + "'," + userId + "," + workshopId + ") SELECT SCOPE_IDENTITY();", strArr);
            string[] strArrSpare = strSpare.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrQty = strQty.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrPri = strPri.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrMrp = strMrp.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrBMrp = strBMrp.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrBrand = strbrand.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrDis = strDis.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrtype = strType.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrFor = strFor.Replace("STOCK", "0").Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrConsumption = strConsumption.Split(new string[] { "$#@" }, StringSplitOptions.None);

            /////////////////
            string[] strArrRate = Rate.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrSgstp = Sgstp.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrSgstA = SgstA.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrIgstp = Igstp.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrIgstA = IgstA.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrTaxable = Taxable.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrTotal = Total.Split(new string[] { "$#@" }, StringSplitOptions.None);
            ///////////////////////
            for (int i = 0; i < strArrSpare.Length; i++)
            {
                float consumption = float.Parse(strArrConsumption[i]);
                float qty = float.Parse(strArrQty[i]);
                float stock = qty - consumption;

                int grnDid = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[GRNDetail] (GRNId,[EntityId] , [EntityType] ,[Qty]  ,[Price],[Discount],[BrandId],[DOC],UserId,WorkshopId,JobcardId,Consumption,Stock,[VendorInvoiceRate],[VendorInvoiceDiscount],[VendorInvoiceSGST],[VendorInvoiceSGSTAmount],[VendorInvoiceCGST],[VendorInvoiceCGSTAmount],[VendorInvoiceIGST],[VendorInvoiceIGSTAmount],[VendorInvoiceTaxable],[VendorInvoiceTotal],ActualAmountPerUnit) VALUES ('" + grnid + "','" + strArrSpare[i] + "' , '" + strArrtype[i] + "' ,'" + strArrQty[i] + "','" + strArrPri[i] + "','" + strArrDis[i] + "','" + strArrBrand[i] + "','" + dbCon.getindiantime().ToString("MM-dd-yyyy hh:mm tt") + "'," + userId + "," + workshopId + "," + strArrFor[i] + "," + consumption + "," + stock + "," + strArrRate[i] + "," + strArrDis[i] + "," + strArrSgstp[i] + "," + strArrSgstA[i] + "," + strArrSgstp[i] + "," + strArrSgstA[i] + "," + strArrIgstp[i] + "," + strArrIgstA[i] + "," + strArrTaxable[i] + "," + strArrTotal[i] + "," + strArrMrp[i] + ") SELECT SCOPE_IDENTITY();", strArr);
                if (strArrtype[i] == "Consumable")
                {
                    //DataTable dt = dbCon.GetDataTable("SELECT id,[Qty] FROM [dbo].[ConsumableConsumableBrandMapping] where [ConsumableId]=" + strArrSpare[i] + " and [BrandId]=" + strArrBrand[i]);
                    //int ide = 0;
                    //if (dt != null && dt.Rows.Count > 0)
                    //{
                    //    dbCon.ExecuteQuery("update ConsumableConsumableBrandMapping set qty=(qty+" + strArrQty[i] + ") where id=" + dt.Rows[0][0].ToString());
                    //    ide = int.Parse(dt.Rows[0][0].ToString());
                    //}
                    //else
                    //{
                    //    ide = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[ConsumableConsumableBrandMapping] ([ConsumableId],[BrandId],[Qty],[MinQty],[MaxQty],[IsActive],[IsDeleted],[DOC],[DOM],[HSNNumber])VALUES('" + strArrSpare[i] + "','" + strArrBrand[i] + "','" + strArrQty[i] + "',1,1000,1,0,getdate(),getdate(),'')  SELECT SCOPE_IDENTITY();", strArr);
                    //}
                    //string qry = "INSERT INTO [dbo].[ConsumableInventaryHistory]([ConsumableConsumableBrandMappingId],[Cr],[Dr],[DOC],[JobCardId],[GRNDetailId])VALUES (" + strArrSpare[i] + "," + strArrQty[i] + ",0,CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')),0," + grnDid + ")";
                    //dbCon.ExecuteQueryWithParams(qry, strArr);

                    //dbCon.ExecuteQuery("Update Consumables Set Price=" + strArrMrp[i] + " where id=" + strArrSpare[i]);

                }
                else
                {
                    DataTable dt = dbCon.GetDataTable("SELECT id,[Qty] FROM [dbo].[SpareSpareBrandMapping] where [SpareId]=" + strArrSpare[i] + " and [BrandId]=" + strArrBrand[i]);
                    int ide = 0;
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        dbCon.ExecuteQuery("update SpareSpareBrandMapping set Mrp='" + strArrBMrp[i] + "', qty=(qty+" + stock + ") where id=" + dt.Rows[0][0].ToString());
                        ide = int.Parse(dt.Rows[0][0].ToString());
                    }
                    else
                    {
                        ide = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[SpareSpareBrandMapping] ([SpareId],[BrandId],[Qty],[MinQty],[MaxQty],[IsActive],[IsDeleted],[DOC],[DOM],[HSNNumber],Mrp)VALUES(" + strArrSpare[i] + "," + strArrBrand[i] + ",'" + stock + "',1,1000,1,0,getdate(),getdate(),'','" + strArrBMrp[i] + "')  SELECT SCOPE_IDENTITY();", strArr);
                    }
                    string msg = stock + " Qty Credited in invetory >> GrnId=" + grnid + " Invoice Number:" + inv + " Received By:" + recby;
                    string qry = "INSERT INTO [dbo].[SpareInventaryHistory]([SpareSpareBrandMappingId],[Cr],[Dr],[DOC],[JobCardId],[GRNDetailId],[Message]) VALUES (" + ide + ",'" + stock + "',0,CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')),0," + grnDid + ",'" + msg.Replace("'", "''") + "')";
                    dbCon.ExecuteQueryWithParams(qry, strArr);
                    dbCon.ExecuteQuery("Update Spare Set Price='" + strArrMrp[i] + "' where id=" + strArrSpare[i]);
                }
            }


            #region  Add Items Into Customers Invoice Change
            DataTable dtJobcardids = dbCon.GetDataTable("SELECT Distinct [JobcardId] FROM [dbo].[GRNDetail]  where JobcardId>0 and  grnid=" + grnid);
            if (dtJobcardids != null && dtJobcardids.Rows.Count > 0)
            {
                for (int i = 0; i < dtJobcardids.Rows.Count; i++)
                {
                    int InvoiceId = 0;

                    bool IsInvoiceNumberGenerated = false;

                    DataTable dtInvoice = dbCon.GetDataTable("SELECT [Id] FROM [dbo].[Invoice] where JobCardId=" + dtJobcardids.Rows[i][0].ToString() + " and isnull(IsCancelled,0)=0 and (GstInvoiceNumber is not null or InvoiceNumber is not null )");
                    if (dtInvoice != null && dtInvoice.Rows.Count > 0)
                    {
                        IsInvoiceNumberGenerated = true;
                    }
                    if (!IsInvoiceNumberGenerated)
                    {
                        dtInvoice = dbCon.GetDataTable("SELECT [Id] FROM [dbo].[Invoice] where JobCardId=" + dtJobcardids.Rows[i][0].ToString() + " and Type=1 and isnull(IsCancelled,0)=0 ");

                        if (dtInvoice != null && dtInvoice.Rows.Count > 0)
                        {
                            InvoiceId = Convert.ToInt32(dtInvoice.Rows[0][0].ToString());
                        }
                        else
                        {
                            try
                            {
                                InvoiceId = Convert.ToInt32(dbCon.GetDataTable("Select Max(Id)+1 from Invoice").Rows[0][0].ToString());
                            }
                            catch (Exception E)
                            {
                                InvoiceId = 1;
                            }
                            dbCon.ExecuteQuery("INSERT INTO [dbo].[Invoice] ([Id],[JobCardId],[WorkshopId],[DOC],[DOM],[Type],[IncludeInGst]) values (" + InvoiceId + "," + dtJobcardids.Rows[i][0].ToString() + ",1,'" + dbCon.getindiantime().ToString("dd-MMM-yyyy HH:ss:mm") + "','" + dbCon.getindiantime().ToString("dd-MMM-yyyy HH:ss:mm") + "',1,1)");
                        }
                        DataTable dtGrnItems = dbCon.GetDataTable("SELECT [Id],Consumption as [Qty],[Price],[JobcardId],[EntityId] FROM [dbo].[GRNDetail]  where JobcardId=" + dtJobcardids.Rows[i][0].ToString() + " and grnid=" + grnid);


                        if (dtGrnItems != null)
                        {
                            for (int j = 0; j < dtGrnItems.Rows.Count; j++)
                            {
                                DataTable dtCalc = dbCon.GetDataTable("Select Price,Convert(decimal(18,2),(isnull(TaxValue,0)/2)) as TaxVal,Convert(decimal(18,2),((Spare.Price*TaxValue)/(100+TaxValue))/2) as TaxAmt,Convert(decimal(18,2),(Price -((Spare.Price*TaxValue)/(100+TaxValue)))) as Taxable  FROM TaxCategory RIGHT OUTER JOIN Spare ON Spare.TaxId = TaxCategory.Id where Spare.id=" + dtGrnItems.Rows[j]["EntityId"].ToString());

                                dbCon.ExecuteQuery("INSERT INTO [dbo].[Invoice_Spare_Mapping]([InvoiceId],[SpareId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId],[GrnDetailId],Mrp,Depreciation) VALUES (" + InvoiceId + "," + dtGrnItems.Rows[j]["EntityId"].ToString() + ",'" + dtCalc.Rows[0]["TaxVal"].ToString() + "','" + dtCalc.Rows[0]["TaxAmt"].ToString() + "','" + dtCalc.Rows[0]["TaxVal"].ToString() + "','" + dtCalc.Rows[0]["TaxAmt"].ToString() + "',0,'" + dtCalc.Rows[0]["Price"].ToString() + "','" + dtGrnItems.Rows[j]["Qty"].ToString() + "',0,(" + dtCalc.Rows[0]["Price"].ToString() + "*" + dtGrnItems.Rows[j]["Qty"].ToString() + "),'" + dbCon.getindiantime().ToString("dd-MMM-yyyy HH:ss:mm") + "',1,0," + dtGrnItems.Rows[j]["Id"].ToString() + "," + dtCalc.Rows[0]["Price"] + ",100)");
                            }
                        }
                    }
                }
            }
            #endregion

            String msgstr = "Inventary Updated GrnId - " + grnid + " ";
            int nItems = 0;
            try
            {
                nItems = int.Parse(dbCon.GetDataTable("select count(id) from grndetail where grnid=" + grnid).Rows[0][0].ToString());
                if (nItems > 0)
                    msgstr += " Items Inserted - " + nItems.ToString();
                else
                    msgstr = "Something went wrong please contact I.T. Team";
            }
            catch (Exception E) { }
            try
            {
                UserActivity objUserAct = new UserActivity();
                objUserAct.InsertUserActivity("Added New Grn On : " + dbCon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss tt") + " GRN ID Is : " + grnid + " & Items Inserted : " + nItems.ToString(), userId, workshopId, "", "Insert", "GRN");
            }
            catch (Exception E) { }
            objectToSerilize.Name = msgstr;
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " AddGrn Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }
    public void AddMotorzLog(string _Log)
    {
        dbCon.ExecuteQuery("INSERT INTO [dbo].[MototrzLog]([Doc],[MyLog])VALUES ('" + dbCon.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss") + "','" + _Log + "')");
    }

    [WebMethod]
    public void AddGrn_v1WithAllocation(string VendorId, string strSpare, string strQty, string strPri, string strType, string strMrp, string strBMrp, string strbrand, string inv, string recby, string rmk, string strDis, string strDt, string dcn, string GrnId, string strGrnDtid, string userId, string workshopId, string strFor, string strConsumption, string Rate, string Sgstp, string SgstA, string Igstp, string IgstA, string Taxable, string Total)
    {
        var objectToSerilize = new Objs();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {


            DateTime date = DateTime.ParseExact(strDt, "dd-MM-yyyy HH:mm tt", System.Globalization.CultureInfo.InvariantCulture);
            //string[] strArr = { };
            //int grnid = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[GRN]([DOC],[DOM],[ReceivedBy],[VendorId],[RefNumber],[Remark],[InvoiceNo],DeliveryDate,dcno,UserId,WorkshopId) VALUES('" + dbCon.getindiantime().ToString("MM-dd-yyyy hh:mm tt") + "','" + dbCon.getindiantime().ToString("MM-dd-yyyy hh:mm tt") + "','" + recby + "','" + VendorId + "',0,'" + rmk + "','" + inv + "','" + date.ToString("MM-dd-yyyy hh:mm tt") + "','" + dcn + "'," + userId + "," + workshopId + ") SELECT SCOPE_IDENTITY();", strArr);
            string[] strArr = { };
            string[] strArr1 = { };
            int grnid = 0;
            DataTable dtdetail = new DataTable();
            if (!GrnId.Equals("0") && !GrnId.Equals("-1"))
            {
                string[] strArr4 = { GrnId };
                dtdetail = dbCon.GetDataTableWithParams("SELECT  entityid as spareid,BrandId,GRNDetail.Id,GRN.Id as grnId,GRNDetail.EntityType, case when GRNDetail.EntityType='spare' then (select Code from motorz.dbo.spare where id=entityid ) else (select name from Consumables where id=entityid ) end as [Part],GRNDetail.Qty, GRNDetail.Price As PurchasePrice,  GRNDetail.Discount,case when GRNDetail.EntityType='spare' then (select price from motorz.dbo.spare where id=entityid ) else (select price from Consumables where id=entityid ) end as PartMrp,case when GRNDetail.EntityType='spare' then (select Name from motorz.dbo.Spare_Brand where id=BrandId ) else (select Name from Consumables_Brand where id=BrandId ) end as BrandName FROM GRN INNER JOIN GRNDetail ON GRN.Id = GRNDetail.GRNId where grn.id=@1", strArr4);

                grnid = dbCon.ExecuteScalarQueryWithParams("update [dbo].[GRN] set [ReceivedBy]='" + recby + "',[VendorId]='" + VendorId + "',[Remark]='" + rmk.ToString() + "',[InvoiceNo]='" + inv + "',[DeliveryDate]='" + date.ToString("MM-dd-yyyy hh:mm tt") + "',DCNo='" + dcn + "',UserId='" + userId + "',WorkshopId='" + workshopId + "'  where id=" + GrnId, strArr1);
                grnid = int.Parse(GrnId.ToString());
            }
            else
            {
                //grnid = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[GRN]([DOC],[DOM],[ReceivedBy],[VendorId],[RefNumber],[Remark],[InvoiceNo],DeliveryDate,dcno,UserId,WorkshopId) VALUES('" + dbCon.getindiantime().ToString("MM-dd-yyyy hh:mm tt") + "','" + dbCon.getindiantime().ToString("MM-dd-yyyy hh:mm tt") + "','" + recby + "','" + VendorId + "',0,'" + rmk + "','" + inv + "','" + date.ToString("MM-dd-yyyy hh:mm tt") + "','" + dcn + "'," + userId + "," + workshopId + ") SELECT SCOPE_IDENTITY();", strArr);

                //grnid = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[GRN]([DOC],[DOM],[ReceivedBy],[VendorId],[RefNumber],[Remark],[InvoiceNo],DeliveryDate,dcno) VALUES('" + dbCon.getindiantime().ToString("MM-dd-yyyy hh:mm tt") + "','" + dbCon.getindiantime().ToString("MM-dd-yyyy hh:mm tt") + "','" + recby + "','" + VendorId + "',0,'" + rmk + "','" + inv + "','" + date.ToString("MM-dd-yyyy hh:mm tt") + "','" + dcn + "') SELECT SCOPE_IDENTITY();", strArr);
            }


            string[] strArrSpare = strSpare.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrQty = strQty.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrPri = strPri.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrMrp = strMrp.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrBMrp = strBMrp.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrBrand = strbrand.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrDis = strDis.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrtype = strType.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrGrndtId = strGrnDtid.Split(new string[] { "$#@" }, StringSplitOptions.None);

            string[] strArrFor = strFor.Replace("STOCK", "0").Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrConsumption = strConsumption.Split(new string[] { "$#@" }, StringSplitOptions.None);

            /////////////////
            string[] strArrRate = Rate.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrSgstp = Sgstp.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrSgstA = SgstA.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrIgstp = Igstp.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrIgstA = IgstA.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrTaxable = Taxable.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrTotal = Total.Split(new string[] { "$#@" }, StringSplitOptions.None);
            ///////////////////////

            if (grnid > 0)
            {
                for (int i = 0; i < strArrSpare.Length; i++)
                {
                    float consumption = float.Parse(strArrConsumption[i]);
                    float qty = float.Parse(strArrQty[i]);
                    float stock = qty - consumption;
                    AddMotorzLog(strArrGrndtId[i] + " Step 1");
                    if (strArrGrndtId[i].Equals("-1"))
                    {
                        AddMotorzLog(strArrGrndtId[i] + " Step 2.1");
                        string[] strarr = { };
                        string str1 = "insert into GRNDetail (EntityId,EntityType,Qty,Price,Discount,BrandId,DOC,GRNId,WorkshopId,JobcardId,Consumption,Stock,[VendorInvoiceRate],[VendorInvoiceDiscount],[VendorInvoiceSGST],[VendorInvoiceSGSTAmount],[VendorInvoiceCGST],[VendorInvoiceCGSTAmount],[VendorInvoiceIGST],[VendorInvoiceIGSTAmount],[VendorInvoiceTaxable],[VendorInvoiceTotal],ActualAmountPerUnit) values ('" + strArrSpare[i] + "','spare','" + strArrQty[i] + "','" + strArrPri[i] + "','" + strArrDis[i] + "','" + strArrBrand[i] + "','" + dbCon.getindiantime() + "','" + grnid + "',1," + strArrFor[i] + "," + consumption + "," + stock + "," + strArrRate[i] + "," + strArrDis[i] + "," + strArrSgstp[i] + "," + strArrSgstA[i] + "," + strArrSgstp[i] + "," + strArrSgstA[i] + "," + strArrIgstp[i] + "," + strArrIgstA[i] + "," + strArrTaxable[i] + "," + strArrTotal[i] + "," + strArrMrp[i] + ") SELECT SCOPE_IDENTITY(); ";

                        int value1 = dbCon.ExecuteScalarQueryWithParams(str1, strarr);
                        //hfid.Value = value1.ToString();

                        if (strArrMrp[i] != null && !String.IsNullOrWhiteSpace(strArrMrp[i]))
                            dbCon.ExecuteQuery("UPDATE [Motorz].[dbo].[Spare] SET [Price] = '" + strArrMrp[i] + "' where id=(select entityid from GRNDetail where id=" + value1 + ")");

                        string str = "select top 1 id,Qty from SpareSpareBrandMapping where SpareId=" + strArrSpare[i] + " and BrandId=" + strArrBrand[i] + " ";
                        DataTable dt1 = new DataTable();
                        dt1 = dbCon.GetDataTable(str);
                        if (dt1 != null && dt1.Rows.Count > 0)
                        {
                            //update nd select
                            long SpareSpareBrandId = long.Parse(dt1.Rows[0]["id"].ToString());
                            float quty = float.Parse(dt1.Rows[0]["Qty"].ToString()) + stock;
                            str1 = "update SpareSpareBrandMapping set Qty= " + quty + ",DOM='" + dbCon.getindiantime() + "' where id=" + SpareSpareBrandId + "";
                            int retval = dbCon.ExecuteQuery(str1);

                            str1 = "INSERT INTO [dbo].[SpareInventaryHistory] ([SpareSpareBrandMappingId],[Cr],[Dr],[DOC],[JobCardId],[OrderItemDetailId],[GRNDetailId],[RequsitionSpareId],[WorkshopId]) VALUES(" + SpareSpareBrandId + ",'" + stock + "',0,'" + dbCon.getindiantime() + "',0,0," + value1 + ",0,1)";
                            retval = dbCon.ExecuteQuery(str1);
                        }
                        else
                        {
                            //insert 
                            string[] strarr1 = { };
                            int SpareSpareBrandId = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[SpareSpareBrandMapping] ([SpareId],[BrandId],[Qty],[MinQty],[MaxQty],[IsActive],[IsDeleted],[DOC],[DOM],[HSNNumber],Mrp,WorkshopId) VALUES('" + strArrSpare[i] + "','" + strArrBrand[i] + "','" + stock + "',1,1000,1,0,'" + dbCon.getindiantime() + "','" + dbCon.getindiantime() + "','','" + strArrMrp[i] + "',1)  SELECT SCOPE_IDENTITY();", strarr1);
                            str1 = "INSERT INTO [dbo].[SpareInventaryHistory] ([SpareSpareBrandMappingId],[Cr],[Dr],[DOC],[JobCardId],[OrderItemDetailId],[GRNDetailId],[RequsitionSpareId],[WorkshopId]) VALUES(" + SpareSpareBrandId + ",'" + stock + "',0,'" + dbCon.getindiantime() + "',0,0," + value1 + ",0,1)";
                            int retval = dbCon.ExecuteQuery(str1);
                        }
                    }
                    else
                    {
                        AddMotorzLog(strArrGrndtId[i] + " Step 2.2");
                        string str = "SELECT [EntityId],[EntityType] ,isnull([Stock],0) as [Qty],[Price],[Discount],[BrandId],[DOC],[GRNId],[WorkshopId],(select top 1 id from SpareSpareBrandMapping where BrandId=[GRNDetail].BrandId and SpareId=EntityId) as SpareBrandId FROM [dbo].[GRNDetail] where [Id]=" + strArrGrndtId[i];
                        DataTable dt = new DataTable();
                        dt = dbCon.GetDataTable(str);
                        AddMotorzLog(strArrGrndtId[i] + " & Count -" + dt.Rows.Count.ToString());
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            AddMotorzLog("strArrMrp " + strArrMrp[i] + " strArrQty " + strArrQty[i]);
                            string strQryGrn = "UPDATE [dbo].[GRNDetail] SET BrandId='" + strArrBrand[i] + "',EntityId='" + strArrSpare[i] + "', [Price] ='" + strArrPri[i] + "',Discount='" + strArrDis[i] + "',Qty='" + strArrQty[i] + "',GRNDetail.VendorInvoiceRate=" + strArrRate[i] + ",GRNDetail.VendorInvoiceCGST=" + strArrSgstp[i] + ",GRNDetail.VendorInvoiceCGSTAmount=" + strArrSgstA[i] + ",GRNDetail.VendorInvoiceSGST=" + strArrSgstp[i] + ",GRNDetail.VendorInvoiceSGSTAmount=" + strArrSgstA[i] + ",GRNDetail.VendorInvoiceIGST=" + strArrIgstp[i] + ",GRNDetail.VendorInvoiceIGSTAmount=" + strArrIgstA[i] + ",GRNDetail.VendorInvoiceTaxable=" + strArrTaxable[i] + ",GRNDetail.VendorInvoiceTotal=" + strArrTotal[i] + ",GRNDetail.JobcardId=" + strArrFor[i] + ",GRNDetail.Consumption=" + consumption + ",GRNDetail.Stock=" + stock + ",ActualAmountPerUnit=" + strArrMrp[i] + "  WHERE id=" + strArrGrndtId[i];
                            // AddMotorzLog(strQryGrn.Replace("'","''"));
                            if (strArrMrp[i] != null && !String.IsNullOrWhiteSpace(strArrMrp[i]) && strArrQty[i] != null && !String.IsNullOrWhiteSpace(strArrQty[i]))
                            {
                                //   AddMotorzLog(strQryGrn);
                                dbCon.ExecuteQuery(strQryGrn);
                            }
                            //SpareBrandId Setting
                            string SpareSpareBrandId = dt.Rows[0]["SpareBrandId"].ToString();
                            if (String.IsNullOrWhiteSpace(SpareSpareBrandId))
                            {
                                string[] strArr3 = { };
                                SpareSpareBrandId = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[SpareSpareBrandMapping] ([SpareId],[BrandId],[Qty],[MinQty],[MaxQty],[IsActive],[IsDeleted],[DOC],[DOM],[HSNNumber],Mrp)VALUES(" + strArrSpare[i] + "," + strArrBrand[i] + ",0,1,1000,1,0,getdate(),getdate(),'','" + strArrMrp[i] + "')  SELECT SCOPE_IDENTITY();", strArr3).ToString();
                            }
                            string SpareSpareBrandIdNew = "0";
                            //Update Customer Mrp
                            if (strArrMrp[i] != null && !String.IsNullOrWhiteSpace(strArrMrp[i]))
                                dbCon.ExecuteQuery("UPDATE [Motorz].[dbo].[Spare] SET [Price] = " + strArrMrp[i] + " where id=(select entityid from GRNDetail where id=" + strArrGrndtId[i] + ")");
                            //Take Previous values
                            string pqty = dt.Rows[0]["qty"].ToString();
                            string pspareid = dt.Rows[0]["EntityId"].ToString();
                            string pbrandid = dt.Rows[0]["BrandId"].ToString();
                            //New Values 
                            string spareid = strArrSpare[i];
                            string brandid = strArrBrand[i];
                            //Now Change/Update Inventory Casses
                            //Case 1 Just Qty Change nothing more (when same spare same brand)
                            float Diff_In_Qty = (float.Parse(pqty) - stock);
                            if (Diff_In_Qty < 0)
                                Diff_In_Qty = (stock - float.Parse(pqty));
                            if ((float.Parse(pspareid) == float.Parse(spareid)) && (float.Parse(pbrandid) == float.Parse(brandid)))
                            {
                                //Now two casses either more or lesser than previous
                                if ((float.Parse(pqty) < stock))
                                {
                                    //Need to add in Invetory                             
                                    string msg = Diff_In_Qty.ToString() + " Qty Added in store inventory on " + dbCon.getindiantime().ToString() + " ( Previous qty was " + pqty + " ,& New qty is " + stock + " )";
                                    //Credit Diff Qty into brand_wise_spare
                                    if (dbCon.ExecuteQuery("UPDATE [dbo].[SpareSpareBrandMapping] SET [Qty] = (isnull(Qty,0)+" + Diff_In_Qty + ") where [Id] = " + SpareSpareBrandId) > 0)
                                    {
                                        string query = "INSERT INTO [dbo].[SpareInventaryHistory] ([SpareSpareBrandMappingId],[Cr],[Dr],[DOC],[JobCardId],[OrderItemDetailId],[GRNDetailId],[RequsitionSpareId],[WorkshopId],message) VALUES(" + SpareSpareBrandId + "," + Diff_In_Qty + ",0,CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')),0,0," + strArrGrndtId[i] + ",0,1,'" + msg.Replace("'", "''") + "')";
                                        if (dbCon.ExecuteQuery(query) > 0)
                                        {

                                        }
                                    }
                                    //Add Into History table
                                }
                                else if ((float.Parse(pqty) > stock))
                                {
                                    //Need to remove from Invetory
                                    string msg = Diff_In_Qty.ToString() + " Qty Removed from store inventory on " + dbCon.getindiantime().ToString() + " ( Previous qty was " + pqty + " ,& New qty is " + stock + " )";
                                    //Debit Diff Qty into brand_wise_spare
                                    if (dbCon.ExecuteQuery("UPDATE [dbo].[SpareSpareBrandMapping] SET [Qty] = (isnull(Qty,0)-" + Diff_In_Qty + ") where id=" + SpareSpareBrandId) > 0)
                                    {
                                        string query = "INSERT INTO [dbo].[SpareInventaryHistory] ([SpareSpareBrandMappingId],[Cr],[Dr],[DOC],[JobCardId],[OrderItemDetailId],[GRNDetailId],[RequsitionSpareId],[WorkshopId],message) VALUES(" + SpareSpareBrandId + ",0," + Diff_In_Qty + ",CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')),0,0," + strArrGrndtId[i] + ",0,1,'" + msg.Replace("'", "''") + "')";
                                        if (dbCon.ExecuteQuery(query) > 0)
                                        {

                                        }
                                    }
                                    //Add Into History table
                                }
                            }
                            else
                            {
                                DataTable dtpart = dbCon.GetDataTable("select name from [Motorz].[dbo].[Spare] where id=" + strArrSpare[i]);
                                DataTable dtbrand = dbCon.GetDataTable("select name from [Motorz].[dbo].[Spare_Brand] where id=" + strArrBrand[i]);
                                DataRow[] sr = dtdetail.Select("Id=" + strArrGrndtId[i] + "");
                                if (dbCon.ExecuteQuery("UPDATE [dbo].[SpareSpareBrandMapping] SET [Qty] = (isnull(Qty,0)-" + pqty + ") where [Id] = " + SpareSpareBrandId) > 0)
                                {
                                    //string msg = strArrQty[i] + " Qty Removed from store inventory on " + dbCon.getindiantime().ToString() + " ( Previous qty was " + pqty + ") Stock Qty move from '" + hfp.Value + "," + hfb.Value + "' to '" + cmbSpare.SelectedItem.Text + "," + cmbBrand.SelectedItem.Text + "'";
                                    string msg = stock + " Qty Removed from store inventory on " + dbCon.getindiantime().ToString() + " ( Previous qty was " + pqty + ") Stock Qty move from '" + sr[0]["Part"].ToString() + "','" + sr[0]["BrandName"].ToString() + "' to '" + dtpart.Rows[0][0].ToString() + "','" + dtbrand.Rows[0][0].ToString() + "'";

                                    string query = "INSERT INTO [dbo].[SpareInventaryHistory] ([SpareSpareBrandMappingId],[Cr],[Dr],[DOC],[JobCardId],[OrderItemDetailId],[GRNDetailId],[RequsitionSpareId],[WorkshopId],message) VALUES(" + SpareSpareBrandId + ",0," + pqty + ",CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')),0,0," + strArrGrndtId[i] + ",0,1,'" + msg.Replace("'", "''") + "')";
                                    dbCon.ExecuteQuery(query);
                                }
                                string str1 = "select top 1 id from SpareSpareBrandMapping where BrandId=" + strArrBrand[i] + " and SpareId=" + strArrSpare[i];
                                DataTable dt1 = new DataTable();
                                dt1 = dbCon.GetDataTable(str1);
                                if (dt1 != null && dt1.Rows.Count > 0)
                                {
                                    SpareSpareBrandIdNew = dt1.Rows[0][0].ToString();
                                }
                                else
                                {
                                    string[] strArr3 = { };
                                    int ide = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[SpareSpareBrandMapping] ([SpareId],[BrandId],[Qty],[MinQty],[MaxQty],[IsActive],[IsDeleted],[DOC],[DOM],[HSNNumber],Mrp)VALUES(" + strArrSpare[i] + "," + strArrBrand[i] + ",0,1,1000,1,0,getdate(),getdate(),'','" + strArrMrp[i] + "')  SELECT SCOPE_IDENTITY();", strArr3);
                                    SpareSpareBrandIdNew = ide.ToString();
                                }
                                //Now Add New Parts Into Inventory
                                if (dbCon.ExecuteQuery("UPDATE [dbo].[SpareSpareBrandMapping] SET [Qty] = (isnull(Qty,0)+" + stock + ") where [Id] = " + SpareSpareBrandIdNew) > 0)
                                {
                                    //string msg = strArrQty[i] + " Qty Added from store inventory on " + dbCon.getindiantime().ToString() + " ( Previous qty was " + pqty + ") Stock Qty move from '" + dtpart.Rows[0][0].ToString() + "," + dtbrand.Rows[0][0].ToString() + "' to '" + cmbSpare.SelectedItem.Text + "," + cmbBrand.SelectedItem.Text + "'";

                                    string msg = stock + " Qty Added Into store inventory on " + dbCon.getindiantime().ToString() + " ( Previous qty was " + pqty + ") Stock Qty move from '" + sr[0]["Part"].ToString() + "," + sr[0]["BrandName"].ToString() + "' to '" + dtpart.Rows[0][0].ToString() + "," + dtbrand.Rows[0][0].ToString() + "'";

                                    string query = "INSERT INTO [dbo].[SpareInventaryHistory] ([SpareSpareBrandMappingId],[Cr],[Dr],[DOC],[JobCardId],[OrderItemDetailId],[GRNDetailId],[RequsitionSpareId],[WorkshopId],message) VALUES(" + SpareSpareBrandIdNew + ",'" + stock + "',0,CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')),0,0," + strArrGrndtId[i] + ",0,1,'" + msg.Replace("'", "''") + "')";
                                    dbCon.ExecuteQuery(query);
                                }
                            }
                        }
                    }
                }

                #region  Add Items Into Customers Invoice Change
                DataTable dtJobcardids = dbCon.GetDataTable("SELECT Distinct [JobcardId] FROM [dbo].[GRNDetail]  where JobcardId>0 and  grnid=" + grnid);
                if (dtJobcardids != null && dtJobcardids.Rows.Count > 0)
                {
                    for (int j = 0; j < dtJobcardids.Rows.Count; j++)
                    {
                        bool IsInvoiceNumberGenerated = false;

                        DataTable dtInvoice = dbCon.GetDataTable("SELECT [Id] FROM [dbo].[Invoice] where JobCardId=" + dtJobcardids.Rows[j][0].ToString() + " and isnull(IsCancelled,0)=0 and (GstInvoiceNumber is not null or InvoiceNumber is not null )");
                        if (dtInvoice != null && dtInvoice.Rows.Count > 0)
                        {
                            IsInvoiceNumberGenerated = true;
                        }
                        if (!IsInvoiceNumberGenerated)
                        {
                            DataTable dtGrnItems = dbCon.GetDataTable("SELECT [Id],Consumption as [Qty],[Price],[JobcardId],[EntityId],ActualAmountPerUnit FROM [dbo].[GRNDetail]  where JobcardId=" + dtJobcardids.Rows[j][0].ToString() + " and grnid=" + grnid);

                            int InvoiceId = 0;
                            dtInvoice = dbCon.GetDataTable("SELECT [Id] FROM [dbo].[Invoice] where JobCardId=" + dtJobcardids.Rows[j][0].ToString() + " and Type=1 and isnull(IsCancelled,0)=0");


                            if (dtInvoice != null && dtInvoice.Rows.Count > 0)
                            {
                                InvoiceId = Convert.ToInt32(dtInvoice.Rows[0][0].ToString());
                            }
                            else
                            {
                                try
                                {
                                    InvoiceId = Convert.ToInt32(dbCon.GetDataTable("Select Max(Id)+1 from Invoice").Rows[0][0].ToString());
                                }
                                catch (Exception E)
                                {
                                    InvoiceId = 1;
                                }
                                dbCon.ExecuteQuery("INSERT INTO [dbo].[Invoice] ([Id],[JobCardId],[WorkshopId],[DOC],[DOM],[Type],[IncludeInGst]) values (" + InvoiceId + "," + dtJobcardids.Rows[j][0].ToString() + ",1,'" + dbCon.getindiantime().ToString("dd-MMM-yyyy HH:ss:mm") + "','" + dbCon.getindiantime().ToString("dd-MMM-yyyy HH:ss:mm") + "',1,1)");
                            }

                            // AddMotorzLog("SELECT [Id],Consumption as [Qty],[Price],[JobcardId],[EntityId] FROM [dbo].[GRNDetail]  where JobcardId=" + dtJobcardids.Rows[j][0].ToString() + " and grnid=" + grnid);
                            AddMotorzLog(dtGrnItems.Rows.Count.ToString());


                            if (dtGrnItems != null)
                            {
                                for (int k = 0; k < dtGrnItems.Rows.Count; k++)
                                {
                                    bool ShouldUpdate = true;
                                    try
                                    {
                                        DataTable dtItemAvailable1 = dbCon.GetDataTable("Select Id,Mrp as ActualAmountPerUnit,Quantity,SpareId from Invoice_Spare_Mapping where  InvoiceId in (Select id from invoice where isnull(IsCancelled,0)=0 and JobCardId=" + dtJobcardids.Rows[j][0].ToString() + " and type=1) and GrnDetailId=" + dtGrnItems.Rows[k]["Id"].ToString());
                                        if (dtItemAvailable1 != null && dtItemAvailable1.Rows.Count > 0)
                                        {
                                            if (dtItemAvailable1.Rows[0]["Quantity"].ToString() == dtGrnItems.Rows[k]["Qty"].ToString() && dtItemAvailable1.Rows[0]["ActualAmountPerUnit"].ToString() == dtGrnItems.Rows[k]["ActualAmountPerUnit"].ToString() && dtItemAvailable1.Rows[0]["SpareId"].ToString() == dtGrnItems.Rows[k]["EntityId"].ToString())
                                            {
                                                ShouldUpdate = false;
                                            }
                                            else
                                            {

                                                dbCon.ExecuteQuery("Delete from [dbo].[Invoice_Spare_Mapping] where refid=" + dtItemAvailable1.Rows[0][0].ToString());
                                                dbCon.ExecuteQuery("Delete from [dbo].[Invoice_Spare_Mapping] where Id=" + dtItemAvailable1.Rows[0][0].ToString());
                                            }
                                        }
                                    }
                                    catch (Exception E) { }

                                    //if (dtGrnItems.Rows[k]["EntityId"].ToString()=="")
                                    AddMotorzLog("INSERT 1.1");
                                    if (ShouldUpdate)
                                    {
                                        DataTable dtCalc = dbCon.GetDataTable("Select Price,Convert(decimal(18,2),(isnull(TaxValue,0)/2)) as TaxVal,Convert(decimal(18,2),((Spare.Price*TaxValue)/(100+TaxValue))/2) as TaxAmt,Convert(decimal(18,2),(Price -((Spare.Price*TaxValue)/(100+TaxValue)))) as Taxable  FROM TaxCategory RIGHT OUTER JOIN Spare ON Spare.TaxId = TaxCategory.Id where Spare.id=" + dtGrnItems.Rows[k]["EntityId"].ToString());
                                        AddMotorzLog("INSERT 1.2");

                                        // AddMotorzLog("INSERT 1.2");
                                        //bool isExist = false;
                                        //DataTable dtItemAvailable = dbCon.GetDataTable("Select * from Invoice_Spare_Mapping where InvoiceId in (Select id from invoice where isnull(IsCancelled,0)=0  and JobCardId=" + dtJobcardids.Rows[j][0].ToString() + " and type=1)  and  GrnDetailId=" + dtGrnItems.Rows[k]["Id"].ToString());
                                        //AddMotorzLog("INSERT 1.3");
                                        //if (dtItemAvailable != null && dtItemAvailable.Rows.Count > 0)
                                        //    isExist = true;
                                        //if (isExist)
                                        //{
                                        //    dbCon.ExecuteQuery("Delete from [dbo].[Invoice_Spare_Mapping] where InvoiceId in (Select id from invoice where isnull(IsCancelled,0)=0  and JobCardId=" + dtJobcardids.Rows[j][0].ToString() + " and type=1)  and GrnDetailId=" + dtGrnItems.Rows[k]["Id"].ToString());
                                        //}
                                        //AddMotorzLog("INSERT 1.4");
                                        try
                                        {

                                            AddMotorzLog("INSERT 1.5");
                                            dbCon.ExecuteQuery("INSERT INTO [dbo].[Invoice_Spare_Mapping]([InvoiceId],[SpareId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId],[GrnDetailId],Mrp,Depreciation) VALUES (" + InvoiceId + "," + dtGrnItems.Rows[k]["EntityId"].ToString() + ",'" + dtCalc.Rows[0]["TaxVal"].ToString() + "','" + dtCalc.Rows[0]["TaxAmt"].ToString() + "','" + dtCalc.Rows[0]["TaxVal"].ToString() + "','" + dtCalc.Rows[0]["TaxAmt"].ToString() + "',0,'" + dtCalc.Rows[0]["Price"].ToString() + "','" + dtGrnItems.Rows[k]["Qty"].ToString() + "',0,(" + dtCalc.Rows[0]["Price"].ToString() + "*" + dtGrnItems.Rows[k]["Qty"].ToString() + "),'" + dbCon.getindiantime().ToString("dd-MMM-yyyy HH:ss:mm") + "',1,0," + dtGrnItems.Rows[k]["Id"].ToString() + "," + dtCalc.Rows[0]["Price"].ToString() + ",100)");
                                            AddMotorzLog("INSERT 1.6");
                                        }
                                        catch (Exception Expc)
                                        {
                                            AddMotorzLog(Expc.Message);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }
            try
            {
                string WorkshopId = HttpContext.Current.Request.Cookies["TUser"]["WorkshopId"];
                string UserId = HttpContext.Current.Request.Cookies["TUser"]["Id"];
                UserActivity objUserAct = new UserActivity();
                objUserAct.InsertUserActivity("Updated GRN on : " + dbCon.getindiantime().ToString("MM-dd-yyyy HH:mm:ss tt") + " GrnId - " + grnid, UserId, WorkshopId, "", "Update", "Grn");
            }
            catch (Exception E) { AddMotorzLog(E.Message); }
            String msgstr = "Inventary Updated GrnId - " + grnid + " ";
            int nItems = 0;
            try
            {
                nItems = int.Parse(dbCon.GetDataTable("select count(id) from grndetail where grnid=" + grnid).Rows[0][0].ToString());
                if (nItems > 0)
                    msgstr += " Items Updated - " + nItems.ToString();
                else
                    msgstr = "Something went wrong please contact I.T. Team";
            }
            catch (Exception E) { AddMotorzLog(E.Message); }
            objectToSerilize.Name = msgstr;
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " AddGrn_v1WithAllocation Msg:" + e.Message, "", e.StackTrace);
            AddMotorzLog(e.Message + " " + e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }


    [WebMethod]
    public void AddGrn_v1(string VendorId, string strSpare, string strQty, string strPri, string strType, string strMrp, string strBMrp, string strbrand, string inv, string recby, string rmk, string strDis, string strDt, string dcn, string GrnId, string strGrnDtid, string userId, string workshopId)
    {
        var objectToSerilize = new Objs();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {


            DateTime date = DateTime.ParseExact(strDt, "dd-MM-yyyy hh:mm tt", System.Globalization.CultureInfo.InvariantCulture);
            //string[] strArr = { };
            //int grnid = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[GRN]([DOC],[DOM],[ReceivedBy],[VendorId],[RefNumber],[Remark],[InvoiceNo],DeliveryDate,dcno,UserId,WorkshopId) VALUES('" + dbCon.getindiantime().ToString("MM-dd-yyyy hh:mm tt") + "','" + dbCon.getindiantime().ToString("MM-dd-yyyy hh:mm tt") + "','" + recby + "','" + VendorId + "',0,'" + rmk + "','" + inv + "','" + date.ToString("MM-dd-yyyy hh:mm tt") + "','" + dcn + "'," + userId + "," + workshopId + ") SELECT SCOPE_IDENTITY();", strArr);
            string[] strArr = { };
            string[] strArr1 = { };
            int grnid = 0;
            DataTable dtdetail = new DataTable();
            if (!GrnId.Equals("0") && !GrnId.Equals("-1"))
            {
                string[] strArr4 = { GrnId };
                dtdetail = dbCon.GetDataTableWithParams("SELECT  entityid as spareid,BrandId,GRNDetail.Id,GRN.Id as grnId,GRNDetail.EntityType, case when GRNDetail.EntityType='spare' then (select Code from motorz.dbo.spare where id=entityid ) else (select name from Consumables where id=entityid ) end as [Part],GRNDetail.Qty, GRNDetail.Price As PurchasePrice,  GRNDetail.Discount,case when GRNDetail.EntityType='spare' then (select price from motorz.dbo.spare where id=entityid ) else (select price from Consumables where id=entityid ) end as PartMrp,case when GRNDetail.EntityType='spare' then (select Name from motorz.dbo.Spare_Brand where id=BrandId ) else (select Name from Consumables_Brand where id=BrandId ) end as BrandName FROM GRN INNER JOIN GRNDetail ON GRN.Id = GRNDetail.GRNId where grn.id=@1", strArr4);

                grnid = dbCon.ExecuteScalarQueryWithParams("update [dbo].[GRN] set [ReceivedBy]='" + recby + "',[VendorId]='" + VendorId + "',[Remark]='" + rmk.ToString() + "',[InvoiceNo]='" + inv + "',[DeliveryDate]='" + date.ToString("MM-dd-yyyy hh:mm tt") + "',DCNo='" + dcn + "',UserId='" + userId + "',WorkshopId='" + workshopId + "'  where id=" + GrnId, strArr1);
                grnid = int.Parse(GrnId.ToString());
            }
            else
            {
                //grnid = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[GRN]([DOC],[DOM],[ReceivedBy],[VendorId],[RefNumber],[Remark],[InvoiceNo],DeliveryDate,dcno,UserId,WorkshopId) VALUES('" + dbCon.getindiantime().ToString("MM-dd-yyyy hh:mm tt") + "','" + dbCon.getindiantime().ToString("MM-dd-yyyy hh:mm tt") + "','" + recby + "','" + VendorId + "',0,'" + rmk + "','" + inv + "','" + date.ToString("MM-dd-yyyy hh:mm tt") + "','" + dcn + "'," + userId + "," + workshopId + ") SELECT SCOPE_IDENTITY();", strArr);

                //grnid = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[GRN]([DOC],[DOM],[ReceivedBy],[VendorId],[RefNumber],[Remark],[InvoiceNo],DeliveryDate,dcno) VALUES('" + dbCon.getindiantime().ToString("MM-dd-yyyy hh:mm tt") + "','" + dbCon.getindiantime().ToString("MM-dd-yyyy hh:mm tt") + "','" + recby + "','" + VendorId + "',0,'" + rmk + "','" + inv + "','" + date.ToString("MM-dd-yyyy hh:mm tt") + "','" + dcn + "') SELECT SCOPE_IDENTITY();", strArr);
            }


            string[] strArrSpare = strSpare.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrQty = strQty.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrPri = strPri.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrMrp = strMrp.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrBMrp = strBMrp.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrBrand = strbrand.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrDis = strDis.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrtype = strType.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrGrndtId = strGrnDtid.Split(new string[] { "$#@" }, StringSplitOptions.None);
            if (grnid > 0)
            {
                for (int i = 0; i < strArrSpare.Length; i++)
                {

                    if (strArrGrndtId[i].Equals("-1"))
                    {
                        string[] strarr = { };
                        string str1 = "insert into GRNDetail (EntityId,EntityType,Qty,Price,Discount,BrandId,DOC,GRNId,WorkshopId) values ('" + strArrSpare[i] + "','spare','" + strArrQty[i] + "','" + strArrPri[i] + "','" + strArrDis[i] + "','" + strArrBrand[i] + "','" + dbCon.getindiantime() + "','" + grnid + "',1) SELECT SCOPE_IDENTITY(); ";

                        int value1 = dbCon.ExecuteScalarQueryWithParams(str1, strarr);
                        //hfid.Value = value1.ToString();

                        if (strArrMrp[i] != null && !String.IsNullOrWhiteSpace(strArrMrp[i]))
                            dbCon.ExecuteQuery("UPDATE [Motorz].[dbo].[Spare] SET [Price] = '" + strArrMrp[i] + "' where id=(select entityid from GRNDetail where id=" + value1 + ")");

                        string str = "select top 1 id,Qty from SpareSpareBrandMapping where SpareId=" + strArrSpare[i] + " and BrandId=" + strArrBrand[i] + " ";
                        DataTable dt1 = new DataTable();
                        dt1 = dbCon.GetDataTable(str);
                        if (dt1 != null && dt1.Rows.Count > 0)
                        {
                            //update nd select
                            long SpareSpareBrandId = long.Parse(dt1.Rows[0]["id"].ToString());
                            float quty = float.Parse(dt1.Rows[0]["Qty"].ToString()) + float.Parse(strArrQty[i].ToString());
                            str1 = "update SpareSpareBrandMapping set Qty= " + quty + ",DOM='" + dbCon.getindiantime() + "' where id=" + SpareSpareBrandId + "";
                            int retval = dbCon.ExecuteQuery(str1);

                            str1 = "INSERT INTO [dbo].[SpareInventaryHistory] ([SpareSpareBrandMappingId],[Cr],[Dr],[DOC],[JobCardId],[OrderItemDetailId],[GRNDetailId],[RequsitionSpareId],[WorkshopId]) VALUES(" + SpareSpareBrandId + ",'" + strArrQty[i] + "',0,'" + dbCon.getindiantime() + "',0,0," + value1 + ",0,1)";
                            retval = dbCon.ExecuteQuery(str1);
                        }
                        else
                        {
                            //insert 
                            string[] strarr1 = { };
                            int SpareSpareBrandId = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[SpareSpareBrandMapping] ([SpareId],[BrandId],[Qty],[MinQty],[MaxQty],[IsActive],[IsDeleted],[DOC],[DOM],[HSNNumber],Mrp,WorkshopId) VALUES('" + strArrSpare[i] + "','" + strArrBrand[i] + "','" + strArrQty[i] + "',1,1000,1,0,'" + dbCon.getindiantime() + "','" + dbCon.getindiantime() + "','','" + strArrMrp[i] + "',1)  SELECT SCOPE_IDENTITY();", strarr1);
                            str1 = "INSERT INTO [dbo].[SpareInventaryHistory] ([SpareSpareBrandMappingId],[Cr],[Dr],[DOC],[JobCardId],[OrderItemDetailId],[GRNDetailId],[RequsitionSpareId],[WorkshopId]) VALUES(" + SpareSpareBrandId + ",'" + strArrQty[i] + "',0,'" + dbCon.getindiantime() + "',0,0," + value1 + ",0,1)";
                            int retval = dbCon.ExecuteQuery(str1);
                        }
                    }
                    else
                    {
                        string str = "SELECT [EntityId],[EntityType] ,isnull([Qty],0) as [Qty],[Price],[Discount],[BrandId],[DOC],[GRNId],[WorkshopId],(select top 1 id from SpareSpareBrandMapping where BrandId=[GRNDetail].BrandId and SpareId=EntityId) as SpareBrandId FROM [dbo].[GRNDetail] where [Id]=" + strArrGrndtId[i];
                        DataTable dt = new DataTable();
                        dt = dbCon.GetDataTable(str);
                        if (dt != null && dt.Rows.Count > 0)
                        {

                            if (strArrMrp[i] != null && !String.IsNullOrWhiteSpace(strArrMrp[i]) && strArrQty[i] != null && !String.IsNullOrWhiteSpace(strArrQty[i]))
                                dbCon.ExecuteQuery("UPDATE [dbo].[GRNDetail] SET BrandId='" + strArrBrand[i] + "',EntityId='" + strArrSpare[i] + "', [Price] ='" + strArrPri[i] + "',Discount='" + strArrDis[i] + "',Qty='" + strArrQty[i] + "'  WHERE id=" + strArrGrndtId[i]);

                            //SpareBrandId Setting
                            string SpareSpareBrandId = dt.Rows[0]["SpareBrandId"].ToString();
                            if (String.IsNullOrWhiteSpace(SpareSpareBrandId))
                            {
                                string[] strArr3 = { };
                                SpareSpareBrandId = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[SpareSpareBrandMapping] ([SpareId],[BrandId],[Qty],[MinQty],[MaxQty],[IsActive],[IsDeleted],[DOC],[DOM],[HSNNumber],Mrp)VALUES(" + strArrSpare[i] + "," + strArrBrand[i] + ",0,1,1000,1,0,getdate(),getdate(),'','" + strArrMrp[i] + "')  SELECT SCOPE_IDENTITY();", strArr3).ToString();
                            }
                            string SpareSpareBrandIdNew = "0";
                            //Update Customer Mrp
                            if (strArrMrp[i] != null && !String.IsNullOrWhiteSpace(strArrMrp[i]))
                                dbCon.ExecuteQuery("UPDATE [Motorz].[dbo].[Spare] SET [Price] = " + strArrMrp[i] + " where id=(select entityid from GRNDetail where id=" + strArrGrndtId[i] + ")");
                            //Take Previous values
                            string pqty = dt.Rows[0]["qty"].ToString();
                            string pspareid = dt.Rows[0]["EntityId"].ToString();
                            string pbrandid = dt.Rows[0]["BrandId"].ToString();
                            //New Values 
                            string spareid = strArrSpare[i];
                            string brandid = strArrBrand[i];
                            //Now Change/Update Inventory Casses
                            //Case 1 Just Qty Change nothing more (when same spare same brand)
                            float Diff_In_Qty = (float.Parse(pqty) - float.Parse(strArrQty[i]));
                            if (Diff_In_Qty < 0)
                                Diff_In_Qty = (float.Parse(strArrQty[i]) - float.Parse(pqty));
                            if ((float.Parse(pspareid) == float.Parse(spareid)) && (float.Parse(pbrandid) == float.Parse(brandid)))
                            {
                                //Now two casses either more or lesser than previous
                                if ((float.Parse(pqty) < float.Parse(strArrQty[i])))
                                {
                                    //Need to add in Invetory                             
                                    string msg = Diff_In_Qty.ToString() + " Qty Added in store inventory on " + dbCon.getindiantime().ToString() + " ( Previous qty was " + pqty + " ,& New qty is " + strArrQty[i] + " )";
                                    //Credit Diff Qty into brand_wise_spare
                                    if (dbCon.ExecuteQuery("UPDATE [dbo].[SpareSpareBrandMapping] SET [Qty] = (isnull(Qty,0)+" + Diff_In_Qty + ") where [Id] = " + SpareSpareBrandId) > 0)
                                    {
                                        string query = "INSERT INTO [dbo].[SpareInventaryHistory] ([SpareSpareBrandMappingId],[Cr],[Dr],[DOC],[JobCardId],[OrderItemDetailId],[GRNDetailId],[RequsitionSpareId],[WorkshopId],message) VALUES(" + SpareSpareBrandId + "," + Diff_In_Qty + ",0,CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')),0,0," + strArrGrndtId[i] + ",0,1,'" + msg.Replace("'", "''") + "')";
                                        if (dbCon.ExecuteQuery(query) > 0)
                                        {

                                        }
                                    }
                                    //Add Into History table
                                }
                                else if ((float.Parse(pqty) > float.Parse(strArrQty[i])))
                                {
                                    //Need to remove from Invetory
                                    string msg = Diff_In_Qty.ToString() + " Qty Removed from store inventory on " + dbCon.getindiantime().ToString() + " ( Previous qty was " + pqty + " ,& New qty is " + strArrQty[i] + " )";
                                    //Debit Diff Qty into brand_wise_spare
                                    if (dbCon.ExecuteQuery("UPDATE [dbo].[SpareSpareBrandMapping] SET [Qty] = (isnull(Qty,0)-" + Diff_In_Qty + ") where id=" + SpareSpareBrandId) > 0)
                                    {
                                        string query = "INSERT INTO [dbo].[SpareInventaryHistory] ([SpareSpareBrandMappingId],[Cr],[Dr],[DOC],[JobCardId],[OrderItemDetailId],[GRNDetailId],[RequsitionSpareId],[WorkshopId],message) VALUES(" + SpareSpareBrandId + ",0," + Diff_In_Qty + ",CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')),0,0," + strArrGrndtId[i] + ",0,1,'" + msg.Replace("'", "''") + "')";
                                        if (dbCon.ExecuteQuery(query) > 0)
                                        {

                                        }
                                    }
                                    //Add Into History table
                                }
                            }
                            else
                            {
                                DataTable dtpart = dbCon.GetDataTable("select name from [Motorz].[dbo].[Spare] where id=" + strArrSpare[i]);
                                DataTable dtbrand = dbCon.GetDataTable("select name from [Motorz].[dbo].[Spare_Brand] where id=" + strArrBrand[i]);
                                DataRow[] sr = dtdetail.Select("Id=" + strArrGrndtId[i] + "");
                                if (dbCon.ExecuteQuery("UPDATE [dbo].[SpareSpareBrandMapping] SET [Qty] = (isnull(Qty,0)-" + pqty + ") where [Id] = " + SpareSpareBrandId) > 0)
                                {
                                    //string msg = strArrQty[i] + " Qty Removed from store inventory on " + dbCon.getindiantime().ToString() + " ( Previous qty was " + pqty + ") Stock Qty move from '" + hfp.Value + "," + hfb.Value + "' to '" + cmbSpare.SelectedItem.Text + "," + cmbBrand.SelectedItem.Text + "'";
                                    string msg = strArrQty[i] + " Qty Removed from store inventory on " + dbCon.getindiantime().ToString() + " ( Previous qty was " + pqty + ") Stock Qty move from '" + sr[0]["Part"].ToString() + "','" + sr[0]["BrandName"].ToString() + "' to '" + dtpart.Rows[0][0].ToString() + "','" + dtbrand.Rows[0][0].ToString() + "'";

                                    string query = "INSERT INTO [dbo].[SpareInventaryHistory] ([SpareSpareBrandMappingId],[Cr],[Dr],[DOC],[JobCardId],[OrderItemDetailId],[GRNDetailId],[RequsitionSpareId],[WorkshopId],message) VALUES(" + SpareSpareBrandId + ",0," + pqty + ",CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')),0,0," + strArrGrndtId[i] + ",0,1,'" + msg.Replace("'", "''") + "')";
                                    dbCon.ExecuteQuery(query);
                                }
                                string str1 = "select top 1 id from SpareSpareBrandMapping where BrandId=" + strArrBrand[i] + " and SpareId=" + strArrSpare[i];
                                DataTable dt1 = new DataTable();
                                dt1 = dbCon.GetDataTable(str1);
                                if (dt1 != null && dt1.Rows.Count > 0)
                                {
                                    SpareSpareBrandIdNew = dt1.Rows[0][0].ToString();
                                }
                                else
                                {
                                    string[] strArr3 = { };
                                    int ide = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[SpareSpareBrandMapping] ([SpareId],[BrandId],[Qty],[MinQty],[MaxQty],[IsActive],[IsDeleted],[DOC],[DOM],[HSNNumber],Mrp)VALUES(" + strArrSpare[i] + "," + strArrBrand[i] + ",0,1,1000,1,0,getdate(),getdate(),'','" + strArrMrp[i] + "')  SELECT SCOPE_IDENTITY();", strArr3);
                                    SpareSpareBrandIdNew = ide.ToString();
                                }
                                //Now Add New Parts Into Inventory
                                if (dbCon.ExecuteQuery("UPDATE [dbo].[SpareSpareBrandMapping] SET [Qty] = (isnull(Qty,0)+" + strArrQty[i] + ") where [Id] = " + SpareSpareBrandIdNew) > 0)
                                {
                                    //string msg = strArrQty[i] + " Qty Added from store inventory on " + dbCon.getindiantime().ToString() + " ( Previous qty was " + pqty + ") Stock Qty move from '" + dtpart.Rows[0][0].ToString() + "," + dtbrand.Rows[0][0].ToString() + "' to '" + cmbSpare.SelectedItem.Text + "," + cmbBrand.SelectedItem.Text + "'";

                                    string msg = strArrQty[i] + " Qty Added Into store inventory on " + dbCon.getindiantime().ToString() + " ( Previous qty was " + pqty + ") Stock Qty move from '" + sr[0]["Part"].ToString() + "," + sr[0]["BrandName"].ToString() + "' to '" + dtpart.Rows[0][0].ToString() + "," + dtbrand.Rows[0][0].ToString() + "'";

                                    string query = "INSERT INTO [dbo].[SpareInventaryHistory] ([SpareSpareBrandMappingId],[Cr],[Dr],[DOC],[JobCardId],[OrderItemDetailId],[GRNDetailId],[RequsitionSpareId],[WorkshopId],message) VALUES(" + SpareSpareBrandIdNew + ",'" + strArrQty[i] + "',0,CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')),0,0," + strArrGrndtId[i] + ",0,1,'" + msg.Replace("'", "''") + "')";
                                    dbCon.ExecuteQuery(query);
                                }

                            }
                        }
                    }
                }
            }
            try
            {
                string WorkshopId = HttpContext.Current.Request.Cookies["TUser"]["WorkshopId"];
                string UserId = HttpContext.Current.Request.Cookies["TUser"]["Id"];
                UserActivity objUserAct = new UserActivity();
                objUserAct.InsertUserActivity("Updated GRN on : " + dbCon.getindiantime().ToString("MM-dd-yyyy HH:mm:ss tt") + " GrnId - " + grnid, UserId, WorkshopId, "", "Update", "Grn");
            }
            catch (Exception E) { }
            String msgstr = "Inventary Updated GrnId - " + grnid + " ";
            int nItems = 0;
            try
            {
                nItems = int.Parse(dbCon.GetDataTable("select count(id) from grndetail where grnid=" + grnid).Rows[0][0].ToString());
                if (nItems > 0)
                    msgstr += " Items Updated - " + nItems.ToString();
                else
                    msgstr = "Something went wrong please contact I.T. Team";
            }
            catch (Exception E) { }
            objectToSerilize.Name = msgstr;
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " AddGrn Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }
    [WebMethod]
    public void GetGrnDetails(string Grnid)
    {
        ObjGrnData ag = new ObjGrnData();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string[] strArr = { Grnid };
            DataTable dt = dbCon.GetDataTableWithParams("SELECT CONVERT(VARCHAR(5), [DeliveryDate], 108) + ' ' +  RIGHT(CONVERT(VARCHAR(30), [DeliveryDate], 9),2) as DeliveryTime,Vendor.Name, DCNo,GRN.[Id],[ReceivedBy],[VendorId],[RefNumber],[Remark],[InvoiceNo],[DeliveryDate] FROM GRN LEFT OUTER JOIN Vendor ON GRN.VendorId = Vendor.Id   where GRN.id=@1", strArr);

            DataTable dt1 = dbCon.GetDataTableWithParams("SELECT  (Select code from spare where id=entityid) as Code,entityid as spareid,BrandId,GRNDetail.Id,GRN.Id as grnId,GRNDetail.EntityType, case when GRNDetail.EntityType='spare' then (select Code from [Motorz].[dbo].[spare] where id=entityid ) else (select name from Consumables where id=entityid ) end as [Part],GRNDetail.Qty, GRNDetail.Price As PurchasePrice,  GRNDetail.Discount,case when GRNDetail.EntityType='spare' then (select price from [Motorz].[dbo].[spare] where id=entityid ) else (select price from Consumables where id=entityid ) end as PartMrp,case when GRNDetail.EntityType='spare' then (select Name from [Motorz].[dbo].[Spare_Brand] where id=BrandId ) else (select Name from [Motorz].[dbo].[Consumables_Brand] where id=BrandId ) end as BrandName ,GRNDetail.VendorInvoiceRate,GRNDetail.VendorInvoiceCGST,GRNDetail.VendorInvoiceCGSTAmount,GRNDetail.VendorInvoiceSGST,GRNDetail.VendorInvoiceSGSTAmount,GRNDetail.VendorInvoiceIGST,GRNDetail.VendorInvoiceIGSTAmount,GRNDetail.VendorInvoiceTaxable,GRNDetail.VendorInvoiceTotal,GRNDetail.JobcardId,GRNDetail.Consumption,GRNDetail.Stock,isnull(GRNDetail.ReturnQty,0) as ReturnQty FROM GRN INNER JOIN GRNDetail ON GRN.Id = GRNDetail.GRNId where grn.id=@1 order by Code ", strArr);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    DateTime dtt = DateTime.Parse(dr["DeliveryDate"].ToString());
                    //DeliveryTime
                    ag.DeliveryTime = dr["DeliveryTime"].ToString();
                    ag.DeliveryDate = dtt.ToString("dd-MM-yyyy");
                    ag.DCNo = dr["DCNo"].ToString();
                    ag.ReceivedBy = dr["ReceivedBy"].ToString();
                    ag.VendorId = dr["Name"].ToString();
                    ag.Remark = dr["Remark"].ToString();
                    ag.InvoiceNo = dr["InvoiceNo"].ToString();

                }
            }

            if (dt1 != null && dt1.Rows.Count > 0)
            {
                foreach (DataRow dr in dt1.Rows)
                {
                    var Spares = new ObjGrnData.ObjGrnDataDetails();

                    Spares.Id = dr["Id"].ToString();
                    Spares.EntityId = dr["Part"].ToString();
                    Spares.EntityType = dr["EntityType"].ToString();
                    Spares.qty = dr["Qty"].ToString();
                    Spares.price = dr["PurchasePrice"].ToString();
                    Spares.dis = dr["Discount"].ToString();
                    Spares.brandid = dr["BrandName"].ToString();
                    Spares.MRP = dr["PartMrp"].ToString();
                    Spares.name = dr["Code"].ToString();

                    Spares.VendorInvoiceRate = dr["VendorInvoiceRate"].ToString();
                    Spares.VendorInvoiceCGST = dr["VendorInvoiceCGST"].ToString();
                    Spares.VendorInvoiceCGSTAmount = dr["VendorInvoiceCGSTAmount"].ToString();
                    Spares.VendorInvoiceSGSTAmount = dr["VendorInvoiceSGSTAmount"].ToString();
                    Spares.VendorInvoiceIGST = dr["VendorInvoiceIGST"].ToString();
                    Spares.VendorInvoiceIGSTAmount = dr["VendorInvoiceIGSTAmount"].ToString();
                    Spares.VendorInvoiceTaxable = dr["VendorInvoiceTaxable"].ToString();
                    Spares.VendorInvoiceTotal = dr["VendorInvoiceTotal"].ToString();
                    Spares.rqty = dr["ReturnQty"].ToString();
                    try
                    {
                        DataTable dtjob = dbCon.GetDataTable("SELECT [Id],Convert(varchar,[Id])+' ,'+(SELECT Vehicle_Model.Name+' ,'+Vehicle.Number FROM Vehicle_Model inner join Vehicle on Vehicle.Vehicle_Model_Id=Vehicle_Model.Id WHERE (Vehicle.Id = [JobCard].Vehicle_Id)) As [Name] FROM [dbo].[JobCard] where jobcard.id=" + dr["JobcardId"].ToString());
                        if (dtjob != null && dtjob.Rows.Count > 0)
                        {
                            Spares.JobcardId = dtjob.Rows[0]["Name"].ToString();
                        }
                        else
                        {
                            Spares.JobcardId = "";
                        }
                    }
                    catch (Exception E) { }
                    Spares.Consumption = dr["Consumption"].ToString();

                    Spares.Stock = dr["Stock"].ToString();

                    ag.Items.Add(Spares);

                }
                try
                {
                    for (int i = 0; i < 10; i++)
                    {
                        var Spares = new ObjGrnData.ObjGrnDataDetails();
                        Spares.Id = "-1";
                        Spares.EntityId = "";
                        Spares.EntityType = "";
                        Spares.qty = "0";
                        Spares.price = "0";
                        Spares.dis = "0";
                        Spares.brandid = "";
                        Spares.MRP = "0";

                        Spares.VendorInvoiceRate = "0";
                        Spares.VendorInvoiceCGST = "0";
                        Spares.VendorInvoiceCGSTAmount = "0";
                        Spares.VendorInvoiceSGSTAmount = "0";
                        Spares.VendorInvoiceIGST = "0";
                        Spares.VendorInvoiceIGSTAmount = "0";
                        Spares.VendorInvoiceTaxable = "0";
                        Spares.VendorInvoiceTotal = "0";
                        Spares.JobcardId = "";
                        Spares.Consumption = "0";
                        Spares.Stock = "0";
                        ag.Items.Add(Spares);


                        //dt.Rows[dt.Rows.Count - 1]["id"] = -1;
                    }
                }
                catch (Exception E) { }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetGrnDetails Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(ag));
    }

    [WebMethod]
    public void GetUnMappedRequisitionItems(string StartDate, string EndDate, string Jobcards, string requisition)
    {
        List<objReqItem> obj = new List<objReqItem>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            DataTable dt = dbCon.GetDataTable("SELECT Requisition_Spare.id,(SELECT Code FROM Spare WHERE (Id = Requisition_Spare.SpareId)) AS Part, Requisition_Spare.Quantity, Vehicle.Number+','+ Vehicle_Brand.Name+','+Vehicle_Model.Name as Vehicle,JobCard.Id as Jobcardid,RequisitionId FROM Requisition_Spare INNER JOIN Requisition ON Requisition_Spare.RequisitionId = Requisition.Id INNER JOIN JobCard ON Requisition.JobCardId = JobCard.Id INNER JOIN Vehicle ON JobCard.Vehicle_Id = Vehicle.Id INNER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id INNER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id WHERE (Requisition_Spare.DOC >= '21-jul-2018 00:00:00') and  (Requisition_Spare.DOC <= '21-jul-2018 23:59:59') order by Part ");
            if (dt != null && dt.Rows.Count > 0)
            {
                objReqItem objsub;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objsub = new objReqItem();
                    objsub.Id = dt.Rows[i]["id"].ToString();
                    objsub.Jobcardid = dt.Rows[i]["Jobcardid"].ToString();
                    objsub.Part = dt.Rows[i]["Part"].ToString();
                    objsub.Qty = dt.Rows[i]["Quantity"].ToString();
                    objsub.Reqid = dt.Rows[i]["RequisitionId"].ToString();
                    objsub.Vehicle = dt.Rows[i]["Vehicle"].ToString();
                    obj.Add(objsub);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetGrnDetails Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    public class objReqItem
    {
        public string Id { get; set; }
        public string Part { get; set; }
        public string Qty { get; set; }
        public string Vehicle { get; set; }
        public string Jobcardid { get; set; }
        public string Reqid { get; set; }
    }


    public class ObjGrnData
    {
        public string DeliveryDate { get; set; }
        public string DeliveryTime { get; set; }
        public string InvoiceNo { get; set; }
        public string ReceivedBy { get; set; }
        public string DCNo { get; set; }
        public string Remark { get; set; }
        public string VendorId { get; set; }
        public class ObjGrnDataDetails
        {
            public string rqty;
            public String Id;
            public String EntityId;
            public String EntityType;
            public String qty;
            public String price;
            public String dis;
            public String brandid;
            public String MRP;
            public String name;

            public String VendorInvoiceRate;
            public String VendorInvoiceCGST;
            public String VendorInvoiceCGSTAmount;
            public String VendorInvoiceSGSTAmount;
            public String VendorInvoiceIGST;
            public String VendorInvoiceIGSTAmount;
            public String VendorInvoiceTaxable;
            public String VendorInvoiceTotal;
            public String JobcardId;
            public String Consumption;
            public String Stock;

        }
        public ObjGrnData()
        {
            Items = new List<ObjGrnDataDetails>();
        }
        public List<ObjGrnDataDetails> Items { get; set; }

    }
    [WebMethod]
    public void PurchaseOrder(string VendorId, string strSpare, string strQty, string strPri, string strType)
    {
        //return;
        var objectToSerilize = new InsertUpdateRecord();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        int OrderId = 0;
        int MasterOrderId = 0;
        try
        {
            string[] strArrSpare = strSpare.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrQty = strQty.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrPri = strPri.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrType = strType.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArr = { VendorId };
            if (strArrSpare.Length > 0)
            {
                MasterOrderId = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[MasterPurchaseOrder]([DOC],[DOM])VALUES(CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')),CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')))  SELECT SCOPE_IDENTITY();", strArr);

                if (strType.Contains("Spare"))
                {
                    OrderId = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[PurchaseOrder]([DOC],[DOM],[VendorId],MasterPurchaseOrderId) VALUES(CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')),CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')),@1," + MasterOrderId + ") SELECT SCOPE_IDENTITY();", strArr);
                    for (int i = 0; i < strArrSpare.Length; i++)
                    {
                        if (strArrType[i].Equals("Spare"))
                        {
                            //string[] strspareVal = GetPartDetailFromPart(strArrSpare[i]).Split(',');
                            string[] strArr1 = { };
                            dbCon.ExecuteQueryWithParams("INSERT INTO [dbo].[PurchaseOrderItem] ([SpareId],[PurchaseOrderId] ,[DOC],[DOM] ,[OrderQty]) VALUES (" + strArrSpare[i] + "," + OrderId + " ,CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')),CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')) ," + strArrQty[i] + ")", strArr);
                        }
                    }
                }
                //if (strType.Contains("Consumable"))
                //{
                //    OrderId = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[PurchaseOrderConsumable]([DOC],[DOM],[VendorId]) VALUES(CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')),CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')),@1) SELECT SCOPE_IDENTITY();", strArr);
                //    for (int i = 0; i < strArrSpare.Length; i++)
                //    {
                //        if (strArrType[i].Equals("Consumable"))
                //        {
                //            //string[] strspareVal = GetPartDetailFromPart(strArrSpare[i]).Split(',');
                //            string[] strArr1 = { };
                //            dbCon.ExecuteQueryWithParams("INSERT INTO [dbo].[PurchaseOrderItemConsumable] ([ConsumableId],[PurchaseOrderId] ,[DOC],[DOM] ,[OrderQty]) VALUES (" + strArrSpare[i] + "," + OrderId + " ,CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')),CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')) ," + strArrQty[i] + ")", strArr);
                //        }
                //    }
                //}
                objectToSerilize.resultflag = "1";
                objectToSerilize.Message = "Purchase Order created - OrderId-" + MasterOrderId;
            }
            else
            {
                objectToSerilize.resultflag = "0";
                objectToSerilize.Message = "Something Went Wrong..";
            }
        }
        catch (Exception e)
        {
            objectToSerilize.resultflag = "0";
            objectToSerilize.Message = e.Message;
            dbCon.InsertLogs(ErrorMsgPrefix + " GetModelByBrandId Msg:" + e.Message, "", e.StackTrace);

        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    [ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    [System.Web.Services.Protocols.SoapHeader("SoapHeader")]
    public void GetPartsForPurchase_FromJobCard()
    {
        var objectToSerilize = new JobCardSpare1();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string query = "SELECT        Spare.Code, Spare.Name, Spare.Id, isnull( (Select  Sum(isnull(Requisition_Spare.Quantity,0))- Sum(- isnull(Requisition_Spare.ReceivedQuantity,0)) from  Requisition_Spare where Requisition_Spare.SpareId=Spare.Id),0) - isnull((SELECT  SUM(Qty) AS Expr1 FROM            SpareSpareBrandMapping WHERE        (SpareId = Spare.Id)),0) AS Quantity FROM            Spare  where (isnull( (Select  Sum(isnull(Requisition_Spare.Quantity,0))- Sum(- isnull(Requisition_Spare.ReceivedQuantity,0)) from  Requisition_Spare where Requisition_Spare.SpareId=Spare.Id),0) - isnull((SELECT  SUM(Qty) AS Expr1 FROM           SpareSpareBrandMapping WHERE        (SpareId = Spare.Id)),0))>0";
            string[] param = { "0" };
            DataTable dt = dbCon.GetDataTableWithParams(query, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var Spares = new SparewithPrice1();
                    //Spares.Brand = dr["brandname"].ToString();
                    Spares.Id = dr["Id"].ToString();
                    Spares.Name = dr["Code"].ToString();
                    //decimal Quantity = Convert.ToDecimal(dr["Quantity"]);
                    Spares.Quantity = dr["Quantity"].ToString();
                    //Spares.PurchasePrice = dr["PurchasePrice"].ToString();
                    Spares.NType = "spare";
                    objectToSerilize.Spares.Add(Spares);
                }
                objectToSerilize.resultflag = "1";
                objectToSerilize.Message = "Success";
            }
        }
        catch (Exception ex)
        {
            objectToSerilize.resultflag = "0";
            objectToSerilize.Message = "Error";
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }


    [WebMethod]
    public void GetJobCardRequsitionV1(string Id = "0")
    {
        var objectToSerilize = new List<JobCardRequsition>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string query = "SELECT  JobCard.Id,CONVERT(VARCHAR(12), JobCard.DOC, 109) as DOC, Vehicle.Number, isnull(Vehicle_Model.Name,'') + ',' + isnull(Vehicle_Variant.Name,'') + ',' + Vehicle_Brand.Name AS Name FROM            Vehicle_Brand RIGHT OUTER JOIN Vehicle ON Vehicle_Brand.Id = Vehicle.Vehicle_Brand_Id LEFT OUTER JOIN Vehicle_Variant ON Vehicle.Vehicle_Variant_Id = Vehicle_Variant.Id LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id RIGHT OUTER JOIN Requisition INNER JOIN JobCard ON Requisition.JobCardId = JobCard.Id ON Vehicle.Id =JobCard.Vehicle_Id GROUP BY JobCard.DOM,JobCard.DOC,JobCard.Id, Vehicle_Model.Name, Vehicle_Variant.Name, Vehicle_Brand.Name, Vehicle.Number having JobCard.id in (Select  JobCardId from Requisition_Spare where isnull(Requisition_Spare.IsAllocate,0)=0 And isnull(Requisition_Spare.IsDeleted,0)=0 ) " + (Id != "0" && Id != "" ? " and ( JobCard.id=" + Id + " or JobCard.id in (Select JobCardid from Requisition where id=" + Id + "))" : "");
            DataTable dt = dbCon.GetDataTable(query + " Order By JobCard.DOM desc ");
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var objMain = new JobCardRequsition();
                    objMain.jobcardnumber = dt.Rows[i]["Id"].ToString();
                    objMain.vehiclename = dt.Rows[i]["Name"].ToString();
                    objMain.vehiclenumber = dt.Rows[i]["Number"].ToString();
                    objMain.doc = dt.Rows[i]["DOC"].ToString();
                    query = "SELECT [Id],case when isnull([IsAllocated],0)=1 then 'Green' else 'Red' end as color FROM [dbo].[Requisition] where JobCardId=" + dt.Rows[i]["Id"].ToString();
                    DataTable dt1 = dbCon.GetDataTable(query);
                    if (dt1 != null && dt1.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt1.Rows.Count; j++)
                        {
                            var obj = new JobCardRequsition.itms();
                            obj.colortype = dt1.Rows[j]["color"].ToString();
                            obj.id = dt1.Rows[j]["Id"].ToString();
                            objMain.requsition.Add(obj);
                        }
                    }
                    objectToSerilize.Add(objMain);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetJobcardRequsition Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void GetJobCardRequsitionDoneV1(string Id = "0")
    {
        var objectToSerilize = new List<JobCardRequsition>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string query = "SELECT  CONVERT(VARCHAR(12), JobCard.DOC, 109) as DOC,JobCard.Id, Vehicle.Number, isnull(Vehicle_Model.Name,'') + ',' + isnull(Vehicle_Variant.Name,'') + ',' + Vehicle_Brand.Name AS Name FROM            Vehicle_Brand RIGHT OUTER JOIN Vehicle ON Vehicle_Brand.Id = Vehicle.Vehicle_Brand_Id LEFT OUTER JOIN Vehicle_Variant ON Vehicle.Vehicle_Variant_Id = Vehicle_Variant.Id LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id RIGHT OUTER JOIN Requisition INNER JOIN JobCard ON Requisition.JobCardId = JobCard.Id ON Vehicle.Id =JobCard.Vehicle_Id GROUP BY JobCard.DOM,JobCard.DOC,JobCard.Id, Vehicle_Model.Name, Vehicle_Variant.Name, Vehicle_Brand.Name, Vehicle.Number having JobCard.id not in (Select  JobCardId from Requisition_Spare where isnull(Requisition_Spare.IsAllocate,0)=0 And isnull(Requisition_Spare.IsDeleted,0)=0)  " + (Id != "0" && Id != "" ? " and ( JobCard.id=" + Id + " or JobCard.id in (Select JobCardid from Requisition where id=" + Id + "))" : "");
            DataTable dt = dbCon.GetDataTable(query + " Order By JobCard.DOM  desc ");
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var objMain = new JobCardRequsition();
                    objMain.jobcardnumber = dt.Rows[i]["Id"].ToString();
                    objMain.vehiclename = dt.Rows[i]["Name"].ToString();
                    objMain.vehiclenumber = dt.Rows[i]["Number"].ToString();
                    objMain.doc = dt.Rows[i]["DOC"].ToString();
                    query = "SELECT [Id],case when isnull([IsAllocated],0)=1 then 'Green' else 'Red' end as color FROM [dbo].[Requisition] where JobCardId=" + dt.Rows[i]["Id"].ToString();
                    DataTable dt1 = dbCon.GetDataTable(query);
                    if (dt1 != null && dt1.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt1.Rows.Count; j++)
                        {
                            var obj = new JobCardRequsition.itms();
                            obj.colortype = dt1.Rows[j]["color"].ToString();
                            obj.id = dt1.Rows[j]["Id"].ToString();
                            objMain.requsition.Add(obj);
                        }
                    }
                    objectToSerilize.Add(objMain);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetJobcardRequsition Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void GetJobCardRequsition()
    {
        var objectToSerilize = new List<JobCardRequsition>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string query = "SELECT JobCard.Id, Vehicle.Number, isnull(Vehicle_Model.Name,'') + ',' + isnull(Vehicle_Variant.Name,'') + ',' + Vehicle_Brand.Name AS Name FROM  Vehicle_Brand RIGHT OUTER JOIN Vehicle ON Vehicle_Brand.Id = Vehicle.Vehicle_Brand_Id LEFT OUTER JOIN Vehicle_Variant ON Vehicle.Vehicle_Variant_Id = Vehicle_Variant.Id LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id RIGHT OUTER JOIN Requisition INNER JOIN JobCard ON Requisition.JobCardId = JobCard.Id ON Vehicle.Id =JobCard.Vehicle_Id GROUP BY JobCard.Id, Vehicle_Model.Name, Vehicle_Variant.Name, Vehicle_Brand.Name, Vehicle.Number having JobCard.id in (Select  JobCardId from Requisition_Spare where isnull(Requisition_Spare.IsAllocate,0)=0 And isnull(Requisition_Spare.IsDeleted,0)=0 )";
            DataTable dt = dbCon.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var objMain = new JobCardRequsition();
                    objMain.jobcardnumber = dt.Rows[i]["Id"].ToString();
                    objMain.vehiclename = dt.Rows[i]["Name"].ToString();
                    objMain.vehiclenumber = dt.Rows[i]["Number"].ToString();

                    query = "SELECT [Id],case when isnull([IsAllocated],0)=1 then 'Green' else 'Red' end as color FROM [dbo].[Requisition] where JobCardId=" + dt.Rows[i]["Id"].ToString();
                    DataTable dt1 = dbCon.GetDataTable(query);
                    if (dt1 != null && dt1.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt1.Rows.Count; j++)
                        {
                            var obj = new JobCardRequsition.itms();
                            obj.colortype = dt1.Rows[j]["color"].ToString();
                            obj.id = dt1.Rows[j]["Id"].ToString();
                            objMain.requsition.Add(obj);
                        }
                    }
                    objectToSerilize.Add(objMain);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetJobcardRequsition Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void GetJobCardRequsitionDone()
    {
        var objectToSerilize = new List<JobCardRequsition>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string query = "SELECT  JobCard.Id, Vehicle.Number, isnull(Vehicle_Model.Name,'') + ',' + isnull(Vehicle_Variant.Name,'') + ',' + Vehicle_Brand.Name AS Name FROM            Vehicle_Brand RIGHT OUTER JOIN Vehicle ON Vehicle_Brand.Id = Vehicle.Vehicle_Brand_Id LEFT OUTER JOIN Vehicle_Variant ON Vehicle.Vehicle_Variant_Id = Vehicle_Variant.Id LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id RIGHT OUTER JOIN Requisition INNER JOIN JobCard ON Requisition.JobCardId = JobCard.Id ON Vehicle.Id =JobCard.Vehicle_Id GROUP BY JobCard.Id, Vehicle_Model.Name, Vehicle_Variant.Name, Vehicle_Brand.Name, Vehicle.Number having JobCard.id not in (Select  JobCardId from Requisition_Spare where isnull(Requisition_Spare.IsAllocate,0)=0 And isnull(Requisition_Spare.IsDeleted,0)=0)";
            DataTable dt = dbCon.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var objMain = new JobCardRequsition();
                    objMain.jobcardnumber = dt.Rows[i]["Id"].ToString();
                    objMain.vehiclename = dt.Rows[i]["Name"].ToString();
                    objMain.vehiclenumber = dt.Rows[i]["Number"].ToString();

                    query = "SELECT [Id],case when isnull([IsAllocated],0)=1 then 'Green' else 'Red' end as color FROM [dbo].[Requisition] where JobCardId=" + dt.Rows[i]["Id"].ToString();
                    DataTable dt1 = dbCon.GetDataTable(query);
                    if (dt1 != null && dt1.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt1.Rows.Count; j++)
                        {
                            var obj = new JobCardRequsition.itms();
                            obj.colortype = dt1.Rows[j]["color"].ToString();
                            obj.id = dt1.Rows[j]["Id"].ToString();
                            objMain.requsition.Add(obj);
                        }
                    }
                    objectToSerilize.Add(objMain);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetJobcardRequsition Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }
    [WebMethod]
    public void GetRequsitionById(string Id)
    {
        var objectToSerilize = new Requsition();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string[] StrArr = { Id };
            try
            {
                dbCon.ExecuteQuery("UPDATE [dbo].[Requisition_Spare] SET [Quantity] = (Select Quantity from JobCard_Spare_Mapping where id= [SpareMappingId])  WHERE RequisitionId=" + Id + " and isnull(IsAllocate,0)=0");
            }
            catch (Exception E) { }
            string query = "SELECT  JobCard.Id, Vehicle.Number, isnull(Vehicle_Model.Name,'') + ',' + isnull(Vehicle_Variant.Name,'') + ',' + Vehicle_Brand.Name AS Name FROM Requisition INNER JOIN JobCard ON Requisition.JobCardId = JobCard.Id INNER JOIN Vehicle ON JobCard.Vehicle_Id = Vehicle.Id LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id LEFT OUTER JOIN Vehicle_Variant ON Vehicle.Vehicle_Variant_Id = Vehicle_Variant.Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id GROUP BY JobCard.Id, Vehicle_Model.Name, Vehicle_Variant.Name, Vehicle_Brand.Name, Vehicle.Number, Vehicle.Number,Requisition.Id having Requisition.Id=@1 ";
            DataTable dt = dbCon.GetDataTableWithParams(query, StrArr);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objectToSerilize.jobcardnumber = dt.Rows[i]["Id"].ToString();
                    objectToSerilize.requsitionnumber = Id;
                    objectToSerilize.vehicle = dt.Rows[i]["Name"].ToString() + ',' + dt.Rows[i]["Number"].ToString();
                    query = "SELECT IsDeleted,[Id],(select Code from Spare where id=spareid) as sname,[SpareId],isnull([Quantity],0) as [Quantity],isnull([ReceivedQuantity],0) as [ReceivedQuantity],[IsAllocate],isnull((Select top 1 amount from JobCard_Spare_Mapping where JobCard_Spare_Mapping.SpareId=[Requisition_Spare].SpareId and JobCard_Spare_Mapping.JobCardId=[Requisition_Spare].JobCardId and id=[Requisition_Spare].SpareMappingId ),0) as Amount,isnull((Select max(price) from GRNDetail where EntityId= SpareId),0) as PurchasePrice,TrnNo FROM [dbo].[Requisition_Spare] where RequisitionId=@1 and Requisition_Spare.SpareMappingId in (Select id from JobCard_Spare_Mapping where isnull(IsDeleted,0)=0)";
                    DataTable dt1 = dbCon.GetDataTableWithParams(query, StrArr);
                    if (dt1 != null && dt1.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt1.Rows.Count; j++)
                        {
                            var obj = new Requsition.itms();
                            obj.id = dt1.Rows[j]["Id"].ToString();
                            obj.Name = dt1.Rows[j]["sname"].ToString();
                            obj.qty = dt1.Rows[j]["Quantity"].ToString();
                            obj.rqty = dt1.Rows[j]["ReceivedQuantity"].ToString();
                            obj.isallocate = dt1.Rows[j]["IsAllocate"].ToString();
                            obj.isdeleted = dt1.Rows[j]["IsDeleted"].ToString();
                            obj.price = dt1.Rows[j]["Amount"].ToString();
                            obj.partid = dt1.Rows[j]["SpareId"].ToString();
                            obj.pp = dt1.Rows[j]["PurchasePrice"].ToString();
                            obj.Trn = dt1.Rows[j]["TrnNo"].ToString();
                            query = " Select id as Brandid,name,isnull((select sum(qty) from SpareSpareBrandMapping where SpareId=" + dt1.Rows[j]["SpareId"].ToString() + " and BrandId=Spare_Brand.Id),0) as Qty from Spare_Brand where isnull(IsDelete,0)=0 order by Qty desc,name";
                            DataTable dt2 = dbCon.GetDataTable(query);
                            for (int k = 0; k < dt2.Rows.Count; k++)
                            {
                                var objsub = new Requsition.itms.availablebrand();
                                objsub.id = dt2.Rows[k]["BrandId"].ToString();
                                objsub.Name = dt2.Rows[k]["name"].ToString();
                                objsub.Qty = dt2.Rows[k]["qty"].ToString();
                                obj.itm.Add(objsub);
                            }
                            objectToSerilize.requsition.Add(obj);
                        }
                    }
                }
                dt = dbCon.GetDataTable("Select Id,Code From Spare where isnull(IsDeleted,0)=0");
                Requsition.Parts prt;
                for (int i = 0; i <= dt.Rows.Count; i++)
                {
                    prt = new Requsition.Parts();
                    prt.id = dt.Rows[i]["Id"].ToString();
                    prt.Name = dt.Rows[i]["Code"].ToString();
                    objectToSerilize.parts.Add(prt);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetRequisitionById Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }
    [WebMethod]
    public void UpdatePartAtAll(string Id, string PartId)
    {
        var objectToSerilize = new Objs();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        objectToSerilize.Id = "0";
        try
        {
            if (dbCon.ExecuteQuery("UPDATE [dbo].[Requisition_Spare] SET [SpareId] =" + PartId + " where SpareMappingId = (Select [Requisition_Spare].SpareMappingId from [Requisition_Spare] where isnull([Requisition_Spare].IsDeleted,0)=0 and id=" + Id + " );") > 0)
            {
                if (dbCon.ExecuteQuery("UPDATE [dbo].[JobCard_Spare_Mapping] SET [SpareId] =" + PartId + " where Id = (Select [Requisition_Spare].SpareMappingId from [Requisition_Spare] where isnull([Requisition_Spare].IsDeleted,0)=0 and  id=" + Id + " );") > 0)
                {
                    dbCon.ExecuteQuery("UPDATE [dbo].[Estimate_Spare_Mapping] SET [SpareId] =" + PartId + " where [MappingId] = (Select [Requisition_Spare].SpareMappingId from [Requisition_Spare] where isnull([Requisition_Spare].IsDeleted,0)=0 and id=" + Id + " ); ");
                    objectToSerilize.Id = "1";
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " UpdatePartAtAll Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }
    [WebMethod]
    public void GetRequsitionByIdDone(string Id)
    {
        var objectToSerilize = new Requsition();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string[] StrArr = { Id };
            string query = "SELECT  JobCard.Id, Vehicle.Number, isnull(Vehicle_Model.Name,'') + ',' + isnull(Vehicle_Variant.Name,'') + ',' + Vehicle_Brand.Name AS Name FROM Requisition INNER JOIN JobCard ON Requisition.JobCardId = JobCard.Id INNER JOIN Vehicle ON JobCard.Vehicle_Id = Vehicle.Id LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id LEFT OUTER JOIN Vehicle_Variant ON Vehicle.Vehicle_Variant_Id = Vehicle_Variant.Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id GROUP BY JobCard.Id, Vehicle_Model.Name, Vehicle_Variant.Name, Vehicle_Brand.Name, Vehicle.Number, Vehicle.Number,Requisition.Id having Requisition.Id=@1 ";
            DataTable dt = dbCon.GetDataTableWithParams(query, StrArr);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    objectToSerilize.jobcardnumber = dt.Rows[i]["Id"].ToString();
                    objectToSerilize.requsitionnumber = Id;
                    objectToSerilize.vehicle = dt.Rows[i]["Name"].ToString() + ',' + dt.Rows[i]["Number"].ToString();
                    query = "SELECT IsDeleted,[Id],(select Code from Spare where id=spareid)+','+isnull((SELECT top 1 Spare_Brand.Name FROM SpareInventaryHistory INNER JOIN SpareSpareBrandMapping ON SpareInventaryHistory.SpareSpareBrandMappingId = SpareSpareBrandMapping.Id INNER JOIN Spare_Brand ON SpareSpareBrandMapping.BrandId = Spare_Brand.Id WHERE  (SpareInventaryHistory.RequsitionSpareId = [Requisition_Spare].Id)),'') as sname,[SpareId],isnull([Quantity],0) as [Quantity],isnull([ReceivedQuantity],0) as [ReceivedQuantity],[IsAllocate],isnull((Select top 1 amount from JobCard_Spare_Mapping where JobCard_Spare_Mapping.SpareId=[Requisition_Spare].SpareId and JobCard_Spare_Mapping.JobCardId=[Requisition_Spare].JobCardId and id=[Requisition_Spare].SpareMappingId ),0) as Amount,isnull((Select max(price) from GRNDetail where EntityId= SpareId),0) as PurchasePrice FROM [dbo].[Requisition_Spare] where RequisitionId=@1";
                    DataTable dt1 = dbCon.GetDataTableWithParams(query, StrArr);
                    if (dt1 != null && dt1.Rows.Count > 0)
                    {
                        for (int j = 0; j < dt1.Rows.Count; j++)
                        {
                            var obj = new Requsition.itms();
                            obj.id = dt1.Rows[j]["Id"].ToString();
                            obj.Name = dt1.Rows[j]["sname"].ToString();
                            obj.qty = dt1.Rows[j]["Quantity"].ToString();
                            obj.rqty = dt1.Rows[j]["ReceivedQuantity"].ToString();
                            obj.isallocate = dt1.Rows[j]["IsAllocate"].ToString();
                            obj.isdeleted = dt1.Rows[j]["IsDeleted"].ToString();
                            obj.price = dt1.Rows[j]["Amount"].ToString();
                            obj.pp = dt1.Rows[j]["PurchasePrice"].ToString();
                            //query = "SELECT [BrandId],(select name from Spare_Brand where id=BrandId) as name,Qty FROM [dbo].[SpareSpareBrandMapping] where [SpareId]=" + dt1.Rows[j]["SpareId"].ToString();
                            //DataTable dt2 = dbCon.GetDataTable(query);
                            //for (int k = 0; k < dt2.Rows.Count; k++)
                            //{
                            //    var objsub = new Requsition.itms.availablebrand();
                            //    objsub.id = dt2.Rows[k]["BrandId"].ToString();
                            //    objsub.Name = dt2.Rows[k]["name"].ToString();
                            //    objsub.Qty = dt2.Rows[k]["qty"].ToString();
                            //    obj.itm.Add(objsub);
                            //}
                            objectToSerilize.requsition.Add(obj);
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetRequisitionById Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void UpdateRequsition(string str, string dtdt)
    {
        var objectToSerilize = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string strRequisitionId = "";
            objectToSerilize.Id = "0";
            string UserId = (HttpContext.Current.Request.Cookies["TUser"]["Id"] != null ? HttpContext.Current.Request.Cookies["TUser"]["Id"] : "0");
            string[] strArrResult = str.Split(new string[] { "$#@" }, StringSplitOptions.None);
            for (int i = 0; i < strArrResult.Length; i++)
            {
                string[] strArrItem = strArrResult[i].Split(new string[] { "," }, StringSplitOptions.None);
                if (strArrItem != null && strArrItem.Length >= 4)
                {
                    string query = "SELECT [SpareId],isnull((Quantity-isnull(ReceivedQuantity,0)),0) as qty,RequisitionId FROM [dbo].[Requisition_Spare] where id=" + strArrItem[0];
                    DataTable dt2 = dbCon.GetDataTable(query);
                    if (dt2 != null && dt2.Rows.Count > 0)
                    {
                        string SpareId = dt2.Rows[0][0].ToString();
                        string QtyNeeded = dt2.Rows[0][1].ToString();
                        string RequsitionId = dt2.Rows[0][2].ToString();
                        if (String.IsNullOrWhiteSpace(strRequisitionId))
                        {
                            strRequisitionId = RequsitionId;
                        }
                        query = "SELECT isnull(Qty,0) as Qty,Id FROM [dbo].[SpareSpareBrandMapping] where BrandId=" + strArrItem[1] + " And  [SpareId]=" + SpareId;
                        DataTable dt = dbCon.GetDataTable(query);
                        string SpareSpareBrandId = "0";
                        string AvailableQty = "0";
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            SpareSpareBrandId = dt.Rows[0][1].ToString();
                            AvailableQty = dt.Rows[0][0].ToString();
                            //if (float.Parse(AvailableQty) >= float.Parse(strArrItem[2]) && float.Parse(QtyNeeded) >= float.Parse(strArrItem[2]))                        
                        }
                        else
                        {
                            string[] strarr1 = { };
                            SpareSpareBrandId = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[SpareSpareBrandMapping] ([SpareId],[BrandId],[Qty],[MinQty],[MaxQty],[IsActive],[IsDeleted],[DOC],[DOM],[HSNNumber],Mrp,WorkshopId)VALUES(" + SpareId + "," + strArrItem[1] + ",'0',1,1000,1,0,'" + dbCon.getindiantime() + "','" + dbCon.getindiantime() + "','','0',1)  SELECT SCOPE_IDENTITY();", strarr1).ToString();
                        }
                        if (float.Parse(QtyNeeded) >= float.Parse(strArrItem[2]))
                        {
                            string[] strArrP = { strArrItem[2], strArrItem[0], SpareSpareBrandId };
                            string msg = strArrItem[2] + " Qty Debited in invetory >> Requisition Id=" + RequsitionId;
                            query = "INSERT INTO [dbo].[SpareInventaryHistory] ([SpareSpareBrandMappingId],[Cr],[Dr],[DOC],[JobCardId],[OrderItemDetailId],[GRNDetailId],[RequsitionSpareId],[WorkshopId],[message]) VALUES(" + SpareSpareBrandId + ",0,@1,CONVERT(datetime, SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30')),0,0,0,@2,1,'" + msg + "')";
                            if (dbCon.ExecuteQueryWithParams(query, strArrP) > 0)
                            {
                                query = "UPDATE [dbo].[SpareSpareBrandMapping] SET [Qty] =([Qty]-@1)  WHERE  id=@3";
                                if (dbCon.ExecuteQueryWithParams(query, strArrP) > 0)
                                {
                                    query = "UPDATE [dbo].[Requisition_Spare] SET [ReceivedQuantity] =(isnull([ReceivedQuantity],0)+@1) ,IsAllocate=" + (float.Parse(QtyNeeded) == float.Parse(strArrItem[2]) ? "1" : "0") + ",AllocateTime='" + dbCon.getindiantime() + "',AllocateBy=" + UserId + " WHERE id=@2";
                                    if (dbCon.ExecuteQueryWithParams(query, strArrP) > 0)
                                    {
                                        objectToSerilize.Id = "1";
                                        query = "SELECT Id FROM [dbo].[Requisition_Spare] where RequisitionId=" + RequsitionId + " and IsAllocate=0 and ISNULL(IsDeleted,0)=0";
                                        DataTable dtv = dbCon.GetDataTable(query);
                                        if (dtv.Rows.Count == 0)
                                        {
                                            query = "UPDATE [dbo].[Requisition] SET [IsAllocated] = 1,AllocatedTime='" + dtdt + "' where id=" + RequsitionId;
                                            dbCon.ExecuteQuery(query);
                                        }
                                    }
                                }
                            }
                            try
                            {
                                //Barcode
                                if (strArrItem[4] != "0")
                                {
                                    dbCon.ExecuteQuery("INSERT INTO [dbo].[GrnVsRequisition]([GrnDetailId],[RequisitionSpareId],[Doc],Qty) VALUES (" + strArrItem[4] + "," + strArrItem[0] + ",'" + dbCon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss") + "','" + strArrItem[2] + "')");
                                }
                            }
                            catch (Exception E) { }
                        }
                    }
                }
                try
                {
                    if (strArrItem != null && strArrItem.Length >= 4)
                    {
                        string query = "update JobCard_Spare_Mapping set Amount='" + strArrItem[3] + "' where id =( SELECT SpareMappingId FROM [dbo].[Requisition_Spare] where id=" + strArrItem[0] + ")";
                        dbCon.ExecuteQuery(query);
                    }
                }
                catch (Exception E) { }
            }
            try
            {
                string WorkshopId = HttpContext.Current.Request.Cookies["TUser"]["WorkshopId"];

                UserActivity objUserAct = new UserActivity();
                objUserAct.InsertUserActivity("Allocate Parts to Requisition No. : " + strRequisitionId, UserId, WorkshopId, "", "Update", "Requisition");
                dbCon.ExecuteQuery("Update Requisition set Allocatedby=" + UserId + " Where id=" + strRequisitionId);
            }
            catch (Exception E) { }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " UpdateRequisition Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void DeleteRequsition(string id)
    {
        var objectToSerilize = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        objectToSerilize.Name = "Fail";
        try
        {
            objectToSerilize.Id = "0";
            string[] strArrP = { id };
            string str = "Select id from Requisition_Spare where RequisitionId=@1 and IsAllocate=1";
            if (dbCon.GetDataTableWithParams(str, strArrP).Rows.Count == 0)
            {
                string query = "Delete From requisition where id=@1";
                if (dbCon.ExecuteQueryWithParams(query, strArrP) > 0)
                {
                    query = "Delete from Requisition_Spare where RequisitionId=@1";
                    if (dbCon.ExecuteQueryWithParams(query, strArrP) > 0)
                    {
                        objectToSerilize.Name = "Success";
                        try
                        {
                            string WorkshopId = HttpContext.Current.Request.Cookies["TUser"]["WorkshopId"];
                            string UserId = HttpContext.Current.Request.Cookies["TUser"]["Id"];
                            UserActivity objUserAct = new UserActivity();
                            objUserAct.InsertUserActivity("Deleted Requisition No. : " + id, UserId, WorkshopId, "", "Delete", "Requisition");
                        }
                        catch (Exception E) { }
                    }
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " DeleteRequsition Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void UpdateService(string id, string name, string isactive, string catid, string amount, string taxid, string hsn)
    {
        var objectToSerilize = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            dbConnection dbCon = new dbConnection();
            if (id != "0")
            {
                String[] Strarr = { name.Replace("'", "''"), (isactive == "true" ? "1" : "0"), id, catid, amount, taxid, hsn };
                dbCon.ExecuteQueryWithParams("update Service Set name=@1, IsActive=@2,CategoryId=@4,Price=@5,taxid=@6,Hsn=@7 where id=@3", Strarr);
            }
            else
            {
                String[] Strarr = { name.Replace("'", "''"), (isactive == "true" ? "1" : "0"), catid, amount, taxid, hsn };
                dbCon.ExecuteQueryWithParams("INSERT INTO [dbo].[Service] ([Name],[IsActive],[IsDeleted],[DOC],[DOM],CategoryId,Price,Taxid,Hsn) VALUES (@1,@2,0,getdate(),getdate(),@3,@4,@5,@6)", Strarr);
            }
            objectToSerilize.Id = "1";
        }
        catch (Exception e)
        {
            objectToSerilize.Id = "0";
            dbCon.InsertLogs(ErrorMsgPrefix + " UpdateService Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }


    [WebMethod]
    public void GetServiceById(string id)
    {
        var objectToSerilize = new Service();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            if (!String.IsNullOrWhiteSpace(id))
            {
                String[] Strarr = { id };
                DataTable dt = dbCon.GetDataTableWithParams("SELECT [Name],[IsActive],categoryid,Price,isnull(TaxId,0) as TaxId,Hsn FROM [dbo].[Service] where id=@1", Strarr);
                objectToSerilize.name = dt.Rows[0][0].ToString();
                //chkIsActive.Checked = (bool)dt.Rows[0][1];
                objectToSerilize.catid = dt.Rows[0][2].ToString();
                objectToSerilize.amount = dt.Rows[0][3].ToString();
                objectToSerilize.taxid = dt.Rows[0]["TaxId"].ToString();
                objectToSerilize.hsn = dt.Rows[0]["Hsn"].ToString();
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetServiceById Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void GetJobCardPayment(string id = "0")
    {
        var objMain = new List<JoCardAmount>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string query = "SELECT  JobCard.Id,isnull(Vehicle_Model.Name,'-') as Model, isnull(Vehicle_Brand.Name,'-') AS BrandName, isnull(Vehicle_Variant.Name,'-') AS Variant, isnull(Vehicle.Number,'-') as Number, isnull(Customer.Name,'-') AS CustomerName, isnull(Customer.Email,'-') as email, isnull(Customer.Mobile,'') mobile FROM   Customer RIGHT OUTER JOIN JobCard ON Customer.Id = JobCard.Customer_Id LEFT OUTER JOIN Vehicle LEFT OUTER JOIN Vehicle_Variant ON Vehicle.Vehicle_Variant_Id = Vehicle_Variant.Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id ON JobCard.Vehicle_Id = Vehicle.Id" + (id != "0" ? " where jobcard.id=" + id : "");
            DataTable dt = dbCon.GetDataTable(query);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var obj = new JoCardAmount();
                obj.CustomerName = dt.Rows[i]["CustomerName"].ToString();
                obj.Mobile = dt.Rows[i]["email"].ToString();
                obj.Email = dt.Rows[i]["mobile"].ToString();
                obj.JobCardId = dt.Rows[i]["Id"].ToString();
                obj.VehicleName = dt.Rows[i]["BrandName"].ToString() + "," + dt.Rows[i]["Model"].ToString() + "," + dt.Rows[i]["Variant"].ToString() + "," + dt.Rows[i]["Number"].ToString();
                query = "SELECT [Id],[JobCardId],[PaymentType],[ChequeNumber],[BankName],[TransactionNumber],[Amount],[WorkShopId],ChequeDate FROM [dbo].[JobCardPayment] where JobCardid=" + dt.Rows[i]["Id"].ToString();
                DataTable dt1 = dbCon.GetDataTable(query);
                for (int j = 0; j < dt1.Rows.Count; j++)
                {
                    var objsub = new JoCardAmount.Payment();
                    objsub.Amount = dt1.Rows[j]["Amount"].ToString();
                    objsub.BankName = dt1.Rows[j]["BankName"].ToString();
                    objsub.ChequeNumber = dt1.Rows[j]["ChequeNumber"].ToString();
                    objsub.PaymentType = dt1.Rows[j]["PaymentType"].ToString();
                    objsub.TrnId = dt1.Rows[j]["TransactionNumber"].ToString();
                    objsub.ChequeDate = dt1.Rows[j]["ChequeDate"].ToString();
                    obj.payment.Add(objsub);
                }
                objMain.Add(obj);
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetJobCard Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objMain));
    }

    [WebMethod]
    public void UpdateJobcardPayment(string date, string jocardid, string type, string amount, string chequenumber, string bank, string chequedate, string trnid = "", string receiptno = "", string id = "0", string remark = "", string paidBy = "")
    {
        var objectToSerilize = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string[] strArr = { jocardid, type, amount, chequenumber, bank, chequedate, date, remark, id, receiptno, paidBy };
            if (id == "0" || id == "-1")
            {
                string query = "INSERT INTO [dbo].[JobCardPayment]([JobCardId],[PaymentType],[ChequeNumber],[ChequeDate],[BankName],[Amount],[DOP],remark,ReceiptNo,PaidBy) VALUES (@1,@2,@4,@6,@5,@3,@7,@8,@10,@11) SELECT SCOPE_IDENTITY();";
                int i = dbCon.ExecuteScalarQueryWithParams(query, strArr);
                objectToSerilize.Id = i.ToString();
                objectToSerilize.Name = "Success";
            }
            else
            {
                string query = "update [dbo].[JobCardPayment] Set [PaymentType]=@2,[ChequeNumber]=@4,[BankName]=@5,[Amount]=@3,remark=@8,ChequeDate=@6,DOP=@7,ReceiptNo=@10,PaidBy=@11 where id=@9";
                int i = dbCon.ExecuteScalarQueryWithParams(query, strArr);
                objectToSerilize.Id = "0";
                objectToSerilize.Name = "Success";
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " InsertPayment Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }
    [WebMethod]
    public void GetHSN()
    {
        var objectToSerilize = new List<Objs2>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string query = "SELECT distinct HSNNumber FROM [dbo].[Spare]  where len(HSNNumber)>0";
            DataTable dt = dbCon.GetDataTable(query);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var obj = new Objs2();
                obj.Name = dt.Rows[i][0].ToString();
                objectToSerilize.Add(obj);
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetHSN Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }


    [WebMethod]
    public void GetJobcardVehicleDetailAndCustomerDetail()
    {
        var objectToSerilize = new List<Objs2>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string query = "SELECT distinct HSNNumber FROM [dbo].[Spare]  where len(HSNNumber)>0";
            DataTable dt = dbCon.GetDataTable(query);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var obj = new Objs2();
                obj.Name = dt.Rows[i][0].ToString();
                objectToSerilize.Add(obj);
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetHSN Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void testChart()
    {
        var objectToSerilize = new List<Objs3>();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            for (int i = 1; i <= 5; i++)
            {
                var obj = new Objs3();
                obj.label = "Name" + i.ToString();
                obj.y = "20";
                objectToSerilize.Add(obj);
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetHSN Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }
    [WebMethod]
    public void UD(string Spare, string Service)
    {
        decimal TotalCGSTForSpare = 0;
        decimal TotalSGSTforService = 0;
        decimal FinalAmount = 0;
        bool ShouldUpdateTotalGST = false;
        var objectToSerilize = new Objs2();
        string EstimateId = "0";
        string jobcard = "0";
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            //dbCon.InsertLogs(" Discount Msg:Spare - " + Spare + " Service - " + Service, "", "");
            //get Items
            string InvoiceId = "0";
            string[] strArrSpare = Spare.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrService = Service.Split(new string[] { "$#@" }, StringSplitOptions.None);
            //dbCon.InsertLogs(" Discount 1", "", "");
            for (int i = 0; i < strArrSpare.Length; i++)
            {
                string[] strItem = strArrSpare[i].Split(',');
                if (strItem.Length >= 4)
                {
                    //string query = "SELECT isnull([ActualAmountPerUnit],0)+isnull([DiscountPerUnit],0) as Amount,[Quantity],InvoiceId FROM [dbo].[Invoice_Spare_Mapping] where id=" + strItem[0];
                    string query = "SELECT isnull([ActualAmountPerUnit],0) as Amount,[Quantity], EstimateId as InvoiceId FROM [dbo].[Estimate_Spare_Mapping] where id=" + strItem[0];
                    DataTable dt = dbCon.GetDataTable(query);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        InvoiceId = dt.Rows[0]["InvoiceId"].ToString();

                        float ft_Qty = float.Parse(dt.Rows[0]["Quantity"].ToString());
                        decimal dc_Amount = decimal.Parse(dt.Rows[0]["Amount"].ToString());
                        bool isInPer = false;
                        if (strItem[1].Contains("%"))
                        {
                            isInPer = true;
                            strItem[1] = strItem[1].Replace("%", "");
                        }
                        decimal _finalamount = Convert.ToDecimal(strItem[2]);
                        decimal _SGSTamount = Convert.ToDecimal(strItem[3]);
                        TotalSGSTforService += _SGSTamount;
                        TotalCGSTForSpare += _SGSTamount;
                        decimal dc_DiscountPerUnit = (isInPer ? ((dc_Amount * decimal.Parse(strItem[1])) / 100) : decimal.Parse(strItem[1]));
                        decimal dc_AmountAfterDiscount = dc_Amount - dc_DiscountPerUnit;
                        decimal dc_TotalDiscount = dc_DiscountPerUnit * (decimal)ft_Qty;
                        decimal dc_TotalAmountAfterDiscount = dc_AmountAfterDiscount * (decimal)ft_Qty;
                        string[] strArr = { dc_DiscountPerUnit.ToString(), dc_AmountAfterDiscount.ToString(), dc_TotalDiscount.ToString(), dc_TotalAmountAfterDiscount.ToString(), strItem[0], _SGSTamount.ToString(), _finalamount.ToString() };
                        if (dc_DiscountPerUnit <= dc_Amount)
                        {
                            //string sql = "UPDATE [dbo].[Invoice_Spare_Mapping] SET [DiscountPerUnit] =@1 ,[ActualAmountPerUnit] =@2 ,[TotalDiscount] =@3 ,[TotalActualAmount] =@4   WHERE id=@5";
                            string sql = "UPDATE [dbo].[Estimate_Spare_Mapping] SET [DiscountPerUnit] =@1 ,[TotalDiscount] =@3,SGSTAmount=@6, CGSTAmount=@6,TotalActualAmount=(@7+(Quantity*InsuranceAmount))   WHERE id=@5";
                            dbCon.ExecuteQueryWithParams(sql, strArr);
                            ShouldUpdateTotalGST = true;
                            EstimateId = InvoiceId;
                        }
                    }
                }
            }
            // dbCon.InsertLogs(" Discount 2", "", "");
            for (int i = 0; i < strArrService.Length; i++)
            {
                string[] strItem = strArrService[i].Split(',');
                if (strItem.Length >= 4)
                {
                    //string query = "SELECT InvoiceId,isnull([ActualAmountPerUnit],0)+isnull([DiscountPerUnit],0) as Amount,SGSTAmount,CGSTAmount,[Quantity] FROM [dbo].[Invoice_Service_Mapping] where id=" + strItem[0];
                    string query = "SELECT EstimateId as InvoiceId,isnull([ActualAmountPerUnit],0) as Amount,SGSTAmount,CGSTAmount,[Quantity] FROM [dbo].[Estimate_Service_Mapping] where id=" + strItem[0];
                    DataTable dt = dbCon.GetDataTable(query);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        InvoiceId = dt.Rows[0]["InvoiceId"].ToString();
                        float ft_Qty = float.Parse(dt.Rows[0]["Quantity"].ToString());
                        decimal dc_Amount = decimal.Parse(dt.Rows[0]["Amount"].ToString());
                        decimal SGSTAmount = decimal.Parse(dt.Rows[0]["SGSTAmount"].ToString());
                        decimal CGSTAmount = decimal.Parse(dt.Rows[0]["CGSTAmount"].ToString());

                        bool isInPer = false;
                        if (strItem[1].Contains("%"))
                        {
                            isInPer = true;
                            strItem[1] = strItem[1].Replace("%", "");
                        }
                        decimal _finalamount = Convert.ToDecimal(strItem[2]);
                        decimal _SGSTamount = Convert.ToDecimal(strItem[3]);
                        TotalSGSTforService += _SGSTamount;
                        TotalCGSTForSpare += _SGSTamount; ;
                        decimal dc_DiscountPerUnit = (isInPer ? ((((dc_Amount - SGSTAmount) - CGSTAmount) * decimal.Parse(strItem[1])) / 100) : decimal.Parse(strItem[1]));
                        //decimal dc_DiscountPerUnit = decimal.Parse(strItem[1]);
                        decimal dc_AmountAfterDiscount = dc_Amount - dc_DiscountPerUnit;
                        decimal dc_TotalDiscount = dc_DiscountPerUnit * (decimal)ft_Qty;
                        decimal dc_TotalAmountAfterDiscount = dc_AmountAfterDiscount * (decimal)ft_Qty;
                        string[] strArr = { dc_DiscountPerUnit.ToString(), dc_AmountAfterDiscount.ToString(), dc_TotalDiscount.ToString(), dc_TotalAmountAfterDiscount.ToString(), strItem[0], _SGSTamount.ToString(), _finalamount.ToString() };
                        if (dc_DiscountPerUnit <= dc_Amount)
                        {
                            //  string sql = "UPDATE [dbo].[Invoice_Service_Mapping] SET [DiscountPerUnit] =@1 ,[ActualAmountPerUnit] =@2 ,[TotalDiscount] =@3 ,[TotalActualAmount] =@4   WHERE id=@5";
                            string sql = "UPDATE [dbo].[Estimate_Service_Mapping] SET [DiscountPerUnit] =@1 ,[TotalDiscount] =@3,SGSTAmount=@6, CGSTAmount=@6,TotalActualAmount=(@7+(Quantity*(InsuranceAmount+((InsuranceAmount*18)/100))))  WHERE id=@5";
                            dbCon.ExecuteQueryWithParams(sql, strArr);
                            ShouldUpdateTotalGST = true;
                            EstimateId = InvoiceId;
                        }
                    }
                }
            }
            // dbCon.InsertLogs(" Discount 3", "", "");
            //try
            //{
            //    string strq = "Select (select Sum(isnull(TotalActualAmount,0)) from Invoice_Spare_Mapping where InvoiceId=" + InvoiceId + ") as TotalSpare ,(select Sum(isnull(TotalActualAmount,0)) from Invoice_Service_Mapping where InvoiceId=" + InvoiceId + ") as TotalService,(select Sum(isnull(TotalDiscount,0)) from Invoice_Spare_Mapping where InvoiceId=" + InvoiceId + ") as TotalDiscountSpare,(select Sum(isnull(TotalDiscount,0)) from Invoice_Service_Mapping where InvoiceId=" + InvoiceId + ") as TotalDiscountService ";
            //    DataTable dt = dbCon.GetDataTable(strq);
            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        decimal dc_SpareAmount = decimal.Parse(dt.Rows[0]["TotalSpare"].ToString());
            //        decimal dc_ServiceAmount = decimal.Parse(dt.Rows[0]["TotalService"].ToString());
            //        decimal dc_SpareDiscountAmount = decimal.Parse(dt.Rows[0]["TotalDiscountSpare"].ToString());
            //        decimal dc_ServiceDiscountAmount = decimal.Parse(dt.Rows[0]["TotalDiscountService"].ToString());

            //        strq = "UPDATE [dbo].[Invoice] SET [TotalSpareAmount] = " + dc_SpareAmount + " ,[TotalServiceAmount] = " + dc_ServiceAmount + " , [FinalTotal] =((isnull([FinalTotal],0)+isnull([TotalDiscount],0))-" + (dc_SpareDiscountAmount + dc_ServiceDiscountAmount) + "),[TotalDiscount] = " + (dc_SpareDiscountAmount + dc_ServiceDiscountAmount) + "   where id=" + InvoiceId;
            //        dbCon.ExecuteQuery(strq);
            //    }
            //}
            //catch (Exception E)
            //{

            //}
            if (ShouldUpdateTotalGST && !String.IsNullOrWhiteSpace(EstimateId) && EstimateId != "0")
            {
                string query = "Update Estimate set TotalSGST=@2 , TotalCGST=@3  where Id=@1 ";
                string[] param = { EstimateId, TotalCGSTForSpare.ToString(), TotalSGSTforService.ToString() };
                dbCon.ExecuteQueryWithParams(query, param);
            }
            // dbCon.InsertLogs(" Discount 4", "", "");
            objectToSerilize.Name = "Success";

            //try
            //{
            //    string aa = urlLink + "EstimateNewDesign?JobCardId=" + jobcard + "&UserId=1002";
            //    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(aa);

            //    HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            //    if (response.StatusCode == HttpStatusCode.OK)
            //    {
            //    }
            //}
            //catch (Exception E) { }
        }
        catch (Exception e)
        {
            objectToSerilize.Name = "Fail";
            dbCon.InsertLogs(ErrorMsgPrefix + " UpdateDiscount Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }


    //Made By Jaydeep (JD)
    [WebMethod]
    public void UD1(string Spare, string Service)
    {
        var objectToSerilize = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            // dbCon.InsertLogs(" Discount Msg:Spare - " + Spare + " Service - " + Service, "", "");
            //get Items
            string InvoiceId = "0";
            string[] strArrSpare = Spare.Split(new string[] { "$#@" }, StringSplitOptions.None);
            string[] strArrService = Service.Split(new string[] { "$#@" }, StringSplitOptions.None);
            dbCon.InsertLogs(" Discount 1", "", "");
            for (int i = 0; i < strArrSpare.Length; i++)
            {
                string[] strItem = strArrSpare[i].Split(',');
                if (strItem.Length >= 2)
                {
                    //string query = "SELECT isnull([ActualAmountPerUnit],0)+isnull([DiscountPerUnit],0) as Amount,[Quantity],InvoiceId FROM [dbo].[Invoice_Spare_Mapping] where id=" + strItem[0];
                    string query = "SELECT isnull([ActualAmountPerUnit],0) as Amount,[Quantity],InvoiceId FROM [dbo].[Invoice_Spare_Mapping] where id=" + strItem[0];
                    DataTable dt = dbCon.GetDataTable(query);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        InvoiceId = dt.Rows[0]["InvoiceId"].ToString();
                        float ft_Qty = float.Parse(dt.Rows[0]["Quantity"].ToString());
                        decimal dc_Amount = decimal.Parse(dt.Rows[0]["Amount"].ToString());
                        float famount = (float.Parse(dc_Amount.ToString()) / ft_Qty);
                        dc_Amount = Convert.ToDecimal((famount));

                        bool isInPer = false;
                        if (strItem[1].Contains("%"))
                        {
                            isInPer = true;
                            strItem[1] = strItem[1].Replace("%", "");
                        }
                        decimal dc_DiscountPerUnit = (isInPer ? ((dc_Amount * decimal.Parse(strItem[1])) / 100) : Convert.ToDecimal(float.Parse(strItem[1]) / ft_Qty));
                        decimal dc_AmountAfterDiscount = dc_Amount - dc_DiscountPerUnit;
                        decimal dc_TotalDiscount = dc_DiscountPerUnit * (decimal)ft_Qty;
                        decimal dc_TotalAmountAfterDiscount = dc_AmountAfterDiscount * (decimal)ft_Qty;
                        string[] strArr = { dc_DiscountPerUnit.ToString(), dc_AmountAfterDiscount.ToString(), dc_TotalDiscount.ToString(), dc_TotalAmountAfterDiscount.ToString(), strItem[0] };
                        if (dc_DiscountPerUnit <= dc_Amount)
                        {
                            //string sql = "UPDATE [dbo].[Invoice_Spare_Mapping] SET [DiscountPerUnit] =@1 ,[ActualAmountPerUnit] =@2 ,[TotalDiscount] =@3 ,[TotalActualAmount] =@4   WHERE id=@5";
                            string sql = "UPDATE [dbo].[Invoice_Spare_Mapping] SET [DiscountPerUnit] =@1 ,[TotalDiscount] =@3   WHERE id=@5";
                            dbCon.ExecuteQueryWithParams(sql, strArr);
                        }

                    }
                }
            }
            //dbCon.InsertLogs(" Discount 2", "", "");
            for (int i = 0; i < strArrService.Length; i++)
            {
                string[] strItem = strArrService[i].Split(',');
                if (strItem.Length >= 2)
                {
                    //string query = "SELECT InvoiceId,isnull([ActualAmountPerUnit],0)+isnull([DiscountPerUnit],0) as Amount,SGSTAmount,CGSTAmount,[Quantity] FROM [dbo].[Invoice_Service_Mapping] where id=" + strItem[0];
                    string query = "SELECT InvoiceId,isnull([ActualAmountPerUnit],0) as Amount,SGSTAmount,CGSTAmount,[Quantity] FROM [dbo].[Invoice_Service_Mapping] where id=" + strItem[0];
                    DataTable dt = dbCon.GetDataTable(query);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        InvoiceId = dt.Rows[0]["InvoiceId"].ToString();
                        float ft_Qty = float.Parse(dt.Rows[0]["Quantity"].ToString());
                        decimal dc_Amount = decimal.Parse(dt.Rows[0]["Amount"].ToString());
                        decimal SGSTAmount = decimal.Parse(dt.Rows[0]["SGSTAmount"].ToString());
                        decimal CGSTAmount = decimal.Parse(dt.Rows[0]["CGSTAmount"].ToString());

                        bool isInPer = false;
                        if (strItem[1].Contains("%"))
                        {
                            isInPer = true;
                            strItem[1] = strItem[1].Replace("%", "");
                        }
                        decimal dc_DiscountPerUnit = (isInPer ? ((((dc_Amount - SGSTAmount) - CGSTAmount) * decimal.Parse(strItem[1])) / 100) : decimal.Parse(strItem[1]));

                        //decimal dc_DiscountPerUnit = decimal.Parse(strItem[1]);
                        decimal dc_AmountAfterDiscount = dc_Amount - dc_DiscountPerUnit;
                        decimal dc_TotalDiscount = dc_DiscountPerUnit * (decimal)ft_Qty;
                        decimal dc_TotalAmountAfterDiscount = dc_AmountAfterDiscount * (decimal)ft_Qty;
                        string[] strArr = { dc_DiscountPerUnit.ToString(), dc_AmountAfterDiscount.ToString(), dc_TotalDiscount.ToString(), dc_TotalAmountAfterDiscount.ToString(), strItem[0] };
                        if (dc_DiscountPerUnit <= dc_Amount)
                        {
                            //  string sql = "UPDATE [dbo].[Invoice_Service_Mapping] SET [DiscountPerUnit] =@1 ,[ActualAmountPerUnit] =@2 ,[TotalDiscount] =@3 ,[TotalActualAmount] =@4   WHERE id=@5";
                            string sql = "UPDATE [dbo].[Invoice_Service_Mapping] SET [DiscountPerUnit] =@1 ,[TotalDiscount] =@3  WHERE id=@5";
                            dbCon.ExecuteQueryWithParams(sql, strArr);
                        }
                    }
                }
            }
            //dbCon.InsertLogs(" Discount 3", "", "");
            //try
            //{
            //    string strq = "Select (select Sum(isnull(TotalActualAmount,0)) from Invoice_Spare_Mapping where InvoiceId=" + InvoiceId + ") as TotalSpare ,(select Sum(isnull(TotalActualAmount,0)) from Invoice_Service_Mapping where InvoiceId=" + InvoiceId + ") as TotalService,(select Sum(isnull(TotalDiscount,0)) from Invoice_Spare_Mapping where InvoiceId=" + InvoiceId + ") as TotalDiscountSpare,(select Sum(isnull(TotalDiscount,0)) from Invoice_Service_Mapping where InvoiceId=" + InvoiceId + ") as TotalDiscountService ";
            //    DataTable dt = dbCon.GetDataTable(strq);
            //    if (dt != null && dt.Rows.Count > 0)
            //    {
            //        decimal dc_SpareAmount = decimal.Parse(dt.Rows[0]["TotalSpare"].ToString());
            //        decimal dc_ServiceAmount = decimal.Parse(dt.Rows[0]["TotalService"].ToString());
            //        decimal dc_SpareDiscountAmount = decimal.Parse(dt.Rows[0]["TotalDiscountSpare"].ToString());
            //        decimal dc_ServiceDiscountAmount = decimal.Parse(dt.Rows[0]["TotalDiscountService"].ToString());

            //        strq = "UPDATE [dbo].[Invoice] SET [TotalSpareAmount] = " + dc_SpareAmount + " ,[TotalServiceAmount] = " + dc_ServiceAmount + " , [FinalTotal] =((isnull([FinalTotal],0)+isnull([TotalDiscount],0))-" + (dc_SpareDiscountAmount + dc_ServiceDiscountAmount) + "),[TotalDiscount] = " + (dc_SpareDiscountAmount + dc_ServiceDiscountAmount) + "   where id=" + InvoiceId;
            //        dbCon.ExecuteQuery(strq);
            //    }
            //}
            //catch (Exception E)
            //{

            //}
            //dbCon.InsertLogs(" Discount 4", "", "");
            objectToSerilize.Name = "Success";
        }
        catch (Exception e)
        {
            objectToSerilize.Name = "Fail";
            dbCon.InsertLogs(ErrorMsgPrefix + " UpdateDiscount Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }




    #region Classes

    public class JoCardAmount
    {
        public string CustomerName { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string VehicleName { get; set; }
        public string JobCardId { get; set; }

        public List<Payment> payment { get; set; }

        public JoCardAmount()
        {
            payment = new List<Payment>();
        }

        public class Payment
        {
            public string Amount { get; set; }
            public string ChequeNumber { get; set; }
            public string ChequeDate { get; set; }
            public string BankName { get; set; }
            public string TrnId { get; set; }
            public string PaymentType { get; set; }
        }

    }
    public class Service
    {
        public string name { get; set; }
        public string amount { get; set; }
        public string catid { get; set; }
        public string taxid { get; set; }
        public string hsn { get; set; }

    }
    public class JobCardRequsition
    {
        public string jobcardnumber { get; set; }
        public string vehiclename { get; set; }
        public string vehiclenumber { get; set; }
        public List<itms> requsition { get; set; }
        public string doc { get; set; }

        public JobCardRequsition()
        {
            requsition = new List<itms>();
        }

        public class itms
        {
            public string id { get; set; }
            public string colortype { get; set; }
        }
    }

    public class Requsition
    {
        public string jobcardnumber { get; set; }
        public string requsitionnumber { get; set; }
        public string vehicle { get; set; }
        public Requsition()
        {
            requsition = new List<itms>();
            parts = new List<Parts>();
        }
        public List<itms> requsition { get; set; }
        public List<Parts> parts { get; set; }
        public class Parts
        {
            public string id { get; set; }
            public string Name { get; set; }
        }
        public class itms
        {
            public string id { get; set; }
            public string Name { get; set; }
            public string Trn { get; set; }
            public string qty { get; set; }
            public string rqty { get; set; }
            public string isallocate { get; set; }
            public string price { get; set; }
            public string pp { get; set; }
            public string isdeleted { get; set; }
            public string partid { get; set; }
            public itms()
            {
                itm = new List<availablebrand>();
            }
            public List<availablebrand> itm { get; set; }
            public class availablebrand
            {
                public string id { get; set; }
                public string Name { get; set; }
                public string Qty { get; set; }
            }
        }
    }
    public class JobCardSpare1
    {
        public string resultflag { get; set; }
        public string Message { get; set; }
        public List<SparewithPrice1> Spares { get; set; }

        public JobCardSpare1()
        {
            resultflag = "";
            Message = "";
            Spares = new List<SparewithPrice1>();
        }
    }
    public class SparewithPrice1
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String Brand { get; set; }
        public String Quantity { get; set; }
        public String PurchasePrice { get; set; }
        public String NType { get; set; }
        public SparewithPrice1()
        {
            Id = "";
            Name = "";
            Quantity = "";
            NType = "";
            PurchasePrice = "";

        }
    }
    public class ServicewithPrice1
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String Price { get; set; }
        public ServicewithPrice1()
        {
            Id = "";
            Name = "";
            Price = "";
        }
    }
    public class InsertUpdateRecord
    {
        public string resultflag { get; set; }
        public string Message { get; set; }

        public InsertUpdateRecord()
        {
            resultflag = "";
            Message = "";
        }
    }

    public class SpareNConsumable
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String PurchasePrice { get; set; }
        public String type { get; set; }
        public SpareNConsumable()
        {
            Id = "";
            PurchasePrice = "";
            type = "";
            Name = "";
        }
    }
    public class Objs
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Objs1> Items { get; set; }
        public Objs()
        {
            Items = new List<Objs1>();
        }
    }
    public class Objs1
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<Objs2> Items { get; set; }
        public Objs1()
        {
            Items = new List<Objs2>();
        }
    }
    public class Objs2
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Id1 { get; set; }
        public string Id2 { get; set; }

    }
    public class Objs4
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Selected { get; set; }
    }
    public class Objs3
    {
        public string y { get; set; }
        public string label { get; set; }
    }

    public class Data
    {
        public string value { get; set; }
    }
    public class Result
    {
        public string pass { get; set; }
    }

    public class PartDetail
    {
        public string name { get; set; }
        public string code { get; set; }
        public string taxid { get; set; }
        public string catid { get; set; }
        public string modelids { get; set; }
        public string variantids { get; set; }
        public string hsn { get; set; }
        public string isgeneric { get; set; }

    }
    public class vendorInvoice
    {
        public string comment { get; set; }
        public string number { get; set; }
        public string amount { get; set; }
        public string date { get; set; }
        public string vendor { get; set; }
        public string vendorid { get; set; }

    }





    #endregion

    #region Hetul
    [WebMethod]
    public void GelAllModelAndVariantList(string spareID)
    {
        var objectToSerilize = new List<SpareNConsumable>();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };

        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            DataTable dt = new DataTable();
            String query = "(select Name, convert(varchar,id) as id,iif(1=1,'Model','')as type,id as order_ids from Vehicle_Model where Isdelete=0) union(select (Vehicle_Variant.Name+' [ '+Vehicle_Model.Name+' ]') as Name,convert(varchar,Vehicle_Variant.id)+'-1' as id,iif(1=1,'Vehicle','')as type,Vehicle_Model_id as order_ids from Vehicle_Variant inner join Vehicle_Model on Vehicle_Model_id=Vehicle_Model.id  where Vehicle_Variant.IsDelete=0 and Vehicle_Model.IsDelete=0)order by order_ids,type;";
            dt = dbCon.GetDataTable(query);
            int spare_ID = 0;
            int.TryParse(spareID, out spare_ID);
            DataTable dtbind = new DataTable();
            if (spare_ID > 0)
            {
                query = "(select convert(varchar,Vehicle_Model.id) as id from Vehicle_Model inner join SpareModelMapping on Vehicle_Model.Id=SpareModelMapping.VehicleModelId where Vehicle_Model.Isdelete=0 and SpareModelMapping.IsDelete=0 and SpareModelMapping.SpareId=" + spare_ID + ") union(select convert(varchar,Vehicle_Variant.id)+'-1' as id from Vehicle_Variant inner join SpareVariantMapping on Vehicle_Variant.id=SpareVariantMapping.VehicleVariantId  where Vehicle_Variant.IsDelete=0 and SpareVariantMapping.IsDelete=0 and SpareVariantMapping.SpareId=" + spare_ID + ");";
                dtbind = dbCon.GetDataTable(query);
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var model = new SpareNConsumable();
                    string id = dr["Id"].ToString();
                    model.Id = id;
                    model.Name = dr["Name"].ToString();
                    model.PurchasePrice = "0";
                    if (dtbind != null && dtbind.Rows.Count > 0)
                    {
                        DataRow[] drr = dtbind.Select("Id = '" + dr["Id"].ToString() + "' ");
                        if (drr.Length > 0)
                        {
                            model.PurchasePrice = "1";
                        }
                    }
                    //model.type = dr["type"].ToString();
                    objectToSerilize.Add(model);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GetAllForPurchases Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }


    //Hetul  NEW 

    [WebMethod]
    public void GETJobcardDetails()
    {
        var objectToSerilize = new List<SpareNConsumable>();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };

        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            DataTable dt = new DataTable();
            String query = "select id,vehicle_id,customer_id,jobstatus_id from jobcard where JobStatus_Id <>3 ";
            dt = dbCon.GetDataTable(query);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    var model = new SpareNConsumable();
                    string id = dr["Id"].ToString();
                    model.Id = id;
                    //model.type = dr["type"].ToString();
                    objectToSerilize.Add(model);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " GETJobcardDetails Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    //[ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    //[System.Web.Services.Protocols.SoapHeader("SoapHeader")]
    public void GetPartsForPurchaseJobCard1(string jobcardid)
    {
        var objectToSerilize = new JobCardSpare1();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string query = " SELECT Spare.Code, Spare.Name, Spare.Id, isnull( (Select  Sum(isnull(Requisition_Spare.Quantity,0))- Sum(- isnull(Requisition_Spare.ReceivedQuantity,0)) from  Requisition_Spare where Requisition_Spare.SpareId=Spare.Id),0) - isnull((SELECT SUM(Qty) AS Expr1 FROM            SpareSpareBrandMapping WHERE (SpareId = Spare.Id)),0) AS Quantity FROM Spare  where (isnull( (Select  Sum(isnull(Requisition_Spare.Quantity,0))- Sum(- isnull(Requisition_Spare.ReceivedQuantity,0)) from  Requisition_Spare where Requisition_Spare.SpareId=Spare.Id),0) - isnull((SELECT  SUM(Qty) AS Expr1 FROM SpareSpareBrandMapping WHERE (SpareId = Spare.Id)),0))>0 ";
            string[] param = { "0" };
            DataTable dt = dbCon.GetDataTableWithParams(query, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (!jobcardid.Equals("0") && !jobcardid.Equals(""))
                {
                    jobcardid = jobcardid.TrimEnd(',');
                    query = " select JobCardId,SpareId,Quantity from JobCard_Spare_Mapping where JobCardId in(" + jobcardid + ")";
                    DataTable dttemp = dbCon.GetDataTable(query);
                    DataTable dtdata = dt.Clone();
                    if (dttemp != null && dttemp.Rows.Count > 0)
                    {
                        for (int i = 0; i < dttemp.Rows.Count; i++)
                        {
                            DataRow[] dr = dt.Select("id= " + dttemp.Rows[i]["SpareId"].ToString() + " ");
                            if (dr.Length > 0)
                            {
                                dtdata.Merge(dr.CopyToDataTable());
                            }
                        }
                    }
                    dt = dtdata.Copy();
                }
                foreach (DataRow dr in dt.Rows)
                {
                    var Spares = new SparewithPrice1();
                    //Spares.Brand = dr["brandname"].ToString();
                    Spares.Id = dr["Id"].ToString();
                    Spares.Name = dr["Code"].ToString();
                    //decimal Quantity = Convert.ToDecimal(dr["Quantity"]);
                    Spares.Quantity = dr["Quantity"].ToString();
                    //Spares.PurchasePrice = dr["PurchasePrice"].ToString();
                    Spares.NType = "spare";
                    objectToSerilize.Spares.Add(Spares);
                }
                objectToSerilize.resultflag = "1";
                objectToSerilize.Message = "Success";
            }
        }
        catch (Exception ex)
        {
            objectToSerilize.resultflag = "0";
            objectToSerilize.Message = "Error";
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }


    [System.Web.Services.WebMethod]

    public void fillTabledatas(int JobCardId = 0, string Spare = "", string Service = "", bool fromDiscount = false)
    {
        var objectToSerilize = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";

        ServiceMethods ServiceMethod = new ServiceMethods();
        var Spares = ServiceMethod.GetJobCardSpareInJobCardForInvoiceV2_new(JobCardId.ToString(), Spare, Service);


        string query = "<table width='100%' border='0' style='border-collapse: collapse;font-size:7.5pt;border-bottom:1px solid #ccc;' cellpadding='0' cellspacing='0'><tbody><tr style='font-weight:bold;'><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding: 5px;'>S.No</td><td width='20%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px' lang='english'>PARTICULARS OFServices</td><td width='10%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>HSN/SAC</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>QTY</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px' lang='english'>UNIT PRICE</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px' lang='english'>Discount Per Unit</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px' lang='english'>TAXABLE AMT</td><td width='27%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0'><table border='0' width='100%' style='font-size:7.5pt;font-weight:bold;' cellpadding='0' cellspacing='0'><tbody><tr><td colspan='2' style='border-bottom:solid thin #ccc;text-align:center;    border-right: 1px solid #ccc;padding:2px'>CGST</td><td colspan='2' style='border-bottom:solid thin #ccc;text-align:center;padding:2px' lang='english'>SGST/UGST</td></tr><tr><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px' lang='english'>Rate (%)</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px' lang='english'>Amount</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>Rate (%)</td><td width='25%' style='text-align:center;padding:2px' lang='english'>Amount</td></tr></tbody></table></td><td width='10%' style='text-align:center;border-top:solid 1px #ccc;padding:2px' lang='english' colspan='4'>Amount</td></tr>";

        int count = 0;
        decimal withoutTaxPrice = 0;
        decimal TOTAlCGSTTax = 0;
        decimal TOTAlSGSTTax = 0;
        decimal TotalDiscount = 0;
        foreach (var temp in Spares.Spares)
        {
            string Temp = "";
            if (temp.Type == "Header")
            {
                if (temp.Name.ToLower().ToString() == "SubTotalForSpare".ToLower().ToString())
                {

                    Temp += "<tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;' lang='english'>Total Before Tax (+)</td><td width='11%' colspan='3' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>   " + temp.TaxableAmount.ToString() + " </td></tr> <tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;' lang='english'><b>Total Discount For Spare(-)</b></td><td width='11%' colspan='3' style='text-align:right;padding:2px;'><b>" + temp.TotalDiscountForSpare.ToString() + "</b></td></tr><tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>CGST (+)</td><td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalCGSTAmount.ToString() + "</td></tr><tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>SGST/UGST (+)</td> <td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalSGSTAmount.ToString() + "</td></tr><tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px; border-right:solid 1px #ccc; border-bottom:solid 1px #ccc; ' lang='english'>Sub Total For Spares</td><td width='11%'  colspan='3' style=text-align:right;padding:2px 10px; border-bottom:solid 1px #ccc; font-weight:bold'>" + temp.SubTotal.ToString() + "</td></tr>";
                    query += Temp;
                }
                else
                {
                    if (fromDiscount)
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td><input id='" + (temp.Name.Contains("Spare") ? "Spare" : "Service") + "-All-Discount' onclick='this.setSelectionRange(0, this.value.length)'  onkeypress='return isNumberKey(event)' onchange='UpdateDiscount(" + (temp.Name.Contains("Spare") ? "1" : "2") + ")' type='text' style='width:80px;margin-left:5px;  border-top: 1px solid #ccc;' onkeypress='return isNumberKey(event)' value=''/></td><td colspan='3' style='text-align:left; padding:2px 10px; border-top:solid 1px #ccc; font-weight:bold'>(i.e. 1000 or i.e 5%)</td></tr>";
                    }
                    else
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td style='border-top:  1px solid #ccc;'></td><td colspan='3' style='text-align:left; padding:2px 10px; border-top:solid 1px #ccc; font-weight:bold'></td></tr>";
                    }
                    query += Temp;
                }
            }
            else
            {

                decimal price = 0;
                try { price = Convert.ToDecimal(temp.UnitPrice); }
                catch (Exception ex)
                {
                }
                Decimal WithoutTaxPrice1 = 0;
                try
                {
                    WithoutTaxPrice1 = Convert.ToDecimal(temp.TaxableAmount);
                }
                catch (Exception ex)
                {
                }
                try
                {
                    TotalDiscount = Convert.ToDecimal(temp.TotalDiscount);
                }
                catch (Exception ex)
                { }
                //Temp += " <tr><td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + (++count) + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'><span>" + temp.Name + "</span></td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.HSNNumber + "</td> <td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + temp.Quantity + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + price + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + (fromDiscount ? "<input type='text' style='width:80px;' onchange='UpdateDiscount(3)' class='cls-" + temp.ItemType + "' data-dis='" + temp.Discount + "' id='" + temp.ItemType + "-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' value='" + temp.Discount + "'/>" : temp.Discount) + "</td><td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.TaxableAmount + "</td><td style='text-align:left;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0;'> <table border='0' width='100%' style='font-size:7.5pt' cellpadding='0' cellspacing='0'><tbody><tr><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + temp.CGSTRate + "</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + temp.CGSTAmount + "</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + temp.SGSTRate + "</td><td width='25%' style='text-align:center;padding:2px'>" + temp.SGSTAmount + "</td></tr></tbody></table></td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + temp.FinalAmount + "</td></tr>";

                Temp += " <tr><td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + (++count) + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'><span>" + temp.Name + "</span></td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.HSNNumber + "</td> <td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + temp.Quantity + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + price + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>"
          + (fromDiscount ? "<input type='text' onclick='this.setSelectionRange(0, this.value.length)' style='width:80px;' onchange='UpdateDiscount(3)' class='cls-" + temp.ItemType + "' id='" + temp.ItemType + "-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "'  data-dis='" + temp.Discount + "' value='" + temp.Discount + "'/>" : temp.Discount) + "</td><td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.TaxableAmount + "</td><td style='text-align:left;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0;'> <table border='0' width='100%' style='font-size:7.5pt' cellpadding='0' cellspacing='0'><tbody><tr><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + temp.CGSTRate + "</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-cgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.CGSTAmount + "</span>" + "</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + temp.SGSTRate + "</td><td width='25%' style='text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-sgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.SGSTAmount + "</span>" + "</td></tr></tbody></table></td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + "<Span  id='" + temp.ItemType + "-fa-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.FinalAmount + "</span>" + "</td></tr>";

                if (temp.Type == "Header")
                {

                }
                query += Temp;
                withoutTaxPrice += WithoutTaxPrice1;


                //if (!String.IsNullOrEmpty(temp.CGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlCGSTTax += Convert.ToDecimal(temp.CGSTAmount);
                //        TOTAlCGSTTax = Convert.ToDecimal(temp.TotalCGSTAmount);
                //        TOTAlCGSTTax = Math.Round(TOTAlCGSTTax, 0);


                //    }
                //    catch (Exception ex) { }
                //}
                //if (!String.IsNullOrEmpty(temp.SGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlSGSTTax += Convert.ToDecimal(temp.SGSTAmount);

                //        TOTAlSGSTTax = Convert.ToDecimal(temp.TotalSGSTAmount);
                //        TOTAlSGSTTax = Math.Round(TOTAlSGSTTax, 0);

                //    }
                //    catch (Exception ex) { }
                //}
            }
        }
        foreach (var temp in Spares.Services)
        {
            string Temp = "";
            if (temp.Type == "Header")
            {
                if (temp.Name.ToLower().ToString() == "SubTotalForService".ToLower().ToString())
                {
                    //            //  " + ((Convert.ToDecimal(Spares[i].SubTotal)) - (Convert.ToDecimal(Spares[i].TotalSGSTAmount) * 2)).ToString() + "
                    Temp += "<tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;' lang='english'>Total Before Tax (+)</td><td width='11%' colspan='3'  style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>   " + temp.TaxableAmount.ToString() + " </td></tr> <tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;' lang='english'><b>Total Discount For Service(-)</b></td><td width='11%' colspan='3' style='text-align:right;padding:2px; '><b>" + temp.TotalDiscountForService.ToString() + "</b></td></tr><tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>CGST (+)</td><td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalCGSTAmount.ToString() + "</td></tr><tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>SGST/UGST (+)</td> <td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalSGSTAmount.ToString() + "</td></tr><tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;' lang='english'>Sub Total For Service</td><td width='11%' colspan='3' style='text-align:right;padding:2px; border-bottom:solid 1px #ccc;'>" + temp.SubTotal.ToString() + "</td></tr>";
                    query += Temp;
                }
                else
                {
                    if (fromDiscount)
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td><input onclick='this.setSelectionRange(0, this.value.length)' id='" + (temp.Name.Contains("Spare") ? "Spare" : "Service") + "-All-Discount' onkeypress='return isNumberKey(event)' onchange='UpdateDiscount(" + (temp.Name.Contains("Spare") ? "1" : "2") + ")' type='text' style='width:80px;margin-left:5px;' onkeypress='return isNumberKey(event)' value=''/></td><td colspan='3'>(i.e. 1000 or i.e 5%)</td></tr>";
                    }
                    else
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td style='border-top:  1px solid #ccc;'></td><td colspan='3'></td></tr>";
                    }
                    query += Temp;
                }
            }
            else
            {

                decimal price = 0;
                try { price = Convert.ToDecimal(temp.UnitPrice); }
                catch (Exception ex)
                {
                }
                Decimal WithoutTaxPrice1 = 0;
                try
                {
                    WithoutTaxPrice1 = Convert.ToDecimal(temp.TaxableAmount);
                }
                catch (Exception ex)
                {
                }
                try
                {
                    TotalDiscount = Convert.ToDecimal(temp.TotalDiscount);
                }
                catch (Exception ex)
                { }


                Temp += " <tr><td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + (++count) + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'><span>" + temp.Name + "</span></td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.HSNNumber + "</td> <td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + temp.Quantity + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + price + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>"
                    + (fromDiscount ? "<input type='text' style='width:80px;' onclick='this.setSelectionRange(0, this.value.length)' onchange='UpdateDiscount(3)' class='cls-" + temp.ItemType + "' id='" + temp.ItemType + "-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "'  data-dis='" + temp.Discount + "' value='" + temp.Discount + "'/>" : temp.Discount) + "</td><td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.TaxableAmount + "</td><td style='text-align:left;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0;'> <table border='0' width='100%' style='font-size:7.5pt' cellpadding='0' cellspacing='0'><tbody><tr><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + temp.CGSTRate + "</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-cgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.CGSTAmount + "</span>" + "</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + temp.SGSTRate + "</td><td width='25%' style='text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-sgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.SGSTAmount + "</span>" + "</td></tr></tbody></table></td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + "<Span  id='" + temp.ItemType + "-fa-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.FinalAmount + "</span>" + "</td></tr>";

                if (temp.Type == "Header")
                {

                }
                query += Temp;
                withoutTaxPrice += WithoutTaxPrice1;
                //if (!String.IsNullOrEmpty(temp.CGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlCGSTTax += Convert.ToDecimal(temp.CGSTAmount);
                //        TOTAlCGSTTax = Convert.ToDecimal(temp.TotalCGSTAmount);
                //        TOTAlCGSTTax = Math.Round(TOTAlCGSTTax, 0);
                //    }
                //    catch (Exception ex) { }
                //}
                //if (!String.IsNullOrEmpty(temp.SGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlSGSTTax += Convert.ToDecimal(temp.SGSTAmount);
                //        TOTAlSGSTTax = Convert.ToDecimal(temp.TotalSGSTAmount);
                //        TOTAlSGSTTax = Math.Round(TOTAlSGSTTax, 0);
                //    }
                //    catch (Exception ex) { }
                //}
            }
            //changes 
        }
        Decimal SubTotal = Convert.ToDecimal(Spares.TotalAmount);
        SubTotal = Math.Round(SubTotal, 2);
        Decimal FinalTotal = SubTotal - TotalDiscount;
        FinalTotal = Math.Round(FinalTotal, 2);
        query += "<tr style='font-weight:bold;'><td colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Total Amount</td><td colspan='3' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>" + Spares.TotalAmount + "</td></tr><tr style='font-weight:bold;'><td colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Total Discount</td><td colspan='3' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'> " + Spares.TotalDiscount + "</td></tr><tr style='font-weight:bold;'><td colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Final Amount</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + Spares.FinalAmount + "</td></tr></tbody></table>";
        int finaltotal = Convert.ToInt32(FinalTotal);
        try
        {
            int jobcardinvoiceAmount = ServiceMethod.updateAmountJobCardInvoiceDetail(JobCardId, finaltotal);
        }
        catch (Exception ex)
        {
        }
        string customernotes = ServiceMethod.CustomerNotesForInvoice(JobCardId.ToString(), "2");
        if (customernotes.Contains(","))
        {
            customernotes = customernotes.Replace(",", ", ");
        }
        string substring = ServiceMethod.NumbersToWords(finaltotal);
        objectToSerilize.Name = query + "<input type='hidden' id='rpnword' value='" + substring + "'>" + "<input type='hidden' id='customernotes' value='" + customernotes + "'>";
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [System.Web.Services.WebMethod]

    public void fillTabledatas_V1(int JobCardId = 0, string Spare = "", string Service = "", bool fromDiscount = false, int type = 1, int fromInvoice = 0, bool fromEstimate = false, bool ShowAllocated = false)
    {
        var objectToSerilize = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";

        ServiceMethods ServiceMethod = new ServiceMethods();
        ServiceClass.JobCardSpareForInvoice Spares;
        if (fromInvoice == 0)
        {
            Spares = ServiceMethod.GetJobCardSpareInJobCardForInvoiceV3_new(JobCardId.ToString(), Spare, Service, type, fromEstimate, ShowAllocated);
        }
        else
        {
            Spares = ServiceMethod.GetJobCardSpareInJobCardForInvoiceV3(JobCardId.ToString(), type);
        }
        string GstNumber = "";
        bool isIgst = false;
        try
        {
            GstNumber = dbCon.GetDataTable("Select isnull((Select Gst_No from Customer where id=Customer_Id),'') from jobcard where id=" + JobCardId).Rows[0][0].ToString();
            if (!String.IsNullOrWhiteSpace(GstNumber))
            {
                string strFirstTwoDigit = GstNumber.Substring(0, 2);
                if (strFirstTwoDigit != "24")
                {
                    isIgst = true;
                }
            }
        }
        catch (Exception E) { }
        string query = "<table width='100%' border='0' style='border-collapse: collapse;font-size:7.5pt;border-bottom:1px solid #ccc;' cellpadding='0' cellspacing='0'><tbody><tr style='font-weight:bold;'><td width='3%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding: 5px;'>S.No</td><td width='20%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px' lang='english'>PARTICULARS OFServices</td><td width='10%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>HSN/SAC</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>QTY</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px' lang='english'>UNIT PRICE</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px' lang='english'>" + (type == 1 ? "Discount" : "Depreciation") + " Per Unit</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px;border-bottom:solid 1px #ccc;' lang='english'>TAXABLE AMT</td><td width='30%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0'><table border='0' width='100%' style='font-size:7.5pt;font-weight:bold;' cellpadding='0' cellspacing='0'><tbody><tr><td colspan='2' style='border-bottom:solid thin #ccc;text-align:center;    border-right: 1px solid #ccc;padding:2px'>CGST</td><td colspan='2' style='border-bottom:solid thin #ccc;text-align:center;padding:2px' lang='english'>SGST/UGST</td><td colspan='2' style='border-bottom:solid thin #ccc;text-align:center;padding:2px' lang='english'>IGST</td></tr><tr><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px' lang='english'>Rate (%)</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px' lang='english'>Amount</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>Rate (%)</td><td width='16%' style='text-align:center;padding:2px' lang='english'>Amount</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>Rate (%)</td><td width='16%' style='text-align:center;padding:2px' lang='english'>Amount</td></tr></tbody></table></td><td width='10%' style='text-align:center;border-top:solid 1px #ccc;padding:2px' lang='english' colspan='4'>Amount</td></tr>";

        int count = 0;
        decimal withoutTaxPrice = 0;
        decimal TOTAlCGSTTax = 0;
        decimal TOTAlSGSTTax = 0;
        decimal TotalDiscount = 0;
        decimal CGSTTax_14 = 0;
        decimal CGSTTaxable_14 = 0;
        decimal CGSTTax_9 = 0;
        decimal CGSTTaxable_9 = 0;
        foreach (var temp in Spares.Spares)
        {
            string Temp = "";
            if (temp.Type == "Header")
            {
                if (temp.Name.ToLower().ToString() == "SubTotalForSpare".ToLower().ToString())
                {

                    Temp += "<tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;' lang='english'>Total Before Tax (+)</td><td width='11%' colspan='3' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>   " + temp.TaxableAmount.ToString() + " </td></tr> <tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;' lang='english'><b>Total " + (type == 1 ? "Discount" : "Depreciation") + " For Spare(-)</b></td><td width='11%' colspan='3' style='text-align:right;padding:2px;'><b>" + temp.TotalDiscountForSpare.ToString() + "</b></td></tr><tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>CGST (+)</td><td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalCGSTAmount.ToString() + "</td></tr><tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>SGST/UGST (+)</td> <td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalSGSTAmount.ToString() + "</td></tr><tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px; border-right:solid 1px #ccc; border-bottom:solid 1px #ccc; ' lang='english'>Sub Total For Spares</td><td width='11%'  colspan='3' style=text-align:right;padding:2px 10px; border-bottom:solid 1px #ccc; font-weight:bold'>" + temp.SubTotal.ToString() + "</td></tr>";
                    query += Temp;
                }
                else
                {
                    if (fromDiscount)
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td><input id='" + (temp.Name.Contains("Spare") ? "Spare" : "Service") + "-All-Discount' onclick='this.setSelectionRange(0, this.value.length)'  onkeypress='return isNumberKey(event)' onchange='UpdateDiscount(" + (temp.Name.Contains("Spare") ? "1" : "2") + ")' type='text' style='width:80px;margin-left:5px;  border-top: 1px solid #ccc;' onkeypress='return isNumberKey(event)' value=''/></td><td colspan='3' style='text-align:left; padding:2px 10px; border-top:solid 1px #ccc; font-weight:bold'>(i.e. 1000 or i.e 5%)</td></tr>";
                    }
                    else
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td style='border-top:  1px solid #ccc;'></td><td colspan='3' style='text-align:left; padding:2px 10px; border-top:solid 1px #ccc; font-weight:bold'></td></tr>";
                    }
                    query += Temp;
                }
            }
            else
            {

                decimal price = 0;
                try
                {
                    if (temp.SGSTRate == "9.00")
                    {
                        CGSTTax_9 += Convert.ToDecimal(temp.SGSTAmount); CGSTTaxable_9 += Convert.ToDecimal(temp.TaxableAmount);
                    }
                    else
                    {
                        CGSTTax_14 += Convert.ToDecimal(temp.SGSTAmount); CGSTTaxable_14 += Convert.ToDecimal(temp.TaxableAmount);
                    }
                }
                catch (Exception E) { }
                try { price = Convert.ToDecimal(temp.UnitPrice); }
                catch (Exception ex)
                {
                }
                Decimal WithoutTaxPrice1 = 0;
                try
                {
                    WithoutTaxPrice1 = Convert.ToDecimal(temp.TaxableAmount);
                }
                catch (Exception ex)
                {
                }
                try
                {
                    TotalDiscount = Convert.ToDecimal(temp.TotalDiscount);
                }
                catch (Exception ex)
                { }
                //Temp += " <tr><td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + (++count) + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'><span>" + temp.Name + "</span></td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.HSNNumber + "</td> <td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + temp.Quantity + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + price + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + (fromDiscount ? "<input type='text' style='width:80px;' onchange='UpdateDiscount(3)' class='cls-" + temp.ItemType + "' data-dis='" + temp.Discount + "' id='" + temp.ItemType + "-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' value='" + temp.Discount + "'/>" : temp.Discount) + "</td><td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.TaxableAmount + "</td><td style='text-align:left;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0;'> <table border='0' width='100%' style='font-size:7.5pt' cellpadding='0' cellspacing='0'><tbody><tr><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + temp.CGSTRate + "</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + temp.CGSTAmount + "</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + temp.SGSTRate + "</td><td width='25%' style='text-align:center;padding:2px'>" + temp.SGSTAmount + "</td></tr></tbody></table></td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + temp.FinalAmount + "</td></tr>";

                Temp += " <tr><td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + (++count) + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'><span>" + temp.Name + "</span></td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.HSNNumber + "</td> <td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + temp.Quantity + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + price + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>"
                    + (fromDiscount ? "<input type='text' onclick='this.setSelectionRange(0, this.value.length)' style='width:80px;' onchange='UpdateDiscount(3)' class='cls-" + temp.ItemType + "' id='" + temp.ItemType + "-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "'  data-dis='" + temp.Discount + "' value='" + temp.Discount + "'/>" : temp.Discount) + "</td><td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.TaxableAmount + "</td><td style='text-align:left;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0;'> <table border='0' width='100%' style='font-size:7.5pt' cellpadding='0' cellspacing='0'><tbody><tr><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + (isIgst ? "0" : temp.CGSTRate) + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-cgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? "0" : temp.CGSTAmount) + "</span>" + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + (isIgst ? "0" : temp.SGSTRate) + "</td><td width='16%' style='text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-sgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? "0" : temp.SGSTAmount) + "</span>" + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + (isIgst ? (Convert.ToDecimal(temp.SGSTRate) * 2).ToString() : "0") + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-igst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? (Convert.ToDecimal(temp.CGSTAmount) * 2).ToString() : "0") + "</span>" + "</td></tr></tbody></table></td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + "<Span  id='" + temp.ItemType + "-fa-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.FinalAmount + "</span>" + "</td></tr>";

                if (temp.Type == "Header")
                {

                }
                query += Temp;
                withoutTaxPrice += WithoutTaxPrice1;


                //if (!String.IsNullOrEmpty(temp.CGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlCGSTTax += Convert.ToDecimal(temp.CGSTAmount);
                //        TOTAlCGSTTax = Convert.ToDecimal(temp.TotalCGSTAmount);
                //        TOTAlCGSTTax = Math.Round(TOTAlCGSTTax, 0);


                //    }
                //    catch (Exception ex) { }
                //}
                //if (!String.IsNullOrEmpty(temp.SGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlSGSTTax += Convert.ToDecimal(temp.SGSTAmount);

                //        TOTAlSGSTTax = Convert.ToDecimal(temp.TotalSGSTAmount);
                //        TOTAlSGSTTax = Math.Round(TOTAlSGSTTax, 0);

                //    }
                //    catch (Exception ex) { }
                //}
            }
        }
        foreach (var temp in Spares.Services)
        {
            string Temp = "";
            if (temp.Type == "Header")
            {
                if (temp.Name.ToLower().ToString() == "SubTotalForService".ToLower().ToString())
                {
                    //            //  " + ((Convert.ToDecimal(Spares[i].SubTotal)) - (Convert.ToDecimal(Spares[i].TotalSGSTAmount) * 2)).ToString() + "
                    Temp += "<tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;' lang='english'>Total Before Tax (+)</td><td width='11%' colspan='3'  style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>   " + temp.TaxableAmount.ToString() + " </td></tr> <tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;' lang='english'><b>Total " + (type == 1 ? "Discount" : "Depreciation") + " For Service(-)</b></td><td width='11%' colspan='3' style='text-align:right;padding:2px; '><b>" + temp.TotalDiscountForService.ToString() + "</b></td></tr><tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>CGST (+)</td><td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalCGSTAmount.ToString() + "</td></tr><tr><td width='90%' colspan='10' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>SGST/UGST (+)</td> <td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalSGSTAmount.ToString() + "</td></tr><tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;' lang='english'>Sub Total For Service</td><td width='11%' colspan='3' style='text-align:right;padding:2px; border-bottom:solid 1px #ccc;'>" + temp.SubTotal.ToString() + "</td></tr>";
                    query += Temp;
                }
                else
                {
                    if (fromDiscount)
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td><input onclick='this.setSelectionRange(0, this.value.length)' id='" + (temp.Name.Contains("Spare") ? "Spare" : "Service") + "-All-Discount' onkeypress='return isNumberKey(event)' onchange='UpdateDiscount(" + (temp.Name.Contains("Spare") ? "1" : "2") + ")' type='text' style='width:80px;margin-left:5px;' onkeypress='return isNumberKey(event)' value=''/></td><td colspan='3'>(i.e. 1000 or i.e 5%)</td></tr>";
                    }
                    else
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td style='border-top:  1px solid #ccc;'></td><td colspan='3'></td></tr>";
                    }
                    query += Temp;
                }
            }
            else
            {

                decimal price = 0;

                try
                {
                    if (temp.SGSTRate == "9.00")
                    {
                        CGSTTax_9 += Convert.ToDecimal(temp.SGSTAmount); CGSTTaxable_9 += Convert.ToDecimal(temp.TaxableAmount);
                    }
                    else
                    {
                        CGSTTax_14 += Convert.ToDecimal(temp.SGSTAmount); CGSTTaxable_14 += Convert.ToDecimal(temp.TaxableAmount);
                    }
                }
                catch (Exception E) { }
                try { price = Convert.ToDecimal(temp.UnitPrice); }
                catch (Exception ex)
                {
                }
                Decimal WithoutTaxPrice1 = 0;
                try
                {
                    WithoutTaxPrice1 = Convert.ToDecimal(temp.TaxableAmount);
                }
                catch (Exception ex)
                {
                }
                try
                {
                    TotalDiscount = Convert.ToDecimal(temp.TotalDiscount);
                }
                catch (Exception ex)
                { }


                Temp += " <tr><td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + (++count) + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'><span>" + temp.Name + "</span></td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.HSNNumber + "</td> <td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + temp.Quantity + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + price + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>"
                    + (fromDiscount ? "<input type='text' style='width:80px;' onclick='this.setSelectionRange(0, this.value.length)' onchange='UpdateDiscount(3)' class='cls-" + temp.ItemType + "' id='" + temp.ItemType + "-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "'  data-dis='" + temp.Discount + "' value='" + temp.Discount + "'/>" : temp.Discount) + "</td><td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.TaxableAmount + "</td><td style='text-align:left;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0;'> <table border='0' width='100%' style='font-size:7.5pt' cellpadding='0' cellspacing='0'><tbody><tr><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + (isIgst ? "0" : temp.CGSTRate) + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-cgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? "0" : temp.CGSTAmount) + "</span>" + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + (isIgst ? "0" : temp.SGSTRate) + "</td><td width='16%' style='text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-sgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? "0" : temp.SGSTAmount) + "</span>" + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + (isIgst ? (Convert.ToDecimal(temp.SGSTRate) * 2).ToString() : "0") + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-igst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? (Convert.ToDecimal(temp.CGSTAmount) * 2).ToString() : "0") + "</span>" + "</td></tr></tbody></table></td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + "<Span  id='" + temp.ItemType + "-fa-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.FinalAmount + "</span>" + "</td></tr>";

                if (temp.Type == "Header")
                {

                }
                query += Temp;
                withoutTaxPrice += WithoutTaxPrice1;
                //if (!String.IsNullOrEmpty(temp.CGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlCGSTTax += Convert.ToDecimal(temp.CGSTAmount);
                //        TOTAlCGSTTax = Convert.ToDecimal(temp.TotalCGSTAmount);
                //        TOTAlCGSTTax = Math.Round(TOTAlCGSTTax, 0);
                //    }
                //    catch (Exception ex) { }
                //}
                //if (!String.IsNullOrEmpty(temp.SGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlSGSTTax += Convert.ToDecimal(temp.SGSTAmount);
                //        TOTAlSGSTTax = Convert.ToDecimal(temp.TotalSGSTAmount);
                //        TOTAlSGSTTax = Math.Round(TOTAlSGSTTax, 0);
                //    }
                //    catch (Exception ex) { }
                //}
            }
            //changes 
        }
        Decimal SubTotal = Convert.ToDecimal(Spares.TotalAmount);
        SubTotal = Math.Round(SubTotal, 2);
        Decimal FinalTotal = SubTotal - TotalDiscount;
        FinalTotal = Math.Round(FinalTotal, 2);
        string strfiltextra = "";
        try
        {
            if (!fromEstimate && type == 1)
            {
                DataTable dtextra = dbCon.GetDataTable("SELECT isnull([CompusaryExcess],0) as [CompusaryExcess] ,isnull([VoluntaryExcess],0) as [VoluntaryExcess] ,isnull([Salvage],0) as [Salvage] FROM [dbo].[JobCard_Insurance_Mapping] where JobCardId=" + JobCardId);
                if (dtextra != null && dtextra.Rows.Count > 0)
                {
                    if (Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Compusary Excess (+)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["CompusaryExcess"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString())).ToString();
                    }
                    if (Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Voluntary Excess (+)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["VoluntaryExcess"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString())).ToString();
                    }
                    if (Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Salvage (+)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["Salvage"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString())).ToString();
                    }
                }
            }

            if (!fromEstimate && type == 2)
            {
                DataTable dtextra = dbCon.GetDataTable("SELECT isnull([CompusaryExcess],0) as [CompusaryExcess] ,isnull([VoluntaryExcess],0) as [VoluntaryExcess] ,isnull([Salvage],0) as [Salvage] FROM [dbo].[JobCard_Insurance_Mapping] where JobCardId=" + JobCardId);
                if (dtextra != null && dtextra.Rows.Count > 0)
                {
                    if (Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Less Compulsory Excess (-)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["CompusaryExcess"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString())).ToString();
                    }
                    if (Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Less Voluntary Excess (-)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["VoluntaryExcess"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString())).ToString();
                    }
                    if (Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Less Salvage (-)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["Salvage"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString())).ToString();
                    }
                }
            }
            Spares.FinalAmount = Math.Round(Convert.ToDecimal(Spares.FinalAmount), 2).ToString();
        }
        catch (Exception E) { }
        query += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Total Amount</td><td colspan='3' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>" + (Convert.ToDecimal(Spares.TotalAmount) + Convert.ToDecimal(Spares.TotalDiscount)) + "</td></tr><tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Less " + (type == 1 ? "Discount" : "Depreciation") + " (-)</td><td colspan='3' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'> " + Spares.TotalDiscount + "</td></tr>" + strfiltextra + "<tr style='font-weight:bold;'><td colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Final Amount</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + Spares.FinalAmount + "</td></tr>" +
            //GST 18%
            //"<tr style='font-weight:bold;'><td colspan='11' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" +

            //"</td>"+
            //GST 28%

            "</tbody></table>";
        if (!isIgst)
        {
            query += "<Div Style='float : right;width:50%;'><table style='width:100%;text-align:center;border:1px solid #676464;'><tr><th>Rate</th><th>Taxable</th><th>CGST</th><th>SGST</th><th>IGST</th><th>Total</th></tr>" +
                "<tr><td>GST 18%</td><td>" + CGSTTaxable_9 + "</td><td>" + CGSTTax_9 + "</td><td>" + CGSTTax_9 + "</td><td>-</td><td>" + Math.Round((CGSTTaxable_9 + CGSTTax_9 + CGSTTax_9), 2) + "</td></tr>" +

                "<tr><td>GST 28%</td><td>" + CGSTTaxable_14 + "</td><td>" + CGSTTax_14 + "</td><td>" + CGSTTax_14 + "</td><td>-</td><td>" + Math.Round((CGSTTaxable_14 + CGSTTax_14 + CGSTTax_14), 2) + "</td></tr>" +
                "<tr><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'>Total</td><td style='border-top:1px solid #676464;'><b>" + Math.Round((CGSTTaxable_14 + CGSTTaxable_9 + (CGSTTax_14 * 2) + (CGSTTax_9 * 2)), 2) + "</b></td></tr>" +
                "</table></Div>";
        }
        else
        {
            query += "<Div Style='float : right;width:50%;'><table style='width:100%;text-align:center;border:1px solid #676464;'><tr><th>Rate</th><th>Taxable</th><th>CGST</th><th>SGST</th><th>IGST</th><th>Total</th></tr>" +
                    "<tr><td>GST 18%</td><td>" + CGSTTaxable_9 + "</td><td>" + 0 + "</td><td>" + 0 + "</td><td>" + (CGSTTax_9 * 2) + "</td><td>" + Math.Round((CGSTTaxable_9 + CGSTTax_9 + CGSTTax_9), 2) + "</td></tr>" +

                    "<tr><td>GST 28%</td><td>" + CGSTTaxable_14 + "</td><td>" + 0 + "</td><td>" + 0 + "</td><td>" + (CGSTTax_14 * 2) + "</td><td>" + Math.Round((CGSTTaxable_14 + CGSTTax_14 + CGSTTax_14), 2) + "</td></tr>" +
                    "<tr><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'>Total</td><td style='border-top:1px solid #676464;'><b>" + Math.Round((CGSTTaxable_14 + CGSTTaxable_9 + (CGSTTax_14 * 2) + (CGSTTax_9 * 2)), 2) + "</b></td></tr>" +
                    "</table></Div>";
        }
        int finaltotal = Convert.ToInt32(FinalTotal);
        try
        {
            if (!fromEstimate)
            {
                string strAmount = "";
                if (type == 1)
                {
                    strAmount = " CustomerInvoiceAmount =" + Spares.FinalAmount;
                }
                else
                {
                    strAmount = " InsuranceInvoiceAmount =" + Spares.FinalAmount;
                }
                dbCon.ExecuteQuery("update jobcard set " + strAmount + " where id=" + JobCardId);
            }
        }
        catch (Exception E) { }
        try
        {
            int jobcardinvoiceAmount = ServiceMethod.updateAmountJobCardInvoiceDetail(JobCardId, finaltotal);
        }
        catch (Exception ex)
        {
        }

        string customernotes = ServiceMethod.CustomerNotesForInvoice(JobCardId.ToString(), "2");
        if (customernotes.Contains(","))
        {
            customernotes = customernotes.Replace(",", ", ");
        }
        string substring = "";
        try
        {
            decimal d = Convert.ToDecimal(Spares.FinalAmount.ToString());
            substring = ServiceMethod.NumbersToWords((int)d);
        }
        catch (Exception E) { }
        objectToSerilize.Name = query + "<input type='hidden' id='rpnword' value='" + substring + "'>" + "<input type='hidden' id='customernotes' value='" + customernotes + "'>";
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void generateGstInvoice(string Jobcardid, string type, int GenerateGstInvoiceNumber = 0)
    {
        var objectToSerilize = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        objectToSerilize.Id = "0";
        try
        {
            if (GenerateGstInvoiceNumber > 0)
            {
                clsDataSourse ClsDataSourse = new clsDataSourse();
                string InvNo = ClsDataSourse.GetGstInvoiceNumber();
                string InvDate = "";
                DataTable dtInvoice = dbCon.GetDataTable(" Select GstInvoiceNumber,Convert(varchar(17),Doc,113) as DOC from [dbo].[Invoice] WHERE isnull(IsCancelled,0)=0 and InvoiceNumber is not null and GstInvoiceNumber is not null and JobCardid=" + Jobcardid + " and  type=" + type);
                if (dtInvoice.Rows.Count > 0)
                {
                    InvNo = dtInvoice.Rows[0][0].ToString();
                    InvDate = dtInvoice.Rows[0][1].ToString();
                }
                else
                {
                    InvDate = dbCon.getindiantime().ToString("dd-MMM-yyyy HH:mm:ss");
                    dbCon.ExecuteQuery("UPDATE [dbo].[Invoice] SET [GstInvoiceNumber] ='" + InvNo + "',Doc='" + InvDate + "'  WHERE isnull(IsCancelled,0)=0 and InvoiceNumber is not null and GstInvoiceNumber is null and JobCardid=" + Jobcardid + " and  type=" + type);
                    //InvDate = dtInvoice.Rows[0][1].ToString();
                }
                objectToSerilize.Id = InvNo;
                objectToSerilize.Name = InvDate;
            }
        }
        catch (Exception) { }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void getGenerateedGstInvoice(string Jobcardid, string type, int invoiceId = 0)
    {
        var objectToSerilize = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        objectToSerilize.Id = "0";
        try
        {
            if (invoiceId > 0)
            {
                clsDataSourse ClsDataSourse = new clsDataSourse();
                string InvNo = "";
                DataTable dtInvoice = dbCon.GetDataTable(" Select GstInvoiceNumber from [dbo].[Invoice] WHERE Id=" + invoiceId);
                if (dtInvoice.Rows.Count > 0)
                {
                    InvNo = dtInvoice.Rows[0][0].ToString();
                }
                objectToSerilize.Id = InvNo;
            }
        }
        catch (Exception) { }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [System.Web.Services.WebMethod]

    public void fillTabledatas_V2(int JobCardId = 0, string Spare = "", string Service = "", bool fromDiscount = false, int type = 1, int fromInvoice = 0, bool fromEstimate = false, bool ShowAllocated = false, int GenerateGstInvoiceNumber = 0)
    {
        var objectToSerilize = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";

        ServiceMethods ServiceMethod = new ServiceMethods();
        ServiceClass.JobCardSpareForInvoice Spares;
        //if (fromInvoice == 0)
        //{
        //    Spares = ServiceMethod.GetJobCardSpareInJobCardForInvoiceV3_new(JobCardId.ToString(), Spare, Service, type, fromEstimate, ShowAllocated);
        //}
        //else
        //{
        Spares = ServiceMethod.GetJobCardSpareInJobCardForInvoiceV3_new_V1(JobCardId: JobCardId.ToString(), type: type, GenerateGstInvoiceNumber: GenerateGstInvoiceNumber);
        //}
        string GstNumber = "";
        bool isIgst = false;
        try
        {
            GstNumber = dbCon.GetDataTable("Select isnull((Select Gst_No from Customer where id=Customer_Id),'') from jobcard where id=" + JobCardId).Rows[0][0].ToString();
            if (!String.IsNullOrWhiteSpace(GstNumber))
            {
                string strFirstTwoDigit = GstNumber.Substring(0, 2);
                if (strFirstTwoDigit != "24")
                {
                    isIgst = true;
                }
            }
        }
        catch (Exception E) { }
        string query = "<table width='100%' border='0' style='border-collapse: collapse;font-size:7.5pt;border-bottom:1px solid #ccc;' cellpadding='0' cellspacing='0'><tbody><tr style='font-weight:bold;'><td width='3%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding: 5px;'>S.No</td><td width='20%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px' lang='english'>PARTICULARS OFServices</td><td width='10%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>HSN/SAC</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>QTY</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px' lang='english'>UNIT PRICE</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px;border-bottom:solid 1px #ccc;' lang='english'>TAXABLE AMT</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px' lang='english'>" + (type == 1 ? "Discount" : "Depreciation Taxable") + " Per Unit</td><td width='30%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0'><table border='0' width='100%' style='font-size:7.5pt;font-weight:bold;' cellpadding='0' cellspacing='0'><tbody><tr><td colspan='2' style='border-bottom:solid thin #ccc;text-align:center;    border-right: 1px solid #ccc;padding:2px'>CGST</td><td colspan='2' style='border-bottom:solid thin #ccc;text-align:center;padding:2px' lang='english'>SGST/UGST</td><td colspan='2' style='border-bottom:solid thin #ccc;text-align:center;padding:2px' lang='english'>IGST</td></tr><tr><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px' lang='english'>Rate (%)</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px' lang='english'>Amount</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>Rate (%)</td><td width='16%' style='text-align:center;padding:2px' lang='english'>Amount</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>Rate (%)</td><td width='16%' style='text-align:center;padding:2px' lang='english'>Amount</td></tr></tbody></table></td><td width='10%' style='text-align:center;border-top:solid 1px #ccc;padding:2px' lang='english' colspan='4'>Amount</td></tr>";

        int count = 0;
        decimal withoutTaxPrice = 0;
        decimal TOTAlCGSTTax = 0;
        decimal TOTAlSGSTTax = 0;
        decimal TotalDiscount = 0;
        decimal CGSTTax_14 = 0;
        decimal CGSTTaxable_14 = 0;
        decimal CGSTTax_9 = 0;
        decimal CGSTTaxable_9 = 0;
        foreach (var temp in Spares.Spares)
        {
            string Temp = "";
            if (temp.Type == "Header")
            {
                if (temp.Name.ToLower().ToString() == "SubTotalForSpare".ToLower().ToString())
                {
                    Temp += "<tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;' lang='english'>Total Before Tax (+)</td><td width='11%' colspan='3' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>   " + temp.TaxableAmount.ToString() + " </td></tr> <tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;' lang='english'><b> " + (type == 1 ? "Total Discount For Spare(-)</b>" : "") + " </td><td width='11%' colspan='3' style='text-align:right;padding:2px;'><b>" + (type == 1 ? temp.TotalDiscountForSpare.ToString():"") + "</b></td></tr><tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>" + (isIgst ? "IGST" : "CGST") + " (+)</td><td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalCGSTAmount.ToString() + "</td></tr><tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>" + (isIgst ? "IGST" : "SGST / UGST") + " (+)</td> <td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalSGSTAmount.ToString() + "</td></tr><tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px; border-right:solid 1px #ccc; border-bottom:solid 1px #ccc; ' lang='english'>Sub Total For Spares</td><td width='11%'  colspan='3' style=text-align:right;padding:2px 10px; border-bottom:solid 1px #ccc; font-weight:bold'>" + temp.SubTotal.ToString() + "</td></tr>";
                    query += Temp;
                }
                else
                {
                    if (fromDiscount)
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td><input id='" + (temp.Name.Contains("Spare") ? "Spare" : "Service") + "-All-Discount' onclick='this.setSelectionRange(0, this.value.length)'  onkeypress='return isNumberKey(event)' onchange='UpdateDiscount(" + (temp.Name.Contains("Spare") ? "1" : "2") + ")' type='text' style='width:80px;margin-left:5px;  border-top: 1px solid #ccc;' onkeypress='return isNumberKey(event)' value=''/></td><td colspan='3' style='text-align:left; padding:2px 10px; border-top:solid 1px #ccc; font-weight:bold'>(i.e. 1000 or i.e 5%)</td></tr>";
                    }
                    else
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td style='border-top:  1px solid #ccc;'></td><td colspan='3' style='text-align:left; padding:2px 10px; border-top:solid 1px #ccc; font-weight:bold'></td></tr>";
                    }
                    query += Temp;
                }
            }
            else
            {
                decimal price = 0;
                try
                {
                    if (temp.SGSTRate == "9.00")
                    {
                        CGSTTax_9 += Convert.ToDecimal(temp.SGSTAmount);
                        if (type == 1)
                        {
                            CGSTTaxable_9 += Convert.ToDecimal(temp.TaxableAmount);
                        }
                        else
                        {
                            CGSTTaxable_9 += (Convert.ToDecimal(temp.TaxableAmount) - Convert.ToDecimal(temp.Discount));
                        }
                    }
                    else
                    {
                        CGSTTax_14 += Convert.ToDecimal(temp.SGSTAmount);
                        if (type == 1)
                        {
                            CGSTTaxable_14 += Convert.ToDecimal(temp.TaxableAmount);
                        }
                        else
                        {
                            CGSTTaxable_14 += (Convert.ToDecimal(temp.TaxableAmount) - Convert.ToDecimal(temp.Discount));
                        }
                    }
                }
                catch (Exception E) { }
                try { price = Convert.ToDecimal(temp.UnitPrice); }
                catch (Exception ex)
                {
                }
                Decimal WithoutTaxPrice1 = 0;
                try
                {
                    WithoutTaxPrice1 = Convert.ToDecimal(temp.TaxableAmount);
                }
                catch (Exception ex)
                {
                }
                try
                {
                    TotalDiscount = Convert.ToDecimal(temp.TotalDiscount);
                }
                catch (Exception ex)
                { }
                //Temp += " <tr><td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + (++count) + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'><span>" + temp.Name + "</span></td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.HSNNumber + "</td> <td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + temp.Quantity + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + price + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + (fromDiscount ? "<input type='text' style='width:80px;' onchange='UpdateDiscount(3)' class='cls-" + temp.ItemType + "' data-dis='" + temp.Discount + "' id='" + temp.ItemType + "-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' value='" + temp.Discount + "'/>" : temp.Discount) + "</td><td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.TaxableAmount + "</td><td style='text-align:left;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0;'> <table border='0' width='100%' style='font-size:7.5pt' cellpadding='0' cellspacing='0'><tbody><tr><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + temp.CGSTRate + "</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + temp.CGSTAmount + "</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + temp.SGSTRate + "</td><td width='25%' style='text-align:center;padding:2px'>" + temp.SGSTAmount + "</td></tr></tbody></table></td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + temp.FinalAmount + "</td></tr>";

                Temp += " <tr><td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + (++count) + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'><span>" + temp.Name + "</span></td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.HSNNumber + "</td> <td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + temp.Quantity + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + price + "</td><td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.TaxableAmount + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>"
                    + (fromDiscount ? "<input type='text' onclick='this.setSelectionRange(0, this.value.length)' style='width:80px;' onchange='UpdateDiscount(3)' class='cls-" + temp.ItemType + "' id='" + temp.ItemType + "-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "'  data-dis='" + temp.Discount + "' value='" + temp.Discount + "'/>" : temp.Discount) + "</td><td style='text-align:left;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0;'> <table border='0' width='100%' style='font-size:7.5pt' cellpadding='0' cellspacing='0'><tbody><tr><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + (isIgst ? "0" : temp.CGSTRate) + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-cgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? "0" : temp.CGSTAmount) + "</span>" + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + (isIgst ? "0" : temp.SGSTRate) + "</td><td width='16%' style='text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-sgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? "0" : temp.SGSTAmount) + "</span>" + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + (isIgst ? (Convert.ToDecimal(temp.SGSTRate) * 2).ToString() : "0") + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-igst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? (Convert.ToDecimal(temp.CGSTAmount) * 2).ToString() : "0") + "</span>" + "</td></tr></tbody></table></td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + "<Span  id='" + temp.ItemType + "-fa-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.FinalAmount + "</span>" + "</td></tr>";

                if (temp.Type == "Header")
                {

                }
                query += Temp;
                withoutTaxPrice += WithoutTaxPrice1;


                //if (!String.IsNullOrEmpty(temp.CGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlCGSTTax += Convert.ToDecimal(temp.CGSTAmount);
                //        TOTAlCGSTTax = Convert.ToDecimal(temp.TotalCGSTAmount);
                //        TOTAlCGSTTax = Math.Round(TOTAlCGSTTax, 0);


                //    }
                //    catch (Exception ex) { }
                //}
                //if (!String.IsNullOrEmpty(temp.SGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlSGSTTax += Convert.ToDecimal(temp.SGSTAmount);

                //        TOTAlSGSTTax = Convert.ToDecimal(temp.TotalSGSTAmount);
                //        TOTAlSGSTTax = Math.Round(TOTAlSGSTTax, 0);

                //    }
                //    catch (Exception ex) { }
                //}
            }
        }
        foreach (var temp in Spares.Services)
        {
            string Temp = "";
            if (temp.Type == "Header")
            {
                if (temp.Name.ToLower().ToString() == "SubTotalForService".ToLower().ToString())
                {
                    //            //  " + ((Convert.ToDecimal(Spares[i].SubTotal)) - (Convert.ToDecimal(Spares[i].TotalSGSTAmount) * 2)).ToString() + "
                    Temp += "<tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;' lang='english'>Total Before Tax (+)</td><td width='11%' colspan='3'  style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>   " + temp.TaxableAmount.ToString() + " </td></tr> <tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;' lang='english'><b> " + (type == 1 ? "Total Discount For Service(-)" : "") + " </b></td><td width='11%' colspan='3' style='text-align:right;padding:2px; '><b>" + (type == 1 ? temp.TotalDiscountForService.ToString() :"")+ "</b></td></tr><tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>" + (isIgst ? "IGST" : "CGST") + " (+)</td><td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalCGSTAmount.ToString() + "</td></tr><tr><td width='90%' colspan='10' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>" + (isIgst ? "IGST" : "SGST / UGST") + " (+)</td> <td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalSGSTAmount.ToString() + "</td></tr><tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;' lang='english'>Sub Total For Service</td><td width='11%' colspan='3' style='text-align:right;padding:2px; border-bottom:solid 1px #ccc;'>" + temp.SubTotal.ToString() + "</td></tr>";
                    query += Temp;
                }
                else
                {
                    if (fromDiscount)
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td><input onclick='this.setSelectionRange(0, this.value.length)' id='" + (temp.Name.Contains("Spare") ? "Spare" : "Service") + "-All-Discount' onkeypress='return isNumberKey(event)' onchange='UpdateDiscount(" + (temp.Name.Contains("Spare") ? "1" : "2") + ")' type='text' style='width:80px;margin-left:5px;' onkeypress='return isNumberKey(event)' value=''/></td><td colspan='3'>(i.e. 1000 or i.e 5%)</td></tr>";
                    }
                    else
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td style='border-top:  1px solid #ccc;'></td><td colspan='3'></td></tr>";
                    }
                    query += Temp;
                }
            }
            else
            {

                decimal price = 0;

                try
                {
                    if (temp.SGSTRate == "9.00")
                    {
                        CGSTTax_9 += Convert.ToDecimal(temp.SGSTAmount); 
                       // CGSTTaxable_9 += Convert.ToDecimal(temp.TaxableAmount);
                        if (type == 1)
                        {
                            CGSTTaxable_9 += Convert.ToDecimal(temp.TaxableAmount);
                        }
                        else
                        {
                            CGSTTaxable_9 += (Convert.ToDecimal(temp.TaxableAmount) - Convert.ToDecimal(temp.Discount));
                        }
                    }
                    else
                    {
                        CGSTTax_14 += Convert.ToDecimal(temp.SGSTAmount);
                        //CGSTTaxable_14 += Convert.ToDecimal(temp.TaxableAmount);
                        if (type == 1)
                        {
                            CGSTTaxable_14 += Convert.ToDecimal(temp.TaxableAmount);
                        }
                        else
                        {
                            CGSTTaxable_14 += (Convert.ToDecimal(temp.TaxableAmount) - Convert.ToDecimal(temp.Discount));
                        }
                    }
                }
                catch (Exception E) { }
                try { price = Convert.ToDecimal(temp.UnitPrice); }
                catch (Exception ex)
                {
                }
                Decimal WithoutTaxPrice1 = 0;
                try
                {
                    WithoutTaxPrice1 = Convert.ToDecimal(temp.TaxableAmount);
                }
                catch (Exception ex)
                {
                }
                try
                {
                    TotalDiscount = Convert.ToDecimal(temp.TotalDiscount);
                }
                catch (Exception ex)
                { }


                Temp += " <tr><td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + (++count) + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'><span>" + temp.Name + "</span></td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.HSNNumber + "</td> <td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + temp.Quantity + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.TaxableAmount + "</td><td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + price + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>"
                    + (fromDiscount ? "<input type='text' style='width:80px;' onclick='this.setSelectionRange(0, this.value.length)' onchange='UpdateDiscount(3)' class='cls-" + temp.ItemType + "' id='" + temp.ItemType + "-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "'  data-dis='" + temp.Discount + "' value='" + temp.Discount + "'/>" : temp.Discount) + "</td><td style='text-align:left;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0;'> <table border='0' width='100%' style='font-size:7.5pt' cellpadding='0' cellspacing='0'><tbody><tr><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + (isIgst ? "0" : temp.CGSTRate) + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-cgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? "0" : temp.CGSTAmount) + "</span>" + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + (isIgst ? "0" : temp.SGSTRate) + "</td><td width='16%' style='text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-sgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? "0" : temp.SGSTAmount) + "</span>" + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + (isIgst ? (Convert.ToDecimal(temp.SGSTRate) * 2).ToString() : "0") + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-igst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? (Convert.ToDecimal(temp.CGSTAmount) * 2).ToString() : "0") + "</span>" + "</td></tr></tbody></table></td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + "<Span  id='" + temp.ItemType + "-fa-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.FinalAmount + "</span>" + "</td></tr>";

                if (temp.Type == "Header")
                {

                }
                query += Temp;
                withoutTaxPrice += WithoutTaxPrice1;
                //if (!String.IsNullOrEmpty(temp.CGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlCGSTTax += Convert.ToDecimal(temp.CGSTAmount);
                //        TOTAlCGSTTax = Convert.ToDecimal(temp.TotalCGSTAmount);
                //        TOTAlCGSTTax = Math.Round(TOTAlCGSTTax, 0);
                //    }
                //    catch (Exception ex) { }
                //}
                //if (!String.IsNullOrEmpty(temp.SGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlSGSTTax += Convert.ToDecimal(temp.SGSTAmount);
                //        TOTAlSGSTTax = Convert.ToDecimal(temp.TotalSGSTAmount);
                //        TOTAlSGSTTax = Math.Round(TOTAlSGSTTax, 0);
                //    }
                //    catch (Exception ex) { }
                //}
            }
            //changes 
        }
        Decimal SubTotal = Convert.ToDecimal(Spares.TotalAmount);
        SubTotal = Math.Round(SubTotal, 2);
        Decimal FinalTotal = SubTotal - TotalDiscount;
        FinalTotal = Math.Round(FinalTotal, 2);
        string strfiltextra = "";
        try
        {
            if (!fromEstimate && type == 1)
            {
                DataTable dtextra = dbCon.GetDataTable("SELECT isnull([CompusaryExcess],0) as [CompusaryExcess] ,isnull([VoluntaryExcess],0) as [VoluntaryExcess] ,isnull([Salvage],0) as [Salvage] FROM [dbo].[JobCard_Insurance_Mapping] where JobCardId=" + JobCardId);
                if (dtextra != null && dtextra.Rows.Count > 0)
                {
                    if (Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Compusary Excess (+)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["CompusaryExcess"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString())).ToString();
                    }
                    if (Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Voluntary Excess (+)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["VoluntaryExcess"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString())).ToString();
                    }
                    if (Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Salvage (+)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["Salvage"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString())).ToString();
                    }
                }
            }

            if (!fromEstimate && type == 2)
            {
                DataTable dtextra = dbCon.GetDataTable("SELECT isnull([CompusaryExcess],0) as [CompusaryExcess] ,isnull([VoluntaryExcess],0) as [VoluntaryExcess] ,isnull([Salvage],0) as [Salvage] FROM [dbo].[JobCard_Insurance_Mapping] where JobCardId=" + JobCardId);
                if (dtextra != null && dtextra.Rows.Count > 0)
                {
                    if (Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Less Compulsory Excess (-)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["CompusaryExcess"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString())).ToString();
                    }
                    if (Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Less Voluntary Excess (-)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["VoluntaryExcess"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString())).ToString();
                    }
                    if (Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Less Salvage (-)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["Salvage"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString())).ToString();
                    }
                }
            }
            Spares.FinalAmount = Math.Round(Convert.ToDecimal(Spares.FinalAmount), 2).ToString();
        }
        catch (Exception E) { }
        query += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Total Amount</td><td colspan='3' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>" + (Convert.ToDecimal(Spares.TotalAmount) + Convert.ToDecimal(Spares.TotalDiscount)) + "</td></tr><tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'> " + (type == 1 ? "Less Discount (-)" : "") + "</td><td colspan='3' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'> " + (type == 1 ?Spares.TotalDiscount:"") + "</td></tr>" + strfiltextra + "<tr style='font-weight:bold;'><td colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Final Amount</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + Spares.FinalAmount + "</td></tr>" +
            //GST 18%
            //"<tr style='font-weight:bold;'><td colspan='11' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" +

            //"</td>"+
            //GST 28%

            "</tbody></table>";
        if (!isIgst)
        {
            query += "<Div Style='float : right;width:50%;'><table style='width:100%;text-align:center;border:1px solid #676464;'><tr><th>Rate</th><th>Taxable</th><th>CGST</th><th>SGST</th><th>IGST</th><th>Total</th></tr>" +
                "<tr><td>GST 18%</td><td>" + CGSTTaxable_9 + "</td><td>" + CGSTTax_9 + "</td><td>" + CGSTTax_9 + "</td><td>-</td><td>" + Math.Round((CGSTTaxable_9 + CGSTTax_9 + CGSTTax_9), 2) + "</td></tr>" +

                "<tr><td>GST 28%</td><td>" + CGSTTaxable_14 + "</td><td>" + CGSTTax_14 + "</td><td>" + CGSTTax_14 + "</td><td>-</td><td>" + Math.Round((CGSTTaxable_14 + CGSTTax_14 + CGSTTax_14), 2) + "</td></tr>" +
                "<tr><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'>Total</td><td style='border-top:1px solid #676464;'><b>" + Math.Round((CGSTTaxable_14 + CGSTTaxable_9 + (CGSTTax_14 * 2) + (CGSTTax_9 * 2)), 2) + "</b></td></tr>" +
                "</table></Div>";
        }
        else
        {
            query += "<Div Style='float : right;width:50%;'><table style='width:100%;text-align:center;border:1px solid #676464;'><tr><th>Rate</th><th>Taxable</th><th>CGST</th><th>SGST</th><th>IGST</th><th>Total</th></tr>" +
                    "<tr><td>GST 18%</td><td>" + CGSTTaxable_9 + "</td><td>" + 0 + "</td><td>" + 0 + "</td><td>" + (CGSTTax_9 * 2) + "</td><td>" + Math.Round((CGSTTaxable_9 + CGSTTax_9 + CGSTTax_9), 2) + "</td></tr>" +

                    "<tr><td>GST 28%</td><td>" + CGSTTaxable_14 + "</td><td>" + 0 + "</td><td>" + 0 + "</td><td>" + (CGSTTax_14 * 2) + "</td><td>" + Math.Round((CGSTTaxable_14 + CGSTTax_14 + CGSTTax_14), 2) + "</td></tr>" +
                    "<tr><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'>Total</td><td style='border-top:1px solid #676464;'><b>" + Math.Round((CGSTTaxable_14 + CGSTTaxable_9 + (CGSTTax_14 * 2) + (CGSTTax_9 * 2)), 2) + "</b></td></tr>" +
                    "</table></Div>";
        }
        int finaltotal = Convert.ToInt32(FinalTotal);
        try
        {
            if (!fromEstimate)
            {
                string strAmount = "";
                if (type == 1)
                {
                    strAmount = " CustomerInvoiceAmount =" + Spares.FinalAmount;
                }
                else
                {
                    strAmount = " InsuranceInvoiceAmount =" + Spares.FinalAmount;
                }
                dbCon.ExecuteQuery("update jobcard set " + strAmount + " where id=" + JobCardId);
            }
        }
        catch (Exception E) { }
        try
        {
            int jobcardinvoiceAmount = ServiceMethod.updateAmountJobCardInvoiceDetail(JobCardId, finaltotal);
        }
        catch (Exception ex)
        {
        }

        string customernotes = ServiceMethod.CustomerNotesForInvoice(JobCardId.ToString(), "2");
        if (customernotes.Contains(","))
        {
            customernotes = customernotes.Replace(",", ", ");
        }
        string substring = "";
        try
        {
            decimal d = Convert.ToDecimal(Spares.FinalAmount.ToString());
            substring = ServiceMethod.NumbersToWords((int)d);
        }
        catch (Exception E) { }
        objectToSerilize.Name = query + "<input type='hidden' id='rpnword' value='" + substring + "'>" + "<input type='hidden' id='customernotes' value='" + customernotes + "'>";
        Context.Response.Write(js.Serialize(objectToSerilize));
    }


    [System.Web.Services.WebMethod]

    public void fillTabledatas_V3(int JobCardId = 0, string Spare = "", string Service = "", bool fromDiscount = false, int type = 1, int fromInvoice = 0, bool fromEstimate = false, bool ShowAllocated = false)
    {
        var objectToSerilize = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";

        ServiceMethods ServiceMethod = new ServiceMethods();
        ServiceClass.JobCardSpareForInvoice Spares;
        //if (fromInvoice == 0)
        //{
        //    Spares = ServiceMethod.GetJobCardSpareInJobCardForInvoiceV3_new(JobCardId.ToString(), Spare, Service, type, fromEstimate, ShowAllocated);
        //}
        //else
        //{
        Spares = ServiceMethod.GetJobCardSpareInJobCardForInvoiceV3_new_V1_Proforma(JobCardId: JobCardId.ToString(), type: type);
        //}
        string GstNumber = "";
        bool isIgst = false;
        try
        {
            GstNumber = dbCon.GetDataTable("Select isnull((Select Gst_No from Customer where id=Customer_Id),'') from jobcard where id=" + JobCardId).Rows[0][0].ToString();
            if (!String.IsNullOrWhiteSpace(GstNumber))
            {
                string strFirstTwoDigit = GstNumber.Substring(0, 2);
                if (strFirstTwoDigit != "24")
                {
                    isIgst = true;
                }
            }
        }
        catch (Exception E) { }
        string query = "<table width='100%' border='0' style='border-collapse: collapse;font-size:7.5pt;border-bottom:1px solid #ccc;' cellpadding='0' cellspacing='0'><tbody><tr style='font-weight:bold;'><td width='3%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding: 5px;'>S.No</td><td width='20%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px' lang='english'>PARTICULARS OFServices</td><td width='10%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>HSN/SAC</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>QTY</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px' lang='english'>UNIT PRICE</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px;border-bottom:solid 1px #ccc;' lang='english'>TAXABLE AMT</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px' lang='english'>" + (type == 1 ? "Discount" : "Depreciation Taxable") + " Per Unit</td><td width='30%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0'><table border='0' width='100%' style='font-size:7.5pt;font-weight:bold;' cellpadding='0' cellspacing='0'><tbody><tr><td colspan='2' style='border-bottom:solid thin #ccc;text-align:center;    border-right: 1px solid #ccc;padding:2px'>CGST</td><td colspan='2' style='border-bottom:solid thin #ccc;text-align:center;padding:2px' lang='english'>" + (isIgst ? "IGST" : "SGST / UGST") + "</td><td colspan='2' style='border-bottom:solid thin #ccc;text-align:center;padding:2px' lang='english'>IGST</td></tr><tr><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px' lang='english'>Rate (%)</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px' lang='english'>Amount</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>Rate (%)</td><td width='16%' style='text-align:center;padding:2px' lang='english'>Amount</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>Rate (%)</td><td width='16%' style='text-align:center;padding:2px' lang='english'>Amount</td></tr></tbody></table></td><td width='10%' style='text-align:center;border-top:solid 1px #ccc;padding:2px' lang='english' colspan='4'>Amount</td></tr>";

        int count = 0;
        decimal withoutTaxPrice = 0;
        decimal TOTAlCGSTTax = 0;
        decimal TOTAlSGSTTax = 0;
        decimal TotalDiscount = 0;
        decimal CGSTTax_14 = 0;
        decimal CGSTTaxable_14 = 0;
        decimal CGSTTax_9 = 0;
        decimal CGSTTaxable_9 = 0;
        foreach (var temp in Spares.Spares)
        {
            string Temp = "";
            if (temp.Type == "Header")
            {
                if (temp.Name.ToLower().ToString() == "SubTotalForSpare".ToLower().ToString())
                {

                    Temp += "<tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;' lang='english'>Total Before Tax (+)</td><td width='11%' colspan='3' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>   " + temp.TaxableAmount.ToString() + " </td></tr> <tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;' lang='english'><b> " + (type == 1 ? "Total Discount  For Spare(-)" : "") + "</b></td><td width='11%' colspan='3' style='text-align:right;padding:2px;'><b>" +(type == 1 ? temp.TotalDiscountForSpare.ToString():"") + "</b></td></tr><tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>CGST (+)</td><td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalCGSTAmount.ToString() + "</td></tr><tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>" + (isIgst ? "IGST" : "SGST / UGST") + " (+)</td> <td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalSGSTAmount.ToString() + "</td></tr><tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px; border-right:solid 1px #ccc; border-bottom:solid 1px #ccc; ' lang='english'>Sub Total For Spares</td><td width='11%'  colspan='3' style=text-align:right;padding:2px 10px; border-bottom:solid 1px #ccc; font-weight:bold'>" + temp.SubTotal.ToString() + "</td></tr>";
                    query += Temp;
                }
                else
                {
                    if (fromDiscount)
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td><input id='" + (temp.Name.Contains("Spare") ? "Spare" : "Service") + "-All-Discount' onclick='this.setSelectionRange(0, this.value.length)'  onkeypress='return isNumberKey(event)' onchange='UpdateDiscount(" + (temp.Name.Contains("Spare") ? "1" : "2") + ")' type='text' style='width:80px;margin-left:5px;  border-top: 1px solid #ccc;' onkeypress='return isNumberKey(event)' value=''/></td><td colspan='3' style='text-align:left; padding:2px 10px; border-top:solid 1px #ccc; font-weight:bold'>(i.e. 1000 or i.e 5%)</td></tr>";
                    }
                    else
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td style='border-top:  1px solid #ccc;'></td><td colspan='3' style='text-align:left; padding:2px 10px; border-top:solid 1px #ccc; font-weight:bold'></td></tr>";
                    }
                    query += Temp;
                }
            }
            else
            {
                decimal price = 0;
                try
                {
                    if (temp.SGSTRate == "9.00")
                    {
                        CGSTTax_9 += Convert.ToDecimal(temp.SGSTAmount); 
                        //CGSTTaxable_9 += Convert.ToDecimal(temp.TaxableAmount);
                        if (type == 1)
                        {
                            CGSTTaxable_9 += Convert.ToDecimal(temp.TaxableAmount);
                        }
                        else
                        {
                            CGSTTaxable_9 += (Convert.ToDecimal(temp.TaxableAmount) - Convert.ToDecimal(temp.Discount));
                        }
                    }
                    else
                    {
                        CGSTTax_14 += Convert.ToDecimal(temp.SGSTAmount); 
                       // CGSTTaxable_14 += Convert.ToDecimal(temp.TaxableAmount);
                        if (type == 1)
                        {
                            CGSTTaxable_14 += Convert.ToDecimal(temp.TaxableAmount);
                        }
                        else
                        {
                            CGSTTaxable_14 += (Convert.ToDecimal(temp.TaxableAmount) - Convert.ToDecimal(temp.Discount));
                        }
                    }
                }
                catch (Exception E) { }
                try { price = Convert.ToDecimal(temp.UnitPrice); }
                catch (Exception ex)
                {
                }
                Decimal WithoutTaxPrice1 = 0;
                try
                {
                    WithoutTaxPrice1 = Convert.ToDecimal(temp.TaxableAmount);
                }
                catch (Exception ex)
                {
                }
                try
                {
                    TotalDiscount = Convert.ToDecimal(temp.TotalDiscount);
                }
                catch (Exception ex)
                { }
                //Temp += " <tr><td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + (++count) + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'><span>" + temp.Name + "</span></td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.HSNNumber + "</td> <td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + temp.Quantity + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + price + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + (fromDiscount ? "<input type='text' style='width:80px;' onchange='UpdateDiscount(3)' class='cls-" + temp.ItemType + "' data-dis='" + temp.Discount + "' id='" + temp.ItemType + "-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' value='" + temp.Discount + "'/>" : temp.Discount) + "</td><td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.TaxableAmount + "</td><td style='text-align:left;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0;'> <table border='0' width='100%' style='font-size:7.5pt' cellpadding='0' cellspacing='0'><tbody><tr><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + temp.CGSTRate + "</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + temp.CGSTAmount + "</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + temp.SGSTRate + "</td><td width='25%' style='text-align:center;padding:2px'>" + temp.SGSTAmount + "</td></tr></tbody></table></td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + temp.FinalAmount + "</td></tr>";

                Temp += " <tr><td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + (++count) + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'><span>" + temp.Name + "</span></td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.HSNNumber + "</td> <td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + temp.Quantity + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + price + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.TaxableAmount + "</td><td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>"
                    + (fromDiscount ? "<input type='text' onclick='this.setSelectionRange(0, this.value.length)' style='width:80px;' onchange='UpdateDiscount(3)' class='cls-" + temp.ItemType + "' id='" + temp.ItemType + "-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "'  data-dis='" + temp.Discount + "' value='" + temp.Discount + "'/>" : temp.Discount) + "</td><td style='text-align:left;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0;'> <table border='0' width='100%' style='font-size:7.5pt' cellpadding='0' cellspacing='0'><tbody><tr><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + (isIgst ? "0" : temp.CGSTRate) + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-cgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? "0" : temp.CGSTAmount) + "</span>" + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + (isIgst ? "0" : temp.SGSTRate) + "</td><td width='16%' style='text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-sgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? "0" : temp.SGSTAmount) + "</span>" + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + (isIgst ? (Convert.ToDecimal(temp.SGSTRate) * 2).ToString() : "0") + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-igst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? (Convert.ToDecimal(temp.CGSTAmount) * 2).ToString() : "0") + "</span>" + "</td></tr></tbody></table></td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + "<Span  id='" + temp.ItemType + "-fa-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.FinalAmount + "</span>" + "</td></tr>";

                if (temp.Type == "Header")
                {

                }
                query += Temp;
                withoutTaxPrice += WithoutTaxPrice1;


                //if (!String.IsNullOrEmpty(temp.CGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlCGSTTax += Convert.ToDecimal(temp.CGSTAmount);
                //        TOTAlCGSTTax = Convert.ToDecimal(temp.TotalCGSTAmount);
                //        TOTAlCGSTTax = Math.Round(TOTAlCGSTTax, 0);


                //    }
                //    catch (Exception ex) { }
                //}
                //if (!String.IsNullOrEmpty(temp.SGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlSGSTTax += Convert.ToDecimal(temp.SGSTAmount);

                //        TOTAlSGSTTax = Convert.ToDecimal(temp.TotalSGSTAmount);
                //        TOTAlSGSTTax = Math.Round(TOTAlSGSTTax, 0);

                //    }
                //    catch (Exception ex) { }
                //}
            }
        }
        foreach (var temp in Spares.Services)
        {
            string Temp = "";
            if (temp.Type == "Header")
            {
                if (temp.Name.ToLower().ToString() == "SubTotalForService".ToLower().ToString())
                {
                    //            //  " + ((Convert.ToDecimal(Spares[i].SubTotal)) - (Convert.ToDecimal(Spares[i].TotalSGSTAmount) * 2)).ToString() + "
                    Temp += "<tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;' lang='english'>Total Before Tax (+)</td><td width='11%' colspan='3'  style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>   " + temp.TaxableAmount.ToString() + " </td></tr> <tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;' lang='english'><b>Total " + (type == 1 ? "Discount" : "Depreciation") + " For Service(-)</b></td><td width='11%' colspan='3' style='text-align:right;padding:2px; '><b>" + temp.TotalDiscountForService.ToString() + "</b></td></tr><tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>" + (isIgst ? "IGST" : "CGST") + " (+)</td><td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalCGSTAmount.ToString() + "</td></tr><tr><td width='90%' colspan='10' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>" + (isIgst ? "IGST" : "SGST / UGST") + " (+)</td> <td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalSGSTAmount.ToString() + "</td></tr><tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;' lang='english'>Sub Total For Service</td><td width='11%' colspan='3' style='text-align:right;padding:2px; border-bottom:solid 1px #ccc;'>" + temp.SubTotal.ToString() + "</td></tr>";
                    query += Temp;
                }
                else
                {
                    if (fromDiscount)
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td><input onclick='this.setSelectionRange(0, this.value.length)' id='" + (temp.Name.Contains("Spare") ? "Spare" : "Service") + "-All-Discount' onkeypress='return isNumberKey(event)' onchange='UpdateDiscount(" + (temp.Name.Contains("Spare") ? "1" : "2") + ")' type='text' style='width:80px;margin-left:5px;' onkeypress='return isNumberKey(event)' value=''/></td><td colspan='3'>(i.e. 1000 or i.e 5%)</td></tr>";
                    }
                    else
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td style='border-top:  1px solid #ccc;'></td><td colspan='3'></td></tr>";
                    }
                    query += Temp;
                }
            }
            else
            {

                decimal price = 0;

                try
                {
                    if (temp.SGSTRate == "9.00")
                    {
                        CGSTTax_9 += Convert.ToDecimal(temp.SGSTAmount);
                        //CGSTTaxable_9 += Convert.ToDecimal(temp.TaxableAmount);
                        if (type == 1)
                        {
                            CGSTTaxable_9 += Convert.ToDecimal(temp.TaxableAmount);
                        }
                        else
                        {
                            CGSTTaxable_9 += (Convert.ToDecimal(temp.TaxableAmount) - Convert.ToDecimal(temp.Discount));
                        }
                    }
                    else
                    {
                        CGSTTax_14 += Convert.ToDecimal(temp.SGSTAmount);
                        // CGSTTaxable_14 += Convert.ToDecimal(temp.TaxableAmount);
                        if (type == 1)
                        {
                            CGSTTaxable_14 += Convert.ToDecimal(temp.TaxableAmount);
                        }
                        else
                        {
                            CGSTTaxable_14 += (Convert.ToDecimal(temp.TaxableAmount) - Convert.ToDecimal(temp.Discount));
                        }
                    }
                }
                catch (Exception E) { }
                try { price = Convert.ToDecimal(temp.UnitPrice); }
                catch (Exception ex)
                {
                }
                Decimal WithoutTaxPrice1 = 0;
                try
                {
                    WithoutTaxPrice1 = Convert.ToDecimal(temp.TaxableAmount);
                }
                catch (Exception ex)
                {
                }
                try
                {
                    TotalDiscount = Convert.ToDecimal(temp.TotalDiscount);
                }
                catch (Exception ex)
                { }


                Temp += " <tr><td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + (++count) + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'><span>" + temp.Name + "</span></td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.HSNNumber + "</td> <td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + temp.Quantity + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + price + "</td><td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.TaxableAmount + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>"
                    + (fromDiscount ? "<input type='text' style='width:80px;' onclick='this.setSelectionRange(0, this.value.length)' onchange='UpdateDiscount(3)' class='cls-" + temp.ItemType + "' id='" + temp.ItemType + "-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "'  data-dis='" + temp.Discount + "' value='" + temp.Discount + "'/>" : temp.Discount) + "</td><td style='text-align:left;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0;'> <table border='0' width='100%' style='font-size:7.5pt' cellpadding='0' cellspacing='0'><tbody><tr><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + (isIgst ? "0" : temp.CGSTRate) + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-cgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? "0" : temp.CGSTAmount) + "</span>" + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + (isIgst ? "0" : temp.SGSTRate) + "</td><td width='16%' style='text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-sgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? "0" : temp.SGSTAmount) + "</span>" + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + (isIgst ? (Convert.ToDecimal(temp.SGSTRate) * 2).ToString() : "0") + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-igst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? (Convert.ToDecimal(temp.CGSTAmount) * 2).ToString() : "0") + "</span>" + "</td></tr></tbody></table></td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + "<Span  id='" + temp.ItemType + "-fa-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.FinalAmount + "</span>" + "</td></tr>";

                if (temp.Type == "Header")
                { }
                query += Temp;
                withoutTaxPrice += WithoutTaxPrice1;
                //if (!String.IsNullOrEmpty(temp.CGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlCGSTTax += Convert.ToDecimal(temp.CGSTAmount);
                //        TOTAlCGSTTax = Convert.ToDecimal(temp.TotalCGSTAmount);
                //        TOTAlCGSTTax = Math.Round(TOTAlCGSTTax, 0);
                //    }
                //    catch (Exception ex) { }
                //}
                //if (!String.IsNullOrEmpty(temp.SGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlSGSTTax += Convert.ToDecimal(temp.SGSTAmount);
                //        TOTAlSGSTTax = Convert.ToDecimal(temp.TotalSGSTAmount);
                //        TOTAlSGSTTax = Math.Round(TOTAlSGSTTax, 0);
                //    }
                //    catch (Exception ex) { }
                //}
            }
            //changes 
        }
        Decimal SubTotal = Convert.ToDecimal(Spares.TotalAmount);
        SubTotal = Math.Round(SubTotal, 2);
        Decimal FinalTotal = SubTotal - TotalDiscount;
        FinalTotal = Math.Round(FinalTotal, 2);
        string strfiltextra = "";
        try
        {
            if (!fromEstimate && type == 1)
            {
                DataTable dtextra = dbCon.GetDataTable("SELECT isnull([CompusaryExcess],0) as [CompusaryExcess] ,isnull([VoluntaryExcess],0) as [VoluntaryExcess] ,isnull([Salvage],0) as [Salvage] FROM [dbo].[JobCard_Insurance_Mapping] where JobCardId=" + JobCardId);
                if (dtextra != null && dtextra.Rows.Count > 0)
                {
                    if (Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Compusary Excess (+)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["CompusaryExcess"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString())).ToString();
                    }
                    if (Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Voluntary Excess (+)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["VoluntaryExcess"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString())).ToString();
                    }
                    if (Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Salvage (+)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["Salvage"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString())).ToString();
                    }
                }
            }

            if (!fromEstimate && type == 2)
            {
                DataTable dtextra = dbCon.GetDataTable("SELECT isnull([CompusaryExcess],0) as [CompusaryExcess] ,isnull([VoluntaryExcess],0) as [VoluntaryExcess] ,isnull([Salvage],0) as [Salvage] FROM [dbo].[JobCard_Insurance_Mapping] where JobCardId=" + JobCardId);
                if (dtextra != null && dtextra.Rows.Count > 0)
                {
                    if (Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Less Compulsory Excess (-)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["CompusaryExcess"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString())).ToString();
                    }
                    if (Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Less Voluntary Excess (-)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["VoluntaryExcess"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString())).ToString();
                    }
                    if (Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Less Salvage (-)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["Salvage"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString())).ToString();
                    }
                }
            }
            Spares.FinalAmount = Math.Round(Convert.ToDecimal(Spares.FinalAmount), 2).ToString();
        }
        catch (Exception E) { }
        query += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Total Amount</td><td colspan='3' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>" + (Convert.ToDecimal(Spares.TotalAmount) + Convert.ToDecimal(Spares.TotalDiscount)) + "</td></tr><tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Less " + (type == 1 ? "Discount" : "Depreciation") + " (-)</td><td colspan='3' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'> " + Spares.TotalDiscount + "</td></tr>" + strfiltextra + "<tr style='font-weight:bold;'><td colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Final Amount</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + Spares.FinalAmount + "</td></tr>" +
            //GST 18%
            //"<tr style='font-weight:bold;'><td colspan='11' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" +

            //"</td>"+
            //GST 28%

            "</tbody></table>";
        if (!isIgst)
        {
            query += "<Div Style='float : right;width:50%;'><table style='width:100%;text-align:center;border:1px solid #676464;'><tr><th>Rate</th><th>Taxable</th><th>CGST</th><th>SGST</th><th>IGST</th><th>Total</th></tr>" +
                "<tr><td>GST 18%</td><td>" + CGSTTaxable_9 + "</td><td>" + CGSTTax_9 + "</td><td>" + CGSTTax_9 + "</td><td>-</td><td>" + Math.Round((CGSTTaxable_9 + CGSTTax_9 + CGSTTax_9), 2) + "</td></tr>" +

                "<tr><td>GST 28%</td><td>" + CGSTTaxable_14 + "</td><td>" + CGSTTax_14 + "</td><td>" + CGSTTax_14 + "</td><td>-</td><td>" + Math.Round((CGSTTaxable_14 + CGSTTax_14 + CGSTTax_14), 2) + "</td></tr>" +
                "<tr><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'>Total</td><td style='border-top:1px solid #676464;'><b>" + Math.Round((CGSTTaxable_14 + CGSTTaxable_9 + (CGSTTax_14 * 2) + (CGSTTax_9 * 2)), 2) + "</b></td></tr>" +
                "</table></Div>";
        }
        else
        {
            query += "<Div Style='float : right;width:50%;'><table style='width:100%;text-align:center;border:1px solid #676464;'><tr><th>Rate</th><th>Taxable</th><th>CGST</th><th>SGST</th><th>IGST</th><th>Total</th></tr>" +
                    "<tr><td>GST 18%</td><td>" + CGSTTaxable_9 + "</td><td>" + 0 + "</td><td>" + 0 + "</td><td>" + (CGSTTax_9 * 2) + "</td><td>" + Math.Round((CGSTTaxable_9 + CGSTTax_9 + CGSTTax_9), 2) + "</td></tr>" +

                    "<tr><td>GST 28%</td><td>" + CGSTTaxable_14 + "</td><td>" + 0 + "</td><td>" + 0 + "</td><td>" + (CGSTTax_14 * 2) + "</td><td>" + Math.Round((CGSTTaxable_14 + CGSTTax_14 + CGSTTax_14), 2) + "</td></tr>" +
                    "<tr><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'>Total</td><td style='border-top:1px solid #676464;'><b>" + Math.Round((CGSTTaxable_14 + CGSTTaxable_9 + (CGSTTax_14 * 2) + (CGSTTax_9 * 2)), 2) + "</b></td></tr>" +
                    "</table></Div>";
        }
        int finaltotal = Convert.ToInt32(FinalTotal);
        try
        {
            if (!fromEstimate)
            {
                string strAmount = "";
                if (type == 1)
                {
                    strAmount = " CustomerInvoiceAmount =" + Spares.FinalAmount;
                }
                else
                {
                    strAmount = " InsuranceInvoiceAmount =" + Spares.FinalAmount;
                }
                dbCon.ExecuteQuery("update jobcard set " + strAmount + " where id=" + JobCardId);
            }
        }
        catch (Exception E) { }
        try
        {
            int jobcardinvoiceAmount = ServiceMethod.updateAmountJobCardInvoiceDetail(JobCardId, finaltotal);
        }
        catch (Exception ex)
        {
        }

        string customernotes = ServiceMethod.CustomerNotesForInvoice(JobCardId.ToString(), "2");
        if (customernotes.Contains(","))
        {
            customernotes = customernotes.Replace(",", ", ");
        }
        string substring = "";
        try
        {
            decimal d = Convert.ToDecimal(Spares.FinalAmount.ToString());
            substring = ServiceMethod.NumbersToWords((int)d);
        }
        catch (Exception E) { }
        objectToSerilize.Name = query + "<input type='hidden' id='rpnword' value='" + substring + "'>" + "<input type='hidden' id='customernotes' value='" + customernotes + "'>";
        Context.Response.Write(js.Serialize(objectToSerilize));
    }


    [System.Web.Services.WebMethod]

    public void fillTabledatas_V3_Profit(int JobCardId = 0, string Spare = "", string Service = "", bool fromDiscount = false, int type = 1, int fromInvoice = 0, bool fromEstimate = false, bool ShowAllocated = false)
    {
        var objectToSerilize = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";

        ServiceMethods ServiceMethod = new ServiceMethods();
        ServiceClass.JobCardSpareForInvoice Spares;
        //if (fromInvoice == 0)
        //{
        //    Spares = ServiceMethod.GetJobCardSpareInJobCardForInvoiceV3_new(JobCardId.ToString(), Spare, Service, type, fromEstimate, ShowAllocated);
        //}
        //else
        //{
        Spares = ServiceMethod.GetJobCardSpareInJobCardForInvoiceV3_new_V1_Proforma(JobCardId: JobCardId.ToString(), type: type);
        //}
        string GstNumber = "";
        bool isIgst = false;
        decimal TotalProfitSpare = 0;
        decimal TotalProfitService = 0;
        try
        {
            GstNumber = dbCon.GetDataTable("Select isnull((Select Gst_No from Customer where id=Customer_Id),'') from jobcard where id=" + JobCardId).Rows[0][0].ToString();
            if (!String.IsNullOrWhiteSpace(GstNumber))
            {
                string strFirstTwoDigit = GstNumber.Substring(0, 2);
                if (strFirstTwoDigit != "24")
                {
                    isIgst = true;
                }
            }
        }
        catch (Exception E) { }
        string query = "<table width='100%' border='0' style='border-collapse: collapse;font-size:7.5pt;border-bottom:1px solid #ccc;' cellpadding='0' cellspacing='0'><tbody><tr style='font-weight:bold;'><td width='3%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding: 5px;'>S.No</td><td width='20%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px' lang='english'>PARTICULARS OFServices</td><td width='10%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>HSN/SAC</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>QTY</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px' lang='english'>UNIT PRICE</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px' lang='english'>" + (type == 1 ? "Discount" : "DEPRECIATION TAXABLE") + " Per Unit</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px;border-bottom:solid 1px #ccc;' lang='english'>TAXABLE AMT</td><td width='30%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0'><table border='0' width='100%' style='font-size:7.5pt;font-weight:bold;' cellpadding='0' cellspacing='0'><tbody><tr><td colspan='2' style='border-bottom:solid thin #ccc;text-align:center;    border-right: 1px solid #ccc;padding:2px'>CGST</td><td colspan='2' style='border-bottom:solid thin #ccc;text-align:center;padding:2px' lang='english'>" + (isIgst ? "IGST" : "SGST / UGST") + "</td><td colspan='2' style='border-bottom:solid thin #ccc;text-align:center;padding:2px' lang='english'>IGST</td></tr><tr><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px' lang='english'>Rate (%)</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px' lang='english'>Amount</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>Rate (%)</td><td width='16%' style='text-align:center;padding:2px' lang='english'>Amount</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>Rate (%)</td><td width='16%' style='text-align:center;padding:2px' lang='english'>Amount</td></tr></tbody></table></td><td width='10%' style='text-align:center;border-top:solid 1px #ccc;padding:2px' lang='english' colspan='2'>Amount</td><td width='10%' style='text-align:center;border-top:solid 1px #ccc;padding:2px' lang='english' colspan='1'>Pur Pri / U</td><td width='10%' style='text-align:center;border-top:solid 1px #ccc;padding:2px' lang='english' colspan='1'>Profit</td></tr>";

        int count = 0;
        decimal withoutTaxPrice = 0;
        decimal TOTAlCGSTTax = 0;
        decimal TOTAlSGSTTax = 0;
        decimal TotalDiscount = 0;
        decimal CGSTTax_14 = 0;
        decimal CGSTTaxable_14 = 0;
        decimal CGSTTax_9 = 0;
        decimal CGSTTaxable_9 = 0;
        string subtotal = "";
        foreach (var temp in Spares.Spares)
        {
            string Temp = "";
            if (temp.Type == "Header")
            {
                if (temp.Name.ToLower().ToString() == "SubTotalForSpare".ToLower().ToString())
                {
                    subtotal = temp.SubTotal;
                    //Temp += "<tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;' lang='english'>Total Before Tax (+)</td><td width='11%' colspan='2' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>   " + temp.TaxableAmount.ToString() + " </td><td width='11%' colspan='2' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'></td></tr> <tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;' lang='english'><b>Total " + (type == 1 ? "Discount" : "Depreciation") + " For Spare(-)</b></td><td width='11%' colspan='2' style='text-align:right;padding:2px;'><b>" + temp.TotalDiscountForSpare.ToString() + "</b></td><td width='11%' colspan='3' style='text-align:right;padding:2px;'><b></b></td></tr><tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>CGST (+)</td><td width='11%' colspan='2' style='text-align:right;padding:2px;'>" + temp.TotalCGSTAmount.ToString() + "</td><td width='11%' colspan='2' style='text-align:right;padding:2px;'></td></tr><tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>" + (isIgst ? "IGST" : "SGST / UGST") + " (+)</td> <td width='11%' colspan='2' style='text-align:right;padding:2px;'>" + temp.TotalSGSTAmount.ToString() + "</td><td width='11%' colspan='2' style='text-align:right;padding:2px;'></td></tr><tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px; border-right:solid 1px #ccc; border-bottom:solid 1px #ccc; ' lang='english'>Sub Total For Spares</td><td width='11%'  colspan='2' style=text-align:right;padding:2px 10px; border-bottom:solid 1px #ccc; font-weight:bold'>" + temp.SubTotal.ToString() + "</td><td width='11%' colspan='2' style='text-align:right;padding:2px;'></td></tr>";
                    //query += Temp;

                }
                else
                {
                    if (fromDiscount)
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td><input id='" + (temp.Name.Contains("Spare") ? "Spare" : "Service") + "-All-Discount' onclick='this.setSelectionRange(0, this.value.length)'  onkeypress='return isNumberKey(event)' onchange='UpdateDiscount(" + (temp.Name.Contains("Spare") ? "1" : "2") + ")' type='text' style='width:80px;margin-left:5px;  border-top: 1px solid #ccc;' onkeypress='return isNumberKey(event)' value=''/></td><td colspan='3' style='text-align:left; padding:2px 10px; border-top:solid 1px #ccc; font-weight:bold'>(i.e. 1000 or i.e 5%)</td></tr>";
                    }
                    else
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td style='border-top:  1px solid #ccc;'></td><td colspan='3' style='text-align:left; padding:2px 10px; border-top:solid 1px #ccc; font-weight:bold'></td></tr>";
                    }
                    query += Temp;
                }
            }
            else
            {
                decimal price = 0;
                try
                {
                    if (temp.SGSTRate == "9.00")
                    {
                        CGSTTax_9 += Convert.ToDecimal(temp.SGSTAmount); CGSTTaxable_9 += Convert.ToDecimal(temp.TaxableAmount);
                    }
                    else
                    {
                        CGSTTax_14 += Convert.ToDecimal(temp.SGSTAmount); CGSTTaxable_14 += Convert.ToDecimal(temp.TaxableAmount);
                    }
                    TotalProfitSpare += Convert.ToDecimal(temp.Profit);
                }
                catch (Exception E) { }
                try { price = Convert.ToDecimal(temp.UnitPrice); }
                catch (Exception ex)
                {
                }
                Decimal WithoutTaxPrice1 = 0;
                try
                {
                    WithoutTaxPrice1 = Convert.ToDecimal(temp.TaxableAmount);
                }
                catch (Exception ex)
                {
                }
                try
                {
                    TotalDiscount = Convert.ToDecimal(temp.TotalDiscount);
                }
                catch (Exception ex)
                { }
                //Temp += " <tr><td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + (++count) + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'><span>" + temp.Name + "</span></td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.HSNNumber + "</td> <td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + temp.Quantity + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + price + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + (fromDiscount ? "<input type='text' style='width:80px;' onchange='UpdateDiscount(3)' class='cls-" + temp.ItemType + "' data-dis='" + temp.Discount + "' id='" + temp.ItemType + "-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' value='" + temp.Discount + "'/>" : temp.Discount) + "</td><td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.TaxableAmount + "</td><td style='text-align:left;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0;'> <table border='0' width='100%' style='font-size:7.5pt' cellpadding='0' cellspacing='0'><tbody><tr><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + temp.CGSTRate + "</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + temp.CGSTAmount + "</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + temp.SGSTRate + "</td><td width='25%' style='text-align:center;padding:2px'>" + temp.SGSTAmount + "</td></tr></tbody></table></td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + temp.FinalAmount + "</td></tr>";

                Temp += " <tr><td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + (++count) + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'><span>" + temp.Name + "</span></td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.HSNNumber + "</td> <td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + temp.Quantity + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + price + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>"
                    + (fromDiscount ? "<input type='text' onclick='this.setSelectionRange(0, this.value.length)' style='width:80px;' onchange='UpdateDiscount(3)' class='cls-" + temp.ItemType + "' id='" + temp.ItemType + "-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "'  data-dis='" + temp.Discount + "' value='" + temp.Discount + "'/>" : temp.Discount) + "</td><td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.TaxableAmount + "</td><td style='text-align:left;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0;'> <table border='0' width='100%' style='font-size:7.5pt' cellpadding='0' cellspacing='0'><tbody><tr><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + (isIgst ? "0" : temp.CGSTRate) + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-cgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? "0" : temp.CGSTAmount) + "</span>" + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + (isIgst ? "0" : temp.SGSTRate) + "</td><td width='16%' style='text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-sgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? "0" : temp.SGSTAmount) + "</span>" + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + (isIgst ? (Convert.ToDecimal(temp.SGSTRate) * 2).ToString() : "0") + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-igst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? (Convert.ToDecimal(temp.CGSTAmount) * 2).ToString() : "0") + "</span>" + "</td></tr></tbody></table></td><td colspan='2' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + "<Span  id='" + temp.ItemType + "-fa-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.FinalAmount + "</span>" + "</td><td colspan='1' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + "<Span  id='" + temp.ItemType + "-Purchase-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.PurchasePrice + "</span>" + "</td><td colspan='1' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + "<Span  id='" + temp.ItemType + "-Profit-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.Profit + "</span>" + "</td></tr>";

                if (temp.Type == "Header")
                {

                }
                query += Temp;
                withoutTaxPrice += WithoutTaxPrice1;


                //if (!String.IsNullOrEmpty(temp.CGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlCGSTTax += Convert.ToDecimal(temp.CGSTAmount);
                //        TOTAlCGSTTax = Convert.ToDecimal(temp.TotalCGSTAmount);
                //        TOTAlCGSTTax = Math.Round(TOTAlCGSTTax, 0);


                //    }
                //    catch (Exception ex) { }
                //}
                //if (!String.IsNullOrEmpty(temp.SGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlSGSTTax += Convert.ToDecimal(temp.SGSTAmount);

                //        TOTAlSGSTTax = Convert.ToDecimal(temp.TotalSGSTAmount);
                //        TOTAlSGSTTax = Math.Round(TOTAlSGSTTax, 0);

                //    }
                //    catch (Exception ex) { }
                //}
            }
        }
        query += "<tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;' lang='english'>Total </td><td width='11%' colspan='2' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>" + subtotal + "</td><td width='11%' colspan='2' style='text-align:right;padding:2px;border-top:solid 1px #ccc;' >" + TotalProfitSpare + "</td></tr>";
        subtotal = "";
        foreach (var temp in Spares.Services)
        {
            string Temp = "";
            if (temp.Type == "Header")
            {
                if (temp.Name.ToLower().ToString() == "SubTotalForService".ToLower().ToString())
                {
                    subtotal = temp.SubTotal.ToString();
                    //            //  " + ((Convert.ToDecimal(Spares[i].SubTotal)) - (Convert.ToDecimal(Spares[i].TotalSGSTAmount) * 2)).ToString() + "
                    //Temp += "<tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;' lang='english'>Total Before Tax (+)</td><td width='11%' colspan='3'  style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>   " + temp.TaxableAmount.ToString() + " </td></tr> <tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;' lang='english'><b>Total " + (type == 1 ? "Discount" : "Depreciation") + " For Service(-)</b></td><td width='11%' colspan='3' style='text-align:right;padding:2px; '><b>" + temp.TotalDiscountForService.ToString() + "</b></td></tr><tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>" + (isIgst ? "IGST" : "CGST") + " (+)</td><td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalCGSTAmount.ToString() + "</td></tr><tr><td width='90%' colspan='10' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>" + (isIgst ? "IGST" : "SGST / UGST") + " (+)</td> <td width='11%' colspan='2' style='text-align:right;padding:2px;'>" + temp.TotalSGSTAmount.ToString() + "</td><td width='11%' colspan='2' style='text-align:right;padding:2px;'></td></tr><tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;' lang='english'>Sub Total For Service</td><td width='11%' colspan='3' style='text-align:right;padding:2px; border-bottom:solid 1px #ccc;'>" + temp.SubTotal.ToString() + "</td></tr>";
                    //query += Temp;
                }
                else
                {
                    if (fromDiscount)
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td><input onclick='this.setSelectionRange(0, this.value.length)' id='" + (temp.Name.Contains("Spare") ? "Spare" : "Service") + "-All-Discount' onkeypress='return isNumberKey(event)' onchange='UpdateDiscount(" + (temp.Name.Contains("Spare") ? "1" : "2") + ")' type='text' style='width:80px;margin-left:5px;' onkeypress='return isNumberKey(event)' value=''/></td><td colspan='3'>(i.e. 1000 or i.e 5%)</td></tr>";
                    }
                    else
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td style='border-top:  1px solid #ccc;'></td><td colspan='3'></td></tr>";
                    }
                    query += Temp;
                }
            }
            else
            {

                decimal price = 0;

                try
                {
                    if (temp.SGSTRate == "9.00")
                    {
                        CGSTTax_9 += Convert.ToDecimal(temp.SGSTAmount); CGSTTaxable_9 += Convert.ToDecimal(temp.TaxableAmount);
                    }
                    else
                    {
                        CGSTTax_14 += Convert.ToDecimal(temp.SGSTAmount); CGSTTaxable_14 += Convert.ToDecimal(temp.TaxableAmount);
                    }
                    TotalProfitService += Convert.ToDecimal(temp.Profit);
                }
                catch (Exception E) { }
                try { price = Convert.ToDecimal(temp.UnitPrice); }
                catch (Exception ex)
                {
                }
                Decimal WithoutTaxPrice1 = 0;
                try
                {
                    WithoutTaxPrice1 = Convert.ToDecimal(temp.TaxableAmount);
                }
                catch (Exception ex)
                {
                }
                try
                {
                    TotalDiscount = Convert.ToDecimal(temp.TotalDiscount);
                }
                catch (Exception ex)
                { }


                Temp += " <tr><td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + (++count) + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'><span>" + temp.Name + "</span></td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.HSNNumber + "</td> <td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + temp.Quantity + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + price + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>"
                    + (fromDiscount ? "<input type='text' style='width:80px;' onclick='this.setSelectionRange(0, this.value.length)' onchange='UpdateDiscount(3)' class='cls-" + temp.ItemType + "' id='" + temp.ItemType + "-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "'  data-dis='" + temp.Discount + "' value='" + temp.Discount + "'/>" : temp.Discount) + "</td><td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.TaxableAmount + "</td><td style='text-align:left;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0;'> <table border='0' width='100%' style='font-size:7.5pt' cellpadding='0' cellspacing='0'><tbody><tr><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + (isIgst ? "0" : temp.CGSTRate) + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-cgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? "0" : temp.CGSTAmount) + "</span>" + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + (isIgst ? "0" : temp.SGSTRate) + "</td><td width='16%' style='text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-sgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? "0" : temp.SGSTAmount) + "</span>" + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + (isIgst ? (Convert.ToDecimal(temp.SGSTRate) * 2).ToString() : "0") + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-igst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? (Convert.ToDecimal(temp.CGSTAmount) * 2).ToString() : "0") + "</span>" + "</td></tr></tbody></table></td><td colspan='2' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + "<Span  id='" + temp.ItemType + "-fa-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.FinalAmount + "</span>" + "</td><td colspan='1' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + "<Span  id='" + temp.ItemType + "-Purchase-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >0.00</span>" + "</td><td colspan='1' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + "<Span  id='" + temp.ItemType + "-Profit-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.Profit + "</span>" + "</td></tr>";

                if (temp.Type == "Header")
                {

                }
                query += Temp;
                withoutTaxPrice += WithoutTaxPrice1;
                //if (!String.IsNullOrEmpty(temp.CGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlCGSTTax += Convert.ToDecimal(temp.CGSTAmount);
                //        TOTAlCGSTTax = Convert.ToDecimal(temp.TotalCGSTAmount);
                //        TOTAlCGSTTax = Math.Round(TOTAlCGSTTax, 0);
                //    }
                //    catch (Exception ex) { }
                //}
                //if (!String.IsNullOrEmpty(temp.SGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlSGSTTax += Convert.ToDecimal(temp.SGSTAmount);
                //        TOTAlSGSTTax = Convert.ToDecimal(temp.TotalSGSTAmount);
                //        TOTAlSGSTTax = Math.Round(TOTAlSGSTTax, 0);
                //    }
                //    catch (Exception ex) { }
                //}
            }
            //changes 
        }
        query += "<tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;' lang='english'>Total </td><td width='11%' colspan='2' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>" + subtotal + "</td><td width='11%' colspan='2' style='text-align:right;padding:2px;border-top:solid 1px #ccc;' >" + TotalProfitService + "</td></tr>";
        Decimal SubTotal = Convert.ToDecimal(Spares.TotalAmount);
        SubTotal = Math.Round(SubTotal, 2);
        Decimal FinalTotal = SubTotal - TotalDiscount;
        FinalTotal = Math.Round(FinalTotal, 2);
        string strfiltextra = "";
        try
        {
            if (!fromEstimate && type == 1)
            {
                DataTable dtextra = dbCon.GetDataTable("SELECT isnull([CompusaryExcess],0) as [CompusaryExcess] ,isnull([VoluntaryExcess],0) as [VoluntaryExcess] ,isnull([Salvage],0) as [Salvage] FROM [dbo].[JobCard_Insurance_Mapping] where JobCardId=" + JobCardId);
                if (dtextra != null && dtextra.Rows.Count > 0)
                {
                    if (Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Compusary Excess (+)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["CompusaryExcess"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString())).ToString();
                    }
                    if (Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Voluntary Excess (+)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["VoluntaryExcess"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString())).ToString();
                    }
                    if (Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Salvage (+)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["Salvage"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString())).ToString();
                    }
                }
            }

            if (!fromEstimate && type == 2)
            {
                DataTable dtextra = dbCon.GetDataTable("SELECT isnull([CompusaryExcess],0) as [CompusaryExcess] ,isnull([VoluntaryExcess],0) as [VoluntaryExcess] ,isnull([Salvage],0) as [Salvage] FROM [dbo].[JobCard_Insurance_Mapping] where JobCardId=" + JobCardId);
                if (dtextra != null && dtextra.Rows.Count > 0)
                {
                    if (Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString()) > 0)
                    {
                        //strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Less Compulsory Excess (-)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["CompusaryExcess"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString())).ToString();
                    }
                    if (Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString()) > 0)
                    {
                        //strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Less Voluntary Excess (-)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["VoluntaryExcess"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString())).ToString();
                    }
                    if (Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString()) > 0)
                    {
                        // strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Less Salvage (-)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["Salvage"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString())).ToString();
                    }
                }
            }
            Spares.FinalAmount = Math.Round(Convert.ToDecimal(Spares.FinalAmount), 2).ToString();
        }
        catch (Exception E) { }
        query += "<tr style='font-weight:bold;'><td colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Final Total</td><td colspan='2' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + Spares.FinalAmount + "</td><td colspan='2' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>" + (TotalProfitService + TotalProfitSpare) + "</td></tr>";
        //query += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Total Profit(Parts+Service)</td><td colspan='4' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>" + (TotalProfitService + TotalProfitSpare) + "</td></tr>" +
            //GST 18%
            //"<tr style='font-weight:bold;'><td colspan='11' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" +

            //"</td>"+
            //GST 28%

           // "</tbody></table>";
        //if (!isIgst)
        //{
        //    query += "<Div Style='float : right;width:50%;'><table style='width:100%;text-align:center;border:1px solid #676464;'><tr><th>Rate</th><th>Taxable</th><th>CGST</th><th>SGST</th><th>IGST</th><th>Total</th></tr>" +
        //        "<tr><td>GST 18%</td><td>" + CGSTTaxable_9 + "</td><td>" + CGSTTax_9 + "</td><td>" + CGSTTax_9 + "</td><td>-</td><td>" + Math.Round((CGSTTaxable_9 + CGSTTax_9 + CGSTTax_9), 2) + "</td></tr>" +

        //        "<tr><td>GST 28%</td><td>" + CGSTTaxable_14 + "</td><td>" + CGSTTax_14 + "</td><td>" + CGSTTax_14 + "</td><td>-</td><td>" + Math.Round((CGSTTaxable_14 + CGSTTax_14 + CGSTTax_14), 2) + "</td></tr>" +
        //        "<tr><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'>Total</td><td style='border-top:1px solid #676464;'><b>" + Math.Round((CGSTTaxable_14 + CGSTTaxable_9 + (CGSTTax_14 * 2) + (CGSTTax_9 * 2)), 2) + "</b></td></tr>" +
        //        "</table></Div>";
        //}
        //else
        //{
        //    query += "<Div Style='float : right;width:50%;'><table style='width:100%;text-align:center;border:1px solid #676464;'><tr><th>Rate</th><th>Taxable</th><th>CGST</th><th>SGST</th><th>IGST</th><th>Total</th></tr>" +
        //            "<tr><td>GST 18%</td><td>" + CGSTTaxable_9 + "</td><td>" + 0 + "</td><td>" + 0 + "</td><td>" + (CGSTTax_9 * 2) + "</td><td>" + Math.Round((CGSTTaxable_9 + CGSTTax_9 + CGSTTax_9), 2) + "</td></tr>" +

        //            "<tr><td>GST 28%</td><td>" + CGSTTaxable_14 + "</td><td>" + 0 + "</td><td>" + 0 + "</td><td>" + (CGSTTax_14 * 2) + "</td><td>" + Math.Round((CGSTTaxable_14 + CGSTTax_14 + CGSTTax_14), 2) + "</td></tr>" +
        //            "<tr><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'>Total</td><td style='border-top:1px solid #676464;'><b>" + Math.Round((CGSTTaxable_14 + CGSTTaxable_9 + (CGSTTax_14 * 2) + (CGSTTax_9 * 2)), 2) + "</b></td></tr>" +
        //            "</table></Div>";
        //}
        int finaltotal = Convert.ToInt32(FinalTotal);
        try
        {
            if (!fromEstimate)
            {
                string strAmount = "";
                if (type == 1)
                {
                    strAmount = " CustomerInvoiceAmount =" + Spares.FinalAmount;
                }
                else
                {
                    strAmount = " InsuranceInvoiceAmount =" + Spares.FinalAmount;
                }
                dbCon.ExecuteQuery("update jobcard set " + strAmount + " where id=" + JobCardId);
            }
        }
        catch (Exception E) { }
        try
        {
            int jobcardinvoiceAmount = ServiceMethod.updateAmountJobCardInvoiceDetail(JobCardId, finaltotal);
        }
        catch (Exception ex)
        {
        }

        string customernotes = ServiceMethod.CustomerNotesForInvoice(JobCardId.ToString(), "2");
        if (customernotes.Contains(","))
        {
            customernotes = customernotes.Replace(",", ", ");
        }
        string substring = "";
        try
        {
            decimal d = Convert.ToDecimal(Spares.FinalAmount.ToString());
            substring = ServiceMethod.NumbersToWords((int)d);
        }
        catch (Exception E) { }
        objectToSerilize.Name = query + "<input type='hidden' id='rpnword' value='" + substring + "'>" + "<input type='hidden' id='customernotes' value='" + customernotes + "'>";
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [System.Web.Services.WebMethod]

    public void fillTabledatas_ProformaWithId(int JobCardId = 0, string Spare = "", string Service = "", bool fromDiscount = false, int type = 1, int fromInvoice = 0, bool fromEstimate = false, bool ShowAllocated = false, int InvoiceId = 0)
    {
        var objectToSerilize = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";

        ServiceMethods ServiceMethod = new ServiceMethods();
        ServiceClass.JobCardSpareForInvoice Spares;
        //if (fromInvoice == 0)
        //{
        //    Spares = ServiceMethod.GetJobCardSpareInJobCardForInvoiceV3_new(JobCardId.ToString(), Spare, Service, type, fromEstimate, ShowAllocated);
        //}
        //else
        //{
        Spares = ServiceMethod.GetJobCardSpareInJobCardForInvoiceV3_new_V1_ProformaById(JobCardId: JobCardId.ToString(), type: type, ProformaInvoiceId: InvoiceId);
        //}
        string GstNumber = "";
        bool isIgst = false;
        try
        {
            GstNumber = dbCon.GetDataTable("Select isnull((Select Gst_No from Customer where id=Customer_Id),'') from jobcard where id=" + JobCardId).Rows[0][0].ToString();
            if (!String.IsNullOrWhiteSpace(GstNumber))
            {
                string strFirstTwoDigit = GstNumber.Substring(0, 2);
                if (strFirstTwoDigit != "24")
                {
                    isIgst = true;
                }
            }
        }
        catch (Exception E) { }
        string query = "<table width='100%' border='0' style='border-collapse: collapse;font-size:7.5pt;border-bottom:1px solid #ccc;' cellpadding='0' cellspacing='0'><tbody><tr style='font-weight:bold;'><td width='3%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding: 5px;'>S.No</td><td width='20%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px' lang='english'>PARTICULARS OFServices</td><td width='10%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>HSN/SAC</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>QTY</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px' lang='english'>UNIT PRICE</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px' lang='english'>" + (type == 1 ? "Discount" : "Depreciation") + " Per Unit</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px;border-bottom:solid 1px #ccc;' lang='english'>TAXABLE AMT</td><td width='30%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0'><table border='0' width='100%' style='font-size:7.5pt;font-weight:bold;' cellpadding='0' cellspacing='0'><tbody><tr><td colspan='2' style='border-bottom:solid thin #ccc;text-align:center;    border-right: 1px solid #ccc;padding:2px'>CGST</td><td colspan='2' style='border-bottom:solid thin #ccc;text-align:center;padding:2px' lang='english'>" + (isIgst ? "IGST" : "SGST / UGST") + "</td><td colspan='2' style='border-bottom:solid thin #ccc;text-align:center;padding:2px' lang='english'>IGST</td></tr><tr><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px' lang='english'>Rate (%)</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px' lang='english'>Amount</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>Rate (%)</td><td width='16%' style='text-align:center;padding:2px' lang='english'>Amount</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>Rate (%)</td><td width='16%' style='text-align:center;padding:2px' lang='english'>Amount</td></tr></tbody></table></td><td width='10%' style='text-align:center;border-top:solid 1px #ccc;padding:2px' lang='english' colspan='4'>Amount</td></tr>";

        int count = 0;
        decimal withoutTaxPrice = 0;
        decimal TOTAlCGSTTax = 0;
        decimal TOTAlSGSTTax = 0;
        decimal TotalDiscount = 0;
        decimal CGSTTax_14 = 0;
        decimal CGSTTaxable_14 = 0;
        decimal CGSTTax_9 = 0;
        decimal CGSTTaxable_9 = 0;
        foreach (var temp in Spares.Spares)
        {
            string Temp = "";
            if (temp.Type == "Header")
            {
                if (temp.Name.ToLower().ToString() == "SubTotalForSpare".ToLower().ToString())
                {

                    Temp += "<tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;' lang='english'>Total Before Tax (+)</td><td width='11%' colspan='3' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>   " + temp.TaxableAmount.ToString() + " </td></tr> <tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;' lang='english'><b>Total " + (type == 1 ? "Discount" : "Depreciation") + " For Spare(-)</b></td><td width='11%' colspan='3' style='text-align:right;padding:2px;'><b>" + temp.TotalDiscountForSpare.ToString() + "</b></td></tr><tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>CGST (+)</td><td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalCGSTAmount.ToString() + "</td></tr><tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>" + (isIgst ? "IGST" : "SGST / UGST") + " (+)</td> <td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalSGSTAmount.ToString() + "</td></tr><tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px; border-right:solid 1px #ccc; border-bottom:solid 1px #ccc; ' lang='english'>Sub Total For Spares</td><td width='11%'  colspan='3' style=text-align:right;padding:2px 10px; border-bottom:solid 1px #ccc; font-weight:bold'>" + temp.SubTotal.ToString() + "</td></tr>";
                    query += Temp;
                }
                else
                {
                    if (fromDiscount)
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td><input id='" + (temp.Name.Contains("Spare") ? "Spare" : "Service") + "-All-Discount' onclick='this.setSelectionRange(0, this.value.length)'  onkeypress='return isNumberKey(event)' onchange='UpdateDiscount(" + (temp.Name.Contains("Spare") ? "1" : "2") + ")' type='text' style='width:80px;margin-left:5px;  border-top: 1px solid #ccc;' onkeypress='return isNumberKey(event)' value=''/></td><td colspan='3' style='text-align:left; padding:2px 10px; border-top:solid 1px #ccc; font-weight:bold'>(i.e. 1000 or i.e 5%)</td></tr>";
                    }
                    else
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td style='border-top:  1px solid #ccc;'></td><td colspan='3' style='text-align:left; padding:2px 10px; border-top:solid 1px #ccc; font-weight:bold'></td></tr>";
                    }
                    query += Temp;
                }
            }
            else
            {
                decimal price = 0;
                try
                {
                    if (temp.SGSTRate == "9.00")
                    {
                        CGSTTax_9 += Convert.ToDecimal(temp.SGSTAmount); CGSTTaxable_9 += Convert.ToDecimal(temp.TaxableAmount);
                    }
                    else
                    {
                        CGSTTax_14 += Convert.ToDecimal(temp.SGSTAmount); CGSTTaxable_14 += Convert.ToDecimal(temp.TaxableAmount);
                    }
                }
                catch (Exception E) { }
                try { price = Convert.ToDecimal(temp.UnitPrice); }
                catch (Exception ex)
                {
                }
                Decimal WithoutTaxPrice1 = 0;
                try
                {
                    WithoutTaxPrice1 = Convert.ToDecimal(temp.TaxableAmount);
                }
                catch (Exception ex)
                {
                }
                try
                {
                    TotalDiscount = Convert.ToDecimal(temp.TotalDiscount);
                }
                catch (Exception ex)
                { }
                //Temp += " <tr><td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + (++count) + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'><span>" + temp.Name + "</span></td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.HSNNumber + "</td> <td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + temp.Quantity + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + price + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + (fromDiscount ? "<input type='text' style='width:80px;' onchange='UpdateDiscount(3)' class='cls-" + temp.ItemType + "' data-dis='" + temp.Discount + "' id='" + temp.ItemType + "-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' value='" + temp.Discount + "'/>" : temp.Discount) + "</td><td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.TaxableAmount + "</td><td style='text-align:left;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0;'> <table border='0' width='100%' style='font-size:7.5pt' cellpadding='0' cellspacing='0'><tbody><tr><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + temp.CGSTRate + "</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + temp.CGSTAmount + "</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + temp.SGSTRate + "</td><td width='25%' style='text-align:center;padding:2px'>" + temp.SGSTAmount + "</td></tr></tbody></table></td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + temp.FinalAmount + "</td></tr>";

                Temp += " <tr><td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + (++count) + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'><span>" + temp.Name + "</span></td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.HSNNumber + "</td> <td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + temp.Quantity + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + price + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>"
                    + (fromDiscount ? "<input type='text' onclick='this.setSelectionRange(0, this.value.length)' style='width:80px;' onchange='UpdateDiscount(3)' class='cls-" + temp.ItemType + "' id='" + temp.ItemType + "-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "'  data-dis='" + temp.Discount + "' value='" + temp.Discount + "'/>" : temp.Discount) + "</td><td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.TaxableAmount + "</td><td style='text-align:left;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0;'> <table border='0' width='100%' style='font-size:7.5pt' cellpadding='0' cellspacing='0'><tbody><tr><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + (isIgst ? "0" : temp.CGSTRate) + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-cgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? "0" : temp.CGSTAmount) + "</span>" + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + (isIgst ? "0" : temp.SGSTRate) + "</td><td width='16%' style='text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-sgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? "0" : temp.SGSTAmount) + "</span>" + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + (isIgst ? (Convert.ToDecimal(temp.SGSTRate) * 2).ToString() : "0") + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-igst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? (Convert.ToDecimal(temp.CGSTAmount) * 2).ToString() : "0") + "</span>" + "</td></tr></tbody></table></td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + "<Span  id='" + temp.ItemType + "-fa-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.FinalAmount + "</span>" + "</td></tr>";

                if (temp.Type == "Header")
                {

                }
                query += Temp;
                withoutTaxPrice += WithoutTaxPrice1;


                //if (!String.IsNullOrEmpty(temp.CGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlCGSTTax += Convert.ToDecimal(temp.CGSTAmount);
                //        TOTAlCGSTTax = Convert.ToDecimal(temp.TotalCGSTAmount);
                //        TOTAlCGSTTax = Math.Round(TOTAlCGSTTax, 0);


                //    }
                //    catch (Exception ex) { }
                //}
                //if (!String.IsNullOrEmpty(temp.SGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlSGSTTax += Convert.ToDecimal(temp.SGSTAmount);

                //        TOTAlSGSTTax = Convert.ToDecimal(temp.TotalSGSTAmount);
                //        TOTAlSGSTTax = Math.Round(TOTAlSGSTTax, 0);

                //    }
                //    catch (Exception ex) { }
                //}
            }
        }
        foreach (var temp in Spares.Services)
        {
            string Temp = "";
            if (temp.Type == "Header")
            {
                if (temp.Name.ToLower().ToString() == "SubTotalForService".ToLower().ToString())
                {
                    //            //  " + ((Convert.ToDecimal(Spares[i].SubTotal)) - (Convert.ToDecimal(Spares[i].TotalSGSTAmount) * 2)).ToString() + "
                    Temp += "<tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;' lang='english'>Total Before Tax (+)</td><td width='11%' colspan='3'  style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>   " + temp.TaxableAmount.ToString() + " </td></tr> <tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;' lang='english'><b>Total " + (type == 1 ? "Discount" : "Depreciation") + " For Service(-)</b></td><td width='11%' colspan='3' style='text-align:right;padding:2px; '><b>" + temp.TotalDiscountForService.ToString() + "</b></td></tr><tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>" + (isIgst ? "IGST" : "CGST") + " (+)</td><td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalCGSTAmount.ToString() + "</td></tr><tr><td width='90%' colspan='10' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>" + (isIgst ? "IGST" : "SGST / UGST") + " (+)</td> <td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalSGSTAmount.ToString() + "</td></tr><tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;' lang='english'>Sub Total For Service</td><td width='11%' colspan='3' style='text-align:right;padding:2px; border-bottom:solid 1px #ccc;'>" + temp.SubTotal.ToString() + "</td></tr>";
                    query += Temp;
                }
                else
                {
                    if (fromDiscount)
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td><input onclick='this.setSelectionRange(0, this.value.length)' id='" + (temp.Name.Contains("Spare") ? "Spare" : "Service") + "-All-Discount' onkeypress='return isNumberKey(event)' onchange='UpdateDiscount(" + (temp.Name.Contains("Spare") ? "1" : "2") + ")' type='text' style='width:80px;margin-left:5px;' onkeypress='return isNumberKey(event)' value=''/></td><td colspan='3'>(i.e. 1000 or i.e 5%)</td></tr>";
                    }
                    else
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td style='border-top:  1px solid #ccc;'></td><td colspan='3'></td></tr>";
                    }
                    query += Temp;
                }
            }
            else
            {

                decimal price = 0;

                try
                {
                    if (temp.SGSTRate == "9.00")
                    {
                        CGSTTax_9 += Convert.ToDecimal(temp.SGSTAmount); CGSTTaxable_9 += Convert.ToDecimal(temp.TaxableAmount);
                    }
                    else
                    {
                        CGSTTax_14 += Convert.ToDecimal(temp.SGSTAmount); CGSTTaxable_14 += Convert.ToDecimal(temp.TaxableAmount);
                    }
                }
                catch (Exception E) { }
                try { price = Convert.ToDecimal(temp.UnitPrice); }
                catch (Exception ex)
                {
                }
                Decimal WithoutTaxPrice1 = 0;
                try
                {
                    WithoutTaxPrice1 = Convert.ToDecimal(temp.TaxableAmount);
                }
                catch (Exception ex)
                {
                }
                try
                {
                    TotalDiscount = Convert.ToDecimal(temp.TotalDiscount);
                }
                catch (Exception ex)
                { }


                Temp += " <tr><td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + (++count) + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'><span>" + temp.Name + "</span></td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.HSNNumber + "</td> <td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + temp.Quantity + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + price + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>"
                    + (fromDiscount ? "<input type='text' style='width:80px;' onclick='this.setSelectionRange(0, this.value.length)' onchange='UpdateDiscount(3)' class='cls-" + temp.ItemType + "' id='" + temp.ItemType + "-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "'  data-dis='" + temp.Discount + "' value='" + temp.Discount + "'/>" : temp.Discount) + "</td><td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.TaxableAmount + "</td><td style='text-align:left;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0;'> <table border='0' width='100%' style='font-size:7.5pt' cellpadding='0' cellspacing='0'><tbody><tr><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + (isIgst ? "0" : temp.CGSTRate) + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-cgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? "0" : temp.CGSTAmount) + "</span>" + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + (isIgst ? "0" : temp.SGSTRate) + "</td><td width='16%' style='text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-sgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? "0" : temp.SGSTAmount) + "</span>" + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + (isIgst ? (Convert.ToDecimal(temp.SGSTRate) * 2).ToString() : "0") + "</td><td width='16%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-igst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + (isIgst ? (Convert.ToDecimal(temp.CGSTAmount) * 2).ToString() : "0") + "</span>" + "</td></tr></tbody></table></td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + "<Span  id='" + temp.ItemType + "-fa-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.FinalAmount + "</span>" + "</td></tr>";

                if (temp.Type == "Header")
                {

                }
                query += Temp;
                withoutTaxPrice += WithoutTaxPrice1;
                //if (!String.IsNullOrEmpty(temp.CGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlCGSTTax += Convert.ToDecimal(temp.CGSTAmount);
                //        TOTAlCGSTTax = Convert.ToDecimal(temp.TotalCGSTAmount);
                //        TOTAlCGSTTax = Math.Round(TOTAlCGSTTax, 0);
                //    }
                //    catch (Exception ex) { }
                //}
                //if (!String.IsNullOrEmpty(temp.SGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlSGSTTax += Convert.ToDecimal(temp.SGSTAmount);
                //        TOTAlSGSTTax = Convert.ToDecimal(temp.TotalSGSTAmount);
                //        TOTAlSGSTTax = Math.Round(TOTAlSGSTTax, 0);
                //    }
                //    catch (Exception ex) { }
                //}
            }
            //changes 
        }
        Decimal SubTotal = Convert.ToDecimal(Spares.TotalAmount);
        SubTotal = Math.Round(SubTotal, 2);
        Decimal FinalTotal = SubTotal - TotalDiscount;
        FinalTotal = Math.Round(FinalTotal, 2);
        string strfiltextra = "";
        try
        {
            if (!fromEstimate && type == 1)
            {
                DataTable dtextra = dbCon.GetDataTable("SELECT isnull([CompusaryExcess],0) as [CompusaryExcess] ,isnull([VoluntaryExcess],0) as [VoluntaryExcess] ,isnull([Salvage],0) as [Salvage] FROM [dbo].[JobCard_Insurance_Mapping] where JobCardId=" + JobCardId);
                if (dtextra != null && dtextra.Rows.Count > 0)
                {
                    if (Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Compusary Excess (+)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["CompusaryExcess"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString())).ToString();
                    }
                    if (Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Voluntary Excess (+)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["VoluntaryExcess"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString())).ToString();
                    }
                    if (Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Salvage (+)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["Salvage"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString())).ToString();
                    }
                }
            }

            if (!fromEstimate && type == 2)
            {
                DataTable dtextra = dbCon.GetDataTable("SELECT isnull([CompusaryExcess],0) as [CompusaryExcess] ,isnull([VoluntaryExcess],0) as [VoluntaryExcess] ,isnull([Salvage],0) as [Salvage] FROM [dbo].[JobCard_Insurance_Mapping] where JobCardId=" + JobCardId);
                if (dtextra != null && dtextra.Rows.Count > 0)
                {
                    if (Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Less Compulsory Excess (-)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["CompusaryExcess"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString())).ToString();
                    }
                    if (Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Less Voluntary Excess (-)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["VoluntaryExcess"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString())).ToString();
                    }
                    if (Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString()) > 0)
                    {
                        strfiltextra += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Less Salvage (-)</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + dtextra.Rows[0]["Salvage"].ToString() + "</td></tr>";
                        Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString())).ToString();
                    }
                }
            }
            Spares.FinalAmount = Math.Round(Convert.ToDecimal(Spares.FinalAmount), 2).ToString();
        }
        catch (Exception E) { }
        query += "<tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Total Amount</td><td colspan='3' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>" + (Convert.ToDecimal(Spares.TotalAmount) + Convert.ToDecimal(Spares.TotalDiscount)) + "</td></tr><tr style='font-weight:bold;'><td colspan='10' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Less " + (type == 1 ? "Discount" : "Depreciation") + " (-)</td><td colspan='3' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'> " + Spares.TotalDiscount + "</td></tr>" + strfiltextra + "<tr style='font-weight:bold;'><td colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Final Amount</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + Spares.FinalAmount + "</td></tr>" +
            //GST 18%
            //"<tr style='font-weight:bold;'><td colspan='11' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" +

            //"</td>"+
            //GST 28%

            "</tbody></table>";
        if (!isIgst)
        {
            query += "<Div Style='float : right;width:50%;'><table style='width:100%;text-align:center;border:1px solid #676464;'><tr><th>Rate</th><th>Taxable</th><th>CGST</th><th>SGST</th><th>IGST</th><th>Total</th></tr>" +
                "<tr><td>GST 18%</td><td>" + CGSTTaxable_9 + "</td><td>" + CGSTTax_9 + "</td><td>" + CGSTTax_9 + "</td><td>-</td><td>" + Math.Round((CGSTTaxable_9 + CGSTTax_9 + CGSTTax_9), 2) + "</td></tr>" +

                "<tr><td>GST 28%</td><td>" + CGSTTaxable_14 + "</td><td>" + CGSTTax_14 + "</td><td>" + CGSTTax_14 + "</td><td>-</td><td>" + Math.Round((CGSTTaxable_14 + CGSTTax_14 + CGSTTax_14), 2) + "</td></tr>" +
                "<tr><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'>Total</td><td style='border-top:1px solid #676464;'><b>" + Math.Round((CGSTTaxable_14 + CGSTTaxable_9 + (CGSTTax_14 * 2) + (CGSTTax_9 * 2)), 2) + "</b></td></tr>" +
                "</table></Div>";
        }
        else
        {
            query += "<Div Style='float : right;width:50%;'><table style='width:100%;text-align:center;border:1px solid #676464;'><tr><th>Rate</th><th>Taxable</th><th>CGST</th><th>SGST</th><th>IGST</th><th>Total</th></tr>" +
                    "<tr><td>GST 18%</td><td>" + CGSTTaxable_9 + "</td><td>" + 0 + "</td><td>" + 0 + "</td><td>" + (CGSTTax_9 * 2) + "</td><td>" + Math.Round((CGSTTaxable_9 + CGSTTax_9 + CGSTTax_9), 2) + "</td></tr>" +

                    "<tr><td>GST 28%</td><td>" + CGSTTaxable_14 + "</td><td>" + 0 + "</td><td>" + 0 + "</td><td>" + (CGSTTax_14 * 2) + "</td><td>" + Math.Round((CGSTTaxable_14 + CGSTTax_14 + CGSTTax_14), 2) + "</td></tr>" +
                    "<tr><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'></td><td style='border-top:1px solid #676464;'>Total</td><td style='border-top:1px solid #676464;'><b>" + Math.Round((CGSTTaxable_14 + CGSTTaxable_9 + (CGSTTax_14 * 2) + (CGSTTax_9 * 2)), 2) + "</b></td></tr>" +
                    "</table></Div>";
        }
        int finaltotal = Convert.ToInt32(FinalTotal);
        try
        {
            if (!fromEstimate)
            {
                string strAmount = "";
                if (type == 1)
                {
                    strAmount = " CustomerInvoiceAmount =" + Spares.FinalAmount;
                }
                else
                {
                    strAmount = " InsuranceInvoiceAmount =" + Spares.FinalAmount;
                }
                dbCon.ExecuteQuery("update jobcard set " + strAmount + " where id=" + JobCardId);
            }
        }
        catch (Exception E) { }
        try
        {
            int jobcardinvoiceAmount = ServiceMethod.updateAmountJobCardInvoiceDetail(JobCardId, finaltotal);
        }
        catch (Exception ex)
        {
        }

        string customernotes = ServiceMethod.CustomerNotesForInvoice(JobCardId.ToString(), "2");
        if (customernotes.Contains(","))
        {
            customernotes = customernotes.Replace(",", ", ");
        }
        string substring = "";
        try
        {
            decimal d = Convert.ToDecimal(Spares.FinalAmount.ToString());
            substring = ServiceMethod.NumbersToWords((int)d);
        }
        catch (Exception E) { }
        objectToSerilize.Name = query + "<input type='hidden' id='rpnword' value='" + substring + "'>" + "<input type='hidden' id='customernotes' value='" + customernotes + "'>";
        Context.Response.Write(js.Serialize(objectToSerilize));
    }


    [WebMethod]
    public void fillTabledatas_Invoice(int JobCardId = 0, string Spare = "", string Service = "", bool fromDiscount = false, int Type = 1)
    {
        var objectToSerilize = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";

        ServiceMethods ServiceMethod = new ServiceMethods();
        var Spares = ServiceMethod.GetJobCardSpareInJobCardForInvoiceV3(JobCardId.ToString(), Type);

        string query = "<table width='100%' border='0' style='border-collapse: collapse;font-size:7.5pt;border-bottom:1px solid #ccc;' cellpadding='0' cellspacing='0'><tbody><tr style='font-weight:bold;'><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding: 5px;'>S.No</td><td width='20%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px' lang='english'>PARTICULARS OFServices</td><td width='10%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>HSN/SAC</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>QTY</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px' lang='english'>UNIT PRICE</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px' lang='english'>Discount Per Unit</td><td width='5%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px' lang='english'>TAXABLE AMT</td><td width='27%' style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0'><table border='0' width='100%' style='font-size:7.5pt;font-weight:bold;' cellpadding='0' cellspacing='0'><tbody><tr><td colspan='2' style='border-bottom:solid thin #ccc;text-align:center;    border-right: 1px solid #ccc;padding:2px'>CGST</td><td colspan='2' style='border-bottom:solid thin #ccc;text-align:center;padding:2px' lang='english'>SGST/UGST</td></tr><tr><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px' lang='english'>Rate (%)</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px' lang='english'>Amount</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>Rate (%)</td><td width='25%' style='text-align:center;padding:2px' lang='english'>Amount</td></tr></tbody></table></td><td width='10%' style='text-align:center;border-top:solid 1px #ccc;padding:2px' lang='english' colspan='4'>Amount</td></tr>";

        int count = 0;
        decimal withoutTaxPrice = 0;
        decimal TOTAlCGSTTax = 0;
        decimal TOTAlSGSTTax = 0;
        decimal TotalDiscount = 0;
        foreach (var temp in Spares.Spares)
        {
            string Temp = "";
            if (temp.Type == "Header")
            {
                if (temp.Name.ToLower().ToString() == "SubTotalForSpare".ToLower().ToString())
                {

                    Temp += "<tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;' lang='english'>Total Before Tax (+)</td><td width='11%' colspan='3' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>   " + temp.TaxableAmount.ToString() + " </td></tr> <tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;' lang='english'><b>Total Discount For Spare(-)</b></td><td width='11%' colspan='3' style='text-align:right;padding:2px;'><b>" + temp.TotalDiscountForSpare.ToString() + "</b></td></tr><tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>CGST (+)</td><td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalCGSTAmount.ToString() + "</td></tr><tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>SGST/UGST (+)</td> <td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalSGSTAmount.ToString() + "</td></tr><tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px; border-right:solid 1px #ccc; border-bottom:solid 1px #ccc; ' lang='english'>Sub Total For Spares</td><td width='11%'  colspan='3' style=text-align:right;padding:2px 10px; border-bottom:solid 1px #ccc; font-weight:bold'>" + temp.SubTotal.ToString() + "</td></tr>";
                    query += Temp;
                }
                else
                {
                    if (fromDiscount)
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td><input id='" + (temp.Name.Contains("Spare") ? "Spare" : "Service") + "-All-Discount' onkeypress='return isNumberKey(event)' onchange='UpdateDiscount(" + (temp.Name.Contains("Spare") ? "1" : "2") + ")' type='text' style='width:80px;margin-left:5px;  border-top: 1px solid #ccc;' onkeypress='return isNumberKey(event)' value=''/></td><td colspan='3' style='text-align:left; padding:2px 10px; border-top:solid 1px #ccc; font-weight:bold'>(i.e. 1000 or i.e 5%)</td></tr>";
                    }
                    else
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td></td><td colspan='3' style='text-align:left; padding:2px 10px; border-top:solid 1px #ccc; font-weight:bold'></td></tr>";
                    }
                    query += Temp;
                }
            }
            else
            {

                decimal price = 0;
                try { price = Convert.ToDecimal(temp.UnitPrice); }
                catch (Exception ex)
                {
                }
                Decimal WithoutTaxPrice1 = 0;
                try
                {
                    WithoutTaxPrice1 = Convert.ToDecimal(temp.TaxableAmount);
                }
                catch (Exception ex)
                {
                }
                try
                {
                    TotalDiscount = Convert.ToDecimal(temp.TotalDiscount);
                }
                catch (Exception ex)
                { }
                //Temp += " <tr><td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + (++count) + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'><span>" + temp.Name + "</span></td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.HSNNumber + "</td> <td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + temp.Quantity + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + price + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + (fromDiscount ? "<input type='text' style='width:80px;' onchange='UpdateDiscount(3)' class='cls-" + temp.ItemType + "' data-dis='" + temp.Discount + "' id='" + temp.ItemType + "-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' value='" + temp.Discount + "'/>" : temp.Discount) + "</td><td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.TaxableAmount + "</td><td style='text-align:left;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0;'> <table border='0' width='100%' style='font-size:7.5pt' cellpadding='0' cellspacing='0'><tbody><tr><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + temp.CGSTRate + "</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + temp.CGSTAmount + "</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + temp.SGSTRate + "</td><td width='25%' style='text-align:center;padding:2px'>" + temp.SGSTAmount + "</td></tr></tbody></table></td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + temp.FinalAmount + "</td></tr>";

                Temp += " <tr><td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + (++count) + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'><span>" + temp.Name + "</span></td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.HSNNumber + "</td> <td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + temp.Quantity + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + price + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>"
          + (fromDiscount ? "<input type='text' style='width:80px;' onchange='UpdateDiscount(3)' class='cls-" + temp.ItemType + "' id='" + temp.ItemType + "-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "'  data-dis='" + temp.Discount + "' value='" + temp.Discount + "'/>" : temp.Discount) + "</td><td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.TaxableAmount + "</td><td style='text-align:left;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0;'> <table border='0' width='100%' style='font-size:7.5pt' cellpadding='0' cellspacing='0'><tbody><tr><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + temp.CGSTRate + "</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-cgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.CGSTAmount + "</span>" + "</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + temp.SGSTRate + "</td><td width='25%' style='text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-sgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.SGSTAmount + "</span>" + "</td></tr></tbody></table></td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + "<Span  id='" + temp.ItemType + "-fa-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.FinalAmount + "</span>" + "</td></tr>";

                if (temp.Type == "Header")
                {

                }
                query += Temp;
                withoutTaxPrice += WithoutTaxPrice1;


                //if (!String.IsNullOrEmpty(temp.CGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlCGSTTax += Convert.ToDecimal(temp.CGSTAmount);
                //        TOTAlCGSTTax = Convert.ToDecimal(temp.TotalCGSTAmount);
                //        TOTAlCGSTTax = Math.Round(TOTAlCGSTTax, 0);


                //    }
                //    catch (Exception ex) { }
                //}
                //if (!String.IsNullOrEmpty(temp.SGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlSGSTTax += Convert.ToDecimal(temp.SGSTAmount);

                //        TOTAlSGSTTax = Convert.ToDecimal(temp.TotalSGSTAmount);
                //        TOTAlSGSTTax = Math.Round(TOTAlSGSTTax, 0);

                //    }
                //    catch (Exception ex) { }
                //}
            }
        }
        foreach (var temp in Spares.Services)
        {
            string Temp = "";
            if (temp.Type == "Header")
            {
                if (temp.Name.ToLower().ToString() == "SubTotalForService".ToLower().ToString())
                {
                    //            //  " + ((Convert.ToDecimal(Spares[i].SubTotal)) - (Convert.ToDecimal(Spares[i].TotalSGSTAmount) * 2)).ToString() + "
                    Temp += "<tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;' lang='english'>Total Before Tax (+)</td><td width='11%' colspan='3'  style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>   " + temp.TaxableAmount.ToString() + " </td></tr> <tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;' lang='english'><b>Total Discount For Service(-)</b></td><td width='11%' colspan='3' style='text-align:right;padding:2px; '><b>" + temp.TotalDiscountForService.ToString() + "</b></td></tr><tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>CGST (+)</td><td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalCGSTAmount.ToString() + "</td></tr><tr><td width='90%' colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;'>SGST/UGST (+)</td> <td width='11%' colspan='3' style='text-align:right;padding:2px;'>" + temp.TotalSGSTAmount.ToString() + "</td></tr><tr><td width='90%'  colspan='8' style='text-align:right;padding-right:5px;border-right:solid 1px #ccc;' lang='english'>Sub Total For Service</td><td width='11%' colspan='3' style='text-align:right;padding:2px; border-bottom:solid 1px #ccc;'>" + temp.SubTotal.ToString() + "</td></tr>";
                    query += Temp;
                }
                else
                {
                    if (fromDiscount)
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td><input id='" + (temp.Name.Contains("Spare") ? "Spare" : "Service") + "-All-Discount' onkeypress='return isNumberKey(event)' onchange='UpdateDiscount(" + (temp.Name.Contains("Spare") ? "1" : "2") + ")' type='text' style='width:80px;margin-left:5px;' onkeypress='return isNumberKey(event)' value=''/></td><td colspan='3'>(i.e. 1000 or i.e 5%)</td></tr>";
                    }
                    else
                    {
                        Temp += " <tr><td colspan='5' style='text-align:left;padding:2px 10px;border-top:solid 1px #ccc;font-weight:bold'>" + temp.Name + "</td><td></td><td colspan='3'></td></tr>";
                    }
                    query += Temp;
                }
            }
            else
            {

                decimal price = 0;
                try { price = Convert.ToDecimal(temp.UnitPrice); }
                catch (Exception ex)
                {
                }
                Decimal WithoutTaxPrice1 = 0;
                try
                {
                    WithoutTaxPrice1 = Convert.ToDecimal(temp.TaxableAmount);
                }
                catch (Exception ex)
                {
                }
                try
                {
                    TotalDiscount = Convert.ToDecimal(temp.TotalDiscount);
                }
                catch (Exception ex)
                { }


                Temp += " <tr><td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + (++count) + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'><span>" + temp.Name + "</span></td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.HSNNumber + "</td> <td style='text-align:center;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:2px'>" + temp.Quantity + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + price + "</td> <td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>"
                    + (fromDiscount ? "<input type='text' style='width:80px;' onchange='UpdateDiscount(3)' class='cls-" + temp.ItemType + "' id='" + temp.ItemType + "-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "'  data-dis='" + temp.Discount + "' value='" + temp.Discount + "'/>" : temp.Discount) + "</td><td style='text-align:center;padding:2px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>" + temp.TaxableAmount + "</td><td style='text-align:left;border-top:solid 1px #ccc;border-right:solid 1px #ccc;padding:0;'> <table border='0' width='100%' style='font-size:7.5pt' cellpadding='0' cellspacing='0'><tbody><tr><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + temp.CGSTRate + "</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-cgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.CGSTAmount + "</span>" + "</td><td width='25%' style='border-right:1px solid #ddd;text-align:center;padding:2px;'>" + temp.SGSTRate + "</td><td width='25%' style='text-align:center;padding:2px'>" + "<Span  id='" + temp.ItemType + "-sgst-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.SGSTAmount + "</span>" + "</td></tr></tbody></table></td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'>" + "<Span  id='" + temp.ItemType + "-fa-" + (temp.ItemType == "Spare" ? temp.InvoiceSpareId : temp.InvoiceServiceId) + "' >" + temp.FinalAmount + "</span>" + "</td></tr>";

                if (temp.Type == "Header")
                {

                }
                query += Temp;
                withoutTaxPrice += WithoutTaxPrice1;
                //if (!String.IsNullOrEmpty(temp.CGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlCGSTTax += Convert.ToDecimal(temp.CGSTAmount);
                //        TOTAlCGSTTax = Convert.ToDecimal(temp.TotalCGSTAmount);
                //        TOTAlCGSTTax = Math.Round(TOTAlCGSTTax, 0);
                //    }
                //    catch (Exception ex) { }
                //}
                //if (!String.IsNullOrEmpty(temp.SGSTAmount))
                //{
                //    try
                //    {
                //        TOTAlSGSTTax += Convert.ToDecimal(temp.SGSTAmount);
                //        TOTAlSGSTTax = Convert.ToDecimal(temp.TotalSGSTAmount);
                //        TOTAlSGSTTax = Math.Round(TOTAlSGSTTax, 0);
                //    }
                //    catch (Exception ex) { }
                //}
            }
            //changes 
        }
        Decimal SubTotal = Convert.ToDecimal(Spares.TotalAmount);
        SubTotal = Math.Round(SubTotal, 2);
        Decimal FinalTotal = SubTotal - TotalDiscount;
        FinalTotal = Math.Round(FinalTotal, 2);
        query += "<tr style='font-weight:bold;'><td colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Total Amount</td><td colspan='3' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'>" + Spares.TotalAmount + "</td></tr><tr style='font-weight:bold;'><td colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Total Discount</td><td colspan='3' style='text-align:right;padding:2px;border-top:solid 1px #ccc;'> " + Spares.TotalDiscount + "</td></tr><tr style='font-weight:bold;'><td colspan='8' style='text-align:right;padding-right:5px;border-top:solid 1px #ccc;border-right:solid 1px #ccc;'>Final Amount</td><td colspan='3' style='text-align:right;padding:2px; border-top:solid 1px #ccc;'> " + Spares.FinalAmount + "</td></tr></tbody></table>";
        int finaltotal = Convert.ToInt32(FinalTotal);
        try
        {
            int jobcardinvoiceAmount = ServiceMethod.updateAmountJobCardInvoiceDetail(JobCardId, finaltotal);
        }
        catch (Exception ex)
        {
        }
        string customernotes = ServiceMethod.CustomerNotesForInvoice(JobCardId.ToString(), "2");
        if (customernotes.Contains(","))
        {
            customernotes = customernotes.Replace(",", ", ");
        }
        string substring = ServiceMethod.NumbersToWords(finaltotal);
        objectToSerilize.Name = query + "<input type='hidden' id='rpnword' value='" + substring + "'>" + "<input type='hidden' id='customernotes' value='" + customernotes + "'>";
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    //[ScriptMethod(UseHttpGet = true, ResponseFormat = ResponseFormat.Json)]
    //[System.Web.Services.Protocols.SoapHeader("SoapHeader")]
    public void GenerateHtml(String htmlData, string filename, string HtmlContent, string jobCardId, string Type = "")

    //public void GenerateHtml(string data, string jobCardId)
    {

        htmlData = HttpUtility.UrlDecode(htmlData);
        var objectToSerilize = new ServiceClass.GeneratePDF();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        if (!String.IsNullOrWhiteSpace(jobCardId) && jobCardId != "0")
        {
            string status = "";
            string FinalDownLoadViewPath = HostingEnvironment.ApplicationPhysicalPath;
            string foldername = "BarcodeImages";
            string invoicefolder = "Invoicepdfs";
            string barcodeimg_path = HostingEnvironment.ApplicationPhysicalPath + foldername;
            string invoicepdf_path = HostingEnvironment.ApplicationPhysicalPath + invoicefolder;
            string invoicepdf_path_with_filename = invoicepdf_path + "\\" + jobCardId.ToString() + ".pdf";
            try
            {
                int jobcard = 0;
                try { jobcard = Convert.ToInt32(jobCardId); }
                catch (Exception ex)
                {
                }
                //string filename1 = ServiceMethod.GetJobCardInvoiceDetail(jobcard);
                //if (File.Exists(invoicepdf_path + "\\" + filename1))
                //{
                //    objectToSerilize.resultflag = "1";
                //    objectToSerilize.Message = Constant.Message.SuccessMessage;
                //    objectToSerilize.InvoiceUrl = Constant.Message.PDFPATHFORJOBCARD + "\\" + filename1;
                //}
                //else
                //{

                #region Html Generation
                string folderPath = invoicepdf_path;
                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                //File Name 
                int fileCount = Directory.GetFiles(invoicepdf_path).Length;
                string strFileName = "JobCardId_" + jobCardId + "_" + (fileCount + 1) + ".html";
                
                //Start Pdf
                //StringReader sr = new StringReader(htmlData);

                //Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                //HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                //using (MemoryStream memoryStream = new MemoryStream())
                //{
                //    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, memoryStream);
                //    pdfDoc.Open();

                //    htmlparser.Parse(sr);
                //    pdfDoc.Close();

                //    byte[] bytes = memoryStream.ToArray();
                //    memoryStream.Close();
                //    File.WriteAllBytes(Path.Combine(invoicepdf_path, "myFile.pdf"), bytes);
                //}  
                //End PDF


                if (Type == "1")
                {
                    strFileName = "Invoice_JobCardId_" + jobCardId + "_" + (fileCount + 1) + ".html";
                    //strFileName = "Profoma_JobCardId_" + jobCardId + "_" + (fileCount + 1) + ".html";
                }
                if (Type == "2")
                {
                    strFileName = "Profoma_JobCardId_" + jobCardId + "_" + (fileCount + 1) + ".html";
                }
                if (Type == "3")
                {
                    strFileName = "JobCardId_" + jobCardId + "_" + (fileCount + 1) + ".html";
                }
                if (Type == "4")
                {
                    strFileName = "Estimate_" + jobCardId + "_" + (fileCount + 1) + ".html";
                }
                if (Type == "11")
                {
                    //strFileName = "Invoice_JobCardId_" + jobCardId + "_" + (fileCount + 1) + ".html";
                    strFileName = "Insurance_Invoice_JobCardId_" + jobCardId + "_" + (fileCount + 1) + ".html";
                }
                if (Type == "12")
                {
                    strFileName = "Insurance_Profoma_JobCardId_" + jobCardId + "_" + (fileCount + 1) + ".html";
                }
                if (Type == "14")
                {
                    strFileName = "Insurance_Estimate_" + jobCardId + "_" + (fileCount + 1) + ".html";
                }
                if (Type == "15")
                {
                    strFileName = "Insurance_Customer_Estimate_" + jobCardId + "_" + (fileCount + 1) + ".html";
                }
                if (jobcard > 0)
                {
                    //string urlAddress = "http://web.motorz.co.in/Accounts/PrintJobCard_Detail.aspx?JobCardId=" + jobcard;
                    //if (Type == "1")
                    //{
                    //    urlAddress = "http://web.motorz.co.in/Accounts/PerformaInvoice.aspx?JobCardId=" + jobcard;
                    //}
                    //if (Type == "2")
                    //{
                    //    urlAddress = "http://web.motorz.co.in/Accounts/PerformaInvoice.aspx?JobCardId=" + jobcard;
                    //}
                    //if (Type == "3")
                    //{
                    //    urlAddress = "http://web.motorz.co.in/Accounts/PrintJobCard_Detail.aspx?JobCardId=" + jobcard;
                    //}
                    //if (Type == "4")
                    //{
                    //    urlAddress = "http://web.motorz.co.in/Accounts/Estimate_Detail.aspx?JobCardId=" + jobcard;
                    //}

                    //  WebBrowser Browser = new WebBrowser();
                    ////  Browser.Url = urlAddress;
                    //  Browser.Navigate(urlAddress);



                    //string html = Browser.DocumentText;
                    //HttpWebRequest request = (HttpWebRequest)WebRequest.Create(urlAddress);

                    //HttpWebResponse response = (HttpWebResponse)request.GetResponse();

                    //if (response.StatusCode == HttpStatusCode.OK)
                    //{
                    //    Stream receiveStream = response.GetResponseStream();
                    //    StreamReader readStream = null;

                    //    if (response.CharacterSet == null)
                    //    {
                    //        readStream = new StreamReader(receiveStream);
                    //    }
                    //    else
                    //    {
                    //        readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                    //    }

                    //string data = htmlData.ToString();

                    if (!String.IsNullOrEmpty(htmlData))
                    {
                        FileStream file = new FileStream(Path.Combine(invoicepdf_path, strFileName), FileMode.Create);
                        using (StreamWriter w = new StreamWriter(file, Encoding.UTF8))
                        {
                            w.WriteLine(htmlData);
                        }
                    }

                    //    response.Close();
                    //    readStream.Close();
                    //    }
                    //
                }
                //doc.Close();

                if (jobcard > 0)
                {
                    int InvoiceNumber = 1;
                    int result = ServiceMethod.InsertJobCardInvoiceDetail(jobcard, InvoiceNumber, strFileName, Type);
                    int result1 = 0;//ServiceMethod.InsertInvoice(jobcard);
                    //if (Type == "4")
                    //{
                    //  //  result1 = ServiceMethod.insertEstimate(jobcard);
                    //}
                }

                objectToSerilize.resultflag = "1";
                objectToSerilize.Message = Constant.Message.SuccessMessage;
                // objectToSerilize.InvoiceUrl = Constant.Message.PDFPATHFORJOBCARD + "//" + strFileName;

                #endregion
                //}
            }
            catch (Exception ex)
            {
                objectToSerilize.resultflag = "0";
                objectToSerilize.Message = Constant.Message.ErrorMessage;
                objectToSerilize.InvoiceUrl = "";
            }
        }
        else
        {
            objectToSerilize.resultflag = "0";
            objectToSerilize.Message = Constant.Message.ErrorMessage;
            objectToSerilize.InvoiceUrl = "";
        }
        Context.Response.Write(js.Serialize(objectToSerilize));

    }


    //[WebMethod]
    //public void GetSpareMrp(string sid)
    //{
    //    var obj = new Objs();
    //    JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
    //    Context.Response.Clear();
    //    Context.Response.ContentType = "application/json";
    //    try
    //    {
    //        DataTable dt = dbCon.GetDataTable("select ");
    //        if (dt != null && dt.Rows.Count > 0)
    //        {
    //            obj.Name = dt.Rows[0]["Name"].ToString();
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        dbCon.InsertLogs(ErrorMsgPrefix + " GetModelByBrandId Msg:" + e.Message, "", e.StackTrace);
    //    }
    //    Context.Response.Write(js.Serialize(obj));
    //}
    #endregion

    [WebMethod]
    public void UpdateSparesTax(string SpareId, string Tax)
    {
        var objectToSerilize = new ServiceClass.Login();
        JavaScriptSerializer js = new JavaScriptSerializer();
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            int spareid = 0;
            decimal tax = 0;
            int TaxId = 0;
            try
            {
                spareid = Convert.ToInt32(SpareId);
                tax = Convert.ToDecimal(Tax);
                TaxId = ServiceMethod.TaxIdFromValue(tax);
            }
            catch (Exception ex)
            {
            }
            if (spareid > 0)
            {
                string updatequery = "update Spare set taxId=@1 where Id=@2";
                string[] param = { TaxId.ToString(), spareid.ToString() };
                int executequery = dbCon.ExecuteQueryWithParams(updatequery, param);
                if (executequery > 0)
                {
                    objectToSerilize.resultflag = "1";
                    objectToSerilize.Message = Constant.Message.UpdateSuccessFull;
                }
                else
                {
                    objectToSerilize.resultflag = "0";
                    objectToSerilize.Message = Constant.Message.UpdateFail;
                }
            }
            else
            {
                objectToSerilize.resultflag = "0";
                objectToSerilize.Message = Constant.Message.NoDataFound;
            }
        }
        catch (Exception ex)
        {
            objectToSerilize.resultflag = "0";
            objectToSerilize.Message = Constant.Message.ErrorMessage;
        }
        Context.Response.Write(js.Serialize(objectToSerilize));
    }

    [WebMethod]
    public void RemovePushToInsurance(string type, string id, string chk)
    {
        var obj = new List<Objs>();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string str = "";
            string[] strArr = { id, chk };
            if (type == "1")
            {
                str = " UPDATE [dbo].[JobCard_Service_Mapping] SET [InsuranceAmount] = 0 ,[PushToInsurance] = @2 where id=@1";
                dbCon.ExecuteQueryWithParams(str, strArr);
            }
            else
            {
                str = " UPDATE [dbo].[JobCard_Spare_Mapping] SET [InsuranceAmount] = 0 ,[PushToInsurance] = @2 where id=@1";
                dbCon.ExecuteQueryWithParams(str, strArr);
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " RemovePushToInsurance Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void SetInsuranceAmount(string type, string id, string amt)
    {
        var obj = new List<Objs>();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string str = "";
            string[] strArr = { id, amt };
            if (type == "1")
            {
                str = " UPDATE [dbo].[JobCard_Service_Mapping] SET [InsuranceAmount] = @2 where id=@1";
                dbCon.ExecuteQueryWithParams(str, strArr);
            }
            else
            {
                str = " UPDATE [dbo].[JobCard_Spare_Mapping] SET [InsuranceAmount] = @2 where id=@1";
                dbCon.ExecuteQueryWithParams(str, strArr);
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " SetInsuranceAmount Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void updateAmountAndQty(string type, string id, string amount, string qty)
    {
        var obj = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            obj.Name = "1";
            string str = "";
            string[] strArr = { id, amount, qty };
            if (type == "1")
            {
                str = " UPDATE [dbo].[JobCard_Service_Mapping] SET Amount=@2,Quantity=@3  where id=@1";
                dbCon.ExecuteQueryWithParams(str, strArr);
            }
            else
            {
                string strFilt = " ,Quantity=@3 ";
                try
                {
                    DataTable dt = dbCon.GetDataTableWithParams("select Quantity,isnull(convert(int,(select Requisition_Spare.IsAllocate from Requisition_Spare where Requisition_Spare.SpareMappingId=JobCard_Spare_Mapping.Id)),0) from JobCard_Spare_Mapping where id=@1", strArr);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][1].ToString().Equals("1"))
                        {
                            if (float.Parse(qty) < float.Parse(dt.Rows[0][0].ToString()))
                            {
                                strFilt = "";
                                obj.Name = " Requsition allocated of qty " + dt.Rows[0][0].ToString() + " So Quantity should be greater than " + dt.Rows[0][0].ToString();
                                obj.Id = dt.Rows[0][0].ToString();
                            }
                        }
                    }
                }
                catch (Exception E) { }
                str = " UPDATE [dbo].[JobCard_Spare_Mapping] SET  Amount=@2" + strFilt + "  where id=@1";
                dbCon.ExecuteQueryWithParams(str, strArr);
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " updateAmountAndQty Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void getMobileFromVehicleNumbar(string number)
    {
        var obj = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            obj.Name = "";
            string[] strarr = { number };
            DataTable dt = dbCon.GetDataTableWithParams("SELECT top 1 isnull(Customer.Mobile,'') FROM  Vehicle INNER JOIN Vehicle_Customer_Mapping ON Vehicle.Id = Vehicle_Customer_Mapping.Vehicle_Id INNER JOIN Customer ON Vehicle_Customer_Mapping.Customer_Id = Customer.Id WHERE (Vehicle.Number = @1) order by Customer_Id desc", strarr);
            if (dt != null && dt.Rows.Count > 0)
            {
                obj.Name = dt.Rows[0][0].ToString();
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " getMobileFromVehicleNumbar Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }
    [WebMethod]
    public void getAllowedByList(string keyword)
    {
        var obj = new List<Objs2>();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string[] strarr = { keyword };
            DataTable dt = dbCon.GetDataTableWithParams("SELECT distinct [allowedBy] FROM [dbo].[JobcardwithoutPayment] where allowedBy like CONCAT('%', @1, '%')", strarr);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var objsub = new Objs2();
                    objsub.Name = dt.Rows[i][0].ToString();
                    obj.Add(objsub);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " getAllowedByList Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void getJobcardType(string jobcardid)
    {
        string type = "0";
        var obj = new List<Objs4>();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string[] strarr = { jobcardid };
            DataTable dt = dbCon.GetDataTableWithParams("SELECT [Name],[Id] FROM [dbo].[JobType] where WorkshopId=1", strarr);
            if (jobcardid != "0")
            {
                try
                {
                    type = dbCon.GetDataTableWithParams("Select Type from jobcard where id=@1", strarr).Rows[0][0].ToString();
                }
                catch (Exception E) { }
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var objsub = new Objs4();
                    objsub.Name = dt.Rows[i][0].ToString();
                    objsub.Id = dt.Rows[i][1].ToString();
                    if (type != null && type != "0" && type != "")
                    {
                        if (dt.Rows[i][1].ToString().Equals(type))
                            objsub.Selected = "1";
                        else
                            objsub.Selected = "0";
                    }
                    else
                    {
                        if (i == 0)
                            objsub.Selected = "1";
                        else
                            objsub.Selected = "0";
                    }
                    obj.Add(objsub);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " getJobcardType Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void updateJobcardType(string id, string type)
    {
        var obj = new List<Objs2>();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string[] strarr = { id, type };
            dbCon.ExecuteQueryWithParams("update jobcard set type=@2 where id=@1", strarr);
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " updateJobcardType Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void getJobcardTypeGate(string jobcardid)
    {
        string type = "0";
        var obj = new List<Objs4>();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string[] strarr = { jobcardid };
            DataTable dt = dbCon.GetDataTableWithParams("SELECT [Name],[Id] FROM [dbo].[JobType] where WorkshopId=1", strarr);
            if (jobcardid != "0")
            {
                try
                {
                    type = dbCon.GetDataTableWithParams("Select JobcardTypeId from GateKeeperVehicleEntry where id=@1", strarr).Rows[0][0].ToString();
                }
                catch (Exception E) { }
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var objsub = new Objs4();
                    objsub.Name = dt.Rows[i][0].ToString();
                    objsub.Id = dt.Rows[i][1].ToString();
                    if (type != null && type != "0" && type != "")
                    {
                        if (dt.Rows[i][1].ToString().Equals(type))
                            objsub.Selected = "1";
                        else
                            objsub.Selected = "0";
                    }
                    else
                    {
                        if (i == 0)
                            objsub.Selected = "1";
                        else
                            objsub.Selected = "0";
                    }
                    obj.Add(objsub);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " getJobcardType Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void updateJobcardTypeGate(string id, string type)
    {
        var obj = new List<Objs2>();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string[] strarr = { id, type };
            dbCon.ExecuteQueryWithParams("update GateKeeperVehicleEntry set JobcardTypeId=@2 where id=@1", strarr);
            try
            {
                string WorkshopId = HttpContext.Current.Request.Cookies["TUser"]["WorkshopId"];
                string UserId = HttpContext.Current.Request.Cookies["TUser"]["Id"];
                UserActivity objUserAct = new UserActivity();
                objUserAct.InsertUserActivity("Assign jobcard Type on : " + dbCon.getindiantime().ToString("MM-dd-yyyy HH:mm:ss tt") + " type - " + type, UserId, WorkshopId, "", "Update", "Jobcard Type");
            }
            catch (Exception E) { }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " updateJobcardType Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }
    [WebMethod]
    public void GetJobcardOverrideHistory(string jobcardid)
    {
        string type = "0";
        var obj = new List<Objs4>();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string[] strarr = { jobcardid };
            DataTable dt = dbCon.GetDataTableWithParams("Select finalAmount,CONVERT(VARCHAR(12), DOC, 107)+' '+Convert(varchar,(datepart(hour,DOC)))+':'+Convert(varchar,datepart(minute,DOC)) from JobCard_Final_Amount_Change_alter where JobCardid=@1 order by id", strarr);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var objsub = new Objs4();
                    objsub.Name = dt.Rows[i][0].ToString();
                    objsub.Id = dt.Rows[i][1].ToString();
                    obj.Add(objsub);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " getJobcardType Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void insertUpdateVendorInvoiceDetail(string id, string vendorId, string invoiceAmount, string invoiceDate, string invoiceNo, string comment, string workShopId, string userId)
    {
        var obj = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string[] strArr = { id, vendorId, invoiceAmount, DateTime.ParseExact(invoiceDate, "dd-MM-yyyy", System.Globalization.CultureInfo.InvariantCulture).ToString("MM-dd-yyyy HH:mm:ss tt"), invoiceNo, comment, workShopId, userId };
            if (!String.IsNullOrWhiteSpace(id))
            {
                string str = "";
                if (id == "0")
                {
                    str = "INSERT INTO [dbo].[VendorInvoice] ([InvoiceDate],[InvoiceNumber],[InvoiceAmount],[VendorId],[DOC],[WorkshopId],[UserId],[DOM],[IsDeleted],[IsPaid],[Comment]) VALUES (@4,@5,@3,@2,'" + dbCon.getindiantime().ToString("MM-dd-yyyy HH:mm:ss tt") + "',@7,@8,'" + dbCon.getindiantime().ToString("MM-dd-yyyy HH:mm:ss tt") + "',0,0,@6) SELECT SCOPE_IDENTITY();";
                    int rid = dbCon.ExecuteScalarQueryWithParams(str, strArr);
                    if (rid > 0)
                    {
                        obj.Id = "1";
                        obj.Id1 = rid.ToString();
                        obj.Id2 = vendorId;
                        try
                        {
                            string WorkshopId = HttpContext.Current.Request.Cookies["TUser"]["WorkshopId"];
                            string UserId = HttpContext.Current.Request.Cookies["TUser"]["Id"];
                            UserActivity objUserAct = new UserActivity();
                            objUserAct.InsertUserActivity("Added Vendor Invoice on : " + dbCon.getindiantime().ToString("MM-dd-yyyy HH:mm:ss tt") + " Invoice Id - " + rid, UserId, WorkshopId, "", "Insert", "Vendor Invoice");
                        }
                        catch (Exception E) { }
                    }
                    else
                    {
                        obj.Id = "0";
                    }
                }
                else
                {
                    str = "Update [dbo].[VendorInvoice] set InvoiceDate=@4,InvoiceNumber=@5,InvoiceAmount=@3,VendorId=@2,DOM='" + dbCon.getindiantime().ToString("MM-dd-yyyy HH:mm:ss tt") + "',Comment=@6 where id=@1";
                    if (dbCon.ExecuteQueryWithParams(str, strArr) > 0)
                    {
                        obj.Id = "1";
                        obj.Id1 = id;
                        obj.Id2 = vendorId;
                        try
                        {
                            string WorkshopId = HttpContext.Current.Request.Cookies["TUser"]["WorkshopId"];
                            string UserId = HttpContext.Current.Request.Cookies["TUser"]["Id"];
                            UserActivity objUserAct = new UserActivity();
                            objUserAct.InsertUserActivity("Updated Vendor Invoice on : " + dbCon.getindiantime().ToString("MM-dd-yyyy HH:mm:ss tt") + " Invoice Id - " + id, UserId, WorkshopId, "", "Update", "Vendor Invoice");
                        }
                        catch (Exception E) { }
                    }
                    else
                    {
                        obj.Id = "0";
                    }
                }

            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " insertUpdateVendorInvoiceDetail Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void getVendorInvoiceDetailById(string id)
    {
        var obj = new vendorInvoice();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            if (!String.IsNullOrWhiteSpace(id) && id != "0")
            {
                string[] strArr = { id };
                string str = "SELECT Vendorid,[Id],case when len(CONVERT(VARCHAR, datepart(day,[InvoiceDate])))=1 then '0'+CONVERT(VARCHAR, datepart(day,[InvoiceDate])) else CONVERT(VARCHAR, datepart(day,[InvoiceDate])) end +'-'+ case when len(CONVERT(VARCHAR, datepart(month,[InvoiceDate])))=1 then '0'+CONVERT(VARCHAR, datepart(month,[InvoiceDate])) else CONVERT(VARCHAR, datepart(month,[InvoiceDate])) end +'-'+CONVERT(VARCHAR, datepart(YEAR,[InvoiceDate])) as [InvoiceDate] ,[InvoiceNumber],[InvoiceAmount],(select name from Vendor where id=Vendorid) as Vendor,Comment FROM [dbo].[VendorInvoice] where ISNULL([IsDeleted],0)=0 and id=@1";
                DataTable dt = dbCon.GetDataTableWithParams(str, strArr);
                if (dt != null && dt.Rows.Count > 0)
                {
                    obj.comment = dt.Rows[0]["comment"].ToString();
                    obj.date = dt.Rows[0]["InvoiceDate"].ToString();
                    obj.amount = dt.Rows[0]["InvoiceAmount"].ToString();
                    obj.vendor = dt.Rows[0]["Vendor"].ToString();
                    obj.number = dt.Rows[0]["InvoiceNumber"].ToString();
                    obj.vendorid = dt.Rows[0]["Vendorid"].ToString();

                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " getVendorInvoiceDetailById Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void MapInvoiceItems(string id, string val)
    {
        int cnt = 0;
        var obj = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        obj.Id = "1";
        try
        {
            if (!String.IsNullOrWhiteSpace(id) && id != "0")
            {
                string[] strItems = val.Split(new char[] { '$' });
                for (int i = 0; i < strItems.Length; i++)
                {
                    if (!String.IsNullOrWhiteSpace(strItems[i]))
                    {
                        string[] strSubItems = strItems[i].Split(new char[] { '#' });
                        if (strSubItems != null)
                        {
                            if (!String.IsNullOrWhiteSpace(strSubItems[0]) && !String.IsNullOrWhiteSpace(strSubItems[1]) && strSubItems.Length >= 2)
                            {
                                string[] strArr = { id, strSubItems[0], strSubItems[1], strSubItems[2] };
                                string str = "UPDATE [dbo].[GRNDetail] SET [VendorInvoiceId] = @1 ,VendorInvoiceComment = @4 where id=@2";
                                dbCon.ExecuteQueryWithParams(str, strArr);
                                cnt++;
                            }
                        }
                    }
                }
                obj.Name = cnt.ToString() + " Items Mapped Successfuly ..";
            }
        }
        catch (Exception e)
        {
            obj.Id = "0";
            obj.Name = "Something Went Wrong..";
            dbCon.InsertLogs(ErrorMsgPrefix + " getVendorInvoiceDetailById Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void RemoveMapInvoiceItems(string id)
    {
        int cnt = 0;
        var obj = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        obj.Id = "1";
        try
        {
            if (!String.IsNullOrWhiteSpace(id) && id != "0")
            {
                string[] strArr = { id };
                string str = "UPDATE [dbo].[GRNDetail] SET [VendorInvoiceId] = 0 ,[VendorInvoiceQty] =0 ,VendorInvoiceComment = '' where id=@1";
                dbCon.ExecuteQueryWithParams(str, strArr);
                cnt++;
                obj.Name = cnt.ToString() + " Items Remove From Mapping ..";
            }
        }
        catch (Exception e)
        {
            obj.Id = "0";
            obj.Name = "Something Went Wrong..";
            dbCon.InsertLogs(ErrorMsgPrefix + " getVendorInvoiceDetailById Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void UpdateMapInvoiceItems(string id, string val, string type)
    {
        int cnt = 0;
        var obj = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        obj.Id = "1";
        try
        {
            if (!String.IsNullOrWhiteSpace(id) && id != "0")
            {
                string[] strArr = { id, val };
                string str = "UPDATE [dbo].[GRNDetail] SET " + (type.Equals("1") ? "VendorInvoiceQty=@2" : "VendorInvoiceComment=@2") + " where id=@1";
                dbCon.ExecuteQueryWithParams(str, strArr);
                cnt++;
                obj.Name = cnt.ToString() + " Items Remove From Mapping ..";
            }
        }
        catch (Exception e)
        {
            obj.Id = "0";
            obj.Name = "Something Went Wrong..";
            dbCon.InsertLogs(ErrorMsgPrefix + " getVendorInvoiceDetailById Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void getInvoiceMappedItems(string id, string workshopid)
    {
        var obj = new List<InvoiceMappedItems>();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string Str = "SELECT GRNDetail.Id, GRNDetail.GRNId,(SELECT Code FROM Spare WHERE (Id = GRNDetail.EntityId)) + ',' + (SELECT Name FROM Spare_Brand WHERE (Id = GRNDetail.BrandId)) AS SpareName, GRNDetail.Qty, GRNDetail.Price, GRNDetail.Discount, GRN.DCNo, isnull(CONVERT(VARCHAR(12), Grn.DeliveryDate, 107),'-') as Delivery_Date, GRN.ReceivedBy,isnull(VendorInvoiceQty,0) as VendorInvoiceQty,isnull(VendorInvoiceComment,'') as VendorInvoiceComment  FROM GRNDetail INNER JOIN GRN ON GRNDetail.GRNId = GRN.Id WHERE (ISNULL(GRNDetail.IsDelete, 0) = 0)  and GRN.WorkshopId=" + workshopid + " AND ISNULL(GRNDetail.VendorInvoiceId, 0)  =" + id;
            DataTable dt = dbCon.GetDataTable(Str);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var objSub = new InvoiceMappedItems();
                    objSub.Delivery_Date = dt.Rows[i]["Delivery_Date"].ToString();
                    objSub.GRNId = dt.Rows[i]["GRNId"].ToString();
                    objSub.Id = dt.Rows[i]["Id"].ToString();
                    objSub.Price = dt.Rows[i]["Price"].ToString();
                    objSub.Qty = dt.Rows[i]["Qty"].ToString();
                    objSub.SpareName = dt.Rows[i]["SpareName"].ToString();
                    objSub.VendorInvoiceComment = dt.Rows[i]["VendorInvoiceComment"].ToString();
                    objSub.VendorInvoiceQty = dt.Rows[i]["VendorInvoiceQty"].ToString();

                    obj.Add(objSub);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " getInvoiceMappedItems Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void getCustomerreviewMasterItems(string Id)
    {
        var obj = new List<Objs4>();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string Str = "SELECT [Id] ,[Value],(Select COUNT(id) from JobCard where CustomerReviewMasterId=[CustomerReviewMaster].id and JobCard.Id=" + Id + ") as isSelected FROM [dbo].[CustomerReviewMaster]";
            DataTable dt = dbCon.GetDataTable(Str);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var objSub = new Objs4();
                    objSub.Id = dt.Rows[i][0].ToString();
                    objSub.Name = dt.Rows[i][1].ToString();
                    objSub.Selected = dt.Rows[i][2].ToString();
                    obj.Add(objSub);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " getInvoiceMappedItems Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void getCustomerreviewMasterItems1(string Id)
    {
        var obj = new List<Objs4>();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {

            string Str = "SELECT [Id] ,[Value],0 as isSelected FROM [dbo].[CustomerReviewMaster]";
            DataTable dt = dbCon.GetDataTable(Str);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var objSub = new Objs4();
                    objSub.Id = dt.Rows[i][0].ToString();
                    objSub.Name = dt.Rows[i][1].ToString();
                    objSub.Selected = dt.Rows[i][2].ToString();
                    obj.Add(objSub);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " getInvoiceMappedItems Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void updateReview(string id, string chkid, string cnt, string dt)
    {
        var obj = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string[] strArr = { id, chkid, cnt, dt };
            string Str = "UPDATE [dbo].[JobCard] SET " + (!String.IsNullOrWhiteSpace(dt) && chkid == "2" ? "[CustomerNextReviewDate] =@4 ," : "") + "[CustomerReviewComment] =@3 ,[CustomerReviewMasterId] =@2  ,[CustomerReviewDate] ='" + dbCon.getindiantime().ToString("dd/MMM/yyyy hh:mm:ss") + "'  WHERE id=@1";
            if (dbCon.ExecuteQueryWithParams(Str, strArr) > 0)
            {
                dbCon.ExecuteQueryWithParams("INSERT INTO [dbo].[CustomerReviewCallHistory]([DOC],[Comment],JobcardId,CustomerReviewMasterId) VALUES('" + dbCon.getindiantime().ToString("dd/MMM/yyyy hh:mm:ss") + "',@3,@1,@2)", strArr);
                obj.Id = "1";
            }
            else
                obj.Id = "0";
            try
            {
                string WorkshopId = HttpContext.Current.Request.Cookies["TUser"]["WorkshopId"];
                string UserId = HttpContext.Current.Request.Cookies["TUser"]["Id"];
                UserActivity objUserAct = new UserActivity();
                objUserAct.InsertUserActivity("Added Customer Review on : " + dbCon.getindiantime().ToString("dd/MMM/yyyy hh:mm:ss") + " Againts JobcardId : " + id + " ", UserId, WorkshopId, "", "Insert", "Jobcard Review");
            }
            catch (Exception E) { }
        }
        catch (Exception e)
        {
            obj.Id = "0";
            dbCon.InsertLogs(ErrorMsgPrefix + " updateReview Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }
    [WebMethod]
    public void OpenJobcard(string Id, string UserId, string WorkshopId)
    {
        var obj = new Objs2();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string Str = "SELECT [VehicleId],[DOC],[UserId],Isnull([JobcardTypeId],0) as [JobcardTypeId] ,isnull([JobcardId],0) as [JobcardId],isnull((SELECT top 1 [Vehicle_Customer_Mapping].[Customer_Id] FROM [dbo].[Vehicle_Customer_Mapping] where [Vehicle_Customer_Mapping].Vehicle_Id=GateKeeperVehicleEntry.VehicleId order by id desc),0) as CustomerId FROM [dbo].[GateKeeperVehicleEntry] where [Id]=" + Id + " And ISNULL([IsDeleted] ,0)=0";
            DataTable dt = dbCon.GetDataTable(Str);
            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["JobcardTypeId"].ToString() != "0" && dt.Rows[0]["JobcardId"].ToString() == "0")
                {
                    DateTime dtm = DateTime.Parse(dt.Rows[0]["DOC"].ToString());
                    int JID = dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[JobCard]([Vehicle_Id],[Customer_Id],[Type],[JobStatus_Id],[IsDelete],[DOC],[DOM],[UserId],[WorkshopId],[HavingInsurance],[InsuranceInvoiceAmount],[CustomerInvoiceAmount]) VALUES (" + dt.Rows[0]["VehicleId"].ToString() + "," + dt.Rows[0]["CustomerId"].ToString() + "," + dt.Rows[0]["JobcardTypeId"].ToString() + ",2,0,'" + dtm.ToString("MM-dd-yyyy HH:mm:ss") + "','" + dbCon.getindiantime().ToString("MM-dd-yyyy HH:mm:ss") + "'," + UserId + "," + WorkshopId + ",0,0,0)  select SCOPE_IDENTITY();", new string[] { });
                    if (JID > 0)
                    {
                        if (dbCon.ExecuteQuery("Update GateKeeperVehicleEntry set JobcardId=" + JID + " Where Id=" + Id) > 0)
                        {
                            obj.Id = "1";
                            obj.Name = "Jobcard Is Successfully Open You Will Redirect To The Jobcard Page...Please Wait...";
                            obj.Id1 = JID.ToString();
                            try
                            {
                                UserActivity objUserAct = new UserActivity();
                                objUserAct.InsertUserActivity("Added New Jobcard On : " + dbCon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss tt") + " JobcardId : " + JID, UserId, WorkshopId, "", "Insert", "Jobcard");
                            }
                            catch (Exception E) { }
                        }
                        else
                        {
                            obj.Id = "0";
                            obj.Name = "Error Msg: Problem In Update JobcardId.";
                        }
                    }
                    else
                    {
                        obj.Id = "0";
                        obj.Name = "Error Msg: Problem In Insert.";
                    }
                }
                else if (dt.Rows[0]["JobcardId"].ToString() == "0")
                {
                    obj.Id = "1";
                    obj.Name = "Error Msg: Jobcard Is Already Opened for This Vehicle. JobCard #  " + dt.Rows[0]["JobcardId"].ToString() + ". You Will Redirect To The Jobcard Page...Please Wait...";
                    obj.Id1 = dt.Rows[0]["JobcardId"].ToString();
                }
                else
                {
                    obj.Id = "0";
                    obj.Name = "Error Msg: JobcardType Not Assigned Or Jobcard Is Already Opened for This Vehicle.";
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " OpenJobcard Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }

    [WebMethod]
    public void getRecommendation(string Id)
    {
        var obj = new List<Objs4>();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            string Str = "SELECT [Id] ,[Recommendation] FROM [dbo].[JobcardRecommendation] where isnull(IsDeleted,0)=0 and JobcardId=" + Id;
            DataTable dt = dbCon.GetDataTable(Str);
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var objSub = new Objs4();
                    objSub.Id = dt.Rows[i][0].ToString();
                    objSub.Name = dt.Rows[i][1].ToString();
                    obj.Add(objSub);
                }
            }
        }
        catch (Exception e)
        {
            dbCon.InsertLogs(ErrorMsgPrefix + " getRecommendation Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }
    [WebMethod]
    public void AddRecommendation(string Id, string Val, string JobcardId)
    {
        var obj = new Objs4();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            if (Id.Equals("0"))
            {
                if (dbCon.ExecuteQuery("INSERT INTO [dbo].[JobcardRecommendation] ([JobcardId],[Recommendation],[DOC]) VALUES ('" + JobcardId + "','" + Val + "','" + dbCon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss tt") + "')") > 0)
                {
                    obj.Id = "1";
                    try
                    {
                        string WorkshopId = HttpContext.Current.Request.Cookies["TUser"]["WorkshopId"];
                        string UserId = HttpContext.Current.Request.Cookies["TUser"]["Id"];
                        UserActivity objUserAct = new UserActivity();
                        objUserAct.InsertUserActivity("Added Recommendation on : " + dbCon.getindiantime().ToString("dd/MMM/yyyy hh:mm:ss") + " Againts JobcardId : " + JobcardId + " ", UserId, WorkshopId, "", "Insert", "Jobcard Recommendation");
                    }
                    catch (Exception E) { }
                }
                else
                {
                    obj.Id = "0";
                    obj.Name = "Data Not Inserted";
                }
            }
            else
            {
                if (dbCon.ExecuteQuery("update [dbo].[JobcardRecommendation] Set [Recommendation]='" + Val + "' where Id=" + Id) > 0)
                {
                    obj.Id = "1";
                    try
                    {
                        string WorkshopId = HttpContext.Current.Request.Cookies["TUser"]["WorkshopId"];
                        string UserId = HttpContext.Current.Request.Cookies["TUser"]["Id"];
                        UserActivity objUserAct = new UserActivity();
                        objUserAct.InsertUserActivity("Updated Recommendation on : " + dbCon.getindiantime().ToString("dd/MMM/yyyy hh:mm:ss") + " Againts JobcardId : " + JobcardId + " ", UserId, WorkshopId, "", "Update", "Jobcard Recommendation");
                    }
                    catch (Exception E) { }
                }
                else
                {
                    obj.Id = "0";
                    obj.Name = "Data Not Updated";
                }
            }

        }
        catch (Exception e)
        {
            obj.Id = "0";
            obj.Name = e.Message;
            dbCon.InsertLogs(ErrorMsgPrefix + " getRecommendation Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }
    [WebMethod]
    public void RemoveRecommendation(string Id)
    {
        var obj = new Objs4();
        JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = Int32.MaxValue };
        Context.Response.Clear();
        Context.Response.ContentType = "application/json";
        try
        {
            if (dbCon.ExecuteQuery("update [dbo].[JobcardRecommendation] Set [IsDeleted]='1' where Id=" + Id) > 0)
            {
                obj.Id = "1";
                try
                {
                    string WorkshopId = HttpContext.Current.Request.Cookies["TUser"]["WorkshopId"];
                    string UserId = HttpContext.Current.Request.Cookies["TUser"]["Id"];
                    UserActivity objUserAct = new UserActivity();
                    objUserAct.InsertUserActivity("Remove Recommendation on : " + dbCon.getindiantime().ToString("dd/MMM/yyyy hh:mm:ss") + " Againts Recommendation Id : " + Id + " ", UserId, WorkshopId, "", "Remove", "Jobcard Recommendation");
                }
                catch (Exception E) { }
            }
            else
            {
                obj.Id = "0";
                obj.Name = "Data Not Updated";
            }
        }
        catch (Exception e)
        {
            obj.Id = "0";
            obj.Name = e.Message;
            dbCon.InsertLogs(ErrorMsgPrefix + " RemoveRecommendation Msg:" + e.Message, "", e.StackTrace);
        }
        Context.Response.Write(js.Serialize(obj));
    }
    public class InvoiceMappedItems
    {
        public string Id { get; set; }
        public string Delivery_Date { get; set; }
        public string SpareName { get; set; }
        public string VendorInvoiceQty { get; set; }
        public string VendorInvoiceComment { get; set; }
        public string GRNId { get; set; }
        public string Price { get; set; }
        public string Qty { get; set; }
    }
}
