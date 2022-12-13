<%@ Page Title="" Language="C#" MasterPageFile="~/Project/Site1.Master" AutoEventWireup="true" CodeBehind="ClaimList.aspx.cs" Inherits="FYP.Project.ClaimList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="style.css" />

    <style type="text/css">
        .button {
            max-width: 20px;
            max-height: 20px;
            margin: 0px 10px;
        }

            .button:hover {
                transform: scale(1.2);
            }

        .modal {
            display: none;
            position: absolute;
            top: 0px;
            left: 0px;
            background-color: black;
            z-index: 100;
            opacity: 0.8;
            filter: alpha(opacity=60);
            -moz-opacity: 0.8;
            min-height: 100%;
        }

        .hideGridColumn {
            display: none;
        }

        #divImage {
            display: none;
            z-index: 1000;
            position: fixed;
            top: 0;
            left: 0;
            background-color: White;
            height: 550px;
            width: 600px;
            padding: 3px;
            border: solid 1px black;
        }
    </style>


    <asp:Panel ID="pnlClaimAppliedList" runat="server">
        <div class="card shadow mb-4">
            <div class="card-header py-3">
                <p class="text-primary m-0 fw-bold" style="font-size: 21px;">Claim List</p>
            </div>
            <div class="card-body" style="overflow-y: hidden;">
                <div class="table-responsive">
                    <asp:GridView ID="gvList" runat="server" CssClass="table table-bordered dataTable1" Width="100%" AutoGenerateColumns="false"
                        OnRowDataBound="OnRowDataBound" OnRowCommand="FireRowCommand"
                        EmptyDataText="No records has been added." DataKeyNames="ClaimID,Name,ClaimStatus,ClaimDate,ClaimAmount,ClaimAttachment">
                        <Columns>
                            <asp:TemplateField HeaderText="Claim ID" ItemStyle-Width="8.5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="lblClaimID" runat="server" Text='<%# Eval("ClaimID")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Staff Name" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="lblStaffName" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="16.5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("ClaimStatus")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Claim Apply Date" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="lblClaimDate" runat="server" Text='<%# Convert.ToDateTime(Eval("ClaimDate")).ToShortDateString()%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Claim Amount" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="lblClaimAmount" runat="server" Text='<%# "RM "+ Eval("ClaimAmount")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-CssClass="hideGridColumn" HeaderStyle-CssClass="hideGridColumn">
                                <ItemTemplate>
                                    <asp:Label ID="lblImageUrl" runat="server" Text='<%#"data:Image/png;base64," +  Convert.ToBase64String((byte[])Eval("ClaimAttachment"))%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Claim Attachment" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                   <asp:Button ID="btnView" runat="server" Text="View" ForeColor="Blue" Style="border: none; background: none; padding: 0; text-decoration-line: underline; cursor: pointer" OnClientClick="return LoadDiv(this);" />                              
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnApprove" runat="server" CssClass="button" Style="cursor: pointer" ImageUrl="~/Image/approved.png" AlternateText="approve" CommandName="Approving" CausesValidation="false" />
                                    <asp:ImageButton ID="btnReject" runat="server" CssClass="button" Style="cursor: pointer" ImageUrl="~/Image/reject.png" AlternateText="reject" CommandName="Rejecting" CausesValidation="false" />
                                    <asp:ImageButton ID="btnViewDetail" runat="server" CssClass="button" Style="cursor: pointer" ImageUrl="~/Image/detail.png" AlternateText="view" CommandName="Selecting" CausesValidation="false" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </asp:Panel>


    <div id="divBackground" class="modal">
    </div>
    <div id="divImage">
        <table style="height: 100%; width: 100%">
            <tr>
                <td valign="middle" align="center">
                    <img id="imgLoader" alt="" src="~/Image/loader.gif" />
                    <img id="imgFull" alt="" src="" style="display: none; height: 500px; width: 590px" />
                </td>
            </tr>
            <tr>
                <td align="center" valign="bottom">
                    <input id="btnClose" type="button" value="close" onclick="HideDiv()" />
                </td>
            </tr>
        </table>
    </div>

    <asp:Panel ID="rejectReasonPopup" runat="server" class="forgetPwModal" Visible="false">
        <div class="forgetPwModal-content">
            <div class="modal-header">                
                <h5>Reject Claim</h5>
                <asp:LinkButton type="button" ID="btnRejectClose" class="close" runat="server" OnClick="btnRejectClose_Click">
                <span>&times;</span>
                </asp:LinkButton>
            </div>
            <div style="padding: 20px;">
                <asp:Label ID="lblPopUpID" runat="server" class="form-label" Text="Claim ID:" Style="display: block; margin-bottom: 15px;"></asp:Label>
                <label class="form-label">Reject Reason</label>
                <span style="color: #FF0000">*</span>
                <asp:TextBox ID="txtRejectReasonPopUp" runat="server" class="form-control" placeholder="Reject Reason*" />
                <asp:Label ID="lbl_reasonError" Style="display: none" runat="server" ForeColor="Red" Font-Size="Smaller" Text="This field is required"></asp:Label>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnSubmitPopUp" runat="server" Text="Submit" class="form-control btn btn-primary rounded submit px-3" OnClientClick="return requiredField()" OnClick="btnSubmit_Click" />
            </div>
        </div>
    </asp:Panel>

    <script type="text/javascript">
        function LoadDiv(Position) {

            var row = Position.parentNode.parentNode;
            var url = row.cells[5].getElementsByTagName('span')[0].innerHTML;

            var img = new Image();
            var bcgDiv = document.getElementById("divBackground");
            var imgDiv = document.getElementById("divImage");
            var imgFull = document.getElementById("imgFull");
            var imgLoader = document.getElementById("imgLoader");
            imgLoader.style.display = "block";
            img.onload = function () {
                imgFull.src = img.src;
                imgFull.style.display = "block";
                imgLoader.style.display = "none";
            };
            img.src = url;
            var width = document.body.clientWidth;
            if (document.body.clientHeight > document.body.scrollHeight) {
                bcgDiv.style.height = document.body.clientHeight + "px";
            }
            else {
                bcgDiv.style.height = document.body.scrollHeight + "px";
            }
            imgDiv.style.left = (width - 650) / 2 + "px";
            imgDiv.style.top = "20px";
            bcgDiv.style.width = "100%";

            bcgDiv.style.display = "block";
            imgDiv.style.display = "block";
            return false;
        }
        function HideDiv() {
            var bcgDiv = document.getElementById("divBackground");
            var imgDiv = document.getElementById("divImage");
            var imgFull = document.getElementById("imgFull");
            if (bcgDiv != null) {
                bcgDiv.style.display = "none";
                imgDiv.style.display = "none";
                imgFull.style.display = "none";
            }
        }
    </script>

    <script type="text/javascript">

        var modal = document.getElementById('<%= rejectReasonPopup.ClientID %>');
        var span = document.getElementsByClassName("forgetPwClose")[0];
        //document.getElementById('<\%= lblStoreRejectReason.ClientID %>').visibility = "hidden";

        // When the user clicks the button, open the modal 
        if (modal !== null) {
            modal.style.display = "block";
            //document.getElementById('<\%= lblStoreRejectReason.ClientID %>').visibility = "visible";
            document.getElementById('<%= rejectReasonPopup.ClientID %>').value = "";
            //document.getElementById('<\%= lblStoreRejectReason.ClientID %>').innerText = "0";
            //document.getElementById('<\%= lblStoreRejectReason.ClientID %>').visibility = "hidden";
        }

        function requiredField() {
            var validate = true;
            if (document.getElementById('<%= txtRejectReasonPopUp.ClientID %>').value.trim() == "") {
                document.getElementById('<%= txtRejectReasonPopUp.ClientID %>').style.borderColor = "Red";
                document.getElementById('<%= lbl_reasonError.ClientID %>').style.display = 'block';
                validate = false;
            }
            else {
                document.getElementById('<%= txtRejectReasonPopUp.ClientID %>').style.borderColor = "";
                document.getElementById('<%= lbl_reasonError.ClientID %>').style.display = 'none';
            }
            return validate
        }

    </script>

</asp:Content>