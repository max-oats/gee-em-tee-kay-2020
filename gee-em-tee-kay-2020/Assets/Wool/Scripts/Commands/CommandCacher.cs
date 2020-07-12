using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Wool
{
    public class CommandCacher : MonoBehaviour
    {
        /** Cached dictionary of methods for this object */
        [SerializeField]
        private List<CommandCache> caches = new List<CommandCache>();

        void Awake()
        {
            GrabCaches();
        }

        public void GrabCaches()
        {
            caches = new List<CommandCache>(Utils.GrabCommandCachesFromObject(gameObject));
        }

        /** Checks whether the given method can be found within this cacher */
        public bool CheckMethod(string cmd)
        {
            List<CommandCache> potentials = caches.FindAll(x => x.commandName == cmd);
            if (potentials.Count == 0)
            {
                /** If no potentials? We don't run it */
                return false;
            }

            return true;
        }

        public bool RunMethod(string cmd, List<object> parameters, System.Action onComplete)
        {
            bool requireWait = false;
            List<CommandCache> potentials = caches.FindAll(x => x.commandName == cmd);
            if (potentials.Count == 0)
            {
                //Sockbug.LogWarning(Logs.Commands, "<b>{0}</b> failed to run command <i>{1}</i>: Command name not found", name, cmd);
                return false;
            }

            bool wasFound = false;
            foreach (CommandCache potential in potentials)
            {
                List<object> finalParams = new List<object>(parameters);
                if (potential.wait)
                {
                    finalParams.Add(onComplete);  
                }
                var methodParameters = potential.method.GetParameters();
                // Check if this is a params array
                if (methodParameters.Length == 1 && methodParameters[0].ParameterType.IsAssignableFrom(typeof(string[])))
                {
                    // Cool, we can send the command!
                    string[][] paramWrapper = new string[1][];
                    List<string> paramwrapps = new List<string>();
                    foreach (object obj in finalParams)
                    {
                        paramwrapps.Add((string)obj);
                    }
                    paramWrapper[0] = paramwrapps.ToArray();

                    object returned = potential.method.Invoke(potential.component, paramWrapper);

                    if (returned != null && returned.GetType().IsAssignableFrom(typeof(bool)))
                    {
                        requireWait = (bool)returned;
                    }
                    wasFound = true;
                    break;
                }
                // Otherwise, verify that this method has the right number of parameters
                else if (methodParameters.Length == finalParams.Count)
                {
                    bool paramsMatch = true;
                    for (int i = 0; i < methodParameters.Length; ++i)
                    {
                        if (!methodParameters[i].ParameterType.IsAssignableFrom(finalParams[i].GetType()))
                        {
                            paramsMatch = false;
                            break;
                        }
                    }

                    if (paramsMatch)
                    {
                        // Cool, we can send the command!
                        object returned = potential.method.Invoke(potential.component, finalParams.ToArray());
                        if (returned != null && returned.GetType().IsAssignableFrom(typeof(bool)))
                        {
                            requireWait = (bool)returned;
                        }
                        wasFound = true;
                        break;
                    }
                }
            }

            if (!wasFound)
            {
                //Sockbug.LogWarning(Logs.Commands, "<b>{0}</b> failed to run command <i>{1}</i>: Command name not found", name, cmd);
                return false;
            }

            //Sockbug.Log(Logs.Commands, "<b>{0}</b> successfully ran command <i>{1}</i>.", name, cmd);
            return requireWait;
        }

        public List<CommandCache> GetCaches()
        {
            return caches;
        }
    }
}