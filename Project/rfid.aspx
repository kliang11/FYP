<%@ Page Title="" Language="C#" MasterPageFile="~/Project/Site1.Master" AutoEventWireup="true" CodeBehind="rfid.aspx.cs" Inherits="FYP.Project.rfid" %>

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
                transform: scale(1.3);
            }
    </style>
    <div class="card shadow mb-4">
        <div class="card-header py-3" <%--style="margin: 15px;--%>>
            <p class="text-primary m-0 fw-bold" style="font-size: 21px;">RFID Card</p>
        </div>
        <div class="card-body" style="overflow-y: hidden;">
            <div class="table-responsive">
                <asp:GridView ID="gvList" runat="server" CssClass="table table-bordered dataTable1" Width="100%" AutoGenerateColumns="false"
                    OnRowEditing="OnRowEditing" OnRowCancelingEdit="OnRowCancelingEdit"
                    OnRowUpdating="OnRowUpdating" OnRowDeleting="OnRowDeleting" EmptyDataText="No records has been added." DataKeyNames="Card_ID,Card_Status,Date,Staff_ID">
                    <Columns>
                        <asp:BoundField DataField="Card_ID" HeaderText="Card ID" />
                        <asp:BoundField DataField="Card_Status" HeaderText="Card Status" />
                        <asp:BoundField DataField="Date" HeaderText="Registered Date" />
                        <asp:BoundField DataField="Staff_ID" HeaderText="Staff ID" />
                        <%-- <asp:BoundField DataField="" HeaderText="Action" />--%>
                        <asp:CommandField ButtonType="Image" ShowEditButton="true" EditImageUrl="~/Image/editing.png" ControlStyle-CssClass="button" UpdateImageUrl="~/Image/tick.png" CancelImageUrl="~/Image/cancel.png" DeleteImageUrl="~/Image/bin.png" ShowDeleteButton="true" HeaderText="Actions" />
                    </Columns>
                </asp:GridView>
            </div>

        </div>
    </div>
</asp:Content>
