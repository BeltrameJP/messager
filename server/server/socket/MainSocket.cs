using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using server.utils;

namespace server.socket {
    public class MainSocket {
        private static MainSocket _instance;

        private TcpListener tcpListener;

        private int idCounter;

        private Dictionary<int, Thread> ThreadsById { get; set; }

        public Dictionary<string, SocketInstance> SocketInstancesByNickname { get; set; }

        public Dictionary<string, IInterpreter> InterpretersByCommand { get; private set; }

        public Dictionary<string, IInterpreter> PrivateInterpretersByCommand { get; private set; }

        private MainSocket() {
            ThreadsById = new Dictionary<int, Thread>();
            SocketInstancesByNickname = new Dictionary<string, SocketInstance>();
            InterpretersByCommand = new Dictionary<string, IInterpreter>();
            PrivateInterpretersByCommand = new Dictionary<string, IInterpreter>();
        }

        public void Start() {
            InterpreterScanner.Scan();

            idCounter = 0;
            tcpListener = new TcpListener(IPAddress.Any, 8888);
            tcpListener.Start();

            Console.WriteLine("Servidor iniciado. Aguardando conexão");

            while (true) {
                TcpClient tcpClient = tcpListener.AcceptTcpClient();
                Thread thread = new Thread(() => AcceptingClient(tcpClient, idCounter));

                thread.Start();
                ThreadsById.Add(idCounter, thread);

                Console.WriteLine("Aguardando o próximo cliente. Número total de conexões feitas: " + idCounter);
                idCounter++;
            }

        }

        public void AcceptingClient(TcpClient tcpClient, int idCounter) {
            int idClient = idCounter;
            string nickname = null;
            try {
                byte[] inStream = new byte[4096];

                SocketInstance socketInstance = new SocketInstance(idClient, tcpClient);

                while (true) {
                    Messager.SendMessage(socketInstance, "Insira seu Nickname");

                    socketInstance.Nickname = nickname = Messager.ReadMessage(socketInstance).Replace(@"[^0-9a-zA-Z]+", "_");




                    UserCommand userCommand = new UserCommand(
                                                socketInstance.Nickname,
                                                socketInstance
                                            );

                    PrivateInterpretersByCommand["enternickname"].Run(userCommand);

                    if (userCommand.Error) {
                        Messager.SendMessage(socketInstance, "Nickname já utilizado. Tente novamente. ");
                    } else {
                        break;
                    }

                }

                while (true) {
                    string message = Messager.ReadMessage(socketInstance);
                    string[] split = message.Split(' ');
                    if (InterpretersByCommand.ContainsKey(split[0])) {
                        InterpretersByCommand[split[0]].Run(
                            new UserCommand(
                                message,
                                socketInstance
                            )
                        );
                    } else {
                        Messager.SendMessage(socketInstance, "Comando não encontrado. Use 'help' para ver os comandos disponíveis.");
                    }
                }
            } catch (Exception e) when (e is SocketException || e is IOException) {
                if (nickname != null) {
                    Console.WriteLine("Cliente Id " + idClient + " (" + nickname + ") se desconectou.");
                    SocketInstancesByNickname.Remove(nickname);
                } else {
                    Console.WriteLine("Cliente Id " + idClient + " se desconectou.");
                }

                // ThreadsById[idClient].Interrupt();
            }
        }

        public static MainSocket GetInstance() {
            if (_instance == null) {
                _instance = new MainSocket();
            }

            return _instance;
        }
    }
}