<%@ Page Title="" Language="C#" MasterPageFile="~/Project/Site1.Master" AutoEventWireup="true" CodeBehind="AddEmployee.aspx.cs" Inherits="FYP.Project.AddEmployee" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="style.css" />
    <div class="addEmployees">
        <label class="h4 mb-4" style="">Add Staff</label>
        <div class="form-group">
            <label class="column">
                Staff Email 
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Email Required" ControlToValidate="txtEmail" ForeColor="Red"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Invalid Email" ControlToValidate="txtEmail" ForeColor="Red" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>
            </label>
            <asp:TextBox ID="txtEmail" runat="server" CssClass="form-control" placeholder="Email*"></asp:TextBox>
        </div>
        <div class="form-group">
            <label>Staff Role</label>
            <asp:DropDownList ID="ddlRole" runat="server" CssClass="form-control">
                <asp:ListItem>Normal Staff</asp:ListItem>
                <asp:ListItem>HR Staff</asp:ListItem>
            </asp:DropDownList>
        </div>
        <asp:Button ID="btnCreate" runat="server" Text="Create" class="btn btn-primary rounded submit px-3" OnClick="btnCreate_Click" />
    </div>
</asp:Content>
