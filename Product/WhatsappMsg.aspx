<%@ Page Title="WhatsappMessage | Salebhai" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="WhatsappMsg.aspx.cs" Inherits="Product_WhatsappMsg" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>WhatsApp Message Change Page
               <small>
                   <asp:Label runat="server" ID="lblDateTime"></asp:Label>
               </small>
            </h1>
            <ol class="breadcrumb">
            </ol>
        </section>
        <section class="content">
            <!-- Small boxes (Stat box) -->
            <div class="box">
                <div class="box-body">
                    <div class="row">
                        <div class="col-md-12" style="margin: 10px">
                            <div class="col-md-3">
                                <asp:Label ID="Label1" runat="server" Text="Home Page Message Share"></asp:Label>
                                <asp:HiddenField ID="HiddenField1" runat="server" />
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="TextBox1" CssClass="form-control inline" Style="width: 344px; margin: 0px; height: 115px;" TextMode="MultiLine" runat="server"></asp:TextBox>
                                <a class="fa fa-search-plus" href="yourLink" target="popup" onclick="window.open('../WhatsappMsgEx/HomeWhatsapp.png','popup','width=600,height=600,scrollbars=no,resizable=no'); return false;">View Screen</a>
                            </div>
                        </div>
                        <div class="col-md-12" style="margin: 10px">
                            <div class="col-md-3">
                                <asp:Label ID="Label2" runat="server" Text="Order Summary Message Share"></asp:Label>
                                <asp:HiddenField ID="HiddenField2" runat="server" />
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="TextBox2" CssClass="form-control inline" TextMode="MultiLine" Style="width: 344px; margin: 0px; height: 115px;" runat="server"></asp:TextBox>
                                <a class="fa fa-search-plus"  href="yourLink" target="popup" onclick="window.open('../WhatsappMsgEx/OrderSummary.png','popup','width=600,height=600,scrollbars=no,resizable=no'); return false;">View Screen</a>
                            </div>
                        </div>
                        <div class="col-md-12" style="margin: 10px">
                            <div class="col-md-3">
                                <asp:Label ID="Label3" runat="server" Text="OrderDetail Message Share"></asp:Label>
                                <asp:HiddenField ID="HiddenField3" runat="server" />
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="TextBox3" CssClass="form-control inline" TextMode="MultiLine" Style="width: 344px; margin: 0px; height: 115px;" runat="server"></asp:TextBox>
                                <a class="fa fa-search-plus"  href="yourLink" target="popup" onclick="window.open('../WhatsappMsgEx/OrderDetail.png','popup','width=600,height=600,scrollbars=no,resizable=no'); return false;">View Screen</a>
                            </div>
                        </div>
                        <div class="col-md-12" style="margin: 10px">
                            <div class="col-md-3">
                                <asp:Label ID="Label4" runat="server" Text="Buy Message Share"></asp:Label>
                                <asp:HiddenField ID="HiddenField4" runat="server" />
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="TextBox4" CssClass="form-control inline" TextMode="MultiLine" Style="width: 344px; margin: 0px; height: 115px;" runat="server"></asp:TextBox>
                                <a class="fa fa-search-plus"  href="yourLink" target="popup" onclick="window.open('../WhatsappMsgEx/Final.png','popup','width=600,height=600,scrollbars=no,resizable=no'); return false;">View Screen</a>
                            </div>
                        </div>
                        <div class="col-md-12" style="margin: 10px;" >
                            <div class="col-md-3">
                                <asp:Label ID="Label5" runat="server" Text="Buy With 1 Message Share"></asp:Label>
                                <asp:HiddenField ID="HiddenField5" runat="server" />
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="TextBox5" CssClass="form-control inline" TextMode="MultiLine" Style="width: 344px; margin: 0px; height: 115px;" runat="server"></asp:TextBox>
                                <a class="fa fa-search-plus"  href="yourLink" target="popup" onclick="window.open('../WhatsappMsgEx/Final.png','popup','width=600,height=600,scrollbars=no,resizable=no'); return false;">View Screen</a>
                            </div>
                        </div>
                        <div class="col-md-12" style="margin: 10px;" >
                            <div class="col-md-3">
                                <asp:Label ID="Label6" runat="server" Text="Buy With 5 Message Share"></asp:Label>
                                <asp:HiddenField ID="HiddenField6" runat="server" />
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="TextBox6" CssClass="form-control inline" TextMode="MultiLine" Style="width: 344px; margin: 0px; height: 115px;" runat="server"></asp:TextBox><br />
                                <a class="fa fa-search-plus"  href="yourLink" target="popup" onclick="window.open('../WhatsappMsgEx/Final.png','popup','width=600,height=600,scrollbars=no,resizable=no'); return false;">View Screen</a>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </section>
    </div>


    <script>
        $("#ContentPlaceHolder1_TextBox1").keyup(function () {
            var msg = $("#ContentPlaceHolder1_TextBox1").val();
            var key = $("#ContentPlaceHolder1_HiddenField1").val();
            $.ajax({
                type: "POST",
                url: "WhatsappMsg.aspx/HomepageMsg",
                data: '{"Msg":"' + msg + '","Key":"' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {
                    //alert(msg);
                },
                error: function (msg) {
                    msg = "There is an error";
                    alert(msg);
                }
            });
        });
        $("#ContentPlaceHolder1_TextBox2").keyup(function () {
            var msg = $("#ContentPlaceHolder1_TextBox2").val();
            var key = $("#ContentPlaceHolder1_HiddenField2").val();
            $.ajax({
                type: "POST",
                url: "WhatsappMsg.aspx/HomepageMsg",
                data: '{"Msg":"' + msg + '","Key":"' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    //alert(msg);
                },
                error: function (msg) {
                    msg = "There is an error";
                    alert(msg);
                }
            });
        });
        $("#ContentPlaceHolder1_TextBox3").keyup (function () {
            var msg = $("#ContentPlaceHolder1_TextBox3").val();
            var key = $("#ContentPlaceHolder1_HiddenField3").val();
            $.ajax({
                type: "POST",
                url: "WhatsappMsg.aspx/HomepageMsg",
                data: '{"Msg":"' + msg + '","Key":"' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    //alert(msg);
                },
                error: function (msg) {
                    msg = "There is an error";
                    alert(msg);
                }
            });
        });
        $("#ContentPlaceHolder1_TextBox4").keyup(function () {
            var msg = $("#ContentPlaceHolder1_TextBox4").val();
            var key = $("#ContentPlaceHolder1_HiddenField4").val();
            $.ajax({
                type: "POST",
                url: "WhatsappMsg.aspx/HomepageMsg",
                data: '{"Msg":"' + msg + '","Key":"' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    //alert(msg);
                },
                error: function (msg) {
                    msg = "There is an error";
                    alert(msg);
                }
            });
        });
        $("#ContentPlaceHolder1_TextBox5").keyup(function () {
            var msg = $("#ContentPlaceHolder1_TextBox5").val();
            var key = $("#ContentPlaceHolder1_HiddenField5").val();
            $.ajax({
                type: "POST",
                url: "WhatsappMsg.aspx/HomepageMsg",
                data: '{"Msg":"' + msg + '","Key":"' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    //alert(msg);
                },
                error: function (msg) {
                    msg = "There is an error";
                    alert(msg);
                }
            });
        });
        $("#ContentPlaceHolder1_TextBox6").keyup(function () {
            var msg = $("#ContentPlaceHolder1_TextBox6").val();
            var key = $("#ContentPlaceHolder1_HiddenField6").val();
            $.ajax({
                type: "POST",
                url: "WhatsappMsg.aspx/HomepageMsg",
                data: '{"Msg":"' + msg + '","Key":"' + key + '"}',
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    //alert(msg);
                },
                error: function (msg) {
                    msg = "There is an error";
                    alert(msg);
                }
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="Server">
</asp:Content>

