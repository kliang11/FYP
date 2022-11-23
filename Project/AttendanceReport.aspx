<%@ Page Title="" Language="C#" MasterPageFile="~/Project/Site1.Master" AutoEventWireup="true" CodeBehind="AttendanceReport.aspx.cs" Inherits="FYP.Project.AttendanceReport1" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="style.css" />
    <div class="profilePanel">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:Panel ID="reportPanel" runat="server" CssClass="profileContentPanel mt-2 mb-2" ScrollBars="Auto">
            <label class="subtitle">Attendance Report</label>

            <div class="form-group" style="margin-top: 30px">
                <label>Type</label>
                <select id="ddlSelect" onchange="GetSelectedTextValue(this)" class="form-control" runat="server">
                    <option value="">- Select -</option>
                    <option value="W">Weekly</option>
                    <option value="M">Monthly</option>
                </select>
                <asp:Label ID="lblSelectError" Style="display: none" runat="server" ForeColor="Red" Font-Size="Smaller" Text="Selection is required"></asp:Label>
                <asp:RequiredFieldValidator ID="rqvSelect" ControlToValidate="ddlSelect" runat="server" ErrorMessage="Selection is required" ForeColor="Red" Font-Size="Smaller" Display="Static"></asp:RequiredFieldValidator>
            </div>

            <div class="form-row">
                <div class="form-group col-md-6">
                    <label>Select Month</label>
                    <asp:TextBox ID="txtMonth" runat="server" CssClass="form-control" TextMode="Month"></asp:TextBox>
                    <asp:Label ID="lblMonthError" Style="display: none" runat="server" ForeColor="Red" Font-Size="Smaller" Text="This field is required"></asp:Label>
                    <asp:RequiredFieldValidator ID="rqvMonth" ControlToValidate="txtMonth" runat="server" ErrorMessage="This field is required" ForeColor="Red" Font-Size="Smaller" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group col-md-6">
                    <label>Select Start Date</label>
                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    <label style="font-weight: lighter; font-size: smaller">7 days will be added as end date</label>
                    <asp:Label ID="lblDateError" Style="display: none" runat="server" ForeColor="Red" Font-Size="Smaller" Text="This field is required"></asp:Label>
                    <asp:RequiredFieldValidator ID="rqvDate" ControlToValidate="txtDate" runat="server" ErrorMessage="This field is required" ForeColor="Red" Font-Size="Smaller" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </div>

            <asp:Button runat="server" Text="Generate Report" OnClick="btnGenerateReport_Click" ID="btnGenerateReport" OnClientClick="SetTarget();" class="btn btn-primary" Style="float: right" />
            <button type="button" class="btn btn-secondary" id="btnReset" onclick="Reset()" style="float: right; margin-right: 10px">Reset</button>

        </asp:Panel>
    </div>

    <CR:CrystalReportViewer ID="CrystalReportViewer1" runat="server" AutoDataBind="true" />


    <script type="text/javascript">
        function SetTarget() {
            document.forms[0].target = "_blank";
        }
        function GetSelectedTextValue(ddlSelect) {
            var selectedValue = ddlSelect.value;
            if (selectedValue == "M") {
                document.getElementById('<%= txtDate.ClientID %>').disabled = true;
                document.getElementById('<%= txtMonth.ClientID %>').disabled = false;
                document.getElementById("<%= rqvMonth.ClientID%>").enabled = true;
                document.getElementById("<%= rqvDate.ClientID%>").style.visibility = "hidden";
                document.getElementById("<%= rqvMonth.ClientID%>").style.visibility = "visible";
                document.getElementById("<%= rqvDate.ClientID%>").enabled = false;
            }
            else if (selectedValue == "W") {
                document.getElementById('<%= txtDate.ClientID %>').disabled = false;
                document.getElementById('<%= txtMonth.ClientID %>').disabled = true;
                document.getElementById("<%= rqvMonth.ClientID%>").style.visibility = "hidden";
                document.getElementById("<%= rqvDate.ClientID%>").style.visibility = "visible";
                document.getElementById("<%= rqvMonth.ClientID%>").enabled = false;
                document.getElementById("<%= rqvDate.ClientID%>").enabled = true;
            }
            else {
                document.getElementById('<%= txtDate.ClientID %>').disabled = false;
                document.getElementById('<%= txtMonth.ClientID %>').disabled = false;
            }
        }
        function Reset() {
            document.getElementById('<%= txtDate.ClientID %>').disabled = false;
            document.getElementById('<%= txtMonth.ClientID %>').disabled = false;
            document.getElementById("<%= rqvSelect.ClientID%>").style.visibility = "hidden";
            document.getElementById("<%= rqvMonth.ClientID%>").style.visibility = "hidden";
            document.getElementById("<%= rqvDate.ClientID%>").style.visibility = "hidden";
            ddlSelect.Reset();
        }
    </script>
</asp:Content>
