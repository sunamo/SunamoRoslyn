namespace SunamoRoslyn._sunamo;

internal class FS
{
    internal static string WithEndSlash(ref string path)
    {
        if (path != string.Empty)
        {
            path = path.TrimEnd('\\') + '\\';
        }
        SH.FirstCharUpper(ref path);
        return path;
    }
}
