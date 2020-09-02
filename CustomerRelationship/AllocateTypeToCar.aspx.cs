using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CustomerRelationship_AllocateTypeToCar : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Request.QueryString["vnum"] != null)
            {
                lable.InnerText = "Allocate jobtype to " + Request.QueryString["vnum"];
            }
        }
    }
}