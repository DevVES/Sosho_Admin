<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="MessageTemplateList.aspx.cs" Inherits="MessageTemplate_MessageTemplate" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    
 <div class="content-wrapper">
        <div class="content-wrapper" style="clear: both; margin: 0px;">
            <section class="content-header">
                <h1 id="h1" runat="server">Message Template List
                </h1>
                <ol class="breadcrumb">
                    <li><a href="../Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                    <li id="l1" runat="server" class="active">Create Message Template
                    </li>
                </ol>
            </section>
            <section class="content">
                 <div class="row">
                <a href="CreateTemplete.aspx" class="btn btn-success pull-right add-padding" style="width: 20%;" target="_blank">ADD</a>
            </div>
                <asp:GridView ID="grd" OnRowDataBound="grd_RowDataBound" OnRowCommand="grd_RowCommand" runat="server" Width="95%" AutoGenerateColumns="False" class="table table-bordered table-hover" rules="all" role="grid" CellPadding="10" CellSpacing="5" AllowSorting="True" HeaderStyle-BackColor="#ede8e8" HeaderStyle-HorizontalAlign="Center" EnableViewState="False" CaptionAlign="Top">
                    <Columns>
                        <asp:BoundField AccessibleHeaderText="SrNo" DataField="SRNO" FooterText="SrNo" HeaderText="SrNo" />
                        <asp:BoundField AccessibleHeaderText="Name" DataField="Name" FooterText="Name" HeaderText="Name" />
                        <asp:BoundField AccessibleHeaderText="Subject" DataField="Subject" FooterText="Subject" HeaderText="Subject" />
                        <asp:CheckBoxField AccessibleHeaderText="IsActive" DataField="IsActive" FooterText="IsActive" HeaderText="IsActive" />
                        <%--<asp:HyperLink AccessibleHeaderText="Edit" DataTextField="Edit" DataControlField="Id" FooterText="Edit" HeaderText="Edit" NavigateUrl='<%# "~/EmailCampaign/CreateTemplete.aspx?Id=" + Eval("Id")%>' Target="_blank" />--%>
                        <asp:TemplateField HeaderText="Edit" Visible="True">
                            <ItemTemplate>
                                <asp:HyperLink runat="server" ID="HyperLink1" NavigateUrl='<%# "http://localhost:2168/MessageTemplate/CreateTemplete.aspx?id=" + Eval("ID")%>' Text='<%# Eval("Edit")%>' Target="_blank"></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>

                </asp:GridView>
            </section>
        </div>
    </div>
    <script type="text/javascript">
        $(document).ready(function () {
            $('#ContentPlaceHolder1_grd').DataTable({
                dom: 'Bfrtip',
                buttons: [
                    'excel'
                ],
                "fixedHeader": true,
                "paging": true,
                "lengthChange": true,
                "searching": true,
                "ordering": true,
                "info": true,
                "autoWidth": false,
                "order": [[1, "dec"]]
            });
        });
    </script>


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" Runat="Server">
</asp:Content>

