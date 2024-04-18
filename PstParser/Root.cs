using PSTParser.NDB;
namespace PSTParser;

public class Root
{
    public byte[] dwReserved { get; set; } = Enumerable.Repeat<byte>(0x00, 4).ToArray();
    public byte[] IBFileEoF { get; set; }
    public byte[] IbAMapList { get; set; }
    public byte[] CbAMapFree { get; set; }
    public byte[] CbPMapFree { get; set; }
    public BREF BREFNBT { get; set; }
    public BREF BREFBBT { get; set; }
    public FAMapValid ValidAMap { get; set; }
    public byte[] wReserved { get; set; } = Enumerable.Repeat<byte>(0x00, 2).ToArray();
    public enum FAMapValid
    {
        INVALID = 0x00,
        VALID = 0x02
    }
/// <summary>
/// Writes to the Header Section in memory of the PST File
/// </summary>
/// <param name="section"></param>
    public byte[] Write()
    {
        byte[] section = [];
       section = section.Concat(dwReserved).ToArray();
       section = section.Concat(IBFileEoF).ToArray();
       section = section.Concat(IbAMapList).ToArray();
       section = section.Concat(CbAMapFree).ToArray();
       section = section.Concat(CbPMapFree).ToArray();
       section = section.Concat(BREFNBT.BrefRoot()).ToArray();
       section = section.Concat(BREFBBT.BrefRoot()).ToArray();
        //TODO: Show indications that All AMaps are valid
        // FIXME: Get Values from AMap Data Structures
        section = section.Append((byte) ValidAMap).ToArray();
        section = section.Concat(wReserved).ToArray();
        return section;
    }
}