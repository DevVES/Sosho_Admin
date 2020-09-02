<%@ Page Title="Notification | Sosho" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="SendImgNotificationScheduler.aspx.cs" Inherits="App_Management_SendImgNotificationScheduler" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <%--<script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>--%>

    <script type="text/javascript" src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>

    <link rel="stylesheet" href="https://cdn.datatables.net/fixedheader/3.1.2/css/fixedHeader.dataTables.min.css" />
    <script src="https://cdn.datatables.net/fixedheader/3.1.2/js/dataTables.fixedHeader.min.js"></script>
    <link rel="stylesheet" href="../../plugins/datepicker/datepicker3.css" />
    <link rel="stylesheet" href="../../plugins/datepicker/datepicker3.css" />
    <script src="../../plugins/datepicker/bootstrap-datepicker.js"></script>

    <link rel="stylesheet" href="../../plugins/timepicker/bootstrap-timepicker.min.css" />
    <script src="../../plugins/timepicker/bootstrap-timepicker.min.js"></script>
    <link href="https://code.jquery.com/ui/1.10.4/themes/ui-lightness/jquery-ui.css" rel="stylesheet" />

    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.5.6/js/dataTables.buttons.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.5.6/js/buttons.flash.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.5.6/js/buttons.html5.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.5.6/js/buttons.print.min.js"></script>


    <script type="text/javascript" src="https://unpkg.com/sweetalert/dist/sweetalert.min.js"></script>

    <div class="content-wrapper">
        <section class="content-header">
            <h1>Send Image Notification via Scheduler
                            <small>
                                <asp:Label runat="server" ID="lblDateTime"></asp:Label></small>
            </h1>
            <ol class="breadcrumb">
                <li><a href="../Home.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                <li class="active">Send Image Notification via Scheduler</li>
            </ol>
        </section>

        <section class="content">



            <div class="box box-primary">
                <div class="box-header">
                    <br />
                    <div class="row">
                        <div class="col-sm-3">
                            <asp:Label ID="lblsendto" Style="margin-top: 10px" class="control-label" runat="server" Text="Send To" Width="150px"></asp:Label>
                        </div>
                        <div class="col-sm-4">
                            <asp:RadioButton ID="rdbsendall" runat="server" Text=" All" Checked="true" AutoPostBack="true" OnCheckedChanged="rdbsendall_CheckedChanged" CssClass="radio-inline" GroupName="SendTo"></asp:RadioButton>

                            <asp:RadioButton ID="rdbsendsel" runat="server" CssClass="radio-inline" Text=" Selected" AutoPostBack="true" OnCheckedChanged="rdbsendsel_CheckedChanged" GroupName="SendTo"></asp:RadioButton>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-sm-3">
                            <asp:Label runat="server" ID="lblcat" Text="Notification Type"></asp:Label>
                        </div>
                        <div class="col-sm-2">
                            <asp:DropDownList class="form-control" runat="server" ID="ddlcategory" Width="200px" AutoPostBack="false">
                                <asp:ListItem Value="1">Text Notification</asp:ListItem>
                                 <asp:ListItem Value="2">Notification With Text and Image</asp:ListItem>
                                <asp:ListItem Value="3">For Update</asp:ListItem>
                            </asp:DropDownList>

                        </div>
                    </div>
                    <br />
                    <div class="row"  style="display:none">
                        <div class="col-sm-3">
                            <asp:Label runat="server" ID="lblProd" Text="Product"></asp:Label>
                        </div>
                        <div class="col-sm-2">
                            <asp:DropDownList class="form-control" runat="server" ID="ddlProduct" Width="200px"></asp:DropDownList>

                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-sm-3">
                            <asp:Label runat="server" ID="lblmob" Text="Mobile"></asp:Label>
                        </div>
                        <div class="col-sm-2">
                            <asp:TextBox TextMode="MultiLine" onkeypress="return isNumber(event);" class="form-control" runat="server" ID="txtmob" Width="200px"></asp:TextBox>
                           
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-sm-3">
                            <asp:Label runat="server" ID="lblmsg" Text="Message"></asp:Label>
                        </div>
                        <div class="col-sm-2">
                            <asp:TextBox TextMode="MultiLine" runat="server" class="form-control" ID="txtmsg" Height="80px" Width="200px"></asp:TextBox>
                        </div>
                    </div>
                    <br />
                    <div class="row">
                        <div class="col-sm-3">
                            <asp:Label runat="server" ID="lblimg" Text="Upload Image"></asp:Label>
                        </div>
                        <div class="col-sm-2">
                            <asp:FileUpload ID="FileUpload2" CssClass="upload filename" runat="server" />
                            <asp:Label ID="Label1" CssClass="message" runat="server" Text="Image size should be 550 W * 300 H" ForeColor="#FF3300" Font-Bold="True"></asp:Label>

                        </div>
                    </div>
                    <br />
                    <div class="row hidden">
                        <div class="col-sm-3">
                            <asp:Label runat="server" ID="lblisoffer" Text="Is Notification an Offer ?"></asp:Label>
                        </div>
                        <div class="col-sm-2">
                            <asp:CheckBox runat="server" ID="chkoffer" />
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-sm-3">
                            <asp:Label runat="server" ID="Label2" Text="Schedule Time" Width="250px"></asp:Label>
                        </div>
                        <div class="col-sm-3">
                            <div class="form-group">
                                <div class="input-group date">
                                    <div class="input-group-addon">
                                        <i class="fa fa-calendar"></i>
                                    </div>
                                    <input type="text" style="width: 175px" class="form-control" runat="server" id="txtSDate" />
                                </div>
                                <!-- /.input group -->
                            </div>
                            <script>
                                $('#ContentPlaceHolder1_txtSDate').datepicker({
                                    format: 'dd/M/yyyy',
                                    autoclose: true
                                });
                            </script>
                        </div>
                        <div class="col-sm-3">
                            <div class="bootstrap-timepicker">
                                <div class="input-group">
                                    <div class="input-group-addon">
                                        <i class="fa fa-fw fa-clock-o"></i>
                                    </div>
                                    <input type="text" id="txttime" class="timepicker" runat="server" />
                                </div>
                            </div>
                            <script>
                                $(".timepicker").timepicker({
                                    showInputs: false
                                });
                            </script>

                        </div>
                    </div>

                    <br />
                    <div class="row">
                        <div class="col-sm-3">
                            <span style="color: red">
                                <asp:Literal runat="server" ID="ltrerr"></asp:Literal></span>
                        </div>
                        <div class="col-sm-2">
                            <asp:Button runat="server" ID="btnsend" Text="Send Notification" class="btn btn-primary " OnClick="btnsend_Click" />
                        </div>
                    </div>
                </div>
            </div>



            <div class="row hidden" style="margin-left: 10px; margin-right: 15px">
                <div class="box box-success" style="overflow-x: auto">
                    <div class="box-header">
                        <asp:GridView ID="gvoffer" class="table table-bordered table-hover dataTable" runat="server" AutoGenerateColumns="False" Caption="Offer Notification" CellPadding="10" CellSpacing="5" Width="98%" OnRowCommand="gvoffer_RowCommand">
                            <Columns>
                                <asp:TemplateField HeaderText="Banner">
                                    <ItemTemplate>
                                        <img style="width:80px;height:75px" alt="" src='<%# Eval("ImageUrl") %>' />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Image Display" DataField="ImageDisplay" />
                                <asp:BoundField HeaderText="Status" DataField="OfferStatus" />
                                <asp:BoundField HeaderText="Message" DataField="OfferMessage" />
                                <asp:BoundField HeaderText="Start Date" DataField="StartDate" />
                                <asp:BoundField HeaderText="End Date" DataField="EndDate" />
                                <asp:TemplateField HeaderText="End Offer">
                                    <ItemTemplate>
                                        <asp:LinkButton runat="server" ID="lnkoffer" Text="End Offer" CommandName="endoffer"
                                            CommandArgument='<%# Eval("Id") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>

                            </Columns>
                        </asp:GridView>
                    </div>
                </div>
            </div>

            <%-- <div style="margin-left:10px" class="row">
                
            </div> 
            --%>
        </section>

    </div>
    <script>
        // Numbers and Comma
        function isNumber(evt) {
            var iKeyCode = (evt.which) ? evt.which : evt.keyCode
            if (iKeyCode != 44 && iKeyCode > 31 && (iKeyCode < 48 || iKeyCode > 58))
                return false;

            return true;
        }

    </script>

</asp:Content>

