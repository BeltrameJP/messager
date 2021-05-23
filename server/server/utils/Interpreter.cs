using System;
using System.Collections;
using System.Diagnostics.Contracts;
using System.Text.RegularExpressions;

namespace server.utils {

    public interface IInterpreter {

        public static readonly Regex REGEX_FOR_TWO_ARGUMENTS = new Regex(@"(?<command>\w+?)\s(?<complement>\w+?)\s(?<message>.+)");
        public static readonly Regex REGEX_FOR_SINGLE_ARGUMENT = new Regex(@"(?<command>\w+?)\s(?<complement>.+)");

        public UserCommand Read(UserCommand userCommand);

        public UserCommand Action(UserCommand userCommand);

        public UserCommand Echo(UserCommand userCommand);

        public UserCommand Run(UserCommand userCommand);

        public string GetCommand();

        public Boolean IsPrivate();

        public string GetDescription();

    }

    public abstract class Interpreter<T> : IInterpreter where T : IEnumerable {

        public string Command { get; protected set; }

        public string Description { get; protected set; }

        public bool ExternUse { get; protected set; }

        public abstract T GetInterpreterEnumerable();

        public string GetCommand() {
            return Command;
        }

        public string GetDescription() {
            return Description;
        }

        public bool IsPrivate() {
            return !ExternUse;
        }
        public abstract UserCommand Read(UserCommand userCommand);

        public abstract UserCommand Action(UserCommand userCommand);

        public abstract UserCommand Echo(UserCommand userCommand);

        public UserCommand Run(UserCommand userCommand) {
            return this.Echo(this.Action(this.Read(userCommand)));
        }

    }
}