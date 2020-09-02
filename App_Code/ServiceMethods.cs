// Decompiled with JetBrains decompiler
// Type: MotorzService.ServiceMethods
// Assembly: MotorzService, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B65507A5-E79A-4012-A38D-E83C7FAAB9CC
// Assembly location: D:\New Projects\MotorzService\MotorzService.dll

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Text;
using WebApplication1;

namespace MotorzService
{
    public class ServiceMethods
    {
        private dbConnection dbCon = new dbConnection();
        private ServiceClass ServiceClass = new ServiceClass();

        public bool VehicleHavingCustomer(string VehilcleId)
        {
            bool flag = false;
            try
            {
                if (!string.IsNullOrEmpty(VehilcleId))
                {
                    DataTable dataTable = this.dbCon.GetDataTable("Select top 1 Id from Vehicle_Customer_Mapping where IsDelete=0 and  Vehicle_Id=" + VehilcleId);
                    if (dataTable != null)
                    {
                        if (dataTable.Rows.Count > 0)
                            flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return flag;
        }

        public string CameraLinkbyJobCardId(string JobCardId)
        {
            string result = Constant.Message.WorkShopVideoUrl;
            if (!String.IsNullOrEmpty(JobCardId))
            {

                //StartDate is not null and EndDate is null and
                string query = "SELECT TOP 1000 Isnull(Link,'') as Link FROM [dbo].[Camera_JobCard_Mapping] inner join Camera_Master on Camera_Master.Id=CameraId where JobCardId=@1 and AssignDate Is Not null and StartDate is not null and EndDate is null";
                string[] param = { JobCardId };
                DataTable dt = dbCon.GetDataTableWithParams(query, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    result = dr["Link"].ToString();
                }
                //else
                //{
                //    string query1 = "SELECT TOP 1000 Isnull(Link,'') as Link FROM [dbo].[Camera_JobCard_Mapping] inner join Camera_Master on Camera_Master.Id=CameraId where JobCardId=@1 and AssignDate Is Not null ";
                //    string[] param1 = { JobCardId };
                //    DataTable dt1 = dbCon.GetDataTableWithParams(query1, param1);
                //    if (dt1 != null && dt1.Rows.Count > 0)
                //    {
                //        DataRow dr = dt1.Rows[0];
                //        result = Constant.Message.WorkShopVideoUrl;
                //    }
                //}
            }
            return result;
        }
        public string GetMonths(string Month)
        {
            string result = "";
            if (!String.IsNullOrEmpty(Month))
            {
                switch (Month)
                {
                    case "01":
                        result = "January";
                        break;
                    case "1":
                        result = "January";
                        break;
                    case "2":
                        result = "february";
                        break;
                    case "02":
                        result = "february";
                        break;
                    case "03":
                        result = "March";
                        break;
                    case "3":
                        result = "March";
                        break;
                    case "04":
                        result = "April";
                        break;
                    case "4":
                        result = "April";
                        break;
                    case "05":
                        result = "May";
                        break;
                    case "5":
                        result = "May";
                        break;
                    case "06":
                        result = "June";
                        break;
                    case "6":
                        result = "June";
                        break;
                    case "07":
                        result = "July";
                        break;
                    case "7":
                        result = "July";
                        break;
                    case "08":
                        result = "August";
                        break;
                    case "8":
                        result = "August";
                        break;
                    case "09":
                        result = "september";
                        break;
                    case "9":
                        result = "september";
                        break;
                    case "10":
                        result = "Octomber";
                        break;
                    case "11":
                        result = "November";
                        break;
                    case "12":
                        result = "December";
                        break;


                }
            }
            return result;

        }
        public bool VehicleHavingInsurance(string VehilcleId)
        {
            bool flag = false;
            try
            {
                if (!string.IsNullOrEmpty(VehilcleId))
                {
                    DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("Select top 1 Id from [Vehicle_Insurance] where IsDeleted=0 and  VehicleId=@1", new string[1]
          {
            Convert.ToInt32(VehilcleId).ToString()
          });
                    if (dataTableWithParams != null)
                    {
                        if (dataTableWithParams.Rows.Count > 0)
                            flag = true;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return flag;
        }

        public ServiceClass.Customer GetCustomerByVehicleId(string VehicleId)
        {
            ServiceClass.Customer customer = new ServiceClass.Customer();
            if (!string.IsNullOrEmpty(VehicleId))
            {
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("select Customer.Id,Name,Email,[Address],Customer.Mobile from Customer inner join Vehicle_Customer_Mapping on Customer_Id=Customer.Id  where IsDelete=0 and IsDeleted=0 and IsActive=1 and Vehicle_Id=@1", new string[1]
        {
          VehicleId
        });
                if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                {
                    DataRow row = dataTableWithParams.Rows[0];
                    customer.id = row["Id"].ToString();
                    customer.Name = row["Name"].ToString();
                    customer.Email = row["Email"].ToString();
                    customer.Address = row["Address"].ToString();
                    customer.Mobile = row["Mobile"].ToString();
                }
            }
            return customer;
        }

        public ServiceClass.Customer GetCustomerById(int CustomerId)
        {
            ServiceClass.Customer customer = new ServiceClass.Customer();
            if (CustomerId > 0)
            {
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("select Customer.Id,Name,Email,[Address],Customer.Mobile from Customer  where IsDeleted=0  and IsActive=1 and Customer.Id=@1", new string[1]
        {
          CustomerId.ToString()
        });
                if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                {
                    DataRow row = dataTableWithParams.Rows[0];
                    customer.id = row["Id"].ToString();
                    customer.Name = row["Name"].ToString();
                    customer.Email = row["Email"].ToString();
                    customer.Address = row["Address"].ToString();
                    customer.Mobile = row["Mobile"].ToString();
                }
            }
            return customer;
        }

        public int GetCustomerIdbyMobile(string Mobile)
        {
            int customerid = 0;
            if (!String.IsNullOrEmpty(Mobile))
            {
                string QUer = "Select Id from Customer where Mobile=@1 and IsActive=1 and Isdeleted=0 order by DOC DESC";
                string[] param = { Mobile };
                DataTable dt = dbCon.GetDataTableWithParams(QUer, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    customerid = Convert.ToInt32(dr["Id"]);
                }
            }
            return customerid;
        }
        public int IsSchemePurchasedByCustomerOfVehicleNo(string VehicleNumber)
        {
            int result = 0;
            if (!String.IsNullOrEmpty(VehicleNumber))
            {
                string query = "Select Id from [SchemeCustomer] where Vehicleno=@1";
                string[] param = { VehicleNumber };
                DataTable dt = dbCon.GetDataTableWithParams(query, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    result = 1;
                }

            }
            return result;
        }

        public string VehicleImageByJobCardId(string JobCardId)
        {
            string str = "";
            try
            {
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("Select Top 1 Isnull(Link,'') AS Link from JobCard_Vehicle_Images where JobCardId=@1 order by DOM desc", new string[1]
        {
          JobCardId
        });
                if (dataTableWithParams != null)
                {
                    if (dataTableWithParams.Rows.Count > 0)
                    {
                        DataRow row = dataTableWithParams.Rows[0];
                        if (!string.IsNullOrEmpty(row["Link"].ToString()))
                            str = Constant.Message.FinalPathForApplication + "/" + row["Link"].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return str;
        }

        #region Surveyour Mapping JobCard
        public int SurveyorMapping(int JobCardId, int SurveyorId, string ClaimNo, string Name, string Mobile, string Email, string AlternativeMobile, string advisiorid)
        {
            int result = 0;
            try
            {
                if (JobCardId > 0 && SurveyorId > 0)
                {
                    string SelectSurveyorQuery = "SELECT TOP 1000 [Id] ,[SurveyorId],[JobCardId] ,[DOC] ,[DOM] FROM [dbo].[Surveyor_JobCard_Mapping] where JobCardId=@1 and SurveyorId=@2";
                    string[] selectSurveyorparam = { JobCardId.ToString(), SurveyorId.ToString() };
                    DataTable dtSelect = dbCon.GetDataTableWithParams(SelectSurveyorQuery, selectSurveyorparam);
                    if (dtSelect != null && dtSelect.Rows.Count > 0)
                    {
                        string id = dtSelect.Rows[0]["ID"].ToString();
                        string UpdateServyorQuery = "Update [dbo].[Surveyor_JobCard_Mapping] Set [DOM] =CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME) ";
                        if (!String.IsNullOrEmpty(ClaimNo))
                        {
                            UpdateServyorQuery += ", ClaimNo=@2 ";
                        }
                        UpdateServyorQuery += " where Id=@1";
                        string[] UpdateServyorparam = { id, ClaimNo };
                        int result1 = dbCon.ExecuteQueryWithParams(UpdateServyorQuery, UpdateServyorparam);
                        if (result1 >= 1)
                        {
                            result = 1;
                        }
                        else
                        {
                            result = 0;
                        }
                    }
                    else
                    {

                        string InsertQuery1 = "INSERT INTO [dbo].[Surveyor_JobCard_Mapping]([JobCardId],[SurveyorId] ,[ClaimNo] ,[DOC],[DOM],[AssignedDate] ,[IsActive] ,[UserId])  VALUES (@1, @2,@3,CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME),CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME),CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME),1,@4) Select SCOPE_IDENTITY()";
                        string[] param1 = { JobCardId.ToString(), SurveyorId.ToString(), ClaimNo.Replace("'", "''"), advisiorid };
                        int result2 = dbCon.ExecuteScalarQueryWithParams(InsertQuery1, param1);
                        if (result2 >= 1)
                        {
                            result = 1;
                        }
                        else
                        {
                            result = 0;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result = -1;
                dbCon.InsertLogs(ex.ToString());
            }

            return result;
        }

        #endregion
        public ServiceClass.Surveyor GetSurveyorByJobCard(string JobCardId)
        {
            var surveyor = new ServiceClass.Surveyor();
            try
            {
                if (!String.IsNullOrEmpty(JobCardId))
                {
                    int jobcardid = Convert.ToInt32(JobCardId);
                    if (jobcardid > 0)
                    {
                        string query = "SELECT Surveyor_JobCard_Mapping.SurveyorId, Isnull(Surveyor_JobCard_Mapping.ClaimNo,0) as ClaimNo, Isnull(serveur.Name,'') as Name,Isnull( serveur.Mobile,'') as MobileNo, Isnull(serveur.Email,'') as Email, Isnull(serveur.AlternativeMobile,'') as AlternativeMobile FROM Surveyor_JobCard_Mapping INNER JOIN  serveur ON Surveyor_JobCard_Mapping.SurveyorId = serveur.Id where JobCardId=@1 order by [Surveyor_JobCard_Mapping].DOM DESC";
                        string[] param = { JobCardId };
                        DataTable dt = dbCon.GetDataTableWithParams(query, param);
                        if (dt != null && dt.Rows.Count > 0)
                        {
                            DataRow dr = dt.Rows[0];
                            surveyor = new ServiceClass.Surveyor();
                            surveyor.Name = dr["Name"].ToString();
                            surveyor.Mobile = dr["MobileNo"].ToString();
                            surveyor.Email = dr["Email"].ToString();
                            surveyor.AlternativeNumber = dr["AlternativeMobile"].ToString();
                            surveyor.ClaimNo = dr["ClaimNo"].ToString();
                            surveyor.resultflag = "1";
                            surveyor.Message = Constant.Message.SuccessMessage;

                        }
                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                dbCon.InsertLogs(ex.ToString());
            }
            return surveyor;
        }

        public decimal JobCardDetailFinalAmount(int JobCardId)
        {
            decimal result = 0;
            if (JobCardId > 0)
            {
                string selectquery = "Select top 1 Id,Isnull(FinalAmount,0) as FinalAmount From JobCard_Final_Amount_Change where JobCardId=@1 order by DOM DESC";
                string[] param = { JobCardId.ToString() };
                DataTable dt = dbCon.GetDataTableWithParams(selectquery, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    result = Convert.ToDecimal(dr["FinalAmount"]);
                }
                else
                {
                    result = (GetTotalServiceAmount(JobCardId.ToString()) + GetTotalSpareAmount(JobCardId.ToString()));
                }
            }
            return result;

        }

        public ServiceClass.Customer GetCustomerByJobCardId(string JobCardId)
        {
            ServiceClass.Customer customer = new ServiceClass.Customer();
            if (!string.IsNullOrEmpty(JobCardId))
            {
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("select Customer.Id,Name,Isnull(Email,'') as Email,Isnull([Address],'') as Address,Customer.Mobile,DOB,Isnull(Pan_No,'') as PanNo, Isnull(Gst_No,'') as GstNo,Isnull(DrivingLicenceNo,'') as DrivingLicenceNo, Isnull(DrivingLicenceExpireDate,'') as DrivingLicenceExpireDate  from Customer inner join JobCard on JobCard.Customer_Id=Customer.Id where JobCard.Id=@1", new string[1]
        {
          JobCardId
        });
                if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                {
                    DataRow row = dataTableWithParams.Rows[0];
                    customer.Address = row["Address"].ToString();
                    customer.Email = row["Email"].ToString();
                    customer.Mobile = row["Mobile"].ToString();
                    customer.id = row["Id"].ToString();
                    customer.Name = row["Name"].ToString();
                    if (!string.IsNullOrEmpty(row["DOB"].ToString()))
                        customer.DOB = row["DOB"].ToString();
                    customer.PanNo = row["PanNo"].ToString();
                    customer.GstNo = row["GstNo"].ToString();

                }
            }
            return customer;
        }

        public ServiceClass.Vehicle GetVehicleByJobCardId(string JobCardId)
        {
            ServiceClass.Vehicle vehicle = new ServiceClass.Vehicle();
            try
            {
                if (!string.IsNullOrEmpty(JobCardId))
                {
                    DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("select Vehicle.Id,Isnull(Name,'') as Name,Isnull(Number,'') as Number,Isnull(Chasis_Number,'') as Chasis_Number,Isnull(Engine_Number,'') as Engine_Number,Isnull(JobCard.KiloMetersRuns1,Vehicle.KiloMetersRuns) as KiloMetersRuns,Isnull(Vehicle_Brand_Id,0) as Vehicle_Brand_Id ,Isnull(Vehicle_Variant_Id,0) as Vehicle_Variant_Id ,Isnull(Vehicle_Model_Id,0) as Vehicle_Model_Id ,ISNULL(ModelYear,'') as ModelYear from Vehicle   inner join JobCard on JobCard.Vehicle_Id=Vehicle.Id where IsDeleted=0  and JobCard.Id=@1", new string[1]
        {
          JobCardId
        });
                    if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                    {
                        DataRow row = dataTableWithParams.Rows[0];
                        vehicle.Id = row["Id"].ToString();
                        vehicle.Name = row["Name"].ToString();
                        vehicle.Number = row["Number"].ToString();
                        vehicle.ChasisNumber = row["Chasis_Number"].ToString();
                        vehicle.EngineNumber = row["Engine_Number"].ToString();
                        vehicle.VehicleBrand = this.GetVehicleBrandById(row["Vehicle_Brand_Id"].ToString());
                        vehicle.VehicleModel = this.GetVehicleModelById(row["Vehicle_Model_Id"].ToString());
                        vehicle.VehicleVariant = this.GetVehicleVariantById(row["Vehicle_Variant_Id"].ToString());
                        vehicle.VehicleSegment = this.GetVehicleSegmentByModelId(row["Vehicle_Model_Id"].ToString());
                        vehicle.KiloMeters = row["KiloMetersRuns"].ToString();

                    }
                }
            }
            catch (Exception E) { }
            return vehicle;
        }

        public ServiceClass.Vehicle GetVehicleById(string VehicleId)
        {
            ServiceClass.Vehicle vehicle = new ServiceClass.Vehicle();
            if (!string.IsNullOrEmpty(VehicleId))
            {
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("select Vehicle.Id,Isnull(Name,'') as Name,Number,Chasis_Number,Engine_Number,KiloMetersRuns,Vehicle_Brand_Id ,Vehicle_Variant_Id ,Vehicle_Model_Id,Isnull(ModelYear,'') as ModelYear from Vehicle   where IsDeleted=0  and Vehicle.Id=@1", new string[1]
        {
          VehicleId
        });
                if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                {
                    DataRow row = dataTableWithParams.Rows[0];
                    vehicle.Id = row["Id"].ToString();
                    vehicle.Name = row["Name"].ToString();
                    vehicle.Number = row["Number"].ToString();
                    vehicle.ChasisNumber = row["Chasis_Number"].ToString();
                    vehicle.EngineNumber = row["Engine_Number"].ToString();

                    vehicle.VehicleBrand = this.GetVehicleBrandById(row["Vehicle_Brand_Id"].ToString());
                    vehicle.VehicleModel = this.GetVehicleModelById(row["Vehicle_Model_Id"].ToString());
                    vehicle.VehicleVariant = this.GetVehicleVariantById(row["Vehicle_Variant_Id"].ToString());
                    vehicle.VehicleSegment = this.GetVehicleSegmentByModelId(row["Vehicle_Model_Id"].ToString());

                    vehicle.KiloMeters = row["KiloMetersRuns"].ToString();
                }
            }
            return vehicle;
        }
        public int GetVehicleSpareServiceByJobCardDetailId(string JobCardDetailId)
        {
            int result = 0;
            if (!String.IsNullOrEmpty(JobCardDetailId))
            {
                int detailid = Convert.ToInt32(JobCardDetailId);
                if (detailid > 0)
                {
                    string query = "Select COUNT(Id) from JobCard_Service_Mapping where JobCardDetailId=@1 and IsDeleted=0";
                    string[] param = { detailid.ToString() };
                    DataTable dt = dbCon.GetDataTableWithParams(query, param);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        result += Convert.ToInt32(dt.Rows[0][0]);
                    }
                }
                // int detailid = Convert.ToInt32(JobCardDetailId);
                if (detailid > 0)
                {
                    string query = "Select COUNT(Id) from JobCard_Spare_Mapping where JobCardDetailId=@1 and IsDeleted=0";
                    string[] param = { detailid.ToString() };
                    DataTable dt = dbCon.GetDataTableWithParams(query, param);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        result += Convert.ToInt32(dt.Rows[0][0]);
                    }
                }
            }
            return result;
        }

        public int ChangeJobCardType(string JobCardId)
        {
            int result = 0;
            try
            {
                if (!String.IsNullOrEmpty(JobCardId))
                {
                    string query = "update JobCard set JobStatus_Id=4 where JobStatus_Id=2 and Id=@1";
                    string[] param = { JobCardId };
                    result = dbCon.ExecuteQueryWithParams(query, param);
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int CloseJobCardType(string JobCardId)
        {
            int result = 0;
            try
            {
                if (!String.IsNullOrEmpty(JobCardId))
                {
                    string query = "update JobCard set JobStatus_Id=3, Job_Close_Date=CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME) where JobStatus_Id=4 and Id=@1";
                    string[] param = { JobCardId };
                    result = dbCon.ExecuteQueryWithParams(query, param);
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int PendingJobCardType(string JobCardId)
        {
            int result = 0;
            try
            {
                if (!String.IsNullOrEmpty(JobCardId))
                {
                    string query = "update JobCard set JobStatus_Id=2 where Id=@1";
                    string[] param = { JobCardId };
                    result = dbCon.ExecuteQueryWithParams(query, param);
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public string GetVehicleSegmentByModelId(string ModelId)
        {
            string str = "";
            if (!string.IsNullOrEmpty(ModelId) && Convert.ToInt32(ModelId) > 0)
            {
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("select Isnull(Segment.Name,'') as Name from Vehicle_Model inner join Segment on Segment.Id=SegmentId where Vehicle_Model.Id=@1", new string[1]
        {
          ModelId
        });
                if (dataTableWithParams.Rows.Count > 0 && dataTableWithParams != null)
                    str = dataTableWithParams.Rows[0]["Name"].ToString();
            }
            return str;
        }
        public string VehicleBrandIdbyName(string Name)
        {
            string str = "0";
            if (!String.IsNullOrEmpty(Name))
            {
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("select Isnull(Id,0) as Id from Vehicle_Brand where Name  like Concat('%',@1,'%')", new string[1]
        {
          Name
        });
                if (dataTableWithParams.Rows.Count > 0 && dataTableWithParams != null)
                    str = dataTableWithParams.Rows[0]["Id"].ToString();
            }
            return str;
        }

        public string VehicleSegmentIdbyName(string Name)
        {
            string str = "0";
            if (!String.IsNullOrEmpty(Name))
            {
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("select Isnull(Id,0) as Id from [Segment] where Name  like Concat('%',@1,'%')", new string[1]
        {
          Name
        });
                if (dataTableWithParams.Rows.Count > 0 && dataTableWithParams != null)
                    str = dataTableWithParams.Rows[0]["Id"].ToString();
            }
            return str;
        }

        public string GetVehicleBrandById(string Id)
        {
            string str = "";
            if (!string.IsNullOrEmpty(Id) && Convert.ToInt32(Id) > 0)
            {
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("select Isnull(Name,'') as Name from Vehicle_Brand where Id=@1", new string[1]
        {
          Id
        });
                if (dataTableWithParams.Rows.Count > 0 && dataTableWithParams != null)
                    str = dataTableWithParams.Rows[0]["Name"].ToString();
            }
            return str;
        }

        public string GetVehicleModelById(string Id)
        {
            string str = "";
            if (!string.IsNullOrEmpty(Id) && Convert.ToInt32(Id) > 0)
            {
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("select Isnull(Name,'') as Name from Vehicle_Model where Id=@1", new string[1]
        {
          Id
        });
                if (dataTableWithParams.Rows.Count > 0 && dataTableWithParams != null)
                    str = dataTableWithParams.Rows[0]["Name"].ToString();
            }
            return str;
        }



        public string GetVehicleBrandWashingById(string Id)
        {
            string str = "";
            if (!string.IsNullOrEmpty(Id) && Convert.ToInt32(Id) > 0)
            {
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("select Isnull(Washing_Vehicle_Brand.Name,'') as Name from Washing_Vehicle_Brand inner join Washing_Vehicle_Model on Washing_Vehicle_Model.Vehicle_Brand_Id=Washing_Vehicle_Brand.Id where Washing_Vehicle_Model.Id=@1", new string[1]
        {
          Id
        });
                if (dataTableWithParams.Rows.Count > 0 && dataTableWithParams != null)
                    str = dataTableWithParams.Rows[0]["Name"].ToString();
            }
            return str;
        }
        public string GetVehicleBrandbyId(string Id)
        {
            string str = "";
            if (!string.IsNullOrEmpty(Id) && Convert.ToInt32(Id) > 0)
            {
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("select Isnull(Name,'') as Name from Washing_Vehicle_Brand  where Id=@1", new string[1]
        {
          Id
        });
                if (dataTableWithParams.Rows.Count > 0 && dataTableWithParams != null)
                    str = dataTableWithParams.Rows[0]["Name"].ToString();
            }
            return str;
        }

        public string GetVehicelModelWashingById(string Id)
        {
            string str = "";
            if (!string.IsNullOrEmpty(Id) && Convert.ToInt32(Id) > 0)
            {
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("select Isnull(Name,'') as Name from Washing_Vehicle_Model where Id=@1", new string[1]
        {
          Id
        });
                if (dataTableWithParams.Rows.Count > 0 && dataTableWithParams != null)
                    str = dataTableWithParams.Rows[0]["Name"].ToString();
            }
            return str;
        }
        public string GetWashingSegmentById(string Id)
        {
            string str = "";
            try
            {
                if (!string.IsNullOrEmpty(Id) && Convert.ToInt32(Id) > 0)
                {
                    DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("select Isnull(Name,'') as Name from WashingSegment where Id=@1", new string[1]
        {
          Id
        });
                    if (dataTableWithParams.Rows.Count > 0 && dataTableWithParams != null)
                        str = dataTableWithParams.Rows[0]["Name"].ToString();
                }
            }
            catch (Exception ex)
            {

            }
            return str;

        }


        public string GetScehmeById(string SchemeId)
        {
            string str = "";
            if (!String.IsNullOrEmpty(SchemeId))
            {
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("Select Isnull(Name,'') as Name from Schemes where Id=@1", new string[1]
        {
          SchemeId
        });
                if (dataTableWithParams.Rows.Count > 0 && dataTableWithParams != null)
                    str = dataTableWithParams.Rows[0]["Name"].ToString();
            }
            return str;
        }


        public string GetTimeSlotById(string TimeSlotId)
        {
            string str = "";
            if (!String.IsNullOrEmpty(TimeSlotId))
            {
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("Select Isnull(SlotTitle,'') as SlotTitle from Slot_Time where Id=@1", new string[1]
        {
          TimeSlotId
        });
                if (dataTableWithParams.Rows.Count > 0 && dataTableWithParams != null)
                    str = dataTableWithParams.Rows[0]["SlotTitle"].ToString();
            }
            return str;
        }

        public int GetTimeSlotByDate(DateTime dt)
        {
            int result = 0;
            try
            {
                string endDatetime = dt.ToString("yyyy-MM-dd HH:mm:ss");
                string query = "Select Isnull(Count(Slot_Time_id),0) as SlotTime  from BookingService where AppointmentDate is not null and   CreatedOnUtc == CONVERT(DATETIME, '" + endDatetime + "', 102)";
                DataTable dtnew = dbCon.GetDataTable(query);
                if (dtnew != null && dtnew.Rows.Count > 0)
                {
                    result = Convert.ToInt32(dtnew.Rows[0]["SlotTime"]);
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public string GetVehicleVariantById(string Id)
        {
            string str = "";
            if (!string.IsNullOrEmpty(Id) && Convert.ToInt32(Id) > 0)
            {
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("select Isnull(Name,'') as Name from Vehicle_Variant where Id=@1", new string[1]
        {
          Id
        });
                if (dataTableWithParams.Rows.Count > 0 && dataTableWithParams != null)
                    str = dataTableWithParams.Rows[0]["Name"].ToString();
            }
            return str;
        }

        public ServiceClass.VehicleInsurance GetVehicleInsuranceByVehicleId(string VehicleId)
        {
            ServiceClass.VehicleInsurance vehicleInsurance = new ServiceClass.VehicleInsurance();
            if (!string.IsNullOrEmpty(VehicleId))
            {
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("SELECT  [Id],[VehicleId],[Insurance_Provider_Id],[Insurance_Number],[Insurance_Expire_Date],[Insurance_Status_Id] FROM [dbo].[Vehicle_Insurance] where VehicleId=@1", new string[1]
        {
          VehicleId
        });
                if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                {
                    DataRow row = dataTableWithParams.Rows[0];
                    vehicleInsurance.VehicleId = row["Id"].ToString();
                    vehicleInsurance.VehicleInsuranceNumber = row["Insurance_Number"].ToString();
                    string str = "";
                    try
                    {
                        if (!string.IsNullOrEmpty(row["Insurance_Expire_Date"].ToString()))
                            str = Convert.ToDateTime(row["Insurance_Expire_Date"]).ToString("dd-MM-yyyy");
                    }
                    catch (Exception ex)
                    {
                    }
                    vehicleInsurance.Insurance_ExpireDate = str;
                    vehicleInsurance.Insurance_Provider = this.GetInsuranceProviderById(row["Insurance_Provider_Id"].ToString());
                    vehicleInsurance.Insurance_Status = this.GetInsuranceStatusById(row["Insurance_Status_Id"].ToString());
                    vehicleInsurance.Insurance_Provider_Id = row["Insurance_Provider_Id"].ToString();
                    vehicleInsurance.Insurance_Status_Id = row["Insurance_Status_Id"].ToString();
                }
            }
            return vehicleInsurance;
        }

        public ServiceClass.InsuranceDetails GetVehicleInsuranceById(string Id)
        {
            ServiceClass.InsuranceDetails InsuranceDetails = new ServiceClass.InsuranceDetails();
            try
            {
                string query = "Select Id, Isnull(Name,'') as Name, Isnull(GSTIN,'') as GSTIN, Isnull(InsuranceAddress,'') as InsuranceAddress, Isnull(ContactNumber,'') as ContactNumber from InsuranceProvider where IsActive=1 and Id=@1";
                string[] param = { Id };
                DataTable dt = dbCon.GetDataTableWithParams(query, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    InsuranceDetails.Id = dr["Id"].ToString();
                    InsuranceDetails.Name = dr["Name"].ToString();
                    InsuranceDetails.GSTIN = dr["GSTIN"].ToString();
                    InsuranceDetails.Address = dr["InsuranceAddress"].ToString();
                    InsuranceDetails.ContactNumber = dr["ContactNumber"].ToString();
                }
            }
            catch (Exception ex)
            {
                return new ServiceClass.InsuranceDetails();
            }
            return InsuranceDetails;
        }

        public string GetInsuranceProviderById(string Id)
        {
            string str = "";
            if (!string.IsNullOrEmpty(Id))
            {
                int num = 0;
                try
                {
                    num = Convert.ToInt32(Id);
                }
                catch (Exception ex)
                {
                }
                if (num > 0)
                {
                    DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("select Name from InsuranceProvider where IsActive=1 and Id =@1;", new string[1]
          {
            Id
          });
                    if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                        str = dataTableWithParams.Rows[0]["Name"].ToString();
                }
            }
            return str;
        }

        public int GetInsuranceProviderByJobCardId(string JobCardId)
        {
            int insuranceid = 0;
            if (!string.IsNullOrEmpty(JobCardId))
            {
                int num = 0;
                try
                {
                    num = Convert.ToInt32(JobCardId);
                }
                catch (Exception ex)
                {
                }
                if (num > 0)
                {
                    DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("SELECT JobCard.Vehicle_Id, Vehicle_Insurance.Insurance_Provider_Id FROM  JobCard INNER JOIN  Vehicle_Insurance ON JobCard.Vehicle_Id = Vehicle_Insurance.VehicleId where JobCard.Id=@1", new string[1]
          {
            JobCardId
          });
                    if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                        insuranceid = Convert.ToInt32(dataTableWithParams.Rows[0]["Insurance_Provider_Id"]);
                }
            }
            return insuranceid;
        }

        public string GetInsuranceStatusById(string Id)
        {
            string str = "";
            if (!string.IsNullOrEmpty(Id))
            {
                int num = 0;
                try
                {
                    num = Convert.ToInt32(Id);
                }
                catch (Exception ex)
                {
                }
                if (num > 0)
                {
                    DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("select Name from InsuranceStatus where IsActive=1 and Id =@1;", new string[1]
          {
            Id
          });
                    if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                        str = dataTableWithParams.Rows[0]["Name"].ToString();
                }
            }
            return str;
        }

        public int InsertActivityLog(string UserId)
        {
            int num = 0;
            try
            {
                num = this.dbCon.ExecuteQuery("INSERT INTO [dbo].[ActivityLog]([ActivityLogTypeId],[UserId],[Comment],[CreatedOnUtc],[IpAddress]) VALUES(0,@UserId,@Comment,GETDATE(),@IpAddress)");
            }
            catch (Exception ex)
            {
            }
            return num;
        }

        public DateTime getindiantime()
        {
            try
            {
                return TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
            }
            catch (Exception ex)
            {
                return DateTime.Now;
            }
        }

        public string GetJobStatus(string Id)
        {
            string str = "";
            try
            {
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("select Name from JobStatus where Id=@1", new string[1]
        {
          Id
        });
                if (dataTableWithParams != null)
                {
                    if (dataTableWithParams.Rows.Count > 0)
                        str = dataTableWithParams.Rows[0]["Name"].ToString();
                }
            }
            catch (Exception ex)
            {
            }
            return str;
        }

        public List<ServiceClass.SparewithPrice> GetJobCardSpareInJobCard(string JobCardId)
        {
            ServiceClass.JobCardSpare jobCardSpare = new ServiceClass.JobCardSpare();
            try
            {
                Decimal num1 = new Decimal(0);
                Decimal num2 = new Decimal(0);
                int Jobcardid = Convert.ToInt32(JobCardId);
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("Select JobCard_Spare_Mapping.Id , SpareId,(Select Name from Spare where Id=SpareId) as Name,Quantity,Amount, Isnull(JobCardDetailId, 0) as JobCardDetailId from JobCard_Spare_Mapping where JobCardId=@1 and IsDeleted=0 order by DOC DESC", new string[1]
        {
          JobCardId
        });
                if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                {
                    foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams.Rows)
                    {
                        ServiceClass.SparewithPrice sparewithPrice = new ServiceClass.SparewithPrice();

                        sparewithPrice.Id = row["SpareId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        Decimal num3 = Convert.ToDecimal(row["Amount"]);
                        Decimal num4 = Convert.ToDecimal(row["Quantity"]);
                        num1 += num3 * num4;
                        sparewithPrice.Price = num3.ToString();
                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        sparewithPrice.JobCardDetailId = row["JobCardDetailId"].ToString();
                        sparewithPrice.ConsumableType = "1";
                        int MappingId = Convert.ToInt32(row["Id"]);
                        int SpareId = Convert.ToInt32(row["SpareId"]);


                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    jobCardSpare.resultflag = "1";
                    jobCardSpare.Message = Constant.Message.SuccessMessage;
                }
                else
                {
                    jobCardSpare.resultflag = "0";
                    jobCardSpare.Message = Constant.Message.NoDataFound;
                }

                DataTable dataTableWithParams1 = this.dbCon.GetDataTableWithParams("Select JobCard_Cosumable_Mapping.Id,ConsumableId,(Select Name from Consumables where Id=ConsumableId) as Name,Quantity,Amount, Isnull(JobCardDetailId, 0) as JobCardDetailId from JobCard_Cosumable_Mapping where JobCardId=@1 and IsDeleted=0 order by DOC DESC", new string[1]
        {
          JobCardId
        });
                if (dataTableWithParams1 != null && dataTableWithParams1.Rows.Count > 0)
                {
                    foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams1.Rows)
                    {
                        ServiceClass.SparewithPrice sparewithPrice = new ServiceClass.SparewithPrice();
                        sparewithPrice.Id = row["ConsumableId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        Decimal num3 = Convert.ToDecimal(row["Amount"]);
                        Decimal num4 = Convert.ToDecimal(row["Quantity"]);
                        num1 += num3 * num4;
                        sparewithPrice.Price = num3.ToString();
                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        sparewithPrice.JobCardDetailId = row["JobCardDetailId"].ToString();
                        sparewithPrice.ConsumableType = "3";
                        int MappingId = Convert.ToInt32(row["Id"]);
                        int SpareId = Convert.ToInt32(row["ConsumableId"]);

                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    jobCardSpare.resultflag = "1";
                    jobCardSpare.Message = Constant.Message.SuccessMessage;
                }
                else
                {
                    jobCardSpare.resultflag = "0";
                    jobCardSpare.Message = Constant.Message.NoDataFound;
                }
                jobCardSpare.FinalAmount = Constant.Message.FinalAmount + num2.ToString();
                return jobCardSpare.Spares;
            }
            catch (Exception ex)
            {
                return new List<ServiceClass.SparewithPrice>();
            }
        }
        public List<ServiceClass.SparesForInvoice> GetJobCardSpareInJobCardForInvoice(string JobCardId)
        {
            ServiceClass.JobCardSpareForInvoice jobCardSpare = new ServiceClass.JobCardSpareForInvoice();
            try
            {
                Decimal num1 = new Decimal(0);
                Decimal num2 = new Decimal(0);
                int Jobcardid = Convert.ToInt32(JobCardId);
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("Select JobCard_Spare_Mapping.Id , SpareId,(Select Name from Spare where Id=SpareId) as Name,Isnull((Select HSNNumber From Spare where Id =SpareId),'') as HSN,Quantity,Amount, Isnull(JobCardDetailId, 0) as JobCardDetailId, Isnull((Select TaxId from Spare where Id=SpareId and IsDeleted=0),0) as TaxId from JobCard_Spare_Mapping where JobCardId=@1 and IsDeleted=0 order by DOC DESC", new string[1]
        {
          JobCardId
        });
                Decimal TotalCGSTAmount = 0;
                Decimal SubTotalForService = 0;
                Decimal SubTotalForSpare = 0;
                if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                {
                    ServiceClass.SparesForInvoice sparewithPrice = new ServiceClass.SparesForInvoice();
                    sparewithPrice.Name = "Spares / Consumables";
                    sparewithPrice.Type = "Header";
                    jobCardSpare.Spares.Add(sparewithPrice);
                    Decimal num3 = 0;

                    foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams.Rows)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();

                        sparewithPrice.Id = row["SpareId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();

                        try
                        {
                            num3 = Convert.ToDecimal(row["Amount"]);
                        }
                        catch (Exception ex)
                        { }
                        Decimal num4 = Convert.ToDecimal(row["Quantity"]);
                        num1 += num3 * num4;
                        sparewithPrice.Price = num1.ToString();
                        sparewithPrice.UnitPrice = (num3).ToString();
                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        int MappingId = Convert.ToInt32(row["Id"]);
                        int SpareId = Convert.ToInt32(row["SpareId"]);
                        int TaxId = Convert.ToInt32(row["TaxId"]);
                        decimal tax = TaxValueFromId(TaxId);
                        decimal taxableamount = 0;
                        decimal discount = 0;
                        decimal TaxRate = Math.Round((tax / 2), 2);
                        decimal TaxAmount = Math.Round(((num3 * num4) * ((tax / 2) / 100)), 2);
                        decimal TaxableAmount = Math.Round(((num3 * num4) - discount - ((num3 * num4) * (tax / 100))), 2);
                        decimal FinalAmont = Math.Round(((num3 * num4) - discount), 2);
                        sparewithPrice.Discount = discount.ToString();
                        sparewithPrice.SGSTRate = TaxRate.ToString();
                        sparewithPrice.CGSTRate = TaxRate.ToString();
                        sparewithPrice.SGSTAmount = TaxAmount.ToString();
                        sparewithPrice.CGSTAmount = TaxAmount.ToString();
                        TotalCGSTAmount += TaxAmount;
                        sparewithPrice.TaxableAmount = TaxableAmount.ToString();
                        sparewithPrice.FinalAmount = FinalAmont.ToString();
                        SubTotalForSpare += Math.Round(((num3 * num4) - discount), 2);
                        //sparewithPrice.TaxableAmount=taxableamount.ToString();
                        sparewithPrice.HSNNumber = row["HSN"].ToString();

                        sparewithPrice.TotalCGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.TotalSGSTAmount = TotalCGSTAmount.ToString();
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    if (jobCardSpare.Spares.Count > 1)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();
                        sparewithPrice.Name = "SubTotalForSpare";
                        sparewithPrice.Type = "Header";
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.SubTotal = SubTotalForSpare.ToString();
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    jobCardSpare.TotalCGSTAmount = TotalCGSTAmount.ToString();
                    jobCardSpare.TotalSGSTAmount = TotalCGSTAmount.ToString();
                    jobCardSpare.resultflag = "1";
                    jobCardSpare.Message = Constant.Message.SuccessMessage;
                }
                else
                {
                    jobCardSpare.resultflag = "0";
                    jobCardSpare.Message = Constant.Message.NoDataFound;
                }

                DataTable dataTableWithParams1 = this.dbCon.GetDataTableWithParams("Select JobCard_Cosumable_Mapping.Id,ConsumableId,(Select Name from Consumables where Id=ConsumableId) as Name,Quantity,Amount, Isnull(JobCardDetailId, 0) as JobCardDetailId, Isnull((Select TaxId from Consumables where Id=ConsumableId),0) as TaxId from JobCard_Cosumable_Mapping where JobCardId=@1 and IsDeleted=0 order by DOC DESC", new string[1]
        {
          JobCardId
        });
                if (dataTableWithParams1 != null && dataTableWithParams1.Rows.Count > 0)
                {
                    // ServiceClass.SparesForInvoice sparewithPrice = new ServiceClass.SparesForInvoice();
                    //sparewithPrice.Name = "Consumables";
                    //jobCardSpare.Spares.Add(sparewithPrice);
                    foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams1.Rows)
                    {
                        ServiceClass.SparesForInvoice sparewithPrice = new ServiceClass.SparesForInvoice();
                        sparewithPrice.Id = row["ConsumableId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        Decimal num3 = Convert.ToDecimal(row["Amount"]);
                        Decimal num4 = Convert.ToDecimal(row["Quantity"]);
                        num1 += num3 * num4;
                        sparewithPrice.UnitPrice = (num3).ToString();
                        sparewithPrice.Price = num3.ToString();
                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        int TaxId = Convert.ToInt32(row["TaxId"]);

                        decimal tax = TaxValueFromId(TaxId);
                        decimal taxableamount = 0;
                        decimal discount = 0;

                        decimal TaxRate = Math.Round((tax / 2), 2);
                        decimal TaxAmount = Math.Round(((num3 * num4) * ((tax / 2) / 100)), 2);
                        decimal TaxableAmount = Math.Round(((num3 * num4) - discount - ((num3 * num4) * (tax / 100))), 2);
                        decimal FinalAmont = Math.Round(((num3 * num4) - discount), 2);

                        sparewithPrice.Discount = discount.ToString();
                        sparewithPrice.SGSTRate = TaxRate.ToString();
                        sparewithPrice.CGSTRate = TaxRate.ToString();
                        sparewithPrice.SGSTAmount = TaxAmount.ToString();
                        sparewithPrice.CGSTAmount = TaxAmount.ToString();
                        sparewithPrice.TaxableAmount = TaxableAmount.ToString();
                        sparewithPrice.FinalAmount = FinalAmont.ToString();
                        SubTotalForSpare += Math.Round(((num3 * num4) - discount), 2);

                        TotalCGSTAmount += TaxAmount;
                        sparewithPrice.SubTotal = SubTotalForSpare.ToString();
                        sparewithPrice.TotalCGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.TotalSGSTAmount = TotalCGSTAmount.ToString();
                        //sparewithPrice.Discount = discount.ToString();
                        //sparewithPrice.SGSTRate = (tax / 2).ToString();
                        //sparewithPrice.CGSTRate = (tax / 2).ToString();
                        //sparewithPrice.SGSTAmount = ((num3 * num4) * ((tax / 2) / 100)).ToString();
                        //sparewithPrice.CGSTAmount = ((num3 * num4) * ((tax / 2) / 100)).ToString();
                        //sparewithPrice.TaxableAmount = (((num3 * num4) - discount) -((num3 * num4) * (tax / 100))).ToString();
                        //sparewithPrice.FinalAmount = ((num3 * num4) - discount).ToString();
                        int MappingId = Convert.ToInt32(row["Id"]);
                        int SpareId = Convert.ToInt32(row["ConsumableId"]);

                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    if (jobCardSpare.Spares.Count > 1)
                    {
                        ServiceClass.SparesForInvoice sparewithPrice = new ServiceClass.SparesForInvoice();
                        sparewithPrice.Name = "SubTotalForSpare";
                        sparewithPrice.Type = "Header";
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.SubTotal = SubTotalForSpare.ToString();
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    jobCardSpare.resultflag = "1";
                    jobCardSpare.Message = Constant.Message.SuccessMessage;
                }
                else
                {
                    jobCardSpare.resultflag = "0";
                    jobCardSpare.Message = Constant.Message.NoDataFound;
                }
                TotalCGSTAmount = 0;

                DataTable datatable = this.dbCon.GetDataTableWithParams("SELECT distinct Categoryid from JobCard_Service_Mapping inner join [Service] on Service.Id=JobCard_Service_Mapping.ServiceId where JobCardId=@1", new string[1] { JobCardId });
                DataTable dataTableWithParams2 = this.dbCon.GetDataTableWithParams("Select JobCard_Service_Mapping.Id , ServiceId,(Select CategoryId from [Service] where Service.Id=ServiceId) as CategoryId,(Select Name from Service where Id=ServiceId) as Name,(Select CategoryId from [Service] where Service.Id=ServiceId) as CategoryId,Isnull((Select HSNNumber From Service where Id =ServiceId),'') as HSN,Quantity,Amount, Isnull(JobCardDetailId, 0) as JobCardDetailId, Isnull((Select TaxId from Service where Id=ServiceId and IsDeleted=0),0) as TaxId from JobCard_Service_Mapping where JobCardId=@1 and IsDeleted=0 order by DOC DESC", new string[1]
                {
                  JobCardId
                });
                //if (datatable != null && datatable.Rows.Count > 0)
                //{


                //foreach (DataRow dr in datatable.Rows)
                //{
                //// string CategoryId = dr["CategoryId"].ToString();
                // dbCon.InsertLogs("CategoryId:::" + CategoryId);
                // if (Convert.ToInt32(CategoryId) > 0)
                // {

                //Decimal num3 = 0;

                //    DataTable dataTableWithParams2 = this.dbCon.GetDataTableWithParams("Select * from (Select JobCard_Service_Mapping.Id , ServiceId,(Select top 1 [Name] from Service where Id=ServiceId) as Name,(Select CategoryId from [Service] where Service.Id=ServiceId) as ServiceCategoryId,Isnull((Select top 1 HSNNumber From [Service] where Id =ServiceId),'') as HSN,Quantity,Amount, Isnull(JobCardDetailId, 0) as JobCardDetailId, Isnull((Select TaxId from Service where Id=ServiceId and IsDeleted=0),0) as TaxId from JobCard_Service_Mapping where JobCardId=@1 and IsDeleted=0  ) as ServiceJobCardDetail where ServiceJobCardDetail.ServiceCategoryId=@2", new string[2]
                //{
                //  JobCardId,CategoryId
                //});
                if (dataTableWithParams2 != null && dataTableWithParams2.Rows.Count > 0)
                {
                    Decimal num3 = 0;
                    ServiceClass.SparesForInvoice sparewithPrice = new ServiceClass.SparesForInvoice();
                    //  string CategoryName = GetCategoryFromCategoryId(CategoryId, "2");
                    sparewithPrice.Type = "Header";
                    sparewithPrice.Name = "Labour";
                    //  sparewithPrice.Name = CategoryName;
                    jobCardSpare.Spares.Add(sparewithPrice);
                    foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams2.Rows)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();

                        sparewithPrice.Id = row["ServiceId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        sparewithPrice.ItemType = "Service";
                        try
                        {
                            num3 = Convert.ToDecimal(row["Amount"]);
                        }
                        catch (Exception ex)
                        { }
                        Decimal num4 = Convert.ToDecimal(row["Quantity"]);
                        num1 += num3 * num4;
                        sparewithPrice.Price = num3.ToString();
                        sparewithPrice.HSNNumber = "998714";
                        sparewithPrice.UnitPrice = (num3).ToString();
                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        int MappingId = Convert.ToInt32(row["Id"]);
                        int SpareId = Convert.ToInt32(row["ServiceId"]);
                        int TaxId = Convert.ToInt32(row["TaxId"]);
                        decimal tax = TaxValueFromId(TaxId);
                        decimal taxableamount = 0;
                        decimal discount = 0;
                        decimal TaxRate = Math.Round((tax / 2), 2);
                        decimal TaxAmount = Math.Round(((num3 * num4) * ((tax / 2) / 100)), 2);
                        decimal TaxableAmount = Math.Round(((num3 * num4) - discount), 2);
                        decimal FinalAmont = Math.Round((((num3 * num4) - discount + ((num3 * num4) * (tax / 100)))), 2);

                        sparewithPrice.Discount = discount.ToString();
                        sparewithPrice.SGSTRate = TaxRate.ToString();
                        sparewithPrice.CGSTRate = TaxRate.ToString();
                        sparewithPrice.SGSTAmount = TaxAmount.ToString();
                        sparewithPrice.CGSTAmount = TaxAmount.ToString();
                        sparewithPrice.TaxableAmount = TaxableAmount.ToString();
                        sparewithPrice.FinalAmount = FinalAmont.ToString();
                        SubTotalForService += FinalAmont;

                        TotalCGSTAmount += TaxAmount;
                        sparewithPrice.TotalCGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.TotalSGSTAmount = TotalCGSTAmount.ToString();
                        //sparewithPrice.Discount = discount.ToString();
                        //sparewithPrice.SGSTRate = TaxRate.ToString();
                        //sparewithPrice.CGSTRate = TaxRate.ToString();
                        //sparewithPrice.SGSTAmount = TaxAmount.ToString();
                        //sparewithPrice.CGSTAmount = TaxAmount.ToString();
                        //sparewithPrice.UnitPrice = FinalAmont.ToString();
                        ////+ ((num3 * num4) * (tax / 100))
                        //sparewithPrice.TaxableAmount = TaxableAmount.ToString();
                        //sparewithPrice.FinalAmount = ((num3 * num4) - discount + ((num3 * num4) * (tax / 100))).ToString();
                        //sparewithPrice.TaxableAmount=taxableamount.ToString();
                        //sparewithPrice.HSNNumber = row["HSN"].ToString();
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    if (jobCardSpare.Spares.Count > 1)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();
                        sparewithPrice.Name = "SubTotalForService";
                        sparewithPrice.Type = "Header";
                        sparewithPrice.TotalCGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.TotalSGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.SubTotal = SubTotalForService.ToString();
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                }
               // dbCon.InsertLogs("JobCard Spare COunt :: " + jobCardSpare.Spares.Count);
                jobCardSpare.resultflag = "1";
                jobCardSpare.Message = Constant.Message.SuccessMessage;

                //        }
                //    }
                //}
                //else
                //{
                //    jobCardSpare.resultflag = "0";
                //    jobCardSpare.Message = Constant.Message.NoDataFound;
                //}

                jobCardSpare.FinalAmount = Constant.Message.FinalAmount + num2.ToString();
                return jobCardSpare.Spares;
            }
            catch (Exception ex)
            {
                dbCon.InsertLogs("GetJobCardSpareInJobCardForInvoice : " + ex.ToString(), ex.StackTrace.ToString(), ex.Message.ToString());
                return new List<ServiceClass.SparesForInvoice>();
            }
        }

        public List<ServiceClass.SparesForInvoice> GetJobCardSpareInJobCardForInvoiceV1(string JobCardId)
        {
            ServiceClass.JobCardSpareForInvoice jobCardSpare = new ServiceClass.JobCardSpareForInvoice();
            try
            {
                Decimal num1 = new Decimal(0);
                Decimal num2 = new Decimal(0);
                int Jobcardid = Convert.ToInt32(JobCardId);
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("SELECT Invoice_Spare_Mapping.Id,ISNULL(Spare.Name, '') AS Name, ISNULL(Spare.HSNNumber, '') AS HSN, Invoice_Spare_Mapping.InvoiceId,Invoice_Spare_Mapping.SpareId, ISNULL(Invoice_Spare_Mapping.CGSTValue, 0) AS CGSTValue, ISNULL(Invoice_Spare_Mapping.CGSTAmount, 0) AS CGSTAmount, ISNULL(Invoice_Spare_Mapping.SGSTValue, 0) AS SGSTValue, ISNULL(Invoice_Spare_Mapping.SGSTAmount, 0) AS SGSTAmount,  Invoice_Spare_Mapping.DiscountPerUnit, Invoice_Spare_Mapping.ActualAmountPerUnit, Invoice_Spare_Mapping.Quantity, Invoice.TotalServiceAmount, Invoice.TotalSpareAmount,isnull(Invoice_Spare_Mapping.TotalDiscount,0) as TotalDiscount FROM  Invoice_Spare_Mapping INNER JOIN Invoice ON Invoice.Id = Invoice_Spare_Mapping.InvoiceId INNER  JOIN Spare ON Spare.Id = Invoice_Spare_Mapping.SpareId WHERE (Invoice.JobCardId = @1) ORDER BY Invoice_Spare_Mapping.DOC DESC ", new string[1]
        {
          JobCardId
        });
                Decimal TotalCGSTAmount = 0;
                Decimal SubTotalForService = 0;
                Decimal SubTotalForSpare = 0;
                decimal d_Total_DiscountSpare_Service = 0;
                decimal TotalTaxableAmountForService = 0;
                decimal TotalTaxableAmountForSpare = 0;

                if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                {
                    ServiceClass.SparesForInvoice sparewithPrice = new ServiceClass.SparesForInvoice();
                    sparewithPrice.Name = "Spares / Consumables";
                    sparewithPrice.Type = "Header";
                    jobCardSpare.Spares.Add(sparewithPrice);
                    Decimal ActualAmountPerUnit = 0;

                    foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams.Rows)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();
                        d_Total_DiscountSpare_Service += decimal.Parse(row["TotalDiscount"].ToString());
                        sparewithPrice.Id = row["SpareId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        sparewithPrice.ItemType = "Spare";
                        sparewithPrice.InvoiceSpareId = row["Id"].ToString();
                        try
                        {
                            ActualAmountPerUnit = Convert.ToDecimal(row["ActualAmountPerUnit"]);
                        }
                        catch (Exception ex)
                        { }
                        Decimal Quantity = Convert.ToDecimal(row["Quantity"]);
                        num1 += ActualAmountPerUnit * Quantity;
                        sparewithPrice.Price = ActualAmountPerUnit.ToString();

                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        //int MappingId = Convert.ToInt32(row["Id"]);
                        //int SpareId = Convert.ToInt32(row["SpareId"]);
                        //int TaxId = Convert.ToInt32(row["TaxId"]);
                        //decimal tax = TaxValueFromId(TaxId);
                        //decimal taxableamount = 0;
                        decimal discount = Math.Round(Convert.ToDecimal(row["DiscountPerUnit"]), 2);

                        sparewithPrice.UnitPrice = (ActualAmountPerUnit + discount).ToString();
                        ActualAmountPerUnit = ActualAmountPerUnit + discount;
                        //decimal TaxRate = Math.Round((tax / 2), 2);
                        decimal TaxAmount = Convert.ToDecimal(row["CGSTAmount"]);


                        TaxAmount = TaxAmount * Quantity;
                        //decimal TaxableAmount = Math.Round(((num3 * num4) - discount - ((num3 * num4) * (tax / 100))), 2);
                        //decimal FinalAmont = Math.Round(((num3 * num4) - discount), 2);
                        sparewithPrice.Discount = discount.ToString();
                        sparewithPrice.SGSTRate = row["SGSTValue"].ToString();
                        sparewithPrice.CGSTRate = row["CGSTValue"].ToString();
                        sparewithPrice.SGSTAmount = DisplayPrice(Convert.ToDecimal(row["SGSTAmount"].ToString()) * Quantity).ToString();
                        sparewithPrice.CGSTAmount = DisplayPrice(Convert.ToDecimal(row["CGSTAmount"].ToString()) * Quantity).ToString();
                        TotalCGSTAmount += TaxAmount;
                        decimal TotalDiscount = (discount);


                        //
                        decimal TaxableAmount = Math.Round((((ActualAmountPerUnit * Quantity) - (2 * TaxAmount))), 2);
                        TotalTaxableAmountForSpare += TaxableAmount;
                        decimal FinalAmont = Math.Round(((ActualAmountPerUnit * Quantity)), 2);
                        if (TaxableAmount < 0)
                        {
                            TaxableAmount = 0;
                        }

                        if (FinalAmont < 0)
                        {
                            FinalAmont = 0;
                        }
                        sparewithPrice.TaxableAmount = TaxableAmount.ToString();
                        sparewithPrice.FinalAmount = FinalAmont.ToString();
                        decimal amount = Math.Round(((ActualAmountPerUnit * Quantity)), 2);
                        if (amount < 0)
                        {
                            amount = 0;
                        }
                        SubTotalForSpare += amount;
                        //sparewithPrice.TaxableAmount=taxableamount.ToString();
                        sparewithPrice.HSNNumber = row["HSN"].ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TotalCGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.TotalSGSTAmount = TotalCGSTAmount.ToString();
                        //   sparewithPrice.SubTotal = row[""].ToString();
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    if (jobCardSpare.Spares.Count > 1)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();
                        sparewithPrice.Name = "SubTotalForSpare";
                        sparewithPrice.Type = "Header";
                        sparewithPrice.TotalCGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.TotalSGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.SubTotal = SubTotalForSpare.ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TaxableAmount = (TotalTaxableAmountForSpare).ToString();
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    jobCardSpare.TotalCGSTAmount = TotalCGSTAmount.ToString();
                    jobCardSpare.TotalSGSTAmount = TotalCGSTAmount.ToString();
                    jobCardSpare.resultflag = "1";
                    jobCardSpare.Message = Constant.Message.SuccessMessage;
                }
                else
                {
                    jobCardSpare.resultflag = "0";
                    jobCardSpare.Message = Constant.Message.NoDataFound;
                }

                TotalCGSTAmount = 0;

                DataTable datatable = this.dbCon.GetDataTableWithParams("SELECT distinct Categoryid from JobCard_Service_Mapping inner join [Service] on Service.Id=JobCard_Service_Mapping.ServiceId where JobCardId=@1", new string[1] { JobCardId });

                DataTable dataTableWithParams2 = this.dbCon.GetDataTableWithParams("SELECT Invoice_Service_Mapping.Id,ISNULL(Service.Name, '') AS Name, ISNULL(Service.HSNNumber, '') AS HSN, Invoice_Service_Mapping.InvoiceId,Invoice_Service_Mapping.ServiceId, ISNULL(Invoice_Service_Mapping.CGSTValue, 0) AS CGSTValue, ISNULL(Invoice_Service_Mapping.CGSTAmount, 0) AS CGSTAmount, ISNULL(Invoice_Service_Mapping.SGSTValue, 0) AS SGSTValue, ISNULL(Invoice_Service_Mapping.SGSTAmount, 0) AS SGSTAmount,  Invoice_Service_Mapping.DiscountPerUnit, Invoice_Service_Mapping.ActualAmountPerUnit, Invoice_Service_Mapping.Quantity, Invoice.TotalServiceAmount, Invoice.TotalSpareAmount,isnull(Invoice_Service_Mapping.TotalDiscount,0) as TotalDiscount FROM  Invoice_Service_Mapping INNER JOIN Invoice ON Invoice.Id = Invoice_Service_Mapping.InvoiceId INNER  JOIN Service ON Service.Id = Invoice_Service_Mapping.ServiceId WHERE (Invoice.JobCardId = @1) ORDER BY Invoice_Service_Mapping.DOC DESC", new string[1]
                {
                  JobCardId
                });

                if (dataTableWithParams2 != null && dataTableWithParams2.Rows.Count > 0)
                {
                    Decimal ActualAmountPerUnit = 0;
                    ServiceClass.SparesForInvoice sparewithPrice = new ServiceClass.SparesForInvoice();
                    sparewithPrice.Type = "Header";
                    sparewithPrice.Name = "Labour";
                    jobCardSpare.Spares.Add(sparewithPrice);
                    foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams2.Rows)
                    {
                        d_Total_DiscountSpare_Service += decimal.Parse(row["TotalDiscount"].ToString());
                        sparewithPrice = new ServiceClass.SparesForInvoice();

                        sparewithPrice.Id = row["ServiceId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        sparewithPrice.ItemType = "Service";
                        sparewithPrice.InvoiceServiceId = row["Id"].ToString();
                        try
                        {
                            ActualAmountPerUnit = Convert.ToDecimal(row["ActualAmountPerUnit"]);
                        }
                        catch (Exception ex)
                        { }
                        Decimal Quantity = Convert.ToDecimal(row["Quantity"]);
                        num1 += ActualAmountPerUnit * Quantity;
                        sparewithPrice.Price = ActualAmountPerUnit.ToString();
                        sparewithPrice.HSNNumber = "998714";
                        // sparewithPrice.UnitPrice = (num3).ToString();
                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        decimal discount = Math.Round(Convert.ToDecimal(row["DiscountPerUnit"]), 2);
                        sparewithPrice.Discount = (discount * Quantity).ToString();
                        //  ActualAmountPerUnit = ActualAmountPerUnit ;

                        sparewithPrice.UnitPrice = (ActualAmountPerUnit + discount).ToString();
                        ActualAmountPerUnit = ActualAmountPerUnit + discount;
                        decimal TotalDiscount = (discount * Quantity);
                        decimal TaxAmount = Convert.ToDecimal(row["CGSTAmount"]);
                        decimal TaxableAmount = Math.Round(((ActualAmountPerUnit * Quantity) - TotalDiscount), 2);
                        TotalTaxableAmountForService += TaxableAmount;
                        decimal FinalAmont = Math.Round((((ActualAmountPerUnit * Quantity) + (2 * TaxAmount))), 2);
                        if (TaxableAmount < 0)
                        {
                            TaxableAmount = 0;
                        }

                        if (FinalAmont < 0)
                        {
                            FinalAmont = 0;
                        }
                        //sparewithPrice.Discount = row["Discount"];
                        sparewithPrice.SGSTRate = row["SGSTValue"].ToString();
                        sparewithPrice.CGSTRate = row["CGSTValue"].ToString();
                        sparewithPrice.SGSTAmount = DisplayPrice(Convert.ToDecimal(row["SGSTAmount"].ToString())).ToString();
                        sparewithPrice.CGSTAmount = DisplayPrice(Convert.ToDecimal(row["CGSTAmount"].ToString())).ToString();
                        //sparewithPrice.SGSTAmount = row["SGSTAmount"].ToString();
                        //sparewithPrice.CGSTAmount = row["CGSTAmount"].ToString();

                        TotalCGSTAmount += TaxAmount;
                        sparewithPrice.TaxableAmount = TaxableAmount.ToString();
                        sparewithPrice.FinalAmount = FinalAmont.ToString();
                        SubTotalForService += FinalAmont;
                        sparewithPrice.TotalCGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.TotalSGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    if (jobCardSpare.Spares.Count > 1)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();
                        sparewithPrice.Name = "SubTotalForService";
                        sparewithPrice.Type = "Header";
                        sparewithPrice.TotalCGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.TotalSGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.SubTotal = SubTotalForService.ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TaxableAmount = (TotalTaxableAmountForService).ToString();
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                }
                //dbCon.InsertLogs("JobCard Spare COunt :: " + jobCardSpare.Spares.Count);
                jobCardSpare.resultflag = "1";
                jobCardSpare.Message = Constant.Message.SuccessMessage;

                //        }
                //    }
                //}
                //else
                //{
                //    jobCardSpare.resultflag = "0";
                //    jobCardSpare.Message = Constant.Message.NoDataFound;
                //}

                jobCardSpare.FinalAmount = Constant.Message.FinalAmount + num2.ToString();
                return jobCardSpare.Spares;
            }
            catch (Exception ex)
            {
                dbCon.InsertLogs("GetJobCardSpareInJobCardForInvoice : " + ex.ToString(), ex.StackTrace.ToString(), ex.Message.ToString());
                return new List<ServiceClass.SparesForInvoice>();
            }
        }

        public ServiceClass.JobCardSpareForInvoice GetJobCardSpareInJobCardForInvoiceV2(string JobCardId)
        {
            ServiceClass.JobCardSpareForInvoice jobCardSpare = new ServiceClass.JobCardSpareForInvoice();
            try
            {
                Decimal num1 = new Decimal(0);
                Decimal num2 = new Decimal(0);
                int Jobcardid = Convert.ToInt32(JobCardId);
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("SELECT Invoice_Spare_Mapping.Id,ISNULL(Spare.Name, '') AS Name, ISNULL(Spare.HSNNumber, '') AS HSN, Invoice_Spare_Mapping.InvoiceId,Invoice_Spare_Mapping.SpareId, ISNULL(Invoice_Spare_Mapping.CGSTValue, 0) AS CGSTValue, ISNULL(Invoice_Spare_Mapping.CGSTAmount, 0) AS CGSTAmount, ISNULL(Invoice_Spare_Mapping.SGSTValue, 0) AS SGSTValue, ISNULL(Invoice_Spare_Mapping.SGSTAmount, 0) AS SGSTAmount,  Invoice_Spare_Mapping.DiscountPerUnit, Invoice_Spare_Mapping.ActualAmountPerUnit, Invoice_Spare_Mapping.Quantity, Invoice.TotalServiceAmount, Invoice.TotalSpareAmount,isnull(Invoice_Spare_Mapping.TotalDiscount,0) as TotalDiscount FROM  Invoice_Spare_Mapping INNER JOIN Invoice ON Invoice.Id = Invoice_Spare_Mapping.InvoiceId INNER  JOIN Spare ON Spare.Id = Invoice_Spare_Mapping.SpareId WHERE (Invoice.JobCardId = @1) ORDER BY Invoice_Spare_Mapping.DOC DESC ", new string[1]
        {
          JobCardId
        });
                Decimal TotalCGSTAmount = 0;
                Decimal SubTotalForService = 0;
                Decimal SubTotalForSpare = 0;
                decimal d_Total_DiscountSpare_Service = 0;
                decimal TotalTaxableAmountForService = 0;
                decimal TotalTaxableAmountForSpare = 0;
                decimal TotalDiscountSpare = 0;
                decimal TotalDiscountService = 0;

                if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                {
                    ServiceClass.SparesForInvoice sparewithPrice = new ServiceClass.SparesForInvoice();
                    sparewithPrice.Name = "Spares / Consumables";
                    sparewithPrice.Type = "Header";
                    jobCardSpare.Spares.Add(sparewithPrice);
                    Decimal ActualAmountPerUnit = 0;

                    foreach (DataRow row in dataTableWithParams.Rows)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();
                        d_Total_DiscountSpare_Service += decimal.Parse(row["TotalDiscount"].ToString());
                        sparewithPrice.Id = row["SpareId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        sparewithPrice.ItemType = "Spare";
                        sparewithPrice.InvoiceSpareId = row["Id"].ToString();
                        try
                        {
                            ActualAmountPerUnit = Convert.ToDecimal(row["ActualAmountPerUnit"]);
                        }
                        catch (Exception ex)
                        { }
                        Decimal Quantity = Convert.ToDecimal(row["Quantity"]);
                        num1 += ActualAmountPerUnit * Quantity;
                        // sparewithPrice.Price = ActualAmountPerUnit+Discount.ToString();

                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        decimal discount = Math.Round(Convert.ToDecimal(row["DiscountPerUnit"]), 2);
                        sparewithPrice.Price = (ActualAmountPerUnit).ToString();
                        TotalDiscountSpare += discount;
                        sparewithPrice.UnitPrice = (ActualAmountPerUnit).ToString();
                        decimal TaxAmount = Convert.ToDecimal(row["CGSTAmount"]);

                        sparewithPrice.Discount = discount.ToString();
                        sparewithPrice.SGSTRate = row["SGSTValue"].ToString();
                        sparewithPrice.CGSTRate = row["CGSTValue"].ToString();
                        sparewithPrice.SGSTAmount = DisplayPrice(Convert.ToDecimal(row["SGSTAmount"].ToString())).ToString();
                        sparewithPrice.CGSTAmount = DisplayPrice(Convert.ToDecimal(row["CGSTAmount"].ToString())).ToString();
                        TotalCGSTAmount += TaxAmount;
                        decimal TotalDiscount = (discount * Quantity);


                        //
                        decimal TaxableAmount = Math.Round((((ActualAmountPerUnit * Quantity) - (2 * TaxAmount) - TotalDiscount)), 2);
                        TotalTaxableAmountForSpare += TaxableAmount;
                        decimal FinalAmont = Math.Round(((ActualAmountPerUnit * Quantity)), 2);
                        FinalAmont = FinalAmont - discount;
                        if (TaxableAmount < 0)
                        {
                            TaxableAmount = 0;
                        }

                        if (FinalAmont < 0)
                        {
                            FinalAmont = 0;
                        }
                        sparewithPrice.TaxableAmount = TaxableAmount.ToString();
                        sparewithPrice.FinalAmount = FinalAmont.ToString();
                        decimal amount = Math.Round(((ActualAmountPerUnit * Quantity)), 2);
                        amount = amount - discount;
                        if (amount < 0)
                        {
                            amount = 0;
                        }
                        SubTotalForSpare += amount;
                        //sparewithPrice.TaxableAmount=taxableamount.ToString();
                        sparewithPrice.HSNNumber = row["HSN"].ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TotalCGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.TotalSGSTAmount = TotalCGSTAmount.ToString();
                        //   sparewithPrice.SubTotal = row[""].ToString();
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    if (jobCardSpare.Spares.Count > 1)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();
                        sparewithPrice.Name = "SubTotalForSpare";
                        sparewithPrice.Type = "Header";
                        sparewithPrice.TotalCGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.TotalSGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.SubTotal = SubTotalForSpare.ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TaxableAmount = DisplayPrice(TotalTaxableAmountForSpare);

                        sparewithPrice.TotalDiscountForSpare = DisplayPrice(TotalDiscountSpare).ToString();

                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    jobCardSpare.TotalCGSTAmount = TotalCGSTAmount.ToString();
                    jobCardSpare.TotalSGSTAmount = TotalCGSTAmount.ToString();
                    jobCardSpare.resultflag = "1";
                    jobCardSpare.Message = Constant.Message.SuccessMessage;
                }
                else
                {
                    jobCardSpare.resultflag = "0";
                    jobCardSpare.Message = Constant.Message.NoDataFound;
                }

                TotalCGSTAmount = 0;

                DataTable datatable = this.dbCon.GetDataTableWithParams("SELECT distinct Categoryid from JobCard_Service_Mapping inner join [Service] on Service.Id=JobCard_Service_Mapping.ServiceId where JobCardId=@1", new string[1] { JobCardId });

                DataTable dataTableWithParams2 = this.dbCon.GetDataTableWithParams("SELECT Invoice_Service_Mapping.Id,ISNULL(Service.Name, '') AS Name, ISNULL(Service.HSNNumber, '') AS HSN, Invoice_Service_Mapping.InvoiceId,Invoice_Service_Mapping.ServiceId, ISNULL(Invoice_Service_Mapping.CGSTValue, 0) AS CGSTValue, ISNULL(Invoice_Service_Mapping.CGSTAmount, 0) AS CGSTAmount, ISNULL(Invoice_Service_Mapping.SGSTValue, 0) AS SGSTValue, ISNULL(Invoice_Service_Mapping.SGSTAmount, 0) AS SGSTAmount,  Invoice_Service_Mapping.DiscountPerUnit, Invoice_Service_Mapping.ActualAmountPerUnit, Invoice_Service_Mapping.Quantity, Invoice.TotalServiceAmount, Invoice.TotalSpareAmount,isnull(Invoice_Service_Mapping.TotalDiscount,0) as TotalDiscount FROM  Invoice_Service_Mapping INNER JOIN Invoice ON Invoice.Id = Invoice_Service_Mapping.InvoiceId INNER  JOIN Service ON Service.Id = Invoice_Service_Mapping.ServiceId WHERE (Invoice.JobCardId = @1) ORDER BY Invoice_Service_Mapping.DOC DESC", new string[1]
                {
                  JobCardId
                });

                if (dataTableWithParams2 != null && dataTableWithParams2.Rows.Count > 0)
                {
                    Decimal ActualAmountPerUnit = 0;
                    ServiceClass.ServicesForInvoice sparewithPrice = new ServiceClass.ServicesForInvoice();
                    sparewithPrice.Type = "Header";
                    sparewithPrice.Name = "Labour";
                    jobCardSpare.Services.Add(sparewithPrice);
                    foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams2.Rows)
                    {
                        d_Total_DiscountSpare_Service += decimal.Parse(row["TotalDiscount"].ToString());
                        sparewithPrice = new ServiceClass.ServicesForInvoice();

                        sparewithPrice.Id = row["ServiceId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        sparewithPrice.ItemType = "Service";
                        sparewithPrice.InvoiceServiceId = row["Id"].ToString();
                        try
                        {
                            ActualAmountPerUnit = Convert.ToDecimal(row["ActualAmountPerUnit"]);
                        }
                        catch (Exception ex)
                        { }
                        Decimal Quantity = Convert.ToDecimal(row["Quantity"]);
                        num1 += ActualAmountPerUnit * Quantity;
                        sparewithPrice.Price = ActualAmountPerUnit.ToString();
                        sparewithPrice.HSNNumber = "998714";
                        // sparewithPrice.UnitPrice = (num3).ToString();
                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        decimal discount = Math.Round(Convert.ToDecimal(row["DiscountPerUnit"]), 2);
                        sparewithPrice.Discount = (discount * Quantity).ToString();
                        //  ActualAmountPerUnit = ActualAmountPerUnit ;
                        TotalDiscountService += (discount * Quantity);
                        sparewithPrice.UnitPrice = (ActualAmountPerUnit).ToString();
                        //  ActualAmountPerUnit = ActualAmountPerUnit + discount;
                        decimal TotalDiscount = (discount);
                        decimal TaxAmount = Convert.ToDecimal(row["CGSTAmount"]);
                        decimal TaxableAmount = Math.Round(((ActualAmountPerUnit * Quantity) - TotalDiscount), 2);
                        TotalTaxableAmountForService += TaxableAmount;
                        decimal FinalAmont = Math.Round((((ActualAmountPerUnit * Quantity) + (2 * TaxAmount))), 2);
                        if (TaxableAmount < 0)
                        {
                            TaxableAmount = 0;
                        }

                        if (FinalAmont < 0)
                        {
                            FinalAmont = 0;
                        }
                        //sparewithPrice.Discount = row["Discount"];
                        sparewithPrice.SGSTRate = row["SGSTValue"].ToString();
                        sparewithPrice.CGSTRate = row["CGSTValue"].ToString();
                        sparewithPrice.SGSTAmount = DisplayPrice(Convert.ToDecimal(row["SGSTAmount"].ToString())).ToString();
                        sparewithPrice.CGSTAmount = DisplayPrice(Convert.ToDecimal(row["CGSTAmount"].ToString())).ToString();
                        //sparewithPrice.SGSTAmount = row["SGSTAmount"].ToString();
                        //sparewithPrice.CGSTAmount = row["CGSTAmount"].ToString();

                        TotalCGSTAmount += TaxAmount;
                        sparewithPrice.TaxableAmount = TaxableAmount.ToString();
                        sparewithPrice.FinalAmount = FinalAmont.ToString();
                        SubTotalForService += FinalAmont;
                        sparewithPrice.TotalCGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.TotalSGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        jobCardSpare.Services.Add(sparewithPrice);
                    }
                    if (jobCardSpare.Spares.Count > 1)
                    {
                        sparewithPrice = new ServiceClass.ServicesForInvoice();
                        sparewithPrice.Name = "SubTotalForService";
                        sparewithPrice.Type = "Header";
                        sparewithPrice.TotalCGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.TotalSGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.SubTotal = SubTotalForService.ToString();
                        // sparewithPrice.TotalDiscountForSpare = TotalDiscountSpare.ToString();
                        sparewithPrice.TotalDiscountForService = TotalDiscountService.ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TaxableAmount = (TotalTaxableAmountForService).ToString();
                        jobCardSpare.Services.Add(sparewithPrice);
                    }
                }
                //dbCon.InsertLogs("JobCard Spare COunt :: " + jobCardSpare.Spares.Count);
                jobCardSpare.resultflag = "1";
                jobCardSpare.Message = Constant.Message.SuccessMessage;

                //        }
                //    }
                //}
                //else
                //{
                //    jobCardSpare.resultflag = "0";
                //    jobCardSpare.Message = Constant.Message.NoDataFound;
                //}


                jobCardSpare.TotalBeforeTaxService = TotalTaxableAmountForService.ToString();
                jobCardSpare.TotalBeforeTaxSpare = TotalTaxableAmountForSpare.ToString();
                jobCardSpare.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                jobCardSpare.SubTotalForService = DisplayPrice(SubTotalForService);
                jobCardSpare.SubTotalForSpare = DisplayPrice(SubTotalForSpare);
                jobCardSpare.TotalAmount = DisplayPrice(SubTotalForService + SubTotalForSpare);
                jobCardSpare.FinalAmount = ((SubTotalForService + SubTotalForSpare) - d_Total_DiscountSpare_Service).ToString();
                return jobCardSpare;
            }
            catch (Exception ex)
            {
                dbCon.InsertLogs("GetJobCardSpareInJobCardForInvoice : " + ex.ToString(), ex.StackTrace.ToString(), ex.Message.ToString());
                return new ServiceClass.JobCardSpareForInvoice();
            }
        }

        public List<ServiceClass.SparesForInvoice> GetJobCardSpareInJobCardForEstimate(string JobCardId)
        {
            ServiceClass.JobCardSpareForInvoice jobCardSpare = new ServiceClass.JobCardSpareForInvoice();
            try
            {
                Decimal num1 = new Decimal(0);
                Decimal num2 = new Decimal(0);
                int Jobcardid = Convert.ToInt32(JobCardId);
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("SELECT Invoice_Spare_Mapping.Id,ISNULL(Spare.Name, '') AS Name, ISNULL(Spare.HSNNumber, '') AS HSN, Invoice_Spare_Mapping.InvoiceId,Invoice_Spare_Mapping.SpareId, ISNULL(Invoice_Spare_Mapping.CGSTValue, 0) AS CGSTValue, ISNULL(Invoice_Spare_Mapping.CGSTAmount, 0) AS CGSTAmount, ISNULL(Invoice_Spare_Mapping.SGSTValue, 0) AS SGSTValue, ISNULL(Invoice_Spare_Mapping.SGSTAmount, 0) AS SGSTAmount,  Invoice_Spare_Mapping.DiscountPerUnit, Invoice_Spare_Mapping.ActualAmountPerUnit, Invoice_Spare_Mapping.Quantity, Invoice.TotalServiceAmount, Invoice.TotalSpareAmount,isnull(Invoice_Spare_Mapping.TotalDiscount,0) as TotalDiscount FROM  Invoice_Spare_Mapping INNER JOIN Invoice ON Invoice.Id = Invoice_Spare_Mapping.InvoiceId INNER  JOIN Spare ON Spare.Id = Invoice_Spare_Mapping.SpareId WHERE (Invoice.JobCardId = @1) ORDER BY Invoice_Spare_Mapping.DOC DESC ", new string[1]
        {
          JobCardId
        });
                Decimal TotalCGSTAmount = 0;
                Decimal SubTotalForService = 0;
                Decimal SubTotalForSpare = 0;
                decimal d_Total_DiscountSpare_Service = 0;
                if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                {
                    ServiceClass.SparesForInvoice sparewithPrice = new ServiceClass.SparesForInvoice();
                    sparewithPrice.Name = "Spares / Consumables";
                    sparewithPrice.Type = "Header";
                    jobCardSpare.Spares.Add(sparewithPrice);
                    Decimal num3 = 0;

                    foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams.Rows)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();
                        d_Total_DiscountSpare_Service += decimal.Parse(row["TotalDiscount"].ToString());
                        sparewithPrice.Id = row["SpareId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        sparewithPrice.ItemType = "Spare";
                        sparewithPrice.InvoiceSpareId = row["Id"].ToString();
                        try
                        {
                            num3 = Convert.ToDecimal(row["ActualAmountPerUnit"]);
                        }
                        catch (Exception ex)
                        { }
                        Decimal num4 = Convert.ToDecimal(row["Quantity"]);
                        num1 += num3 * num4;
                        sparewithPrice.Price = num3.ToString();

                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        //int MappingId = Convert.ToInt32(row["Id"]);
                        //int SpareId = Convert.ToInt32(row["SpareId"]);
                        //int TaxId = Convert.ToInt32(row["TaxId"]);
                        //decimal tax = TaxValueFromId(TaxId);
                        //decimal taxableamount = 0;
                        decimal discount = Math.Round(Convert.ToDecimal(row["DiscountPerUnit"]), 2);
                        sparewithPrice.UnitPrice = (num3).ToString();
                        num3 = num3 + discount;
                        //decimal TaxRate = Math.Round((tax / 2), 2);
                        decimal TaxAmount = Convert.ToDecimal(row["CGSTAmount"]);


                        TaxAmount = TaxAmount * num4;
                        //decimal TaxableAmount = Math.Round(((num3 * num4) - discount - ((num3 * num4) * (tax / 100))), 2);
                        //decimal FinalAmont = Math.Round(((num3 * num4) - discount), 2);
                        sparewithPrice.Discount = discount.ToString();
                        sparewithPrice.SGSTRate = row["SGSTValue"].ToString();
                        sparewithPrice.CGSTRate = row["CGSTValue"].ToString();
                        sparewithPrice.SGSTAmount = DisplayPrice(Convert.ToDecimal(row["SGSTAmount"].ToString()) * num4).ToString();
                        sparewithPrice.CGSTAmount = DisplayPrice(Convert.ToDecimal(row["CGSTAmount"].ToString()) * num4).ToString();
                        TotalCGSTAmount += TaxAmount;
                        decimal TotalDiscount = (discount * num4);


                        //- (TaxAmount * 2)
                        decimal TaxableAmount = Math.Round((((num3 * num4) - TotalDiscount)), 2);
                        decimal FinalAmont = Math.Round(((num3 * num4) - TotalDiscount), 2);
                        if (TaxableAmount < 0)
                        {
                            TaxableAmount = 0;
                        }

                        if (FinalAmont < 0)
                        {
                            FinalAmont = 0;
                        }
                        sparewithPrice.TaxableAmount = TaxableAmount.ToString();
                        sparewithPrice.FinalAmount = FinalAmont.ToString();
                        decimal amount = Math.Round(((num3 * num4) - TotalDiscount), 2);
                        if (amount < 0)
                        {
                            amount = 0;
                        }
                        SubTotalForSpare += amount;
                        //sparewithPrice.TaxableAmount=taxableamount.ToString();
                        sparewithPrice.HSNNumber = row["HSN"].ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TotalCGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.TotalSGSTAmount = TotalCGSTAmount.ToString();
                        //   sparewithPrice.SubTotal = row[""].ToString();
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    if (jobCardSpare.Spares.Count > 1)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();
                        sparewithPrice.Name = "SubTotalForSpare";
                        sparewithPrice.Type = "Header";
                        sparewithPrice.TotalCGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.TotalSGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.SubTotal = SubTotalForSpare.ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    jobCardSpare.TotalCGSTAmount = TotalCGSTAmount.ToString();
                    jobCardSpare.TotalSGSTAmount = TotalCGSTAmount.ToString();
                    jobCardSpare.resultflag = "1";
                    jobCardSpare.Message = Constant.Message.SuccessMessage;
                }
                else
                {
                    jobCardSpare.resultflag = "0";
                    jobCardSpare.Message = Constant.Message.NoDataFound;
                }

                TotalCGSTAmount = 0;

                DataTable datatable = this.dbCon.GetDataTableWithParams("SELECT distinct Categoryid from JobCard_Service_Mapping inner join [Service] on Service.Id=JobCard_Service_Mapping.ServiceId where JobCardId=@1", new string[1] { JobCardId });

                DataTable dataTableWithParams2 = this.dbCon.GetDataTableWithParams("SELECT Invoice_Service_Mapping.Id,ISNULL(Service.Name, '') AS Name, ISNULL(Service.HSNNumber, '') AS HSN, Invoice_Service_Mapping.InvoiceId,Invoice_Service_Mapping.ServiceId, ISNULL(Invoice_Service_Mapping.CGSTValue, 0) AS CGSTValue, ISNULL(Invoice_Service_Mapping.CGSTAmount, 0) AS CGSTAmount, ISNULL(Invoice_Service_Mapping.SGSTValue, 0) AS SGSTValue, ISNULL(Invoice_Service_Mapping.SGSTAmount, 0) AS SGSTAmount,  Invoice_Service_Mapping.DiscountPerUnit, Invoice_Service_Mapping.ActualAmountPerUnit, Invoice_Service_Mapping.Quantity, Invoice.TotalServiceAmount, Invoice.TotalSpareAmount,isnull(Invoice_Service_Mapping.TotalDiscount,0) as TotalDiscount FROM  Invoice_Service_Mapping INNER JOIN Invoice ON Invoice.Id = Invoice_Service_Mapping.InvoiceId INNER  JOIN Service ON Service.Id = Invoice_Service_Mapping.ServiceId WHERE (Invoice.JobCardId = @1) ORDER BY Invoice_Service_Mapping.DOC DESC", new string[1]
                {
                  JobCardId
                });

                if (dataTableWithParams2 != null && dataTableWithParams2.Rows.Count > 0)
                {
                    Decimal num3 = 0;
                    ServiceClass.SparesForInvoice sparewithPrice = new ServiceClass.SparesForInvoice();
                    sparewithPrice.Type = "Header";
                    sparewithPrice.Name = "Labour";
                    jobCardSpare.Spares.Add(sparewithPrice);
                    foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams2.Rows)
                    {
                        d_Total_DiscountSpare_Service += decimal.Parse(row["TotalDiscount"].ToString());
                        sparewithPrice = new ServiceClass.SparesForInvoice();

                        sparewithPrice.Id = row["ServiceId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        sparewithPrice.ItemType = "Service";
                        sparewithPrice.InvoiceServiceId = row["Id"].ToString();
                        try
                        {
                            num3 = Convert.ToDecimal(row["ActualAmountPerUnit"]);
                        }
                        catch (Exception ex)
                        { }
                        Decimal num4 = Convert.ToDecimal(row["Quantity"]);
                        num1 += num3 * num4;
                        sparewithPrice.Price = num3.ToString();
                        sparewithPrice.HSNNumber = "998714";
                        // sparewithPrice.UnitPrice = (num3).ToString();
                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        decimal discount = Math.Round(Convert.ToDecimal(row["DiscountPerUnit"]), 2);
                        sparewithPrice.Discount = discount.ToString();
                        sparewithPrice.UnitPrice = (num3).ToString();
                        num3 = num3 + discount;
                        decimal TotalDiscount = (discount * num4);
                        decimal TaxAmount = Convert.ToDecimal(row["CGSTAmount"]);
                        decimal TaxableAmount = Math.Round(((num3 * num4) - TotalDiscount), 2);
                        decimal FinalAmont = Math.Round((((num3 * num4) - TotalDiscount) + (TaxAmount * 2)), 2);
                        if (TaxableAmount < 0)
                        {
                            TaxableAmount = 0;
                        }

                        if (FinalAmont < 0)
                        {
                            FinalAmont = 0;
                        }
                        //sparewithPrice.Discount = row["Discount"];
                        sparewithPrice.SGSTRate = row["SGSTValue"].ToString();
                        sparewithPrice.CGSTRate = row["CGSTValue"].ToString();
                        sparewithPrice.SGSTAmount = DisplayPrice(Convert.ToDecimal(row["SGSTAmount"].ToString()) * num4).ToString();
                        sparewithPrice.CGSTAmount = DisplayPrice(Convert.ToDecimal(row["CGSTAmount"].ToString()) * num4).ToString();
                        //sparewithPrice.SGSTAmount = row["SGSTAmount"].ToString();
                        //sparewithPrice.CGSTAmount = row["CGSTAmount"].ToString();
                        TotalCGSTAmount += TaxAmount;
                        sparewithPrice.TaxableAmount = TaxableAmount.ToString();
                        sparewithPrice.FinalAmount = FinalAmont.ToString();
                        SubTotalForService += FinalAmont;


                        sparewithPrice.TotalCGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.TotalSGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    if (jobCardSpare.Spares.Count > 1)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();
                        sparewithPrice.Name = "SubTotalForService";
                        sparewithPrice.Type = "Header";
                        sparewithPrice.TotalCGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.TotalSGSTAmount = TotalCGSTAmount.ToString();
                        sparewithPrice.SubTotal = SubTotalForService.ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                }
               // dbCon.InsertLogs("JobCard Spare COunt :: " + jobCardSpare.Spares.Count);
                jobCardSpare.resultflag = "1";
                jobCardSpare.Message = Constant.Message.SuccessMessage;

                //        }
                //    }
                //}
                //else
                //{
                //    jobCardSpare.resultflag = "0";
                //    jobCardSpare.Message = Constant.Message.NoDataFound;
                //}

                jobCardSpare.FinalAmount = Constant.Message.FinalAmount + num2.ToString();
                return jobCardSpare.Spares;
            }
            catch (Exception ex)
            {
                dbCon.InsertLogs("GetJobCardSpareInJobCardForInvoice : " + ex.ToString(), ex.StackTrace.ToString(), ex.Message.ToString());
                return new List<ServiceClass.SparesForInvoice>();
            }
        }


        public decimal RoundPrice(decimal price, int points)
        {

            return Math.Round(price, points);
        }


        public string DisplayPrice(decimal Price)
        {
            return Price.ToString("0.00");
        }

        public string GetCategoryFromCategoryId(string CategoryId, string MasterCategoryId)
        {
            string result = "";
            string query = "Select Isnull(Name,'') as Name FROM Category where Id=@1 ";
            string[] param = { CategoryId, MasterCategoryId };
            DataTable dt = dbCon.GetDataTableWithParams(query, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                result = dr["Name"].ToString();
            }
            return result;
        }


        public decimal TaxValueFromId(int TaxId)
        {
            decimal result = 0;
            try
            {
                string query = "SELECT TOP (1000) [TaxValue]  FROM [Motorz].[dbo].[TaxCategory] where Id=@1";
                string[] param = { TaxId.ToString() };
                DataTable dt = dbCon.GetDataTableWithParams(query, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    result = Convert.ToDecimal(dr["TaxValue"]);
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public int TaxIdFromValue(decimal value)
        {
            int result = 0;
            try
            {
                string query = "SELECT TOP (1) [Id]  FROM [Motorz].[dbo].[TaxCategory] where TaxValue=@1";
                string[] param = { value.ToString() };
                DataTable dt = dbCon.GetDataTableWithParams(query, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    result = Convert.ToInt32(dr["Id"]);
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }



        public List<ServiceClass.SparewithPrice> GetJobCardSpareInJobCardWithDetails(string JobCardId, string JobCardDetailId)
        {
            ServiceClass.JobCardSpare jobCardSpare = new ServiceClass.JobCardSpare();
            try
            {
                int jobcardid = Convert.ToInt32(JobCardId);
                Decimal num1 = new Decimal(0);
                Decimal num2 = new Decimal(0);
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("Select JobCard_Spare_Mapping.Id,SpareId,(Select Name from Spare where Id=SpareId) as Name,Quantity,Amount, Isnull(JobCardDetailId, 0) as JobCardDetailId from JobCard_Spare_Mapping where JobCardId=@1 and JobCardDetailId=@2 and IsDeleted=0 order by DOC DESC", new string[2]
        {
          JobCardId,JobCardDetailId
        });
                if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                {
                    foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams.Rows)
                    {
                        ServiceClass.SparewithPrice sparewithPrice = new ServiceClass.SparewithPrice();
                        int MappingId = Convert.ToInt32(row["Id"]);
                        int SpareId = Convert.ToInt32(row["SpareId"]);
                        sparewithPrice.Id = row["SpareId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        Decimal num3 = Convert.ToDecimal(row["Amount"]);
                        Decimal num4 = Convert.ToDecimal(row["Quantity"]);
                        num1 += num3 * num4;
                        sparewithPrice.Price = num3.ToString();
                        sparewithPrice.ConsumableType = "1";
                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        sparewithPrice.JobCardDetailId = row["JobCardDetailId"].ToString();

                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    jobCardSpare.resultflag = "1";
                    jobCardSpare.Message = Constant.Message.SuccessMessage;
                }
                else
                {
                    jobCardSpare.resultflag = "0";
                    jobCardSpare.Message = Constant.Message.NoDataFound;
                }

                DataTable dataTableWithParams1 = this.dbCon.GetDataTableWithParams("Select JobCard_Cosumable_Mapping.Id,ConsumableId,(Select Name from [Consumables] where Id=[ConsumableId]) as Name,Quantity,Amount, Isnull(JobCardDetailId, 0) as JobCardDetailId from [JobCard_Cosumable_Mapping] where JobCardId=@1 and JobCardDetailId=@2 and IsDeleted=0 order by DOC DESC", new string[2]
        {
          JobCardId,JobCardDetailId
        });
                if (dataTableWithParams1 != null && dataTableWithParams1.Rows.Count > 0)
                {
                    foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams1.Rows)
                    {
                        ServiceClass.SparewithPrice sparewithPrice = new ServiceClass.SparewithPrice();
                        int MappingId = Convert.ToInt32(row["Id"]);
                        int ConsumableId = Convert.ToInt32(row["ConsumableId"]);

                        sparewithPrice.Id = row["ConsumableId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        Decimal num3 = Convert.ToDecimal(row["Amount"]);
                        Decimal num4 = Convert.ToDecimal(row["Quantity"]);
                        num1 += num3 * num4;
                        sparewithPrice.Price = num3.ToString();
                        sparewithPrice.ConsumableType = "3";
                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        sparewithPrice.JobCardDetailId = row["JobCardDetailId"].ToString();

                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    jobCardSpare.resultflag = "1";
                    jobCardSpare.Message = Constant.Message.SuccessMessage;
                }
                else
                {
                    jobCardSpare.resultflag = "0";
                    jobCardSpare.Message = Constant.Message.NoDataFound;
                }
                jobCardSpare.FinalAmount = Constant.Message.FinalAmount + num2.ToString();
                return jobCardSpare.Spares;
            }
            catch (Exception ex)
            {
                return new List<ServiceClass.SparewithPrice>();
            }
        }

        public string GetProblemsInJobCard(string JobCard)
        {
            string str = "";
            try
            {
                if (!string.IsNullOrEmpty(JobCard))
                {
                    int int32 = Convert.ToInt32(JobCard);
                    if (int32 > 0)
                    {
                        DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("DECLARE @listStr1 VARCHAR(MAX) SELECT @listStr1 = COALESCE(@listStr1+',' , '') + Convert(varchar,Name) FROM [dbo].[General_Problem_JobCard_Mapping] inner join General_Problems on General_Problems.Id=General_ProblemId where IsActive=1 and [General_Problem_JobCard_Mapping].JobCardId=@1 SELECT isnull(@listStr1,0) as Problems", new string[1]
            {
              int32.ToString()
            });
                        if (dataTableWithParams != null)
                        {
                            if (dataTableWithParams.Rows.Count > 0)
                                str = dataTableWithParams.Rows[0]["Problems"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                str = "";
            }
            return str;
        }

        public string GetAllProblems()
        {
            string str = "";
            try
            {

                DataTable dataTableWithParams = this.dbCon.GetDataTable("DECLARE @listStr1 VARCHAR(MAX) SELECT @listStr1 = COALESCE(@listStr1+',' , '') + Convert(varchar,Name) FROM  General_Problems  where IsActive=1  SELECT isnull(@listStr1,0) as Problems");

                if (dataTableWithParams != null)
                {
                    if (dataTableWithParams.Rows.Count > 0)
                        str = dataTableWithParams.Rows[0]["Problems"].ToString();
                }

            }
            catch (Exception ex)
            {
                str = "";
            }
            return str;
        }

        public string GetAllPerticulars()
        {
            string str = "";
            try
            {

                DataTable dataTableWithParams = this.dbCon.GetDataTable("DECLARE @listStr1 VARCHAR(MAX) SELECT @listStr1 = COALESCE(@listStr1+',' , '') + Convert(varchar,Name) FROM  Perticulars  where IsActive=1  SELECT isnull(@listStr1,0) as Problems");

                if (dataTableWithParams != null)
                {
                    if (dataTableWithParams.Rows.Count > 0)
                        str = dataTableWithParams.Rows[0]["Problems"].ToString();
                }

            }
            catch (Exception ex)
            {
                str = "";
            }
            return str;
        }

        public string GetCustomerNoteInJobCardList(string JobCard)
        {
            string str = "";
            try
            {
                if (!string.IsNullOrEmpty(JobCard))
                {
                    int int32 = Convert.ToInt32(JobCard);
                    if (int32 > 0)
                    {
                        DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("DECLARE @listStr1 VARCHAR(MAX) SELECT @listStr1 = COALESCE(@listStr1+',' , '') + Convert(varchar,Notes) FROM [dbo].CustomerNote  where JobCard_Id=@1 SELECT isnull(@listStr1,0) as Notes", new string[1]
            {
              int32.ToString()
            });
                        if (dataTableWithParams != null)
                        {
                            if (dataTableWithParams.Rows.Count > 0)
                                str = dataTableWithParams.Rows[0]["Notes"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                str = "";
            }
            return str;
        }
        public string GetJobCardInvoiceDetail(int JobCardId)
        {
            string result1 = "";
            if (JobCardId > 0)
            {
                string query = "Select Isnull(InvoiceUrl,'') as InvoiceUrl from JobCard_Invoice where JobCardId=@1 order by DOM DESC";
                string[] param = { JobCardId.ToString() };
                DataTable result = dbCon.GetDataTableWithParams(query, param);
                if (result != null && result.Rows.Count > 0)
                {
                    DataRow dr = result.Rows[0];
                    result1 = dr["InvoiceUrl"].ToString();
                }

            }
            return result1;


        }

        public int GetJobCardInvoiceDetailId(int JobCardId)
        {
            int result1 = 0;
            if (JobCardId > 0)
            {
                string query = "Select Isnull(Id,0) as InvoiceUrl from JobCard_Invoice where JobCardId=@1 order by DOM DESC";
                string[] param = { JobCardId.ToString() };
                DataTable result = dbCon.GetDataTableWithParams(query, param);
                if (result != null && result.Rows.Count > 0)
                {
                    DataRow dr = result.Rows[0];
                    result1 = Convert.ToInt32(dr["InvoiceUrl"]);
                }

            }
            return result1;


        }

        public int updateAmountJobCardInvoiceDetail(int JobCardId, decimal Amount)
        {
            int result = 0;
            if (JobCardId > 0)
            {
                string Selectquery = "Select * from JobCard_Invoice_TotalAmount where JobCardId=@1";
                string[] param = { JobCardId.ToString(), Amount.ToString() };
                DataTable dt = dbCon.GetDataTableWithParams(Selectquery, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string query = "update [JobCard_Invoice_TotalAmount] set FinalAmount=@2,DOM=CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME) where [JobCardId]=@1";
                    //  string[] param = { JobCardInvoiceDetailId.ToString(), Amount.ToString() };
                    result = dbCon.ExecuteQueryWithParams(query, param);
                }
                else
                {
                    string InsertQuery = "INSERT INTO [dbo].[JobCard_Invoice_TotalAmount]([JobCardId],[FinalAmount],[DOC],[DOM]) VALUES(@1 ,@2,CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME),CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME))";
                    result = dbCon.ExecuteQueryWithParams(InsertQuery, param);
                }
            }
            return result;
        }

        public int InsertJobCardInvoiceDetail(int jobcardid, int InvoiceNumber, string InvoiceUrl)
        {
            int result = 0;
            if (jobcardid > 0 && InvoiceNumber > 0 && !String.IsNullOrEmpty(InvoiceUrl))
            {
                string query = "INSERT INTO [dbo].[JobCard_Invoice]([JobCardId],[Invoice_Number],[InvoiceUrl],[DOC],[DOM]) VALUES (@1,@2,@3,CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME),CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME))";
                string[] param = { jobcardid.ToString(), InvoiceNumber.ToString(), InvoiceUrl };
                result = dbCon.ExecuteQueryWithParams(query, param);

            }
            return result;
        }


        public int InsertVehicle(string Vehicleno, int BrandId, int ModelId)
        {
            int result = 0;

            try
            {
                if (!String.IsNullOrEmpty(Vehicleno) && BrandId > 0 && ModelId > 0)
                {
                    String Query = "Select Id from  Vehicle where Number=@1 and IsDeleted=0";
                    string[] Sparam = { Vehicleno };
                    DataTable dtselect = dbCon.GetDataTableWithParams(Query, Sparam);
                    if (dtselect.Rows.Count == 0)
                    {
                        String InsertQuery = "INSERT INTO [dbo].[Vehicle] ([Number],[Vehicle_Brand_Id],[Vehicle_Model_Id],[IsDeleted],[DOC],[DOM]) VALUES (@1,@2,@3,0,CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME),CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME)) Select SCOPE_IDENTITY()";
                        string[] param = { Vehicleno, BrandId.ToString(), ModelId.ToString() };
                        int Executequery = dbCon.ExecuteScalarQueryWithParams(InsertQuery, param);
                        result = Executequery;
                    }
                    else
                    {
                        result = Convert.ToInt32(dtselect.Rows[0]["Id"]);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return result;

        }
        public string GetAdvisiorNoteInJobCardLIst(string JobCard)
        {
            string str = "";
            try
            {
                if (!string.IsNullOrEmpty(JobCard))
                {
                    int int32 = Convert.ToInt32(JobCard);
                    if (int32 > 0)
                    {
                        DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("DECLARE @listStr1 VARCHAR(MAX) SELECT @listStr1 =COALESCE(@listStr1+',' , '') + Convert(varchar,Notes) FROM [dbo].AdvisiorNote  where JobCard_Id=@1 SELECT isnull(@listStr1,0) as Notes", new string[1]
            {
              int32.ToString()
            });
                        if (dataTableWithParams != null)
                        {
                            if (dataTableWithParams.Rows.Count > 0)
                                str = dataTableWithParams.Rows[0]["Notes"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                str = "";
            }
            return str;
        }
        public string NumbersToWords(int inputNumber)
        {
            int inputNo = inputNumber;

            if (inputNo == 0)
                return "Zero";

            int[] numbers = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            if (inputNo < 0)
            {
                sb.Append("Minus ");
                inputNo = -inputNo;
            }

            string[] words0 = {"" ,"One ", "Two ", "Three ", "Four ",
            "Five " ,"Six ", "Seven ", "Eight ", "Nine "};
            string[] words1 = {"Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ",
            "Fifteen ","Sixteen ","Seventeen ","Eighteen ", "Nineteen "};
            string[] words2 = {"Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ",
            "Seventy ","Eighty ", "Ninety "};
            string[] words3 = { "Thousand ", "Lakh ", "Crore " };

            numbers[0] = inputNo % 1000; // units
            numbers[1] = inputNo / 1000;
            numbers[2] = inputNo / 100000;
            numbers[1] = numbers[1] - 100 * numbers[2]; // thousands
            numbers[3] = inputNo / 10000000; // crores
            numbers[2] = numbers[2] - 100 * numbers[3]; // lakhs

            for (int i = 3; i > 0; i--)
            {
                if (numbers[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (numbers[i] == 0) continue;
                u = numbers[i] % 10; // ones
                t = numbers[i] / 10;
                h = numbers[i] / 100; // hundreds
                t = t - 10 * h; // tens
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0) sb.Append("and ");
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }
            return sb.ToString().TrimEnd();
        }


        public string GetProblemsinJobCardList(string JobCard, string Type)
        {
            string str = "";
            try
            {
                if (!string.IsNullOrEmpty(JobCard))
                {
                    int int32 = Convert.ToInt32(JobCard);
                    if (int32 > 0)
                    {
                        DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("DECLARE @listStr1 VARCHAR(MAX) SELECT @listStr1 =COALESCE(@listStr1+',' , '') + Convert(varchar,Text) FROM [dbo].[JobCard_Details]  where JobCardId=@1 and Type=@2 and IsCustomerAgreed=1 SELECT isnull(@listStr1,'') as Notes", new string[2]
            {
              int32.ToString(),
              Type.ToString()
            });
                        if (dataTableWithParams != null)
                        {
                            if (dataTableWithParams.Rows.Count > 0)
                                str = dataTableWithParams.Rows[0]["Notes"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                str = "";
            }
            return str;

        }
        public string CustomerNotesForInvoice(string Jobcard, string Type)
        {
            string str = "";
            try
            {
                if (!string.IsNullOrEmpty(Jobcard))
                {
                    int int32 = Convert.ToInt32(Jobcard);
                    if (int32 > 0)
                    {
                        DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("DECLARE @listStr1 VARCHAR(MAX) SELECT @listStr1 =COALESCE(@listStr1+',' , '') + Convert(varchar(max),Text) FROM [dbo].[JobCard_Details]  where JobCardId=@1 and Type=@2 and isnull(IsDeleted,0)=0 SELECT isnull(@listStr1,'') as Notes", new string[2]
            {
              int32.ToString(),
              Type.ToString()
            });
                        if (dataTableWithParams != null)
                        {
                            if (dataTableWithParams.Rows.Count > 0)
                                str = dataTableWithParams.Rows[0]["Notes"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                str = "";
            }
            return str;
        }

        public bool GetProblemsInJobCardList(string Problem, string JobCardId, string Type)
        {
            bool result = false;
            string str = "";
            try
            {
                if (!string.IsNullOrEmpty(JobCardId))
                {
                    int int32 = Convert.ToInt32(JobCardId);
                    if (int32 > 0)
                    {
                        DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("DECLARE @listStr1 VARCHAR(MAX) SELECT @listStr1 =COALESCE(@listStr1+',' , '') + Convert(varchar,Text) FROM [dbo].[JobCard_Details]  where JobCardId=@1 and Type=@2 and IsCustomerAgreed=1 SELECT isnull(@listStr1,'') as Notes", new string[2]
            {
              int32.ToString(),
              Type.ToString()
            });
                        if (dataTableWithParams != null)
                        {
                            if (dataTableWithParams.Rows.Count > 0)
                                str = dataTableWithParams.Rows[0]["Notes"].ToString();
                            if (str.Contains(Problem))
                            {
                                result = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public string GetCustomerNoteInJobCard(string JobCard)
        {
            string str = "";
            try
            {
                if (!string.IsNullOrEmpty(JobCard))
                {
                    int int32 = Convert.ToInt32(JobCard);
                    if (int32 > 0)
                    {
                        DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("DECLARE @listStr1 VARCHAR(MAX) SELECT @listStr1 = COALESCE(@listStr1+'~', '') + Convert(varchar,Notes)+'~'+ Convert(varchar,Id)  FROM [dbo].CustomerNote  where JobCard_Id=@1 SELECT isnull(@listStr1,0) as Notes", new string[1]
            {
              int32.ToString()
            });
                        if (dataTableWithParams != null)
                        {
                            if (dataTableWithParams.Rows.Count > 0)
                                str = dataTableWithParams.Rows[0]["Notes"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                str = "";
            }
            return str;
        }

        public string GetAdvisiorNoteInJobCard(string JobCard)
        {
            string str = "";
            try
            {
                if (!string.IsNullOrEmpty(JobCard))
                {
                    int int32 = Convert.ToInt32(JobCard);
                    if (int32 > 0)
                    {
                        DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("DECLARE @listStr1 VARCHAR(MAX) SELECT @listStr1 = COALESCE(@listStr1+'~', '') + Convert(varchar,Notes)+'~'+ Convert(varchar,Id)  FROM [dbo].AdvisiorNote  where JobCard_Id=@1 SELECT isnull(@listStr1,0) as Notes", new string[1]
            {
              int32.ToString()
            });
                        if (dataTableWithParams != null)
                        {
                            if (dataTableWithParams.Rows.Count > 0)
                                str = dataTableWithParams.Rows[0]["Notes"].ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                str = "";
            }
            return str;
        }

        public bool GetProblemNameInJobCard(string Problem, string JobCardId, string Type)
        {

            bool result = false;
            string str = "";
            try
            {
                if (!string.IsNullOrEmpty(JobCardId))
                {
                    int int32 = Convert.ToInt32(JobCardId);
                    if (int32 > 0)
                    {
                        DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("DECLARE @listStr1 VARCHAR(MAX) SELECT @listStr1 = COALESCE(@listStr1+',' , '') + Convert(varchar,Name) FROM [dbo].[General_Problem_JobCard_Mapping] inner join General_Problems on General_Problems.Id=General_ProblemId where IsActive=1 and [General_Problem_JobCard_Mapping].JobCardId=@1 SELECT isnull(@listStr1,'') as Problems", new string[1]
            {
              int32.ToString()
            });
                        if (dataTableWithParams != null)
                        {
                            if (dataTableWithParams.Rows.Count > 0)
                                str = dataTableWithParams.Rows[0]["Problems"].ToString();
                            if (str.Contains(Problem))
                            {
                                result = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }

        public bool GetPerticularNameInJobCard(string Problem, string JobCardId, string Type)
        {

            bool result = false;
            string str = "";
            try
            {
                if (!string.IsNullOrEmpty(JobCardId))
                {
                    int int32 = Convert.ToInt32(JobCardId);
                    if (int32 > 0)
                    {
                        DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("DECLARE @listStr1 VARCHAR(MAX) SELECT @listStr1 = COALESCE(@listStr1+',' , '') + Convert(varchar,Name) FROM [dbo].[Perticular_JobCard_Mapping] inner join [Perticulars] on [Perticulars].Id=Perticular_Id where IsActive=1 and [Perticular_JobCard_Mapping].JobCardId=@1 SELECT isnull(@listStr1,'') as Problems", new string[1]
            {
              int32.ToString()
            });
                        if (dataTableWithParams != null)
                        {
                            if (dataTableWithParams.Rows.Count > 0)
                                str = dataTableWithParams.Rows[0]["Problems"].ToString();
                            if (str.Contains(Problem))
                            {
                                result = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        public bool EstimateConfirmed(int EstimateId)
        {
            bool flag = false;
            try
            {
                if (EstimateId > 0)
                {
                    DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("Select Isnull(IsConfimed,0) as IsConfirmed from Estimate where Id=@1", new string[1]
          {
            EstimateId.ToString()
          });
                    if (dataTableWithParams != null)
                    {
                        if (dataTableWithParams.Rows.Count > 0)
                        {
                            foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams.Rows)
                                flag = Convert.ToBoolean(row["IsConfirmed"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return flag;
        }

        public List<ServiceClass.ServicewithPrice> GetJobCardServiceInJobCard(string JobCardId)
        {
            ServiceClass.JobCardService jobCardService = new ServiceClass.JobCardService();
            try
            {
                Decimal num1 = new Decimal(0);
                Decimal num2 = new Decimal(0);
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("Select ServiceId,(Select Name from Service where Id=ServiceId) as Name ,Quantity,Amount,Isnull(JobCardDetailId,0) as JobCardDetailId from JobCard_Service_Mapping where JobCardId=@1 and IsDeleted=0  order by DOC DESC", new string[1]
        {
          JobCardId
        });
                if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                {
                    foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams.Rows)
                    {
                        ServiceClass.ServicewithPrice servicewithPrice = new ServiceClass.ServicewithPrice();
                        servicewithPrice.Id = row["ServiceId"].ToString();
                        servicewithPrice.Name = row["Name"].ToString();
                        Decimal num3 = Convert.ToDecimal(row["Amount"]);
                        Decimal num4 = new Decimal(1);
                        try
                        {
                            num4 = (Decimal)Convert.ToInt32(row["Quantity"]);
                        }
                        catch (Exception ex)
                        {
                        }
                        num1 += num3 * num4;
                        servicewithPrice.Price = num3.ToString();
                        servicewithPrice.Amount = num3.ToString();
                        servicewithPrice.Quantity = num4.ToString();
                        servicewithPrice.JobCardDetailId = row["JobCardDetailId"].ToString();
                        jobCardService.Services.Add(servicewithPrice);
                    }
                    Decimal num5 = num2 + num1;
                    jobCardService.TotalServiceAmount = Constant.Message.TotalServiceAmount + (object)num1;
                    jobCardService.resultflag = "1";
                    jobCardService.Message = Constant.Message.SuccessMessage;
                    jobCardService.FinalAmount = Constant.Message.FinalAmount + num5.ToString();
                }
                else
                {
                    jobCardService.resultflag = "0";
                    jobCardService.Message = Constant.Message.NoDataFound;
                }
                return jobCardService.Services;
            }
            catch (Exception ex)
            {
                return new List<ServiceClass.ServicewithPrice>();
            }
        }

        public List<ServiceClass.ServicewithPrice> GetJobCardServiceInJobCardWithDetail(string JobCardId, string JobCardDetailId)
        {
            ServiceClass.JobCardService jobCardService = new ServiceClass.JobCardService();
            try
            {
                Decimal num1 = new Decimal(0);
                Decimal num2 = new Decimal(0);
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("Select ServiceId,(Select Name from Service where Id=ServiceId) as Name ,Quantity,Amount,Isnull(JobCardDetailId,0) as JobCardDetailId from JobCard_Service_Mapping where JobCardId=@1 and JobCardDetailId=@2 and IsDeleted=0  order by DOC DESC", new string[2]
        {
          JobCardId,
          JobCardDetailId
        });
                if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                {
                    foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams.Rows)
                    {
                        ServiceClass.ServicewithPrice servicewithPrice = new ServiceClass.ServicewithPrice();
                        servicewithPrice.Id = row["ServiceId"].ToString();
                        servicewithPrice.Name = row["Name"].ToString();
                        Decimal num3 = Convert.ToDecimal(row["Amount"]);
                        Decimal num4 = new Decimal(1);
                        try
                        {
                            num4 = (Decimal)Convert.ToInt32(row["Quantity"]);
                        }
                        catch (Exception ex)
                        {
                        }
                        num1 += num3 * num4;

                        servicewithPrice.Price = num3.ToString();
                        servicewithPrice.Amount = num3.ToString();
                        servicewithPrice.Quantity = num4.ToString();

                        servicewithPrice.JobCardDetailId = row["JobCardDetailId"].ToString();
                        jobCardService.Services.Add(servicewithPrice);
                    }
                    Decimal num5 = num2 + num1;
                    jobCardService.TotalServiceAmount = Constant.Message.TotalServiceAmount + (object)num1;
                    jobCardService.resultflag = "1";
                    jobCardService.Message = Constant.Message.SuccessMessage;
                    jobCardService.FinalAmount = Constant.Message.FinalAmount + num5.ToString();
                }
                else
                {
                    jobCardService.resultflag = "0";
                    jobCardService.Message = Constant.Message.NoDataFound;
                }
                return jobCardService.Services;
            }
            catch (Exception ex)
            {
                return new List<ServiceClass.ServicewithPrice>();
            }
        }

        #region Requisition methods
        public int updateRequisitionSpare(int jobcardid)
        {
            int result = 0;
            if (jobcardid > 0)
            {
                string SelectQuery = "Select * from Requisition_Spare where [JobCardId]=@1";
                string[] param = { jobcardid.ToString() };
                DataTable dt = dbCon.GetDataTableWithParams(SelectQuery, param);
                if (dt != null && dt.Rows.Count > 0)
                {

                    string query = "update [Requisition_Spare] set  DOM=CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME)";

                    query += " where [JobCardId]=@1";
                    string[] param1 = { jobcardid.ToString() };
                    result = dbCon.ExecuteQueryWithParams(query, param1);
                    if (result > 0)
                    {
                        result = 1;
                    }
                }
            }
            return result;
        }
        public int InsertRequisitionMaster(int jobcardid, string datetime)
        {
            int result = 0;
            try
            {
                if (jobcardid > 0)
                {
                    string query1 = "INSERT INTO [dbo].[Requisition] ([JobCardId] ,[IsActive] ,[DOC]  ,[DOM]";
                    if (!String.IsNullOrEmpty(datetime))
                    {
                        query1 += ",[DateString]";
                    }
                    query1 += ") VALUES(@1,1,CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME),CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME)";
                    if (!String.IsNullOrEmpty(datetime))
                    {
                        query1 += ",CONVERT(DATETIME, '" + datetime + "', 102)";
                    }
                    query1 += ") Select SCOPE_IDENTITY() ";
                    string[] param1 = { jobcardid.ToString() };
                    result = dbCon.ExecuteScalarQueryWithParams(query1, param1);
                }
            }
            catch (Exception ex)
            {
            }
            return result;

        }

        public int UpdateRequisitionMaster(int jobcardid, string datetime)
        {
            int result = 0;
            try
            {
                if (jobcardid > 0)
                {
                    string query1 = "Update [dbo].[Requisition] set [DOM]=CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME)";
                    if (!String.IsNullOrEmpty(datetime))
                    {
                        query1 += ",[DateString]=CONVERT(DATETIME, '" + datetime + "', 102)";
                    }
                    //query1 += ") VALUES(@1,1,CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME),CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME)";
                    //if (!String.IsNullOrEmpty(datetime))
                    //{
                    //    query1 += ",CONVERT(DATETIME, '" + datetime + "', 102)";
                    //}
                    query1 += "where JobCardId=@1 ";
                    string[] param1 = { jobcardid.ToString() };
                    result = dbCon.ExecuteQueryWithParams(query1, param1);
                }
            }
            catch (Exception ex)
            {
            }
            return result;

        }



        public int InsertRequisitionSpare(int jobcardId, int spareid, int RequisitionId, float quantity, int mappingid)
        {
            int result = 0;
            if (jobcardId > 0 && spareid > 0 && RequisitionId > 0 && quantity > 0)
            {
                string query = "INSERT INTO [dbo].[Requisition_Spare]([JobCardId],[SpareId],[RequisitionId],[Quantity],[DOC],[DOM]";
                if (mappingid > 0)
                {
                    query += ", SpareMappingId";
                }
                query += ") VALUES (@1,@2,@3,@4,CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME),CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME)";
                if (mappingid > 0)
                {
                    query += " , @5";
                }
                query += ")";
                string[] param = { jobcardId.ToString(), spareid.ToString(), RequisitionId.ToString(), quantity.ToString(), mappingid.ToString() };
                result = dbCon.ExecuteQueryWithParams(query, param);
            }
            return result;
        }
        #region Spare
        //SpareId Or ConsumableId =Id
        public int IsSpareRequestionExist(int MappingId, int Type, int JobCardId, int Id)
        {
            int result = 0;
            string[] param = { JobCardId.ToString(), Id.ToString(), MappingId.ToString() };
            DataTable dt = null;
            if (MappingId > 0 && JobCardId > 0 && Type == 1 && Id > 0)
            {
                string query = "select Top 1 Id from Requisition_Spare where JobCardId=@1 and SpareId=@2 and SpareMappingId=@3";
                dt = dbCon.GetDataTableWithParams(query, param);
            }
            if (MappingId > 0 && JobCardId > 0 && Type == 3)
            {
                string query1 = "select Top 1 Id from [Requisition_Consumable] where JobCardId=@1 and SpareId=@2 and ConsumableMappingId=@3";
                dt = dbCon.GetDataTableWithParams(query1, param);
            }
            if (dt != null && dt.Rows.Count > 0)
            {
                result = 1;
            }
            return result;
        }
        #endregion



        public int InsertRequisitionConsumable(int jobcardId, int spareid, int RequisitionId, float quantity, int mappingId)
        {
            int result = 0;
            if (jobcardId > 0 && spareid > 0 && RequisitionId > 0 && quantity > 0)
            {
                string query = "INSERT INTO [dbo].[Requisition_Consumable]([JobCardId],[ConsumableId]";
                if (mappingId > 0)
                {
                    query += ",ConsumableMappingId";
                }
                query += ",[RequisitionId],[Quantity],[DOC],[DOM]) VALUES (@1,@2,@3,@4,CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME),CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME)";
                if (mappingId > 0)
                {
                    query += ",@5";
                }

                query += ")";
                string[] param = { jobcardId.ToString(), spareid.ToString(), RequisitionId.ToString(), quantity.ToString(), mappingId.ToString() };
                result = dbCon.ExecuteQueryWithParams(query, param);
            }
            return result;
        }
        public int updateRequisitionConsumable(int jobcardid)
        {
            int result = 0;
            string SelectQuery = "Select * from Requisition_Consumable where [JobCardId]=@1";
            string[] param = { jobcardid.ToString() };
            DataTable dt = dbCon.GetDataTableWithParams(SelectQuery, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                string query = "update [Requisition_Consumable] set  DOM=CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME) where [JobCardId]=@1";
                string[] param1 = { jobcardid.ToString() };
                result = dbCon.ExecuteQueryWithParams(query, param1);
                if (result > 0)
                {
                    result = 1;
                }
            }
            return result;
        }

        public int IsResendRequisition(int JobCardId)
        {
            int result = 0;
            try
            {
                if (JobCardId > 0)
                {
                    string query = "Select Id from [Requisition] where JobCardId=@1";
                    string[] param = { JobCardId.ToString() };
                    DataTable dt = dbCon.GetDataTableWithParams(query, param);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        result = 1;
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return result;
        }
        #endregion


        public string GetEstimateType(int JobCardId, int Id)
        {
            string str = "Old";
            if (JobCardId > 0 && Id > 0)
            {
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("Select Top 1 Id from Estimate where JobCardId=@1  Order By DOM asc", new string[1]
        {
          JobCardId.ToString()
        });
                str = dataTableWithParams == null || dataTableWithParams.Rows.Count <= 0 ? "New" : (Convert.ToInt32(dataTableWithParams.Rows[0]["Id"]) != Id ? "Revised" : "Old");
            }
            return str;
        }

        public Decimal EstimateCost(int JobCardId)
        {
            Decimal num1 = new Decimal(0);
            try
            {
                if (JobCardId > 0)
                {
                    DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("Select top 1 TotalSpareAmount,TotalServiceAmount from Estimate where JobCardId=@1 order by DOM desc", new string[1]
          {
            JobCardId.ToString()
          });
                    if (dataTableWithParams != null)
                    {
                        if (dataTableWithParams.Rows.Count > 0)
                        {
                            DataRow row = dataTableWithParams.Rows[0];
                            Decimal num2 = new Decimal(0);
                            Decimal num3 = new Decimal(0);
                            num1 = Convert.ToDecimal(row["TotalSpareAmount"]) + Convert.ToDecimal(row["TotalServiceAmount"]);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return num1;
        }

        public bool IsCustomerJobCardRunning(int CustomerId)
        {
            bool result = false;
            try
            {
                if (CustomerId > 0)
                {
                    DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("SELECT ID  FROM [Motorz].[dbo].[JobCard] where Customer_Id=@1 and JobStatus_Id=4 and IsDelete=0", new string[1]
          {
            CustomerId.ToString()
          });
                    if (dataTableWithParams != null)
                    {
                        if (dataTableWithParams.Rows.Count > 0)
                        {
                            result = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }

        public bool IsRevisitedEstimateBJobCardId(string JobCardId)
        {
            bool flag = false;
            if (!string.IsNullOrEmpty(JobCardId))
            {
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("Select Isnull(IsRevisedEstimate,0) as IsRevisredEstimated from Estimate where JobCardId=@1 ", new string[1]
        {
          JobCardId
        });
                if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                    flag = true;
            }
            return flag;
        }



        public int InsertVehicleBrand(string Name)
        {
            int i = 0;
            if (!String.IsNullOrEmpty(Name))
            {
                string query = "INSERT INTO [dbo].[Vehicle_Brand]([Name],[IsActive],[IsDelete],[DisplayOrder],[DOC],[DOM]) VALUES(@1,1,0,0,CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME),CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME)) Select SCOPE_IDENTITY()";
                string[] param = { Name.Replace("'", "''") };
                i = dbCon.ExecuteScalarQueryWithParams(query, param);
            }
            return i;
        }
        public int InsertVehicleBrandWashing(string Name)
        {
            int i = 0;
            if (!String.IsNullOrEmpty(Name))
            {
                String Selectquery = "Select * from Washing_Vehicle_Brand where Name  like Concat('%',@1,'%')";
                string[] parm = { Name };
                DataTable dt = dbCon.GetDataTableWithParams(Selectquery, parm);
                if (dt.Rows.Count == 0)
                {
                    string query = "INSERT INTO [dbo].[Washing_Vehicle_Brand]([Name],[IsActive],[IsDelete],[DisplayOrder],[DOC],[DOM]) VALUES(@1,1,0,0,CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME),CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME)) Select SCOPE_IDENTITY()";
                    string[] param = { Name.Replace("'", "''") };
                    i = dbCon.ExecuteScalarQueryWithParams(query, param);
                }
                else
                {
                    i = -1;

                    //i = Convert.ToInt32(dt.Rows[0]["Id"]);
                }
            }
            return i;
        }

        public int DeleteVehicleBrand(int Id)
        {
            int i = 0;
            if (Id > 0)
            {
                string selectquery = "Select id from Washing_Vehicle_Brand where Id=@1";
                string[] param = { Id.ToString() };
                DataTable dt = dbCon.GetDataTableWithParams(selectquery, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string query = "Delete [Washing_Vehicle_Brand]  where Id =@1";
                    string[] param1 = { Id.ToString() };
                    i = dbCon.ExecuteQueryWithParams(query, param1);
                }
            }
            return i;
        }

        public int DeleteVehicleModel(int Id)
        {
            int i = 0;
            if (Id > 0)
            {
                string selectquery = "Select Id from Washing_Vehicle_Model where Id=@1";
                string[] param = { Id.ToString() };
                DataTable dt = dbCon.GetDataTableWithParams(selectquery, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string query = "Delete Washing_Vehicle_Model where Id =@1";
                    string[] param1 = { Id.ToString() };
                    i = dbCon.ExecuteQueryWithParams(query, param1);
                }
            }
            return i;
        }

        public int DeleteModelMappingEntry(int Id)
        {
            int i = 0;
            if (Id > 0)
            {
                string selectquery = "Select Id from Segment_Model_Mapping_Washing where Id=@1";
                string[] param = { Id.ToString() };
                DataTable dt = dbCon.GetDataTableWithParams(selectquery, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string query = "Delete Segment_Model_Mapping_Washing where Id =@1";
                    string[] param1 = { Id.ToString() };
                    i = dbCon.ExecuteQueryWithParams(query, param1);
                }
            }
            return i;
        }


        public int InsertVehicleSegment(string Name)
        {
            int i = 0;
            if (!String.IsNullOrEmpty(Name))
            {

                string query = "INSERT INTO [dbo].[Segment]([Name],[DisplayOrder] ,[IsActive],[IsDeleted],[DOM],[DOC]) VALUES (@1,0,1,0,CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME),CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME)) Select SCOPE_IDENTITY()";
                string[] param = { Name.Replace("'", "''") };
                i = dbCon.ExecuteScalarQueryWithParams(query, param);
            }
            return i;
        }
        public int InsertVehicleModel(string Name, int SegmentId, int BrandId)
        {
            int i = 0;
            if (!String.IsNullOrEmpty(Name) && SegmentId > 0 && BrandId > 0)
            {
                string Selectquery = "Select Id from Vehicle_Model where SegmentId=@1 and Vehicle_Brand_Id=@2";
                string[] selectparam = { SegmentId.ToString(), BrandId.ToString() };
                DataTable dtselect = dbCon.GetDataTableWithParams(Selectquery, selectparam);
                if (dtselect.Rows.Count == 0)
                {
                    string query = "INSERT INTO [dbo].[Vehicle_Model]([Name],[Vehicle_Brand_Id] ,[SegmentId],[IsActive],[IsDelete],[DOC] ,[DOM],[DisplayOrder]) VALUES(@1,@2,@3,1,0 ,CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME),CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME),0) Select SCOPE_IDENTITY()";
                    string[] param = { Name.Replace("'", "''"), SegmentId.ToString(), BrandId.ToString() };
                    i = dbCon.ExecuteScalarQueryWithParams(query, param);
                }
                else
                {
                    string query = "Update [dbo].[Vehicle_Model] Set DOM=CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME) where SegmentId=@1 and Vehicle_Brand_Id=@2 ";
                    string[] param = { SegmentId.ToString(), BrandId.ToString() };
                    i = dbCon.ExecuteScalarQueryWithParams(query, param);
                    i = Convert.ToInt32(dtselect.Rows[0]["Id"]);

                }
            }
            return i;
        }
        public int InsertMappingSegmentModelBrand(int modelid, int SegmentId)
        {
            int i = 0;
            if (modelid > 0 && SegmentId > 0)
            {
                string Selectquery = "Select Id from Segment_Model_Mapping_Washing where SegmentId=@1 and ModelId=@2 ";
                string[] selectparam = { SegmentId.ToString(), modelid.ToString() };
                DataTable dtselect = dbCon.GetDataTableWithParams(Selectquery, selectparam);
                if (dtselect.Rows.Count == 0)
                {

                    string query = "INSERT INTO [dbo].[Segment_Model_Mapping_Washing]([SegmentId],[ModelId]  ,[DOC],[DOM] ,[IsActive] ,[IsDeleted]) VALUES (@1,@2,CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME),CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME),1,0) Select SCOPE_IDENTITY()";
                    string[] param = { SegmentId.ToString(), modelid.ToString() };
                    i = dbCon.ExecuteScalarQueryWithParams(query, param);
                }
                else
                {
                    string query = "Update [dbo].[Segment_Model_Mapping_Washing] Set DOM=CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME) where SegmentId=@1 and ModelId=@2  ";
                    string[] param = { SegmentId.ToString(), modelid.ToString() };
                    i = dbCon.ExecuteScalarQueryWithParams(query, param);
                    i = Convert.ToInt32(dtselect.Rows[0]["Id"]);
                }
            }
            return i;
        }


        public string GetJobCardCreatedDate(string ID)
        {
            string str = "";
            try
            {
                if (!string.IsNullOrEmpty(ID))
                {
                    Convert.ToInt32(ID);
                    DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("Select DOC from JobCard where Id =@1", new string[1]
          {
            ID
          });
                    if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                    {
                        foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams.Rows)
                            str = row["DOC"].ToString();
                    }
                    else
                        str = this.getindiantime().ToString();
                }
            }
            catch (Exception ex)
            {
                str = this.getindiantime().ToString();
            }
            return str;
        }

        public DataTable SavePhotos(List<string> fileData, string albumName, string UserId)
        {
            DataTable dataTable = new DataTable();
            try
            {
                dataTable.Columns.Add("WebView_Photo_File_Name");
                dataTable.Columns.Add("DownloadView_Photo_File_Name");
                if (fileData != null && albumName != null && albumName != "")
                {
                    albumName = albumName.Trim().Replace(" ", "_");
                    for (int index = 0; index < fileData.Count; ++index)
                    {
                        DataRow row = dataTable.NewRow();
                        string str1 = fileData[index].ToString();
                        if (File.Exists(Constant.Message.FinalTempPhotos + "\\" + str1))
                        {
                            string sourceFileName = Constant.Message.FinalTempPhotos + "\\" + str1;
                            string str2 = str1;
                            string destFileName = Constant.Message.FinalWebViewPath + "\\" + str2;
                            if (!Directory.Exists(Constant.Message.FinalWebViewPath))
                                Directory.CreateDirectory(Constant.Message.FinalWebViewPath);
                            File.Move(sourceFileName, destFileName);
                            row["WebView_Photo_File_Name"] = (object)str2;
                            row["DownloadView_Photo_File_Name"] = (object)str2;
                           // this.dbCon.InsertLogs("File Transfered");
                            dataTable.Rows.Add(row);
                        }
                        else
                            this.dbCon.InsertLogs("File not exist ");
                    }
                }
                else
                    this.dbCon.InsertLogs("Something Went Wrong!");
            }
            catch (Exception ex)
            {
                this.dbCon.InsertLogs(ex.ToString());
                throw;
            }
            return dataTable;
        }

        public int InsertLogsForApp(string Name = "")
        {
            int num = 0;
            try
            {
                this.dbCon.ExecuteQueryWithParams("INSERT INTO [dbo].[Log]([name]) VALUES(@1)", new string[1]
        {
          Name
        });
            }
            catch (Exception ex)
            {
                num = -1;
            }
            return num;
        }


        public string UploadPhotoAlbums(string userid, string albumName, List<string> albumPhotos, string JobCardId)
        {
            string str = "";
            DataTable dataTable = new DataTable();
            try
            {
                if (albumPhotos.Count > 0)
                {
                    DataTable DtImageList = this.SavePhotos(albumPhotos, JobCardId, userid);
                    str = "Photos Uploaded";
                    if (DtImageList.Rows.Count > 0)
                        this.InsertVehicleImages(userid, DtImageList, JobCardId);
                }
            }
            catch (Exception ex)
            {
            }
            return str;
        }

        public int InsertVehicleImages(string UserId, DataTable DtImageList, string JobCardId)
        {
            int num = 0;
            string str = "";
            if (!string.IsNullOrEmpty(UserId) && !string.IsNullOrEmpty(JobCardId))
            {
                Convert.ToInt32(JobCardId);
                if (DtImageList.Rows.Count > 0)
                {
                    foreach (DataRow row in (InternalDataCollectionBase)DtImageList.Rows)
                    {
                        if (row != null)
                        {
                            if (!string.IsNullOrEmpty(row["WebView_Photo_File_Name"].ToString()))
                                str = row["WebView_Photo_File_Name"].ToString();
                            num = this.dbCon.ExecuteScalarQueryWithParams("INSERT INTO [dbo].[JobCard_Vehicle_Images]([JobCardId],[Link] ,[IsDeleted] ,[DOC] ,[DOM]) VALUES (@1,@2,0,CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME),CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME)) SELECT SCOPE_IDENTITY()", new string[2]
              {
                JobCardId,
                str
              });
                        }
                    }
                }
            }
            return num;
        }

        public string JobCardDateFormat(DateTime AuctionDate)
        {
            return string.Format("{0:MMMM dd, yyyy}", (object)AuctionDate);
        }
        public string JobCardDateFormatWithTime(DateTime AuctionDate)
        {
            return string.Format("{0:dd MMMM, yyyy HH:mm:ss}", (object)AuctionDate);
        }
        public int IsEstimateGenerated(string JobCardId)
        {
            int i = 0;
            try
            {
                if (!String.IsNullOrEmpty(JobCardId))
                {
                    string query = "SELECT TOP 1 Id FROM [dbo].[Estimate] where JobCardId=@1 ";
                    string[] param = { JobCardId };
                    DataTable dt = dbCon.GetDataTableWithParams(query, param);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        i = 1;
                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
            }
            return i;
        }

        public int IsAssignedCameraToJobCardId(string JobCardId)
        {
            int i = 0;
            try
            {
                if (!String.IsNullOrEmpty(JobCardId))
                {


                    //string query = "SELECT TOP 1 Id FROM [dbo].[Camera_JobCard_Mapping] where CameraId=@1 and IsActive=1 and AssignDate is not null order by AssignDate desc";
                    // in floor manager assign and Start Work query
                    string query = "SELECT TOP 1 Id FROM [dbo].[Camera_JobCard_Mapping] where CameraId=@1 and IsActive=1 and AssignDate is not null and StartDate is not null order by AssignDate desc";
                    string[] param = { JobCardId };
                    DataTable dt = dbCon.GetDataTableWithParams(query, param);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        i = 1;
                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
            }
            return i;
        }

        public int IsRunningCameraToJobCardId(string JobCardId)
        {
            int i = 0;
            try
            {
                if (!String.IsNullOrEmpty(JobCardId))
                {
                    string query = "SELECT TOP 1000 Id FROM [dbo].[Camera_JobCard_Mapping] where CameraId=@1 and IsActive=1 and AssignDate is not null and StartDate is not null and EndDate is null";
                    string[] param = { JobCardId };
                    DataTable dt = dbCon.GetDataTableWithParams(query, param);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        i = 1;
                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
            }
            return i;
        }
        public int WorkStatusCameraToJobCardId(string JobCardId)
        {
            int i = 0;
            try
            {
                if (!String.IsNullOrEmpty(JobCardId))
                {

                    string query1 = "SELECT TOP 1000 * FROM [dbo].[Camera_JobCard_Mapping] where CameraId=@1 and IsActive=1 and StartDate is not null and EndDate is null";
                    string[] param1 = { JobCardId };
                    DataTable dt1 = dbCon.GetDataTableWithParams(query1, param1);
                    if (dt1 != null && dt1.Rows.Count > 0)
                    {
                        //running
                        i = 0;
                        return i;
                    }
                    string query2 = "SELECT TOP 1000 * FROM [dbo].[Camera_JobCard_Mapping] where CameraId=@1 and IsActive=1 and AssignDate is not null and StartDate is  null and EndDate is null";
                    string[] param2 = { JobCardId };
                    DataTable dt2 = dbCon.GetDataTableWithParams(query2, param2);
                    if (dt2 != null && dt2.Rows.Count > 0)
                    {
                        //only assign not running
                        i = 1;
                        return i;
                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
            }
            return i;
        }

        public int CameraIdfromJobCardId(string JobCardId)
        {
            int cameraId = 0;
            try
            {
                if (!String.IsNullOrEmpty(JobCardId))
                {
                    //string query = "SELECT TOP 1000 CameraId FROM [dbo].[Camera_JobCard_Mapping] where JobCardId=@1  ";
                    //string[] param = { JobCardId };
                    //DataTable dt = dbCon.GetDataTableWithParams(query, param);
                    //if (dt != null && dt.Rows.Count > 0)
                    //{
                    //    cameraId = Convert.ToInt32(dt.Rows[0]["CameraId"]);
                    //}
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
            }
            return cameraId;
        }

        public int AssignJobCardbyCamera(string CameraId)
        {
            int i = 0;
            try
            {
                if (!String.IsNullOrEmpty(CameraId))
                {
                    //string query = "SELECT TOP 1 JobCardId FROM [dbo].[Camera_JobCard_Mapping] inner join JobCard on JobCard.id=Camera_JobCard_Mapping.JobCardId where CameraId=@1 and IsActive=1 and AssignDate is not null and Startdate is  null and EndDate is null order by AssignDate desc  ";
                    string query = "SELECT TOP 1 JobCardId FROM [dbo].[Camera_JobCard_Mapping] inner join JobCard on JobCard.id=Camera_JobCard_Mapping.JobCardId where CameraId=@1 and IsActive=1 and AssignDate is not null and Startdate is not null and EndDate is null order by AssignDate desc  ";
                    string[] param = { CameraId };
                    DataTable dt = dbCon.GetDataTableWithParams(query, param);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        i = Convert.ToInt32(dr["JobCardId"]);

                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
            }
            return i;
        }


        public string GetUrlByJobCardId(string JobCardId)
        {
            string url = "";
            try
            {
                if (!String.IsNullOrEmpty(JobCardId))
                {
                    string query = "SELECT TOP 1 Isnull(Link,'') as Link FROM [dbo].[Camera_JobCard_Mapping] inner join JobCard on JobCard.id=Camera_JobCard_Mapping.JobCardId inner join Camera_master on Camera_Master.Id=CameraId where [Camera_JobCard_Mapping].JobcardId=@1";
                    string[] param = { JobCardId };
                    DataTable dt = dbCon.GetDataTableWithParams(query, param);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        url = dr["Link"].ToString();

                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {
            }
            return url;
        }

        public int InsertInsurance(string VehicleId, string InsuranceProviderId, string InsuranceNo, string InsuranceExpireDate)
        {
            int num;
            try
            {
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("select * from Vehicle_Insurance where VehicleId=@1", new string[1]
        {
          VehicleId
        });
                if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                    num = this.dbCon.ExecuteQueryWithParams("UPDATE [dbo].[Vehicle_Insurance]  SET [Insurance_Provider_Id] = @2,[Insurance_Number] = @3,[Insurance_Expire_Date] = @4,[DOM] = CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME) WHERE [VehicleId] = @1", new string[4]
          {
            VehicleId,
            InsuranceProviderId,
            InsuranceNo,
            InsuranceExpireDate
          });
                else
                    num = this.dbCon.ExecuteQueryWithParams("INSERT INTO [dbo].[Vehicle_Insurance]([VehicleId],[Insurance_Provider_Id] ,[Insurance_Number],[Insurance_Expire_Date] ,[Insurance_Status_Id] ,[DOC] ,[DOM] ,[IsActive] ,[IsDeleted]) VALUES (@1,@2 ,@3,@4,1,CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME) ,CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME),1,0) ", new string[4]
          {
            VehicleId,
            InsuranceProviderId,
            InsuranceNo,
            InsuranceExpireDate
          });
            }
            catch (Exception ex)
            {
                num = 0;
            }
            return num;
        }

        public int UpdateInsurance(string VehicleId, string InsuranceProviderId, string InsuranceNo, string InsuranceExpireDate)
        {
            try
            {
                return this.dbCon.ExecuteScalarQueryWithParams("UPDATE [dbo].[Vehicle_Insurance]  SET [Insurance_Provider_Id] = @2,[Insurance_Number] = @3,[Insurance_Expire_Date] = @4,[DOM] = CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME) WHERE [VehicleId] = @1 SELECT SCOPE_IDENTITY()", new string[4]
        {
          VehicleId,
          InsuranceProviderId,
          InsuranceNo,
          InsuranceExpireDate
        });
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public string DOBFormat(string DOB)
        {
            string str = "";
            try
            {
                if (!string.IsNullOrEmpty(DOB))
                    str = Convert.ToDateTime(DOB).ToString("dd-MM-yyyy");
            }
            catch (Exception ex)
            {
                str = "";
            }
            return str;
        }

        public int InsertCustomerVehicleMapping(string CustomerId, string VehicleId)
        {
            int num = 0;
            try
            {
                if (!string.IsNullOrEmpty(CustomerId))
                {
                    if (!string.IsNullOrEmpty(VehicleId))
                    {
                        DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("Select * from Vehicle_Customer_Mapping where Customer_Id=@1 and Vehicle_Id=@2", new string[2]
            {
              CustomerId,
              VehicleId
            });
                        if (dataTableWithParams == null || dataTableWithParams.Rows.Count == 0)
                            num = this.dbCon.ExecuteQueryWithParams("INSERT INTO [dbo].[Vehicle_Customer_Mapping]([Customer_Id],[Vehicle_Id],[IsDelete],[DOC],[DOM]) VALUES (@1 , @2 ,0,GETDATE(),GETDATE())", new string[2]
              {
                CustomerId,
                VehicleId
              });
                        else
                            num = this.dbCon.ExecuteQueryWithParams("Update Vehicle_Customer_Mapping Set Customer_Id=@1 where Vehicle_Id=@2", new string[2]
              {
                CustomerId,
                VehicleId
              });
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return num;
        }

        public int InsertVehicleCustomerMapping(string JobCardId)
        {
            int num1 = 0;
            try
            {
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("Select Vehicle_Id, Customer_Id from JobCard where Id=@1", new string[1]
        {
          JobCardId
        });
                if (dataTableWithParams != null)
                {
                    if (dataTableWithParams.Rows.Count > 0)
                    {
                        DataRow row = dataTableWithParams.Rows[0];
                        int num2 = 0;
                        try
                        {
                            num2 = Convert.ToInt32(row["Vehicle_Id"]);
                        }
                        catch (Exception ex)
                        {
                        }
                        int num3 = 0;
                        try
                        {
                            num3 = Convert.ToInt32(row["Customer_Id"]);
                        }
                        catch (Exception ex)
                        {
                        }
                        if (num3 > 0)
                        {
                            if (num2 > 0)
                                num1 = this.InsertCustomerVehicleMapping(num3.ToString(), num2.ToString());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.dbCon.InsertLogs(ex.Message);
            }
            return num1;
        }

        public Decimal GetTotalSpareAmount(string JobCardId)
        {
            int num = 0;
            try
            {
                if (!string.IsNullOrEmpty(JobCardId))
                {
                    DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("(Select  Isnull(SUM(Amount * Quantity),0) as TotalSpareAmount from JobCard_Spare_Mapping where JobCardId=@1 and IsDeleted=0) ", new string[1]
          {
            JobCardId
          });
                    if (dataTableWithParams != null)
                    {
                        if (dataTableWithParams.Rows.Count > 0)
                            num += Convert.ToInt32(dataTableWithParams.Rows[0]["TotalSpareAmount"]);
                    }
                }
                if (!string.IsNullOrEmpty(JobCardId))
                {
                    DataTable dataTableWithParams1 = this.dbCon.GetDataTableWithParams("(Select  Isnull(SUM(Amount * Quantity),0) as TotalSpareAmount from JobCard_Cosumable_Mapping where JobCardId=@1 and IsDeleted=0) ", new string[1]
          {
            JobCardId
          });
                    if (dataTableWithParams1 != null)
                    {
                        if (dataTableWithParams1.Rows.Count > 0)
                            num += Convert.ToInt32(dataTableWithParams1.Rows[0]["TotalSpareAmount"]);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return (Decimal)num;
        }

        public Decimal GetJobCardFinalAmountChanges(int JobcardId)
        {
            decimal finalamount = 0;
            if (JobcardId > 0)
            {
                string query = "Select Top 1 Isnull(FinalAmount,0) as FinalAmount from [JobCard_Final_Amount_Change] where JobcardId=@1 Order by DOC DESC";
                string[] param = { JobcardId.ToString() };
                DataTable dt = dbCon.GetDataTableWithParams(query, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    finalamount = Convert.ToDecimal(dr["FinalAmount"]);
                }
            }
            return finalamount;

        }

        public bool GetJobCardOverrideChanges(int JobcardId)
        {
            bool finalamount = false;
            if (JobcardId > 0)
            {
                string query = "Select Top 1 Isnull(IsOverride,0) as IsOverride from [JobCard_Final_Amount_Change] where JobcardId=@1 Order by DOC DESC";
                string[] param = { JobcardId.ToString() };
                DataTable dt = dbCon.GetDataTableWithParams(query, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    finalamount = Convert.ToBoolean(dr["IsOverride"]);
                }
            }
            return finalamount;

        }

        public bool updateJobCardOverrideChanges(int jobcardid)
        {
            bool finalamount = false;
            if (jobcardid > 0)
            {
                string query = "Select Top 1 Id from [JobCard_Final_Amount_Change] where JobcardId=@1 Order by DOC DESC";
                string[] param = { jobcardid.ToString() };
                DataTable dt = dbCon.GetDataTableWithParams(query, param);
                if (dt != null && dt.Rows.Count > 0)
                {

                    string updatequery = "update JobCard_Final_Amount_Change set IsOverride=0  where Id in (Select Top 1 Id from [JobCard_Final_Amount_Change] where JobcardId=@1 Order by DOC DESC)";
                    int Executequery = dbCon.ExecuteQueryWithParams(updatequery, param);
                    if (Executequery >= 1)
                    {
                        finalamount = false;
                    }
                    else
                    {
                        DataRow dr = dt.Rows[0];
                        finalamount = Convert.ToBoolean(dr["IsOverride"]);
                    }
                }
            }
            return finalamount;
        }
        public bool GetJobCardFinalAmountChangesSendToCustomer(int JobcardId)
        {
            bool RESULT = false;
            if (JobcardId > 0)
            {
                string query = "SELECT TOP 1 [Id],Isnull([ShowToCustomer],0) as ShowToCustomer FROM [Motorz].[dbo].[Estimate] WHERE JobCardId=@1 order by DOM DESC";
                string[] param = { JobcardId.ToString() };
                DataTable dt = dbCon.GetDataTableWithParams(query, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    RESULT = Convert.ToBoolean(dr["ShowToCustomer"]);
                }
            }
            return RESULT;

        }
        public int GetQuantityOFService(int ServiceId, int JobCardId)
        {
            int result = 0;
            if (ServiceId > 0 && JobCardId > 0)
            {
                string query = "Select Isnull(SUM(Quantity),0) from JobCard_Service_Mapping where ServiceId=@1 and JobCardId=@2  and IsDeleted=0";
                string[] param = { ServiceId.ToString(), JobCardId.ToString() };
                DataTable dt = dbCon.GetDataTableWithParams(query, param);
                if (dt != null && dt.Rows.Count > 0)
                {

                    try { result = Convert.ToInt32(dt.Rows[0][0]); }
                    catch (Exception ex)
                    {
                    }
                }

            }
            return result;
        }
        public int GetQuantityOFSpares(int ServiceId, int JobCardId)
        {
            int result = 0;
            if (ServiceId > 0 && JobCardId > 0)
            {
                string query = "Select top 1 Isnull(SUM(Quantity),0) as Quantity from JobCard_Spare_Mapping where SpareId=@1 and JobCardId=@2  and IsDeleted=0 group by JobCardDetailId";
                string[] param = { ServiceId.ToString(), JobCardId.ToString() };
                DataTable dt = dbCon.GetDataTableWithParams(query, param);
                if (dt != null && dt.Rows.Count > 0)
                {

                    try { result = Convert.ToInt32(dt.Rows[0][0]); }
                    catch (Exception ex)
                    {
                    }
                }

            }
            return result;
        }


        public Decimal GetTotalSpareAmountWithDetail(string JobCardId, string JobCardDetailId)
        {
            Decimal num = 0;
            try
            {
                if (!string.IsNullOrEmpty(JobCardId) && !String.IsNullOrEmpty(JobCardDetailId))
                {
                    DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("(Select  Isnull(SUM(Amount * Quantity),0) as TotalSpareAmount from JobCard_Spare_Mapping where JobCardId=@1 and JobCardDetailId=@2 and IsDeleted=0) ", new string[2]
          {
            JobCardId,JobCardDetailId
          });
                    if (dataTableWithParams != null)
                    {
                        if (dataTableWithParams.Rows.Count > 0)
                            num += Convert.ToDecimal(dataTableWithParams.Rows[0]["TotalSpareAmount"]);
                    }
                }
                if (!string.IsNullOrEmpty(JobCardId) && !String.IsNullOrEmpty(JobCardDetailId))
                {
                    DataTable dataTableWithParams1 = this.dbCon.GetDataTableWithParams("(Select  Isnull(SUM(Amount * Quantity),0) as TotalSpareAmount from JobCard_Cosumable_Mapping where JobCardId=@1 and JobCardDetailId=@2 and IsDeleted=0) ", new string[2]
          {
            JobCardId,JobCardDetailId
          });
                    if (dataTableWithParams1 != null)
                    {
                        if (dataTableWithParams1.Rows.Count > 0)
                            num += Convert.ToDecimal(dataTableWithParams1.Rows[0]["TotalSpareAmount"]);
                    }
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
            return (Decimal)num;
        }

        public Decimal GetTotalServiceAmount(string JobCardId)
        {
            Decimal num = 0;
            try
            {
                if (!string.IsNullOrEmpty(JobCardId))
                {
                    DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("(Select  Isnull(SUM(Amount * Quantity),0) as TotalServiceAmount from  JobCard_Service_Mapping  where JobCardId=@1 and IsDeleted=0) ", new string[1]
          {
            JobCardId
          });
                    if (dataTableWithParams != null)
                    {
                        if (dataTableWithParams.Rows.Count > 0)
                            num = Convert.ToDecimal(dataTableWithParams.Rows[0]["TotalServiceAmount"]);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return (Decimal)num;
        }

        public Decimal GetTotalServiceAmountWithDetail(string JobCardId, string JobCardDetailId)
        {
            Decimal num = 0;
            try
            {
                if (!string.IsNullOrEmpty(JobCardId) && !String.IsNullOrEmpty(JobCardDetailId))
                {
                    DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("(Select  Isnull(SUM(Amount * Quantity),0) as TotalServiceAmount from  JobCard_Service_Mapping  where JobCardId=@1 and JobCardDetailId=@2 and IsDeleted=0) ", new string[2]
          {
            JobCardId,
            JobCardDetailId
          });
                    if (dataTableWithParams != null)
                    {
                        if (dataTableWithParams.Rows.Count > 0)
                            num = Convert.ToDecimal(dataTableWithParams.Rows[0]["TotalServiceAmount"]);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return (Decimal)num;
        }

        public ServiceClass.InsertVehicleSpare GetVehicleSpareAndServiceByJobCardId(int JobCardId)
        {
            ServiceClass.InsertVehicleSpare insertVehicleSpare = new ServiceClass.InsertVehicleSpare();
            if (JobCardId <= 0)
                return new ServiceClass.InsertVehicleSpare();
            insertVehicleSpare.TotalServiceAmount = this.GetTotalServiceAmount(JobCardId.ToString()).ToString();
            insertVehicleSpare.TotalSpareAmount = this.GetTotalSpareAmount(JobCardId.ToString()).ToString();
            insertVehicleSpare.Spares = this.GetJobCardSpareInJobCard(JobCardId.ToString());
            insertVehicleSpare.Services = this.GetJobCardServiceInJobCard(JobCardId.ToString());
            insertVehicleSpare.FinalAmount = (this.GetTotalServiceAmount(JobCardId.ToString()) + this.GetTotalSpareAmount(JobCardId.ToString())).ToString();
            return insertVehicleSpare;
        }

        public int DeleteAllServicebyJobCardDetailId(int JobCardId, int JobCardDetailId)
        {
            if (JobCardId <= 0)
                return 0;
            return this.dbCon.ExecuteQueryWithParams("update JobCard_Service_Mapping  set IsDeleted=1  Where  JobCardId=@1 and JobCardDetailId=@2 and IsDeleted=0", new string[2]
      {
        JobCardId.ToString(),
        JobCardDetailId.ToString()
      });
        }
        public int DeleteAllServicebyJobCard(int JobCardId)
        {
            if (JobCardId <= 0)
                return 0;
            return this.dbCon.ExecuteQueryWithParams("update JobCard_Service_Mapping  set IsDeleted=1  Where  JobCardId=@1 and IsDeleted=0", new string[1]
      {
        JobCardId.ToString()
      });
        }
        public int DeleteAllCustomerNotesByJobCard(int JobCardId)
        {
            if (JobCardId <= 0)
                return 0;
            return this.dbCon.ExecuteQueryWithParams("Delete CustomerNote  Where  JobCardId=@1", new string[1]
      {
        JobCardId.ToString()
      });
        }
        public int DeleteAllAdvisiorNotesByJobCard(int JobCardId)
        {
            if (JobCardId <= 0)
                return 0;
            return this.dbCon.ExecuteQueryWithParams("Delete AdvisiorNote  Where  JobCardId=@1", new string[1]
      {
        JobCardId.ToString()
      });
        }

        public int DeleteAllSparesbyJobCard(int JobCardId)
        {
            if (JobCardId <= 0)
                return 0;
            return this.dbCon.ExecuteQueryWithParams("update JobCard_Spare_Mapping  set IsDeleted=1  Where  JobCardId=@1 and IsDeleted=0", new string[1]
      {
        JobCardId.ToString()
      });
        }

        public int DeleteAllConsumablesbyJobCard(int JobCardId)
        {
            if (JobCardId <= 0)
                return 0;
            return this.dbCon.ExecuteQueryWithParams("update JobCard_Cosumable_Mapping  set IsDeleted=1  Where  JobCardId=@1 and IsDeleted=0", new string[1]
      {
        JobCardId.ToString()
      });
        }
        public int DeleteAllConsumablesbyJobCardDetail(int JobCardId, int JobCardDetailId)
        {
            if (JobCardId <= 0)
                return 0;
            return this.dbCon.ExecuteQueryWithParams("update JobCard_Cosumable_Mapping  set IsDeleted=1  Where  JobCardId=@1 and JobCardDetailId=@2 and IsDeleted=0", new string[2]
      {
        JobCardId.ToString(), JobCardDetailId.ToString()
      });
        }
        public int DeleteAllSparesbyJobCardDetail(int JobCardId, int JobCardDetailId)
        {
            if (JobCardId <= 0 || JobCardDetailId <= 0)
                return 0;
            return this.dbCon.ExecuteQueryWithParams("update JobCard_Spare_Mapping  set IsDeleted=1  Where  JobCardId=@1 and JobCardDetailId=@2 and IsDeleted=0", new string[2]
      {
        JobCardId.ToString(),
        JobCardDetailId.ToString()
      });
        }

        public int TotalJobCardIssuedByAdvisior(int UserId)
        {
            int num = 0;
            try
            {
                if (UserId > 0)
                {
                    DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("Select Count(Id) as TotalJobCard from JobCard where UserId=@1 and Isdelete=0 ", new string[1]
          {
            UserId.ToString()
          });
                    if (dataTableWithParams != null)
                    {
                        if (dataTableWithParams.Rows.Count > 0)
                            num = Convert.ToInt32(dataTableWithParams.Rows[0]["TotalJobCard"]);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return num;
        }

        public int TotalJobCardOpeningbyAdvisior(int UserId)
        {
            int num = 0;
            try
            {
                if (UserId > 0)
                {
                    DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("Select Count(Id) as TotalJobCard from JobCard where UserId=@1 and JobStatus_Id=1  and Isdelete=0  ", new string[1]
          {
            UserId.ToString()
          });
                    if (dataTableWithParams != null)
                    {
                        if (dataTableWithParams.Rows.Count > 0)
                            num = Convert.ToInt32(dataTableWithParams.Rows[0]["TotalJobCard"]);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return num;
        }

        public int TotalJobCardPendingbyAdvisior(int UserId)
        {
            int num = 0;
            try
            {
                if (UserId > 0)
                {
                    DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("Select Count(Id) as TotalJobCard from JobCard where UserId=@1 and JobStatus_Id in (2)  and Isdelete=0 ", new string[1]
          {
            UserId.ToString()
          });
                    if (dataTableWithParams != null)
                    {
                        if (dataTableWithParams.Rows.Count > 0)
                            num = Convert.ToInt32(dataTableWithParams.Rows[0]["TotalJobCard"]);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return num;
        }

        public int TotalJobCardRuningbyAdvisior(int UserId)
        {
            int num = 0;
            try
            {
                if (UserId > 0)
                {
                    DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("Select Count(Id) as TotalJobCard from JobCard where UserId=@1 and JobStatus_Id in (4)  and Isdelete=0 ", new string[1]
          {
            UserId.ToString()
          });
                    if (dataTableWithParams != null)
                    {
                        if (dataTableWithParams.Rows.Count > 0)
                            num = Convert.ToInt32(dataTableWithParams.Rows[0]["TotalJobCard"]);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return num;
        }
        public int TotalJobCardClosedByAdvisior(int UserId)
        {
            int num = 0;
            try
            {
                if (UserId > 0)
                {
                    DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("Select Count(Id) as TotalJobCard from JobCard where UserId=@1 and JobStatus_Id=3  and Isdelete=0  ", new string[1]
          {
            UserId.ToString()
          });
                    if (dataTableWithParams != null)
                    {
                        if (dataTableWithParams.Rows.Count > 0)
                            num = Convert.ToInt32(dataTableWithParams.Rows[0]["TotalJobCard"]);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return num;
        }

        public string GetOtpByMobieNumber(string MobileNumber = "")
        {
            string str;
            if (!string.IsNullOrEmpty(MobileNumber))
            {
                DataTable dataTable = this.dbCon.GetDataTable("select top 1 [OTP] from OTP_Verification where MobileNumber='" + MobileNumber + "' order by Id desc;");
                str = dataTable == null || dataTable.Rows.Count <= 0 ? "" : dataTable.Rows[0]["OTP"].ToString();
            }
            else
                str = "";
            return str;
        }

        public string RegistergenerateOTP()
        {
            int num = 6;
            string[] strArray = "1,2,3,4,5,6,7,8,9,0".Split(',');
            string str1 = "";
            Random random = new Random();
            for (int index = 0; index < num; ++index)
            {
                string str2 = strArray[random.Next(0, strArray.Length)];
                str1 += str2;
            }
            return str1;
        }

        public string GetAdvisiorNumberbyId(string UserId)
        {
            string result = "";
            try
            {
                string query = "Select mobile_number from Users where Id=@1";
                string[] param = { UserId };
                DataTable dt = dbCon.GetDataTableWithParams(query, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    result = dr["mobile_number"].ToString();
                }
            }
            catch (Exception ex)
            {
            }

            return result;

        }
        //JobType: 1 : Customernotes; 2 : AdvisiorNote , 3 : Problems
        // TypeId Means ProblemId Or CustomerNoteId or AdvisiorNoteId
        public int InsertJobCardDetail(int jobcardid, int Jobtype, string Text, int TypeId, int flag)
        {
            int result = 0;
            if (jobcardid > 0 && Jobtype > 0 && !String.IsNullOrEmpty(Text) && TypeId > 0)
            {
                if (Jobtype == 1 || Jobtype == 2)
                {
                    if (flag == 0)
                    {
                        string Deletequery = "Delete JobCard_Details where JobCardid=@1 and Type=@2 ";
                        string[] param1 = { jobcardid.ToString(), Jobtype.ToString() }; ;
                        int result13 = dbCon.ExecuteQueryWithParams(Deletequery, param1);
                        flag = 1;
                    }
                }

                string SelectQuery = "Select * from JobCard_Details where JobCardId=@1 and Type=@2 and Text=@4 ";
                if (Jobtype == 1 || Jobtype == 2)
                {
                    SelectQuery += "and TypeId=@3";
                }
                string[] param = { jobcardid.ToString(), Jobtype.ToString(), TypeId.ToString(), Text };

                DataTable dt = dbCon.GetDataTableWithParams(SelectQuery, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    string updatequery = "update JobCard_Details set Text=@3 ,DOM=CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME) where JobCardId=@1 and Type=@2 and TypeId-@4";
                    string[] param1 = { jobcardid.ToString(), Jobtype.ToString(), Text.Replace("'", "''"), TypeId.ToString() };
                    int executequery = dbCon.ExecuteQueryWithParams(updatequery, param1);
                    result = executequery;
                }
                else
                {
                    string Insertquery = "INSERT INTO [dbo].[JobCard_Details]([JobCardId],[Type],[TypeId] ,[IsCustomerAgreed] ,[Text],[DOC] ,[DOM]) VALUES (@1,@2,@3,@4,@5,CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME),CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME))";
                    string[] param2 = { jobcardid.ToString(), Jobtype.ToString(), TypeId.ToString(), "1", Text.Replace("'", "''") };
                    int executequery = dbCon.ExecuteQueryWithParams(Insertquery, param2);
                    result = executequery;
                }
            }
            return result;
        }



        public bool IsEstimateSendTOCustomer(int jobcardid)
        {
            bool res = false;
            if (jobcardid > 0)
            {
                string result = "Select TOP 1 Isnull(ShowToCustomer,0) as  ShowToCustomer from Estimate where JobCardId=@1 order by DOM DESC";
                string[] param = { jobcardid.ToString() };
                DataTable dt = dbCon.GetDataTableWithParams(result, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    res = Convert.ToBoolean(dr["ShowToCustomer"]);
                }
            }
            return res;
        }

        public Decimal LastEstimateSendTOCustomer(int jobcardid)
        {
            decimal res = 0;
            if (jobcardid > 0)
            {
                string result = "Select TOP 1 Isnull(EstimateTotal,0) as  EstimateTotal from Estimate where JobCardId=@1 and ShowToCustomer=1 order by DOM DESC";
                string[] param = { jobcardid.ToString() };
                DataTable dt = dbCon.GetDataTableWithParams(result, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    res = Convert.ToDecimal(dr["EstimateTotal"]);
                }
            }
            return res;
        }
        public string OutTimeForVehicle(string Id)
        {
            string result = "";
            if (!String.IsNullOrEmpty(Id))
            {
                int id = Convert.ToInt32(Id);
                if (id > 0)
                {
                    string query = "Select OutTime from Vehicle_Entry_GatePass where Id=@1";
                    string[] param = { Id };
                    DataTable dt = dbCon.GetDataTableWithParams(query, param);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            if (!String.IsNullOrEmpty(dr["OutTime"].ToString()))
                                result = dr["OutTime"].ToString();
                        }
                    }
                }
            }
            return result;
        }

        public int GetCategoryIdfromProductId(int ProductId)
        {
            int CategoryId = 0;
            if (ProductId > 0)
            {
                string query = "SELECT Isnull([CategoryId],0) as CategoryId  FROM [Motorz].[dbo].[ECommerce_Product_Category_Mapping] where ProductId=@1";
                string[] param = { ProductId.ToString() };
                DataTable dt = dbCon.GetDataTableWithParams(query, param);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow dr = dt.Rows[0];
                    CategoryId = Convert.ToInt32(dr["CategoryId"]);

                }

            }
            return CategoryId;
        }

        //HETUL


        public ServiceClass.JobCardSpareForInvoice GetJobCardSpareInJobCardForInvoiceV2_new(string JobCardId, string Spare = "", string Service = "")
        {
            ServiceClass.JobCardSpareForInvoice jobCardSpare = new ServiceClass.JobCardSpareForInvoice();
            try
            {
                Decimal num1 = new Decimal(0);
                Decimal num2 = new Decimal(0);
                int Jobcardid = Convert.ToInt32(JobCardId);
                string InvoiceId = "0";
                string[] strArrSpare = Spare.Split(new string[] { "$#@" }, StringSplitOptions.None);
                string[] strArrService = Service.Split(new string[] { "$#@" }, StringSplitOptions.None);

                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("SELECT Estimate_Spare_Mapping.Id,ISNULL(Spare.Name, '') AS Name, ISNULL(Spare.HSNNumber, '') AS HSN, Estimate_Spare_Mapping.EstimateId as InvoiceId,Estimate_Spare_Mapping.SpareId, ISNULL(Estimate_Spare_Mapping.CGSTValue, 0) AS CGSTValue, ISNULL(Estimate_Spare_Mapping.CGSTAmount, 0) AS CGSTAmount, ISNULL(Estimate_Spare_Mapping.SGSTValue, 0) AS SGSTValue, ISNULL(Estimate_Spare_Mapping.SGSTAmount, 0) AS SGSTAmount,  Estimate_Spare_Mapping.DiscountPerUnit, Estimate_Spare_Mapping.ActualAmountPerUnit, Estimate_Spare_Mapping.Quantity, Estimate.TotalServiceAmount, Estimate.TotalSpareAmount,isnull(Estimate_Spare_Mapping.TotalDiscount,0) as TotalDiscount FROM  Estimate_Spare_Mapping INNER JOIN Estimate ON Estimate.Id = Estimate_Spare_Mapping.EstimateId INNER  JOIN Spare ON Spare.Id = Estimate_Spare_Mapping.SpareId WHERE (Estimate.id in (Select top 1 id from Estimate where JobCardId=@1 order by id desc )) and Isnull(Estimate_Spare_Mapping.isDeleted,0)=0  ORDER BY Estimate_Spare_Mapping.DOC DESC ", new string[1]
        {
          JobCardId
        });
                //Spare
                for (int i = 0; i < strArrSpare.Length; i++)
                {
                    string[] strItem = strArrSpare[i].Split(',');
                    if (strItem.Length >= 2)
                    {
                        DataRow[] drdata = dataTableWithParams.Select("id=" + strItem[0]);
                        if (drdata.Length > 0)
                        {
                            double qty = 0;
                            double.TryParse(drdata[0]["Quantity"].ToString(), out qty);

                            decimal amt = 0;
                            decimal.TryParse(drdata[0]["ActualAmountPerUnit"].ToString(), out amt);

                            bool isInPer = false;
                            if (strItem[1].Contains("%"))
                            {
                                isInPer = true;
                                strItem[1] = strItem[1].Replace("%", "");
                            }
                            if (String.IsNullOrEmpty(strItem[1]))
                            {
                                strItem[1] = "0";
                            }
                            decimal dc_DiscountPerUnit = (isInPer ? ((amt * decimal.Parse(strItem[1])) / 100) : decimal.Parse(strItem[1]));
                            decimal dc_TotalDiscount = dc_DiscountPerUnit * (decimal)qty;

                            drdata[0]["DiscountPerUnit"] = dc_DiscountPerUnit;
                            drdata[0]["TotalDiscount"] = dc_TotalDiscount;
                        }
                    }
                }
                Decimal TotalCGSTAmount = 0;
                Decimal SubTotalForService = 0;
                Decimal SubTotalForSpare = 0;
                decimal d_Total_DiscountSpare_Service = 0;
                decimal TotalTaxableAmountForService = 0;
                decimal TotalTaxableAmountForSpare = 0;
                decimal TotalDiscountSpare = 0;
                decimal TotalDiscountService = 0;

                if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                {
                    ServiceClass.SparesForInvoice sparewithPrice = new ServiceClass.SparesForInvoice();
                    sparewithPrice.Name = "Spares / Consumables";
                    sparewithPrice.Type = "Header";
                    jobCardSpare.Spares.Add(sparewithPrice);
                    Decimal ActualAmountPerUnit = 0;

                    foreach (DataRow row in dataTableWithParams.Rows)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();
                        d_Total_DiscountSpare_Service += decimal.Parse(row["TotalDiscount"].ToString());
                        sparewithPrice.Id = row["SpareId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        sparewithPrice.ItemType = "Spare";
                        sparewithPrice.InvoiceSpareId = row["Id"].ToString();
                        try
                        {
                            ActualAmountPerUnit = Convert.ToDecimal(row["ActualAmountPerUnit"]);
                        }
                        catch (Exception ex)
                        { }
                        Decimal Quantity = Convert.ToDecimal(row["Quantity"]);
                        num1 += ActualAmountPerUnit * Quantity;
                        // sparewithPrice.Price = ActualAmountPerUnit+Discount.ToString();

                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        decimal discount = Math.Round(Convert.ToDecimal(row["DiscountPerUnit"]), 2);
                        if (ActualAmountPerUnit <= discount)
                        {
                            discount = ActualAmountPerUnit;
                        }
                        // discount = discount * Quantity;


                        decimal totaldis = discount * Quantity;

                        sparewithPrice.Price = (ActualAmountPerUnit).ToString();
                        TotalDiscountSpare += totaldis;
                        sparewithPrice.UnitPrice = (ActualAmountPerUnit).ToString();
                        decimal MRP = (ActualAmountPerUnit - discount) * Quantity;
                        decimal TotalTax = (2 * Convert.ToDecimal(row["SGSTValue"]));
                        //   decimal TaxAmount = (ActualAmountPerUnit - discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);
                        decimal TaxAmount = (((MRP) * (TotalTax) / (TotalTax + 100)) / 2);
                        //  decimal TaxAmount = Convert.ToDecimal(row["CGSTAmount"]) * Quantity;
                        //   decimal TaxAmount = (ActualAmountPerUnit - discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);
                        //(Total Amount ) - ((Total Amount) * GST) / GST + 100)  
                        //  decimal TaxAmount =   ((ActualAmountPerUnit - discount) * Quantity)  - (((ActualAmountPerUnit - discount) * Quantity) * (2*Convert.ToDecimal(row["SGSTValue"])) /((2*Convert.ToDecimal(row["SGSTValue"])) + 100)) ;

                        //(((ActualAmountPerUnit - discount) * Quantity) * ((Convert.ToDecimal(row["SGSTValue"])) / (100 + ((Convert.ToDecimal(row["SGSTValue"]))))));
                        sparewithPrice.Discount = discount.ToString();
                        sparewithPrice.SGSTRate = row["SGSTValue"].ToString();
                        sparewithPrice.CGSTRate = row["CGSTValue"].ToString();
                        //sparewithPrice.SGSTAmount = DisplayPrice(Convert.ToDecimal(row["SGSTAmount"].ToString())* Quantity).ToString();
                        //sparewithPrice.CGSTAmount = DisplayPrice(Convert.ToDecimal(row["CGSTAmount"].ToString()) * Quantity).ToString();
                        sparewithPrice.SGSTAmount = DisplayPrice(TaxAmount);
                        sparewithPrice.CGSTAmount = DisplayPrice(TaxAmount);
                        TotalCGSTAmount += TaxAmount;
                        decimal TotalDiscount = (totaldis);


                        //
                        decimal TaxableAmount = ((ActualAmountPerUnit - discount) * Quantity) - (((ActualAmountPerUnit - discount) * Quantity) * (2 * (Convert.ToDecimal(row["SGSTValue"])) / (100 + (2 * (Convert.ToDecimal(row["SGSTValue"]))))));//Math.Round((((ActualAmountPerUnit * Quantity) - (2 * TaxAmount) )), 2);
                        TotalTaxableAmountForSpare += TaxableAmount;
                        decimal FinalAmont = ((ActualAmountPerUnit - discount) * Quantity);//- TotalDiscount), 2);
                        //   FinalAmont = FinalAmont - discount;
                        if (TaxableAmount < 0)
                        {
                            TaxableAmount = 0;
                        }

                        if (FinalAmont < 0)
                        {
                            FinalAmont = 0;
                        }
                        sparewithPrice.TaxableAmount = DisplayPrice(TaxableAmount).ToString();
                        sparewithPrice.FinalAmount = FinalAmont.ToString();
                        decimal amount = ((ActualAmountPerUnit - discount) * Quantity);
                        //  amount = amount - discount;
                        if (amount < 0)
                        {
                            amount = 0;
                        }
                        SubTotalForSpare += amount;
                        //sparewithPrice.TaxableAmount=taxableamount.ToString();
                        sparewithPrice.HSNNumber = row["HSN"].ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        //   sparewithPrice.SubTotal = row[""].ToString();
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    if (jobCardSpare.Spares.Count > 1)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();
                        sparewithPrice.Name = "SubTotalForSpare";
                        sparewithPrice.Type = "Header";
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.SubTotal = DisplayPrice(SubTotalForSpare).ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TaxableAmount = DisplayPrice(TotalTaxableAmountForSpare);

                        sparewithPrice.TotalDiscountForSpare = DisplayPrice(TotalDiscountSpare).ToString();

                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    jobCardSpare.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                    jobCardSpare.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                    jobCardSpare.resultflag = "1";
                    jobCardSpare.Message = Constant.Message.SuccessMessage;
                }
                else
                {
                    jobCardSpare.resultflag = "0";
                    jobCardSpare.Message = Constant.Message.NoDataFound;
                }

                TotalCGSTAmount = 0;

                DataTable datatable = this.dbCon.GetDataTableWithParams("SELECT distinct Categoryid from JobCard_Service_Mapping inner join [Service] on Service.Id=JobCard_Service_Mapping.ServiceId where JobCardId=@1", new string[1] { JobCardId });

                DataTable dataTableWithParams2 = this.dbCon.GetDataTableWithParams("SELECT Estimate_Service_Mapping.Id,ISNULL(Service.Name, '') AS Name, ISNULL(Service.HSNNumber, '') AS HSN, Estimate_Service_Mapping.EstimateId as InvoiceId,Estimate_Service_Mapping.ServiceId, ISNULL(Estimate_Service_Mapping.CGSTValue, 0) AS CGSTValue, ISNULL(Estimate_Service_Mapping.CGSTAmount, 0) AS CGSTAmount, ISNULL(Estimate_Service_Mapping.SGSTValue, 0) AS SGSTValue, ISNULL(Estimate_Service_Mapping.SGSTAmount, 0) AS SGSTAmount,  Estimate_Service_Mapping.DiscountPerUnit, Estimate_Service_Mapping.ActualAmountPerUnit, Estimate_Service_Mapping.Quantity, Estimate.TotalServiceAmount, Estimate.TotalSpareAmount,isnull(Estimate_Service_Mapping.TotalDiscount,0) as TotalDiscount FROM  Estimate_Service_Mapping INNER JOIN Estimate ON Estimate.Id = Estimate_Service_Mapping.EstimateId INNER  JOIN Service ON Service.Id = Estimate_Service_Mapping.ServiceId WHERE (Estimate.id in (Select top 1 id from Estimate where JobCardId=@1 order by id desc )) and Isnull(Estimate_Service_Mapping.isDeleted,0)=0 ORDER BY Estimate_Service_Mapping.DOC DESC", new string[1]
                {
                  JobCardId
                });

                //services

                for (int i = 0; i < strArrService.Length; i++)
                {
                    string[] strItem = strArrService[i].Split(',');
                    if (strItem.Length >= 2)
                    {

                        DataRow[] drdata = dataTableWithParams2.Select("id=" + strItem[0]);
                        if (drdata.Length > 0)
                        {
                            double qty = 0;
                            double.TryParse(drdata[0]["Quantity"].ToString(), out qty);

                            decimal amt = 0;
                            decimal.TryParse(drdata[0]["ActualAmountPerUnit"].ToString(), out amt);

                            bool isInPer = false;
                            if (strItem[1].Contains("%"))
                            {
                                isInPer = true;
                                strItem[1] = strItem[1].Replace("%", "");
                            }
                            if (String.IsNullOrEmpty(strItem[1]))
                            {
                                strItem[1] = "0";
                            }
                            decimal dc_DiscountPerUnit = (isInPer ? ((amt * decimal.Parse(strItem[1])) / 100) : decimal.Parse(strItem[1]));
                            decimal dc_TotalDiscount = dc_DiscountPerUnit * (decimal)qty;

                            drdata[0]["DiscountPerUnit"] = dc_DiscountPerUnit;
                            drdata[0]["TotalDiscount"] = dc_TotalDiscount;
                        }
                    }
                }

                if (dataTableWithParams2 != null && dataTableWithParams2.Rows.Count > 0)
                {
                    Decimal ActualAmountPerUnit = 0;
                    ServiceClass.ServicesForInvoice sparewithPrice = new ServiceClass.ServicesForInvoice();
                    sparewithPrice.Type = "Header";
                    sparewithPrice.Name = "Labour";
                    jobCardSpare.Services.Add(sparewithPrice);
                    foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams2.Rows)
                    {
                        d_Total_DiscountSpare_Service += decimal.Parse(row["TotalDiscount"].ToString());
                        sparewithPrice = new ServiceClass.ServicesForInvoice();

                        sparewithPrice.Id = row["ServiceId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        sparewithPrice.ItemType = "Service";
                        sparewithPrice.InvoiceServiceId = row["Id"].ToString();
                        try
                        {
                            ActualAmountPerUnit = Convert.ToDecimal(row["ActualAmountPerUnit"]);
                        }
                        catch (Exception ex)
                        { }
                        Decimal Quantity = Convert.ToDecimal(row["Quantity"]);

                        sparewithPrice.Price = ActualAmountPerUnit.ToString();
                        sparewithPrice.HSNNumber = "998714";
                        // sparewithPrice.UnitPrice = (num3).ToString();
                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        decimal discount = Math.Round(Convert.ToDecimal(row["DiscountPerUnit"]), 2);

                        if (ActualAmountPerUnit <= discount)
                        {
                            discount = ActualAmountPerUnit;
                        }


                        sparewithPrice.Discount = (discount).ToString();
                        //  ActualAmountPerUnit = ActualAmountPerUnit ;
                        TotalDiscountService += (discount * Quantity);
                        sparewithPrice.UnitPrice = (ActualAmountPerUnit).ToString();
                        //  ActualAmountPerUnit = ActualAmountPerUnit + discount;
                        //decimal TotalDiscount = (discount );
                        decimal TotalDiscount = (discount * Quantity);
                        decimal MRP = (ActualAmountPerUnit - discount) * Quantity;
                        decimal TotalTax = (2 * Convert.ToDecimal(row["SGSTValue"]));


                        decimal TaxAmount = ((MRP) * (((TotalTax) / 2) / 100));
                        num1 += ActualAmountPerUnit * Quantity + (2 * TaxAmount);
                        //   decimal TaxAmount = (ActualAmountPerUnit - discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);
                        //   decimal TaxAmount = (((MRP) * (TotalTax) / (TotalTax + 100))/2);
                        //  decimal TaxAmount = ((ActualAmountPerUnit - discount) * Quantity) - (((ActualAmountPerUnit - discount) * Quantity) * (2 * Convert.ToDecimal(row["SGSTValue"])) / ((2 * Convert.ToDecimal(row["SGSTValue"])) + 100));
                        // decimal TaxAmount = ((ActualAmountPerUnit - discount) * Quantity)  - (((ActualAmountPerUnit - discount) * Quantity) * Convert.ToDecimal(row["SGSTValue"]) /(Convert.ToDecimal(row["SGSTValue"]) + 100)) ;//(((ActualAmountPerUnit - discount) * Quantity) * ( (Convert.ToDecimal(row["SGSTValue"])) / (100 + ( (Convert.ToDecimal(row["SGSTValue"]))))));
                        //  decimal TaxAmount = (ActualAmountPerUnit-discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);

                        //decimal TaxAmount = Convert.ToDecimal(row["CGSTAmount"]);
                        decimal TaxableAmount = ((ActualAmountPerUnit - discount) * Quantity);
                        TotalTaxableAmountForService += TaxableAmount;
                        decimal FinalAmont = (((ActualAmountPerUnit - discount) * Quantity) + (2 * TaxAmount));
                        if (TaxableAmount < 0)
                        {
                            TaxableAmount = 0;
                        }

                        if (FinalAmont < 0)
                        {
                            FinalAmont = 0;
                        }
                        //sparewithPrice.Discount = row["Discount"];
                        sparewithPrice.SGSTRate = row["SGSTValue"].ToString();
                        sparewithPrice.CGSTRate = row["CGSTValue"].ToString();
                        //sparewithPrice.SGSTAmount = DisplayPrice(Convert.ToDecimal(row["SGSTAmount"].ToString())).ToString();
                        //sparewithPrice.CGSTAmount = DisplayPrice(Convert.ToDecimal(row["CGSTAmount"].ToString())).ToString();
                        sparewithPrice.SGSTAmount = DisplayPrice(TaxAmount);
                        sparewithPrice.CGSTAmount = DisplayPrice(TaxAmount);
                        //sparewithPrice.SGSTAmount = row["SGSTAmount"].ToString();
                        //sparewithPrice.CGSTAmount = row["CGSTAmount"].ToString();

                        TotalCGSTAmount += TaxAmount;
                        sparewithPrice.TaxableAmount = DisplayPrice(TaxableAmount).ToString();
                        sparewithPrice.FinalAmount = DisplayPrice(FinalAmont).ToString();
                        SubTotalForService += FinalAmont;
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        jobCardSpare.Services.Add(sparewithPrice);
                    }
                    if (jobCardSpare.Spares.Count > 1)
                    {
                        sparewithPrice = new ServiceClass.ServicesForInvoice();
                        sparewithPrice.Name = "SubTotalForService";
                        sparewithPrice.Type = "Header";
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.SubTotal = DisplayPrice(SubTotalForService).ToString();
                        // sparewithPrice.TotalDiscountForSpare = TotalDiscountSpare.ToString();
                        sparewithPrice.TotalDiscountForService = DisplayPrice(TotalDiscountService).ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TaxableAmount = DisplayPrice(TotalTaxableAmountForService).ToString();
                        jobCardSpare.Services.Add(sparewithPrice);
                    }
                }
                //dbCon.InsertLogs("JobCard Spare COunt :: " + jobCardSpare.Spares.Count);
                jobCardSpare.resultflag = "1";
                jobCardSpare.Message = Constant.Message.SuccessMessage;

                //        }
                //    }
                //}
                //else
                //{
                //    jobCardSpare.resultflag = "0";
                //    jobCardSpare.Message = Constant.Message.NoDataFound;
                //}


                jobCardSpare.TotalBeforeTaxService = TotalTaxableAmountForService.ToString();
                jobCardSpare.TotalBeforeTaxSpare = TotalTaxableAmountForSpare.ToString();
                jobCardSpare.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                jobCardSpare.SubTotalForService = DisplayPrice(SubTotalForService);
                jobCardSpare.SubTotalForSpare = DisplayPrice(SubTotalForSpare);
                jobCardSpare.TotalAmount = DisplayPrice(SubTotalForService + SubTotalForSpare);
                // - d_Total_DiscountSpare_Service
                jobCardSpare.FinalAmount = DisplayPrice(SubTotalForService + SubTotalForSpare);
                return jobCardSpare;
            }
            catch (Exception ex)
            {
                dbCon.InsertLogs("GetJobCardSpareInJobCardForInvoice : " + ex.ToString(), ex.StackTrace.ToString(), ex.Message.ToString());
                return new ServiceClass.JobCardSpareForInvoice();
            }
        }


        public ServiceClass.JobCardSpareForInvoice GetJobCardSpareInJobCardForInvoiceV3_new(string JobCardId, string Spare = "", string Service = "", int type = 1, bool fromEstimate = false, bool ShowAllocated = false)
        {
            ServiceClass.JobCardSpareForInvoice jobCardSpare = new ServiceClass.JobCardSpareForInvoice();
            try
            {
                Decimal num1 = new Decimal(0);
                Decimal num2 = new Decimal(0);
                int Jobcardid = Convert.ToInt32(JobCardId);
                string InvoiceId = "0";
                string[] strArrSpare = Spare.Split(new string[] { "$#@" }, StringSplitOptions.None);
                string[] strArrService = Service.Split(new string[] { "$#@" }, StringSplitOptions.None);

                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("SELECT Estimate_Spare_Mapping.Id,ISNULL(Spare.Name, '') AS Name, ISNULL(Spare.HSNNumber, '') AS HSN, Estimate_Spare_Mapping.EstimateId as InvoiceId,Estimate_Spare_Mapping.SpareId, ISNULL(Estimate_Spare_Mapping.CGSTValue, 0) AS CGSTValue, ISNULL(Estimate_Spare_Mapping.CGSTAmount, 0) AS CGSTAmount, ISNULL(Estimate_Spare_Mapping.SGSTValue, 0) AS SGSTValue, ISNULL(Estimate_Spare_Mapping.SGSTAmount, 0) AS SGSTAmount," + (type == 1 && !fromEstimate ? " Estimate_Spare_Mapping.DiscountPerUnit " : " (Estimate_Spare_Mapping.ActualAmountPerUnit-isnull(Estimate_Spare_Mapping.InsuranceAmount,0)) ") + " as DiscountPerUnit, " + (type == 1 && !fromEstimate ? " (Estimate_Spare_Mapping.ActualAmountPerUnit-Estimate_Spare_Mapping.InsuranceAmount) as ActualAmountPerUnit " : "  Estimate_Spare_Mapping.ActualAmountPerUnit as ActualAmountPerUnit ") + ", Estimate_Spare_Mapping.Quantity, Estimate.TotalServiceAmount, Estimate.TotalSpareAmount," + (type == 1 && !fromEstimate ? "isnull(Estimate_Spare_Mapping.TotalDiscount,0)" : " ((isnull(Estimate_Spare_Mapping.ActualAmountPerUnit-Estimate_Spare_Mapping.InsuranceAmount,0))*Estimate_Spare_Mapping.Quantity) ") + " as TotalDiscount,Convert(int,isnull(Estimate_Spare_Mapping.PushToInsurance,0)) as PushToInsurance ,isnull(Estimate_Spare_Mapping.InsuranceAmount,0) as InsuranceAmount,Estimate_Spare_Mapping.[MappingId] as MapId FROM  Estimate_Spare_Mapping INNER JOIN Estimate ON Estimate.Id = Estimate_Spare_Mapping.EstimateId INNER  JOIN Spare ON Spare.Id = Estimate_Spare_Mapping.SpareId WHERE (Estimate.id in (Select top 1 id from Estimate where JobCardId=@1 order by id desc )) and Isnull(Estimate_Spare_Mapping.isDeleted,0)=0 " + (type == 2 ? " And isnull(Estimate_Spare_Mapping.PushToInsurance,0)=1 " : " ") + (ShowAllocated ? " And ( (isnull((Select JobCard_Spare_Mapping.AllowWithoutAllocation from JobCard_Spare_Mapping where id=Estimate_Spare_Mapping.MappingId),0)=1) or (Estimate_Spare_Mapping.MappingId in (Select Requisition_Spare.SpareMappingId from Requisition_Spare where Requisition_Spare.IsAllocate=1) or Estimate_Spare_Mapping.SpareId in (Select spareid from AllowedPartsToGenerateInvoiceWithoutAllocation where isnull(IsDeleted,0)=0) ) or Estimateid in (Select id from estimate where Jobcardid in (Select id from jobcard where id=@1 and jobcard.DOC<'03-Jul-2018 00:00:00' and JobCard.IsGatePassGenerated=1))) " : " ") + "   ORDER BY Estimate_Spare_Mapping.DOC DESC ", new string[1]
        {
          JobCardId
        });
                //Spare
                for (int i = 0; i < strArrSpare.Length; i++)
                {
                    string[] strItem = strArrSpare[i].Split(',');
                    if (strItem.Length >= 2)
                    {
                        DataRow[] drdata = dataTableWithParams.Select("id=" + strItem[0]);
                        if (drdata.Length > 0)
                        {
                            double qty = 0;
                            double.TryParse(drdata[0]["Quantity"].ToString(), out qty);

                            decimal amt = 0;
                            decimal.TryParse(drdata[0]["ActualAmountPerUnit"].ToString(), out amt);

                            bool isInPer = false;
                            if (strItem[1].Contains("%"))
                            {
                                isInPer = true;
                                strItem[1] = strItem[1].Replace("%", "");
                            }
                            if (String.IsNullOrEmpty(strItem[1]))
                            {
                                strItem[1] = "0";
                            }
                            decimal dc_DiscountPerUnit = (isInPer ? ((amt * decimal.Parse(strItem[1])) / 100) : decimal.Parse(strItem[1]));
                            decimal dc_TotalDiscount = dc_DiscountPerUnit * (decimal)qty;

                            drdata[0]["DiscountPerUnit"] = dc_DiscountPerUnit;
                            drdata[0]["TotalDiscount"] = dc_TotalDiscount;
                        }
                    }
                }
                Decimal TotalCGSTAmount = 0;
                Decimal FinalTotalCGSTAmount = 0;
                Decimal SubTotalForService = 0;
                Decimal SubTotalForSpare = 0;
                decimal d_Total_DiscountSpare_Service = 0;
                decimal TotalTaxableAmountForService = 0;
                decimal TotalTaxableAmountForSpare = 0;
                decimal TotalDiscountSpare = 0;
                decimal TotalDiscountService = 0;

                if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                {
                    ServiceClass.SparesForInvoice sparewithPrice = new ServiceClass.SparesForInvoice();
                    sparewithPrice.Name = "Spares / Consumables";
                    sparewithPrice.Type = "Header";
                    jobCardSpare.Spares.Add(sparewithPrice);
                    Decimal ActualAmountPerUnit = 0;

                    foreach (DataRow row in dataTableWithParams.Rows)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();
                        d_Total_DiscountSpare_Service += decimal.Parse(row["TotalDiscount"].ToString());
                        sparewithPrice.Id = row["SpareId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        sparewithPrice.MappingId = row["MapId"].ToString();
                        sparewithPrice.ItemType = "Spare";
                        sparewithPrice.InvoiceSpareId = row["Id"].ToString();
                        try
                        {
                            ActualAmountPerUnit = Convert.ToDecimal(row["ActualAmountPerUnit"]);
                        }
                        catch (Exception ex)
                        { }
                        Decimal Quantity = Convert.ToDecimal(row["Quantity"]);
                        num1 += ActualAmountPerUnit * Quantity;
                        // sparewithPrice.Price = ActualAmountPerUnit+Discount.ToString();

                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        decimal discount = Math.Round(Convert.ToDecimal(row["DiscountPerUnit"]), 2);
                        if (ActualAmountPerUnit <= discount)
                        {
                            discount = ActualAmountPerUnit;
                        }
                        // discount = discount * Quantity;


                        decimal totaldis = discount * Quantity;

                        sparewithPrice.Price = (ActualAmountPerUnit).ToString();
                        TotalDiscountSpare += totaldis;
                        sparewithPrice.UnitPrice = (ActualAmountPerUnit).ToString();
                        decimal MRP = (ActualAmountPerUnit - discount) * Quantity;
                        decimal TotalTax = (2 * Convert.ToDecimal(row["SGSTValue"]));
                        //   decimal TaxAmount = (ActualAmountPerUnit - discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);
                        decimal TaxAmount = (((MRP) * (TotalTax) / (TotalTax + 100)) / 2);
                        //  decimal TaxAmount = Convert.ToDecimal(row["CGSTAmount"]) * Quantity;
                        //   decimal TaxAmount = (ActualAmountPerUnit - discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);
                        //(Total Amount ) - ((Total Amount) * GST) / GST + 100)  
                        //  decimal TaxAmount =   ((ActualAmountPerUnit - discount) * Quantity)  - (((ActualAmountPerUnit - discount) * Quantity) * (2*Convert.ToDecimal(row["SGSTValue"])) /((2*Convert.ToDecimal(row["SGSTValue"])) + 100)) ;

                        //(((ActualAmountPerUnit - discount) * Quantity) * ((Convert.ToDecimal(row["SGSTValue"])) / (100 + ((Convert.ToDecimal(row["SGSTValue"]))))));
                        sparewithPrice.Discount = discount.ToString();
                        sparewithPrice.SGSTRate = row["SGSTValue"].ToString();
                        sparewithPrice.CGSTRate = row["CGSTValue"].ToString();
                        //sparewithPrice.SGSTAmount = DisplayPrice(Convert.ToDecimal(row["SGSTAmount"].ToString())* Quantity).ToString();
                        //sparewithPrice.CGSTAmount = DisplayPrice(Convert.ToDecimal(row["CGSTAmount"].ToString()) * Quantity).ToString();
                        sparewithPrice.SGSTAmount = DisplayPrice(TaxAmount);
                        sparewithPrice.CGSTAmount = DisplayPrice(TaxAmount);
                        TotalCGSTAmount += TaxAmount;
                        decimal TotalDiscount = (totaldis);


                        //
                        decimal TaxableAmount = ((ActualAmountPerUnit - discount) * Quantity) - (((ActualAmountPerUnit - discount) * Quantity) * (2 * (Convert.ToDecimal(row["SGSTValue"])) / (100 + (2 * (Convert.ToDecimal(row["SGSTValue"]))))));//Math.Round((((ActualAmountPerUnit * Quantity) - (2 * TaxAmount) )), 2);
                        TotalTaxableAmountForSpare += TaxableAmount;
                        decimal FinalAmont = ((ActualAmountPerUnit - discount) * Quantity);//- TotalDiscount), 2);
                        //   FinalAmont = FinalAmont - discount;
                        if (TaxableAmount < 0)
                        {
                            TaxableAmount = 0;
                        }

                        if (FinalAmont < 0)
                        {
                            FinalAmont = 0;
                        }
                        sparewithPrice.TaxableAmount = DisplayPrice(TaxableAmount).ToString();
                        sparewithPrice.FinalAmount = FinalAmont.ToString();
                        decimal amount = ((ActualAmountPerUnit - discount) * Quantity);
                        //  amount = amount - discount;
                        if (amount < 0)
                        {
                            amount = 0;
                        }
                        SubTotalForSpare += amount;
                        //sparewithPrice.TaxableAmount=taxableamount.ToString();
                        sparewithPrice.HSNNumber = row["HSN"].ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        //   sparewithPrice.SubTotal = row[""].ToString();
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    if (jobCardSpare.Spares.Count > 1)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();
                        sparewithPrice.Name = "SubTotalForSpare";
                        sparewithPrice.Type = "Header";
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.SubTotal = DisplayPrice(SubTotalForSpare).ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TaxableAmount = DisplayPrice(TotalTaxableAmountForSpare);

                        sparewithPrice.TotalDiscountForSpare = DisplayPrice(TotalDiscountSpare).ToString();

                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    jobCardSpare.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                    jobCardSpare.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                    jobCardSpare.resultflag = "1";
                    jobCardSpare.Message = Constant.Message.SuccessMessage;
                }
                else
                {
                    jobCardSpare.resultflag = "0";
                    jobCardSpare.Message = Constant.Message.NoDataFound;
                }
                FinalTotalCGSTAmount = TotalCGSTAmount;
                TotalCGSTAmount = 0;
                //Step 1
                DataTable datatable = this.dbCon.GetDataTableWithParams("SELECT distinct Categoryid from JobCard_Service_Mapping inner join [Service] on Service.Id=JobCard_Service_Mapping.ServiceId where JobCardId=@1", new string[1] { JobCardId });

                DataTable dataTableWithParams2 = this.dbCon.GetDataTableWithParams("SELECT Estimate_Service_Mapping.Id,ISNULL(Service.Name, '') AS Name, ISNULL(Service.HSNNumber, '') AS HSN, Estimate_Service_Mapping.EstimateId as InvoiceId,Estimate_Service_Mapping.ServiceId, ISNULL(Estimate_Service_Mapping.CGSTValue, 0) AS CGSTValue, ISNULL(Estimate_Service_Mapping.CGSTAmount, 0) AS CGSTAmount, ISNULL(Estimate_Service_Mapping.SGSTValue, 0) AS SGSTValue, ISNULL(Estimate_Service_Mapping.SGSTAmount, 0) AS SGSTAmount,  " + (type == 1 && !fromEstimate ? " Estimate_Service_Mapping.DiscountPerUnit " : " (Estimate_Service_Mapping.ActualAmountPerUnit-isnull(Estimate_Service_Mapping.InsuranceAmount,0)) ") + " as DiscountPerUnit, " + (type == 1 && !fromEstimate ? " (Estimate_Service_Mapping.ActualAmountPerUnit-Estimate_Service_Mapping.InsuranceAmount) as ActualAmountPerUnit " : "  Estimate_Service_Mapping.ActualAmountPerUnit as ActualAmountPerUnit ") + ", Estimate_Service_Mapping.Quantity, Estimate.TotalServiceAmount, Estimate.TotalSpareAmount," + (type == 1 && !fromEstimate ? "isnull(Estimate_Service_Mapping.TotalDiscount,0)" : " ((Estimate_Service_Mapping.ActualAmountPerUnit-isnull(Estimate_Service_Mapping.InsuranceAmount,0))*Estimate_Service_Mapping.Quantity) ") + " as TotalDiscount,Estimate_Service_Mapping.[MappingId] as MapId FROM  Estimate_Service_Mapping INNER JOIN Estimate ON Estimate.Id = Estimate_Service_Mapping.EstimateId INNER  JOIN Service ON Service.Id = Estimate_Service_Mapping.ServiceId WHERE (Estimate.id in (Select top 1 id from Estimate where JobCardId=@1 order by id desc )) and Isnull(Estimate_Service_Mapping.isDeleted,0)=0  " + (type == 2 ? " And isnull(Estimate_Service_Mapping.PushToInsurance,0)=1 " : " ") + "   ORDER BY Estimate_Service_Mapping.DOC DESC", new string[1]
                {
                  JobCardId
                });

                //services

                for (int i = 0; i < strArrService.Length; i++)
                {
                    string[] strItem = strArrService[i].Split(',');
                    if (strItem.Length >= 2)
                    {

                        DataRow[] drdata = dataTableWithParams2.Select("id=" + strItem[0]);
                        if (drdata.Length > 0)
                        {
                            double qty = 0;
                            double.TryParse(drdata[0]["Quantity"].ToString(), out qty);

                            decimal amt = 0;
                            decimal.TryParse(drdata[0]["ActualAmountPerUnit"].ToString(), out amt);

                            bool isInPer = false;
                            if (strItem[1].Contains("%"))
                            {
                                isInPer = true;
                                strItem[1] = strItem[1].Replace("%", "");
                            }
                            if (String.IsNullOrEmpty(strItem[1]))
                            {
                                strItem[1] = "0";
                            }
                            decimal dc_DiscountPerUnit = (isInPer ? ((amt * decimal.Parse(strItem[1])) / 100) : decimal.Parse(strItem[1]));
                            decimal dc_TotalDiscount = dc_DiscountPerUnit * (decimal)qty;

                            drdata[0]["DiscountPerUnit"] = dc_DiscountPerUnit;
                            drdata[0]["TotalDiscount"] = dc_TotalDiscount;
                        }
                    }
                }

                if (dataTableWithParams2 != null && dataTableWithParams2.Rows.Count > 0)
                {
                    Decimal ActualAmountPerUnit = 0;
                    ServiceClass.ServicesForInvoice sparewithPrice = new ServiceClass.ServicesForInvoice();
                    sparewithPrice.Type = "Header";
                    sparewithPrice.Name = "Labour";
                    jobCardSpare.Services.Add(sparewithPrice);
                    foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams2.Rows)
                    {
                        d_Total_DiscountSpare_Service += decimal.Parse(row["TotalDiscount"].ToString());
                        sparewithPrice = new ServiceClass.ServicesForInvoice();

                        sparewithPrice.Id = row["ServiceId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        sparewithPrice.MappingId = row["MapId"].ToString();
                        sparewithPrice.ItemType = "Service";
                        sparewithPrice.InvoiceServiceId = row["Id"].ToString();
                        try
                        {
                            ActualAmountPerUnit = Convert.ToDecimal(row["ActualAmountPerUnit"]);
                        }
                        catch (Exception ex)
                        { }
                        Decimal Quantity = Convert.ToDecimal(row["Quantity"]);

                        sparewithPrice.Price = ActualAmountPerUnit.ToString();
                        sparewithPrice.HSNNumber = "998714";
                        // sparewithPrice.UnitPrice = (num3).ToString();
                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        decimal discount = Math.Round(Convert.ToDecimal(row["DiscountPerUnit"]), 2);

                        if (ActualAmountPerUnit <= discount)
                        {
                            discount = ActualAmountPerUnit;
                        }


                        sparewithPrice.Discount = (discount).ToString();
                        //  ActualAmountPerUnit = ActualAmountPerUnit ;
                        TotalDiscountService += (discount * Quantity);
                        sparewithPrice.UnitPrice = (ActualAmountPerUnit).ToString();
                        //  ActualAmountPerUnit = ActualAmountPerUnit + discount;
                        //decimal TotalDiscount = (discount );
                        decimal TotalDiscount = (discount * Quantity);
                        decimal MRP = (ActualAmountPerUnit - discount) * Quantity;
                        decimal TotalTax = (2 * Convert.ToDecimal(row["SGSTValue"]));


                        decimal TaxAmount = ((MRP) * (((TotalTax) / 2) / 100));
                        num1 += ActualAmountPerUnit * Quantity + (2 * TaxAmount);
                        //   decimal TaxAmount = (ActualAmountPerUnit - discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);
                        //   decimal TaxAmount = (((MRP) * (TotalTax) / (TotalTax + 100))/2);
                        //  decimal TaxAmount = ((ActualAmountPerUnit - discount) * Quantity) - (((ActualAmountPerUnit - discount) * Quantity) * (2 * Convert.ToDecimal(row["SGSTValue"])) / ((2 * Convert.ToDecimal(row["SGSTValue"])) + 100));
                        // decimal TaxAmount = ((ActualAmountPerUnit - discount) * Quantity)  - (((ActualAmountPerUnit - discount) * Quantity) * Convert.ToDecimal(row["SGSTValue"]) /(Convert.ToDecimal(row["SGSTValue"]) + 100)) ;//(((ActualAmountPerUnit - discount) * Quantity) * ( (Convert.ToDecimal(row["SGSTValue"])) / (100 + ( (Convert.ToDecimal(row["SGSTValue"]))))));
                        //  decimal TaxAmount = (ActualAmountPerUnit-discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);

                        //decimal TaxAmount = Convert.ToDecimal(row["CGSTAmount"]);
                        decimal TaxableAmount = ((ActualAmountPerUnit - discount) * Quantity);
                        TotalTaxableAmountForService += TaxableAmount;
                        decimal FinalAmont = (((ActualAmountPerUnit - discount) * Quantity) + (2 * TaxAmount));
                        if (TaxableAmount < 0)
                        {
                            TaxableAmount = 0;
                        }

                        if (FinalAmont < 0)
                        {
                            FinalAmont = 0;
                        }
                        //sparewithPrice.Discount = row["Discount"];
                        sparewithPrice.SGSTRate = row["SGSTValue"].ToString();
                        sparewithPrice.CGSTRate = row["CGSTValue"].ToString();
                        //sparewithPrice.SGSTAmount = DisplayPrice(Convert.ToDecimal(row["SGSTAmount"].ToString())).ToString();
                        //sparewithPrice.CGSTAmount = DisplayPrice(Convert.ToDecimal(row["CGSTAmount"].ToString())).ToString();
                        sparewithPrice.SGSTAmount = DisplayPrice(TaxAmount);
                        sparewithPrice.CGSTAmount = DisplayPrice(TaxAmount);
                        //sparewithPrice.SGSTAmount = row["SGSTAmount"].ToString();
                        //sparewithPrice.CGSTAmount = row["CGSTAmount"].ToString();

                        TotalCGSTAmount += TaxAmount;
                        sparewithPrice.TaxableAmount = DisplayPrice(TaxableAmount).ToString();
                        sparewithPrice.FinalAmount = DisplayPrice(FinalAmont).ToString();
                        SubTotalForService += FinalAmont;
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        jobCardSpare.Services.Add(sparewithPrice);
                    }
                    if (jobCardSpare.Services.Count > 1)
                    {
                        sparewithPrice = new ServiceClass.ServicesForInvoice();
                        sparewithPrice.Name = "SubTotalForService";
                        sparewithPrice.Type = "Header";
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.SubTotal = DisplayPrice(SubTotalForService).ToString();
                        // sparewithPrice.TotalDiscountForSpare = TotalDiscountSpare.ToString();
                        sparewithPrice.TotalDiscountForService = DisplayPrice(TotalDiscountService).ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TaxableAmount = DisplayPrice(TotalTaxableAmountForService).ToString();
                        jobCardSpare.Services.Add(sparewithPrice);
                    }
                }

                //Step 2
                FinalTotalCGSTAmount += TotalCGSTAmount;
                jobCardSpare.FinalTotalCGSTAmount = FinalTotalCGSTAmount.ToString();
              //  dbCon.InsertLogs("JobCard Spare COunt :: " + jobCardSpare.Spares.Count);
                jobCardSpare.resultflag = "1";
                jobCardSpare.Message = Constant.Message.SuccessMessage;

                //        }
                //    }
                //}
                //else
                //{
                //    jobCardSpare.resultflag = "0";
                //    jobCardSpare.Message = Constant.Message.NoDataFound;
                //}


                jobCardSpare.TotalBeforeTaxService = TotalTaxableAmountForService.ToString();
                jobCardSpare.TotalBeforeTaxSpare = TotalTaxableAmountForSpare.ToString();
                jobCardSpare.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                jobCardSpare.SubTotalForService = DisplayPrice(SubTotalForService);
                jobCardSpare.SubTotalForSpare = DisplayPrice(SubTotalForSpare);
                jobCardSpare.TotalAmount = DisplayPrice(SubTotalForService + SubTotalForSpare);
                // - d_Total_DiscountSpare_Service
                jobCardSpare.FinalAmount = DisplayPrice(SubTotalForService + SubTotalForSpare);
                return jobCardSpare;
            }
            catch (Exception ex)
            {
                dbCon.InsertLogs("GetJobCardSpareInJobCardForInvoice : " + ex.ToString(), ex.StackTrace.ToString(), ex.Message.ToString());
                return new ServiceClass.JobCardSpareForInvoice();
            }
        }

        //Main Invoice
        public ServiceClass.JobCardSpareForInvoice GetJobCardSpareInJobCardForInvoiceV3_new_V1(string JobCardId, string Spare = "", string Service = "", int type = 1, bool fromEstimate = false, bool ShowAllocated = false, int GenerateGstInvoiceNumber=0)
        {
            ServiceClass.JobCardSpareForInvoice jobCardSpare = new ServiceClass.JobCardSpareForInvoice();
            try
            {
                Decimal num1 = new Decimal(0);
                Decimal num2 = new Decimal(0);
                int Jobcardid = Convert.ToInt32(JobCardId);
                string InvoiceId = "0";
                string[] strArrSpare = Spare.Split(new string[] { "$#@" }, StringSplitOptions.None);
                string[] strArrService = Service.Split(new string[] { "$#@" }, StringSplitOptions.None);
                if (GenerateGstInvoiceNumber == 1)
                {
                    //clsDataSourse ClsDataSourse = new clsDataSourse();
                    //dbCon.ExecuteQuery("UPDATE [dbo].[Invoice] SET [GstInvoiceNumber] ='" + ClsDataSourse.GetGstInvoiceNumber() + "'  WHERE isnull(IsCancelled,0)=0 and InvoiceNumber is not null and GstInvoiceNumber is null and JobCardid=" + Jobcardid + " and  type=" + type);
                }
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("SELECT GstInvoiceNumber,Invoice_Spare_Mapping.Id,ISNULL(Spare.Name, '') AS Name, ISNULL(Spare.HSNNumber, '') AS HSN, Estimate.Id as InvoiceId,Invoice_Spare_Mapping.SpareId, ISNULL(Invoice_Spare_Mapping.CGSTValue, 0) AS CGSTValue, ISNULL(Invoice_Spare_Mapping.CGSTAmount, 0) AS CGSTAmount, ISNULL(Invoice_Spare_Mapping.SGSTValue, 0) AS SGSTValue, ISNULL(Invoice_Spare_Mapping.SGSTAmount, 0) AS SGSTAmount, " + (type == 2 ? "isnull((Select  ISM.ActualAmountPerUnit from Invoice_Spare_Mapping as ISM where ISM.Id=Invoice_Spare_Mapping.RefId),0)" : "isnull(Invoice_Spare_Mapping.TotalDiscount,0)") + "  as DiscountPerUnit, " + (type == 2 ? "isnull((Select  ISM.ActualAmountPerUnit-(((ISM.ActualAmountPerUnit*(ISM.CGSTValue*2))/(100+(ISM.CGSTValue*2)))) from Invoice_Spare_Mapping as ISM where ISM.Id=Invoice_Spare_Mapping.RefId),0)" : "isnull(Invoice_Spare_Mapping.TotalDiscount,0)") + "  as DiscountPerUnitTaxable, " + (type == 2 ? "iif(Invoice_Spare_Mapping.RefId is not null,(Select ISM.Mrp from Invoice_Spare_Mapping as ISM where ISM.Id=Invoice_Spare_Mapping.RefId),Invoice_Spare_Mapping.ActualAmountPerUnit)" : " Invoice_Spare_Mapping.ActualAmountPerUnit ") + " as ActualAmountPerUnit , Invoice_Spare_Mapping.Quantity, Estimate.TotalServiceAmount, Estimate.TotalSpareAmount, isnull(Invoice_Spare_Mapping.TotalDiscount,0) as TotalDiscount,0 as PushToInsurance ,0 as InsuranceAmount,Invoice_Spare_Mapping.[MappingId] as MapId FROM  Invoice_Spare_Mapping as Invoice_Spare_Mapping  INNER JOIN Invoice as Estimate ON Estimate.Id = Invoice_Spare_Mapping.InvoiceId INNER  JOIN Spare ON Spare.Id = Invoice_Spare_Mapping.SpareId WHERE Estimate.JobCardId=@1 and Estimate.Type=" + type + " and GstInvoiceNumber is not null and isnull(Estimate.IsCancelled,0)=0 and (Invoice_Spare_Mapping.ActualAmountPerUnit>0 or isnull(Invoice_Spare_Mapping.IsMrpChanged,0)=0)  ORDER BY Invoice_Spare_Mapping.DOC DESC  ", new string[1]
        {
          JobCardId
        });
                //Spare
                for (int i = 0; i < strArrSpare.Length; i++)
                {
                    string[] strItem = strArrSpare[i].Split(',');
                    if (strItem.Length >= 2)
                    {
                        DataRow[] drdata = dataTableWithParams.Select("id=" + strItem[0]);
                        if (drdata.Length > 0)
                        {
                            double qty = 0;
                            double.TryParse(drdata[0]["Quantity"].ToString(), out qty);

                            decimal amt = 0;
                            decimal.TryParse(drdata[0]["ActualAmountPerUnit"].ToString(), out amt);

                            bool isInPer = false;
                            if (strItem[1].Contains("%"))
                            {
                                isInPer = true;
                                strItem[1] = strItem[1].Replace("%", "");
                            }
                            if (String.IsNullOrEmpty(strItem[1]))
                            {
                                strItem[1] = "0";
                            }
                            decimal dc_DiscountPerUnit = (isInPer ? ((amt * decimal.Parse(strItem[1])) / 100) : decimal.Parse(strItem[1]));
                            decimal dc_TotalDiscount = dc_DiscountPerUnit * (decimal)qty;

                            drdata[0]["DiscountPerUnit"] = dc_DiscountPerUnit;
                            drdata[0]["TotalDiscount"] = dc_TotalDiscount;
                        }
                    }
                }
                Decimal TotalCGSTAmount = 0;
                Decimal FinalTotalCGSTAmount = 0;
                Decimal SubTotalForService = 0;
                Decimal SubTotalForSpare = 0;
                decimal d_Total_DiscountSpare_Service = 0;
                decimal TotalTaxableAmountForService = 0;
                decimal TotalTaxableAmountForSpare = 0;
                decimal TotalDiscountSpare = 0;
                decimal TotalDiscountService = 0;

                if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                {
                    ServiceClass.SparesForInvoice sparewithPrice = new ServiceClass.SparesForInvoice();
                    sparewithPrice.Name = "Spares / Consumables";
                    sparewithPrice.Type = "Header";
                    jobCardSpare.Spares.Add(sparewithPrice);
                    Decimal ActualAmountPerUnit = 0;

                    foreach (DataRow row in dataTableWithParams.Rows)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();
                        d_Total_DiscountSpare_Service += decimal.Parse(row["TotalDiscount"].ToString());
                        sparewithPrice.Id = row["SpareId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        sparewithPrice.MappingId = row["MapId"].ToString();
                        sparewithPrice.ItemType = "Spare";
                        sparewithPrice.InvoiceSpareId = row["Id"].ToString();
                        try
                        {
                            ActualAmountPerUnit = Convert.ToDecimal(row["ActualAmountPerUnit"]);
                        }
                        catch (Exception ex)
                        { }
                        Decimal Quantity = Convert.ToDecimal(row["Quantity"]);
                        num1 += ActualAmountPerUnit * Quantity;
                        // sparewithPrice.Price = ActualAmountPerUnit+Discount.ToString();

                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        decimal discount = Math.Round(Convert.ToDecimal(row["DiscountPerUnit"]), 2);
                        if (ActualAmountPerUnit <= discount)
                        {
                            discount = ActualAmountPerUnit;
                        }
                        // discount = discount * Quantity;


                        decimal totaldis = discount * Quantity;

                        sparewithPrice.Price = (ActualAmountPerUnit).ToString();
                        TotalDiscountSpare += totaldis;
                        sparewithPrice.UnitPrice = (ActualAmountPerUnit).ToString();
                        decimal MRP = (ActualAmountPerUnit - discount) * Quantity;
                        decimal TotalTax = (2 * Convert.ToDecimal(row["SGSTValue"]));
                        //   decimal TaxAmount = (ActualAmountPerUnit - discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);
                        decimal TaxAmount = (((MRP) * (TotalTax) / (TotalTax + 100)) / 2);
                        //  decimal TaxAmount = Convert.ToDecimal(row["CGSTAmount"]) * Quantity;
                        //   decimal TaxAmount = (ActualAmountPerUnit - discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);
                        //(Total Amount ) - ((Total Amount) * GST) / GST + 100)  
                        //  decimal TaxAmount =   ((ActualAmountPerUnit - discount) * Quantity)  - (((ActualAmountPerUnit - discount) * Quantity) * (2*Convert.ToDecimal(row["SGSTValue"])) /((2*Convert.ToDecimal(row["SGSTValue"])) + 100)) ;

                        //(((ActualAmountPerUnit - discount) * Quantity) * ((Convert.ToDecimal(row["SGSTValue"])) / (100 + ((Convert.ToDecimal(row["SGSTValue"]))))));
                        
                        //Change For NIA
                        //sparewithPrice.Discount = discount.ToString();
                        sparewithPrice.Discount = Math.Round(Convert.ToDecimal(row["DiscountPerUnitTaxable"]), 2).ToString();
                        sparewithPrice.SGSTRate = row["SGSTValue"].ToString();
                        sparewithPrice.CGSTRate = row["CGSTValue"].ToString();
                        //sparewithPrice.SGSTAmount = DisplayPrice(Convert.ToDecimal(row["SGSTAmount"].ToString())* Quantity).ToString();
                        //sparewithPrice.CGSTAmount = DisplayPrice(Convert.ToDecimal(row["CGSTAmount"].ToString()) * Quantity).ToString();
                        sparewithPrice.SGSTAmount = DisplayPrice(TaxAmount);
                        sparewithPrice.CGSTAmount = DisplayPrice(TaxAmount);
                        TotalCGSTAmount += TaxAmount;
                        decimal TotalDiscount = (totaldis);


                        //
                        decimal TaxableAmount = 0;
                        if (type == 1)
                        {
                            TaxableAmount = ((ActualAmountPerUnit - discount) * Quantity) - (((ActualAmountPerUnit - discount) * Quantity) * (2 * (Convert.ToDecimal(row["SGSTValue"])) / (100 + (2 * (Convert.ToDecimal(row["SGSTValue"]))))));//Math.Round((((ActualAmountPerUnit * Quantity) - (2 * TaxAmount) )), 2);
                            TotalTaxableAmountForSpare += TaxableAmount;
                        }
                        else
                        {
                            TaxableAmount = ((ActualAmountPerUnit) * Quantity) - (((ActualAmountPerUnit) * Quantity) * (2 * (Convert.ToDecimal(row["SGSTValue"])) / (100 + (2 * (Convert.ToDecimal(row["SGSTValue"]))))));
                            decimal TaxableAfterDepriciation = ((ActualAmountPerUnit - discount) * Quantity) - (((ActualAmountPerUnit - discount) * Quantity) * (2 * (Convert.ToDecimal(row["SGSTValue"])) / (100 + (2 * (Convert.ToDecimal(row["SGSTValue"]))))));//Math.Round((((ActualAmountPerUnit * Quantity) - (2 * TaxAmount) )), 2);
                            TotalTaxableAmountForSpare += TaxableAfterDepriciation;
                        }
                        decimal FinalAmont = ((ActualAmountPerUnit - discount) * Quantity);//- TotalDiscount), 2);
                        //   FinalAmont = FinalAmont - discount;
                        if (TaxableAmount < 0)
                        {
                            TaxableAmount = 0;
                        }

                        if (FinalAmont < 0)
                        {
                            FinalAmont = 0;
                        }
                        sparewithPrice.TaxableAmount = DisplayPrice(TaxableAmount).ToString();
                        sparewithPrice.FinalAmount = FinalAmont.ToString();
                        decimal amount = ((ActualAmountPerUnit - discount) * Quantity);
                        //  amount = amount - discount;
                        if (amount < 0)
                        {
                            amount = 0;
                        }
                        SubTotalForSpare += amount;
                        //sparewithPrice.TaxableAmount=taxableamount.ToString();
                        sparewithPrice.HSNNumber = row["HSN"].ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        //   sparewithPrice.SubTotal = row[""].ToString();
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    if (jobCardSpare.Spares.Count > 1)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();
                        sparewithPrice.Name = "SubTotalForSpare";
                        sparewithPrice.Type = "Header";
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.SubTotal = DisplayPrice(SubTotalForSpare).ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TaxableAmount = DisplayPrice(TotalTaxableAmountForSpare);
                        sparewithPrice.TotalDiscountForSpare = DisplayPrice(TotalDiscountSpare).ToString();
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    jobCardSpare.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                    jobCardSpare.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                    jobCardSpare.resultflag = "1";
                    jobCardSpare.Message = Constant.Message.SuccessMessage;
                }
                else
                {
                    jobCardSpare.resultflag = "0";
                    jobCardSpare.Message = Constant.Message.NoDataFound;
                }
                FinalTotalCGSTAmount = TotalCGSTAmount;
                TotalCGSTAmount = 0;
                //Step 1
                DataTable datatable = this.dbCon.GetDataTableWithParams("SELECT distinct Categoryid from JobCard_Service_Mapping inner join [Service] on Service.Id=JobCard_Service_Mapping.ServiceId where JobCardId=@1", new string[1] { JobCardId });

                DataTable dataTableWithParams2 = this.dbCon.GetDataTableWithParams("SELECT Estimate_Service_Mapping.Id,ISNULL(Service.Name, '') AS Name, ISNULL(Service.HSNNumber, '') AS HSN, Estimate.Id as InvoiceId,Estimate_Service_Mapping.ServiceId, ISNULL(Estimate_Service_Mapping.CGSTValue, 0) AS CGSTValue, ISNULL(Estimate_Service_Mapping.CGSTAmount, 0) AS CGSTAmount, ISNULL(Estimate_Service_Mapping.SGSTValue, 0) AS SGSTValue, ISNULL(Estimate_Service_Mapping.SGSTAmount, 0) AS SGSTAmount,   " + (type == 2 ? "isnull((Select ISM.ActualAmountPerUnit from Invoice_Service_Mapping as ISM where ISM.Id=Estimate_Service_Mapping.RefId),0)" : "isnull(Estimate_Service_Mapping.DiscountPerUnit,0)") + " as DiscountPerUnit, " + (type == 2 ? "iif(Estimate_Service_Mapping.RefId is not null,(Select ISM.ActualAmountPerUnit+Estimate_Service_Mapping.ActualAmountPerUnit from Invoice_Service_Mapping as ISM where ISM.Id=Estimate_Service_Mapping.RefId),Estimate_Service_Mapping.ActualAmountPerUnit)" : " Estimate_Service_Mapping.ActualAmountPerUnit ") + "  as ActualAmountPerUnit , Estimate_Service_Mapping.Quantity, Estimate.TotalServiceAmount, Estimate.TotalSpareAmount, isnull(Estimate_Service_Mapping.TotalDiscount,0) as TotalDiscount,Estimate_Service_Mapping.[MappingId] as MapId FROM  Invoice_Service_Mapping as Estimate_Service_Mapping INNER JOIN Invoice as Estimate ON Estimate.Id = Estimate_Service_Mapping.Invoiceid INNER  JOIN Service ON Service.Id = Estimate_Service_Mapping.ServiceId WHERE Estimate.JobcardId=@1 and Estimate.Type=" + type + " and isnull(Estimate.IsCancelled,0)=0  and isnull(IsCancelled,0)=0", new string[1]
                {
                  JobCardId
                });

                //services

                for (int i = 0; i < strArrService.Length; i++)
                {
                    string[] strItem = strArrService[i].Split(',');
                    if (strItem.Length >= 2)
                    {

                        DataRow[] drdata = dataTableWithParams2.Select("id=" + strItem[0]);
                        if (drdata.Length > 0)
                        {
                            double qty = 0;
                            double.TryParse(drdata[0]["Quantity"].ToString(), out qty);

                            decimal amt = 0;
                            decimal.TryParse(drdata[0]["ActualAmountPerUnit"].ToString(), out amt);

                            bool isInPer = false;
                            if (strItem[1].Contains("%"))
                            {
                                isInPer = true;
                                strItem[1] = strItem[1].Replace("%", "");
                            }
                            if (String.IsNullOrEmpty(strItem[1]))
                            {
                                strItem[1] = "0";
                            }
                            decimal dc_DiscountPerUnit = (isInPer ? ((amt * decimal.Parse(strItem[1])) / 100) : decimal.Parse(strItem[1]));
                            decimal dc_TotalDiscount = dc_DiscountPerUnit * (decimal)qty;

                            drdata[0]["DiscountPerUnit"] = dc_DiscountPerUnit;
                            drdata[0]["TotalDiscount"] = dc_TotalDiscount;
                        }
                    }
                }
                if (dataTableWithParams2 != null && dataTableWithParams2.Rows.Count > 0)
                {
                    Decimal ActualAmountPerUnit = 0;
                    ServiceClass.ServicesForInvoice sparewithPrice = new ServiceClass.ServicesForInvoice();
                    sparewithPrice.Type = "Header";
                    sparewithPrice.Name = "Labour";
                    jobCardSpare.Services.Add(sparewithPrice);
                    foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams2.Rows)
                    {
                        d_Total_DiscountSpare_Service += decimal.Parse(row["TotalDiscount"].ToString());
                        sparewithPrice = new ServiceClass.ServicesForInvoice();

                        sparewithPrice.Id = row["ServiceId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        sparewithPrice.MappingId = row["MapId"].ToString();
                        sparewithPrice.ItemType = "Service";
                        sparewithPrice.InvoiceServiceId = row["Id"].ToString();
                        try
                        {
                            ActualAmountPerUnit = Convert.ToDecimal(row["ActualAmountPerUnit"]);
                        }
                        catch (Exception ex)
                        { }
                        Decimal Quantity = Convert.ToDecimal(row["Quantity"]);

                        sparewithPrice.Price = ActualAmountPerUnit.ToString();
                        sparewithPrice.HSNNumber = "998714";
                        // sparewithPrice.UnitPrice = (num3).ToString();
                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        decimal discount = Math.Round(Convert.ToDecimal(row["DiscountPerUnit"]), 2);

                        if (ActualAmountPerUnit <= discount)
                        {
                            discount = ActualAmountPerUnit;
                        }


                        sparewithPrice.Discount = (discount).ToString();
                        //  ActualAmountPerUnit = ActualAmountPerUnit ;
                        TotalDiscountService += (discount * Quantity);
                        sparewithPrice.UnitPrice = (ActualAmountPerUnit).ToString();
                        //  ActualAmountPerUnit = ActualAmountPerUnit + discount;
                        //decimal TotalDiscount = (discount );
                        decimal TotalDiscount = (discount * Quantity);
                        decimal MRP = 0;

                        //NIA Change
                       // if(type==1)
                            MRP= (ActualAmountPerUnit - discount) * Quantity;
                        //else
                        //    MRP = ActualAmountPerUnit * Quantity;
                        decimal TotalTax = (2 * Convert.ToDecimal(row["SGSTValue"]));
                        decimal TaxAmount = ((MRP) * (((TotalTax) / 2) / 100));
                        num1 += ActualAmountPerUnit * Quantity + (2 * TaxAmount);
                        //   decimal TaxAmount = (ActualAmountPerUnit - discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);
                        //   decimal TaxAmount = (((MRP) * (TotalTax) / (TotalTax + 100))/2);
                        //  decimal TaxAmount = ((ActualAmountPerUnit - discount) * Quantity) - (((ActualAmountPerUnit - discount) * Quantity) * (2 * Convert.ToDecimal(row["SGSTValue"])) / ((2 * Convert.ToDecimal(row["SGSTValue"])) + 100));
                        // decimal TaxAmount = ((ActualAmountPerUnit - discount) * Quantity)  - (((ActualAmountPerUnit - discount) * Quantity) * Convert.ToDecimal(row["SGSTValue"]) /(Convert.ToDecimal(row["SGSTValue"]) + 100)) ;//(((ActualAmountPerUnit - discount) * Quantity) * ( (Convert.ToDecimal(row["SGSTValue"])) / (100 + ( (Convert.ToDecimal(row["SGSTValue"]))))));
                        //  decimal TaxAmount = (ActualAmountPerUnit-discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);

                        //decimal TaxAmount = Convert.ToDecimal(row["CGSTAmount"]);

                        //NIA Change
                        decimal TaxableAmount = 0;// ((ActualAmountPerUnit - discount) * Quantity);
                        if (type == 1)
                        {
                            TaxableAmount = (ActualAmountPerUnit - discount) * Quantity;
                            TotalTaxableAmountForService += TaxableAmount;
                        }
                        else
                        {
                            decimal TaxableAmountAfterDepriciation = (ActualAmountPerUnit - discount) * Quantity;
                            TaxableAmount = ActualAmountPerUnit * Quantity;
                            TotalTaxableAmountForService += TaxableAmountAfterDepriciation;
                        }
                        
                        decimal FinalAmont = (((ActualAmountPerUnit - discount) * Quantity) + (2 * TaxAmount));
                        if (TaxableAmount < 0)
                        {
                            TaxableAmount = 0;
                        }

                        if (FinalAmont < 0)
                        {
                            FinalAmont = 0;
                        }
                        //sparewithPrice.Discount = row["Discount"];
                        sparewithPrice.SGSTRate = row["SGSTValue"].ToString();
                        sparewithPrice.CGSTRate = row["CGSTValue"].ToString();
                        //sparewithPrice.SGSTAmount = DisplayPrice(Convert.ToDecimal(row["SGSTAmount"].ToString())).ToString();
                        //sparewithPrice.CGSTAmount = DisplayPrice(Convert.ToDecimal(row["CGSTAmount"].ToString())).ToString();
                        sparewithPrice.SGSTAmount = DisplayPrice(TaxAmount);
                        sparewithPrice.CGSTAmount = DisplayPrice(TaxAmount);
                        //sparewithPrice.SGSTAmount = row["SGSTAmount"].ToString();
                        //sparewithPrice.CGSTAmount = row["CGSTAmount"].ToString();

                        TotalCGSTAmount += TaxAmount;
                        sparewithPrice.TaxableAmount = DisplayPrice(TaxableAmount).ToString();
                        sparewithPrice.FinalAmount = DisplayPrice(FinalAmont).ToString();
                        SubTotalForService += FinalAmont;
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        jobCardSpare.Services.Add(sparewithPrice);
                    }
                    if (jobCardSpare.Services.Count > 1)
                    {
                        sparewithPrice = new ServiceClass.ServicesForInvoice();
                        sparewithPrice.Name = "SubTotalForService";
                        sparewithPrice.Type = "Header";
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.SubTotal = DisplayPrice(SubTotalForService).ToString();
                        // sparewithPrice.TotalDiscountForSpare = TotalDiscountSpare.ToString();
                        sparewithPrice.TotalDiscountForService = DisplayPrice(TotalDiscountService).ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TaxableAmount = DisplayPrice(TotalTaxableAmountForService).ToString();
                        jobCardSpare.Services.Add(sparewithPrice);
                    }
                }

                //Step 2
                FinalTotalCGSTAmount += TotalCGSTAmount;
                jobCardSpare.FinalTotalCGSTAmount = FinalTotalCGSTAmount.ToString();
              //  dbCon.InsertLogs("JobCard Spare COunt :: " + jobCardSpare.Spares.Count);
                jobCardSpare.resultflag = "1";
                jobCardSpare.Message = Constant.Message.SuccessMessage;

                //        }
                //    }
                //}
                //else
                //{
                //    jobCardSpare.resultflag = "0";
                //    jobCardSpare.Message = Constant.Message.NoDataFound;
                //}


                jobCardSpare.TotalBeforeTaxService = TotalTaxableAmountForService.ToString();
                jobCardSpare.TotalBeforeTaxSpare = TotalTaxableAmountForSpare.ToString();
                jobCardSpare.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                jobCardSpare.SubTotalForService = DisplayPrice(SubTotalForService);
                jobCardSpare.SubTotalForSpare = DisplayPrice(SubTotalForSpare);
                jobCardSpare.TotalAmount = DisplayPrice(SubTotalForService + SubTotalForSpare);
                // - d_Total_DiscountSpare_Service
                jobCardSpare.FinalAmount = DisplayPrice(SubTotalForService + SubTotalForSpare);
                return jobCardSpare;
            }
            catch (Exception ex)
            {
                dbCon.InsertLogs("GetJobCardSpareInJobCardForInvoice : " + ex.ToString(), ex.StackTrace.ToString(), ex.Message.ToString());
                return new ServiceClass.JobCardSpareForInvoice();
            }
        }

        //Proforma Current not cancelled
        public ServiceClass.JobCardSpareForInvoice GetJobCardSpareInJobCardForInvoiceV3_new_V1_Proforma(string JobCardId, string Spare = "", string Service = "", int type = 1, bool fromEstimate = false, bool ShowAllocated = false)
        {
            ServiceClass.JobCardSpareForInvoice jobCardSpare = new ServiceClass.JobCardSpareForInvoice();
            try
            {
                Decimal num1 = new Decimal(0);
                Decimal num2 = new Decimal(0);
                int Jobcardid = Convert.ToInt32(JobCardId);
                string InvoiceId = "0";
                string[] strArrSpare = Spare.Split(new string[] { "$#@" }, StringSplitOptions.None);
                string[] strArrService = Service.Split(new string[] { "$#@" }, StringSplitOptions.None);


                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("SELECT Invoice_Spare_Mapping.Id,ISNULL(Spare.Name, '') AS Name, ISNULL(Spare.HSNNumber, '') AS HSN, Estimate.Id as InvoiceId,Invoice_Spare_Mapping.SpareId, ISNULL(Invoice_Spare_Mapping.CGSTValue, 0) AS CGSTValue, ISNULL(Invoice_Spare_Mapping.CGSTAmount, 0) AS CGSTAmount, ISNULL(Invoice_Spare_Mapping.SGSTValue, 0) AS SGSTValue, ISNULL(Invoice_Spare_Mapping.SGSTAmount, 0) AS SGSTAmount, " + (type == 2 ? "(Select ISM.ActualAmountPerUnit  from Invoice_Spare_Mapping as ISM where ISM.Id=Invoice_Spare_Mapping.RefId)" : "isnull(Invoice_Spare_Mapping.DiscountPerUnit,0)") + "  as DiscountPerUnit, " + (type == 2 ? "isnull((Select  ISM.ActualAmountPerUnit-(((ISM.ActualAmountPerUnit*(ISM.CGSTValue*2))/(100+(ISM.CGSTValue*2)))) from Invoice_Spare_Mapping as ISM where ISM.Id=Invoice_Spare_Mapping.RefId),0)" : "isnull(Invoice_Spare_Mapping.TotalDiscount,0)") + "  as DiscountPerUnitTaxable,  " + (type == 2 ? "(Select ISM.Mrp from Invoice_Spare_Mapping as ISM where ISM.Id=Invoice_Spare_Mapping.RefId)" : " Invoice_Spare_Mapping.ActualAmountPerUnit ") + " as ActualAmountPerUnit  , Invoice_Spare_Mapping.Quantity, Estimate.TotalServiceAmount, Estimate.TotalSpareAmount, isnull(Invoice_Spare_Mapping.TotalDiscount,0) as TotalDiscount,0 as PushToInsurance ,0 as InsuranceAmount,Invoice_Spare_Mapping.[MappingId] as MapId,((Invoice_Spare_Mapping.ActualAmountPerUnit-Invoice_Spare_Mapping.DiscountPerUnit) -(Invoice_Spare_Mapping.CGSTAmount+Invoice_Spare_Mapping.SGSTAmount))-Convert(decimal(18,2),(isnull((Select GRNDetail.VendorInvoiceRate from GRNDetail where id=iif(Estimate.Type=1,GrnDetailId,(Select ISM.GrnDetailId from Invoice_Spare_Mapping as ISM where id=Invoice_Spare_Mapping.refid ))),isnull(Invoice_Spare_Mapping.PurchaseAmount,((Invoice_Spare_Mapping.ActualAmountPerUnit-Invoice_Spare_Mapping.DiscountPerUnit) -(Invoice_Spare_Mapping.CGSTAmount+Invoice_Spare_Mapping.SGSTAmount))))*(iif(Estimate.Type=1,Invoice_Spare_Mapping.Depreciation,100-(Select ISM.Depreciation from Invoice_Spare_Mapping as ISM where id=Invoice_Spare_Mapping.refid ))))/100) as Profit,Convert(decimal(18,2),(isnull((Select GRNDetail.VendorInvoiceRate from GRNDetail where id=iif(Estimate.Type=1,GrnDetailId,(Select ISM.GrnDetailId from Invoice_Spare_Mapping as ISM where id=Invoice_Spare_Mapping.refid ))),isnull(Invoice_Spare_Mapping.PurchaseAmount,0))*(iif(Estimate.Type=1,Invoice_Spare_Mapping.Depreciation,100-(Select ISM.Depreciation from Invoice_Spare_Mapping as ISM where id=Invoice_Spare_Mapping.refid ))))/100) as PurchasePrice FROM  Invoice_Spare_Mapping as Invoice_Spare_Mapping  INNER JOIN Invoice as Estimate ON Estimate.Id = Invoice_Spare_Mapping.InvoiceId INNER  JOIN Spare ON Spare.Id = Invoice_Spare_Mapping.SpareId WHERE Estimate.JobCardId=@1 and Estimate.Type=" + type + " and isnull(IsCancelled,0)=0 and (Invoice_Spare_Mapping.ActualAmountPerUnit>0 or isnull(Invoice_Spare_Mapping.IsMrpChanged,0)=0)  ORDER BY Invoice_Spare_Mapping.DOC DESC  ", new string[1]
        {
          JobCardId
        });
                //Spare
                for (int i = 0; i < strArrSpare.Length; i++)
                {
                    string[] strItem = strArrSpare[i].Split(',');
                    if (strItem.Length >= 2)
                    {
                        DataRow[] drdata = dataTableWithParams.Select("id=" + strItem[0]);
                        if (drdata.Length > 0)
                        {
                            double qty = 0;
                            double.TryParse(drdata[0]["Quantity"].ToString(), out qty);

                            decimal amt = 0;
                            decimal.TryParse(drdata[0]["ActualAmountPerUnit"].ToString(), out amt);

                            bool isInPer = false;
                            if (strItem[1].Contains("%"))
                            {
                                isInPer = true;
                                strItem[1] = strItem[1].Replace("%", "");
                            }
                            if (String.IsNullOrEmpty(strItem[1]))
                            {
                                strItem[1] = "0";
                            }
                            decimal dc_DiscountPerUnit = (isInPer ? ((amt * decimal.Parse(strItem[1])) / 100) : decimal.Parse(strItem[1]));
                            decimal dc_TotalDiscount = dc_DiscountPerUnit * (decimal)qty;

                            drdata[0]["DiscountPerUnit"] = dc_DiscountPerUnit;
                            drdata[0]["TotalDiscount"] = dc_TotalDiscount;
                        }
                    }
                }
                Decimal TotalCGSTAmount = 0;
                Decimal FinalTotalCGSTAmount = 0;
                Decimal SubTotalForService = 0;
                Decimal SubTotalForSpare = 0;
                decimal d_Total_DiscountSpare_Service = 0;
                decimal TotalTaxableAmountForService = 0;
                decimal TotalTaxableAmountForSpare = 0;
                decimal TotalDiscountSpare = 0;
                decimal TotalDiscountService = 0;

                if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                {
                    ServiceClass.SparesForInvoice sparewithPrice = new ServiceClass.SparesForInvoice();
                    sparewithPrice.Name = "Spares / Consumables";
                    sparewithPrice.Type = "Header";
                    jobCardSpare.Spares.Add(sparewithPrice);
                    Decimal ActualAmountPerUnit = 0;

                    foreach (DataRow row in dataTableWithParams.Rows)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();
                        d_Total_DiscountSpare_Service += decimal.Parse(row["TotalDiscount"].ToString());
                        sparewithPrice.Id = row["SpareId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        sparewithPrice.MappingId = row["MapId"].ToString();
                        sparewithPrice.ItemType = "Spare";
                        sparewithPrice.InvoiceSpareId = row["Id"].ToString();
                        try
                        {
                            ActualAmountPerUnit = Convert.ToDecimal(row["ActualAmountPerUnit"]);
                            sparewithPrice.Profit = row["Profit"].ToString();
                            sparewithPrice.PurchasePrice = row["PurchasePrice"].ToString();
                        }
                        catch (Exception ex)
                        { }
                        Decimal Quantity = Convert.ToDecimal(row["Quantity"]);
                        num1 += ActualAmountPerUnit * Quantity;
                        // sparewithPrice.Price = ActualAmountPerUnit+Discount.ToString();

                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        decimal discount = Math.Round(Convert.ToDecimal(row["DiscountPerUnit"]), 2);
                        if (ActualAmountPerUnit <= discount)
                        {
                            discount = ActualAmountPerUnit;
                        }
                        // discount = discount * Quantity;


                        decimal totaldis = discount * Quantity;

                        sparewithPrice.Price = (ActualAmountPerUnit).ToString();
                        TotalDiscountSpare += totaldis;
                        sparewithPrice.UnitPrice = (ActualAmountPerUnit).ToString();
                        decimal MRP = (ActualAmountPerUnit - discount) * Quantity;
                        decimal TotalTax = (2 * Convert.ToDecimal(row["SGSTValue"]));
                        //   decimal TaxAmount = (ActualAmountPerUnit - discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);
                        decimal TaxAmount = (((MRP) * (TotalTax) / (TotalTax + 100)) / 2);
                        //sparewithPrice.Discount = discount.ToString();
                        sparewithPrice.Discount = Math.Round(Convert.ToDecimal(row["DiscountPerUnitTaxable"]), 2).ToString();
                        sparewithPrice.SGSTRate = row["SGSTValue"].ToString();
                        sparewithPrice.CGSTRate = row["CGSTValue"].ToString();
                        sparewithPrice.SGSTAmount = DisplayPrice(TaxAmount);
                        sparewithPrice.CGSTAmount = DisplayPrice(TaxAmount);
                        TotalCGSTAmount += TaxAmount;
                        decimal TotalDiscount = (totaldis);


                        //
                        //decimal TaxableAmount = ((ActualAmountPerUnit - discount) * Quantity) - (((ActualAmountPerUnit - discount) * Quantity) * (2 * (Convert.ToDecimal(row["SGSTValue"])) / (100 + (2 * (Convert.ToDecimal(row["SGSTValue"]))))));//Math.Round((((ActualAmountPerUnit * Quantity) - (2 * TaxAmount) )), 2);
                        //TotalTaxableAmountForSpare += TaxableAmount;

                        decimal TaxableAmount = 0;
                        if (type == 1)
                        {
                            TaxableAmount = ((ActualAmountPerUnit - discount) * Quantity) - (((ActualAmountPerUnit - discount) * Quantity) * (2 * (Convert.ToDecimal(row["SGSTValue"])) / (100 + (2 * (Convert.ToDecimal(row["SGSTValue"]))))));//Math.Round((((ActualAmountPerUnit * Quantity) - (2 * TaxAmount) )), 2);
                            TotalTaxableAmountForSpare += TaxableAmount;
                        }
                        else
                        {
                            TaxableAmount = ((ActualAmountPerUnit) * Quantity) - (((ActualAmountPerUnit) * Quantity) * (2 * (Convert.ToDecimal(row["SGSTValue"])) / (100 + (2 * (Convert.ToDecimal(row["SGSTValue"]))))));
                            decimal TaxableAfterDepriciation = ((ActualAmountPerUnit - discount) * Quantity) - (((ActualAmountPerUnit - discount) * Quantity) * (2 * (Convert.ToDecimal(row["SGSTValue"])) / (100 + (2 * (Convert.ToDecimal(row["SGSTValue"]))))));//Math.Round((((ActualAmountPerUnit * Quantity) - (2 * TaxAmount) )), 2);
                            TotalTaxableAmountForSpare += TaxableAfterDepriciation;
                        }

                        decimal FinalAmont = ((ActualAmountPerUnit - discount) * Quantity);//- TotalDiscount), 2);
                        //   FinalAmont = FinalAmont - discount;
                        if (TaxableAmount < 0)
                        {
                            TaxableAmount = 0;
                        }

                        if (FinalAmont < 0)
                        {
                            FinalAmont = 0;
                        }
                        sparewithPrice.TaxableAmount = DisplayPrice(TaxableAmount).ToString();
                        sparewithPrice.FinalAmount = FinalAmont.ToString();
                        decimal amount = ((ActualAmountPerUnit - discount) * Quantity);
                        //  amount = amount - discount;
                        if (amount < 0)
                        {
                            amount = 0;
                        }
                        SubTotalForSpare += amount;
                        //sparewithPrice.TaxableAmount=taxableamount.ToString();
                        sparewithPrice.HSNNumber = row["HSN"].ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        //   sparewithPrice.SubTotal = row[""].ToString();
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    if (jobCardSpare.Spares.Count > 1)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();
                        sparewithPrice.Name = "SubTotalForSpare";
                        sparewithPrice.Type = "Header";
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.SubTotal = DisplayPrice(SubTotalForSpare).ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TaxableAmount = DisplayPrice(TotalTaxableAmountForSpare);
                        sparewithPrice.TotalDiscountForSpare = DisplayPrice(TotalDiscountSpare).ToString();
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    jobCardSpare.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                    jobCardSpare.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                    jobCardSpare.resultflag = "1";
                    jobCardSpare.Message = Constant.Message.SuccessMessage;
                }
                else
                {
                    jobCardSpare.resultflag = "0";
                    jobCardSpare.Message = Constant.Message.NoDataFound;
                }
                FinalTotalCGSTAmount = TotalCGSTAmount;
                TotalCGSTAmount = 0;
                //Step 1
                DataTable datatable = this.dbCon.GetDataTableWithParams("SELECT distinct Categoryid from JobCard_Service_Mapping inner join [Service] on Service.Id=JobCard_Service_Mapping.ServiceId where JobCardId=@1", new string[1] { JobCardId });

                DataTable dataTableWithParams2 = this.dbCon.GetDataTableWithParams("SELECT Estimate_Service_Mapping.Id,ISNULL(Service.Name, '') AS Name, ISNULL(Service.HSNNumber, '') AS HSN, Estimate.Id as InvoiceId,Estimate_Service_Mapping.ServiceId, ISNULL(Estimate_Service_Mapping.CGSTValue, 0) AS CGSTValue, ISNULL(Estimate_Service_Mapping.CGSTAmount, 0) AS CGSTAmount, ISNULL(Estimate_Service_Mapping.SGSTValue, 0) AS SGSTValue, ISNULL(Estimate_Service_Mapping.SGSTAmount, 0) AS SGSTAmount,   " + (type == 2 ? "(Select ISM.ActualAmountPerUnit from Invoice_Service_Mapping as ISM where ISM.Id=Estimate_Service_Mapping.RefId)" : "isnull(Estimate_Service_Mapping.DiscountPerUnit,0)") + " as DiscountPerUnit, " + (type == 2 ? "(Select ISM.ActualAmountPerUnit+Estimate_Service_Mapping.ActualAmountPerUnit from Invoice_Service_Mapping as ISM where ISM.Id=Estimate_Service_Mapping.RefId)" : " Estimate_Service_Mapping.ActualAmountPerUnit ") + "  as ActualAmountPerUnit  , Estimate_Service_Mapping.Quantity, Estimate.TotalServiceAmount, Estimate.TotalSpareAmount, isnull(Estimate_Service_Mapping.TotalDiscount,0) as TotalDiscount,Estimate_Service_Mapping.[MappingId] as MapId,(Estimate_Service_Mapping.ActualAmountPerUnit-isnull(Estimate_Service_Mapping.DiscountPerUnit,0))*Estimate_Service_Mapping.Quantity as Profit  FROM  Invoice_Service_Mapping as Estimate_Service_Mapping INNER JOIN Invoice as Estimate ON Estimate.Id = Estimate_Service_Mapping.Invoiceid INNER  JOIN Service ON Service.Id = Estimate_Service_Mapping.ServiceId WHERE Estimate.JobcardId=@1 and Estimate.Type=" + type + "  and isnull(IsCancelled,0)=0 ", new string[1]
                {
                  JobCardId
                });

                //services

                for (int i = 0; i < strArrService.Length; i++)
                {
                    string[] strItem = strArrService[i].Split(',');
                    if (strItem.Length >= 2)
                    {

                        DataRow[] drdata = dataTableWithParams2.Select("id=" + strItem[0]);
                        if (drdata.Length > 0)
                        {
                            double qty = 0;
                            double.TryParse(drdata[0]["Quantity"].ToString(), out qty);

                            decimal amt = 0;
                            decimal.TryParse(drdata[0]["ActualAmountPerUnit"].ToString(), out amt);

                            bool isInPer = false;
                            if (strItem[1].Contains("%"))
                            {
                                isInPer = true;
                                strItem[1] = strItem[1].Replace("%", "");
                            }
                            if (String.IsNullOrEmpty(strItem[1]))
                            {
                                strItem[1] = "0";
                            }
                            decimal dc_DiscountPerUnit = (isInPer ? ((amt * decimal.Parse(strItem[1])) / 100) : decimal.Parse(strItem[1]));
                            decimal dc_TotalDiscount = dc_DiscountPerUnit * (decimal)qty;

                            drdata[0]["DiscountPerUnit"] = dc_DiscountPerUnit;
                            drdata[0]["TotalDiscount"] = dc_TotalDiscount;
                        }
                    }
                }

                if (dataTableWithParams2 != null && dataTableWithParams2.Rows.Count > 0)
                {
                    Decimal ActualAmountPerUnit = 0;
                    ServiceClass.ServicesForInvoice sparewithPrice = new ServiceClass.ServicesForInvoice();
                    sparewithPrice.Type = "Header";
                    sparewithPrice.Name = "Labour";
                    jobCardSpare.Services.Add(sparewithPrice);
                    foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams2.Rows)
                    {
                        d_Total_DiscountSpare_Service += decimal.Parse(row["TotalDiscount"].ToString());
                        sparewithPrice = new ServiceClass.ServicesForInvoice();

                        sparewithPrice.Id = row["ServiceId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        sparewithPrice.MappingId = row["MapId"].ToString();
                        sparewithPrice.ItemType = "Service";
                        sparewithPrice.InvoiceServiceId = row["Id"].ToString();
                        try
                        {
                            ActualAmountPerUnit = Convert.ToDecimal(row["ActualAmountPerUnit"]);
                            sparewithPrice.Profit = row["Profit"].ToString();
                        }
                        catch (Exception ex)
                        { }
                        Decimal Quantity = Convert.ToDecimal(row["Quantity"]);

                        sparewithPrice.Price = ActualAmountPerUnit.ToString();
                        sparewithPrice.HSNNumber = "998714";
                        // sparewithPrice.UnitPrice = (num3).ToString();
                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        decimal discount = Math.Round(Convert.ToDecimal(row["DiscountPerUnit"]), 2);

                        if (ActualAmountPerUnit <= discount)
                        {
                            discount = ActualAmountPerUnit;
                        }


                        sparewithPrice.Discount = (discount).ToString();
                        //  ActualAmountPerUnit = ActualAmountPerUnit ;
                        TotalDiscountService += (discount * Quantity);
                        sparewithPrice.UnitPrice = (ActualAmountPerUnit).ToString();
                        //  ActualAmountPerUnit = ActualAmountPerUnit + discount;
                        //decimal TotalDiscount = (discount );
                        decimal TotalDiscount = (discount * Quantity);
                        decimal MRP = (ActualAmountPerUnit - discount) * Quantity;
                        decimal TotalTax = (2 * Convert.ToDecimal(row["SGSTValue"]));


                        decimal TaxAmount = ((MRP) * (((TotalTax) / 2) / 100));
                        num1 += ActualAmountPerUnit * Quantity + (2 * TaxAmount);
                        //   decimal TaxAmount = (ActualAmountPerUnit - discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);
                        //   decimal TaxAmount = (((MRP) * (TotalTax) / (TotalTax + 100))/2);
                        //  decimal TaxAmount = ((ActualAmountPerUnit - discount) * Quantity) - (((ActualAmountPerUnit - discount) * Quantity) * (2 * Convert.ToDecimal(row["SGSTValue"])) / ((2 * Convert.ToDecimal(row["SGSTValue"])) + 100));
                        // decimal TaxAmount = ((ActualAmountPerUnit - discount) * Quantity)  - (((ActualAmountPerUnit - discount) * Quantity) * Convert.ToDecimal(row["SGSTValue"]) /(Convert.ToDecimal(row["SGSTValue"]) + 100)) ;//(((ActualAmountPerUnit - discount) * Quantity) * ( (Convert.ToDecimal(row["SGSTValue"])) / (100 + ( (Convert.ToDecimal(row["SGSTValue"]))))));
                        //  decimal TaxAmount = (ActualAmountPerUnit-discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);

                        //decimal TaxAmount = Convert.ToDecimal(row["CGSTAmount"]);
                        decimal TaxableAmount = 0;// ((ActualAmountPerUnit - discount) * Quantity);
                        if (type == 1)
                        {
                            TaxableAmount = (ActualAmountPerUnit - discount) * Quantity;
                            TotalTaxableAmountForService += TaxableAmount;
                        }
                        else
                        {
                            decimal TaxableAmountAfterDepriciation = (ActualAmountPerUnit - discount) * Quantity;
                            TaxableAmount = ActualAmountPerUnit * Quantity;
                            TotalTaxableAmountForService += TaxableAmountAfterDepriciation;
                        }
                        //decimal TaxableAmount = ((ActualAmountPerUnit - discount) * Quantity);
                        //TotalTaxableAmountForService += TaxableAmount;
                        decimal FinalAmont = (((ActualAmountPerUnit - discount) * Quantity) + (2 * TaxAmount));
                        if (TaxableAmount < 0)
                        {
                            TaxableAmount = 0;
                        }

                        if (FinalAmont < 0)
                        {
                            FinalAmont = 0;
                        }
                        //sparewithPrice.Discount = row["Discount"];
                        sparewithPrice.SGSTRate = row["SGSTValue"].ToString();
                        sparewithPrice.CGSTRate = row["CGSTValue"].ToString();
                        //sparewithPrice.SGSTAmount = DisplayPrice(Convert.ToDecimal(row["SGSTAmount"].ToString())).ToString();
                        //sparewithPrice.CGSTAmount = DisplayPrice(Convert.ToDecimal(row["CGSTAmount"].ToString())).ToString();
                        sparewithPrice.SGSTAmount = DisplayPrice(TaxAmount);
                        sparewithPrice.CGSTAmount = DisplayPrice(TaxAmount);
                        //sparewithPrice.SGSTAmount = row["SGSTAmount"].ToString();
                        //sparewithPrice.CGSTAmount = row["CGSTAmount"].ToString();

                        TotalCGSTAmount += TaxAmount;
                        sparewithPrice.TaxableAmount = DisplayPrice(TaxableAmount).ToString();
                        sparewithPrice.FinalAmount = DisplayPrice(FinalAmont).ToString();
                        SubTotalForService += FinalAmont;
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        jobCardSpare.Services.Add(sparewithPrice);
                    }
                    if (jobCardSpare.Services.Count > 1)
                    {
                        sparewithPrice = new ServiceClass.ServicesForInvoice();
                        sparewithPrice.Name = "SubTotalForService";
                        sparewithPrice.Type = "Header";
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.SubTotal = DisplayPrice(SubTotalForService).ToString();
                        // sparewithPrice.TotalDiscountForSpare = TotalDiscountSpare.ToString();
                        sparewithPrice.TotalDiscountForService = DisplayPrice(TotalDiscountService).ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TaxableAmount = DisplayPrice(TotalTaxableAmountForService).ToString();
                        jobCardSpare.Services.Add(sparewithPrice);
                    }
                }

                //Step 2
                FinalTotalCGSTAmount += TotalCGSTAmount;
                jobCardSpare.FinalTotalCGSTAmount = FinalTotalCGSTAmount.ToString();
                //dbCon.InsertLogs("JobCard Spare COunt :: " + jobCardSpare.Spares.Count);
                jobCardSpare.resultflag = "1";
                jobCardSpare.Message = Constant.Message.SuccessMessage;

                //        }
                //    }
                //}
                //else
                //{
                //    jobCardSpare.resultflag = "0";
                //    jobCardSpare.Message = Constant.Message.NoDataFound;
                //}


                jobCardSpare.TotalBeforeTaxService = TotalTaxableAmountForService.ToString();
                jobCardSpare.TotalBeforeTaxSpare = TotalTaxableAmountForSpare.ToString();
                jobCardSpare.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                jobCardSpare.SubTotalForService = DisplayPrice(SubTotalForService);
                jobCardSpare.SubTotalForSpare = DisplayPrice(SubTotalForSpare);
                jobCardSpare.TotalAmount = DisplayPrice(SubTotalForService + SubTotalForSpare);
                // - d_Total_DiscountSpare_Service
                jobCardSpare.FinalAmount = DisplayPrice(SubTotalForService + SubTotalForSpare);
                return jobCardSpare;
            }
            catch (Exception ex)
            {
                dbCon.InsertLogs("GetJobCardSpareInJobCardForInvoice : " + ex.ToString(), ex.StackTrace.ToString(), ex.Message.ToString());
                return new ServiceClass.JobCardSpareForInvoice();
            }
        }

        //Proforma By Id For History
        public ServiceClass.JobCardSpareForInvoice GetJobCardSpareInJobCardForInvoiceV3_new_V1_ProformaById(string JobCardId, string Spare = "", string Service = "", int type = 1, bool fromEstimate = false, bool ShowAllocated = false, int ProformaInvoiceId = 0)
        {
            ServiceClass.JobCardSpareForInvoice jobCardSpare = new ServiceClass.JobCardSpareForInvoice();
            try
            {
                Decimal num1 = new Decimal(0);
                Decimal num2 = new Decimal(0);
                int Jobcardid = Convert.ToInt32(JobCardId);
                string InvoiceId = "0";
                string[] strArrSpare = Spare.Split(new string[] { "$#@" }, StringSplitOptions.None);
                string[] strArrService = Service.Split(new string[] { "$#@" }, StringSplitOptions.None);

                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("SELECT Invoice_Spare_Mapping.Id,ISNULL(Spare.Name, '') AS Name, ISNULL(Spare.HSNNumber, '') AS HSN, Estimate.Id as InvoiceId,Invoice_Spare_Mapping.SpareId, ISNULL(Invoice_Spare_Mapping.CGSTValue, 0) AS CGSTValue, ISNULL(Invoice_Spare_Mapping.CGSTAmount, 0) AS CGSTAmount, ISNULL(Invoice_Spare_Mapping.SGSTValue, 0) AS SGSTValue, ISNULL(Invoice_Spare_Mapping.SGSTAmount, 0) AS SGSTAmount, " + (type == 2 ? "(Select ISM.ActualAmountPerUnit from Invoice_Spare_Mapping as ISM where ISM.Id=Invoice_Spare_Mapping.RefId)" : "isnull(Invoice_Spare_Mapping.DiscountPerUnit,0)") + "  as DiscountPerUnit,  " + (type == 2 ? "(Select ISM.Mrp from Invoice_Spare_Mapping as ISM where ISM.Id=Invoice_Spare_Mapping.RefId)" : " Invoice_Spare_Mapping.ActualAmountPerUnit ") + " as ActualAmountPerUnit , Invoice_Spare_Mapping.Quantity, Estimate.TotalServiceAmount, Estimate.TotalSpareAmount,isnull(Invoice_Spare_Mapping.TotalDiscount,0) as TotalDiscount,0 as PushToInsurance ,0 as InsuranceAmount,Invoice_Spare_Mapping.[MappingId] as MapId FROM  Invoice_Spare_Mapping as Invoice_Spare_Mapping  INNER JOIN Invoice as Estimate ON Estimate.Id = Invoice_Spare_Mapping.InvoiceId INNER  JOIN Spare ON Spare.Id = Invoice_Spare_Mapping.SpareId WHERE Estimate.JobCardId=@1 and Estimate.Type=" + type + " and Estimate.id=" + ProformaInvoiceId + "  ORDER BY Invoice_Spare_Mapping.DOC DESC  ", new string[1]
                    {
                      JobCardId
                    });
                //Spare
                for (int i = 0; i < strArrSpare.Length; i++)
                {
                    string[] strItem = strArrSpare[i].Split(',');
                    if (strItem.Length >= 2)
                    {
                        DataRow[] drdata = dataTableWithParams.Select("id=" + strItem[0]);
                        if (drdata.Length > 0)
                        {
                            double qty = 0;
                            double.TryParse(drdata[0]["Quantity"].ToString(), out qty);

                            decimal amt = 0;
                            decimal.TryParse(drdata[0]["ActualAmountPerUnit"].ToString(), out amt);

                            bool isInPer = false;
                            if (strItem[1].Contains("%"))
                            {
                                isInPer = true;
                                strItem[1] = strItem[1].Replace("%", "");
                            }
                            if (String.IsNullOrEmpty(strItem[1]))
                            {
                                strItem[1] = "0";
                            }
                            decimal dc_DiscountPerUnit = (isInPer ? ((amt * decimal.Parse(strItem[1])) / 100) : decimal.Parse(strItem[1]));
                            decimal dc_TotalDiscount = dc_DiscountPerUnit * (decimal)qty;

                            drdata[0]["DiscountPerUnit"] = dc_DiscountPerUnit;
                            drdata[0]["TotalDiscount"] = dc_TotalDiscount;
                        }
                    }
                }
                Decimal TotalCGSTAmount = 0;
                Decimal FinalTotalCGSTAmount = 0;
                Decimal SubTotalForService = 0;
                Decimal SubTotalForSpare = 0;
                decimal d_Total_DiscountSpare_Service = 0;
                decimal TotalTaxableAmountForService = 0;
                decimal TotalTaxableAmountForSpare = 0;
                decimal TotalDiscountSpare = 0;
                decimal TotalDiscountService = 0;

                if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                {
                    ServiceClass.SparesForInvoice sparewithPrice = new ServiceClass.SparesForInvoice();
                    sparewithPrice.Name = "Spares / Consumables";
                    sparewithPrice.Type = "Header";
                    jobCardSpare.Spares.Add(sparewithPrice);
                    Decimal ActualAmountPerUnit = 0;

                    foreach (DataRow row in dataTableWithParams.Rows)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();
                        d_Total_DiscountSpare_Service += decimal.Parse(row["TotalDiscount"].ToString());
                        sparewithPrice.Id = row["SpareId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        sparewithPrice.MappingId = row["MapId"].ToString();
                        sparewithPrice.ItemType = "Spare";
                        sparewithPrice.InvoiceSpareId = row["Id"].ToString();
                        try
                        {
                            ActualAmountPerUnit = Convert.ToDecimal(row["ActualAmountPerUnit"]);
                        }
                        catch (Exception ex)
                        { }
                        Decimal Quantity = Convert.ToDecimal(row["Quantity"]);
                        num1 += ActualAmountPerUnit * Quantity;
                        // sparewithPrice.Price = ActualAmountPerUnit+Discount.ToString();

                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        decimal discount = Math.Round(Convert.ToDecimal(row["DiscountPerUnit"]), 2);
                        if (ActualAmountPerUnit <= discount)
                        {
                            discount = ActualAmountPerUnit;
                        }
                        // discount = discount * Quantity;


                        decimal totaldis = discount * Quantity;

                        sparewithPrice.Price = (ActualAmountPerUnit).ToString();
                        TotalDiscountSpare += totaldis;
                        sparewithPrice.UnitPrice = (ActualAmountPerUnit).ToString();
                        decimal MRP = (ActualAmountPerUnit - discount) * Quantity;
                        decimal TotalTax = (2 * Convert.ToDecimal(row["SGSTValue"]));
                        //   decimal TaxAmount = (ActualAmountPerUnit - discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);
                        decimal TaxAmount = (((MRP) * (TotalTax) / (TotalTax + 100)) / 2);
                        //  decimal TaxAmount = Convert.ToDecimal(row["CGSTAmount"]) * Quantity;
                        //   decimal TaxAmount = (ActualAmountPerUnit - discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);
                        //(Total Amount ) - ((Total Amount) * GST) / GST + 100)  
                        //  decimal TaxAmount =   ((ActualAmountPerUnit - discount) * Quantity)  - (((ActualAmountPerUnit - discount) * Quantity) * (2*Convert.ToDecimal(row["SGSTValue"])) /((2*Convert.ToDecimal(row["SGSTValue"])) + 100)) ;

                        //(((ActualAmountPerUnit - discount) * Quantity) * ((Convert.ToDecimal(row["SGSTValue"])) / (100 + ((Convert.ToDecimal(row["SGSTValue"]))))));
                        sparewithPrice.Discount = discount.ToString();
                        sparewithPrice.SGSTRate = row["SGSTValue"].ToString();
                        sparewithPrice.CGSTRate = row["CGSTValue"].ToString();
                        //sparewithPrice.SGSTAmount = DisplayPrice(Convert.ToDecimal(row["SGSTAmount"].ToString())* Quantity).ToString();
                        //sparewithPrice.CGSTAmount = DisplayPrice(Convert.ToDecimal(row["CGSTAmount"].ToString()) * Quantity).ToString();
                        sparewithPrice.SGSTAmount = DisplayPrice(TaxAmount);
                        sparewithPrice.CGSTAmount = DisplayPrice(TaxAmount);
                        TotalCGSTAmount += TaxAmount;
                        decimal TotalDiscount = (totaldis);


                        //
                        decimal TaxableAmount = ((ActualAmountPerUnit - discount) * Quantity) - (((ActualAmountPerUnit - discount) * Quantity) * (2 * (Convert.ToDecimal(row["SGSTValue"])) / (100 + (2 * (Convert.ToDecimal(row["SGSTValue"]))))));//Math.Round((((ActualAmountPerUnit * Quantity) - (2 * TaxAmount) )), 2);
                        TotalTaxableAmountForSpare += TaxableAmount;
                        decimal FinalAmont = ((ActualAmountPerUnit - discount) * Quantity);//- TotalDiscount), 2);
                        //   FinalAmont = FinalAmont - discount;
                        if (TaxableAmount < 0)
                        {
                            TaxableAmount = 0;
                        }

                        if (FinalAmont < 0)
                        {
                            FinalAmont = 0;
                        }
                        sparewithPrice.TaxableAmount = DisplayPrice(TaxableAmount).ToString();
                        sparewithPrice.FinalAmount = FinalAmont.ToString();
                        decimal amount = ((ActualAmountPerUnit - discount) * Quantity);
                        //  amount = amount - discount;
                        if (amount < 0)
                        {
                            amount = 0;
                        }
                        SubTotalForSpare += amount;
                        //sparewithPrice.TaxableAmount=taxableamount.ToString();
                        sparewithPrice.HSNNumber = row["HSN"].ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        //   sparewithPrice.SubTotal = row[""].ToString();
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    if (jobCardSpare.Spares.Count > 1)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();
                        sparewithPrice.Name = "SubTotalForSpare";
                        sparewithPrice.Type = "Header";
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.SubTotal = DisplayPrice(SubTotalForSpare).ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TaxableAmount = DisplayPrice(TotalTaxableAmountForSpare);
                        sparewithPrice.TotalDiscountForSpare = DisplayPrice(TotalDiscountSpare).ToString();
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    jobCardSpare.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                    jobCardSpare.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                    jobCardSpare.resultflag = "1";
                    jobCardSpare.Message = Constant.Message.SuccessMessage;
                }
                else
                {
                    jobCardSpare.resultflag = "0";
                    jobCardSpare.Message = Constant.Message.NoDataFound;
                }
                FinalTotalCGSTAmount = TotalCGSTAmount;
                TotalCGSTAmount = 0;
                //Step 1
                DataTable datatable = this.dbCon.GetDataTableWithParams("SELECT distinct Categoryid from JobCard_Service_Mapping inner join [Service] on Service.Id=JobCard_Service_Mapping.ServiceId where JobCardId=@1", new string[1] { JobCardId });

                DataTable dataTableWithParams2 = this.dbCon.GetDataTableWithParams("SELECT Estimate_Service_Mapping.Id,ISNULL(Service.Name, '') AS Name, ISNULL(Service.HSNNumber, '') AS HSN, Estimate.Id as InvoiceId,Estimate_Service_Mapping.ServiceId, ISNULL(Estimate_Service_Mapping.CGSTValue, 0) AS CGSTValue, ISNULL(Estimate_Service_Mapping.CGSTAmount, 0) AS CGSTAmount, ISNULL(Estimate_Service_Mapping.SGSTValue, 0) AS SGSTValue, ISNULL(Estimate_Service_Mapping.SGSTAmount, 0) AS SGSTAmount,  " + (type == 2 ? "(Select ISM.ActualAmountPerUnit from Invoice_Service_Mapping as ISM where ISM.Id=Estimate_Service_Mapping.RefId)" : "isnull(Estimate_Service_Mapping.DiscountPerUnit,0)") + " as DiscountPerUnit, " + (type == 2 ? "(Select ISM.ActualAmountPerUnit+Estimate_Service_Mapping.ActualAmountPerUnit from Invoice_Service_Mapping as ISM where ISM.Id=Estimate_Service_Mapping.RefId)" : " Estimate_Service_Mapping.ActualAmountPerUnit ") + "  as ActualAmountPerUnit , Estimate_Service_Mapping.Quantity, Estimate.TotalServiceAmount, Estimate.TotalSpareAmount,isnull(Estimate_Service_Mapping.TotalDiscount,0) as TotalDiscount,Estimate_Service_Mapping.[MappingId] as MapId FROM  Invoice_Service_Mapping as Estimate_Service_Mapping INNER JOIN Invoice as Estimate ON Estimate.Id = Estimate_Service_Mapping.Invoiceid INNER  JOIN Service ON Service.Id = Estimate_Service_Mapping.ServiceId WHERE Estimate.JobcardId=@1 and Estimate.Type=" + type + "  and Estimate.id=" + ProformaInvoiceId + " ", new string[1]
                {
                  JobCardId
                });

                //services

                for (int i = 0; i < strArrService.Length; i++)
                {
                    string[] strItem = strArrService[i].Split(',');
                    if (strItem.Length >= 2)
                    {

                        DataRow[] drdata = dataTableWithParams2.Select("id=" + strItem[0]);
                        if (drdata.Length > 0)
                        {
                            double qty = 0;
                            double.TryParse(drdata[0]["Quantity"].ToString(), out qty);

                            decimal amt = 0;
                            decimal.TryParse(drdata[0]["ActualAmountPerUnit"].ToString(), out amt);

                            bool isInPer = false;
                            if (strItem[1].Contains("%"))
                            {
                                isInPer = true;
                                strItem[1] = strItem[1].Replace("%", "");
                            }
                            if (String.IsNullOrEmpty(strItem[1]))
                            {
                                strItem[1] = "0";
                            }
                            decimal dc_DiscountPerUnit = (isInPer ? ((amt * decimal.Parse(strItem[1])) / 100) : decimal.Parse(strItem[1]));
                            decimal dc_TotalDiscount = dc_DiscountPerUnit * (decimal)qty;

                            drdata[0]["DiscountPerUnit"] = dc_DiscountPerUnit;
                            drdata[0]["TotalDiscount"] = dc_TotalDiscount;
                        }
                    }
                }

                if (dataTableWithParams2 != null && dataTableWithParams2.Rows.Count > 0)
                {
                    Decimal ActualAmountPerUnit = 0;
                    ServiceClass.ServicesForInvoice sparewithPrice = new ServiceClass.ServicesForInvoice();
                    sparewithPrice.Type = "Header";
                    sparewithPrice.Name = "Labour";
                    jobCardSpare.Services.Add(sparewithPrice);
                    foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams2.Rows)
                    {
                        d_Total_DiscountSpare_Service += decimal.Parse(row["TotalDiscount"].ToString());
                        sparewithPrice = new ServiceClass.ServicesForInvoice();

                        sparewithPrice.Id = row["ServiceId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        sparewithPrice.MappingId = row["MapId"].ToString();
                        sparewithPrice.ItemType = "Service";
                        sparewithPrice.InvoiceServiceId = row["Id"].ToString();
                        try
                        {
                            ActualAmountPerUnit = Convert.ToDecimal(row["ActualAmountPerUnit"]);
                        }
                        catch (Exception ex)
                        { }
                        Decimal Quantity = Convert.ToDecimal(row["Quantity"]);

                        sparewithPrice.Price = ActualAmountPerUnit.ToString();
                        sparewithPrice.HSNNumber = "998714";
                        // sparewithPrice.UnitPrice = (num3).ToString();
                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        decimal discount = Math.Round(Convert.ToDecimal(row["DiscountPerUnit"]), 2);

                        if (ActualAmountPerUnit <= discount)
                        {
                            discount = ActualAmountPerUnit;
                        }


                        sparewithPrice.Discount = (discount).ToString();
                        //  ActualAmountPerUnit = ActualAmountPerUnit ;
                        TotalDiscountService += (discount * Quantity);
                        sparewithPrice.UnitPrice = (ActualAmountPerUnit).ToString();
                        //  ActualAmountPerUnit = ActualAmountPerUnit + discount;
                        //decimal TotalDiscount = (discount );
                        decimal TotalDiscount = (discount * Quantity);
                        decimal MRP = (ActualAmountPerUnit - discount) * Quantity;
                        decimal TotalTax = (2 * Convert.ToDecimal(row["SGSTValue"]));


                        decimal TaxAmount = ((MRP) * (((TotalTax) / 2) / 100));
                        num1 += ActualAmountPerUnit * Quantity + (2 * TaxAmount);
                        //   decimal TaxAmount = (ActualAmountPerUnit - discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);
                        //   decimal TaxAmount = (((MRP) * (TotalTax) / (TotalTax + 100))/2);
                        //  decimal TaxAmount = ((ActualAmountPerUnit - discount) * Quantity) - (((ActualAmountPerUnit - discount) * Quantity) * (2 * Convert.ToDecimal(row["SGSTValue"])) / ((2 * Convert.ToDecimal(row["SGSTValue"])) + 100));
                        // decimal TaxAmount = ((ActualAmountPerUnit - discount) * Quantity)  - (((ActualAmountPerUnit - discount) * Quantity) * Convert.ToDecimal(row["SGSTValue"]) /(Convert.ToDecimal(row["SGSTValue"]) + 100)) ;//(((ActualAmountPerUnit - discount) * Quantity) * ( (Convert.ToDecimal(row["SGSTValue"])) / (100 + ( (Convert.ToDecimal(row["SGSTValue"]))))));
                        //  decimal TaxAmount = (ActualAmountPerUnit-discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);

                        //decimal TaxAmount = Convert.ToDecimal(row["CGSTAmount"]);
                        decimal TaxableAmount = ((ActualAmountPerUnit - discount) * Quantity);
                        TotalTaxableAmountForService += TaxableAmount;
                        decimal FinalAmont = (((ActualAmountPerUnit - discount) * Quantity) + (2 * TaxAmount));
                        if (TaxableAmount < 0)
                        {
                            TaxableAmount = 0;
                        }

                        if (FinalAmont < 0)
                        {
                            FinalAmont = 0;
                        }
                        //sparewithPrice.Discount = row["Discount"];
                        sparewithPrice.SGSTRate = row["SGSTValue"].ToString();
                        sparewithPrice.CGSTRate = row["CGSTValue"].ToString();
                        //sparewithPrice.SGSTAmount = DisplayPrice(Convert.ToDecimal(row["SGSTAmount"].ToString())).ToString();
                        //sparewithPrice.CGSTAmount = DisplayPrice(Convert.ToDecimal(row["CGSTAmount"].ToString())).ToString();
                        sparewithPrice.SGSTAmount = DisplayPrice(TaxAmount);
                        sparewithPrice.CGSTAmount = DisplayPrice(TaxAmount);
                        //sparewithPrice.SGSTAmount = row["SGSTAmount"].ToString();
                        //sparewithPrice.CGSTAmount = row["CGSTAmount"].ToString();

                        TotalCGSTAmount += TaxAmount;
                        sparewithPrice.TaxableAmount = DisplayPrice(TaxableAmount).ToString();
                        sparewithPrice.FinalAmount = DisplayPrice(FinalAmont).ToString();
                        SubTotalForService += FinalAmont;
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        jobCardSpare.Services.Add(sparewithPrice);
                    }
                    if (jobCardSpare.Services.Count > 1)
                    {
                        sparewithPrice = new ServiceClass.ServicesForInvoice();
                        sparewithPrice.Name = "SubTotalForService";
                        sparewithPrice.Type = "Header";
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.SubTotal = DisplayPrice(SubTotalForService).ToString();
                        // sparewithPrice.TotalDiscountForSpare = TotalDiscountSpare.ToString();
                        sparewithPrice.TotalDiscountForService = DisplayPrice(TotalDiscountService).ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TaxableAmount = DisplayPrice(TotalTaxableAmountForService).ToString();
                        jobCardSpare.Services.Add(sparewithPrice);
                    }
                }

                //Step 2
                FinalTotalCGSTAmount += TotalCGSTAmount;
                jobCardSpare.FinalTotalCGSTAmount = FinalTotalCGSTAmount.ToString();
                //dbCon.InsertLogs("JobCard Spare COunt :: " + jobCardSpare.Spares.Count);
                jobCardSpare.resultflag = "1";
                jobCardSpare.Message = Constant.Message.SuccessMessage;

                //        }
                //    }
                //}
                //else
                //{
                //    jobCardSpare.resultflag = "0";
                //    jobCardSpare.Message = Constant.Message.NoDataFound;
                //}


                jobCardSpare.TotalBeforeTaxService = TotalTaxableAmountForService.ToString();
                jobCardSpare.TotalBeforeTaxSpare = TotalTaxableAmountForSpare.ToString();
                jobCardSpare.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                jobCardSpare.SubTotalForService = DisplayPrice(SubTotalForService);
                jobCardSpare.SubTotalForSpare = DisplayPrice(SubTotalForSpare);
                jobCardSpare.TotalAmount = DisplayPrice(SubTotalForService + SubTotalForSpare);
                // - d_Total_DiscountSpare_Service
                jobCardSpare.FinalAmount = DisplayPrice(SubTotalForService + SubTotalForSpare);
                return jobCardSpare;
            }
            catch (Exception ex)
            {
                dbCon.InsertLogs("GetJobCardSpareInJobCardForInvoice : " + ex.ToString(), ex.StackTrace.ToString(), ex.Message.ToString());
                return new ServiceClass.JobCardSpareForInvoice();
            }
        }
        public ServiceClass.RCDetail GetRCDetail(int JobCardId)
        {

            var objectToSerilize = new ServiceClass.RCDetail();
            string query = "Select RTO_Name,(NULLIF(Registration_Valid_Upto,'')) as Registration_Valid_Upto,Colour,CubicCapacity,(NULLIF(Registration_Date,'')) as Registration_Date  ,Hypothesis_To , Manufacturing_Yr , Registration_On_Owner from RC_Details where JobCardId=@1 and IsActive=1 and IsDeleted=0";
            string[] param = { JobCardId.ToString() };
            DataTable dt = dbCon.GetDataTableWithParams(query, param);
            if (dt != null && dt.Rows.Count > 0)
            {
                DataRow dr = dt.Rows[0];
                objectToSerilize.resultflag = "1";
                objectToSerilize.Message = Constant.Message.SuccessMessage;
                objectToSerilize.RTOName = dr["RTO_Name"].ToString();
                if (!String.IsNullOrEmpty(dr["Registration_Valid_Upto"].ToString()))
                    objectToSerilize.RegValidDt = DOBFormat(dr["Registration_Valid_Upto"].ToString());
                objectToSerilize.Color = dr["Colour"].ToString();
                objectToSerilize.CubicCapacity = dr["CubicCapacity"].ToString();
                if (!String.IsNullOrEmpty(dr["Registration_Date"].ToString()))
                    objectToSerilize.DateOfRegistration = DOBFormat(dr["Registration_Date"].ToString());

                objectToSerilize.HypothecatedTo = dr["Hypothesis_To"].ToString();
                objectToSerilize.ModelYear = dr["Manufacturing_Yr"].ToString();
                objectToSerilize.RegistrationOwner = dr["Registration_On_Owner"].ToString();

            }
            else
            {
                objectToSerilize.resultflag = "0";
                objectToSerilize.Message = Constant.Message.NoDataFound;
            }
            return objectToSerilize;
        }
        public ServiceClass.JobCardSpareForInvoice GetJobCardSpareInJobCardForInvoiceV3(string JobCardId, int type = 1)
        {
            ServiceClass.JobCardSpareForInvoice jobCardSpare = new ServiceClass.JobCardSpareForInvoice();
            try
            {
                Decimal num1 = new Decimal(0);
                Decimal num2 = new Decimal(0);
                int Jobcardid = Convert.ToInt32(JobCardId);
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("SELECT Invoice_Spare_Mapping.Id,ISNULL(Spare.Name, '') AS Name, ISNULL(Spare.HSNNumber, '') AS HSN, Invoice_Spare_Mapping.InvoiceId,Invoice_Spare_Mapping.SpareId, ISNULL(Invoice_Spare_Mapping.CGSTValue, 0) AS CGSTValue, ISNULL(Invoice_Spare_Mapping.CGSTAmount, 0) AS CGSTAmount, ISNULL(Invoice_Spare_Mapping.SGSTValue, 0) AS SGSTValue, ISNULL(Invoice_Spare_Mapping.SGSTAmount, 0) AS SGSTAmount,  Invoice_Spare_Mapping.DiscountPerUnit, Invoice_Spare_Mapping.ActualAmountPerUnit, Invoice_Spare_Mapping.Quantity, Invoice.TotalServiceAmount, Invoice.TotalSpareAmount,isnull(Invoice_Spare_Mapping.TotalDiscount,0) as TotalDiscount FROM  Invoice_Spare_Mapping INNER JOIN Invoice ON Invoice.Id = Invoice_Spare_Mapping.InvoiceId INNER  JOIN Spare ON Spare.Id = Invoice_Spare_Mapping.SpareId WHERE (Invoice.JobCardId = @1) and Invoice.type=" + type + " ORDER BY Invoice_Spare_Mapping.DOC DESC ", new string[1]
                {
                  JobCardId
                });
                //        DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("SELECT Estimate_Spare_Mapping.Id,ISNULL(Spare.Name, '') AS Name, ISNULL(Spare.HSNNumber, '') AS HSN, Estimate_Spare_Mapping.EstimateId as InvoiceId,Estimate_Spare_Mapping.SpareId, ISNULL(Estimate_Spare_Mapping.CGSTValue, 0) AS CGSTValue, ISNULL(Estimate_Spare_Mapping.CGSTAmount, 0) AS CGSTAmount, ISNULL(Estimate_Spare_Mapping.SGSTValue, 0) AS SGSTValue, ISNULL(Estimate_Spare_Mapping.SGSTAmount, 0) AS SGSTAmount,  Estimate_Spare_Mapping.DiscountPerUnit, Estimate_Spare_Mapping.ActualAmountPerUnit, Estimate_Spare_Mapping.Quantity, Estimate.TotalServiceAmount, Estimate.TotalSpareAmount,isnull(Estimate_Spare_Mapping.TotalDiscount,0) as TotalDiscount FROM  Estimate_Spare_Mapping INNER JOIN Estimate ON Estimate.Id = Estimate_Spare_Mapping.EstimateId INNER  JOIN Spare ON Spare.Id = Estimate_Spare_Mapping.SpareId WHERE (Estimate.id in (Select top 1 id from Estimate where JobCardId=@1 order by id desc )) and Isnull(Estimate_Spare_Mapping.isDeleted,0)=0  ORDER BY Estimate_Spare_Mapping.DOC DESC ", new string[1]
                //{
                //  JobCardId
                //});
                Decimal TotalCGSTAmount = 0;
                Decimal SubTotalForService = 0;
                Decimal SubTotalForSpare = 0;
                decimal d_Total_DiscountSpare_Service = 0;
                decimal TotalTaxableAmountForService = 0;
                decimal TotalTaxableAmountForSpare = 0;
                decimal TotalDiscountSpare = 0;
                decimal TotalDiscountService = 0;

                if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                {
                    ServiceClass.SparesForInvoice sparewithPrice = new ServiceClass.SparesForInvoice();
                    sparewithPrice.Name = "Spares / Consumables";
                    sparewithPrice.Type = "Header";
                    jobCardSpare.Spares.Add(sparewithPrice);
                    Decimal ActualAmountPerUnit = 0;

                    foreach (DataRow row in dataTableWithParams.Rows)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();
                        d_Total_DiscountSpare_Service += decimal.Parse(row["TotalDiscount"].ToString());
                        sparewithPrice.Id = row["SpareId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        sparewithPrice.ItemType = "Spare";
                        sparewithPrice.InvoiceSpareId = row["Id"].ToString();
                        try
                        {
                            ActualAmountPerUnit = Convert.ToDecimal(row["ActualAmountPerUnit"]);
                        }
                        catch (Exception ex)
                        { }
                        Decimal Quantity = Convert.ToDecimal(row["Quantity"]);
                        num1 += ActualAmountPerUnit * Quantity;
                        // sparewithPrice.Price = ActualAmountPerUnit+Discount.ToString();

                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        decimal discount = Math.Round(Convert.ToDecimal(row["DiscountPerUnit"]), 2);




                        //discount = discount * Quantity;



                        decimal totaldis = discount * Quantity;
                        sparewithPrice.Price = (ActualAmountPerUnit).ToString();
                        TotalDiscountSpare += totaldis;
                        sparewithPrice.UnitPrice = (ActualAmountPerUnit).ToString();
                        //  decimal TaxAmount = Convert.ToDecimal(row["CGSTAmount"]) * Quantity;
                        decimal MRP = (ActualAmountPerUnit - discount) * Quantity;
                        decimal TotalTax = (2 * Convert.ToDecimal(row["SGSTValue"]));
                        //   decimal TaxAmount = (ActualAmountPerUnit - discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);
                        decimal TaxAmount = (((MRP) * (TotalTax) / (TotalTax + 100)) / 2);
                        //  decimal TaxAmount = (((ActualAmountPerUnit - discount) * Quantity) * ((Convert.ToDecimal(row["SGSTValue"]) / (100 + ((Convert.ToDecimal(row["SGSTValue"])))))));
                        sparewithPrice.Discount = discount.ToString();
                        sparewithPrice.SGSTRate = row["SGSTValue"].ToString();
                        sparewithPrice.CGSTRate = row["CGSTValue"].ToString();
                        //sparewithPrice.SGSTAmount = DisplayPrice(Convert.ToDecimal(row["SGSTAmount"].ToString())* Quantity).ToString();
                        //sparewithPrice.CGSTAmount = DisplayPrice(Convert.ToDecimal(row["CGSTAmount"].ToString()) * Quantity).ToString();
                        sparewithPrice.SGSTAmount = DisplayPrice(TaxAmount);
                        sparewithPrice.CGSTAmount = DisplayPrice(TaxAmount);
                        TotalCGSTAmount += TaxAmount;
                        decimal TotalDiscount = (totaldis);


                        //
                        decimal TaxableAmount = ((ActualAmountPerUnit - discount) * Quantity) - (((ActualAmountPerUnit - discount) * Quantity) * (2 * (Convert.ToDecimal(row["SGSTValue"])) / (100 + (2 * (Convert.ToDecimal(row["SGSTValue"]))))));// Math.Round((((ActualAmountPerUnit * Quantity) - (2 * TaxAmount))), 2);
                        TotalTaxableAmountForSpare += TaxableAmount;
                        decimal FinalAmont = (((ActualAmountPerUnit - discount) * Quantity));
                        //   FinalAmont = FinalAmont - discount;
                        if (TaxableAmount < 0)
                        {
                            TaxableAmount = 0;
                        }

                        if (FinalAmont < 0)
                        {
                            FinalAmont = 0;
                        }
                        sparewithPrice.TaxableAmount = DisplayPrice(TaxableAmount).ToString();
                        sparewithPrice.FinalAmount = DisplayPrice(FinalAmont).ToString();
                        decimal amount = ((ActualAmountPerUnit - discount) * Quantity);
                        //  amount = amount - discount;
                        if (amount < 0)
                        {
                            amount = 0;
                        }
                        SubTotalForSpare += amount;
                        //sparewithPrice.TaxableAmount=taxableamount.ToString();
                        sparewithPrice.HSNNumber = row["HSN"].ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        //   sparewithPrice.SubTotal = row[""].ToString();
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    if (jobCardSpare.Spares.Count > 1)
                    {
                        sparewithPrice = new ServiceClass.SparesForInvoice();
                        sparewithPrice.Name = "SubTotalForSpare";
                        sparewithPrice.Type = "Header";
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.SubTotal = DisplayPrice(SubTotalForSpare).ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TaxableAmount = DisplayPrice(TotalTaxableAmountForSpare);

                        sparewithPrice.TotalDiscountForSpare = DisplayPrice(TotalDiscountSpare).ToString();

                        jobCardSpare.Spares.Add(sparewithPrice);
                    }
                    jobCardSpare.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                    jobCardSpare.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                    jobCardSpare.resultflag = "1";
                    jobCardSpare.Message = Constant.Message.SuccessMessage;
                }
                else
                {
                    jobCardSpare.resultflag = "0";
                    jobCardSpare.Message = Constant.Message.NoDataFound;
                }

                TotalCGSTAmount = 0;
                //DataTable datatable = this.dbCon.GetDataTableWithParams("SELECT distinct Categoryid from JobCard_Service_Mapping inner join [Service] on Service.Id=JobCard_Service_Mapping.ServiceId where JobCardId=@1", new string[1] { JobCardId });

                //DataTable dataTableWithParams2 = this.dbCon.GetDataTableWithParams("SELECT Estimate_Service_Mapping.Id,ISNULL(Service.Name, '') AS Name, ISNULL(Service.HSNNumber, '') AS HSN, Estimate_Service_Mapping.EstimateId as InvoiceId,Estimate_Service_Mapping.ServiceId, ISNULL(Estimate_Service_Mapping.CGSTValue, 0) AS CGSTValue, ISNULL(Estimate_Service_Mapping.CGSTAmount, 0) AS CGSTAmount, ISNULL(Estimate_Service_Mapping.SGSTValue, 0) AS SGSTValue, ISNULL(Estimate_Service_Mapping.SGSTAmount, 0) AS SGSTAmount,  Estimate_Service_Mapping.DiscountPerUnit, Estimate_Service_Mapping.ActualAmountPerUnit, Estimate_Service_Mapping.Quantity, Estimate.TotalServiceAmount, Estimate.TotalSpareAmount,isnull(Estimate_Service_Mapping.TotalDiscount,0) as TotalDiscount FROM  Estimate_Service_Mapping INNER JOIN Estimate ON Estimate.Id = Estimate_Service_Mapping.EstimateId INNER  JOIN Service ON Service.Id = Estimate_Service_Mapping.ServiceId WHERE (Estimate.id in (Select top 1 id from Estimate where JobCardId=@1 order by id desc )) and Isnull(Estimate_Spare_Mapping.isDeleted,0)=0 ORDER BY Estimate_Service_Mapping.DOC DESC", new string[1]
                //{
                //  JobCardId
                //});


                DataTable datatable = this.dbCon.GetDataTableWithParams("SELECT distinct Categoryid from JobCard_Service_Mapping inner join [Service] on Service.Id=JobCard_Service_Mapping.ServiceId where JobCardId=@1", new string[1] { JobCardId });

                DataTable dataTableWithParams2 = this.dbCon.GetDataTableWithParams("SELECT Invoice_Service_Mapping.Id,ISNULL(Service.Name, '') AS Name, ISNULL(Service.HSNNumber, '') AS HSN, Invoice_Service_Mapping.InvoiceId,Invoice_Service_Mapping.ServiceId, ISNULL(Invoice_Service_Mapping.CGSTValue, 0) AS CGSTValue, ISNULL(Invoice_Service_Mapping.CGSTAmount, 0) AS CGSTAmount, ISNULL(Invoice_Service_Mapping.SGSTValue, 0) AS SGSTValue, ISNULL(Invoice_Service_Mapping.SGSTAmount, 0) AS SGSTAmount,  Invoice_Service_Mapping.DiscountPerUnit, Invoice_Service_Mapping.ActualAmountPerUnit, Invoice_Service_Mapping.Quantity, Invoice.TotalServiceAmount, Invoice.TotalSpareAmount,isnull(Invoice_Service_Mapping.TotalDiscount,0) as TotalDiscount FROM  Invoice_Service_Mapping INNER JOIN Invoice ON Invoice.Id = Invoice_Service_Mapping.InvoiceId INNER  JOIN Service ON Service.Id = Invoice_Service_Mapping.ServiceId WHERE (Invoice.JobCardId = @1) and Invoice.type=" + type + " ORDER BY Invoice_Service_Mapping.DOC DESC", new string[1]
                {
                  JobCardId
                });


                if (dataTableWithParams2 != null && dataTableWithParams2.Rows.Count > 0)
                {
                    Decimal ActualAmountPerUnit = 0;
                    ServiceClass.ServicesForInvoice sparewithPrice = new ServiceClass.ServicesForInvoice();
                    sparewithPrice.Type = "Header";
                    sparewithPrice.Name = "Labour";
                    jobCardSpare.Services.Add(sparewithPrice);
                    foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams2.Rows)
                    {
                        d_Total_DiscountSpare_Service += decimal.Parse(row["TotalDiscount"].ToString());
                        sparewithPrice = new ServiceClass.ServicesForInvoice();

                        sparewithPrice.Id = row["ServiceId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        sparewithPrice.ItemType = "Service";
                        sparewithPrice.InvoiceServiceId = row["Id"].ToString();
                        try
                        {
                            ActualAmountPerUnit = Convert.ToDecimal(row["ActualAmountPerUnit"]);
                        }
                        catch (Exception ex)
                        { }
                        Decimal Quantity = Convert.ToDecimal(row["Quantity"]);

                        sparewithPrice.Price = ActualAmountPerUnit.ToString();
                        sparewithPrice.HSNNumber = "998714";
                        // sparewithPrice.UnitPrice = (num3).ToString();
                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        decimal discount = Math.Round(Convert.ToDecimal(row["DiscountPerUnit"]), 2);




                        sparewithPrice.Discount = (discount).ToString();
                        //  ActualAmountPerUnit = ActualAmountPerUnit ;
                        TotalDiscountService += (discount * Quantity);
                        sparewithPrice.UnitPrice = (ActualAmountPerUnit).ToString();
                        //  ActualAmountPerUnit = ActualAmountPerUnit + discount;
                        //decimal TotalDiscount = (discount );
                        decimal TotalDiscount = (discount * Quantity);
                        decimal MRP = (ActualAmountPerUnit - discount) * Quantity;
                        decimal TotalTax = (2 * Convert.ToDecimal(row["SGSTValue"]));
                        //   decimal TaxAmount = (ActualAmountPerUnit - discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);
                        //   decimal TaxAmount = (((MRP) * (TotalTax) / (TotalTax + 100)) / 2);

                        decimal TaxAmount = ((MRP) * (((TotalTax) / 2) / 100));
                        num1 += (ActualAmountPerUnit * Quantity) + (2 * TaxAmount);
                        // decimal TaxAmount = (ActualAmountPerUnit - discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);
                        //decimal TaxAmount = ((ActualAmountPerUnit - discount) * Quantity) - (((ActualAmountPerUnit - discount) * Quantity) * (2 * Convert.ToDecimal(row["SGSTValue"])) / ((2 * Convert.ToDecimal(row["SGSTValue"])) + 100)); //decimal TaxAmount = (((ActualAmountPerUnit - discount) * Quantity) * ((Convert.ToDecimal(row["SGSTValue"]) / (100 + ((Convert.ToDecimal(row["SGSTValue"])))))));
                        //decimal TaxAmount = Convert.ToDecimal(row["CGSTAmount"]);
                        decimal TaxableAmount = (((ActualAmountPerUnit - discount) * Quantity));
                        TotalTaxableAmountForService += TaxableAmount;
                        decimal FinalAmont = (((ActualAmountPerUnit - discount) * Quantity) + (2 * TaxAmount));
                        if (TaxableAmount < 0)
                        {
                            TaxableAmount = 0;
                        }

                        if (FinalAmont < 0)
                        {
                            FinalAmont = 0;
                        }
                        //sparewithPrice.Discount = row["Discount"];
                        sparewithPrice.SGSTRate = row["SGSTValue"].ToString();
                        sparewithPrice.CGSTRate = row["CGSTValue"].ToString();
                        //sparewithPrice.SGSTAmount = DisplayPrice(Convert.ToDecimal(row["SGSTAmount"].ToString())).ToString();
                        //sparewithPrice.CGSTAmount = DisplayPrice(Convert.ToDecimal(row["CGSTAmount"].ToString())).ToString();
                        sparewithPrice.SGSTAmount = DisplayPrice(TaxAmount);
                        sparewithPrice.CGSTAmount = DisplayPrice(TaxAmount);
                        //sparewithPrice.SGSTAmount = row["SGSTAmount"].ToString();
                        //sparewithPrice.CGSTAmount = row["CGSTAmount"].ToString();

                        TotalCGSTAmount += TaxAmount;
                        sparewithPrice.TaxableAmount = DisplayPrice(TaxableAmount).ToString();
                        sparewithPrice.FinalAmount = DisplayPrice(FinalAmont).ToString();
                        SubTotalForService += FinalAmont;
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        jobCardSpare.Services.Add(sparewithPrice);
                    }
                    if (jobCardSpare.Spares.Count > 1)
                    {
                        sparewithPrice = new ServiceClass.ServicesForInvoice();
                        sparewithPrice.Name = "SubTotalForService";
                        sparewithPrice.Type = "Header";
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.SubTotal = DisplayPrice(SubTotalForService).ToString();
                        // sparewithPrice.TotalDiscountForSpare = TotalDiscountSpare.ToString();
                        sparewithPrice.TotalDiscountForService = DisplayPrice(TotalDiscountService).ToString();
                        sparewithPrice.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                        sparewithPrice.TaxableAmount = DisplayPrice(TotalTaxableAmountForService).ToString();
                        jobCardSpare.Services.Add(sparewithPrice);
                    }
                }
               // dbCon.InsertLogs("JobCard Spare COunt :: " + jobCardSpare.Spares.Count);
                jobCardSpare.resultflag = "1";
                jobCardSpare.Message = Constant.Message.SuccessMessage;

                //        }
                //    }
                //}
                //else
                //{
                //    jobCardSpare.resultflag = "0";
                //    jobCardSpare.Message = Constant.Message.NoDataFound;
                //}


                jobCardSpare.TotalBeforeTaxService = DisplayPrice(TotalTaxableAmountForService).ToString();
                jobCardSpare.TotalBeforeTaxSpare = DisplayPrice(TotalTaxableAmountForSpare).ToString();
                jobCardSpare.TotalDiscount = DisplayPrice(d_Total_DiscountSpare_Service);
                jobCardSpare.SubTotalForService = DisplayPrice(SubTotalForService);
                jobCardSpare.SubTotalForSpare = DisplayPrice(SubTotalForSpare);
                jobCardSpare.TotalAmount = DisplayPrice(SubTotalForService + SubTotalForSpare);
                //- d_Total_DiscountSpare_Service
                jobCardSpare.FinalAmount = DisplayPrice(SubTotalForService + SubTotalForSpare);
                //  jobCardSpare.FinalAmount = DisplayPrice(num1 - d_Total_DiscountSpare_Service).ToString();
                return jobCardSpare;
            }
            catch (Exception ex)
            {
                dbCon.InsertLogs("GetJobCardSpareInJobCardForInvoice : " + ex.ToString(), ex.StackTrace.ToString(), ex.Message.ToString());
                return new ServiceClass.JobCardSpareForInvoice();
            }
        }

        public int InsertJobCardInvoiceDetail(int jobcardid, int InvoiceNumber, string InvoiceUrl, string InvoiceType)
        {
            int result = 0;
            if (jobcardid > 0 && InvoiceNumber > 0 && !String.IsNullOrEmpty(InvoiceUrl))
            {
                string query = "INSERT INTO [dbo].[JobCard_Invoice]([JobCardId],[Invoice_Number],[InvoiceUrl],[DOC],[DOM]";
                if (!String.IsNullOrEmpty(InvoiceType))
                {
                    query += ", InvoiceType";
                }
                query += " ) VALUES (@1,@2,@3,CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME),CAST(SWITCHOFFSET(SYSDATETIMEOFFSET(), '+05:30') AS DATETIME)";
                if (!String.IsNullOrEmpty(InvoiceType))
                {
                    query += ", @4";
                }

                query += ")";
                string[] param = { jobcardid.ToString(), InvoiceNumber.ToString(), InvoiceUrl.Replace("'", "''"), InvoiceType };
                result = dbCon.ExecuteQueryWithParams(query, param);

            }
            return result;
        }

        public ServiceClass.JobCardSpareForInvoice GetJobCardSpareInJobCardForInvoice_Testing(string JobCardId)
        {
            ServiceClass.JobCardSpareForInvoice jobCardSpare = new ServiceClass.JobCardSpareForInvoice();
            try
            {
                Decimal num1 = new Decimal(0);
                Decimal num2 = new Decimal(0);
                int Jobcardid = Convert.ToInt32(JobCardId);
                DataTable dataTableWithParams = this.dbCon.GetDataTableWithParams("SELECT JobCard_Spare_Mapping.Id , SpareId,Isnull(JobCardDetailId, 0) as JobCardDetailId,Isnull(Spare.TaxId,0) AS TaxId ,Quantity,AmounT,Isnull(HSNNumber,'') as HSN,Name FROM JobCard_Spare_Mapping iNNER JOIN Spare ON Spare.ID=SpareId WHERE JobCard_Spare_Mapping.IsDeleted=0  AND JobCardId=@1 Order by JobCard_Spare_Mapping.DOC DESC", new string[1]
        {
          JobCardId
        });


                Decimal TotalCGSTAmount = 0;
                Decimal SubTotalForService = 0;
                Decimal SubTotalForSpare = 0;
                Decimal TotalSGSTAmountFinal = 0;
                if (dataTableWithParams != null && dataTableWithParams.Rows.Count > 0)
                {

                    Decimal ActualAmountPerUnit = 0;

                    foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams.Rows)
                    {
                        ServiceClass.SparesForInvoice sparewithPrice = new ServiceClass.SparesForInvoice();

                        sparewithPrice.Id = row["SpareId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();

                        try
                        {
                            ActualAmountPerUnit = Convert.ToDecimal(row["Amount"]);
                        }
                        catch (Exception ex)
                        { }

                        Decimal Quantity = Convert.ToDecimal(row["Quantity"]);
                        num1 += ActualAmountPerUnit * Quantity;
                        sparewithPrice.Price = num1.ToString();
                        sparewithPrice.UnitPrice = (ActualAmountPerUnit).ToString();
                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        int MappingId = Convert.ToInt32(row["Id"]);
                        int SpareId = Convert.ToInt32(row["SpareId"]);
                        int TaxId = Convert.ToInt32(row["TaxId"]);
                        decimal tax = TaxValueFromId(TaxId);
                        //     decimal discount = Math.Round(Convert.ToDecimal(row["DiscountPerUnit"]), 2);

                        decimal taxableamount = 0;

                        decimal discount = 0;
                        decimal totaldis = discount * Quantity;
                        decimal MRP = (ActualAmountPerUnit - discount) * Quantity;
                        decimal TotalTax = (tax);
                        //   decimal TaxAmount = (ActualAmountPerUnit - discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);
                        decimal TaxAmount = (((MRP) * (TotalTax) / (TotalTax + 100)) / 2);

                        //
                        decimal TaxRate = (tax / 2);
                        //  decimal TaxAmount = ((ActualAmountPerUnit * num4) * (TaxRate / 100));
                        //   decimal TaxableAmount = ((ActualAmountPerUnit * Quantity) - discount - (TaxAmount *2) );
                        // decimal FinalAmont = ((ActualAmountPerUnit * Quantity) - discount);
                        decimal FinalAmont = Math.Round(MRP, 2);
                        decimal TaxableAmount = (MRP) - (2 * TaxAmount);// Math.Round((((ActualAmountPerUnit * Quantity) - (2 * TaxAmount))), 2);

                        sparewithPrice.MappingId = MappingId.ToString();
                        sparewithPrice.Discount = DisplayPrice(discount).ToString();
                        sparewithPrice.SGSTRate = DisplayPrice(TaxRate).ToString();
                        sparewithPrice.CGSTRate = DisplayPrice(TaxRate).ToString();
                        sparewithPrice.SGSTAmount = DisplayPrice(TaxAmount).ToString();
                        sparewithPrice.CGSTAmount = DisplayPrice(TaxAmount).ToString();
                        TotalCGSTAmount += TaxAmount;
                        TotalSGSTAmountFinal += TaxAmount;
                        sparewithPrice.TaxableAmount = DisplayPrice(TaxableAmount).ToString();
                        sparewithPrice.FinalAmount = DisplayPrice(FinalAmont).ToString();

                        SubTotalForSpare += FinalAmont;
                        //sparewithPrice.TaxableAmount=taxableamount.ToString();
                        sparewithPrice.HSNNumber = row["HSN"].ToString();

                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }

                    jobCardSpare.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                    jobCardSpare.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                    jobCardSpare.resultflag = "1";
                    jobCardSpare.Message = Constant.Message.SuccessMessage;
                }
                else
                {
                    jobCardSpare.resultflag = "0";
                    jobCardSpare.Message = Constant.Message.NoDataFound;
                }

                DataTable dataTableWithParams1 = this.dbCon.GetDataTableWithParams("Select JobCard_Cosumable_Mapping.Id,ConsumableId,(Select Name from Consumables where Id=ConsumableId) as Name,Quantity,Amount, Isnull(JobCardDetailId, 0) as JobCardDetailId, Isnull((Select TaxId from Consumables where Id=ConsumableId),0) as TaxId from JobCard_Cosumable_Mapping where JobCardId=@1 and IsDeleted=0 order by DOC DESC", new string[1]
        {
          JobCardId
        });
                if (dataTableWithParams1 != null && dataTableWithParams1.Rows.Count > 0)
                {

                    foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams1.Rows)
                    {
                        ServiceClass.SparesForInvoice sparewithPrice = new ServiceClass.SparesForInvoice();
                        sparewithPrice.Id = row["ConsumableId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();
                        Decimal ActualAmountPerUnit = Convert.ToDecimal(row["Amount"]);
                        Decimal Quantity = Convert.ToDecimal(row["Quantity"]);
                        //   num1 += ActualAmountPerUnit * Quantity;
                        sparewithPrice.UnitPrice = (ActualAmountPerUnit).ToString();
                        sparewithPrice.Price = ActualAmountPerUnit.ToString();
                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        int TaxId = Convert.ToInt32(row["TaxId"]);

                        decimal tax = TaxValueFromId(TaxId);
                        decimal taxableamount = 0;
                        decimal discount = 0;

                        decimal TaxRate = (tax / 2);
                        //decimal TaxAmount = ((ActualAmountPerUnit * Quantity) * (TaxRate / 100));
                        //decimal TaxableAmount = ((ActualAmountPerUnit * Quantity) - discount - TaxAmount);
                        //decimal FinalAmont = ((ActualAmountPerUnit * Quantity) - discount);
                        //sparewithPrice.Price = (ActualAmountPerUnit).ToString();

                        sparewithPrice.UnitPrice = (ActualAmountPerUnit).ToString();
                        //  decimal TaxAmount = Convert.ToDecimal(row["CGSTAmount"]) * Quantity;
                        decimal MRP = (ActualAmountPerUnit - discount) * Quantity;
                        decimal TotalTax = tax;
                        //   decimal TaxAmount = (ActualAmountPerUnit - discount) * Quantity * (Convert.ToDecimal(row["SGSTValue"]) / 100);
                        decimal TaxAmount = (((MRP) * (TotalTax) / (TotalTax + 100)) / 2);

                        decimal TaxableAmount = (MRP) - (2 * TaxAmount);
                        num1 += ActualAmountPerUnit * Quantity;
                        decimal FinalAmont = Math.Round((((ActualAmountPerUnit - discount) * Quantity)), 2);
                        //   num1 += FinalAmont;
                        sparewithPrice.Discount = DisplayPrice(discount).ToString();
                        sparewithPrice.SGSTRate = DisplayPrice(TaxRate).ToString();
                        sparewithPrice.CGSTRate = DisplayPrice(TaxRate).ToString();
                        sparewithPrice.SGSTAmount = DisplayPrice(TaxAmount).ToString();
                        sparewithPrice.CGSTAmount = DisplayPrice(TaxAmount).ToString();
                        sparewithPrice.TaxableAmount = DisplayPrice(TaxableAmount).ToString();
                        sparewithPrice.FinalAmount = DisplayPrice(FinalAmont).ToString();
                        SubTotalForSpare += ((ActualAmountPerUnit * Quantity) - discount);
                        TotalSGSTAmountFinal += TaxAmount;
                        TotalCGSTAmount += TaxAmount;
                        sparewithPrice.SubTotal = DisplayPrice(SubTotalForSpare).ToString();
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        //sparewithPrice.Discount = discount.ToString();
                        //sparewithPrice.SGSTRate = (tax / 2).ToString();
                        //sparewithPrice.CGSTRate = (tax / 2).ToString();
                        //sparewithPrice.SGSTAmount = ((num3 * num4) * ((tax / 2) / 100)).ToString();
                        //sparewithPrice.CGSTAmount = ((num3 * num4) * ((tax / 2) / 100)).ToString();
                        //sparewithPrice.TaxableAmount = (((num3 * num4) - discount) -((num3 * num4) * (tax / 100))).ToString();
                        //sparewithPrice.FinalAmount = ((num3 * num4) - discount).ToString();
                        int MappingId = Convert.ToInt32(row["Id"]);
                        int SpareId = Convert.ToInt32(row["ConsumableId"]);
                        sparewithPrice.MappingId = MappingId.ToString();
                        jobCardSpare.Spares.Add(sparewithPrice);
                    }

                    jobCardSpare.resultflag = "1";
                    jobCardSpare.Message = Constant.Message.SuccessMessage;
                }
                else
                {
                    jobCardSpare.resultflag = "0";
                    jobCardSpare.Message = Constant.Message.NoDataFound;
                }
                TotalCGSTAmount = 0;

                DataTable datatable = this.dbCon.GetDataTableWithParams("SELECT distinct Categoryid from JobCard_Service_Mapping inner join [Service] on Service.Id=JobCard_Service_Mapping.ServiceId where JobCardId=@1", new string[1] { JobCardId });
                DataTable dataTableWithParams2 = this.dbCon.GetDataTableWithParams("Select JobCard_Service_Mapping.Id , ServiceId,Isnull(JobCardDetailId, 0) as JobCardDetailId,Isnull(Service.TaxId,0) AS TaxId ,Quantity,AmounT,Isnull(HSNNumber,'') as HSN,Name from JobCard_Service_Mapping  iNNER JOIN Service ON Service.ID=ServiceId   where  JobCard_Service_Mapping.IsDeleted=0  AND JobCardId=@1 Order by JobCard_Service_Mapping.DOC Desc", new string[1]
                {
                  JobCardId
                });
                //if (datatable != null && datatable.Rows.Count > 0)
                //{


                //foreach (DataRow dr in datatable.Rows)
                //{
                //// string CategoryId = dr["CategoryId"].ToString();
                // dbCon.InsertLogs("CategoryId:::" + CategoryId);
                // if (Convert.ToInt32(CategoryId) > 0)
                // {

                //Decimal num3 = 0;

                //    DataTable dataTableWithParams2 = this.dbCon.GetDataTableWithParams("Select * from (Select JobCard_Service_Mapping.Id , ServiceId,(Select top 1 [Name] from Service where Id=ServiceId) as Name,(Select CategoryId from [Service] where Service.Id=ServiceId) as ServiceCategoryId,Isnull((Select top 1 HSNNumber From [Service] where Id =ServiceId),'') as HSN,Quantity,Amount, Isnull(JobCardDetailId, 0) as JobCardDetailId, Isnull((Select TaxId from Service where Id=ServiceId and IsDeleted=0),0) as TaxId from JobCard_Service_Mapping where JobCardId=@1 and IsDeleted=0  ) as ServiceJobCardDetail where ServiceJobCardDetail.ServiceCategoryId=@2", new string[2]
                //{
                //  JobCardId,CategoryId
                //});
                if (dataTableWithParams2 != null && dataTableWithParams2.Rows.Count > 0)
                {
                    Decimal ActualAmountPerUnit = 0;

                    foreach (DataRow row in (InternalDataCollectionBase)dataTableWithParams2.Rows)
                    {
                        ServiceClass.ServicesForInvoice sparewithPrice = new ServiceClass.ServicesForInvoice();

                        sparewithPrice.Id = row["ServiceId"].ToString();
                        sparewithPrice.Name = row["Name"].ToString();

                        try
                        {
                            ActualAmountPerUnit = Convert.ToDecimal(row["Amount"]);
                        }
                        catch (Exception ex)
                        { }
                        Decimal Quantity = Convert.ToDecimal(row["Quantity"]);

                        sparewithPrice.Price = ActualAmountPerUnit.ToString();
                        sparewithPrice.HSNNumber = "998714";
                        sparewithPrice.UnitPrice = (ActualAmountPerUnit).ToString();
                        sparewithPrice.Quantity = row["Quantity"].ToString();
                        int MappingId = Convert.ToInt32(row["Id"]);
                        int SpareId = Convert.ToInt32(row["ServiceId"]);
                        int TaxId = Convert.ToInt32(row["TaxId"]);
                        sparewithPrice.MappingId = MappingId.ToString();
                        decimal discount = 0;
                        decimal tax = TaxValueFromId(TaxId);
                        if (tax == 0)
                        {
                            tax = 18;
                        }
                        decimal TotalDiscount = (discount * Quantity);
                        decimal MRP = (ActualAmountPerUnit - discount) * Quantity;
                        decimal TotalTax = (tax);
                        decimal TaxAmount = ((MRP) * (((TotalTax) / 2) / 100));

                        decimal TaxableAmount = Math.Round(MRP, 2);
                        // TotalTaxableAmountForService += TaxableAmount;
                        decimal FinalAmont = Math.Round(((MRP + (2 * TaxAmount))), 2);
                        num1 += (ActualAmountPerUnit * Quantity) + (2 * TaxAmount);
                        decimal taxableamount = 0;

                        decimal TaxRate = (tax / 2);
                        //decimal TaxAmount = ((ActualAmountPerUnit * Quantity) * (TaxRate / 100));
                        //decimal TaxableAmount = ((ActualAmountPerUnit * Quantity) - discount);
                        //decimal FinalAmont = (((ActualAmountPerUnit * Quantity) - discount + TaxAmount));

                        sparewithPrice.Discount = DisplayPrice(discount).ToString();
                        sparewithPrice.SGSTRate = DisplayPrice(TaxRate).ToString();
                        sparewithPrice.CGSTRate = DisplayPrice(TaxRate).ToString();
                        sparewithPrice.SGSTAmount = DisplayPrice(TaxAmount).ToString();
                        sparewithPrice.CGSTAmount = DisplayPrice(TaxAmount).ToString();
                        sparewithPrice.TaxableAmount = DisplayPrice(TaxableAmount).ToString();
                        sparewithPrice.FinalAmount = DisplayPrice(FinalAmont).ToString();
                        SubTotalForService += FinalAmont;
                        TotalSGSTAmountFinal += TaxAmount;
                        TotalCGSTAmount += TaxAmount;
                        sparewithPrice.TotalCGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();
                        sparewithPrice.TotalSGSTAmount = DisplayPrice(TotalCGSTAmount).ToString();

                        jobCardSpare.Services.Add(sparewithPrice);
                    }

                }
                //   dbCon.InsertLogs("JobCard Spare COunt :: " + jobCardSpare.Spares.Count);
                jobCardSpare.resultflag = "1";
                jobCardSpare.Message = Constant.Message.SuccessMessage;

                //        }
                //    }
                //}
                //else
                //{
                //    jobCardSpare.resultflag = "0";
                //    jobCardSpare.Message = Constant.Message.NoDataFound;
                //}
                jobCardSpare.TotalCGSTAmount = DisplayPrice(TotalSGSTAmountFinal).ToString();
                jobCardSpare.TotalSGSTAmount = DisplayPrice(TotalSGSTAmountFinal).ToString();
                jobCardSpare.resultflag = "1";
                jobCardSpare.Message = Constant.Message.SuccessMessage;
                jobCardSpare.FinalAmount = num1.ToString();
                return jobCardSpare;
            }
            catch (Exception ex)
            {
                dbCon.InsertLogs("GetJobCardSpareInJobCardForInvoice : " + ex.ToString(), ex.StackTrace.ToString(), ex.Message.ToString());
                return new ServiceClass.JobCardSpareForInvoice();
            }
        }

        public ServiceClass.JobCardDetail JobCardDetailById(int jobcard)
        {
            ServiceClass.JobCardDetail jobCardClass = new ServiceClass.JobCardDetail();
            try
            {
                if (jobcard > 0)
                {
                    string query = "select * from JobCard where Id=@1";
                    string[] param = { jobcard.ToString() };
                    DataTable dt = dbCon.GetDataTableWithParams(query, param);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        jobCardClass.JobCardId = jobcard.ToString();
                        jobCardClass.JobCardTypeId = dr["Type"].ToString();
                        jobCardClass.JobCardType = GetJobCardType(dr["Type"].ToString());
                        jobCardClass.JobCardStatus = GetJobStatus(dr["JobStatus_Id"].ToString());
                        jobCardClass.JobCardStatusId = dr["JobStatus_Id"].ToString();
                        jobCardClass.JobCardDate = dr["DOC"].ToString();

                        return jobCardClass;
                    }
                }
                return new ServiceClass.JobCardDetail();
            }
            catch (Exception ex)
            {
                return new ServiceClass.JobCardDetail();
            }
        }
        public string GetJobCardType(string jobcardid)
        {
            string result = "";
            try
            {
                int jobtype = 0;
                if (!String.IsNullOrEmpty(jobcardid))
                {
                    int jobcard = Convert.ToInt32(jobcardid);
                    string query = "Select Isnull([Type],0) as Type from JobCard where Id=@1";
                    string[] param = { jobcard.ToString() };
                    DataTable dt = dbCon.GetDataTableWithParams(query, param);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        DataRow dr = dt.Rows[0];
                        jobtype = Convert.ToInt32(dr["Type"]);

                    }
                }
                if (jobtype == 1)
                {
                    result = "Services";
                }
                else if (jobtype == 2)
                {
                    result = "Accident";
                }
                else if (jobtype == 3)
                {
                    result = "Running Repair";
                }
                else if (jobtype == 4)
                {
                    result = "Washing";
                }
                else if (jobtype == 5)
                {
                    result = "AMC";
                }
                else
                {
                    result = "";
                }
            }
            catch (Exception ex)
            {
                dbCon.InsertLogs(ex.ToString(), ex.StackTrace.ToString());
            }
            return result;
        }
        public bool VehicleHavingInsurance1(string JobCardId)
        {
            bool flag = false;
            try
            {
                if (!string.IsNullOrEmpty(JobCardId))
                {
                    string query = "Select Isnull(HavingInsurance,0) as HavingInsurance from Jobcard where Id=@1";
                    string[] param = { JobCardId };
                    DataTable dt = dbCon.GetDataTableWithParams(query, param);
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        flag = Convert.ToBoolean(dt.Rows[0]["HavingInsurance"]);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return flag;
        }
    }
}
