// Decompiled with JetBrains decompiler
// Type: MotorzService.ServiceClass
// Assembly: MotorzService, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B65507A5-E79A-4012-A38D-E83C7FAAB9CC
// Assembly location: D:\New Projects\MotorzService\MotorzService.dll

using System;
using System.Collections.Generic;

namespace MotorzService
{
    public class ServiceClass
    {
        public class Login
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public string Username { get; set; }

            public string UserId { get; set; }


            public Login()
            {
                this.resultflag = "";
                this.Message = "";
                this.Username = "";
                this.UserId = "";
            }
        }
        public class Surveyor
        {
            public string resultflag { get; set; }

            public string Message { get; set; }
            public string Name { get; set; }
            public string Email { get; set; }
            public string Mobile { get; set; }
            public string AlternativeNumber { get; set; }
            public string ClaimNo { get; set; }
            public Surveyor()
            {
                resultflag = "";
                Message = "";
                Name = "";
                Email = "";
                Mobile = "";
                ClaimNo = "";
                AlternativeNumber = "";
            }

        }
        public class CameraLogin
        {
            public string resultflag { get; set; }

            public string Message { get; set; }
            public string JobCardId { get; set; }
            public string CameraId { get; set; }
            public CameraLogin()
            {
                resultflag = "";
                Message = "";
                JobCardId = "";
                CameraId = "";

            }

        }


        public class VehicleBrand
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public List<ServiceClass.BrandList> Brands { get; set; }

