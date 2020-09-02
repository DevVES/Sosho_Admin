using MotorzService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using WebApplication1;

/// <summary>
/// Summary description for HavingInsuranceDetails
/// </summary>
public class HavingInsuranceDetails
{
    public HavingInsuranceDetails()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public List<string> HavingInsuranceDetailsWithJobCard(string JobCardId,bool FromInvoice=false,int Type=1)
    {
        List<string> strLst = new List<string>();
        dbConnection dbCon = new dbConnection();
        ServiceMethods ServiceMethod = new ServiceMethods();

        bool HavingInsurance = false;
        var customerJobCardDetails = new ServiceClass.CustomerJobCardDetails();
        var notes = ServiceMethod.GetCustomerNoteInJobCardList(JobCardId.ToString());
        var Customer = ServiceMethod.GetCustomerByJobCardId(JobCardId.ToString());
        var Vehicle = ServiceMethod.GetVehicleByJobCardId(JobCardId.ToString());
        #region Having Insurance
        DataTable dataTableWithParams1 = dbCon.GetDataTableWithParams("Select Id, JobStatus_Id, DOC, DOM,Vehicle_Id,Customer_Id, Job_Close_Date as CloseDate, UserId from JobCard where IsDelete=0 and Id=@1", new string[1]
        {
          JobCardId.ToString()
        });
        if (dataTableWithParams1 != null)
        {
            if (dataTableWithParams1.Rows.Count > 0)
            {
                int int32_1 = Convert.ToInt32(JobCardId);
                DataRow row1 = dataTableWithParams1.Rows[0];
                customerJobCardDetails.JobCardId = row1["Id"].ToString();
                customerJobCardDetails.JobCardStatus = ServiceMethod.GetJobStatus(row1["JobStatus_Id"].ToString());
                DateTime dateTime1 = Convert.ToDateTime(row1["DOC"]);
                customerJobCardDetails.DateOfCreate = ServiceMethod.JobCardDateFormatWithTime(dateTime1);
                if (!string.IsNullOrEmpty(row1["CloseDate"].ToString()))
                {
                    DateTime dateTime2 = Convert.ToDateTime(row1["CloseDate"]);
                    try
                    {
                        customerJobCardDetails.DateOfClose = ServiceMethod.JobCardDateFormatWithTime(dateTime2);
                    }
                    catch (Exception ex)
                    {
                        customerJobCardDetails.DateOfClose = "";
                    }
                }
                if (!string.IsNullOrEmpty(row1["Vehicle_Id"].ToString()))
                {
                    int int32_2 = Convert.ToInt32(row1["Vehicle_Id"]);
                    if (int32_2 > 0)
                    {
                        ServiceClass.Vehicle vehicleById = ServiceMethod.GetVehicleById(int32_2.ToString());
                        customerJobCardDetails.VehicleId = vehicleById.Id;
                        customerJobCardDetails.VehicleNo = vehicleById.Number;
                        customerJobCardDetails.VehicleName = vehicleById.Name;
                        customerJobCardDetails.ChasisNumber = vehicleById.ChasisNumber;
                        customerJobCardDetails.VehicleBrand = vehicleById.VehicleBrand;
                        customerJobCardDetails.VehicleModel = vehicleById.VehicleModel;
                        customerJobCardDetails.VehicleVariant = vehicleById.VehicleVariant;
                        //  customerJobCardDetails.VehicleModel = vehicleById.ModelYear;
                    }
                }
                ServiceClass.InsuranceDetails InsuranceDetails =new ServiceClass.InsuranceDetails();
                if (!string.IsNullOrEmpty(row1["Vehicle_Id"].ToString()))
                {
                    int int32_2 = Convert.ToInt32(row1["Vehicle_Id"]);
                    if (int32_2 > 0)
                    {
                        ServiceClass.Customer customerByVehicleId = ServiceMethod.GetCustomerByVehicleId(int32_2.ToString());
                        customerJobCardDetails.CustomerName = customerByVehicleId.Name;
                        customerJobCardDetails.CustomerMobile = customerByVehicleId.Mobile;
                        bool flag = ServiceMethod.VehicleHavingInsurance1(int32_1.ToString());
                        HavingInsurance = flag;
                        customerJobCardDetails.HavingInsurance = Convert.ToInt32(flag).ToString();
                        if (flag)
                        {
                            ServiceClass.VehicleInsurance insuranceByVehicleId = ServiceMethod.GetVehicleInsuranceByVehicleId(int32_2.ToString());
                            customerJobCardDetails.VehicleInsuranceNumber = insuranceByVehicleId.VehicleInsuranceNumber;
                            customerJobCardDetails.Insurance_ExpireDate = insuranceByVehicleId.Insurance_ExpireDate;
                            customerJobCardDetails.Insurance_Provider = insuranceByVehicleId.Insurance_Provider;
                            customerJobCardDetails.Insurance_Provider_Id = insuranceByVehicleId.Insurance_Provider_Id;
                             InsuranceDetails=  ServiceMethod.GetVehicleInsuranceById(insuranceByVehicleId.Insurance_Provider_Id);
                            
                            customerJobCardDetails.Insurance_Status = insuranceByVehicleId.Insurance_Status;
                            customerJobCardDetails.Insurance_Status_Id = insuranceByVehicleId.Insurance_Status_Id;
                        }
                    }
                }
                DataTable dataTableWithParams2 = dbCon.GetDataTableWithParams("Select Id,Isnull(Link,'') as Link from JobCard_Vehicle_Images where JobCardId=@1 and IsDeleted=0", new string[1]
            {
              int32_1.ToString()
            });
                if (dataTableWithParams2 != null && dataTableWithParams2.Rows.Count > 0)
                {
                    foreach (DataRow row2 in (InternalDataCollectionBase)dataTableWithParams2.Rows)
                    {
                        ServiceClass.VehicleImages vehicleImages = new ServiceClass.VehicleImages();
                        string str1 = "";
                        string str2 = "";
                        if (!string.IsNullOrEmpty(row2["Link"].ToString()))
                            str1 = row2["Link"].ToString();
                        if (!string.IsNullOrEmpty(str1))
                            //     str2 = Constant.Message.FinalPathForApplication + "/" + str1;
                            if (!string.IsNullOrEmpty(str2))
                            {
                                vehicleImages.Id = row2["Id"].ToString();
                                vehicleImages.Name = str2;
                                customerJobCardDetails.VehicleImages.Add(vehicleImages);
                            }
                    }
                }
                customerJobCardDetails.CustomerNotes = ServiceMethod.GetProblemsinJobCardList(JobCardId.ToString(), "1");
                customerJobCardDetails.AdvisiorNotes = ServiceMethod.GetProblemsinJobCardList(JobCardId.ToString(), "2");
                customerJobCardDetails.Problems = ServiceMethod.GetProblemsinJobCardList(JobCardId.ToString(), "3");
                customerJobCardDetails.VideoUrl = ServiceMethod.CameraLinkbyJobCardId(JobCardId.ToString());
                customerJobCardDetails.Spares = ServiceMethod.GetJobCardSpareInJobCardForInvoice(JobCardId.ToString());
                customerJobCardDetails.Services = ServiceMethod.GetJobCardServiceInJobCard(JobCardId.ToString());
                var Spares = ServiceMethod.GetJobCardSpareInJobCardForInvoice(JobCardId.ToString());

                var getRCDetail = ServiceMethod.GetRCDetail(int.Parse(JobCardId));
               // FromInvoice = true;
                string InvoiceId = GetInvoiceIdFromJobCard(int.Parse(JobCardId), FromInvoice,Type);
                string InvoiceDate = GetInvoiceDateFromJobCard(int.Parse(JobCardId), FromInvoice, Type);
        #endregion
                if (HavingInsurance && Type==2)//Come
                {
                    string InnerHtml = "<tr style='text-transform: uppercase; '> <td colspan='1' style='text-align:center;font-weight:bold;border-right: 1px solid #ccc;border-bottom: 1px solid #ccc;'> Insurance Details </td><td colspan='2' style='text-align:center;font-weight:bold;border-bottom: 1px solid #ccc;'> Vehicle Details </td></tr><tr> <td width='50%' style='border-right: 1px solid #ccc;border-bottom: 1px solid #ccc;padding: 5px;vertical-align: top;'> <table style='width:100%;font-size:8pt !important; padding: 5px;'> <tbody><tr style='#SHOWVISIBILITYTR1#'> <td width='20%' style='padding-top: 1px;'> <span style='word-break: break-word;line-height: 1;'>Name:</span></td><td style='padding-top: 1px;' colspan='3'> <span style='word-break: break-word;line-height: 1;'>" + customerJobCardDetails.Insurance_Provider + "</span></td></tr><tr style='#SHOWVISIBILITYTR3#'> <td style='vertical-align: top;'> <span style='word-break: break-word;line-height: 1;'>GSTIN:</span></td><td style='vertical-align: top;padding-right: 2px;' colspan='3'> <span style='word-break: break-word;line-height: 1; '>" + InsuranceDetails.GSTIN + "</span></td></tr><tr> <td width='25%' style='vertical-align: top;'> <span style='word-break: break-word;line-height: 1;'>Policy Number:</span></td><td width='25%' style='vertical-align: top;padding-right: 2px;'> <span style='word-break: break-word; line-height: 1;'>" + customerJobCardDetails.VehicleInsuranceNumber + "</span></td></tr><tr style='#SHOWVISIBILITYTR4#'> <td> <span style='word-break: break-word;line-height: 1;'>Phone:</span></td><td> <span style='word-break: break-word;line-height: 1;'>" + InsuranceDetails.ContactNumber + "</span></td></tr><tr style='#SHOWVISIBILITYTR2#'> <td style='vertical-align: top;'> <span style='word-break: break-word;line-height: 1;'>Address:</span></td><td colspan='3' style='vertical-align: top;padding-right: 2px;'> <span style='word-break: break-word;line-height: 1; '>" + InsuranceDetails.Address + "</span></td></tr><tr style='border-top: 1px solid #ccc;'><td colspan='4'>&nbsp;</td></tr><tr> <td width='25%' style='vertical-align: top;'> <span style='word-break: break-word;line-height: 1;'>Exp Dt:</span></td><td width='25%' style='vertical-align: top;padding-right: 2px;'> <span style='word-break: break-word;line-height: 1; '>" + customerJobCardDetails.Insurance_ExpireDate + "</span></td><td width='19%' style='vertical-align: top;'> <span style='word-break: break-word;line-height: 1;'>Claim Dt:</span></td><td width='31%' style='vertical-align: top;padding-right: 2px;'> <span style='word-break: break-word;line-height: 1; '></span></td></tr><tr> <td style='vertical-align: top;'> <span style='word-break: break-word;line-height: 1;'>Claim No:</span></td><td style='vertical-align: top;padding-right: 2px;'> <span style='word-break: break-word; line-height: 1;'></span></td><td style='vertical-align: top;'> <span style='word-break: break-word;line-height: 1;'>IDV(Rs.):</span></td><td style='vertical-align: top;padding-right: 2px;'> <span style='word-break: break-word;line-height: 1; '></span></td></tr></tbody></table> </td><td width='50' style='border-bottom: 1px solid #ccc;padding: 5px;vertical-align: top;'><table style='width:100%;font-size:8pt !important; padding: 5px;'> <tbody><tr> <td width='20%'> <span style='word-break: break-word;line-height: 1;padding-top: 1px;'> Invoice No: </span> </td><td width='30%'> <span style='word-break: break-word;line-height: 1;padding-top: 1px;'>" + InvoiceId + " </span> </td><td width='20%'> <span style='word-break: break-word;line-height: 1;padding-top: 1px;'> Jobcard No: </span> </td><td width='30%'> <span style='word-break: break-word;line-height: 1;padding-top: 1px;'> <asp:Label ID='JobCardNo' runat='server' Text='" + JobCardId.ToString() + "'>" + JobCardId.ToString() + "</asp:Label> </span> </td></tr><tr> <td> <span style='word-break: break-word;line-height: 1;'> Invoice Date: </span> </td><td> <span style='word-break: break-word;line-height: 1;'> <asp:Label ID='Invoice_Date' runat='server' Text='" + InvoiceDate + "'>" + InvoiceDate + "</asp:Label> </span> </td><td> <span style='word-break: break-word;line-height: 1;'> Jobcard Date: </span> </td><td> <span style='word-break: break-word;line-height: 1;'> <asp:Label ID='JobCard_Date' runat='server' Text='" + customerJobCardDetails.DateOfCreate + "'>" + customerJobCardDetails.DateOfCreate + "</asp:Label></span> </td></tr><tr> <td> <span style='word-break: break-word;line-height: 1;'> Reg. Number: </span> </td><td> <span style='word-break: break-word;line-height: 1;'> <asp:Label ID='Vehicle_Reg_No' runat='server' Text='" + Vehicle.Number + "'>" + Vehicle.Number + "</asp:Label></span> </td><td> <span style='word-break: break-word;line-height: 1;'> KMS. Driven</span> </td><td> <span style='word-break: break-word;line-height: 1;'> <asp:Label ID='Kms' runat='server' Text='" + Vehicle.KiloMeters + "'>" + Vehicle.KiloMeters + "</asp:Label></span> </td></tr><tr> <td> <span style='word-break: break-word; line-height: 1;'>VIN:</span> </td><td> <span style='word-break: break-word; line-height: 1;'> <asp:Label ID='Chasis_No' runat='server' Text='" + Vehicle.ChasisNumber + "'>" + Vehicle.ChasisNumber + "</asp:Label></span> </td><td> <span style='word-break: break-word;'> Eng No: </span> </td><td> <span style='word-break: break-word;'> <asp:Label ID='EngineNo' runat='server' Text='" + Vehicle.EngineNumber + "'>" + Vehicle.EngineNumber + "</asp:Label> </span> </td></tr><tr> <td> <span style='word-break: break-word;'> Model: </span> </td><td colspan='3'> <span style='word-break: break-word;line-height: 1;'><asp:Label ID='ModelName' runat='server' Text='" + Vehicle.VehicleBrand + " " + Vehicle.VehicleModel + "'>" + Vehicle.VehicleBrand + " " + Vehicle.VehicleModel + "</asp:Label> </span> </td></tr></tbody></table>";
                    //+" <tr> <td colspan='4' style='text-align: center;font-weight:bold;border-top: solid thin #ccc;font-weight:bold;border-bottom: solid thin #ccc;#SHOWVISIBILITYTR1#;'> <strong>RC Details</strong> </td></tr><tr style='text-transform: uppercase;'> <td width='20%'> <span style='word-break: break-word;line-height: 1;'> Registration Owner : </span></td><td width='30%'> <span style='word-break: break-word;line-height: 1;'> </span></td><td width='20%'> <span style='word-break: break-word;line-height: 1;'> Registration Dt :</span></td><td width='30%'> <span>0000-00-00</span></td><!-- <td width='10%'> <span style='word-break: break-word;line-height: 1;'> Registration Dt :</span></td><td width='20%'> <span>0000-00-00</span></td>--> </tr><tr style='text-transform: uppercase;#SHOWVISIBILITYTR8#;'> <td width='20%'> <span style='word-break: break-word;line-height: 1;'> RTO Name : </span></td><td width='30%'> <span style='word-break: break-word;line-height: 1;'> </span></td><td width='20%'> <span style='word-break: break-word;line-height: 1;'>Color :</span></td><td width='30%'> <span></span></td></tr><tr style='text-transform: uppercase;#SHOWVISIBILITYTR9#;'> <td width='20%'> <span style='word-break: break-word;line-height: 1;'> Cubic Capacity : </span></td><td width='30%'> <span style='word-break: break-word;line-height: 1;'> 0</span></td><td width='20%'> <span style='word-break: break-word;line-height: 1;'>Registration valid upto :</span></td><td width='30%'> <span>0000-00-00</span></td></tr><tr style='text-transform: uppercase;#SHOWVISIBILITYTR10#;'> <td width='20%'> <span style='word-break: break-word;line-height: 1;'> Hypothecated To : </span></td><td width='30%'> <span style='word-break: break-word;line-height: 1;'> </span></td><td width='20%'> <span style='word-break: break-word;line-height: 1;'>Manufactured Yr :</span></td><td width='30%'></td></tr>";
                    string width = "20%";
                    //string InnerHtmlRc = "<table style='width:100%;font-size:8pt !important; padding: 5px;'><tbody><tr> <td colspan='4' style='text-align: center;font-weight:bold;border-top: solid thin #ccc;font-weight:bold;border-bottom: solid thin #ccc;#SHOWVISIBILITYTR1#;'> <strong>RC Details</strong> </td></tr><tr style='text-transform: uppercase;'> <td width='20%'> <span style='word-break: break-word;line-height: 1;'> Registration Owner : </span></td><td width='30%'> <span style='word-break: break-word;line-height: 1;'>" + getRCDetail.RegistrationOwner + " </span></td><td width='20%'> <span style='word-break: break-word;line-height: 1;'> Registration Dt :</span></td><td width='10%'> <span>" + getRCDetail.DateOfRegistration + " </span></td><!-- <td width='10%'> <span style='word-break: break-word;line-height: 1;'> Registration Dt :</span></td><td width='" + width + "'> <span>" + getRCDetail.DateOfRegistration + "</span></td>--> </tr><tr style='text-transform: uppercase;#SHOWVISIBILITYTR8#;'> <td width='" + width + "'> <span style='word-break: break-word;line-height: 1;'> RTO Name : </span></td><td width='30%'> <span style='word-break: break-word;line-height: 1;'>" + getRCDetail.RTOName + " </span></td><td width='" + width + "'> <span style='word-break: break-word;line-height: 1;'>Color :</span></td><td width='30%'> <span>" + getRCDetail.Color + " </span></td></tr><tr style='text-transform: uppercase;#SHOWVISIBILITYTR9#;'> <td width='" + width + "'> <span style='word-break: break-word;line-height: 1;'> Cubic Capacity : </span></td><td width='30%'> <span style='word-break: break-word;line-height: 1;'> " + getRCDetail.CubicCapacity + "</span></td><td width='" + width + "'> <span style='word-break: break-word;line-height: 1;'>Registration valid upto :</span></td><td width='30%'> <span>" + getRCDetail.RegValidDt + "</span></td></tr><tr style='text-transform: uppercase;#SHOWVISIBILITYTR10#;'> <td width='" + width + "'> <span style='word-break: break-word;line-height: 1;'> Hypothecated To : </span></td><td width='30%'> <span style='word-break: break-word;line-height: 1;'> </span></td><td width='" + width + "'> <span style='word-break: break-word;line-height: 1;'>Manufactured Yr :</span></td><td width='30%'><span>" + getRCDetail.ModelYear + "</span></td></tr>";

                    strLst.Add(InnerHtml);
                   // strLst.Add(InnerHtmlRc);
                }
                else
                {
                    string InnerHtml = "<tr style='text-transform: uppercase; '> <td colspan='1' style='text-align:center;font-weight:bold;border-right: 1px solid #ccc;border-bottom: 1px solid #ccc;'> Customer Details </td><td colspan='2' style='text-align:center;font-weight:bold;border-bottom: 1px solid #ccc;'> Vehicle Details </td></tr><tr> <td width='45%' style='border-right: 1px solid #ccc;border-bottom: 1px solid #ccc;padding: 0px 1px 0px 1px;vertical-align: top;'> <table style='width:100%;font-size:8pt !important;padding: 5px;'> <tbody><tr> <td width='17%' style='padding-top: 1px;'> <span style='word-break: break-word;line-height: 1;'>Name:</span> </td><td width='83%' style='padding-top: 1px;'> <span style='word-break: break-word; line-height: 1;'><asp:Label ID='Cus_Name' runat='server' Text='" + Customer.Name + "'>" + Customer.Name + "</asp:Label></span> </td></tr><tr> <td style='vertical-align: top;'> <span style='word-break: break-all;'>Address:</span> </td><td style='vertical-align: top;padding-right: 2px;'> <span style='word-break: break-all; '><asp:Label ID='Cus_Address' runat='server' Text='" + Customer.Address + "'>" + Customer.Address + "</asp:Label></span> </td></tr><tr> <td> <span style='word-break: break-word;line-height: 1;'>Phone:</span> </td><td> <span style='word-break: break-word;line-height: 1;'><asp:Label ID='Cus_Phone' runat='server' Text='" + Customer.Mobile + "'>" + Customer.Mobile + "</asp:Label></span> </td></tr><tr> <td> <span style='word-break: break-word;line-height: 1;'> Email: </span> </td><td> <span style='word-break: break-word;line-height: 1;'>" + Customer.Email + " </span> </td></tr><tr> <td> <span style='word-break: break-word;line-height: 1;'> GST: </span> </td><td> <span style='word-break: break-word;line-height: 1;'>" + Customer.GstNo + " </span> </td></tr></tbody></table> </td><td width='55%' style='border-bottom: 1px solid #ccc;padding: 0px 1px 0px 1px;'> <table style='width:100%;font-size:8pt !important;padding: 5px;'> <tbody><tr> <td width='20%'> <span style='word-break: break-word;line-height: 1;padding-top: 1px;'> Invoice No: </span> </td><td width='30%'> <span style='word-break: break-word;line-height: 1;padding-top: 1px;'>" + InvoiceId + "  </span> </td><td width='20%'> <span style='word-break: break-word;line-height: 1;padding-top: 1px;'> Jobcard No: </span> </td><td width='30%'> <span style='word-break: break-word;line-height: 1;padding-top: 1px;'> <asp:Label ID='JobCardNo' runat='server' Text='" + JobCardId.ToString() + "'>" + JobCardId.ToString() + "</asp:Label> </span> </td></tr><tr> <td> <span style='word-break: break-word;line-height: 1;'> Invoice Date: </span> </td><td> <span style='word-break: break-word;line-height: 1;'> <asp:Label ID='Invoice_Date' runat='server' Text='" + InvoiceDate + "'>" + InvoiceDate + "</asp:Label> </span> </td><td> <span style='word-break: break-word;line-height: 1;'> Jobcard Date: </span> </td><td> <span style='word-break: break-word;line-height: 1;'> <asp:Label ID='JobCard_Date' runat='server' Text='" + customerJobCardDetails.DateOfCreate + "'>" + customerJobCardDetails.DateOfCreate + "</asp:Label></span> </td></tr><tr> <td> <span style='word-break: break-word;line-height: 1;'> Reg. Number: </span> </td><td> <span style='word-break: break-word;line-height: 1;'> <asp:Label ID='Vehicle_Reg_No' runat='server' Text='" + Vehicle.Number + "'>" + Vehicle.Number + "</asp:Label></span> </td><td> <span style='word-break: break-word;line-height: 1;'> KMS. Driven</span> </td><td> <span style='word-break: break-word;line-height: 1;'> <asp:Label ID='Kms' runat='server' Text='" + Vehicle.KiloMeters + "'>" + Vehicle.KiloMeters + "</asp:Label></span> </td></tr><tr> <td> <span style='word-break: break-word; line-height: 1;'>VIN:</span> </td><td> <span style='word-break: break-word; line-height: 1;'> <asp:Label ID='Chasis_No' runat='server' Text='" + Vehicle.ChasisNumber + "'>" + Vehicle.ChasisNumber + "</asp:Label></span> </td><td> <span style='word-break: break-word;'> Eng No: </span> </td><td> <span style='word-break: break-word;'> <asp:Label ID='EngineNo' runat='server' Text='" + Vehicle.EngineNumber + "'>" + Vehicle.EngineNumber + "</asp:Label> </span> </td></tr><tr> <td> <span style='word-break: break-word;'> Model: </span> </td><td colspan='3'> <span style='word-break: break-word;line-height: 1;'><asp:Label ID='ModelName' runat='server' Text='" + Vehicle.VehicleBrand + " " + Vehicle.VehicleModel + "'>" + Vehicle.VehicleBrand + " " + Vehicle.VehicleModel + "</asp:Label> </span> </td></tr>";
                    strLst.Add(InnerHtml);
                }
            }
        }
        return strLst;
    }

