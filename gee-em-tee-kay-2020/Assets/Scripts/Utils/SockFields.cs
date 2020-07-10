namespace Socks
{
    public class FieldAttribute : System.Attribute
    {
        public FieldAttribute()
        {
            name = "";
            category = "";
        }

        public string name;
        public string category = "";
        public string dependOn = "";
        public bool readOnly = false;
    }

    public class OptionAttribute : System.Attribute
    {
        public OptionAttribute(string optionName)
        {
            name = optionName;
        }

        public string name;
    }
}