<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ManageWallets.aspx.cs" Inherits="Wallet_ManageWallets" ValidateRequest="false" %>


<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <link rel="stylesheet" href="../../plugins/datepicker/datepicker3.css" />
    <link rel="stylesheet" href="../../plugins/timepicker/bootstrap-timepicker.min.css" />
    <script src="../../plugins/datepicker/bootstrap-datepicker.js"></script>
    <script src="../../plugins/timepicker/bootstrap-timepicker.min.js"></script>



    <script src="https://cdn.datatables.net/fixedheader/3.1.2/js/dataTables.fixedHeader.min.js"></script>
    <link rel="stylesheet" href="https://cdn.datatables.net/fixedheader/3.1.2/css/fixedHeader.dataTables.min.css" />
    <link href="https://code.jquery.com/ui/1.10.4/themes/ui-lightness/jquery-ui.css" rel="stylesheet" />
    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.5.6/js/dataTables.buttons.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.5.6/js/buttons.flash.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.5.6/js/buttons.html5.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.5.6/js/buttons.print.min.js"></script>


    <%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
    <%-- <script src="https://cdn.ckeditor.com/4.5.7/standard/ckeditor.js"></script>--%>
    <link rel="stylesheet" href="../../plugins/select2/select2.min.css" />
    <%--<script src="../../plugins/jQuery/jQuery-2.2.0.min.js"></script>--%>
    <%-- <script src="../../plugins/select2/select2.full.min.js"></script>--%>
    <style>
        .select2-container .select2-selection--single {
            height: 34px;
        }

        .block {
            height: 150px;
            width: 200px;
            border: 1px solid aliceblue;
            overflow-y: scroll;
        }
    </style>

    <style type="text/css">
        .content {
            min-height: 100vh;
        }
    </style>

    <div class="content-wrapper">
        <section class="content-header">
            <h1>Manage Wallets</h1>
        </section>

        <style type="text/css">
            .btn-block {
                display: block;
                width: 20%;
            }
        </style>

        <section class="content" style="">
            <div class="row">
                <div class=" col-md-12">
                    <div class="col-md-3">
                        <%--<asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" CssClass="btn btn-block btn-info" Width="120 px" OnClientClick="return Validate()" title="Save" />--%>
                        <asp:Label ID="lblmsg" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                    <div class="col-md-3">
                    </div>
                    <div class="col-md-3">
                    </div>
                    <div class="col-md-3">
                        <a href="WalletList.aspx" class="btn btn-block btn-success pull-right" style="width: 50%" target="_blank" title="Back to List">Back To List</a>
                    </div>
                </div>

            </div>

            <div id="Tabs" role="tabpanel">
                <br />
                <ul class="nav nav-tabs" role="tablist">
                    <li class="active">
                        <a href="#basic" aria-controls="basic" role="tab" data-toggle="tab" title="BASIC">BASIC</a></li>
                    <li>
                        <a href="#customerlist" aria-controls="customerlist" role="tab" data-toggle="tab" title="CUSTOMER" runat="server" id="customertab">CUSTOMER</a></li>

                </ul>
                <div class="tab-content table-responsive" style="padding-top: 20px">

                    <div role="tabpanel" class="tab-pane active" id="basic">
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblcname" runat="server" Text="Campaign Name"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtcname" runat="server" CssClass="form-control" Width="40%" placeholder="Campaign Name"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spncname" style="color: #d9534f; display: none;">This field is required</span>
                                    <input type="hidden" id="hdnIsAdmin" runat="server" />
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblOfferType" runat="server" Text="Offer Type"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:DropDownList ID="ddlOfferType" runat="server" Width="290px" class="form-control" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select Offer Type" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnOfferType" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>

                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblWalletAmount" runat="server" Text="Wallet Amount"></asp:Label>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtWalletAmount" runat="server" CssClass="form-control calculate" Width="40%" onkeypress="return isNumber(event)" placeholder="Wallet Amount"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnWalletAmount" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblcouponcode" runat="server" Text="Coupon Code"></asp:Label>
                                </div>
                                <div class="col-md-7 pad" style="padding-left: 0px;">
                                    <div class="col-md-5 pad">
                                        <asp:TextBox ID="txtcouponcode" runat="server" CssClass="form-control" placeholder="Coupon Code"> </asp:TextBox>
                                    </div>
                                    <div class="col-md-2 pad">
                                        <asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="btn btn-block btn-info" title="Generate" OnClick="BtnGenerate_Click" OnClientClick="return Validate()" />
                                    </div>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spncouponcode" style="color: #d9534f; display: none;" runat="server">This field is required</span>
                                </div>

                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblgrpType" runat="server" Text="Redemption Type"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:DropDownList ID="ddlgrpType" runat="server" class="form-control" Width="40%">
                                        <asp:ListItem Value="">Select Type</asp:ListItem>
                                        <asp:ListItem Value="%">%</asp:ListItem>
                                        <asp:ListItem Value="Fixed">Fixed</asp:ListItem>
                                        <asp:ListItem Value="Full Amount Applicable">Full Amount Applicable</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spngrpType" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblgrpTypeValue" runat="server" Text="Redemption Value"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtgrpTypeValue" runat="server" CssClass="form-control calculate" Width="40%" onkeypress="return isNumber(event)" placeholder="Redemption Value"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spngrpTypeValue" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblMinOrderAmt" runat="server" Text="Minimum Order Amount"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtgrpMinOrderAmt" runat="server" CssClass="form-control" Width="40%" placeholder="Minimum Order Amount"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spngrpMinOrderAmt" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblstartdate" runat="server" Text="StartDate"></asp:Label><span style="color: red">*</span>

                                </div>
                                <div class="col-md-9 pad" style="padding: 0">
                                    <div class="col-md-3 pad">
                                        <asp:TextBox ID="txtdt" CssClass="form-control" placeholder="Select Date" runat="server"></asp:TextBox>
                                        <script>
                                            $('#ContentPlaceHolder1_txtdt').datepicker({
                                                format: 'dd/M/yyyy',
                                                autoclose: true
                                            });
                                        </script>

                                    </div>
                                    <div class="col-md-3 pad">
                                        <div class="bootstrap-timepicker">

                                            <div class="input-group">

                                                <asp:TextBox runat="server" type="text" ID="txttime" class="form-control timepicker"></asp:TextBox>
                                                <div class="input-group-addon">
                                                    <i class="fa fa-clock-o"></i>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>

                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblenddate" runat="server" Text="EnDate"></asp:Label><span style="color: red">*</span>

                                </div>
                                <div class="col-md-9 pad" style="padding: 0">
                                    <div class="col-md-3 pad">
                                        <asp:TextBox ID="txtdt1" CssClass="form-control" placeholder="Select Date" runat="server"></asp:TextBox>
                                        <script>
                                            $('#ContentPlaceHolder1_txtdt1').datepicker({
                                                format: 'dd/M/yyyy',
                                                autoclose: true
                                            });
                                        </script>

                                    </div>
                                    <div class="col-md-3 pad">
                                        <div class="bootstrap-timepicker">
                                            <div class="input-group">
                                                <asp:TextBox runat="server" type="text" ID="txttime1" class="form-control timepicker"></asp:TextBox>
                                                <div class="input-group-addon">
                                                    <i class="fa fa-clock-o"></i>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </div>

                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblIsFirstOrderApplicable" runat="server" Text="Applicable For First Order"></asp:Label>
                                </div>
                                <div class="col-md-7 pad">
                                    <input id="chkIsFirstOrderApplicable" name="IsFirstOrderApplicable" type="checkbox" value="valIsFirstOrderApplicable" runat="server" />
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblApplyAllCustomer" runat="server" Text="Apply All Customer"></asp:Label>
                                </div>
                                <div class="col-md-7 pad">
                                    <%--<input id="chkApplyAllCustomer" name="IsApplyAllCustomer" type="checkbox" value="valIsApplyAllCustomer" runat="server" onserverchange="chkCustomer_Clicked" />--%>
                                    <asp:CheckBox ID="chkApplyAllCustomer" runat="server" AutoPostBack="true" name="IsApplyAllCustomer" value="valIsApplyAllCustomer" OnCheckedChanged="chkCustomer_Clicked" />
                                </div>
                            </div>
                        </div>


                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblisactive" runat="server" Text="IsActive"></asp:Label>
                                </div>
                                <div class="col-md-7 pad">
                                    <input id="chkisactive" name="isActive" type="checkbox" value="valactive" runat="server" />

                                </div>
                            </div>

                            <div class="row pad-bottom">
                                <div class="col-md-12">
                                    <div class="col-md-3 pad">
                                    </div>
                                    <div class="col-md-9 pad">
                                        <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="btn btn-block btn-info" Width="120 px" title="Save" OnClick="BtnSave_Click" OnClientClick="return Validate()" />
                                        <asp:Button ID="BtnCancel" runat="server" Text="Cancel" CssClass="btn btn-block btn-info" Width="120 px" title="Cancel" Visible="false" OnClientClick="return Validate()" />
                                    </div>
                                </div>
                            </div>

                            <script type="text/javascript" lang="javascript">
                                            function isNumber(evt) {
                                                var theEvent = evt || window.event;
                                                var key = theEvent.keyCode || theEvent.which;
                                                var keyCode = key;
                                                key = String.fromCharCode(key);
                                                if (key.length == 0) return;
                                                var regex = /^[0-9.\b]+$/;
                                                if (keyCode == 188 || keyCode == 190) {
                                                    return;
                                                } else {
                                                    if (!regex.test(key)) {
                                                        theEvent.returnValue = false;
                                                        if (theEvent.preventDefault) theEvent.preventDefault();
                                                    }
                                                }
                                            }
                            </script>



                        </div>



                    </div>

                    <div role="tabpanel" class="tab-pane" id="customerlist">
                        <div style="width: 100%;" class="table-responsive">
                            <asp:GridView ID="gvcustomerlist" runat="server" Width="95%" AutoGenerateColumns="False" class="table table-bordered table-hover"
                                rules="all" role="grid" CellPadding="10" CellSpacing="5" AllowSorting="True" HeaderStyle-BackColor="#ede8e8"
                                HeaderStyle-HorizontalAlign="Center" Caption="<b><u>CUSTOMER LIST</u></b>" CaptionAlign="Top">
                                <%--OnRowDataBound="gvcustomerlist_RowDataBound" OnRowCommand="gvcustomerlist_RowCommand" EnableViewState="False"--%>
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:CheckBox HeaderText="Apply" ID="Apply" runat="server" />
                                            <asp:HiddenField ID="HiddenFieldCustomerId" runat="server" Value='<%# Bind("Id") %>' />

                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Mobile" DataField="Mobile" ItemStyle-Width="150" />
                                    <asp:BoundField HeaderText="FirstName" DataField="FirstName" />
                                    <asp:BoundField HeaderText="LastName" DataField="LastName" />
                                    <asp:BoundField HeaderText="Email" DataField="Email" />
                                    <asp:BoundField HeaderText="Sex" DataField="Sex" />
                                    <asp:BoundField HeaderText="Address" DataField="Address" />
                                    <asp:BoundField HeaderText="Pincode" DataField="Pincode" />

                                </Columns>
                                <HeaderStyle HorizontalAlign="Center" BackColor="#EDE8E8"></HeaderStyle>
                            </asp:GridView>
                        </div>

                        <script type="text/javascript">
                                            $(document).ready(function () {
                                                $('#ContentPlaceHolder1_gvcustomerlist').DataTable({
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
                    </div>

                </div>
            </div>


            <script type="text/javascript" lang="javascript">
                                            function validatenumerics(key) {
                                                //getting key code of pressed key
                                                var keycode = (key.which) ? key.which : key.keyCode;
                                                //comparing pressed keycodes

                                                if (keycode > 31 && (keycode < 48 || keycode > 57)) {
                                                    alert(" You can enter only characters 0 to 9 ");
                                                    return false;
                                                }
                                                else return true;


                                            }
            </script>
            <script>
                                            var today = new Date();
                                            var dd = today.getDate();
                                            var mm = today.getMonth() + 1; //January is 0!
                                            var yyyy = today.getFullYear();
                                            if (dd < 10) {
                                                dd = '0' + dd
                                            }
                                            if (mm < 10) {
                                                mm = '0' + mm
                                            }
                                            today = dd + '-' + mm + '-' + yyyy;
                                            $('#txtdt').val(today);
                                            $('#txtdt').datepicker({
                                                format: 'dd-mm-yyyy',
                                                autoclose: true
                                            });
                                            $(".timepicker").timepicker({
                                                showInputs: false
                                            });
            </script>


            <script>
                                            var today = new Date();
                                            var dd = today.getDate();
                                            var mm = today.getMonth() + 1; //January is 0!
                                            var yyyy = today.getFullYear();
                                            if (dd < 10) {
                                                dd = '0' + dd
                                            }
                                            if (mm < 10) {
                                                mm = '0' + mm
                                            }
                                            today = dd + '-' + mm + '-' + yyyy;
                                            $('#txtdt1').val(today);
                                            $('#txtdt1').datepicker({
                                                format: 'dd-mm-yyyy',
                                                autoclose: true
                                            });
                                            $(".timepicker").timepicker({
                                                showInputs: false
                                            });


            </script>
            <script>

                                            $("#ContentPlaceHolder1_BtnSave").click(function () {
                                                var flag = true;
                                                var cnameval = $("#ContentPlaceHolder1_txtcname").val();
                                                var TypeNameval = $("#ContentPlaceHolder1_ddlgrpType").val();
                                                var grpTypeval = $("#ContentPlaceHolder1_txtgrpTypeValue").val();
                                                var couponCode = $("#ContentPlaceHolder1_txtcouponcode").val();
                                                var walletAmount = $("#ContentPlaceHolder1_txtWalletAmount").val();

                                                var MinOrderAmt = $("#ContentPlaceHolder1_txtgrpMinOrderAmt").val();
                                                var offertype = $("#ContentPlaceHolder1_ddlOfferType").val();
                                                if (cnameval == "") {
                                                    $("#spncname").css('display', 'block');
                                                    flag = false;
                                                }
                                                if (offertype == "") {
                                                    $("#spnOfferType").css('display', 'block');
                                                    flag = false;
                                                }
                                                if (walletAmount == "" && offertype == "1") {
                                                    $("#spnWalletAmount").css('display', 'block');
                                                    flag = false;
                                                }
                                                if (couponCode == "" && offertype == "3") {
                                                    $("#spncouponcode").css('display', 'block');
                                                    flag = false;
                                                }
                                                if (TypeNameval == "") {
                                                    $("#spngrpType").css('display', 'block');
                                                    flag = false;
                                                }
                                                if (grpTypeval == "") {
                                                    $("#spngrpTypeValue").css('display', 'block');
                                                    flag = false;
                                                }
                                                if (MinOrderAmt == "") {
                                                    $("#spngrpMinOrderAmt").css('display', 'block');
                                                    flag = false;
                                                }

                                                if (flag) {
                                                    $('#ContentPlaceHolder1_BtnSave').click();
                                                }
                                                return flag;
                                            });

            </script>
            <script>
                $("#ContentPlaceHolder1_ddlgrpType").change(function () {
                    var type = $("#ContentPlaceHolder1_ddlgrpType").val();
                    var walletAmt = $("#ContentPlaceHolder1_txtWalletAmount").val();
                    if (type == "Full Amount Applicable") {
                        $("#ContentPlaceHolder1_txtgrpTypeValue").val(walletAmt);
                    }

                });

                $("#ContentPlaceHolder1_ddlOfferType").change(function () {
                    var offertype = $("#ContentPlaceHolder1_ddlOfferType").val();
                    $("#ContentPlaceHolder1_txtWalletAmount").hide();
                    $("#ContentPlaceHolder1_lblWalletAmount").hide();
                    $("#ContentPlaceHolder1_txtcouponcode").hide();
                    $("#ContentPlaceHolder1_lblcouponcode").hide();
                    $("#ContentPlaceHolder1_btnGenerate").hide();
                    if (offertype == "1") {
                        $("#ContentPlaceHolder1_txtWalletAmount").show();
                        $("#ContentPlaceHolder1_lblWalletAmount").show();
                    }
                    if (offertype == "3") {
                        $("#ContentPlaceHolder1_txtcouponcode").show();
                        $("#ContentPlaceHolder1_lblcouponcode").show();
                        $("#ContentPlaceHolder1_btnGenerate").show();
                    }
                });

            </script>
        </section>
    </div>
</asp:Content>
