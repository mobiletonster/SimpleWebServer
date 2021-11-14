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
    TcpClient client = listener.AcceptTcpClient();

    Console.WriteLine($"Connection accepted. {x}");
    NetworkStream ns = client.GetStream();

    byte[] byteTime = Encoding.ASCII.GetBytes(DateTime.Now.ToString() + $" {x}");

    try
    {
        ns.Write(byteTime, 0, byteTime.Length);
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
