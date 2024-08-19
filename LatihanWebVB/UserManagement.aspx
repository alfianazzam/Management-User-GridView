<%@ Page Title="User Management" Language="VB" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserManagement.aspx.vb" Inherits="LatihanWebVB.UserManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="container mt-4">
        <h1 class="mb-4">User Management</h1>
         <!-- Notification Label -->
 <asp:Label ID="NotificationLabel" runat="server" CssClass="alert alert-info d-none"></asp:Label>
        <div class="card mb-4">
            <div class="card-header d-flex justify-content-between align-items-center">
                <h2 class="h5 mb-0">User List</h2>
                <button type="button" class="btn btn-primary" data-bs-toggle="modal" data-bs-target="#addUserModal">
                    Add New User
                </button>
            </div>
            <div class="card-body">
                <asp:GridView ID="gvUsers" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False" OnRowCommand="gvUsers_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="ID" />
                        <asp:BoundField DataField="Name" HeaderText="Name" />
                        <asp:BoundField DataField="Email" HeaderText="Email" />
                        <asp:TemplateField HeaderText="Actions">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" Text="Edit" CommandName="EditUser" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-primary" />
                                <asp:Button ID="btnDelete" runat="server" Text="Delete" CommandName="DeleteUser" CommandArgument='<%# Eval("Id") %>' CssClass="btn btn-sm btn-danger" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <div class="card">
            <div class="card-header">
                <h2 class="h5 mb-0">User Action Logs</h2>
            </div>
            <div class="card-body">
                <asp:GridView ID="gvLogs" runat="server" CssClass="table table-striped table-bordered" AutoGenerateColumns="False">
                    <Columns>
                        <asp:BoundField DataField="id" HeaderText="Log ID" />
                        <asp:BoundField DataField="action" HeaderText="Action" />
                        <asp:BoundField DataField="user_id" HeaderText="User ID" />
                        <asp:BoundField DataField="log_timestamp" HeaderText="Timestamp" />
                        <asp:BoundField DataField="description" HeaderText="Description" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>

    <!-- Add User Modal -->
    <div class="modal fade" id="addUserModal" tabindex="-1" aria-labelledby="addUserModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="addUserModalLabel">Add New User</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <div class="mb-3">
                        <asp:TextBox ID="txtAddName" runat="server" CssClass="form-control" placeholder="Name"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <asp:TextBox ID="txtAddEmail" runat="server" CssClass="form-control" placeholder="Email"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <asp:TextBox ID="txtAddPassword" runat="server" TextMode="Password" CssClass="form-control" placeholder="Password"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    <asp:Button ID="btnAddUser" runat="server" Text="Add User" CssClass="btn btn-primary" OnClick="btnAddUser_Click" />
                </div>
            </div>
        </div>
    </div>

    <!-- Edit User Modal -->
    <div class="modal fade" id="editUserModal" tabindex="-1" aria-labelledby="editUserModalLabel" aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="editUserModalLabel">Edit User</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">
                    <input type="hidden" id="hdnEditUserId" runat="server" />
                    <div class="mb-3">
                        <asp:TextBox ID="txtEditName" runat="server" CssClass="form-control" placeholder="Name"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <asp:TextBox ID="txtEditEmail" runat="server" CssClass="form-control" placeholder="Email"></asp:TextBox>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    <asp:Button ID="btnUpdateUser" runat="server" Text="Update User" CssClass="btn btn-primary" OnClick="btnUpdateUser_Click" />
                </div>
            </div>
        </div>
    </div>

<script type="text/javascript">
    function showEditModal() {
        var modal = new bootstrap.Modal(document.getElementById('editUserModal'));
        if (modal) {
            modal.show();
        } else {
            console.error('EditUserModal element not found.');
        }
    }
</script>



</asp:Content>
