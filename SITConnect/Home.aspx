<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Home.aspx.cs" Inherits="SITConnect.Home" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Home page</title>

    <%--<script>
        function validate() {
            var str = document.getElementById('<%=newpassword_tb.ClientID%>').value;

            if (str.length < 12) {
                document.getElementById("pwdchecker_lbl").innerHTML = "Password length must be at least 12 characters";
                document.getElementById("pwdchecker_lbl").style.color = "Red";
                return ("too_short")
            }

            else if (str.search(/[0-9]/) == -1) {
                document.getElementById("pwdchecker_lbl").innerHTML = "Password requires at least 1 number";
                document.getElementById("pwdchecker_lbl").style.color = "Red";
                return ("no_number");
            }

            else if (str.search(/[a-z]/) == -1) {
                document.getElementById("pwdchecker_lbl").innerHTML = "Password requires at least 1 lowercase character";
                document.getElementById("pwdchecker_lbl").style.color = "Red";
                return ("no_lowercase");
            }

            else if (str.search(/[A-Z]/) == -1) {
                document.getElementById("pwdchecker_lbl").innerHTML = "Password requires at least 1 uppercase character";
                document.getElementById("pwdchecker_lbl").style.color = "Red";
                return ("no_uppercase");
            }

            else if (str.search(/[^a-zA-Z0-9]/) == -1) {
                document.getElementById("pwdchecker_lbl").innerHTML = "Password requires at least 1 special character";
                document.getElementById("pwdchecker_lbl").style.color = "Red";
                return ("no_special");
            }


            document.getElementById("pwdchecker_lbl").innerHTML = "";
        }
    </script>--%>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h1>SITConnect Stationery Store</h1>
            <asp:Label ID="hellouser_lbl" runat="server"></asp:Label>
            <br />
            <br />
            Pencil - $1<br />
            Pen - $2<br />
            File - $3<br />
            <br />
            <br />

                <%--<h3>Change Password</h3>
            <asp:Label ID="oldpwde_lbl" runat="server"></asp:Label>
            <br />
                <asp:TextBox ID="oldpassword_tb" runat="server" onkeyup="javascript:validate()" Width="322px" TextMode="Password" Height="16px" PlaceHolder="Old password" OnTextChanged="oldpassword_tb_TextChanged"></asp:TextBox>
            <br />
            <asp:Label ID="newpwde_lbl" runat="server"></asp:Label>
            <br />
                <asp:TextBox ID="newpassword_tb" runat="server" onkeyup="javascript:validate()" Width="322px" TextMode="Password" Height="16px" PlaceHolder="New password"></asp:TextBox>
            <br />
            <br />
                <asp:TextBox ID="confirmpassword_tb" runat="server" onkeyup="javascript:validate()" Width="322px" TextMode="Password" Height="16px" PlaceHolder="Confirm new password"></asp:TextBox>    
            <br />
            <br />
                <asp:Button ID="password_btn" runat="server" OnClick="PasswordBtn_Click" Text="Change password" Height="28px" Width="131px" Visible="False" />
            <br />
            <br />--%>

                <h3>Credit Card Info</h3>
                <asp:TextBox ID="cname_tb" runat="server" Width="175px" Height="16px" ReadOnly="True"></asp:TextBox>
                <asp:TextBox ID="cvv_tb" runat="server" Width="106px" Height="16px" MaxLength="3" ReadOnly="True"></asp:TextBox>
            <br />
            <br />
                <asp:TextBox ID="cardno_tb" runat="server" Width="324px" Height="16px" MaxLength="16" ReadOnly="True"></asp:TextBox>
                <br />
            <br />
                <asp:TextBox ID="expdatem_tb" runat="server" Width="39px" Height="16px" MaxLength="2" Placeholder="MM" ReadOnly="True"></asp:TextBox>
                /
                <asp:TextBox ID="expdatey_tb" runat="server" Width="85px" Height="16px" MaxLength="4" Placeholder="YYYY" ReadOnly="True"></asp:TextBox>
            <br />
            <br />

            <asp:Button ID="logout_btn" runat="server" OnClick="LogoutBtn_Click" Text="Log Out" Height="28px" Width="131px" Visible="False" />
        </div>
    </form>
</body>
</html>
