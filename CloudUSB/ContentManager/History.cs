using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentManager
{
    public enum HistoryType
    {
        Create,
        Delete,
        Change,
        Rename,
        Etc
    }

    public enum HistoryFileType
    {
        Folder,
        File        
    }

    public class History
    {
        public HistoryType Type { get; set; }
        public HistoryFileType FType { get; set; }
        public FileData Data { get; set; }
        public String OldPath { get; set; }
        public bool Sync { get; set; }
        public int HistoryId { get; set; }
        public string Date { get; set; }

        public History() { }

        public History(HistoryType historytype, FileData filedata, string date, bool sync)
        {
            this.Type = historytype;
            this.Data = filedata;
            this.Sync = sync;
            this.Date = date;
        }

        public History(HistoryType historytype, FileData filedata, String oldpath, string date, bool sync)
        {
            this.Type = historytype;
            this.Data = filedata;
            this.OldPath = oldpath;
            this.Sync = sync;
            this.Date = date;
        }
    }
}
