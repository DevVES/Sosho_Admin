<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="UploadHomepageBanner.aspx.cs" Inherits="Banner_UploadHomepageBanner" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <link rel="stylesheet" href="../../plugins/datepicker/datepicker3.css" />
    <link rel="stylesheet" href="../../plugins/timepicker/bootstrap-timepicker.min.css" />
    <script src="../../plugins/datepicker/bootstrap-datepicker.js"></script>
    <script src="../../plugins/timepicker/bootstrap-timepicker.min.js"></script>

    <div class="content-wrapper">
        <section class="content-header">
            <h1>Upload HomePage Banner</h1>
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

        <section class="content" style="">
            <div class="row">
                <a href="HomePageBannerList.aspx" class="btn btn-block btn-success pull-right" width="30%" target="_blank">Back To List</a>
            </div>

            <div id="Tabs" role="tabpanel">
                <br />
                <ul class="nav nav-tabs" role="tablist">
                    <li class="active">
                        <a href="#basic" aria-controls="basic" role="tab" data-toggle="tab" title="BASIC">BASIC</a>
                    </li>
                    <li>
                        <a href="#intermediate" aria-controls="intermediate" role="tab" data-toggle="tab" title="INTERMEDIATE BANNERS">INTERMEDIATE BANNERS</a>
                    </li>
                </ul>
                <div class="tab-content table-responsive" style="padding-top: 20px">
                    <div role="tabpanel" class="tab-pane active" id="basic">
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblTitle" runat="server" Text="Title"></asp:Label><span style="color: red">*</span>
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
                                    <asp:Label ID="lblLink" runat="server" Text="Link"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-9 pad">
                                    <asp:TextBox ID="txtLink" runat="server" CssClass="form-control" Width="40%"> </asp:TextBox>
                                    <asp:RegularExpressionValidator ID="regUrl" runat="server" ControlToValidate="txtLink" ValidationExpression="(([\w]+:)?\/\/)?(([\d\w]|%[a-fA-f\d]{2,2})+(:([\d\w]|%[a-fA-f\d]{2,2})+)?@)?([\d\w][-\d\w]{0,253}[\d\w]\.)+[\w]{2,4}(:[\d]+)?(\/([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)*(\?(&?([-+_~.\d\w]|%[a-fA-f\d]{2,2})=?)*)?(#([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)?$" Text="Enter a valid URL" />
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

                    <div role="tabpanel" class="tab-pane" id="intermediate">
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="Label2" runat="server" Text="Type"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:DropDownList ID="ddlintermediateType" runat="server" class="form-control" Width="40%">
                                        <%--<asp:ListItem Value="">Select Banner Type</asp:ListItem>
                                        <asp:ListItem Value="1">Top Banner</asp:ListItem>
                                        <asp:ListItem Value="2">Second Banner</asp:ListItem>--%>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnintermediateType" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblintermediateTitle" runat="server" Text="Title"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtintermediateTitle" runat="server" CssClass="form-control" Width="40%" onkeypress="return validatenumerics(event)" placeholder="Title"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnintermediateTitle" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblintermediateAction" runat="server" Text="Action"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:DropDownList ID="ddlintermedicateAction" runat="server" class="form-control" Width="40%">
                                        <asp:ListItem Value="">Select Action Type</asp:ListItem>
                                        <asp:ListItem Value="Add To Cart">Add To Cart</asp:ListItem>
                                        <asp:ListItem Value="Navigate To Category">Navigate To Category</asp:ListItem>
                                        <asp:ListItem Value="Open Url">Open Url</asp:ListItem>
                                        <asp:ListItem Value="None">None</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnintermediateAction" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                 <div class="col-md-3 pad">
                                <asp:Label ID="lblintermediateCategory" runat="server" Text="Category"></asp:Label>
                                     </div>
                            
                            <div class="col-md-7 pad">
                                <asp:DropDownList ID="ddlintermedicateCategory" runat="server" class="form-control" Width="40%" AppendDataBoundItems="true">
                                    <asp:ListItem Text="Select Category Name" Value=""></asp:ListItem>
                                </asp:DropDownList>
                            </div>
                            <div class="col-md-2 pad">
                                <span id="spnintermediateCategory" style="color: #d9534f; display: none;">This field is required</span>
                            </div>
                                </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblintermediateAltText" runat="server" Text="AltText"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtintermediateAltText" runat="server" CssClass="form-control" Width="40%" Placeholder="AltText"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnintermediateAltText" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblintermediateLink" runat="server" Text="Link"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtintermediateLink" runat="server" CssClass="form-control" Width="40%" Placeholder="Link"> </asp:TextBox>
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ControlToValidate="txtLink" ValidationExpression="(([\w]+:)?\/\/)?(([\d\w]|%[a-fA-f\d]{2,2})+(:([\d\w]|%[a-fA-f\d]{2,2})+)?@)?([\d\w][-\d\w]{0,253}[\d\w]\.)+[\w]{2,4}(:[\d]+)?(\/([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)*(\?(&?([-+_~.\d\w]|%[a-fA-f\d]{2,2})=?)*)?(#([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)?$" Text="Enter a valid URL" />
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnintermediateLink" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblintermediateStartDate" runat="server" Text="StartDate"></asp:Label><span style="color: red">*</span>

                                </div>
                                <div class="col-md-9 pad" style="padding: 0">
                                    <div class="col-md-3 pad">
                                        <asp:TextBox ID="txtintermediateStartDate" CssClass="form-control" placeholder="Select Date" runat="server"></asp:TextBox>
                                        <script>
                                            $('#ContentPlaceHolder1_txtintermediateStartDate').datepicker({
                                                format: 'dd/M/yyyy',
                                                autoclose: true
                                            });
                                        </script>

                                    </div>
                                    <div class="col-md-3 pad">
                                        <div class="bootstrap-timepicker">

                                            <div class="input-group">

                                                <asp:TextBox runat="server" type="text" ID="txtintermediateStartDatetimepicker" class="form-control timepicker"></asp:TextBox>
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
                                    <asp:Label ID="Label6" runat="server" Text="EnDate"></asp:Label><span style="color: red">*</span>

                                </div>
                                <div class="col-md-9 pad" style="padding: 0">
                                    <div class="col-md-3 pad">
                                        <asp:TextBox ID="txtintermediateEndDate" CssClass="form-control" placeholder="Select Date" runat="server"></asp:TextBox>
                                        <script>
                                            $('#ContentPlaceHolder1_txtintermediateEndDate').datepicker({
                                                format: 'dd/M/yyyy',
                                                autoclose: true
                                            });
                                        </script>

                                    </div>
                                    <div class="col-md-3 pad">
                                        <div class="bootstrap-timepicker">
                                            <div class="input-group">
                                                <asp:TextBox runat="server" type="text" ID="txtintermediateEndDatetimepicker" class="form-control timepicker"></asp:TextBox>
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
                                    <asp:Label ID="lblintermediate" runat="server" Text="IsActive"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <input id="ChkintermediateisActive" name="isActive" type="checkbox" value="valactive" runat="server" />

                                </div>
                            </div>

                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="Label8" runat="server" Text="Banner"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-9 pad">
                                    <asp:FileUpload ID="FileUpload2" onchange="intermediatepreviewFile()" OnDataBinding="FileUpload2_DataBinding" OnLoad="FileUpload2_Load" OnInit="FileUpload2Control_Init" runat="server" />
                                    <asp:Image ID="intermediateimage" Width="202px" Height="90px" runat="server" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server"
                                        ControlToValidate="FileUpload2" ErrorMessage="Only .jpg,.png,.jpeg,.gif Files are allowed" Font-Bold="True"
                                        Font-Size="Medium" ValidationExpression="(.*?)\.(jpg|jpeg|png|JPG|JPEG|PNG)$"></asp:RegularExpressionValidator>

                                    <asp:Button ID="BtnintermediateRemoveImage" runat="server" Text="Remove Image" OnClick="BtnintermediateRemoveImage_Click" CssClass="btn btn-block btn-danger" Width="120 px" />

                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                </div>
                                <div class="col-md-3 pad">
                                    <asp:Button ID="BtnintermediateSave" runat="server" Text="Save" OnClick="BtnintermediateSave_Click" CssClass="btn btn-block btn-info" Width="120 px" OnClientClick="return Validate()" Title="Save" />
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
                function intermediatepreviewFile() {
                    var preview1 = document.querySelector('#<%=intermediateimage.ClientID %>');
                    var file1 = document.querySelector('#<%=FileUpload2.ClientID %>').files[0];
                    var reader1 = new FileReader();

                    reader1.onloadend = function () {
                        preview1.src = reader1.result;
                    }

                    if (file1) {
                        reader1.readAsDataURL(file1);
                    } else {
                        preview1.src = "";
                    }
                }

                $('#ContentPlaceHolder1_BtnintermediateSave').click(function () {
                    var flag = true;
                    var typeval = $("#ContentPlaceHolder1_ddlintermediateType").val();
                    var titleval = $("#ContentPlaceHolder1_txtintermediateTitle").val();
                    var Alttextval = $("#ContentPlaceHolder1_txtintermediateAltText").val();
                    var linkval = $("#ContentPlaceHolder1_txtintermediateAltText").val();
                    var Actionval = $("#ContentPlaceHolder1_ddlintermedicateAction").val();
                    val Categoryval = $("#ContentPlaceHolder1_ddlintermedicateCategory").val();
                    if (typeval == "") {
                        $("#spnintermediateType").css('display', 'block');
                        flag = false;
                    }
                    if (titleval == "") {
                        $("#spnintermediateTitle").css('display', 'block');
                        flag = false;
                    }
                    if (Alttextval == "") {
                        $("#spnintermediateAltText").css('display', 'block');
                        flag = false;
                    }
                    if (linkval == "") {
                        $("#spnintermediateLink").css('display', 'block');
                        flag = false;
                    }
                    if (Actionval == "") {
                        $("#spnintermediateAction").css('display', 'block');
                        flag = false;
                    }
                    else {
                        if (Actioval == "Navigate To Category") {
                            if (Categoryval == "") {
                                $("#spnintermediateCategory").css('display', 'block');
                                flag = false;
                            }
                        }
                    }
                    if (flag) {
                        $('#ContentPlaceHolder1_BtnintermediateSave').click();
                    }
                    return flag;
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
