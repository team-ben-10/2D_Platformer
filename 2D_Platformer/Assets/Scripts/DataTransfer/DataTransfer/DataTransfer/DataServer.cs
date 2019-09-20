using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Reflection;

namespace DataTransfer.DataTransfer
{
    class DataServer
    {
        public List<object> savedVars = new List<object>();
        public delegate bool OnServerCallback(int clientID, object data);
        public OnServerCallback onMessageRecieved;
        public OnServerCallback onClientDisconnect;
        public int Port { get; private set; }
        public List<TcpClient> ConnectedClients { get; private set; }
        Type type;
        public DataServer(int port, Type type)
        {
            Port = port;
            ConnectedClients = new List<TcpClient>();
            this.type = type;
        }

        public void Setup()
        {
            Console.WriteLine("Server started!");
            IPEndPoint iPEndPoint = new IPEndPoint(IPAddress.Any, Port);
            TcpListener listener = new TcpListener(iPEndPoint);
            listener.Start();
            new Thread(() =>
            {
                TcpClient client = listener.AcceptTcpClient();
                while (client != null)
                {
                    if (client != null && client.Connected)
                    {
                        ConnectedClients.Add(client);
                        BroadCast("Ein neuer Client ist gejoint!");
                        Console.WriteLine("Neuer Client gejoint!");
                    }
                    client = listener.AcceptTcpClient();
                }
            }).Start();
        }

        public void Run()
        {
            new Thread(() =>
            {
                while (true)
                {
                    string message = Console.ReadLine();
                    var methods = Assembly.GetAssembly(type).GetTypes()
                      .SelectMany(t => t.GetMethods())
                      .Where(m => m.GetCustomAttributes(typeof(CommandAttribute), false).Length > 0)
                      .ToArray();
                    string[] splitter = message.Split(' ');
                    foreach (var item in methods)
                    {
                        if (((CommandAttribute)item.GetCustomAttribute(typeof(CommandAttribute), false)).Name == splitter[0])
                        {
                            List<string> args = new List<string>();
                            for (int i = 1; i < splitter.Length; i++)
                            {
                                args.Add(splitter[i]);
                            }
                            item.Invoke(null, args.ToArray());
                        }
                    }
                }
            }).Start();
        }

        public void ListenForInput()
        {
            while (true)
            {
                if (ConnectedClients.Count > 0)
                {
                    for (int i = 0; i < ConnectedClients.Count; i++)
                    {
                        if (i <= ConnectedClients.Count)
                        {
                            TcpClient client = ConnectedClients[i];
                            if (client != null)
                            {
                                if (client.Connected)
                                {
                                    NetworkStream stream = client.GetStream();

                                    if (stream.DataAvailable)
                                    {
                                        byte[] buffer = new byte[Console.WindowWidth - 1];
                                        stream.Read(buffer, 0, buffer.Length);
                                        string message = Encoding.UTF8.GetString(buffer);
                                        if (message.Trim() != "")
                                        {
                                            if (message.Trim().Contains("!disconnect!"))
                                            {
                                                var save = onClientDisconnect;
                                                onClientDisconnect.Invoke(i,null);
                                                ConnectedClients[i] = null;
                                                client.Dispose();
                                                onClientDisconnect = save;
                                            }
                                            else
                                            {
                                                Console.WriteLine(message);
                                                if (onMessageRecieved != null)
                                                {
                                                    var save = (OnServerCallback)onMessageRecieved.Clone();
                                                    if (onMessageRecieved.Invoke(i, message))
                                                    {
                                                        for (int a = 0; a < ConnectedClients.Count; a++)
                                                        {
                                                            TcpClient client2 = ConnectedClients[a];
                                                            if (client2 != null)
                                                            {
                                                                if (client != client2 && client2.Connected)
                                                                {
                                                                    NetworkStream sr = client2.GetStream();
                                                                    sr.Write(buffer, 0, buffer.Length);
                                                                }
                                                            }
                                                        }
                                                    }
                                                    onMessageRecieved = save;
                                                }
                                                else
                                                {
                                                    for (int a = 0; a < ConnectedClients.Count; a++)
                                                    {
                                                        TcpClient client2 = ConnectedClients[a];
                                                        if (client != client2 && client2.Connected)
                                                        {
                                                            NetworkStream sr = client2.GetStream();
                                                            sr.Write(buffer, 0, buffer.Length);
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        public void BroadCast(string message)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            for (int a = 0; a < ConnectedClients.Count; a++)
            {
                if (ConnectedClients[a] != null)
                {
                    TcpClient client2 = ConnectedClients[a];
                    NetworkStream sr = client2.GetStream();
                    sr.Write(buffer, 0, buffer.Length);
                }
            }
        }

        public void SendMessage(string message, int clientID)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(message);
            if (clientID > -1 && clientID < ConnectedClients.Count)
            {
                if (ConnectedClients[clientID] != null)
                {
                    TcpClient client2 = ConnectedClients[clientID];
                    NetworkStream sr = client2.GetStream();
                    sr.Write(buffer, 0, buffer.Length);
                }
            }
        }

        public void Kick(int clientID)
        {
            SendMessage("You have been kicked!", clientID);
            ConnectedClients[clientID].Dispose();
            ConnectedClients[clientID] = null;
        }

        public void Ban(int clientID)
        {
            ConnectedClients[clientID].Client.Disconnect(false);
        }
        
        public int getClientIDFromClient(TcpClient client)
        {
            for (int i = 0; i < ConnectedClients.Count; i++)
            {
                var currentclient = ConnectedClients[i];
                if(currentclient.Connected && client.Connected && currentclient == client)
                {
                    return i;
                }
            }
            return -1;
        }

    }
}
