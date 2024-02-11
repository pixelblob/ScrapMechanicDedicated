using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.CodeDom.Compiler;
using System.CodeDom;

using static ScrapMechanicDedicated.GameStateController;
using static ScrapMechanicDedicated.GameStateManager;

namespace ScrapMechanicDedicated
{
    static class TcpServer
    {
        static TcpListener server = new TcpListener(IPAddress.Any, 8989);
        static List<TcpClient> listConnectedClients = new List<TcpClient>();

        public static void startTcpServer()
        {

            ServerStarted += updateTcpServerState;
            ServerStopped += updateTcpServerState;
            ServerSuspended += updateTcpServerState;
            ServerResumed += updateTcpServerState;
            ServerPlayerJoined += updateTcpServerPlayers;
            ServerPlayerLeft += updateTcpServerPlayers;

            server.Start();
            accept_connection();
            //Thread th = new Thread(new ThreadStart(StartAccepter));
            //th.Start();
        }

        private static void updateTcpServerState()
        {
            broadcastLine("serverRunning " + serverRunning);
            broadcastLine("serverSuspended " + serverSuspended);
        }

        private static void updateTcpServerPlayers(string name)
        {
            broadcastLine("playerCount " + playerCount);
            string playerString = string.Join(",", playersList);
            broadcastLine("playerList " + playerString);
        }

        private static void accept_connection()
        {
            server.BeginAcceptTcpClient(handle_connection, server);  //this is called asynchronously and will run in a different thread
        }

        private static void handle_connection(IAsyncResult result)  //the parameter is a delegate, used to communicate between threads
        {
            Debug.WriteLine("NEW CON HANDLE");
            accept_connection();  //once again, checking for any other incoming connections
            Debug.WriteLine("NEW CON HANDLE ACC");
            TcpClient client = server.EndAcceptTcpClient(result);  //creates the TcpClient
            listConnectedClients.Add(client);

            Debug.WriteLine("NEW CON HANDLE STRM");

            NetworkStream ns = client.GetStream();

            broadcastLine("serverRunning " + serverRunning);
            broadcastLine("serverSuspended " + serverSuspended);
            broadcastLine("playerCount " + playerCount);
            string playerString = string.Join(",", playersList);
            broadcastLine("playerList " + playerString);

            while (client.Connected)  //while the client is connected, we look for incoming messages
            {
                byte[] msg = new byte[1024];     //the messages arrive as byte array
                Debug.WriteLine("NEW CON HANDLE WAIT MSG");
                int n = 0;
                try
                {
                    n = ns.Read(msg, 0, msg.Length);   //the same networkstream reads the message sent by the client
                }
                catch (Exception)
                {

                }
                Debug.WriteLine("NEW CON HANDLE GOT MSG");

                if (n == 0)
                {
                    Debug.WriteLine("Client has been disconnected?");
                    listConnectedClients.Remove(client);
                    return; // connection is lost
                }

                var textMessage = Encoding.Default.GetString(msg).Trim();

                using (System.IO.StringReader reader = new System.IO.StringReader(textMessage))
                {
                    string line = reader.ReadLine();
                    Debug.WriteLine(line);
                    if (line == "startServer")
                    {
                        Debug.WriteLine($"Start Server TCP CLIENT");
                        startServer();
                    }
                    else if (line == "stopServer")
                    {
                        Debug.WriteLine($"Stop Server TCP CLIENT");
                        stopServer();
                    }
                    else if (line == "resumeServer")
                    {
                        Debug.WriteLine($"Resume Server TCP CLIENT");
                        resumeServer();
                    }
                    else if (line == "suspendServer")
                    {
                        Debug.WriteLine($"Suspend Server TCP CLIENT");
                        suspendServer();
                    }
                }

            };

        }


        public static void broadcastLine(string line)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(line + "\r\n");

            broadcastToAll(bytes);
        }

        public static void broadcastToAll(byte[] bytes)
        {
            foreach (var client in listConnectedClients)
            {
                try
                {
                    client.Client.Send(bytes);
                }
                catch (Exception)
                {
                }

            }
        }

        private static string ToLiteral(string input)
        {
            using (var writer = new StringWriter())
            {
                using (var provider = CodeDomProvider.CreateProvider("CSharp"))
                {
                    provider.GenerateCodeFromExpression(new CodePrimitiveExpression(input), writer, null);
                    return writer.ToString();
                }
            }
        }

        static bool SocketConnected(Socket s)
        {
            bool part1 = s.Poll(1000, SelectMode.SelectRead);
            bool part2 = (s.Available == 0);
            if (part1 & part2)
            {//connection is closed
                return false;
            }
            return true;
        }
    }
}
