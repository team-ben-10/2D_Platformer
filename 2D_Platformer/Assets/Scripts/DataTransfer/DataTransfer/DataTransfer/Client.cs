using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace DataTransfer.DataTransfer
{
    class Client
    {
        public int Port { get; private set; }
        public IPAddress IP { get; private set; }
        public TcpClient TCPClient { get; private set; }
        public delegate bool OnClientCallback(object data);
        public OnClientCallback onMessageRecieved;
        public OnClientCallback onFailedConnection;
        public Client(string ip, int port)
        {
            IP = IPAddress.Parse(ip);
            Port = port;
        }
        public void Setup()
        {
            IPEndPoint iPEndPoint = new IPEndPoint(IP, Port);
            TCPClient = new TcpClient();
            try
            {
                TCPClient.Connect(iPEndPoint);
            }
            catch (Exception e)
            {
                if (onFailedConnection != null)
                    onFailedConnection.Invoke(e);
            }
        }

        public void Writing()
        {
            new Thread(() =>
            {
                while (TCPClient.Connected)
                {
                    string msg = Console.ReadLine();
                    WriteMessage(msg);
                }
            }).Start();
        }

        public void Run()
        {
            new Thread(() =>
            {
                while (TCPClient.Connected)
                {
                    NetworkStream stream = TCPClient.GetStream();
                    if (stream.DataAvailable)
                    {
                        byte[] buffer = new byte[300 * 150 * 5];
                        stream.Read(buffer, 0, buffer.Length);
                        string message = Encoding.UTF8.GetString(buffer);
                        if (onMessageRecieved != null)
                        {
                            var save = onMessageRecieved;
                            if (message.Trim() != "" && onMessageRecieved.Invoke(message))
                            {
                                Console.WriteLine(message);
                            }
                            onMessageRecieved = save;
                        }
                        else
                        {
                            if (message.Trim() != "")
                            {
                                Console.WriteLine(message);
                            }
                        }
                    }
                }
            }).Start();
        }

        public void WriteMessage(string msg)
        {
            NetworkStream sr = TCPClient.GetStream();
            if (msg.Trim() != "")
            {
                byte[] buffer = Encoding.UTF8.GetBytes(msg);
                sr.Write(buffer, 0, buffer.Length);
            }
        }

        byte[] ShortenByteArray(byte[] arr)
        {
            List<byte> bytes = new List<byte>();
            foreach (var item in arr)
            {
                if (item != 0)
                {
                    bytes.Add(item);
                }
            }
            return bytes.ToArray();
        }
    }
}
