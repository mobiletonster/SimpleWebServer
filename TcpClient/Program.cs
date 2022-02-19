// See https://aka.ms/new-console-template for more information
using System.Net.Sockets;
using System.Text;

Console.WriteLine("Hello, World!");

bool done = false;
while (!done)
{
    try
    {
        var client = new TcpClient("localhost", 8800);

        NetworkStream ns = client.GetStream();

        byte[] byteMessage = Encoding.ASCII.GetBytes("Hey Siru");

            await ns.WriteAsync(byteMessage, 0, byteMessage.Length);

            byte[] bytes = new byte[1024];
        int bytesRead = await ns.ReadAsync(bytes, 0, bytes.Length);

        Console.WriteLine(Encoding.ASCII.GetString(bytes, 0, bytesRead));

        client.Close();

    }
    catch (Exception e)
    {
        Console.WriteLine(e.ToString());
    }
}
