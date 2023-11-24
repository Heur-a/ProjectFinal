<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserPage.aspx.cs" Inherits="ProjectFinal.UserPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Edit Personal Information</h2>
            <asp:Label ID="lblMessage" runat="server" ForeColor="Red" EnableViewState="False"></asp:Label>
            <br />
            <br />
            <asp:Label ID="lblName" runat="server" Text="Name:"></asp:Label>
            <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="lblSurname" runat="server" Text="Surname:"></asp:Label>
            <asp:TextBox ID="txtSurname" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="lblDOB" runat="server" Text="Date of Birth:"></asp:Label>
            <asp:TextBox ID="txtDOB" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="lblNationality" runat="server" Text="Nationality:"></asp:Label>
            <asp:TextBox ID="txtNationality" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="lblIDNumber" runat="server" Text="ID Number:"></asp:Label>
            <asp:TextBox ID="txtIDNumber" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Label ID="lblAddress" runat="server" Text="Address:"></asp:Label>
            <asp:TextBox ID="txtAddress" runat="server"></asp:TextBox>
            <br />
            <br />
            <asp:Button ID="btnUpdate" runat="server" Text="Update Information" OnClick="btnUpdate_Click" />
            <br />
            <br />

            <h2>Subjects in Current Academic Year</h2>
            <asp:GridView ID="gridSubjects" runat="server" AutoGenerateColumns="False">
                <Columns>
                    <asp:BoundField DataField="SubjectName" HeaderText="Subject Name" SortExpression="SubjectName" />
                    <asp:BoundField DataField="Credits" HeaderText="Credits" SortExpression="Credits" />
                    <asp:BoundField DataField="Semester" HeaderText="Semester" SortExpression="Semester" />
                    <asp:BoundField DataField="Professor" HeaderText="Professor" SortExpression="Professor" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
