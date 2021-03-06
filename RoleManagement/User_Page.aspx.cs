﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebApplication1;
using System.Data;
using OfficeOpenXml;
public partial class RoleManagement_User_Page : System.Web.UI.Page
{
    dbConnection dbc = new dbConnection();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                lblDateTime.Text = String.Format("{0:ddd, MMM d, yyyy}", dbc.getindiantime());
                btnsave.Text = "Save";
                ddluser = dbc.FillCombo(ddluser, "select Id,Username from Users ", "Username", "Id");//where id <> 1
                bindrole();
                getdata();
            }
        }
        catch (Exception ex)
        {
            ltrErr.Text = ex.Message;
        }
    }
    public void bindrole()
    {
        try
        {
            DataTable dtdata = dbc.GetDataTable("Select * from Pages");
            if (dtdata.Rows.Count > 0)
            {
                grd.Caption = "Pages List: " + dtdata.Rows.Count; ;
                grd.DataSource = dtdata;
                grd.DataBind();
            }
            else
            {
                dtdata = new DataTable();
                grd.Caption = "Pages List: " + dtdata.Rows.Count; ;
                grd.DataSource = dtdata;
                grd.DataBind();
            }
        }
        catch (Exception ex)
        {
            ltrErr.Text = ex.Message;
        }
    }
    protected void ddluser_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            getdata();
        }
        catch (Exception ex)
        {
            ltrErr.Text = ex.Message;
        }
    }
    protected void btnsave_Click(object sender, EventArgs e)
    {
        try
        {
            ltrErr.Text = "";
            string ids = string.Empty;
            foreach (GridViewRow gvrow in grd.Rows)
            {
                CheckBox chk = (CheckBox)gvrow.FindControl("chkRow");
                if (chk != null & chk.Checked)
                {
                    Label lbl = (Label)gvrow.FindControl("lblid");
                    ids += lbl.Text + ",";
                }
            }
            ids = ids.TrimEnd(',');
            if (ids == "")
            {
                ltrErr.Text = "Please select page for particular user.";
                return;
            }
            int i = dbc.ExecuteQuery("delete from User_Page_Mapping where UserId=" + ddluser.SelectedValue + "");

            int j = 0;
            string[] loopid = ids.Split(',');
            for (int k = 0; k < loopid.Length; k++)
            {
                j = dbc.ExecuteQuery("insert into User_Page_Mapping Values(" + ddluser.SelectedValue + "," + loopid[k] + ")");
            }
            if (j > 0)
            {
                ltrErr.Text = "Page assigned.";
            }

        }
        catch (Exception ex)
        {
            ltrErr.Text = ex.Message;
        }
    }

    public void getdata()
    {
        try
        {
            foreach (GridViewRow row in grd.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkRow");
                chk.Checked = false;

            }
            ltrErr.Text = "";
            DataTable dt = dbc.GetDataTable("select * from User_Page_Mapping where userid=" + ddluser.SelectedValue + "");
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    foreach (GridViewRow row in grd.Rows)
                    {
                        Label lbl = (Label)row.FindControl("lblid");
                        if (lbl.Text == dt.Rows[i]["PageId"].ToString())
                        {
                            CheckBox chk = (CheckBox)row.FindControl("chkRow");
                            chk.Checked = true;
                            break;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ltrErr.Text = ex.Message;
        }
    }
}