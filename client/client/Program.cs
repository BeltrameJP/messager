
using client.socket;

namespace client {
    class Program {
        static void Main(string[] args) {
            ClientSocket clientSocket = ClientSocket.GetInstance();

            clientSocket.Start();
        }


    }

}
