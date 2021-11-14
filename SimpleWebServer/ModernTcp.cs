﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SimpleWebServer
{
    // Code based on David Fowler's DotNetCodingPatterns for (modren) Asynchronous Socket Server
    // https://github.com/davidfowl/DotNetCodingPatterns/blob/main/2.md
    public class ModernTcp
    {
        private readonly int _port;
        public ModernTcp(int port)
        {
            _port = port;
        }
        public async Task StartAsync()
        {
            using var listenSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            listenSocket.Bind(new IPEndPoint(IPAddress.Loopback, _port));

            Console.WriteLine($"Listening on {listenSocket.LocalEndPoint}");

            listenSocket.Listen();

            while (true)
            {
                // Wait for a new connection to arrive
                var connection = await listenSocket.AcceptAsync();
                Console.WriteLine($"Connection accepted");
                // We got a new connection spawn a task to so that we can echo the contents of the connection
                _ = Task.Run(async () =>
                {
                    var buffer = new byte[4096];
                    try
                    {
                        while (true)
                        {
                            int read = await connection.ReceiveAsync(buffer, SocketFlags.None);
                            if (read == 0)
                            {
                                break;
                            }
                            var incomingMessage = Encoding.ASCII.GetString(buffer, 0, read);
                            Console.WriteLine(incomingMessage);
                            var responseMessage = GetResponseMessage("My responses are limited.");
                            //var outgoingMessage = Encoding.ASCII.GetBytes("My responses are limited.");
                            // await connection.SendAsync(buffer[..read], SocketFlags.None);
                            await connection.SendAsync(responseMessage, SocketFlags.None);
                        }
                    }
                    finally
                    {
                        connection.Dispose();
                    }
                });
            }
        }

        public byte[] GetResponseMessage(string message)
        {
            var result = "HTTP/1.1 200 Ok" + Environment.NewLine
                                + "Content-Length: " + message.Length + Environment.NewLine
                                + "Content-Type: " + "text/plain" + Environment.NewLine
                                + Environment.NewLine
                                + message
                                + Environment.NewLine + Environment.NewLine;
            return Encoding.ASCII.GetBytes(result);
        }
    }
}