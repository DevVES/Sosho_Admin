<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="AddPages.aspx.cs" Inherits="RoleManagement_AddPages" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
      <div class="content-wrapper">
        <section class="content">
            <div class="row">
                <div class="col-md-12">
                    <div class="box">
                        <div class="box-header with-border">
                            <h3 class="box-title">Add Pages
       
                            <small>
                                <asp:Label runat="server" ID="lblDateTime"></asp:Label></small>
                            </h3>
                        </div>
                      
                            <%--<div class="row table-responsive" style="margin: 20px">--%>
                               <div class="row" style="margin: 20px">
                                   <div class="col-xs-2">
                                       <asp:Label runat="server" text="Page Name" ID="lblname"></asp:Label>
                                   </div>
                                   <div class="col-xs-6">
                                       <asp:TextBox runat="server" ID="txtname" Width="400px"></asp:TextBox>
                                   </div>
                                   </div> 
                         <div class="row" style="margin: 20px">
                                   <div class="col-xs-2">
                                       <asp:Label runat="server" text="Page URL" ID="lblurl"></asp:Label>
                                   </div>
                                   <div class="col-xs-6">
                                       <asp:TextBox runat="server" ID="txturl" Width="400px"></asp:TextBox>
                                   </div>
                                   </div> 
                                     <div class="row" style="margin: 20px">
                                   <div class="col-xs-4">
                                       <asp:Button runat="server" OnClick="btnsave_Click" CssClass="btn btn-primary" Text="Save" Width="100px" ID="btnsave"></asp:Button>
                                   </div>
                               </div>
                           <%-- </div>--%>
                       
                        <div>
                            <span style="color: red;margin-left:10px">
                                <asp:Literal runat="server" ID="ltrerr"></asp:Literal>
                            </span>
                        </div>
                        
                         <div class="row table-responsive">
                                <div style="width: 96%; margin-left: 2%; margin-right: 20%;">

                                    <asp:GridView ID="GridView1" class="table table-bordered table-hover dataTable" runat="server" AutoGenerateColumns="False" CellPadding="10" CellSpacing="5" Width="96%" EnableViewState="false">

                                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                                        <Columns>
                                            <asp:BoundField DataField="Id" HeaderText="Id" Visible="false" ></asp:BoundField>
                                            <asp:BoundField DataField="Name" HeaderText="Page Name" />
                                            <asp:BoundField DataField="PageUrl" HeaderText="Page URL" />   
                                             <asp:BoundField DataField="DOC" HeaderText="Created Date" />                                            
                                            <asp:HyperLinkField HeaderText="Edit" Text="Edit" DataNavigateUrlFormatString ="~/RoleManagement/AddPages.aspx?ID={0}" DataNavigateUrlFields="Id" />
                                            <asp:HyperLinkField HeaderText="Delete" Text="Delete" DataNavigateUrlFields="ID" DataNavigateUrlFormatString="~/RoleManagement/AddPages.aspx?DID={0}" />
                                        </Columns>
                                    </asp:GridView>

                                </div>
                           </div>
                       
                    </div>
                </div>
            </div>

        </section>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" Runat="Server">
</asp:Content>

