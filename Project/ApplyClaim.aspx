<%@ Page Title="" Language="C#" MasterPageFile="~/Project/Site1.Master" AutoEventWireup="true" CodeBehind="ApplyClaim.aspx.cs" Inherits="FYP.Project.ApplyClaim" %>

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

        .file-upload {
            display: block;
            /*overflow: hidden;*/
            text-align: center;
            vertical-align: middle;
            /*font-family: Arial;*/
            border: 1px solid #124d77;
            background: #007dc1;
            color: #fff;
            /*border-radius: 6px;*/
            /*-moz-border-radius: 6px;*/
            cursor: pointer;
            text-shadow: #000 1px 1px 2px;
            -webkit-border-radius: 6px;
            margin-left: auto;
            margin-right: auto;
        }

            .file-upload:hover {
                /*background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #0061a7), color-stop(1, #007dc1));*/
                /*background: -moz-linear-gradient(top, #0061a7 5%, #007dc1 100%);*/
                /*background: -webkit-linear-gradient(top, #0061a7 5%, #007dc1 100%);*/
                /*background: -o-linear-gradient(top, #0061a7 5%, #007dc1 100%);*/
                /*background: -ms-linear-gradient(top, #0061a7 5%, #007dc1 100%);*/
                background: linear-gradient(to bottom, #0061a7 5%, #007dc1 100%);
                /*filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#0061a7', endColorstr='#007dc1',GradientType=0);*/
                background-color: #0061a7;
            }

        /* The button size */
        .file-upload {
            height: 6vh;
        }

            .file-upload, .file-upload span {
                width: 15vh;
            }

                .file-upload input {
                    top: 0;
                    left: 0;
                    margin: 0;
                    font-size: 11px;
                    font-weight: bold;
                    /* Loses tab index in webkit if width is set to 0 */
                    opacity: 0;
                    width: 0;
                    /*filter: alpha(opacity=0);*/
                }

                .file-upload strong {
                    font: normal 12px Tahoma,sans-serif;
                    text-align: center;
                    vertical-align: middle;
                }

                .file-upload span {
                    top: 0;
                    left: 0;
                    display: block;
                    /* Adjust button text vertical alignment */
                    padding-top: 5px;
                    width: auto;
                    margin-left: auto;
                    margin-right: auto;
                }
    </style>

    <asp:Panel ID="Panel1" runat="server">
        <div class="card shadow mb-4">
        <div class="card-header py-3">
            <p class="text-primary m-0 fw-bold" style="font-size: 21px;">Claim Apply List</p>
        </div>
        <div style="position: absolute; margin-top: 16px; margin-right: 21px; top: 0; right: 0;">
            <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#modal" onclick="claimChange()">+</button>
        </div>
        <div class="card-body" style="overflow-y: hidden;">
            <div class="table-responsive">
                <asp:GridView ID="gvList" runat="server" CssClass="table table-bordered dataTable1" Width="100%" AutoGenerateColumns="false"
                    OnRowDataBound="OnRowDataBound" OnSelectedIndexChanged="gvList_SelectedIndexChanged"
                    EmptyDataText="No records has been added." DataKeyNames="ClaimID,ClaimStatus,ClaimDate,ClaimAmount">
                    <Columns>
                        <asp:TemplateField HeaderText="Claim ID" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblClaimID" runat="server" Text='<%# Eval("ClaimID")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("ClaimStatus")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Claim Apply Date" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblClaimDate" runat="server" Text='<%# Convert.ToDateTime(Eval("ClaimDate")).ToShortDateString()%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Claim Amount" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblClaimAmount" runat="server" Text='<%# "RM" + (Eval("ClaimAmount"))%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ButtonType="Image" ControlStyle-CssClass="button"
                            SelectImageUrl="~/Image/detail.png" ShowSelectButton="True"
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
                    <h5 class="modal-title">Apply New Claim</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" onclick="clearr()">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group" style="margin: 0px 3px 10px">
                        <span style="color: #FF0000">* </span>
                        <label for="recipient-name" class="col-form-label">Date From:</label>
                        <asp:TextBox ID="txt_dateFrom" runat="server" class="form-control" TextMode="Date"></asp:TextBox>
                        <asp:Label ID="lbl_dateFromError" Style="display: none" runat="server" ForeColor="Red" Font-Size="Smaller" Text="This field is required"></asp:Label>
                    </div>
                    <div class="form-group" style="margin: 0px 3px 10px">
                        <span style="color: #FF0000">* </span>
                        <label for="message-text" class="col-form-label">Date To:</label>
                        <asp:TextBox ID="txt_dateTo" runat="server" class="form-control" TextMode="Date"></asp:TextBox>
                        <asp:Label ID="lbl_dateToError" Style="display: none" runat="server" ForeColor="Red" Font-Size="Smaller" Text="This field is required"></asp:Label>
                    </div>
                    <div class="form-group" style="margin: 0px 3px 10px">
                        <span style="color: #FF0000">* </span>
                        <label for="message-text" class="col-form-label">Claim Type:</label>
                        <asp:DropDownList ID="ddlClaimType" runat="server" class="form-control" DataTextField="ClaimTypeDesc" DataValueField="ClaimTypeID" OnDataBound="ddlClaimType_DataBound" onchange="claimChange()"></asp:DropDownList>
                    </div>
                    <div class="form-group" style="margin: 0px 3px 10px">
                        <span style="color: #FF0000">* </span>
                        <label for="message-text" class="col-form-label">Claim Amount:</label>
                        <asp:TextBox ID="txt_Amount" runat="server" class="form-control" TextMode="Number" step="any"></asp:TextBox>
                        <asp:Label ID="lbl_Amount" Style="display: none" runat="server" ForeColor="Red" Font-Size="Smaller" Text="This field is required"></asp:Label>
                    </div>
                    <div class="form-group" style="margin: 0px 3px 10px">
                        <span style="color: #FF0000">* </span>
                        <label for="message-text" class="col-form-label">Claim Reason:</label>
                        <asp:TextBox ID="txt_Reason" runat="server" class="form-control"></asp:TextBox>
                        <asp:Label ID="lbl_Reason" Style="display: none" runat="server" ForeColor="Red" Font-Size="Smaller" Text="This field is required"></asp:Label>
                    </div>
                    <div class="form-group" style="margin: 0px 3px 10px">
                        <span style="color: #FF0000">* </span>
                        <label for="message-text" class="col-form-label">Claim Attachment:</label>
                        <asp:Image ID="imgAttachment" runat="server" alt="attachment" src="" ImageUrl="" Width="300px" Height="300px" Style="max-height: 300px; max-width: 300px; display: block; margin-left: auto; margin-right: auto; border: 1px solid black; margin-bottom: 10px;" />
                        <label id="lblFileUpload" class="file-upload">
                            <span><strong>Upload Image</strong></span>
                            <asp:FileUpload ID="FileUpload1" runat="server" onchange="fileValidation(this)" accept="image/*" name="image"></asp:FileUpload>
                        </label>
                        <asp:Label ID="lbl_Image" Style="display: none" runat="server" ForeColor="Red" Font-Size="Smaller" Text="Attachment is required"></asp:Label>
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

    <asp:Label ID="lblStoreClaimType" Style="display: none; width:0px; height:0px;" runat="server" Text=""></asp:Label>
    <asp:Label ID="lblStoreTotalClaimAmount" Style="display: none; width:0px; height:0px;" runat="server" Text=""></asp:Label>


    <script>
        var amountCanClaim = 0.0;

        function fileValidation(input) {

            if (typeof (input.files) != "undefined") {
                //var fileInput = document.getElementById('file');

                var filePath = input.value;

                // Allowing file type
                var allowedExtensions = /(\.jpg|\.jpeg|\.png)$/i;

                if (!allowedExtensions.exec(filePath)) {
                    alert('Image must be in .jpg, .jpeg, .png format only.');
                    fileInput.value = '';
                    return false;
                }
                else {

                    // Image preview
                    if (input.files && input.files[0]) {
                        var reader = new FileReader();
                        reader.onload = function (e) {
                            document.getElementById('<%= imgAttachment.ClientID %>').setAttribute("src", e.target.result);
                        }
                        reader.readAsDataURL(input.files[0]);
                    }
                }
            }
        }

        function claimChange() {
            var selectValue = document.getElementById('<%= ddlClaimType.ClientID %>').value
            var amountValue = document.getElementById('<%= lblStoreTotalClaimAmount.ClientID %>').textContent;
            selectValue = "Id=" + selectValue;

            var contentRetrieve = amountValue.split(selectValue)[1];
            contentRetrieve = contentRetrieve.split("Total=")[1];
            contentRetrieve = contentRetrieve.split(',')[0];
            var totalHadClaim = parseFloat(contentRetrieve).toFixed(2)

            var ddlClaimType2 = document.getElementById('<%= ddlClaimType.ClientID %>')
            var selectText2 = ddlClaimType2.options[ddlClaimType2.selectedIndex].text;
            selectText2 = "Type=" + selectText2;

            var claimTypeValue2 = document.getElementById('<%= lblStoreClaimType.ClientID %>').textContent;
            var contentRetrieve2 = claimTypeValue2.split(selectText2)[1];
            contentRetrieve2 = contentRetrieve2.split("Max=")[1];
            contentRetrieve2 = contentRetrieve2.split(',')[0];
            var num = parseFloat(contentRetrieve2).toFixed(2)

            amountCanClaim = parseFloat(num - totalHadClaim)
            var lblAmountError = document.getElementById('<%= lbl_Amount.ClientID %>');
            lblAmountError.textContent = "You are able to claim for RM" + amountCanClaim + " only for this claim type.";
            lblAmountError.style.display = 'block';
            lblAmountError.style.color = "black";
        }


        function requiredField() {
            var validate = true;
            var dateFrom = document.getElementById('<%= txt_dateFrom.ClientID %>');
            var dateTo = document.getElementById('<%= txt_dateTo.ClientID %>');
            var amount = document.getElementById('<%= txt_Amount.ClientID %>');
            var reason = document.getElementById('<%= txt_Reason.ClientID %>');
            var image = document.getElementById('<%= imgAttachment.ClientID %>')
            var lblDateFromError = document.getElementById('<%= lbl_dateFromError.ClientID %>');
            var lblDateToError = document.getElementById('<%= lbl_dateToError.ClientID %>');
            var lblAmountError = document.getElementById('<%= lbl_Amount.ClientID %>');
            var lblReasonError = document.getElementById('<%= lbl_Reason.ClientID %>');
            var lblImageError = document.getElementById('<%= lbl_Image.ClientID %>');
            

            if (dateFrom.value.trim() == "") {
                dateFrom.style.borderColor = "Red";
                lblDateFromError.style.display = 'block';
                validate = false;
            }
            else {
                dateFrom.style.borderColor = "";
                lblDateFromError.style.display = 'none';
            }
            if (dateTo.value.trim() == "") {
                dateTo.style.borderColor = "Red";
                lblDateToError.style.display = 'block';
                lblDateToError.textContent = "This field is required";
                validate = false;
            }
            else if (dateTo.value.trim() != "" && (dateTo.value < dateFrom.value)) {
                lblDateToError.textContent = "The date is invalid";
                dateTo.style.borderColor = "Red";
                lblDateToError.style.display = 'block';
            }
            else {
                dateTo.style.borderColor = "";
                lblDateToError.style.display = 'none';
            }

            if (amount.value.trim() == "") {
                amount.style.borderColor = "Red";
                lblAmountError.style.display = 'block';
                lblAmountError.style.color = "Red";
                lblAmountError.textContent = "This field is required";
                validate = false;
            }
            else if (amount.value.trim() != "" && (parseFloat(amount.value) > parseFloat(amountCanClaim))) {
                amount.style.borderColor = "Red";
                lblAmountError.style.display = 'block';
                lblAmountError.style.color = "Red";
                lblAmountError.textContent = "The amount entered exceeds the maximum claim amount(RM" + amountCanClaim + ")" + " that can be claimed.";
                validate = false;
            }
            else if (amount.value.trim() != "") {
                amount.style.borderColor = "";
                lblAmountError.style.display = 'none';
            }
            else {
                lblAmountError.textContent = "You are able to claim for RM" + amountCanClaim + " only for this claim type.";
                lblAmountError.style.display = 'block';
                lblAmountError.style.color = "black";
            }


            if (reason.value.trim() == "") {
                reason.style.borderColor = "Red";
                lblReasonError.style.display = 'block';
                validate = false;
            }
            else {
                reason.style.borderColor = "";
                lblReasonError.style.display = 'none';
            }
            if (image.getAttribute('src') == "") {
                image.style.borderColor = "Red";
                lblImageError.style.display = 'block';
                validate = false;
            }
            else {
                image.style.borderColor = "";
                lblImageError.style.display = 'none';
            }


            if (validate) {
                document.getElementById("<%= btnConfirm.ClientID %>").click();
            }
        }

        function clearr() {
            var dateFrom = document.getElementById('<%= txt_dateFrom.ClientID %>');
            var dateTo = document.getElementById('<%= txt_dateTo.ClientID %>');
            var amount = document.getElementById('<%= txt_Amount.ClientID %>');
            var reason = document.getElementById('<%= txt_Reason.ClientID %>');
            var lblDateFromError = document.getElementById('<%= lbl_dateFromError.ClientID %>');
            var lblDateToError = document.getElementById('<%= lbl_dateToError.ClientID %>');
            var lblAmountError = document.getElementById('<%= lbl_Amount.ClientID %>');
            var lblReasonError = document.getElementById('<%= lbl_Reason.ClientID %>');
            var lblImageError = document.getElementById('<%= lbl_Image.ClientID %>');
            var image = document.getElementById('<%= imgAttachment.ClientID %>')
            var uploadFile = document.getElementById('<%=FileUpload1.ClientID %>');

            dateFrom.value = "";
            dateTo.value = "";
            amount.value = "";
            reason.value = "";
            dateFrom.style.borderColor = "";
            lblDateFromError.style.display = 'none';
            dateTo.style.borderColor = "";
            lblDateToError.style.display = 'none';
            amount.style.borderColor = "";
            lblAmountError.style.display = 'none';
            reason.style.borderColor = "";
            lblReasonError.style.display = 'none';
            lblImageError.style.display = 'none';
            image.setAttribute("src", '');
            image.style.borderColor = "";
            uploadFile.value = '';
        }

    </script>





</asp:Content>
