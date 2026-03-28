namespace SunamoRoslyn._public;

/// <summary>
/// Represents a name-value pair for Roslyn code analysis.
/// </summary>
public class ABRoslyn
{
    internal static Type TypeInstance { get; set; } = typeof(ABRoslyn);

    /// <summary>
    /// The name component of the pair.
    /// </summary>
    public string A { get; set; }

    /// <summary>
    /// The value component of the pair.
    /// </summary>
    public object B { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="ABRoslyn"/> class.
    /// </summary>
    /// <param name="name">The name component.</param>
    /// <param name="value">The value component.</param>
    public ABRoslyn(string name, object value)
    {
        A = name;
        B = value;
    }

    /// <summary>
    /// Creates a new <see cref="ABRoslyn"/> instance from a type and value.
    /// </summary>
    /// <param name="name">The type whose full name will be used as the name component.</param>
    /// <param name="value">The value component.</param>
    /// <returns>A new <see cref="ABRoslyn"/> instance.</returns>
    public static ABRoslyn Get(Type name, object value)
    {
        return new ABRoslyn(name.FullName!, value);
    }

    /// <summary>
    /// Creates a new <see cref="ABRoslyn"/> instance from a string name and value.
    /// </summary>
    /// <param name="name">The name component.</param>
    /// <param name="value">The value component.</param>
    /// <returns>A new <see cref="ABRoslyn"/> instance.</returns>
    public static ABRoslyn Get(string name, object value)
    {
        return new ABRoslyn(name, value);
    }

    /// <summary>
    /// Returns a string representation of this name-value pair.
    /// </summary>
    /// <returns>A string in the format "Name:Value".</returns>
    public override string ToString()
    {
        return A + ":" + B;
    }
}
