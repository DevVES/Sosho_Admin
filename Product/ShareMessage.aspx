<%@ Page Title="ShareMessage | Salebhai" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="ShareMessage.aspx.cs" Inherits="Product_ShareMessage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="content-wrapper">
        <!-- Content Header (Page header) -->
        <section class="content-header">
            <h1>Share Social Media Message
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
                                <asp:Label ID="Label1" runat="server" Text="WhatsApp Message Share"></asp:Label>
                                <asp:HiddenField ID="HiddenField1" runat="server" />
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="TextBox1" CssClass="form-control inline" Style="width: 344px; margin: 0px; height: 115px;" TextMode="MultiLine" runat="server"></asp:TextBox>
                                <%--<a class="fa fa-search-plus" href="yourLink" target="popup" onclick="window.open('../WhatsappMsgEx/HomeWhatsapp.png','popup','width=600,height=600,scrollbars=no,resizable=no'); return false;">View Screen</a>--%>
                            </div>
                        </div>
                        <div class="col-md-12" style="margin: 10px">
                            <div class="col-md-3">
                                <asp:Label ID="Label2" runat="server" Text="Facebook Message Share"></asp:Label>
                                <asp:HiddenField ID="HiddenField2" runat="server" />
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="TextBox2" CssClass="form-control inline" TextMode="MultiLine" Style="width: 344px; margin: 0px; height: 115px;" runat="server"></asp:TextBox>
                                <%--<a class="fa fa-search-plus"  href="yourLink" target="popup" onclick="window.open('../WhatsappMsgEx/OrderSummary.png','popup','width=600,height=600,scrollbars=no,resizable=no'); return false;">View Screen</a>--%>
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
                url: "ShareMessage.aspx/HomepageMsg",
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
                url: "ShareMessage.aspx/HomepageMsg",
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

