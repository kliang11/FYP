<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="FYP.Project.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.0/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="style.css" />
    <style type="text/css">
        body {
            height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
            background-color: rgb(248,249,253);
        }
    </style>
</head>
<body>

    <form id="form2" runat="server">

        <%--<form id="form1" runat="server">--%>

        <%--        <nav class="navbar navbar-expand d-flex flex-column align-item-start bg-light" id="sidebar">
            <div class="container-fluid">
                <ul class="navbar-nav d-flex flex-column mt-5 w-100">
                    <li class="nav-item">
                        <a class="nav-link" href="#">Link 1</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#">Link 2</a>
                    </li>
                    <li class="nav-item">
                        <a class="nav-link" href="#">Link 3</a>
                    </li>
                </ul>
            </div>
        </nav>

        <h1 style="margin-left:15%">ABCDEFG</h1>
        <h1 style="margin-left:15%">asd</h1>--%>

        <%--        <section class="ftco-section">
            <div class="container">
                <div class="row justify-content-center">
                    <div class="col-md-7 col-lg-5">
                        <div class="login-wrap p-4 p-md-5">
                            <div class="icon d-flex align-items-center justify-content-center">
                                <span class="fa fa-user-o"></span>
                            </div>
                            <h3 class="text-center mb-4">Sign In</h3>
                            <table style="width: 100%;" class="login-form">
                                <tr>
                                    <div class="form-group">
                                        <asp:TextBox ID="TextBox1" runat="server" class="form-control rounded-left" placeholder="Username"></asp:TextBox>
                                    </div>
                                </tr>
                                <tr>
                                    <div class="form-group d-flex">
                                        <asp:TextBox ID="TextBox2" runat="server" class="form-control rounded-left" placeholder="Password" TextMode="Password"></asp:TextBox>
                                    </div>
                                </tr>
                                <tr>
                                    <div class="form-group">
                                        <asp:Button ID="Button1" runat="server" Text="Login" class="form-control btn btn-primary rounded submit px-3" />
                                    </div>
                                </tr>
                                <tr>
                                    <div class="form-group d-md-flex">
                                        <div class="w-50">
                                            <label class="checkbox-wrap checkbox-primary">
                                                Remember Me                                            
                                            <asp:CheckBox ID="CheckBox1" runat="server" />
                                                <span class="checkmark"></span>
                                            </label>
                                        </div>
                                        <div class="w-50 text-md-right">
                                            <a href="#">Forgot Password</a>
                                        </div>
                                    </div>
                                </tr>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        </section>--%>

        <%--<div>
            <asp:TextBox ID="TextBox1" runat="server" class="form-control rounded-left" placeholder="Username"></asp:TextBox>
        </div>--%>

        <%-- <section>

            <div class="container">

                <div class="row justify-content-center">
                    <table class="MyTable">
                        <tr>
                            <td></td>
                        </tr>
                        <tr>
                            <td>
                                <img src="../Image/loginIcon.png" alt="Login Icon" height="100" width="100" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <h3 class="text-center mb-4">Sign In</h3>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtUsername" runat="server" class="form-control" placeholder="Username" /></td>
                        </tr>
                        <tr>
                            <td class="auto-style1" />
                        </tr>
                        <tr>
                            <td>
                                <asp:TextBox ID="txtPassword" runat="server" class="form-control col-sm-3" placeholder="Password" TextMode="Password" /></td>
                        </tr>
                        <tr>
                            <td class="auto-style1" />
                        </tr>
                        <tr>
                            <td>
                                <asp:Button ID="btnLogin" runat="server" Text="Login" class="form-control btn btn-primary rounded submit px-3" /></td>
                        </tr>
                        <tr>
                            <td class="auto-style1" />
                        </tr>
                        <tr>
                            <td class="row">
                                <label class="checkbox-wrap checkbox-primary col-sm-6">
                                    Remember Me
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                                    <span class="checkmark"></span>
                                </label>
                                <div class="w-50 text-md col-sm-3" align="right">
                                    <a href="#">Forgot Password</a>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td></td>
                        </tr>
                    </table>
                </div>
            </div>
            <div class="center">

            </div>
        </section>--%>
        <%--</form>--%>
        <div class="login">

            <div class="image ">
                <img src="../Image/loginIcon.png" alt="Login Icon" height="100" width="100" />
            </div>
            <h1 class="mb-5" align="center">Login</h1>
            <div class="form-group">
                <label class="form-label">Email Address</label>
                <span style="color: #FF0000">*</span>
                <asp:TextBox ID="txtEmail" runat="server" class="form-control" placeholder="Email Address*" />
            </div>
            <div class="form-group">
                <label class="form-label">Password</label>
                <span style="color: #FF0000">*</span>
                <asp:TextBox ID="txtPassword" runat="server" class="form-control col-sm-3" placeholder="Password*" TextMode="Password" />
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
            <asp:Button ID="btnLogin" runat="server" Text="Login" class="form-control btn btn-primary rounded submit px-3 mt-3" OnClick="btnLogin_Click" />
            <%--<asp:ValidationSummary ID="ValidationSummary1" runat="server" class="erroMsg"/>--%>
            <asp:Label ID="lblErrorMsg" runat="server" Text="Label" class="erroMsg" Visible="False"></asp:Label>
        </div>

        <div id="resetPwPopup" class="forgetPwModal">
            <!-- Modal content -->
            <div class="modal-content">
                <span class="forgetPwClose">&times;</span>
                <p>Some text in the Modal..</p>
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            </div>
        </div>

        <script>
            // Get the modal
            var modal = document.getElementById("resetPwPopup");

            // Get the button that opens the modal
            var btn = document.getElementById("btnForgetPw");

            // Get the <span> element that closes the modal
            var span = document.getElementsByClassName("forgetPwClose")[0];

            // When the user clicks the button, open the modal 
            btn.onclick = function () {
                modal.style.display = "block";
            }

            // When the user clicks on <span> (x), close the modal
            span.onclick = function () {
                modal.style.display = "none";
            }

            // When the user clicks anywhere outside of the modal, close it
            window.onclick = function (event) {
                if (event.target == modal) {
                    modal.style.display = "none";
                }
            }
        </script>
    </form>
</body>
</html>
