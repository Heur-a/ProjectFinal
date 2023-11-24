<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="index.aspx.cs" Inherits="ProjectFinal.index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login Page</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Login</h2>
            <div>
                <label for="txtUsername">Nom d'usuari:</label>
                <asp:TextBox ID="txtUsername" runat="server" />
            </div>
            <div>
                <label for="txtPassword">Contrasenya:</label>
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" />
            </div>
            <div>
                <asp:Button ID="btnLogin" runat="server" Text="Iniciar sessió" OnClick="btnLogin_Click" />
            </div>
        </div>
    </form>
</body>
</html>
