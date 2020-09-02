<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="Role.aspx.cs" Inherits="RoleManagement_Role" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="content-wrapper">
        <section class="content-header">
            <h1>Add/Edit Role
       
                            <small>
                                <asp:Label runat="server" ID="lblDateTime"></asp:Label></small>
            </h1>
            <ol class="breadcrumb">
                <li><a href="../Home.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Role</li>
            </ol>
        </section>
        <section class="content">
            <div style="margin-left: 15px">
            <div class="row" style="margin-top:15px"> 
                 <div class="col-xs-1" >Role</div>
                 <div class="col-xs-3" ><asp:TextBox ID="txtrole" Width="250px" runat="server" class="form-control"></asp:TextBox></div>
            </div>
            
             <div style="margin-top:15px;margin-left:95px"><asp:Button ID="btnsave" runat="server" class="btn btn-block btn-primary" Text="Save" Width="150px" Style="text-align: center;" OnClick="cmdSave_Click" /></div><br />
                            <div>
                                    <span style="color: red">
                                        <asp:Literal ID="ltrErr" runat="server"></asp:Literal>
                                        <br />
                                    </span>
                                </div>                  
                </div>
           

            <div class="row">
                <div style="width: 60%; margin-left: 2%; margin-right: 20%;">
                    <asp:GridView ID="grd" class="table table-bordered table-hover dataTable" runat="server" AutoGenerateColumns="False" Caption="Role List" CellPadding="10" CellSpacing="5" Width="100%" >

                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                        <Columns>
                             <asp:BoundField DataField="RoleName" HeaderText="Role"></asp:BoundField>
                       
                            <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="Role.aspx?ID={0}" HeaderText="Edit" Text="Edit"></asp:HyperLinkField>
                            <asp:HyperLinkField DataNavigateUrlFields="Id" DataNavigateUrlFormatString="Role.aspx?DID={0}" HeaderText="Delete" Text="Delete"></asp:HyperLinkField>
                        </Columns>
                    </asp:GridView>
                    
                </div>
            </div>
        </section>
    </div>  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" Runat="Server">
</asp:Content>

