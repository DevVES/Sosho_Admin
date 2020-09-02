<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="RunQuery.aspx.cs" Inherits="RunQuery" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <script src="https://cdn.datatables.net/fixedheader/3.1.2/js/dataTables.fixedHeader.min.js"></script>
    <link rel="stylesheet" href="https://cdn.datatables.net/fixedheader/3.1.2/css/fixedHeader.dataTables.min.css" />
    <script src="https://cdn.datatables.net/buttons/1.5.1/js/dataTables.buttons.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.5.1/js/buttons.flash.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.32/pdfmake.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.32/vfs_fonts.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.5.1/js/buttons.html5.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.5.1/js/buttons.print.min.js"></script>
    <style type="text/css">
        .content-wrapper, .right-side, .main-footer {
            margin-left: 18%;
        }

        .box {
            overflow-x: scroll;
        }
    </style>
    <div class="content-wrapper" style="margin-left : 17%"> <%--style="overflow: auto; min-height: 779px;"--%>
        <div class="fixed grid_wrapper" >
            <%--<div class="fixed_header">
                <div class="clearfix" style="width: 100%">--%>

                    <!-- Content Header (Page header) -->
                    <section class="content-header">
                        <h1>Run Query  
                <small>
                    <asp:Label runat="server" ID="lblDateTime"></asp:Label></small>
                        </h1>
                        <ol class="breadcrumb">
                            <li><a href="../Home.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                            <li class="active">Run Query</li>
                        </ol>
                    </section>
                    <section class="content">
                        <div class="row">
                            <div class="col-xs-12">
                                <!-- interactive chart -->
                                <div>
                                    <div class="box">
                                        <div class="box-header with-border">
                                            <div class="form-group">

                                                <div class="row" style="width: 50%; margin-left: 10px; margin-right: 25%;">
                                                    <br />
                                                    <div class="row">
                                                        <div class="col-xs-3">
                                                            Database Name
                                                        </div>
                                                        <div class="col-xs-4">
                                                            <%--<asp:TextBox ID="txtdbname"  runat="server" Width="600px" class="form-control"></asp:TextBox>--%>
                                                            <asp:DropDownList ID="ddldbname" CssClass="Textbox form-control select2" runat="server" Width="250px">
                                                            
                                    </asp:DropDownList>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <div class="row">
                                                        <div class="col-xs-3">
                                                            Run Query
                                                        </div>
                                                        <div class="col-xs-4">
                                                            <asp:TextBox ID="txtName" TextMode="MultiLine" runat="server" Width="600px" Height="100px" class="form-control"></asp:TextBox>
                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtName" runat="server" ErrorMessage="Name Required"></asp:RequiredFieldValidator>--%>
                                                        </div>
                                                    </div>
                                                    <br />
                                                    <div class="row">
                                                        <div class="col-xs-3">
                                                            Password
                                                        </div>
                                                        <div class="col-xs-4">
                                                            <input name="txtpass" runat="server" type="password" autocomplete="new-password" id="txtpass" class="form-control" style="width:600px;"/>

                                                            <%--<asp:TextBox ID="txtpass" TextMode="Password" runat="server" class="form-control"></asp:TextBox>--%>
                                                            <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator1" ControlToValidate="txtName" runat="server" ErrorMessage="Name Required"></asp:RequiredFieldValidator>--%>
                                                        </div>
                                                    </div>
                                                    <div class="row">
                                                        <div class="col-xs-3"></div>
                                                        <div class="col-xs-2">
                                                            <br />

                                                            <asp:Button ID="btnsearch" runat="server" class="btn btn-block btn-primary" OnClick="btnsearch_Click" Text="Search" Width="150px" Style="text-align: center;" />
                                                            <div>
                                                            </div>
                                                        </div>
                                                        <span style="width: 90%; color: red">
                                                            <asp:Literal ID="ltrErr" runat="server"></asp:Literal>
                                                            <br />
                                                        </span>
                                                    </div>
                                                </div>

                                            </div>
                                        </div>
                                    </div>
                                    <!-- /.box-body-->
                                </div>
                                <!-- /.box -->
                            </div>
                        </div>
                        <div class="row">
                            <div class="col-xs-12">
                                <!-- interactive chart -->
                                <div class="box">
                                    <div class="box-header with-border">
                                        <i class="fa fa-table"></i>
                                        <h3 runat="server" id="h3TableTitle" class="box-title">Details</h3>
                                        <div class="box-body">
                                            <div class="table-responsive" style="width: 60%; margin-left: 2%; margin-right: 20%; display: inline">
                                                <asp:GridView ID="grd" OnRowDataBound="grd_RowDataBound" class="table table-bordered table-hover dataTable GridPager a GridPager span" runat="server" AutoGenerateColumns="true" Caption="Details" CellPadding="10" CellSpacing="5" Width="100%">

                                                    <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />

                                                </asp:GridView>

                                            </div>
                                        </div>
                                    </div>
                                    <!-- /.box -->
                                </div>
                            </div>
                            <div style="padding-left: 20px; margin-top: 30px; margin-bottom: 30px;">

                                <asp:Button ID="btnExport" CssClass="btn btn-primary" OnClientClick="return callfootercss()"
                                    runat="server" Text="Export To Excel" Visible="true" OnClick="btnExport_Click" />
                            </div>
                        </div>
                    </section>
                </div>
            </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ContentPlaceHolder1_grd').DataTable({
                "fixedHeader": true,
                "paging": false,
                "lengthChange": true,
                "searching": true,
                "ordering": false,
                "info": true,
                "autoWidth": true,
                "dom": 'Bfrtip',
                buttons: [
					'copy', 'csv', 'excel', 'pdf', 'print'
                ]
            });
            $(".sidebar-toggle").click(function () {
                reInitDt();
            });
            //Change .content-wrapper related to scroll event
            $('.content-wrapper').scroll(function () {
                reInitDt();
            });
            $('#ContentPlaceHolder1_grd').on('column-sizing.dt', function (e, settings) {
                //alert("Resizing called");
            });
        });
        function reInitDt() {
            setTimeout(function () { $('#ContentPlaceHolder1_grd').DataTable().columns.adjust().draw(); }, 600);
        }
    </script>
    <style>
        .dt-button {
            background-color: #00c0ef;
            border-color: #00acd6;
            color: white;
            padding: 6px 12px;
            border-radius: 3px;
            border: none;
        }

            .dt-button:hover {
                background-color: #00acd6;
            }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" Runat="Server">
</asp:Content>

