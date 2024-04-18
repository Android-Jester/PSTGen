using System.Diagnostics;
using EMLParser;

namespace PSTEmlGeneratorBin;

class Program
{
    static void Main(string[] args)
    {
        var sw = new Stopwatch();
        sw.Start();
        // Get EmlFile Directory
        var emlDirectory = new EMLDirectory(@"C:\Users\Duah Kwadwo Adjei\Documents");
        // var pstFile = new PSTFile();
        // pstFile.Generate(emlDirectory);
        
        sw.Stop();
        Console.WriteLine("Duration: {0}", sw.ElapsedTicks);
    }
}