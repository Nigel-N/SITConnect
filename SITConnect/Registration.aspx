<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Registration.aspx.cs" Inherits="SITConnect.Registration" ValidateRequest="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Registration Page</title>

    <style>
        #lname_lbl {
            margin-left : 111px;
        }
        #lname_tb {
            margin-left : 20px;
        }
        #strchecker_lbl {
            margin-left :120px;
        }

        #cvv_lbl {
            margin-left : 105px;
        }
        #cvv_tb {
            margin-left : 33px;
        }

        #reg_form_details {
            position: fixed;
            top: 40%;
            left: 50%;
            transform: translate(-50%, -50%);
            border: 2px solid black;
            padding: 50px;
        }

    </style>

    <script>
        function validate() {
            var str = document.getElementById('<%=password_tb.ClientID%>').value;

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
    </script>
</head>
<body>
<form id="reg_form" method="post" runat="server">
        <div id="reg_form_details">
            <div id="reg_form_details2">
                <h1>Account Registration</h1>
                <h3>Personal Info</h3>
                <asp:Label ID="fname_lbl" runat="server" Text="First Name"></asp:Label>
                <asp:Label ID="lname_lbl" runat="server" Text="Last Name"></asp:Label>
                <br />
                <asp:TextBox ID="fname_tb" runat="server" Width="150px" Height="16px"></asp:TextBox>
                <asp:TextBox ID="lname_tb" runat="server" Width="150px" Height="16px"></asp:TextBox>
                <br />
                <asp:Label ID="namee_lbl" runat="server"></asp:Label>
                <br />
                <br />
                <asp:Label ID="email_lbl" runat="server" Text="Email "></asp:Label>
                <br />
                <asp:TextBox ID="email_tb" runat="server" Width="326px" Height="16px"></asp:TextBox>
                <br />
                <asp:Label ID="emaile_lbl" runat="server"></asp:Label>
                <br />
                <br />
                <asp:Label ID="dob_lbl" runat="server" Text="Date of Birth"></asp:Label>
                <br />
                <asp:TextBox ID="dob_tb" runat="server" Width="324px" TextMode="Date" Height="16px" MaxLength="8"></asp:TextBox>
                <br />
                <asp:Label ID="dobe_lbl" runat="server"></asp:Label>
                <br />
                <br />
                <asp:Label ID="password_lbl" runat="server" Text="Password"></asp:Label>
                <asp:Label ID="strchecker_lbl" runat="server">Strength : Very Weak</asp:Label>
                <br />
                <asp:TextBox ID="password_tb" runat="server" onkeyup="javascript:validate()" Width="322px" TextMode="Password" Height="16px"></asp:TextBox>
                <br />
                <asp:Label ID="pwdchecker_lbl" runat="server"></asp:Label>
                <br />
                <asp:Button ID="pwdstrength_btn" runat="server" OnClick="pwdstrength_btn_click" Text="Check Strength" type="button" />
                <br />
                <h3>Credit Card Info</h3>
                <br />
                <asp:Label ID="cname_lbl" runat="server" Text="Cardholder Name"></asp:Label>
                <asp:Label ID="cvv_lbl" runat="server" Text="CVV"></asp:Label>
                <br />
                <asp:TextBox ID="cname_tb" runat="server" Width="175px" Height="16px"></asp:TextBox>
                <asp:TextBox ID="cvv_tb" runat="server" Width="106px" Height="16px" MaxLength="3"></asp:TextBox>
                <br />
                <asp:Label ID="cnamee_lbl" runat="server"></asp:Label>
                <asp:Label ID="cvve_lbl" runat="server"></asp:Label>
                <br />
                <br />
                <asp:Label ID="cardno_lbl" runat="server" Text="Card Number"></asp:Label>
                <br />
                <asp:TextBox ID="cardno_tb" runat="server" Width="324px" Height="16px" MaxLength="16"></asp:TextBox>
                <br />
                <asp:Label ID="cardnoe_lbl" runat="server"></asp:Label>
                <br />
                <br />
                <asp:Label ID="expdate_lbl" runat="server" Text="Expiration Date"></asp:Label>
                <br />
                <asp:TextBox ID="expdatem_tb" runat="server" Width="39px" Height="16px" MaxLength="2" Placeholder="MM"></asp:TextBox>/
                <asp:TextBox ID="expdatey_tb" runat="server" Width="39px" Height="16px" MaxLength="2" Placeholder="YY"></asp:TextBox>
                <br />
                <asp:Label ID="expdatee_lbl" runat="server"></asp:Label>
                <br />
                <br />
                <asp:Button ID="register_btn" runat="server" OnClick="RegisterBtn_Click" Text="Register" Height="28px" Width="432px" />
                <br />
                <br />
                Already have an account?
                <a href='Login.aspx'>Login to account</a>
            </div>
        </div>
    </form>
</body>
</html>
