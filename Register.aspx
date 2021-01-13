<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <style type="text/css">
        .content {
            min-height: 100vh;
        }
    </style>



    <%--<script src=" https://code.jquery.com/jquery-3.3.1.js"></script>--%>
    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.5.6/js/dataTables.buttons.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.5.6/js/buttons.flash.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.5.6/js/buttons.html5.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.5.6/js/buttons.print.min.js"></script>
    <%-- <script src="../plugins/jQuery/jQuery-2.2.0.min.js"></script>--%>


    <script src="../../plugins/datepicker/bootstrap-datepicker.js"></script>
    <link rel="stylesheet" href="../../plugins/datepicker/datepicker3.css" />
    <link rel="stylesheet" href="https://cdn.datatables.net/buttons/1.5.6/css/buttons.dataTables.min.css" />

    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Register
               <small>
                   <asp:Label runat="server" ID="lblDateTime"></asp:Label>
               </small>
            </h1>
            <ol class="breadcrumb">
            </ol>
        </section>
        <section class="content">
            <!-- Small boxes (Stat box) -->


            <div class="row">
                <div class="col-md-12">
                    <div class="col-md-3 col-sm-6 col-xs-12">
                        <div class="form-group">
                            <asp:Label runat="server" ID="Startdate"></asp:Label>
                            <asp:TextBox ID="txtdt" CssClass="form-control" placeholder="Select Start Date" runat="server"></asp:TextBox>
                            <script>
                                $('#ContentPlaceHolder1_txtdt').datepicker({
                                    format: 'dd/mm/yyyy',
                                    autoclose: true
                                });
                            </script>
                            <!-- /.input group -->

                        </div>
                    </div>
                    <div class="col-md-3 col-sm-6 col-xs-12">
                        <div class="form-group">
                            <asp:Label runat="server" ID="EndDate"></asp:Label>
                            <asp:TextBox ID="txtdt1" CssClass="form-control" placeholder="Select End Date" runat="server"></asp:TextBox>
                            <script>
                                $('#ContentPlaceHolder1_txtdt1').datepicker({
                                    format: 'dd/mm/yyyy',
                                    autoclose: true
                                });
                            </script>
                            <!-- /.input group -->
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-6 col-xs-12">
                        <asp:Button ID="BtnGo" runat="server" Text="Go" Width="70Px" CssClass="btn btn-block  btn-info" OnClick="Button1_Click" Title="Go" />
                    </div>
                    <div class="col-md-3 col-sm-6 col-xs-12">
                        <a href="AddJurisdiction.aspx" class="btn btn-success pull-right add-padding" style="width: 50px; margin: 20px">Add</a>
                    </div>
                </div>
            </div>
            <%--            <div class="box-body">
            <div style="width: 100%;" class="table-responsive">
                <asp:GridView ID="gvRegisterlist" OnRowDataBound="gvRegisterlist_RowDataBound" runat="server" Width="95%" AutoGenerateColumns="False" class="table table-bordered table-hover" rules="all" role="grid" CellPadding="10" CellSpacing="5" AllowSorting="True" HeaderStyle-BackColor="#ede8e8" CssClass="table dataTable table-hover table-bordered table-responsive"  HeaderStyle-HorizontalAlign="Center" EnableViewState="False" Caption="<b><u>REGISTRATION LIST</u></b>" CaptionAlign="Top">
                     <Columns>
                          <asp:BoundField HeaderText="Registration Date" DataField="RegistrationDate" />
                        <asp:BoundField HeaderText="Mobile" DataField="Mobile" />
                        <asp:BoundField HeaderText="FirstName" DataField="FirstName" />
                        <asp:BoundField HeaderText="LastName" DataField="LastName" />
                    </Columns>
                    <HeaderStyle HorizontalAlign="Center" BackColor="#EDE8E8"></HeaderStyle>
                </asp:GridView>
            </div>
                 </div>--%>



            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="box box-warning">
                    <div class="box-header">
                        <h3 class="box-title">Registration List</h3>
                    </div>
                    <div class="box-body">
                        <div class="table-responsive">
                            <asp:GridView ID="gvRegisterlist" AutoGenerateColumns="false" runat="server" AllowSorting="True" CssClass="table dataTable table-hover table-bordered table-responsive" OnRowDataBound="gvRegisterlist_RowDataBound" Width="100%">
                                <Columns>
                                    <asp:BoundField HeaderText="Registration Date" DataField="RegistrationDate" />
                                    <asp:BoundField HeaderText="Mobile" DataField="Mobile" />
                                    <asp:BoundField HeaderText="FirstName" DataField="FirstName" />
                                    <asp:BoundField HeaderText="LastName" DataField="LastName" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                </div>
            </div>

            <script type="text/javascript">
                                $(document).ready(function () {
                                    $('#ContentPlaceHolder1_gvRegisterlist').DataTable({
                                        "fixedHeader": true,
                                        "paging": true,
                                        "order": [[5, "desc"]],
                                        "lengthChange": true,
                                        "deferRender": true,
                                        "ordering": true,
                                        "scrollX": true,
                                        "info": true,
                                        "autoWidth": false,
                                        "alwaysCloneTop": false,
                                    });
                                });
            </script>
        </section>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="Server">
</asp:Content>

