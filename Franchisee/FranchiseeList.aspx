<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="FranchiseeList.aspx.cs" Inherits="Franchisee_FranchiseeList" %>

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
            <h1>Franchisee List
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
                <a href="AddFranchisee.aspx" class="btn btn-success pull-right add-padding" style="width: 80px">ADD</a>
            </div>
            <div class="row">
                 <div class="col-md-2 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <input id="chkisactive" name="isActive" type="checkbox" value="valactive" runat="server" checked />
                        &nbsp;
                            <asp:Label ID="lblisactive" runat="server" Text="Active" Font-Size="18px"></asp:Label>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-3 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <asp:DropDownList ID="ddlSuperFranchisee" runat="server" class="form-control" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-3 col-sm-6 col-xs-12">
                    <div class="form-group">
                        <asp:DropDownList ID="ddlMasterFranchisee" runat="server" class="form-control" AutoPostBack="true">
                        </asp:DropDownList>
                    </div>
                </div>
                
                <div class="col-md-3 col-sm-6 col-xs-12">
                    <asp:Button ID="Button2" runat="server" Text="Go" Width="70Px" CssClass="btn btn-block  btn-info" OnClick="Button1_Click" />

                </div>
            </div>

            <div style="width: 100%;" class="table-responsive">
                <asp:GridView ID="gvFranchiseelist" 
                    runat="server" Width="95%" AutoGenerateColumns="False" class="table table-bordered table-hover" rules="all"
                    role="grid" CellPadding="10" CellSpacing="5" AllowSorting="True" HeaderStyle-BackColor="#ede8e8"
                    HeaderStyle-HorizontalAlign="Center" Caption="<b><u>FRANCHISEE LIST</u></b>" CaptionAlign="Top">
                    <Columns>
                        <asp:BoundField HeaderText="Franchisee Name" DataField="FranchiseeName" />
                      <asp:BoundField HeaderText="Super Franchisee" DataField="SuperFranchiseeName" />
                        <asp:BoundField HeaderText="Master Franchisee" DataField="MasterFranchiseeName" />
                        <asp:BoundField HeaderText="Contact Number" DataField="FranchiseeContactNumber" />
                        <asp:BoundField HeaderText="EmailAddress" DataField="FranchiseeEmailAddress" />
                        <asp:BoundField HeaderText="CustomerCode" DataField="FranchiseeCustomerCode" />
                        <asp:BoundField HeaderText="ShortUrl" DataField="ShortUrl" />

                        <asp:HyperLinkField DataNavigateUrlFields="Id" ControlStyle-CssClass="red" HeaderText="EDIT" DataNavigateUrlFormatString="~/Franchisee/AddFranchisee.aspx?Id={0}" Text="Edit" />
                    </Columns>
                    <HeaderStyle HorizontalAlign="Center" BackColor="#EDE8E8"></HeaderStyle>
                </asp:GridView>
            </div>
            <!-- this is bootstrp modal popup -->
            
            <!-- end -->
            <script type="text/javascript">
                                            $(document).ready(function () {
                                                $('#ContentPlaceHolder1_gvFranchiseelist').DataTable({
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
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="Server">
</asp:Content>

