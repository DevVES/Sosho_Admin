<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="order_details.aspx.cs" Inherits="order_details" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            .
            <h1>Order Detail
               <small>
                   <asp:Label runat="server" ID="lblDateTime"></asp:Label>
               </small>
            </h1>
            <ol class="breadcrumb">
                <%--<li><a href="../Home1.aspx"><i class="fa fa-dashboard"></i>Old Dashboard</a></li>--%>
            </ol>
        </section>

        <style type="text/css">
            .content {
                min-height: 100vh;
            }
        </style>

        <section class="content">

            <div class="col-md-12 outer-sec">
                <div id="lbladdress" runat="server">
                    <div class="col-md-4 padding">
                        <div class="address-sec">
                            <h4>Delivery Address</h4>
                            <h5 id="lbladdname" runat="server">Pratixa patel</h5>
                            <div class="ship-address">
                                <p id="lbladd" runat="server">257, Shaktinath, G.H.B-392001</p>
                                <p id="lbladdstate" runat="server">Gujarat, India</p>
                            </div>
                            <p class="number">Phone Number: <span id="lbladdmob" runat="server">7069926537</span></p>
                        </div>
                    </div>
                </div>
                <div class="col-md-4 padding">
                    <div class="order-inner-sec">
                        <h4>Order Details</h4>
                        <table>
                            <tr>
                                <td colspan="1">Order Id:</td>
                                <td class="pleft" id="lblorderid" runat="server">102114</td>
                            </tr>
                            <tr>
                                <td colspan="1">Order Date:</td>
                                <td class="pleft" id="orderdatedid" runat="server">wednesday 5, 2019</td>
                            </tr>
                            <tr>
                                <td colspan="1">Order MRP:</td>
                                <td class="pleft" id="lblmrp" runat="server">Rs.450</td>
                            </tr>
                            <tr>
                                <td colspan="1">Order Amount:</td>
                                <td class="pleft" id="lbltotordeamt" runat="server">Rs.550</td>
                            </tr>
                            <tr>
                                <td colspan="1">Order Status:</td>
                                <td class="pleft" id="lblstatus" runat="server"></td>
                            </tr>
                        </table>
                    </div>
                    <%--<p>Order Id:<span>102114</span></p>
                       <p>Order Date:<span>wednesday 5, 2019</span></p>
                       <p>Order Total:<span>Rs.450</span></p>
                       <p>Order Amount Paid:<span>Rs.550</span></p>--%>
                </div>
                <div class="col-md-4 actions padding" style="display: none">
                    <h4>More Actions</h4>
                    <p>
                        <i class="fa fa-file-text-o icon"></i>Download Invoice<span><asp:Button CssClass="dl-btn" ID="Button1" runat="server" Text="Download" />
                        </span>
                    </p>
                    <%--<div class="share">
                            <p>Share with your friend</p>
                            <img src="images/facebook.png" />
                            <img src="images/instagram.png" />
                            <img src="images/whatsapp.png" />
                        </div>--%>
                </div>
            </div>

            <div class="col-md-12 ">

<%--                <div class="box box-warning box-solid">
                    <div class="box-header with-border">
                        <h3 class="box-title">Customer Reffere List</h3>

                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                        </div>
                        <!-- /.box-tools -->
                    </div>
                    <!-- /.box-header -->
                    <div class="box-body">
                        <asp:GridView ID="grd" CssClass="table table-hover table-responsive table-hover table-bordered" AllowSorting="True" Width="100%" runat="server" OnRowDataBound="grd_RowDataBound"></asp:GridView>
                    </div>
                    <!-- /.box-body -->
                </div>--%>
                <!-- /.box -->




            </div>

            <%--<div class="row">
                    <div class="col-md-12 outer-sec">
                        <div class="col-md-4 padding">
                            <div class="product-des">
                                <div class="col-md-12 padding-inner border">
                                    <div class="col-md-4 col-sm-4 col-xs-4 padding">
                                        <div id="lblimg123" runat="server">
                                            <img src="images/burfi.jpg" id="lblimg" runat="server" />
                                        </div>
                                    </div>
                                    <div class="col-md-8 col-sm-8 col-xs-8 padding">
                                        <p class="name" id="lblnamee" runat="server">Suleman Mithaiwala Sweets</p>
                                        <p class="qty">Qty:<span id="lblqtyno" runat="server">20</span></p>
                                        <div class="weight">
                                            <p>weight:<span id="lblweigh" runat="server">200-gm</span></p>
                                       
                                        </div>
                                       
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-4 padding" style="display: none">
                            <div class="group-detail">
                                <table>
                                    <tr>
                                        <td colspan="1">Group by discount</td>
                                        <td class="pleft">(-) ₹2000</td>
                                    </tr>
                                    <tr>
                                        <td colspan="1">Group by overage</td>
                                        <td class="pleft">₹2000</td>
                                    </tr>
                                    <tr>
                                        <td colspan="1">Grand Total</td>
                                        <td class="pleft">₹2000</td>
                                    </tr>
                                    <tr>
                                        <td colspan="1">Balancing Amount</td>
                                        <td class="pleft">₹2000</td>
                                    </tr>
                                </table>

                            </div>
                        </div>

                        <div class="col-md-4 padding">
                            <div class="delivered">
                                
                                <p class="method">Payment Method : <span id="lblpayment" runat="server"></span></p>
                            </div>
                        </div>
                    </div>
                </div>--%>
        </section>
    </div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="Server">
</asp:Content>

