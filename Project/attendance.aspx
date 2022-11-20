<%@ Page Title="Attendance" Language="C#" MasterPageFile="~/Project/Site1.Master" AutoEventWireup="true" CodeBehind="attendance.aspx.cs" Inherits="FYP.Project.attendance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<%--    <style>
        .card {
            box-shadow: 1px 1px 1px rgb(0 0 0 / 10%), -1px 0 1px rgb(0 0 0 / 5%);
        }

        .gvbutton {
            max-width: 20px;
            max-height: 20px;
            margin: 0px 10px;
        }

            .gvbutton:hover {
                transform: scale(1.2);
            }

        .btnDateStlye {
            background-color: white;
            border: 1px solid #cccccc;
        }

            .btnDateStlye:hover {
                background-color: #ededed;
                cursor: pointer;
            }
    </style>--%>

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
                                <h2 id="countPresent" runat="server" style="color: white">8</h2>
                                <span style="color: white; font-size: larger">Present</span>
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
                                <h2 id="CountOnLeave" runat="server" style="color: white">0</h2>
                                <span style="color: white; font-size: larger">On leave</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <div class="card shadow mb-4">
        <div class="card-header py-3">
            <p class="text-primary m-0 fw-bold" style="font-size: 21px;">Attendance</p>
        </div>
        <div style="position: absolute; margin-top: 16px; margin-right: 21px; top: 0; right: 0;">
            <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#modal">+</button>
        </div>
        <div class="card-body" style="overflow-y: hidden;">
            <div class="table-responsive">
                <div style="text-align: center">
                    <asp:Button ID="btnLeft" runat="server" Text="<" OnClick="minusDate" CssClass="btnDateStlye" />
                    <asp:TextBox ID="txtSelectDate" runat="server" AutoPostBack="true" type="Date" OnTextChanged="txtSelectDate_TextChanged" Style="border: 1px solid #cccccc"></asp:TextBox>
                    <asp:Button ID="btnRight" runat="server" Text=">" OnClick="addDate" CssClass="btnDateStlye" />
                </div>
                <asp:GridView ID="gvList" runat="server" CssClass="table table-bordered dataTable1" Width="100%" AutoGenerateColumns="false"
                    OnRowDataBound="OnRowDataBound" OnRowEditing="OnRowEditing" OnRowCancelingEdit="OnRowCancelingEdit"
                    OnRowUpdating="OnRowUpdating" OnRowDeleting="OnRowDeleting" OnSelectedIndexChanged="OnRowSelect" EmptyDataText="No records has been added." DataKeyNames="AttendanceID,Name,AttendanceStatus,
                    AttendanceTimeIn,AttendanceTimeOut">
                    <Columns>
                        <asp:TemplateField HeaderText="Attendance ID" ItemStyle-Width="16.5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblAttendanceID" runat="server" Text='<%# "A" + Eval("AttendanceID")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblAttendanceID" runat="server" Text='<%# "A" + Eval("AttendanceID") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Staff Name" ItemStyle-Width="16.5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblStaffID" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblStaffID" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="16.5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblAttendanceStatus" runat="server" Text='<%# Eval("AttendanceStatus") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:Label ID="lblAttendanceStatus" runat="server" Text='<%# Eval("AttendanceStatus") %>'></asp:Label>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Time In" ItemStyle-Width="16.5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblTimeIn" runat="server" Text='<%# Eval("AttendanceTimeIn")  %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtTimeIn" runat="server" Text='<%# Eval("AttendanceTimeIn") %>' TextMode="Time"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Time Out" ItemStyle-Width="16.5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblTimeOut" runat="server" Text='<%# Eval("AttendanceTimeOut") %>'></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtTimeOut" runat="server" Text='<%# Eval("AttendanceTimeOut") %>' TextMode="Time"></asp:TextBox>
                            </EditItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ButtonType="Image" ShowEditButton="true" EditImageUrl="~/Image/editing.png" ControlStyle-CssClass="gvbutton"
                            UpdateImageUrl="~/Image/tick.png" CancelImageUrl="~/Image/cancel.png" DeleteImageUrl="~/Image/bin.png" ShowDeleteButton="true" ShowSelectButton="true" SelectImageUrl="~/Image/detail.png"
                            HeaderText="Actions" ItemStyle-Width="16.5%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" />
                    </Columns>
                </asp:GridView>

            </div>
        </div>
    </div>

    <div class="modal fade" id="modal" tabindex="-1" role="dialog" aria-hidden="true" data-backdrop="static">
        <div class="modal-dialog modal-dialog-centered" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Add New Attendance</h5>
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close" onclick="clearr()">
                        <span aria-hidden="true">&times;</span>
                    </button>
                </div>
                <div class="modal-body">
                    <div class="form-group" style="margin: 0px 3px 10px">
                        <span style="color: #FF0000">* </span>
                        <label for="recipient-name" class="col-form-label">Staff Name:</label>
                        <asp:DropDownList ID="ddlStaffName" runat="server" DataSourceID="SqlDataSource1" CssClass="form-control" DataTextField="Name" DataValueField="Staff_ID"></asp:DropDownList>
                        <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>" SelectCommand="SELECT [Name], [Staff_ID] FROM [Staff]"></asp:SqlDataSource>
                    </div>
                    <div class="form-group" style="margin: 0px 3px 10px">
                        <span style="color: #FF0000">* </span>
                        <label for="message-text" class="col-form-label">Date:</label>
                        <asp:TextBox ID="txtDate" runat="server" class="form-control" TextMode="Date"></asp:TextBox>
                        <asp:Label ID="lbldateError" Style="display: none" runat="server" ForeColor="Red" Font-Size="Smaller" Text="This field is required"></asp:Label>
                    </div>

                    <div class="form-group" style="margin: 0px 3px 10px">
                        <span style="color: #FF0000">* </span>
                        <label for="message-text" class="col-form-label">Time In:</label>
                        <asp:TextBox ID="txtTimeIn" runat="server" class="form-control" TextMode="Time"></asp:TextBox>
                        <asp:Label ID="lblTimeInError" Style="display: none" runat="server" ForeColor="Red" Font-Size="Smaller" Text="This field is required"></asp:Label>
                    </div>

                    <div class="form-group" style="margin: 0px 3px 10px">
                        <span style="color: #FF0000">* </span>
                        <label for="message-text" class="col-form-label">Time Out:</label>
                        <asp:TextBox ID="txtTimeOut" runat="server" class="form-control" TextMode="Time"></asp:TextBox>
                        <asp:Label ID="lblTimeOutError" Style="display: none" runat="server" ForeColor="Red" Font-Size="Smaller" Text="This field is required"></asp:Label>
                    </div>
                </div>
                <div class="modal-footer">
                    <button id="btnReset" type="button" class="btn btn-secondary" data-dismiss="modal" onclick="clearr()">Cancel</button>
                    <button type="button" class="btn btn-primary" id="btnValidate" onclick="requiredField()">Confirm</button>
                    <asp:Button ID="btnConfirm" Style="display: none" runat="server" Text="Button" OnClick="addAttendance" CausesValidation="False" />

                </div>
            </div>
        </div>
    </div>
    <script type="text/javascript">
        function requiredField() {
            var validate = true;
            var date = document.getElementById('<%= txtDate.ClientID %>');
            var timeIn = document.getElementById('<%= txtTimeIn.ClientID %>');
            var timeOut = document.getElementById('<%= txtTimeOut.ClientID %>');
            var lblDateError = document.getElementById('<%= lbldateError.ClientID %>');
            var lblTimeInError = document.getElementById('<%= lblTimeInError.ClientID %>');
            var lblTimeOutError = document.getElementById('<%= lblTimeOutError.ClientID %>');


            if (date.value.trim() == "") {
                date.style.borderColor = "Red";
                lblDateError.style.display = 'block';
                validate = false;
            }
            else {
                date.style.borderColor = "";
                lblDateError.style.display = 'none';
            }
            if (timeIn.value.trim() == "") {
                timeIn.style.borderColor = "Red";
                lblTimeInError.style.display = 'block';
                validate = false;
            }
            else {
                timeIn.style.borderColor = "";
                lblTimeInError.style.display = 'none';
            }
            if (timeOut.value.trim() == "") {
                timeOut.style.borderColor = "Red";
                lblTimeOutError.style.display = 'block';
                validate = false;
            }
            else {
                timeOut.style.borderColor = "";
                lblTimeOutError.style.display = 'none';
            }
            if (validate) {
                document.getElementById("<%= btnConfirm.ClientID %>").click();
            }
        }
        function clearr() {
            var date = document.getElementById('<%= txtDate.ClientID %>');
            var timeIn = document.getElementById('<%= txtTimeIn.ClientID %>');
            var timeOut = document.getElementById('<%= txtTimeOut.ClientID %>');
            var lblDateError = document.getElementById('<%= lbldateError.ClientID %>');
            var lblTimeInError = document.getElementById('<%= lblTimeInError.ClientID %>');
            var lblTimeOutError = document.getElementById('<%= lblTimeOutError.ClientID %>');

            date.value = "";
            timeIn.value = "";
            timeOut.value = "";
            date.style.borderColor = "";
            lblDateError.style.display = 'none';
            timeIn.style.borderColor = "";
            lblTimeInError.style.display = 'none';
            timeOut.style.borderColor = "";
            lblTimeOutError.style.display = 'none';
        }


    </script>

</asp:Content>
