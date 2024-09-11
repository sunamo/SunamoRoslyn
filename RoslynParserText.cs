namespace SunamoRoslyn;

/// <summary>
///     RoslynParser - use roslyn classes
///     RoslynParserText - no use roslyn classes, only text or indexer
/// </summary>
public class RoslynParserText
{
    private static
#if ASYNC
        async Task AddPageMethodsAsync
#else
void AddPageMethods
#endif
        (StringBuilder sb, List<string> files)
    {
        var Instance = SourceCodeIndexerRoslyn.Instance;

        foreach (var file in files)
        {
#if ASYNC
            await
#endif
                Instance.ProcessFile(file, NamespaceCodeElementsType.Nope, ClassCodeElementsType.Method, false, false);
        }

        foreach (var file2 in Instance.classCodeElements)
        {
            sb.AppendLine(file2.Key);
            foreach (var method in file2.Value)
                if (method.Name.StartsWith("On") || method.Name.StartsWith(sess.i18n(XlfKeys.Page) + "_"))
                    sb.AppendLine(method.Name);
        }
    }

    public
#if ASYNC
        async Task FindPageMethodAsync
#else
void FindPageMethod
#endif
        (string sczRootPath)
    {
        var sb = new StringBuilder();

        var project = new List<string>();

        var folders = Directory.GetDirectories(sczRootPath, "*", SearchOption.TopDirectoryOnly);
        foreach (var item in folders)
        {
            var nameProject = Path.GetFileName(item);
            if (nameProject.EndsWith("X"))
            {
                var project2 = nameProject.Substring(0, nameProject.Length - 1);
                // General files is in Nope. GeneralX is only for pages in General folder
                if (project2 != XlfKeys.General) project.Add(project2);

                var files = Directory
                    .GetFiles(item, FS.MascFromExtension(AllExtensions.cs), SearchOption.TopDirectoryOnly).ToList();
#if ASYNC
                await AddPageMethodsAsync
#else
AddPageMethods
#endif
                    (sb, files);
            }
        }

        foreach (var item in project)
        {
            var path = Path.Combine(sczRootPath, item);
            var pages = Directory.GetFiles(path, "*Page*.cs", SearchOption.TopDirectoryOnly).ToList();
#if ASYNC
            await AddPageMethodsAsync
#else
AddPageMethods
#endif

                (sb, pages);
        }

        ClipboardHelper.SetText(sb);
    }
}