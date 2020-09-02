<%@ Page Title="" Language="C#" MasterPageFile="~/main.master" AutoEventWireup="true" CodeFile="AllocateTypeToCar.aspx.cs" Inherits="CustomerRelationship_AllocateTypeToCar" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script>
        var urlstr = "<%=WebApplication1.dbConnection.ServiceUrl%>/MotorzService.asmx";
        var urlstr1 = "../MotorzInner.asmx";
    </script>
    <div class="content-wrapper">
        <section class="content-header">
            <h1 id="lable" runat="server"></h1>
        </section>
        <section class="content">
            <div class="box box-default">
                <center>
                <div class="row">
                    <div class="col-md-12 col-sm-12 ">
                        <b style="font-size: 20px">Jobcard Type</b>
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-12 col-sm-12" id="divtype">
                    </div>
                </div>
                <br />
                <div class="row">
                    <div class="col-md-12 col-sm-12">
                        <input type="button" value="Save" id="Button2" class="btn btn-block btn-success" style="width: 90px;" onclick="changeType()" />
                    </div>
                    <script>
                        fillType();
                        function fillType() {
                            var jobcrdid = '<%=(String.IsNullOrWhiteSpace(Request.QueryString["Id"])?"0":Request.QueryString["Id"])%>';
                                $.ajax({
                                    type: "Get",
                                    url: urlstr1 + "/getJobcardTypeGate",
                                    data: { "jobcardid": jobcrdid },
                                    datatype: "json",
                                    success: function (data) {
                                        var str = "";
                                        var str = "";
                                        if (data.length > 0) {
                                            for (i = 0; i < data.length; i++) {
                                                str += "<label><input type='radio'  name='ship' value='" + data[i].Id + "' " + (data[i].Selected == "1" ? "checked" : "") + " />" + data[i].Name + "</label>";
                                            }
                                        }
                                        else {
                                            str += 'No Type Found';
                                        }
                                        $("#divtype").html(str);
                                    },
                                    failure: function (data) {
                                        alert(data.Message);
                                    }
                                });
                            }
                            function changeType() {
                                var id = $('input[name=ship]:checked').val();
                                var jobcrdid = '<%=(String.IsNullOrWhiteSpace(Request.QueryString["Id"])?"0":Request.QueryString["Id"])%>';
                                    $.ajax({
                                        type: "Get",
                                        url: urlstr1 + "/updateJobcardTypeGate",
                                        data: { "id": jobcrdid, "type": id },
                                        datatype: "json",
                                        success: function (data) {
                                            showswalsuccess("Data Has Been Save Successfully", 2000);
                                            setTimeout(function () {
                                                window.close();
                                            }, 2000)
                                        },
                                        failure: function (data) {
                                            alert(data.Message);
                                        }
                                    });
                                }
                    </script>
                </div>
                <br />
                    </center>
            </div>
        </section>
    </div>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="cphScripts" runat="Server">
</asp:Content>



