using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;

public partial class SubCategory_SubCategory : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            try
            {
                string categoryqry = "SELECT CategoryId,CategoryName FROM Category where isnull(IsDeleted,0)=0 AND isnull(IsActive,0)=1 order by Sequence asc";
                DataTable dtcategory = new DataTable();
                dtcategory = dbc.GetDataTable(categoryqry);
                dtcategory.Rows.Add("0", "Select Category");
                DataView dv = dtcategory.DefaultView;
                dv.Sort = "CategoryId asc";
                DataTable sortedDT = dv.ToTable();
                ddlCategoryName.DataSource = sortedDT;
                ddlCategoryName.DataTextField = "CategoryName";
                ddlCategoryName.DataValueField = "CategoryId";
                ddlCategoryName.DataBind();
                //txtdt.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txtdt1.Text = DateTime.Now.ToString("dd/MM/yyyy");
                DataList();
            }
            catch (Exception W) { }
        }

    }
    private void DataList()
    {
        //String from = txtdt.Text.ToString();
        //String to = txtdt1.Text.ToString();

        //String[] StrPart = from.Split('/');

        //String[] StrPart1 = to.Split('/');
        int isActive = 0;
        if (chkisactive.Checked)
        {
            isActive = 1;
        }

        int categoryId = Convert.ToInt32(ddlCategoryName.SelectedValue);
        string query = "SELECT S.Id,S.SubCategory,S.CategoryID,C.CategoryName,S.Description,S.IsActive,S.CreatedOn " +
                       " FROM tblSubCategory S " +
                       " LEFT JOIN Category C ON C.CategoryID = S.CategoryId " +
                       " where isnull(S.IsDeleted,0)=0  " +
                        " AND ISNULL(S.ISActive, 0) = " + isActive;


        if (categoryId > 0)
        {
            query += " and S.CategoryID=" + categoryId;
        }
        //"and convert(date,S.CreatedOn,103)>='" + StrPart[2] + "-" + StrPart[1] + "-" + StrPart[0] + "' and convert(date,S.CreatedOn,103)<='" + StrPart1[2] + "-" + StrPart1[1] + "-" + StrPart1[0] + 
        query += "  order by S.sequence desc ";
        DataTable dtSubCategorylist = dbc.GetDataTable(query);
        if (dtSubCategorylist.Rows.Count > 0)
        {
            gvSubCategorylist.DataSource = dtSubCategorylist;
            gvSubCategorylist.DataBind();
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        DataList();
    }
    protected void gvSubCategorylist_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.Header)
        {
            e.Row.TableSection = TableRowSection.TableHeader;
        }
    }

    protected void OnSelectedIndexChanged(object sender, EventArgs e)
    {
        DataList();
    }

}