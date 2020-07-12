using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Wool
{
    public class CommandManager : MonoBehaviour
    {
        /** List of the currently running command runners */
        protected List<CommandRunner> runners = new List<CommandRunner>();

        /** Cache of method infos from CommandRunner */
        protected List<MethodInfo> defaultMethods = new List<MethodInfo>();

        public bool RunCommand(MonoBehaviour owner, string command, System.Action onComplete)
        {
            // CommandRunner runner = new CommandRunner(owner, command, onComplete);
            // runner.onFinished += OnFinished;
            // runners.Add(runner);
            // return runner.StartCommand();

            return true;
        }

        public bool RunCommand(MonoBehaviour owner, string[] commands, System.Action onComplete)
        {
            // CommandRunner runner = new CommandRunner(owner, commands, onComplete);
            // runner.onFinished += OnFinished;
            // runners.Add(runner);
            // return runner.StartCommand();
            
            return true;
        }

        public void OnFinished(CommandRunner runner)
        {
            StartCoroutine(RemoveWithDelay(runner));
        }

        public IEnumerator RemoveWithDelay(CommandRunner runner)
        {
            yield return new WaitForSeconds(1.0f);

            runners.Remove(runner);
        }

        /** Basic wait command */
        [Wool.Command("wait", wait=true)]
        public bool Wait(float waitTime, System.Action onComplete)
        {
            if (waitTime == 0f)
            {
                return false;
            }
            else
            {
                StartCoroutine(WaitCoroutine(waitTime, onComplete));
                return true;
            }
        }

        /** Basic wait command */
        [Wool.Command("wait", wait=true)]
        public bool Wait(System.Action onComplete)
        {
            StartCoroutine(WaitCoroutine(0.3f, onComplete));
            return true;
        }

        IEnumerator WaitCoroutine(float time, System.Action onComplete)
        {
            for (float i = 0; i < time; i += Time.deltaTime)
                yield return null;

            onComplete();
        }
    }
}