﻿// index.aspx.cs

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
            // Utilitza una ruta relativa des del directori de treball del teu projecte per referenciar la base de dades.
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

                                // Realitza la comprovació del rol i redirigeix segons sigui admin, profesor o user.
                                switch (roleName)
                                {
                                    case "admin":
                                        Response.Redirect("PaginaAdmin.aspx");
                                        break;
                                    case "profesor":
                                        Response.Redirect("PaginaProfesor.aspx");
                                        break;
                                    case "user":
                                        Response.Redirect("PaginaUser.aspx");
                                        break;
                                    default:
                                        Response.Write("Rol no reconegut");
                                        break;
                                }
                            }
                        }
                        else
                        {
                            // Credencials incorrectes, pots mostrar un missatge d'error.
                            Response.Write("Credencials incorrectes");
                        }
                    }
                }
            }
        }
    }
}
