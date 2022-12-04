<%@ Page Title="" Language="C#" MasterPageFile="~/Project/Site1.Master" AutoEventWireup="true" CodeBehind="PayrollDetailList.aspx.cs" Inherits="FYP.Project.PayrollDetailList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <link rel="stylesheet" href="style.css">
    <style type="text/css">
        .button {
            max-width: 20px;
            max-height: 20px;
            margin: 0px 10px;
        }

            .button:hover {
                transform: scale(1.2);
            }
    </style>

    <div class="card shadow mb-4">
        <div class="card-header py-3">
            <asp:Label ID="lblTitle" runat="server" class="text-primary m-0 fw-bold" Style="font-size: 21px;">Payroll List For </asp:Label>
        </div>
        <div class="card-body" style="overflow-y: hidden;">
            <div class="table-responsive">
                <asp:GridView ID="gvList" runat="server" CssClass="table table-bordered dataTable1" Width="100%" AutoGenerateColumns="false"
                    OnRowDataBound="OnRowDataBound" OnRowCommand="FireRowCommand"
                    EmptyDataText="No records has been added." DataKeyNames="PayslipID,PaymentMethod,BasicSalary,Bonus, UnpaidLeaveSalary, ProcessStatus">
                    <Columns>
                        <asp:TemplateField HeaderText="Payslip ID" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblPayslipID" runat="server" Text='<%# Eval("PayslipID") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Payment Method" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblPaymentMethod" runat="server" Text='<%# Eval("PaymentMethod") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Basic Salary" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblBasicSalary" runat="server" Text='<%# Eval("BasicSalary") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Bonus" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblBonus" runat="server" Text='<%# Eval("Bonus") %>' Visible="false"></asp:Label>
                                <asp:TextBox ID="txtBonus" runat="server" Text='<%# Eval("Bonus")  %>' CssClass="form-control" TextMode="Number" min="0"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Amount of unpaid leave" ItemStyle-Width="10%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblUnpaidLeaveSalary" runat="server" Text='<%# Eval("UnpaidLeaveSalary") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
<%--                        <asp:TemplateField HeaderText="Bonus" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:TextBox ID="txtBonus" runat="server" Text='<%# Eval("Bonus")  %>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Status" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblProcessStatus" runat="server" Text='<%# Eval("ProcessStatus")  %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Action" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:ImageButton ID="btnProcess" runat="server" CssClass="button" Style="cursor: pointer" ImageUrl="~/Image/process.png" AlternateText="process" CommandName="Processing" CausesValidation="false" />
                                <%--<asp:ImageButton ID="btnSend" runat="server" CssClass="button" Style="cursor: pointer" ImageUrl="~/Image/send.png" AlternateText="send" CommandName="Sending" CausesValidation="false" />--%>
                                <asp:ImageButton ID="btnEdit" runat="server" CssClass="button" Style="cursor: pointer" ImageUrl="~/Image/editing.png" AlternateText="edit" CommandName="Editing" CausesValidation="false" />
                                <asp:ImageButton ID="btnViewDetail" runat="server" CssClass="button" Style="cursor: pointer" ImageUrl="~/Image/detail.png" AlternateText="detail" CommandName="Detailing" CausesValidation="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <asp:Label ID="lblEmployerEPF" runat="server" Text="" Width="0" Height="0" Visible="false"></asp:Label>
    <asp:Label ID="lblEmployeeEPF" runat="server" Text="" Width="0" Height="0" Visible="false"></asp:Label>
    <asp:Label ID="lblEmployerSocso" runat="server" Text="" Width="0" Height="0" Visible="false"></asp:Label>
    <asp:Label ID="lblEmployeeSocso" runat="server" Text="" Width="0" Height="0" Visible="false"></asp:Label>
    <asp:Label ID="lblEmployerEIS" runat="server" Text="" Width="0" Height="0" Visible="false"></asp:Label>
    <asp:Label ID="lblEmployeeEIS" runat="server" Text="" Width="0" Height="0" Visible="false"></asp:Label>


</asp:Content>
