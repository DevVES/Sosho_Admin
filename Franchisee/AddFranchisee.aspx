<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="AddFranchisee.aspx.cs" Inherits="Franchisee_AddFranchisee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <link rel="stylesheet" href="../../plugins/datepicker/datepicker3.css" />
    <link rel="stylesheet" href="../../plugins/timepicker/bootstrap-timepicker.min.css" />
    <script src="../../plugins/datepicker/bootstrap-datepicker.js"></script>
    <script src="../../plugins/timepicker/bootstrap-timepicker.min.js"></script>
    <link href="https://code.jquery.com/ui/1.10.4/themes/ui-lightness/jquery-ui.css" rel="stylesheet" />
    <div class="content-wrapper">
        <section class="content-header">
            <h1>Add Franchisee</h1>
        </section>
        <style>
            .btn-block {
                display: block;
                width: 20%;
            }
        </style>
        <style type="text/css">
            .content {
                min-height: 100vh;
            }
        </style>
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

            #password-strength-status {
                padding: 5px 10px;
                color: #FFFFFF;
                border-radius: 4px;
                margin-top: 5px;
            }

            #Conpwdpassword-strength-status {
                padding: 5px 10px;
                color: #FFFFFF;
                border-radius: 4px;
                margin-top: 5px;
            }

            .medium-password {
                background-color: #b7d60a;
                border: #BBB418 1px solid;
            }

            .weak-password {
                background-color: #ce1d14;
                border: #AA4502 1px solid;
            }

            .strong-password {
                background-color: #12CC1A;
                border: #0FA015 1px solid;
            }
        </style>
        <section class="content" style="">
            <div class="row">
                <a href="FranchiseeList.aspx" class="btn btn-block btn-success pull-right" width="30%">Back To List</a>
            </div>

            <div id="Tabs" role="tabpanel">
                <br />
                <div class="tab-content table-responsive" style="padding-top: 20px">
                    <div role="tabpanel" class="tab-pane active">
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblSuperFranchisee" runat="server" Text="Super Franchisee"></asp:Label>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:DropDownList ID="ddlSuperFranchisee" runat="server" class="form-control" Width="40%">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnSuperFranchisee" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblMasterFranchisee" runat="server" Text="Master Franchisee"></asp:Label>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:DropDownList ID="ddlMasterFranchisee" runat="server" class="form-control" Width="40%">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnMasterFranchisee" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblFranchiseeName" runat="server" Text="Franchisee Name"></asp:Label><span id="span1" runat="server" style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtFranchiseeName" runat="server" CssClass="form-control" Width="40%" Placeholder="Franchisee Name"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnFranchiseeName" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblContactNumber" runat="server" Text="Contact Number"></asp:Label><span style="color: red">*</span>
                                </div>

                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtContactNumber" runat="server" CssClass="form-control" Width="40%" placeholder="Contact Number"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnContactNumber" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblEmailAddress" runat="server" Text="EmailAddress"></asp:Label><span style="color: red">*</span>
                                </div>

                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtEmailAddress" runat="server" CssClass="form-control" Width="40%" placeholder="EmailAddress"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnEmailAddress" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblCustomerCode" runat="server" Text="CustomerCode"></asp:Label><span style="color: red">*</span>
                                </div>

                                <div class="col-md-3 pad">
                                    <asp:TextBox ID="txtCustomerCode" runat="server" CssClass="form-control" placeholder="CustomerCode"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <asp:Button ID="btnGenerate" runat="server" Text="Generate" CssClass="btn btn-block btn-info" title="Generate" OnClick="BtnGenerate_Click" OnClientClick="return Validate()" />
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnCustomerCode" style="color: #d9534f; display: none;">This field is required</span>
                                    <span id="spnCustomerCodeExists" style="color: #d9534f; display: none;">Code already exists!!</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblShortUrl" runat="server" Text="ShortUrl"></asp:Label><span style="color: red">*</span>
                                </div>

                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtShortUrl" runat="server" CssClass="form-control" Width="40%" placeholder="ShortUrl"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnShortUrl" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblQRCodeURL" runat="server" Text="QRCodeURL"></asp:Label>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtQRCodeURL" runat="server" CssClass="form-control" TextMode="multiline" Rows="5" Width="40%"> </asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblisactive" runat="server" Text="IsActive"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-9 pad">
                                    <input id="chkisactive" name="isActive" type="checkbox" value="valactive" runat="server" />

                                </div>
                            </div>

                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblUserName" runat="server" Text="User Name"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtUserName" runat="server" CssClass="form-control" Width="40%" placeholder="User Name"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnUserName" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblPassword" runat="server" Text="Password"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtPassword" runat="server" CssClass="form-control" Width="40%" placeholder="Password" TextMode="Password" MaxLength="15" onkeyup="checkPasswordStrength()"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnPassword" style="color: #d9534f; display: none;">This field is required</span>
                                    <span class="text-danger" id="spnPasswordCheck" style="display: none;">Passwords must contain: 8 characters, 1 number, 1 special character [i.e. #$*& ] and at least one alphabet letter.
                                    </span>
                                    <div id="password-strength-status"></div>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblConfirmPassword" runat="server" Text="Confirm Password"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtConfirmPassword" runat="server" CssClass="form-control" Width="40%" placeholder="Confirm Password" TextMode="Password" MaxLength="15"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnConfirmPassword" style="color: #d9534f; display: none;">New password and confirm password does not match</span>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblQRCodeImages" runat="server" Text="QRCode"></asp:Label>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:Image ID="QRCodeImage" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                </div>
                                <div class="col-md-3 pad">
                                    <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="btn btn-block btn-info" OnClick="BtnSave_Click" Width="120 px" />
                                    <input type="hidden" id="hdnFranchiseeID" runat="server" />
                                    <input type="hidden" id="hdnContact" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <script>
                $(document).ready(function () {
                    $("#ContentPlaceHolder1_txtCustomerCode").blur(function () {
                        debugger
                        $.ajax({
                            type: "POST",
                            contentType: "application/json;charset=utf-8",
                            url: "AddFranchisee.aspx/CheckCustomerCode",
                            data: "{'codeText':'" + $("#ContentPlaceHolder1_txtCustomerCode").val() + "'}",
                            dataType: "json",
                            success: function (data) {
                                if (data.d) {
                                    $("#spnCustomerCodeExists").css('display', 'none');
                                }
                                else {
                                    $("#spnCustomerCodeExists").css('display', 'block');
                                }
                            },
                            error: function (result) {
                                alert("Error");
                            }
                        });
                    });

                    $("#ContentPlaceHolder1_BtnSave").click(function () {
                        var flag = true;
                        var franchiseeNameval = $("#ContentPlaceHolder1_txtFranchiseeName").val();
                        var contactNumberval = $("#ContentPlaceHolder1_txtContactNumber").val();
                        var emailAddressval = $("#ContentPlaceHolder1_txtEmailAddress").val();
                        var customerCodeval = $("#ContentPlaceHolder1_txtCustomerCode").val();
                        var shortURLval = $("#ContentPlaceHolder1_txtShortUrl").val();
                        var password = $("#ContentPlaceHolder1_txtPassword").val();
                        var cpassword = $("#ContentPlaceHolder1_txtConfirmPassword").val();
                        if (franchiseeNameval == "") {
                            $("#spnFranchiseeName").css('display', 'block');
                            flag = false;
                        }
                        if (contactNumberval == "") {
                            $("#spnContactNumber").css('display', 'block');
                            flag = false;
                        }
                        //if (emailAddressval == "") {
                        //    $("#spnEmailAddress").css('display', 'block');
                        //    flag = false;
                        //}
                        if (customerCodeval == "") {
                            $("#spnCustomerCode").css('display', 'block');
                            flag = false;
                        }
                        if (shortURLval == "") {
                            $("#spnShortUrl").css('display', 'block');
                            flag = false;
                        }
                        if (password != cpassword) {
                            $('#spnConfirmPassword').css('display', 'block');
                            flag = false;
                        }
                        if (flag) {
                            $('#ContentPlaceHolder1_BtnSave').click();
                        }
                        return flag;
                    });

                });

                function checkPasswordStrength() {
                    var number = /([0-9])/;
                    var alphabets = /([a-zA-Z])/;
                    var special_characters = /([~,!,@@,#,$,%,^,&,*,-,_,+,=,?,>,<])/;
                    var password = $("#ContentPlaceHolder1_txtPassword").val();
                    var flag = true;
                    $('#spnPassword').html('');
                    $('#spnPassword').css('display', 'none');
                    debugger
                    if (password == '') {
                        $('#spnPassword').css('display', 'block');
                        $('#spnPassword').html('This field is required');
                        flag = false;
                    }
                    else if (password.length < 8) {
                        $('#password-strength-status').removeClass();
                        $('#password-strength-status').addClass('weak-password');
                        $('#password-strength-status').html("Weak (should be atleast 8 characters.)");
                        flag = false;
                    }
                    else {
                        if ($('#ContentPlaceHolder1_txtPassword').val().match(number) && $('#ContentPlaceHolder1_txtPassword').val().match(alphabets) && $('#ContentPlaceHolder1_txtPassword').val().match(special_characters)) {
                            $('#password-strength-status').removeClass();
                            $('#password-strength-status').addClass('strong-password');
                            $('#password-strength-status').html("Strong");
                        }
                        else {
                            $('#password-strength-status').removeClass();
                            $('#password-strength-status').addClass('medium-password');
                            $('#password-strength-status').html("Medium (should include alphabets, numbers and special characters.)");
                            flag = false;
                        }
                    }
                    return flag
                }
            </script>

        </section>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="Server">
</asp:Content>