    public List<string> HavingInsuranceDetailsWithJobCard_v1(string JobCardId, bool FromInvoice = false, int Type = 1,bool ShowHeader=false)
    {
        List<string> strLst = new List<string>();
        dbConnection dbCon = new dbConnection();
        ServiceMethods ServiceMethod = new ServiceMethods();
        bool HavingInsurance = false;
        var customerJobCardDetails = new ServiceClass.CustomerJobCardDetails();
        var notes = ServiceMethod.GetCustomerNoteInJobCardList(JobCardId.ToString());
        var Customer = ServiceMethod.GetCustomerByJobCardId(JobCardId.ToString());
        var Vehicle = ServiceMethod.GetVehicleByJobCardId(JobCardId.ToString());
        #region Having Insurance
        DataTable dataTableWithParams1 = dbCon.GetDataTableWithParams("Select Id, JobStatus_Id, DOC, DOM,Vehicle_Id,Customer_Id, Job_Close_Date as CloseDate, UserId from JobCard where IsDelete=0 and Id=@1", new string[1]
        {
          JobCardId.ToString()
        });
        if (dataTableWithParams1 != null)
        {
            if (!ShowHeader)
            {
                return strLst;
            }
            if (dataTableWithParams1.Rows.Count > 0)
            {
                int int32_1 = Convert.ToInt32(JobCardId);
                DataRow row1 = dataTableWithParams1.Rows[0];
                customerJobCardDetails.JobCardId = row1["Id"].ToString();
                customerJobCardDetails.JobCardStatus = ServiceMethod.GetJobStatus(row1["JobStatus_Id"].ToString());
                DateTime dateTime1 = Convert.ToDateTime(row1["DOC"]);
                customerJobCardDetails.DateOfCreate = ServiceMethod.JobCardDateFormatWithTime(dateTime1);
                if (!string.IsNullOrEmpty(row1["CloseDate"].ToString()))
                {
                    DateTime dateTime2 = Convert.ToDateTime(row1["CloseDate"]);
                    try
                    {
                        customerJobCardDetails.DateOfClose = ServiceMethod.JobCardDateFormatWithTime(dateTime2);
                    }
                    catch (Exception ex)
                    {
                        customerJobCardDetails.DateOfClose = "";
                    }
                }
                if (!string.IsNullOrEmpty(row1["Vehicle_Id"].ToString()))
                {
                    int int32_2 = Convert.ToInt32(row1["Vehicle_Id"]);
                    if (int32_2 > 0)
                    {
                        ServiceClass.Vehicle vehicleById = ServiceMethod.GetVehicleById(int32_2.ToString());
                        customerJobCardDetails.VehicleId = vehicleById.Id;
                        customerJobCardDetails.VehicleNo = vehicleById.Number;
                        customerJobCardDetails.VehicleName = vehicleById.Name;
                        customerJobCardDetails.ChasisNumber = vehicleById.ChasisNumber;
                        customerJobCardDetails.VehicleBrand = vehicleById.VehicleBrand;
                        customerJobCardDetails.VehicleModel = vehicleById.VehicleModel;
                        customerJobCardDetails.VehicleVariant = vehicleById.VehicleVariant;
                        //  customerJobCardDetails.VehicleModel = vehicleById.ModelYear;
                    }
                }
                ServiceClass.InsuranceDetails InsuranceDetails = new ServiceClass.InsuranceDetails();
                if (!string.IsNullOrEmpty(row1["Vehicle_Id"].ToString()))
                {
                    int int32_2 = Convert.ToInt32(row1["Vehicle_Id"]);
                    if (int32_2 > 0)
                    {
                        ServiceClass.Customer customerByVehicleId = ServiceMethod.GetCustomerByVehicleId(int32_2.ToString());
                        customerJobCardDetails.CustomerName = customerByVehicleId.Name;
                        customerJobCardDetails.CustomerMobile = customerByVehicleId.Mobile;
                        bool flag = ServiceMethod.VehicleHavingInsurance1(int32_1.ToString());
                        HavingInsurance = flag;
                        customerJobCardDetails.HavingInsurance = Convert.ToInt32(flag).ToString();
                        if (flag)
                        {
                            ServiceClass.VehicleInsurance insuranceByVehicleId = ServiceMethod.GetVehicleInsuranceByVehicleId(int32_2.ToString());
                            customerJobCardDetails.VehicleInsuranceNumber = insuranceByVehicleId.VehicleInsuranceNumber;
                            customerJobCardDetails.Insurance_ExpireDate = insuranceByVehicleId.Insurance_ExpireDate;
                            customerJobCardDetails.Insurance_Provider = insuranceByVehicleId.Insurance_Provider;
                            customerJobCardDetails.Insurance_Provider_Id = insuranceByVehicleId.Insurance_Provider_Id;
                            InsuranceDetails = ServiceMethod.GetVehicleInsuranceById(insuranceByVehicleId.Insurance_Provider_Id);

                            customerJobCardDetails.Insurance_Status = insuranceByVehicleId.Insurance_Status;
                            customerJobCardDetails.Insurance_Status_Id = insuranceByVehicleId.Insurance_Status_Id;
                        }
                    }
                }
                DataTable dataTableWithParams2 = dbCon.GetDataTableWithParams("Select Id,Isnull(Link,'') as Link from JobCard_Vehicle_Images where JobCardId=@1 and IsDeleted=0", new string[1]
            {
              int32_1.ToString()
            });
                if (dataTableWithParams2 != null && dataTableWithParams2.Rows.Count > 0)
                {
                    foreach (DataRow row2 in (InternalDataCollectionBase)dataTableWithParams2.Rows)
                    {
                        ServiceClass.VehicleImages vehicleImages = new ServiceClass.VehicleImages();
                        string str1 = "";
                        string str2 = "";
                        if (!string.IsNullOrEmpty(row2["Link"].ToString()))
                            str1 = row2["Link"].ToString();
                        if (!string.IsNullOrEmpty(str1))
                            //     str2 = Constant.Message.FinalPathForApplication + "/" + str1;
                            if (!string.IsNullOrEmpty(str2))
                            {
                                vehicleImages.Id = row2["Id"].ToString();
                                vehicleImages.Name = str2;
                                customerJobCardDetails.VehicleImages.Add(vehicleImages);
                            }
                    }
                }
                customerJobCardDetails.CustomerNotes = ServiceMethod.GetProblemsinJobCardList(JobCardId.ToString(), "1");
                customerJobCardDetails.AdvisiorNotes = ServiceMethod.GetProblemsinJobCardList(JobCardId.ToString(), "2");
                customerJobCardDetails.Problems = ServiceMethod.GetProblemsinJobCardList(JobCardId.ToString(), "3");
                customerJobCardDetails.VideoUrl = ServiceMethod.CameraLinkbyJobCardId(JobCardId.ToString());
                customerJobCardDetails.Spares = ServiceMethod.GetJobCardSpareInJobCardForInvoice(JobCardId.ToString());
                customerJobCardDetails.Services = ServiceMethod.GetJobCardServiceInJobCard(JobCardId.ToString());
                var Spares = ServiceMethod.GetJobCardSpareInJobCardForInvoice(JobCardId.ToString());

                var getRCDetail = ServiceMethod.GetRCDetail(int.Parse(JobCardId));
                // FromInvoice = true;
                string InvoiceId = GetInvoiceIdFromJobCard_v1(int.Parse(JobCardId), FromInvoice, Type);
                string InvoiceDate = GetInvoiceDateFromJobCard_v1(int.Parse(JobCardId), FromInvoice, Type);
        #endregion
                if (HavingInsurance && Type == 2)//Come
                {
                    string InnerHtml = "<tr style='text-transform: uppercase; '> <td colspan='1' style='text-align:center;font-weight:bold;border-right: 1px solid #ccc;border-bottom: 1px solid #ccc;'> Insurance Details </td><td colspan='2' style='text-align:center;font-weight:bold;border-bottom: 1px solid #ccc;'> Vehicle Details </td></tr><tr> <td width='50%' style='border-right: 1px solid #ccc;border-bottom: 1px solid #ccc;padding: 5px;vertical-align: top;'> <table style='width:100%;font-size:8pt !important; padding: 5px;'> <tbody><tr style='#SHOWVISIBILITYTR1#'> <td width='20%' style='padding-top: 1px;'> <span style='word-break: break-word;line-height: 1;'>Name:</span></td><td style='padding-top: 1px;' colspan='3'> <span style='word-break: break-word;line-height: 1;'>" + customerJobCardDetails.Insurance_Provider + "</span></td></tr><tr style='#SHOWVISIBILITYTR3#'> <td style='vertical-align: top;'> <span style='word-break: break-word;line-height: 1;'>GSTIN:</span></td><td style='vertical-align: top;padding-right: 2px;' colspan='3'> <span style='word-break: break-word;line-height: 1; '>" + InsuranceDetails.GSTIN + "</span></td></tr><tr> <td width='25%' style='vertical-align: top;'> <span style='word-break: break-word;line-height: 1;'>Policy Number:</span></td><td width='25%' style='vertical-align: top;padding-right: 2px;'> <span style='word-break: break-word; line-height: 1;'>" + customerJobCardDetails.VehicleInsuranceNumber + "</span></td></tr><tr style='#SHOWVISIBILITYTR4#'> <td> <span style='word-break: break-word;line-height: 1;'>Phone:</span></td><td> <span style='word-break: break-word;line-height: 1;'>" + InsuranceDetails.ContactNumber + "</span></td></tr><tr style='#SHOWVISIBILITYTR2#'> <td style='vertical-align: top;'> <span style='word-break: break-word;line-height: 1;'>Address:</span></td><td colspan='3' style='vertical-align: top;padding-right: 2px;'> <span style='word-break: break-word;line-height: 1; '>" + InsuranceDetails.Address + "</span></td></tr><tr style='border-top: 1px solid #ccc;'><td colspan='4'>&nbsp;</td></tr><tr> <td width='25%' style='vertical-align: top;'> <span style='word-break: break-word;line-height: 1;'>Exp Dt:</span></td><td width='25%' style='vertical-align: top;padding-right: 2px;'> <span style='word-break: break-word;line-height: 1; '>" + customerJobCardDetails.Insurance_ExpireDate + "</span></td><td width='19%' style='vertical-align: top;'> <span style='word-break: break-word;line-height: 1;'>Claim Dt:</span></td><td width='31%' style='vertical-align: top;padding-right: 2px;'> <span style='word-break: break-word;line-height: 1; '></span></td></tr><tr> <td style='vertical-align: top;'> <span style='word-break: break-word;line-height: 1;'>Claim No:</span></td><td style='vertical-align: top;padding-right: 2px;'> <span style='word-break: break-word; line-height: 1;'></span></td><td style='vertical-align: top;'> <span style='word-break: break-word;line-height: 1;'>IDV(Rs.):</span></td><td style='vertical-align: top;padding-right: 2px;'> <span style='word-break: break-word;line-height: 1; '></span></td></tr></tbody></table> </td><td width='50' style='border-bottom: 1px solid #ccc;padding: 5px;vertical-align: top;'><table style='width:100%;font-size:8pt !important; padding: 5px;'> <tbody><tr> <td width='20%'> <span style='word-break: break-word;line-height: 1;padding-top: 1px;'> Invoice No: </span> </td><td width='30%'> <span id='inv-no' style='word-break: break-word;line-height: 1;padding-top: 1px;'>" + InvoiceId + " </span> </td><td width='20%'> <span style='word-break: break-word;line-height: 1;padding-top: 1px;'> Jobcard No: </span> </td><td width='30%'> <span style='word-break: break-word;line-height: 1;padding-top: 1px;'> <asp:Label ID='JobCardNo' runat='server' Text='" + JobCardId.ToString() + "'>" + JobCardId.ToString() + "</asp:Label> </span> </td></tr><tr> <td> <span style='word-break: break-word;line-height: 1;'> Invoice Date: </span> </td><td> <span style='word-break: break-word;line-height: 1;'> <asp:Label ID='Invoice_Date' runat='server' Text='" + InvoiceDate + "'>" + InvoiceDate + "</asp:Label> </span> </td><td> <span style='word-break: break-word;line-height: 1;'> Jobcard Date: </span> </td><td> <span style='word-break: break-word;line-height: 1;'> <asp:Label ID='JobCard_Date' runat='server' Text='" + customerJobCardDetails.DateOfCreate + "'>" + customerJobCardDetails.DateOfCreate + "</asp:Label></span> </td></tr><tr> <td> <span style='word-break: break-word;line-height: 1;'> Reg. Number: </span> </td><td> <span style='word-break: break-word;line-height: 1;'> <asp:Label ID='Vehicle_Reg_No' runat='server' Text='" + Vehicle.Number + "'>" + Vehicle.Number + "</asp:Label></span> </td><td> <span style='word-break: break-word;line-height: 1;'> KMS. Driven</span> </td><td> <span style='word-break: break-word;line-height: 1;'> <asp:Label ID='Kms' runat='server' Text='" + Vehicle.KiloMeters + "'>" + Vehicle.KiloMeters + "</asp:Label></span> </td></tr><tr> <td> <span style='word-break: break-word; line-height: 1;'>VIN:</span> </td><td> <span style='word-break: break-word; line-height: 1;'> <asp:Label ID='Chasis_No' runat='server' Text='" + Vehicle.ChasisNumber + "'>" + Vehicle.ChasisNumber + "</asp:Label></span> </td><td> <span style='word-break: break-word;'> Eng No: </span> </td><td> <span style='word-break: break-word;'> <asp:Label ID='EngineNo' runat='server' Text='" + Vehicle.EngineNumber + "'>" + Vehicle.EngineNumber + "</asp:Label> </span> </td></tr><tr> <td> <span style='word-break: break-word;'> Model: </span> </td><td> <span style='word-break: break-word;line-height: 1;'><asp:Label ID='ModelName' runat='server' Text='" + Vehicle.VehicleBrand + " " + Vehicle.VehicleModel + "'>" + Vehicle.VehicleBrand + " " + Vehicle.VehicleModel + "</asp:Label> </span> </td><td> <span style='word-break: break-word;'> Customer Name: </span> </td><td> <span style='word-break: break-word;'> <asp:Label ID='CustName' runat='server' Text='" + Customer.Name + "'>" + Customer.Name + "</asp:Label> </span> </td></tr></tbody></table>";
                    //+" <tr> <td colspan='4' style='text-align: center;font-weight:bold;border-top: solid thin #ccc;font-weight:bold;border-bottom: solid thin #ccc;#SHOWVISIBILITYTR1#;'> <strong>RC Details</strong> </td></tr><tr style='text-transform: uppercase;'> <td width='20%'> <span style='word-break: break-word;line-height: 1;'> Registration Owner : </span></td><td width='30%'> <span style='word-break: break-word;line-height: 1;'> </span></td><td width='20%'> <span style='word-break: break-word;line-height: 1;'> Registration Dt :</span></td><td width='30%'> <span>0000-00-00</span></td><!-- <td width='10%'> <span style='word-break: break-word;line-height: 1;'> Registration Dt :</span></td><td width='20%'> <span>0000-00-00</span></td>--> </tr><tr style='text-transform: uppercase;#SHOWVISIBILITYTR8#;'> <td width='20%'> <span style='word-break: break-word;line-height: 1;'> RTO Name : </span></td><td width='30%'> <span style='word-break: break-word;line-height: 1;'> </span></td><td width='20%'> <span style='word-break: break-word;line-height: 1;'>Color :</span></td><td width='30%'> <span></span></td></tr><tr style='text-transform: uppercase;#SHOWVISIBILITYTR9#;'> <td width='20%'> <span style='word-break: break-word;line-height: 1;'> Cubic Capacity : </span></td><td width='30%'> <span style='word-break: break-word;line-height: 1;'> 0</span></td><td width='20%'> <span style='word-break: break-word;line-height: 1;'>Registration valid upto :</span></td><td width='30%'> <span>0000-00-00</span></td></tr><tr style='text-transform: uppercase;#SHOWVISIBILITYTR10#;'> <td width='20%'> <span style='word-break: break-word;line-height: 1;'> Hypothecated To : </span></td><td width='30%'> <span style='word-break: break-word;line-height: 1;'> </span></td><td width='20%'> <span style='word-break: break-word;line-height: 1;'>Manufactured Yr :</span></td><td width='30%'></td></tr>";
                    string width = "20%";
                    //string InnerHtmlRc = "<table style='width:100%;font-size:8pt !important; padding: 5px;'><tbody><tr> <td colspan='4' style='text-align: center;font-weight:bold;border-top: solid thin #ccc;font-weight:bold;border-bottom: solid thin #ccc;#SHOWVISIBILITYTR1#;'> <strong>RC Details</strong> </td></tr><tr style='text-transform: uppercase;'> <td width='20%'> <span style='word-break: break-word;line-height: 1;'> Registration Owner : </span></td><td width='30%'> <span style='word-break: break-word;line-height: 1;'>" + getRCDetail.RegistrationOwner + " </span></td><td width='20%'> <span style='word-break: break-word;line-height: 1;'> Registration Dt :</span></td><td width='10%'> <span>" + getRCDetail.DateOfRegistration + " </span></td><!-- <td width='10%'> <span style='word-break: break-word;line-height: 1;'> Registration Dt :</span></td><td width='" + width + "'> <span>" + getRCDetail.DateOfRegistration + "</span></td>--> </tr><tr style='text-transform: uppercase;#SHOWVISIBILITYTR8#;'> <td width='" + width + "'> <span style='word-break: break-word;line-height: 1;'> RTO Name : </span></td><td width='30%'> <span style='word-break: break-word;line-height: 1;'>" + getRCDetail.RTOName + " </span></td><td width='" + width + "'> <span style='word-break: break-word;line-height: 1;'>Color :</span></td><td width='30%'> <span>" + getRCDetail.Color + " </span></td></tr><tr style='text-transform: uppercase;#SHOWVISIBILITYTR9#;'> <td width='" + width + "'> <span style='word-break: break-word;line-height: 1;'> Cubic Capacity : </span></td><td width='30%'> <span style='word-break: break-word;line-height: 1;'> " + getRCDetail.CubicCapacity + "</span></td><td width='" + width + "'> <span style='word-break: break-word;line-height: 1;'>Registration valid upto :</span></td><td width='30%'> <span>" + getRCDetail.RegValidDt + "</span></td></tr><tr style='text-transform: uppercase;#SHOWVISIBILITYTR10#;'> <td width='" + width + "'> <span style='word-break: break-word;line-height: 1;'> Hypothecated To : </span></td><td width='30%'> <span style='word-break: break-word;line-height: 1;'> </span></td><td width='" + width + "'> <span style='word-break: break-word;line-height: 1;'>Manufactured Yr :</span></td><td width='30%'><span>" + getRCDetail.ModelYear + "</span></td></tr>";

                    strLst.Add(InnerHtml);
                    // strLst.Add(InnerHtmlRc);
                }
                else
                {
                    string InnerHtml = "<tr style='text-transform: uppercase; '> <td colspan='1' style='text-align:center;font-weight:bold;border-right: 1px solid #ccc;border-bottom: 1px solid #ccc;'> Customer Details </td><td colspan='2' style='text-align:center;font-weight:bold;border-bottom: 1px solid #ccc;'> Vehicle Details </td></tr><tr> <td width='45%' style='border-right: 1px solid #ccc;border-bottom: 1px solid #ccc;padding: 0px 1px 0px 1px;vertical-align: top;'> <table style='width:100%;font-size:8pt !important;padding: 5px;'> <tbody><tr> <td width='17%' style='padding-top: 1px;'> <span style='word-break: break-word;line-height: 1;'>Name:</span> </td><td width='83%' style='padding-top: 1px;'> <span style='word-break: break-word; line-height: 1;'><asp:Label ID='Cus_Name' runat='server' Text='" + Customer.Name + "'>" + Customer.Name + "</asp:Label></span> </td></tr><tr> <td style='vertical-align: top;'> <span style='word-break: break-all;'>Address:</span> </td><td style='vertical-align: top;padding-right: 2px;'> <span style='word-break: break-all; '><asp:Label ID='Cus_Address' runat='server' Text='" + Customer.Address + "'>" + Customer.Address + "</asp:Label></span> </td></tr><tr> <td> <span style='word-break: break-word;line-height: 1;'>Phone:</span> </td><td> <span style='word-break: break-word;line-height: 1;'><asp:Label ID='Cus_Phone' runat='server' Text='" + Customer.Mobile + "'>" + Customer.Mobile + "</asp:Label></span> </td></tr><tr> <td> <span style='word-break: break-word;line-height: 1;'> Email: </span> </td><td> <span style='word-break: break-word;line-height: 1;'>" + Customer.Email + " </span> </td></tr><tr> <td> <span style='word-break: break-word;line-height: 1;'> GST: </span> </td><td> <span style='word-break: break-word;line-height: 1;'>" + Customer.GstNo + " </span> </td></tr></tbody></table> </td><td width='55%' style='border-bottom: 1px solid #ccc;padding: 0px 1px 0px 1px;'> <table style='width:100%;font-size:8pt !important;padding: 5px;'> <tbody><tr> <td width='20%'> <span style='word-break: break-word;line-height: 1;padding-top: 1px;'> Invoice No: </span> </td><td width='30%'> <span id='inv-no'  style='word-break: break-word;line-height: 1;padding-top: 1px;'>" + InvoiceId + "  </span> </td><td width='20%'> <span style='word-break: break-word;line-height: 1;padding-top: 1px;'> Jobcard No: </span> </td><td width='30%'> <span style='word-break: break-word;line-height: 1;padding-top: 1px;'> <asp:Label ID='JobCardNo' runat='server' Text='" + JobCardId.ToString() + "'>" + JobCardId.ToString() + "</asp:Label> </span> </td></tr><tr> <td> <span style='word-break: break-word;line-height: 1;'> Invoice Date: </span> </td><td> <span style='word-break: break-word;line-height: 1;'> <asp:Label ID='Invoice_Date' runat='server' Text='" + InvoiceDate + "'>" + InvoiceDate + "</asp:Label> </span> </td><td> <span style='word-break: break-word;line-height: 1;'> Jobcard Date: </span> </td><td> <span style='word-break: break-word;line-height: 1;'> <asp:Label ID='JobCard_Date' runat='server' Text='" + customerJobCardDetails.DateOfCreate + "'>" + customerJobCardDetails.DateOfCreate + "</asp:Label></span> </td></tr><tr> <td> <span style='word-break: break-word;line-height: 1;'> Reg. Number: </span> </td><td> <span style='word-break: break-word;line-height: 1;'> <asp:Label ID='Vehicle_Reg_No' runat='server' Text='" + Vehicle.Number + "'>" + Vehicle.Number + "</asp:Label></span> </td><td> <span style='word-break: break-word;line-height: 1;'> KMS. Driven</span> </td><td> <span style='word-break: break-word;line-height: 1;'> <asp:Label ID='Kms' runat='server' Text='" + Vehicle.KiloMeters + "'>" + Vehicle.KiloMeters + "</asp:Label></span> </td></tr><tr> <td> <span style='word-break: break-word; line-height: 1;'>VIN:</span> </td><td> <span style='word-break: break-word; line-height: 1;'> <asp:Label ID='Chasis_No' runat='server' Text='" + Vehicle.ChasisNumber + "'>" + Vehicle.ChasisNumber + "</asp:Label></span> </td><td> <span style='word-break: break-word;'> Eng No: </span> </td><td> <span style='word-break: break-word;'> <asp:Label ID='EngineNo' runat='server' Text='" + Vehicle.EngineNumber + "'>" + Vehicle.EngineNumber + "</asp:Label> </span> </td></tr><tr> <td> <span style='word-break: break-word;'> Model: </span> </td><td colspan='3'> <span style='word-break: break-word;line-height: 1;'><asp:Label ID='ModelName' runat='server' Text='" + Vehicle.VehicleBrand + " " + Vehicle.VehicleModel + "'>" + Vehicle.VehicleBrand + " " + Vehicle.VehicleModel + "</asp:Label> </span> </td></tr>";
                    strLst.Add(InnerHtml);
                }
            }
        }
        return strLst;
    }
    public string CustomerNotes(string JobCardId)
    {
        ServiceMethods ServiceMethod = new ServiceMethods();
        return ServiceMethod.CustomerNotesForInvoice(JobCardId.ToString(), "2");
    }
    public string GetJobCardDate(int jobcardId)
    {
        dbConnection dbCon = new dbConnection();
        ServiceMethods ServiceMethod = new ServiceMethods();
        string name = "";
        if (jobcardId > 0)
        {
            string query = "Select Isnull(DOC,'') as DOC from JobCard where  JobCard.Id=@1";
            string[] param = { jobcardId.ToString() };
            DataTable dt = dbCon.GetDataTableWithParams(query, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                name = dr["DOC"].ToString();

                DateTime dtDate = Convert.ToDateTime(dr["DOC"]);
                name = ServiceMethod.JobCardDateFormatWithTime(dtDate);
                // name = ServiceMethod.DOBFormat(name);

            }
        }
        return name;
    }

