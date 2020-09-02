using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class MessageTemplate_MessageTemplate : System.Web.UI.Page
{
    dbConnection dbCon = new dbConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
        BindData();
    }

    public void BindData()
    {
        SqlConnection conn = null;
        SqlDataReader rdr = null;

        try
        {
            conn = dbCon.GetConnectionForAdapter();
            SqlCommand cmd = new SqlCommand("MessageTemplateList", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            conn.Open();
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                var tb = new DataTable();
                tb.Load(dr);
                conn.Close();
                if (tb != null && tb.Rows.Count > 0)
                {
                    grd.DataSource = tb;
                    grd.DataBind();

                }
            }
        }
        catch
        {

        }
        finally
        {
            if (conn != null)
            {
                conn.Close();
            }
            if (rdr != null)
            {
                rdr.Close();
            }
        }
    }
    protected void grd_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.TableSection = TableRowSection.TableHeader;
        }
    }
    protected void grd_RowCommand(object sender, GridViewCommandEventArgs e)
    {
    }
   
}