using SimpleWebServer;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Begin server");
        //var server = new HttpServer(8000);
        //var server = new SimpleListener(8000);
        var server = new ModernTcp(8000);
        await server.StartAsync();
        Console.WriteLine("End server");
    }
}


public class HttpServer 
{
    private readonly TcpListener _listener;
    private readonly int _port;

    public HttpServer(int port)
    {
        _port = port;
        _listener = new TcpListener(IPAddress.Parse("127.0.0.1"), _port);
    }

    public async Task StartAsync()
    {
        bool done = false;
        int request = 0;
        _listener.Start();
        Console.WriteLine($"Listening on http://localhost:{_port}");
        while (!done)
        {
            Console.Write("Waiting for connection...");
            using (var client = await _listener.AcceptTcpClientAsync())
            {
                request++;
                Console.WriteLine($"Connection request #{request}.");
                using (var stream = client.GetStream())
                {
                    var buffer = new byte[256];
                    var length = await stream.ReadAsync(buffer, 0, buffer.Length);
                    var incomingMessage = Encoding.UTF8.GetString(buffer, 0, length);
                    if (incomingMessage.Contains("/favicon.ico"))
                    {
                        byte[] notFoundResult = Encoding.UTF8.GetBytes("HTTP/1.1 404 Not Found");
                        try
                        {
                            await stream.WriteAsync(notFoundResult, 0, notFoundResult.Length);

                            var data = new Byte[256];

                            // String to store the response ASCII representation.
                            String responseData = String.Empty;

                            // Read the first batch of the TcpServer response bytes.
                            Int32 bytes = stream.Read(data, 0, data.Length);
                            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                            Console.WriteLine("Received: {0}", responseData);

                            stream.Close();
                            client.Close();

                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());

                        }
                    }
                    else
                    {
                        // Write the response now.
                        var result = $"<!DOCTYPE><html><body><h1>My responses are limited. Request #{request} {DateTime.Now}</h1></body></html>";

                        byte[] byteResult = Encoding.UTF8.GetBytes("HTTP/1.1 200 Ok" + Environment.NewLine
                                + "Content-Length: " + result.Length + Environment.NewLine
                                + "Content-Type: " + "text/html" + Environment.NewLine

                                + Environment.NewLine
                                + result
                                + Environment.NewLine + Environment.NewLine);

                        try
                        {
                            await stream.WriteAsync(byteResult, 0, byteResult.Length);

                            var data = new Byte[256];

                            // String to store the response ASCII representation.
                            String responseData = String.Empty;

                            // Read the first batch of the TcpServer response bytes.
                            Int32 bytes = stream.Read(data, 0, data.Length);
                            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                            Console.WriteLine("Received: {0}", responseData);

                            stream.Close();
                            client.Close();
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.ToString());
                        }
                    }
                }
            }

        }

        _listener.Stop();
    }
}



