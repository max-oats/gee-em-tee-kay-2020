using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Wool
{
    public class CommandCache
    {
        public CommandCache(MonoBehaviour component, string cmd, MethodInfo method, bool wait)
        {
            this.component = component;
            this.commandName = cmd;
            this.method = method;
            this.wait = wait;
        }

        public MonoBehaviour component;

        public string commandName;

        public MethodInfo method;

        public bool wait;
    }
}