using System.Text;
using PSTParser.NDB;
using EMLParser;
namespace PSTParser;
    public class PSTHeader
    {
        public string DWMagic { get; set; }

        public string DwCRCPartial { get; set; } = ""; 

        public byte[] WMagicClient { get; } = [0x53, 0x4D];

        public short WVer { get; set; } = 24;

        public short WVerClient { get; set; } = 19;

        public byte BPlatformCreate { get; set; } = 0x01;
        public byte BPlatformAccess { get; set; } = 0x01;
        public byte[] DwReserved1 { get; set; } = Enumerable.Repeat<byte>(0x00, 4).ToArray();
        public byte[] DwReserved2 { get; set; } = Enumerable.Repeat<byte>(0x00, 4).ToArray();
        public byte[] BidUnused { get; set; } = Enumerable.Repeat<byte>(0x00, 8).ToArray();
        /// <summary>
        /// Reserved space do not modify
        /// </summary>
        private byte[] BidNextB{ get; } = Enumerable.Repeat<byte>(0x00, 8).ToArray();
        /// <summary>
        /// BID Next Page
        /// Unicode format only 8 bytes reserved
        /// </summary>
        public byte[] BidNextP { get; set; } = Enumerable.Repeat<byte>(0x00, 8).ToArray();
        /// <summary>
        /// Increases whenever there's a change in the HEADER CRC
        /// </summary>
        public byte[] DwUnique { get; set; } = Enumerable.Repeat<byte>(0x00, 4).ToArray();

        /// <summary>
        ///  Array of 32 bit NIDs corresponding to one of the 32 possible NID_TYPEs
        /// </summary>
        /// <returns></returns>
        public byte[] RgNID { get; set; } = Enumerable.Repeat<byte>(0x00, 128).ToArray();
        public byte[] QwUnused { get; set; } = Enumerable.Repeat<byte>(0x00, 8).ToArray();
        public Root Root { get; set; }
        public byte[] RgbFM = Enumerable.Repeat<byte>(0xFF, 128).ToArray();
        public byte[] RgbFP = Enumerable.Repeat<byte>(0xFF, 128).ToArray();
        public byte BSentinel = 0x80;
        public BlockEncoding BCryptMethod;
        public byte[] RgbReserved = Enumerable.Repeat<byte>(0x00, 2).ToArray();

        public bool IsUNICODE { get; set; } = true;
        

        public NDB.PSTBTree NodeBT { get; set; }
        public NDB.PSTBTree BlockBT { get; set; }

        public enum BlockEncoding
        {
            NONE=0x00,
            PERMUTE=0x01,
            CYCLIC=0x02,
            EDPCRYPTED = 0x10,
        }

        public PSTHeader(PSTFile pst)
        {
            using(var mmfView = pst.PSTMMF.CreateViewAccessor(0, 684))
            {
                
                var temp = new byte[4];
                mmfView.ReadArray(0, temp, 0, 4);
                DWMagic = Encoding.Default.GetString(temp);

                WVer = mmfView.ReadByte(10);
                IsUNICODE = WVer > 23;
                if (IsUNICODE) return;
                //root.PSTSize = ByteReverse.ReverseULong(root.PSTSize);
                BSentinel = mmfView.ReadByte(512);
                var cryptMethod = mmfView.ReadByte(513);

                BCryptMethod = (BlockEncoding) cryptMethod;

                var bytes = new byte[16];
                mmfView.ReadArray(216, bytes, 0, bytes.Length);
                var nbt_bref = new BREF(bytes);

                mmfView.ReadArray(232, bytes, 0, 16);
                var bbt_bref = new BREF(bytes);

                NodeBT = new NDB.PSTBTree(nbt_bref, pst);
                BlockBT = new NDB.PSTBTree(bbt_bref, pst);
            }
        }

        public PSTHeader(EMLFile eml)
        {
            DWMagic = "!BDN";
        }
    }
