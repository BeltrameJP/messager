using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using server.socket;
using server.utils;

namespace server.interpreter {
    public class BroadcastInterpreter : Interpreter<Dictionary<string, SocketInstance>> {
        public BroadcastInterpreter() {
            this.Command = "broadcast";
            this.ExternUse = true;
            this.Description = "Use " + Command + " <mensagem> para enviar uma mensagem a todos no servidor.";
        }
        public override UserCommand Read(UserCommand userCommand) {
            GroupCollection groupCollection = IInterpreter.REGEX_FOR_SINGLE_ARGUMENT.Match(userCommand.InputMessage).Groups;

            userCommand.Partials.Add(groupCollection[1].Value);
            userCommand.Partials.Add(groupCollection[2].Value);

            return userCommand;
        }
        public override UserCommand Action(UserCommand userCommand) {
            foreach (var (key, value) in MainSocket.GetInstance().SocketInstancesByNickname) {
                Messager.SendMessage((SocketInstance)value, "'" + userCommand.SocketInstance.Nickname + "'" + " para todos: " + userCommand.Partials[1]);
            }

            return userCommand;
        }

        public override UserCommand Echo(UserCommand userCommand) {
            Console.WriteLine("Command " + this.Command + ": " + userCommand.SocketInstance.Nickname);

            return userCommand;
        }


        public override Dictionary<string, SocketInstance> GetInterpreterEnumerable() {
            return MainSocket.GetInstance().SocketInstancesByNickname;
        }

    }
}
