<%@ Page Title="" Language="C#" MasterPageFile="~/Project/Site1.Master" AutoEventWireup="true" CodeBehind="LeaveStaff.aspx.cs" Inherits="FYP.Project.LeaveStaff" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="style.css">
    <div class="card-deck" style="margin-bottom: 25px">
        <div class="col-xl-3 col-sm-6 col-12">
            <div class="card">
                <div class="card-content" style="background-color: #66a2e5e8">
                    <div class="card-body">
                        <div class="media d-flex">
                            <div class="align-self-center">
                                <i class="fas fa-plane fa-3x float-left" style="color: white"></i>
                            </div>
                            <div class="media-body text-right">
                                <h2 id="countCasualLeave" runat="server" style="color: white">8 Days</h2>
                                <span style="color: white; font-size: larger">Casual Leave Available</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="col-xl-3 col-sm-6 col-12">
            <div class="card">
                <div class="card-content" style="background-color: #82d1dd">
                    <div class="card-body">
                        <div class="media d-flex">
                            <div class="align-self-center">
                                <i class="fas fa-hospital fa-3x float-left" style="color: white"></i>
                            </div>
                            <div class="media-body text-right">
                                <h2 id="countSickLeave" runat="server" style="color: white">10 Days</h2>
                                <span style="color: white; font-size: larger">Sick Leave Available</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
    <div class="card shadow mb-4">
        <div class="card-header py-3">
            <p class="text-primary m-0 fw-bold" style="font-size: 21px;">Leave</p>
        </div>
        <div style="position: absolute; margin-top: 16px; margin-right: 21px; top: 0; right: 0;">
            <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#modal" style="padding: 6px 13px">+</button>
        </div>
        <div class="card-body" style="overflow-y: hidden;">
            <div class="table-responsive">
                <asp:GridView ID="gvList" runat="server" CssClass="table table-bordered dataTable1" Width="100%" AutoGenerateColumns="false"
                    OnRowDataBound="OnRowDataBound" OnSelectedIndexChanged="OnRowSelect" EmptyDataText="No records has been added." DataKeyNames="">
                    <Columns>
                        <asp:TemplateField HeaderText="Leave Type" ItemStyle-Width="18%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblLeaveType" runat="server" Text='<%# Eval("LeaveType") %>'></asp:Label>
                                <asp:Label ID="lblLeaveID" runat="server" Text='<%# Eval("LeaveAppID") %>' Style="display: none"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Leave From" ItemStyle-Width="18%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblLeaveStartDate" runat="server" Text='<%# Eval("LeaveDateStart","{0:d}")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Leave To" ItemStyle-Width="18%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblLeaveEndDate" runat="server" Text='<%# Eval("LeaveDateEnd","{0:d}") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Total Day" ItemStyle-Width="18%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblTotalDay" runat="server" Text='<%# Eval("TotalDay")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="18%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblLeaveStatus" runat="server" Text='<%# Eval("LeaveStatus") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ButtonType="Image" ControlStyle-CssClass="gvbutton"
                            ShowSelectButton="true" SelectImageUrl="~/Image/detail.png"
                            HeaderText="Actions" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" CausesValidation="false" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    <div class="modal fade" id="modal" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Apply Leave</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" onclick="clearr()">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group" style="margin: 0px 3px 10px">
                        <span style="color: #FF0000">* </span>
                        <label for="recipient-name" class="col-form-label">Staff Name:</label>
                        <asp:DropDownList ID="ddlLeaveType" runat="server" CssClass="form-control">
                            <asp:ListItem Value="Casual Leave">Casual Leave</asp:ListItem>
                            <asp:ListItem Value="Sick Leave">Sick Leave</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div class="form-group" style="margin: 0px 3px 10px">
                        <span style="color: #FF0000">* </span>
                        <label for="message-text" class="col-form-label">Leave Date:</label>
                        <asp:TextBox ID="txtDate" runat="server" class="form-control" TextMode="Date"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqvDate" runat="server" ControlToValidate="txtDate" ForeColor="Red" Font-Size="Smaller" Display="Dynamic" ErrorMessage="This field is required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    </div>

                    <div class="form-group" style="margin: 0px 3px 10px">
                        <span style="color: #FF0000">* </span>
                        <label for="message-text" class="col-form-label">Total Leave Day:</label>
                        <asp:TextBox ID="txtLeaveDay" runat="server" class="form-control" TextMode="Number" MaxLength="2" onkeydown="return ((event.keyCode<=57 && event.keyCode>=48) && event.keyCode!=32 || event.keyCode == 8);"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqvLeaveDay1" runat="server" ControlToValidate="txtLeaveDay" ForeColor="Red" Font-Size="Smaller" Display="Dynamic" ErrorMessage="This field is required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        <asp:RangeValidator ID="rqvLeaveDay2" runat="server" ControlToValidate="txtLeaveDay" Type="Integer" ErrorMessage="Input must between 1 to 14" MinimumValue=1 MaximumValue="14" SetFocusOnError="true" ForeColor="Red" Font-Size="Smaller" Display="Dynamic"></asp:RangeValidator>
                    </div>

                    <div class="form-group" style="margin: 0px 3px 10px">
                        <span style="color: #FF0000">* </span>
                        <label for="message-text" class="col-form-label">Attachment</label>
                        <asp:FileUpload ID="attachment" runat="server" CssClass="form-control-file" accept="image/*" name="image"></asp:FileUpload>
                        <asp:RequiredFieldValidator ID="rqvField1" runat="server" ControlToValidate="attachment" ForeColor="Red" Font-Size="Smaller" Display="Dynamic" ErrorMessage="This field is required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="rqvField2" runat="server" Display="Dynamic" ErrorMessage="Image must be in .jpg, .jpeg, .png format only." ForeColor="Red" Font-Size="Smaller" ValidationExpression="^(([a-zA-Z]:)|(\\{2}\w+)\$?)(\\(\w[\w].*))+(.jpg|.JPG|.jpeg|.JPEG|.png|.PNG)$" ControlToValidate="attachment"></asp:RegularExpressionValidator>
                       
                    </div>

                    <div class="form-group" style="margin: 0px 3px 10px">
                        <span style="color: #FF0000">* </span>
                        <label for="message-text" class="col-form-label">Leave Reason</label>
                        <asp:TextBox ID="txtReason" runat="server" class="form-control" TextMode="MultiLine"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rqvReason" runat="server" ControlToValidate="txtReason" ForeColor="Red" Font-Size="Smaller" Display="Dynamic" ErrorMessage="This field is required" SetFocusOnError="true"></asp:RequiredFieldValidator>
                    </div>

                </div>
                <div class="modal-footer">
                    <button id="btnReset" type="button" class="btn btn-secondary" data-dismiss="modal" onclick="clearr()">Cancel</button>
                    <asp:Button ID="btnConfirm" runat="server" class="btn btn-primary" Text="Apply" OnClick="btnConfirm_Click" />

                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function clearr() {
            document.getElementById("<%= rqvDate.ClientID%>").style.display = "none";
            document.getElementById("<%= rqvLeaveDay1.ClientID%>").style.display = "none";
            document.getElementById("<%= rqvLeaveDay2.ClientID%>").style.display = "none";
            document.getElementById("<%= rqvField1.ClientID%>").style.display = "none";
            document.getElementById("<%= rqvField2.ClientID%>").style.display = "none";
            document.getElementById("<%= rqvReason.ClientID%>").style.display = "none";

            document.getElementById("<%= txtDate.ClientID%>").value = "";
            document.getElementById("<%= txtLeaveDay.ClientID%>").value = "";
            document.getElementById("<%= txtReason.ClientID%>").value = "";
            document.getElementById("<%= attachment.ClientID%>").value = "";
            document.getElementById("<%= ddlLeaveType.ClientID%>").selectedIndex = 0;
        }
    </script>
</asp:Content>
