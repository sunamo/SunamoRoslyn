namespace SunamoRoslyn;

public class RoslynParserText
{
    private static
        async Task AddPageMethodsAsync
        (StringBuilder stringBuilder, List<string> files)
    {
        SourceCodeIndexerRoslyn indexer = SourceCodeIndexerRoslyn.Instance;
        foreach (var file in files)
        {
            await
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

    public
        async Task FindPageMethodAsync
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
                await AddPageMethodsAsync
                        (stringBuilder, files);
            }
        }
        foreach (var item in projectNames)
        {
            string projectPath = Path.Combine(rootPath, item);
            var pageFiles = Directory.GetFiles(projectPath, "*Page*.cs", SearchOption.TopDirectoryOnly).ToList();
            await AddPageMethodsAsync
                (stringBuilder, pageFiles);
        }
    }
}
