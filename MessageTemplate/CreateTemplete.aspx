<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="CreateTemplete.aspx.cs" Inherits="MessageTemplate_CreateTemplete" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <link rel="stylesheet" href="../../plugins/datepicker/datepicker3.css" />
    <link rel="stylesheet" href="../../plugins/timepicker/bootstrap-timepicker.min.css" />
    <script src="../../plugins/datepicker/bootstrap-datepicker.js"></script>
    <script src="../../plugins/timepicker/bootstrap-timepicker.min.js"></script>


    <script src="https://cdn.datatables.net/fixedheader/3.1.2/js/dataTables.fixedHeader.min.js"></script>
    <link rel="stylesheet" href="https://cdn.datatables.net/fixedheader/3.1.2/css/fixedHeader.dataTables.min.css" />
    <link href="https://code.jquery.com/ui/1.10.4/themes/ui-lightness/jquery-ui.css" rel="stylesheet" />

    <script src="https://cdn.datatables.net/1.10.19/js/jquery.dataTables.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.5.6/js/dataTables.buttons.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.5.6/js/buttons.flash.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jszip/3.1.3/jszip.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/pdfmake.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/pdfmake/0.1.53/vfs_fonts.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.5.6/js/buttons.html5.min.js"></script>
    <script src="https://cdn.datatables.net/buttons/1.5.6/js/buttons.print.min.js"></script>

     <%@ Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor" %>
    <%-- <script src="https://cdn.ckeditor.com/4.5.7/standard/ckeditor.js"></script>--%>
    <link rel="stylesheet" href="../../plugins/select2/select2.min.css" />

    <script src="//cdn.ckeditor.com/4.10.1/standard/ckeditor.js"></script>
    
    <style>
         .select2-container .select2-selection--single {
            height: 34px;
        }

        .bottom {
            margin-bottom: 10px;
        }

        .main-div {
            margin: 2% 0px;
            padding: 20px;
            padding-right: 0px;
            padding-left: 0px;
        }

        .comment {
            width: 100%;
        }

        .res-12 {
            padding-left: 0px;
            padding-right: 0px;
            margin-bottom: 3%;
        }
    </style>
    <div class="content-wrapper">
        <div class="content-wrapper" style="clear: both; margin: 0px;">
            <section class="content-header">
                <h1 id="h1" runat="server">Create Message Template(<a href="MessageTemplateList.aspx">Back to List</a>)<small><asp:Label runat="server" ID="lblDateTime"></asp:Label></small>
                </h1>
                <ol class="breadcrumb">
                    <li><a href="../Dashboard.aspx"><i class="fa fa-dashboard"></i>Home</a></li>
                    <li id="l1" runat="server" class="active">Create Message Template
                    </li>
                </ol>
            </section>
            <section class="content">
                <div class="row">
                    <asp:HiddenField ID="hftime" runat="server" />
                    <div class="col-md-12 main-div">
                        <div class="row">
                            <div class="col-md-12 bottom">
                                <div class="col-md-12 res-12">
                                    <div class="col-md-2">Allowed Token</div>
                                    <div class="col-md-10">
                                        <asp:Label ID="lblTokenlist" runat="server"></asp:Label>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="row">
                            
                        <div class="col-md-12 bottom">
                            <div class="col-md-6">
                                <div class="col-md-12 res-12">
                                    <div class="col-md-4">Template Name</div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtname" class="form-control" runat="server"></asp:TextBox> 
                                         <asp:Label ID="lblTempleteID" Visible="false" class="form-control" runat="server"></asp:Label> 
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 bottom">
                            <div class="col-md-6">
                                <div class="col-md-12 res-12">
                                    <div class="col-md-4">
                                        IsActive
                                    </div>
                                    <div class="col-md-8 radio-button">
                                        <%-- <input id="Checkbox1" onchange="valueChanged()" type="checkbox" name="isSchedualCheck" />--%>
                                        <asp:CheckBox ID="chkisActive" name="isActive" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>
                       
                        <div class="col-md-12 bottom">
                            <div class="col-md-6">
                                <div class="col-md-12 res-12">
                                    <div class="col-md-4">Subject</div>
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtSubject" class="form-control" runat="server"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 bottom">
                            <div class="col-md-12">
                                <div class="col-md-12 res-12">
                                    <div class="col-md-2">Body</div>
                                    <div class="col-md-10"> 
                                        <CKEditor:CKEditorControl class="form-control" ID="txtFullDescription" runat="server">
                                    </CKEditor:CKEditorControl>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12 bottom">
                            <div class="col-md-12">
                                <div class="col-md-6">
                                    <div class="col-md-12 res-12">
                                        <div class="col-md-4">Select Email Account</div>
                                        <div class="col-md-8">
                                            <select class="form-control" style="width: 100%;" runat="server" id="cmbMailAccount">
                                            </select>
                                        </div>
                                    </div>

                                </div>
                            </div>
                        </div>
                        <%--<div class="col-md-12 bottom">
                            <div class="col-md-6">
                                <div class="col-md-12 res-12">
                                    <div class="col-md-4">
                                        Attached static file:	
                                    </div>
                                    <div class="col-md-8 radio-button">
                                        <asp:CheckBox ID="chkHasAttachment" name="isActive" runat="server" />
                                        <asp:FileUpload class="form-control" ID="FileUpload1" runat="server" />
                                    </div>
                                </div>
                            </div>
                        </div>--%>
                        <div class="col-md-6">
                            <div class="col-md-12 res-12">
                                <div class="col-md-4">
                                    <asp:Button ID="Button2" class="btn btn-block btn-warning" runat="server" Text="ReSet" OnClick="Button2_Click"></asp:Button>
                                </div>
                                <div class="col-md-4">
                                    <asp:Button ID="Button3" class="btn btn-block btn-success" runat="server" Text="Save" OnClick="Button3_Click"></asp:Button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                    </div>
            </section>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" Runat="Server">
</asp:Content>

