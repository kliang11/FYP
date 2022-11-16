<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="FYP.Project.ChangePassword" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">
    <title></title>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1" />
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.2.0/dist/css/bootstrap.min.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/5.13.0/css/all.min.css" />
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.2.0/dist/js/bootstrap.bundle.min.js"></script>
    <link rel="stylesheet" href="style.css" />
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
        <div class="changePassword ">
            <asp:Label ID="lblTitle" class="mb-5 h2" runat="server" Style="margin-left: 25%;" Text="Change Password"></asp:Label>
            <div class="mt-4">
                <div class="form-group">
                    <div class="label">
                        <asp:Label ID="lblCurrentPassword" runat="server" Text="Current Password"></asp:Label>
                        <asp:Label ID="lblStar" runat="server" Style="color: #FF0000" Text="*"></asp:Label>
                    </div>
                    <div class="input-group mb-3">
                        <asp:TextBox ID="txtCurrentPw" runat="server" class="form-control" placeholder="Current Password*"></asp:TextBox>
                        <asp:Label ID="lblEye" runat="server" class="input-group-text"><i class="far fa-eye-slash" id="togglePassword" style="cursor: pointer" ></i></asp:Label>
                    </div>
                </div>
                <div class="form-group">
                    <div class="label">
                        <asp:Label ID="Label3" runat="server" Text="New Password"></asp:Label>
                        <span style="color: #FF0000">*</span>
                    </div>
                    <div class="input-group mb-3">
                        <asp:TextBox ID="txtNewPw" runat="server" class="form-control" placeholder="New Password*"></asp:TextBox>
                        <span class="input-group-text"><i class="far fa-eye-slash" id="togglePassword2" style="cursor: pointer" ></i></span>
                    </div>
                </div>
                <div class="form-group">
                    <div class="label">
                        <asp:Label ID="Label4" runat="server" Text="Confirm New Password"></asp:Label>
                        <span style="color: #FF0000">*</span>
                    </div>
                    <asp:TextBox ID="txtConfirmNewPw" runat="server" class="form-control" placeholder="Confirm New Password*"></asp:TextBox>
                </div>

                <asp:Label ID="lblErrorMsg" runat="server" Text="Label" class="errorMsg" Visible="False"></asp:Label>

                <div>
                    <asp:Button ID="btnChange" class="form-control btn btn-success rounded submit px-3 mt-3" runat="server" Text="Save" OnClick="btnChange_Click" />
                </div>
                <div>
                    <asp:Button ID="btnCancel" class="form-control btn btn-danger rounded submit px-3 mt-3" runat="server" Text="Back" OnClick="btnCancel_Click" />
                </div>
            </div>
        </div>
        <asp:Label ID="lblStorePassword" runat="server" Text="" Visible="false"></asp:Label>
        <asp:Label ID="lblNeedRenew" runat="server" Text="" Visible="false"></asp:Label>

        <script type="text/javascript">


            var togglePassword = null;
            togglePassword = document.querySelector("#togglePassword");
            if (togglePassword != null) {
                const password = document.querySelector("#txtCurrentPw");
                togglePassword.addEventListener("click", function () {
                    // toggle the type attribute
                    const type = password.getAttribute("type") === "password" ? "text" : "password";
                    password.setAttribute("type", type);
                    // toggle the eye icon
                    this.classList.toggle('fa-eye');
                    this.classList.toggle('fa-eye-slash');
                });
            }

            var togglePassword2 = null;
            togglePassword2 = document.querySelector("#togglePassword2");
            if (togglePassword2 != null) {
                const password2 = document.querySelector("#txtNewPw");
                const password3 = document.querySelector("#txtConfirmNewPw");

                togglePassword2.addEventListener("click", function () {

                    // toggle the type attribute
                    const type = password2.getAttribute("type") === "password" ? "text" : "password";
                    password2.setAttribute("type", type);
                    password3.setAttribute("type", type);
                    // toggle the eye icon
                    this.classList.toggle('fa-eye');
                    this.classList.toggle('fa-eye-slash');
                });               
            }
        </script>
    </form>


</body>
</html>
