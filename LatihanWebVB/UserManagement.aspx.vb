Imports Npgsql
Imports System.Configuration
Imports System.Data

Public Class UserManagement
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            BindUserGrid()
            BindLogGrid()
        End If
    End Sub

    Private Sub BindUserGrid()
        Dim connString As String = ConfigurationManager.ConnectionStrings("PostgresConnection").ConnectionString
        Using conn As New NpgsqlConnection(connString)
            Dim cmd As New NpgsqlCommand("SELECT * FROM users", conn)
            Dim dt As New DataTable()

            conn.Open()
            Dim adapter As New NpgsqlDataAdapter(cmd)
            adapter.Fill(dt)
            gvUsers.DataSource = dt
            gvUsers.DataBind()
        End Using
    End Sub

    Private Sub BindLogGrid()
        Dim connString As String = ConfigurationManager.ConnectionStrings("PostgresConnection").ConnectionString
        Using conn As New NpgsqlConnection(connString)
            Dim cmd As New NpgsqlCommand("SELECT * FROM logs ORDER BY log_timestamp DESC", conn)
            Dim dt As New DataTable()

            conn.Open()
            Dim adapter As New NpgsqlDataAdapter(cmd)
            adapter.Fill(dt)
            gvLogs.DataSource = dt
            gvLogs.DataBind()
        End Using
    End Sub

    Protected Sub btnAddUser_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim connString As String = ConfigurationManager.ConnectionStrings("PostgresConnection").ConnectionString
        Using conn As New NpgsqlConnection(connString)
            Dim cmd As New NpgsqlCommand("INSERT INTO users (name, email, password) VALUES (@name, @email, @password)", conn)
            cmd.Parameters.AddWithValue("@name", txtAddName.Text)
            cmd.Parameters.AddWithValue("@email", txtAddEmail.Text)
            cmd.Parameters.AddWithValue("@password", txtAddPassword.Text)

            conn.Open()
            cmd.ExecuteNonQuery()
            AddLog("Add User", "Added user with name " & txtAddName.Text)
            ShowNotification("User added successfully.", True)
        End Using
        BindUserGrid()
        BindLogGrid()
    End Sub

    Protected Sub btnUpdateUser_Click(ByVal sender As Object, ByVal e As EventArgs)
        Dim userId As Integer = Convert.ToInt32(hdnEditUserId.Value)
        Dim connString As String = ConfigurationManager.ConnectionStrings("PostgresConnection").ConnectionString
        Using conn As New NpgsqlConnection(connString)
            Dim cmd As New NpgsqlCommand("UPDATE users SET name = @name, email = @email WHERE id = @id", conn)
            cmd.Parameters.AddWithValue("@name", txtEditName.Text)
            cmd.Parameters.AddWithValue("@email", txtEditEmail.Text)
            cmd.Parameters.AddWithValue("@id", userId)

            conn.Open()
            cmd.ExecuteNonQuery()
            AddLog("Update User", "Updated user with ID " & userId)
            ShowNotification("User updated successfully.", True)
        End Using
        BindUserGrid()
        BindLogGrid()
    End Sub

    Protected Sub gvUsers_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs)
        If e.CommandName = "DeleteUser" Then
            Dim userId As Integer = Convert.ToInt32(e.CommandArgument)
            Dim connString As String = ConfigurationManager.ConnectionStrings("PostgresConnection").ConnectionString
            Using conn As New NpgsqlConnection(connString)
                Dim cmd As New NpgsqlCommand("DELETE FROM users WHERE id = @id", conn)
                cmd.Parameters.AddWithValue("@id", userId)

                conn.Open()
                cmd.ExecuteNonQuery()
                AddLog("Delete User", "Deleted user with ID " & userId)
                ShowNotification("User deleted successfully.", True)
            End Using
            BindUserGrid()
            BindLogGrid()
        ElseIf e.CommandName = "EditUser" Then
            ' Load user data to populate the edit modal
            Dim userId As Integer = Convert.ToInt32(e.CommandArgument)
            Dim connString As String = ConfigurationManager.ConnectionStrings("PostgresConnection").ConnectionString
            Using conn As New NpgsqlConnection(connString)
                Dim cmd As New NpgsqlCommand("SELECT name, email FROM users WHERE id = @id", conn)
                cmd.Parameters.AddWithValue("@id", userId)

                conn.Open()
                Dim reader As NpgsqlDataReader = cmd.ExecuteReader()
                If reader.Read() Then
                    ' Set values for the modal fields
                    txtEditName.Text = reader("name").ToString()
                    txtEditEmail.Text = reader("email").ToString()
                    hdnEditUserId.Value = userId.ToString()
                    ' Call JavaScript function to show the modal
                    ScriptManager.RegisterStartupScript(Me, Me.GetType(), "ShowEditModal", "showEditModal(" & userId & ", '" & reader("name").ToString().Replace("'", "\'") & "', '" & reader("email").ToString().Replace("'", "\'") & "');", True)
                End If
            End Using
        End If
    End Sub

    Private Sub AddLog(action As String, description As String)
        Dim connString As String = ConfigurationManager.ConnectionStrings("PostgresConnection").ConnectionString
        Using conn As New NpgsqlConnection(connString)
            Dim cmd As New NpgsqlCommand("INSERT INTO logs (action, description, log_timestamp) VALUES (@action, @description, @log_timestamp)", conn)
            cmd.Parameters.AddWithValue("@action", action)
            cmd.Parameters.AddWithValue("@description", description)
            cmd.Parameters.AddWithValue("@log_timestamp", DateTime.Now)

            conn.Open()
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Sub ShowNotification(message As String, isSuccess As Boolean)
        NotificationLabel.Text = message
        NotificationLabel.CssClass = If(isSuccess, "alert alert-success", "alert alert-danger")
        NotificationLabel.Visible = True
    End Sub
End Class
