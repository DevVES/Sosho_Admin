<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="403.aspx.cs" Inherits="_403" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
    <link rel="stylesheet" href="../../plugins/datepicker/datepicker3.css">
    <script src="../../plugins/datepicker/bootstrap-datepicker.js"></script>



    <script>
        var strCache = "";
        //var urlstr = "http://api.motorz.co.in/MotorzService.asmx";
        var urlstr = "../MotorzInner.asmx";
        var numshow = 10;
    </script>
    <div>
        <div class="content-wrapper">
            <!-- Content Header (Page header) -->
            <section class="content-header">
                <h1>403 Access Denied.
                            <small>
                                <asp:Label runat="server" ID="lblDateTime"></asp:Label>
                            </small>
                </h1>
                <%--<script>
                    document.body.classList.add("sidebar-collapse");
                </script>--%>
                <ol class="breadcrumb">
                </ol>
            </section>
            <section class="content">
                <center>
                <div class="box">
                        <div class="error-page" >
                            <h2 class="headline text-yellow">403
                                    <div class="error-content">
                                        <h2 style="color:black;"><i class="fa fa-fw fa-lock"></i>Oops! Access Denied..</h2>
                                        <p>
                                            <h4 style="color:black;">You are not authorized to access this page.</h4>
                                        </p>
                                    </div>
                        </h2>
                                
                                <!-- /.error-content -->
                    </div>
                </div>
        
                </center>
            </section>
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="Server">
</asp:Content>









