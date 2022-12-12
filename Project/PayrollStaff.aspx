<%@ Page Title="" Language="C#" MasterPageFile="~/Project/Site1.Master" AutoEventWireup="true" CodeBehind="PayrollStaff.aspx.cs" Inherits="FYP.Project.PayrollStaff" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="style.css">

    <div class="card shadow mb-4">
        <div class="card-header py-3">
            <p class="text-primary m-0 fw-bold" style="font-size: 21px;">Payroll List</p>
        </div>
        <div class="card-body" style="overflow-y: hidden;">
            <div class="table-responsive">
                <div style="clear: both;" class="mb-2"></div>
                <asp:GridView ID="gvList" runat="server" CssClass="table table-bordered dataTable1" Width="100%" AutoGenerateColumns="false"
                    OnRowDataBound="OnRowDataBound" EmptyDataText="No records has been added." DataKeyNames="PayslipID,PayrollListID,Staff_ID,DateGenerated,NetSalary"
                    OnSelectedIndexChanged="gvList_SelectedIndexChanged">
                    <Columns>
                        <asp:TemplateField HeaderText="Payslip ID" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblPayslipID" runat="server" Text='<%# Eval("PayslipID") %>'></asp:Label>
                                <asp:Label ID="lblPayrollListID" runat="server" Text='<%# Eval("PayrollListID") %>' Style="display: none"></asp:Label>
                                <asp:Label ID="lblStaffID" runat="server" Text='<%# Eval("Staff_ID") %>' Style="display: none"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Date Generated" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblDate" runat="server" Text='<%# Eval("DateGenerated","{0:d}")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Net Salary" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblNetSalary" runat="server" Text='<%# Eval("NetSalary") %>'></asp:Label>
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

</asp:Content>
