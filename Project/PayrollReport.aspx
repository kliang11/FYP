<%@ Page Title="" Language="C#" MasterPageFile="~/Project/Site1.Master" AutoEventWireup="true" CodeBehind="PayrollReport.aspx.cs" Inherits="FYP.Project.PayrollReport1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="style.css" />
    <div class="profilePanel">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <asp:Panel ID="reportPanel" runat="server" CssClass="profileContentPanel mt-2 mb-2" ScrollBars="Auto">
            <label class="subtitle">Payroll Report</label>

            <div class="form-group" style="margin-top: 30px">
                <label>Type</label>
                <select id="ddlSelect" onchange="GetSelectedTextValue(this)" class="form-control" runat="server">
                    <option>Monthly</option>
                    <option>Weekly</option>
                </select>
                <asp:Label ID="lblSelectError" Style="display: none" runat="server" ForeColor="Red" Font-Size="Smaller" Text="Selection is required"></asp:Label>
            </div>

            <div class="form-row">
                <div class="form-group col-md-6">
                    <label>Select Month</label>
                    <asp:TextBox ID="txtMonth" runat="server" CssClass="form-control" TextMode="Month"></asp:TextBox>
                    <asp:Label ID="lblMonthError" Style="display: none" runat="server" ForeColor="Red" Font-Size="Smaller" Text="This field is required"></asp:Label>
                    <asp:RequiredFieldValidator ID="rqvMonth" ControlToValidate="txtMonth" runat="server" ErrorMessage="This field is required" ForeColor="Red" Font-Size="Smaller" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
                <div class="form-group col-md-6">
                    <label>Select Week</label>
                    <asp:TextBox ID="txtDate" runat="server" CssClass="form-control" TextMode="Week"></asp:TextBox>
                    <asp:Label ID="lblDateError" Style="display: none" runat="server" ForeColor="Red" Font-Size="Smaller" Text="This field is required"></asp:Label>
                    <asp:RequiredFieldValidator ID="rqvDate" ControlToValidate="txtDate" runat="server" ErrorMessage="This field is required" ForeColor="Red" Font-Size="Smaller" Display="Dynamic"></asp:RequiredFieldValidator>
                </div>
            </div>

            <asp:Button runat="server" Text="Generate Report" OnClick="btnGenerateReport_Click" ID="btnGenerateReport" OnClientClick="SetTarget();" class="btn btn-primary" Style="float: right" />
            <button type="button" class="btn btn-secondary" id="btnReset" onclick="Reset()" style="float: right; margin-right: 10px">Reset</button>

        </asp:Panel>
    </div>

    <script type="text/javascript">
        document.getElementById('<%= txtDate.ClientID %>').disabled = true;
        document.getElementById('<%= txtMonth.ClientID %>').disabled = false;
        document.getElementById('<%= txtDate.ClientID %>').value = "";
        document.getElementById("<%= rqvMonth.ClientID%>").enabled = true;
        document.getElementById("<%= rqvDate.ClientID%>").style.visibility = "hidden";
        document.getElementById("<%= rqvMonth.ClientID%>").style.visibility = "visible";
        document.getElementById("<%= rqvDate.ClientID%>").enabled = false;

        function SetTarget() {
            document.forms[0].target = "_blank";
        }
        function GetSelectedTextValue(ddlSelect) {
            var selectedValue = ddlSelect.value;
            if (selectedValue == "Monthly") {
                document.getElementById('<%= txtDate.ClientID %>').disabled = true;
                document.getElementById('<%= txtMonth.ClientID %>').disabled = false;
                document.getElementById('<%= txtDate.ClientID %>').value = "";
                document.getElementById("<%= rqvMonth.ClientID%>").enabled = true;
                document.getElementById("<%= rqvDate.ClientID%>").style.visibility = "hidden";
                document.getElementById("<%= rqvMonth.ClientID%>").style.visibility = "visible";
                document.getElementById("<%= rqvDate.ClientID%>").enabled = false;
            }
            else if (selectedValue == "Weekly") {
                document.getElementById('<%= txtDate.ClientID %>').disabled = false;
                document.getElementById('<%= txtMonth.ClientID %>').disabled = true;
                document.getElementById('<%= txtMonth.ClientID %>').value = "";
                document.getElementById("<%= rqvMonth.ClientID%>").style.visibility = "hidden";
                document.getElementById("<%= rqvDate.ClientID%>").style.visibility = "visible";
                document.getElementById("<%= rqvMonth.ClientID%>").enabled = false;
                document.getElementById("<%= rqvDate.ClientID%>").enabled = true;
            }
        }
        function Reset() {
            document.getElementById('<%= txtDate.ClientID %>').disabled = true;
            document.getElementById('<%= txtMonth.ClientID %>').disabled = false;
            document.getElementById('<%= txtDate.ClientID %>').value = "";
            document.getElementById("<%= rqvMonth.ClientID%>").enabled = true;
            document.getElementById("<%= rqvDate.ClientID%>").style.visibility = "hidden";
            document.getElementById("<%= rqvMonth.ClientID%>").style.visibility = "visible";
            document.getElementById("<%= rqvDate.ClientID%>").enabled = false;
        }
    </script>

</asp:Content>
