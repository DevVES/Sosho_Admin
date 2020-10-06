<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="UploadBanner.aspx.cs" Inherits="Banner_UploadBanner" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <link rel="stylesheet" href="../../plugins/datepicker/datepicker3.css" />
    <link rel="stylesheet" href="../../plugins/timepicker/bootstrap-timepicker.min.css" />
    <script src="../../plugins/datepicker/bootstrap-datepicker.js"></script>
    <script src="../../plugins/timepicker/bootstrap-timepicker.min.js"></script>

    <div class="content-wrapper">
        <section class="content-header">
            <h1>Upload Banner</h1>
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
    </style>
        <section class="content" style="">
            <div class="row">
                <a href="HomePageBannerList.aspx" class="btn btn-block btn-success pull-right" width="30%">Back To List</a>
            </div>

            <div id="Tabs" role="tabpanel">
                <br />
                <%--<ul class="nav nav-tabs" role="tablist">
                    <li class="active">
                        <a href="#basic" aria-controls="basic" role="tab" data-toggle="tab" title="BASIC">BASIC</a>
                    </li>
                    <li>
                        <a href="#intermediate" aria-controls="intermediate" role="tab" data-toggle="tab" title="INTERMEDIATE BANNERS">INTERMEDIATE BANNERS</a>
                    </li>
                </ul>--%>
                <div class="tab-content table-responsive" style="padding-top: 20px">
                    <div role="tabpanel" class="tab-pane active">
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblBannerType" runat="server" Text="Type"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:DropDownList ID="ddlBannerType" runat="server" class="form-control" Width="40%">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnBannerType" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>
                          
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblTitle1" runat="server" Text="Title"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-9 pad">
                                    <asp:TextBox ID="txtTitle1" runat="server" CssClass="form-control" Width="40%" onkeypress="return validatenumerics(event)"> </asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblAltText" runat="server" Text="AltText"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-9 pad">
                                    <asp:TextBox ID="txtAltText" runat="server" CssClass="form-control" Width="40%"> </asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblbasicAction" runat="server" Text="Action"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:DropDownList ID="ddlbasicAction" runat="server" class="form-control" Width="40%" AutoPostBack="true" OnSelectedIndexChanged="OnSelectedBasicActionChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnbasicAction" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom" id="dvbasicLink" runat="server">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblbasicLink" runat="server" Text="Link" Visible="false"></asp:Label><span id="span1" runat="server" style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtbasicLink" runat="server" CssClass="form-control" Width="40%" Placeholder="Link" Visible="false"> </asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server" ControlToValidate="txtbasicLink" ValidationExpression="(([\w]+:)?\/\/)?(([\d\w]|%[a-fA-f\d]{2,2})+(:([\d\w]|%[a-fA-f\d]{2,2})+)?@)?([\d\w][-\d\w]{0,253}[\d\w]\.)+[\w]{2,4}(:[\d]+)?(\/([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)*(\?(&?([-+_~.\d\w]|%[a-fA-f\d]{2,2})=?)*)?(#([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)?$" Text="Enter a valid URL" />
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnbasicLink" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom" id="dvbasicCategory" runat="server">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblbasicCategory" runat="server" Text="Action Category" Visible="false"></asp:Label><span style="color: red">*</span>
                                </div>

                                <div class="col-md-7 pad">
                                    <asp:DropDownList ID="ddlbasicCategory" runat="server" class="form-control" Width="40%" AppendDataBoundItems="true" Visible="false">
                                        <asp:ListItem Text="Select Category Name" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnbasicCategory" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom" id="dvbasicProduct" runat="server">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblbasicProduct" runat="server" Text="Product" Visible="false"></asp:Label><span style="color: red">*</span>
                                </div>

                                <div class="col-md-7 pad">
                                    <asp:DropDownList ID="ddlbasicProduct" runat="server" class="form-control" Width="40%" AppendDataBoundItems="true" Visible="false">
                                        <asp:ListItem Text="Select Product Name" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnbasicProduct" style="color: #d9534f; display: none;">This field is required</span>
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
                                    <asp:Label ID="Label1" runat="server" Text="EnDate"></asp:Label><span style="color: red">*</span>

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
                                    <asp:Label ID="lblattachment" runat="server" Text="Banner"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-9 pad">
                                    <asp:FileUpload ID="FileUpload1" onchange="previewFile()" OnDataBinding="FileUpload1_DataBinding" OnLoad="FileUpload1_Load" OnInit="FileUploadControl_Init" runat="server" />
                                    <asp:Image ID="productimg" Width="202px" Height="90px" runat="server" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                        ControlToValidate="FileUpload1" ErrorMessage="Only .jpg,.png,.jpeg,.gif Files are allowed" Font-Bold="True"
                                        Font-Size="Medium" ValidationExpression="(.*?)\.(jpg|jpeg|png|JPG|JPEG|PNG)$"></asp:RegularExpressionValidator>

                                    <asp:Button ID="Button1" runat="server" Text="Remove Image" OnClick="Button1_Click" CssClass="btn btn-block btn-danger" Width="120 px" />
                                    <%--<asp:Image ID="img" Width="202px" Height="90px" runat="server" />--%>
                                    <%--<asp:FileUpload ID="fileupload12"  OnDataBinding="fileupload12_DataBinding"  OnInit="FileUploadControl_Init" runat="server" accept=".png,.jpg,.jpeg" />--%>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom" id="divCategorySelection" runat="server">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblCategory" runat="server" Text="Category"></asp:Label>
                                </div>
                                <div class="col-md-7 pad">
                                    <div class="block">
                                        <asp:CheckBoxList runat="server" ID="chklstCategory" RepeatLayout="Table">
                                        </asp:CheckBoxList>
                                    </div>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnCategory" style="color: #d9534f; display: none;">Please check at least one checkbox</span>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom" id="divBasicJurisdictionIncharge" runat="server">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblBasicJurisdictionIncharge" runat="server" Text="Jurisdiction Incharge"></asp:Label>
                                </div>
                                <div class="col-md-7 pad">
                                    <div class="block">
                                        <asp:CheckBoxList runat="server" ID="chklstBasicJurisdictionIncharge" RepeatLayout="Table">
                                        </asp:CheckBoxList>
                                        <%----<input id="Hidden1" runat="server" type="hidden" />--%>
                                    </div>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnBasicJurisdictionIncharge" style="color: #d9534f; display: none;">Please check at least one checkbox</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                </div>
                                <div class="col-md-3 pad">
                                    <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" CssClass="btn btn-block btn-info" Width="120 px" OnClientClick="return Validate()" />
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
            <script type="text/javascript">

                                            function previewFile() {
                                                var preview = document.querySelector('#<%=productimg.ClientID %>');
                                                var file = document.querySelector('#<%=FileUpload1.ClientID %>').files[0];
                                                var reader = new FileReader();

                                                reader.onloadend = function () {
                                                    preview.src = reader.result;
                                                }

                                                if (file) {
                                                    reader.readAsDataURL(file);
                                                } else {
                                                    preview.src = "";
                                                }
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


        </section>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="Server">
</asp:Content>
