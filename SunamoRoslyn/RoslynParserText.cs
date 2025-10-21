// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy

namespace SunamoRoslyn;
/// <summary>
/// RoslynParser - use roslyn classes
/// RoslynParserText - no use roslyn classes, only text or indexer
/// </summary>
public class RoslynParserText
{


    private static
#if ASYNC
        async Task AddPageMethodsAsync
#else
void AddPageMethods
#endif
        (StringBuilder stringBuilder, List<string> files)
    {
        SourceCodeIndexerRoslyn Instance = SourceCodeIndexerRoslyn.Instance;

        foreach (var file in files)
        {

#if ASYNC
            await
#endif
                Instance.ProcessFile(file, NamespaceCodeElementsType.Nope, ClassCodeElementsType.Method, false, false);
        }

        foreach (var file2 in Instance.classCodeElements)
        {
            stringBuilder.AppendLine(file2.Key);
            foreach (var method in file2.Value)
            {
                if (method.Name.StartsWith("On") || method.Name.StartsWith("Page" + "_"))
                {
                    stringBuilder.AppendLine(method.Name);
                }
            }
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
        StringBuilder stringBuilder = new StringBuilder();

        List<string> project = new List<string>();

        var folders = Directory.GetDirectories(sczRootPath, "*", SearchOption.TopDirectoryOnly);
        foreach (var item in folders)
        {
            string nameProject = Path.GetFileName(item);
            if (nameProject.EndsWith("X"))
            {
                string project2 = nameProject.Substring(0, nameProject.Length - 1);
                // General files is in Nope. GeneralX is only for pages in General folder
                if (project2 != "General")
                {
                    project.Add(project2);
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

        foreach (var item in project)
        {
            string path = Path.Combine(sczRootPath, item);
            var pages = Directory.GetFiles(path, "*Page*.cs", SearchOption.TopDirectoryOnly).ToList();
#if ASYNC
            await AddPageMethodsAsync
#else
AddPageMethods
#endif

                (stringBuilder, pages);
        }

        //ClipboardService.SetText(stringBuilder);
    }


}