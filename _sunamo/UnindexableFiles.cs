namespace SunamoRoslyn._sunamo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class UnindexableFiles
{
    public static UnindexableFiles uf = new UnindexableFiles();

    private UnindexableFiles()
    {
    }

    public List<string> unindexablePathPartsFiles = new List<string>();
    public List<string> unindexableFileNamesFiles = new List<string>();
    public List<string> unindexableFileNamesExactlyFiles = new List<string>();
    public List<string> unindexablePathEndsFiles = new List<string>();
    public List<string> unindexablePathStartsFiles = new List<string>();
}
