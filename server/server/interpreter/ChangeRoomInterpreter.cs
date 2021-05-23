using System.Collections.Generic;
using System.Text.RegularExpressions;
using server.socket;
using server.utils;

namespace server.interpreter {
    public class ChangeRoomInterpreter : Interpreter<Dictionary<string, string>> {

        private Dictionary<string, string> RoomByNickName { get; set; }

        public ChangeRoomInterpreter() {
            RoomByNickName = new Dictionary<string, string>();

            this.Command = "changeroom";
            this.ExternUse = true;
            this.Description = "Use " + this.Command + " <nome_da_sala> para conversar no seu chat atual.";
        }

        public override UserCommand Read(UserCommand userCommand) {
            GroupCollection groupCollection = IInterpreter.REGEX_FOR_SINGLE_ARGUMENT.Match(userCommand.InputMessage).Groups;

            userCommand.Partials.Add(groupCollection[1].Value);
            userCommand.Partials.Add(groupCollection[2].Value);

            return userCommand;
        }

        public override UserCommand Action(UserCommand userCommand) {
            if (RoomByNickName.ContainsKey(userCommand.SocketInstance.Nickname)) {
                ((Interpreter<Dictionary<string, Dictionary<string, SocketInstance>>>)MainSocket.GetInstance()
                    .InterpretersByCommand["room"]).GetInterpreterEnumerable()[userCommand.SocketInstance.Nickname].Remove(userCommand.SocketInstance.Nickname);
            }

            RoomByNickName[userCommand.SocketInstance.Nickname] = userCommand.Partials[1];

            ((Interpreter<Dictionary<string, Dictionary<string, SocketInstance>>>)MainSocket.GetInstance()
                    .InterpretersByCommand["room"]).GetInterpreterEnumerable()[userCommand.Partials[1]].Add(userCommand.SocketInstance.Nickname, userCommand.SocketInstance);

            return userCommand;
        }

        public override UserCommand Echo(UserCommand userCommand) {
            Messager.SendMessage(userCommand.SocketInstance, "Você foi transferido para o canal " + userCommand.Partials[1]
                                + ". Para falar, só utilizar room <message>");
            return userCommand;
        }

        public override Dictionary<string, string> GetInterpreterEnumerable() {
            return RoomByNickName;
        }
    }
}