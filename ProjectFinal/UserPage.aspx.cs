using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectFinal
{
    public partial class UserPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Retrieve the user ID from the Session
                int userId = (int)Session["UserID"];

                // Load personal information
                LoadUserInfo(userId);

                // Call the updated method to load and bind student academic information
                //LoadStudentAcademicInfo(userId);
            }
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            // Retrieve the updated information from the textboxes
            string name = txtName.Text;
            string surname = txtSurname.Text;
            DateTime dob = DateTime.Parse(txtDOB.Text); // Assuming the date is in a valid format
            string nationality = txtNationality.Text;
            int idNumber = int.Parse(txtIDNumber.Text); // Assuming ID-number is an integer
            string address = txtAddress.Text;

            // Update the information in the database
            UpdateUserInfo(name, surname, dob, nationality, idNumber, address);

            int userId = (int)Session["UserID"];

            // Reload the updated information
            LoadUserInfo(userId);

            // Display a message indicating the update was successful
            lblMessage.Text = "Personal information updated successfully!";
        }

        protected void btnLoadSubjects_Click(object sender, EventArgs e)
        {
            int userId = (int)Session["UserID"];
            // Get the academic year from the TextBox
            string semester = txtSemester.Text.Trim();

            // Call a method to load and display subjects based on the entered academic year
            LoadSubjectsForSemester(semester, userId);
        }


        private void LoadUserInfo(int userId)
        {
            // Load the existing information from the SQLite database and populate the textboxes
            int idNumber = userId; // Replace with the actual ID-number for the user
            string databasePath = Server.MapPath("~/database2.db");
            string connectionString = $"Data Source={databasePath};Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sqlQuery = @"
                SELECT
                    u.name,
                    u.surrname AS surname,
                    s.dateOfBirth AS 'date of birth',
                    s.nationality,
                    u.id AS 'ID-number',
                    s.address
                FROM
                    [user] u
                JOIN
                    student s ON u.id = s.user_id
                WHERE
                    u.id = @IDNumber;";

                using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                {
                    command.Parameters.AddWithValue("@IDNumber", idNumber);

                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            txtName.Text = reader["name"].ToString();
                            txtSurname.Text = reader["surname"].ToString();
                            txtDOB.Text = reader["date of birth"].ToString();
                            txtNationality.Text = reader["nationality"].ToString();
                            txtIDNumber.Text = reader["ID-number"].ToString();
                            txtAddress.Text = reader["address"].ToString();
                        }
                    }
                }
            }
        }

        private void UpdateUserInfo(string name, string surname, DateTime dob, string nationality, int idNumber, string address)
        {
            // Update the user information in the SQLite database
            string databasePath = Server.MapPath("~/database2.db");
            string connectionString = $"Data Source={databasePath};Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string sqlUpdate = @"
                UPDATE [user]
                SET name = @Name, surrname = @Surname
                WHERE id = @IDNumber;

                UPDATE student
                SET dateOfBirth = @DOB, nationality = @Nationality, address = @Address
                WHERE user_id = @IDNumber;";

                using (SQLiteCommand command = new SQLiteCommand(sqlUpdate, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Surname", surname);
                    command.Parameters.AddWithValue("@DOB", dob);
                    command.Parameters.AddWithValue("@Nationality", nationality);
                    command.Parameters.AddWithValue("@IDNumber", idNumber);
                    command.Parameters.AddWithValue("@Address", address);

                    command.ExecuteNonQuery();
                }
            }
        }

        private void LoadSubjectsForSemester(string Semester, int userId)
        {
            // Use Server.MapPath to get the physical path of the database file
            string databasePath = Server.MapPath("~/database2.db");
            string connectionString = $"Data Source={databasePath};Version=3;";

            // Replace the following query with your actual query to retrieve student academic information
            string sqlQuery = @"
        SELECT
            s.subjectName AS SubjectName,
            shu.credits AS Credits,
            u.name || ' ' || u.surrname AS Professor
        FROM
            subject s
        INNER JOIN
            subject_has_user shu ON s.idsubject = shu.subject_idsubject
        INNER JOIN
            [user] u ON shu.user_id = u.id
        WHERE  
            shu.YearCoursed = @Semester
            AND u.role_idrole = (SELECT idrole FROM role WHERE role = 'profesor');
";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(sqlQuery, connection))
                {
                    // Replace @StudentId and @Semester with actual values
                    command.Parameters.AddWithValue("@StudentId", userId);
                    command.Parameters.AddWithValue("@Semester", Semester);

                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        // Bind the DataTable to the GridView
                        gridSubjects.DataSource = dataTable;
                        gridSubjects.DataBind();
                    }
                }
            }
        }

    }
}