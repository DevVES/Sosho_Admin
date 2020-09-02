<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default3.aspx.cs" Inherits="Default3" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="http://code.jquery.com/jquery-latest.min.js"></script>
    <script type="text/javascript" src="http://www.jqueryscript.net/demo/Simple-jQuery-Based-Barcode-Generator-Barcode/jquery-barcode.js"></script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
           <div id="demo"></div>   
            <script>
                $("#demo").barcode(
"1234567890128", // Value barcode (dependent on the type of barcode)
"ean13" // type (string)
);
            </script>
        </div>
    </form>
</body>
</html>