            public VehicleBrand()
            {
                this.resultflag = "";
                this.Message = "";
                this.Brands = new List<ServiceClass.BrandList>();
            }
        }

        public class BrandList
        {
            public string VehicleBrandId { get; set; }

            public string Name { get; set; }

            public string DisplayOrder { get; set; }

            public BrandList()
            {
                this.VehicleBrandId = "";
                this.Name = "";
                this.DisplayOrder = "";
            }
        }

        public class Segment
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public List<ServiceClass.SegmentDetails> Segments { get; set; }

            public Segment()
            {
                this.resultflag = "";
                this.Message = "";
                this.Segments = new List<ServiceClass.SegmentDetails>();
            }
        }

        public class SegmentDetails
        {
            public string SegmentId { get; set; }

            public string SegmentName { get; set; }

            public SegmentDetails()
            {
                this.SegmentId = "";
                this.SegmentName = "";
            }
        }

        public class VehicleModel
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public List<ServiceClass.ModelDetail> Models { get; set; }

            public VehicleModel()
            {
                this.resultflag = "";
                this.Message = "";
                this.Models = new List<ServiceClass.ModelDetail>();
            }
        }

        public class ModelDetail
        {
            public string ModelId { get; set; }

            public string ModelName { get; set; }
        }

        public class VehicleVariants
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public List<ServiceClass.VariantsDetail> Variants { get; set; }

            public VehicleVariants()
            {
                this.resultflag = "";
                this.Message = "";
                this.Variants = new List<ServiceClass.VariantsDetail>();
            }
        }

        public class VariantsDetail
        {
            public string VariantId { get; set; }

            public string VariantName { get; set; }

            public VariantsDetail()
            {
                this.VariantId = "";
                this.VariantName = "";
            }
        }

        public class GetVehicleDetail
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public string VehicleNo { get; set; }

            public string ChesisNo { get; set; }

            public string EngineNo { get; set; }

            public string KiloMetersRuns { get; set; }

            public ServiceClass.Customer Customer { get; set; }

            public ServiceClass.VehicleInsurance Insurance { get; set; }

            public GetVehicleDetail()
            {
                this.resultflag = "";
                this.Message = "";
                this.VehicleNo = "";
                this.ChesisNo = "";
                this.EngineNo = "";
                this.KiloMetersRuns = "";
                this.Customer = new ServiceClass.Customer();
                this.Insurance = new ServiceClass.VehicleInsurance();
            }
        }

        public class Customer
        {
            public string id { get; set; }

            public string Name { get; set; }

            public string Email { get; set; }

            public string Address { get; set; }

            public string Mobile { get; set; }

            public string DOB { get; set; }

            public string PanNo { get; set; }

            public string GstNo { get; set; }

            public Customer()
            {
                this.id = "";
                this.Name = "";
                this.Email = "";
                this.Address = "";
                this.Mobile = "";
                this.DOB = "";
                this.PanNo = "";
                this.GstNo = "";
            }
        }

        public class EstimateNewDesign
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public List<ServiceClass.EstimateDetail> EstimateDetails { get; set; }

            public EstimateNewDesign()
            {
                this.resultflag = "";
                this.Message = "";
                this.EstimateDetails = new List<ServiceClass.EstimateDetail>();
            }
        }

        public class EstimateDetail
        {
            public string Title = "";
            public string TotalAmount = "";

            public List<ServiceClass.SpareANDServiceDESC> Child { get; set; }

            public EstimateDetail()
            {
                this.Child = new List<ServiceClass.SpareANDServiceDESC>();
            }
        }

        public class SpareANDServiceDESC
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string Price { get; set; }

            public string Quantity { get; set; }
            public string JobCardDetailId { get; set; }

            public string ConsumableType { get; set; }
            public string MappingId { get; set; }
            public string IsAddedInRequisition { get; set; }
            public string IsAllocated { get; set; }
            public string RequisitionSpareId { get; set; }
            public string Receivequantity { get; set; }
            public SpareANDServiceDESC()
            {
                this.Id = "";
                this.Name = "";
                this.Price = "";
                this.Quantity = "";
                JobCardDetailId = "";
                ConsumableType = "";
                MappingId = "";
                IsAddedInRequisition = "";
                IsAllocated = "";
                RequisitionSpareId = "0";
                Receivequantity = "0";
            }
        }

        public class EstimateNew
        {
            public string resultflag = "";
            public string Message = "";
            public string SpareTitle = "";
            public string ServiceTitle = "";
            public string TotalServiceAmount = "";
            public string TotalSpareAmount = "";
            public string FinalAmount = "";
            public string Problems = "";
            public string CustomerNotes = "";
            public string AdvisiorNotes = "";
            public string TotalServiceSpareCount = "";
            public string IsResendFlag = "";
            public string InvoiceUrl = "";
            public string ProFormaInvoiceUrl = "";
            public string EstimateInvoiceUrl = "";
            public string IsOverrideAmount = "";
            public string IsEstimateSendToCustomer = "";

            public List<ServiceClass.SpareANDServiceDESC> Spares { get; set; }

            public List<ServiceClass.SpareANDServiceDESC> Services { get; set; }

            public EstimateNew()
            {
                this.Spares = new List<ServiceClass.SpareANDServiceDESC>();
                this.Services = new List<ServiceClass.SpareANDServiceDESC>();
            }
        }

        public class SpareAndServiceDetails
        {
            public string SpareTitle = "";
            public string ServiceTitle = "";
            public string TotalServiceAmount = "";
            public string TotalSpareAmount = "";
            public string FinalAmount = "";

            public List<ServiceClass.SpareANDServiceDESC> Spares { get; set; }

            public List<ServiceClass.SpareANDServiceDESC> Services { get; set; }

            public SpareAndServiceDetails()
            {
                this.Spares = new List<ServiceClass.SpareANDServiceDESC>();
                this.Services = new List<ServiceClass.SpareANDServiceDESC>();
            }
        }

        public class HomePageAdvisior
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public string TotalJobCard { get; set; }

            public string TotalOpeningJobCard { get; set; }
            public string TotalPendingJobCard { get; set; }

            public string TotalRunningJobCard { get; set; }

            public string TotalClosedJobCard { get; set; }

            public HomePageAdvisior()
            {
                this.resultflag = "";
                this.Message = "";
                this.TotalJobCard = "";
                this.TotalOpeningJobCard = "";
                this.TotalRunningJobCard = "";
                this.TotalClosedJobCard = "";
                TotalPendingJobCard = "";
            }
        }

        public class LoginAgent
        {
            public string resultflag = "";
            public string Message = "";
        }

        public class Vehicle
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string Number { get; set; }

            public string ChasisNumber { get; set; }

            public string EngineNumber { get; set; }

            public string VehicleBrand { get; set; }

            public string VehicleModel { get; set; }

            public string VehicleVariant { get; set; }

            public string VehicleSegment { get; set; }

            public string KiloMeters { get; set; }
            public string IsSchemeApplied { get; set; }

            public Vehicle()
            {
                this.Id = "";
                this.Name = "";
                this.Number = "";
                this.ChasisNumber = "";
                this.EngineNumber = "";
                this.VehicleBrand = "";
                this.VehicleModel = "";
                this.VehicleVariant = "";
                this.KiloMeters = "";
                this.VehicleSegment = "";
            }
        }

        public class VehicleInsurance
        {
            public string VehicleId { get; set; }

            public string VehicleInsuranceNumber { get; set; }

            public string Insurance_ExpireDate { get; set; }

            public string Insurance_Provider_Id { get; set; }

            public string Insurance_Status_Id { get; set; }

            public string Insurance_Status { get; set; }

            public string Insurance_Provider { get; set; }

            public VehicleInsurance()
            {
                this.VehicleId = "";
                this.VehicleInsuranceNumber = "";
                this.Insurance_ExpireDate = "";
                this.Insurance_Provider = "";
                this.Insurance_Status = "";
                this.Insurance_Provider_Id = "";
                this.Insurance_Status_Id = "";
            }
        }

        public class Spare
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public List<ServiceClass.Spares> Spares { get; set; }

            public Spare()
            {
                this.resultflag = "";
                this.Message = "";
                this.Spares = new List<ServiceClass.Spares>();
            }
        }

        public class Spares
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public Spares()
            {
                this.Id = "";
                this.Name = "";
            }
        }

        public class Service
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public List<ServiceClass.ServiceList> ServiceList { get; set; }

            public Service()
            {
                this.resultflag = "";
                this.Message = "";
                this.ServiceList = new List<ServiceClass.ServiceList>();
            }
        }

        public class ServiceList
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string Quantity { get; set; }

            public string Price { get; set; }

            public ServiceList()
            {
                this.Id = "";
                this.Name = "";
                this.Quantity = "";
                this.Price = "";
            }
        }

        public class InsuranceJobCardList
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public List<ServiceClass.InsuranceJobList> InsuranceJobList { get; set; }

            public InsuranceJobCardList()
            {
                this.InsuranceJobList = new List<ServiceClass.InsuranceJobList>();
                this.resultflag = "";
                this.Message = "";
            }
        }

        public class InsuranceJobList
        {
            public string JobCardId { get; set; }

            public string JobCardStatus { get; set; }

            public string DateOfCreate { get; set; }

            public string DateOfClose { get; set; }

            public string VehicleName { get; set; }

            public string VehicleNo { get; set; }

            public string VehicleId { get; set; }

            public string CustomerName { get; set; }

            public string CustomerMobile { get; set; }

            public string ChasisNumber { get; set; }

            public string VehicleModel { get; set; }

            public string VehicleBrand { get; set; }

            public string VehicleImageUrl { get; set; }

            public string VehicleVariant { get; set; }

            public string EstimateCost { get; set; }
            public string VideoUrl { get; set; }

            public InsuranceJobList()
            {
                this.JobCardId = "";
                this.ChasisNumber = "";
                this.JobCardStatus = "";
                this.DateOfCreate = "";
                this.VehicleId = "";
                this.VehicleName = "";
                this.VehicleNo = "";
                this.CustomerName = "";
                this.VehicleModel = "";
                this.VehicleBrand = "";
                this.VehicleVariant = "";
                this.DateOfClose = "";
                this.CustomerMobile = "";
                this.VehicleImageUrl = "";
                this.EstimateCost = "";
                VideoUrl = "";
            }
        }

        public class JobCard
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public string TotalCount { get; set; }

            public List<ServiceClass.JobCardList> JobCardList { get; set; }

            public JobCard()
            {
                this.JobCardList = new List<ServiceClass.JobCardList>();
                this.TotalCount = "";
                this.resultflag = "";
                this.Message = "";
            }
        }

        public class JobCardList
        {
            public string JobCardId { get; set; }

            public string JobCardStatus { get; set; }

            public string DateOfCreate { get; set; }

            public string DateOfClose { get; set; }

            public string VehicleName { get; set; }

            public string VehicleNo { get; set; }

            public string VehicleId { get; set; }

            public string CustomerName { get; set; }

            public string CustomerMobile { get; set; }

            public string ChasisNumber { get; set; }

            public string VehicleModel { get; set; }

            public string VehicleBrand { get; set; }

            public string VehicleVariant { get; set; }

            public string HavingInsurance { get; set; }

            public string VehicleInsuranceNumber { get; set; }

            public string Insurance_ExpireDate { get; set; }

            public string Insurance_Provider { get; set; }

            public string Insurance_Provider_Id { get; set; }

            public string Insurance_Status { get; set; }

            public string Insurance_Status_Id { get; set; }

            public JobCardList()
            {
                this.JobCardId = "";
                this.ChasisNumber = "";
                this.JobCardStatus = "";
                this.DateOfCreate = "";
                this.VehicleId = "";
                this.VehicleName = "";
                this.VehicleNo = "";
                this.CustomerName = "";
                this.VehicleModel = "";
                this.VehicleBrand = "";
                this.VehicleVariant = "";
                this.DateOfClose = "";
                this.CustomerMobile = "";
                this.HavingInsurance = "";
                this.VehicleInsuranceNumber = "";
                this.Insurance_ExpireDate = "";
                this.Insurance_Provider = "";
                this.Insurance_Provider_Id = "";
                this.Insurance_Status = "";
                this.Insurance_Status_Id = "";
            }
        }

        public class Estimate
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public List<ServiceClass.RevisedEstimate> EstimateList { get; set; }

            public Estimate()
            {
                this.resultflag = "";
                this.Message = "";
                this.EstimateList = new List<ServiceClass.RevisedEstimate>();
            }
        }

        public class RevisedEstimate
        {
            public string EstimateId { get; set; }

            public string TotalSpareAmount { get; set; }

            public string TotalServiceAmount { get; set; }

            public string FinalAmount { get; set; }

            public string IsRevisedEstimate { get; set; }

            public string EstimateType { get; set; }

            public string SummaryTitle { get; set; }

            public string IsConfirmed { get; set; }

            public string ConfirmTitle { get; set; }

            public string IsCheck { get; set; }

            public List<ServiceClass.SparewithPrice> Spares { get; set; }

            public List<ServiceClass.ServicewithPrice> Services { get; set; }

            public RevisedEstimate()
            {
                this.TotalSpareAmount = "";
                this.TotalServiceAmount = "";
                this.FinalAmount = "";
                this.IsRevisedEstimate = "";
                this.Spares = new List<ServiceClass.SparewithPrice>();
                this.Services = new List<ServiceClass.ServicewithPrice>();
                this.EstimateType = "Old";
                this.SummaryTitle = "";
                this.EstimateId = "";
                this.IsConfirmed = "";
                this.IsCheck = "";
                this.ConfirmTitle = "";
            }
        }

        public class RootSpare
        {
            public List<ServiceClass.SpareDetailRoot> Spares { get; set; }

            public RootSpare()
            {
                this.Spares = new List<ServiceClass.SpareDetailRoot>();
            }
        }

        public class RootService
        {
            public List<ServiceClass.ServiceDetailRoot> Services { get; set; }

            public RootService()
            {
                this.Services = new List<ServiceClass.ServiceDetailRoot>();
            }
        }

        public class ServiceDetailRoot
        {
            public string Id { get; set; }

            public string Amount { get; set; }

            public string Quantity { get; set; }

            public string JobCardId { get; set; }

            public ServiceDetailRoot()
            {
                this.Id = "";
                this.Amount = "";
                this.Quantity = "";
                this.JobCardId = "";
            }
        }

        public class SpareDetailRoot
        {
            public string Id { get; set; }

            public string Amount { get; set; }

            public string Quantity { get; set; }

            public string JobCardId { get; set; }

            public SpareDetailRoot()
            {
                this.Id = "";
                this.Amount = "";
                this.Quantity = "";
                this.JobCardId = "";
            }
        }

        public class InsertUpdate
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public string Id { get; set; }

            public InsertUpdate()
            {
                this.resultflag = "";
                this.Message = "";
                this.Id = "";
            }
        }

        public class FTPCredential
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public string HostId { get; set; }

            public string Username { get; set; }

            public string Password { get; set; }

            public string FolderName { get; set; }

            public FTPCredential()
            {
                this.resultflag = "1";
                this.Message = "";
                this.HostId = "";
                this.Username = "";
                this.Password = "";
                this.FolderName = "";
            }
        }

        public class InsertUpdateRecord
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public InsertUpdateRecord()
            {
                this.resultflag = "";
                this.Message = "";
            }
        }

        public class InsertRequisition
        {
            public String resultflag = "";
            public String Message = "";
            public String IsResend = "";
        }
        public class ConfirmbyCustomer
        {
            public string resultflag { get; set; }

            public string Message { get; set; }
            public string FinalAmount { get; set; }
            public ConfirmbyCustomer()
            {
                this.resultflag = "";
                this.Message = "";
                FinalAmount = "";
            }
        }

        public class InsertVehicleSpare
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public string TotalSpareAmount { get; set; }

            public string TotalServiceAmount { get; set; }

            public string FinalAmount { get; set; }

            public List<ServiceClass.SparewithPrice> Spares { get; set; }

            public List<ServiceClass.ServicewithPrice> Services { get; set; }
            public string ServiceSpareCount { get; set; }
            public string TotalServiceSpareCount { get; set; }

            public InsertVehicleSpare()
            {
                this.resultflag = "";
                this.Message = "";
                this.TotalSpareAmount = "";
                this.TotalServiceAmount = "";
                this.FinalAmount = "";
                this.Spares = new List<ServiceClass.SparewithPrice>();
                this.Services = new List<ServiceClass.ServicewithPrice>();
                ServiceSpareCount = "";
                TotalServiceSpareCount = "";
            }
        }

        public class NewJobCard
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public string JobCardNumber { get; set; }

            public string StartDate { get; set; }

            public string EndDate { get; set; }

            public string CreatedByAdvisiorId { get; set; }

            public NewJobCard()
            {
                this.resultflag = "";
                this.Message = "";
                this.JobCardNumber = "";
                this.StartDate = "";
                this.EndDate = "";
                this.CreatedByAdvisiorId = "";
            }
        }

        public class BundleAddtoNotes
        {
            public string resultflag = "";
            public string Message = "";
        }

        public class JobCardSpare
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public string SummaryTitle { get; set; }

            public string TotalSparesAmount { get; set; }

            public string TotalServiceAmount { get; set; }

            public string FinalAmount { get; set; }

            public List<ServiceClass.SparewithPrice> Spares { get; set; }

            public List<ServiceClass.ServicewithPrice> Services { get; set; }

            public JobCardSpare()
            {
                this.resultflag = "";
                this.Message = "";
                this.Spares = new List<ServiceClass.SparewithPrice>();
                this.Services = new List<ServiceClass.ServicewithPrice>();
                this.SummaryTitle = "Summary";
                this.TotalServiceAmount = "";
                this.TotalSparesAmount = "";
                this.FinalAmount = "";
            }
        }

        public class JobCardSpareForInvoice
        {
            public string FinalTotalCGSTAmount { get; set; }
            public string resultflag { get; set; }

            public string Message { get; set; }

            public string SummaryTitle { get; set; }

            public string TotalSparesAmount { get; set; }

            public string TotalServiceAmount { get; set; }

            public string FinalAmount { get; set; }
            public string TotalCGSTAmount { get; set; }
            public string TotalSGSTAmount { get; set; }
            public string TotalBeforeTaxSpare { get; set; }
            public string TotalBeforeTaxService { get; set; }
            public string SubTotalForSpare { get; set; }
            public string SubTotalForService { get; set; }
            public string TotalAmount { get; set; }
            public string TotalDiscount { get; set; }


            public List<ServiceClass.SparesForInvoice> Spares { get; set; }

            public List<ServiceClass.ServicesForInvoice> Services { get; set; }

            public JobCardSpareForInvoice()
            {
                this.resultflag = "";
                this.Message = "";
                this.Spares = new List<ServiceClass.SparesForInvoice>();
                this.Services = new List<ServiceClass.ServicesForInvoice>();
                this.SummaryTitle = "Summary";
                this.TotalServiceAmount = "";
                this.TotalSparesAmount = "";
                this.FinalAmount = "";
                TotalCGSTAmount = "";
                TotalSGSTAmount = "";
                TotalBeforeTaxSpare = "";
                TotalBeforeTaxService = "";
                SubTotalForSpare = "";
                SubTotalForService = "";
                TotalAmount = "";
                TotalDiscount = "";
            }

        }



        public class JobCardService
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public string SummaryTitle { get; set; }

            public string TotalServiceAmount { get; set; }


            public string FinalAmount { get; set; }

            public List<ServiceClass.ServicewithPrice> Services { get; set; }

            public JobCardService()
            {
                this.resultflag = "";
                this.Message = "";
                this.SummaryTitle = "Summary";
                this.TotalServiceAmount = "";
                this.Services = new List<ServiceClass.ServicewithPrice>();

            }
        }

        public class SparewithPrice
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string Quantity { get; set; }

            public string Price { get; set; }
            public string JobCardDetailId { get; set; }

            public string PurchasePrice { get; set; }

            public string IsAllocated { get; set; }
            public string ConsumableType { get; set; }
            public string Type { get; set; }
            public string MappingId { get; set; }
            public string AllowWithoutAllocation { get; set; }
            public string  Cust { get; set; }
            public string Dep { get; set; }
            public string Ins { get; set; }
            public string Discount { get; set; }
            public string IsMrpChanged { get; set; }
            public string GrnMrp { get; set; }
            public string PurchaseAmount { get; set; }
            public SparewithPrice()
            {
                PurchaseAmount = "";
                this.Id = "";
                this.Name = "";
                this.Quantity = "";
                this.Price = "";
                this.PurchasePrice = "";
                this.IsAllocated = "";
                ConsumableType = "";
                Type = "";

                this.Cust = "";
                this.Dep = "";
                this.Ins = "";
                this.Discount = "";
            }
        }


        public class SparesForInvoice
        {
            public string Id { get; set; }

            public string Name { get; set; }
            public string Type { get; set; }

            public string Quantity { get; set; }

            public string Price { get; set; }
            public string UnitPrice { get; set; }
            public string HSNNumber { get; set; }
            public string Discount { get; set; }

            public string TaxableAmount { get; set; }
            public string TotalDiscountForSpare { get; set; }
            public string CGSTRate { get; set; }
            public string CGSTAmount { get; set; }
            public string SGSTRate { get; set; }
            public string SGSTAmount { get; set; }

            public string WithOutTaxAmount { get; set; }
            public string FinalAmount { get; set; }
            public string TotalCGSTAmount { get; set; }
            public string TotalSGSTAmount { get; set; }
            public string SubTotal { get; set; }

            public string InvoiceSpareId { get; set; }
            public string InvoiceServiceId { get; set; }
            public string ItemType { get; set; }
            public string TotalDiscount { get; set; }
            public string MappingId { get; set; }
            public string Profit { get; set; }
            public string PurchasePrice { get; set; }
            public SparesForInvoice()
            {
                this.Id = "";
                this.Name = "";
                this.Quantity = "";
                this.Price = "";
                UnitPrice = "";
                TaxableAmount = "";
                CGSTRate = "";
                CGSTAmount = "";
                SGSTRate = "";
                SGSTAmount = "";
                FinalAmount = "";
                HSNNumber = "";
                Type = "";
                TotalCGSTAmount = "";
                TotalSGSTAmount = "";
                SubTotal = "";
                InvoiceSpareId = "";
                InvoiceServiceId = "";
                ItemType = "";
                TotalDiscount = "";
                TotalDiscountForSpare = "";
                Profit = "";
            }
        }
        public class ServicesForInvoice
        {
            public string Id { get; set; }

            public string Name { get; set; }
            public string Type { get; set; }

            public string Quantity { get; set; }

            public string Price { get; set; }
            public string UnitPrice { get; set; }
            public string HSNNumber { get; set; }
            public string Discount { get; set; }

            public string TaxableAmount { get; set; }
            public string CGSTRate { get; set; }
            public string CGSTAmount { get; set; }
            public string SGSTRate { get; set; }
            public string SGSTAmount { get; set; }

            public string WithOutTaxAmount { get; set; }
            public string FinalAmount { get; set; }
            public string TotalCGSTAmount { get; set; }
            public string TotalSGSTAmount { get; set; }
            public string SubTotal { get; set; }

            public string InvoiceSpareId { get; set; }
            public string InvoiceServiceId { get; set; }
            public string ItemType { get; set; }
            public string TotalDiscountForService { get; set; }
            public string TotalDiscount { get; set; }
            public string MappingId { get; set; }
            public string Profit { get; set; }
            public ServicesForInvoice()
            {
                this.Id = "";
                this.Name = "";
                this.Quantity = "";
                this.Price = "";
                UnitPrice = "";
                TaxableAmount = "";
                CGSTRate = "";
                CGSTAmount = "";
                SGSTRate = "";
                SGSTAmount = "";
                FinalAmount = "";
                HSNNumber = "";
                Type = "";
                TotalCGSTAmount = "";
                TotalSGSTAmount = "";
                SubTotal = "";
                InvoiceSpareId = "";
                InvoiceServiceId = "";
                ItemType = "";
                TotalDiscount = "";
                TotalDiscountForService = "";
                Profit = "";
            }
        }

        public class ServicewithPrice
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string Amount { get; set; }
            public string Price { get; set; }

            public string Quantity { get; set; }
            public string JobCardDetailId { get; set; }
            public ServicewithPrice()
            {
                this.Id = "";
                this.Name = "";
                this.Amount = "";
                this.Quantity = "";
                JobCardDetailId = "";
                Price = "";
            }
        }

        public class Note
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public string Id { get; set; }

            public Note()
            {
                this.resultflag = "";
                this.Message = "";
                this.Id = "";
            }
        }

        public class GetCustomerNote
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public List<ServiceClass.NoteDetail> Notes { get; set; }

            public GetCustomerNote()
            {
                this.resultflag = "";
                this.Message = "";
                this.Notes = new List<ServiceClass.NoteDetail>();
            }
        }

        public class NoteDetail
        {
            public string Id { get; set; }

            public string Name { get; set; }


            public NoteDetail()
            {
                this.Id = "";
                this.Name = "";
            }
        }

        public class GetCustomer
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public string Id { get; set; }

            public string Name { get; set; }

            public string Address { get; set; }

            public string Mobile { get; set; }

            public string Email { get; set; }

            public GetCustomer()
            {
                this.resultflag = "";
                this.Message = "";
                this.Id = "";
                this.Email = "";
                this.Name = "";
                this.Address = "";
                this.Mobile = "";
            }
        }

        public class Camera
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public string Id { get; set; }

            public string Name { get; set; }

            public string StaticIp { get; set; }

            public string InternalIp { get; set; }

            public string HttpPort { get; set; }

            public string RtspPort { get; set; }

            public ServiceClass.VehicleInformation Vehicle { get; set; }

            public ServiceClass.CustomerInformation Customer { get; set; }

            public Camera()
            {
                this.resultflag = "";
                this.Message = "";
                this.Id = "";
                this.Name = "";
                this.StaticIp = "";
                this.InternalIp = "";
                this.HttpPort = "";
                this.RtspPort = "";
                this.Vehicle = new ServiceClass.VehicleInformation();
                this.Customer = new ServiceClass.CustomerInformation();
            }
        }

        public class VehicleInformation
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string Number { get; set; }

            public string EngineNumber { get; set; }

            public string ChasisNumber { get; set; }

            public string KiloMeters { get; set; }

            public VehicleInformation()
            {
                this.Id = "";
                this.Name = "";
                this.Number = "";
                this.EngineNumber = "";
                this.ChasisNumber = "";
                this.KiloMeters = "";
            }
        }

        public class CustomerInformation
        {
            public string Name { get; set; }

            public string Email { get; set; }

            public string Mobile { get; set; }

            public string Address { get; set; }
        }

        public class ListOfAdvisior
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public List<ServiceClass.AdvisiorList> Advisiors { get; set; }

            public ListOfAdvisior()
            {
                this.Advisiors = new List<ServiceClass.AdvisiorList>();
            }
        }

        public class AdvisiorList
        {
            public string Id { get; set; }

            public string UserName { get; set; }

            public string Password { get; set; }

            public AdvisiorList()
            {
                this.Id = "";
                this.UserName = "";
                this.Password = "";
            }
        }

        public class GeneralProblems
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public List<ServiceClass.Problems> Problems { get; set; }

            public GeneralProblems()
            {
                this.Problems = new List<ServiceClass.Problems>();
            }
        }

        public class Problems
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string IsCheck { get; set; }
        }

        public class GeneralProblemsByJobCard
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public List<ServiceClass.ProblemsByJobCard> Problems { get; set; }

            public GeneralProblemsByJobCard()
            {
                this.resultflag = "";
                this.Message = "";
                this.Problems = new List<ServiceClass.ProblemsByJobCard>();
            }
        }

        public class ProblemsByJobCard
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string IsActive { get; set; }

            public ProblemsByJobCard()
            {
                this.Id = "";
                this.Name = "";
                this.IsActive = "";
            }
        }

        public class Sample
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public List<ServiceClass.Sample2> Spares { get; set; }

            public Sample()
            {
                this.resultflag = "";
                this.Message = "";
                this.Spares = new List<ServiceClass.Sample2>();
            }
        }

        public class Sample2
        {
            public string Id { get; set; }

            public string PurchasePrice { get; set; }

            public Sample2()
            {
                this.Id = "";
                this.PurchasePrice = "";
            }
        }

        public class GetSpare
        {
            public string resultflag = "";
            public string Message = "";

            public List<ServiceClass.SpareList> Spares { get; set; }

            public GetSpare()
            {
                this.Spares = new List<ServiceClass.SpareList>();
            }
        }

        //public class SpareList
        //{
        //    public string Id = "";
        //    public string Name = "";
        //    public string Price = "";
        //    public string Quantity = "";
        //    public string ConsumableType = "";

        //}
        public class SpareList
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public string Quantity { get; set; }

            public string Price { get; set; }
            public string ConsumableType { get; set; }

            public SpareList()
            {
                this.Id = "";
                this.Name = "";
                this.Quantity = "";
                this.Price = "";
                this.ConsumableType = "";
            }
        }


        public class JobCardStatus
        {
            public string TotalSpareCount { get; set; }

            public string Status { get; set; }

            public string ModelId { get; set; }

            public string VariantId { get; set; }

            public string BrandId { get; set; }

            public string VehicleNumber { get; set; }

            public string ChasisNumber { get; set; }

            public string EngineNumber { get; set; }

            public string ModelName { get; set; }

            public string VariantName { get; set; }

            public string BrandName { get; set; }

            public JobCardStatus()
            {
                this.Status = "";
                this.TotalSpareCount = "";
                this.ModelId = "";
                this.VariantId = "";
                this.BrandId = "";
                this.VehicleNumber = "";
                this.ChasisNumber = "";
                this.EngineNumber = "";
                this.ModelName = "";
                this.VariantName = "";
                this.BrandName = "";
            }
        }

        public class JobCardVehicleDetail
        {
            public string VehicleBrandId { get; set; }

            public string VehicleModelId { get; set; }

            public string resultflag { get; set; }

            public string Message { get; set; }

            public string Id { get; set; }

            public string VehicleId { get; set; }

            public string VehicleNo { get; set; }

            public string ChasisNumber { get; set; }

            public string EngineNumber { get; set; }

            public string VehicleBrand { get; set; }

            public string VehicleModel { get; set; }

            public string VehicleVariant { get; set; }

            public string VehicleSegment { get; set; }

            public string KiloMeters { get; set; }

            public string HavingInsurance { get; set; }

            public string VehicleInsuranceNumber { get; set; }

            public string Insurance_ExpireDate { get; set; }

            public string Insurance_Provider { get; set; }

            public string Insurance_Status { get; set; }

            public string Insurance_Provider_Id { get; set; }

            public string Insurance_Status_Id { get; set; }

            public string Name { get; set; }
            public string Email { get; set; }
            public string Mobile { get; set; }
            public string AlternativeNumber { get; set; }
            public string ClaimNo { get; set; }

            public string VehicleVariantId { get; set; }
            public List<ServiceClass.VehicleImages> VehicleImages { get; set; }

            public JobCardVehicleDetail()
            {   
                this.VehicleBrandId="";
                this.VehicleModelId="";
                this.resultflag = "";
                this.Message = "";
                this.Id = "";
                this.VehicleId = "";
                this.VehicleNo = "";
                this.ChasisNumber = "";
                this.EngineNumber = "";
                this.VehicleBrand = "";
                this.VehicleModel = "";
                this.VehicleVariant = "";
                this.KiloMeters = "";
                this.HavingInsurance = "";
                this.VehicleInsuranceNumber = "";
                this.Insurance_ExpireDate = "";
                this.Insurance_Provider = "";
                this.Insurance_Status = "";
                this.Insurance_Provider_Id = "";
                this.Insurance_Status_Id = "";
                this.VehicleSegment = "";
                this.VehicleImages = new List<ServiceClass.VehicleImages>();
                Name = "";
                Email = "";
                Mobile = "";
                AlternativeNumber = "";
                ClaimNo = "";
                VehicleVariantId = "";
            }
        }

        public class VehicleImages
        {
            public string Id { get; set; }

            public string Name { get; set; }

            public VehicleImages()
            {
                this.Id = "";
                this.Name = "";
            }
        }

        public class JobCardCustomerDetail
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public string Id { get; set; }

            public string Name { get; set; }

            public string Email { get; set; }

            public string Address { get; set; }

            public string Mobile { get; set; }

            public string DOB { get; set; }

            public string PanNo { get; set; }
            public string DrivingLicense { get; set; }
            public string DrivingExpireDate { get; set; }
            public string GstNo { get; set; }
            public string Problems { get; set; }
            public string AdvisiorNotes { get; set; }

            public List<ServiceClass.NoteDetail> CustomerNotes { get; set; }

            public JobCardCustomerDetail()
            {
                this.resultflag = "";
                this.Message = "";
                this.Id = "";
                this.Name = "";
                this.Email = "";
                this.Address = "";
                this.Mobile = "";
                this.DOB = "";
                this.PanNo = "";
                this.GstNo = "";
                this.CustomerNotes = new List<ServiceClass.NoteDetail>();
                AdvisiorNotes = "";
                Problems = "";
                DrivingLicense = "";
                DrivingExpireDate = "";
            }
        }

        public class InsuranceProvider
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public List<ServiceClass.InsuranceDetails> InsuranceProviders { get; set; }

            public InsuranceProvider()
            {
                this.resultflag = "";
                this.Message = "";
                this.InsuranceProviders = new List<ServiceClass.InsuranceDetails>();
            }
        }

        public class InsuranceDetails
        {
            public string Id { get; set; }

            public string Name { get; set; }
            public string Address = "";
            public string GSTIN = "";
            public string ContactNumber = "";

            public InsuranceDetails()
            {
                this.Id = "";
                this.Name = "";
            }
        }

        public class OtpGenerate
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public string Otp { get; set; }

            public string UserId { get; set; }
        }
        public class CameraMaster
        {
            public string resultflag { get; set; }

            public string Message { get; set; }
            public List<CameraList> CameraList { get; set; }
            public CameraMaster()
            {
                resultflag = "";
                Message = "";
                CameraList = new List<CameraList>();
            }
        }
        public class PendingJobCardlist
        {
            public string resultflag { get; set; }

            public string Message { get; set; }
            public List<JobCardlist> JobCardList { get; set; }
            public PendingJobCardlist()
            {
                resultflag = "";
                Message = "";
                JobCardList = new List<JobCardlist>();
            }
        }
        public class JobCardlist
        {
            public string JobCardId { get; set; }
            public string VehicleNo { get; set; }
            public string IsAssigned { get; set; }

            public string IsRunning { get; set; }
            public string AssignedTitle { get; set; }
            public JobCardlist()
            {
                JobCardId = "";
                VehicleNo = "";
                AssignedTitle = "";
                IsAssigned = "";
                IsRunning = "";
            }
        }
        public class CameraList
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Link { get; set; }
            public string IsAssigned { get; set; }
            public string IsRunning { get; set; }
            public string JobCardId { get; set; }
            public string VehicleNo { get; set; }
            public string Model { get; set; }
            public CameraList()
            {
                Id = "";
                Name = "";
                Link = "";
                IsAssigned = "";
                JobCardId = "";
                IsRunning = "";
                VehicleNo = "";
                Model = "";
            }
        }
        public class CouponCustomersList
        {
            public string resultflag { get; set; }

            public string Message { get; set; }
            public List<CouponCustomerDetails> CustomerList { get; set; }

            public CouponCustomersList()
            {
                resultflag = "";
                Message = "";
                CustomerList = new List<CouponCustomerDetails>();
            }


        }
        public class CouponCustomerDetails
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Mobile { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string Scheme { get; set; }
            public string PaymentAmount { get; set; }
            public string PaymentOption { get; set; }
            public string IsOtpVerify { get; set; }
            public string VehicleNo { get; set; }
            public string SchemeRegisterDate { get; set; }
            public CouponCustomerDetails()
            {
                Id = "";
                Name = "";
                Mobile = "";
                Email = "";
                Address = "";
                Scheme = "";
                PaymentAmount = "";
                PaymentOption = "";
                IsOtpVerify = "";
                VehicleNo = "";
                SchemeRegisterDate = "";
            }

        }

        public class CustomerJobCardDetails
        {
            public string resultflag { get; set; }

            public string Message { get; set; }

            public string JobCardId { get; set; }

            public string JobCardStatus { get; set; }

            public string DateOfCreate { get; set; }

            public string DateOfClose { get; set; }

            public string VehicleName { get; set; }

            public string VehicleNo { get; set; }

            public string VehicleId { get; set; }

            public string CustomerName { get; set; }

            public string CustomerMobile { get; set; }

            public string ChasisNumber { get; set; }

            public string VehicleModel { get; set; }

            public string VehicleBrand { get; set; }

            public string VehicleVariant { get; set; }

            public List<ServiceClass.VehicleImages> VehicleImages { get; set; }

            public string HavingInsurance { get; set; }

            public string VehicleInsuranceNumber { get; set; }

            public string Insurance_ExpireDate { get; set; }

            public string Insurance_Provider { get; set; }

            public string Insurance_Provider_Id { get; set; }

            public string Insurance_Status { get; set; }

            public string Insurance_Status_Id { get; set; }

            public string VideoUrl { get; set; }
            public List<SparesForInvoice> Spares { get; set; }
            public List<ServicewithPrice> Services { get; set; }
            public string TotalSpareAmount { get; set; }
            public string TotalServiceAmount { get; set; }
            public string FinalAmount { get; set; }
            public string TalkToAdvisiorNumber { get; set; }
            public string ConfirmCheckBoxTitle { get; set; }
            public string CustomerNotes { get; set; }
            public string AdvisiorNotes { get; set; }
            public string Problems { get; set; }

            public CustomerJobCardDetails()
            {
                this.resultflag = "";
                this.Message = "";
                this.JobCardId = "";
                this.ChasisNumber = "";
                this.JobCardStatus = "";
                this.DateOfCreate = "";
                this.VehicleId = "";
                this.VehicleName = "";
                this.VehicleNo = "";
                this.CustomerName = "";
                this.VehicleModel = "";
                this.VehicleBrand = "";
                this.VehicleVariant = "";
                this.DateOfClose = "";
                this.CustomerMobile = "";
                this.VehicleImages = new List<ServiceClass.VehicleImages>();
                this.VideoUrl = "";
                this.HavingInsurance = "";
                this.VehicleInsuranceNumber = "";
                this.Insurance_ExpireDate = "";
                this.Insurance_Provider = "";
                this.Insurance_Provider_Id = "";
                this.Insurance_Status = "";
                this.Insurance_Status_Id = "";
                Spares = new List<SparesForInvoice>();
                Services = new List<ServicewithPrice>();
                TotalSpareAmount = "";
                TotalServiceAmount = "";
                FinalAmount = "";
                TalkToAdvisiorNumber = "";
                ConfirmCheckBoxTitle = "";
                CustomerNotes = "";
                AdvisiorNotes = "";
                Problems = "";
            }
        }

    

        public class Schemes
        {
            public string resultflag { get; set; }
            public string Message { get; set; }
            public List<SchemeList> SchemeList { get; set; }
            public List<TimeSlot> TimeSlots { get; set; }
            public Schemes()
            {
                resultflag = "";
                Message = "";
                SchemeList = new List<SchemeList>();
                TimeSlots = new List<TimeSlot>();
            }
        }
        public class SelectModel
        {
            public string resultflag { get; set; }
            public string Message { get; set; }
            public string TotalCount { get; set; }
            public List<Models> Models { get; set; }
            public SelectModel()
            {
                resultflag = "";
                Message = "";
                Models = new List<Models>();
                TotalCount = "";
            }
        }
        public class Models
        {
            public string ModelId { get; set; }
            public string BrandId { get; set; }
            public string Name { get; set; }
            public Models()
            {
                BrandId = "";
                ModelId = "";
                Name = "";
            }
        }
        public class SchemeList
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
            public string Price { get; set; }
            public string IsChecked { get; set; }
            public string IsFree { get; set; }
            public SchemeList()
            {
                Id = "";
                Name = "";
                Description = "";
                Price = "";
                IsChecked = "false";
                IsFree = "";
            }
        }
        public class TimeSlot
        {
            public string Id { get; set; }
            public string Title { get; set; }
            public string IsChecked { get; set; }
            public TimeSlot()
            {
                Id = "";
                Title = "";
                IsChecked = "false";
            }
        }
        public class BookService
        {
            public string resultflag { get; set; }
            public string Message { get; set; }
            public List<BookServiceDetails> ServiceDetail { get; set; }
            public BookService()
            {
                resultflag = "";
                Message = "";
                ServiceDetail = new List<BookServiceDetails>();
            }
        }
        public class BookServiceDetails
        {
            public string id { get; set; }
            public string BookType { get; set; }
            public string Brand { get; set; }
            public string Model { get; set; }
            public string Scheme { get; set; }
            public string AppointmentDate { get; set; }
            public string SlotTime { get; set; }
            public BookServiceDetails()
            {
                id = "";
                BookType = "";
                Brand = "";
                Model = "";
                Scheme = "";
                AppointmentDate = "";
                SlotTime = "";
            }

        }
        public class HomePageNewDesign
        {
            public String resultflag { get; set; }
            public String Message { get; set; }

            public String IsSchemePurchase { get; set; }
            public List<BannerDataNew> SlidingBanner { get; set; }

            public BestSellingItems Category1 { get; set; }
            public BestSellingItems Category2 { get; set; }

            public HomePageNewDesign()
            {
                resultflag = "";
                Message = "";
                IsSchemePurchase = "";
                SlidingBanner = new List<BannerDataNew>();
                Category1 = new BestSellingItems();
                Category2 = new BestSellingItems();


            }
        }
        public class BannerDataNew
        {
            public String Title = "";
            public String ImageURL = "";
            public String Type = "";
            //Type: 1 = Service, 2= Washing, 3= Insurance
        }
        public class BestSellingItems
        {
            public String Title = "";
            public String ImageUrl = "";
            public String CategoryName = "";

            public List<TopSellingProducts> Products { get; set; }

            public BestSellingItems()
            {
                Products = new List<TopSellingProducts>();
            }
        }
        public class TopSellingProducts
        {
            public String ProductId = "";
            public String ProductName = "";
            public String ImageUrl = "";

        }
        public class JobCardDetails
        {
            public string resultflag = "";
            public string Message = "";
            public string FinalAmount = "";
            public string IsEstimateSendToCustomer = "";
            public string IsOverrideAmount = "";
            public string TotalSpareAmount = "";
            public string TotalServiceAmount = "";
            public List<JobNoteDetail> ProblemList { get; set; }

            public JobCardDetails()
            {
                ProblemList = new List<JobNoteDetail>();
            }
        }
        public class JobNoteDetail
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string type { get; set; }

            public string TypeId { get; set; }
            public string Header { get; set; }
            public string Title { get; set; }
            public string IsChecked { get; set; }
            public string ServiceSpareCount { get; set; }
            public JobNoteDetail()
            {
                Id = "";
                Name = "";
                type = "";
                TypeId = "";
                Header = "";
                Title = "";
                IsChecked = "";
                ServiceSpareCount = "";
            }
        }
        public class GeneratePDF
        {
            public string resultflag = "";
            public string Message = "";
            public string InvoiceUrl = "";

        }
        public class CustomerVehicles
        {
            public string resultflag = "";
            public string Message = "";

            public List<Vehicle> Vehicles { get; set; }
            public CustomerVehicles()
            {
                Vehicles = new List<Vehicle>();
            }
        }
        public class DateSelectionForBooking
        {
            public string resultflag = "";
            public string Message = "";

            public List<BookingDate> BookingDates { get; set; }
            public DateSelectionForBooking()
            {
                BookingDates = new List<BookingDate>();
            }

        }
        public class BookingDate
        {
            public string Type = "";
            public string Date = "";
            public string CountOfVehicleBook = "";
            public string IsAvailable = "";


        }
        public class Brands
        {
            public string resultflag = "";
            public string Message = "";
            public List<BrandListNew> BrandList { get; set; }
            public Brands()
            {
                BrandList = new List<BrandListNew>();

            }
        }
        public class BrandListNew
        {
            public string VehicleBrandId { get; set; }

            public string Name { get; set; }

            public string DisplayOrder { get; set; }

            public string IsCheck { get; set; }
            public BrandListNew()
            {
                this.VehicleBrandId = "";
                this.Name = "";
                this.DisplayOrder = "";
                IsCheck = "0";
            }
        }
        public class Segments
        {
            public string resultflag = "";
            public string Message = "";
            public List<BrandListNew> SegmentList { get; set; }
            public Segments()
            {
                SegmentList = new List<BrandListNew>();

            }
        }
        public class Model
        {
            public string resultflag = "";
            public string Message = "";
            public List<WashingModel> Models { get; set; }
            public Model()
            {
                Models = new List<WashingModel>();
            }
        }
        public class WashingModel
        {
            public string ModelId { get; set; }
            public string BrandId { get; set; }
            public string Name { get; set; }
            public string IsCheck { get; set; }
            public WashingModel()
            {
                BrandId = "";
                ModelId = "";
                Name = "";
                IsCheck = "0";
            }
        }
        public class GetSegmentModelMapping
        {
            public string resultflag = "";
            public string Message = "";
            public List<GetSegmentModel> ModelList { get; set; }
            public GetSegmentModelMapping()
            {
                ModelList = new List<GetSegmentModel>();
            }
        }
        public class GetSegmentModel
        {
            public string Id = "";
            public string SegmentId = "";
            public string ModelId = "";
            public string BrandId = "";
            public string Segment = "";
            public string Model = "";
            public string Brand = "";

        }
        public class GetModel
        {
            public string resultflag = "";
            public string Message = "";
            public string Id = "";
            public string Name = "";
            public List<WashingModel> Model { get; set; }
            public GetModel()
            {
                Model = new List<WashingModel>();
            }
        }
        public class GetBrand
        {
            public string resultflag = "";
            public string Message = "";
            public string Id = "";
            public string Name = "";
            public List<BrandListNew> BrandList { get; set; }
            public GetBrand()
            {
                BrandList = new List<BrandListNew>();
            }
        }
        public class CustomerCarSchemeDetail
        {
            public string resultflag = "";
            public string Message = "";
            public List<SchemeList> CarWashingList { get; set; }
            public List<SchemeList> Treatments { get; set; }
            public List<SchemeList> SchemeList { get; set; }
            public CustomerCarSchemeDetail()
            {
                SchemeList = new List<SchemeList>();
                Treatments = new List<SchemeList>();
                CarWashingList = new List<SchemeList>();
            }

        }
        public class RequesitionList
        {
            public string Type { get; set; }
            public string RequesitionId { get; set; }
            public string Id { get; set; }
            public string Name { get; set; }
            public string Quantity { get; set; }

            public string Amount { get; set; }
            public string IsAllocated { get; set; }
            public string AllocateStatus { get; set; }
            public string AllocateStatusId { get; set; }
            public string Available { get; set; }

            public RequesitionList()
            {
                RequesitionId = "";
                Id = "";
                Name = "";
                Quantity = "";
                Amount = "";
                IsAllocated = "";
                AllocateStatus = "";
                Type = "";
                AllocateStatusId = "";
                Available = "";
            }


        }
        public class GetRegion
        {
            public String resultflag = "";
            public String Message = "";
            public List<RegionProduct> Requisition { get; set; }
            public GetRegion()
            {
                Requisition = new List<RegionProduct>();
            }
        }
        public class RegionProduct
        {
            public String Title = "";
            public String CreateDate = "";

            public List<RequesitionList> RequisitionList { get; set; }
            public RegionProduct()
            {
                RequisitionList = new List<RequesitionList>();
            }
        }

        public class RCDetail
        {
            public string resultflag = "";
            public string Message = "";
            public string RegistrationOwner = "";
            public string CubicCapacity = "";
            public string Color = "";
            public string ModelYear = "";
            public string DateOfRegistration = "";
            public string RegValidDt = "";
            public string RTOName = "";
            public string HypothecatedTo = "";

        }

        public class ClaimDetails
        {
            public string resultflag = "";
            public string Message = "";
            public string ClaimNo = "";
            public string ClaimDate = "";
            public string ClaimAmount = "";
            public string InsuranceApprovedAmount = "";
            public string InsuranceApprovedDate = "";
            public string CompusaryExcess = "";
            public string VoluntaryExcess = "";
            public string Salvage = "";
            public string InsuranceType = "";
            public string ImageUrl = "";
        }
        public class JobCardDetail
        {
            public string resultflag = "";
            public string Message = "";
            public string JobCardId = "";
            public string JobCardDate = "";
            public string JobCardStatus = "";
            public string JobCardStatusId = "";
            public string JobCardTypeId = "";
            public string JobCardType = "";
        
        }
    }
}
