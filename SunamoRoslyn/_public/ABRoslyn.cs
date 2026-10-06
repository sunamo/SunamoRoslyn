namespace SunamoRoslyn._public;

public class ABRoslyn
{
    internal static Type TypeInstance { get; set; } = typeof(ABRoslyn);

    public string A { get; set; }

    public object B { get; set; }

    public ABRoslyn(string name, object value)
    {
        A = name;
        B = value;
    }

    public static ABRoslyn Get(Type name, object value)
    {
        return new ABRoslyn(name.FullName!, value);
    }

    public static ABRoslyn Get(string name, object value)
    {
        return new ABRoslyn(name, value);
    }

    public override string ToString()
    {
        return $"{A}:{B}";
    }
}
