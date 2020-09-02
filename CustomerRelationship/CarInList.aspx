<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="CarInList.aspx.cs" Inherits="CustomerRelationship_CarInList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <link rel="stylesheet" href="../../plugins/datepicker/datepicker3.css">
    <script src="../../plugins/datepicker/bootstrap-datepicker.js"></script>
    <script>
        var strCache = "";
        //var urlstr = "http://api.motorz.co.in/MotorzService.asmx";
        var urlstr = "../MotorzInner.asmx";
        var numshow = 10;
    </script>
    <div>
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>Car In List
                            <small>
                                <asp:Label runat="server" ID="lblDateTime"></asp:Label>
                            </small>
                </h1>
                <script>
                    document.body.classList.add("sidebar-collapse");
                </script>
                <ol class="breadcrumb">
                </ol>
            </section>
            <section class="content">
                <div class="box">
                    <div class="box-body table-responsive">
                        <table style="width: 100%;" class="table table-hover dataTable table-responsive table-bordered table-striped">
                    <tr>
                                    <td> </td>
                                    <td>
                                        <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                        <div class="form-group">
                                            <label>Start Date:</label>
                                            <div class="input-group date">
                                                <div class="input-group-addon">
                                                    <i class="fa fa-calendar"></i>
                                                </div>
                                                <input type="text" class="form-control pull-right" runat="server" name="startdate" id="startdate">
                                            </div>
                                            <!-- /.input group -->
                                        </div>
                                        
                                    </td>
                                    <td></td>
                                    <td>
                                        <div class="form-group">
                                            <label>End Date:</label>
                                            <div class="input-group date">
                                                <div class="input-group-addon">
                                                    <i class="fa fa-calendar"></i>
                                                </div>
                                                <input type="text" class="form-control pull-right" runat="server" name="enddate" id="enddate">
                                            </div>
                                            <!-- /.input group -->
                                        </div>
                                        
                                    </td>
                                    <td><div class="form-group">
                                            <label>&nbsp;</label>
                                            <div class="input-group date">
                                                
                                                <asp:Button ID="Button2" runat="server" Text="Go" Width="70Px" CssClass="btn btn-block btn-info" OnClick="Button1_Click" />
                                            </div>
                                            <!-- /.input group -->
                                        </div>
                                        </td>
                                </tr>
                    <tr><td colspan="4"> </td></tr>
                </table>
                        <asp:GridView ID="GridView1" AutoGenerateColumns="False" runat="server" AllowSorting="True" CssClass="table table-hover dataTable table-responsive table-bordered table-striped" OnRowDataBound="grd_RowDataBound" Width="100%">
                            <Columns>
                                <asp:TemplateField HeaderText="#" >
                                    <ItemTemplate>
                                        <a id="lnk-<%# Eval("Id") %>" style="width:60px;" onclick="makegreen(<%# Eval("Id") %>)" target="_blank"  href="AllocateTypeToCar.aspx?id=<%# Eval("Id") %>&vnum=<%# Eval("Number") %>" class="btn btn-block btn-info btn-sm"  ><%# Eval("Id") %></a>
                                    </ItemTemplate>
                                <ItemStyle Width="100px"></ItemStyle>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Number" HeaderText="Number" />
                                <asp:BoundField DataField="Dt" HeaderText="In Date Time" />
                                 <asp:BoundField DataField="Type" HeaderText="Jobcard Type" />
                            </Columns>
                        </asp:GridView>
                        <script type="text/javascript">
                           
                            $(document).ready(function () {
                                $('#ContentPlaceHolder1_GridView1').DataTable({
                                    "fixedHeader": true,
                                    "paging": true,
                                    "lengthChange": true,
                                    "searching": true,
                                    "ordering": true,
                                    "info": true,
                                    "autoWidth": false
                                });
                            });
                        </script>
                        <script>
                            $('#ContentPlaceHolder1_enddate').datepicker({
                                format: 'dd/M/yyyy',
                                autoclose: true
                            });
                            $('#ContentPlaceHolder1_startdate').datepicker({
                                format: 'dd/M/yyyy',
                                autoclose: true
                            });
                            function makegreen(Id) {
                                var currentRow = $("#lnk-" + Id).closest('tr');
                                currentRow.css('background', '#53e69d');
                            }
                        </script>
                    </div>
                </div>
            </section>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="Server">
</asp:Content>







