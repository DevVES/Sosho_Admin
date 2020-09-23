<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="AddCategory.aspx.cs" Inherits="Category_AddCategory" %>

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
            <h1>Add Category</h1>
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
                        <a href="Category.aspx" class="btn btn-block btn-success pull-right" style="width: 50%">Back To List</a>
                    </div>
                </div>
            </div>
            <div class="row pad-bottom">
                <div class="col-md-12">
                    <div class="col-md-3 pad">
                        <asp:Label ID="lblCategoryName" runat="server" Text="Name"></asp:Label><span style="color: red">*</span>
                    </div>
                    <div class="col-md-7 pad">
                        <asp:TextBox ID="txtCategoryName" runat="server" CssClass="form-control" Width="40%" placeholder="Category Name"> </asp:TextBox>
                    </div>
                    <div class="col-md-2 pad">
                        <span id="spnCategoryName" style="color: #d9534f; display: none;">This field is required</span>
                    </div>
                </div>
            </div>
            <div class="row pad-bottom">
                <div class="col-md-12">
                    <div class="col-md-3 pad">
                        <asp:Label ID="lblDescription" runat="server" Text="Description"></asp:Label><span style="color: red">*</span>
                    </div>
                    <div class="col-md-7 pad">
                        <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" Width="40%" placeholder="Category Description"> </asp:TextBox>
                    </div>
                    <div class="col-md-2 pad">
                        <span id="spnDescription" style="color: #d9534f; display: none;">This field is required</span>
                    </div>
                </div>
            </div>
            <div class="row pad-bottom">
                <div class="col-md-12">
                    <div class="col-md-3 pad">
                        <asp:Label ID="lblImage" runat="server" Text="Image"></asp:Label><span style="color: red">*</span>
                    </div>
                    <div class="col-md-9 pad">
                        <asp:FileUpload ID="FileUpload1" runat="server" onchange="previewFile()" OnDataBinding="FileUpload1_DataBinding" OnLoad="FileUpload1_Load" OnInit="FileUploadControl_Init" />
                        <asp:Image ID="CategoryImage" Width="202px" Height="90px" runat="server" />
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                            ControlToValidate="FileUpload1" ErrorMessage="Only .jpg,.png,.jpeg,.gif Files are allowed" Font-Bold="True"
                            Font-Size="Medium" ValidationExpression="(.*?)\.(jpg|jpeg|png|JPG|JPEG|PNG)$"></asp:RegularExpressionValidator>

                        <asp:Button ID="BtnRemoveImage" runat="server" Text="Remove Image" CssClass="btn btn-block btn-danger" Width="120 px" title="Remove Image" OnClick="BtnRemoveImage_Click" />
                    </div>
                </div>
            </div>

            <div class="row pad-bottom">
                <div class="col-md-12">
                    <div class="col-md-3 pad">
                        <asp:Label ID="lblSequence" runat="server" Text="Sequence"></asp:Label><span style="color: red">*</span>
                    </div>
                    <div class="col-md-7 pad">
                        <asp:TextBox ID="txtSequence" runat="server" TextMode="Number" CssClass="form-control" Width="40%" onkeypress="return isNumber(event)" placeholder="Sequence"> </asp:TextBox>
                    </div>
                    <div class="col-md-2 pad">
                        <span id="spnSequence" style="color: #d9534f; display: none;">This field is required</span>
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
            var hiddenvalue = $("#hdnmainIsAdmin").val();
            if (hiddenvalue == "False") {
                $("#ContentPlaceHolder1_FileUpload1").attr('disabled', true);
                $("#ContentPlaceHolder1_BtnRemoveImage").attr('disabled', true);
                $("#ContentPlaceHolder1_BtnSave").attr('disabled', true);
            }
        });
        $('#ContentPlaceHolder1_BtnSave').click(function () {
            var flag = true;
            var CategoryName = $("#ContentPlaceHolder1_txtCategoryName").val();
            var Description = $("#ContentPlaceHolder1_txtDescription").val();
            if (CategoryName == "") {
                $("#spnCategoryName").css('display', 'block');
                flag = false;
            }
            if (Description == "") {
                $("#spnDescription").css('display', 'block');
                flag = false;
            }
            if (flag) {
                $("#ContentPlaceHolder1_BtnSave").click();
            }
            return flag;
        });
        function previewFile() {
            var preview = document.querySelector('#<%=CategoryImage.ClientID %>');
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="Server">
</asp:Content>


