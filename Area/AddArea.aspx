<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="AddArea.aspx.cs" Inherits="Area_AddArea" %>

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
            <h1>Add Area</h1>
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
                        <a href="Area.aspx" class="btn btn-block btn-success pull-right" style="width: 50%" target="_blank">Back To List</a>
                    </div>
                </div>
            </div>
            <div class="row pad-bottom">
                <div class="col-md-12">
                    <div class="col-md-3 pad">
                        <asp:Label ID="lblZipCode" runat="server" Text="ZipCode"></asp:Label><span style="color: red">*</span>
                    </div>
                    <div class="col-md-7 pad">
                        <asp:DropDownList ID="ddlZipCode" runat="server" Width="290px" class="form-control" AppendDataBoundItems="true" OnSelectedIndexChanged = "OnSelectedIndexChanged" AutoPostBack = "true">
                            <asp:ListItem Text="Select ZipCode" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2 pad">
                        <span id="spnZipCode" style="color: #d9534f; display: none;">This field is required</span>
                    </div>
                </div>
            </div>

            <div class="row pad-bottom">
                <div class="col-md-12">
                    <div class="col-md-3 pad">
                        <asp:Label ID="lblLocation" runat="server" Text="Location"></asp:Label><span style="color: red">*</span>
                    </div>
                    <div class="col-md-7 pad">
                        <asp:DropDownList ID="ddlLocation" runat="server" Width="290px" class="form-control" AppendDataBoundItems="true">
                            <asp:ListItem Text="Select Location" Value=""></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-2 pad">
                        <span id="spnLocation" style="color: #d9534f; display: none;">This field is required</span>
                    </div>
                </div>
            </div>

            

            <div class="row pad-bottom">
                <div class="col-md-12">
                    <div class="col-md-3 pad">
                        <asp:Label ID="lblArea" runat="server" Text="Area"></asp:Label><span style="color: red">*</span>
                    </div>
                    <div class="col-md-7 pad">
                        <asp:TextBox ID="txtArea" runat="server" CssClass="form-control" Width="40%" placeholder="Area Name"> </asp:TextBox>
                    </div>
                    <div class="col-md-2 pad">
                        <span id="spnArea" style="color: #d9534f; display: none;">This field is required</span>
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
            var zipcode = $("#ContentPlaceHolder1_ddlZipCode").val();
            var location = $("#ContentPlaceHolder1_ddlLocation").val();
            var area = $("#ContentPlaceHolder1_txtArea").val();
            if (zipcode == "") {
                $("#spnZipCode").css('display', 'block');
                flag = false;
            }
            if (location == "") {
                $("#spnLocation").css('display', 'block');
                flag = false;
            }
            if (area == "") {
                $("#spnArea").css('display', 'block');
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

