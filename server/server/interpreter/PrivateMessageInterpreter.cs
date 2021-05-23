using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using server.socket;
using server.utils;

namespace server.interpreter {
    public class PrivateMessageInterpreter : Interpreter<Dictionary<string, SocketInstance>> {

        public PrivateMessageInterpreter() {
            this.Command = "w";
            this.ExternUse = true;
            this.Description = "Use " + this.Command + " <nome_do_participante> para enviar uma mensagem em privado.";
        }

        public override UserCommand Read(UserCommand userCommand) {
            GroupCollection groupCollection = IInterpreter.REGEX_FOR_TWO_ARGUMENTS.Match(userCommand.InputMessage).Groups;

            userCommand.Partials.Add(groupCollection[1].Value);
            userCommand.Partials.Add(groupCollection[2].Value);
            userCommand.Partials.Add(groupCollection[3].Value);

            return userCommand;
        }

        public override UserCommand Action(UserCommand userCommand) {
            MainSocket mainSocket = MainSocket.GetInstance();

            if (this.GetInterpreterEnumerable().ContainsKey(userCommand.Partials[1])) {
                SocketInstance socketInstance = this.GetInterpreterEnumerable()[userCommand.Partials[1]];
                userCommand.OutputMessage = userCommand.Partials[2];

                Messager.SendMessage(socketInstance, userCommand.SocketInstance.Nickname + ": " + userCommand.OutputMessage);
            } else {
                Messager.SendMessage(userCommand.SocketInstance, "Usuário não encontrado no servidor.");
            }

            return userCommand;
        }

        public override UserCommand Echo(UserCommand userCommand) {
            Console.WriteLine("Command " + this.Command + ":" + userCommand.SocketInstance.Nickname + " to " + userCommand.Partials[1]);
            return userCommand;
        }

        public override Dictionary<string, SocketInstance> GetInterpreterEnumerable() {
            return MainSocket.GetInstance().SocketInstancesByNickname;
        }
    }
}