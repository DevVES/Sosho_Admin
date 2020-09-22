<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="AddJurisdiction.aspx.cs" Inherits="Jurisdiction_AddJurisdiction" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="../Scripts/jquery-1.12.4.min.js"></script>
    <link rel="stylesheet" href="../../plugins/datepicker/datepicker3.css" />
    <link rel="stylesheet" href="../../plugins/timepicker/bootstrap-timepicker.min.css" />
    <script src="../../plugins/datepicker/bootstrap-datepicker.js"></script>
    <script src="../../plugins/timepicker/bootstrap-timepicker.min.js"></script>
    <style type="text/css">
        .block {
            height: 150px;
            width: 200px;
            border: 1px solid aliceblue;
            overflow-y: scroll;
        }

        .btn-block {
            display: block;
            width: 20%;
        }

        .content {
            min-height: 100vh;
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
    <div class="content-wrapper">
        <section class="content-header">
            <h1>Add Jurisdiction</h1>
        </section>
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="col-md-3">
                        <asp:Label ID="lblmsg" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                    <div class="col-md-3">
                    </div>
                    <div class="col-md-3">
                    </div>
                    <div class="col-md-3">
                        <a href="Jurisdiction.aspx" class="btn btn-block btn-success pull-right" style="width: 50%" target="_blank" title="Back To List">Back To List</a>
                    </div>
                </div>


            </div>
            <div id="Tabs" role="tabpanel">
                <br />
                <ul class="nav nav-tabs" role="tablist">
                    <li class="active">
                        <a href="#basic" aria-controls="basic" role="tab" data-toggle="tab" title="BASIC">BASIC</a>
                    </li>
                   <%-- <li>
                        <a href="#user" aria-controls="basic" role="tab" data-toggle="tab" title="USER">USER</a>
                    </li>--%>
                </ul>
                <div class="tab-content table-responsive" style="padding-top: 20px">
                    <div role="tabpanel" class="tab-pane active" id="basic">
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblJurisdictionIncharge" runat="server" Text="Jurisdiction Incharge"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtJurisdictionIncharge" runat="server" CssClass="form-control" Width="40%" placeholder="Jurisdiction Incharge"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnJurisdictionIncharge" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblContact" runat="server" Text="Contact"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtContact" runat="server" CssClass="form-control" Width="40%" placeholder="Contact"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnContact" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblEmailId" runat="server" Text="Email Id"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtEmailId" runat="server" CssClass="form-control" Width="40%" placeholder="Email Id"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnEmailId" style="color: #d9534f; display: none;">This field is required</span>
                                    <span id="spnInvalidEmail" style="color: #d9534f; display: none;">Invalid Email</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblState" runat="server" Text="State"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:DropDownList class="form-control" runat="server" ID="ddlState" Width="290px" AutoPostBack="true" OnSelectedIndexChanged="OnStateSelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnState" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblCity" runat="server" Text="City"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:DropDownList class="form-control" runat="server" ID="ddlCity" Width="290px" AutoPostBack="true" OnSelectedIndexChanged="OnCitySelectedIndexChanged"></asp:DropDownList>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnCity" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblComments" runat="server" Text="Comments"></asp:Label>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtComments" runat="server" CssClass="form-control" Width="40%" placeholder="Comments"> </asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblIsActive" runat="server" Text="Is Active"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-9 pad">
                                    <input id="chkisactive" name="isActive" type="checkbox" value="valactive" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblPinCode" runat="server" Text="Pin Code"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <div class="block">
                                        <asp:CheckBoxList runat="server" ID="chklstPincode" RepeatLayout="Table">
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnPinCode" style="color: #d9534f; display: none;">Please check at least one checkbox</span>
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
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                </div>
                                <div class="col-md-9 pad">
                                    <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="btn btn-block btn-info" Width="120 px" title="Save" OnClick="BtnSave_Click" />
                                    <input type="hidden" id="hdnJurisdictionID" runat="server" />
                                    <input type="hidden" id="hdnContact" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                    <%--<div class="tab-pane" role="tabpanel" id="user">
                        
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                </div>
                                <div class="col-md-9 pad">
                                    <asp:Button ID="BtnUserSave" runat="server" Text="Save" CssClass="btn btn-block btn-info" Width="120 px" title="Save" OnClick="BtnUserSave_Click" />
                                </div>
                            </div>
                        </div>
                    </div>--%>
                </div>
            </div>

        </section>
    </div>
    <script type="text/javascript">
        $('#ContentPlaceHolder1_BtnSave').click(function () {
            var flag = true;
            var JurisdictionIncharge = $("#ContentPlaceHolder1_txtJurisdictionIncharge").val();
            var Contact = $("#ContentPlaceHolder1_txtContact").val();
            var EmailId = $("#ContentPlaceHolder1_txtEmailId").val();
            var PinCodechecked = $("#ContentPlaceHolder1_chklstPincode input[type='checkbox']:checked").length > 0;
            var UserName = $("#ContentPlaceHolder1_txtUserName").val();
            var Password = $("#ContentPlaceHolder1_txtPassword").val();
            var ConfirmPassword = $("#ContentPlaceHolder1_txtConfirmPassword").val();
            var hdnId = $("#ContentPlaceHolder1_hdnJurisdictionID").val();
            var number = /([0-9])/;
            var alphabets = /([a-zA-Z])/;
            var special_characters = /([~,!,@@,#,$,%,^,&,*,-,_,+,=,?,>,<])/;
            if (UserName == "") {
                $("#spnUserName").css('display', 'block');
                flag = false;
            }
            if (Password == "") {
                $("#spnPassword").css('display', 'block');
                flag = false;
            }
            else {
                if (Password.length < 8) {
                    $('#spnPassword').css('display', 'block');
                    $('#spnPassword').html('Password must contain contain at least eight characters');
                    flag = false;
                }
                if ($('#ContentPlaceHolder1_txtPassword').val().match(number) && $('#ContentPlaceHolder1_txtPassword').val().match(alphabets) && $('#ContentPlaceHolder1_txtPassword').val().match(special_characters)) {
                }
                else {
                    $('#password-strength-status').removeClass();
                    $('#password-strength-status').addClass('medium-password');
                    $('#password-strength-status').html("Medium (should include alphabets, numbers and special characters.)");
                    flag = false;
                }
            }
            if (ConfirmPassword == "") {
                $("#spnConfirmPassword").css('display', 'block');
                flag = false;
            }
            if (Password != ConfirmPassword) {
                $("#spnConfirmPassword").css('display', 'block');
                flag = false;
            }
            //if (hdnId == undefined || hdnId == "0" || hdnId == "") {
            //    $("#ContentPlaceHolder1_lblmsg").html('Please Add Jurisdiction');
            //    flag = false;
            //}
            if (JurisdictionIncharge == "") {
                $("#spnJurisdictionIncharge").css('display', 'block');
                flag = false;
            }
            if (Contact == "") {
                $("#spnContact").css('display', 'block');
                flag = false;
            }
            if (EmailId == "") {
                $("#spnEmailId").css('display', 'block');
                flag = false;
            }
            else {
                if ($("#ContentPlaceHolder1_txtEmailId").val().length != 0) {
                    var emailAddress = $("#ContentPlaceHolder1_txtEmailId").val();
                    var Emailvalue = '^[a-zA-Z0-9_\\.-]+@([a-zA-Z0-9-]+\\.)+[a-zA-Z]{2,6}$';
                    var pattern = new RegExp(Emailvalue);
                    var EmailValid = pattern.test(emailAddress);
                    if (!EmailValid) {
                        $('#spnInvalidEmail').css('display', 'block');
                        flag = false;
                    }
                }
            }
            if (!PinCodechecked) {
                $("#spnPinCode").css('display', 'block');
                flag = false;
            }
            if (flag) {
                $("#ContentPlaceHolder1_BtnSave").click();
            }
            return flag;
        });
        //$('#ContentPlaceHolder1_BtnUserSave').click(function () {
        //    var flag = true;
        //    var UserName = $("#ContentPlaceHolder1_txtUserName").val();
        //    var Password = $("#ContentPlaceHolder1_txtPassword").val();
        //    var ConfirmPassword = $("#ContentPlaceHolder1_txtConfirmPassword").val();
        //    var hdnId = $("#ContentPlaceHolder1_hdnJurisdictionID").val();
        //    var number = /([0-9])/;
        //    var alphabets = /([a-zA-Z])/;
        //    var special_characters = /([~,!,@@,#,$,%,^,&,*,-,_,+,=,?,>,<])/;
        //    if (UserName == "") {
        //        $("#spnUserName").css('display', 'block');
        //        flag = false;
        //    }
        //    if (Password == "") {
        //        $("#spnPassword").css('display', 'block');
        //        flag = false;
        //    }
        //    else {
        //        if (Password.length < 8) {
        //            $('#spnPassword').css('display', 'block');
        //            $('#spnPassword').html('Password must contain contain at least eight characters');
        //            flag = false;
        //        }
        //        if ($('#ContentPlaceHolder1_txtPassword').val().match(number) && $('#ContentPlaceHolder1_txtPassword').val().match(alphabets) && $('#ContentPlaceHolder1_txtPassword').val().match(special_characters)) {
        //        }
        //        else {
        //            $('#password-strength-status').removeClass();
        //            $('#password-strength-status').addClass('medium-password');
        //            $('#password-strength-status').html("Medium (should include alphabets, numbers and special characters.)");
        //            flag = false;
        //        }
        //    }
        //    if (ConfirmPassword == "") {
        //        $("#spnConfirmPassword").css('display', 'block');
        //        flag = false;
        //    }
        //    if (Password != ConfirmPassword) {
        //        $("#spnConfirmPassword").css('display', 'block');
        //        flag = false;
        //    }
        //    if (hdnId == undefined || hdnId == "0" || hdnId == "") {
        //        $("#ContentPlaceHolder1_lblmsg").html('Please Add Jurisdiction');
        //        flag = false;
        //    }
        //    if (flag) {

        //    }
        //    return flag;
        //});
        function checkPasswordStrength() {
            var number = /([0-9])/;
            var alphabets = /([a-zA-Z])/;
            var special_characters = /([~,!,@@,#,$,%,^,&,*,-,_,+,=,?,>,<])/;
            var password = $("#ContentPlaceHolder1_txtPassword").val();
            var flag = true;
            $('#spnPassword').html('');
            $('#spnPassword').css('display', 'none');
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="Server">
</asp:Content>

