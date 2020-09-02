using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebApplication1;

/// <summary>
/// Summary description for clsDataSourse
/// </summary>
public class clsDataSourse
{
    dbConnection obj = new dbConnection();
    public clsDataSourse()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    


    public string GetGstInvoiceNumber()
    {
        string strInvoiceNumber = String.Empty;
        try
        {
            DateTime dtCurrentDate = obj.getindiantime();
            string strWorkshopCode = "A";
            string strMonth = GetMonthCode(dtCurrentDate.Month);
            string strDate = dtCurrentDate.ToString("dd");
            string strYear = getCurrentFinancialYear(dtCurrentDate);
            //Find Todays Last Invoice  Number
            strInvoiceNumber = strWorkshopCode + "/" + strYear + "/" + strMonth + "/" + strDate + "/";
            DataTable dt = obj.GetDataTable("Select top 1 GstInvoiceNumber from invoice where GstInvoiceNumber like '" + strInvoiceNumber + "%'  order by Convert(int,REPLACE(GstInvoiceNumber,'" + strInvoiceNumber + "','')) desc");
            int currentInvoiceNumber = 0;
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0][0] != null)
                    {
                        int.TryParse(dt.Rows[0][0].ToString().Replace(strInvoiceNumber, ""), out currentInvoiceNumber);
                    }
                }
            }
            else
            {
                //Sql Or Connection Error
            }
            strInvoiceNumber = strInvoiceNumber + getThreeDigitNumber(currentInvoiceNumber + 1);
        }
        catch (Exception E)
        {
        }
        return strInvoiceNumber;
    }

    private string getThreeDigitNumber(int p)
    {
        string strThreeDigitNumber = String.Empty;
        try
        {
            if (p >= 100)
                return p.ToString();
            else if (p >= 10)
                return "0" + p.ToString();
            else
                return "00" + p.ToString();
        }
        catch (Exception E) { }
        return strThreeDigitNumber;
    }

    private string getCurrentFinancialYear(DateTime dtCurrentDate)
    {
        string strFinacialYear = String.Empty;
        try
        {
            int nTwoDigitYear = int.Parse(dtCurrentDate.ToString("yy"));
            int nMonth = dtCurrentDate.Month;
            if (nMonth >= 4)
            {
                strFinacialYear = nTwoDigitYear.ToString() + "-" + (nTwoDigitYear + 1).ToString();
            }
            else
            {
                strFinacialYear = (nTwoDigitYear - 1).ToString() + "-" + nTwoDigitYear.ToString();
            }
        }
        catch (Exception E) { }
        return strFinacialYear;
    }

    private string GetMonthCode(int p)
    {
        try
        {
            switch (p)
            {
                case 1:
                    return "A";
                case 2:
                    return "B";
                case 3:
                    return "C";
                case 4:
                    return "D";
                case 5:
                    return "E";
                case 6:
                    return "F";
                case 7:
                    return "G";
                case 8:
                    return "H";
                case 9:
                    return "I";
                case 10:
                    return "J";
                case 11:
                    return "K";
                case 12:
                    return "L";
            }

        }
        catch (Exception E) { }
        return "";
    }

    public static bool AllowInvoice(string Id)
    {
        return true;
        try
        {
            dbConnection obj1 = new dbConnection();
            string Str = "SELECT JobCard.Id, Spare.Code as Name, Convert(varchar,JobCard_Spare_Mapping.Quantity) as Quantity, JobCard_Spare_Mapping.Id AS MappingId,iif(Requisition_Spare.Id is null,0,1) as IsReqSent,isnull(Convert(varchar(17),Requisition_Spare.DOC,113),'') as ReqSentOn, iif(isnull(Requisition_Spare.IsAllocate,0)=0,0,1) as IsAllocated, isnull((Select name from Users where id= Requisition_Spare.AllocateBy),'') as AllocatedBy , isnull(Convert(varchar(17),Requisition_Spare.AllocateTime,113),'') as AllocatedOn,isnull(Convert(varchar,Requisition_Spare.RequisitionId),'') as ReqId,isnull(convert(varchar,Requisition_Spare.trnNo),'') as trnNo FROM  JobCard INNER JOIN JobCard_Spare_Mapping ON JobCard.Id = JobCard_Spare_Mapping.JobCardId INNER JOIN Spare ON JobCard_Spare_Mapping.SpareId = Spare.Id INNER JOIN JobCard_Details ON JobCard_Spare_Mapping.JobCardDetailId = JobCard_Details.Id LEFT OUTER JOIN Requisition_Spare ON JobCard_Spare_Mapping.Id = Requisition_Spare.SpareMappingId and Requisition_Spare.Isdeleted=0 WHERE (JobCard.Id = " + Id + ") AND (JobCard_Spare_Mapping.IsDeleted = 0) AND (JobCard.WorkshopId = 1) AND (JobCard_Details.IsDeleted = 0) and isnull(Requisition_Spare.IsAllocate,0)=0 ";
            DataTable dt = obj1.GetDataTable(Str);
            Str = "SELECT id from JobCard WHERE JobCard.Id = " + Id + " And IsGatePassGenerated=1 and doc<='03-jul-2018 00:00:00' ";
            DataTable dt1 = obj1.GetDataTable(Str);
            if (dt.Rows.Count == 0 || dt1.Rows.Count > 0)
            {
                return true;
            }
        }
        catch (Exception E) { }
        return true;
    }
    public DataTable SpareBrand()
    {
        String Str = "SELECT [Id],[Name],case when [IsActive]=1 then 'True' Else 'False' end as IsActive  FROM [dbo].[Spare_Brand] where isdelete=0 order by id desc";
        DataTable dt = obj.GetDataTable(Str);
        DataRow newRow = dt.NewRow();
        newRow[0] = 0;
        newRow[1] = "Select";
        dt.Rows.InsertAt(newRow, 0);
        return dt;
    }
    public DataTable Grn()
    {
        String Str = "SELECT  GRN.Id as GrnId,isnull(CONVERT(VARCHAR(12), Grn.DeliveryDate, 107),'-') as Delivery_Date,GRN.ReceivedBy,(SELECT Name FROM Vendor WHERE (Id = GRN.VendorId)) AS VendorName, GRN.Remark,  GRN.InvoiceNo,dcNo as DC_NO,(select Count(id) from GRNDetail where GRNId=grn.Id ) as Items FROM GRN where isnull(isdelete,0)=0 order by GRN.DeliveryDate desc;";
        return obj.GetDataTable(Str);
    }
    public DataTable GrnDetail(string id)
    {
        string[] strArr = { id };
        String Str = "SELECT  GRNDetail.Id,GRN.Id as grnId, case when GRNDetail.EntityType='spare' then (select Code from spare where id=entityid ) else (select name from Consumables where id=entityid ) end as [Part],GRNDetail.Qty, GRNDetail.Price As PurchasePrice,  GRNDetail.Discount,case when GRNDetail.EntityType='spare' then (select price from spare where id=entityid ) else (select price from Consumables where id=entityid ) end as PartMrp,case when GRNDetail.EntityType='spare' then (select Name from Spare_Brand where id=BrandId ) else (select Name from Consumables_Brand where id=BrandId ) end as BrandName FROM GRN INNER JOIN GRNDetail ON GRN.Id = GRNDetail.GRNId where grn.id=@1";
        return obj.GetDataTableWithParams(Str, strArr);
    }
    public DataTable GrnDetail1(string id)
    {
        string[] strArr = { id };
        String Str = "SELECT  entityid as spareid,BrandId,GRNDetail.Id,GRN.Id as grnId, case when GRNDetail.EntityType='spare' then (select Code from spare where id=entityid ) else (select name from Consumables where id=entityid ) end as [Part],GRNDetail.Qty, GRNDetail.Price As PurchasePrice,  GRNDetail.Discount,case when GRNDetail.EntityType='spare' then (select price from spare where id=entityid ) else (select price from Consumables where id=entityid ) end as PartMrp,case when GRNDetail.EntityType='spare' then (select Name from Spare_Brand where id=BrandId ) else (select Name from Consumables_Brand where id=BrandId ) end as BrandName FROM GRN INNER JOIN GRNDetail ON GRN.Id = GRNDetail.GRNId where grn.id=@1";
        DataTable dt = obj.GetDataTableWithParams(Str, strArr);
        try
        {
            for (int i = 0; i < 5; i++)
            {
                // dt.Rows.Add();
                dt.Rows.Add(0, 0, -1, -1, "", 0, 0, 0, 0, "");
                //dt.Rows[dt.Rows.Count - 1]["id"] = -1;
            }
        }
        catch (Exception E) { }
        return dt;
    }
    public DataTable StoreInvetory(string spareid, bool showzero = true)
    {
        //  DataTable dataTable = HttpContext.Current.Cache["StoreInvetory-1"] as DataTable;
        // if (dataTable == null)
        //{
        string str = "SELECT SpareSpareBrandMapping.id, Spare.Code Part_Name, Spare_Brand.Name AS Brand,SpareSpareBrandMapping.Qty as Available_Qty, SpareSpareBrandMapping.MinQty as Reorder_Level, SpareSpareBrandMapping.MaxQty  as Maximum_Level FROM Spare_Brand INNER JOIN SpareSpareBrandMapping ON Spare_Brand.Id = SpareSpareBrandMapping.BrandId INNER JOIN Spare ON SpareSpareBrandMapping.SpareId = Spare.Id   " + (!showzero ? "" : " where SpareSpareBrandMapping.Qty>0 ");//where Spare.Id=" + spareid;
        DataTable dataTable = obj.GetDataTable(str);
        //     HttpContext.Current.Cache["StoreInvetory-1"] = dataTable;
        // }
        return dataTable;
        //string str = "SELECT SpareSpareBrandMapping.id, Spare.Code Part_Name, Spare_Brand.Name AS Brand,SpareSpareBrandMapping.Qty as Available_Qty, SpareSpareBrandMapping.MinQty as Reorder_Level, SpareSpareBrandMapping.MaxQty  as Maximum_Level FROM Spare_Brand INNER JOIN SpareSpareBrandMapping ON Spare_Brand.Id = SpareSpareBrandMapping.BrandId INNER JOIN Spare ON SpareSpareBrandMapping.SpareId = Spare.Id ";
        //return obj.GetDataTable(str);
    }
    public DataTable StoreInvetory1(string From, string To)
    {
        string str = " SELECT SpareSpareBrandMapping.Id,Spare.Name AS [Spare Name], Spare_Brand.Name AS [Brand Name], (Sum(Cr)-Sum(Dr)) Available_Qty_Between_Date ,SpareSpareBrandMapping.Qty Available_Qty_Till_Date FROM  SpareInventaryHistory INNER JOIN SpareSpareBrandMapping ON SpareInventaryHistory.SpareSpareBrandMappingId = SpareSpareBrandMapping.Id INNER JOIN Spare ON SpareSpareBrandMapping.SpareId = Spare.Id INNER JOIN Spare_Brand ON SpareSpareBrandMapping.BrandId = Spare_Brand.Id where (grndetailid in (select id from GRNDetail where GRNId in (Select id from grn where DeliveryDate >='" + From + " 00:00:00' And DeliveryDate <='" + To + " 23:59:00') )or(RequsitionSpareId in (Select id from Requisition_Spare where RequisitionId in (Select id from Requisition where AllocatedTime>='" + From + " 00:00:00' and AllocatedTime<='" + To + " 23:59:00')))) group by Spare.Name,Spare_Brand.Name,SpareSpareBrandMapping.Qty,SpareSpareBrandMapping.Id order by SpareSpareBrandMapping.Id ";
        DataTable dataTable = obj.GetDataTable(str);
        return dataTable;
    }
    public DataTable JobCardGrn(string From, string To)
    {
        string str = "Select id as [Job Card Id],CONVERT(VARCHAR(6), DOC, 107) as DOC,(Select Number from Vehicle where id=Vehicle_Id) as [Vehcicle Number],isnull((select top 1 iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Spare_Mapping.IsDeleted,0)=0),0)+iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Service_Mapping.IsDeleted,0)=0),0) FROM [dbo].[Estimate] where JobCardId=JobCard.Id order by  id desc  ),0) as [Est A],isnull((select top 1 iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Spare_Mapping.IsDeleted,0)=0),0) FROM [dbo].[Estimate] where JobCardId=JobCard.Id order by  id desc  ),0) as [Est Spare],isnull((Select Sum(Quantity) from JobCard_Spare_Mapping where JobCardId=jobcard.Id),0) as [Total Items Qty],isnull((SELECT  STUFF((SELECT ',' + isnull(CAST(GRN.Id AS VARCHAR(10)),'')+'['+CONVERT(VARCHAR(6), DeliveryDate, 107)+']'  FROM  GRN INNER JOIN GRNDetail ON GRN.Id = GRNDetail.GRNId INNER JOIN JobCard_Spare_Mapping ON GRNDetail.EntityId = JobCard_Spare_Mapping.SpareId WHERE        (JobCard_Spare_Mapping.IsDeleted = 0) and JobCard_Spare_Mapping.JobCardId=JobCard.Id group by GRN.Id,DeliveryDate FOR XML PATH('')), 1, 1, '') AS listStr),'') as [Grn Ids] from JobCard where isnull(IsDelete,0)=0 and jobcard.doc>='" + From + " 00:00:00' and jobcard.doc<='" + To + " 23:59:59'";
        DataTable dataTable = obj.GetDataTable(str);
        for (int i = 0; i < dataTable.Rows.Count; i++)
        {
            if (dataTable.Rows[i]["Grn Ids"] != null)
            {
                string strval = "";
                string[] stra = dataTable.Rows[i]["Grn Ids"].ToString().Split(',');
                for (int j = 0; j < stra.Length; j++)
                {
                    if (strval.Length > 0)
                    {
                        strval += ",";
                    }
                    if (!String.IsNullOrWhiteSpace(stra[j]))
                    {
                        string[] stra1 = stra[j].Split('[');
                        if (stra1.Length > 0)
                        {
                            strval += "<a href='../Store/grnEdit.aspx?id=" + stra1[0] + "' target='_blank'>" + stra1[0] + "[" + stra1[1] + "</a>";
                        }
                    }
                }
                dataTable.Rows[i]["Grn Ids"] = strval;
            }
        }
        return dataTable;
    }
    public DataTable JobCardGrnDetail(string Id)
    {

        string str = "Select ROW_NUMBER ( ) OVER ( order by id ) as RowNo,SpareId,(Select Code from Spare where id=SpareId) as [Part Name],Amount as [Estimate Amount Per Unit],Quantity,(Quantity*Amount) as [Total Estimate Amount],isnull(Convert(decimal(18,2),(Select Sum(isnull(Price,0))/(case when Count(Id)=0 then 1 else Count(Id) end) from GRNDetail where EntityId=JobCard_Spare_Mapping.SpareId)),0) as [Purchase Avg Price],Quantity*isnull(Convert(decimal(18,2),(Select Sum(isnull(Price,0))/(case when Count(Id)=0 then 1 else Count(Id) end) from GRNDetail where EntityId=JobCard_Spare_Mapping.SpareId)),0) [Total Purchase Price], Convert(varchar,Convert(decimal(18,2), (Amount- isnull(Convert(decimal(18,2),(Select Sum(isnull(Price,0))/(case when Count(Id)=0 then 1 else Count(Id) end) from GRNDetail where EntityId=JobCard_Spare_Mapping.SpareId)),0))/((case when Amount=0 then 1 else Amount end)/100)))+' %' as Margin , isnull((SELECT  STUFF((SELECT ',' + isnull(CAST(GRN.Id AS VARCHAR(10)),'')+'['+CONVERT(VARCHAR(6), DeliveryDate, 107)+']'  FROM  GRN INNER JOIN GRNDetail ON GRN.Id = GRNDetail.GRNId  WHERE  GRNDetail.EntityId=JobCard_Spare_Mapping.SpareId group by GRN.Id,DeliveryDate FOR XML PATH('')), 1, 1, '') AS listStr),'') as [Grn Ids] from  JobCard_Spare_Mapping where JobCardId in (Select id from jobcard where isnull(IsDelete,0)=0)  and  isnull(JobCard_Spare_Mapping.IsDeleted,0)=0 and JobCardId=" + Id;
        DataTable dataTable = obj.GetDataTable(str);
        for (int i = 0; i < dataTable.Rows.Count; i++)
        {
            if (dataTable.Rows[i]["Grn Ids"] != null)
            {
                string strval = "";
                string[] stra = dataTable.Rows[i]["Grn Ids"].ToString().Split(',');
                for (int j = 0; j < stra.Length; j++)
                {
                    if (strval.Length > 0)
                    {
                        strval += ",";
                    }
                    if (!String.IsNullOrWhiteSpace(stra[j]))
                    {
                        string[] stra1 = stra[j].Split('[');
                        if (stra1.Length > 0)
                        {
                            strval += "<a href='../Store/grnEdit.aspx?id=" + stra1[0] + "' target='_blank'>" + stra1[0] + "[" + stra1[1] + "</a>";
                        }
                    }
                }
                dataTable.Rows[i]["Grn Ids"] = strval;
            }
        }
        return dataTable;
    }
    public DataTable ItemInvetoryHistory(string Id)
    {
        string[] strArr = { Id };
        string str = "SELECT SpareInventaryHistory.Id,(SELECT Spare.Code + ',' + Spare_Brand.Name AS Expr1 FROM            SpareSpareBrandMapping INNER JOIN Spare ON SpareSpareBrandMapping.SpareId = Spare.Id INNER JOIN Spare_Brand ON SpareSpareBrandMapping.BrandId = Spare_Brand.Id WHERE        (SpareSpareBrandMapping.Id = SpareInventaryHistory.SpareSpareBrandMappingId)) AS [Name And Brand], SpareInventaryHistory.Cr, SpareInventaryHistory.Dr,case when isnull([SpareInventaryHistory].GRNDetailId,0)>0  then CONVERT(varchar(17), GRN.DeliveryDate , 113 ) else CONVERT(varchar(12), Requisition.AllocatedTime , 113 ) end  as DOC, GrnId, RequisitionId,ISNULL(Requisition.JobCardId,0) AS JOBCARDID, SpareInventaryHistory.Message FROM SpareInventaryHistory LEFT OUTER JOIN Requisition INNER JOIN Requisition_Spare AS Requisition_Spare_1 ON Requisition.Id = Requisition_Spare_1.RequisitionId ON SpareInventaryHistory.RequsitionSpareId = Requisition_Spare_1.Id LEFT OUTER JOIN GRN INNER JOIN GRNDetail AS GRNDetail_1 ON GRN.Id = GRNDetail_1.GRNId ON SpareInventaryHistory.GRNDetailId = GRNDetail_1.Id where SpareSpareBrandMappingId=@1 ";
        return obj.GetDataTableWithParams(str, strArr);
    }
    public DataTable ItemInvetoryHistory1(string Id, string From, string To)
    {
        string[] strArr = { Id, From, To };
        string str = " SELECT SpareSpareBrandMapping.Id,Spare.Name AS [Spare Name], Spare_Brand.Name AS [Brand Name],Cr,isnull((select grnid from grndetail where id=GRNDetailId),0) as GrnId,Dr,Isnull((select RequisitionId from Requisition_Spare where id= RequsitionSpareId),0) as RequisitionId FROM  SpareInventaryHistory INNER JOIN SpareSpareBrandMapping ON SpareInventaryHistory.SpareSpareBrandMappingId = SpareSpareBrandMapping.Id INNER JOIN Spare ON SpareSpareBrandMapping.SpareId = Spare.Id INNER JOIN Spare_Brand ON SpareSpareBrandMapping.BrandId = Spare_Brand.Id where (grndetailid in (select id from GRNDetail where GRNId in (Select id from grn where DeliveryDate >='" + From + " 00:00:00' And DeliveryDate <='" + To + " 23:59:00') )or(RequsitionSpareId in (Select id from Requisition_Spare where RequisitionId in (Select id from Requisition where AllocatedTime>='" + From + " 00:00:00' and AllocatedTime<='" + To + " 23:59:00')))) and SpareSpareBrandMapping.Id=@1 ";
        return obj.GetDataTableWithParams(str, strArr);
    }
    public DataTable ConsumableBrand()
    {
        String Str = "SELECT [Id],[Name],case when [IsActive]=1 then 'True' Else 'False' end as IsActive  FROM [dbo].[Consumables_Brand] where isdelete=0 order by id desc";
        return obj.GetDataTable(Str);
    }
    public DataTable TaxCategory()
    {
        String Str = "SELECT [Id],[Name],HSNNumber,case when [IsActive]=1 then 'True' Else 'False' end as IsActive  FROM [dbo].[TaxCategory] where isdelete=0 order by id desc";
        return obj.GetDataTable(Str);
    }
    public DataTable Consumable(string keyword)
    {
        string strFilt = " and name like '%" + keyword + "%' ";
        String Str = "SELECT Price,(select name from Consumables_Brand where id=BrandId) as Brand,(select name from Taxcategory where id=TaxId) as Tax,[Id],[Name],case when [IsActive]=1 then 'True' Else 'False' end as IsActive,(Select Name from category where id=categoryID) as CatName  FROM [dbo].[Consumables] where isdeleted=0 " + (!String.IsNullOrWhiteSpace(keyword) ? strFilt : "") + " order by id desc";
        return obj.GetDataTable(Str);
    }
    public DataTable Service()
    {
        String Str = "SELECT (select name from Taxcategory where id=TaxId) as Tax,[Id],[Name],Price,case when [IsActive]=1 then 'True' Else 'False' end as IsActive,(Select Name from category where id=categoryID) as CatName  FROM [dbo].[Service] where isdeleted=0 order by id desc";
        return obj.GetDataTable(Str);
    }
    public DataTable VehicleBrand()
    {
        String Str = "SELECT [Id],[Name],case when [IsActive]=1 then 'True' Else 'False' end as IsActive,DisplayOrder  FROM [dbo].[Vehicle_Brand] where isdelete=0 order by id desc";
        return obj.GetDataTable(Str);
    }
    public DataTable InsuranceProvider()
    {
        String Str = "SELECT [Id],[Name],case when [IsActive]=1 then 'True' Else 'False' end as IsActive,Mobile,ContactNumber,GSTIn,DisplayName  FROM [dbo].[InsuranceProvider] where isnull(isdeleted,0)=0 order by id desc";
        return obj.GetDataTable(Str);
    }
    public DataTable VehicleBrandOrderByName()
    {
        String Str = "SELECT [Id],[Name] FROM [dbo].[Vehicle_Brand] where isdelete=0 order by Name";
        return obj.GetDataTable(Str);
    }
    public DataTable VehicleModel(string keyword)
    {
        string strFilt = " and name like '%" + keyword + "%' ";
        String Str = "SELECT [Id],[Name],case when [IsActive]=1 then 'True' Else 'False' end as IsActive,DisplayOrder,(Select Top 1 Name from Vehicle_Brand where id=Vehicle_Brand_Id) as Brand,(Select Top 1 Name from Segment where id=SegmentId) as segment   FROM [dbo].[Vehicle_Model] where isdelete=0 " + (!String.IsNullOrWhiteSpace(keyword) ? strFilt : "") + " order by id desc";
        return obj.GetDataTable(Str);
    }
    public DataTable Segment()
    {
        String Str = "SELECT [Id],[Name],case when [IsActive]=1 then 'True' Else 'False' end as IsActive,displayOrder  FROM [dbo].[Segment] where isdeleted=0 order by id desc";
        return obj.GetDataTable(Str);
    }
    public DataTable SegmentOrderByName()
    {
        String Str = "SELECT [Id],[Name],case when [IsActive]=1 then 'True' Else 'False' end as IsActive,displayOrder  FROM [dbo].[Segment] where isdeleted=0 order by id desc";
        return obj.GetDataTable(Str);
    }
    public DataTable VehicleVariant(string keyword)
    {
        string strFilt = " and name like '%" + keyword + "%' ";
        String Str = "SELECT Id,[Name],[IsActive],DisplayOrder,Vehicle_Model_Id,(Select name from Vehicle_Brand where id = (Select Vehicle_Brand_Id from Vehicle_Model where id=Vehicle_Model_Id )) as Brand, (Select name from Segment where id = (Select SegmentId from Vehicle_Model where id=Vehicle_Model_Id )) as Segment,(Select name from Vehicle_Model where id= Vehicle_Model_Id) As Model FROM [dbo].[Vehicle_Variant] where isdelete=0" + (!String.IsNullOrWhiteSpace(keyword) ? strFilt : "") + " order by id desc";
        return obj.GetDataTable(Str);
    }
    public DataTable ModelOrderByName(string Brand, string Segment, string variantId)
    {
        String[] StrArr = { Brand, Segment, variantId };
        String Str = "SELECT [Id],[Name],case when id=(Select id from Vehicle_Model where id in (select Vehicle_Variant.Vehicle_Model_Id from Vehicle_Variant where Id=@3 ) ) then 'btn btn-warning dropdown-toggle' else 'btn btn-primary btn-sm btn-flat' end as cssClass FROM [dbo].[Vehicle_Model] where isdelete=0 And Vehicle_Brand_Id=@1   order by Name";
        return obj.GetDataTableWithParams(Str, StrArr);
    }
    public DataTable ModelOrderByName1(string Brand, string Segment)
    {
        String StrF = "";
        if (!String.IsNullOrWhiteSpace(Segment) && Segment != "0")
            StrF = "And SegmentId in (" + Segment + ")";
        String[] StrArr = { Brand, Segment };
        String Str = "SELECT [Id],[Name],case when id=(Select id from Vehicle_Model where id in (select Vehicle_Variant.Vehicle_Model_Id from Vehicle_Variant where Id=0 ) ) then 'btn btn-warning dropdown-toggle' else 'btn btn-primary btn-sm btn-flat' end as cssClass FROM [dbo].[Vehicle_Model] where isdelete=0 And Vehicle_Brand_Id in(" + Brand + ") " + StrF + "   order by Name";
        return obj.GetDataTableWithParams(Str, StrArr);
    }
    public DataTable VariantOrderByName1(string Model)
    {
        String[] StrArr = { Model };
        String Str = "SELECT [Id],[Name],case when id= ( Select id from [Vehicle_Variant] where id in (select id from Spare where Id=0 ) ) then 'btn btn-warning dropdown-toggle' else 'btn btn-primary btn-sm btn-flat' end as cssClass FROM [dbo].[Vehicle_Variant] where isdelete=0 And Vehicle_Model_Id in (" + Model + ")  order by Name";
        return obj.GetDataTableWithParams(Str, StrArr);
    }
    public DataTable VariantOrderByName(string Model, string SpareId)
    {
        String[] StrArr = { Model, SpareId };
        String Str = "SELECT [Id],[Name],case when id= ( Select id from [Vehicle_Variant] where id in (select Vehicle_Variant_Id from Spare where Id=@2 ) ) then 'btn btn-warning dropdown-toggle' else 'btn btn-primary btn-sm btn-flat' end as cssClass FROM [dbo].[Vehicle_Variant] where isdelete=0 And Vehicle_Model_Id=@1  order by Name";
        return obj.GetDataTableWithParams(Str, StrArr);
    }
    public DataTable MainCategory()
    {
        String Str = "SELECT [Id],[Name],case when [IsActive]=1 then 'True' Else 'False' end as IsActive from MasterCategory where isdeleted=0";
        return obj.GetDataTable(Str);
    }
    public DataTable Category(string keyword)
    {
        string strFilt = " And name like '%" + keyword + "%' ";

        String Str = "SELECT [Id],isnull((Select c.name+' >> ' from Category as c where c.Id=Category.ParentCategoryId),'')+[Name] as Name ,case when [IsActive]=1 then 'True' Else 'False' end as IsActive,(SELECT [Name] from MasterCategory where id=MasterCategoryId) as MCat from Category where isdeleted=0  " + (!String.IsNullOrWhiteSpace(keyword) ? strFilt : "") + "   order by id desc";
        return obj.GetDataTable(Str);
    }
    public DataTable Spare(string keyword)
    {
        string strFilt = " And (name like '%" + keyword + "%' or  Number like '%" + keyword + "%' or  Code like '%" + keyword + "%')";
        String Str = "SELECT (select name from Taxcategory where id=TaxId) as Tax,[Id],[Name],[Number],[Code],case when [IsActive]=1 then 'True' Else 'False' end as IsActive,(Select Name from Category where id=Category_Id) as categoryname FROM [dbo].[Spare] where id>0  " + (!String.IsNullOrWhiteSpace(keyword) ? strFilt : "") + "  Order by Id Desc";
        return obj.GetDataTable(Str);
    }
    public DataTable SpareNew(string spareid)
    {
        String Str = "SELECT (select name from Taxcategory where id=TaxId) as Tax,[Id],[Name],[Number],[Code],case when [IsActive]=1 then 'True' Else 'False' end as IsActive,(Select Name from Category where id=Category_Id) as categoryname FROM [dbo].[Spare] where id=" + spareid;
        return obj.GetDataTable(Str);
    }
    public DataTable Spare1(string keyword)
    {
        String Str = "SELECT [Id],[Code] FROM [dbo].[Spare] where isnull(IsDeleted,0)=0 Order by code";
        DataTable dt = obj.GetDataTable(Str);
        DataRow newRow = dt.NewRow();
        newRow[0] = 0;
        newRow[1] = "Select";

        dt.Rows.InsertAt(newRow, 0);
        return dt;
    }
    public DataTable SearchSpare(string Name, string Number, string Code)
    {
        string[] StrArr = { Name, Number, Code };
        string StrFilt = "";
        if (!String.IsNullOrWhiteSpace(Name))
        {
            StrFilt = " And Name like '%@1%'";
        }
        String Str = "SELECT (select name from Taxcategory where id=TaxId) as Tax,[Id],[Name],[Number],[Code],case when [IsActive]=1 then 'True' Else 'False' end as IsActive,(Select Name from Category where id=Category_Id) as categoryname FROM [dbo].[Spare] where IsDeleted=0 " + StrFilt + " Order by Id Desc";
        return obj.GetDataTable(Str);
    }
    public DataTable Vendor()
    {
        String Str = "SELECT Id,Name, Email, Mobile, GSTIN, ContactPerson1, ContactPerson2, Address, City, State, BankName, BankBranchName, AccountNumber, AccountHolderName, IFSCCode, IsDeleted FROM Vendor where IsDeleted=0";
        return obj.GetDataTable(Str);
    }
    public DataTable JobCardPayment(bool OnlyInsurance, bool IsNeft, bool IsCash, bool IsCheque, bool IsPending, string StartDate, string EndDate)
    {
        String Str = " SELECT  isnull(InsuranceInvoiceAmount,0) as InsuranceInvoiceAmount,isnull(CustomerInvoiceAmount,0) as CustomerInvoiceAmount, isnull((Select isnull((Select sum(isnull(TotalActualAmount,0)) from Invoice_Spare_Mapping where InvoiceId=Invoice.Id),0)+isnull((Select  sum(isnull(TotalActualAmount,0)) from Invoice_Service_Mapping where (InvoiceId=Invoice.Id)),0) from Invoice where JobCardid=JobCard.Id),0) as invoiceAmount,isnull((Select Top 1 EstimateTotal from Estimate where JobCardId=jobcard.Id order by id desc),0) as Estimate,JobCard.Id jobcardid,CONVERT(VARCHAR(12), JobCard.DOC, 107) as StartDate, isnull(CONVERT(VARCHAR(12), JobCard.Job_Close_Date, 107),'-') as EndDate, isnull(Vehicle_Model.Name,'-') +' ,'+ isnull(Vehicle_Brand.Name,'-') + ' ,'+ isnull(Vehicle.Number,'-') as Vehicle, isnull(Customer.Name,'-') AS CustomerName,isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) as PaidBy_Insurance,Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') as PaidBy_Customer,case when ((isnull((Select Sum(Amount) from  [dbo].[JobCardPayment] where jobcardid=JobCard.Id),0)+10)>=(isnull((Select isnull((Select sum(isnull(TotalActualAmount,0)) from Invoice_Spare_Mapping where InvoiceId=Invoice.Id),0)+isnull((Select sum(isnull(TotalActualAmount,0)) from Invoice_Service_Mapping where (InvoiceId=Invoice.Id)),0) from Invoice where JobCardid=JobCard.Id),0)) and (Select COUNT(id) from JobCardPayment where JobCardId=JobCard.Id)>0) then 'Y' else 'N' end as Paid, isnull((SELECT sum(Amount) FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),0) as PaidAmount,case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'Y' else 'NA' end as HavingInsurance FROM   Customer RIGHT OUTER JOIN JobCard ON Customer.Id = JobCard.Customer_Id LEFT OUTER JOIN Vehicle LEFT OUTER JOIN Vehicle_Variant ON Vehicle.Vehicle_Variant_Id = Vehicle_Variant.Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id ON JobCard.Vehicle_Id = Vehicle.Id where JobCard.id>0 ";
        if (IsCash && IsCheque && IsPending && IsNeft)
        {

        }
        else
        {
            //if (IsCash && IsCheque)
            //{
            //    Str += " AND (isnull((SELECT top 1 [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='CASH' OR isnull((SELECT top 1 [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='CHEQUE')";
            //}
            //else if (IsCash && IsPending)
            //{
            //    Str += " AND (isnull((SELECT top 1 [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='CASH' OR isnull((SELECT [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='PENDING')";
            //}
            //else if (IsPending && IsCheque)
            //{
            //    Str += " AND (isnull((SELECT top 1 [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='CHEQUE' OR isnull((SELECT top 1 [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='PENDING')";
            //}
            //else 
            if (IsCash)
            {
                Str += " and ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cash%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%Cash%'))";
            }
            else if (IsCheque)
            {
                Str += " and ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%Cheque%'))";
            }
            else if (IsPending)
            {
                Str += " and ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%pending%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%pending%'))";
            }
            else if (IsNeft)
            {
                Str += " and ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%NEFT%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%NEFT%'))";
            }
        }
        if (!String.IsNullOrWhiteSpace(StartDate))
        {
            Str += " And JobCard.doc>='" + StartDate + " 00:00:00.000' ";
        }
        if (!String.IsNullOrWhiteSpace(EndDate))
        {
            Str += " And JobCard.doc<='" + EndDate + " 23:59:59.000' ";
        }
        if (OnlyInsurance)
        {
            Str += " and isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0)>0 ";
        }
        return obj.GetDataTable(Str);
    }
    public DataTable JobCardPaymentV1(bool IsWash, bool OnlyInsurance, bool IsNeft, bool IsCash, bool IsCheque, bool IsPending, string StartDate, string EndDate
        , bool isIMPS, bool isRTGS, bool isCCard, bool isDCard
        , bool isUPI, bool isAEPS, bool isUSSD, bool isDisputed, bool isIn, bool isOut
        )
    {
        String Str = "SELECT  isnull(InsuranceInvoiceAmount,0) as InsuranceInvoiceAmount,isnull(CustomerInvoiceAmount,0) as CustomerInvoiceAmount,isnull(( SELECT top 1 [FinalTotal]-(isnull((Select Sum(isnull(TotalDiscount,0)) from Invoice_Spare_Mapping where InvoiceId=Invoice.Id),0)+isnull((Select Sum(isnull(TotalDiscount,0)) from Invoice_Service_Mapping where InvoiceId=Invoice.Id),0)) FROM [dbo].Invoice where JobCardId=JobCard.id order by id desc ),0) as invoiceAmount,isnull((select top 1 (Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(IsDeleted,0)!=1 )+(Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(IsDeleted,0)!=1) FROM [dbo].[Estimate] where JobCardId=JobCard.id order by  id desc ),0) as Estimate,JobCard.Id jobcardid,CONVERT(VARCHAR(12), JobCard.DOC, 107) as StartDate, isnull(CONVERT(VARCHAR(12), JobCard.Job_Close_Date, 107),'-') as EndDate, isnull(Vehicle_Model.Name,'-') +' ,'+ isnull(Vehicle_Brand.Name,'-') + ' ,'+ isnull(Vehicle.Number,'-') as Vehicle, isnull(Customer.Name,'-') AS CustomerName,isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) as PaidBy_Insurance,Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') as PaidBy_Customer,case when ((isnull((Select Sum(Amount) from  [dbo].[JobCardPayment] where jobcardid=JobCard.Id),0)+10)>=isnull((select top 1 (Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(IsDeleted,0)!=1)+(Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(IsDeleted,0)!=1) FROM [dbo].[Estimate] where JobCardId=JobCard.id order by  id desc ),0) and (Select COUNT(id) from JobCardPayment where JobCardId=JobCard.Id)>0) then 'Y' else 'N' end as Paid, isnull((SELECT sum(Amount) FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),0) as PaidAmount,case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'Y' else 'NA' end as HavingInsurance FROM   Customer RIGHT OUTER JOIN JobCard ON Customer.Id = JobCard.Customer_Id LEFT OUTER JOIN Vehicle LEFT OUTER JOIN Vehicle_Variant ON Vehicle.Vehicle_Variant_Id = Vehicle_Variant.Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id ON JobCard.Vehicle_Id = Vehicle.Id where JobCard.id>0 ";
        if (IsCash && IsCheque && IsPending && IsNeft && isIMPS && isRTGS && isCCard && isDCard && isUPI && isAEPS && isUSSD && isDisputed)
        {

        }
        else
        {
            string strfilt = " And (";

            if (IsCash)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += " ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cash%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%Cash%'))";
                }
            }
            if (IsCheque)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += " ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%Cheque%'))";
                }
                else
                {
                    strfilt += " or ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%Cheque%'))";
                }
            }
            if (IsPending)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += "  ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%pending%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%pending%'))";
                }
                else
                {
                    strfilt += " or ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%pending%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%pending%'))";
                }
            }
            if (IsNeft)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += " ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%NEFT%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%NEFT%'))";
                }
                else
                {
                    strfilt += " or ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%NEFT%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%NEFT%'))";
                }
            }


            //&& isIMPS && isRTGS && isCCard && isDCard && isUPI && isAEPS && isUSSD && isDisputed
            if (isIMPS)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += " ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%IMPS%'))";
                }
                else
                {
                    strfilt += " or ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%IMPS%'))";
                }
            }
            // isCCard && isDCard && isUPI && isAEPS && isUSSD && isDisputed
            if (isRTGS)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += " ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%RTGS%'))";
                }
                else
                {
                    strfilt += " or ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%RTGS%'))";
                }
            }

            // isDCard && isUPI && isAEPS && isUSSD && isDisputed
            if (isCCard)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += " ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%CCard%'))";
                }
                else
                {
                    strfilt += " or ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%CCard%'))";
                }
            }
            //  isUPI && isAEPS && isUSSD && isDisputed
            if (isDCard)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += " ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%DCard%'))";
                }
                else
                {
                    strfilt += " or ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%DCard%'))";
                }
            }

            //isAEPS && isUSSD && isDisputed
            if (isUPI)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += " ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%UPI%'))";
                }
                else
                {
                    strfilt += " or ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%UPI%'))";
                }
            }

            //isUSSD && isDisputed
            if (isAEPS)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += " ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%AEPS%'))";
                }
                else
                {
                    strfilt += " or ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%AEPS%'))";
                }
            }

            // && isDisputed
            if (isUSSD)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += " ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%USSD%'))";
                }
                else
                {
                    strfilt += " or ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%USSD%'))";
                }
            }

            // && isDisputed
            if (isDisputed)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += " ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%Dispute%'))";
                }
                else
                {
                    strfilt += " or ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%Dispute%'))";
                }
            }
            strfilt += " ) ";
            Str += strfilt;
        }
        if (!String.IsNullOrWhiteSpace(StartDate))
        {
            Str += " And (JobCard.doc >= '" + StartDate + " 00:00:00.000') ";
        }
        if (!String.IsNullOrWhiteSpace(EndDate))
        {
            Str += " And (JobCard.doc <= '" + EndDate + " 23:59:59.000') ";
        }

        if (isIn && isOut)
        {

        }
        else
        {
            if (isIn)
            {
                Str += " And isnull(IsGatePassGenerated,0) =0 ";
            }
            if (isOut)
            {
                Str += " And isnull(IsGatePassGenerated,0) =1 ";
            }
        }
        if (!IsWash)
        {
            Str += " And JobCard.[Type] <> 4 ";
        }
        if (OnlyInsurance)
        {
            Str += " and isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0)>0 ";
        }
        return obj.GetDataTable(Str);
    }


    public DataTable JobCardPaymentV2(bool IsWash, bool OnlyInsurance, bool IsNeft, bool IsCash, bool IsCheque, bool IsPending, string StartDate, string EndDate, bool isIMPS, bool isRTGS, bool isCCard, bool isDCard, bool isUPI, bool isAEPS, bool isUSSD, bool isDisputed, bool isIn, bool isOut)
    {
        String Str = "SELECT  CONVERT(VARCHAR(12), JobCard.DOC, 107)+' '+Convert(varchar,(datepart(hour,jobcard.DOc)))+':'+Convert(varchar,datepart(minute,jobcard.DOc)) as [Start Date], JobCard.Id [Job Card Id],(Select name from jobtype where id=jobcard.type) as Type ,isnull((select top 1 iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Spare_Mapping.IsDeleted,0)=0),0)+iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Service_Mapping.IsDeleted,0)=0),0) FROM [dbo].[Estimate] where JobCardId=JobCard.Id order by  id desc  ),0)+isnull((select top 1 isnull(JobCard_Insurance_Mapping.Salvage,0)+isnull(JobCard_Insurance_Mapping.VoluntaryExcess,0)+isnull(JobCard_Insurance_Mapping.CompusaryExcess,0) from JobCard_Insurance_Mapping where JobCardId=JobCard.Id),0) as [Est A],case when ((isnull((Select Sum(Amount) from  [dbo].[JobCardPayment] where jobcardid=JobCard.Id),0)+10)>=(isnull((select top 1 iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Spare_Mapping.IsDeleted,0)=0),0)+iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Service_Mapping.IsDeleted,0)=0),0) FROM [dbo].[Estimate] where JobCardId=JobCard.Id order by  id desc  ),0)+isnull((select top 1 isnull(JobCard_Insurance_Mapping.Salvage,0)+isnull(JobCard_Insurance_Mapping.VoluntaryExcess,0)+isnull(JobCard_Insurance_Mapping.CompusaryExcess,0) from JobCard_Insurance_Mapping where JobCardId=JobCard.Id),0)) and (Select COUNT(id) from JobCardPayment where JobCardId=JobCard.Id)>0) then 'Y' else 'N' end as IsPaid  , isnull((SELECT sum(Amount) FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),0) as [T Paid A],isnull(CustomerInvoiceAmount,0) as [Cust Est A] , Isnull((SELECT Sum(isnull([Amount],0))   FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer') ,0) as [Paid By Cust], Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10)) [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(max)'),1,2,' ')) ,'PENDING') as [C P Mode] ,case when isnull(JobCard.HavingInsurance,0) >0 then 'Y' else 'NA' end as IsInsurance ,isnull(InsuranceInvoiceAmount,0) as [Ins Est A] , Isnull((SELECT Sum(isnull([Amount],0))   FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Insurance') ,0) as [Paid By Ins], isnull((STUFF((SELECT ',' + CAST([PaymentType] AS VARCHAR(10)) [text()] FROM  [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) as [I P Mode] ,isnull((Select top 1 GstInvoiceNumber from Invoice where  isnull(iscancelled,0)=0 and  JobCardId=JobCard.Id and Type=2),0) as [Inv No Ins] ,isnull((Select top 1 FinalTotal from Invoice where JobCardId=JobCard.Id and Type=2),0) as [Invoice A Ins],isnull((Select top 1 convert(varchar(12),Doc,113) from Invoice where JobCardId=JobCard.Id and Type=2),0) as [Inv Dt Ins],isnull((Select top 1 GstInvoiceNumber from Invoice where isnull(iscancelled,0)=0 and JobCardId=JobCard.Id and Type=1),0) as [Inv No Cust],isnull((Select top 1 FinalTotal from Invoice where JobCardId=JobCard.Id and Type=1),0) as [Invoice A Cust],isnull((Select top 1 convert(varchar(12),Doc,113) from Invoice where JobCardId=JobCard.Id and Type=1),0) as [Inv Dt Cust]   , isnull(CONVERT(VARCHAR(12), JobCard.GatePassGeneratedDate, 107),'-')+' '+Convert(varchar,(datepart(hour,jobcard.GatePassGeneratedDate)))+':'+Convert(varchar,datepart(minute,jobcard.GatePassGeneratedDate)) as [Out Date] ,isnull(Vehicle_Model.Name,'-') +' ,'+ isnull(Vehicle_Brand.Name,'-') + ' ,'+ isnull(Vehicle.Number,'-') as Vehicle,isnull(Customer.Name,'-') AS Customer,case when isnull(IsGatePassGenerated,0) =0 then 'In' else 'Out' end as [Car In Or Out] FROM   Customer RIGHT OUTER JOIN JobCard ON Customer.Id = JobCard.Customer_Id LEFT OUTER JOIN Vehicle LEFT OUTER JOIN Vehicle_Variant ON Vehicle.Vehicle_Variant_Id = Vehicle_Variant.Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id ON JobCard.Vehicle_Id = Vehicle.Id where JobCard.id>0 ";
        if (IsCash && IsCheque && IsPending && IsNeft)
        {

        }
        else
        {
            string strfilt = " And (";
            if (IsCash)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += " ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cash%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%Cash%'))";
                }
            }
            if (IsCheque)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += " ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%Cheque%'))";
                }
                else
                {
                    strfilt += " or ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%Cheque%'))";
                }
            }
            if (IsPending)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += "  ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%pending%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%pending%'))";
                }
                else
                {
                    strfilt += " or ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%pending%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%pending%'))";
                }
            }
            if (IsNeft)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += " ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%NEFT%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%NEFT%'))";
                }
                else
                {
                    strfilt += " or ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%NEFT%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%NEFT%'))";
                }
            }


            //&& isIMPS && isRTGS && isCCard && isDCard && isUPI && isAEPS && isUSSD && isDisputed
            if (isIMPS)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += " ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%IMPS%'))";
                }
                else
                {
                    strfilt += " or ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%IMPS%'))";
                }
            }
            // isCCard && isDCard && isUPI && isAEPS && isUSSD && isDisputed
            if (isRTGS)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += " ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%RTGS%'))";
                }
                else
                {
                    strfilt += " or ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%RTGS%'))";
                }
            }

            // isDCard && isUPI && isAEPS && isUSSD && isDisputed
            if (isCCard)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += " ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%CCard%'))";
                }
                else
                {
                    strfilt += " or ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%CCard%'))";
                }
            }
            //  isUPI && isAEPS && isUSSD && isDisputed
            if (isDCard)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += " ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%DCard%'))";
                }
                else
                {
                    strfilt += " or ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%DCard%'))";
                }
            }

            //isAEPS && isUSSD && isDisputed
            if (isUPI)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += " ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%UPI%'))";
                }
                else
                {
                    strfilt += " or ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%UPI%'))";
                }
            }

            //isUSSD && isDisputed
            if (isAEPS)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += " ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%AEPS%'))";
                }
                else
                {
                    strfilt += " or ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%AEPS%'))";
                }
            }

            // && isDisputed
            if (isUSSD)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += " ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%USSD%'))";
                }
                else
                {
                    strfilt += " or ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%USSD%'))";
                }
            }

            // && isDisputed
            if (isDisputed)
            {
                if (strfilt.Equals(" And ("))
                {
                    strfilt += " ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%Dispute%'))";
                }
                else
                {
                    strfilt += " or ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%Dispute%'))";
                }
            }



            strfilt += " ) ";

            Str += strfilt;
        }
        if (!String.IsNullOrWhiteSpace(StartDate))
        {
            Str += " And (JobCard.doc>='" + StartDate + " 00:00:00.000' and JobCard.doc<='" + EndDate + " 23:59:59.000') ";
        }
        //if (!String.IsNullOrWhiteSpace(StartDate))
        //{
        //    Str += " And JobCard.doc>='" + StartDate + " 00:00:00.000' ";
        //}
        //if (!String.IsNullOrWhiteSpace(EndDate))
        //{
        //    Str += " And JobCard.doc<='" + EndDate + " 23:59:59.000' ";
        //}

        if (isIn && isOut)
        {

        }
        else
        {
            if (isIn)
            {
                Str += " And isnull(IsGatePassGenerated,0) =0 ";
            }
            if (isOut)
            {
                Str += " And isnull(IsGatePassGenerated,0) =1 ";
            }
        }
        if (OnlyInsurance)
        {
            Str += " and isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0)>0 ";
        }
        return obj.GetDataTable(Str + " order by JobCard.DOC ");
    }

    public DataTable JobCardPaymentV33(bool IsWash, bool OnlyInsurance, bool IsNeft, bool IsCash, bool IsCheque, bool IsPending, string StartDate, string EndDate, bool isIMPS, bool isRTGS, bool isCCard, bool isDCard, bool isUPI, bool isAEPS, bool isUSSD, bool isDisputed, bool isIn, bool isOut)
    {
        String Str = "Select [Start Date],[Job Card Id],[Type],[Vehicle],Customer,[Car In Or Out] from (SELECT  CONVERT(VARCHAR(12), JobCard.DOC, 107)+' '+Convert(varchar,(datepart(hour,jobcard.DOc)))+':'+Convert(varchar,datepart(minute,jobcard.DOc)) as [Start Date], JobCard.Id [Job Card Id],(Select name from jobtype where id=jobcard.type) as Type ,isnull((select top 1 iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Spare_Mapping.IsDeleted,0)=0),0)+iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Service_Mapping.IsDeleted,0)=0),0) FROM [dbo].[Estimate] where JobCardId=JobCard.Id order by  id desc  ),0)+isnull((select top 1 isnull(JobCard_Insurance_Mapping.Salvage,0)+isnull(JobCard_Insurance_Mapping.VoluntaryExcess,0)+isnull(JobCard_Insurance_Mapping.CompusaryExcess,0) from JobCard_Insurance_Mapping where JobCardId=JobCard.Id),0) as [Est A],case when ((isnull((Select Sum(Amount) from  [dbo].[JobCardPayment] where jobcardid=JobCard.Id),0)+10)>=(isnull((select top 1 iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Spare_Mapping.IsDeleted,0)=0),0)+iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Service_Mapping.IsDeleted,0)=0),0) FROM [dbo].[Estimate] where JobCardId=JobCard.Id order by  id desc  ),0)+isnull((select top 1 isnull(JobCard_Insurance_Mapping.Salvage,0)+isnull(JobCard_Insurance_Mapping.VoluntaryExcess,0)+isnull(JobCard_Insurance_Mapping.CompusaryExcess,0) from JobCard_Insurance_Mapping where JobCardId=JobCard.Id),0)) and (Select COUNT(id) from JobCardPayment where JobCardId=JobCard.Id)>0) then 'Y' else 'N' end as IsPaid  , isnull((SELECT sum(Amount) FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),0) as [T Paid A],isnull(CustomerInvoiceAmount,0) as [Cust Est A] , Isnull((SELECT Sum(isnull([Amount],0))   FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer') ,0) as [Paid By Cust], Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10)) [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(max)'),1,2,' ')) ,'PENDING') as [C P Mode] ,case when isnull(JobCard.HavingInsurance,0) >0 then 'Y' else 'NA' end as IsInsurance ,isnull(InsuranceInvoiceAmount,0) as [Ins Est A] , Isnull((SELECT Sum(isnull([Amount],0))   FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Insurance') ,0) as [Paid By Ins], isnull((STUFF((SELECT ',' + CAST([PaymentType] AS VARCHAR(10)) [text()] FROM  [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) as [I P Mode] ,isnull((Select top 1 GstInvoiceNumber from Invoice where  isnull(iscancelled,0)=0 and  JobCardId=JobCard.Id and Type=2),0) as [Inv No Ins] ,isnull((Select top 1 FinalTotal from Invoice where JobCardId=JobCard.Id and Type=2),0) as [Invoice A Ins],isnull((Select top 1 convert(varchar(12),Doc,113) from Invoice where JobCardId=JobCard.Id and Type=2),0) as [Inv Dt Ins],isnull((Select top 1 GstInvoiceNumber from Invoice where isnull(iscancelled,0)=0 and JobCardId=JobCard.Id and Type=1),0) as [Inv No Cust],isnull((Select top 1 FinalTotal from Invoice where JobCardId=JobCard.Id and Type=1),0) as [Invoice A Cust],isnull((Select top 1 convert(varchar(12),Doc,113) from Invoice where JobCardId=JobCard.Id and Type=1),0) as [Inv Dt Cust]   , isnull(CONVERT(VARCHAR(12), JobCard.GatePassGeneratedDate, 107),'-')+' '+Convert(varchar,(datepart(hour,jobcard.GatePassGeneratedDate)))+':'+Convert(varchar,datepart(minute,jobcard.GatePassGeneratedDate)) as [Out Date] ,isnull(Vehicle_Model.Name,'-') +' ,'+ isnull(Vehicle_Brand.Name,'-') + ' ,'+ isnull(Vehicle.Number,'-') as Vehicle,isnull(Customer.Name,'-') AS Customer,case when isnull(IsGatePassGenerated,0) =0 then 'In' else 'Out' end as [Car In Or Out] FROM   Customer RIGHT OUTER JOIN JobCard ON Customer.Id = JobCard.Customer_Id LEFT OUTER JOIN Vehicle LEFT OUTER JOIN Vehicle_Variant ON Vehicle.Vehicle_Variant_Id = Vehicle_Variant.Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id ON JobCard.Vehicle_Id = Vehicle.Id where JobCard.id>0 ";
        
        //if (!String.IsNullOrWhiteSpace(StartDate))
        //{
        //    Str += " And JobCard.doc>='" + StartDate + " 00:00:00.000' ";
        //}
        //if (!String.IsNullOrWhiteSpace(EndDate))
        //{
        //    Str += " And JobCard.doc<='" + EndDate + " 23:59:59.000' ";
        //}

        Str += " And isnull(IsGatePassGenerated,0) =0 ";
        return obj.GetDataTable(Str + " ) as c ");
    }
    public DataTable PurchaseImageGrnid()
    {
        dbConnection dbc = new dbConnection();
        string year1 = dbc.getindiantime().ToString("yyyy");
        string mo = dbc.getindiantime().ToString("MM");
        int monthcheck;
        int.TryParse(mo.ToString(), out monthcheck);
        if (monthcheck <= 3)
        {
            year1 = dbc.getindiantime().AddYears(-1).ToString("yyyy");
        }



        string Query12 = "select Id,Convert(varchar(MAX),Id) as Name from grn where  Doc>='" + year1 + "-04-01 00:00:00.000'  order by id desc";

        DataTable dt = obj.GetDataTable(Query12);
        DataRow newRow = dt.NewRow();
        newRow[0] = 0;
        newRow[1] = "Select GRNID";
        dt.Rows.InsertAt(newRow, 0);
        return dt;
    }
    public DataTable JobCardPaymentV3(string StartDate, string EndDate, bool isIn, bool isOut)
    {
        String Str = " SELECT  JobCard.Id [Job Card Id]," + (isOut ? " CONVERT(VARCHAR(12),JobCard.GatePassGeneratedDate, 107)+' '+Convert(varchar,(datepart(hour,GatePassGeneratedDate)))+':'+Convert(varchar,datepart(minute,GatePassGeneratedDate)) as [Out Date] " : " CONVERT(VARCHAR(12),JobCard.DOC, 107)+' '+Convert(varchar,(datepart(hour,JobCard.DOC)))+':'+Convert(varchar,datepart(minute,JobCard.DOC)) as [In Date] ") + " , SUBSTRING(Vehicle_Brand.Name,1,4)+','+isnull((select name from Vehicle_Model where id=(select Vehicle_Model_Id from Vehicle where id=JobCard.Vehicle_Id)),'') as [Brand And Model],Vehicle.Number,(select name from customer where id=Customer_Id) as [Cust Name],(Select name from JobType where id=Type) as Type ,case when JobCard.Type=2 then (select name from InsuranceProvider where id=(select Vehicle_Insurance.Insurance_Provider_Id  from Vehicle_Insurance where Vehicle_Insurance.VehicleId=Vehicle_Id) ) else '' end as [ins] ,isnull(CustomerInvoiceAmount,0) as [Cust Est A] ,isnull(InsuranceInvoiceAmount,0) as [Ins Est A] ,isnull((select top 1 iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Spare_Mapping.IsDeleted,0)=0),0)+iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Service_Mapping.IsDeleted,0)=0),0) FROM [dbo].[Estimate] where JobCardId=JobCard.Id order by  id desc  ),0)+isnull((select top 1 isnull(JobCard_Insurance_Mapping.Salvage,0)+isnull(JobCard_Insurance_Mapping.VoluntaryExcess,0)+isnull(JobCard_Insurance_Mapping.CompusaryExcess,0) from JobCard_Insurance_Mapping where JobCardId=JobCard.Id),0) as [Est A], Isnull((SELECT Sum(isnull([Amount],0)) FROM [JobCardPayment]  where  Convert(datetime,DOP) >= '" + StartDate + " 00:00:00.000' and Convert(datetime,DOP)<='" + EndDate + " 23:59:59.000' and  jobcardid=JobCard.Id and [JobCardPayment].PaymentType in ('Cash','dispute')) ,0) as [Cash] , Isnull((SELECT Sum(isnull([Amount],0)) FROM [JobCardPayment]  where  Convert(datetime,DOP) >= '" + StartDate + " 00:00:00.000' and Convert(datetime,DOP)<='" + EndDate + " 23:59:59.000'  and jobcardid=JobCard.Id and [JobCardPayment].PaymentType='Cheque') ,0) as [Cheque] , Isnull((SELECT Sum(isnull([Amount],0)) FROM [JobCardPayment]  where  Convert(datetime,DOP) >= '" + StartDate + " 00:00:00.000' and Convert(datetime,DOP)<='" + EndDate + " 23:59:59.000' and  jobcardid=JobCard.Id and [JobCardPayment].PaymentType in ('CCard','DCard')) ,0) as [Card], Isnull((SELECT Sum(isnull([Amount],0)) FROM [JobCardPayment]  where  Convert(datetime,DOP) >= '" + StartDate + " 00:00:00.000' and Convert(datetime,DOP)<='" + EndDate + " 23:59:59.000' and  jobcardid=JobCard.Id and [JobCardPayment].PaymentType in ('NEFT')) ,0) as [NEFT],isnull((Select top 1 comment from JobcardwithoutPayment where jobcardId=JobCard.Id and IsDelete=0 order by id desc),'') as Comment,isnull((STUFF((SELECT ', ' + CAST((Select name from [Users] where id=[JobCardPayment].CollcetedBy) AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),'') as CollctedBy FROM Customer RIGHT OUTER JOIN JobCard ON Customer.Id = JobCard.Customer_Id LEFT OUTER JOIN Vehicle LEFT OUTER JOIN Vehicle_Variant ON Vehicle.Vehicle_Variant_Id = Vehicle_Variant.Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id ON JobCard.Vehicle_Id = Vehicle.Id where JobCard.id>0 ";

        if (!String.IsNullOrWhiteSpace(StartDate))
        {
            //Str += " And ( "+(isOut?"":" (JobCard.doc>='" + StartDate + " 00:00:00.000' and JobCard.doc<='" + EndDate + " 23:59:59.000') or ")+" (JobCard.GatePassGeneratedDate>='" + StartDate + " 00:00:00.000' and JobCard.GatePassGeneratedDate <= '" + EndDate + " 23:59:59.000' ) or JobCard.Id in (Select JobCardId from JobCardPayment where Convert(datetime,DOP) >= '" + StartDate + " 00:00:00.000' and Convert(datetime,DOP)<='" + EndDate + " 23:59:59.000' ) ) ";
            Str += " And ( " + (isOut ? " (JobCard.GatePassGeneratedDate>='" + StartDate + " 00:00:00.000' and JobCard.GatePassGeneratedDate <= '" + EndDate + " 23:59:59.000' ) or " : " ( JobCard.doc<='" + EndDate + " 23:59:59.000') or ") + "  JobCard.Id in (Select JobCardId from JobCardPayment where Convert(datetime,DOP) >= '" + StartDate + " 00:00:00.000' and Convert(datetime,DOP)<='" + EndDate + " 23:59:59.000' " + (isOut ? " and JobCard.GatePassGeneratedDate <= '" + EndDate + " 23:59:59.000' " : "") + " ) ) ";
        }
        if (isIn && isOut)
        {

        }
        else
        {
            if (isIn)
            {
                Str += " And (JobCard.GatePassGeneratedDate > '" + EndDate + " 23:59:59.000' or JobCard.GatePassGeneratedDate is null )";
            }
            if (isOut)
            {
                Str += " And isnull(IsGatePassGenerated,0) =1 ";
            }
        }
        DataTable dt = obj.GetDataTable(Str + " order by " + (isOut ? " Type " : " Type "));
        return dt;
    }
    public DataTable JobCardPaymentV4(string StartDate, string EndDate, bool isIn, bool isOut)
    {
        String Str = " SELECT  JobCard.Id [Job Card Id],CONVERT(VARCHAR(12),JobCard.GatePassGeneratedDate, 107)+' '+Convert(varchar,(datepart(hour,GatePassGeneratedDate)))+':'+Convert(varchar,datepart(minute,GatePassGeneratedDate)) as [Out Date] , SUBSTRING(Vehicle_Brand.Name,1,4)+','+isnull((select name from Vehicle_Model where id=(select Vehicle_Model_Id from Vehicle where id=JobCard.Vehicle_Id)),'') as [Brand And Model],Vehicle.Number,(select name from customer where id=Customer_Id) as [Cust Name],(Select name from JobType where id=Type) as Type  ,case when JobCard.Type=2 then (select name from InsuranceProvider where id=(select Vehicle_Insurance.Insurance_Provider_Id  from Vehicle_Insurance where Vehicle_Insurance.VehicleId=Vehicle_Id) ) else '' end as [ins] ,isnull(CustomerInvoiceAmount,0) as [Cust Est A] ,isnull(InsuranceInvoiceAmount,0) as [Ins Est A] ,isnull((select top 1 iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Spare_Mapping.IsDeleted,0)=0),0)+iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Service_Mapping.IsDeleted,0)=0),0) FROM [dbo].[Estimate] where JobCardId=JobCard.Id order by  id desc  ),0)+isnull((select top 1 isnull(JobCard_Insurance_Mapping.Salvage,0)+isnull(JobCard_Insurance_Mapping.VoluntaryExcess,0)+isnull(JobCard_Insurance_Mapping.CompusaryExcess,0) from JobCard_Insurance_Mapping where JobCardId=JobCard.Id),0) as [Est A], Isnull((SELECT Sum(isnull([Amount],0)) FROM [JobCardPayment]  where  Convert(datetime,DOP) >= '" + StartDate + " 00:00:00.000' and Convert(datetime,DOP)<='" + EndDate + " 23:59:59.000' and  jobcardid=JobCard.Id and [JobCardPayment].PaymentType in ('Cash','dispute')) ,0) as [Cash] , Isnull((SELECT Sum(isnull([Amount],0)) FROM [JobCardPayment]  where  Convert(datetime,DOP) >= '" + StartDate + " 00:00:00.000' and Convert(datetime,DOP)<='" + EndDate + " 23:59:59.000'  and jobcardid=JobCard.Id and [JobCardPayment].PaymentType='Cheque') ,0) as [Cheque] , Isnull((SELECT Sum(isnull([Amount],0)) FROM [JobCardPayment]  where  Convert(datetime,DOP) >= '" + StartDate + " 00:00:00.000' and Convert(datetime,DOP)<='" + EndDate + " 23:59:59.000' and  jobcardid=JobCard.Id and [JobCardPayment].PaymentType in ('CCard','DCard')) ,0) as [Card], Isnull((SELECT Sum(isnull([Amount],0)) FROM [JobCardPayment]  where  Convert(datetime,DOP) >= '" + StartDate + " 00:00:00.000' and Convert(datetime,DOP)<='" + EndDate + " 23:59:59.000' and  jobcardid=JobCard.Id and [JobCardPayment].PaymentType in ('NEFT')) ,0) as [NEFT],isnull((Select top 1 comment from JobcardwithoutPayment where jobcardId=JobCard.Id and IsDelete=0 order by id desc),'') as Comment,isnull((STUFF((SELECT ', ' + CAST((Select name from [Users] where id=[JobCardPayment].CollcetedBy) AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),'') as CollctedBy  FROM Customer RIGHT OUTER JOIN JobCard ON Customer.Id = JobCard.Customer_Id LEFT OUTER JOIN Vehicle LEFT OUTER JOIN Vehicle_Variant ON Vehicle.Vehicle_Variant_Id = Vehicle_Variant.Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id ON JobCard.Vehicle_Id = Vehicle.Id where JobCard.id>0  And isnull(IsGatePassGenerated,0) =1 ";

        if (!String.IsNullOrWhiteSpace(StartDate))
        {
            Str += " And ( (JobCard.GatePassGeneratedDate>='" + StartDate + " 00:00:00.000' and JobCard.GatePassGeneratedDate <= '" + EndDate + " 23:59:59.000' ) or (JobCard.Id in (Select JobCardId from JobCardPayment where Convert(datetime,DOP) >= '" + StartDate + " 00:00:00.000' and Convert(datetime,DOP)<='" + EndDate + " 23:59:59.000') and JobCard.GatePassGeneratedDate <= '" + EndDate + " 23:59:59.000'  ) ) ";

            // Str += " And (JobCard.Id in (Select JobCardId from JobCardPayment where Convert(datetime,DOP) >= '" + StartDate + " 00:00:00.000' and Convert(datetime,DOP)<='" + EndDate + " 23:59:59.000' ) ) ";
        }
        DataTable dt = new DataTable();
        dt.Columns.Add(""); dt.Columns.Add(""); dt.Columns.Add(""); dt.Columns.Add(""); dt.Columns.Add("");
        dt.Columns.Add(""); dt.Columns.Add(""); dt.Columns.Add(""); dt.Columns.Add(""); dt.Columns.Add("");
        dt.Columns.Add(""); dt.Columns.Add(""); dt.Columns.Add(""); dt.Columns.Add(""); dt.Columns.Add("");
        dt.Columns.Add("");
        dt.Columns[1].DataType = typeof(string);
        DataRow drdate = dt.NewRow();
        string ToTime = "";
        ToTime = "23:59:59";
        DateTime dtEndDate = DateTime.Parse(EndDate);
        if (dtEndDate.Date == obj.getindiantime().Date)
        {
            ToTime = obj.getindiantime().ToString("HH:mm:ss");
        }
        drdate[0] = ""; drdate[1] = "From : "; drdate[2] = StartDate + " 00:00:00"; drdate[3] = "To : "; drdate[4] = EndDate + " " + ToTime; drdate[5] = ""; drdate[6] = ""; drdate[7] = ""; drdate[8] = ""; drdate[9] = ""; drdate[10] = ""; drdate[11] = ""; drdate[12] = ""; drdate[13] = ""; drdate[14] = ""; drdate[15] = "";
        dt.Rows.Add(drdate);

        DataRow dr = dt.NewRow();
        dr[0] = "Car Out"; dr[1] = ""; dr[2] = ""; dr[3] = ""; dr[4] = ""; dr[5] = ""; dr[6] = ""; dr[7] = ""; dr[8] = ""; dr[9] = ""; dr[10] = ""; dr[11] = ""; dr[12] = ""; dr[13] = ""; dr[14] = ""; dr[15] = "";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr[0] = "Job Card Id"; dr[1] = "Out Date"; dr[2] = "Brand And Model"; dr[3] = "Number"; dr[4] = "Cust_Name"; dr[5] = "Type"; dr[6] = "Ins_Name"; dr[7] = "Cust Est A"; dr[8] = "Ins Est A"; dr[9] = "Est A"; dr[10] = "Cash"; dr[11] = "Cheque"; dr[12] = "Card"; dr[13] = "NEFT"; dr[14] = "Comment"; dr[15] = "Collected By";
        dt.Rows.Add(dr);

        DataTable dt1 = obj.GetDataTable(Str + " order by  [Type]  ");
        decimal est = 0; decimal cash = 0; decimal cheque = 0; decimal card = 0; decimal neft = 0;
        for (int i = 0; i < dt1.Rows.Count; i++)
        {
            est += Convert.ToDecimal(dt1.Rows[i][9].ToString());
            cash += Convert.ToDecimal(dt1.Rows[i][10].ToString());
            cheque += Convert.ToDecimal(dt1.Rows[i][11].ToString());
            card += Convert.ToDecimal(dt1.Rows[i][12].ToString());
            neft += Convert.ToDecimal(dt1.Rows[i][13].ToString());
            DataRow dr1 = dt.NewRow();
            dr1[0] = dt1.Rows[i][0].ToString(); dr1[1] = dt1.Rows[i][1].ToString(); dr1[2] = dt1.Rows[i][2].ToString(); dr1[3] = dt1.Rows[i][3].ToString(); dr1[4] = dt1.Rows[i][4].ToString(); dr1[5] = dt1.Rows[i][5].ToString(); dr1[6] = dt1.Rows[i][6].ToString(); dr1[7] = dt1.Rows[i][7].ToString(); dr1[8] = dt1.Rows[i][8].ToString(); dr1[9] = dt1.Rows[i][9].ToString(); dr1[10] = dt1.Rows[i][10].ToString(); dr1[11] = dt1.Rows[i][11].ToString(); dr1[12] = dt1.Rows[i][12].ToString(); dr1[13] = dt1.Rows[i][13].ToString(); dr1[14] = dt1.Rows[i][14].ToString();
            dr1[15] = dt1.Rows[i][15].ToString();
            dt.Rows.Add(dr1);
        }
        dr = dt.NewRow();
        dr[0] = "Total : "; dr[1] = ""; dr[2] = ""; dr[3] = ""; dr[4] = ""; dr[5] = ""; dr[6] = ""; dr[7] = "0"; dr[8] = "0"; dr[9] = est.ToString(); dr[10] = cash.ToString(); dr[11] = cheque.ToString(); dr[12] = card.ToString(); dr[13] = neft.ToString(); dr[14] = ""; dr[15] = "";
        dt.Rows.Add(dr);

        dr = dt.NewRow();
        dr[0] = "Car In"; dr[1] = ""; dr[2] = ""; dr[3] = ""; dr[4] = ""; dr[5] = ""; dr[6] = ""; dr[7] = ""; dr[8] = ""; dr[9] = ""; dr[10] = ""; dr[11] = ""; dr[12] = ""; dr[13] = ""; dr[14] = ""; dr[15] = "";
        dt.Rows.Add(dr);
        dr = dt.NewRow();
        dr[0] = "Job Card Id"; dr[1] = "In Date"; dr[2] = "Brand And Model"; dr[3] = "Number"; dr[4] = "Cust_Name"; dr[5] = "Type"; dr[6] = "Ins_Name"; dr[7] = "Cust Est A"; dr[8] = "Ins Est A"; dr[9] = "Est A"; dr[10] = "Cash"; dr[11] = "Cheque"; dr[12] = "Card"; dr[13] = "NEFT"; dr[14] = "Comment"; dr[15] = "Collected By";
        dt.Rows.Add(dr);

        Str = " SELECT  JobCard.Id [Job Card Id],CONVERT(VARCHAR(12), JobCard.DOC, 107)+' '+Convert(varchar,(datepart(hour,JobCard.DOC)))+':'+Convert(varchar,datepart(minute,JobCard.DOC)) as [In Date],  SUBSTRING(Vehicle_Brand.Name,1,4)+','+isnull((select name from Vehicle_Model where id=(select Vehicle_Model_Id from Vehicle where id=JobCard.Vehicle_Id)),'') as [Brand And Model] ,Vehicle.Number,(select name from customer where id=Customer_Id) as [Cust Name],(Select name from JobType where id=Type) as Type ,case when JobCard.Type=2 then (select name from InsuranceProvider where id=(select Vehicle_Insurance.Insurance_Provider_Id  from Vehicle_Insurance where Vehicle_Insurance.VehicleId=Vehicle_Id) ) else '' end as [ins] ,isnull(CustomerInvoiceAmount,0) as [Cust Est A] ,isnull(InsuranceInvoiceAmount,0) as [Ins Est A] ,isnull((select top 1 iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Spare_Mapping.IsDeleted,0)=0),0)+iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Service_Mapping.IsDeleted,0)=0),0) FROM [dbo].[Estimate] where JobCardId=JobCard.Id order by  id desc  ),0)+isnull((select top 1 isnull(JobCard_Insurance_Mapping.Salvage,0)+isnull(JobCard_Insurance_Mapping.VoluntaryExcess,0)+isnull(JobCard_Insurance_Mapping.CompusaryExcess,0) from JobCard_Insurance_Mapping where JobCardId=JobCard.Id),0) as [Est A], Isnull((SELECT Sum(isnull([Amount],0)) FROM [JobCardPayment]  where  Convert(datetime,DOP) >= '" + StartDate + " 00:00:00.000' and Convert(datetime,DOP)<='" + EndDate + " 23:59:59.000' and  jobcardid=JobCard.Id and [JobCardPayment].PaymentType in ('Cash','dispute')) ,0) as [Cash] , Isnull((SELECT Sum(isnull([Amount],0)) FROM [JobCardPayment]  where  Convert(datetime,DOP) >= '" + StartDate + " 00:00:00.000' and Convert(datetime,DOP)<='" + EndDate + " 23:59:59.000'  and jobcardid=JobCard.Id and [JobCardPayment].PaymentType='Cheque') ,0) as [Cheque] , Isnull((SELECT Sum(isnull([Amount],0)) FROM [JobCardPayment]  where  Convert(datetime,DOP) >= '" + StartDate + " 00:00:00.000' and Convert(datetime,DOP)<='" + EndDate + " 23:59:59.000' and  jobcardid=JobCard.Id and [JobCardPayment].PaymentType in ('CCard','DCard')) ,0) as [Card], Isnull((SELECT Sum(isnull([Amount],0)) FROM [JobCardPayment]  where  Convert(datetime,DOP) >= '" + StartDate + " 00:00:00.000' and Convert(datetime,DOP)<='" + EndDate + " 23:59:59.000' and  jobcardid=JobCard.Id and [JobCardPayment].PaymentType in ('NEFT')) ,0) as [NEFT],isnull((Select top 1 comment from JobcardwithoutPayment where jobcardId=JobCard.Id and IsDelete=0 order by id desc),'') as Comment,isnull((STUFF((SELECT ', ' + CAST((Select name from [Users] where id=[JobCardPayment].CollcetedBy) AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),'') as CollctedBy   FROM Customer RIGHT OUTER JOIN JobCard ON Customer.Id = JobCard.Customer_Id LEFT OUTER JOIN Vehicle LEFT OUTER JOIN Vehicle_Variant ON Vehicle.Vehicle_Variant_Id = Vehicle_Variant.Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id ON JobCard.Vehicle_Id = Vehicle.Id where JobCard.id>0   And (JobCard.GatePassGeneratedDate > '" + EndDate + " 23:59:59.000' or JobCard.GatePassGeneratedDate is null ) ";

        if (!String.IsNullOrWhiteSpace(StartDate))
        {
            //Str += " And ((JobCard.doc>='" + StartDate + " 00:00:00.000' and JobCard.doc<='" + EndDate + " 23:59:59.000') or (JobCard.GatePassGeneratedDate>='" + StartDate + " 00:00:00.000' and JobCard.GatePassGeneratedDate <= '" + EndDate + " 23:59:59.000' ) or JobCard.Id in (Select JobCardId from JobCardPayment where DOP >= '" + StartDate + " 00:00:00.000' and DOP<='" + EndDate + " 23:59:59.000' ) ) ";
            Str += "  And (JobCard.doc<='" + EndDate + " 23:59:59.000')  ";
        }
        dt1 = obj.GetDataTable(Str + " order by [Type] ");
        est = 0; cash = 0; cheque = 0; card = 0; neft = 0;
        for (int i = 0; i < dt1.Rows.Count; i++)
        {
            est += Convert.ToDecimal(dt1.Rows[i][9].ToString());
            cash += Convert.ToDecimal(dt1.Rows[i][10].ToString());
            cheque += Convert.ToDecimal(dt1.Rows[i][11].ToString());
            card += Convert.ToDecimal(dt1.Rows[i][12].ToString());
            neft += Convert.ToDecimal(dt1.Rows[i][13].ToString());
            DataRow dr1 = dt.NewRow();
            dr1[0] = dt1.Rows[i][0].ToString(); dr1[1] = dt1.Rows[i][1].ToString(); dr1[2] = dt1.Rows[i][2].ToString(); dr1[3] = dt1.Rows[i][3].ToString(); dr1[4] = dt1.Rows[i][4].ToString(); dr1[5] = dt1.Rows[i][5].ToString(); dr1[6] = dt1.Rows[i][6].ToString(); dr1[7] = dt1.Rows[i][7].ToString(); dr1[8] = dt1.Rows[i][8].ToString(); dr1[9] = dt1.Rows[i][9].ToString(); dr1[10] = dt1.Rows[i][10].ToString(); dr1[11] = dt1.Rows[i][11].ToString(); dr1[12] = dt1.Rows[i][12].ToString(); dr1[13] = dt1.Rows[i][13].ToString(); dr1[14] = dt1.Rows[i][14].ToString(); dr1[15] = dt1.Rows[i][15].ToString();
            dt.Rows.Add(dr1);
        }
        dr = dt.NewRow();
        dr[0] = "Total : "; dr[1] = ""; dr[2] = ""; dr[3] = ""; dr[4] = ""; dr[5] = ""; dr[6] = ""; dr[7] = "0"; dr[8] = "0"; dr[9] = est.ToString(); dr[10] = cash.ToString(); dr[11] = cheque.ToString(); dr[12] = card.ToString(); dr[13] = neft.ToString(); dr[14] = ""; dr[15] = "";
        dt.Rows.Add(dr);
        return dt;
    }
    public DataTable JobCardPaymentV6(bool IsWash, bool OnlyInsurance, bool IsNeft, bool IsCash, bool IsCheque, bool IsPending, string StartDate, string EndDate
        , bool isIMPS, bool isRTGS, bool isCCard, bool isDCard
       , bool isUPI, bool isAEPS, bool isUSSD, bool isDisputed, bool isIn, bool isOut)
    {
        String Str = "SELECT  isnull(JobCard.CustomerReviewComment,'') as CustomerReviewComment,isnull((Select Value from CustomerReviewMaster where id=JobCard.CustomerReviewMasterId),'') as reviewType,(Select JobType.Name from JobType where id=JobCard.Type) as type,CONVERT(VARCHAR(12), JobCard.DOC, 107)+' '+Convert(varchar,(datepart(hour,jobcard.DOc)))+':'+Convert(varchar,datepart(minute,jobcard.DOc)) as [Start Date], JobCard.Id [Job Card Id] ,isnull((select top 1 iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Spare_Mapping.IsDeleted,0)=0),0)+iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Service_Mapping.IsDeleted,0)=0),0) FROM [dbo].[Estimate] where JobCardId=JobCard.Id order by  id desc  ),0)+isnull((select top 1 isnull(JobCard_Insurance_Mapping.Salvage,0)+isnull(JobCard_Insurance_Mapping.VoluntaryExcess,0)+isnull(JobCard_Insurance_Mapping.CompusaryExcess,0) from JobCard_Insurance_Mapping where JobCardId=JobCard.Id),0) as [Est A],case when ((isnull((Select Sum(Amount) from  [dbo].[JobCardPayment] where jobcardid=JobCard.Id),0)+10)>=(isnull((select top 1 iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Spare_Mapping.IsDeleted,0)=0),0)+iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Service_Mapping.IsDeleted,0)=0),0) FROM [dbo].[Estimate] where JobCardId=JobCard.Id order by  id desc  ),0)+isnull((select top 1 isnull(JobCard_Insurance_Mapping.Salvage,0)+isnull(JobCard_Insurance_Mapping.VoluntaryExcess,0)+isnull(JobCard_Insurance_Mapping.CompusaryExcess,0) from JobCard_Insurance_Mapping where JobCardId=JobCard.Id),0)) and (Select COUNT(id) from JobCardPayment where JobCardId=JobCard.Id)>0) then 'Y' else 'N' end as IsPaid  , isnull((SELECT sum(Amount) FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),0) as [T Paid A],isnull(CustomerInvoiceAmount,0) as [Cust Est A] , Isnull((SELECT Sum(isnull([Amount],0))   FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer') ,0) as [Paid By Cust], Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10)) [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(max)'),1,2,' ')) ,'PENDING') as [C P Mode] ,case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'Y' else 'NA' end as IsInsurance ,isnull(InsuranceInvoiceAmount,0) as [Ins Est A] , Isnull((SELECT Sum(isnull([Amount],0))   FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Insurance') ,0) as [Paid By Ins], isnull((STUFF((SELECT ',' + CAST([PaymentType] AS VARCHAR(10)) [text()] FROM  [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) as [I P Mode] ,isnull(( SELECT top 1 [FinalTotal]-(isnull((Select Sum(isnull(TotalDiscount,0)) from Invoice_Spare_Mapping where InvoiceId=Invoice.Id),0)+isnull((Select Sum(isnull(TotalDiscount,0)) from Invoice_Service_Mapping where InvoiceId=Invoice.Id),0)) FROM [dbo].Invoice where JobCardId=JobCard.id order by id desc ),0) as [Invoice A]   , isnull(CONVERT(VARCHAR(12), JobCard.GatePassGeneratedDate, 107),'-')+' '+Convert(varchar,(datepart(hour,jobcard.GatePassGeneratedDate)))+':'+Convert(varchar,datepart(minute,jobcard.GatePassGeneratedDate)) as [Out Date] ,isnull(Vehicle_Model.Name,'-') +' ,'+ isnull(Vehicle_Brand.Name,'-') + ' ,'+ isnull(Vehicle.Number,'-') as Vehicle,isnull(Customer.Name,'-')+' , '+isnull(Customer.Mobile,'-') AS Customer,case when isnull(IsGatePassGenerated,0) =0 then 'In' else 'Out' end as [Car In Or Out] FROM   Customer RIGHT OUTER JOIN JobCard ON Customer.Id = JobCard.Customer_Id LEFT OUTER JOIN Vehicle LEFT OUTER JOIN Vehicle_Variant ON Vehicle.Vehicle_Variant_Id = Vehicle_Variant.Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id ON JobCard.Vehicle_Id = Vehicle.Id where JobCard.id>0 And isnull(IsGatePassGenerated,0) =1 and (isnull(CustomerReviewMasterId,0)=0 or (JobCard.CustomerNextReviewDate>='" + StartDate + "  00:00:00' And JobCard.CustomerNextReviewDate<='" + EndDate + " 23:59:59') ) ";
        //if (!String.IsNullOrWhiteSpace(StartDate) && !String.IsNullOrWhiteSpace(EndDate))
        //{
        //    Str += " And (JobCard.GatePassGeneratedDate>='" + StartDate + " 00:00:00' And JobCard.GatePassGeneratedDate<='" + EndDate + " 23:59:59') or (JobCard.CustomerNextReviewDate>='" + StartDate + "  00:00:00' And JobCard.CustomerNextReviewDate<='" + EndDate + " 23:59:59') ";
        //}
        //if (!String.IsNullOrWhiteSpace(EndDate))
        //{
        //    Str += "  ";
        //}
        return obj.GetDataTable(Str + " order by JobCard.GatePassGeneratedDate desc ");
    }

    public DataTable JobCardPaymentV7(bool IsWash, bool OnlyInsurance, bool IsNeft, bool IsCash, bool IsCheque, bool IsPending, string StartDate, string EndDate
       , bool isIMPS, bool isRTGS, bool isCCard, bool isDCard
      , bool isUPI, bool isAEPS, bool isUSSD, bool isDisputed, bool isIn, bool isOut, bool sat, bool psat, bool dsat)
    {
        String Str = "SELECT (Select Count(id) from CustomerReviewCallHistory where JobcardId=JobCard.Id) as [R Cnt],isnull(CONVERT(VARCHAR(6), JobCard.CustomerReviewDate, 113)+','+SUBSTRING(CONVERT(VARCHAR(17), JobCard.CustomerReviewDate, 113),10,8),'') as [Review Date],isnull(JobCard.CustomerReviewComment,'') as CustomerReviewComment,isnull((Select Code from CustomerReviewMaster where id=JobCard.CustomerReviewMasterId),'') as reviewType,(Select JobType.Name from JobType where id=JobCard.Type) as type,CONVERT(VARCHAR(6), JobCard.DOC, 113)+','+SUBSTRING(CONVERT(VARCHAR(17), JobCard.DOC, 113),10,8) as [Start Date], JobCard.Id [Job Card Id] ,isnull((select top 1 iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Spare_Mapping.IsDeleted,0)=0),0)+iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Service_Mapping.IsDeleted,0)=0),0) FROM [dbo].[Estimate] where JobCardId=JobCard.Id order by  id desc  ),0)+isnull((select top 1 isnull(JobCard_Insurance_Mapping.Salvage,0)+isnull(JobCard_Insurance_Mapping.VoluntaryExcess,0)+isnull(JobCard_Insurance_Mapping.CompusaryExcess,0) from JobCard_Insurance_Mapping where JobCardId=JobCard.Id),0) as [Est A],case when ((isnull((Select Sum(Amount) from  [dbo].[JobCardPayment] where jobcardid=JobCard.Id),0)+10)>=(isnull((select top 1 iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Spare_Mapping.IsDeleted,0)=0),0)+iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Service_Mapping.IsDeleted,0)=0),0) FROM [dbo].[Estimate] where JobCardId=JobCard.Id order by  id desc  ),0)+isnull((select top 1 isnull(JobCard_Insurance_Mapping.Salvage,0)+isnull(JobCard_Insurance_Mapping.VoluntaryExcess,0)+isnull(JobCard_Insurance_Mapping.CompusaryExcess,0) from JobCard_Insurance_Mapping where JobCardId=JobCard.Id),0)) and (Select COUNT(id) from JobCardPayment where JobCardId=JobCard.Id)>0) then 'Y' else 'N' end as IsPaid  , isnull((SELECT sum(Amount) FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),0) as [T Paid A],isnull(CustomerInvoiceAmount,0) as [Cust Est A] , Isnull((SELECT Sum(isnull([Amount],0))   FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer') ,0) as [Paid By Cust], Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10)) [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(max)'),1,2,' ')) ,'PENDING') as [C P Mode] ,case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'Y' else 'NA' end as IsInsurance ,isnull(InsuranceInvoiceAmount,0) as [Ins Est A] , Isnull((SELECT Sum(isnull([Amount],0))   FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Insurance') ,0) as [Paid By Ins], isnull((STUFF((SELECT ',' + CAST([PaymentType] AS VARCHAR(10)) [text()] FROM  [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) as [I P Mode] ,isnull(( SELECT top 1 [FinalTotal]-(isnull((Select Sum(isnull(TotalDiscount,0)) from Invoice_Spare_Mapping where InvoiceId=Invoice.Id),0)+isnull((Select Sum(isnull(TotalDiscount,0)) from Invoice_Service_Mapping where InvoiceId=Invoice.Id),0)) FROM [dbo].Invoice where JobCardId=JobCard.id order by id desc ),0) as [Invoice A]   , CONVERT(VARCHAR(6), JobCard.GatePassGeneratedDate, 113)+','+SUBSTRING(CONVERT(VARCHAR(17), JobCard.GatePassGeneratedDate, 113),10,8) as  [Out Date] ,isnull(Vehicle_Model.Name,'-') +' ,'+ isnull(substring(Vehicle_Brand.Name,1,4),'-') + ' ,'+ isnull(Vehicle.Number,'-') as Vehicle,isnull(Customer.Name,'-')+' , '+isnull(Customer.Mobile,'-') AS Customer,case when isnull(IsGatePassGenerated,0) =0 then 'In' else 'Out' end as [Car In Or Out] FROM   Customer RIGHT OUTER JOIN JobCard ON Customer.Id = JobCard.Customer_Id LEFT OUTER JOIN Vehicle LEFT OUTER JOIN Vehicle_Variant ON Vehicle.Vehicle_Variant_Id = Vehicle_Variant.Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id ON JobCard.Vehicle_Id = Vehicle.Id where JobCard.id>0 And isnull(IsGatePassGenerated,0) =1 and (JobCard.CustomerReviewDate>='" + StartDate + "  00:00:00' And JobCard.CustomerReviewDate<='" + EndDate + " 23:59:59') ";
        //if (!String.IsNullOrWhiteSpace(StartDate) && !String.IsNullOrWhiteSpace(EndDate))
        //{
        //    Str += " And (JobCard.GatePassGeneratedDate>='" + StartDate + " 00:00:00' And JobCard.GatePassGeneratedDate<='" + EndDate + " 23:59:59') or (JobCard.CustomerNextReviewDate>='" + StartDate + "  00:00:00' And JobCard.CustomerNextReviewDate<='" + EndDate + " 23:59:59') ";
        //}
        //if (!String.IsNullOrWhiteSpace(EndDate))
        //{
        //    Str += "  ";
        //}
        string strFilt = "";
        if (sat)
        {
            strFilt += "1";
        }
        if (dsat)
        {
            if (String.IsNullOrWhiteSpace(strFilt))
                strFilt += "4";
            else
                strFilt += ",4";
        }
        if (psat)
        {
            if (String.IsNullOrWhiteSpace(strFilt))
                strFilt += "3";
            else
                strFilt += ",3";
        }
        return obj.GetDataTable(Str + " and jobcard.CustomerReviewMasterId in(" + strFilt + ")  " + " order by JobCard.GatePassGeneratedDate desc ");
    }


    public DataTable JobCardPaymentV8(string StartDate, string EndDate, string WorkshopId = "1", int type = 0)
    {
        string Str = "SELECT  CONVERT(VARCHAR(12), JobCard.DOC, 107)+' '+Convert(varchar,(datepart(hour,jobcard.DOc)))+':'+Convert(varchar,datepart(minute,jobcard.DOc)) as [Start Date], JobCard.Id [Job Card Id] ,isnull((select top 1 iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Spare_Mapping.IsDeleted,0)=0),0)+iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Service_Mapping.IsDeleted,0)=0),0) FROM [dbo].[Estimate] where JobCardId=JobCard.Id order by  id desc  ),0)+isnull((select top 1 isnull(JobCard_Insurance_Mapping.Salvage,0)+isnull(JobCard_Insurance_Mapping.VoluntaryExcess,0)+isnull(JobCard_Insurance_Mapping.CompusaryExcess,0) from JobCard_Insurance_Mapping where JobCardId=JobCard.Id),0) as [Est A],case when ((isnull((Select Sum(Amount) from  [dbo].[JobCardPayment] where jobcardid=JobCard.Id),0)+10)>=(isnull((select top 1 iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Spare_Mapping.IsDeleted,0)=0),0)+iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Service_Mapping.IsDeleted,0)=0),0) FROM [dbo].[Estimate] where JobCardId=JobCard.Id order by  id desc  ),0)+isnull((select top 1 isnull(JobCard_Insurance_Mapping.Salvage,0)+isnull(JobCard_Insurance_Mapping.VoluntaryExcess,0)+isnull(JobCard_Insurance_Mapping.CompusaryExcess,0) from JobCard_Insurance_Mapping where JobCardId=JobCard.Id),0)) and (Select COUNT(id) from JobCardPayment where JobCardId=JobCard.Id)>0) then 'Y' else 'N' end as IsPaid  , isnull((SELECT sum(Amount) FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),0) as [T Paid A],isnull(CustomerInvoiceAmount,0) as [Cust Est A] , Isnull((SELECT Sum(isnull([Amount],0))   FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer') ,0) as [Paid By Cust], Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10)) [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(max)'),1,2,' ')) ,'PENDING') as [C P Mode] ,case when isnull(JobCard.HavingInsurance,0) >0 then 'Y' else 'NA' end as IsInsurance ,isnull(InsuranceInvoiceAmount,0) as [Ins Est A] , Isnull((SELECT Sum(isnull([Amount],0))   FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Insurance') ,0) as [Paid By Ins], isnull((STUFF((SELECT ',' + CAST([PaymentType] AS VARCHAR(10)) [text()] FROM  [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) as [I P Mode] ,isnull((Select SUM(Invoice.FinalTotal) from Invoice where jobcardid=jobcard.Id),0) as [Invoice A I+C]   , isnull(CONVERT(VARCHAR(12), JobCard.GatePassGeneratedDate, 107),'-')+' '+Convert(varchar,(datepart(hour,jobcard.GatePassGeneratedDate)))+':'+Convert(varchar,datepart(minute,jobcard.GatePassGeneratedDate)) as [Out Date] ,isnull(Vehicle_Model.Name,'-') +' ,'+ isnull(Vehicle_Brand.Name,'-') + ' ,'+ isnull(Vehicle.Number,'-') as Vehicle,isnull(Customer.Name,'-') AS Customer,case when isnull(IsGatePassGenerated,0) =0 then 'In' else 'Out' end as [Car In Or Out],IsClosePaymentRequested FROM   Customer RIGHT OUTER JOIN JobCard ON Customer.Id = JobCard.Customer_Id LEFT OUTER JOIN Vehicle LEFT OUTER JOIN Vehicle_Variant ON Vehicle.Vehicle_Variant_Id = Vehicle_Variant.Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id ON JobCard.Vehicle_Id = Vehicle.Id where JobCard.id>0 and (case when ((isnull((Select Sum(Amount) from  [dbo].[JobCardPayment] where jobcardid=JobCard.Id),0)+10)>=(isnull((select top 1 iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Spare_Mapping.IsDeleted,0)=0),0)+iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Service_Mapping.IsDeleted,0)=0),0) FROM [dbo].[Estimate] where JobCardId=JobCard.Id order by  id desc  ),0)+isnull((select top 1 isnull(JobCard_Insurance_Mapping.Salvage,0)+isnull(JobCard_Insurance_Mapping.VoluntaryExcess,0)+isnull(JobCard_Insurance_Mapping.CompusaryExcess,0) from JobCard_Insurance_Mapping where JobCardId=JobCard.Id),0)) and (Select COUNT(id) from JobCardPayment where JobCardId=JobCard.Id)>0) then 'Y' else 'N' end)='N' and isnull((select top 1 iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Spare_Mapping.IsDeleted,0)=0),0)+iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Service_Mapping.IsDeleted,0)=0),0) FROM [dbo].[Estimate] where JobCardId=JobCard.Id order by  id desc  ),0)+isnull((select top 1 isnull(JobCard_Insurance_Mapping.Salvage,0)+isnull(JobCard_Insurance_Mapping.VoluntaryExcess,0)+isnull(JobCard_Insurance_Mapping.CompusaryExcess,0) from JobCard_Insurance_Mapping where JobCardId=JobCard.Id),0)>0  and IsGatePassGenerated=1 ";
        if (!String.IsNullOrWhiteSpace(StartDate))
        {
            //Str += " And (JobCard.doc>='" + StartDate + " 00:00:00.000' and JobCard.doc<='" + EndDate + " 23:59:59.000') ";
            Str += "and ((JobCard.id in(Select JobCardid from invoice Where invoice.DOC>='" + StartDate + " 00:00:00' and invoice.DOC<='" + EndDate + " 23:59:59') ) or (JobCard.id not in(Select JobCardid from invoice) and  GatePassGeneratedDate>='" + StartDate + " 00:00:00' and GatePassGeneratedDate<='" + EndDate + " 23:59:59' )) ";
        }
        DataTable dt = obj.GetDataTable(Str);
        try
        {
            if (type == 1)
                dt = dt.Select("([Cust Est A]-[Paid By Cust])>0").CopyToDataTable();
            if (type == 2)
                dt = dt.Select("([Ins Est A]-[Paid By Ins])>0").CopyToDataTable();
        }
        catch (Exception E) { }
        return dt;
    }
    public DataTable JobCardPayment1(string StartDate, string EndDate)
    {
        bool OnlyInsurance = true; bool IsNeft = true; bool IsCash = true; bool IsCheque = true; bool IsPending = true;
        String Str = " SELECT  isnull(InsuranceInvoiceAmount,0) as InsuranceInvoiceAmount,isnull(CustomerInvoiceAmount,0) as CustomerInvoiceAmount,isnull((Select isnull((Select sum(isnull(TotalActualAmount,0)) from Invoice_Spare_Mapping where InvoiceId=Invoice.Id),0)+isnull((Select sum(isnull(TotalActualAmount,0)) from Invoice_Service_Mapping where (InvoiceId=Invoice.Id)),0) from Invoice where JobCardid=JobCard.Id),0) as invoiceAmount,isnull((Select Top 1 EstimateTotal from Estimate where JobCardId=jobcard.Id order by id desc),0) as Estimate,JobCard.Id jobcardid,CONVERT(VARCHAR(12), JobCard.DOC, 107) as StartDate, isnull(CONVERT(VARCHAR(12), JobCard.Job_Close_Date, 107),'-') as EndDate, isnull(Vehicle_Model.Name,'-') +' ,'+ isnull(Vehicle_Brand.Name,'-') + ' ,'+ isnull(Vehicle.Number,'-') as Vehicle, isnull(Customer.Name,'-') AS CustomerName,isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) as PaidBy_Insurance,Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') as PaidBy_Customer,case when (isnull((Select Sum(Amount) from  [dbo].[JobCardPayment] where jobcardid=JobCard.Id),0)+10)>=(isnull((Select isnull((Select sum(isnull(TotalActualAmount,0)) from Invoice_Spare_Mapping where InvoiceId=Invoice.Id),0)+isnull((Select sum(isnull(TotalActualAmount,0)) from Invoice_Service_Mapping where (InvoiceId=Invoice.Id)),0) from Invoice where JobCardid=JobCard.Id),0)) then 'Y' else 'N' end as Paid, isnull((SELECT sum(Amount) FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),0) as PaidAmount,case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'Y' else 'NA' end as HavingInsurance FROM   Customer RIGHT OUTER JOIN JobCard ON Customer.Id = JobCard.Customer_Id LEFT OUTER JOIN Vehicle LEFT OUTER JOIN Vehicle_Variant ON Vehicle.Vehicle_Variant_Id = Vehicle_Variant.Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id ON JobCard.Vehicle_Id = Vehicle.Id where JobCard.id>0 ";
        if (IsCash && IsCheque && IsPending && IsNeft)
        {

        }
        else
        {
            //if (IsCash && IsCheque)
            //{
            //    Str += " AND (isnull((SELECT top 1 [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='CASH' OR isnull((SELECT top 1 [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='CHEQUE')";
            //}
            //else if (IsCash && IsPending)
            //{
            //    Str += " AND (isnull((SELECT top 1 [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='CASH' OR isnull((SELECT [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='PENDING')";
            //}
            //else if (IsPending && IsCheque)
            //{
            //    Str += " AND (isnull((SELECT top 1 [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='CHEQUE' OR isnull((SELECT top 1 [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='PENDING')";
            //}
            //else 
            if (IsCash)
            {
                Str += " and ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cash%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%Cash%'))";
            }
            else if (IsCheque)
            {
                Str += " and ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%Cheque%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%Cheque%'))";
            }
            else if (IsPending)
            {
                Str += " and ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%pending%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%pending%'))";
            }
            else if (IsNeft)
            {
                Str += " and ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%NEFT%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%NEFT%'))";
            }
        }
        if (!String.IsNullOrWhiteSpace(StartDate))
        {
            Str += " And JobCard.doc>='" + StartDate + "'";
        }
        if (!String.IsNullOrWhiteSpace(EndDate))
        {
            Str += " And JobCard.doc<='" + EndDate + "'";
        }
        if (OnlyInsurance)
        {
            Str += " and isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0)>0 ";
        }
        return obj.GetDataTable(Str);
    }
    public DataTable GetJobCardPaymentsByJobcardId(string Id)
    {
        string query = " SELECT Id,[JobCardId],[PaymentType],[ChequeNumber],[BankName],[TransactionNumber],[Amount],isnull(SUBSTRING(replace(ChequeDate,'Jan  1 1900 12:00AM',''), 1, 12),'')  as ChequeDate,isnull(SUBSTRING(replace(DOP,'Jan  1 1900 12:00AM',''), 1, 12),'') as Payment_Received_Date,Remark,isnull(ReceiptNo,'') as ReceiptNo,isnull(PaidBy,'') as PaidBy FROM [dbo].[JobCardPayment] where JobCardid=" + Id;
        return obj.GetDataTable(query);
    }
    public DataTable GetJobCardPaymentsByJobcardId_Insurance(string Id)
    {
        string query = " SELECT Id,[JobCardId],[PaymentType],[ChequeNumber],[BankName],[TransactionNumber],[Amount],isnull(SUBSTRING(replace(ChequeDate,'Jan  1 1900 12:00AM',''), 1, 12),'')  as ChequeDate,isnull(SUBSTRING(replace(DOP,'Jan  1 1900 12:00AM',''), 1, 12),'') as Payment_Received_Date,Remark,isnull(ReceiptNo,'') as ReceiptNo,isnull(PaidBy,'') as PaidBy FROM [dbo].[JobCardPayment] where isnull(PaidBy,'')='Insurance' and JobCardid=" + Id;
        return obj.GetDataTable(query);
    }
    public DataTable GetJobCardPaymentsByJobcardId_Customer(string Id)
    {
        string query = " SELECT Id,[JobCardId],[PaymentType],[ChequeNumber],[BankName],[TransactionNumber],[Amount],isnull(SUBSTRING(replace(ChequeDate,'Jan  1 1900 12:00AM',''), 1, 12),'')  as ChequeDate,isnull(SUBSTRING(replace(DOP,'Jan  1 1900 12:00AM',''), 1, 12),'') as Payment_Received_Date,Remark,isnull(ReceiptNo,'') as ReceiptNo,isnull(PaidBy,'') as PaidBy FROM [dbo].[JobCardPayment] where isnull(PaidBy,'')='Customer' and  JobCardid=" + Id;
        return obj.GetDataTable(query);
    }

    public DataTable AllPartsWithJobcardAndRequisition(string SpareId)
    {
        string str = " SELECT  Spare.Id, Spare.Code + ' , ' + isnull(Spare_Brand.Name,'') AS Brand FROM Spare_Brand INNER JOIN SpareSpareBrandMapping ON Spare_Brand.Id = SpareSpareBrandMapping.BrandId RIGHT OUTER JOIN Spare ON SpareSpareBrandMapping.SpareId = Spare.Id where Spare.Id=" + SpareId;
        return obj.GetDataTable(str);
    }
    public DataTable AllPartsWithJobcardAndRequisition1(string key)
    {
        string str = " SELECT  Spare.Id, Spare.Code + ' , ' + isnull(Spare_Brand.Name,'') AS Brand FROM Spare_Brand INNER JOIN SpareSpareBrandMapping ON Spare_Brand.Id = SpareSpareBrandMapping.BrandId RIGHT OUTER JOIN Spare ON SpareSpareBrandMapping.SpareId = Spare.Id where code like '%" + key + "%' ";
        return obj.GetDataTable(str);
    }
    public DataTable GetJobcardItemById(string SpareId)
    {
        string str = "Select Amount,Id,Quantity,SpareId from JobCard_Spare_Mapping where id =" + SpareId + " and IsDeleted=0  Order by DOC desc";
        return obj.GetDataTable(str);
    }
    public DataTable GetJobcardMove(string SpareId)
    {
        string query = "Select CONVERT(VARCHAR(12), DOC, 109) as [Date],Quantity,JobCardId,Id from JobCard_Spare_Mapping where SpareId=" + SpareId + "  and IsDeleted=0 Order by DOC desc";
        return obj.GetDataTable(query);
    }

    public DataTable GetGRNMove(string SpareId)
    {
        string query = "Select CONVERT(VARCHAR(12), DOC, 109) as [Date],Qty as Quantity,GRNID,Id,GRNDetail.Price from GRNDetail where GRNDetail.EntityId=" + SpareId + " Order by DOC";
        return obj.GetDataTable(query);
    }

    #region Jaydeep
    public DataTable JobCardDetails(string StartDate, string EndDate)
    {
        String Str = "SELECT  (Select name from jobtype where id=jobcard.type) as JobType,CONVERT(VARCHAR(12), JobCard.DOC, 107)+' '+Convert(varchar,(datepart(hour,jobcard.DOc)))+':'+Convert(varchar,datepart(minute,jobcard.DOc)) as [Start Date], JobCard.Id [Job Card Id] ,isnull((select top 1 iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Spare_Mapping.IsDeleted,0)=0),0)+iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Service_Mapping.IsDeleted,0)=0),0) FROM [dbo].[Estimate] where JobCardId=JobCard.Id order by  id desc  ),0) as [Est A],case when ((isnull((Select Sum(Amount) from  [dbo].[JobCardPayment] where jobcardid=JobCard.Id),0)+10)>=isnull((select top 1 iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Spare_Mapping.IsDeleted,0)=0),0)+iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Service_Mapping.IsDeleted,0)=0),0) FROM [dbo].[Estimate] where JobCardId=JobCard.Id order by  id desc ),0) and (Select COUNT(id) from JobCardPayment where JobCardId=JobCard.Id)>0) then 'Y' else 'N' end as IsPaid , isnull((SELECT sum(Amount) FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),0) as [T Paid A],isnull(CustomerInvoiceAmount,0) as [Cust Est A] , Isnull((SELECT Sum(isnull([Amount],0))   FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer') ,0) as [Paid By Cust], Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10)) [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(max)'),1,2,' ')) ,'PENDING') as [C P Mode] ,case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'Y' else 'NA' end as IsInsurance ,isnull(InsuranceInvoiceAmount,0) as [Ins Est A] , Isnull((SELECT Sum(isnull([Amount],0))   FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Insurance') ,0) as [Paid By Ins], isnull((STUFF((SELECT ',' + CAST([PaymentType] AS VARCHAR(10)) [text()] FROM  [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) as [I P Mode] ,isnull(( SELECT top 1 [FinalTotal]  FROM [dbo].Invoice where JobCardId=JobCard.id order by id desc ),0) as [Invoice A] , isnull(CONVERT(VARCHAR(12), JobCard.GatePassGeneratedDate, 107),'-')+' '+Convert(varchar,(datepart(hour,jobcard.GatePassGeneratedDate)))+':'+Convert(varchar,datepart(minute,jobcard.GatePassGeneratedDate)) as [Out Date],isnull(Vehicle_Model.Name,'-') +' ,'+ isnull(Vehicle_Brand.Name,'-') + ' ,'+ isnull(Vehicle.Number,'-') as Vehicle,isnull(Customer.Name,'-') AS Customer,case when isnull(IsGatePassGenerated,0) =0 then 'In' else 'Out' end as [Car In Or Out] FROM   Customer RIGHT OUTER JOIN JobCard ON Customer.Id = JobCard.Customer_Id LEFT OUTER JOIN Vehicle LEFT OUTER JOIN Vehicle_Variant ON Vehicle.Vehicle_Variant_Id = Vehicle_Variant.Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id ON JobCard.Vehicle_Id = Vehicle.Id where JobCard.id>0  ";

        if (!String.IsNullOrWhiteSpace(StartDate))
        {
            Str += " And JobCard.DOC>='" + StartDate + " 00:00:00'";
        }
        if (!String.IsNullOrWhiteSpace(EndDate))
        {
            Str += " And JobCard.DOC<='" + EndDate + " 23:59:59'";
        }
        Str += "order by JobCard.DOC, JobCard.Id";
        DataTable dt = obj.GetDataTable(Str);
        // DataTable dtJobCardDetials = dt.DefaultView.ToTable(true, "Id", "DateOfCreationInRamp", "DateOfCreationInApp", "Customer_name", "vehicle");
        return dt;
    }

    public DataTable GRNReportDetails(string StartDate, string EndDate, string WorkshopId = "1")
    {
        String Str = " SELECT  GRN.Id as GrnId,isnull(CONVERT(VARCHAR(12), Grn.DeliveryDate, 107),'-') as DeliveryDate,GRN.ReceivedBy,(SELECT Name FROM Vendor WHERE (Id = GRN.VendorId)) AS VendorName, GRN.Remark,  GRN.InvoiceNo,DCNo,(select Count(id) from GRNDetail where GRNId=grn.Id ) as Items,STUFF((SELECT distinct ', ' + '<a target=\"_blank\" href=\"../accounts/GenerateInvoice.aspx?AP=1&id='+CAST(JobCardId AS VARCHAR(10))+'\">'+CAST(JobCardId AS VARCHAR(max))+''+'</a>' [text()] FROM Grndetail WHERE GRNId = Grn.ID FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ') List_Output FROM GRN where Id>0 and [WorkshopId]=" + WorkshopId + " and isnull([IsDelete],0)=0 ";

        if (!String.IsNullOrWhiteSpace(StartDate))
        {
            Str += " And GRN.DeliveryDate>='" + StartDate + " 00:00:00' ";
        }
        if (!String.IsNullOrWhiteSpace(EndDate))
        {
            Str += " And GRN.DeliveryDate<='" + EndDate + " 23:59:59'";
        }
        Str += " order by GRN.DeliveryDate desc";
        DataTable dt = obj.GetDataTable(Str);
        return dt;
    }
    public DataTable JobCardPaymentWithInsuranceCheck(bool OnlyInsurance, bool IsNeft, bool IsCash, bool IsCheque, bool IsPending, string StartDate, string EndDate)
    {
        try
        {
            //Select * from (SELECT isnull(InsuranceInvoiceAmount,0) as InsuranceInvoiceAmount,isnull(CustomerInvoiceAmount,0) as CustomerInvoiceAmount,isnull((Select isnull((Select sum(isnull(TotalActualAmount,0)) from Invoice_Spare_Mapping where InvoiceId=Invoice.Id),0)+isnull((Select sum(isnull(TotalActualAmount,0)) from Invoice_Service_Mapping where (InvoiceId=Invoice.Id)),0) from Invoice where JobCardid=JobCard.Id),0) as invoiceAmount,  isnull((Select Top 1 EstimateTotal from Estimate where JobCardId=jobcard.Id order by id desc),0) as Estimate,JobCard.Id jobcardid,CONVERT(VARCHAR(12), JobCard.DOC, 107) as StartDate, isnull(CONVERT(VARCHAR(12), JobCard.Job_Close_Date, 107),'-') as EndDate, isnull(Vehicle_Model.Name,'-') +' ,'+ isnull(Vehicle_Brand.Name,'-') + ' ,'+ isnull(Vehicle.Number,'-') as Vehicle, isnull(Customer.Name,'-') AS CustomerName,isnull((SELECT Top 1 [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING') as PaidBy,case when (Select Count(id) from  [dbo].[JobCardPayment] where jobcardid=JobCard.Id)>0 then 'Y' else 'N' end as Paid,isnull((SELECT Top 1 Amount FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),0) as PaidAmount  ,case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'Y' else 'NA' end as HavingInsurance, case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end as PaidBy_Insurance ,Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') as PaidBy_Customer FROM   Customer RIGHT OUTER JOIN JobCard ON Customer.Id = JobCard.Customer_Id LEFT OUTER JOIN Vehicle LEFT OUTER JOIN Vehicle_Variant ON Vehicle.Vehicle_Variant_Id = Vehicle_Variant.Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id ON JobCard.Vehicle_Id = Vehicle.Id where JobCard.id>0  OR ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%NEFT%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%NEFT%')) ) as TableName 


            String Str = "Select * from (SELECT isnull(InsuranceInvoiceAmount,0) as InsuranceInvoiceAmount,isnull(CustomerInvoiceAmount,0) as CustomerInvoiceAmount,isnull((Select isnull((Select sum(isnull(TotalActualAmount,0)) from Invoice_Spare_Mapping where InvoiceId=Invoice.Id),0)+isnull((Select sum(isnull(TotalActualAmount,0)) from Invoice_Service_Mapping where (InvoiceId=Invoice.Id)),0) from Invoice where JobCardid=JobCard.Id),0) as invoiceAmount,  isnull((Select Top 1 EstimateTotal from Estimate where JobCardId=jobcard.Id order by id desc),0) as Estimate,JobCard.Id jobcardid,CONVERT(VARCHAR(12), JobCard.DOC, 107) as StartDate, isnull(CONVERT(VARCHAR(12), JobCard.Job_Close_Date, 107),'-') as EndDate, isnull(Vehicle_Model.Name,'-') +' ,'+ isnull(Vehicle_Brand.Name,'-') + ' ,'+ isnull(Vehicle.Number,'-') as Vehicle, isnull(Customer.Name,'-') AS CustomerName,isnull((SELECT Top 1 [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING') as PaidBy,case when (Select Count(id) from  [dbo].[JobCardPayment] where jobcardid=JobCard.Id)>0 then 'Y' else 'N' end as Paid,isnull((SELECT Top 1 Amount FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),0) as PaidAmount  ,case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'Y' else 'NA' end as HavingInsurance, case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end as PaidBy_Insurance ,Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') as PaidBy_Customer FROM   Customer RIGHT OUTER JOIN JobCard ON Customer.Id = JobCard.Customer_Id LEFT OUTER JOIN Vehicle LEFT OUTER JOIN Vehicle_Variant ON Vehicle.Vehicle_Variant_Id = Vehicle_Variant.Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id ON JobCard.Vehicle_Id = Vehicle.Id where JobCard.id>0 ";
            if (IsCash && IsCheque && IsPending && IsNeft)
            {
                Str += " OR ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%NEFT%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%NEFT%'))";
            }
            else
            {
                if (IsCash && IsCheque && IsNeft)
                {
                    Str += " AND (isnull((SELECT top 1 [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='CASH' OR isnull((SELECT top 1 [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='CHEQUE') OR "
                    + "( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%NEFT%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%NEFT%'))";

                }
                else if (IsCash && IsPending && IsNeft)
                {
                    Str += " AND (isnull((SELECT top 1 [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='CASH' OR isnull((SELECT top 1 [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='PENDING') OR   " + "( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%NEFT%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%NEFT%'))";
                }

                else if (IsPending && IsCheque && IsNeft)
                {
                    Str += " AND (isnull((SELECT top 1  [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='CHEQUE' OR isnull((SELECT top 1  [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='PENDING') OR   " + "( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%NEFT%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%NEFT%'))";
                }
                else if (IsCash && IsCheque)
                {
                    Str += " AND (isnull((SELECT  top 1  [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='CASH' OR isnull((SELECT top 1  [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='CHEQUE')";
                }

                else if (IsCash && IsPending)
                {
                    Str += " AND (isnull((SELECT  top 1  [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='CASH' OR isnull((SELECT top 1  [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='PENDING')";
                }

                else if (IsPending && IsCheque)
                {
                    Str += " AND (isnull((SELECT  top 1  [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='CHEQUE' OR isnull((SELECT top 1  [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='PENDING')";
                }
                else if (IsCash && IsNeft)
                {
                    Str += " AND (isnull((SELECT  top 1  [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='CASH' ) OR   " + "( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%NEFT%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%NEFT%'))";
                }
                else if (IsCheque && IsNeft)
                {
                    Str += " AND (isnull((SELECT  top 1  [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='CHEQUE' ) OR " + "( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%NEFT%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR (MAX)'),1,2,' ')) ,'PENDING') like '%NEFT%'))";
                }
                else if (IsPending && IsNeft)
                {
                    Str += " AND (isnull((SELECT  top 1  [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='PENDING' ) OR " + "( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%NEFT%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%NEFT%'))";
                }
                else if (IsCash)
                {
                    Str += " AND (isnull((SELECT  top 1  [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='CASH' )";
                }


                else if (IsCheque)
                {
                    Str += " AND (isnull((SELECT  top 1  [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='CHEQUE' )";
                }

                else if (IsPending)
                {
                    Str += " AND (isnull((SELECT  top 1  [PaymentType] FROM [dbo].[JobCardPayment] where jobcardid=JobCard.Id),'PENDING')='PENDING' )";
                }

                else if (IsNeft)
                {
                    Str += " and ( (isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id and PaidBy='Insuarnce' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),case when isnull((Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=JobCard.Vehicle_Id),0) >0 then 'PENDING' else 'NA' end) like '%NEFT%') or (Isnull((STUFF((SELECT ', ' + CAST([PaymentType] AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment]  where jobcardid=JobCard.Id and PaidBy='Customer' FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'PENDING') like '%NEFT%'))";
                }
                if (!String.IsNullOrWhiteSpace(StartDate))
                {
                    Str += " And JobCard.doc>='" + StartDate + "'";
                }
                if (!String.IsNullOrWhiteSpace(EndDate))
                {
                    Str += " And JobCard.doc<='" + EndDate + "'";
                }
            }
            Str += ") as TableName";
            if (OnlyInsurance)
            {
                Str += " where TableName.HavingInsurance='Y'";
            }

            return obj.GetDataTable(Str);

        }
        catch (Exception ex)
        { throw; }
    }

    public DataTable GetSpareWithTax()
    {
        string query = "Select Id, Isnull(Code,'') as Code,Isnull((Select TaxValue from TaxCategory where Id= TaxId),0) as TAX from Spare where IsDeleted=0";
        DataTable dt = obj.GetDataTable(query);
        return dt;

    }
    #endregion

    #region  HEtul

    public DataTable VendorDetails(string StartDate, string EndDate)
    {
        String Str = "select Vendor.id,Name,Mobile,grn.Id as grnid,(Qty*Price)as Prices,grn.doc from Vendor inner join GRN on vendor.Id=VendorId inner join GRNDetail on GRN.Id=GRNId where Vendor.IsDeleted=0 ";

        if (!String.IsNullOrWhiteSpace(StartDate))
        {
            Str += " And grn.doc>='" + StartDate + "'";
        }
        if (!String.IsNullOrWhiteSpace(EndDate))
        {
            Str += " And grn.doc<='" + EndDate + "'";
        }
        Str += "order by VendorId";
        DataTable dt = obj.GetDataTable(Str);
        DataTable dtVendorDetials = dt.DefaultView.ToTable(true, "id", "Name", "Mobile");
        DataTable dtGrnDetails = dt.DefaultView.ToTable(true, "id", "grnid");
        dtVendorDetials.Columns.Add("Grn");
        dtVendorDetials.Columns.Add("Amt", typeof(Double));
        for (int i = 0; i < dtVendorDetials.Rows.Count; i++)
        {
            DataRow[] drdata = dtGrnDetails.Select("id=" + int.Parse(dtVendorDetials.Rows[i]["id"].ToString()) + " ");
            dtVendorDetials.Rows[i]["Grn"] = drdata.Length;
            double amt = 0;
            object amtprice = dt.Compute("sum(Prices)", "id=" + int.Parse(dtVendorDetials.Rows[i]["id"].ToString()) + " ");
            double.TryParse(amtprice.ToString(), out amt);
            dtVendorDetials.Rows[i]["Amt"] = amt;
        }
        return dtVendorDetials;
    }

    public DataTable VendorWiseGRNList(int vendor, string StartDate, string EndDate)
    {
        String Str = "select GRN.Id,ReceivedBy,InvoiceNo,CONVERT(varchar(11),DeliveryDate,103)as date,sum(Qty)as Quantity,sum(Price)as purchase from GRN inner join GRNDetail on grn.Id =GRNId where VendorId=" + vendor + " ";

        if (!String.IsNullOrWhiteSpace(StartDate))
        {
            Str += " And grn.doc>='" + StartDate + "'";
        }
        if (!String.IsNullOrWhiteSpace(EndDate))
        {
            Str += " And grn.doc<='" + EndDate + "'";
        }
        Str += "group by GRN.Id,ReceivedBy,InvoiceNo,DeliveryDate order by GRN.id";
        DataTable dt = obj.GetDataTable(Str);

        return dt;
    }

    public DataTable InvoiceBaseOnGRN(int GRn, ref string vendor, ref string invoice, ref string date)
    {
        String Str = "select Vendor.id,Vendor.Name,Mobile,grn.Id,GRNDetail.Qty,GRNDetail.Price,Discount,InvoiceNo,CONVERT(varchar(11),DeliveryDate,103)as date,spare.Code,TaxValue from Vendor inner join GRN on vendor.Id=VendorId inner join GRNDetail on GRN.Id=GRNId inner join spare on GRNDetail.EntityId=spare.Id inner join TaxCategory on TaxId=TaxCategory.Id where Vendor.IsDeleted=0  And grn.Id = " + GRn + " order by grn.Id ";

        DataTable dt = obj.GetDataTable(Str);

        return dt;
    }

    #endregion

    #region darshan

    public DataTable VendorInvoicesListItemsByVendorId(string VendorId, string StartDate, string EndDate, string WorkshopId)
    {
        DataTable dt = new DataTable();
        if (VendorId != "")
        {
            string Str = "SELECT isnull(GRNDetail.VendorInvoiceRate,0) as Rate,isnull(Grndetail.VendorInvoiceDiscount,0) as Discount,isnull(Grndetail.VendorInvoiceSGST,0) as sgstper,isnull(GRNDetail.VendorInvoiceSGSTAmount,0) as sgst,isnull(grndetail.VendorInvoiceCGST,0) as cgstper,isnull(grndetail.VendorInvoiceCGSTAmount,0) as cgst,isnull(	grndetail.VendorInvoiceIGST,0) as igstper,isnull(grndetail.VendorInvoiceIGSTAmount,0) as igst,isnull(VendorInvoiceTaxable,0) as taxableamt,isnull(grndetail.VendorInvoiceTotal,0) as total,isnull((select name from Vendor where id=Vendorid),'-') as Vendor,GRNDetail.Id, GRNDetail.GRNId,(SELECT Code FROM Spare WHERE (Id = GRNDetail.EntityId)) + ',' + (SELECT Name FROM Spare_Brand WHERE (Id = GRNDetail.BrandId)) AS SpareName, GRNDetail.Qty, GRNDetail.Price, GRNDetail.Discount, GRN.DCNo, isnull(CONVERT(VARCHAR(12), Grn.DeliveryDate, 107),'-') as Delivery_Date, GRN.ReceivedBy,isnull(VendorInvoiceQty,0) as VendorInvoiceQty,isnull(VendorInvoiceComment,'') as VendorInvoiceComment  FROM GRNDetail INNER JOIN GRN ON GRNDetail.GRNId = GRN.Id WHERE (ISNULL(GRNDetail.IsDelete, 0) = 0) AND (ISNULL(GRNDetail.VendorInvoiceId, 0) = 0) and GRN.WorkshopId=" + WorkshopId + " And (VendorId=" + VendorId + " or VendorId=0 )";

            if (!String.IsNullOrWhiteSpace(StartDate))
            {
                Str += " And GRN.DeliveryDate>='" + StartDate + " 00:00:00' ";
            }
            if (!String.IsNullOrWhiteSpace(EndDate))
            {
                Str += " And GRN.DeliveryDate<='" + EndDate + " 23:59:59'";
            }
            Str += " order by GRN.DeliveryDate desc";
            dt = obj.GetDataTable(Str);
            return dt;
        }
        return dt;
    }

    public DataTable VendorInvoicesListItemsByInvoiceId(string InvoiceId, string WorkshopId)
    {
        DataTable dt = new DataTable();
        if (InvoiceId != "")
        {
            string Str = "SELECT isnull(GRNDetail.VendorInvoiceRate,0) as Rate,isnull(Grndetail.VendorInvoiceDiscount,0) as Discount,isnull(Grndetail.VendorInvoiceSGST,0) as sgstper,isnull(GRNDetail.VendorInvoiceSGSTAmount,0) as sgst,isnull(grndetail.VendorInvoiceCGST,0) as cgstper,isnull(grndetail.VendorInvoiceCGSTAmount,0) as cgst,isnull(	grndetail.VendorInvoiceIGST,0) as igstper,isnull(grndetail.VendorInvoiceIGSTAmount,0) as igst,isnull(VendorInvoiceTaxable,0) as taxableamt,isnull(grndetail.VendorInvoiceTotal,0) as total,isnull((select name from Vendor where id=Vendorid),'-') as Vendor,GRNDetail.Id, GRNDetail.GRNId,(SELECT Code FROM Spare WHERE (Id = GRNDetail.EntityId)) + ',' + (SELECT Name FROM Spare_Brand WHERE (Id = GRNDetail.BrandId)) AS SpareName, GRNDetail.Qty, GRNDetail.Price, GRNDetail.Discount, GRN.DCNo, isnull(CONVERT(VARCHAR(12), Grn.DeliveryDate, 107),'-') as Delivery_Date, GRN.ReceivedBy,isnull(VendorInvoiceQty,0) as VendorInvoiceQty,isnull(VendorInvoiceComment,'') as VendorInvoiceComment  FROM GRNDetail INNER JOIN GRN ON GRNDetail.GRNId = GRN.Id WHERE (ISNULL(GRNDetail.IsDelete, 0) = 0)  and GRN.WorkshopId=" + WorkshopId + " AND ISNULL(GRNDetail.VendorInvoiceId, 0)  =" + InvoiceId;
            Str += " order by GRN.DeliveryDate desc";
            dt = obj.GetDataTable(Str);
            return dt;
        }

        return dt;
    }

    #endregion
    public DataTable NoneBrandRequisition()
    {
        string str = "";
        DataTable dataTable = obj.GetDataTable(str);
        for (int i = 0; i < dataTable.Rows.Count; i++)
        {
            if (dataTable.Rows[i]["Grn Ids"] != null)
            {
                string strval = "Select  RequisitionId,(Select  name from spare where id= SpareId) as SpareName,isnull((SELECT  STUFF((SELECT ',' + isnull(CAST(GRN.Id AS VARCHAR(10)),'')+'['+CONVERT(VARCHAR(6), DeliveryDate, 107)+']'  FROM  GRN INNER JOIN GRNDetail ON GRN.Id = GRNDetail.GRNId where GRNDetail.EntityId=Requisition_Spare.SpareId group by GRN.Id,DeliveryDate FOR XML PATH('')), 1, 1, '') AS listStr),'') as [Grn Ids] from Requisition_Spare where id in (SELECT SpareInventaryHistory.RequsitionSpareId FROM  SpareSpareBrandMapping INNER JOIN SpareInventaryHistory ON SpareSpareBrandMapping.Id = SpareInventaryHistory.SpareSpareBrandMappingId WHERE (SpareSpareBrandMapping.BrandId = 34) AND (SpareInventaryHistory.Dr > 0)) and IsAllocate=1 and IsDeleted=0 and RequisitionId in (select id from Requisition where IsDeleted=0)";
                string[] stra = dataTable.Rows[i]["Grn Ids"].ToString().Split(',');
                for (int j = 0; j < stra.Length; j++)
                {
                    if (strval.Length > 0)
                    {
                        strval += ",";
                    }
                    if (!String.IsNullOrWhiteSpace(stra[j]))
                    {
                        string[] stra1 = stra[j].Split('[');
                        if (stra1.Length > 0)
                        {
                            strval += "<a href='../Store/grnEdit.aspx?id=" + stra1[0] + "' target='_blank'>" + stra1[0] + "[" + stra1[1] + "</a>";
                        }
                    }
                }
                dataTable.Rows[i]["Grn Ids"] = strval;
            }
        }
        return dataTable;
    }


    public DataTable JobcardSpare(string jobcardId)
    {
        string query = " SELECT isnull(convert(decimal(18,2),((Amount-InsuranceAmount)/(case when ([Amount]/100)=0 then 1 else ([Amount]/100) end) )),0) as per,'' as isDes,(select name from taxcategory where id =  (Select TaxId from Spare where id=SpareId)) as tax,[Id],(select name from Spare where id= [SpareId]) as Name,[Amount],[Quantity],isnull(InsuranceAmount,0) as InsuranceAmount,(Amount-isnull(InsuranceAmount,0)) as CustomerAmount ,([Quantity]*isnull(InsuranceAmount,0) ) as TotalInsuranceAmount,(Quantity*(Amount-isnull(InsuranceAmount,0))) as TotalCustomerAmount,case when isnull(PushToInsurance,0)=0 then '' else 'checked' end as isChecked   FROM [dbo].[JobCard_Spare_Mapping] where JobCardId=" + jobcardId + " and [IsDeleted]=0 ";
        DataTable dt = obj.GetDataTable(query);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["isChecked"].ToString().Equals("checked"))
            {
                dt.Rows[i]["isChecked"] = "Checked='Checked'";
            }
            else
            {
                dt.Rows[i]["isDes"] = "disabled='disabled'";
            }
        }
        return dt;
    }
    public DataTable JobcardService(string jobcardId)
    {
        string query = " SELECT isnull(convert(decimal(18,2),((Amount-InsuranceAmount)/(case when ([Amount]/100)=0 then 1 else ([Amount]/100) end) )),0) as per,'' as isDes,[Id],(select name from taxcategory where id =  (Select TaxId from Service where id=ServiceId)) as tax,(select name from Service where id= ServiceId) as Name,[Amount],[Quantity],isnull(InsuranceAmount,0) as InsuranceAmount,(Amount-isnull(InsuranceAmount,0)) as CustomerAmount,([Quantity]*isnull(InsuranceAmount,0) ) as TotalInsuranceAmount,(Quantity*(Amount-isnull(InsuranceAmount,0))) as TotalCustomerAmount,case when isnull(PushToInsurance,0)=0 then '' else 'checked' end as isChecked FROM [dbo].[JobCard_Service_Mapping] where JobCardId=" + jobcardId + " and [IsDeleted]=0 ";
        DataTable dt = obj.GetDataTable(query);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            if (dt.Rows[i]["isChecked"].ToString().Equals("checked"))
            {
                dt.Rows[i]["isChecked"] = "Checked='Checked'";
            }
            else
            {
                dt.Rows[i]["isDes"] = "disabled='disabled'";
            }
        }
        return dt;
    }
    public DataTable VendorInvoicesList(string WorkShopId)
    {
        string query = "SELECT [Id],CONVERT(VARCHAR(12), [InvoiceDate], 107) as [InvoiceDate],[InvoiceNumber],[InvoiceAmount],(select name from Vendor where id=Vendorid) as Vendor,CONVERT(VARCHAR(12), DOC, 107) as [DOC],[comment] FROM [dbo].[VendorInvoice] where ISNULL([IsDeleted],0)=0 and WorkshopId=" + WorkShopId + " order by id desc";
        DataTable dt = obj.GetDataTable(query);
        return dt;
    }

    public DataTable VendorInvoicesListItemsByVendorId_Old(string VendorId, string StartDate, string EndDate, string WorkshopId)
    {
        string Str = "SELECT isnull((select name from Vendor where id=Vendorid),'-') as Vendor,GRNDetail.Id, GRNDetail.GRNId,(SELECT Code FROM Spare WHERE (Id = GRNDetail.EntityId)) + ',' + (SELECT Name FROM Spare_Brand WHERE (Id = GRNDetail.BrandId)) AS SpareName, GRNDetail.Qty, GRNDetail.Price, GRNDetail.Discount, GRN.DCNo, isnull(CONVERT(VARCHAR(12), Grn.DeliveryDate, 107),'-') as Delivery_Date, GRN.ReceivedBy,isnull(VendorInvoiceQty,0) as VendorInvoiceQty,isnull(VendorInvoiceComment,'') as VendorInvoiceComment  FROM GRNDetail INNER JOIN GRN ON GRNDetail.GRNId = GRN.Id WHERE (ISNULL(GRNDetail.IsDelete, 0) = 0) AND (ISNULL(GRNDetail.VendorInvoiceId, 0) = 0) and GRN.WorkshopId=" + WorkshopId + " And (VendorId=" + VendorId + " or VendorId=0 )";

        if (!String.IsNullOrWhiteSpace(StartDate))
        {
            Str += " And GRN.DeliveryDate>='" + StartDate + " 00:00:00' ";
        }
        if (!String.IsNullOrWhiteSpace(EndDate))
        {
            Str += " And GRN.DeliveryDate<='" + EndDate + " 23:59:59'";
        }
        Str += " order by GRN.DeliveryDate desc";
        DataTable dt = obj.GetDataTable(Str);
        return dt;
    }
    public DataTable VendorInvoicesListItemsByInvoiceId_Old(string InvoiceId, string WorkshopId)
    {
        string Str = "SELECT isnull((select name from Vendor where id=Vendorid),'-') as Vendor,GRNDetail.Id, GRNDetail.GRNId,(SELECT Code FROM Spare WHERE (Id = GRNDetail.EntityId)) + ',' + (SELECT Name FROM Spare_Brand WHERE (Id = GRNDetail.BrandId)) AS SpareName, GRNDetail.Qty, GRNDetail.Price, GRNDetail.Discount, GRN.DCNo, isnull(CONVERT(VARCHAR(12), Grn.DeliveryDate, 107),'-') as Delivery_Date, GRN.ReceivedBy,isnull(VendorInvoiceQty,0) as VendorInvoiceQty,isnull(VendorInvoiceComment,'') as VendorInvoiceComment  FROM GRNDetail INNER JOIN GRN ON GRNDetail.GRNId = GRN.Id WHERE (ISNULL(GRNDetail.IsDelete, 0) = 0)  and GRN.WorkshopId=" + WorkshopId + " AND ISNULL(GRNDetail.VendorInvoiceId, 0)  =" + InvoiceId;
        Str += " order by GRN.DeliveryDate desc";
        DataTable dt = obj.GetDataTable(Str);
        return dt;
    }
    public DataTable CarInList(string StartDate, string EndDate, bool OnlyAllocated = false, string WorkshopId = "1")
    {
        string Str = "SELECT Id,isnull((Select name from JobType where id=[JobcardTypeId]),'') as Type,(Select number from Vehicle where id=VehicleId) as Number,CONVERT(varchar(17), [DOC] , 113 ) as Dt FROM [dbo].[GateKeeperVehicleEntry] where isnull([JobcardId],0)=0 and isnull(IsDeleted,0)=0 and WorkshopId=" + WorkshopId + "  ";
        if (!String.IsNullOrWhiteSpace(StartDate))
        {
            Str += " And DOC>='" + StartDate + " 00:00:00' ";
        }
        if (!String.IsNullOrWhiteSpace(EndDate))
        {
            Str += " And DOC<='" + EndDate + " 23:59:59'";
        }
        if (OnlyAllocated)
        {
            Str += " And isnull(JobcardTypeId,0)>0 And isnull([JobcardId],0)=0 ";
        }
        Str += " order by DOC desc";
        DataTable dt = obj.GetDataTable(Str);
        return dt;
    }

    public DataTable CarInListReport(string StartDate, string EndDate, string WorkshopId = "1")
    {
        string Str = "Select CONVERT(varchar(17), GateKeeperVehicleEntry.[DOC] , 113 ) as [In Date & Time],isnull(Convert(varchar,Jobcard.Id),'Pending') [Jobcard No],isnull((Select name from JobType where id=GateKeeperVehicleEntry.JobcardTypeId),'pending') as Type,isnull((SELECT        isnull(Vehicle_Brand.Name,'') + ' ,' + isnull(Vehicle_Model.Name,'') + ' ,' + Vehicle.Number AS Expr1 FROM            Vehicle LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id where Vehicle.id=GateKeeperVehicleEntry.VehicleId),'') as Vehicle,isnull((SELECT        Name+' ,'+ Mobile FROM            Customer where id=Customer_Id),'') as customer, Isnull((STUFF((SELECT ', ' + Notes [text()] FROM CustomerNote  where JobCard_id=JobCard.Id  FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')) ,'') as [Customer Demand(s)] from GateKeeperVehicleEntry LEFT OUTER JOIN JobCard on GateKeeperVehicleEntry.JobcardId=JobCard.Id  where GateKeeperVehicleEntry.Id>0 and GateKeeperVehicleEntry.WorkshopId=" + WorkshopId + " ";
        if (!String.IsNullOrWhiteSpace(StartDate))
        {
            Str += " And GateKeeperVehicleEntry.DOC>='" + StartDate + " 00:00:00' ";
        }
        if (!String.IsNullOrWhiteSpace(EndDate))
        {
            Str += " And GateKeeperVehicleEntry.DOC<='" + EndDate + " 23:59:59'";
        }
        Str += " order by GateKeeperVehicleEntry.DOC ";
        DataTable dt = obj.GetDataTable(Str);
        return dt;
    }
    public DataTable JobcardListAccident(string StartDate, string EndDate, string WorkshopId = "1")
    {
        string Str = "SELECT (Select name from JobType where id=JobCard.Type) as type,(Select case when isnull(name,'')='Closed' then '<span class=''label label-success''>'+name+'</span>' when isnull(name,'')='Pending' then '<span class=''label label-danger''>'+name+'</span>' else '<span class=''label label-warning''>'+name+'</span>' end from JobStatus where id=JobCard.JobStatus_Id) as Status,isnull((Select  top 1 GstInvoiceNumber from Invoice where isnull(IsCancelled,0)=0 and JobCardId=JobCard.Id and Invoice.Type=1),'') as InvoiceNo,isnull((Select  top 1 GstInvoiceNumber from Invoice where isnull(IsCancelled,0)=0 and JobCardId=JobCard.Id  and Invoice.Type=2),'') as InvoiceNoI,isnull(CustomerInvoiceAmount,0) as [Cust Est A] ,isnull(InsuranceInvoiceAmount,0) as [Ins Est A],isnull((select top 1 iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Spare_Mapping.IsDeleted,0)=0),0)+iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Service_Mapping.IsDeleted,0)=0),0) FROM [dbo].[Estimate] where JobCardId=JobCard.Id order by  id desc  ),0)+isnull((select top 1 isnull(JobCard_Insurance_Mapping.Salvage,0)+isnull(JobCard_Insurance_Mapping.VoluntaryExcess,0)+isnull(JobCard_Insurance_Mapping.CompusaryExcess,0) from JobCard_Insurance_Mapping where JobCardId=JobCard.Id),0) as [Est A],JobCard.Id, isnull(Customer.Name,'')+' ,'+isnull(Customer.Mobile,'') +' ,'+ isnull(Customer.Email,'') AS [Customer Name], isnull(Vehicle.Number,'')+' ,'+isnull(Vehicle_Brand.Name,'')+' ,'+isnull(Vehicle_Model.Name,'') AS Vehicle#,   CONVERT(varchar(17), JobCard.DOC , 113 )  AS [In Date], CONVERT(varchar(17), JobCard.GatePassGeneratedDate , 113 )  AS [Out Date], isnull(serveur.Name,'') +','+ isnull(serveur.Mobile,'')+','+isnull(serveur.Email,'') AS [Surveyor Detail],  isnull(InsuranceProvider.Name,'') AS [Insurance Provider] FROM JobCard_Insurance_Mapping LEFT OUTER JOIN InsuranceProvider ON JobCard_Insurance_Mapping.Insurance_Provider_Id = InsuranceProvider.Id RIGHT OUTER JOIN JobCard ON JobCard_Insurance_Mapping.Id = JobCard.Id LEFT OUTER JOIN Surveyor_JobCard_Mapping LEFT OUTER JOIN serveur ON Surveyor_JobCard_Mapping.SurveyorId = serveur.Id ON JobCard.Id = Surveyor_JobCard_Mapping.JobCardId LEFT OUTER JOIN Vehicle_Model RIGHT OUTER JOIN Vehicle ON Vehicle_Model.Id = Vehicle.Vehicle_Model_Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id ON JobCard.Vehicle_Id = Vehicle.Id LEFT OUTER JOIN Customer ON JobCard.Customer_Id = Customer.Id WHERE (JobCard.Type in (2,8,9)) And jobcard.WorkshopId=" + WorkshopId;
        if (!String.IsNullOrWhiteSpace(StartDate))
        {
            Str += " And jobcard.DOC>='" + StartDate + " 00:00:00' ";
        }
        if (!String.IsNullOrWhiteSpace(EndDate))
        {
            Str += " And jobcard.DOC<='" + EndDate + " 23:59:59'";
        }
        Str += " order by jobcard.DOC Desc ";
        DataTable dt = obj.GetDataTable(Str);
        return dt;
    }
    public DataTable JobcardListService(string StartDate, string EndDate, string WorkshopId = "1")
    {
        string Str = "SELECT (Select case when isnull(name,'')='Closed' then '<span class=''label label-success''>'+name+'</span>' when isnull(name,'')='Pending' then '<span class=''label label-danger''>'+name+'</span>' else '<span class=''label label-warning''>'+name+'</span>' end from JobStatus where id=JobCard.JobStatus_Id) as Status,(Select name from JobType where id=JobCard.Type) as type,isnull((Select  top 1 GstInvoiceNumber from Invoice where isnull(IsCancelled,0)=0 and JobCardId=JobCard.Id),'') as InvoiceNo,isnull(CustomerInvoiceAmount,0) as [Cust Est A] ,isnull(InsuranceInvoiceAmount,0) as [Ins Est A],isnull((select top 1 iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Spare_Mapping.IsDeleted,0)=0),0)+iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Service_Mapping.IsDeleted,0)=0),0) FROM [dbo].[Estimate] where JobCardId=JobCard.Id order by  id desc  ),0)+isnull((select top 1 isnull(JobCard_Insurance_Mapping.Salvage,0)+isnull(JobCard_Insurance_Mapping.VoluntaryExcess,0)+isnull(JobCard_Insurance_Mapping.CompusaryExcess,0) from JobCard_Insurance_Mapping where JobCardId=JobCard.Id),0) as [Est A],JobCard.Id, isnull(Customer.Name,'')+' ,'+isnull(Customer.Mobile,'') +' ,'+ isnull(Customer.Email,'') AS [Customer Name], isnull(Vehicle.Number,'')+' ,'+isnull(Vehicle_Brand.Name,'')+' ,'+isnull(Vehicle_Model.Name,'') AS Vehicle#,   CONVERT(varchar(17), JobCard.DOC , 113 )  AS [In Date], CONVERT(varchar(17), JobCard.GatePassGeneratedDate , 113 )  AS [Out Date], isnull(serveur.Name,'') +','+ isnull(serveur.Mobile,'')+','+isnull(serveur.Email,'') AS [Surveyor Detail],  isnull(InsuranceProvider.Name,'') AS [Insurance Provider] FROM JobCard_Insurance_Mapping LEFT OUTER JOIN InsuranceProvider ON JobCard_Insurance_Mapping.Insurance_Provider_Id = InsuranceProvider.Id RIGHT OUTER JOIN JobCard ON JobCard_Insurance_Mapping.Id = JobCard.Id LEFT OUTER JOIN Surveyor_JobCard_Mapping LEFT OUTER JOIN serveur ON Surveyor_JobCard_Mapping.SurveyorId = serveur.Id ON JobCard.Id = Surveyor_JobCard_Mapping.JobCardId LEFT OUTER JOIN Vehicle_Model RIGHT OUTER JOIN Vehicle ON Vehicle_Model.Id = Vehicle.Vehicle_Model_Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id ON JobCard.Vehicle_Id = Vehicle.Id LEFT OUTER JOIN Customer ON JobCard.Customer_Id = Customer.Id WHERE (JobCard.Type <> 2) And jobcard.WorkshopId=" + WorkshopId;
        if (!String.IsNullOrWhiteSpace(StartDate))
        {
            Str += " And jobcard.DOC>='" + StartDate + " 00:00:00' ";
        }
        if (!String.IsNullOrWhiteSpace(EndDate))
        {
            Str += " And jobcard.DOC<='" + EndDate + " 23:59:59'";
        }
        Str += " order by jobcard.DOC Desc ";
        DataTable dt = obj.GetDataTable(Str);
        return dt;
    }
    public DataTable JobcardListServiceRecommendation(string StartDate, string EndDate, string WorkshopId = "1")
    {
        string Str = "SELECT case when isnull(jobcard.recommended,0)=1 then '<input class=''grn-row'' type=''hidden'' id='''+Convert(varchar,jobcard.Id)+'-hdn'' />' else '' End as Recommended ,(Select case when isnull(name,'')='Closed' then '<span class=''label label-success''>'+name+'</span>' when isnull(name,'')='Pending' then '<span class=''label label-danger''>'+name+'</span>' else '<span class=''label label-warning''>'+name+'</span>' end from JobStatus where id=JobCard.JobStatus_Id) as Status,(Select name from JobType where id=JobCard.Type) as type,isnull((Select  Max(Id) from Invoice where JobCardId=JobCard.Id),0) as InvoiceNo,isnull(CustomerInvoiceAmount,0) as [Cust Est A] ,isnull(InsuranceInvoiceAmount,0) as [Ins Est A],isnull((select top 1 iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Spare_Mapping.IsDeleted,0)=0),0)+iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Service_Mapping.IsDeleted,0)=0),0) FROM [dbo].[Estimate] where JobCardId=JobCard.Id order by  id desc  ),0)+isnull((select top 1 isnull(JobCard_Insurance_Mapping.Salvage,0)+isnull(JobCard_Insurance_Mapping.VoluntaryExcess,0)+isnull(JobCard_Insurance_Mapping.CompusaryExcess,0) from JobCard_Insurance_Mapping where JobCardId=JobCard.Id),0) as [Est A],JobCard.Id, isnull(Customer.Name,'')+' ,'+isnull(Customer.Mobile,'') +' ,'+ isnull(Customer.Email,'') AS [Customer Name], isnull(Vehicle.Number,'')+' ,'+isnull(Vehicle_Brand.Name,'')+' ,'+isnull(Vehicle_Model.Name,'') AS Vehicle#,   CONVERT(varchar(17), JobCard.DOC , 113 )  AS [In Date], CONVERT(varchar(17), JobCard.GatePassGeneratedDate , 113 )  AS [Out Date], isnull(serveur.Name,'') +','+ isnull(serveur.Mobile,'')+','+isnull(serveur.Email,'') AS [Surveyor Detail],  isnull(InsuranceProvider.Name,'') AS [Insurance Provider] FROM JobCard_Insurance_Mapping LEFT OUTER JOIN InsuranceProvider ON JobCard_Insurance_Mapping.Insurance_Provider_Id = InsuranceProvider.Id RIGHT OUTER JOIN JobCard ON JobCard_Insurance_Mapping.Id = JobCard.Id LEFT OUTER JOIN Surveyor_JobCard_Mapping LEFT OUTER JOIN serveur ON Surveyor_JobCard_Mapping.SurveyorId = serveur.Id ON JobCard.Id = Surveyor_JobCard_Mapping.JobCardId LEFT OUTER JOIN Vehicle_Model RIGHT OUTER JOIN Vehicle ON Vehicle_Model.Id = Vehicle.Vehicle_Model_Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id ON JobCard.Vehicle_Id = Vehicle.Id LEFT OUTER JOIN Customer ON JobCard.Customer_Id = Customer.Id WHERE  jobcard.WorkshopId=" + WorkshopId;
        if (!String.IsNullOrWhiteSpace(StartDate))
        {
            Str += " And jobcard.DOC>='" + StartDate + " 00:00:00' ";
        }
        if (!String.IsNullOrWhiteSpace(EndDate))
        {
            Str += " And jobcard.DOC<='" + EndDate + " 23:59:59'";
        }
        Str += " order by jobcard.DOC Desc ";
        DataTable dt = obj.GetDataTable(Str);
        return dt;
    }
    public DataTable GSTFillingReport(string StartDate, string EndDate, string WorkshopId = "1", bool chkPaymentClose = false, bool chkPaymentReceived = false, bool chkNoPayment = false, bool InvoiceGenerated = true)
    {
        string strFilt = (InvoiceGenerated?" NOT ":String.Empty);
        string Str = "Select iif(Invoice.Type=2,convert(varchar,isnull((Select top 1 JobCard_Insurance_Mapping.ClaimNo from JobCard_Insurance_Mapping where JobCardId=Invoice.Jobcardid),0)),'') as ClaimNo,(Taxable18+Taxable28+SGST18+IGST18+CGST18+SGST28+IGST28+CGST28) as [gstTotal],iif(Invoice.Type=2,convert(varchar(Max),isnull((Select top 1 'web.motorz.co.in/Accounts/Liability/'+JobCard_Insurance_Mapping.ImageUrl from JobCard_Insurance_Mapping where JobCardId=Invoice.Jobcardid),0)),'') as LiabilityDoc,isnull((Select top 1 JobcardPayment.PaymentType from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Customer'),0) as [Payment Mode C],isnull((Select top 1 JobcardPayment.PaymentType from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Insurance'),0) [Payment Mode I],isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Customer'),0) as [Payment Received C],isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Insurance'),0) [Payment Received I],isnull((SELECT substring(isnull(Vehicle_Brand.Name,''),1,4) + ' ,' + isnull(Vehicle_Model.Name,'') FROM Vehicle LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id where Vehicle.id=(Select Vehicle_Id from jobcard where jobcard.id=jobcardid)),'') as Modal,iif(Invoice.Type=2,convert(varchar,isnull((Select top 1 Convert(varchar,JobCard_Insurance_Mapping.Insurance_Approved_Amount) from JobCard_Insurance_Mapping where JobCardId=Invoice.Jobcardid),0)),'') as Liability,(Select name from Users where Id=(Select JobCard.UserId from JobCard where JobCard.Id=Invoice.JobCardId)) as [Advisor],(Select name from Users where Id=Invoice.ApprovedBy) as [Approved By],(Select name from JobType where id=(Select JobCard.Type from jobcard where id=jobcardid)) as Jobcardtype,JobCardId,GstInvoiceNumber as [GSTinv],Convert(varchar(17),Invoice.DOC,113) as Inv_Date,isnull(iif(Type=1,isnull((SELECT Customer.Gst_No FROM JobCard INNER JOIN Customer ON JobCard.Customer_Id = Customer.Id WHERE (JobCard.Id = Invoice.JobCardId)),''),(select InsuranceProvider.GSTIN from InsuranceProvider where id=(select Vehicle_Insurance.Insurance_Provider_Id  from Vehicle_Insurance where Vehicle_Insurance.VehicleId=(Select Vehicle_Id from jobcard where id=jobcardid)) )),'') as GSTIN,iif(Type=1,isnull((Select name from customer where id=(Select customer_id from jobcard where jobcard.id=jobcardid)),''),(select name from InsuranceProvider where id=(select Vehicle_Insurance.Insurance_Provider_Id  from Vehicle_Insurance where Vehicle_Insurance.VehicleId=(Select Vehicle_Id from jobcard where id=jobcardid)) )) as Name,isnull((Select Number from Vehicle where id=(Select Vehicle_Id from jobcard where jobcard.id=jobcardid)),'') as Vehicle#,Taxable18,CGST18 as [CGST9],SGST18 as [SGST9],IGST18,Taxable28,CGST28 as [CGST14],CGST28 as [SGST14],IGST28, FinalTotal as [Final Amount],iif(Type=1,isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Customer'),0) ,isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Insurance'),0)) as [Payment Received],iif((Select isnull(JobCard.IsClosePaymentRequested,0) from JobCard where id=Invoice.Id)=1,'Y','N') as [Is Requested To Close Payment],iif(Type=1,'Customer','Insurance') as InvoiceType , GstInvoiceNumber as GSTinv  from Invoice where isnull(Invoice.Iscancelled,0)=0 and GstInvoiceNumber IS " + strFilt + " NULL and InvoiceNumber is not null AND WorkshopId= " + WorkshopId;
        if (!String.IsNullOrWhiteSpace(StartDate))
        {
            Str += " And DOC>='" + StartDate + " 00:00:00' ";
        }
        if (!String.IsNullOrWhiteSpace(EndDate))
        {
            Str += " And DOC<='" + EndDate + " 23:59:59'";
        }
        if (chkPaymentReceived)
        {
            Str += " And iif(Type=1,isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Customer'),0) ,isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Insurance'),0))>0 ";
        }
        if (chkNoPayment)
        {
            Str += " And iif(Type=1,isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Customer'),0) ,isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Insurance'),0))=0 ";
        }
        if (chkPaymentClose)
        {
            Str += " And iif((Select isnull(JobCard.IsClosePaymentRequested,0) from JobCard where id=Invoice.JobcardId)=1,'Y','N')='y' ";
        }
        Str += " order by DOC Desc ";
        DataTable dt = obj.GetDataTable(Str);
        return dt;
    }

    public DataTable GSTFillingReportSales(string StartDate, string EndDate, string WorkshopId = "1", bool chkPaymentClose = false, bool chkPaymentReceived = false, bool chkNoPayment = false, bool InvoiceGenerated = true)
    {
        string strFilt = (InvoiceGenerated ? " NOT " : String.Empty);
        string Str = "Select GSTinv as [Inv No.],JobCardId as [Job card ID],InvoiceType as [Invoice Type],Inv_Date as [Inv Date],isnull(GSTIN,'') as GSTIN,name as Name,[Vehicle#],[Modal],[Taxable18],[CGST9],[SGST9],[IGST18],[Taxable28],[CGST14],[SGST14],[IGST28],gstTotal as [Gst Total],TotalParts as [Total Parts Amount],[Total Labour] as [Total Labour Amount],[Final Amount],Liability,[Payment Received],[Payment Mode C],[Payment Received C],[Payment Mode I],[Payment Received I],[Jobcardtype] as [Jobcard type],ClaimNo as [Claim No],[LiabilityDoc] as [Liability Doc] from ( Select (Select SUM(isnull(TotalActualAmount,0)-isnull(TotalDiscount,0)) from Invoice_Spare_Mapping where InvoiceId=Invoice.Id) as [TotalParts],(Select SUM((isnull(Invoice_Service_Mapping.TotalActualAmount,0)-isnull(Invoice_Service_Mapping.TotalDiscount,0))+((isnull(Invoice_Service_Mapping.CGSTAmount*Quantity,0))*2)) from Invoice_Service_Mapping where InvoiceId=Invoice.id) as [Total Labour],(Taxable18+Taxable28+SGST18+IGST18+CGST18+SGST28+IGST28+CGST28) as [gstTotal],iif(Invoice.Type=2,convert(varchar,isnull((Select top 1 JobCard_Insurance_Mapping.ClaimNo from JobCard_Insurance_Mapping where JobCardId=Invoice.Jobcardid),0)),'') as ClaimNo,iif(Invoice.Type=2,convert(varchar(Max),isnull((Select top 1 'web.motorz.co.in/Accounts/Liability/'+JobCard_Insurance_Mapping.ImageUrl from JobCard_Insurance_Mapping where JobCardId=Invoice.Jobcardid),0)),'') as LiabilityDoc,isnull((Select top 1 JobcardPayment.PaymentType from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Customer'),0) as [Payment Mode C],isnull((Select top 1 JobcardPayment.PaymentType from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Insurance'),0) [Payment Mode I],isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Customer'),0) as [Payment Received C],isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Insurance'),0) [Payment Received I],isnull((SELECT substring(isnull(Vehicle_Brand.Name,''),1,4) + ' ,' + isnull(Vehicle_Model.Name,'') FROM Vehicle LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id where Vehicle.id=(Select Vehicle_Id from jobcard where jobcard.id=jobcardid)),'') as Modal,iif(Invoice.Type=2,convert(varchar,isnull((Select top 1 Convert(varchar,JobCard_Insurance_Mapping.Insurance_Approved_Amount) from JobCard_Insurance_Mapping where JobCardId=Invoice.Jobcardid),0)),'') as Liability,(Select name from Users where Id=(Select JobCard.UserId from JobCard where JobCard.Id=Invoice.JobCardId)) as [Advisor],(Select name from Users where Id=Invoice.ApprovedBy) as [Approved By],(Select name from JobType where id=(Select JobCard.Type from jobcard where id=jobcardid)) as Jobcardtype,JobCardId,GstInvoiceNumber as [GSTinv],Convert(varchar(17),Invoice.DOC,113) as Inv_Date,iif(Type=1,isnull((SELECT Customer.Gst_No FROM JobCard INNER JOIN Customer ON JobCard.Customer_Id = Customer.Id WHERE (JobCard.Id = Invoice.JobCardId)),''),(select InsuranceProvider.GSTIN from InsuranceProvider where id=(select Vehicle_Insurance.Insurance_Provider_Id  from Vehicle_Insurance where Vehicle_Insurance.VehicleId=(Select Vehicle_Id from jobcard where id=jobcardid)) )) as GSTIN,iif(Type=1,isnull((Select name from customer where id=(Select customer_id from jobcard where jobcard.id=jobcardid)),''),(select name from InsuranceProvider where id=(select Vehicle_Insurance.Insurance_Provider_Id  from Vehicle_Insurance where Vehicle_Insurance.VehicleId=(Select Vehicle_Id from jobcard where id=jobcardid)) )) as Name,isnull((Select Number from Vehicle where id=(Select Vehicle_Id from jobcard where jobcard.id=jobcardid)),'') as Vehicle#,Taxable18,CGST18 as [CGST9],SGST18 as [SGST9],IGST18,Taxable28,CGST28 as [CGST14],CGST28 as [SGST14],IGST28, FinalTotal as [Final Amount],iif(Type=1,isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Customer'),0) ,isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Insurance'),0)) as [Payment Received],iif((Select isnull(JobCard.IsClosePaymentRequested,0) from JobCard where id=Invoice.Id)=1,'Y','N') as [Is Requested To Close Payment],iif(Type=1,'Customer','Insurance') as InvoiceType  from Invoice where isnull(Invoice.Iscancelled,0)=0 and GstInvoiceNumber IS " + strFilt + " NULL and InvoiceNumber is not null AND WorkshopId= " + WorkshopId;
        if (!String.IsNullOrWhiteSpace(StartDate))
        {
            Str += " And DOC>='" + StartDate + " 00:00:00' ";
        }
        if (!String.IsNullOrWhiteSpace(EndDate))
        {
            Str += " And DOC<='" + EndDate + " 23:59:59'";
        }
        if (chkPaymentReceived)
        {
            Str += " And iif(Type=1,isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Customer'),0) ,isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Insurance'),0))>0 ";
        }
        if (chkNoPayment)
        {
            Str += " And iif(Type=1,isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Customer'),0) ,isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Insurance'),0))=0 ";
        }
        if (chkPaymentClose)
        {
            Str += " And iif((Select isnull(JobCard.IsClosePaymentRequested,0) from JobCard where id=Invoice.JobcardId)=1,'Y','N')='y' ";
        }
        Str += " ) as c order by c.Inv_Date";
        DataTable dt = obj.GetDataTable(Str);
        if (dt != null && dt.Rows.Count > 0)
        {
            DataRow dr1 = dt.NewRow();
            //[Payment Received C],[Payment Mode I],[Payment Received I]
            dr1[0] = "Total";
            dr1["Taxable18"] = dt.Compute("Sum([Taxable18])", string.Empty);
            dr1["CGST9"] = dt.Compute("Sum([CGST9])", string.Empty);
            dr1["SGST9"] = dt.Compute("Sum([SGST9])", string.Empty);
            dr1["IGST18"] = dt.Compute("Sum([IGST18])", string.Empty);
            dr1["Taxable28"] = dt.Compute("Sum([Taxable28])", string.Empty);
            dr1["CGST14"] = dt.Compute("Sum([CGST14])", string.Empty);
            dr1["SGST14"] = dt.Compute("Sum([SGST14])", string.Empty);
            dr1["IGST28"] = dt.Compute("Sum([IGST28])", string.Empty);
            dr1["Final Amount"] = dt.Compute("Sum([Final Amount])", string.Empty);
            dr1["Gst Total"] = dt.Compute("Sum([Gst Total])", string.Empty);
            dr1["Payment Received"] = dt.Compute("Sum([Payment Received])", string.Empty);
            dr1["Payment Received C"] = dt.Compute("Sum([Payment Received C])", string.Empty);
            dr1["Payment Received I"] = dt.Compute("Sum([Payment Received I])", string.Empty);
            dt.Rows.Add(dr1);
        }
        return dt;
    }

    public DataTable GSTFillingReportHSNParts(string StartDate, string EndDate)
    {
        string Str = "Select Hsn as HSN,Description, UQC,Convert(varchar,SUM(Qty)) as [Total Quantity],Convert(varchar,SUM(Total)) as [Total Value],Convert(varchar,SUM(Taxable)) as [Taxable Value],SUM(Igst) as [Integrated Tax Amount],Convert(varchar,SUM(Cgst)) as [Central Tax Amount],Convert(varchar,SUM(Sgst)) as [State/UT Tax Amount],'' as [Cess Amount] from (Select 'Motors Parts' as Description,'NOS-NUMBERS' as UQC,Hsn,SUM(Quantity) as Qty,SUM(Taxable) as Taxable,SUM(Cgst) as Cgst,SUM(Sgst) as Sgst,0 as Igst,SUM(Total) as Total from (Select (Select SUBSTRING(hsnNumber,1,2) from Spare where Id=Invoice_Spare_Mapping.SpareId) as Hsn,Invoice_Spare_Mapping.CGSTValue*2 as Rate,Quantity,(Invoice_Spare_Mapping.TotalActualAmount-((Invoice_Spare_Mapping.CGSTAmount*2)*Quantity)) as Taxable,(Invoice_Spare_Mapping.CGSTAmount*Quantity) as Cgst,(Invoice_Spare_Mapping.CGSTAmount*Quantity) as Sgst,Invoice_Spare_Mapping.TotalActualAmount as Total from invoice inner join Invoice_Spare_Mapping on Invoice_Spare_Mapping.InvoiceId=Invoice.Id where  invoice.DOC >='" + StartDate + " 00:00:00' and invoice.DOC <='" + EndDate + " 23:59:59' And isnull(IsCancelled,0)=0 And GstInvoiceNumber is not null and IGST18=0 and IGST28=0 ) as c where Total>0  group by Hsn union Select 'Motors Parts' as Description,'NOS-NUMBERS' as UQC,Hsn,SUM(Quantity) as Qty,SUM(Taxable) as Taxable,0 as Cgst,0 as Sgst,SUM(Sgst+Cgst) as Igst,SUM(Total) as Total from (Select (Select SUBSTRING(hsnNumber,1,2) from Spare where Id=Invoice_Spare_Mapping.SpareId) as Hsn,Invoice_Spare_Mapping.CGSTValue*2 as Rate,Quantity,(Invoice_Spare_Mapping.TotalActualAmount-((Invoice_Spare_Mapping.CGSTAmount*2)*Quantity)) as Taxable,(Invoice_Spare_Mapping.CGSTAmount*Quantity) as Cgst,(Invoice_Spare_Mapping.CGSTAmount*Quantity) as Sgst,Invoice_Spare_Mapping.TotalActualAmount as Total from invoice inner join Invoice_Spare_Mapping on Invoice_Spare_Mapping.InvoiceId=Invoice.Id where  invoice.DOC >='" + StartDate + " 00:00:00' and invoice.DOC <='" + EndDate + " 23:59:59' And isnull(IsCancelled,0)=0 And GstInvoiceNumber is not null and IGST18>0 and IGST28>0  ) as c where Total>0  group by Hsn union Select 'Labour' as Description,'OTH-OTHER' as UQC,Hsn,0 as Qty,SUM(Taxable) as Taxable,SUM(Cgst) as Cgst,SUM(Sgst) as Sgst,0 as Igst,SUM(Total) as Total from (Select '99' as Hsn,((Invoice_Service_Mapping.CGSTValue*2)) as Rate,Quantity,(Invoice_Service_Mapping.TotalActualAmount-Invoice_Service_Mapping.TotalDiscount) as Taxable,(Invoice_Service_Mapping.CGSTAmount*Quantity) as Cgst,(Invoice_Service_Mapping.CGSTAmount*Quantity) as Sgst,(Invoice_Service_Mapping.TotalActualAmount-Invoice_Service_Mapping.TotalDiscount)+((Invoice_Service_Mapping.CGSTAmount*Quantity)*2) as Total from invoice inner join Invoice_Service_Mapping on Invoice_Service_Mapping.InvoiceId=Invoice.Id where isnull(IsCancelled,0)=0 And GstInvoiceNumber is not null and invoice.DOC >='" + StartDate + " 00:00:00' and invoice.DOC <='" + EndDate + " 23:59:59' and IGST18=0 and IGST28=0 ) as c group by Hsn union Select 'Labour' as Description,'OTH-OTHER' as UQC,Hsn,0 as Qty,SUM(Taxable) as Taxable,0 as Cgst,0 as Sgst,SUM(Sgst+Cgst) as Igst,SUM(Total) as Total from (Select '99' as Hsn,((Invoice_Service_Mapping.CGSTValue*2)) as Rate,Quantity,(Invoice_Service_Mapping.TotalActualAmount-Invoice_Service_Mapping.TotalDiscount) as Taxable,(Invoice_Service_Mapping.CGSTAmount*Quantity) as Cgst,(Invoice_Service_Mapping.CGSTAmount*Quantity) as Sgst,(Invoice_Service_Mapping.TotalActualAmount-Invoice_Service_Mapping.TotalDiscount)+((Invoice_Service_Mapping.CGSTAmount*Quantity)*2) as Total from invoice inner join Invoice_Service_Mapping on Invoice_Service_Mapping.InvoiceId=Invoice.Id where isnull(IsCancelled,0)=0 And GstInvoiceNumber is not null and invoice.DOC >='" + StartDate + " 00:00:00' and invoice.DOC <='" + EndDate + " 23:59:59' and IGST18>0 and IGST28>0 ) as c group by Hsn ) as HsnData group by HsnData.Hsn,UQC,Description";
        DataTable dt = obj.GetDataTable(Str);
        DataTable dt1 = new DataTable();
        for (int i = 0; i < dt.Columns.Count; i++)
        {
            dt1.Columns.Add(dt.Columns[i].ColumnName);
        }
        DataRow dr1 = dt1.NewRow();
        DataRow dr2 = dt1.NewRow();
        DataRow drHeader = dt1.NewRow();
        //HSN	Description	UQC	Total Quantity	Total Value	Taxable Value	Integrated Tax Amount	Central Tax Amount	State/UT Tax Amount	Cess Amount

        drHeader["HSN"] = "HSN";
        drHeader["Description"] = "Description";
        drHeader["UQC"] = "UQC";
        drHeader["Total Quantity"] = "Total Quantity";
        drHeader["Total Value"] = "Total Value";
        drHeader["Taxable Value"] = "Taxable Value";
        drHeader["Integrated Tax Amount"] = "Integrated Tax Amount";
        drHeader["Central Tax Amount"] = "Central Tax Amount";
        drHeader["State/UT Tax Amount"] = "State/UT Tax Amount";
        drHeader["Cess Amount"] = "Cess Amount";
       
        dt1.Rows.Add(dr1);
        dt1.Rows.Add(dr2);
        dt1.Rows.Add(drHeader);
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            DataRow dr3 = dt1.NewRow();
            dr3[0] = dt.Rows[i][0].ToString();
            dr3[1] = dt.Rows[i][1].ToString();
            dr3[2] = dt.Rows[i][2].ToString();
            dr3[3] = dt.Rows[i][3].ToString();
            dr3[4] = dt.Rows[i][4].ToString();
            dr3[5] = dt.Rows[i][5].ToString();
            dr3[6] = dt.Rows[i][6].ToString();
            dr3[7] = dt.Rows[i][7].ToString();
            dr3[8] = dt.Rows[i][8].ToString();
            dr3[9] = dt.Rows[i][9].ToString();
            dt1.Rows.Add(dr3);
        }
            return dt1;
    }

    //public DataTable GSTFillingReportExportForJson(string StartDate, string EndDate, string WorkshopId = "1")
    //{
    //    //GSTIN/UIN of Recipient	Invoice Number	Invoice date	Invoice Value	Place Of Supply	Reverse Charge	Applicable % of Tax Rate	Invoice Type	E-Commerce GSTIN	Rate	Taxable Value	Cess Amount


    //    string Str = "Select GSTinv as [Inv No.],JobCardId as [Job card ID],InvoiceType as [Invoice Type],Inv_Date as [Inv Date],isnull(GSTIN,'') as GSTIN,name as Name,[Vehicle#],[Modal],[Taxable18],[CGST9],[SGST9],[IGST18],[Taxable28],[CGST14],[SGST14],[IGST28],gstTotal as [Gst Total],TotalParts as [Total Parts Amount],[Total Labour] as [Total Labour Amount],[Final Amount],Liability,[Payment Received],[Payment Mode C],[Payment Received C],[Payment Mode I],[Payment Received I],[Jobcardtype] as [Jobcard type],ClaimNo as [Claim No],[LiabilityDoc] as [Liability Doc] from ( Select (Select SUM(isnull(TotalActualAmount,0)-isnull(TotalDiscount,0)) from Invoice_Spare_Mapping where InvoiceId=Invoice.Id) as [TotalParts],(Select SUM((isnull(Invoice_Service_Mapping.TotalActualAmount,0)-isnull(Invoice_Service_Mapping.TotalDiscount,0))+((isnull(Invoice_Service_Mapping.CGSTAmount*Quantity,0))*2)) from Invoice_Service_Mapping where InvoiceId=Invoice.id) as [Total Labour],(Taxable18+Taxable28+SGST18+IGST18+CGST18+SGST28+IGST28+CGST28) as [gstTotal],iif(Invoice.Type=2,convert(varchar,isnull((Select top 1 JobCard_Insurance_Mapping.ClaimNo from JobCard_Insurance_Mapping where JobCardId=Invoice.Jobcardid),0)),'') as ClaimNo,iif(Invoice.Type=2,convert(varchar(Max),isnull((Select top 1 'web.motorz.co.in/Accounts/Liability/'+JobCard_Insurance_Mapping.ImageUrl from JobCard_Insurance_Mapping where JobCardId=Invoice.Jobcardid),0)),'') as LiabilityDoc,isnull((Select top 1 JobcardPayment.PaymentType from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Customer'),0) as [Payment Mode C],isnull((Select top 1 JobcardPayment.PaymentType from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Insurance'),0) [Payment Mode I],isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Customer'),0) as [Payment Received C],isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Insurance'),0) [Payment Received I],isnull((SELECT substring(isnull(Vehicle_Brand.Name,''),1,4) + ' ,' + isnull(Vehicle_Model.Name,'') FROM Vehicle LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id where Vehicle.id=(Select Vehicle_Id from jobcard where jobcard.id=jobcardid)),'') as Modal,iif(Invoice.Type=2,convert(varchar,isnull((Select top 1 Convert(varchar,JobCard_Insurance_Mapping.Insurance_Approved_Amount) from JobCard_Insurance_Mapping where JobCardId=Invoice.Jobcardid),0)),'') as Liability,(Select name from Users where Id=(Select JobCard.UserId from JobCard where JobCard.Id=Invoice.JobCardId)) as [Advisor],(Select name from Users where Id=Invoice.ApprovedBy) as [Approved By],(Select name from JobType where id=(Select JobCard.Type from jobcard where id=jobcardid)) as Jobcardtype,JobCardId,GstInvoiceNumber as [GSTinv],Convert(varchar(17),Invoice.DOC,113) as Inv_Date,iif(Type=1,isnull((SELECT Customer.Gst_No FROM JobCard INNER JOIN Customer ON JobCard.Customer_Id = Customer.Id WHERE (JobCard.Id = Invoice.JobCardId)),''),(select InsuranceProvider.GSTIN from InsuranceProvider where id=(select Vehicle_Insurance.Insurance_Provider_Id  from Vehicle_Insurance where Vehicle_Insurance.VehicleId=(Select Vehicle_Id from jobcard where id=jobcardid)) )) as GSTIN,iif(Type=1,isnull((Select name from customer where id=(Select customer_id from jobcard where jobcard.id=jobcardid)),''),(select name from InsuranceProvider where id=(select Vehicle_Insurance.Insurance_Provider_Id  from Vehicle_Insurance where Vehicle_Insurance.VehicleId=(Select Vehicle_Id from jobcard where id=jobcardid)) )) as Name,isnull((Select Number from Vehicle where id=(Select Vehicle_Id from jobcard where jobcard.id=jobcardid)),'') as Vehicle#,Taxable18,CGST18 as [CGST9],SGST18 as [SGST9],IGST18,Taxable28,CGST28 as [CGST14],CGST28 as [SGST14],IGST28, FinalTotal as [Final Amount],iif(Type=1,isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Customer'),0) ,isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Insurance'),0)) as [Payment Received],iif((Select isnull(JobCard.IsClosePaymentRequested,0) from JobCard where id=Invoice.Id)=1,'Y','N') as [Is Requested To Close Payment],iif(Type=1,'Customer','Insurance') as InvoiceType  from Invoice where isnull(Invoice.Iscancelled,0)=0 and GstInvoiceNumber IS Not NULL and InvoiceNumber is not null AND WorkshopId= " + WorkshopId;
    //    if (!String.IsNullOrWhiteSpace(StartDate))
    //    {
    //        Str += " And DOC>='" + StartDate + " 00:00:00' ";
    //    }
    //    if (!String.IsNullOrWhiteSpace(EndDate))
    //    {
    //        Str += " And DOC<='" + EndDate + " 23:59:59'";
    //    }
    //    Str += " ) as c order by c.Inv_Date Desc ";
    //    DataTable dt = obj.GetDataTable(Str);
    //    return dt;
    //}

    public DataTable InvoiceVsPaymentReport(string compareType,string compareAmount,string StartDate, string EndDate, string WorkshopId = "1", bool chkPaymentClose = false, bool chkPaymentReceived = false, bool chkNoPayment = false)
    {
        string sym=">=";
        if (compareType == "1")
            sym = "=";
        else if (compareType == "2")
            sym = ">=";
        else if (compareType == "3")
            sym = "<=";
        else if (compareType == "4")
            sym = "<>";
        string strcompareAmount = "";

        if (!String.IsNullOrWhiteSpace(compareAmount))
        {
            strcompareAmount = " And ([Final Amount]-([Payment Received1]+[Payment Received]))" + sym + compareAmount;
        }
        string Str = "Select *,[Final Amount]-([Payment Received1]+[Payment Received]) as Diff from (Select Invoice.DOC,(Select name from Users where Id=(Select JobCard.UserId from JobCard where JobCard.Id=Invoice.JobCardId)) as [Advisor],(Select name from Users where Id=Invoice.ApprovedBy) as [Approved By],(Select name from JobType where id=(Select JobCard.Type from jobcard where id=jobcardid)) as Jobcardtype,JobCardId,GstInvoiceNumber as [GSTinv],Convert(varchar(17),Invoice.DOC,113) as Inv_Date,iif(Type=1,isnull((SELECT Customer.Gst_No FROM JobCard INNER JOIN Customer ON JobCard.Customer_Id = Customer.Id WHERE (JobCard.Id = Invoice.JobCardId)),''),(select InsuranceProvider.GSTIN from InsuranceProvider where id=(select Vehicle_Insurance.Insurance_Provider_Id  from Vehicle_Insurance where Vehicle_Insurance.VehicleId=(Select Vehicle_Id from jobcard where id=jobcardid)) )) as GSTIN,iif(Type=1,isnull((Select name from customer where id=(Select customer_id from jobcard where jobcard.id=jobcardid)),''),(select name from InsuranceProvider where id=(select Vehicle_Insurance.Insurance_Provider_Id  from Vehicle_Insurance where Vehicle_Insurance.VehicleId=(Select Vehicle_Id from jobcard where id=jobcardid)) )) as Name,isnull((Select Number from Vehicle where id=(Select Vehicle_Id from jobcard where jobcard.id=jobcardid)),'') as Vehicle#,Taxable18,CGST18 as [CGST9],SGST18 as [SGST9],IGST18,Taxable28,CGST28 as [CGST14],CGST28 as [SGST14],IGST28, FinalTotal as [Final Amount],isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Customer'),0) as [Payment Received],isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Insurance'),0) [Payment Received1],iif((Select isnull(JobCard.IsClosePaymentRequested,0) from JobCard where id=Invoice.Id)=1,'Y','N') as [Is Requested To Close Payment],iif(Type=1,'C','I') as InvoiceType  from Invoice where isnull(Invoice.Iscancelled,0)=0 and GstInvoiceNumber IS NOT NULL AND WorkshopId= " + WorkshopId;
        if (!String.IsNullOrWhiteSpace(StartDate))
        {
            Str += " And DOC>='" + StartDate + " 00:00:00' ";
        }
        if (!String.IsNullOrWhiteSpace(EndDate))
        {
            Str += " And DOC<='" + EndDate + " 23:59:59'";
        }
        if (chkPaymentReceived)
        {
            Str += " And iif(Type=1,isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Customer'),0) ,isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Insurance'),0))>0 ";
        }
        if (chkNoPayment)
        {
            Str += " And iif(Type=1,isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Customer'),0) ,isnull((Select Sum(Amount) from JobcardPayment where JobcardPayment.Jobcardid=Invoice.jobcardid and paidby='Insurance'),0))=0 ";
        }
        if (chkPaymentClose)
        {
            Str += " And iif((Select isnull(JobCard.IsClosePaymentRequested,0) from JobCard where id=Invoice.JobcardId)=1,'Y','N')='y' ";
        }
        Str += ") as c where [Final Amount]<>([Payment Received1]+[Payment Received]) " + strcompareAmount + " order by DOC Desc ";
        DataTable dt = obj.GetDataTable(Str);
        return dt;
    }

    public DataTable ProfitReport(string StartDate, string EndDate, string WorkshopId = "1")
    {
        DataTable dt = new DataTable();

        string Str = " SELECT 0 as [Profit On Labours],0 as [Profit On Parts],0 as Profit,isnull((Select sum(Amount) from JobCard_Expenses where JobcardId=JobCard.id),0) as [Other Expense],Colour,(Select SUM(Amount) from JobCardPayment where JobCardPayment.JobCardId=JobCard.Id and JobCardPayment.PaidBy='customer') as [cust Amt Rec],(Select SUM(Amount) from JobCardPayment where JobCardPayment.JobCardId=JobCard.Id and JobCardPayment.PaidBy='insurance') as [Ins Amt Rec],JobCard.Id [Job Card Id],isnull((Select  top 1 GstInvoiceNumber from Invoice where isnull(IsCancelled,0)=0 and JobCardId=JobCard.Id and Invoice.Type=1),'') as [Inv No Cust],isnull((Select  top 1 GstInvoiceNumber from Invoice where isnull(IsCancelled,0)=0 and JobCardId=JobCard.Id  and Invoice.Type=2),'') as [Inv No Ins],isnull((Select  top 1 Convert(varchar,FinalTotal) from Invoice where isnull(IsCancelled,0)=0 and JobCardId=JobCard.Id and Invoice.Type=1),Convert(varchar,jobcard.CustomerInvoiceAmount)) as [Inv A Cust],isnull((Select  top 1 Convert(varchar,FinalTotal) from Invoice where isnull(IsCancelled,0)=0 and JobCardId=JobCard.Id  and Invoice.Type=2),Convert(varchar,jobcard.InsuranceInvoiceAmount)) as [Inv A Ins],isnull((Select  top 1 iif(GstInvoiceNumber is null,'P','I') from Invoice where isnull(IsCancelled,0)=0 and JobCardId=JobCard.Id and Invoice.Type=1),iif(JobCard.DOC<'01-May-2019 00:00:00',iif(isnull(jobcard.CustomerInvoiceAmount,0)>0,'P',''),'E')) as [Inv A Cust F],isnull((Select  top 1 iif(GstInvoiceNumber is null,'P','I') from Invoice where isnull(IsCancelled,0)=0 and JobCardId=JobCard.Id  and Invoice.Type=2),iif(JobCard.DOC<'01-May-2019 00:00:00',iif(isnull(jobcard.InsuranceInvoiceAmount,0)>0,'P',''),'E')) as [Inv A Ins F],CONVERT(varchar(17),JobCard.DOC,113) as [In Date]  , CONVERT(varchar(17),GatePassGeneratedDate,113) as [Out Date] , Vehicle_Brand.Name+','+isnull((select name from Vehicle_Model where id=(select Vehicle_Model_Id from Vehicle where id=JobCard.Vehicle_Id)),'') as [Brand And Model],Vehicle.Number,(select name + ' ' +customer.Mobile from customer where id=Customer_Id) as [Cust Name],(Select name from JobType where id=Type) as Type ,case when JobCard.Type=2 then (select name from InsuranceProvider where id=(select Vehicle_Insurance.Insurance_Provider_Id  from Vehicle_Insurance where Vehicle_Insurance.VehicleId=Vehicle_Id) ) else '' end as [ins] ,isnull(CustomerInvoiceAmount,0) as [Cust Est A] ,isnull(InsuranceInvoiceAmount,0) as [Ins Est A] ,isnull((select top 1 iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Spare_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Spare_Mapping.IsDeleted,0)=0),0)+iSNULL((Select Sum(isnull(TotalActualAmount,0)) from Estimate_Service_Mapping where EstimateId=Estimate.Id and isnull(Estimate_Service_Mapping.IsDeleted,0)=0),0) FROM [dbo].[Estimate] where JobCardId=JobCard.Id order by  id desc  ),0)+isnull((select top 1 isnull(JobCard_Insurance_Mapping.Salvage,0)+isnull(JobCard_Insurance_Mapping.VoluntaryExcess,0)+isnull(JobCard_Insurance_Mapping.CompusaryExcess,0) from JobCard_Insurance_Mapping where JobCardId=JobCard.Id),0) as [Est A], Isnull((SELECT Sum(isnull([Amount],0)) FROM [JobCardPayment]  where jobcardid=JobCard.Id and [JobCardPayment].PaymentType in ('Cash','dispute')) ,0) as [Cash] , Isnull((SELECT Sum(isnull([Amount],0)) FROM [JobCardPayment]  where  jobcardid=JobCard.Id and [JobCardPayment].PaymentType='Cheque') ,0) as [Cheque] , Isnull((SELECT Sum(isnull([Amount],0)) FROM [JobCardPayment]  where  jobcardid=JobCard.Id and [JobCardPayment].PaymentType in ('CCard','DCard')) ,0) as [Card], Isnull((SELECT Sum(isnull([Amount],0)) FROM [JobCardPayment]  where    jobcardid=JobCard.Id and [JobCardPayment].PaymentType in ('NEFT')) ,0) as [NEFT],isnull((Select top 1 comment from JobcardwithoutPayment where jobcardId=JobCard.Id and IsDelete=0 order by id desc),'') as Comment,isnull((STUFF((SELECT ', ' + CAST((Select name from [Users] where id=[JobCardPayment].CollcetedBy) AS VARCHAR(10))+'('+CAST([Amount] AS VARCHAR(10))+')' [text()] FROM [JobCardPayment] where jobcardid=JobCard.Id FOR XML PATH(''), TYPE) .value('.','NVARCHAR(MAX)'),1,2,' ')),'') as CollctedBy FROM Customer RIGHT OUTER JOIN JobCard ON Customer.Id = JobCard.Customer_Id LEFT OUTER JOIN Vehicle LEFT OUTER JOIN Vehicle_Variant ON Vehicle.Vehicle_Variant_Id = Vehicle_Variant.Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id ON JobCard.Vehicle_Id = Vehicle.Id where JobCard.id>0 and JobCard.doc>='" + StartDate + " 00:00:00' and JobCard.doc<='" + EndDate + " 23:59:59' AND JobCard.WorkshopId=" + WorkshopId;
        dt = obj.GetDataTable(Str);

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            try
            {
                decimal Profit = 0;
                DataTable dtPartsProfit = obj.GetDataTable("Select Convert(Decimal(18,2),isnull(((Invoice_Spare_Mapping.ActualAmountPerUnit-Invoice_Spare_Mapping.DiscountPerUnit) -(Invoice_Spare_Mapping.CGSTAmount+Invoice_Spare_Mapping.SGSTAmount))-Convert(decimal(18,2),(isnull((Select GRNDetail.VendorInvoiceRate from GRNDetail where id=iif(Invoice.Type=1,GrnDetailId,(Select ISM.GrnDetailId from Invoice_Spare_Mapping as ISM where id=Invoice_Spare_Mapping.refid ))),isnull(Invoice_Spare_Mapping.PurchaseAmount,((Invoice_Spare_Mapping.ActualAmountPerUnit-Invoice_Spare_Mapping.DiscountPerUnit) -(Invoice_Spare_Mapping.CGSTAmount+Invoice_Spare_Mapping.SGSTAmount))))*(iif(Invoice.Type=1,Invoice_Spare_Mapping.Depreciation,100-(Select ISM.Depreciation from Invoice_Spare_Mapping as ISM where id=Invoice_Spare_Mapping.refid ))))/100),0)) as Profit from Invoice_Spare_Mapping inner join Invoice on Invoice.id=Invoice_Spare_Mapping.InvoiceId where Invoice.JobCardId=" + dt.Rows[i]["Job Card Id"].ToString() + " and isnull(Invoice.IsCancelled,0)=0");
                if (dtPartsProfit != null && dtPartsProfit.Rows.Count > 0)
                {
                    Profit = (decimal)dtPartsProfit.Compute("Sum(Profit)", string.Empty);
                    dt.Rows[i]["Profit On Parts"] = (decimal)dtPartsProfit.Compute("Sum(Profit)", string.Empty);
                }
                DataTable dtService = obj.GetDataTable("Select Convert(Decimal(18,2),isnull(((Estimate_Service_Mapping.ActualAmountPerUnit-isnull(Estimate_Service_Mapping.DiscountPerUnit,0))*Estimate_Service_Mapping.Quantity),0)) as Profit from Invoice_Service_Mapping as Estimate_Service_Mapping inner join Invoice on Invoice.id=Estimate_Service_Mapping.InvoiceId where Invoice.JobCardId=" + dt.Rows[i]["Job Card Id"].ToString() + " and isnull(Invoice.IsCancelled,0)=0");
                if (dtService != null && dtService.Rows.Count > 0)
                {
                    Profit += (decimal)dtService.Compute("Sum(Profit)", string.Empty);
                    dt.Rows[i]["Profit On Labours"] = (decimal)dtService.Compute("Sum(Profit)", string.Empty);
                }
                Profit -= Convert.ToDecimal(dt.Rows[i]["Other Expense"].ToString());
                dt.Rows[i]["Profit"] = Profit;
            }
            catch (Exception E) { 

            }
        }
        return dt;
    }

    public DataTable UserActivityReport(string StartDate, string EndDate, string WorkshopId = "1")
    {
        string Str = "Select CONVERT(varchar(20), [DOC] , 113 ) as Date,(Select Users.UserName from Users where id=userid) as UserName,Activity,Action,ActivityType from [UserActivity] where WorkshopId= " + WorkshopId;
        if (!String.IsNullOrWhiteSpace(StartDate))
        {
            Str += " And DOC>='" + StartDate + " 00:00:00' ";
        }
        if (!String.IsNullOrWhiteSpace(EndDate))
        {
            Str += " And DOC<='" + EndDate + " 23:59:59'";
        }
        Str += " order by DOC Desc ";
        DataTable dt = obj.GetDataTable(Str);
        return dt;
    }
    public DataTable InventoryBug(string WorkshopId = "1")
    {
        string Str = "Select (select name from spare where id=SpareId) as Part,(select name from Spare_Brand where id=BrandID) as Brand,Qty as [Avl Qty],isnull((Select sum(cr)-sum(dr) from SpareInventaryHistory where SpareSpareBrandMappingId=SpareSpareBrandMapping.Id),0) as [TCr-TDr]  from SpareSpareBrandMapping  where Convert(decimal(10,2),qty)<>Convert(decimal(10,2),isnull((Select sum(cr)-sum(dr) from SpareInventaryHistory where SpareSpareBrandMappingId=SpareSpareBrandMapping.Id),0))";
        DataTable dt = obj.GetDataTable(Str);
        return dt;
    }
    public workshopDetail GetWorkShopDetail(int WorkshopId)
    {
        var objWorkshop = new workshopDetail();
        try
        {
            string query = "SELECT [Id],[Name],[Address],[Email],[Phone_no],[GSTIN_No] FROM [dbo].[Workshop] where Id=" + WorkshopId;
            DataTable dt = obj.GetDataTable(query);
            if (dt != null && dt.Rows.Count > 0)
            {
                objWorkshop.Id = dt.Rows[0]["Id"].ToString();
                objWorkshop.Name = dt.Rows[0]["Name"].ToString();
                objWorkshop.Address = dt.Rows[0]["Address"].ToString();
                objWorkshop.Email = dt.Rows[0]["Email"].ToString();
                objWorkshop.Phone_no = dt.Rows[0]["Phone_no"].ToString();
                objWorkshop.GSTIN_No = dt.Rows[0]["GSTIN_No"].ToString();
            }
        }
        catch (Exception E) { }
        return objWorkshop;
    }
    public DataTable RequisitionListByJobcardId(string JobcardId)
    {
        DataTable dt = obj.GetDataTable("SELECT JobCard.Id, Spare.Name, Convert(varchar,JobCard_Spare_Mapping.Quantity) as Quantity, JobCard_Spare_Mapping.Id AS MappingId,iif(Requisition_Spare.Id is null,0,1) as IsReqSent,isnull(Convert(varchar(17),Requisition_Spare.DOC,113),'') as ReqSentOn, iif(isnull(Requisition_Spare.IsAllocate,0)=0,0,1) as IsAllocated, isnull((Select name from Users where id= Requisition_Spare.AllocateBy),'') as AllocatedBy , isnull(Convert(varchar(17),Requisition_Spare.AllocateTime,113),'') as AllocatedOn,isnull(Convert(varchar,Requisition_Spare.RequisitionId),'') as ReqId,isnull(convert(varchar,Requisition_Spare.trnNo),'') as trnNo FROM  JobCard INNER JOIN JobCard_Spare_Mapping ON JobCard.Id = JobCard_Spare_Mapping.JobCardId INNER JOIN Spare ON JobCard_Spare_Mapping.SpareId = Spare.Id INNER JOIN JobCard_Details ON JobCard_Spare_Mapping.JobCardDetailId = JobCard_Details.Id LEFT OUTER JOIN Requisition_Spare ON JobCard_Spare_Mapping.Id = Requisition_Spare.SpareMappingId WHERE (JobCard.Id = " + JobcardId + ") AND (JobCard_Spare_Mapping.IsDeleted = 0) AND (JobCard.WorkshopId = 1) AND (JobCard_Details.IsDeleted = 0)");
        return dt;
    }

    public ClsInvoiceApproval GetInvoiceApprovalDetailByJobcardId(string JobcardId)
    {
        ClsInvoiceApproval ClsInvoiceApproval
            = new ClsInvoiceApproval();
        try
        {
            //Customer Invoice Total 
            DataTable dt = obj.GetDataTable("Select isnull((Select SUM((isnull(Invoice_Spare_Mapping.ActualAmountPerUnit,0)-isnull(Invoice_Spare_Mapping.DiscountPerUnit,0))*Invoice_Spare_Mapping.Quantity) from Invoice_Spare_Mapping where Invoice_Spare_Mapping.InvoiceId=Invoice.Id),0)+isnull((Select SUM(((isnull(Invoice_Service_Mapping.ActualAmountPerUnit,0)-isnull(Invoice_Service_Mapping.DiscountPerUnit,0))*Invoice_Service_Mapping.Quantity)+((Invoice_Service_Mapping.CGSTAmount*2)*Invoice_Service_Mapping.Quantity)) from Invoice_Service_Mapping where Invoice_Service_Mapping.InvoiceId=Invoice.Id),0) from invoice where jobcardid=" + JobcardId + " and ISNULL(IsCancelled,0)=0 and Type=1");
            if (dt != null && dt.Rows.Count > 0)
                ClsInvoiceApproval.lblCustBilledAmount = (dt.Rows[0][0] != null ? dt.Rows[0][0].ToString() : "NAN");
            //Insurance Invoice Total if Any
            dt = obj.GetDataTable("Select isnull((Select SUM((isnull(Invoice_Spare_Mapping.ActualAmountPerUnit,0)-isnull(Invoice_Spare_Mapping.DiscountPerUnit,0))*Invoice_Spare_Mapping.Quantity) from Invoice_Spare_Mapping where Invoice_Spare_Mapping.InvoiceId=Invoice.Id),0)+isnull((Select SUM(((isnull(Invoice_Service_Mapping.ActualAmountPerUnit,0)-isnull(Invoice_Service_Mapping.DiscountPerUnit,0))*Invoice_Service_Mapping.Quantity)+((Invoice_Service_Mapping.CGSTAmount*2))) from Invoice_Service_Mapping where Invoice_Service_Mapping.InvoiceId=Invoice.Id),0) from invoice where jobcardid=" + JobcardId + " and ISNULL(IsCancelled,0)=0 and Type=2");
            if (dt != null && dt.Rows.Count > 0)
            {
                ClsInvoiceApproval.txtClaimedAmount = (dt.Rows[0][0] != null ? dt.Rows[0][0].ToString() : "NAN");
                //Set Claim Amount If Skipped From Any where
                obj.ExecuteQuery("update JobCard_Insurance_Mapping set ClaimAmount=" + ClsInvoiceApproval.txtClaimedAmount + " where JobCardid=" + JobcardId);
            }

            //Select isnull(CompusaryExcess,0) from JobCard_Insurance_Mapping where JobCardid=5449
            dt = obj.GetDataTable("Select CompusaryExcess,VoluntaryExcess,Salvage,Insurance_Approved_Amount,ImageUrl from JobCard_Insurance_Mapping where JobCardid=" + JobcardId);
            if (dt != null && dt.Rows.Count > 0)
            {
                ClsInvoiceApproval.CompusaryExcess = (dt.Rows[0]["CompusaryExcess"] != null ? dt.Rows[0]["CompusaryExcess"].ToString() : "NAN");
                ClsInvoiceApproval.VoluntaryExcess = (dt.Rows[0]["VoluntaryExcess"] != null ? dt.Rows[0]["VoluntaryExcess"].ToString() : "NAN");
                ClsInvoiceApproval.Salvage = (dt.Rows[0]["Salvage"] != null ? dt.Rows[0]["Salvage"].ToString() : "NAN");
                ClsInvoiceApproval.txtLiability = (dt.Rows[0]["Insurance_Approved_Amount"] != null ? dt.Rows[0]["Insurance_Approved_Amount"].ToString() : "NAN");
                ClsInvoiceApproval.Url = (dt.Rows[0]["ImageUrl"] != null ? dt.Rows[0]["ImageUrl"].ToString() : "NAN");
                decimal dClaimedAmount = 0; decimal dApprovedAmount = 0; decimal dDiffAmount = 0; decimal dCompusaryExcess = 0; decimal dVoluntaryExcess = 0; decimal dSalvage = 0; decimal dlblCustBilledAmount = 0;
                decimal.TryParse(ClsInvoiceApproval.txtClaimedAmount, out dClaimedAmount);
                decimal.TryParse(ClsInvoiceApproval.txtLiability, out dApprovedAmount);
                //Try to get other Payable Amount to customer
                decimal.TryParse(ClsInvoiceApproval.CompusaryExcess, out dCompusaryExcess);
                decimal.TryParse(ClsInvoiceApproval.VoluntaryExcess, out dVoluntaryExcess);
                decimal.TryParse(ClsInvoiceApproval.Salvage, out dSalvage);
                decimal.TryParse(ClsInvoiceApproval.lblCustBilledAmount, out dlblCustBilledAmount);
                dDiffAmount = dClaimedAmount - (dApprovedAmount + dCompusaryExcess + dVoluntaryExcess + dSalvage);
                ClsInvoiceApproval.txtDiff = dDiffAmount.ToString();
                //Set Customer Payable Amount In Case of Diff In Insurance Payment & Other Excesses
                ClsInvoiceApproval.txtCustPayable = (dlblCustBilledAmount + dDiffAmount + dCompusaryExcess + dVoluntaryExcess + dSalvage).ToString();
            }
            else
            {
                ClsInvoiceApproval.txtCustPayable = ClsInvoiceApproval.lblCustBilledAmount;
            }
            dt = obj.GetDataTable("Select CustomerPayableAmount from Jobcard where isnull(PriGatepassProcessDone,0)=1 and id=" + JobcardId);
            if (dt != null && dt.Rows.Count > 0)
            {
                ClsInvoiceApproval.txtCustPayable = dt.Rows[0][0].ToString();
            }
        }
        catch (Exception E)
        {
            if (E != null)
                obj.InsertLogs(E.Message, (E.InnerException != null ? E.InnerException.ToString() : ""), E.StackTrace);
        }
        return ClsInvoiceApproval;
    }

    #region Classes


    //txtClaimedAmount
    //txtLiability
    //txtDiff
    //lblCustBilledAmount
    //txtCustPayable
    public class ClsInvoiceApproval
    {
        public string txtClaimedAmount { get; set; }
        public string txtLiability { get; set; }
        public string txtDiff { get; set; }
        public string lblCustBilledAmount { get; set; }
        public string txtCustPayable { get; set; }
        public string CompusaryExcess { get; set; }
        public string VoluntaryExcess { get; set; }
        public string Salvage { get; set; }
        public string Url { get; set; }

    }

    public class workshopDetail
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Phone_no { get; set; }
        public string GSTIN_No { get; set; }
    }
    #endregion
}