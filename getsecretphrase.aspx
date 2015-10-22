<%@ Page Language="C#" AutoEventWireup="true" CodeFile="getsecretphrase.aspx.cs" Inherits="Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.1.3/jquery.min.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <h1>Hola!</h1>
        <asp:Label ID="lblGuid" runat="server"></asp:Label>
        <br />
        Algorithm: <asp:Label ID="lblAlgorithm" runat="server"></asp:Label>
        <br />
        Words: <asp:Label ID="lblWords" runat="server"></asp:Label>
        <br />
        Fibonacci Number: <asp:Label ID="lblFibonacciNumber" runat="server"></asp:Label>
        <br />
        Ordered Words: <asp:Label ID="lblOrderedWords" runat="server"></asp:Label>
        <br />
        New Words: <asp:Label ID="lblNewWords" runat="server"></asp:Label>
        <br />
        Concatenated Words: <asp:Label ID="lblConcatenatedWords" runat="server"></asp:Label>
        <br />
        Encoded String: <asp:Label ID="lblEncodedString" runat="server"></asp:Label>
        <br /><br />
        Success count:<asp:Label ID="lblSuccessCount" runat="server" Text="0"></asp:Label>
        <br />
        CrashAndBurn count:<asp:Label ID="lblCrashAndBurnCount" runat="server" Text="0"></asp:Label>
        <br />
        Winner count:<asp:Label ID="lblWinnerCount" runat="server" Text="0"></asp:Label>
        <br />
    </div>
    </form>
</body>
</html>
