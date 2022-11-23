<%@ Page Title="" Language="C#" MasterPageFile="~/Project/Site1.Master" AutoEventWireup="true" CodeBehind="ClaimType.aspx.cs" Inherits="FYP.ClaimType" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
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

    <asp:Panel ID="pnlClaimTypeList" runat="server">
        <div class="card shadow mb-4">
            <div class="card-header py-3">
                <p class="text-primary m-0 fw-bold" style="font-size: 21px;">Claim Type List</p>
            </div>
            <div style="position: absolute; margin-top: 16px; margin-right: 21px; top: 0; right: 0;">
                <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#modal">+</button>
            </div>
            <div class="card-body" style="overflow-y: hidden;">
                <div class="table-responsive">
                    <asp:GridView ID="gvList" runat="server" CssClass="table table-bordered dataTable1" Width="100%" AutoGenerateColumns="false"
                        OnRowDataBound="OnRowDataBound" OnRowEditing="OnRowEditing" OnRowCancelingEdit="OnRowCancelingEdit"
                        OnRowUpdating="OnRowUpdating" OnRowDeleting="OnRowDeleting" EmptyDataText="No records has been added." DataKeyNames="ClaimTypeID,ClaimTypeDesc,MaxClaim">
                        <Columns>
                            <asp:TemplateField HeaderText="Claim Type ID" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="lblClaimTypeID" runat="server" Text='<%# Eval("ClaimTypeID")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Eval("ClaimTypeDesc") %>'></asp:Label>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Claim Type" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="lblClaimType" runat="server" Text='<%# Eval("ClaimTypeDesc")%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtClaimType" runat="server" Text='<%# Eval("ClaimTypeDesc") %>'></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ErrorMessage="Claim Type Required*" Display="Dynamic" SetFocusOnError="False" ControlToValidate="txtClaimType"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Maximum Claim Amount" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmount" runat="server" Text='<%# (Eval("MaxClaim"))%>'></asp:Label>
                                </ItemTemplate>
                                <EditItemTemplate>
                                    <asp:TextBox ID="txtAmount" runat="server" Text='<%# Eval("MaxClaim") %>' TextMode="Number" step="any"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Amount Required*" Display="Dynamic" SetFocusOnError="False" ControlToValidate="txtAmount"></asp:RequiredFieldValidator>
                                </EditItemTemplate>
                            </asp:TemplateField>
                            <asp:CommandField ButtonType="Image" ShowEditButton="true" EditImageUrl="~/Image/editing.png" ControlStyle-CssClass="button"
                                UpdateImageUrl="~/Image/tick.png" CancelImageUrl="~/Image/cancel.png" DeleteImageUrl="~/Image/bin.png" ShowDeleteButton="true"
                                HeaderText="Actions" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </asp:Panel>

    <div class="modal fade" id="modal" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Add New Claim Type</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" onclick="clearr()">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group" style="margin: 0px 3px 10px">
                        <span style="color: #FF0000">* </span>
                        <label for="message-text" class="col-form-label">Claim Type:</label>
                        <asp:TextBox ID="txt_ClaimType" runat="server" class="form-control" TextMode="SingleLine" placeholder="Claim Type* - eg.Travel"></asp:TextBox>
                        <asp:Label ID="lbl_ClaimTypeError" Style="display: none" runat="server" ForeColor="Red" Font-Size="Smaller" Text="Claim Type is required"></asp:Label>
                    </div>
                    <div class="form-group" style="margin: 0px 3px 10px">
                        <span style="color: #FF0000">* </span>
                        <label for="message-text" class="col-form-label">Amount (RM):</label>
                        <asp:TextBox ID="txt_Amount" runat="server" class="form-control" TextMode="Number" step="any" min="0" Text="0" placeholder="Amount* - eg.1000"></asp:TextBox>
                        <asp:Label ID="lbl_AmountError" Style="display: none" runat="server" ForeColor="Red" Font-Size="Smaller" Text="Amount is required"></asp:Label>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnReset" type="button" class="btn btn-secondary" data-dismiss="modal" onclick="clearr()">Cancel</button>
                    <button type="button" class="btn btn-primary" id="btnValidate" onclick="requiredField()">Confirm</button>
                    <asp:Button ID="btnConfirm" Style="display: none" runat="server" Text="Button" OnClick="btnConfirm_Click" CausesValidation="False" />
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function requiredField() {
            var validate = true;
            if (document.getElementById('<%= txt_ClaimType.ClientID %>').value.trim() == "") {
                document.getElementById('<%= txt_ClaimType.ClientID %>').style.borderColor = "Red";
                document.getElementById('<%= lbl_ClaimTypeError.ClientID %>').style.display = 'block';
                validate = false;
            }
            else {
                document.getElementById('<%= txt_ClaimType.ClientID %>').style.borderColor = "";
                document.getElementById('<%= lbl_ClaimTypeError.ClientID %>').style.display = 'none';
            }
            if (document.getElementById('<%= txt_Amount.ClientID %>').value.trim() == "") {
                document.getElementById('<%= txt_Amount.ClientID %>').style.borderColor = "Red";
                document.getElementById('<%= lbl_AmountError.ClientID %>').style.display = 'block';
                validate = false;
            }
            else {
                document.getElementById('<%= txt_Amount.ClientID %>').style.borderColor = "";
                document.getElementById('<%= lbl_AmountError.ClientID %>').style.display = 'none';
            }
            if (validate) {
                document.getElementById("<%= btnConfirm.ClientID %>").click();
            }
        }
        function clearr() {
            document.getElementById('<%= txt_ClaimType.ClientID %>').value = "";
            document.getElementById('<%= txt_ClaimType.ClientID %>').style.borderColor = "";
            document.getElementById('<%= lbl_ClaimTypeError.ClientID %>').style.display = 'none';
            document.getElementById('<%= txt_Amount.ClientID %>').value = "";
            document.getElementById('<%= txt_Amount.ClientID %>').style.borderColor = "";
            document.getElementById('<%= lbl_AmountError.ClientID %>').style.display = 'none';
        }
    </script>

</asp:Content>
