<%@ Page Title="Login" Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FYP.Project.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <link rel="shortcut icon" type="image/x-icon" href="../Image/company_logo.ico" />
    <title></title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.13.0/css/all.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.0/dist/js/bootstrap.bundle.min.js"></script>

    <link rel="stylesheet" href="../Project/style.css" />
    <style type="text/css">
        body {
            height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            background-color: rgb(236,239,241);
        }
    </style>
</head>
<body>

    <form id="form1" runat="server">

        <div class="login">

            <div class="image ">
                <img class="loginImg" src="../Image/loginIcon.png" alt="Login Icon" height="100" width="100" />
            </div>
            <h1 class="mb-4" align="center">Login</h1>
            <div class="form-group">
                <label class="form-label">Email Address</label>
                <span style="color: #FF0000">*</span>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Invalid Email" ControlToValidate="txtEmail" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Display="Dynamic"></asp:RegularExpressionValidator>
                <asp:TextBox ID="txtEmail" runat="server" class="form-control" placeholder="Email Address*" />
            </div>
            <div class="form-group">
                <label class="form-label">Password</label>
                <span style="color: #FF0000">*</span>
                <div class="input-group mb-3">
                    <asp:TextBox ID="txtPassword" runat="server" class="form-control col-sm-3" placeholder="Password*" />
                    <span class="input-group-text"><i class="far fa-eye-slash" id="togglePassword" style="cursor: pointer"></i></span>
                </div>
            </div>
            <div class="mt-2">
                <label class="checkbox-wrap checkbox-primary alignleft">
                    Remember Me
                        <asp:CheckBox ID="chkbxRememberMe" runat="server" />
                    <span class="checkmark"></span>
                </label>
                <div class="alignright">
                    <%--<a href="#">Forgot Password</a>--%>
                    <button type='button' class="forgetPwLink" id="btnForgetPw">Forgot Password</button>
                </div>
                <div style="clear: both;"></div>
            </div>
            <asp:Button ID="btnLogin" runat="server" Text="Login" class="form-control btn btn-primary rounded submit px-3 mt-3" OnClick="btnLogin_Click" CausesValidation="False" />
            <%--<asp:ValidationSummary ID="ValidationSummary1" runat="server" class="erroMsg"/>--%>
            <asp:Label ID="lblErrorMsg" runat="server" Text="Label" class="errorMsg" Visible="False"></asp:Label>
        </div>

        <div id="forgetPwPopup" class="forgetPwModal">
            <!-- Modal content -->
            <div class="forgetPwModal-content" style="width: 40%;">
                <div class="modal-header" style="padding: 1rem 1rem; border-bottom: 1px solid #dee2e6;">
                    <h5 class="pt-2">Forgot Password?</h5>
                    <span class="forgetPwClose pb-1">&times;</span>
                </div>
                <div class="modal-body" style="padding: 1rem;">
                    <p style="font-size: 14px;">Enter your email address and get a recovery code to reset your password.</p>
                    <label class="form-label">Email Address</label>
                    <span style="color: #FF0000">*</span>
                    <asp:TextBox ID="txtEmailPopUp" runat="server" class="form-control" placeholder="Email Address*" />
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="Please enter valid email*" ControlToValidate="txtEmailPopUp" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*" Font-Size="14px">&nbsp</asp:RegularExpressionValidator>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Please enter valid email*" ControlToValidate="txtEmailPopUp" ForeColor="Red" Font-Size="14px">&nbsp</asp:RequiredFieldValidator>
                    <span id="lblEmailNotExist" style="color: red; font-size: 21px; margin-left: -0.21rem;">&#x2022 <span style="font-size: 14px; margin-left: 0.29rem;">The entered email address does not exist*</span></span>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ForeColor="Red" Font-Size="14px" />
                </div>
                <div class="modal-footer" style="padding: .75rem; border-top: 1px solid #dee2e6; border-bottom-right-radius: calc(.3rem - 1px); border-bottom-left-radius: calc(.3rem - 1px);">
                    <asp:Button ID="btnSubmitPopUp" runat="server" Text="Submit" class="form-control btn btn-primary rounded submit px-3 mt-3" OnClick="btnSubmit_Click" />                    
                </div>

            </div>
        </div>

        <asp:Label ID="lblStoreResetEmail" runat="server" Text="0" Width="0" Height="0"></asp:Label>

        <script type="text/javascript">

            // Get the modal
            var modal = document.getElementById("forgetPwPopup");

            // Get the button that opens the modal
            var btn = document.getElementById("btnForgetPw");

            // Get the <span> element that closes the modal
            var span = document.getElementsByClassName("forgetPwClose")[0];

            // When the user clicks the button, open the modal 
            btn.onclick = function () {
                document.getElementById("txtEmailPopUp").value = "";
                modal.style.display = "block";
                document.getElementById('<%= lblStoreResetEmail.ClientID %>').innerText = "0";
            }

            // When the user clicks on <span> (x), close the modal
            span.onclick = function () {
                modal.style.display = "none";
                document.getElementById('<%= lblStoreResetEmail.ClientID %>').innerText = "0";
            }

            // When the user clicks anywhere outside of the modal, close it
            window.onclick = function (event) {
                if (event.target == modal) {
                    modal.style.display = "none";
                    document.getElementById('<%= lblStoreResetEmail.ClientID %>').innerText = "0";
                }
            }

            document.getElementById('<%= lblStoreResetEmail.ClientID %>').style.display = "none";
            var x = null;
            x = document.getElementById('<%= lblStoreResetEmail.ClientID %>').innerText
            document.getElementById("lblEmailNotExist").style.visibility = "hidden";
            if (x === "1") {
                modal.style.display = "block";
                document.getElementById("lblEmailNotExist").style.visibility = "visible";
                document.getElementById('<%= lblStoreResetEmail.ClientID %>').style.display = "none";
            }


            const togglePassword = document.querySelector("#togglePassword");
            const password = document.querySelector("#txtPassword");

            togglePassword.addEventListener("click", function () {

                // toggle the type attribute
                const type = password.getAttribute("type") === "password" ? "text" : "password";
                password.setAttribute("type", type);
                // toggle the eye icon
                this.classList.toggle('fa-eye');
                this.classList.toggle('fa-eye-slash');
            });

        </script>
    </form>
</body>
</html>
