namespace SunamoRoslyn;

/// <summary>
/// Parses C# source code using text and indexer-based approaches without Roslyn syntax classes.
/// RoslynParser handles parsing with Roslyn classes.
/// </summary>
public class RoslynParserText
{
    /// <summary>
    /// Adds page event handler methods from the given files to the string builder.
    /// </summary>
    /// <param name="stringBuilder">The string builder to append method names to.</param>
    /// <param name="files">The list of C# file paths to process.</param>
    private static
#if ASYNC
        async Task AddPageMethodsAsync
#else
void AddPageMethods
#endif
        (StringBuilder stringBuilder, List<string> files)
    {
        SourceCodeIndexerRoslyn indexer = SourceCodeIndexerRoslyn.Instance;
        foreach (var file in files)
        {
#if ASYNC
            await
#endif
                indexer.ProcessFile(file, NamespaceCodeElementsType.Nope, ClassCodeElementsType.Method, false, false);
        }
        foreach (var fileEntry in indexer.classCodeElements)
        {
            stringBuilder.AppendLine(fileEntry.Key);
            foreach (var method in fileEntry.Value)
            {
                if (method.Name.StartsWith("On") || method.Name.StartsWith("Page" + "_"))
                {
                    stringBuilder.AppendLine(method.Name);
                }
            }
        }
    }

    /// <summary>
    /// Finds page event handler methods in all project folders under the given root path.
    /// </summary>
    /// <param name="rootPath">The root path containing project folders to scan.</param>
    public
#if ASYNC
        async Task FindPageMethodAsync
#else
void FindPageMethod
#endif
        (string rootPath)
    {
        StringBuilder stringBuilder = new StringBuilder();
        List<string> projectNames = new List<string>();
        var folders = Directory.GetDirectories(rootPath, "*", SearchOption.TopDirectoryOnly);
        foreach (var item in folders)
        {
            string projectName = Path.GetFileName(item);
            if (projectName.EndsWith("X"))
            {
                string projectNameWithoutSuffix = projectName.Substring(0, projectName.Length - 1);
                if (projectNameWithoutSuffix != "General")
                {
                    projectNames.Add(projectNameWithoutSuffix);
                }
                var files = Directory.GetFiles(item, "*.cs", SearchOption.TopDirectoryOnly).ToList();
#if ASYNC
                await AddPageMethodsAsync
#else
AddPageMethods
#endif
                        (stringBuilder, files);
            }
        }
        foreach (var item in projectNames)
        {
            string projectPath = Path.Combine(rootPath, item);
            var pageFiles = Directory.GetFiles(projectPath, "*Page*.cs", SearchOption.TopDirectoryOnly).ToList();
#if ASYNC
            await AddPageMethodsAsync
#else
AddPageMethods
#endif
                (stringBuilder, pageFiles);
        }
    }
}
