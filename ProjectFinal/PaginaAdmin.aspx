<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminPage.aspx.cs" Inherits="ProjectFinal.AdminPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Administrator Page</title>
    <style>
        table {
            border-collapse: collapse;
            width: 100%;
            margin-top: 20px;
        }

        th, td {
            border: 1px solid #dddddd;
            text-align: left;
            padding: 8px;
        }

        th {
            background-color: #f2f2f2;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Welcome Administrator</h2>

            <h3>Students:</h3>
            <asp:GridView ID="gvStudents" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="User ID" />
                    <asp:BoundField DataField="name" HeaderText="Name" />
                    <asp:BoundField DataField="surrname" HeaderText="Surname" />
                    <asp:BoundField DataField="dateOfBirth" HeaderText="Date of Birth" />
                    <asp:BoundField DataField="nationality" HeaderText="Nationality" />
                    <asp:BoundField DataField="address" HeaderText="Address" />
                </Columns>
            </asp:GridView>

            <h3>Professors:</h3>
            <asp:GridView ID="gvProfessors" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="User ID" />
                    <asp:BoundField DataField="name" HeaderText="Name" />
                    <asp:BoundField DataField="surrname" HeaderText="Surname" />
                </Columns>
            </asp:GridView>

            <h3>Subjects:</h3>
            <asp:GridView ID="gvSubjects" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="idsubject" HeaderText="Subject ID" />
                    <asp:BoundField DataField="subjectName" HeaderText="Subject Name" />
                    <asp:BoundField DataField="description" HeaderText="Description" />
                    
                </Columns>
            </asp:GridView>

            <!-- DropDownList to select the Subject -->
            <asp:DropDownList ID="ddlSubjects" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlSubjects_SelectedIndexChanged">
            </asp:DropDownList>

            <!-- New GridView for Students in the Subject -->
            <h3>Students for Subject:</h3>
            <asp:GridView ID="gvStudentsForSubject" runat="server" AutoGenerateColumns="False" OnRowDeleting="gvStudentsForSubject_RowDeleting">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="User ID" />
                    <asp:BoundField DataField="name" HeaderText="Name" />
                    <asp:BoundField DataField="surrname" HeaderText="Surname" />
                    <asp:BoundField DataField="dateOfBirth" HeaderText="Date of Birth" />
                    <asp:BoundField DataField="nationality" HeaderText="Nationality" />
                    <asp:BoundField DataField="address" HeaderText="Address" />
                    <asp:TemplateField HeaderText="Actions">
                        <ItemTemplate>
                            <asp:LinkButton ID="lnkDelete" runat="server" Text="Delete" CommandName="Delete" OnClientClick="return confirm('Are you sure you want to delete this student from the subject?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>

            <!-- New GridView for Professors in the Subject -->
            <h3>Professors for Subject:</h3>
            <asp:GridView ID="gvProfessorsForSubject" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="id" HeaderText="User ID" />
                    <asp:BoundField DataField="name" HeaderText="Name" />
                    <asp:BoundField DataField="surrname" HeaderText="Surname" />
                </Columns>
            </asp:GridView>

        </div>
    </form>
</body>
</html>