    public string GetInvoiceIdFromJobCard(int jobcard, bool isfromInvoice,int type=1)
    {
        string InvoiceId = "";
           dbConnection dbCon = new dbConnection();
        ServiceMethods ServiceMethod = new ServiceMethods();
        if (isfromInvoice)
        {
            string query = "Select top 1 Id  from Invoice where JobCardId=@1 and type=" + type + " order by Id desc";
            string[] param = { jobcard.ToString() };
            DataTable dt = dbCon.GetDataTableWithParams(query, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                InvoiceId = dr["Id"].ToString();
            }
            
        }
        return InvoiceId;
    }
    public string GetInvoiceIdFromJobCard_v1(int jobcard, bool isfromInvoice, int type = 1)
    {
        string InvoiceId = "";
        dbConnection dbCon = new dbConnection();
        ServiceMethods ServiceMethod = new ServiceMethods();
        if (isfromInvoice)
        {
            string query = "Select top 1 [GstInvoiceNumber]  from Invoice where JobCardId=@1 and type=" + type + " and GstInvoiceNumber is not null order by Id desc";
            string[] param = { jobcard.ToString() };
            DataTable dt = dbCon.GetDataTableWithParams(query, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                InvoiceId = dr["GstInvoiceNumber"].ToString();
            }
        }
        return InvoiceId;
    }
    public string GetInvoiceDateFromJobCard(int jobcard, bool isfromInvoice, int type = 1)
    {
        string InvoiceId = "";
        dbConnection dbCon = new dbConnection();
        ServiceMethods ServiceMethod = new ServiceMethods();
        if (isfromInvoice)
        {
            string query = "Select top 1 NULLIF(DOC,'') as DOC  from Invoice where JobCardId=@1 and type=" + type + "  order by Id desc";
            string[] param = { jobcard.ToString() };
            DataTable dt = dbCon.GetDataTableWithParams(query, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                DateTime dtDate=Convert.ToDateTime(dr["DOC"]);
                InvoiceId = ServiceMethod.JobCardDateFormatWithTime(dtDate);
            }

        }
        return InvoiceId;
    }
    public string GetInvoiceDateFromJobCard_v1(int jobcard, bool isfromInvoice, int type = 1)
    {
        string InvoiceId = "";
        dbConnection dbCon = new dbConnection();
        ServiceMethods ServiceMethod = new ServiceMethods();
        if (isfromInvoice)
        {
            string query = "Select top 1 NULLIF(DOC,'') as DOC  from Invoice where JobCardId=@1 and type=" + type + "  order by Id desc";
            string[] param = { jobcard.ToString() };
            DataTable dt = dbCon.GetDataTableWithParams(query, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                DateTime dtDate = Convert.ToDateTime(dr["DOC"]);
                InvoiceId = ServiceMethod.JobCardDateFormatWithTime(dtDate);
            }

        }
        return InvoiceId;
    }

}
 