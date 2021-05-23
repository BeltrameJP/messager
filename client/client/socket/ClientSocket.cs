using System;
using System.Net.Sockets;
using System.Threading;

namespace client.socket {
    public class ClientSocket {

        private static ClientSocket _instance;

        private ClientSocket() {

        }

        public void Start() {
            TcpClient tcpClient = new TcpClient();

            Console.WriteLine("Insira o IP de conexão (O padrão é: 127.0.0.1)");
            tcpClient.Connect(Console.ReadLine(), 8888);

            NetworkStream serverStream = tcpClient.GetStream();


            Thread writeThread = new Thread(() => WriteToServer(serverStream));
            Thread listenThread = new Thread(() => ListenToServer(serverStream, tcpClient));

            listenThread.Start();
            writeThread.Start();
        }

        private void ListenToServer(NetworkStream serverStream, TcpClient tcpClient) {
            byte[] inStream;
            while (true) {
                inStream = new byte[4096];
                serverStream.Read(inStream, 0, inStream.Length);

                string str = System.Text.Encoding.UTF8.GetString(inStream);

                Console.WriteLine(str.Substring(0, str.IndexOf('\0')));
            }
        }

        private void WriteToServer(NetworkStream serverStream) {
            byte[] outStream;
            while (true) {
                outStream = System.Text.Encoding.UTF8.GetBytes(Console.ReadLine());

                serverStream.Write(outStream, 0, outStream.Length);
            }
        }


        public static ClientSocket GetInstance() {
            if (_instance == null) {
                _instance = new ClientSocket();
            }

            return _instance;
        }
    }
}