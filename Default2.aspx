<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default2.aspx.cs" Inherits="Default2" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://code.jquery.com/jquery-3.3.1.min.js"></script>
    <script src="https://cdn.jsdelivr.net/jsbarcode/3.6.0/JsBarcode.all.min.js"></script>
    <style>
        body {
            background: rgb(204,204,204);
        }

        #barcodetbl td {
            /*padding-bottom: 5px;*/
            text-align: center;
            vertical-align: bottom;
        }
        .spanb {
            font-family: verdana;
            font-size: 10px;
        }
        .svgclass {
            /*padding-bottom: 4px;*/
            padding-top: 4px;
            padding-bottom: 0px;
            width: 100%;
        }

        page {
            background: white;
            display: block;
            margin: 0 auto;
            margin-bottom: 0.5cm;
            /*box-shadow: 0 0 0.5cm rgba(0,0,0,0.5);*/
        }

            page[size="A4"] {
                width: 21cm;
                height: 29.7cm;
            }

                page[size="A4"][layout="portrait"] {
                    width: 29.7cm;
                    height: 21cm;
                }

            page[size="A3"] {
                width: 29.7cm;
                height: 42cm;
            }

                page[size="A3"][layout="portrait"] {
                    width: 42cm;
                    height: 29.7cm;
                }

            page[size="A5"] {
                width: 14.8cm;
                height: 21cm;
            }

                page[size="A5"][layout="portrait"] {
                    width: 21cm;
                    height: 14.8cm;
                }

        @media print {
            body, page {
                margin: 0;
                box-shadow: 0;
            }
        }
    </style>
</head>
<body>

    <page size="A4">
    <form id="form1" runat="server">
        <table style="width: 100%;" id="barcodetbl">
            <tr style="width: 100%;">
                <td id="tdbarcode0" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode0"></svg></td>
                <td id="tdbarcode1" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode1"></svg></td>
                <td id="tdbarcode2" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode2"></svg></td>
                <td id="tdbarcode3" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode3"></svg></td>
                <td id="tdbarcode4" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode4"></svg></td>
            </tr>
            <tr style="width: 100%;">
                <td id="tdbarcode5" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode5"></svg></td>
                <td id="tdbarcode6" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode6"></svg></td>
                <td id="tdbarcode7" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode7"></svg></td>
                <td id="tdbarcode8" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode8"></svg></td>
                <td id="tdbarcode9" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode9"></svg></td>
            </tr>
            <tr style="width: 100%;">
                <td id="tdbarcode10" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode10"></svg></td>
                <td id="tdbarcode11" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode11"></svg></td>
                <td id="tdbarcode12" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode12"></svg></td>
                <td id="tdbarcode13" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode13"></svg></td>
                <td id="tdbarcode14" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode14"></svg></td>
            </tr>
            <tr style="width: 100%;">
                <td id="tdbarcode15" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode15"></svg></td>
                <td id="tdbarcode16" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode16"></svg></td>
                <td id="tdbarcode17" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode17"></svg></td>
                <td id="tdbarcode18" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode18"></svg></td>
                <td id="tdbarcode19" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode19"></svg></td>
            </tr>
            <tr style="width: 100%;">
                <td id="tdbarcode20" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode20"></svg></td>
                <td id="tdbarcode21" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode21"></svg></td>
                <td id="tdbarcode22" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode22"></svg></td>
                <td id="tdbarcode23" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode23"></svg></td>
                <td id="tdbarcode24" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode24"></svg></td>
            </tr>
            <tr style="width: 100%;">
                <td id="tdbarcode25" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode25"></svg></td>
                <td id="tdbarcode26" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode26"></svg></td>
                <td id="tdbarcode27" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode27"></svg></td>
                <td id="tdbarcode28" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode28"></svg></td>
                <td id="tdbarcode29" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode29"></svg></td>
            </tr>
            <tr style="width: 100%;">
                <td id="tdbarcode30" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode30"></svg></td>
                <td id="tdbarcode31" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode31"></svg></td>
                <td id="tdbarcode32" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode32"></svg></td>
                <td id="tdbarcode33" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode33"></svg></td>
                <td id="tdbarcode34" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode34"></svg></td>
            </tr>
            <tr style="width: 100%;">
                <td id="tdbarcode35" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode35"></svg></td>
                <td id="tdbarcode36" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode36"></svg></td>
                <td id="tdbarcode37" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode37"></svg></td>
                <td id="tdbarcode38" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode38"></svg></td>
                <td id="tdbarcode39" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode39"></svg></td>
            </tr>
            <tr style="width: 100%;">
                <td id="tdbarcode40" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode40"></svg></td>
                <td id="tdbarcode41" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode41"></svg></td>
                <td id="tdbarcode42" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode42"></svg></td>
                <td id="tdbarcode43" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode43"></svg></td>
                <td id="tdbarcode44" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode44"></svg></td>
            </tr>
            <tr style="width: 100%;">
                <td id="tdbarcode45" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode45"></svg></td>
                <td id="tdbarcode46" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode46"></svg></td>
                <td id="tdbarcode47" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode47"></svg></td>
                <td id="tdbarcode48" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode48"></svg></td>
                <td id="tdbarcode49" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode49"></svg></td>
            </tr>
            <tr style="width: 100%;">
                <td id="tdbarcode50" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode50"></svg></td>
                <td id="tdbarcode51" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode51"></svg></td>
                <td id="tdbarcode52" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode52"></svg></td>
                <td id="tdbarcode53" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode53"></svg></td>
                <td id="tdbarcode54" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode54"></svg></td>
            </tr>
            <tr style="width: 100%;">
                <td id="tdbarcode55" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode55"></svg></td>
                <td id="tdbarcode56" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode56"></svg></td>
                <td id="tdbarcode57" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode57"></svg></td>
                <td id="tdbarcode58" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode58"></svg></td>
                <td id="tdbarcode59" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode59"></svg></td>
            </tr>
            <tr style="width: 100%;">
                <td id="tdbarcode60" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode60"></svg></td>
                <td id="tdbarcode61" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode61"></svg></td>
                <td id="tdbarcode62" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode62"></svg></td>
                <td id="tdbarcode63" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode63"></svg></td>
                <td id="tdbarcode64" style="width: 20%;height:84.39px;">
                    <svg style="width: 100%;" id="barcode64"></svg></td>
            </tr>
        </table>
        <div id="div1">
        </div>
        <script>
            var str = "";
            var startfrom = 61;
            var endfrom = 65;
            var variation = 0;
            for (i = 1; i <= 65; i++) {
                if (i >= startfrom && i <= endfrom)
                {
                    JsBarcode("#barcode" + (i - 1), 4561 + (i - 1), { width: 1.2, height: 41.4, fontSize: 13 });
                }
                else
                {
                    JsBarcode("#barcode" + (i - 1), 4561 + (i - 1), { width: 1.2, height: 41.4, fontSize: 13 });
                    $("#barcode" + (i - 1)).hide();
                    $("#tdbarcode" + (i - 1)).height(84.39 - variation);
                    $("#tdbarcode" + (i - 1)).width(157);
                }               
            }
        </script>
    </form>
    </page>

</body>
</html>
