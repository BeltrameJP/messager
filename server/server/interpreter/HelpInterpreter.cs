using System;
using System.Collections.Generic;
using System.Linq;
using server.socket;
using server.utils;

namespace server.interpreter {
    public class HelpInterpreter : Interpreter<Dictionary<string, SocketInstance>> {
        public HelpInterpreter() {
            this.Command = "help";
            this.ExternUse = true;
            this.Description = "Use " + this.Command + " para ter um resumo de todos os comandos disponíveis.";
        }

        public override UserCommand Read(UserCommand userCommand) {
            return userCommand;
        }
        public override UserCommand Action(UserCommand userCommand) {
            List<string> str = new List<string>();

            foreach (var (key, value) in MainSocket.GetInstance().InterpretersByCommand) {
                str.Add(((IInterpreter)value).GetCommand() + " -> " + ((IInterpreter)value).GetDescription());
            }

            Messager.SendMessage(userCommand.SocketInstance, string.Join("\r\n", str));

            return userCommand;
        }

        public override UserCommand Echo(UserCommand userCommand) {
            Console.WriteLine(userCommand.SocketInstance.Nickname + ": " + Command);

            return userCommand;
        }

        public override Dictionary<string, SocketInstance> GetInterpreterEnumerable() {
            throw new System.NotImplementedException();
        }

    }
}