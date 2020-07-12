using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace Wool
{
    public class CommandRunner
    {
        // public delegate void OnFinished(CommandRunner runner);
        // public OnFinished onFinished;

        // private System.Action onComplete;
        // private MonoBehaviour owner;
        // private List<string> commands = new List<string>();
        // private int idx = 0;

        // public CommandRunner(MonoBehaviour owner, string command, System.Action onComplete)
        // {
        //     this.owner = owner;
        //     this.onComplete = onComplete;

        //     ParseIntoCommands(command);
        // }

        // public CommandRunner(MonoBehaviour owner, string[] commands, System.Action onComplete)
        // {
        //     this.owner = owner;

        //     this.onComplete = onComplete;
        //     ParseIntoCommands(commands);
        // }

        // void ParseIntoCommands(string fullString)
        // {   
        //     // Split into lines
        //     var lines = fullString.Split('\n');

        //     ParseIntoCommands(lines);
        // }

        // void ParseIntoCommands(string[] cmds)
        // {
        //     foreach (string line in cmds)
        //     {
        //         GetCommandFromLine(owner.gameObject.name, line);
        //     }
        // }

        // public bool StartCommand()
        // {
        //     while (idx < commands.Count)
        //     {
        //         bool pause = Dispatch(commands[idx]);

        //         idx++;

        //         if (pause)
        //         {
        //             return false;
        //         }
        //     }

        //     onComplete?.Invoke();
        //     onFinished?.Invoke(this);
        //     return true;
        // }

        // public void GoToNext()
        // {
        //     while (idx < commands.Count)
        //     {
        //         bool pause = Dispatch(commands[idx]);

        //         idx++;

        //         if (pause)
        //         {
        //             return;
        //         }
        //     }

        //     onComplete?.Invoke();
        //     onFinished?.Invoke(this);
        // }

        // void GetCommandFromLine(string ownerName, string line)
        // {
        //     // command string
        //     string command = line.Trim();
        //     if (command.Length == 0)
        //     {
        //         return;
        //     }

        //     // Split into words
        //     var words = command.Split(' ');

        //     // Switch "me"'s to actual mono name
        //     if (words[0] == "me")
        //     {
        //         words[0] = ownerName;
        //     }

        //     if (words.Length > 1)
        //     {
        //         if (words[1][0] == '#')
        //         {
        //             //string[] lines = Game.commands.GetShortcut(words[1]);
                
        //             if (lines != null)
        //             {
        //                 foreach (string shortcutLine in lines)
        //                 {
        //                     GetCommandFromLine(words[0], shortcutLine);
        //                 }
        //             }

        //             return;
        //         }
        //     }

        //     string finalString = "";
        //     foreach (string word in words)
        //     {
        //         string finalWord = word;
        //         if (word[0] == '@')
        //         {
        //             //finalWord = Game.flags.GetFlag(word.Substring(1));
        //         }

        //         finalString += finalWord + " ";
        //     }
        //     commands.Add(finalString.Trim());
        // }

        // bool Dispatch(string command)
        // {
        //     // Trim command
        //     command = command.Trim();
        //     var words = command.Split(' ');

        //     if (words.Length == 0)
        //     {
        //         //Sockbug.LogWarning(Logs.Commands, "Attempted to perform command '{0}' but failed: splitting the string returned no words.", command);
        //         return true;
        //     }

        //     string commandName;
        //     commandName = words[0].ToLower();

        //     List<object> parameters = new List<object>();
        //     for (int i = 1; i < words.Length; ++i)
        //     {
        //         float output = -1f;
        //         if (float.TryParse(words[i], out output))
        //         {
        //             parameters.Add(output);
        //         }
        //         else
        //         {
        //             parameters.Add(words[i]);
        //         }
        //     }

        //     /** First- test any global commands */
        //     //GameObject sceneObject = Game.commands.gameObject;
        //     /** Grab the cacher from the object */
        //     //CommandCacher cacher = sceneObject.GetComponent<CommandCacher>();
        //     //if (cacher == null)
        //     {
        //         /** Add/run cacher if null */
        //         //cacher = sceneObject.AddComponent<CommandCacher>();
        //     }

        //     //if (cacher.CheckMethod(commandName))
        //     {
        //         //return cacher.RunMethod(commandName, parameters, GoToNext);
        //     }
                
        //     // need 2 parameters in order to have both a command name
        //     // and the name of an object to find
        //     if (words.Length < 2)
        //     {
        //         //Sockbug.LogWarning(Logs.Commands, "COMMAND: not enough parameters to perform command: " + command);
        //         return true;
        //     }

        //     /** update command and object names */
        //     var objectName = words[0];
        //     commandName = words[1];

        //     /** If command = me, set to caller of runcommand */
        //     if (owner != null && objectName.ToLower() == "me")
        //     {
        //         objectName = owner.gameObject.name;
        //     }

        //     parameters = new List<object>();
        //     for (int i = 2; i < words.Length; ++i)
        //     {
        //         float output = -1f;
        //         if (float.TryParse(words[i], out output))
        //         {
        //             parameters.Add(output);
        //         }
        //         else
        //         {
        //             parameters.Add(words[i]);
        //         }
        //     }

        //     /** Find associated game object */
        //     sceneObject = Game.entities.FindOrCreateAndSpawn(objectName);
        //     if (sceneObject == null)
        //     {
        //         sceneObject = GameObject.Find(objectName);

        //         if (sceneObject == null)
        //         {
        //             Debug.LogErrorFormat("Object '{0}' attempted to run command '{1}' but failed: SceneObject not found.", objectName, commandName);
        //             return true;
        //         }
        //     }

        //     /** Grab the cacher from the object */
        //     cacher = sceneObject.GetComponent<CommandCacher>();
        //     if (cacher == null)
        //     {
        //         /** Add/run cacher if null */
        //         cacher = sceneObject.AddComponent<CommandCacher>();
        //     }

        //     return cacher.RunMethod(commandName, parameters, GoToNext);
        // }
    }
}