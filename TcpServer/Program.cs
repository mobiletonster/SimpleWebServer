// See https://aka.ms/new-console-template for more information
using System.Net.Sockets;
using System.Net;
using System.Text;

Console.WriteLine("Hello, World!");
bool done = false;

var listener = new TcpListener(IPAddress.Any, 100);

listener.Start();
int x = 0;

while (!done)
{
    Console.Write("Waiting for connection...");
    TcpClient client = await listener.AcceptTcpClientAsync();
    // TcpClient client = listener.AcceptTcpClient();

    Console.WriteLine($"Connection accepted. {x}");
    
    NetworkStream ns = client.GetStream();

    byte[] bytes = new byte[1024];
    int bytesRead = await ns.ReadAsync(bytes, 0, bytes.Length);

    Console.WriteLine(Encoding.ASCII.GetString(bytes, 0, bytesRead));


    byte[] byteTime = Encoding.ASCII.GetBytes(DateTime.Now.ToString() + $" {x}");

    try
    {
        await ns.WriteAsync(byteTime,0, byteTime.Length);
        //ns.Write(byteTime, 0, byteTime.Length);
        
        ns.Close();
        client.Close();
    }
    catch (Exception e)
    {
        Console.WriteLine(e.ToString());
    }
    x++;
}

listener.Stop();
