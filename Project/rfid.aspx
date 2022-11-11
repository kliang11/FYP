<%@ Page Title="" Language="C#" MasterPageFile="~/Project/Site1.Master" AutoEventWireup="true" CodeBehind="rfid.aspx.cs" Inherits="FYP.Project.rfid" %>

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

    <div class="card shadow mb-4">
        <div class="card-header py-3" <%--style="margin: 15px;--%>>
            <p class="text-primary m-0 fw-bold" style="font-size: 21px;">RFID Card</p>
        </div>
        <div style="position: absolute; margin-top: 16px; margin-right: 21px; top: 0; right: 0;">
            <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#exampleModal">+</button>
        </div>
        <div class="card-body" style="overflow-y: hidden;">
            <div class="table-responsive">
                <asp:GridView ID="gvList" runat="server" CssClass="table table-bordered dataTable1" Width="100%" AutoGenerateColumns="false"
                    OnRowDataBound="OnRowDataBound" OnRowEditing="OnRowEditing" OnRowCancelingEdit="OnRowCancelingEdit"
                    OnRowUpdating="OnRowUpdating" OnRowDeleting="OnRowDeleting" EmptyDataText="No records has been added." DataKeyNames="Card_ID,Card_Status,Date,Staff_ID">
                    <Columns>
                        <asp:TemplateField HeaderText="Card ID" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblCardID" runat="server" Text='<%# Eval("Card_ID")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblCardID" runat="server" Text='<%# Eval("Card_ID") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Card Status" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblCardStatus" runat="server" Text='<%# Eval("Card_Status")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblCardStatus" runat="server" Text='<%# Eval("Card_Status") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Registered Date" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Date")).ToShortDateString()%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtDate" runat="server" Text='<%# Convert.ToDateTime(Eval("Date")).ToShortDateString() %>'></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Staff ID" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblStaffID" runat="server" Text='<%# Eval("Staff_ID")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtStaffID" runat="server" Text='<%# Eval("Staff_ID") %>'></asp:TextBox>
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

    <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="exampleModalLabel">Add New RFID</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group" style="margin: 0px 3px 10px">
                        <span style="color: #FF0000">* </span>
                        <label for="recipient-name" class="col-form-label">RFID ID:</label>
                        <asp:TextBox ID="txt_rfidid" runat="server" class="form-control" TextMode="Number" onkeydown="return event.keyCode !== 69"></asp:TextBox>
                        <asp:Label ID="lbl_rfidError" Style="display: none" runat="server" ForeColor="Red" Font-Size="Smaller" Text="This field is required"></asp:Label>

                    </div>
                    <div class="form-group" style="margin: 0px 3px 10px">
                        <span style="color: #FF0000">* </span>
                        <label for="message-text" class="col-form-label">Date:</label>
                        <asp:TextBox ID="txt_date" runat="server" class="form-control" TextMode="Date"></asp:TextBox>
                        <asp:Label ID="lbl_dateError" Style="display: none" runat="server" ForeColor="Red" Font-Size="Smaller" Text="This field is required"></asp:Label>

                    </div>
                    <div class="form-group" style="margin: 0px 3px 10px">
                        <label for="message-text" class="col-form-label">Staff ID:</label>
                        <span style="color: #5251519e; font-size: smaller">(Optional)</span>
                        <asp:TextBox ID="txt_staffid" runat="server" class="form-control" TextMode="Number"></asp:TextBox>
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
            if (document.getElementById('<%= txt_rfidid.ClientID %>').value.trim() == "") {
                document.getElementById('<%= txt_rfidid.ClientID %>').style.borderColor = "Red";
                document.getElementById('<%= lbl_rfidError.ClientID %>').style.display = 'block';
                validate = false;
            }
            else {
                document.getElementById('<%= txt_rfidid.ClientID %>').style.borderColor = "";
                document.getElementById('<%= lbl_rfidError.ClientID %>').style.display = 'none';

            }
            if (document.getElementById('<%= txt_date.ClientID %>').value.trim() == "") {
                document.getElementById('<%= txt_date.ClientID %>').style.borderColor = "Red";
                document.getElementById('<%= lbl_dateError.ClientID %>').style.display = 'block';
                validate = false;
            }
            else {
                document.getElementById('<%= txt_date.ClientID %>').style.borderColor = "";
                document.getElementById('<%= lbl_dateError.ClientID %>').style.display = 'none';
            }
            if (validate) {
                document.getElementById("<%= btnConfirm.ClientID %>").click();            
            }
        }
        function clearr() {
            document.getElementById('<%= txt_rfidid.ClientID %>').value = "";
            document.getElementById('<%= txt_date.ClientID %>').value = "";
            document.getElementById('<%= txt_rfidid.ClientID %>').style.borderColor = "";
            document.getElementById('<%= lbl_rfidError.ClientID %>').style.display = 'none';
            document.getElementById('<%= txt_date.ClientID %>').style.borderColor = "";
            document.getElementById('<%= lbl_dateError.ClientID %>').style.display = 'none';
        }


    </script>
</asp:Content>
