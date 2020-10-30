<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ProductList.aspx.cs" Inherits="Product_ProductList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>

    <link href="../plugins/datepicker/datepicker3.css" rel="stylesheet" />
    <script src="../plugins/datepicker/bootstrap-datepicker.js"></script>



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
            <h1>Product List
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
                <a href="ManageProducts.aspx" class="btn btn-success pull-right add-padding" style="width: 80px">ADD</a>
            </div>
            <div class="row">
                <div class="col-md-3 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <asp:Label runat="server" ID="Startdate"></asp:Label>
                        <asp:TextBox ID="txtdt" CssClass="form-control" placeholder="Select Date" runat="server"></asp:TextBox>
                        <script>
                            $('#ContentPlaceHolder1_txtdt').datepicker({
                                format: 'dd/mm/yyyy',
                                autoclose: true
                            });
                        </script>
                        <!-- /.input group -->
                    </div>
                </div>
                <div class="col-md-3 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <asp:Label runat="server" ID="EndDate"></asp:Label>
                        <asp:TextBox ID="txtdt1" CssClass="form-control" placeholder="Select Date" runat="server"></asp:TextBox>
                        <script>
                            $('#ContentPlaceHolder1_txtdt1').datepicker({
                                format: 'dd/mm/yyyy',
                                autoclose: true
                            });
                        </script>
                        <!-- /.input group -->
                    </div>
                </div>




            </div>

            <div class="row">
                <div class="col-md-3 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <asp:DropDownList ID="ddlCategoryName" runat="server" class="form-control" OnSelectedIndexChanged="OnSelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-3 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <asp:DropDownList ID="ddlSubCategoryName" runat="server" class="form-control" OnSelectedIndexChanged="OnSelectedIndexSubCategoryChanged" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-3 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <asp:DropDownList ID="ddlProduct" runat="server" class="form-control">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-3 col-sm-6 col-xs-12">

                    <asp:Button ID="Button2" runat="server" Text="Go" Width="70Px" CssClass="btn btn-block  btn-info" OnClick="Button1_Click" />

                </div>
            </div>

            <div style="width: 100%;" class="table-responsive">
                <asp:GridView ID="gvproductlist" OnRowDataBound="gvproductlist_RowDataBound" OnRowCommand="gvproductlist_RowCommand"
                    runat="server" Width="95%" AutoGenerateColumns="False" class="table table-bordered table-hover" rules="all"
                    role="grid" CellPadding="10" CellSpacing="5" AllowSorting="True" HeaderStyle-BackColor="#ede8e8" 
                    HeaderStyle-HorizontalAlign="Center" Caption="<b><u>PRODUCT LIST</u></b>" CaptionAlign="Top">
                    <Columns>
                        <%-- <asp:TemplateField HeaderText="OrderId" Visible="True">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="HyperLink2" NavigateUrl='<%# "~/admin/SubOrderDetails.aspx?OrderId=" + Eval("OrderId")%>' Text='<%# Eval("OrderId")%>' Target="_blank"></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>--%>

                        <asp:BoundField HeaderText="Name" DataField="Name" />
                        <asp:BoundField HeaderText="Product Type" DataField="ProductType" />
                        <%--<asp:BoundField HeaderText="Offer" DataField="Offer" />
                        <asp:BoundField HeaderText="BuyWith1FriendPrice" DataField="BuyWith1Friend" />
                        <asp:BoundField HeaderText="BuyWith5FriendPrice" DataField="BuyWith5Friend" />
                        --%><asp:BoundField HeaderText="IsActive" DataField="IsActive" />
                        <asp:BoundField HeaderText="StartDate" DataField="StartDate" />
                        <asp:BoundField HeaderText="EndDate" DataField="EndDate" />


                        <asp:HyperLinkField DataNavigateUrlFields="Id" ControlStyle-CssClass="red" HeaderText="EDIT" DataNavigateUrlFormatString="~/Product/ManageProducts.aspx?Id={0}" Text="Edit" />
                        <%--<asp:HyperLinkField DataNavigateUrlFields="Id" ControlStyle-CssClass="red" HeaderText="Change Price" DataNavigateUrlFormatString="~/Product/ProductList.aspx?Id={0}" Text="Change Price" />--%>
                        <%--    <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button Text="Change Price" ID="btnPopup"  runat="server" ControlStyle-CssClass="red" CommandName="Price" OnClientClick="return false;" />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="">
                            <ItemTemplate>
                                <%--<input type="button" id="btnPopup"  runat="server"  />--%>
                                <asp:LinkButton ID="btnPopup" CommandName="Price" runat="server" CommandArgument='<%#Eval("Id") %>'>Change Price</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <HeaderStyle HorizontalAlign="Center" BackColor="#EDE8E8"></HeaderStyle>
                </asp:GridView>
            </div>
            <!-- this is bootstrp modal popup -->
            <div id="myModal" class="modal fade">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content" style="width: 120%;">
                        <div class="modal-header">
                            <button type="button" class="close" data-dismiss="modal" aria-hidden="true">×</button>
                            <h4 class="modal-title">Variants Of 
                                <label id="lblProdName" runat="server"></label>
                            </h4>
                        </div>
                        <div class="modal-body" style="overflow-y: auto; max-height: 100%; margin-bottom: 20px;">
                            <asp:Label ID="lblmessage" runat="server" ClientIDMode="Static"></asp:Label>

                            <asp:GridView ID="grdgProduct" runat="server" AutoGenerateColumns="false" OnRowEditing="GridView1_RowEditing"
                                AllowPaging="true" Width="99%">
                                <PagerStyle ForeColor="#8C4510"
                                    HorizontalAlign="Center"></PagerStyle>
                                <HeaderStyle ForeColor="White" Font-Bold="True"
                                    BackColor="#A55129"></HeaderStyle>
                                <Columns>
                                    <asp:TemplateField HeaderText="Unit Name">
                                        <ItemTemplate>
                                            <asp:HiddenField ID="HiddenFieldgrpid" runat="server" Value='<%# Bind("grpId") %>' />
                                            <asp:HiddenField ID="HiddenFieldgrpUnitId" runat="server" Value='<%# Bind("grpUnitId") %>' />
                                            <asp:HiddenField ID="HiddenFieldgrpImage" runat="server" Value='<%# Bind("grpImage") %>' />
                                            <asp:HiddenField ID="HiddenFieldgrpisOutOfStock" runat="server" Value='<%# Bind("grpisOutOfStock") %>' />
                                            <asp:HiddenField ID="HiddenFieldgrpisSelected" runat="server" Value='<%# Bind("grpisSelected") %>' />
                                            <asp:Label ID="lblname" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "grpUnitName")%>'></asp:Label>

                                            <asp:HiddenField ID="HiddenFieldMinQty" runat="server" Value='<%# Bind("grpMinQty") %>' />
                                            <asp:HiddenField ID="HiddenFieldMaxQty" runat="server" Value='<%# Bind("grpMaxQty") %>' />
                                            <asp:HiddenField ID="HiddenFieldIsQtyFreeze" runat="server" Value='<%# Bind("grpIsQtyFreeze") %>' />
                                            <asp:HiddenField ID="HiddenFieldIsBestBuy" runat="server" Value='<%# Bind("grpisBestBuy") %>' />
                                            <asp:HiddenField ID="HiddenFieldFreezeQty" runat="server" Value='<%# Bind("grpFreezeQty") %>' />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField HeaderText="Unit" DataField="grpUnit" />
                                    <%--<asp:TemplateField HeaderText="MRP" ItemStyle-Width="120">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtGrpMrp" Text='<%# Eval("grpMrp") %>' AutoPostBack="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:BoundField HeaderText="MRP" DataField="grpMrp" />
                                  <%--  <asp:TemplateField HeaderText="DiscountType" ItemStyle-Width="120">
                                        <ItemTemplate>
                                            <asp:TextBox runat="server" ID="txtGrpDiscountType" Text='<%# Eval("grpDiscountType") %>' AutoPostBack="true" />
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:BoundField HeaderText="Discount Type" DataField="grpDiscountType" />
                                    <asp:BoundField HeaderText="Discount" DataField="grpDiscount"  />
                                    <asp:BoundField HeaderText="Sosho Price" DataField="grpSoshoPrice" />
                                    <asp:BoundField HeaderText="Packing Type" DataField="grpPackingType" />
                                    <%--<asp:BoundField HeaderText="Is Selected" DataField="grpisSelected" />--%>

                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:LinkButton Text="Edit" runat="server" CommandName="Edit" CausesValidation="false" />
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                            <asp:LinkButton Text="Update" runat="server" OnClick="OnUpdate" CausesValidation="false" />

                                        </EditItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-default" data-dismiss="modal">Close</button>
                        </div>
                    </div>
                </div>
            </div>
            <!-- end -->
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

                                //$("#txtGrpMrp").blur(function () {
                                //    alert("This input field has lost its focus.");
                                //});

                });

                            function openModal() {
                                $('[id*=myModal').modal('show');
                            } 


            </script>

        </section>
    </div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="Server">
</asp:Content>

