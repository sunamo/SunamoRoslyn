namespace SunamoRoslyn._public;

/// <summary>
/// Represents a collection of <see cref="ABRoslyn"/> name-value pairs.
/// </summary>
public class ABCRoslyn : List<ABRoslyn>
{
    /// <summary>
    /// An empty collection instance.
    /// </summary>
    public static ABCRoslyn Empty = new();

    /// <summary>
    /// Initializes a new empty instance of the <see cref="ABCRoslyn"/> class.
    /// </summary>
    public ABCRoslyn()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ABCRoslyn"/> class with specified capacity, filled with nulls.
    /// </summary>
    /// <param name="capacity">The number of null elements to add.</param>
    public ABCRoslyn(int capacity) : base(capacity)
    {
        for (var i = 0; i < capacity; i++) Add(null!);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ABCRoslyn"/> class from interleaved name-value pairs or existing collections.
    /// </summary>
    /// <param name="setsNameValue">The name-value pairs or collections to add.</param>
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
    /// Initializes a new instance of the <see cref="ABCRoslyn"/> class from an array of <see cref="ABRoslyn"/> elements.
    /// </summary>
    /// <param name="collection">The elements to add.</param>
    public ABCRoslyn(params ABRoslyn[] collection)
    {
        AddRange(collection);
    }

    /// <summary>
    /// Gets the number of elements in this collection.
    /// </summary>
    public int Length => Count;

    /// <summary>
    /// Returns a comma-separated string representation of all elements.
    /// </summary>
    /// <returns>A comma-separated string of all elements.</returns>
    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in this) stringBuilder.Append(item + ",");
        return stringBuilder.ToString();
    }

    /// <summary>
    /// Returns only the value components as an object array.
    /// Must be array due to SQL, see https://stackoverflow.com/questions/9149919/no-mapping-exists-from-object-type-system-collections-generic-list-when-executin
    /// </summary>
    /// <returns>An array of value components.</returns>
    public object[] OnlyBs()
    {
        return OnlyBsList().ToArray();
    }

    /// <summary>
    /// Returns only the value components as a list.
    /// </summary>
    /// <returns>A list of value components.</returns>
    public List<object> OnlyBsList()
    {
        var result = new List<object>(Count);
        for (var i = 0; i < Count; i++) result.Add(this[i].B);
        return result;
    }

    /// <summary>
    /// Returns only the name components as a list.
    /// </summary>
    /// <returns>A list of name components.</returns>
    public List<string> OnlyAs()
    {
        var result = new List<string>(Count);
        for (var i = 0; i < Count; i++) result[i] = this[i].A;
        return result;
    }

    /// <summary>
    /// Returns only the value components from a given list.
    /// </summary>
    /// <param name="list">The list of <see cref="ABRoslyn"/> elements.</param>
    /// <returns>A list of value components.</returns>
    public static List<object> OnlyBs(List<ABRoslyn> list)
    {
        return list.Select(element => element.B).ToList();
    }
}
