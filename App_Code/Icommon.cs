using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Icommon
/// </summary>
public interface Icommon
{
    bool IsCashless(string Jobcardid);
    bool IsAccidentJobcard(string Jobcardid);
    bool IsGatepassGenerated(string Jobcardid);
    bool IsGatepassDocumentUploadedInSystem(string Jobcardid);
    System.Data.DataTable generateAndReturnGstDocs(string startdate, string enddate);
    string returnStateNameFromStateCode(string stateCode);
}