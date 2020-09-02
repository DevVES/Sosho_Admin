<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="UpdateJurisdiction.aspx.cs" Inherits="Jurisdiction_AddJurisdiction" %>

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

        
    </style>
    <div class="content-wrapper">
        <section class="content-header">
            <h1>Update Jurisdiction</h1>
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
                </ul>
                <div class="tab-content table-responsive" style="padding-top: 20px">
                    <div role="tabpanel" class="tab-pane active" id="basic">
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblJurisdictionIncharge" runat="server" Text="Jurisdiction Incharge"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtJurisdictionIncharge" runat="server" CssClass="form-control" Width="40%" placeholder="Jurisdiction Incharge" ReadOnly="true"> </asp:TextBox>
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
                                    <asp:DropDownList class="form-control" runat="server" ID="ddlState" Width="290px" AutoPostBack="true" OnSelectedIndexChanged="OnStateSelectedIndexChanged" Enabled="false"></asp:DropDownList>
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
                                    <asp:DropDownList class="form-control" runat="server" ID="ddlCity" Width="290px" AutoPostBack="true" OnSelectedIndexChanged="OnCitySelectedIndexChanged" Enabled="false"></asp:DropDownList>
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
                                    <input id="chkisactive" name="isActive" type="checkbox" value="valactive" runat="server" disabled="disabled"  />
                                </div>
                            </div>
                        </div>
                       
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                </div>
                                <div class="col-md-9 pad">
                                    <asp:Button ID="BtnUpdate" runat="server" Text="Update" CssClass="btn btn-block btn-info" Width="120 px" title="Update" OnClick="BtnUpdate_Click" />
                                    <input type="hidden" id="hdnJurisdictionID" runat="server" />
                                </div>
                            </div>
                        </div>
                    </div>
                   
                </div>
            </div>

        </section>
    </div>
    <script type="text/javascript">
        $('#ContentPlaceHolder1_BtnUpdate').click(function () {
            var flag = true;
            var JurisdictionIncharge = $("#ContentPlaceHolder1_txtJurisdictionIncharge").val();
            var Contact = $("#ContentPlaceHolder1_txtContact").val();
            var EmailId = $("#ContentPlaceHolder1_txtEmailId").val();
            
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
           
            if (flag) {
                $("#ContentPlaceHolder1_BtnUpdate").click();
            }
            return flag;
        });
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="Server">
</asp:Content>

