using System.Collections.Generic;
using System.Text.RegularExpressions;
using server.socket;
using server.utils;

namespace server.interpreter {
    public class CreateRoom : Interpreter<Dictionary<string, List<SocketInstance>>> {

        public CreateRoom() {
            this.Command = "createroom";
            this.ExternUse = true;
            this.Description = "Use " + this.Command + " <nome_da_sala> para criar uma nova sala.";
        }

        public override UserCommand Read(UserCommand userCommand) {
            GroupCollection groupCollection = IInterpreter.REGEX_FOR_SINGLE_ARGUMENT.Match(userCommand.InputMessage).Groups;

            userCommand.Partials.Add(groupCollection[1].Value);
            userCommand.Partials.Add(groupCollection[2].Value);

            return userCommand;
        }

        public override UserCommand Action(UserCommand userCommand) {
            Dictionary<string, Dictionary<string, SocketInstance>> roomByNickName =
                    ((Interpreter<Dictionary<string, Dictionary<string, SocketInstance>>>)MainSocket.GetInstance().InterpretersByCommand["room"]).GetInterpreterEnumerable();

            roomByNickName[userCommand.Partials[1]] = new Dictionary<string, SocketInstance>();

            return userCommand;
        }

        public override UserCommand Echo(UserCommand userCommand) {
            Messager.SendMessage(userCommand.SocketInstance, "Sala " + userCommand.Partials[1]
                            + " criada. Para transferir-se, basta digitar 'changeroom " + userCommand.Partials[1] + "'");

            return userCommand;
        }

        public override Dictionary<string, List<SocketInstance>> GetInterpreterEnumerable() {
            throw new System.NotImplementedException();
        }

    }
}