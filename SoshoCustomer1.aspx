<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="SoshoCustomer1.aspx.cs" Inherits="SoshoCustomer1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <h1>Sosho customer
               <small>
                   <span id="ContentPlaceHolder1_lblDateTime"></span>
               </small>
            </h1>
            <ol class="breadcrumb">
            </ol>
        </section>
        <section class="content">
             <div class="col-md-12" style="padding: 10px">
                <div class="col-lg-3">
             </div>
                <div class="col-lg-2">
                </div>
                <div class="col-lg-4">
                    <br />
                </div>
                 <div class="col-lg-3">
                      <asp:Button ID="btnCustomer" runat="server" Text="Download" Width="85Px" CssClass="btn btn-block  btn-info" OnClick="btnCustomer_Click" />
                 </div>
            </div>
            <div class="col-lg-12 col-md-12 col-sm-12 col-xs-12">
                <div class="box box-warning">
                    <div class="box-header">
                        <h3 class="box-title">Sosho customer</h3>
                    </div>
                    <div class="box-body" runat="server" id="d">
                        <div class="table-responsive">
                            <asp:GridView ID="grd" AutoGenerateColumns="false" runat="server" AllowSorting="True" OnPageIndexChanging="grd_PageIndexChanging" CssClass="table dataTable table-hover table-bordered table-responsive" Width="100%" AllowPaging="false" OnRowDataBound="grd_RowDataBound">
                                <Columns>
                                    <%--<asp:BoundField DataField="RegisterDate" HeaderText="RegisterDate"></asp:BoundField>--%>
                                       <asp:BoundField DataField="Name" HeaderText="Name"/>
                                    <asp:BoundField DataField="Mobile" HeaderText=" MobileNumber"/>
                                    <asp:BoundField DataField="CustAddress" HeaderText="Address"/>
                                    <asp:BoundField DataField="CustPin" HeaderText="Pincode"/>
                                   <%-- <asp:BoundField DataField="IsSalebhai" HeaderText="IsSalebhai"></asp:BoundField>--%>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>

        </section>
    </div>
    <script type="text/javascript">


        $(document).ready(function () {
            $('#ContentPlaceHolder1_grd').DataTable({
                "fixedHeader": true,
                "paging": false,
                "lengthChange": false,
                "searching": true,
                "ordering": true,
                "info": true,
                "autoWidth": true,
            });
        });
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="Server">
</asp:Content>

