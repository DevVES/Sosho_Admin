<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="User.aspx.cs" Inherits="RoleManagement_User" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <h1>Add/Edit User
       
                            <small>
                                <asp:Label runat="server" ID="lblDateTime"></asp:Label></small>
            </h1>
            <ol class="breadcrumb">
                <li><a href="../Home.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">User</li>
            </ol>
        </section>
        <section class="content">
            <div class="row">
                <div class="col-xs-12">
                    <div class="box box-primary"style="overflow:hidden">
                        <div style="margin-left: 15px">
                            <div class="row" style="margin-top: 15px">
                                <div class="col-sm-2">Username</div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtusername" Width="250px" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" style="margin-top: 15px">
                                <div class="col-sm-2">Password</div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtpwd" Width="250px" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                            <div class="row" style="margin-top: 15px">
                                <div class="col-sm-2">Number</div>
                                <div class="col-sm-4">
                                    <asp:TextBox ID="txtnumber" Width="250px" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                       
         

            <div class="row">
               
                        <div style="margin-top: 15px; margin-left: 95px">
                            <asp:Button ID="btnsave" runat="server" class="btn btn-block btn-primary" Text="Save" Width="150px" Style="text-align: center;margin-left:10%" OnClick="cmdSave_Click" />
                        </div>
                        <br />
                        <div>
                            <span style="color: red">
                                <asp:Literal ID="ltrErr" runat="server"></asp:Literal>
                                <br />
                            </span>
                        </div>

                    </div>
                           </div>
                         </div>
                    </div>
            </div>

                    <div class="row table-responsive" style="overflow-x: hidden;">
                         <div class="col-xs-12">
                    <div class="box">
                        <div style="width: 98%; margin-left: 1%; overflow: auto">
                            <asp:GridView ID="grd" class="table table-bordered table-hover dataTable" runat="server" AutoGenerateColumns="False" Caption="User List" CellPadding="10" CellSpacing="5" Width="100%" AlternatingRowStyle-BackColor="#F5F5F5"
                                HeaderStyle-BackColor="#ede8e8" HeaderStyle-HorizontalAlign="Center">

                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                <Columns>
                                    <asp:BoundField DataField="UserName" HeaderText="Username"></asp:BoundField>
                                    <asp:BoundField DataField="Password" HeaderText="Password"></asp:BoundField>
                                    <asp:BoundField DataField="mobile_number" HeaderText="Number"></asp:BoundField>
                                    <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="User.aspx?ID={0}" HeaderText="Edit" Text="Edit"></asp:HyperLinkField>
                                    <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="User.aspx?DID={0}" HeaderText="Delete" Text="Delete"></asp:HyperLinkField>
                                </Columns>
                            </asp:GridView>
                            <br/>
                            <%--  <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="CollectionBoy" TypeName="clsDataSourse"></asp:ObjectDataSource>--%>
                        </div>
                    </div>
                </div>
            </div>

        </section>
    </div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="Server">
</asp:Content>

