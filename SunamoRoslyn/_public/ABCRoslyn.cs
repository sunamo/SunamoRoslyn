namespace SunamoRoslyn._public;

public class ABCRoslyn : List<ABRoslyn>
{
    public static ABCRoslyn Empty = new();

    public ABCRoslyn()
    {
    }

    public ABCRoslyn(int capacity) : base(capacity)
    {
        for (var i = 0; i < capacity; i++) Add(null!);
    }

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

    public ABCRoslyn(params ABRoslyn[] collection)
    {
        AddRange(collection);
    }

    public int Length => Count;

    public override string ToString()
    {
        var stringBuilder = new StringBuilder();
        foreach (var item in this) stringBuilder.Append(item + ",");
        return stringBuilder.ToString();
    }

    // Must be array due to SQL, see https://stackoverflow.com/questions/9149919/no-mapping-exists-from-object-type-system-collections-generic-list-when-executin
    public object[] OnlyBs()
    {
        return OnlyBsList().ToArray();
    }

    public List<object> OnlyBsList()
    {
        var result = new List<object>(Count);
        for (var i = 0; i < Count; i++) result.Add(this[i].B);
        return result;
    }

    public List<string> OnlyAs()
    {
        var result = new List<string>(Count);
        for (var i = 0; i < Count; i++) result[i] = this[i].A;
        return result;
    }

    public static List<object> OnlyBs(List<ABRoslyn> list)
    {
        return list.Select(element => element.B).ToList();
    }
}
