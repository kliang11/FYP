<%@ Page Title="Leave" Language="C#" MasterPageFile="~/Project/Site1.Master" AutoEventWireup="true" CodeBehind="LeaveList.aspx.cs" Inherits="FYP.Project.LeaveList" %>

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

    <div class="card-deck" style="margin-bottom: 25px">
        <div class="col-xl-3 col-sm-6 col-12">
            <div class="card">
                <div class="card-content" style="background-color: #50bb69">
                    <div class="card-body">
                        <div class="media d-flex">
                            <div class="align-self-center">
                                <i class="fas fa-calendar-check fa-3x float-left" style="color: white"></i>
                            </div>
                            <div class="media-body text-right">
                                <h2 id="countApproved" runat="server" style="color: white">8</h2>
                                <span style="color: white; font-size: larger">Approved</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xl-3 col-sm-6 col-12">
            <div class="card">
                <div class="card-content" style="background-color: #f5c62d">
                    <div class="card-body">
                        <div class="media d-flex">
                            <div class="align-self-center">
                                <i class="fas fa-clock fa-3x float-left" style="color: white"></i>
                            </div>
                            <div class="media-body text-right">
                                <h2 id="countPending" runat="server" style="color: white">10</h2>
                                <span style="color: white; font-size: larger">Pending</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xl-3 col-sm-6 col-12">
            <div class="card">
                <div class="card-content" style="background-color: #ef6a6a">
                    <div class="card-body">
                        <div class="media d-flex">
                            <div class="align-self-center">
                                <i class="fas fa-calendar-times fa-3x float-left" style="color: white"></i>
                            </div>
                            <div class="media-body text-right">
                                <h2 id="countRejected" runat="server" style="color: white">0</h2>
                                <span style="color: white; font-size: larger">Rejected</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <asp:Panel ID="pnlLeaveAppliedList" runat="server">
        <div class="card shadow mb-4">
            <div class="card-header py-3">
                <p class="text-primary m-0 fw-bold" style="font-size: 21px;">Leave</p>
            </div>
            <div class="card-body" style="overflow-y: hidden;">
                <div class="table-responsive">
                    <asp:GridView ID="gvList" runat="server" CssClass="table table-bordered dataTable1" Width="100%" AutoGenerateColumns="false"
                        OnRowDataBound="OnRowDataBound" OnRowCommand="FireRowCommand"
                        EmptyDataText="No records has been added." DataKeyNames="LeaveAppID,Name,StaffID,LeaveType,LeaveDateStart,LeaveDateEnd,TotalDay,LeaveStatus,LeaveAttachments">
                        <Columns>
                            <asp:TemplateField HeaderText="Leave ID" ItemStyle-Width="11%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="lblLeaveID" runat="server" Text='<%# Eval("LeaveAppID")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Staff Name" ItemStyle-Width="11%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="lblStaffName" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
                                    <asp:Label ID="lblStaffID" runat="server" Text='<%# Eval("StaffID")%>' style="display:none"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Leave Type" ItemStyle-Width="11%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="lblLeaveType" runat="server" Text='<%# Eval("LeaveType") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Leave From" ItemStyle-Width="11%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="lblLeaveStartDate" runat="server" Text='<%# Eval("LeaveDateStart","{0:d}")  %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Leave To" ItemStyle-Width="11%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="lblLeaveEndDate" runat="server" Text='<%# Eval("LeaveDateEnd","{0:d}") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Total Day" ItemStyle-Width="11%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="lblTotalDay" runat="server" Text='<%# Eval("TotalDay")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" ItemStyle-Width="11%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("LeaveStatus")%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField ItemStyle-CssClass="hideGridColumn" HeaderStyle-CssClass="hideGridColumn">
                                <ItemTemplate>
                                    <asp:Label ID="lblImageUrl" runat="server" Text='<%#"data:Image/png;base64," +  Convert.ToBase64String((byte[])Eval("LeaveAttachments"))%>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Leave Attachment" ItemStyle-Width="11%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                                <ItemTemplate>
                                    <%--<asp:ImageButton ID="imgBtnClaimAttachment" runat="server" ImageUrl='<%# Eval("LeaveAttachments")%>' AlternateText="attachment" Width="100px" Height="100px" Style="cursor: pointer; max-width: 100px; max-height: 100px;" OnClientClick="return LoadDiv(this.src);" />    --%>
                                    <asp:Button ID="btnView" runat="server" Text="View" ForeColor="Blue" Style="border: none; background: none; padding: 0; text-decoration-line: underline; cursor: pointer" OnClientClick="return LoadDiv(this);" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Action" ItemStyle-Width="12%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
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
                <h5>Reject Leave</h5>
                <asp:LinkButton type="button" ID="btnRejectClose" class="close" runat="server" OnClick="btnRejectClose_Click">
                <span>&times;</span>
                </asp:LinkButton>
            </div>
            <div style="padding: 20px;">
                <asp:Label ID="lblPopUpID" runat="server" class="form-label" Text="Leave ID:" Style="display: block; margin-bottom: 15px;"></asp:Label>
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
            var url = row.cells[7].getElementsByTagName('span')[0].innerHTML;

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
