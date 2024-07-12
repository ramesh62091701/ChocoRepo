<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Assign.aspx.cs" Inherits="Aspx_Demo.Assign" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Assign Details</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label ID="lblLoDetails" runat="server" Text="LO Details"></asp:Label>
            <br />
            <asp:TextBox ID="txtUserName" runat="server" Placeholder="Enter Username"></asp:TextBox>
            <br />
            <asp:Button ID="btnAddUser" runat="server" Text="Add User" OnClick="btnAddUser_Click" />
            <asp:TextBox ID="txtUserId" runat="server" Placeholder="Enter UserId to Remove"></asp:TextBox>
            <br />
            <asp:Button ID="btnRemoveUser" runat="server" Text="Remove User" OnClick="btnRemoveUser_Click" />
        </div>
    </form>
</body>
</html>

