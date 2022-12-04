<%@ Page Title="" Language="C#" MasterPageFile="~/Project/Site1.Master" AutoEventWireup="true" CodeBehind="payroll.aspx.cs" Inherits="FYP.Project.payroll" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="style.css">
    <%-- <div class="card-deck" style="margin-bottom: 25px">
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
                                <span style="color: white; font-size: larger">Processed</span>
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
                                <i class="fas fa-hourglass-half fa-3x float-left" style="color: white"></i>
                            </div>
                            <div class="media-body text-right">
                                <h2 id="countOnLeave" runat="server" style="color: white">10</h2>
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
                                <i class="fas fa-clock fa-3x float-left" style="color: white"></i>
                            </div>
                            <div class="media-body text-right">
                                <h2 id="countAbsent" runat="server" style="color: white">0</h2>
                                <span style="color: white; font-size: larger">Overdue</span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>--%>

    <div class="card shadow mb-4">
        <div class="card-header py-3">
            <p class="text-primary m-0 fw-bold" style="font-size: 21px;">Payroll List</p>
        </div>
        <div style="position: absolute; margin-top: 16px; margin-right: 21px; top: 0; right: 0;">
            <asp:Button ID="btnAdd" runat="server" class="btn btn-primary" Text="+" CausesValidation="False" OnClick="btnAdd_Click" />
        </div>
        <div class="card-body" style="overflow-y: hidden;">
            <div class="table-responsive">
                <div style="text-align: center" >
                    <asp:Button ID="btnLeftWeek" runat="server" Text="<" OnClick="minusWeek" CssClass="btnDateStlye" />
                    <asp:TextBox ID="txtSelectWeek" runat="server" AutoPostBack="true" TextMode="Week" OnTextChanged="txtSelectWeek_TextChanged" Style="border: 1px solid #cccccc"></asp:TextBox>
                    <asp:Button ID="btnRightWeek" runat="server" Text=">" OnClick="addWeek" CssClass="btnDateStlye" />
                    <asp:Button ID="btnLeftMonth" runat="server" Text="<" OnClick="minusMonth" CssClass="btnDateStlye" />
                    <asp:TextBox ID="txtSelectMonth" runat="server" AutoPostBack="true" TextMode="Month" OnTextChanged="txtSelectMonth_TextChanged" Style="border: 1px solid #cccccc"></asp:TextBox>
                    <asp:Button ID="btnRightMonth" runat="server" Text=">" OnClick="addMonth" CssClass="btnDateStlye" />
                    <asp:DropDownList ID="ddlMonthWeek" runat="server" OnSelectedIndexChanged="ddlMonthWeek_SelectedIndexChanged" AutoPostBack="True" CssClass="form-control" style="float: right; width:auto">
                        <asp:ListItem>All</asp:ListItem>
                        <asp:ListItem>Monthly</asp:ListItem>
                        <asp:ListItem>Weekly</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div style="clear: both;" class="mb-2"></div>
                <asp:GridView ID="gvList" runat="server" CssClass="table table-bordered dataTable1" Width="100%" AutoGenerateColumns="false"
                    OnRowDataBound="OnRowDataBound" EmptyDataText="No records has been added." DataKeyNames="PayrollListID,StringDate,Status,Payperiod"
                    OnSelectedIndexChanged="gvList_SelectedIndexChanged">
                    <Columns>
                        <asp:TemplateField HeaderText="Payroll Date" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("StringDate") %>'></asp:Label>
                                <asp:Label ID="lblPayrollListID" runat="server" Text='<%#  Eval("PayrollListID")%>' Style="display: none"></asp:Label>
                                <asp:Label ID="lblRealDate" runat="server" Text='<%#  Eval("Date")%>' Style="display: none"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Pay Period" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblPayperiod" runat="server" Text='<%# Eval("Payperiod") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ButtonType="Image" ControlStyle-CssClass="gvbutton"
                            ShowSelectButton="true" SelectImageUrl="~/Image/detail.png"
                            HeaderText="Actions" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <asp:Panel ID="addPayrollListPopup" runat="server" class="forgetPwModal" Visible="false">
        <div class="forgetPwModal-content">
            <div class="modal-header">
                <h5>Add New Payroll List</h5>
                <asp:LinkButton type="button" ID="btnRejectClose" class="close" runat="server" OnClick="btnRejectClose_Click" CausesValidation="False">
                <span>&times;</span>
                </asp:LinkButton>
            </div>
            <div style="padding: 20px;">
                <div class="form-group" style="margin: 0px 3px 10px">
                    <label class="form-label">Pay Period</label>
                    <asp:DropDownList ID="ddlPayperiodPopUp" runat="server" OnSelectedIndexChanged="ddlPayperiodPopUp_SelectedIndexChanged" CssClass="form-control" AutoPostBack="True">
                        <asp:ListItem>Monthly</asp:ListItem>
                        <asp:ListItem>Weekly</asp:ListItem>
                    </asp:DropDownList>
                </div>
                <div class="form-group" style="margin: 0px 3px 10px">
                    <span style="color: #FF0000">*</span>
                    <label class="form-label">From Date</label>
                    <asp:TextBox ID="txtDatePopUp" runat="server" TextMode="Month" class="form-control" />
                    <asp:Label ID="lbl_DatePopUp" Style="display: none" runat="server" ForeColor="Red" Font-Size="Smaller" Text="This field is required"></asp:Label>
                </div>
            </div>
            <div class="modal-footer">
                <asp:Button ID="btnSubmitPopUp" runat="server" Text="Submit" class="form-control btn btn-primary rounded submit px-3" OnClientClick="return requiredField()" OnClick="btnSubmit_Click" />
            </div>
        </div>
    </asp:Panel>

<%--    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
    <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>--%>

    <script type="text/javascript">

        var modal = document.getElementById('<%= addPayrollListPopup.ClientID %>');

        // When the user clicks the button, open the modal 
        if (modal !== null) {
            modal.style.display = "block";
            document.getElementById('<%= addPayrollListPopup.ClientID %>').value = "";
        }

        function requiredField() {
            var validate = true;
            if (document.getElementById('<%= txtDatePopUp.ClientID %>').value.trim() == "") {
                document.getElementById('<%= txtDatePopUp.ClientID %>').style.borderColor = "Red";
                document.getElementById('<%= lbl_DatePopUp.ClientID %>').style.display = 'block';
                validate = false;
            }
            else {
                document.getElementById('<%= txtDatePopUp.ClientID %>').style.borderColor = "";
                document.getElementById('<%= lbl_DatePopUp.ClientID %>').style.display = 'none';
            }
            return validate
        }
    </script>

</asp:Content>
