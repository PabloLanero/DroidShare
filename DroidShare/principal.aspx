<%@ Page Async="true" Language="C#" AutoEventWireup="true" CodeFile="principal.aspx.cs" Inherits="principal" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Photo Receiver</title>
    <style>
        body {
            position: relative;
            min-height: 100vh; /* Asegura que el cuerpo tenga al menos el alto de la ventana del navegador */
            margin: 0;
            padding: 0;
        }
        .button-container {
            position: fixed; /* Posición fija */
            bottom: 10px;
            right: 10px;
            z-index: 999; /* Asegura que el botón esté sobre otros elementos */
        }

        header {
            background-color: #333;
            color: #fff;
            padding: 10px;
            display: flex;
            justify-content: space-between;
            align-items: center;
        }
        #logo {
            width: 100px;
        }
        .app-name {
            font-size: 36px;
            font-family: "The Seasons";
            text-align: center;
            margin: 0;
            line-height: 1;
        }
        nav {
            display: flex;
        }
        nav a {
            color: #fff;
            text-decoration: none;
            margin-left: 20px;
        }
        .sidebar {
            height: 100vh;
            width: 0;
            position: fixed;
            top: 0;
            left: 0;
            background-color: #111;
            overflow-x: hidden;
            padding-top: 60px;
            transition: 0.3s;
        }
        .sidebar a {
            padding: 10px;
            text-decoration: none;
            font-size: 18px;
            color: #818181;
            display: block;
            transition: 0.3s;
        }
        .sidebar a:hover {
            color: #f1f1f1;
        }
        .open-btn {
            position: fixed;
            bottom: 20px;
            left: 20px;
            background-color: #333;
            color: #fff;
            padding: 10px;
            border: none;
            cursor: pointer;
            outline: none;
        }
        .images-container {
            display: flex;
            flex-wrap: wrap;
            justify-content: center;
            padding: 20px;
        }
        .images-container img {
            width: 200px;
            height: auto;
            margin: 10px;
            transition: transform 0.3s ease-in-out;
        }
        .images-container img:hover {
            transform: scale(1.1);
        }
        #fileUpload {
            margin-bottom: 10px;
        }
        .button-container {
            position: absolute;
            bottom: 10px;
            right: 10px;
        }
        #btnUpload {
            padding: 10px 20px;
            background-color: #4CAF50;
            color: white;
            border: none;
            border-radius: 5px;
            cursor: pointer;
            margin-right: 10px;
        }
    </style>
    
</head>
<body>
    <form id="form1" runat="server">
        <header>
            <div>
                <img id="logo" src="photos_default/magotipo.png" alt="Logo"/>
                <span class="app-name">DroidShare</span>
            </div>
        </header>

        <div class="images-container" id="div_fotos">
            <!-- Las imágenes se cargarán dinámicamente aquí -->
            <asp:Repeater ID="imageRepeater" runat="server">
                <ItemTemplate>
                    <img src='<%# Container.DataItem %>' width="200" height="auto" style="margin: 10px; transition: transform 0.3s ease-in-out;" />
                </ItemTemplate>
            </asp:Repeater>
        </div>  

        <div class="container">
            <br />
            <div class="button-container">
                <asp:Button ID="MostrarIP4" runat="server" type="button" Text="Recargar página" OnClick="MostrarIP4_Click" />
                <asp:Button ID="IniciarServer" runat="server" type="button" Text="Empezar a Recibir fotos" OnClick="startServerButton_Click" />
                
            <asp:Label ID="statusLabel" runat="server" Text="Servidor no iniciado." />
                <br />
            </div>
        </div>
        

    </form>
</body>
</html>
