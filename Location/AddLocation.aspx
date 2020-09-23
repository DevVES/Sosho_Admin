<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="AddLocation.aspx.cs" Inherits="Location_AddLocation" %>

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
            <h1>Add Location</h1>
        </section>
        <section class="content" style="">
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
                        <a href="Location.aspx" class="btn btn-block btn-success pull-right" style="width: 50%">Back To List</a>
                    </div>
                </div>
            </div>
            <div class="row pad-bottom">
                <div class="col-md-12">
                    <div class="col-md-3 pad">
                        <asp:Label ID="lblStateName" runat="server" Text="State Name"></asp:Label><span style="color: red">*</span>
                    </div>
                    <div class="col-md-7 pad">
                        <asp:DropDownList ID="ddlStateName" runat="server" Width="290px" class="form-control" AppendDataBoundItems="true">
                            <asp:ListItem Text="Select State" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2 pad">
                        <span id="spnStateName" style="color: #d9534f; display: none;">This field is required</span>
                    </div>
                </div>
            </div>

            <div class="row pad-bottom">
                <div class="col-md-12">
                    <div class="col-md-3 pad">
                        <asp:Label ID="lblCityName" runat="server" Text="City Name"></asp:Label><span style="color: red">*</span>
                    </div>
                    <div class="col-md-7 pad">
                        <asp:DropDownList ID="ddlCityName" runat="server" Width="290px" class="form-control" AppendDataBoundItems="true">
                            <asp:ListItem Text="Select City" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2 pad">
                        <span id="spnCityName" style="color: #d9534f; display: none;">This field is required</span>
                    </div>
                </div>
            </div>

            <div class="row pad-bottom">
                <div class="col-md-12">
                    <div class="col-md-3 pad">
                        <asp:Label ID="lblZipCode" runat="server" Text="ZipCode"></asp:Label><span style="color: red">*</span>
                    </div>
                    <div class="col-md-7 pad">
                        <asp:TextBox ID="txtZipCode" runat="server" CssClass="form-control" Width="40%" placeholder="ZipCode"> </asp:TextBox>
                    </div>
                    <div class="col-md-2 pad">
                        <span id="spnZipCode" style="color: #d9534f; display: none;">This field is required</span>
                    </div>
                </div>
            </div>

            <div class="row pad-bottom">
                <div class="col-md-12">
                    <div class="col-md-3 pad">
                        <asp:Label ID="lblLocationName" runat="server" Text="Location Name"></asp:Label><span style="color: red">*</span>
                    </div>
                    <div class="col-md-7 pad">
                        <asp:TextBox ID="txtLocationName" runat="server" CssClass="form-control" Width="40%" placeholder="Location Name"> </asp:TextBox>
                    </div>
                    <div class="col-md-2 pad">
                        <span id="spnLocationName" style="color: #d9534f; display: none;">This field is required</span>
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
                    </div>
                    <div class="col-md-3 pad">
                        <asp:Button ID="BtnSave" runat="server" Text="Save" CssClass="btn btn-block btn-info" Width="120 px" title="Save" OnClick="BtnSave_Click" />
                    </div>
                </div>
            </div>
        </section>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            
        });
        $('#ContentPlaceHolder1_BtnSave').click(function () {
            var flag = true;
            var StateName = $("#ContentPlaceHolder1_ddlStateName").val();
            var CityName = $("#ContentPlaceHolder1_ddlCityName").val();
            var zipCode = $("#ContentPlaceHolder1_txtZipCode").val();
            var locationName = $("#ContentPlaceHolder1_txtLocationName").val();
            if (StateName == "") {
                $("#spnStateName").css('display', 'block');
                flag = false;
            }
            if (CityName == "") {
                $("#spnCityName").css('display', 'block');
                flag = false;
            }
            if (zipCode == "") {
                $("#spnZipCode").css('display', 'block');
                flag = false;
            }
            if (locationName == "") {
                $("#spnLocationName").css('display', 'block');
                flag = false;
            }
            if (flag) {
                $("#ContentPlaceHolder1_BtnSave").click();
            }
            return flag;
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="Server">
</asp:Content>


