using System;
using System.Net.Sockets;
using System.Net.WebSockets;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TcpTestServer {
    [TestClass]
    public class UnitTest1 {

        [TestMethod]
        public void Test_Equal_NickName() {
            string sameName = "beltramejp";

            TcpClient tcpClient1 = MethodBag.InitializeConection();
            NetworkStream networkStream1 = tcpClient1.GetStream();

            MethodBag.WriteToServer(networkStream1, sameName);
            string response1 = MethodBag.ListenToServer(networkStream1);

            TcpClient tcpClient2 = MethodBag.InitializeConection();
            NetworkStream networkStream2 = tcpClient1.GetStream();

            MethodBag.WriteToServer(networkStream2, sameName);
            string response2 = MethodBag.ListenToServer(networkStream1);

            tcpClient1.Close();
            tcpClient2.Close();

            if (!response2.StartsWith("Bem vindo")) {
                throw new Exception("Existem duas pessoas com o mesmo nick");
            }
        }

        [TestMethod]
        public void Test_Private_Message() {
            string name1 = "beltramejp";
            string name2 = "joaobeltrame";
            string response1, response2, response3;

            TcpClient tcpClient1 = MethodBag.InitializeConection();
            NetworkStream networkStream1 = tcpClient1.GetStream();
            response1 = MethodBag.ListenToServer(networkStream1);

            MethodBag.WriteToServer(networkStream1, name1);
            response1 = MethodBag.ListenToServer(networkStream1);

            TcpClient tcpClient2 = MethodBag.InitializeConection();
            NetworkStream networkStream2 = tcpClient2.GetStream();
            response2 = MethodBag.ListenToServer(networkStream2);

            MethodBag.WriteToServer(networkStream2, name2);
            response2 = MethodBag.ListenToServer(networkStream2);

            string[] messageTo2 = { "w", name2, "TESTE" };
            MethodBag.WriteToServer(networkStream1, string.Join(" ", messageTo2));

            response3 = MethodBag.ListenToServer(networkStream2);

            tcpClient1.Close();
            tcpClient2.Close();

            if (!response3.StartsWith(name1) || !response3.Contains(messageTo2[2])) {
                throw new Exception("A resposta não chegou como deveria: " + response3);
            }

        }

        [TestMethod]
        public void Test_Broadcast_Message() {
            string name1 = "beltramejp";
            string name2 = "joaobeltrame";

            TcpClient tcpClient1 = MethodBag.InitializeConection();
            NetworkStream networkStream1 = tcpClient1.GetStream();
            string response1 = MethodBag.ListenToServer(networkStream1);

            MethodBag.WriteToServer(networkStream1, name1);
            string response2 = MethodBag.ListenToServer(networkStream1);

            TcpClient tcpClient2 = MethodBag.InitializeConection();
            NetworkStream networkStream2 = tcpClient2.GetStream();
            string response3 = MethodBag.ListenToServer(networkStream2);

            MethodBag.WriteToServer(networkStream2, name2);
            string response4 = MethodBag.ListenToServer(networkStream2);

            string[] broadcast = { "broadcast", "Ola Mundo" };
            MethodBag.WriteToServer(networkStream1, string.Join(" ", broadcast));

            string response5 = MethodBag.ListenToServer(networkStream2);

            if (!response5.Contains(broadcast[1]) || !response5.Contains(name1)) {
                throw new Exception("A resposta não chegou como deveria: " + response3);
            }

        }
    }

    public static class MethodBag {
        public static TcpClient InitializeConection() {
            TcpClient tcpClient = new TcpClient();

            tcpClient.Connect("127.0.0.1", 8888);

            return tcpClient;
        }

        public static string ListenToServer(NetworkStream serverStream) {
            byte[] inStream = new byte[4096];

            serverStream.Read(inStream, 0, inStream.Length);

            string str = System.Text.Encoding.UTF8.GetString(inStream);

            return (str.Substring(0, str.IndexOf('\0')));
        }

        public static void WriteToServer(NetworkStream serverStream, string message) {
            byte[] outStream = System.Text.Encoding.UTF8.GetBytes(message);

            serverStream.Write(outStream, 0, outStream.Length);
        }
    }
}
