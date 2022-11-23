<%@ Page Title="" Language="C#" MasterPageFile="~/Project/Site1.Master" AutoEventWireup="true" CodeBehind="EmployeeList.aspx.cs" Inherits="FYP.Project.EmployeeList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="style.css" />

    <style>
        .button {
            max-width: 20px;
            max-height: 20px;
            margin: 0px 10px;
        }

            .button:hover {
                transform: scale(1.2);
            }
    </style>

    <div class="card shadow mb-4">
        <div class="card-header py-3" <%--style="margin: 15px;--%>>
            <p class="text-primary m-0 fw-bold" style="font-size: 21px;">Staff List</p>
        </div>
        <div style="position: absolute; margin-top: 16px; margin-right: 21px; top: 0; right: 0;">
            <asp:Button ID="btnAdd" runat="server" class="btn btn-primary" Text="+" CausesValidation="False" OnClick="btnAdd_Click" />
        </div>
        <div class="card-body" style="overflow-y: hidden;">
            <div class="table-responsive">
                <asp:GridView ID="gvList" runat="server" CssClass="table table-bordered dataTable1" Width="100%" AutoGenerateColumns="false"
                    OnRowDataBound="OnRowDataBound" OnRowDeleting="OnRowDeleting" OnSelectedIndexChanged="gvList_SelectedIndexChanged"
                    EmptyDataText="No staff records." DataKeyNames="Photo, Staff_ID, Name, Email, Role">
                    <Columns>
                        <asp:TemplateField HeaderText="Image" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Image ID="imgStaff" runat="server" Style="height: 35px; width: 35px; border-radius: 50%;" alt="Avatar" ImageUrl='<%#"data:Image/png;base64," +  Convert.ToBase64String((byte[])Eval("Photo"))%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Staff ID" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblStaffID" runat="server" Text='<%# Eval("Staff_ID")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Staff Name" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblStaffName" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Staff Email" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblStaffEmail" runat="server" Text='<%# Eval("Email")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Staff Role" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblStaffRole" runat="server" Text='<%# Eval("Role")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ButtonType="Image" ControlStyle-CssClass="button"
                            DeleteImageUrl="~/Image/bin.png" ShowDeleteButton="true"
                            SelectImageUrl="~/Image/detail.png" ShowSelectButton="True"
                            HeaderText="Actions" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" />
                        <asp:CommandField ButtonType="Image" ControlStyle-CssClass="button"
                            SelectImageUrl="~/Image/detail.png" ShowSelectButton="True"
                            HeaderText="Actions" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <asp:Panel ID="rejectReasonPopup" runat="server" class="forgetPwModal" Visible="false">
        <div class="forgetPwModal-content">
            <div class="modal-header">
                <h5>Add New Staff</h5>
                <asp:LinkButton type="button" ID="btnRejectClose" class="close" runat="server" OnClick="btnRejectClose_Click" CausesValidation="False">
                <span>&times;</span>
                </asp:LinkButton>
            </div>
            <div style="padding: 20px;">
                <div class="form-group" style="margin: 0px 3px 10px">
                    <span style="color: #FF0000">*</span>
                    <label class="form-label">Staff Email</label>
                    <asp:TextBox ID="txtStaffEmailPopUp" runat="server" class="form-control" placeholder="Staff Email*" />
                    <asp:Label ID="lbl_EmailError" Style="display: none" runat="server" ForeColor="Red" Font-Size="Smaller" Text="This field is required"></asp:Label>
                </div>
                <div class="form-group" style="margin: 0px 3px 10px">
                    <label class="form-label">Staff Role</label>
                    <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control">
                        <asp:ListItem>Normal Staff</asp:ListItem>
                        <asp:ListItem>HR Staff</asp:ListItem>
                    </asp:DropDownList>
                </div>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnSubmitPopUp" runat="server" Text="Submit" class="form-control btn btn-primary rounded submit px-3" OnClientClick="return requiredField()" OnClick="btnSubmit_Click" />
            </div>
        </div>
    </asp:Panel>

    <script type="text/javascript">

        var modal = document.getElementById('<%= rejectReasonPopup.ClientID %>');

        // When the user clicks the button, open the modal 
        if (modal !== null) {
            modal.style.display = "block";
            document.getElementById('<%= rejectReasonPopup.ClientID %>').value = "";
        }

        function requiredField() {
            var validate = true;
            if (document.getElementById('<%= txtStaffEmailPopUp.ClientID %>').value.trim() == "") {
                document.getElementById('<%= txtStaffEmailPopUp.ClientID %>').style.borderColor = "Red";
                document.getElementById('<%= lbl_EmailError.ClientID %>').style.display = 'block';
                validate = false;
                document.getElementById('<%= lbl_EmailError.ClientID %>').textContent = "This field is required";
            }
            else if (document.getElementById('<%= txtStaffEmailPopUp.ClientID %>').value.trim() !== "" && !(/^\w+([\.-]?\w+)*@\w+([\.-]?\w+)*(\.\w{2,3})+$/.test(document.getElementById('<%= txtStaffEmailPopUp.ClientID %>').value.trim()))) {
                document.getElementById('<%= txtStaffEmailPopUp.ClientID %>').style.borderColor = "Red";
                document.getElementById('<%= lbl_EmailError.ClientID %>').style.display = 'block';
                validate = false;
                document.getElementById('<%= lbl_EmailError.ClientID %>').textContent = "Email entered is not valid.";
            }
            else {
                document.getElementById('<%= txtStaffEmailPopUp.ClientID %>').style.borderColor = "";
                document.getElementById('<%= lbl_EmailError.ClientID %>').style.display = 'none';
            }
            return validate
        }


    </script>

</asp:Content>
