// index.aspx.cs

using System;
using System.Data;
using System.Data.SQLite;

namespace ProjectFinal
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                // Use a relative path from your project's working directory to reference the database.
                string databasePath = Server.MapPath("~/database2.db");
                string connectionString = $"Data Source={databasePath};Version=3;";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    string query = "SELECT user.*, role.role AS RoleName FROM user " +
                                   "INNER JOIN role ON user.role_idrole = role.idrole " +
                                   "WHERE username = @Username AND password = @Password";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", txtUsername.Text);
                        command.Parameters.AddWithValue("@Password", txtPassword.Text);

                        using (SQLiteDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    string roleName = reader["RoleName"].ToString();

                                    // Check the role and redirect accordingly, whether it's admin, teacher, or user.
                                    switch (roleName)
                                    {
                                        case "admin":
                                            Response.Redirect("AdminPage.aspx");
                                            break;
                                        case "teacher":
                                            Response.Redirect("TeacherPage.aspx");
                                            break;
                                        case "user":
                                            Response.Redirect("UserPage.aspx");
                                            break;
                                        default:
                                            Response.Write("Role not recognized");
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                // Incorrect credentials, you can display an error message.
                                Response.Write("Incorrect credentials");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions, log them, or display an error message.
                Response.Write($"An error occurred: {ex.Message}");
            }
        }
    }
}
