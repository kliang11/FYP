<%@ Page Title="" Language="C#" MasterPageFile="~/Project/Site1.Master" AutoEventWireup="true" CodeBehind="attendanceStaff.aspx.cs" Inherits="FYP.Project.attendanceStaff" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="style.css">
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
                                <i class="fas fa-plane fa-3x float-left" style="color: white"></i>
                            </div>
                            <div class="media-body text-right">
                                <h2 id="countOnLeave" runat="server" style="color: white">10</h2>
                                <span style="color: white; font-size: larger">On Leave</span>
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
                                <h2 id="countAbsent" runat="server" style="color: white">0</h2>
                                <span style="color: white; font-size: larger">Absent</span>
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
            <asp:ImageButton ID="ImageButton1" runat="server" OnClick="btnReport_Click" CssClass="btn btn-primary" OnClientClick="SetTarget();" ImageUrl="~/Image/download.png" Style="padding: 10px 11px;" />
        </div>
        <div class="card-body" style="overflow-y: hidden;">
            <div class="table-responsive">
                <div style="text-align: center">
                    <asp:Button ID="btnLeft" runat="server" Text="<" OnClick="minusMonth" CssClass="btnDateStlye" />
                    <asp:TextBox ID="txtSelectMonth" runat="server" AutoPostBack="true" TextMode="Month" OnTextChanged="txtSelectMonth_TextChanged" Style="border: 1px solid #cccccc"></asp:TextBox>
                    <asp:Button ID="btnRight" runat="server" Text=">" OnClick="addMonth" CssClass="btnDateStlye" />
                </div>
                <asp:GridView ID="gvList" runat="server" CssClass="table table-bordered dataTable1" Width="100%" AutoGenerateColumns="false"
                    OnRowDataBound="OnRowDataBound" OnSelectedIndexChanged="OnRowSelect" EmptyDataText="No records has been added." DataKeyNames="AttendanceDate,AttendanceID,WorkingHour,IsLate,
                    AttendanceTimeIn,AttendanceTimeOut,AttendanceStatus">
                    <Columns>
                        <asp:TemplateField HeaderText="Attendance Date" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblAttendanceDate" runat="server" Text='<%# Eval("AttendanceDate","{0:d}") %>'></asp:Label>
                                <asp:Label ID="lblAttendanceID" runat="server" Text='<%#  Eval("AttendanceID")%>' Style="display: none"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Time In" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblTimeIn" runat="server" Text='<%# Eval("AttendanceTimeIn")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Time Out" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblTimeOut" runat="server" Text='<%# Eval("AttendanceTimeOut") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Working Hour" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblStaffWorkingHour" runat="server" Text='<%# Eval("WorkingHour")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblAttendanceStatus" runat="server" Text='<%# Eval("AttendanceStatus") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Arrival" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblStaffArrival" runat="server" Text='<%# Eval("IsLate") %>'></asp:Label>
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

    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
    </script>
</asp:Content>
