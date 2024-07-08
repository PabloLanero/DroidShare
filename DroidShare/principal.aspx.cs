using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using System.Text;
using System.Net.Sockets;
using System.Net.NetworkInformation;


public partial class principal : System.Web.UI.Page
{
    private const string UploadFolder = "photos_default";
    private const string Url = "http://localhost:5000/upload/";
    
    protected async void Page_Load(object sender, EventArgs e)
    {

        
            await LoadPhotosAsync();
        string ipAddress = GetLocalIPv4();
        statusLabel.Text = "Tu codigo para utilizar en el movil es: "+ ipAddress;


    }

    protected async void startServerButton_Click(object sender, EventArgs e)
    {
        try
        {
            // Nombre del archivo batch a ejecutar
            string batFileName = "run_upload_server.bat";
            // Directorio de trabajo actual de la aplicación
            string workingDirectory = AppDomain.CurrentDomain.BaseDirectory;
            // Ruta completa del archivo batch
            string batFilePath = Path.Combine(workingDirectory, batFileName);
            // Configurar la información para iniciar el proceso
            ProcessStartInfo processStartInfo = new ProcessStartInfo(batFilePath)
            {
                WorkingDirectory = workingDirectory,
                UseShellExecute = false, // No usar el shell del sistema operativo para ejecutar el proceso
                CreateNoWindow = true, // No crear una ventana para el proceso
                RedirectStandardOutput = true, // Redirigir la salida estándar del proceso
                RedirectStandardError = true // Redirigir los errores estándar del proceso
            };

            // Crear y configurar el proceso
            Process process = new Process
            {
                StartInfo = processStartInfo
            };

            // Iniciar el proceso
            process.Start();
            // Leer la salida estándar y los errores del proceso
            string output = process.StandardOutput.ReadToEnd();
            string errors = process.StandardError.ReadToEnd();
            // Esperar a que el proceso termine
            process.WaitForExit();

            // Aquí puedes manejar la salida del proceso si es necesario
            Console.WriteLine(output);
            if (!string.IsNullOrEmpty(errors))
            {
                statusLabel.Text= ("Errores: " + errors);
            }
        }
        catch (Exception ex)
        {
            // Manejo de excepciones
            statusLabel.Text = ("Ocurrió un error al iniciar el servicio: " + ex.Message);
        }
    }



    private static async Task HandleIncomingConnections(HttpListener listener)
    {
        //while (true)
        //{
            // Esperar una nueva conexión HTTP entrante
            HttpListenerContext context = await listener.GetContextAsync();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            // Comprobar si la solicitud es un POST y si la URL es /upload/
            if (request.HttpMethod == "POST" && request.Url.AbsolutePath == "/upload/")
            {
                // Comprobar si la solicitud contiene un cuerpo (entidad)
                if (!request.HasEntityBody)
                {
                    response.StatusCode = (int)HttpStatusCode.BadRequest;
                    await WriteResponse(response, "No file part");
                    //continue;
                }
                // Obtener el nombre del archivo del encabezado de la solicitud
                try
                {
                    string filename = request.Headers["filename"];
                    if (string.IsNullOrEmpty(filename))
                    {
                        response.StatusCode = (int)HttpStatusCode.BadRequest;
                        await WriteResponse(response, "No selected file");
                       // continue;
                    }
                    // Combinar la ruta del directorio de carga con el nombre del archivo
                    string filePath = Path.Combine(UploadFolder, filename);
                    // Crear un archivo y copiar el contenido de la solicitud en él
                    using (FileStream fs = new FileStream(filePath, FileMode.Create))
                    {
                        await request.InputStream.CopyToAsync(fs);
                    }
                    // Responder con un estado OK
                    response.StatusCode = (int)HttpStatusCode.OK;
                    await WriteResponse(response, "File successfully uploaded");
                }
                catch (Exception ex)
                {
                    // Manejo de excepciones: responder con un error interno del servidor
                    response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    await WriteResponse(response, $"File upload failed: {ex.Message}");
                }
            }
            else
            {
                // Si no es una solicitud válida, responder con Not Found
                response.StatusCode = (int)HttpStatusCode.NotFound;
                await WriteResponse(response, "Not Found");
            }
        //}
    }

    private static async Task WriteResponse(HttpListenerResponse response, string message)
    {
        // Escribir la respuesta en el cuerpo de la respuesta HTTP
        byte[] data = Encoding.UTF8.GetBytes(message);
        response.ContentType = "text/plain";
        response.ContentEncoding = Encoding.UTF8;
        response.ContentLength64 = data.Length;
        await response.OutputStream.WriteAsync(data, 0, data.Length);
        response.Close();
    }


    private async Task LoadPhotosAsync()
    {
        string photosPath = Server.MapPath("photos_default");
        if (Directory.Exists(photosPath))
        {
            var photos = Directory.GetFiles(photosPath);
            imageRepeater.DataSource = photos.Select(path => ResolveUrl("~/photos_default/" + Path.GetFileName(path)));
            imageRepeater.DataBind();
        }
    }

    static string GetLocalIPv4()
    {
        string ipAddress = String.Empty;

        // Obtener todas las interfaces de red disponibles
        NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();

        foreach (NetworkInterface network in interfaces)
        {
            // Filtrar solo las interfaces que están activas y son IPv4
            if (network.OperationalStatus == OperationalStatus.Up &&
                network.NetworkInterfaceType != NetworkInterfaceType.Loopback &&
                network.NetworkInterfaceType != NetworkInterfaceType.Tunnel &&
                network.NetworkInterfaceType != NetworkInterfaceType.Unknown)
            {
                foreach (UnicastIPAddressInformation ip in network.GetIPProperties().UnicastAddresses)
                {
                    // Buscar la primera dirección IPv4
                    if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                    {
                        ipAddress = ip.Address.ToString();
                        return ipAddress;
                    }
                }
            }
        }

        return ipAddress;
    }

    

    private void LoadPhotos()
    {
        string photosPath = Server.MapPath("photos_default");
        if (Directory.Exists(photosPath))

        {
            var photos = Directory.GetFiles(photosPath);
            imageRepeater.DataSource = photos.Select(path => ResolveUrl("~/photos_default/" + Path.GetFileName(path)));
            imageRepeater.DataBind();
        }
        
    }


    protected void MostrarIP4_Click(object sender, EventArgs e)
    {
        string ipAddress = GetLocalIPv4();
    }
}
