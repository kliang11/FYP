<%@ Page Title="" Language="C#" MasterPageFile="~/Project/Site1.Master" AutoEventWireup="true" CodeBehind="EmployeeList.aspx.cs" Inherits="FYP.Project.EmployeeList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

    <style>
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
        <div class="card-header py-3" <%--style="margin: 15px;--%>>
            <p class="text-primary m-0 fw-bold" style="font-size: 21px;">Staff List</p>
        </div>
        <div style="position: absolute; margin-top: 16px; margin-right: 21px; top: 0; right: 0;">
            <button type="button" class="btn btn-primary" data-toggle="modal" data-target="#exampleModal">+</button>
        </div>
        <div class="card-body" style="overflow-y: hidden;">
            <div class="table-responsive">
                <asp:GridView ID="gvList" runat="server" CssClass="table table-bordered dataTable1" Width="100%" AutoGenerateColumns="false"
                    OnRowDataBound="OnRowDataBound" OnRowDeleting="OnRowDeleting" OnSelectedIndexChanged="gvList_SelectedIndexChanged" 
                    EmptyDataText="No staff records." DataKeyNames="Photo, Staff_ID, Name, Email, Role">
                    <Columns>
                        <asp:TemplateField HeaderText="Image" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Image ID="imgStaff" runat="server" Style="height: 35px; width: 35px; border-radius: 50%;" alt="Avatar" ImageUrl='<%#"data:Image/png;base64," +  Convert.ToBase64String((byte[])Eval("Photo"))%>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Staff ID" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblStaffID" runat="server" Text='<%# Eval("Staff_ID")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Staff Name" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblStaffName" runat="server" Text='<%# Eval("Name")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Staff Email" ItemStyle-Width="20%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblStaffEmail" runat="server" Text='<%# Eval("Email")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Staff Role" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center">
                            <ItemTemplate>
                                <asp:Label ID="lblStaffRole" runat="server" Text='<%# Eval("Role")%>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:CommandField ButtonType="Image" ControlStyle-CssClass="button"
                            DeleteImageUrl="~/Image/bin.png" ShowDeleteButton="true"
                            SelectImageUrl="~/Image/detail.png" ShowSelectButton="True"
                            HeaderText="Actions" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" />
                        <asp:CommandField ButtonType="Image" ControlStyle-CssClass="button"                            
                            SelectImageUrl="~/Image/detail.png" ShowSelectButton="True"
                            HeaderText="Actions" ItemStyle-Width="15%" ItemStyle-HorizontalAlign="Center" HeaderStyle-CssClass="text-center" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

</asp:Content>
