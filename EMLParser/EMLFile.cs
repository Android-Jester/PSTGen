namespace EMLParser;

public class EMLFile
{
    public string FileName { get; set; }
    public string FilePath { get; set; }
    public string FileContent { get; set; }
    // public byte[]? Attachment { get; set; }

   public EMLFile(string filePath)
   {
       FileName = filePath.TrimEnd().Split(@"\").Last();
       FilePath = filePath;
       FileContent = File.ReadAllText(filePath);
   }


}