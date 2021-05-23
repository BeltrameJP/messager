using System.Net.Sockets;

namespace server.socket {
    public class SocketInstance {

        private int Id { get; set; }
        public string Nickname { get; set; }
        public TcpClient Socket { get; private set; }

        public SocketInstance() { }

        public SocketInstance(int id, TcpClient socket) {
            Id = id;
            Socket = socket;
        }

        public void Start() {
            byte[] inStream = new byte[4096];
            while (true) {

            }
        }

    }
}