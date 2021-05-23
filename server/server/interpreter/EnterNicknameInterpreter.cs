
using System;
using System.Collections.Generic;
using server.socket;
using server.utils;

namespace server.interpreter {
    public class EnterNicknameInterpreter : Interpreter<Dictionary<string, SocketInstance>> {
        public EnterNicknameInterpreter() {
            this.Command = "enternickname";
            this.ExternUse = false;
        }
        public override UserCommand Read(UserCommand userCommand) {
            return userCommand;
        }
        public override UserCommand Action(UserCommand userCommand) {
            if (!MainSocket.GetInstance().SocketInstancesByNickname.ContainsKey(userCommand.SocketInstance.Nickname)) {
                MainSocket.GetInstance().SocketInstancesByNickname[userCommand.SocketInstance.Nickname] = userCommand.SocketInstance;
            } else {
                userCommand.Error = true;
            }

            return userCommand;
        }

        public override UserCommand Echo(UserCommand userCommand) {
            if (userCommand.Error) {
                Messager.SendMessage(userCommand.SocketInstance, "Nickname já está em uso ou é inválido");
            } else {
                Messager.SendMessage(userCommand.SocketInstance, "Bem vindo " + userCommand.SocketInstance.Nickname + ". Utilize help para ver os comandos disponíveis.");
            }

            return userCommand;
        }


        public override Dictionary<string, SocketInstance> GetInterpreterEnumerable() {
            return MainSocket.GetInstance().SocketInstancesByNickname;
        }

    }
}