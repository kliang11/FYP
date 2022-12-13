<%@ Page Title="" Language="C#" MasterPageFile="~/Project/Site1.Master" AutoEventWireup="true" CodeBehind="ProfilePage.aspx.cs" Inherits="FYP.Project.ProfilePage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="style.css" />

    <div class="profilePanel">

        <header class="pb-2 fixed-profile-header" style="border-bottom: 1px solid #c9c9c9;">
            <asp:Label ID="lblTitle" runat="server" Text="Staff" CssClass="alignleft mt-2"></asp:Label>
            <asp:LinkButton ID="btnEditt" runat="server" CssClass="alignright btn btn-danger" OnClientClick="return showUploadFile()" OnClick="btnEditt_Click" CausesValidation="False"><i class="fas fa-edit"></i> Edit </asp:LinkButton>
            <asp:LinkButton ID="btnSave" runat="server" CssClass="alignright btn btn-success" OnClientClick="return hideUploadFileDeleteButton()" OnClick="btnSave_Click" Visible="False"><i class="fas fa-save"></i> Save </asp:LinkButton>
            <asp:LinkButton ID="btnBack" runat="server" CssClass="alignright btn btn-light" CausesValidation="False" OnClientClick="return alertConfirmBack()" OnClick="btnBack_Click"><i class="fa fa-angle-left"></i> Back </asp:LinkButton>
            <div style="clear: both;"></div>
        </header>

        <asp:Panel ID="jobPanel" runat="server" CssClass="profileContentPanel mt-2 mb-2" ScrollBars="Auto">
            <label class="subtitle">Profile</label>

            <asp:Image ID="imgProfile" runat="server" class="profileImage" alt="profileImage" ClientIDMode="Static" ImageUrl="" />
            <div style="text-align: center; margin-bottom: 1%;">
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" Display="Dynamic" ErrorMessage="Image must be in .jpg, .jpeg, .png format only." ForeColor="Red" ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.jpg|.JPG|.jpeg|.JPEG|.png|.PNG)$" ControlToValidate="FileUpload1"></asp:RegularExpressionValidator>
            </div>
            <div style="text-align: center; position: relative;">
                <asp:LinkButton ID="btnDeletes" runat="server" CssClass="btn btn-light mb-2" OnClientClick="hideDeleteButton()" OnClick="btnDelete_Click" CausesValidation="False" Style="display: inline-block;"><i class="fas fa-trash"></i> Delete </asp:LinkButton>
            </div>
            <label id="lblFileUpload" class="file-upload">
                <span><strong>Upload Image</strong></span>
                <asp:FileUpload ID="FileUpload1" runat="server" onchange="showimagepreview(this)" accept="image/*" name="image"></asp:FileUpload>
            </label>

            <div class="form-group">
                <div class="rows">
                    <label class="column" style="margin-right: 2%">
                        Staff ID
                    </label>
                    <label class="column">
                        Full Name  
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Name Required" ControlToValidate="txtName" ForeColor="Red"></asp:RequiredFieldValidator>
                    </label>
                </div>
                <div class="rows">
                    <asp:TextBox ID="txtStaffID" runat="server" CssClass="form-control column" placeholder="Staff ID*" Style="margin-right: 2%" Enabled="false"></asp:TextBox>
                    <asp:TextBox ID="txtName" runat="server" CssClass="form-control column" placeholder="Full Name*"></asp:TextBox>
                </div>
            </div>
            <div class="form-group">
                <label>
                    Email
                </label>
                <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Email*" Enabled="false"></asp:TextBox>
            </div>
        </asp:Panel>

    </div>

    <script type="text/javascript">

        function showimagepreview(input) {

            if (typeof (input.files) != "undefined") {
                var size = parseFloat(input.files[0].size / 1024).toFixed(2);
                if (size > 5000) {
                    alert("Image has exceeded 5 MB");
                    //var fu = input
                    //if (fu != null) {
                    //    fu.setAttribute("type", "input");
                    //    fu.setAttribute("type", "file");
                    //    document.getElementById("imgProfile").setAttribute("src", "");
                    //}

                }
                else {
                    if (input.files && input.files[0]) {
                        var reader = new FileReader();
                        reader.onload = function (e) {

                            document.getElementById("imgProfile").setAttribute("src", e.target.result); e;

                        }
                        reader.readAsDataURL(input.files[0]
                        );
                    }
                }
                document.getElementById('<%= btnDeletes.ClientID %>').style.display = "inline-block";
            }
        }

        function hideDeleteButton() {
            document.getElementById('<%= btnDeletes.ClientID %>').style.display = "none";
            return true;
        }

        var x = null;
        x = document.getElementById('<%= btnSave.ClientID %>');
        var y = null;
        y = document.getElementById('<%= btnEditt.ClientID %>');
        if (x === null || y !== null) {
            document.getElementById("lblFileUpload").style.display = "none";
            document.getElementById('<%= btnDeletes.ClientID %>').style.display = "none";
        }
        else {
            document.getElementById("lblFileUpload").style.display = "block";
            document.getElementById('<%= btnDeletes.ClientID %>').style.display = "inline-block";

        }
        var url = "deleted!)^%$(!";
        url = document.getElementById("imgProfile").getAttribute("src");
        if (url == "data:Image/png;base64,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") {
            document.getElementById('<%= btnDeletes.ClientID %>').style.display = "none";
        }
        else if (url == "deleted!)^%$(!") {
            document.getElementById('<%= btnDeletes.ClientID %>').style.display = "none";
        }
        else if (x !== null || y === null) {
            document.getElementById('<%= btnDeletes.ClientID %>').style.display = "inline-block";
        }

        function showUploadFile() {
            document.getElementById("lblFileUpload").style.display = "block";
            return true;
        }

        function hideUploadFileDeleteButton() {
            if (x === null || y !== null) {
                document.getElementById("lblFileUpload").style.display = "none";
                document.getElementById('<%= btnDeletes.ClientID %>').style.display = "none";
            }
            return true;
        }

        function checkAndShow() {
            return true;
        }

        function alertConfirmBack() {
            if (x !== null || y === null) {
                return confirm("Are you sure you want to back without saving?");
            }
            else {
                return true;
            }
        }


    </script>

</asp:Content>
