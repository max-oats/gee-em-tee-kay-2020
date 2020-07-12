namespace Wool
{
    /** Wool.Command attribute */
    [System.AttributeUsage(System.AttributeTargets.Method)]
    public class CommandAttribute : System.Attribute
    {
        public CommandAttribute(params string[] commandString) 
        {
            this.commandStrings = commandString;
        }

        /** Required parameter: The command string/strings to call the command. */
        public string[] commandStrings { get; private set; }

        /** Optional parameter: Tells the runner whether it should pause command execution or not */
        public bool wait = false;
    }
}