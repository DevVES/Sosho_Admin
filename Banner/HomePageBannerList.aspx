<%@ Page Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="HomePageBannerList.aspx.cs" Inherits="Banner_HomePageBannerList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
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
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <link rel="stylesheet" href="../../plugins/datepicker/datepicker3.css">
    <script src="../../plugins/datepicker/bootstrap-datepicker.js"></script>
    <!-- Content Wrapper. Contains page content -->
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>HomePage Banner List
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
                <div class="col-md-12">
                    <div class="col-md-3 col-sm-6 col-xs-12">
                        <div class="form-group">
                            <asp:Label runat="server" ID="Startdate"></asp:Label>
                            <asp:TextBox ID="txtdt" CssClass="form-control" placeholder="Select Date" runat="server"></asp:TextBox>
                            <script>
                                $('#ContentPlaceHolder1_txtdt').datepicker({
                                    format: 'dd/M/yyyy',
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
                                    format: 'dd/M/yyyy',
                                    autoclose: true
                                });
                            </script>
                            <!-- /.input group -->
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-6 col-xs-12">

                        <div class="col-md-12">
                            <asp:DropDownList ID="ddlBannerType" runat="server" class="form-control">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-md-3 col-sm-6 col-xs-12">

                        <asp:Button ID="Button2" runat="server" Text="Go" Width="70Px" CssClass="btn btn-block  btn-info" OnClick="Button1_Click" />

                    </div>
                    <div class="col-md-6 col-sm-6 col-xs-12">
                        <a href="UploadBanner.aspx" class="btn btn-success pull-right add-padding" style="width: 50px; margin: 20px" target="_blank">Add</a>
                    </div>
                </div>
            </div>

            <div style="width: 100%;" class="table-responsive">
                <asp:GridView ID="gvbannerlist" OnRowDataBound="gvbannerlist_RowDataBound" OnRowCommand="gvbannerlist_RowCommand" runat="server" Width="95%" AutoGenerateColumns="False" class="table table-bordered table-hover" rules="all" role="grid" CellPadding="10" CellSpacing="5" AllowSorting="True" HeaderStyle-BackColor="#ede8e8" HeaderStyle-HorizontalAlign="Center" EnableViewState="False" Caption="<b><u>HOMEPAGE BANNER LIST</u></b>" CaptionAlign="Top">
                    <Columns>
                        <%-- <asp:TemplateField HeaderText="OrderId" Visible="True">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="HyperLink2" NavigateUrl='<%# "~/admin/SubOrderDetails.aspx?OrderId=" + Eval("OrderId")%>' Text='<%# Eval("OrderId")%>' Target="_blank"></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:BoundField HeaderText="Type" DataField="Type" />
                        <asp:BoundField HeaderText="Title" DataField="Title" />
                        <asp:BoundField HeaderText="Link" DataField="Link" />
                        <asp:BoundField HeaderText="ImageName" DataField="ImageName" />
                        <asp:BoundField HeaderText="IsActive" DataField="IsActive" />
                        <asp:BoundField HeaderText="StartDate" DataField="StartDate" />
                        <asp:BoundField HeaderText="EndDate" DataField="EndDate" />


                        <%--<asp:HyperLinkField DataNavigateUrlFields="Id" ControlStyle-CssClass="red" HeaderText="EDIT" Target="_blank" DataNavigateUrlFormatString="~/Banner/UploadHomepageBanner.aspx?Id={0}&amp;TypeId={1}" Text="Edit" />--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:HyperLink ID="lnkEdit"
                                    NavigateUrl='<%# String.Format("~/Banner/UploadBanner.aspx?Id={0}&TypeId={1}", Eval("Id"), Eval("TypeId")) %>'
                                    Text="EDIT" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                    <HeaderStyle HorizontalAlign="Center" BackColor="#EDE8E8"></HeaderStyle>
                </asp:GridView>
            </div>
            <script type="text/javascript">
                                $(document).ready(function () {
                                    $('#ContentPlaceHolder1_gvbannerlist').DataTable({
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
                                });
            </script>

        </section>
    </div>
</asp:Content>
