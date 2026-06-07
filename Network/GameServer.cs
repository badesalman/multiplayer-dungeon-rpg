using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConOOD_RpgGame.Model;
using ConOOD_RpgGame.Network.DTO;
using ConOOD_RpgGame.World.Controllers;

namespace ConOOD_RpgGame.Network
{
    public class GameServer
    {
        private readonly TcpListener listener;
        private readonly MultiplayerGameModel model;
        private readonly GameController controller;

        private readonly List<ClientConnection> clients;
        private readonly object clientsLock = new object();

        private readonly AutoResetEvent actionSignal;
        private bool running;

        public GameServer(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
            model = new MultiplayerGameModel();
            controller = new GameController(model);

            clients = new List<ClientConnection>();
            actionSignal = new AutoResetEvent(false);
            running = true;
        }

        public void Start()
        {
            listener.Start();
            System.Console.WriteLine("Server started on port 5555.");

            Task.Run(() => ProcessActionQueue());

            while (running)
            {
                TcpClient tcpClient = listener.AcceptTcpClient();

                int playerId = model.AddPlayer();

                if (playerId == -1)
                {
                    tcpClient.Close();
                    continue;
                }

                ClientConnection connection =
                    new ClientConnection(playerId, tcpClient);

                lock (clientsLock)
                {
                    clients.Add(connection);
                }

                JsonProtocol.Send(connection.Writer, new NetworkMessageDto
                {
                    Type = "WELCOME",
                    PlayerId = playerId,
                    State = model.CreateStateDto(playerId),
                    Message = "Connected."
                });

                BroadcastState();

                Task.Run(() => HandleClient(connection));
            }
        }

        private void HandleClient(ClientConnection connection)
        {
            while (running)
            {
                NetworkMessageDto message =
                    JsonProtocol.Receive(connection.Reader);

                if (message == null)
                {
                    break;
                }

                message.PlayerId = connection.PlayerId;

                bool accepted = controller.HandlePlayerAction(message);

                if (accepted)
                {
                    actionSignal.Set();
                }
            }

            model.RemovePlayer(connection.PlayerId);

            lock (clientsLock)
            {
                clients.Remove(connection);
            }

            BroadcastState();
            connection.Close();
        }

        private void ProcessActionQueue()
        {
            while (running)
            {
                actionSignal.WaitOne();

                while (model.ProcessNextAction())
                {
                    BroadcastState();
                }
            }
        }

        private void BroadcastState()
        {
            lock (clientsLock)
            {
                foreach (ClientConnection client in clients.ToArray())
                {
                    GameStateDto state = model.CreateStateDto(client.PlayerId);

                    JsonProtocol.Send(client.Writer, new NetworkMessageDto
                    {
                        Type = "STATE",
                        State = state
                    });
                }
            }
        }

        private class ClientConnection
        {
            public int PlayerId { get; }
            private readonly TcpClient tcpClient;
            public StreamReader Reader { get; }
            public StreamWriter Writer { get; }

            public ClientConnection(int playerId, TcpClient tcpClient)
            {
                PlayerId = playerId;
                this.tcpClient = tcpClient;

                NetworkStream stream = tcpClient.GetStream();

                Reader = new StreamReader(stream, Encoding.UTF8);
                Writer = new StreamWriter(stream, Encoding.UTF8);
                Writer.AutoFlush = true;
            }

            public void Close()
            {
                tcpClient.Close();
            }
        }
    }
}