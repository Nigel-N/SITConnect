<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="SITConnect.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>SITConnect Stationery Store</h1>
            <asp:Label ID="hellouser_lbl" runat="server"></asp:Label>
            <br />
                <h3>Credit Card Info</h3>
                <asp:TextBox ID="cname_tb" runat="server" Width="175px" Height="16px" ReadOnly="True"></asp:TextBox>
                <asp:TextBox ID="cvv_tb" runat="server" Width="106px" Height="16px" MaxLength="3" ReadOnly="True"></asp:TextBox>
                <br />
            <br />
            <br />
                <asp:TextBox ID="cardno_tb" runat="server" Width="324px" Height="16px" MaxLength="16" ReadOnly="True"></asp:TextBox>
                <br />
            <br />
                <asp:TextBox ID="expdatem_tb" runat="server" Width="39px" Height="16px" MaxLength="2" Placeholder="MM" ReadOnly="True"></asp:TextBox>
                /
                <asp:TextBox ID="expdatey_tb" runat="server" Width="85px" Height="16px" MaxLength="4" Placeholder="YYYY" ReadOnly="True"></asp:TextBox>
            <br />
            <asp:Button ID="logout_btn" runat="server" OnClick="LogoutBtn_Click" Text="Log Out" Height="28px" Width="131px" Visible="False" />
        </div>
    </form>
</body>
</html>
