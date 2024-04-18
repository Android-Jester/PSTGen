namespace EMLParser;

public class EMLDirectory
{
    private List<EMLFile> EmlFiles { get; set; } = [];
    public string DirectoryName { get; set; }

    public EMLDirectory(string emlDirectoryPath)
    {
        var directory = Directory.EnumerateFiles(emlDirectoryPath);
        foreach(var filePath in directory) {
            if (filePath.Split(".").Last() == "eml")
            {
                DirectoryName = emlDirectoryPath.Trim().Split(@"\").Last();
                EmlFiles.Add(new EMLFile(filePath));    
            }
        }
    }
}