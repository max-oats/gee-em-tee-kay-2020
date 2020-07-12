
[System.AttributeUsage(System.AttributeTargets.Class)]
public class DialogueTagAttribute : System.Attribute
{
    public DialogueTagAttribute(params string[] tag)
    {
        this.tag = tag;
    }

    public string[] tag;
}