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
                //Load students into the StudentList
                LoadUsersDropDown();
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

        private void AddStudentToSubject(string studentName, string subjectName)
        {
            try
            {
                // Estableix la connexió amb la base de dades
                string databasePath = Server.MapPath("~/database2.db");
                string connectionString = $"Data Source={databasePath};Version=3;";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Obtenir l'ID de l'estudiant
                    string studentIdQuery = "SELECT id FROM user WHERE name = @studentName";
                    using (SQLiteCommand studentIdCommand = new SQLiteCommand(studentIdQuery, connection))
                    {
                        studentIdCommand.Parameters.AddWithValue("@studentName", studentName);
                        string userId = studentIdCommand.ExecuteScalar()?.ToString();

                        if (userId != null)
                        {
                            // Obtenir l'ID de l'assignatura
                            string subjectIdQuery = "SELECT idsubject FROM subject WHERE subjectName = @subjectName";
                            using (SQLiteCommand subjectIdCommand = new SQLiteCommand(subjectIdQuery, connection))
                            {
                                subjectIdCommand.Parameters.AddWithValue("@subjectName", subjectName);
                                string subjectId = subjectIdCommand.ExecuteScalar()?.ToString();

                                if (subjectId != null)
                                {
                                    // Comprova si ja existeix una entrada per a aquest estudiant i aquesta assignatura
                                    string checkQuery = "SELECT COUNT(*) FROM user_has_subject WHERE user_id = @userId AND subject_idsubject = @subjectId";
                                    using (SQLiteCommand checkCommand = new SQLiteCommand(checkQuery, connection))
                                    {
                                        checkCommand.Parameters.AddWithValue("@userId", userId);
                                        checkCommand.Parameters.AddWithValue("@subjectId", subjectId);

                                        int existingEntries = Convert.ToInt32(checkCommand.ExecuteScalar());

                                        if (existingEntries == 0)
                                        {
                                            // Crea la comanda SQL per afegir l'estudiant a la signatura
                                            string insertQuery = "INSERT INTO user_has_subject (user_id, subject_idsubject) VALUES (@userId, @subjectId);";
                                            using (SQLiteCommand insertCommand = new SQLiteCommand(insertQuery, connection))
                                            {
                                                // Afegeix els paràmetres per evitar injeccions SQL
                                                insertCommand.Parameters.AddWithValue("@userId", userId);
                                                insertCommand.Parameters.AddWithValue("@subjectId", subjectId);

                                                // Executa la comanda d'afegir
                                                insertCommand.ExecuteNonQuery();
                                            }

                                            // Mostra un missatge d'èxit (pots adaptar-ho segons les teves necessitats)
                                            Response.Write("<script>alert('Estudiant afegit a la signatura amb èxit.');</script>");
                                        }
                                        else
                                        {
                                            // Mostra un missatge indicant que l'estudiant ja està afegit a aquesta assignatura
                                            Response.Write("<script>alert('Aquest estudiant ja està afegit a aquesta signatura.');</script>");
                                        }
                                    }
                                }
                                else
                                {
                                    // Mostra un missatge indicant que l'assignatura no existeix
                                    Response.Write("<script>alert('Aquesta assignatura no existeix.');</script>");
                                }
                            }
                        }
                        else
                        {
                            // Mostra un missatge indicant que l'estudiant no existeix
                            Response.Write("<script>alert('Aquest estudiant no existeix.');</script>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // En cas d'error, mostra un missatge d'error (pots adaptar-ho segons les teves necessitats)
                Response.Write("<script>alert('Error en afegir l'estudiant a la signatura.');</script>");
                // També pots afegir lògica per registrar l'error si és necessari
            }
        }
      

        

        private bool IsStudent(string userId)
        {
            // Implement logic to check if the user with the given ID has the role of a student
            // You can query the database or use any other method to check the role
            // Return true if the user is a student, otherwise return false
            // Example: return GetUserRole(userId) == "Student";
            return true; // Modify this based on your actual logic
        }

        // ... (existing code)

        protected void ddlSubjects_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedSubjectId = ddlSubjects.SelectedValue;

            if (!string.IsNullOrEmpty(selectedSubjectId))
            {
                // Load students for the selected subject
                LoadStudentsForSubject(selectedSubjectId);

                // Load professors for the selected subject
                LoadProfessorsForSubject(selectedSubjectId);

                // Show the panel for adding a student to the subject
                pnlAddStudent.Visible = true;
            }
            else
            {
                // If no subject is selected, clear the GridViews and hide the panel
                gvStudentsForSubject.DataSource = null;
                gvStudentsForSubject.DataBind();

                gvProfessorsForSubject.DataSource = null;
                gvProfessorsForSubject.DataBind();

                pnlAddStudent.Visible = false;
            }
        }

        

        private void LoadUsersDropDown()
        {
            string databasePath = Server.MapPath("~/database2.db");
            string connectionString = $"Data Source={databasePath};Version=3;";
            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT id, name FROM user";
                SQLiteCommand command = new SQLiteCommand(query, connection);
                SQLiteDataReader reader = command.ExecuteReader();

                ddlUsers.DataSource = reader;
                ddlUsers.DataTextField = "name";
                ddlUsers.DataValueField = "id";
                ddlUsers.DataBind();

                ddlUsers.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select a User --", ""));
            }
        }

        protected void btnAddStudentToSubject_Click(object sender, EventArgs e)
        {
            string selectedStudentId = ddlUsers.SelectedValue;
            string selectedSubjectId = ddlSubjects.SelectedValue;

            if (!string.IsNullOrEmpty(selectedStudentId) && !string.IsNullOrEmpty(selectedSubjectId))
            {
                // Check if the selected user has the role of a student
                if (IsStudent(selectedStudentId))
                {
                    // Add the student to the subject
                    AddStudentToSubject(selectedStudentId, selectedSubjectId);

                    // Clear the selected student in the DropDownList
                    ddlUsers.ClearSelection();

                    // Trigger a page refresh
                    Response.Redirect(Request.RawUrl);
                }
                else
                {
                    // Show a message that the selected user is not a student
                    Response.Write("<script>alert('The selected user is not a student.');</script>");
                }
            }
            else
            {
                // Show a message indicating that all fields need to be filled
                Response.Write("<script>alert('Please select a student and a subject.');</script>");
            }
        }

        // ... (existing code)

        protected void btnAddStudentToDatabase_Click(object sender, EventArgs e)
        {
            string newStudentName = txtNewStudentName.Text.Trim();
            string newStudentSurname = txtNewStudentSurname.Text.Trim();
            DateTime.TryParse(txtNewStudentDateOfBirth.Text.Trim(), out DateTime newStudentDateOfBirth);
            string newStudentNationality = txtNewStudentNationality.Text.Trim();
            string newStudentAddress = txtNewStudentAddress.Text.Trim();
            string newStudentUsername = txtNewStudentUsername.Text.Trim();

            if (!string.IsNullOrEmpty(newStudentName))
            {
                // Add the new student to the database
                AddStudentToDatabase(newStudentName, newStudentSurname, newStudentDateOfBirth, newStudentNationality, newStudentAddress, newStudentUsername);

                // Clear the text boxes
                txtNewStudentName.Text = "";
                txtNewStudentSurname.Text = "";
                txtNewStudentDateOfBirth.Text = "";
                txtNewStudentNationality.Text = "";
                txtNewStudentAddress.Text = "";

                // Trigger a page refresh
                Response.Redirect(Request.RawUrl);
            }
            else
            {
                // Show a message indicating that the student name needs to be filled
                Response.Write("<script>alert('Please enter the new student's name.');</script>");
            }
        }

        // ... (your existing code)

        private void AddStudentToDatabase(string newStudentName, string newStudentSurname, DateTime newStudentDateOfBirth, string newStudentNationality, string newStudentAddress, string newStudentUsername)
        {
            try
            {
                // Establish connection with the database
                string databasePath = Server.MapPath("~/database2.db");
                string connectionString = $"Data Source={databasePath};Version=3;";

                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();

                    // Insert the new user into the user table
                    string insertUserQuery = "INSERT INTO user (name, surrname, dateOfBirth, nationality, address, username, role_idrole) " +
                                             "VALUES (@newStudentName, @newStudentSurname, @newStudentDateOfBirth, @newStudentNationality, @newStudentAddress, @newStudentUsername, 3)";
                    using (SQLiteCommand insertUserCommand = new SQLiteCommand(insertUserQuery, connection))
                    {
                        // Add parameters to prevent SQL injection
                        insertUserCommand.Parameters.AddWithValue("@newStudentName", newStudentName);
                        insertUserCommand.Parameters.AddWithValue("@newStudentSurname", newStudentSurname);
                        insertUserCommand.Parameters.AddWithValue("@newStudentDateOfBirth", newStudentDateOfBirth.ToString("yyyy-MM-dd"));
                        insertUserCommand.Parameters.AddWithValue("@newStudentNationality", newStudentNationality);
                        insertUserCommand.Parameters.AddWithValue("@newStudentAddress", newStudentAddress);
                        insertUserCommand.Parameters.AddWithValue("@newStudentUsername", newStudentUsername);

                        // Execute the insert command for the user
                        insertUserCommand.ExecuteNonQuery();
                    }

                    // Get the ID of the newly inserted user
                    string getUserIdQuery = "SELECT last_insert_rowid()";
                    using (SQLiteCommand getUserIdCommand = new SQLiteCommand(getUserIdQuery, connection))
                    {
                        int userId = Convert.ToInt32(getUserIdCommand.ExecuteScalar());

                        // Insert the user into the student table
                        string insertStudentQuery = "INSERT INTO student (dateOfBirth, nationality, address, user_id) " +
                                                    "VALUES (@newStudentDateOfBirth, @newStudentNationality, @newStudentAddress, @userId)";
                        using (SQLiteCommand insertStudentCommand = new SQLiteCommand(insertStudentQuery, connection))
                        {
                            insertStudentCommand.Parameters.AddWithValue("@newStudentDateOfBirth", newStudentDateOfBirth.ToString("yyyy-MM-dd"));
                            insertStudentCommand.Parameters.AddWithValue("@newStudentNationality", newStudentNationality);
                            insertStudentCommand.Parameters.AddWithValue("@newStudentAddress", newStudentAddress);
                            insertStudentCommand.Parameters.AddWithValue("@userId", userId);

                            insertStudentCommand.ExecuteNonQuery();
                        }
                    }

                    // Show a success message (you can customize it as needed)
                    Response.Write("<script>alert('New student added to the database successfully.');</script>");
                }
            }
            catch (Exception ex)
            {
                // In case of an error, show an error message (you can customize it as needed)
                Response.Write("<script>alert('Error adding new student to the database.');</script>");
                // You can also add logic to log the error if needed
            }
        }

        // ... (your existing code)


        // ... (existing code)

    }
}










    
