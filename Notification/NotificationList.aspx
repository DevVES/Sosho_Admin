<%@ Page Title="Notification Scheduler Details" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="NotificationList.aspx.cs" Inherits="AppManagement_NotificationList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <h1>Notification Scheduler Details
                <small>
                    <asp:Label runat="server" ID="lblDateTime"></asp:Label></small>
            </h1>
            <ol class="breadcrumb">
                <li><a href="../Home.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Notification Scheduler Details</li>
            </ol>
        </section>

        <section class="content-header">

            <div class="alert alert-info alert-dismissible">
                <button type="button" class="close" data-dismiss="alert" aria-hidden="true">×</button>
                <h4><i class="icon fa fa-check"></i>Scheduler Called!</h4>
                <asp:Literal ID="alitlastcall" runat="server"></asp:Literal>
            </div>

        </section>

        <section class="content">

            <div class="row">
                <div class="col-xs-12">
                    <div class="box">
                        <div class="box-header with-border">
                            <div>
                                <span style="color: red; margin-left: 15px">
                                    <asp:Literal ID="ltrErr" runat="server"></asp:Literal>
                                </span>
                            </div>

                            <%--  <div class="row" style="margin-left: 1%; margin-bottom:2%">
                                <asp:DropDownList ID="ddlDateType" runat="server" class="form-control" OnSelectedIndexChanged="ddlDateType_SelectedIndexChanged" AutoPostBack="true" Width="250px"></asp:DropDownList>
                            </div>--%>


                            <div class="row  table-responsive">
                                <div style="width: 95%; margin-left: 2%; margin-right: 2%;">
                                    <asp:GridView Caption="Payment Offer List" ID="grd" CssClass="table table-bordered table-hover dataTable" runat="server" AutoGenerateColumns="False" OnRowCommand="grd_RowCommand" OnRowDataBound="grd_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="Id" ReadOnly="true" Visible="false" />

                                            <asp:BoundField HeaderText="Status" DataField="IsSend" ReadOnly="true" />

                                            <%-- <asp:TemplateField HeaderText="Message">
                                                <ItemTemplate>
                                                    <asp:Label Font-Size="18px" Font-Bold="true" runat="server" ID="lblCampaignName" Text='<%# Eval("Message") %>'></asp:Label><br />
                                                    Product Name:<asp:Label runat="server" ID="Label1" Text='<%# Eval("ProductName") %>'></asp:Label><br />
                                                    Category Name:
                                                    <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("CategoryName") %>'></asp:Label>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>

                                            <asp:TemplateField HeaderText="Configure" ItemStyle-Width="22%">
                                                <ItemTemplate>
                                                    SendTo:
                                            <asp:Label runat="server" ID="lblenddt" Text='<%# Eval("SendTo") %>'></asp:Label><br />
                                                    Notification Type:
                                            <asp:Label runat="server" ID="lblupdt" Text='<%# Eval("NotificationType") %>'></asp:Label>
                                                    <br />
                                                    Image:
                            <asp:HyperLink ID="hyperlink1" NavigateUrl='<%# Eval("View1") %>'
                                runat="server" Text='<%# Eval("View2") %>' />
                                                    <%-- <asp:Label runat="server" ID="Label3" Text='<%# Eval("View1") %>'></asp:Label>--%>
                                                </ItemTemplate>

                                                <ItemStyle Width="22%"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:HyperLinkField DataNavigateUrlFields="Id" HeaderText="Total" DataNavigateUrlFormatString="../Notification/NotificationList.aspx" DataTextField="AllMobile" />

                                            <asp:HyperLinkField DataNavigateUrlFields="Id" HeaderText="Pending" DataNavigateUrlFormatString="../Notification/NotificationList.aspx" DataTextField="Pending" />
                                            <asp:HyperLinkField DataNavigateUrlFields="Id" HeaderText="Processed" DataNavigateUrlFormatString="../Notification/NotificationList.aspx" DataTextField="Processed" />

                                            <asp:HyperLinkField DataNavigateUrlFields="Id" HeaderText="Fail" DataNavigateUrlFormatString="../Notification/NotificationList.aspx" DataTextField="Fail" />

                                            <asp:HyperLinkField DataNavigateUrlFields="Id" HeaderText="Success" DataNavigateUrlFormatString="../Notification/NotificationList.aspx" DataTextField="Success" />



                                            <asp:TemplateField HeaderText="Notification Date" ItemStyle-Width="22%">
                                                <ItemTemplate>
                                                    Create Date:<asp:Label runat="server" ID="lblstartdt" Text='<%# Eval("DateOfCreate") %>'></asp:Label><br />
                                                    Modify Date:
                                            <asp:Label runat="server" ID="lblenddt" Text='<%# Eval("DateOfMotific") %>'></asp:Label><br />
                                                    Expired Date:
                                            <asp:Label runat="server" ID="lblupdt" Text='<%# Eval("DateOfExpiredTime") %>'></asp:Label><br />
                                                    Schedule Time:
                                            <asp:Label runat="server" ID="Label2" Text='<%# Eval("DateOfScheduleTime") %>'></asp:Label>
                                                </ItemTemplate>

                                                <ItemStyle Width="22%"></ItemStyle>
                                            </asp:TemplateField>

                                            <asp:BoundField HeaderText="Send By" DataField="UserName" ReadOnly="true" />

                                        </Columns>
                                    </asp:GridView>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>
    <script type="text/javascript">
        function isNumberKey(evt) {
            var charCode = (evt.which) ? evt.which : event.keyCode
            if (charCode > 31 && (charCode < 48 || charCode > 57))
                return false;

            return true;
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="Server">
</asp:Content>

