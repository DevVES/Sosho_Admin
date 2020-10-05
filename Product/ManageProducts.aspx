<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ManageProducts.aspx.cs" Inherits="Product_ManageProducts" ValidateRequest="false" %>

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
            <h1>Manage Products</h1>
        </section>

        <style type="text/css">
            .btn-block {
                display: block;
                width: 20%;
            }
        </style>
        <%--  <script src="JS/jquery-1.10.2.min.js" type="text/javascript"></script>
        <script src="JS/jquery-te-1.4.0.min.js" type="text/javascript"></script>--%>
        <%--<script type="text/javascript">
    $('.textEditor1').jqte();
    $(".textEditor").jqte({
        blur: function () {
            document.getElementById('<%=hdText.ClientID %>').value = document.getElementById('<%=txtEditor.ClientID %>').value;
    }
    });
</script>--%>
        <section class="content" style="">
            <div class="row">
                <div class=" col-md-12">
                    <div class="col-md-3">
                        <asp:Button ID="BtnSave" runat="server" Text="Save" OnClick="BtnSave_Click" CssClass="btn btn-block btn-info" Width="120 px" OnClientClick="return Validate()" title="Save" />
                        <asp:Label ID="lblmsg" runat="server" ForeColor="Red"></asp:Label>
                    </div>
                    <div class="col-md-3">
                    </div>
                    <div class="col-md-3">
                    </div>
                    <div class="col-md-3">
                        <a href="ProductList.aspx" class="btn btn-block btn-success pull-right" style="width: 50%" title="Back to List">Back To List</a>
                    </div>
                </div>

            </div>
            <%--<div class="row pad-bottom">
                <div class="col-md-12">
                    <div class="col-md-3 pad">
                    </div>
                    <div class="col-md-3 pad">
                       
                    </div>
                </div>
            </div>--%>
            <div id="Tabs" role="tabpanel">
                <br />
                <ul class="nav nav-tabs" role="tablist">
                    <li class="active">
                        <a href="#basic" aria-controls="basic" role="tab" data-toggle="tab" title="BASIC">BASIC</a></li>
                    <li id="ligrpProduct">
                        <a href="#grpproduct" aria-controls="grpproduct" role="tab" data-toggle="tab" title="GRPPRODUCT">PRICE</a></li>
                    <li>
                        <a href="#sco" aria-controls="sco" role="tab" data-toggle="tab" title="SCO">SCO</a></li>
                    <li>
                        <a href="#price" aria-controls="price" role="tab" data-toggle="tab" title="PRICE">PRICE</a></li>

                    <li>
                        <a href="#notes" aria-controls="notes" role="tab" data-toggle="tab" title="NOTES">NOTES</a></li>
                    <li>
                        <a href="#images" aria-controls="images" role="tab" data-toggle="tab" title="IMAGES">IMAGES</a></li>
                </ul>

                <div class="tab-content table-responsive" style="padding-top: 20px">

                    <div role="tabpanel" class="tab-pane active" id="basic">
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblpname" runat="server" Text="Product Name"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtpname" runat="server" CssClass="form-control" Width="40%" placeholder="Product Name"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnpname" style="color: #d9534f; display: none;">This field is required</span>
                                    <input type="hidden" id="hdnIsAdmin" runat="server" />
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblCategoryName" runat="server" Text="Category Name"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:DropDownList ID="ddlCategoryName" runat="server" Width="290px" class="form-control" AppendDataBoundItems="true" OnSelectedIndexChanged="OnSelectedCategoryChanged"  AutoPostBack = "true">
                                        <asp:ListItem Text="Select Category Name" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnCategoryName" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>

                        </div>
                         <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblSubCategoryName" runat="server" Text="SubCategory Name"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:DropDownList ID="ddlSubCategoryName" runat="server" Width="290px" class="form-control" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select SubCategory Name" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnSubCategoryName" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblProductType" runat="server" Text="Product Type"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:DropDownList class="form-control" runat="server" ID="ddlProductType" Width="290px" AppendDataBoundItems="true">
                                        <asp:ListItem Text="Select Product Type" Value=""></asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnProductType" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblIsProductDescription" runat="server" Text="Show Product Description"></asp:Label>
                                </div>
                                <div class="col-md-7 pad">
                                    <input id="chkIsProductDescription" name="IsProductDescription" type="checkbox" value="valIsProductDescription" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom" id="divFullDescription">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblpdisc" runat="server" Text="Product Description"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-9 pad">
                                    <CKEditor:CKEditorControl class="form-control" ID="txtFullDescription" runat="server">
                                    </CKEditor:CKEditorControl>
                                    <span id="spnFullDescription" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>




                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 ">
                                    <asp:Label ID="lblvdo" runat="server" Text="Video Link"></asp:Label>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtvdo" runat="server" CssClass="form-control" Width="40%" placeholder="Video Link"> </asp:TextBox>
                                    <%--<asp:RegularExpressionValidator ID="regUrl" runat="server" ControlToValidate="txtvdo" ValidationExpression="(([\w]+:)?\/\/)?(([\d\w]|%[a-fA-f\d]{2,2})+(:([\d\w]|%[a-fA-f\d]{2,2})+)?@)?([\d\w][-\d\w]{0,253}[\d\w]\.)+[\w]{2,4}(:[\d]+)?(\/([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)*(\?(&?([-+_~.\d\w]|%[a-fA-f\d]{2,2})=?)*)?(#([-+_~.\d\w]|%[a-fA-f\d]{2,2})*)?$" Text="Enter a valid URL" />--%>
                                    <%--<asp:Image ID="img" Width="202px" Height="90px" runat="server" />--%>
                                    <%--<asp:FileUpload ID="fileupload12"  OnDataBinding="fileupload12_DataBinding"  OnInit="FileUploadControl_Init" runat="server" accept=".png,.jpg,.jpeg" />--%>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnvdo" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 ">
                                    <asp:Label ID="lblsoldtime" runat="server" Text="Sold Time"></asp:Label><span style="color: red">*</span>
                                </div>

                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtNoOfSoldItems" runat="server" onkeypress="return isNumber(event)" CssClass="form-control" Width="40%" placeholder="Sold Time">25</asp:TextBox>

                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnsoldtime" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>


                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblgst" runat="server" Text="GST"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad" style="display: flex">
                                    <asp:DropDownList runat="server" Width="40%" ID="ddlgst" CssClass="form-control"></asp:DropDownList>
                                    <a class="btn btn-success" data-toggle="modal" data-target="#modal-default"><i class="fa fa-fw fa-plus-circle"></i></a>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spngst" style="color: #d9534f; display: none;">This field is required</span>
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
                                    <asp:Label ID="lblDisplayOrder" runat="server" Text="Display Order"></asp:Label><span style="color: red">*</span>
                                </div>

                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtDisplayOrder" runat="server" onkeypress="return isNumber(event)" CssClass="form-control" Width="40%" placeholder="Display Order"></asp:TextBox>

                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnDisplayOrder" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblProductBanner" runat="server" Text="Special Message"></asp:Label>
                                </div>

                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtProductBanner" runat="server" CssClass="form-control" Width="40%" placeholder="Special Message"></asp:TextBox>

                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblRecommended" runat="server" Text="Recommended"></asp:Label>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtRecommended" runat="server" CssClass="form-control" Width="40%" placeholder="Recommended"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblIsFreeShipping" runat="server" Text="IsFreeShipping"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <input id="chkIsFreeShipping" name="isActive" type="checkbox" value="valactive" runat="server" />

                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblIsFixedShipping" runat="server" Text="IsFixedShipping"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <%--<input id="chkIsFixedShipping" name="isActive" type="checkbox" value="valactive" runat="server" AutoPostBack="true" OnCheckedChanged="ChckedChanged" />--%>
                                    <asp:CheckBox ID="chkIsFixedShipping" runat="server" AutoPostBack="true" OnCheckedChanged="ChckedChanged" /> 

                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblFixedShipRate" runat="server" Text="FixedShipRate"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtFixedShipRate" runat="server" CssClass="form-control" Width="40%" onkeypress="return isNumber(event)" Text="0"  placeholder="FixedShipRate"> </asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblisactive" runat="server" Text="IsActive"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <input id="chkisactive" name="isActive" type="checkbox" value="valactive" runat="server" />

                                </div>
                            </div>


                            <div class="modal fade" id="modal-default" style="display: none;">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                <span aria-hidden="true">×</span></button>
                                            <h4 class="modal-title">Add New GST(%)</h4>
                                        </div>
                                        <div class="modal-body">
                                            <div class="row pad-bottom">
                                                <div class="col-md-4">
                                                    GST
                                                </div>
                                                <div class="col-md-8">
                                                    <asp:TextBox ID="txtgst1" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                            <br />
                                            <div class="row pad-bottom">
                                                <div class="col-md-4">
                                                    GST Tax Value
                                                </div>
                                                <div class="col-md-8">
                                                    <asp:TextBox ID="txtvalue" runat="server" onkeypress="return isNumber(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                                            <asp:Button ID="Button2" CssClass="btn btn-primary" runat="server" Text="Save changes" OnClick="Button11_Click" />

                                        </div>
                                    </div>
                                    <!-- /.modal-content -->
                                </div>
                                <!-- /.modal-dialog -->
                            </div>


                            <div class="modal fade" id="modal-default1" style="display: none;">
                                <div class="modal-dialog">
                                    <div class="modal-content">
                                        <div class="modal-header">
                                            <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                                                <span aria-hidden="true">×</span></button>
                                            <h4 class="modal-title">Add New UnitName</h4>
                                        </div>
                                        <div class="modal-body">
                                            <div class="row pad-bottom">
                                                <div class="col-md-4">
                                                    UnitName
                                                </div>
                                                <div class="col-md-8">
                                                    <asp:TextBox ID="txtunitvalue1" runat="server" onkeypress="return isNumber(event)"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                        <div class="modal-footer">
                                            <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Close</button>
                                            <asp:Button ID="Button5" CssClass="btn btn-primary" runat="server" Text="Save changes" OnClick="Button22_Click" />

                                        </div>
                                    </div>
                                    <!-- /.modal-content -->
                                </div>
                                <!-- /.modal-dialog -->
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

                        <div class="row pad-bottom" id="divIsApproved" runat="server">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblIsApproved" runat="server" Text="Approved"></asp:Label>
                                </div>
                                <div class="col-md-7 pad">
                                    <input id="ChkIsApproved" name="IsApproved" type="checkbox" value="valIsApproved" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom" id="divJurisdictionIncharge" runat="server">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblJurisdictionIncharge" runat="server" Text="Jurisdiction Incharge"></asp:Label>
                                </div>
                                <div class="col-md-7 pad">
                                    <div class="block">
                                        <asp:CheckBoxList runat="server" ID="chklstJurisdictionIncharge" RepeatLayout="Table">
                                        </asp:CheckBoxList>
                                        <input id="hdnProductCreatedBy" runat="server" type="hidden" />
                                    </div>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnJurisdictionIncharge" style="color: #d9534f; display: none;">Please check at least one checkbox</span>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom" id="divRejectedReason" runat="server">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblRejectedReason" runat="server" Text="Rejected Reason"></asp:Label>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtRejectedReason" runat="server" CssClass="form-control" Width="40%" placeholder="Rejected Reason"></asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnRejectedReason" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>

                    </div>

                    <div role="tabpanel" class="tab-pane" id="grpproduct">
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblgrpUnitName" runat="server" Text="Unit Name"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:DropDownList runat="server" Width="40%" ID="ddlgrpUnitName" CssClass="form-control"></asp:DropDownList>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblgrpUnit" runat="server" Text="Unit"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtgrpUnit" runat="server" TextMode="Number" CssClass="form-control" Width="40%" onkeypress="return isNumber(event)" placeholder="Unit"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spngrpUnit" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblgrpMRP" runat="server" Text="MRP"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtgrpMRP" runat="server" CssClass="form-control" Width="40%" onkeypress="return isNumber(event)" placeholder="MRP"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spngrpMRP" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblgrpDiscountType" runat="server" Text="Discount Type"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:DropDownList ID="ddlgrpDiscountType" runat="server" class="form-control" Width="40%">
                                        <asp:ListItem Value="">Select Discount Type</asp:ListItem>
                                        <asp:ListItem Value="%">%</asp:ListItem>
                                        <asp:ListItem Value="Fixed">Fixed</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spngrpDiscountType" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblgrpDiscount" runat="server" Text="Discount"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtgrpDiscount" runat="server" CssClass="form-control calculate" Width="40%" onkeypress="return isNumber(event)" placeholder="Discount"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spngrpDiscount" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblSoshoPrice" runat="server" Text="Sosho Price"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtgrpSoshoPrice" runat="server" CssClass="form-control" Width="40%" placeholder="Sosho Price"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spngrpSoshoPrice" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblgrpPackingType" runat="server" Text="Packing Type"></asp:Label>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtgrpPackingType" runat="server" CssClass="form-control" Width="40%" placeholder="Packing Type"> </asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblgrpImages" runat="server" Text="Images"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:FileUpload ID="FileUploadgrpImages" runat="server" onchange="grppreviewFile()" OnDataBinding="FileUploadgrpImages_DataBinding" OnLoad="FileUploadgrpImages_Load" OnInit="FileUploadgrpImagesControl_Init" />
                                    <asp:Image ID="GrpImage" Width="202px" Height="90px" runat="server" />
                                    <asp:Button ID="BtnRemoveImage" runat="server" Text="Remove Image" CssClass="btn btn-block btn-danger" Width="120 px" title="Remove Image" OnClick="BtnRemoveImage_Click" />
                                </div>
                                <div class="col-md-2 pad">
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator4" runat="server"
                                        ControlToValidate="FileUploadgrpImages" ErrorMessage="Only .jpg,.png,.jpeg,.gif Files are allowed" Font-Bold="True"
                                        Font-Size="Medium" ValidationExpression="(.*?)\.(jpg|jpeg|png|JPG|JPEG|PNG)$"></asp:RegularExpressionValidator>
                                    <span id="spngrpImages" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblgrpIsDeleted" runat="server" Text="Is Deleted"></asp:Label>
                                </div>
                                <div class="col-md-7 pad">
                                    <input id="chkgrpIsDeleted" name="isgrpIsDeleted" type="checkbox" value="valIsDeleted" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblgrpIsOutOfStock" runat="server" Text="Is OutOfStock"></asp:Label>
                                </div>
                                <div class="col-md-7 pad">
                                    <input id="chkgrpIsOutOfStock" name="isgrpOutofStock" type="checkbox" value="valIsOutOfStock" runat="server" />
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblgrpIsSelected" runat="server" Text="Is Selected"></asp:Label>
                                </div>
                                <div class="col-md-7 pad">
                                    <input id="chkgrpIsSelected" name="isgrpSelectd" type="checkbox" value="valIsSelected" runat="server" />
                                </div>
                            </div>
                        </div>
                          <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblgrpIsBestBuy" runat="server" Text="Is Best Buy"></asp:Label>
                                </div>
                                <div class="col-md-7 pad">
                                    <input id="chkgrpIsBestBuy" name="isgrpbestBuy" type="checkbox" value="valIsBestBuy" runat="server" />
                                </div>
                            </div>
                        </div>
                         <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="Isquantityfreez" runat="server" Text="IsQuantityFreez"  ></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:CheckBox ID="chkIsQuantityFreez" runat="server" AutoPostBack="true" OnCheckedChanged="FreezeQtyChckedChanged" /> 
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 ">
                                    <asp:Label ID="lblFreezeQty" runat="server" Text="Freeze Qty"></asp:Label>
                                </div>

                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtFreezeQty" runat="server" onkeypress="return isNumber(event)" CssClass="form-control" Width="40%" placeholder="Freeze Qty" Text="1"></asp:TextBox>
                                </div>

                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 ">
                                    <asp:Label ID="Label5" runat="server" Text="Max Qty"></asp:Label>
                                </div>

                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtMaxQty" runat="server" onkeypress="return isNumber(event)" CssClass="form-control" Width="40%" placeholder="Max Qty" Text="99"></asp:TextBox>

                                </div>

                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 ">
                                    <asp:Label ID="Label6" runat="server" Text="Min Qty"></asp:Label>
                                </div>

                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtMinQty" runat="server" onkeypress="return isNumber(event)" CssClass="form-control" Width="40%" placeholder="Min Qty" Text="1"></asp:TextBox>

                                </div>

                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                </div>
                                <div class="col-md-9 pad">
                                    <asp:Button ID="BtnAdd" runat="server" Text="Add" CssClass="btn btn-block btn-info" Width="120 px" title="Add" OnClick="BtnAdd_Click" OnClientClick="return Validate()" />
                                    <asp:Button ID="BtnUpdate" runat="server" Text="Update" CssClass="btn btn-block btn-info" Width="120 px" title="Update" OnClick="BtnUpdate_Click" Visible="false" OnClientClick="return Validate()" />
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <%--<asp:GridView ID="grdgProduct" runat="server" AutoGenerateColumns="false" EmptyDataText="No records has been added." class="table table-bordered table-hover" rules="all" role="grid" HeaderStyle-BackColor="#ede8e8" HeaderStyle-HorizontalAlign="Center" Width="95%" CellPadding="10" CellSpacing="5" >

                                    <Columns>
                                        <asp:TemplateField HeaderText="Unit Name">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="HiddenFieldgrpUnitId" runat="server" Value='<%# Bind("grpUnitId") %>' />
                                                <asp:HiddenField ID="HiddenFieldgrpImage" runat="server" Value='<%# Bind("grpImage") %>' />
                                                <asp:HiddenField ID="HiddenFieldgrpisOutOfStock" runat="server" Value='<%# Bind("grpisOutOfStock") %>' />
                                                <asp:HiddenField ID="HiddenFieldgrpisSelected" runat="server" Value='<%# Bind("grpisSelected") %>' />
                                                <asp:Label ID="lblname" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "grpUnitName")%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Unit" DataField="grpUnit" />
                                        <asp:BoundField HeaderText="MRP" DataField="grpMrp" />
                                        <asp:BoundField HeaderText="Discount Type" DataField="grpDiscountType" />
                                        <asp:BoundField HeaderText="Discount" DataField="grpDiscount" />
                                        <asp:BoundField HeaderText="Sosho Price" DataField="grpSoshoPrice" />
                                        <asp:BoundField HeaderText="Packing Type" DataField="grpPackingType" />
                                        <asp:BoundField HeaderText="Is Selected" DataField="grpisSelected" />
                                       <asp:TemplateField>
            <ItemTemplate>
                 <asp:HyperLink ID="lnkEdit" 
                     navigateurl='<%# String.Format("~/Product/ManageProducts.aspx?Id={0}&grpId={1}", Eval("Id"), Eval("grpId")) %>'
                    Text="EDIT" Visible='<%# Eval("Status").ToString() == "Update" %>'
                    runat="server" />
            </ItemTemplate>
        </asp:TemplateField>
                                      
                                    </Columns>
                                </asp:GridView>--%>
                                

                                <asp:GridView ID="grdgProduct" runat="server"  AutoGenerateColumns="false" 
                                    AllowPaging="true" OnRowEditing="OnRow_Editing" Width="99%"    >
                                    <PagerStyle ForeColor="#8C4510" 
          HorizontalAlign="Center"></PagerStyle>
        <HeaderStyle ForeColor="White" Font-Bold="True" 
          BackColor="#A55129"></HeaderStyle>
                                          <Columns>
                                        <asp:TemplateField HeaderText="Unit Name">
                                            <ItemTemplate>
                                                <asp:HiddenField ID="HiddenFieldgrpUnitId" runat="server" Value='<%# Bind("grpUnitId") %>' />
                                                <asp:HiddenField ID="HiddenFieldgrpImage" runat="server" Value='<%# Bind("grpImage") %>' />
                                                <asp:HiddenField ID="HiddenFieldgrpisOutOfStock" runat="server" Value='<%# Bind("grpisOutOfStock") %>' />
                                                <asp:HiddenField ID="HiddenFieldgrpisSelected" runat="server" Value='<%# Bind("grpisSelected") %>' />
                                                <asp:Label ID="lblname" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "grpUnitName")%>'></asp:Label>
                                                <asp:HiddenField ID="HiddenFieldgrpid" runat="server" Value='<%# Bind("grpId") %>' />
                                                <asp:HiddenField ID="HiddenFieldMinQty" runat="server" Value='<%# Bind("grpMinQty") %>' />
                                                <asp:HiddenField ID="HiddenFieldMaxQty" runat="server" Value='<%# Bind("grpMaxQty") %>' />
                                                <asp:HiddenField ID="HiddenFieldIsQtyFreeze" runat="server" Value='<%# Bind("grpIsQtyFreeze") %>' />
                                                <asp:HiddenField ID="HiddenFieldIsBestBuy" runat="server" Value='<%# Bind("grpisBestBuy") %>' />
                                                <asp:HiddenField ID="HiddenFieldFreezeQty" runat="server" Value='<%# Bind("grpFreezeQty") %>' />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderText="Unit" DataField="grpUnit" />
                                        <asp:BoundField HeaderText="MRP" DataField="grpMrp" />
                                        <asp:BoundField HeaderText="Discount Type" DataField="grpDiscountType" />
                                        <asp:BoundField HeaderText="Discount" DataField="grpDiscount" />
                                        <asp:BoundField HeaderText="Sosho Price" DataField="grpSoshoPrice" />
                                        <asp:BoundField HeaderText="Packing Type" DataField="grpPackingType" />
                                        <asp:BoundField HeaderText="Is Selected" DataField="grpisSelected" />
                                       
                                      <asp:TemplateField>
        <ItemTemplate>
            <asp:LinkButton Text="Edit" runat="server" CommandName="Edit" CausesValidation="false" />
        </ItemTemplate>
        <EditItemTemplate>
            <asp:LinkButton Text="Update" runat="server" OnClick="OnUpdate" CausesValidation="false" />
            <asp:LinkButton Text="Cancel" runat="server" OnClick="OnCancel" CausesValidation="false" />
        </EditItemTemplate>
    </asp:TemplateField>
                                              <%-- <asp:TemplateField>
            <ItemTemplate>
                 <asp:HyperLink ID="lnkEdit" 
                     navigateurl='<%# String.Format("~/Product/ManageProducts.aspx?Id={0}&grpId={1}", Eval("Id"), Eval("grpId")) %>'
                    Text="EDIT" Visible='<%# Eval("Status").ToString() == "Update" %>'
                    runat="server" />
            </ItemTemplate>
                                                   </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>
    
                            </div>
                        </div>
                    </div>

                    <div role="tabpanel" class="tab-pane" id="sco">
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblmetatag" runat="server" Text="Meta Tag"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-9 pad">
                                    <asp:TextBox ID="txtmetatag" runat="server" CssClass="form-control" Width="40%" placeholder="Meta Tag"> </asp:TextBox>

                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator3" runat="server" ControlToValidate="txtmetatag" ValidationExpression="[a-zA-Z0-9,_,,]*$" ErrorMessage="*Valid characters: Alphabets and Special Characters(,)." />
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblmetadisc" runat="server" Text="Meta Description"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-9 pad">
                                    <asp:TextBox ID="txtmetadisc" TextMode="multiline" Columns="50" Rows="5" runat="server" CssClass="form-control" Width="40%" placeholder="Meta Description"> </asp:TextBox>

                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 ">
                                    <asp:Label ID="lblogimg" runat="server" Text="OG Image"></asp:Label>
                                </div>
                                <div class="col-md-9 pad">
                                    <asp:FileUpload ID="FileUpload2" onchange="previewFile1()" OnDataBinding="FileUpload2_DataBinding" OnLoad="FileUpload2_Load" OnInit="FileUpload2Control_Init" runat="server" />
                                    <asp:Image ID="productimg1" Width="202px" Height="90px" runat="server" />
                                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server"
                                        ControlToValidate="FileUpload2" ErrorMessage="Only .jpg,.png,.jpeg,.gif Files are allowed" Font-Bold="True"
                                        Font-Size="Medium" ValidationExpression="(.*?)\.(jpg|jpeg|png|JPG|JPEG|PNG)$"></asp:RegularExpressionValidator>
                                    <br />
                                    <asp:Button ID="ogbtnremoveimage" runat="server" Text="Remove Image" OnClick="ogbtnremoveimage_Click" CssClass="btn btn-block btn-danger" Width="120 px" />
                                    <%--<asp:Image ID="img" Width="202px" Height="90px" runat="server" />--%>
                                    <%--<asp:FileUpload ID="fileupload12"  OnDataBinding="fileupload12_DataBinding"  OnInit="FileUploadControl_Init" runat="server" accept=".png,.jpg,.jpeg" />--%>
                                </div>
                            </div>
                        </div>
                        <script type="text/javascript">
                                function previewFile1() {
                                    var preview2 = document.querySelector('#<%=productimg1.ClientID %>');
                                var file2 = document.querySelector('#<%=FileUpload2.ClientID %>').files[0];
                                var reader2 = new FileReader();

                                reader2.onloadend = function () {
                                    preview2.src = reader2.result;
                                }

                                if (file2) {
                                    reader2.readAsDataURL(file2);
                                } else {
                                    preview2.src = "";
                                }
                                }

                        </script>

                    </div>

                    <div role="tabpanel" class="tab-pane" id="price">
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblunit" runat="server" Text="Unit"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtunit" runat="server" TextMode="Number" CssClass="form-control" Width="40%" onkeypress="return isNumber(event)" placeholder="Unit"> </asp:TextBox>

                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnunit" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblunitname" runat="server" Text="Unit Name"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad" style="display: flex">
                                    <asp:DropDownList runat="server" Width="40%" ID="ddlunitname" CssClass="form-control"></asp:DropDownList>
                                    <a class="btn btn-success" data-toggle="modal" data-target="#modal-default1"><i class="fa fa-fw fa-plus-circle"></i></a>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnunitname" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblMrp" runat="server" Text="MRP"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtMRP" runat="server" CssClass="form-control" Width="40%" onkeypress="return isNumber(event)" placeholder="MRP"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnMRP" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="Label2" runat="server" Text="Discount Type"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:DropDownList ID="ddlDiscountType" runat="server" class="form-control" Width="40%" >
                                        <asp:ListItem Value="">Select Discount Type</asp:ListItem>
                                        <asp:ListItem Value="%">%</asp:ListItem>
                                        <asp:ListItem Value="Fixed">Fixed</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnDiscountType" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="Label3" runat="server" Text="Discount"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtDiscount" runat="server" CssClass="form-control calculatediscount" Width="40%" onkeypress="return isNumber(event)" placeholder="Discount"> </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnDiscount" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="Label4" runat="server" Text="Sosho Price"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtSoshoPrice" runat="server" CssClass="form-control" Width="40%" placeholder="Sosho Price" > </asp:TextBox>
                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnSoshoPrice" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom" style="display: none;">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblprice" runat="server" Text="Buy Alone(₹)"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtprice" runat="server" CssClass="form-control" Width="40%" onkeypress="return isNumber(event)" placeholder="Buy Alone(₹)">0</asp:TextBox>

                                </div>
                                <div class="col-md-2 pad">
                                    <span id="spnprice" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom" style="display: none;">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lbloffer" runat="server" Text="Offer(₹)"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtoffer" runat="server" CssClass="form-control" Text="0" Width="40%" onkeypress="return isNumber(event)" placeholder="Offer(₹)">0</asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom" style="display: none;">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblBuyWith1Friend" runat="server" Text="BuyWith1Friend(₹)"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtBuyWith1Friend" runat="server" CssClass="form-control" Width="40%" onkeypress="return isNumber(event)" placeholder="BuyWith1Friend(₹)">0</asp:TextBox>
                                </div>
                            </div>
                        </div>
                        <div class="row pad-bottom" style="display: none;">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblBuyWith5Friend" runat="server" Text="BuyWith5Friend(₹)"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-7 pad">
                                    <asp:TextBox ID="txtBuyWith5Friend" runat="server" CssClass="form-control" Width="40%" onkeypress="return isNumber(event)" placeholder="BuyWith5Friend(₹)">0</asp:TextBox>
                                </div>
                            </div>
                        </div>
                       

                        

                    </div>


                    <div role="tabpanel" class="tab-pane" id="notes">

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblkey" runat="server" Text="Key Features"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-9 pad">
                                    <CKEditor:CKEditorControl class="form-control" ID="ckkey" runat="server">
                                    </CKEditor:CKEditorControl>
                                    <span id="spnkey" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>

                        <div class="row pad-bottom">
                            <div class="col-md-12">
                                <div class="col-md-3 pad">
                                    <asp:Label ID="lblnote" runat="server" Text="Notes"></asp:Label><span style="color: red">*</span>
                                </div>
                                <div class="col-md-9 pad">
                                    <CKEditor:CKEditorControl class="form-control" ID="cknotes" runat="server">
                                    </CKEditor:CKEditorControl>
                                    <span id="spnnote" style="color: #d9534f; display: none;">This field is required</span>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div role="tabpanel" class="tab-pane" id="images">
                        <div class="col-md-12">
                            <div class="col-md-3 ">
                                <asp:Label ID="lblimages" runat="server" Text="Images"></asp:Label><span style="color: red">*</span>
                            </div>
                            <div class="col-md-9 pad">
                                <asp:FileUpload ID="FileUploadMainImages" onchange="previewFile()" OnDataBinding="FileUploadMainImages_DataBinding" OnLoad="FileUploadMainImages_Load" OnInit="FileUploadMainImagesControl_Init" runat="server" />

                                <asp:Image ID="productimg" Width="202px" Height="90px" runat="server" />
                                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server"
                                    ControlToValidate="FileUploadMainImages" ErrorMessage="Only .jpg,.png,.jpeg,.gif Files are allowed" Font-Bold="True"
                                    Font-Size="Medium" ValidationExpression="(.*?)\.(jpg|jpeg|png|JPG|JPEG|PNG)$"></asp:RegularExpressionValidator>

                                <br />
                                <asp:Button ID="bannerremoveImage" runat="server" Text="Remove Image" OnClick="bannerremoveImage_Click1" CssClass="btn btn-block btn-danger" Width="120 px" />
                                <%-- <asp:Button ID="bannerremoveImage" runat="server" Text="Remove Image" OnClick="bannerremoveImage_Click" CssClass="btn btn-block btn-danger" Width="120 px" />--%>
                                <asp:Label ID="lblResult" runat="server" />
                                <%--<asp:Image ID="img" Width="202px" Height="90px" runat="server" />--%>
                                <%--<asp:FileUpload ID="fileupload12"  OnDataBinding="fileupload12_DataBinding"  OnInit="FileUploadControl_Init" runat="server" accept=".png,.jpg,.jpeg" />--%>
                            </div>
                        </div>
                        <div class="row pad-bottom">


                            <div class="col-md-12">


                                <asp:GridView runat="server" ID="gvImage" OnRowDataBound="gvImage_RowDataBound" AutoGenerateColumns="false" AllowPaging="True" OnRowCancelingEdit="gvImage_RowCancelingEdit" OnRowCommand="gvImage_RowCommand" DataKeyNames="ImageId" OnRowEditing="gvImage_RowEditing" OnRowUpdating="gvImage_RowUpdating" OnRowDeleting="gvImage_RowDeleting" class="table table-bordered table-hover" role="grid" CellPadding="10" CellSpacing="5" AllowSorting="True" HeaderStyle-BackColor="#ede8e8" HeaderStyle-HorizontalAlign="Center" Caption="<b><u>UPLOADED IMAGES</u></b>" CaptionAlign="Top" autopostback="false">

                                    <Columns>
                                        <%--<asp:TemplateField HeaderStyle-Width="16%">
                                            <ItemTemplate>


                                                <asp:Button ID="btn_Edit" runat="server" CssClass="btn btn-success" Text="Edit" CommandName="Edit" />
                                                <asp:Button ID="LkB11" runat="server" CssClass="btn btn-success" Text="Delete" CommandName="Delete" />
                                              
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Button ID="btn_Update" runat="server" Text="Update" CommandName="Update" />
                                                <asp:Button ID="btn_Cancel" runat="server" Text="Cancel" CommandName="Cancel" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>--%>

                                        <asp:CommandField ShowEditButton="true" HeaderStyle-Width="8%" ControlStyle-CssClass="btn btn-success" />
                                        <asp:CommandField ShowDeleteButton="true" HeaderStyle-Width="8%" ControlStyle-CssClass="btn btn-danger" />
                                        <asp:TemplateField HeaderText="ImageId" HeaderStyle-Width="100px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblImgId" runat="server" Text='<%#Container.DataItemIndex+1%>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Image" HeaderStyle-Width="200px">
                                            <ItemTemplate>
                                                <asp:Image ID="Image1" runat="server" ImageUrl='<%# Eval("Image") %>' Height="80px" Width="100px" />
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:Image ID="img_user" runat="server" ImageUrl='<%# Eval("Image") %>' Height="80px" Width="100px" />
                                                <br />
                                                <asp:FileUpload ID="FileUpload1" runat="server" />
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="ImageName" HeaderStyle-Width="200px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblImageName" runat="server" Text='<%# Eval("ImageName") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_ImageName" runat="server" Text='<%# Eval("ImageName") %>' ReadOnly="true"></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="ImageDisplayOrder" HeaderStyle-Width="200px">
                                            <ItemTemplate>
                                                <asp:Label ID="lblImageDisplayOrder" runat="server" Text='<%# Eval("ImageDisplayOrder") %>'></asp:Label>
                                            </ItemTemplate>
                                            <EditItemTemplate>
                                                <asp:TextBox ID="txt_ImageDisplayOrder" runat="server" Text='<%# Eval("ImageDisplayOrder") %>'></asp:TextBox>
                                            </EditItemTemplate>
                                        </asp:TemplateField>


                                        <%--<asp:HyperLinkField HeaderText="Edit" HeaderStyle-Width="8%" DataNavigateUrlFields="ImageId" DataNavigateUrlFormatString="~/Product/ManageProducts.aspx?DimgId={0}" Target="_blank" Text="Delete" />--%>


                                        <%--                                        <asp:TemplateField HeaderStyle-Width="8%">
                                            <ItemTemplate>
                                                
                                                <asp:LinkButton ID="LkB1" runat="server" CssClass="btn btn-success" CommandName="Edit" CommandArgument='<%# Eval("ImageId") %>'>Edit</asp:LinkButton>
                                            </ItemTemplate>

                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderStyle-Width="8%">
                                            <ItemTemplate>
                                                <asp:LinkButton runat="server" ID="LkB11" CommandName="Delete" CssClass="btn btn-danger" CommandArgument='<%# Eval("ImageId") %>'> Delete
                                                </asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>--%>
                                    </Columns>
                                </asp:GridView>

                            </div>

                        </div>
                        <script type="text/javascript">
                            function previewFile() {
                                var preview3 = document.querySelector('#<%=productimg.ClientID %>');
                                var file3 = document.querySelector('#<%=FileUploadMainImages.ClientID %>').files[0];
                                var reader3 = new FileReader();

                                reader3.onloadend = function () {
                                    preview3.src = reader3.result;
                                }

                                if (file3) {
                                    reader3.readAsDataURL(file3);
                                } else {
                                    preview3.src = "";
                                }
                            }
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
                $(document).ready(function () {
                    var value = $("#ContentPlaceHolder1_ddlProductType").val();
                    //if (value == "2") {
                    //    $('#Tabs ul li:eq(1)').show();
                    //    $('#Tabs ul li:eq(3)').hide();
                    //    $('#Tabs ul li:eq(5)').hide();
                    //}
                    //else {
                    //    $('#Tabs ul li:eq(1)').hide();
                    //    $('#Tabs ul li:eq(3)').show();
                    //    $('#Tabs ul li:eq(5)').show();
                    //}
                    $('#Tabs ul li:eq(1)').show();
                    $('#Tabs ul li:eq(3)').hide();
                    $('#Tabs ul li:eq(5)').hide();
                    CalulateSoshoPrice();
                    CalulatePSoshoPrice();

                    var isProductval = $("#ContentPlaceHolder1_chkIsProductDescription").is(":checked");

                    if (isProductval == true) {
                        $("#divFullDescription").show();
                        $('#Tabs ul li:eq(4)').show();
                    }
                    else {
                        $("#divFullDescription").hide();
                        $('#Tabs ul li:eq(4)').hide();
                    }

                });

                $("#ContentPlaceHolder1_ddlProductType").change(function () {
                    var end = this.value;
                    $('#Tabs ul li:eq(1)').show();
                    $('#Tabs ul li:eq(3)').hide();
                    $('#Tabs ul li:eq(5)').hide();
                    //if (end == "2") {
                    //    $('#Tabs ul li:eq(1)').show();
                    //    $('#Tabs ul li:eq(3)').hide();
                    //    $('#Tabs ul li:eq(5)').hide();
                    //}
                    //else {
                    //    $('#Tabs ul li:eq(1)').hide();
                    //    $('#Tabs ul li:eq(3)').show();
                    //    $('#Tabs ul li:eq(5)').show();
                    //}
                });

               $("#ContentPlaceHolder1_ddlDiscountType").change(function () {
                    debugger
                    var end = this.value;
                    alert(end);
                    if (end != "") {
                        $("#spnDiscountType").css('display', 'none');
                        var total = 0;
                        var grpDiscountTypeval = $("#ContentPlaceHolder1_ddlDiscountType").val();
                        var grpMrpval = $("#ContentPlaceHolder1_txtMRP").val();
                        var val = $("#ContentPlaceHolder1_txtDiscount").val();
                        var soshoPrice = $("#ContentPlaceHolder1_txtSoshoPrice").val();
                        if (grpDiscountTypeval == "%") {
                            //total = grpMrpval - ((grpMrpval * val) / 100);
                            total = (soshoPrice * 100) / grpMrpval;
                        }
                        else if (grpDiscountTypeval == "Fixed") {
                            //total = grpMrpval - val;
                            total = grpMrpval - soshoPrice;
                        }
                        $('#<%=txtSoshoPrice.ClientID %>').val(total.toFixed(2));
                    }
                    else {
                        $("#spnDiscountType").css('display', 'block');
                    }
                });
               
                $('#ContentPlaceHolder1_txtgrpSoshoPrice').focusout(function (event) {
                    debugger
                    var total = 0;
                    var grpDiscountTypeval = $("#ContentPlaceHolder1_ddlgrpDiscountType").val();
                    var grpMrpval = $("#ContentPlaceHolder1_txtgrpMRP").val();
                    var val = $("#ContentPlaceHolder1_txtgrpDiscount").val();
                    var soshoPrice = $("#ContentPlaceHolder1_txtgrpSoshoPrice").val();
                    if (grpDiscountTypeval == "%") {
                        //total = grpMrpval - ((grpMrpval * val) / 100);
                        total = 100 - (soshoPrice * 100) / grpMrpval;
                    }
                    else if (grpDiscountTypeval == "Fixed") {
                        //total = grpMrpval - val;
                        total = grpMrpval - soshoPrice;
                    }
                    $('#<%=txtgrpDiscount.ClientID %>').val(total.toFixed(2));
                });

                $('#ContentPlaceHolder1_txtgrpMRP').focusout(function (event) {
                    debugger
                    var total = 0;
                    var grpDiscountTypeval = $("#ContentPlaceHolder1_ddlgrpDiscountType").val();
                    var grpMrpval = $("#ContentPlaceHolder1_txtgrpMRP").val();
                    var val = $("#ContentPlaceHolder1_txtgrpDiscount").val();
                    var soshoPrice = $("#ContentPlaceHolder1_txtgrpSoshoPrice").val();
                    if (grpDiscountTypeval == "%") {
                        //total = grpMrpval - ((grpMrpval * val) / 100);
                        total = 100 - (soshoPrice * 100) / grpMrpval;
                    }
                    else if (grpDiscountTypeval == "Fixed") {
                        //total = grpMrpval - val;
                        total = grpMrpval - soshoPrice;
                    }
                    $('#<%=txtgrpDiscount.ClientID %>').val(total.toFixed(2));
                });
                $("#ContentPlaceHolder1_ddlgrpDiscountType").change(function () {
                    var end = this.value;
                    if (end != "") {
                        $("#spngrpDiscountType").css('display', 'none');
                        var total = 0;
                        var grpDiscountTypeval = $("#ContentPlaceHolder1_ddlgrpDiscountType").val();
                        var grpMrpval = $("#ContentPlaceHolder1_txtgrpMRP").val();
                        var val = $("#ContentPlaceHolder1_txtgrpDiscount").val();
                        if (grpDiscountTypeval == "%") {
                            total = grpMrpval - ((grpMrpval * val) / 100);
                        }
                        else if (grpDiscountTypeval == "Fixed") {
                            total = grpMrpval - val;
                        }
                        $('#<%=txtgrpSoshoPrice.ClientID %>').val(total.toFixed(2));
                    }
                    else {
                        $("#spngrpDiscountType").css('display', 'block');
                    }
                });

                $('#ContentPlaceHolder1_BtnAdd').click(function () {
                    var flag = true;
                    var grpUnitval = $("#ContentPlaceHolder1_txtgrpUnit").val();
                    var grpMrpval = $("#ContentPlaceHolder1_txtgrpMRP").val();
                    var grpDiscountTypeval = $("#ContentPlaceHolder1_ddlgrpDiscountType").val();
                    var grpDiscountval = $("#ContentPlaceHolder1_txtgrpDiscount").val();
                    var grpSoshoPriceval = $("#ContentPlaceHolder1_txtgrpSoshoPrice").val();
                    var grppreview1 = document.querySelector('#<%=GrpImage.ClientID %>');
                    var totalRows = $("#<%=grdgProduct.ClientID %> tr").length;
                    if (grpUnitval == "") {
                        $("#spngrpUnit").css('display', 'block');
                        flag = false;
                    }
                    if (grpMrpval == "") {
                        $("#spngrpMRP").css('display', 'block');
                        flag = false;
                    }
                    if (grpDiscountTypeval == "") {
                        $("#spngrpDiscountType").css('display', 'block');
                        flag = false;
                    }
                    if (grpDiscountval == "") {
                        $("#spngrpDiscount").css('display', 'block');
                        flag = false;
                    }
                    if (grpSoshoPriceval == "") {
                        $("#spngrpSoshoPrice").css('display', 'block');
                        flag = false;
                    }
                    if (grppreview1.src == "") {
                        $("#spngrpImages").css('display', 'block');
                        flag = false;
                    }

                    if (flag) {
                        $('#ContentPlaceHolder1_BtnAdd').click();
                    }
                    return flag;
                });
                function CalulateSoshoPrice() {
                    $(".calculate").each(function () {
                        $(this).keyup(function () {
                            var total = 0;
                            $(".calculate").each(function () {
                                if (!isNaN(this.value) && this.value.length != 0) {
                                    $("#spngrpDiscountType").css('display', 'none');
                                    var grpDiscountTypeval = $("#ContentPlaceHolder1_ddlgrpDiscountType").val();
                                    var grpMrpval = $("#ContentPlaceHolder1_txtgrpMRP").val();
                                    if (grpDiscountTypeval == "%") {
                                        total = grpMrpval - ((grpMrpval * this.value) / 100);
                                    }
                                    else if (grpDiscountTypeval == "Fixed") {
                                        total = grpMrpval - this.value;
                                    }
                                    else {
                                        $("#spngrpDiscountType").css('display', 'block');
                                    }
                                }
                            });
                            $('#<%=txtgrpSoshoPrice.ClientID %>').val(total.toFixed(2));
                        });
                    });
                }
                function grppreviewFile() {
                    var preview1 = document.querySelector('#<%=GrpImage.ClientID %>');
                    var file1 = document.querySelector('#<%=FileUploadgrpImages.ClientID %>').files[0];
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

                $("#ContentPlaceHolder1_BtnSave").click(function () {
                    var flag = true;
                    var IsAdminval = $("#ContentPlaceHolder1_hdnIsAdmin").val();
                    var pnameval = $("#ContentPlaceHolder1_txtpname").val();
                    var CategoryNameval = $("#ContentPlaceHolder1_ddlCategoryName").val();
                    var ProductTypeval = $("#ContentPlaceHolder1_ddlProductType").val();
                    var videolinkval = $("#ContentPlaceHolder1_txtvdo").val();
                    var soldtimeval = $("#ContentPlaceHolder1_txtNoOfSoldItems").val();
                    var gstval = $("#ContentPlaceHolder1_ddlgst").val();
                    var unitval = $("#ContentPlaceHolder1_txtunit").val();
                    var unitnameval = $("#ContentPlaceHolder1_ddlunitname").val();
                    var displayorderval = $("#ContentPlaceHolder1_txtDisplayOrder").val();
                    var grpDiscountTypeval = $("#ContentPlaceHolder1_ddlDiscountType").val();
                    var grpMrpval = $("#ContentPlaceHolder1_txtMRP").val();
                    var val = $("#ContentPlaceHolder1_txtDiscount").val();
                    var isProductval = $("#ContentPlaceHolder1_chkIsProductDescription").is(":checked");
                    var Productdesval = $("#ContentPlaceHolder1_txtFullDescription").val();
                    var KeyFeatureval = $("#ContentPlaceHolder1_ckkey").val();
                    var Notesval = $("#ContentPlaceHolder1_cknotes").val();

                    var isApprovedval = $("#ContentPlaceHolder1_ChkIsApproved").prop('checked');
                    var Inchargechecked = $("#ContentPlaceHolder1_chklstJurisdictionIncharge input[type='checkbox']:checked").length > 0;
                    var rejectedreasonval = $("#ContentPlaceHolder1_txtRejectedReason").val();

                    if (pnameval == "") {
                        $("#spnpname").css('display', 'block');
                        flag = false;
                    }
                    if (CategoryNameval == "") {
                        $("#spnCategoryName").css('display', 'block');
                        flag = false;
                    }
                    if (ProductTypeval == "") {
                        $("#spnProductType").css('display', 'block');
                        flag = false;
                    }
                    //if (videolinkval == "") {
                    //    $("#spnvdo").css('display', 'block');
                    //    flag = false;
                    //}
                    if (soldtimeval == "") {
                        $("#spnsoldtime").css('display', 'block');
                        flag = false;
                    }
                    if (gstval == "") {
                        $("#spngst").css('display', 'block');
                        flag = false;
                    }
                    //if (unitval == "") {
                    //    if (ProductTypeval == "1") {
                    //        $("#spnunit").css('display', 'block');
                    //        flag = false;
                    //    }
                    //}
                    //if (unitnameval == "") {
                    //    if (ProductTypeval == "1") {
                    //        $("#spnunitname").css('display', 'block');
                    //        flag = false;
                    //    }
                    //}
                    if (displayorderval == "") {
                        $("#spnDisplayOrder").css('display', 'block');
                        flag = false;
                    }
                    if (ProductTypeval == "2") {
                        var totalRows = $("#<%=grdgProduct.ClientID %> tr").length;
                    }
                    //if (grpDiscountTypeval == "") {
                    //    $("#spnDiscountType").css('display', 'block');
                    //    flag = false;
                    //}
                    //if (grpMrpval == "") {
                    //    $("#spnDiscount").css('display', 'block');
                    //    flag = false;
                    //}
                    //if (val == "") {
                    //    $("#spnSoshoPrice").css('display', 'block');
                    //    flag = false;
                    //}
                    //if (isProductval == true) {
                    //    if (Productdesval == "") {
                    //         $("#spnFullDescription").css('display', 'block');
                    //        flag = false;
                    //    }
                    //    if (KeyFeatureval == "") {
                    //        $("#spnkey").css('display', 'block');
                    //        flag = false;
                    //    }
                    //    if (Notesval == "") {
                    //        $("#spnnote").css('display', 'block');
                    //        flag = false;
                    //    }
                    //}
                    if (IsAdminval == "1") {
                        if (isApprovedval == true) {
                            if (!Inchargechecked) {
                                $("#spnJurisdictionIncharge").css('display', 'block');
                                flag = false;
                            }
                        }
                        else if (isApprovedval == false) {
                            if (rejectedreasonval == "") {
                                $("#spnRejectedReason").css('display', 'block');
                                flag = false;
                            }
                        }
                    }
                    if (flag) {
                        $('#ContentPlaceHolder1_BtnSave').click();
                    }
                    return flag;
                });


                

                

                function CalulatePSoshoPrice() {
                    $(".calculatediscount").each(function () {
                        $(this).keyup(function () {
                            var total = 0;
                            $(".calculatediscount").each(function () {
                                if (!isNaN(this.value) && this.value.length != 0) {
                                    $("#spnDiscountType").css('display', 'none');
                                    var grpDiscountTypeval = $("#ContentPlaceHolder1_ddlDiscountType").val();
                                    var grpMrpval = $("#ContentPlaceHolder1_txtMRP").val();
                                    if (grpDiscountTypeval == "%") {
                                        total = grpMrpval - ((grpMrpval * this.value) / 100);
                                    }
                                    else if (grpDiscountTypeval == "Fixed") {
                                        total = grpMrpval - this.value;
                                    }
                                    else {
                                        $("#spnDiscountType").css('display', 'block');
                                    }
                                }
                            });
                            $('#<%=txtSoshoPrice.ClientID %>').val(total.toFixed(2));
                        });
                    });
                }

                $("#ContentPlaceHolder1_chkgrpIsSelected").change(function () {
                    var totalRows = $("#<%=grdgProduct.ClientID %> tr").length;
                    if (this.checked) {
                        if (totalRows != 0) {
                            $("#<%=grdgProduct.ClientID %> tr").each(function (index) {
                                if (!this.rowIndex) return; // skip first row
                                $("#<%=grdgProduct.ClientID %> tr").children('td').each((index, td) => {
                                    //console.log(td);
                                    var IsSelected = $(this).find("td:eq(7)").html();
                                    if (IsSelected == "True") {
                                        var returnVal = confirm("Are you sure Is Selected true?");
                                        if (returnVal == true) {
                                            $("#<%=grdgProduct.ClientID %> tr").find("td:eq(7)").html("False");
                                        }
                                        else {
                                            return false;
                                        }
                                    }
                                });

                            });
                        }
                    }

                });

                $("#ContentPlaceHolder1_chkIsProductDescription").change(function () {

                    if (this.checked) {
                        $("#divFullDescription").show();
                        $('#Tabs ul li:eq(4)').show();
                    }
                    else {
                        $("#divFullDescription").hide();
                        $('#Tabs ul li:eq(4)').hide();
                    }
                });
            </script>

        </section>
    </div>
</asp:Content>
