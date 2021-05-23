using System;
using System.Collections.Generic;
using System.Linq;
using server.socket;

namespace server.utils {
    public static class InterpreterScanner {
        public static void Scan() {
            MainSocket mainSocket = MainSocket.GetInstance();

            Type type = typeof(IInterpreter);
            IEnumerable<Type> types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p));

            foreach (Type t in types) {
                if (t.IsClass && !t.IsAbstract) {
                    var obj = (IInterpreter)Activator.CreateInstance(t);

                    if (obj.IsPrivate()) {
                        mainSocket.PrivateInterpretersByCommand.Add(obj.GetCommand(), obj);
                    } else {
                        mainSocket.InterpretersByCommand.Add(obj.GetCommand(), obj);
                    }

                    Console.WriteLine("Interpretador \"" + obj.GetCommand() + "\" iniciado");
                }
            }
        }
    }
}