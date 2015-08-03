using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ContentManager
{
    public class Metadata
    {
        public Hashtable MetadataTable;
        
        public Metadata()
        {
            MetadataTable = new Hashtable();
        }

        public Metadata(Hashtable h)
        {
            MetadataTable = h;
        }

        public ArrayList getMetadata(string Key){
            return ((ArrayList)MetadataTable[Key]);
        }

        public void addKey(string Key)
        {
            MetadataTable.Add(Key, new MetadataElement());
        }

        public string[] getKeys()
        {
            string[] Keys = new string[MetadataTable.Count];
            MetadataTable.Keys.CopyTo(Keys, 0);
            return Keys;
        }

        public void addMetadata(string Key, FileData Value)
        {
            if (MetadataTable.Contains(Key))
            {
                ((MetadataElement)MetadataTable[Key]).File.Add(Value);
            }
            else
            {
                MetadataElement newArray = new MetadataElement();
                newArray.File.Add(Value);
                MetadataTable.Add(Key, newArray);
            }
        }

        public string toJSON()
        {
            return JsonConvert.SerializeObject(this.MetadataTable);
        }

        public static Hashtable toMetadata(String json)
        {
            Hashtable h = JsonConvert.DeserializeObject<Hashtable>(json);
            string[] a = new string[h.Count];
            if (h.Count > 0)
            {
                string[] Keys = new string[h.Count];
                h.Keys.CopyTo(Keys, 0);
                for (int i = 0; i < h.Count; i++)
                {
                    string[] jArray = (h[Keys[i]] as JArray).ToObject<string[]>() as string[];
                    ArrayList aList = new ArrayList(jArray);
                    h[Keys[i]] = aList;
                }                   
            }            
            return h;
        }
    }

    public class MetadataElement
    {        
        public string Tag { get; set; }
        public ArrayList File { get; set; }
        public MetadataElement()
        {
            File = new ArrayList();
        }
    }
}
