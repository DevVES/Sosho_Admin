<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ProductAssignToCategory.aspx.cs" Inherits="Product_ProductAssignToCategory" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>

    <link rel="stylesheet" href="../../plugins/datepicker/datepicker3.css" />
    <link rel="stylesheet" href="../../plugins/timepicker/bootstrap-timepicker.min.css" />
    <script src="../../plugins/datepicker/bootstrap-datepicker.js"></script>
    <script src="../../plugins/timepicker/bootstrap-timepicker.min.js"></script>

    <style type="text/css">
        .content {
            min-height: 100vh;
        }
    </style>

    <style>
        .red {
            position: relative;
            text-decoration: none;
            color: #fff;
            background: #2783cb;
            text-align: center;
            padding: 2px 10px;
            width: 75px;
            border-radius: 5px;
            border: solid 1px #2770cb;
            transition: all 0.1s;
            -webkit-box-shadow: 0px 9px 0px #2770cb; /*for opera and safari*/
            -moz-box-shadow: 0px 9px 0px #2770cb; /*for mozilla*/
            -o-box-shadow: 0px 9px 0px #2770cb; /*for opera*/
            -ms-box-shadow: 0px 9px 0px #2770cb; /*for I.E.*/
        }

        .add-padding {
            padding: 7px 100px;
        }
    </style>

    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Assign Products To Category
               <small>
                   <asp:Label runat="server" ID="lblDateTime"></asp:Label>
               </small>
            </h1>
            <ol class="breadcrumb">
            </ol>
        </section>
        <section class="content">
            <!-- Small boxes (Stat box) -->

            <div class="row">
                <asp:Button ID="btnAdd" runat="server" Text="Add" Width="70Px" CssClass="btn btn-success pull-right add-padding" OnClick="BtnAdd_Click" />
            </div>
            <asp:Label>From Category: </asp:Label>
            <div class="row">
                
                <div class="col-md-3 col-sm-6 col-xs-12">
                    
                    <div class="form-group">
                        <asp:DropDownList ID="ddlCategoryName" runat="server" class="form-control" OnSelectedIndexChanged="OnSelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-6 pad">
                        <span id="spnCategoryName" style="color: #d9534f; display: none;">This field is required</span>
                    </div>
                </div>
                <div class="col-md-3 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <asp:DropDownList ID="ddlSubCategoryName" runat="server" class="form-control">
                        </asp:DropDownList>
                    </div>
                     <div class="col-md-6 pad">
                        <span id="spnSubCategoryName" style="color: #d9534f; display: none;">This field is required</span>
                    </div>
                </div>
                
            </div>
            <asp:Label>To Category: </asp:Label>
            <div class="row">
                <div class="col-md-3 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <asp:DropDownList ID="ddlToCategoryName" runat="server" class="form-control" OnSelectedIndexChanged="OnToSelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                    <div class="col-md-6 pad">
                        <span id="spnToCategoryName" style="color: #d9534f; display: none;">This field is required</span>
                    </div>
                </div>
                <div class="col-md-3 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <asp:DropDownList ID="ddlToSubCategoryName" runat="server" class="form-control">
                        </asp:DropDownList>
                    </div>
                     <div class="col-md-6 pad">
                        <span id="spnToSubCategoryName" style="color: #d9534f; display: none;">This field is required</span>
                    </div>
                </div>
                <div class="col-md-3 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <asp:DropDownList ID="ddlJurisdiction" runat="server" class="form-control">
                        </asp:DropDownList>
                    </div>
                     <div class="col-md-6 pad">
                        <span id="spnJurisdiction" style="color: #d9534f; display: none;">This field is required</span>
                    </div>
                </div>
                <div class="col-md-3 col-sm-6 col-xs-12">
                    <asp:Button ID="Button2" runat="server" Text="Go" Width="70Px" CssClass="btn btn-block  btn-info" OnClick="Button1_Click" OnClientClick="return Validate()"  />
                </div>
            </div>

            <div style="width: 100%;" class="table-responsive">
                <asp:GridView ID="gvproductlist" OnRowDataBound="gvproductlist_RowDataBound" OnRowCommand="gvproductlist_RowCommand"
                    runat="server" Width="95%" AutoGenerateColumns="False" class="table table-bordered table-hover" rules="all"
                    role="grid" CellPadding="10" CellSpacing="5" AllowSorting="True" HeaderStyle-BackColor="#ede8e8"
                    HeaderStyle-HorizontalAlign="Center" Caption="<b><u>PRODUCT LIST</u></b>" CaptionAlign="Top">
                    <Columns>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <asp:HiddenField ID="HiddenFieldgrpid" runat="server" Value='<%# Bind("Id") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Select">
                            <EditItemTemplate>
                                <asp:CheckBox ID="chkProduct" runat="server" />
                            </EditItemTemplate>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkProduct" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Name" DataField="Name" />
                        <asp:BoundField HeaderText="Product Type" DataField="ProductType" />
                        <asp:BoundField HeaderText="IsActive" DataField="IsActive" />
                        <asp:BoundField HeaderText="StartDate" DataField="StartDate" />
                        <asp:BoundField HeaderText="EndDate" DataField="EndDate" />
                    </Columns>
                    <HeaderStyle HorizontalAlign="Center" BackColor="#EDE8E8"></HeaderStyle>
                </asp:GridView>
            </div>

            <script type="text/javascript">
                $(document).ready(function () {
                    $('#ContentPlaceHolder1_gvproductlist').DataTable({
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

                    $("#ContentPlaceHolder1_Button2").click(function () {
                        var flag = true;
                        var categoryval = $("#ContentPlaceHolder1_ddlCategoryName").val();
                        var subcategoryval = $("#ContentPlaceHolder1_ddlSubCategoryName").val();
                        var Jurisdictionval = $("#ContentPlaceHolder1_ddlJurisdiction").val();
                        var tocategoryval = $("#ContentPlaceHolder1_ddlToCategoryName").val();
                        var tosubcategoryval = $("#ContentPlaceHolder1_ddlToSubCategoryName").val();
                        if (categoryval == "0") {
                            $("#spnCategoryName").css('display', 'block');
                            flag = false;
                        }
                        if (subcategoryval == "0") {
                            $("#spnSubCategoryName").css('display', 'block');
                            flag = false;
                        }
                        if (tocategoryval == "0") {
                            $("#spnToCategoryName").css('display', 'block');
                            flag = false;
                        }
                        if (tosubcategoryval == "0") {
                            $("#spnToSubCategoryName").css('display', 'block');
                            flag = false;
                        }
                        if (Jurisdictionval == "0") {
                            $("#spnJurisdiction").css('display', 'block');
                            flag = false;
                        }
                        if (flag) {
                            $('#ContentPlaceHolder1_Button2').click();
                        }
                        return flag;
                    })
                });
            </script>
        </section>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="Server">
</asp:Content>
