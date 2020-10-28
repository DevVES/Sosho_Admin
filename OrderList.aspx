<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="OrderList.aspx.cs" Inherits="OrderList" %>

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

    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Order List
               <small>
                   <asp:Label runat="server" ID="lblDateTime"></asp:Label>
               </small>
            </h1>
            <ol class="breadcrumb">
                <%--<li><a href="../Home1.aspx"><i class="fa fa-dashboard"></i>Old Dashboard</a></li>--%>
            </ol>
        </section>
        <section class="content">
            <div class="col-md-12" style="padding: 10px">
                <div class="col-lg-2">
                    <asp:Label ID="Label1" runat="server" Text="From Date"></asp:Label><br />
                    <input type="text" class="form-control pull-right" runat="server" id="startdate" />
                    <%-- <asp:TextBox ID="startdate" CssClass="form-control" runat="server"></asp:TextBox>--%>
                </div>
                <div class="col-lg-2">
                    <asp:Label ID="Label2" runat="server" Text="To Date"></asp:Label><br />
                    <input type="text" class="form-control pull-right" runat="server" id="enddate" />

                </div>
                <div class="col-lg-4">
                    <br />
                    <asp:Button ID="Button2" runat="server" Text="Go" Width="70Px" CssClass="btn btn-block  btn-info" OnClick="Button2_Click" />
                </div>
            </div>
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="box box-warning">
                    <div class="box-header">
                        <h3 class="box-title">Order List</h3>
                    </div>
                    <div class="box-body">
                        <div class="table-responsive">
                            <asp:GridView ID="grdGrn" AutoGenerateColumns="false" runat="server" AllowSorting="True" CssClass="table dataTable table-hover table-bordered table-responsive" OnRowDataBound="grdGrn_RowDataBound" Width="100%">
                                <Columns>
                                    <asp:TemplateField HeaderText="Order Id" ItemStyle-Width="30">
                                        <ItemTemplate>
                                            <asp:HyperLink runat="server" Target="_blank" NavigateUrl='<%# Eval("ordid", "/Order/order_details.aspx?Orderid={0}") %>' Text='<%# Eval("ordid") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <%--   <asp:BoundField DataField="ordid" HeaderText="Order Id" />--%>
                                    <asp:BoundField DataField="CreatedOnUtc" HeaderText="Date" />
                                    <asp:BoundField DataField="FirstName" HeaderText="Customer Name" />
                                    <asp:BoundField DataField="Mobile" HeaderText="Customer Mobile No" />
                                    <asp:BoundField DataField="cadd" HeaderText="Address" />
                                    <asp:BoundField DataField="Name" HeaderText="Product Name" />
                                    <asp:BoundField DataField="PaymentAmt" HeaderText="Payment Amount" />
                                    <asp:BoundField DataField="Totalamt" HeaderText="Total Amount" />
                                    <asp:BoundField DataField="TotalQTY" HeaderText="Quantity" />
                                    <asp:BoundField DataField="Ex" HeaderText="Oder Status" />
                                    <asp:BoundField DataField="DeliveryManAmt" HeaderText="Delivery Man Received Amount" />
                                    <asp:BoundField DataField="FrenchiessAmt" HeaderText="Franchisee Received Amount" />
                                    <asp:BoundField DataField="AdminAmount" HeaderText="Sosho Received Amount" />
                                    <%-- <asp:HyperLinkField ControlStyle-CssClass="btn btn-social" DataNavigateUrlFields="ordid" Target="_blank" DataNavigateUrlFormatString="Order/order_details.aspx?Orderid={0}" HeaderText="Order Detail" Text="View"></asp:HyperLinkField>--%>
                                    <asp:TemplateField HeaderText="Deliverd">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdn" Value='<%# Eval("OrderStatusId") %>' runat="server" />
                                            <asp:HiddenField ID="oid" Value='<%# Eval("ordid") %>' runat="server" />
                                            <asp:Literal ID="ltr" runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Cancel">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdn1" Value='<%# Eval("OrderStatusId") %>' runat="server" />
                                            <asp:HiddenField ID="oid1" Value='<%# Eval("ordid") %>' runat="server" />
                                            <asp:Literal ID="ltr1" runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                         <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="hdn2" Value='<%# Eval("OrderStatusId") %>' runat="server" />
                                            <asp:HiddenField ID="oid2" Value='<%# Eval("ordid") %>' runat="server" />
                                            <asp:Literal ID="ltr2" runat="server"></asp:Literal>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--  <asp:HyperLinkField HeaderText="" DataTextField="ordid"  />--%>
                                    <%--<asp:BoundField DataField="TotalGram" HeaderText="Quality" />--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>

                </div>
            </div>

        </section>
    </div>
    <input type="hidden" id="hdnOrderId" />
    <div class="modal fade" id="paymentStatusModal" role="dialog">
    <div class="modal-dialog">
    
      <!-- Modal content-->
      <div class="modal-content">
        <div class="modal-header">
          <button type="button" class="close" data-dismiss="modal">&times;</button>
          <h4 class="modal-title">Payment Status</h4>
        </div>
        <div class="modal-body">
            
            <div class="row">
            <div class="col-md-3 pad">
                        <asp:Label ID="lblReceiveAmount" runat="server" Text="Receive Amount"></asp:Label><span style="color: red">*</span>
                    </div>
                    
          <div class="col-md-7 pad">
              <input type="text" id="txtReceiveAmount" class="form-control" placeholder="Amount" />
                    </div>
                </div>
        </div>
        <div class="modal-footer">
                <button type="button" class="btn btn-secondary" data-dismiss="modal" >Cancel</button>
                <button type="button" class="btn btn-primary" id="btnStatusSave">Save</button>
        </div>
      </div>
      
    </div>
  </div>

    <script>
        function SubmitData(OrderId) {
            
            var name = 'Ram';
            var gender = 'Male';
            var age = '30';
            if (confirm("Mark Order NUmber:" + OrderId + " As Delivered, Are you sure?")) {
                $.ajax({
                    type: "POST",
                    url: "OrderList.aspx/SaveData",
                    data: '{"Id":"' + OrderId + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        showswalsuccess(OrderId + ' Order Number Marked As Delivered', 2000);
                        $("#del-" + OrderId).hide();
                        $("#can-" + OrderId).hide();
                    },
                    error: function (msg) {
                        msg = "There is an error";
                        alert(msg);
                    }
                });
            }
        }
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
    </script>

    <script type="text/javascript">


        $(document).ready(function () {
            $('#ContentPlaceHolder1_grdGrn').DataTable({
                "fixedHeader": false,
                "paging": false,
                "lengthChange": false,
                "searching": true,
                "ordering": true,
                "info": true,
                "autoWidth": true,
                dom: 'Bfrtip',
                buttons: [
                    'copy', 'csv', 'excel', 'pdf', 'print'
                ]
            });

            $("#btnStatusSave").click(function () {
                debugger
                //alert($("#hdnOrderId").val());
                var OrderId = $("#hdnOrderId").val();
                var receiveAmount =  $("#txtReceiveAmount").val();
                //window.location.href = "OrderList.aspx/SavePaymentStatusHistory?OrderId = " + OrderId + "&ReceiveAmount=" + receiveAmount;
                $.ajax({
                    type: "POST",
                    url: "OrderList.aspx/SavePaymentStatusHistory",
                    data: '{"OrderId":"' + OrderId + '","ReceiveAmount":"' + receiveAmount + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        $('#paymentStatusModal').modal('hide');
                        //window.location.href = "OrderList.aspx/fillData";
                        $("#<%=Button2.ClientID%>").click();
                    },
                    error: function (msg) {
                        msg = "There is an error";
                        alert(msg);
                    }
                });
            })
        });

        //$(document).ready(function () {
        //    $('#ContentPlaceHolder1_grdGrn').DataTable({
        //        "fixedHeader": true,
        //        "paging": false,
        //        "lengthChange": true,
        //        "searching": true,
        //        "ordering": false,
        //        "info": true,
        //        "autoWidth": true,

        //        dom: 'Bfrtip',
        //        buttons: [
        //            'copy', 'csv', 'excel', 'pdf', 'print'
        //        ]
        //    });
        //});
    </script>

    <script>
        function Cancel(OrderId) {

            var name = 'Ram';
            var gender = 'Male';
            var age = '30';
            if (confirm("Mark Order Number:" + OrderId + " As Cancelled, Are you sure?")) {
                $.ajax({
                    type: "POST",
                    url: "OrderList.aspx/SaveData1",
                    data: '{"Id":"' + OrderId + '"}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (data) {
                        showswalsuccess(OrderId + ' Order Number Marked As Cancelled', 2000);
                        $("#del-" + OrderId).hide();
                        $("#can-" + OrderId).hide();
                    },
                    error: function (msg) {
                        msg = "There is an error";
                        alert(msg);
                    }
                });
            }
        }

        function StatusUpdateModal(orderid) {
            //alert(orderid);
            $('#paymentStatusModal').modal('show');
            $("#hdnOrderId").val(orderid);

        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="Server">
</asp:Content>

