<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="WalletList.aspx.cs" Inherits="Wallet_WalletList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>

    <link href="../plugins/datepicker/datepicker3.css" rel="stylesheet" />
    <script src="../plugins/datepicker/bootstrap-datepicker.js"></script>



    <style type="text/css">
        .content {
            min-height: 100vh;
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
   
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Wallet List
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
                <a href="ManageWallets.aspx" class="btn btn-success pull-right add-padding" style="width:80px" >ADD</a>
            </div>
            <div class="row">
                <div class="col-md-3 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <asp:Label runat="server" ID="Startdate"></asp:Label>
                        <asp:TextBox ID="txtdt" CssClass="form-control" placeholder="Select Date" runat="server"></asp:TextBox>
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
                        <asp:TextBox ID="txtdt1" CssClass="form-control" placeholder="Select Date" runat="server"></asp:TextBox>
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

                    <asp:Button ID="Button2" runat="server" Text="Go" Width="70Px" CssClass="btn btn-block  btn-info" OnClick="Button1_Click" />

                </div>

            </div>

            <div style="width: 100%;" class="table-responsive">
                <asp:GridView ID="gvwalletlist" OnRowDataBound="gvwalletlist_RowDataBound" OnRowCommand="gvwalletlist_RowCommand" runat="server" Width="95%" AutoGenerateColumns="False" class="table table-bordered table-hover" rules="all" role="grid" CellPadding="10" CellSpacing="5" AllowSorting="True" HeaderStyle-BackColor="#ede8e8" HeaderStyle-HorizontalAlign="Center" EnableViewState="False" Caption="<b><u>WALLET LIST</u></b>" CaptionAlign="Top">
                    <Columns>
                        <asp:BoundField HeaderText="Campaign Name" DataField="campaign_name" />
                        <asp:BoundField HeaderText="Coupon Code" DataField="coupon_code" />
                        <asp:BoundField HeaderText="Type" DataField="per_type" />
                        <asp:BoundField HeaderText="Amount" DataField="per_amount" />
                        <asp:BoundField HeaderText="Min Order Amount" DataField="min_order_amount" />
                        <asp:BoundField HeaderText="IsActive" DataField="is_active" />
                        <asp:BoundField HeaderText="StartDate" DataField="start_date" />
                        <asp:BoundField HeaderText="EndDate" DataField="end_date" />

                        <asp:HyperLinkField DataNavigateUrlFields="Id" ControlStyle-CssClass="red" HeaderText="EDIT" Target="_blank" DataNavigateUrlFormatString="~/Wallet/ManageWallets.aspx?Id={0}" Text="Edit" />

                    </Columns>
                    <HeaderStyle HorizontalAlign="Center" BackColor="#EDE8E8"></HeaderStyle>
                </asp:GridView>
            </div>
          
              <script type="text/javascript">
                $(document).ready(function () {
                    $('#ContentPlaceHolder1_gvwalletlist').DataTable({
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

