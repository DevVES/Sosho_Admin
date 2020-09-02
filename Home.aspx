<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" ValidateRequest="false" AutoEventWireup="true" CodeFile="Home.aspx.cs" Inherits="Home" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <link rel="stylesheet" href="../../plugins/datepicker/datepicker3.css" />
    <script src="../../plugins/datepicker/bootstrap-datepicker.js"></script>
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">

            <h1>Dashboard
               <small>
                   <asp:Label runat="server" ID="lblDateTime"></asp:Label>
               </small>
            </h1>
            <asp:Literal ID="ltrErr" runat="server"></asp:Literal>
            <ol class="breadcrumb">
            </ol>
        </section>
        <section class="content">
            <!-- Small boxes (Stat box) -->
            <div class="row">
                <div class="col-md-12">
                    <div style="display: none">
                        <input type="text" id="lblenddate" runat="server" />
                    </div>
                    <div class="alert alert-success alert-dismissible">
                        <div id="divTest" runat="server"></div>
                    </div>
                </div>
            </div>
            <div class="row">
                <div class="col-md-12">
                    <div class="col-md-1"></div>
                    <div class="col-md-2" style="text-align: center">
                        <input type="button" value="  <<  " id="prvalldate" />
                    </div>
                    <div class="col-md-2">
                    </div>
                    <div class="col-md-2" style="text-align: center">
                        <input type="button" value="  >>  " id="nextalldate" /><br />
                    </div>

                </div>

                <div class="col-md-12" style="padding: 5px">
                    <div class="col-md-1">
                        <asp:Label ID="Label2" runat="server" Text="From Date"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <%--<input type="button" value="<<" id="prvalldate" /><br />--%>
                        <input type="button" value="<" id="prvfromdate" />
                        <i class="fa fa-calendar"></i>
                        <asp:TextBox ID="txtDate" runat="server" Width="60%" CssClass="form-control inline" AutoCompleteType="Disabled">
                        </asp:TextBox>
                        <input type="button" value=">" id="nextfromdate" /><br />
                        <ajaxToolkit:CalendarExtender Format="dd-MM-yyyy" TargetControlID="txtDate" ID="CalendarExtender1" runat="server"></ajaxToolkit:CalendarExtender>
                        &nbsp;  
                    </div>

                    <div class="col-md-1">
                        <asp:Label ID="Label1" runat="server" Text="To Date"></asp:Label>
                    </div>
                    <div class="col-md-3">
                        <%--<input type="button" value=">>" id="nextalldate" /><br />--%>
                        <input type="button" value="<" id="prvtodate" />
                        <i class="fa fa-calendar"></i>
                        <asp:TextBox ID="txtDate1" runat="server" Width="60%" CssClass="form-control inline" AutoCompleteType="Disabled">
                        </asp:TextBox>
                        <input type="button" value=">" id="nexttodate" /><br />
                        <ajaxToolkit:CalendarExtender Format="dd-MM-yyyy" TargetControlID="txtDate1" ID="CalendarExtender2" runat="server"></ajaxToolkit:CalendarExtender>

                    </div>
                    <div class="col-md-1">
                        <asp:Button ID="cmdGo" class="btn btn-block btn-primary" Width="100px" runat="server" Text="Go" OnClick="Button1_Click"></asp:Button>
                    </div>
                    <div class="col-md-3">
                    </div>
                </div>

            </div>
            <div class="row">

                <div class="col-lg-3 col-xs-6">
                    <!-- small box -->
                    <div class="small-box bg-aqua">
                        <div class="inner">
                            <h3>
                                <asp:Literal ID="ltrReqVsGrn" runat="server"></asp:Literal>
                            </h3>
                            <p>Total Orders</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion-bag"></i>
                        </div>
                        <a href="OrderList.aspx" target="_blank" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="col-lg-3 col-xs-6">
                    <!-- small box -->
                    <div class="small-box bg-olive">
                        <div class="inner">
                            <h3>
                                <asp:Literal ID="ltrcustomer" runat="server"></asp:Literal>
                            </h3>
                            <p>Total Registration</p>
                        </div>
                        <div class="icon">
                            <i class="ion ion ion-ios-people-outline"></i>
                        </div>
                        <a href="#" target="_blank" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>

                <div class="col-lg-3 col-xs-6">
                    <!-- small box -->
                    <div class="small-box bg-yellow-gradient">
                        <div class="inner">
                            <h3>
                                <asp:Literal ID="Ltr1" runat="server"></asp:Literal>
                            </h3>
                            <p>Buy Alone</p>
                        </div>
                        <div class="icon">
                            <i class="ion-android-person"></i>

                        </div>
                        <a href="#" target="_blank" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="col-lg-3 col-xs-6">
                    <!-- small box -->
                    <div class="small-box bg-purple-gradient">
                        <div class="inner">
                            <h3>
                                <asp:Literal ID="Literal2" runat="server"></asp:Literal></h3>

                            <p>Buy With 1</p>
                        </div>
                        <div class="icon">
                            <i class="ion-android-people"></i>

                        </div>
                        <a href="#" target="_blank" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
                <div class="col-lg-3 col-xs-6">
                    <!-- small box -->
                    <div class="small-box bg-red-gradient">
                        <div class="inner">
                            <h3>
                                <asp:Literal ID="Ltr3" runat="server"></asp:Literal></h3>

                            <p>Buy With 5</p>
                        </div>
                        <div class="icon">
                            <i class="fa fa-users"></i>

                        </div>
                        <a href="#" target="_blank" class="small-box-footer">More info <i class="fa fa-arrow-circle-right"></i></a>
                    </div>
                </div>
              
                    
                <!-- right col -->
            </div>
            <div class="box" style="display:none"  >
                <div class="box-body">

                    <div class="row" >
                        <div class="col-md-6"  >
                            <div class="box box-warning"  >

                                <div class="box-header" >
                                    <h3 class="box-title">Last 5 Order.</h3>
                                </div>
                                <div class="table-responsive" >
                                    <asp:GridView ID="grd_Lst_5_Order" AutoGenerateColumns="false" runat="server" AllowSorting="True" CssClass="table dataTable table-hover table-bordered table-responsive" Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="Id" HeaderText="Id" />
                                            <asp:BoundField DataField="Name" HeaderText="Customer Name" />
                                            <asp:BoundField DataField="MobileNumber" HeaderText="Mobile Number" />
                                            <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                            <asp:BoundField DataField="OrderDate" HeaderText="Order Date" />
                                            <asp:BoundField DataField="TotalQty" HeaderText="Qty." />
                                        </Columns>
                                    </asp:GridView>
                                </div>

                            </div>
                        </div>

                        <div class="col-md-6"  >
                            <div class="box box-warning">
                                <div class="box-header" >
                                    <h3 class="box-title">Last 5 Registration.</h3>
                                </div>
                                <div class="table-responsive">
                                    <asp:GridView ID="grd_Lst_5_Regis" AutoGenerateColumns="false" runat="server" AllowSorting="True" CssClass="table dataTable table-hover table-bordered table-responsive" Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="Id" HeaderText="Customer Id" />
                                            <asp:BoundField DataField="Custname" HeaderText="Name" />
                                            <asp:BoundField DataField="MobileNumber" HeaderText="Mobile Number" />
                                            <asp:BoundField DataField="PinCode" HeaderText="PinCode" />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    
                    
                        <div class="col-md-6">
                            <div class="box box-warning" >
                                <div class="box-header" >
                                    <h3 class="box-title">Last 5 Buy with Alone Order.</h3>
                                </div>
                                <div class="table-responsive">
                                    <asp:GridView ID="gr_Lst_5_Buy_with_lon_order" AutoGenerateColumns="false" runat="server" AllowSorting="True" CssClass="table dataTable table-hover table-bordered table-responsive" Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="Id" HeaderText="Id" />
                                            <asp:BoundField DataField="Name" HeaderText="Customer Name" />
                                            <asp:BoundField DataField="MobileNumber" HeaderText="Mobile Number" />
                                            <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                            <asp:BoundField DataField="OrderDate" HeaderText="OrderDate" />
                                            <asp:BoundField DataField="TotalQty" HeaderText="Qty." />
                                            <%--<asp:BoundField DataField="BuyWith" HeaderText="BuyWith" />--%>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-6" >
                            <div class="box box-warning" >
                                <div class="box-header" >
                                    <h3 class="box-title">Last 5 Buy with 1 Order.</h3>
                                </div>
                                <div class="table-responsive" >
                                    <asp:GridView ID="grd_lst_5_buy_with_2" AutoGenerateColumns="false" runat="server" AllowSorting="True" CssClass="table dataTable table-hover table-bordered table-responsive" Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="Id" HeaderText="Id" />
                                            <asp:BoundField DataField="Name" HeaderText="Customer Name" />
                                            <asp:BoundField DataField="MobileNumber" HeaderText="Mobile Number" />
                                            <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                            <asp:BoundField DataField="OrderDate" HeaderText="OrderDate" />
                                            <asp:BoundField DataField="TotalQty" HeaderText="Qty." />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    
                   
                        <div class="col-md-6"  >
                            <div class="box box-warning" >
                                <div class="box-header" >
                                    <h3 class="box-title">Last 5 Buy with 6 Order.</h3>
                                </div>
                                <div class="table-responsive" >
                                    <asp:GridView ID="grd_last_5Buy_with_6" AutoGenerateColumns="false" runat="server" AllowSorting="True" CssClass="table dataTable table-hover table-bordered table-responsive" Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="Id" HeaderText="Id" />
                                            <asp:BoundField DataField="Name" HeaderText="Customer Name" />
                                            <asp:BoundField DataField="MobileNumber" HeaderText="Mobile Number" />
                                            <asp:BoundField DataField="ProductName" HeaderText="Product Name" />
                                            <asp:BoundField DataField="OrderDate" HeaderText="OrderDate" />
                                            <asp:BoundField DataField="TotalQty" HeaderText="Qty." />
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                    <!-- /.row (main row) -->
                    <div class="row">
                        <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12" style="display: none">
                            <div class="box box-warning">
                                <div class="box-header">
                                    <h3 class="box-title">Last 50 Order.</h3>
                                </div>
                                <div class="table-responsive">
                                    <asp:GridView ID="grdGrn" AutoGenerateColumns="false" runat="server" AllowSorting="True" CssClass="table dataTable table-hover table-bordered table-responsive" OnRowDataBound="GridView1_RowDataBound" Width="100%">
                                        <Columns>
                                            <asp:BoundField DataField="ordid" HeaderText="Order Id" />
                                            <asp:BoundField DataField="Mobile" HeaderText="Customer Mobile No" />
                                            <asp:BoundField DataField="Custname" HeaderText="Customer Name" />
                                            <asp:BoundField DataField="cadd" HeaderText="Address" />
                                            <asp:BoundField DataField="PinCode" HeaderText="Pin Code" />
                                            <asp:BoundField DataField="OrderDate" HeaderText="OrderDate" />
                                            <asp:BoundField DataField="Name" HeaderText="Product Name" />
                                            <asp:BoundField DataField="PaymentAmt" HeaderText="Payment Amount" />
                                            <asp:BoundField DataField="Totalamt" HeaderText="Totoal Amount" />
                                            <asp:BoundField DataField="TotalQty" HeaderText="Qty." />
                                            <asp:BoundField DataField="Ex" HeaderText="Oder Status" />
                                            <%--<asp:BoundField DataField="TotalGram" HeaderText="Quality" />--%>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </div>
                            <div class="box-footer text-center">
                                <a target="_blank" href="OrderList.aspx">View More<i class="fa fa-fw fa-external-link-square"></i></a>
                            </div>
                        </div>

                    </div>
                </div>
            </div>
        </section>
        <!-- Main content -->
        <!-- /.content -->
    </div>
    <script>
        var dDate = $("#<%= txtDate.ClientID %>").val();
        var dDate1 = $("#<%= txtDate1.ClientID %>").val();


        $(document).ready(function () {

            //For Timer

            var data123 = $("#ContentPlaceHolder1_lblenddate").val();


            var ddd = Date.parse(data123);
            //var countDownDate = new Date("Oct 15, 2019 12:00:00").getTime();
            var countDownDate = new Date(ddd).getTime()

            // Update the count down every 1 second
            var x = setInterval(function () {

                // Get today's date and time
                var now = new Date().getTime();

                // Find the distance between now and the count down date
                var distance = countDownDate - now;

                // Time calculations for days, hours, minutes and seconds
                var days = Math.floor(distance / (1000 * 60 * 60 * 24));
                var hours = Math.floor((distance % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
                var minutes = Math.floor((distance % (1000 * 60 * 60)) / (1000 * 60));
                var seconds = Math.floor((distance % (1000 * 60)) / 1000);

                // Output the result in an element with id="demo"
                //document.getElementById("demo").innerHTML = days + "d " + hours + "h "
                //+ minutes + "m " + seconds + "s ";
                document.getElementById("demo").innerHTML = days + "d " + hours + "h "
               + minutes + "m " + seconds + "s ";

                // If the count down is over, write some text 
                if (distance < 0) {
                    clearInterval(x);
                    document.getElementById("demo").innerHTML = "EXPIRED";
                }
            }, 1000);


            //for Start Date End Date

            $("#nextalldate").click(function () {
                var txtdt1 = document.getElementById("<%=txtDate.ClientID%>").value;
                var txtdt2 = document.getElementById("<%=txtDate1.ClientID%>").value;
                //alert(txtdt1 + " " + txtdt2);

                var cvdate = txtdt1.split('-');
                var cvdate1 = txtdt2.split('-');


                var myDate = new Date(cvdate[2] + "-" + cvdate[1] + "-" + cvdate[0]);
                var myDate1 = new Date(cvdate1[2] + "-" + cvdate1[1] + "-" + cvdate1[0]);

                myDate.setDate(myDate.getDate() + 1);
                myDate1.setDate(myDate1.getDate() + 1);

                var date = myDate.getDate();
                if (date.toString().length == 1) {
                    date = "0" + date;
                }
                var month = myDate.getMonth() + 1; // january is month 0 in javascript
                if (month.toString().length == 1) {
                    month = "0" + month;
                }
                var year = myDate.getFullYear();
                var nextdate = date + "-" + month + "-" + year;
                //$("#txtDate").val(nextdate);
                document.getElementById("<%=txtDate.ClientID%>").value = nextdate;

                var date1 = myDate1.getDate();
                if (date1.toString().length == 1) {
                    date1 = "0" + date1;
                }
                var month1 = myDate1.getMonth() + 1; // january is month 0 in javascript
                if (month1.toString().length == 1) {
                    month1 = "0" + month1;
                }
                var year1 = myDate1.getFullYear();
                var nextdate1 = date1 + "-" + month1 + "-" + year1;
                //$("#txtDate").val(nextdate);
                document.getElementById("<%=txtDate1.ClientID%>").value = nextdate1;
            });

            $("#prvalldate").click(function () {
                var txtdt1 = document.getElementById("<%=txtDate.ClientID%>").value;
                var txtdt2 = document.getElementById("<%=txtDate1.ClientID%>").value;
                //alert(txtdt1 + " " + txtdt2);
                var cvdate = txtdt1.split('-');
                var cvdate1 = txtdt2.split('-');

                var myDate = new Date(cvdate[2] + "-" + cvdate[1] + "-" + cvdate[0]);
                var myDate1 = new Date(cvdate1[2] + "-" + cvdate1[1] + "-" + cvdate1[0]);

                myDate.setDate(myDate.getDate() - 1);
                myDate1.setDate(myDate1.getDate() - 1);
                var date = myDate.getDate();
                if (date.toString().length == 1) {
                    date = "0" + date;
                }
                var month = myDate.getMonth() + 1; // january is month 0 in javascript
                if (month.toString().length == 1) {
                    month = "0" + month;
                }
                var year = myDate.getFullYear();
                var nextdate = date + "-" + month + "-" + year;
                //$("#txtDate").val(nextdate);
                document.getElementById("<%=txtDate.ClientID%>").value = nextdate;

                var date1 = myDate1.getDate();
                if (date1.toString().length == 1) {
                    date1 = "0" + date1;
                }
                var month1 = myDate1.getMonth() + 1; // january is month 0 in javascript
                if (month1.toString().length == 1) {
                    month1 = "0" + month1;
                }
                var year1 = myDate1.getFullYear();
                var nextdate1 = date1 + "-" + month1 + "-" + year1;
                //$("#txtDate").val(nextdate);
                document.getElementById("<%=txtDate1.ClientID%>").value = nextdate1;


            });

            $("#nextfromdate").click(function () {
                var txtdt1 = document.getElementById("<%=txtDate.ClientID%>").value;
                //alert(txtdt1 + " " + txtdt2);
                var cvdate = txtdt1.split('-');
                var myDate = new Date(cvdate[2] + "-" + cvdate[1] + "-" + cvdate[0]);
                myDate.setDate(myDate.getDate() + 1);
                var date = myDate.getDate();
                if (date.toString().length == 1) {
                    date = "0" + date;
                }
                var month = myDate.getMonth() + 1; // january is month 0 in javascript
                if (month.toString().length == 1) {
                    month = "0" + month;
                }
                var year = myDate.getFullYear();
                var nextdate = date + "-" + month + "-" + year;
                //$("#txtDate").val(nextdate);
                document.getElementById("<%=txtDate.ClientID%>").value = nextdate;
            });
            $("#nexttodate").click(function () {
                var txtdt1 = document.getElementById("<%=txtDate1.ClientID%>").value;
                //alert(txtdt1 + " " + txtdt2);
                var cvdate = txtdt1.split('-');
                var myDate = new Date(cvdate[2] + "-" + cvdate[1] + "-" + cvdate[0]);
                myDate.setDate(myDate.getDate() + 1);
                var date = myDate.getDate();
                if (date.toString().length == 1) {
                    date = "0" + date;
                }
                var month = myDate.getMonth() + 1; // january is month 0 in javascript
                if (month.toString().length == 1) {
                    month = "0" + month;
                }
                var year = myDate.getFullYear();
                var nextdate = date + "-" + month + "-" + year;
                //$("#txtDate").val(nextdate);
                document.getElementById("<%=txtDate1.ClientID%>").value = nextdate;
            });
            $("#prvfromdate").click(function () {

                var txtdt1 = document.getElementById("<%=txtDate.ClientID%>").value;
                //alert(txtdt1 + " " + txtdt2);
                var cvdate = txtdt1.split('-');
                var myDate = new Date(cvdate[2] + "-" + cvdate[1] + "-" + cvdate[0]);
                myDate.setDate(myDate.getDate() - 1);
                var date = myDate.getDate();
                if (date.toString().length == 1) {
                    date = "0" + date;
                }
                var month = myDate.getMonth() + 1; // january is month 0 in javascript
                if (month.toString().length == 1) {
                    month = "0" + month;
                }
                var year = myDate.getFullYear();
                var nextdate = date + "-" + month + "-" + year;
                //$("#txtDate").val(nextdate);
                document.getElementById("<%=txtDate.ClientID%>").value = nextdate;
            });
            $("#prvtodate").click(function () {

                var txtdt1 = document.getElementById("<%=txtDate1.ClientID%>").value;
                //alert(txtdt1 + " " + txtdt2);
                var cvdate = txtdt1.split('-');
                var myDate = new Date(cvdate[2] + "-" + cvdate[1] + "-" + cvdate[0]);
                myDate.setDate(myDate.getDate() - 1);
                var date = myDate.getDate();
                if (date.toString().length == 1) {
                    date = "0" + date;
                }
                var month = myDate.getMonth() + 1; // january is month 0 in javascript
                if (month.toString().length == 1) {
                    month = "0" + month;
                }
                var year = myDate.getFullYear();
                var nextdate = date + "-" + month + "-" + year;
                //$("#txtDate").val(nextdate);
                document.getElementById("<%=txtDate1.ClientID%>").value = nextdate;
            });

        });



    </script>

</asp:Content>
