    <%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Jurisdiction.aspx.cs" Inherits="Jurisdiction_Jurisdiction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <style type="text/css">
            .content
            {
                min-height:100vh;
            }
        </style>

    <style>
        .red {
            position: relative;
            text-decoration: none;
            color: #fff;
            background: #2783cb;
            text-align: center;
            padding: 2px 10px;
            width: 75px;
            border-radius: 5px;
            border: solid 1px #2770cb;
            transition: all 0.1s;
            -webkit-box-shadow: 0px 9px 0px #2770cb; /*for opera and safari*/
            -moz-box-shadow: 0px 9px 0px #2770cb; /*for mozilla*/
            -o-box-shadow: 0px 9px 0px #2770cb; /*for opera*/
            -ms-box-shadow: 0px 9px 0px #2770cb; /*for I.E.*/
        }

        .add-padding {
            padding: 7px 100px;
        }
    </style>
    <script src="../Scripts/jquery-1.12.4.min.js"></script>
    <link rel="stylesheet" href="../../plugins/datepicker/datepicker3.css">
    <script src="../../plugins/datepicker/bootstrap-datepicker.js"></script>
     <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Jurisdiction
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

            <div style="width: 100%;" class="table-responsive">
                <asp:GridView ID="gvJurisdictionlist" OnRowDataBound="gvJurisdictionlist_RowDataBound" runat="server" Width="95%" AutoGenerateColumns="False" class="table table-bordered table-hover" rules="all" role="grid" CellPadding="10" CellSpacing="5" AllowSorting="True" HeaderStyle-BackColor="#ede8e8" HeaderStyle-HorizontalAlign="Center" EnableViewState="False" Caption="<b><u>JURISDICTION LIST</u></b>" CaptionAlign="Top">
                     <Columns>
                          <asp:BoundField HeaderText="Jurisdiction Incharge" DataField="JurisdictionIncharge" />
                        <asp:BoundField HeaderText="Contact" DataField="Contact" />
                        <asp:BoundField HeaderText="Email" DataField="EmalID" />
                        <asp:BoundField HeaderText="State" DataField="StateName" />
                        <asp:BoundField HeaderText="City" DataField="CityName" />
                        <asp:BoundField HeaderText="Comments" DataField="Comments" />
                           <asp:BoundField HeaderText="IsActive" DataField="IsActive" />

                          <asp:HyperLinkField DataNavigateUrlFields="JurisdictionID" ControlStyle-CssClass="red" HeaderText="EDIT" Target="_blank" DataNavigateUrlFormatString="~/Jurisdiction/AddJurisdiction.aspx?Id={0}" Text="Edit" />

                    </Columns>
                    <HeaderStyle HorizontalAlign="Center" BackColor="#EDE8E8"></HeaderStyle>
                </asp:GridView>
            </div>
             <script type="text/javascript">
                $(document).ready(function () {
                    $('#ContentPlaceHolder1_gvJurisdictionlist').DataTable({
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
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" Runat="Server">
</asp:Content>

