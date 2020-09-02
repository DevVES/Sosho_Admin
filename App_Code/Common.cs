using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Common
/// </summary>
public class Common : Icommon
{
    WebApplication1.dbConnection dbConnection = new WebApplication1.dbConnection();
    String strAccidentJobcardType = "2,8,9";
    public Common()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public bool IsAccidentJobcard(string Jobcardid)
    {
        System.Data.DataTable Dt = dbConnection.GetDataTable("Select id from JobCard where JobCard.Type in (" + strAccidentJobcardType + ") and Id=" + Jobcardid);
        if (Dt != null && Dt.Rows.Count > 0)
            return true;
        //Reach to this point suggest false
        return false;
    }
    public bool IsCashless(string Jobcardid)
    {
        System.Data.DataTable Dt = dbConnection.GetDataTable("Select JobCard_Insurance_Mapping.Insurance_Type from JobCard_Insurance_Mapping where Insurance_Type=1 and JobCardId=" + Jobcardid);
        if (Dt != null && Dt.Rows.Count > 0)
        {
            return true;
        }
        return false;
    }
    public bool IsGatepassGenerated(string Jobcardid)
    {
        System.Data.DataTable Dt = dbConnection.GetDataTable("Select id from JobCard where isnull(IsGatePassGenerated,0)=1 and Id=" + Jobcardid);
        if (Dt != null && Dt.Rows.Count > 0)
            return true;
        //Reach to this point suggest false
        return false;
    }
    public bool IsGatepassDocumentUploadedInSystem(string Jobcardid)
    {
        System.Data.DataTable Dt = dbConnection.GetDataTable("Select id from JobCard where isnull(IsGatePassGenerated,0)=1 and len(isnull(GatepassDocument,''))>2  and Id=" + Jobcardid);
        if (Dt != null && Dt.Rows.Count > 0)
            return true;
        //Reach to this point suggest false
        return false;
    }
    public System.Data.DataTable generateAndReturnGstDocs(string startdate, string enddate)
    {
        DataTable dtdocsCalc = dbConnection.GetDataTable("Select GstInvoiceNumber from Invoice where DOC>='" + startdate + " 00:00:00' and DOC<='" + enddate + " 23:59:59' and GstInvoiceNumber is not null and isnull(IsCancelled,0)=0 order by DOC");
        DataTable dtdocs = new DataTable("docs");
        dtdocs.Columns.Add("Nature of Document");
        dtdocs.Columns.Add("Sr. No. From");
        dtdocs.Columns.Add("Sr. No. To");
        dtdocs.Columns.Add("Total Number");
        dtdocs.Columns.Add("Cancelled");

        //Add Blank Row
        DataRow drdocs1 = dtdocs.NewRow();
        DataRow drdocs2 = dtdocs.NewRow();
        dtdocs.Rows.Add(drdocs1);
        dtdocs.Rows.Add(drdocs2);

        //Add Header
        DataRow drdocs3 = dtdocs.NewRow();
        drdocs3["Nature of Document"] = "Nature of Document";
        drdocs3["Sr. No. From"] = "Sr. No. From";
        drdocs3["Sr. No. To"] = "Sr. No. To";
        drdocs3["Total Number"] = "Total Number";
        drdocs3["Cancelled"] = "Cancelled";
        dtdocs.Rows.Add(drdocs3);

        DataRow drdocs = dtdocs.NewRow();
        drdocs["Nature of Document"] = "Invoices for outward supply";
        drdocs["Sr. No. From"] = (dtdocsCalc != null && dtdocsCalc.Rows.Count > 0 ? dtdocsCalc.Rows[0][0].ToString() : "");
        drdocs["Sr. No. To"] = (dtdocsCalc != null && dtdocsCalc.Rows.Count > 0 ? dtdocsCalc.Rows[dtdocsCalc.Rows.Count - 1][0].ToString() : "");
        drdocs["Total Number"] = dtdocsCalc.Rows.Count.ToString();
        drdocs["Cancelled"] = countCancelledInvoiceBetweenDate(startdate, enddate).ToString();
        dtdocs.Rows.Add(drdocs);
        return dtdocs;
    }

    private int countCancelledInvoiceBetweenDate(string startdate, string enddate)
    {
        DataTable dtCancelledInvoiceCount = dbConnection.GetDataTable("Select SUM(Diff) from (Select replace(Substring(m.GstInvoiceNumber,1,13),'/','') as Hdr, (Select Max(Convert(int,replace(Substring(g.GstInvoiceNumber,13,6),'/',''))) from Invoice as g where g.DOC>='" + startdate + " 00:00:00' and g.DOC<='" + enddate + " 23:59:59' and g.GstInvoiceNumber is not null and isnull(g.IsCancelled,0)=0 and replace(Substring(m.GstInvoiceNumber,1,13),'/','')=replace(Substring(g.GstInvoiceNumber,1,13),'/','') )-COUNT(*) as Diff from Invoice as m where m.DOC>='" + startdate + " 00:00:00' and m.DOC<='" + enddate + " 23:59:59' and m.GstInvoiceNumber is not null and isnull(m.IsCancelled,0)=0 group by replace(Substring(m.GstInvoiceNumber,1,13),'/','')) as tbl");
        if (dtCancelledInvoiceCount != null && dtCancelledInvoiceCount.Rows.Count > 0)
            return (int)dtCancelledInvoiceCount.Rows[0][0];
        return 0;
    }
    public string returnStateNameFromStateCode(string stateCode)
    {
        //SELECT [Code] ,[Final Code] FROM [dbo].[GstStatesCode] where code='24'
        DataTable dt = dbConnection.GetDataTable("SELECT [Code] ,[Final Code] FROM [dbo].[GstStatesCode] where code='" + stateCode + "'");
        if (dt != null && dt.Rows.Count > 0)
            return dt.Rows[0][0].ToString();
        return "";
    }
}
