<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="User_Role.aspx.cs" Inherits="RoleManagement_User_Role" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <section class="content-header">
            <h1>User Role Mapping
       
                            <small>
                                <asp:Label runat="server" ID="lblDateTime"></asp:Label></small>
            </h1>
            <ol class="breadcrumb">
                <li><a href="../Home.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">User Role</li>
            </ol>
        </section>
        <section class="content">
            <div style="margin-left: 15px">
                <div class="row" style="margin-top: 15px">
                    <div class="col-xs-1">User</div>
                    <div class="col-xs-3">
                        <asp:DropDownList ID="ddlrole" runat="server" OnSelectedIndexChanged="ddlrole_SelectedIndexChanged" AutoPostBack="True" Style="width: 250px" CssClass="form-control" />
                    </div>
                    <br />
                </div>
                <div class="row">
                    <div style="width: 60%; margin-left: 2%; margin-right: 20%;">
                        <asp:GridView ID="grd" class="table table-bordered table-hover dataTable" runat="server" AutoGenerateColumns="False" Caption="Role List" CellPadding="10" CellSpacing="5" Width="100%" >

                            <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" />
                            <Columns>
                                  
                                <asp:TemplateField HeaderText="Select">
                                      <HeaderStyle  HorizontalAlign="Center" VerticalAlign="Middle" />
                                       <HeaderTemplate>
                            <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="CheckAllEmp(this);" />
                        </HeaderTemplate>
                                   <%-- <ItemStyle HorizontalAlign="Center" VerticalAlign="Middle" />--%>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="chkRow" runat="server" />
                                        <asp:Label runat="server" Visible="false"  ID="lblid" Text='<%#Eval("Id") %>'></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:BoundField DataField="RoleName" HeaderText="Role"></asp:BoundField>

                            </Columns>
                        </asp:GridView>

                    </div>
                </div>
                <div style="margin-top: 15px; margin-left: 95px">
                    <asp:Button ID="btnsave" runat="server" class="btn btn-block btn-primary" Text="Save" Width="150px" Style="text-align: center;" OnClick="cmdSave_Click" />
                </div>
                <br />
                <div>
                    <span style="color: red">
                        <asp:Literal ID="ltrErr" runat="server"></asp:Literal>
                        <br />
                    </span>
                </div>
            </div>
        </section>
    </div>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="Server">
    <script type="text/javascript" >
        function CheckAllEmp(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById("<%=grd.ClientID %>");
            alert(GridVwHeaderChckbox.id);

            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }
    </script>
</asp:Content>

