using System.Collections.Generic;
using System.Text.RegularExpressions;
using server.socket;
using server.utils;

namespace server.interpreter {
    public class ChatRoomInterpreter : Interpreter<Dictionary<string, Dictionary<string, SocketInstance>>> {

        private Dictionary<string, Dictionary<string, SocketInstance>> SocketInstancesByRoom;

        public ChatRoomInterpreter() {
            SocketInstancesByRoom = new Dictionary<string, Dictionary<string, SocketInstance>>();

            this.Command = "room";
            this.ExternUse = true;
            this.Description = "Use " + this.Command + " <mensagem> para conversar no seu chat atual.";
        }

        public override UserCommand Read(UserCommand userCommand) {
            GroupCollection groupCollection = IInterpreter.REGEX_FOR_SINGLE_ARGUMENT.Match(userCommand.InputMessage).Groups;

            userCommand.Partials.Add(groupCollection[1].Value);
            userCommand.Partials.Add(groupCollection[2].Value);

            return userCommand;
        }
        public override UserCommand Action(UserCommand userCommand) {
            Dictionary<string, string> roomByNickName =
                    ((Interpreter<Dictionary<string, string>>)MainSocket.GetInstance().InterpretersByCommand["changeroom"]).GetInterpreterEnumerable();

            Dictionary<string, SocketInstance> socketInstances = SocketInstancesByRoom[roomByNickName[userCommand.SocketInstance.Nickname]];

            foreach (var (key, socketInstance) in socketInstances) {
                Messager.SendMessage(socketInstance, "[" + roomByNickName[userCommand.SocketInstance.Nickname] + "]" + userCommand.SocketInstance.Nickname
                                        + ": " + userCommand.Partials[1]);
            }

            return userCommand;
        }

        public override UserCommand Echo(UserCommand userCommand) {
            return userCommand;
        }

        public override Dictionary<string, Dictionary<string, SocketInstance>> GetInterpreterEnumerable() {
            return SocketInstancesByRoom;
        }

    }
}