using System;
using System.Net.Sockets;
using server.socket;

namespace server.utils {
    public static class Messager {

        public static void SendMessage(SocketInstance socketInstance, string message) {
            NetworkStream networkStream = socketInstance.Socket.GetStream();

            byte[] messageByte = System.Text.Encoding.UTF8.GetBytes(message);

            networkStream.Write(messageByte, 0, messageByte.Length);
            networkStream.Flush();
        }

        public static string ReadMessage(SocketInstance socketInstance) {
            byte[] inStream = new byte[4096];

            NetworkStream networkStream = socketInstance.Socket.GetStream();
            networkStream.Read(inStream, 0, inStream.Length);

            string str = System.Text.Encoding.UTF8.GetString(inStream);

            return str.Substring(0, str.IndexOf('\0'));
        }

    }
}