using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1;
using System.Data;

/// <summary>
/// Summary description for OfferPriceUpdate
/// </summary>
public class OfferPriceUpdate
{
    public OfferPriceUpdate()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public void OfferPriceUpdateMethod(DataTable dtCampaing,string VariantID,double prdprice)
    {
        try
        {
            dbConnection dbc = new dbConnection();

            DataRow[] drCmap = dtCampaing.Select("VariantId = " + VariantID);
            if (drCmap.Length > 0)
            {
                DataTable dtCampVar = drCmap.CopyToDataTable();

                for (int m = 0; m < dtCampVar.Rows.Count; m++)
                {
                    double NewPrice = prdprice;
                    string perc = dtCampVar.Rows[m]["OfferPerc"].ToString();
                    string CampingID = dtCampVar.Rows[m]["CampaingId"].ToString();

                    //decimal.TryParse(dtCampVar.Rows[m]["MRP"].ToString(), out price);

                    if (perc.StartsWith("-"))
                    {
                        string[] valu = perc.Split('-');
                        double per = 0;
                        double.TryParse(valu[1].ToString(), out per);
                        double result = (NewPrice * per) / 100;
                        double showprice = NewPrice - result;
                        showprice = Math.Ceiling(showprice);

                        String StrCampVer = "Update Taaza_Offer_Product_Mapping set UpdatedOnUtc=CONVERT(DATETIME, '" + dbc.getDOCMtime() + "', 102), MRP=" + NewPrice + ",OfferPrice =" + showprice + " Where id=" + dtCampVar.Rows[m]["Id"].ToString() + "";
                        dbc.ExecuteQuery(StrCampVer);
                    }
                    else
                    {
                        double per = 0;
                        double.TryParse(perc.ToString(), out per);
                        double result = (NewPrice * per) / 100;
                        double showprice = NewPrice + result;
                        showprice = Math.Ceiling(showprice);

                        String StrCampVer = "Update Taaza_Offer_Product_Mapping set UpdatedOnUtc=CONVERT(DATETIME, '" + dbc.getDOCMtime() + "', 102), MRP=" + NewPrice + ",OfferPrice =" + showprice + " Where id=" + dtCampVar.Rows[m]["Id"].ToString() + "";
                        dbc.ExecuteQuery(StrCampVer);
                    }

                    String Str2 = "SELECT Taaza_Offer_Product_Mapping.*, Taaza_Campaign_Master.Id as CampaingId, Taaza_Campaign_Master.Campaign_Status, Taaza_Campaign_Master.Deleted AS CampaignDeleted FROM Taaza_Campaign_Master INNER JOIN Taaza_Campaign_Product ON Taaza_Campaign_Master.Id = Taaza_Campaign_Product.Campaign_Id INNER JOIN Taaza_Offer_Product_Mapping ON Taaza_Campaign_Product.ProductId = Taaza_Offer_Product_Mapping.OfferProductId where Taaza_Campaign_Master.Id = " + CampingID + ";";
                    DataTable dtCampaingDetails = dbc.GetDataTable(Str2);

                    double CampTotal = 0;
                    string OfferProdId = "";
                    if(dtCampaingDetails.Rows.Count>0)
                    {
                        DataRow[] drCampPRod = dtCampaingDetails.Select("IsPrimaryProduct = 1");

                        if (drCampPRod.Length > 0)
                        {
                            //DataTable dttemcam = drCampPRod.CopyToDataTable();
                            OfferProdId = drCampPRod[0]["OfferProductId"].ToString();
                        }

                        for (int k = 0; k < dtCampaingDetails.Rows.Count; k++)
                        {
                            double verprice = 0;
                            double.TryParse(dtCampaingDetails.Rows[k]["OfferPrice"].ToString(), out verprice);
                            CampTotal += verprice;
                        }

                        String Str3 = "select * from Product_Variant where ProductId = " + OfferProdId + ";";
                        DataTable dtCampOffVer = dbc.GetDataTable(Str3);

                        if (dtCampOffVer.Rows.Count > 0)
                        {
                            //string PrimVerID = dtCampOffVer.Rows[0]["Id"].ToString();
                            String StrCampVer = "Update Product_Variant set DOM=CONVERT(DATETIME, '" + dbc.getDOCMtime() + "', 102), MRP=" + CampTotal + " Where Id=" + dtCampOffVer.Rows[0]["Id"].ToString() + "";
                            dbc.ExecuteQuery(StrCampVer);
                        }

                    }

                }
            }
        }
        catch (Exception e)
        {
        }
    }
}