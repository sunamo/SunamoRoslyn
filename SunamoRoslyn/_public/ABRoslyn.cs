namespace SunamoRoslyn._public;

/// <summary>
/// Represents the AB roslyn type.
/// </summary>
public class ABRoslyn
{
    internal static Type TypeInstance { get; set; } = typeof(ABRoslyn);

    /// <summary>
    /// Gets or sets the a.
    /// </summary>
    public string A { get; set; }

    /// <summary>
    /// Gets or sets the b.
    /// </summary>
    public object B { get; set; }

    /// <summary>
    /// AB roslyn.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="value">The value.</param>
    /// <returns>The AB roslyn result.</returns>
    public ABRoslyn(string name, object value)
    {
        A = name;
        B = value;
    }

    /// <summary>
    /// Initializes a new instance of the get class.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="value">The value.</param>
    public static ABRoslyn Get(Type name, object value)
    {
        return new ABRoslyn(name.FullName!, value);
    }

    /// <summary>
    /// Get.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="value">The value.</param>
    /// <returns>The get result.</returns>
    public static ABRoslyn Get(string name, object value)
    {
        return new ABRoslyn(name, value);
    }

    /// <summary>
    /// To string.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="value">The value.</param>
    /// <returns>The to string result.</returns>
    public override string ToString()
    {
        return $"{A}:{B}";
    }
}
