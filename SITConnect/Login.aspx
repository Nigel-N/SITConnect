<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SITConnect.Login" ValidateRequest="true" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login Page</title>

    <style>
        #log_form_details {
            position: fixed;
            top: 30%;
            left: 50%;
            transform: translate(-50%, -50%);
            border: 2px solid black;
            padding: 50px;
        }
 
    </style>

    <script src="https://www.google.com/recaptcha/api.js?render=6LfeWGkeAAAAAH05ohys3OUFmf6IymK2EieEiFDA" ></script>
    

</head>
<body>
    <form id="log_form" method="post" runat="server">
        <div>
            
        </div>
        
        <div id="log_form_details">
            <div id="log_form_details2">     
                <h1 class="auto-style1">
                    Log In
                    <asp:Label ID="name_lbl" runat="server" Text="" EnableViewState="false"></asp:Label> <br />
                </h1>
                    <asp:Label ID="xss_msg" runat="server" Text="" EnableViewState="false"></asp:Label> <br />
                    
                    <asp:ScriptManager ID="ScriptMangager" runat="server"></asp:ScriptManager>

                    <asp:UpdatePanel ID="UpdatePanel" runat="server" UpdateMode="Always">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="Timer" EventName="Tick" />
                        </Triggers>

                        <ContentTemplate>
                            
                            <asp:Label ID="DisplayTextMinutes" runat="server" Text=""></asp:Label>
                            <asp:Label ID="DisplayTextSeconds" runat="server" Text=""></asp:Label>

                        </ContentTemplate>
                    </asp:UpdatePanel>

                    <asp:Timer ID="Timer" runat="server" Interval="1000" OnTick="Timer_Tick" Enabled="false"></asp:Timer>

                <asp:TextBox ID="email_tb" runat="server" Width="292px" Height="16px" Placeholder="Email"></asp:TextBox>
                <br />
                <br />
                <asp:TextBox ID="password_tb" runat="server" Width="294px" TextMode="Password" Height="16px" PlaceHolder="Password"></asp:TextBox>
                <br />
                <br />
                <asp:Button ID="login_btn" runat="server" OnClick="LoginBtn_Click" Text="Log In" Height="28px" Width="303px" />
                <br />
                <br />
                <br />
                New to SITConnect store?
                <a href='Registration.aspx'>Register an account</a>
                <br />
                <br />
                <input type="hidden" id="g-recaptcha-response" name="g-recaptcha-response"/>
            </div>
        </div>
    </form>

    <script>
        grecaptcha.ready(function () {
            grecaptcha.execute('6LfeWGkeAAAAAH05ohys3OUFmf6IymK2EieEiFDA', { action: 'Login' }).then(function (token) {
                document.getElementById("g-recaptcha-response").value = token;
            });
        });
    </script>
</body>
</html>
