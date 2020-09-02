<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="AddCustomerReview.aspx.cs" Inherits="CustomerRelationship_AddCustomerReview" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <link rel="stylesheet" href="../../bootstrap/css/bootstrap.min.css">
    <link rel="stylesheet" href="../../plugins/datepicker/datepicker3.css">
    <script src="../../plugins/datepicker/bootstrap-datepicker.js"></script>
    <script src="../../bootstrap/js/bootstrap.min.js"></script>
    <link rel="stylesheet" href="../../plugins/timepicker/bootstrap-timepicker.min.css" />
    <script src="../../plugins/timepicker/bootstrap-timepicker.min.js"></script>
    <!-- Bootstrap Color Picker -->

    <!-- FastClick -->
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
                <h1>Add Customer Review for Jobcard No. <%=Request.QueryString["Id"] %>
                    <small>
                        <asp:label runat="server" id="lblDateTime"></asp:label>
                    </small>
                </h1>
                <script>
                    document.body.classList.add("sidebar-collapse");
                </script>
                <ol class="breadcrumb">
                </ol>
            </section>
            <section class="content">
                <input type="hidden" value="1" id="chks" />
                <div class="box">
                    <div class="box-body table-responsive">
                        <div class="row">
                            <div class="col-sm-4 col-xs-12">
                                <div class="description-block border-right">
                                    <span class="description-header text-green" style="font-size: 25px;"><i class="fa fa-fw fa-user"></i></span>
                                    <h5 class="description-header">Customer Detail.</h5>
                                    <span class="description-text" id="txtCust" runat="server"></span>
                                </div>
                                <!-- /.description-block -->
                            </div>
                            <!-- /.col -->
                            <div class="col-sm-4 col-xs-12">
                                <div class="description-block border-right">
                                    <span class="description-percentage text-yellow" style="font-size: 25px;"><i class="fa fa-fw fa-car"></i></span>
                                    <h5 class="description-header">Vehicle Detail.</h5>
                                    <span class="description-text" id="txtV" runat="server"></span>
                                </div>
                                <!-- /.description-block -->
                            </div>
                            <div class="col-sm-4 col-xs-12">
                                <div class="description-block border-right">
                                    <span class="description-percentage text-red" style="font-size: 25px;"><i class="fa fa-fw fa-rupee"></i></span>
                                    <h5 class="description-header" id="txtTEA" runat="server"></h5>
                                    <span class="description-text" id="txtTPA" runat="server"></span>
                                </div>
                                <!-- /.description-block -->
                            </div>
                            <!-- /.col -->
                        </div>
                        <div class="row">
                            <hr style="width: 100%;" />
                        </div>
                        <div class="row">
                             <div class="col-sm-06 col-xs-12">
                                     <asp:GridView AutoGenerateColumns="true" Width="100%" CssClass="table table-hover dataTable table-responsive table-bordered table-striped no-footer" ID="GridView1" runat="server" EmptyDataText="No History">
                                     </asp:GridView>
                             </div>
                        </div>
                        <div class="row">
                            <div class="col-sm-4 col-xs-12">
                                <div class="user-block">
                                    <center> <span class="username pull-left">
                                        <a href="#">Customer Demand(s).</a>
                                    </span></center>
                                </div>
                                <!-- /.user-block -->
                                <p style="margin-left: 5px;" runat="server" id="txtcd">
                                </p>
                            </div>
                            <div class="col-sm-8 col-xs-12">
                                <div class="user-block">
                                    <center><span class="username ">
                                        <a href="#">Customer Review</a>
                                    </span> </center>
                                </div>
                                <center>
                                <div class="form-group" id="divrate">
                                    <%--<label><input type="radio" name="r3"  id="chk-1" onchange="chkchng(1)" class="flat-red"  checked />Satisfied</label> <label><input type="radio" name="r3" id="chk-2" class="flat-red" onchange="    chkchng(2)" />Not Know Call Later</label> <label><input type="radio" onchange="chkchng(3)" name="r3" id="chk-3" class="flat-red" />Partially Satisfied</label> <label><input onchange="chkchng(4)" type="radio" name="r3" id="chk-4" class="flat-red" />Dissatisfied</label> --%></div>
                                    <div class="form-group" id="divdate"  style="display:none;">
                                        <table><tr><td>
                                            <label class="pull-left">Next Date:</label>
                                            <div class="input-group date">
                                                <div class="input-group-addon">
                                                    <i class="fa fa-calendar"></i>
                                                </div>
                                                <input type="text" class="form-control pull-right" runat="server" name="startdate" id="startdate">
                                                 <script>
                                                     $('#ContentPlaceHolder1_startdate').datepicker({
                                                         format: 'dd/M/yyyy',
                                                         autoclose: true
                                                     });
                                                 </script>
                                            </div>
                                            </td><td>
                                            <!-- /.input group -->
                                           <div class="bootstrap-timepicker">
                                               
                                        <div class="input-group">
                                            <input type="text" id="txttime"  class="form-control timepicker pull-right" runat="server" />
                                            <script>
                                                $(".timepicker").timepicker({
                                                    showInputs: false
                                                });
                                            </script>
                                            <div class="input-group-addon">
                                                <i class="fa fa-clock-o"></i>
                                            </div>
                                        </div>
                                    </div>
                                            </td></tr></table>
                                        </div>
                                 
                                    <div class="form-group" id="divcomment" style="display:none;">
                                            <label class="pull-left">Comment:</label>
                                            <div class="input-group date">
                                                <div class="input-group-addon">
                                                    <i class="fa fa-fw fa-commenting-o"></i>
                                                </div>
                                                <textarea type="text" runat="server" class="form-control comment pull-right" id="txtc"/>
                                                 <script>
                                                     $('#ContentPlaceHolder1_startdate').datepicker({
                                                         format: 'dd/M/yyyy',
                                                         autoclose: true
                                                     });
                                                 </script>
                                            </div>
                                            <!-- /.input group -->
                                        </div>
                                     <div class="form-group" id="divbtn" style="display:none;">
                                            <label class="pull-left">&nbsp;</label>
                                            <div class="input-group date">
                                
                                                <input type="button" id="btnsave" onclick="SaveData()" style="width:150px;" class="btn btn-block btn-success pull-left"  value="Save"/>
                                            </div>
                                            <!-- /.input group -->
                                        </div>
                                </center>
                            </div>
                        </div>
                        <div class="row">
                            <hr style="width: 100%;" />
                        </div>
                        <div class="col-md-12">
                            <div class="nav-tabs-custom">
                                <ul class="nav nav-tabs">
                                    <li class="active"><a href="#activity" data-toggle="tab" aria-expanded="true">Customer</a></li>
                                    <li class="" id="lnkInsurance" runat="server"><a href="#timeline" data-toggle="tab" aria-expanded="false">Insurance</a></li>
                                </ul>
                                <div class="tab-content">
                                    <div class="tab-pane active" id="activity">
                                        <iframe style="width: 100%; height: 500px" src="../Accounts/PerformaInvoice.aspx?JobCardId=<%=(String.IsNullOrWhiteSpace(Request.QueryString["Id"])?"0":Request.QueryString["Id"]) %>&type=1"></iframe>
                                    </div>
                                    <div class="tab-pane" id="timeline">
                                        <iframe style="width: 100%; height: 500px" src="../Accounts/PerformaInvoice.aspx?JobCardId=<%=(String.IsNullOrWhiteSpace(Request.QueryString["Id"])?"0":Request.QueryString["Id"]) %>&type=2"></iframe>
                                    </div>
                                </div>
                                <!-- /.tab-content -->
                            </div>
                            <!-- /.nav-tabs-custom -->
                        </div>

                    </div>
                </div>
            </section>
        </div>
    </div>
    <%--<script src="../../plugins/jQuery/jQuery-2.2.0.min.js"></script>--%>

    <!-- Bootstrap 3.3.6 -->

    <!-- Select2 -->
    <script>
        $(document).ready(function () {
            fillReview();
        });

        function chkchng(id) {
            $("#chks").val(id);
            $("#divdate").hide();
            $("#divcomment").hide();
            $("#divbtn").show();
            if (id == 2) {
                $("#divdate").show();
                $("#divcomment").show();
                $("#divbtn").show();
            }
            if (id == 3 || id == 4) {
                $("#divcomment").show();
                $("#divbtn").show();
            }
        }
        function fillReview() {
            //<label><input type="radio" name="r3" class="flat-red" checked>Satisfied</label>
            $.ajax({
                type: "POST",
                url: urlstr + "/getCustomerreviewMasterItems1",
                data: { "Id": '<%=Request.QueryString["Id"]%>' },
                dataType: "json",
                success: function (data) {
                    var ischeck = 0;
                    var str = "";

                    for (i = 0; i < data.length; i++) {
                        if (data[i].Selected == "1") {
                            ischeck = 1;
                        }
                    }
                    for (i = 0; i < data.length; i++) {
                        str += "<label><input type='radio' onclick='chkchng(" + data[i].Id + ")' name='r3' id='chk-" + data[i].Id + "' class='flat-red' " + (data[i].Selected == "1" || (ischeck == 0 && data[i].Id == "1") ? "checked" : "") + ">" + data[i].Name + "</label> ";
                        if (data[i].Selected == "1") {
                            $("#chk-" + data[i].Id).attr('checked', true);
                            chkchng(data[i].Id);
                        }
                    }
                    if (ischeck == 1) {

                    }
                    else {
                        $("#chk-1").attr('checked', true);
                        chkchng(1);
                    }
                    $("#divrate").html(str);
                    return true;
                },
                failure: function (data) {
                }
            });
        }
        function SaveData() {
            $("#btnsave").prop('disabled', true);
            var id = '<%=Request.QueryString["Id"]%>';
            var chkid = $("#chks").val();
            var dt = $("#ContentPlaceHolder1_startdate").val() + " " + $("#ContentPlaceHolder1_txttime").val();
            var cmt = $("#ContentPlaceHolder1_txtc").val();
            if (chkid == 2) {
                if (dt == "") {
                    $("#modal-danger-all").modal('show');
                    $("#modal-danger-all-body").html("Add Comment...");
                    return;
                }
            }
            if (chkid == 3 || chkid == 4) {
                if (cmt == "") {
                    $("#modal-danger-all").modal('show');
                    $("#modal-danger-all-body").html("Add Comment...");
                    return;
                }
            }
            $.ajax({
                type: "GET",
                url: urlstr + "/updateReview",
                data: { "id": id, "chkid": chkid, "cnt": cmt, "dt": dt },
                dataType: "json",
                success: function (data) {
                    if (data.Id == "1") {
                        showswalsuccess("Review has been save successfully.", 2500);
                        setTimeout(function () {
                            window.close();
                        }, 2500)
                    }
                    else {
                        $("#btnsave").prop('disabled', false);
                        $("#modal-danger-all").modal('show');
                        $("#modal-danger-all-body").html("Something Went Wrong..");
                    }
                }
            });
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="Server">
</asp:Content>








