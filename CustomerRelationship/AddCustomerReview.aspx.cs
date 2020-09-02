using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CustomerRelationship_AddCustomerReview : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            WebApplication1.dbConnection dbcon = new WebApplication1.dbConnection();
            if (!String.IsNullOrWhiteSpace( Request.QueryString["Id"]))
            {
                
               
                startdate.Value = dbcon.getindiantime().AddDays(3).ToString("dd/MMM/yyyy");
                DataTable dt = dbcon.GetDataTableWithParams("SELECT  isnull(Vehicle_Model.Name,'-') +' ,'+ isnull(Vehicle_Brand.Name,'-') + ' ,'+ isnull(Vehicle.Number,'-') as Vehicle,isnull(Customer.Name,'-')+' , '+isnull(Customer.Mobile,'-') AS Customer,isnull(CustomerReviewComment,'') as Cmt,CustomerNextReviewDate as dt,jobcard.Type FROM   Customer RIGHT OUTER JOIN JobCard ON Customer.Id = JobCard.Customer_Id LEFT OUTER JOIN Vehicle LEFT OUTER JOIN Vehicle_Variant ON Vehicle.Vehicle_Variant_Id = Vehicle_Variant.Id LEFT OUTER JOIN Vehicle_Brand ON Vehicle.Vehicle_Brand_Id = Vehicle_Brand.Id LEFT OUTER JOIN Vehicle_Model ON Vehicle.Vehicle_Model_Id = Vehicle_Model.Id ON JobCard.Vehicle_Id = Vehicle.Id where JobCard.id>0 And isnull(IsGatePassGenerated,0) =1 and jobcard.id=@1", new string[] { Request.QueryString["Id"] });
                if (dt != null && dt.Rows.Count > 0)
                {
                    txtV.InnerHtml = dt.Rows[0][0].ToString();
                    txtCust.InnerHtml = dt.Rows[0][1].ToString();
                    //txtc.Value = dt.Rows[0][2].ToString();
                    try
                    {
                        //if (dt.Rows[0][3] != null)
                        //{
                        //    if (!String.IsNullOrWhiteSpace(dt.Rows[0][3].ToString()))
                        //    {
                        //        DateTime dt1 = Convert.ToDateTime(dt.Rows[0][3].ToString());
                        //        startdate.Value = dt1.ToString("dd/MMM/yyyy");
                        //        txttime.Value = dt1.ToString("hh:MM tt");
                        //    }
                        //}
                        if (dt.Rows[0][4] != null)
                        {
                            if (!String.IsNullOrWhiteSpace(dt.Rows[0][4].ToString()))
                            {
                                if (!dt.Rows[0][4].ToString().Equals("2"))
                                    lnkInsurance.Visible = false;
                            }
                        }
                    }
                    catch (Exception E) { }
                }
                dt = dbcon.GetDataTableWithParams("Select Notes from  CustomerNote where ISNULL(IsDeleted,0)=0 and JobCard_id=@1", new string[] { Request.QueryString["Id"] });
                if (dt != null && dt.Rows.Count > 0)
                {
                    string Str = "<ul style='margin-left: 5%;'>";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Str += "<li>" + dt.Rows[i][0].ToString() + "</li>";
                    }
                    Str += "</ul>";
                    txtcd.InnerHtml = Str;
                    //string Str = "";
                    //for (int i = 0; i < dt.Rows.Count; i++)
                    //{
                    //    Str += "" + dt.Rows[i][0].ToString() + (i >= (dt.Rows.Count-1)?"": ",");
                    //}
                    ////Str += "</ul>";
                    //txtcd.InnerHtml = Str;
                }
            }
            if (Request.QueryString["TEA"] != null)
            {
                txtTEA.InnerHtml = "Estimate  : ₹ " + Request.QueryString["TEA"];
            }
            if (Request.QueryString["TPA"] != null)
            {
                txtTPA.InnerHtml = "Paid   : ₹ " + Request.QueryString["TPA"];
            }

            DataTable dt1 = dbcon.GetDataTableWithParams("SELECT  CONVERT(varchar(17), [DOC] , 113 ) as Date,[Comment],isnull((Select CustomerReviewMaster.Value from CustomerReviewMaster where Id=CustomerReviewMasterId),'') as Review FROM [dbo].[CustomerReviewCallHistory] where jobcardid=@1", new string[] { Request.QueryString["Id"] });
            if (dt1.Rows.Count > 0)
            {
                GridView1.Caption = "History";
                GridView1.DataSource = dt1;
                GridView1.DataBind();
            }
            else
            {
                GridView1.Caption = "";
                GridView1.DataSource = dt1;
                GridView1.DataBind();
            }
        }
        catch (Exception E) { }
    }
}