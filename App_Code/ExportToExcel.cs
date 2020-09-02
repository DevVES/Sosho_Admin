using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using OfficeOpenXml;
using System.Net;
using OfficeOpenXml.Style;


/// <summary>
/// Summary description for ExportToExcel
/// </summary>
public class ExportToExcel
{
	public ExportToExcel()
	{
		
	}

    public void GenerateExcel(DataSet ds, string Title = "", bool hideCol = false, bool fromGst = false)
    {
        try
        {
                if (ds != null && ds.Tables.Count > 0)
                {
                    using (ExcelPackage xp = new ExcelPackage())
                    {
                        foreach (DataTable dt in ds.Tables)
                        {
                            if (hideCol)
                            {
                                try
                                {
                                    for (int l = 0; l < dt.Columns.Count; l++)
                                    {
                                        dt.Columns[l].Caption = "";
                                    }
                                }
                                catch (Exception E) { }
                            }
                            ExcelWorksheet ws = xp.Workbook.Worksheets.Add(dt.TableName);
                            int rowstart =0;
                            int colstart = 1;
                            int rowend = rowstart;
                            int colend = colstart + dt.Columns.Count-1;

                            //ws.Cells[rowstart, colstart, rowend, colend].Merge = true;
                            //ws.Cells[rowstart, colstart, rowend, colend].Value = dt.TableName;
                            //ws.Cells[rowstart, colstart, rowend, colend].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            //ws.Cells[rowstart, colstart, rowend, colend].Style.Font.Bold = true;
                            //ws.Cells[rowstart, colstart, rowend, colend].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                            //ws.Cells[rowstart, colstart, rowend, colend].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                            rowstart += 1;
                            rowend = rowstart + dt.Rows.Count;
                            ws.Cells[rowstart, colstart].LoadFromDataTable(dt, true);
                            int i = 0;
                            foreach (DataColumn dc in dt.Columns)
                            {
                                i++;
                                if (dc.DataType == typeof(DateTime))
                                {
                                    ws.Column(i).Style.Numberformat.Format = "dd/MM/yyyy HH:mm:ss";
                                }
                                if (!hideCol)
                                {
                                    ws.Cells[1, 1, 1, i].Style.Font.Bold = true;
                                    ws.Cells[1, 1, 1, i].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                    ws.Cells[1, 1, 1, i].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                                    ws.Cells[1, 1, 1, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                }
                                else
                                {
                                    if (fromGst)
                                    {
                                        ws.Cells[4, 1, 4, i].Style.Font.Bold = true;
                                        ws.Cells[4, 1, 4, i].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                        ws.Cells[4, 1, 4, i].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightBlue);
                                        ws.Cells[4, 1, 4, i].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                                    }
                                }
                            }
                               ws.Cells[ws.Dimension.Address].AutoFitColumns();
                               ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Top.Style =
                               ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Bottom.Style =
                               ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Left.Style =
                               ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        }
                        Byte[] fileBytes = xp.GetAsByteArray();
                        HttpContext.Current.Response.ClearContent();
                        string file = Title + "_" + DateTime.Now.ToString("dd_MM_yyyy") + ".xlsx";
                        // Add the content disposition (file name to be customizable) to be exported.  
                        HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename="+file+"");

                        // add the required content type  
                        HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        // write the bytes to the file and end the response  
                        HttpContext.Current.Response.BinaryWrite(fileBytes);
                        HttpContext.Current.Response.End();        
                    }
                }
        }
        catch(Exception EE)
        {
           
        }
    }

}