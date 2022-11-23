<%@ Page Title="" Language="C#" MasterPageFile="~/Project/Site1.Master" AutoEventWireup="true" CodeBehind="ClaimDetails.aspx.cs" Inherits="FYP.Project.ClaimDetails" %>

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


    <div class="profilePanel">

        <header class="pb-2 fixed-profile-header" style="border-bottom: 1px solid #c9c9c9;">
            <asp:Button ID="btnApproves" runat="server" CssClass="alignleft btn btn-success" Style="background: url(/Image/approved.png) 
            left 3px top 5px no-repeat #28a745; padding: 5px 10px 5px 30px;"
                CausesValidation="False" Text="Approve" Visible="False" OnClick="btnApproves_Click" />
            <asp:Button ID="btnReject" runat="server" CssClass="alignleft btn btn-danger" Style="background: url(/Image/reject.png) 
            left 3px top 5px no-repeat #dc3545; padding: 5px 10px 5px 30px; margin-left: 10px;"
                CausesValidation="False" Text="Reject" Visible="False" OnClick="btnReject_Click" />
            <asp:LinkButton ID="btnBack" runat="server" CssClass="alignright btn btn-light" CausesValidation="False" OnClick="btnBack_Click"><i class="fa fa-angle-left"></i> Back </asp:LinkButton>
            <div style="clear: both;"></div>
        </header>

        <asp:Panel ID="claimPanel" runat="server" CssClass="profileContentPanel mt-2 mb-2" ScrollBars="Auto">
            <label class="subtitle">Claim Details</label>

            <div class="form-group">
                <div class="rows">
                    <label class="column" style="margin-right: 2%">Claim ID</label>
                    <label class="column">Claim Applied Date</label>
                </div>
                <div class="rows">
                    <asp:TextBox ID="txtClaimID" runat="server" CssClass="form-control column" Enabled="False" Style="margin-right: 2%"></asp:TextBox>
                    <asp:TextBox ID="txtClaimDate" runat="server" CssClass="form-control column" Enabled="False"></asp:TextBox>
                </div>
            </div>

            <asp:Panel ID="panelStaff" runat="server" class="form-group">
                <div class="rows">
                    <label class="column" style="margin-right: 2%">Staff ID</label>
                    <label class="column">Staff Name</label>
                </div>
                <div class="rows">
                    <asp:TextBox ID="txtStaffID" runat="server" CssClass="form-control column" Enabled="False" Style="margin-right: 2%"></asp:TextBox>
                    <asp:TextBox ID="txtStaffName" runat="server" CssClass="form-control column" Enabled="False"></asp:TextBox>
                </div>
            </asp:Panel>

            <div class="form-group">
                <div class="rows">
                    <label class="column" style="margin-right: 2%">Date From</label>
                    <label class="column">Date To</label>
                </div>
                <div class="rows">
                    <asp:TextBox ID="txtDateFrom" runat="server" CssClass="form-control column" Enabled="False" Style="margin-right: 2%"></asp:TextBox>
                    <asp:TextBox ID="txtDateTo" runat="server" CssClass="form-control column" Enabled="False"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <div class="rows">
                    <label class="column" style="margin-right: 2%">Claim Type</label>
                    <label class="column">Claim Amount</label>
                </div>
                <div class="rows">
                    <asp:TextBox ID="txtClaimType" runat="server" CssClass="form-control column" Enabled="False" Style="margin-right: 2%"></asp:TextBox>
                    <asp:TextBox ID="txtAmount" runat="server" CssClass="form-control column" Enabled="False"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <label>Claim Reason</label>
                <asp:TextBox ID="txtDescription" runat="server" CssClass="form-control" Text="" Enabled="False" TextMode="MultiLine"></asp:TextBox>
            </div>

            <div class="form-group">
                <div class="rows">
                    <label class="column" style="margin-right: 2%">Status</label>
                    <label class="column">Claim Receive</label>
                </div>
                <div class="rows">
                    <asp:TextBox ID="txtStatus" runat="server" CssClass="form-control column" Enabled="False" Style="margin-right: 2%"></asp:TextBox>
                    <asp:TextBox ID="txtClaimReceive" runat="server" CssClass="form-control column" Enabled="False"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <label>Reject Reason</label>
                <asp:TextBox ID="txtReason" runat="server" CssClass="form-control" Text="" Enabled="False" TextMode="MultiLine"></asp:TextBox>
            </div>

            <div class="form-group" style="text-align: center;">
                <label>Claim Attachment</label>
                <asp:ImageButton ID="imgBtnClaimAttachment" runat="server" CssClass="profileImage" ImageUrl="" AlternateText="attachment" Style="cursor: pointer;" OnClientClick="return LoadDiv(this.src);" />
            </div>

        </asp:Panel>
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

    <script type="text/javascript">
        function LoadDiv(url) {
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

        // When the user clicks the button, open the modal 
        if (modal !== null) {
            modal.style.display = "block";
            document.getElementById('<%= rejectReasonPopup.ClientID %>').value = "";            
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
