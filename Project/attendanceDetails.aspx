<%@ Page Title="" Language="C#" MasterPageFile="~/Project/Site1.Master" AutoEventWireup="true" CodeBehind="attendanceDetails.aspx.cs" Inherits="FYP.Project.attendanceDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
     <link rel="stylesheet" href="style.css" />
     <div class="profilePanel">

        <header class="pb-2 fixed-profile-header" style="border-bottom: 1px solid #c9c9c9;">
            <asp:LinkButton ID="btnBack" runat="server" CssClass="alignright btn btn-light"  CausesValidation="False" OnClick="btnBack_Click" ><i class="fa fa-angle-left"></i> Back </asp:LinkButton>
            <div style="clear: both;"></div>
        </header>

        <asp:Panel ID="attendancePanel" runat="server" CssClass="profileContentPanel mt-2 mb-2" ScrollBars="Auto">
            <label class="subtitle">Attendance Details</label>

            <asp:Image ID="imgProfile" runat="server" class="profileImage" alt="profileImage" ClientIDMode="Static" ImageUrl="~/Image/defaultProfileImg.png" />

            <div class="form-group">
                 <div class="rows">
                    <label class="column" style="margin-right: 2%">Attendance ID</label>
                    <label class="column">Attendance Date</label>                                   
                </div>
                <div class="rows">
                    <asp:TextBox ID="txtAttdID" runat="server" CssClass="form-control column" Enabled="False" Style="margin-right: 2%"></asp:TextBox>
                    <asp:TextBox ID="txtAttedDate" runat="server" CssClass="form-control column" Enabled="False"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <div class="rows">
                    <label class="column" style="margin-right: 2%">Staff ID</label>
                    <label class="column">Staff Name</label>                                   
                </div>
                <div class="rows">
                    <asp:TextBox ID="txtStaffID" runat="server" CssClass="form-control column" Enabled="False" Style="margin-right: 2%"></asp:TextBox>
                    <asp:TextBox ID="txtStaffName" runat="server" CssClass="form-control column" Enabled="False"></asp:TextBox>
                </div>
            </div>

             <div class="form-group">
                <div class="rows">
                    <label class="column" style="margin-right: 2%">Attendance Status</label>
                    <label class="column">Arrival</label>                                   
                </div>
                <div class="rows">
                    <asp:TextBox ID="txtAttdStatus" runat="server" CssClass="form-control column" Enabled="False" Style="margin-right: 2%"></asp:TextBox>
                    <asp:TextBox ID="txtIsLate" runat="server" CssClass="form-control column" Enabled="False"></asp:TextBox>
                </div>
            </div>

            <div class="form-group">
                <div class="rows">
                    <label class="column" style="margin-right: 2%">Time In</label>
                    <label class="column">Time Out</label>                                   
                </div>
                <div class="rows">
                    <asp:TextBox ID="txtTimeIn" runat="server" CssClass="form-control column" Enabled="False" Style="margin-right: 2%"></asp:TextBox>
                    <asp:TextBox ID="txtTimeOut" runat="server" CssClass="form-control column" Enabled="False"></asp:TextBox>
                </div>
            </div>
        
            <div class="form-group">
                <div class="rows">
                    <label class="column" style="margin-right: 2%">Working Hour</label>
                    <label class="column">Overtime</label>                                   
                </div>
                <div class="rows">
                    <asp:TextBox ID="txtWorkHour" runat="server" CssClass="form-control column" Enabled="False" Style="margin-right: 2%"></asp:TextBox>
                    <asp:TextBox ID="txtOvertime" runat="server" CssClass="form-control column" Enabled="False"></asp:TextBox>
                </div>
            </div>
        </asp:Panel>
    </div>

</asp:Content>
