using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ContentManager
{
    public class FileData
    {
        public FileData()
        {
            Metadata = new ArrayList();
            Parent = 0;
            FileId = -1;
            ID = "-1";
        }
        public string FileName { get; set; }
        public int Parent { get; set; }
        public int FileId { get; set; }
        public string FullPathStr { get; set; }
        public string ID { get; set; }
        public ArrayList Metadata { get; set; }
    }
}
