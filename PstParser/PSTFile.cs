using System;
using System.Collections.Generic;
using System.IO;
using System.IO.MemoryMappedFiles;
using PSTParser.Message_Layer;
using PSTParser.NDB;

namespace PSTParser;
    public class PSTFile
    {


        public PSTHeader Header { get; set; }
        //public static PSTFile CurPST { get; set; }
        public string Path { get; set; }
        public MemoryMappedFile PSTMMF { get; set; }
        public MailStore MailStore { get; set; }
        public MailFolder TopOfPST { get; set; }
        public NamedToPropertyLookup NamedPropertyLookup { get; set; }

        public PSTFile(string path)
        {
            this.Path = path;
            this.PSTMMF = MemoryMappedFile.CreateFromFile(path, FileMode.Open);

            this.Header = new PSTHeader(this);

            /*var messageStoreData = BlockBO.GetNodeData(SpecialNIDs.NID_MESSAGE_STORE);
            var temp = BlockBO.GetNodeData(SpecialNIDs.NID_ROOT_FOLDER);*/
            this.MailStore = new MailStore(this);

            this.TopOfPST = new MailFolder(this.MailStore.RootFolder.NID, new List<string>(), this);
            this.NamedPropertyLookup = new NamedToPropertyLookup(this);
            //var temp = new TableContext(rootEntryID.NID);
            //PasswordReset.ResetPassword();

        }

     
        public Tuple<ulong,ulong> GetNodeBIDs(ulong NID)
        {
            return this.Header.NodeBT.Root.GetNIDBID(NID);
        }

     

        public BBTENTRY GetBlockBBTEntry(ulong item1)
        {
            return this.Header.BlockBT.Root.GetBIDBBTEntry(item1);
        }
    }
