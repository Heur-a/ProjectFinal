using System;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Web.UI.WebControls;

namespace ProjectFinal
{
    public partial class AdminPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadStudents();
                LoadProfessors();
                LoadSubjects();

                // Load subjects into the DropDownList
                LoadSubjectsDropDown();
            }
        }

        private void LoadSubjectsDropDown()
        {
            string databasePath = Server.MapPath("~/database2.db");
            string connectionString = $"Data Source={databasePath};Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT idsubject, subjectName FROM subject";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();

                ddlSubjects.DataSource = reader;
                ddlSubjects.DataTextField = "subjectName";
                ddlSubjects.DataValueField = "idsubject";
                ddlSubjects.DataBind();

                ddlSubjects.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select a Subject --", ""));
            }
        }

        private void LoadStudents()
        {
            string databasePath = Server.MapPath("~/database2.db");
            string connectionString = $"Data Source={databasePath};Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT user.*, student.dateOfBirth, student.nationality, student.address " +
                               "FROM user " +
                               "INNER JOIN student ON user.id = student.user_id " +
                               "WHERE user.role_idrole = 3 " +
                               "GROUP BY user.id";

                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                gvStudents.DataSource = dt;
                gvStudents.DataBind();
            }
        }

        private void LoadProfessors()
        {
            string databasePath = Server.MapPath("~/database2.db");
            string connectionString = $"Data Source={databasePath};Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT user.*, GROUP_CONCAT(subject.subjectName) AS Subjects " +
                               "FROM user " +
                               "LEFT JOIN subject ON user.id = subject.idsubject " +
                               "WHERE user.role_idrole = 2 " +
                               "GROUP BY user.id";

                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                gvProfessors.DataSource = dt;
                gvProfessors.DataBind();
            }
        }

        private void LoadSubjects()
        {
            string databasePath = Server.MapPath("~/database2.db");
            string connectionString = $"Data Source={databasePath};Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT subject.idsubject, subject.subjectName, subject.description " +
                               "FROM subject ";

                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                gvSubjects.DataSource = dt;
                gvSubjects.DataBind();
            }
        }

        protected void ddlSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedSubjectId = ddlSubjects.SelectedValue;

            if (!string.IsNullOrEmpty(selectedSubjectId))
            {
                // Load students for the selected subject
                LoadStudentsForSubject(selectedSubjectId);

                // Load professors for the selected subject
                LoadProfessorsForSubject(selectedSubjectId);
            }
            else
            {
                // If no subject is selected, clear the GridViews
                gvStudentsForSubject.DataSource = null;
                gvStudentsForSubject.DataBind();

                gvProfessorsForSubject.DataSource = null;
                gvProfessorsForSubject.DataBind();
            }
        }

        private void LoadStudentsForSubject(string subjectId)
        {
            string databasePath = Server.MapPath("~/database2.db");
            string connectionString = $"Data Source={databasePath};Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT user.*, student.dateOfBirth, student.nationality, student.address " +
                               "FROM user " +
                               "INNER JOIN student ON user.id = student.user_id " +
                               "INNER JOIN subject_has_user ON user.id = subject_has_user.user_id " +
                               $"WHERE user.role_idrole = 3 AND subject_has_user.subject_idsubject = {subjectId} " +
                               "GROUP BY user.id";

                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                gvStudentsForSubject.DataSource = dt;
                gvStudentsForSubject.DataBind();
            }
        }

        private void LoadProfessorsForSubject(string subjectId)
        {
            string databasePath = Server.MapPath("~/database2.db");
            string connectionString = $"Data Source={databasePath};Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT user.*, GROUP_CONCAT(subject.subjectName) AS Subjects " +
                               "FROM user " +
                               "LEFT JOIN subject ON user.id = subject.idsubject " +
                               "INNER JOIN subject_has_user ON user.id = subject_has_user.user_id " +
                               $"WHERE user.role_idrole = 2 AND subject_has_user.subject_idsubject = {subjectId} " +
                               "GROUP BY user.id";

                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                gvProfessorsForSubject.DataSource = dt;
                gvProfessorsForSubject.DataBind();
            }
        }

        protected void gvStudentsForSubject_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            // Get the user ID and subject ID of the selected row
            string userId = gvStudentsForSubject.DataKeys[e.RowIndex].Values["id"].ToString();
            string subjectId = ddlSubjects.SelectedValue;

            // Implement the logic for deletion in the database
            DeleteUserSubjectEntry(userId, subjectId);

            // Refresh the data
            LoadStudentsForSubject(subjectId);
        }

        private void DeleteUserSubjectEntry(string userId, string subjectId)
        {
            try
            {
                // Establish connection with the database
                string databasePath = Server.MapPath("~/database2.db");
                string connectionString = $"Data Source={databasePath};Version=3;";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Create the SQL command to delete the entry from user_has_subject
                    string query = "DELETE FROM user_has_subject WHERE user_id = @userId AND subject_idsubject = @subjectId";

                    using (SQLiteCommand command = new SQLiteCommand(query, connection))
                    {
                        // Add parameters to prevent SQL injection
                        command.Parameters.AddWithValue("@userId", userId);
                        command.Parameters.AddWithValue("@subjectId", subjectId);

                        // Execute the delete command
                        command.ExecuteNonQuery();
                    }
                }

                // Show a success message (you can customize it as needed)
                Response.Write("<script>alert('Student deleted from the subject successfully.');</script>");
            }
            catch (Exception ex)
            {
                // In case of an error, show an error message (you can customize it as needed)
                Response.Write("<script>alert('Error deleting student from the subject.');</script>");
                // You can also add logic to log the error if needed
            }
        }
    }
}
