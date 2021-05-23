using System;
using System.Collections.Generic;
using System.Net.Sockets;
using server.socket;

namespace server.utils {
    public class UserCommand {
        public string InputMessage { get; set; }

        public List<string> Partials { get; set; }

        public string OutputMessage { get; set; }

        public bool Error { get; set; }

        public SocketInstance SocketInstance { get; set; }

        public UserCommand(string inputMessage, SocketInstance socketInstance) {
            InputMessage = inputMessage;
            SocketInstance = socketInstance;
            Error = false;

            Partials = new List<string>();
        }
    }
}