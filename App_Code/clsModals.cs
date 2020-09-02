using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for clsModals
/// </summary>
public class clsModals
{
	public clsModals()
	{
		//
		// TODO: Add constructor logic here
		//
        
	}
    public class clsSendOtpResponce
    {
      public  string response { get; set; }
      public string message { get; set; }   
    }
    public class clsOtpVarificationResponce
    {
        public string response { get; set; }
        public string message { get; set; }
        public string userid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public string Sex { get; set; }
    }

    //ProductModel

    public class getproduct
    {

        public string response;
        public string message;
        public List<ProductDataList> ProductList { get; set; }
       
    }
    public class ProductDataList
    {
        public string pname;
        public string pdec;
        public string pkey;
        public string pnote;
        public string pprice;
        public string pwight;
        public string pvideo;
        public string poffer;
        public string pbuy2;
        public string pbuy5;
        public string shipping;
        public string psold;
        public string pJustBougth;


        public List<ProductDataImagelist> ProductImageList { get; set; }
    }
    public class ProductDataImagelist
    {
        public string prodid;
        public string proimagid;
        public string PImgname;
        public string PDisOrder;
    }

    //Banner Image    
        public class BnnerImage
        {

            public string response;
            public string message;
            public List<BannerDataList> BannerImageList { get; set; }

        }
        public class BannerDataList
        {
            public string ImgUrl;
            public string Title;
            public string AltText;
            public string DataLink;
        }

    //Add Address Sucessfully Message

        public class CustAddress
        {
            public string Response;
            public string Message;
            public string LastId;

        }

    //customer order details

    public class custorderdetails
    {
        public string Response;
        public string Message;
        public string Orderid;
        public string Buywith;
        public string OrderDate;
        public string OrderTime;
        public string Custoffercode;
        public string ReferCode;
        public string ProductEnddate;
        public string ProductStartDate;
        public string ProductId;
        public string OrderItemId;
    }
    
}