using System;
using System.Collections;
using System.Collections.Generic;
using server.socket;

namespace server {
    class Program {
        static void Main(string[] args) {
            Console.WriteLine("Servidor iniciado");
            MainSocket.GetInstance().Start();

            List<string> keyList = new List<string>(MainSocket.GetInstance().InterpretersByCommand.Keys);

            foreach (string key in keyList) {
                Console.WriteLine(key);
            }

        }
    }
}
