namespace SunamoRoslyn._public;

/// <summary>
/// Represents the ABC roslyn type.
/// </summary>
public class ABCRoslyn : List<ABRoslyn>
{
    /// <summary>
    /// The empty.
    /// </summary>
    public static ABCRoslyn Empty = new();

    /// <summary>
    /// ABC roslyn.
    /// </summary>
    /// <returns>The ABC roslyn result.</returns>
    public ABCRoslyn()
    {
    }

    /// <summary>
    /// ABC roslyn.
    /// </summary>
    /// <param name="capacity">The capacity.</param>
    /// <returns>The ABC roslyn result.</returns>
    public ABCRoslyn(int capacity) : base(capacity)
    {
        for (var i = 0; i < capacity; i++) Add(null!);
    }

    /// <summary>
    /// Initializes a new instance of the ABC roslyn class.
    /// </summary>
    /// <param name="i">The i.</param>
    public ABCRoslyn(params object[] setsNameValue)
    {
        if (setsNameValue.Length == 0) return;
        var firstElement = setsNameValue[0];
        var elementType = firstElement.GetType();
        var innerType = elementType;
        if (firstElement is IList)
        {
            var listElements = (IList)firstElement;
            var firstListElement = listElements.Count != 0 ? listElements[0] : null;
            innerType = firstListElement!.GetType();
        }

        if (innerType == typeof(ABRoslyn))
        {
            for (var i = 0; i < setsNameValue.Length; i++)
            {
                var currentElement = setsNameValue[i];
                innerType = currentElement.GetType();
                if (innerType == ABRoslyn.TypeInstance)
                {
                    Add((ABRoslyn)currentElement);
                }
                else
                {
                    var listValue = (IList)currentElement;
                    foreach (var item in listValue)
                    {
                        var roslynAb = (ABRoslyn)item;
                        Add(roslynAb);
                    }
                }
            }
        }
        else if (elementType == typeof(ABCRoslyn))
        {
            var collection = (ABCRoslyn)firstElement;
            AddRange(collection);
        }
        else
        {
            for (var i = 0; i < setsNameValue.Length; i++) Add(ABRoslyn.Get(setsNameValue[i].ToString()!, setsNameValue[++i]));
        }
    }

    /// <summary>
    /// ABC roslyn.
    /// </summary>
    /// <param name="collection">The collection.</param>
    /// <returns>The ABC roslyn result.</returns>
    public ABCRoslyn(params ABRoslyn[] collection)
    {
        AddRange(collection);
    }

    /// <summary>
    /// Gets the length.
    /// </summary>
    public int Length => Count;

    /// <summary>
    /// To string.
    /// </summary>
    /// <returns>The to string result.</returns>
    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in this) stringBuilder.Append(item + ",");
        return stringBuilder.ToString();
    }

    // Must be array due to SQL, see https://stackoverflow.com/questions/9149919/no-mapping-exists-from-object-type-system-collections-generic-list-when-executin
    /// <summary>
    /// Only bs.
    /// </summary>
    /// <returns>The only bs result.</returns>
    public object[] OnlyBs()
    {
        return OnlyBsList().ToArray();
    }

    /// <summary>
    /// Only bs list.
    /// </summary>
    /// <returns>The only bs list result.</returns>
    public List<object> OnlyBsList()
    {
        var result = new List<object>(Count);
        for (var i = 0; i < Count; i++) result.Add(this[i].B);
        return result;
    }

    /// <summary>
    /// Only as.
    /// </summary>
    /// <typeparam name="object">The type object.</typeparam>
    /// <param name="Count">The count.</param>
    /// <returns>The only as result.</returns>
    public List<string> OnlyAs()
    {
        var result = new List<string>(Count);
        for (var i = 0; i < Count; i++) result[i] = this[i].A;
        return result;
    }

    /// <summary>
    /// Only bs.
    /// </summary>
    /// <typeparam name="string">The type string.</typeparam>
    /// <param name="Count">The count.</param>
    /// <returns>The only bs result.</returns>
    public static List<object> OnlyBs(List<ABRoslyn> list)
    {
        return list.Select(element => element.B).ToList();
    }
}
