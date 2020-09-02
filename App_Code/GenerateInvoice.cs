using MotorzService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebApplication1;

/// <summary>
/// Summary description for GenerateInvoice
/// </summary>
public class GenerateInvoice
{
	public GenerateInvoice()
	{
		//
		// TODO: Add constructor logic here
		//
	}
    public bool generateInvoiceFromEstimate(string jobCardId,int type=1)
    {
        try
        {
            dbConnection dbcon = new dbConnection();
            DataTable dt=dbcon.GetDataTable("Select id from Invoice where JobCardId=" + jobCardId);
            if (dt.Rows.Count == 0)
            {
                String str = "Insert INTO [dbo].[Invoice] ([JobCardId],[TotalSpareAmount],[TotalServiceAmount],[TotalSGST],[TotalCGST],[TotalDiscount],[FinalTotal],[WorkshopId],[DOC],[DOM]) ( SELECT top 1 [JobCardId],[TotalSpareAmount],[TotalServiceAmount],[TotalSGST],[TotalCGST],[TotalDiscount],[FinalTotal],[WorkshopId],getdate(),getdate() FROM [dbo].[Estimate] where JobCardId=" + jobCardId + "  and id=(select max(id) from Estimate where JobCardId=" + jobCardId + ")) ";
                dbcon.ExecuteQuery(str);
                str = "Select top 1 id from invoice where jobcardid=" + jobCardId + " Order by id desc";
                string id = dbcon.GetDataTable(str).Rows[0][0].ToString();
                str = "INSERT INTO [dbo].[Invoice_Spare_Mapping] ([InvoiceId],[SpareId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId]) ( select " + id + ",[SpareId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId] from Estimate_Spare_Mapping where Isnull(IsDeleted,0) =0 and  Estimateid=(select max(id) from Estimate where JobCardId=" + jobCardId + "))";
                dbcon.ExecuteQuery(str);

                str = "INSERT INTO [dbo].[Invoice_Service_Mapping] ([InvoiceId],[ServiceId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId])(Select " + id + ",[ServiceId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId] from  Estimate_Service_Mapping where Isnull(IsDeleted,0) =0 and   Estimateid=(select max(id) from Estimate where JobCardId=" + jobCardId + "))";
                dbcon.ExecuteQuery(str);
            }
            else
            {
                string strqry = "SELECT top 1 [JobCardId],[TotalSpareAmount],[TotalServiceAmount],[TotalSGST],[TotalCGST],[TotalDiscount],[FinalTotal],[WorkshopId],getdate(),getdate() FROM [dbo].[Estimate] where JobCardId=" + jobCardId + "  and id=(select max(id) from Estimate where JobCardId=" + jobCardId + ")";
                DataTable dt1=dbcon.GetDataTable(strqry);
                if (dt1 != null && dt1.Rows.Count > 0)
                {
                    String str = "Update [Invoice] set [TotalSpareAmount]='" + dt1.Rows[0]["TotalSpareAmount"].ToString() + "',[TotalServiceAmount]='" + dt1.Rows[0]["TotalServiceAmount"].ToString() + "',[TotalSGST]='" + dt1.Rows[0]["TotalSGST"].ToString() + "',[TotalCGST]='" + dt1.Rows[0]["TotalCGST"].ToString() + "',[TotalDiscount]='" + dt1.Rows[0]["TotalDiscount"].ToString() + "',[FinalTotal]='" + dt1.Rows[0]["FinalTotal"].ToString() + "' where id=" + dt.Rows[0][0].ToString();
                    dbcon.ExecuteQuery(str);
                    string id = dt.Rows[0][0].ToString();

                    dbcon.ExecuteQuery("delete from Invoice_Spare_Mapping where invoiceid="+id);
                    str = "INSERT INTO [dbo].[Invoice_Spare_Mapping] ([InvoiceId],[SpareId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId]) ( select " + id + ",[SpareId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId] from Estimate_Spare_Mapping where Isnull(IsDeleted,0) =0 and  Estimateid=(select max(id) from Estimate where JobCardId=" + jobCardId + "))";
                    dbcon.ExecuteQuery(str);

                    dbcon.ExecuteQuery("delete from Invoice_Service_Mapping where invoiceid=" + id);
                    str = "INSERT INTO [dbo].[Invoice_Service_Mapping] ([InvoiceId],[ServiceId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId])(Select " + id + ",[ServiceId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId] from  Estimate_Service_Mapping where Isnull(IsDeleted,0) =0 and   Estimateid=(select max(id) from Estimate where JobCardId=" + jobCardId + "))";
                    dbcon.ExecuteQuery(str);
                }
            }
            return true;
        }
        catch (Exception E)
        {
            
        }
        return false;
    }
    public bool generateInvoiceFromEstimate_V1(string jobCardId, int type = 1)
    {
        try
        {
            decimal CGSTTax_14 = 0;
            decimal IGSTTax_14 = 0;
            decimal CGSTTaxable_14 = 0;
            decimal CGSTTax_9 = 0;
            decimal IGSTTax_9 = 0;
            decimal CGSTTaxable_9 = 0;
            dbConnection dbcon = new dbConnection();
            string GstNumber = "";
            bool isIgst = false;
            try
            {
                GstNumber = dbcon.GetDataTable("Select isnull((Select Gst_No from Customer where id=Customer_Id),'') from jobcard where id=" + jobCardId).Rows[0][0].ToString();
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
            String str = "";
            
            ServiceMethods ServiceMethod = new ServiceMethods();
            var Spares = ServiceMethod.GetJobCardSpareInJobCardForInvoiceV3_new(jobCardId, "", "", type,false,true);
            try
            {
                foreach (var temp in Spares.Spares)
                {
                    if (temp.Type != "Header")
                    {
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
                    }
                }
                foreach (var temp in Spares.Services)
                {
                    if (temp.Type != "Header")
                    {
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
                    }
                }
                if (type == 1)
                {
                    DataTable dtextra = dbcon.GetDataTable("SELECT isnull([CompusaryExcess],0) as [CompusaryExcess] ,isnull([VoluntaryExcess],0) as [VoluntaryExcess] ,isnull([Salvage],0) as [Salvage] FROM [dbo].[JobCard_Insurance_Mapping] where JobCardId=" + jobCardId);
                    if (dtextra != null && dtextra.Rows.Count > 0)
                    {
                        if (Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString()) > 0)
                        {
                            Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString())).ToString();
                        }
                        if (Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString()) > 0)
                        {
                            Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString())).ToString();
                        }
                        if (Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString()) > 0)
                        {
                            Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString())).ToString();
                        }
                    }
                }

                if (type == 2)
                {
                    DataTable dtextra = dbcon.GetDataTable("SELECT isnull([CompusaryExcess],0) as [CompusaryExcess] ,isnull([VoluntaryExcess],0) as [VoluntaryExcess] ,isnull([Salvage],0) as [Salvage] FROM [dbo].[JobCard_Insurance_Mapping] where JobCardId=" + jobCardId);
                    if (dtextra != null && dtextra.Rows.Count > 0)
                    {
                        if (Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString()) > 0)
                        {
                            Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString())).ToString();
                        }
                        if (Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString()) > 0)
                        {
                            Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString())).ToString();
                        }
                        if (Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString()) > 0)
                        {
                            Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString())).ToString();
                        }
                    }
                }
            }
            catch (Exception E) { }
            if (isIgst)
            {
                IGSTTax_14=(CGSTTax_14*2);
                CGSTTax_14 = 0;
                IGSTTax_9 = (CGSTTax_9 * 2);
                CGSTTax_9 = 0;
            }
            DataTable dt = dbcon.GetDataTable("Select id from Invoice where JobCardId=" + jobCardId+" And type="+type );
            if (dt.Rows.Count == 0)
            {
                str = "INSERT INTO [dbo].[Invoice] (Id,[JobCardId] ,[TotalSpareAmount],[TotalServiceAmount],[TotalSGST],[TotalCGST],[TotalDiscount],[FinalTotal],[WorkshopId],[DOC],[DOM],[Type],[IncludeInGst],[Taxable18],[SGST18],[CGST18],[IGST18],[Taxable28],[SGST28],[CGST28],[IGST28]) VALUES ((Select max(Id)+1 from invoice) ," + jobCardId + " ," + Spares.SubTotalForSpare + "," + Spares.SubTotalForService + "," + Spares.FinalTotalCGSTAmount + "," + Spares.FinalTotalCGSTAmount + "," + Spares.TotalDiscount + "," + Spares.FinalAmount + ",1,'" + dbcon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss tt") + "','" + dbcon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss tt") + "'," + type + ",1," + CGSTTaxable_9 + "," + CGSTTax_9 + "," + CGSTTax_9 + "," + IGSTTax_9 + "," + CGSTTaxable_14 + "," + CGSTTax_14 + "," + CGSTTax_14 + "," + IGSTTax_14 + ")";
                dbcon.ExecuteQuery(str);
                str = "Select top 1 id from invoice where jobcardid=" + jobCardId + " and type=" + type + " Order by id desc";
                string id = dbcon.GetDataTable(str).Rows[0][0].ToString();
                for (int i = 1; i < Spares.Services.Count - 1; i++)
                {
                    str = "INSERT INTO [dbo].[Invoice_Service_Mapping] ([InvoiceId],[ServiceId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId]) VALUES (" + id + "," + Spares.Services[i].Id + "," + Spares.Services[i].CGSTRate + "," + Spares.Services[i].CGSTAmount + "," + Spares.Services[i].SGSTRate + "," + Spares.Services[i].SGSTAmount + "," + (type == 1 ? Spares.Services[i].Discount : "0") + "," + Spares.Services[i].Price + "," + Spares.Services[i].Quantity + "," + (type == 1 ? Spares.Services[i].TotalDiscount : "0") + "," + Spares.Services[i].FinalAmount + ",'" + dbcon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss tt") + "',1," + Spares.Services[i].MappingId + ")";
                    dbcon.ExecuteQuery(str);
                }
                for (int i = 1; i < Spares.Spares.Count - 1; i++)
                {
                    str = "INSERT INTO [dbo].[Invoice_Spare_Mapping] ([InvoiceId],[SpareId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId]) VALUES (" + id + "," + Spares.Spares[i].Id + "," + Spares.Spares[i].CGSTRate + "," + Spares.Spares[i].CGSTAmount + "," + Spares.Spares[i].SGSTRate + "," + Spares.Spares[i].SGSTAmount + "," + (type == 1 ? Spares.Spares[i].Discount : "0") + "," + Spares.Spares[i].Price + "," + Spares.Spares[i].Quantity + "," + (type == 1 ? Spares.Spares[i].TotalDiscount : "0") + "," + Spares.Spares[i].FinalAmount + ",'" + dbcon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss tt") + "',1," + Spares.Spares[i].MappingId + ")";
                    dbcon.ExecuteQuery(str);
                }
                try
                {
                    string WorkshopId = HttpContext.Current.Request.Cookies["TUser"]["WorkshopId"];
                    string UserId = HttpContext.Current.Request.Cookies["TUser"]["Id"];
                    UserActivity objUserAct = new UserActivity();
                    objUserAct.InsertUserActivity("Generate New Invoice for " + (type == 1 ? "Customer" : "Insurance") + " Invoice No. : " + id, UserId, WorkshopId, "", "Generate", "Invoice");
                }
                catch (Exception E) { }
            }
            else
            {
               // if (dbcon.GetDataTable("SELECT id FROM [dbo].[JobCard] where isnull([IsGatePassGenerated],0)=0 and Id="+jobCardId).Rows.Count>0)
                {
                    string id = dt.Rows[0][0].ToString();
                    str = "";
                    str = "Update [Invoice] set [TotalSGST]='" + Spares.FinalTotalCGSTAmount + "',[TotalCGST]='" + Spares.FinalTotalCGSTAmount + "',[FinalTotal]='" + Spares.FinalAmount + "',[Taxable18]=" + CGSTTaxable_9 + ",[SGST18]=" + CGSTTax_9 + ",[CGST18]=" + CGSTTax_9 + ",[IGST18]=" + IGSTTax_9 + ",[Taxable28]=" + CGSTTaxable_14 + ",[SGST28]=" + CGSTTax_14 + ",[CGST28]=" + CGSTTax_14 + ",[IGST28]=" + IGSTTax_14 + " where id=" + id;
                    dbcon.ExecuteQuery(str);
                    dbcon.ExecuteQuery("delete from Invoice_Service_Mapping where invoiceid=" + id);
                    for (int i = 1; i < Spares.Services.Count - 1; i++)
                    {
                        str = "INSERT INTO [dbo].[Invoice_Service_Mapping] ([InvoiceId],[ServiceId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId]) VALUES (" + id + "," + Spares.Services[i].Id + "," + Spares.Services[i].CGSTRate + "," + Spares.Services[i].CGSTAmount + "," + Spares.Services[i].SGSTRate + "," + Spares.Services[i].SGSTAmount + "," + (type == 1 ? Spares.Services[i].Discount : "0") + "," + Spares.Services[i].Price + "," + Spares.Services[i].Quantity + "," + (type == 1 ? Spares.Services[i].TotalDiscount : "0") + "," + Spares.Services[i].FinalAmount + ",'" + dbcon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss tt") + "',1," + Spares.Services[i].MappingId + ")";
                        dbcon.ExecuteQuery(str);
                    }
                    dbcon.ExecuteQuery("delete from Invoice_Spare_Mapping where invoiceid=" + id);
                    for (int i = 1; i < Spares.Spares.Count - 1; i++)
                    {
                        str = "INSERT INTO [dbo].[Invoice_Spare_Mapping] ([InvoiceId],[SpareId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId]) VALUES (" + id + "," + Spares.Spares[i].Id + "," + Spares.Spares[i].CGSTRate + "," + Spares.Spares[i].CGSTAmount + "," + Spares.Spares[i].SGSTRate + "," + Spares.Spares[i].SGSTAmount + "," + (type == 1 ? Spares.Spares[i].Discount : "0") + "," + Spares.Spares[i].Price + "," + Spares.Spares[i].Quantity + "," + (type == 1 ? Spares.Spares[i].TotalDiscount : "0") + "," + Spares.Spares[i].FinalAmount + ",'" + dbcon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss tt") + "',1," + Spares.Spares[i].MappingId + ")";
                        dbcon.ExecuteQuery(str);
                    }
                    try
                    {
                        string WorkshopId = HttpContext.Current.Request.Cookies["TUser"]["WorkshopId"];
                        string UserId = HttpContext.Current.Request.Cookies["TUser"]["Id"];
                        UserActivity objUserAct = new UserActivity();
                        objUserAct.InsertUserActivity("Update Invoice for " + (type == 1 ? "Customer" : "Insurance") + " Invoice No. : " + id, UserId, WorkshopId, "", "Update", "Invoice");
                    }
                    catch (Exception E) { }
                }
            }
            return true;
        }
        catch (Exception E)
        {

        }
        return false;
    }

    public bool generateInvoiceFromEstimate_V1_V1(string jobCardId, int type = 1)
    {
        try
        {
            decimal CGSTTax_14 = 0;
            decimal IGSTTax_14 = 0;
            decimal CGSTTaxable_14 = 0;
            decimal CGSTTax_9 = 0;
            decimal IGSTTax_9 = 0;
            decimal CGSTTaxable_9 = 0;
            dbConnection dbcon = new dbConnection();
            string GstNumber = "";
            bool isIgst = false;
            try
            {
                GstNumber = dbcon.GetDataTable("Select isnull((Select Gst_No from Customer where id=Customer_Id),'') from jobcard where id=" + jobCardId).Rows[0][0].ToString();
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
            String str = "";

            ServiceMethods ServiceMethod = new ServiceMethods();
            var Spares = ServiceMethod.GetJobCardSpareInJobCardForInvoiceV3_new_V1(jobCardId, "", "", type, false, true);
            try
            {
                foreach (var temp in Spares.Spares)
                {
                    if (temp.Type != "Header")
                    {
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
                    }
                }
                foreach (var temp in Spares.Services)
                {
                    if (temp.Type != "Header")
                    {
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
                    }
                }
                if (type == 1)
                {
                    DataTable dtextra = dbcon.GetDataTable("SELECT isnull([CompusaryExcess],0) as [CompusaryExcess] ,isnull([VoluntaryExcess],0) as [VoluntaryExcess] ,isnull([Salvage],0) as [Salvage] FROM [dbo].[JobCard_Insurance_Mapping] where JobCardId=" + jobCardId);
                    if (dtextra != null && dtextra.Rows.Count > 0)
                    {
                        if (Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString()) > 0)
                        {
                            Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString())).ToString();
                        }
                        if (Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString()) > 0)
                        {
                            Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString())).ToString();
                        }
                        if (Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString()) > 0)
                        {
                            Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString())).ToString();
                        }
                    }
                }

                if (type == 2)
                {
                    DataTable dtextra = dbcon.GetDataTable("SELECT isnull([CompusaryExcess],0) as [CompusaryExcess] ,isnull([VoluntaryExcess],0) as [VoluntaryExcess] ,isnull([Salvage],0) as [Salvage] FROM [dbo].[JobCard_Insurance_Mapping] where JobCardId=" + jobCardId);
                    if (dtextra != null && dtextra.Rows.Count > 0)
                    {
                        if (Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString()) > 0)
                        {
                            Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString())).ToString();
                        }
                        if (Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString()) > 0)
                        {
                            Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString())).ToString();
                        }
                        if (Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString()) > 0)
                        {
                            Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString())).ToString();
                        }
                    }
                }
            }
            catch (Exception E) { }
            if (isIgst)
            {
                IGSTTax_14 = (CGSTTax_14 * 2);
                CGSTTax_14 = 0;
                IGSTTax_9 = (CGSTTax_9 * 2);
                CGSTTax_9 = 0;
            }
            DataTable dt = dbcon.GetDataTable("Select id,GstInvoiceNumber from Invoice where isnull(IsCancelled,0)=0 and JobCardId=" + jobCardId + " And type=" + type);
            if (dt!=null && dt.Rows.Count > 0)
            {
                string id = dt.Rows[0][0].ToString();
                
                str = "Update [Invoice] set [TotalSGST]='" + Spares.FinalTotalCGSTAmount + "',[TotalCGST]='" + Spares.FinalTotalCGSTAmount + "',[FinalTotal]='" + Spares.FinalAmount + "',[Taxable18]=" + CGSTTaxable_9 + ",[SGST18]=" + CGSTTax_9 + ",[CGST18]=" + CGSTTax_9 + ",[IGST18]=" + IGSTTax_9 + ",[Taxable28]=" + CGSTTaxable_14 + ",[SGST28]=" + CGSTTax_14 + ",[CGST28]=" + CGSTTax_14 + ",[IGST28]=" + IGSTTax_14 + " where id=" + id;
                dbcon.ExecuteQuery(str);
                string GstInvoiceNumber = dt.Rows[0]["GstInvoiceNumber"].ToString();

                try
                {
                    string WorkshopId = HttpContext.Current.Request.Cookies["TUser"]["WorkshopId"];
                    string UserId = HttpContext.Current.Request.Cookies["TUser"]["Id"];
                    UserActivity objUserAct = new UserActivity();
                    objUserAct.InsertUserActivity("Print Invoice for " + (type == 1 ? "Customer" : "Insurance") + " Invoice No. : " + GstInvoiceNumber+" , Jobcard No. -"+jobCardId, UserId, WorkshopId, "", "Update", "Invoice");
                }
                catch (Exception E) { }

                //str = "INSERT INTO [dbo].[Invoice] (Id,[JobCardId] ,[TotalSpareAmount],[TotalServiceAmount],[TotalSGST],[TotalCGST],[TotalDiscount],[FinalTotal],[WorkshopId],[DOC],[DOM],[Type],[IncludeInGst],[Taxable18],[SGST18],[CGST18],[IGST18],[Taxable28],[SGST28],[CGST28],[IGST28]) VALUES ((Select max(Id)+1 from invoice) ," + jobCardId + " ," + Spares.SubTotalForSpare + "," + Spares.SubTotalForService + "," + Spares.FinalTotalCGSTAmount + "," + Spares.FinalTotalCGSTAmount + "," + Spares.TotalDiscount + "," + Spares.FinalAmount + ",1,'" + dbcon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss tt") + "','" + dbcon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss tt") + "'," + type + ",1," + CGSTTaxable_9 + "," + CGSTTax_9 + "," + CGSTTax_9 + "," + IGSTTax_9 + "," + CGSTTaxable_14 + "," + CGSTTax_14 + "," + CGSTTax_14 + "," + IGSTTax_14 + ")";
                //dbcon.ExecuteQuery(str);
                //str = "Select top 1 id from invoice where jobcardid=" + jobCardId + " and type=" + type + " Order by id desc";
                //string id = dbcon.GetDataTable(str).Rows[0][0].ToString();
                //for (int i = 1; i < Spares.Services.Count - 1; i++)
                //{
                //    str = "INSERT INTO [dbo].[Invoice_Service_Mapping] ([InvoiceId],[ServiceId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId]) VALUES (" + id + "," + Spares.Services[i].Id + "," + Spares.Services[i].CGSTRate + "," + Spares.Services[i].CGSTAmount + "," + Spares.Services[i].SGSTRate + "," + Spares.Services[i].SGSTAmount + "," + (type == 1 ? Spares.Services[i].Discount : "0") + "," + Spares.Services[i].Price + "," + Spares.Services[i].Quantity + "," + (type == 1 ? Spares.Services[i].TotalDiscount : "0") + "," + Spares.Services[i].FinalAmount + ",'" + dbcon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss tt") + "',1," + Spares.Services[i].MappingId + ")";
                //    dbcon.ExecuteQuery(str);
                //}
                //for (int i = 1; i < Spares.Spares.Count - 1; i++)
                //{
                //    str = "INSERT INTO [dbo].[Invoice_Spare_Mapping] ([InvoiceId],[SpareId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId]) VALUES (" + id + "," + Spares.Spares[i].Id + "," + Spares.Spares[i].CGSTRate + "," + Spares.Spares[i].CGSTAmount + "," + Spares.Spares[i].SGSTRate + "," + Spares.Spares[i].SGSTAmount + "," + (type == 1 ? Spares.Spares[i].Discount : "0") + "," + Spares.Spares[i].Price + "," + Spares.Spares[i].Quantity + "," + (type == 1 ? Spares.Spares[i].TotalDiscount : "0") + "," + Spares.Spares[i].FinalAmount + ",'" + dbcon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss tt") + "',1," + Spares.Spares[i].MappingId + ")";
                //    dbcon.ExecuteQuery(str);
                //}
                //try
                //{
                //    string WorkshopId = HttpContext.Current.Request.Cookies["TUser"]["WorkshopId"];
                //    string UserId = HttpContext.Current.Request.Cookies["TUser"]["Id"];
                //    UserActivity objUserAct = new UserActivity();
                //    objUserAct.InsertUserActivity("Generate New Invoice for " + (type == 1 ? "Customer" : "Insurance") + " Invoice No. : " + id, UserId, WorkshopId, "", "Generate", "Invoice");
                //}
                //catch (Exception E) { }
            }
            //else
            //{
            //    string id = dt.Rows[0][0].ToString();
            //    if (dbcon.GetDataTable("SELECT id FROM [dbo].[JobCard] where isnull([IsGatePassGenerated],0)=0 and Id=" + jobCardId).Rows.Count > 0)
            //    {
            //        str = "Update [Invoice] set [TotalSGST]='" + Spares.FinalTotalCGSTAmount + "',[TotalCGST]='" + Spares.FinalTotalCGSTAmount + "',[FinalTotal]='" + Spares.FinalAmount + "',[Taxable18]=" + CGSTTaxable_9 + ",[SGST18]=" + CGSTTax_9 + ",[CGST18]=" + CGSTTax_9 + ",[IGST18]=" + IGSTTax_9 + ",[Taxable28]=" + CGSTTaxable_14 + ",[SGST28]=" + CGSTTax_14 + ",[CGST28]=" + CGSTTax_14 + ",[IGST28]=" + IGSTTax_14 + " where id=" + id;
            //        dbcon.ExecuteQuery(str);
                   
            //        //dbcon.ExecuteQuery("delete from Invoice_Service_Mapping where invoiceid=" + id);
            //        //for (int i = 1; i < Spares.Services.Count - 1; i++)
            //        //{
            //        //    str = "INSERT INTO [dbo].[Invoice_Service_Mapping] ([InvoiceId],[ServiceId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId]) VALUES (" + id + "," + Spares.Services[i].Id + "," + Spares.Services[i].CGSTRate + "," + Spares.Services[i].CGSTAmount + "," + Spares.Services[i].SGSTRate + "," + Spares.Services[i].SGSTAmount + "," + (type == 1 ? Spares.Services[i].Discount : "0") + "," + Spares.Services[i].Price + "," + Spares.Services[i].Quantity + "," + (type == 1 ? Spares.Services[i].TotalDiscount : "0") + "," + Spares.Services[i].FinalAmount + ",'" + dbcon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss tt") + "',1," + Spares.Services[i].MappingId + ")";
            //        //    dbcon.ExecuteQuery(str);
            //        //}
            //        //dbcon.ExecuteQuery("delete from Invoice_Spare_Mapping where invoiceid=" + id);
            //        //for (int i = 1; i < Spares.Spares.Count - 1; i++)
            //        //{
            //        //    str = "INSERT INTO [dbo].[Invoice_Spare_Mapping] ([InvoiceId],[SpareId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId]) VALUES (" + id + "," + Spares.Spares[i].Id + "," + Spares.Spares[i].CGSTRate + "," + Spares.Spares[i].CGSTAmount + "," + Spares.Spares[i].SGSTRate + "," + Spares.Spares[i].SGSTAmount + "," + (type == 1 ? Spares.Spares[i].Discount : "0") + "," + Spares.Spares[i].Price + "," + Spares.Spares[i].Quantity + "," + (type == 1 ? Spares.Spares[i].TotalDiscount : "0") + "," + Spares.Spares[i].FinalAmount + ",'" + dbcon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss tt") + "',1," + Spares.Spares[i].MappingId + ")";
            //        //    dbcon.ExecuteQuery(str);
            //        //}
            //        try
            //        {
            //            string WorkshopId = HttpContext.Current.Request.Cookies["TUser"]["WorkshopId"];
            //            string UserId = HttpContext.Current.Request.Cookies["TUser"]["Id"];
            //            UserActivity objUserAct = new UserActivity();
            //            objUserAct.InsertUserActivity("Update Invoice for " + (type == 1 ? "Customer" : "Insurance") + " Invoice No. : " + id, UserId, WorkshopId, "", "Update", "Invoice");
            //        }
            //        catch (Exception E) { }
            //    }
            //}
            return true;
        }
        catch (Exception E)
        {

        }
        return false;
    }

    public bool generateInvoiceFromEstimate_V1_V1_Proforma(string jobCardId, int type = 1)
    {
        try
        {
            decimal CGSTTax_14 = 0;
            decimal IGSTTax_14 = 0;
            decimal CGSTTaxable_14 = 0;
            decimal CGSTTax_9 = 0;
            decimal IGSTTax_9 = 0;
            decimal CGSTTaxable_9 = 0;
            dbConnection dbcon = new dbConnection();
            string GstNumber = "";
            bool isIgst = false;
            try
            {
                GstNumber = dbcon.GetDataTable("Select isnull((Select Gst_No from Customer where id=Customer_Id),'') from jobcard where id=" + jobCardId).Rows[0][0].ToString();
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
            String str = "";

            ServiceMethods ServiceMethod = new ServiceMethods();
            var Spares = ServiceMethod.GetJobCardSpareInJobCardForInvoiceV3_new_V1_Proforma(jobCardId, "", "", type, false, true);
            try
            {
                foreach (var temp in Spares.Spares)
                {
                    if (temp.Type != "Header")
                    {
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
                    }
                }
                foreach (var temp in Spares.Services)
                {
                    if (temp.Type != "Header")
                    {
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
                    }
                }
                if (type == 1)
                {
                    DataTable dtextra = dbcon.GetDataTable("SELECT isnull([CompusaryExcess],0) as [CompusaryExcess] ,isnull([VoluntaryExcess],0) as [VoluntaryExcess] ,isnull([Salvage],0) as [Salvage] FROM [dbo].[JobCard_Insurance_Mapping] where JobCardId=" + jobCardId);
                    if (dtextra != null && dtextra.Rows.Count > 0)
                    {
                        if (Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString()) > 0)
                        {
                            Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString())).ToString();
                        }
                        if (Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString()) > 0)
                        {
                            Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString())).ToString();
                        }
                        if (Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString()) > 0)
                        {
                            Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) + Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString())).ToString();
                        }
                    }
                }

                if (type == 2)
                {
                    DataTable dtextra = dbcon.GetDataTable("SELECT isnull([CompusaryExcess],0) as [CompusaryExcess] ,isnull([VoluntaryExcess],0) as [VoluntaryExcess] ,isnull([Salvage],0) as [Salvage] FROM [dbo].[JobCard_Insurance_Mapping] where JobCardId=" + jobCardId);
                    if (dtextra != null && dtextra.Rows.Count > 0)
                    {
                        if (Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString()) > 0)
                        {
                            Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["CompusaryExcess"].ToString())).ToString();
                        }
                        if (Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString()) > 0)
                        {
                            Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["VoluntaryExcess"].ToString())).ToString();
                        }
                        if (Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString()) > 0)
                        {
                            Spares.FinalAmount = (Convert.ToDecimal(Spares.FinalAmount) - Convert.ToDecimal(dtextra.Rows[0]["Salvage"].ToString())).ToString();
                        }
                    }
                }
            }
            catch (Exception E) { }
            if (isIgst)
            {
                IGSTTax_14 = (CGSTTax_14 * 2);
                CGSTTax_14 = 0;
                IGSTTax_9 = (CGSTTax_9 * 2);
                CGSTTax_9 = 0;
            }
            DataTable dt = dbcon.GetDataTable("Select id,GstInvoiceNumber from Invoice where isnull(IsCancelled,0)=0 and JobCardId=" + jobCardId + " And type=" + type);
            if (dt != null && dt.Rows.Count > 0)
            {
                string id = dt.Rows[0][0].ToString();
                string GstInvoiceNumber = dt.Rows[0]["GstInvoiceNumber"].ToString();
                str = "Update [Invoice] set [TotalSGST]='" + Spares.FinalTotalCGSTAmount + "',[TotalCGST]='" + Spares.FinalTotalCGSTAmount + "',[FinalTotal]='" + Spares.FinalAmount + "',[Taxable18]=" + CGSTTaxable_9 + ",[SGST18]=" + CGSTTax_9 + ",[CGST18]=" + CGSTTax_9 + ",[IGST18]=" + IGSTTax_9 + ",[Taxable28]=" + CGSTTaxable_14 + ",[SGST28]=" + CGSTTax_14 + ",[CGST28]=" + CGSTTax_14 + ",[IGST28]=" + IGSTTax_14 + " where id=" + id;
                dbcon.ExecuteQuery(str);

                try
                {
                    string WorkshopId = HttpContext.Current.Request.Cookies["TUser"]["WorkshopId"];
                    string UserId = HttpContext.Current.Request.Cookies["TUser"]["Id"];
                    UserActivity objUserAct = new UserActivity();
                    objUserAct.InsertUserActivity("Print Proforma Invoice for " + (type == 1 ? "Customer" : "Insurance") + " Invoice No. : " + GstInvoiceNumber + " , Jobcard No. -" + jobCardId, UserId, WorkshopId, "", "Update", "Invoice");
                }
                catch (Exception E) { }

                //str = "INSERT INTO [dbo].[Invoice] (Id,[JobCardId] ,[TotalSpareAmount],[TotalServiceAmount],[TotalSGST],[TotalCGST],[TotalDiscount],[FinalTotal],[WorkshopId],[DOC],[DOM],[Type],[IncludeInGst],[Taxable18],[SGST18],[CGST18],[IGST18],[Taxable28],[SGST28],[CGST28],[IGST28]) VALUES ((Select max(Id)+1 from invoice) ," + jobCardId + " ," + Spares.SubTotalForSpare + "," + Spares.SubTotalForService + "," + Spares.FinalTotalCGSTAmount + "," + Spares.FinalTotalCGSTAmount + "," + Spares.TotalDiscount + "," + Spares.FinalAmount + ",1,'" + dbcon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss tt") + "','" + dbcon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss tt") + "'," + type + ",1," + CGSTTaxable_9 + "," + CGSTTax_9 + "," + CGSTTax_9 + "," + IGSTTax_9 + "," + CGSTTaxable_14 + "," + CGSTTax_14 + "," + CGSTTax_14 + "," + IGSTTax_14 + ")";
                //dbcon.ExecuteQuery(str);
                //str = "Select top 1 id from invoice where jobcardid=" + jobCardId + " and type=" + type + " Order by id desc";
                //string id = dbcon.GetDataTable(str).Rows[0][0].ToString();
                //for (int i = 1; i < Spares.Services.Count - 1; i++)
                //{
                //    str = "INSERT INTO [dbo].[Invoice_Service_Mapping] ([InvoiceId],[ServiceId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId]) VALUES (" + id + "," + Spares.Services[i].Id + "," + Spares.Services[i].CGSTRate + "," + Spares.Services[i].CGSTAmount + "," + Spares.Services[i].SGSTRate + "," + Spares.Services[i].SGSTAmount + "," + (type == 1 ? Spares.Services[i].Discount : "0") + "," + Spares.Services[i].Price + "," + Spares.Services[i].Quantity + "," + (type == 1 ? Spares.Services[i].TotalDiscount : "0") + "," + Spares.Services[i].FinalAmount + ",'" + dbcon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss tt") + "',1," + Spares.Services[i].MappingId + ")";
                //    dbcon.ExecuteQuery(str);
                //}
                //for (int i = 1; i < Spares.Spares.Count - 1; i++)
                //{
                //    str = "INSERT INTO [dbo].[Invoice_Spare_Mapping] ([InvoiceId],[SpareId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId]) VALUES (" + id + "," + Spares.Spares[i].Id + "," + Spares.Spares[i].CGSTRate + "," + Spares.Spares[i].CGSTAmount + "," + Spares.Spares[i].SGSTRate + "," + Spares.Spares[i].SGSTAmount + "," + (type == 1 ? Spares.Spares[i].Discount : "0") + "," + Spares.Spares[i].Price + "," + Spares.Spares[i].Quantity + "," + (type == 1 ? Spares.Spares[i].TotalDiscount : "0") + "," + Spares.Spares[i].FinalAmount + ",'" + dbcon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss tt") + "',1," + Spares.Spares[i].MappingId + ")";
                //    dbcon.ExecuteQuery(str);
                //}
                //try
                //{
                //    string WorkshopId = HttpContext.Current.Request.Cookies["TUser"]["WorkshopId"];
                //    string UserId = HttpContext.Current.Request.Cookies["TUser"]["Id"];
                //    UserActivity objUserAct = new UserActivity();
                //    objUserAct.InsertUserActivity("Generate New Invoice for " + (type == 1 ? "Customer" : "Insurance") + " Invoice No. : " + id, UserId, WorkshopId, "", "Generate", "Invoice");
                //}
                //catch (Exception E) { }
            }
            //else
            //{
            //    string id = dt.Rows[0][0].ToString();
            //    if (dbcon.GetDataTable("SELECT id FROM [dbo].[JobCard] where isnull([IsGatePassGenerated],0)=0 and Id=" + jobCardId).Rows.Count > 0)
            //    {
            //        str = "Update [Invoice] set [TotalSGST]='" + Spares.FinalTotalCGSTAmount + "',[TotalCGST]='" + Spares.FinalTotalCGSTAmount + "',[FinalTotal]='" + Spares.FinalAmount + "',[Taxable18]=" + CGSTTaxable_9 + ",[SGST18]=" + CGSTTax_9 + ",[CGST18]=" + CGSTTax_9 + ",[IGST18]=" + IGSTTax_9 + ",[Taxable28]=" + CGSTTaxable_14 + ",[SGST28]=" + CGSTTax_14 + ",[CGST28]=" + CGSTTax_14 + ",[IGST28]=" + IGSTTax_14 + " where id=" + id;
            //        dbcon.ExecuteQuery(str);

            //        //dbcon.ExecuteQuery("delete from Invoice_Service_Mapping where invoiceid=" + id);
            //        //for (int i = 1; i < Spares.Services.Count - 1; i++)
            //        //{
            //        //    str = "INSERT INTO [dbo].[Invoice_Service_Mapping] ([InvoiceId],[ServiceId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId]) VALUES (" + id + "," + Spares.Services[i].Id + "," + Spares.Services[i].CGSTRate + "," + Spares.Services[i].CGSTAmount + "," + Spares.Services[i].SGSTRate + "," + Spares.Services[i].SGSTAmount + "," + (type == 1 ? Spares.Services[i].Discount : "0") + "," + Spares.Services[i].Price + "," + Spares.Services[i].Quantity + "," + (type == 1 ? Spares.Services[i].TotalDiscount : "0") + "," + Spares.Services[i].FinalAmount + ",'" + dbcon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss tt") + "',1," + Spares.Services[i].MappingId + ")";
            //        //    dbcon.ExecuteQuery(str);
            //        //}
            //        //dbcon.ExecuteQuery("delete from Invoice_Spare_Mapping where invoiceid=" + id);
            //        //for (int i = 1; i < Spares.Spares.Count - 1; i++)
            //        //{
            //        //    str = "INSERT INTO [dbo].[Invoice_Spare_Mapping] ([InvoiceId],[SpareId],[CGSTValue],[CGSTAmount],[SGSTValue],[SGSTAmount],[DiscountPerUnit],[ActualAmountPerUnit],[Quantity],[TotalDiscount],[TotalActualAmount],[DOC],[WorkshopId],[MappingId]) VALUES (" + id + "," + Spares.Spares[i].Id + "," + Spares.Spares[i].CGSTRate + "," + Spares.Spares[i].CGSTAmount + "," + Spares.Spares[i].SGSTRate + "," + Spares.Spares[i].SGSTAmount + "," + (type == 1 ? Spares.Spares[i].Discount : "0") + "," + Spares.Spares[i].Price + "," + Spares.Spares[i].Quantity + "," + (type == 1 ? Spares.Spares[i].TotalDiscount : "0") + "," + Spares.Spares[i].FinalAmount + ",'" + dbcon.getindiantime().ToString("dd-MMM-yyyy hh:mm:ss tt") + "',1," + Spares.Spares[i].MappingId + ")";
            //        //    dbcon.ExecuteQuery(str);
            //        //}
            //        try
            //        {
            //            string WorkshopId = HttpContext.Current.Request.Cookies["TUser"]["WorkshopId"];
            //            string UserId = HttpContext.Current.Request.Cookies["TUser"]["Id"];
            //            UserActivity objUserAct = new UserActivity();
            //            objUserAct.InsertUserActivity("Update Invoice for " + (type == 1 ? "Customer" : "Insurance") + " Invoice No. : " + id, UserId, WorkshopId, "", "Update", "Invoice");
            //        }
            //        catch (Exception E) { }
            //    }
            //}
            return true;
        }
        catch (Exception E)
        {

        }
        return false;
    }
}