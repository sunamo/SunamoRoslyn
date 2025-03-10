namespace SunamoRoslyn._sunamo;

internal class FS
{
    /// <summary>
    ///     Usage: Exceptions.FileWasntFoundInDirectory
    /// </summary>
    /// <param name="v"></param>
    /// <returns></returns>
    internal static string WithEndSlash(ref string v)
    {
        if (v != string.Empty)
        {
            v = v.TrimEnd('\\') + '\\';
        }
        SH.FirstCharUpper(ref v);
        return v;
    }
}