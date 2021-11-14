// See https://aka.ms/new-console-template for more information
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Hello, World!");

bool done = false;
while (!done)
{
    try
    {
        var client = new TcpClient("localhost", 100);

        NetworkStream ns = client.GetStream();

        byte[] bytes = new byte[1024];
        int bytesRead = ns.Read(bytes, 0, bytes.Length);

        Console.WriteLine(Encoding.ASCII.GetString(bytes, 0, bytesRead));

        client.Close();

    }
    catch (Exception e)
    {
        Console.WriteLine(e.ToString());
    }
}
