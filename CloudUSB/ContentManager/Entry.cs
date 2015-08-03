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
    
    public enum EntryType
    {
        File,
        Folder,
        Category
    }

    public class Entry
    {
        
        public EntryElement Root { get; set; }        

        public string RootPath { get; set; }
        
        public Queue<History> HistoryQueue { get; set; }        
        public Hashtable Meta { get; set; }        
        
        public int Limit { get; set; }

        [JsonIgnoreAttribute]
        public EntryElement Parent { get; set; }
        public EntryElement CurrentRoot { get; set; }
        public ArrayList ServerHistroy { get; set; }
        public ArrayList ClientHistory { get; set; }
        public ArrayList ConfilicHistory { get; set; }
        public Hashtable LinearList { get; set; }

        public int count;
        public int limit = 1000;
        
        public Entry() 
        {
            count = -1;
            this.Root = null;
            this.CurrentRoot = null;
            this.Parent = null;
            Meta = new Hashtable();
            LinearList = new Hashtable();
            HistoryQueue = new Queue<History>();            
            ServerHistroy = new ArrayList();
            ClientHistory = new ArrayList();
            ConfilicHistory = new ArrayList();
            
            Parent = null;
            Limit = limit;
        }

        public Entry(string root)
        {
            count = -1;
            RootPath = root;
            Meta = new Hashtable();
            LinearList = new Hashtable();
            HistoryQueue = new Queue<History>();            
            ServerHistroy = new ArrayList();
            ClientHistory = new ArrayList();
            ConfilicHistory = new ArrayList();
            Root = new EntryElement(this, EntryType.Folder, root);
            CurrentRoot = Root;
            Parent = null;
            Limit = limit;
            
        }

        public void setRoot(EntryElement root)
        {
            RootPath = "\\";
            Root = root;
            CurrentRoot = Root;
            Parent = null;

        }

        public void setRoot(string root)
        {
            RootPath = root;
            Root = new EntryElement(this, EntryType.Folder, root);
            CurrentRoot = Root;
            Parent = null;
        }

        public void buildEntry()
        {
            if (Root != null)
            {
                Root.FILENAME = "";
                buildEntryHelper(Root);

            }
        }

        public void buildEntryHelper(EntryElement r)
        {
            String[] fileList = Directory.GetFiles(RootPath + r.PATH + "\\" + r.FILENAME);
            String[] folderList = Directory.GetDirectories(RootPath + r.PATH + "\\" + r.FILENAME);

            r.addChildren(this, fileList, folderList);
            foreach (EntryElement e in r.getDirectories())
            {
                buildEntryHelper(e);
            }
        }

        public string toJSON()
        {
            return JsonConvert.SerializeObject(this);
        }

        public static Entry toEntry(String json)
        {
            Entry entry = JsonConvert.DeserializeObject<Entry>(json);
            string[] keys = new string[entry.Meta.Count];
            entry.Meta.Keys.CopyTo(keys, 0);
            foreach (string k in keys)
            {
                //array.ToObject<List<SelectableEnumItem>>()
                JArray jo = (JArray)entry.Meta[k];
                List<FileData> entryList = jo.ToObject<List<FileData>>();
                ArrayList ee = new ArrayList(entryList.ToArray());
          
                entry.Meta.Remove(k);
                entry.Meta.Add(k, ee);                
            }
            parseEntryJSON(entry.Root);
            return entry;
        }

        public static void parseEntryJSON(EntryElement root)
        {
            if (root.Children.Count == 0) return;
            ArrayList ChildrenObject = new ArrayList();
            JObject[] entrylist = root.Children.ToArray(typeof(JObject)) as JObject[];
            root.Children.Clear();
            foreach (JObject e in entrylist)
            {
                EntryElement ee = JsonConvert.DeserializeObject<EntryElement>(e.ToString());
                ChildrenObject.Add(ee);
                parseEntryJSON(ee);
            }
            root.Children = ChildrenObject;

            string[] keys = new string[root.NamedChildren.Count];
            root.NamedChildren.Keys.CopyTo(keys, 0);

            foreach (string k in keys)
            {
                JObject jo = (JObject)root.NamedChildren[k];
                EntryElement ee = JsonConvert.DeserializeObject<EntryElement>(jo.ToString());
                root.NamedChildren.Remove(k);
                root.NamedChildren.Add(k, ee);
                parseEntryJSON(ee);
            }
            
        }

        public History[] getHistory()
        {
            ArrayList historyList = new ArrayList();
            foreach (History h in HistoryQueue)
            {
                if(!h.Sync)
                    historyList.Add(h);
            }
            return historyList.ToArray(typeof(History)) as History[];
        }

        public static Queue<History> curQueue = new Queue<History>();
        public static Queue<History> svrQueue = new Queue<History>();
        public static Hashtable metaTable = new Hashtable();

        /// <summary>
        /// 두 Entry를 병합, 서버와 클라이언트간 동기화를 위해 사용. 병합 후 히스토리 생성
        /// </summary>
        /// <param name="prv"></param>
        /// <param name="cur"></param>
        public static void mergeEntry(Entry svr, Entry clt, RequestManager rm)
        {
            //Client용 queue
            curQueue.Clear();

            //Server용 queue
            svrQueue.Clear();

            //병합 시작
            mergeEntryHelper(svrQueue, curQueue, svr, clt, rm);
        }

        public static void mergeEntryHelper(Queue<History> svrQueue, Queue<History> cltQueue, Entry svr, Entry clt, RequestManager rm)
        {
            //병합은 삭제 없이 생성만 수행

            //1. 업로드 : FID가 없는 엔트리에 대해 업로드 수행 후 FID 설정
            //폴더는 이름이 있는지 없는지 확인 있다면 FID 있지 확인 후 FID 설정
            clt.Root.File.FileId = svr.Root.FID;
            uploadEntryElement(clt, svr.Root, clt.Root, rm);
            string[] Keys = new string[clt.Meta.Count];
            clt.Meta.Keys.CopyTo(Keys, 0);
            foreach (string key in Keys)
            {
                ArrayList al = (ArrayList)clt.Meta[key];
                foreach (FileData val in al)
                {
                    EntryElement t = EntryElement.getChild(clt.Root, val.ID);
                    rm.insertMeta(rm.userID, rm.aKey, t.File, key);
                }
            }

            //2. 다운로드 : 파일 비교 후 클라이언트 측 엔트리에 존재하지 않는 파일 모두 다운로드
            //파일아이디로 엔트리 검색 후 null인 노드에 대해 다운로드한다
            downloadEntryElement(clt, svr.Root, clt.Root, rm);
        }

        public static EntryElement findTarget(EntryElement root, EntryElement target)
        {
            if (root.FID == target.File.FileId)
                return root;
            else
            {
                if (root.Type == EntryType.Folder)
                {
                    EntryElement[] nextfile = root.getChildren();

                    foreach (EntryElement n in nextfile)
                    {
                        EntryElement e = findTarget(n, target);
                        if (e != null)
                            return e;
                    }
                }
                
                return null;
            }
        }

        public static void downloadEntryElement(Entry cltEntry, EntryElement svr, EntryElement clt, RequestManager rm)
        {
            EntryElement[] children = svr.getChildren();
            foreach (EntryElement c in children)
            {
                EntryElement targetElement = Entry.findTarget(clt, c);
                if (targetElement == null)
                {
                    string fullPath = cltEntry.RootPath + c.File.FullPathStr + "\\" + c.FILENAME;
                    if (c.Type == EntryType.File)
                    {                        
                        rm.downloadFile(rm.userID, rm.aKey, cltEntry, c);
                        EntryElement newElement = new EntryElement(cltEntry, EntryType.File, fullPath);
                        newElement.File.FileId = c.FID;
                        EntryElement parent = Entry.findTarget(clt, svr);
                        parent.addChild(cltEntry, newElement);
                    }
                    else
                    {
                        //폴더 생성
                        System.IO.Directory.CreateDirectory(fullPath);
                        EntryElement newElement = new EntryElement(cltEntry, EntryType.Folder, fullPath);
                        newElement.File.FileId = c.FID;
                        EntryElement parent = Entry.findTarget(clt, svr);
                        parent.addChild(cltEntry, newElement);
                        //생성 된 폴더에 대해 재귀
                        downloadEntryElement(cltEntry, c, clt, rm);
                    }
                }
                else
                {
                    //다운받지 않아도 존재하는 파일. 이부분에서 변경 날짜 확인 후 충돌리스트로
                    //만약 폴더라면 재귀
                    if (!c.FILENAME.Equals(targetElement.FILENAME))
                    {

                    }
                    else
                    {
                        if (c.Type == EntryType.Folder)
                        {
                            downloadEntryElement(cltEntry, c, clt, rm);
                        }
                    }
                }
            }
        }

        public static void uploadEntryElement(Entry cltEntry, EntryElement svr, EntryElement clt, RequestManager rm)
        {
            EntryElement[] cltList = clt.getChildren();
            foreach (EntryElement e in cltList)
            {   
                if (e.FID < 0)
                {
                    //동기화 되지 않은 폴더 및 파일 -> 업로드한다                    
                    e.File.Parent = clt.FID;
                    if (e.Type == EntryType.Folder)
                    {
                        int fid = rm.generateFolder(rm.userID, rm.aKey, e.File);
                        e.File.FileId = fid;
                        uploadEntryElement(cltEntry, svr, e, rm);                        
                    }
                    else
                    {
                        int fid = rm.uploadFile(rm.userID, rm.aKey, cltEntry, e.File);
                        e.File.FileId = fid;
                    }
                }
                else
                {
                    //동기화된 파일 -> 충돌 확인
                    EntryElement targetElement = Entry.findTarget(svr, e);
                    if (targetElement == null)
                    {
                        if (e.Type == EntryType.Folder)
                        {
                            e.File.Parent = clt.FID;
                            int fid = rm.generateFolder(rm.userID, rm.aKey, e.File);
                            e.File.FileId = fid;
                            
                            uploadEntryElement(cltEntry, svr, e, rm);
                        }
                        else
                        {
                            int fid = rm.uploadFile(rm.userID, rm.aKey, cltEntry, e.File);
                            e.File.FileId = fid;
                        }
                    }
                    else
                    {
                        if (!e.FILENAME.Equals(targetElement.FILENAME))
                        {
                            //rm.renameFile(rm.userID, rm.aKey, e);
                        }
                        else
                        {
                            if (e.Type == EntryType.Folder)
                                uploadEntryElement(cltEntry, svr, e, rm);
                        }
                    }
                }
            }
        }

  

        /// <summary>
        /// 두 Entry를 비교, 클라이언트의 동기화 이전 목록과 현제 목록을 비교하여 History 생성 및 Metadata입력
        /// </summary>
        /// <param name="prv"></param>
        /// <param name="cur"></param>
        public static void compareEntry(Entry prv, Entry cur)
        {
            curQueue.Clear();
            metaTable.Clear();
            compareEntryHelper(curQueue, metaTable, prv, cur);
        }

        public static void compareEntryHelper(Queue<History> historyQueue, Hashtable metadata, Entry prv, Entry cur)
        {
            recursiveCompareDelete(historyQueue, metadata, prv, prv.Root, cur.Root);
            recursiveCompareCreate(historyQueue, metadata, prv, prv.Root, cur.Root);
        }

        public static EntryElement findTargetByID(EntryElement root, EntryElement e)
        {
            if (root.Type == EntryType.Folder)
            {
                if (root.FILENAME == e.FILENAME)
                    return root;
                else
                {
                    EntryElement[] nextfile = root.getChildren();

                    foreach (EntryElement n in nextfile)
                    {
                        EntryElement element = findTargetByID(n, e);
                        if (element != null)
                            return element;
                    }
                    return null;
                }
            }
            else
            {
                if (root.File.ID == e.File.ID)
                    return root;
                else
                    return null;
            }            
        }

        public static void recursiveCompareDelete(Queue<History> historyQueue, Hashtable metadata, Entry prvEntry, EntryElement prv, EntryElement cur)
        {
                        
            //파일 삭제 : Prv에 있는데 Current에 없는것, 과거 리스트를 현재 리스트에 대조하여 현재 리스트에 없는 Element는 삭제된 Element이다.
            EntryElement[] prvList = prv.getChildren();
            foreach (EntryElement e in prvList)
            {
                EntryElement targetElement = Entry.findTargetByID(cur, e);
                if (targetElement == null)
                {
                    deleteEntry(historyQueue, prvEntry, e);
                }
                else
                {
                    //폴더면 제귀적으로 자식 노드를 구성
                    if (targetElement.Type == EntryType.Folder)
                        recursiveCompareDelete(historyQueue, metadata, prvEntry, e, cur);
                }
            }            
        }

        public static void enqueTag(string tag, FileData f)
        {
            if (metaTable.Contains(tag))
            {
                ((ArrayList)metaTable[tag]).Add(f);
            }
            else
            {
                metaTable.Add(tag, new ArrayList(new FileData[] { f }));
            }
        }

        public static void recursiveCompareCreate(Queue<History> historyQueue, Hashtable metadata, Entry prvEntry, EntryElement prv, EntryElement cur)
        {
            //파일 생성 : Prv에 없는데 Current에 있는것, 현재 리스트를 과거 리스트에 대조하여 과거 리스트에 없는 Element는 새로 생성된 Element이다.
            EntryElement[] curList = cur.getChildren();
            foreach (EntryElement e in curList)
            {
                EntryElement targetElement = Entry.findTargetByID(prv, e);
                if (targetElement == null)
                {
                    createEntry(historyQueue, metadata, prvEntry, prv, e);
                }
                else
                {
                    //폴더나 파일명이 서로 다르다면
                    if (!e.FILENAME.Equals(targetElement.FILENAME))
                    {
                        historyQueue.Enqueue(targetElement.changeFilename(e.FILENAME));
                    }

                    e.FID = targetElement.FID;
                    //폴더면 제귀적으로 자식 노드를 구성, 파일이면 메타데이터 추가
                    if (targetElement.Type == EntryType.Folder)
                    {                        
                        recursiveCompareCreate(historyQueue, metadata, prvEntry, targetElement, e);
                    }
                    else
                    {
                        e.Metadata = targetElement.Metadata;
                        string[] keys = e.Metadata.ToArray(typeof(string)) as string[];
                        foreach (string key in keys)
                        {
                            if (metadata.Contains(key))
                            {
                                ((ArrayList)metadata[key]).Add(e.File);
                            }
                            else
                            {
                                ArrayList newList = new ArrayList();
                                newList.Add(e.File);
                                metadata.Add(key, newList);
                            }
                        }
                    }

                    
                }
            }
        }

        public static void createEntry(Queue<History> historyQueue, Hashtable metadata, Entry entry, EntryElement p, EntryElement e)
        {
            EntryElement newElement = new EntryElement
            {
                Index = e.Index,
                Type = e.Type,
                File = e.File,
                Children = e.Children,
                NamedChildren = e.NamedChildren,
                Metadata = e.Metadata
            };

            

            historyQueue.Enqueue(p.addChild(entry, newElement));

            if (e.Type == EntryType.Folder)
            {
                EntryElement[] childrenList = e.getChildren();

                foreach (EntryElement c in childrenList)
                {
                    createEntry(historyQueue, metadata, entry, newElement, c);
                }
            }
        }

        public static void deleteEntry(Queue<History> historyQueue, Entry entry, EntryElement e)
        {
            EntryElement parent = EntryElement.getParent(entry, e);
            if (parent != null)
            {
                if (e.Type == EntryType.Folder)
                {
                    deleteEntryHelper(historyQueue, entry, e);
                }
                historyQueue.Enqueue(parent.removeChild(e));
            }
        }

        public static void deleteEntryHelper(Queue<History> historyQueue, Entry entry, EntryElement e)
        {
            EntryElement[] folderList = e.getDirectories();
            EntryElement[] fileList = e.getFiles();

            foreach (EntryElement d in folderList)
            {
                deleteEntry(historyQueue, entry, d);
                historyQueue.Enqueue(e.removeChild(d));
            }

            
            string date = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
            foreach (EntryElement f in fileList)
            {                
                string[] ext = f.FILENAME.Split('.');
                if (ext.Length > 1)
                    entry.addMetaData(ext.Last(), e.File);
                entry.addMetaData(date, e.File);
                historyQueue.Enqueue(e.removeChild(f));
            }
        }

        public static Entry buildEntryFromFileList(Entry entry, EntryElement[] fileList)
        {
            if (fileList.Length > 0)
            {
                Entry newEntry = new Entry();
                newEntry.setRoot(fileList[0]);
                entry.Root.File.FileId = newEntry.Root.File.FileId;
                for (int i = 1; i < fileList.Length; i++)
                {
                    EntryElement parent = EntryElement.findParent(newEntry.Root, fileList[i]);
                    parent.addChild(entry, fileList[i]);
                }
                return newEntry;
            }
            return null;            
        }
    
        public string[] getMetaKeys()
        {
            string[] Keys = new string[Meta.Count];
            Meta.Keys.CopyTo(Keys, 0);
            return Keys;
        }

        public void addMetaData(string t, FileData e)
        {
            EntryElement target = EntryElement.getChild(this.Root, e.ID);
            if (!Meta.Contains(t))
            {
                if (!target.Metadata.Contains(t))
                {
                    ArrayList newTagList = new ArrayList();
                    newTagList.Add(e);
                    Meta.Add(t, newTagList);
                    target.Metadata.Add(t);
                }
            }
            else
            {
                if (!target.Metadata.Contains(t))
                {
                    ((ArrayList)Meta[t]).Add(e);
                    target.Metadata.Add(t);
                }
            }
        }

        public void removeMetaData(FileData f, string tag)
        {
            EntryElement target = EntryElement.getChild(this.Root, f.ID);
            FileData[] fileListinMeta = ((ArrayList)Meta[tag]).ToArray(typeof(FileData)) as FileData[];
            
            foreach (FileData t in fileListinMeta)
            {
                if (t.ID.Equals(f.ID))
                {
                    ((ArrayList)Meta[tag]).Remove(t);
                }
            }

            target.Metadata.Remove(tag);
        }

        public void removeMetaData(string f, string tag)
        {
            EntryElement target = EntryElement.getChild(this.Root, f);
            FileData[] fileListinMeta = ((ArrayList)Meta[tag]).ToArray(typeof(FileData)) as FileData[];

            foreach (FileData t in fileListinMeta)
            {
                if (t.ID.Equals(f))
                {
                    ((ArrayList)Meta[tag]).Remove(t);
                }
            }

            target.Metadata.Remove(tag);
        }

        
//////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


        //Old method
        public void makeEntry()
        {            
            String[] filelist = Directory.GetFiles(RootPath + Root.File.FullPathStr);
            String[] folderlist = Directory.GetDirectories(RootPath + Root.File.FullPathStr);
            makeEntryList(Root, filelist, folderlist);
            Root.Index = count++;
            LinearList.Clear();
            if (Root.File.FileId != -1)
            {
                LinearList.Add(Root.File.FileId, Root);
            }
            //search entry
            searchEntry(Root);
        }

        public void makeEntryList(EntryElement root, String[] filelist, String[] folderlist)
        {
            
            foreach (String s in filelist)
            {
                String parentpath = s;
                String[] token = s.Split('\\');
                string[] rtoken = RootPath.Split('\\');
                string rpath = "";
                for (int i = rtoken.Length; i < token.Length; i++)
                {
                    rpath = rpath + "\\" + token[i];
                }
                if (token != null)
                {
                    if (!root.NamedChildren.ContainsKey(token.Last()))
                    {
                        EntryElement newElement = new EntryElement(EntryType.File, new FileData { FileName = token.Last(), FullPathStr = rpath });

                        if (root.File.FileId != -1)
                        {
                            if (!LinearList.ContainsKey(root.File.FileId))
                                LinearList.Add(root.File.FileId, root);
                        }

                        root.Children.Add(newElement);
                        root.NamedChildren.Add(token.Last(), newElement);
                    }
                }
                
            }
            foreach (String s in folderlist)
            {
                String parentpath = s;
                String[] token = s.Split('\\');
                string[] rtoken = RootPath.Split('\\');
                string rpath = "";
                for (int i = rtoken.Length; i < token.Length; i++)
                {
                    rpath = rpath + "\\" + token[i];
                }
                if (token != null)
                {
                    if (!root.NamedChildren.ContainsKey(token.Last()))
                    {
                        EntryElement newElement = new EntryElement(EntryType.Folder, new FileData { FileName = token.Last(), FullPathStr = rpath });

                        if (root.File.FileId != -1)
                        {
                            if (!LinearList.ContainsKey(root.File.FileId))
                                LinearList.Add(root.File.FileId, root);
                        }

                        root.Children.Add(newElement);
                        root.NamedChildren.Add(token.Last(), newElement);
                    }
                }
                
            }
        }

        public void searchEntry(EntryElement root)
        {            
            EntryElement[] entrylist = root.Children.ToArray(typeof(EntryElement)) as EntryElement[];
            foreach (EntryElement e in entrylist)
            {
                e.Index = count++;
                if (e.Type == EntryType.Folder)
                {
                    String[] filelist = Directory.GetFiles(RootPath + e.File.FullPathStr);
                    String[] folderlist = Directory.GetDirectories(RootPath + e.File.FullPathStr);
                    makeEntryList(e, filelist, folderlist);
                    searchEntry(e);
                }
            }
        }

        public void moveDirectory(string path)
        {
            if (!CurrentRoot.File.FullPathStr.Equals(path)) //뒤로가기 버튼 클릭 시, 에러발생
            {
                string[] p = path.Split('\\');
                string foldername = p[p.Count() - 1];
                EntryElement target = null;

                foreach (EntryElement e in CurrentRoot.Children)
                {
                    if (e.Type == EntryType.Folder)
                    {
                        if (e.File.FileName.Equals(foldername))
                        {
                            target = e;
                        }
                    }
                }
                Parent = CurrentRoot;
                CurrentRoot = target;
            }
        }

        

        

        public static void writeJSON(string JSON, string file)
        {
            using (StreamWriter sw = new StreamWriter(@"\\"+file))
            {
                sw.Write(JSON);
            }
        }

        public static string readJSON(string file)
        {
            string JSON = null;
            using (StreamReader sr = new StreamReader(@"\\" + file))
            {
                JSON = sr.ReadToEnd();
            }
            return JSON;
        }

        public void makeEntryFromData(FileData[] fileList)
        {
            //search root
            EntryElement root = new EntryElement();
            foreach (FileData f in fileList)
            {
                if (f.Parent == 0)
                {
                    root.File = f;
                    break;
                }
            }

        }

        public void deleteFile(EntryElement parent, EntryElement target, string date)
        {
            if (target.Type == EntryType.Folder)
            {
                EntryElement[] children = target.Children.ToArray(typeof(EntryElement)) as EntryElement[];
                foreach (EntryElement e in children)
                {
                    deleteFile(target, e, date);
                }
                HistoryQueue.Enqueue(new History(HistoryType.Delete, target.File, date, false));
                parent.Children.Remove(target);
                parent.NamedChildren.Remove(target.File.FileName);
            }
            else if (target.Type == EntryType.File)
            {
                HistoryQueue.Enqueue(new History(HistoryType.Delete, target.File, date, false));
                parent.Children.Remove(target);
                parent.NamedChildren.Remove(target.File.FileName);
                deleteFileFromMeta(target); 
            }

        }

        public void deleteFileFromMeta(EntryElement target)
        {

        }

        public void onFileChanged(object source, FileSystemEventArgs e)
        {
            if (HistoryQueue.Count >= Limit) HistoryQueue.Dequeue();
            string[] rpath = RootPath.Split('\\');
            string[] bpath = e.FullPath.Split('\\');
            string[] path = null;

            EntryElement targetFolder = Root;
            string resultPath = "";

            if (rpath.Length - bpath.Length > 1)
            {
                path = new string[rpath.Length - bpath.Length-1];
                for (int i = rpath.Length; i < bpath.Length-1; i++)
                {
                    path[i] = bpath[i];
                    resultPath = resultPath + "\\" + bpath[i];
                }
                for (int i = 0; i < path.Length; i++)
                {
                    targetFolder = (EntryElement)targetFolder.NamedChildren[path[i]];
                }
            }
            
            FileData newFIle = new FileData
            {
                FileName = e.Name,
                FullPathStr = resultPath,
                Parent = targetFolder.File.FileId
            };
            
            EntryElement newEntryElement;
            string fullPath = RootPath + resultPath + "\\" + newFIle.FileName;
            if ((File.GetAttributes(resultPath) & FileAttributes.Directory) == FileAttributes.Directory)
            {
                newEntryElement = new EntryElement
                {
                    File = newFIle,
                    Type = EntryType.Folder
                };
            }
            else
            {
                newEntryElement = new EntryElement
                {
                    File = newFIle,
                    Type = EntryType.File
                };
            }

            string date = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);

            if (e.ChangeType == WatcherChangeTypes.Created)
            {                
                HistoryQueue.Enqueue(new History(HistoryType.Create, newFIle, date, false));                
            }
            else if (e.ChangeType == WatcherChangeTypes.Deleted)
            {                
                deleteFile(targetFolder, (EntryElement)(targetFolder.NamedChildren[e.Name]), date);                
            }
            else if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                HistoryQueue.Enqueue(new History(HistoryType.Change, newFIle, date, false));
            }            
        }

        public void onFileRenamed(object source, RenamedEventArgs e)
        {
            if (HistoryQueue.Count >= Limit) HistoryQueue.Dequeue();

            string[] rpath = RootPath.Split('\\');
            string[] bpath = e.FullPath.Split('\\');
            string[] path = null;

            EntryElement targetFolder = Root;

            string date = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
            string resultPath = "";
            if (rpath.Length - bpath.Length > 1)
            {
                path = new string[rpath.Length - bpath.Length - 1];
                for (int i = rpath.Length; i < bpath.Length - 1; i++)
                {
                    path[i] = bpath[i];
                    resultPath = resultPath + "\\" + bpath[i];
                }
                for (int i = 0; i < path.Length; i++)
                {
                    targetFolder = (EntryElement)targetFolder.NamedChildren[path[i]];
                }
            }

            FileData newFIle = new FileData
            {
                FileName = e.Name,
                FullPathStr = resultPath
            };

            HistoryQueue.Enqueue(new History(HistoryType.Rename, newFIle, e.OldFullPath, date, false));            
        }    

    }

    public class EntryElement
    {
        public int Index { get; set; }
        public EntryType Type { get; set; }
        public FileData File { get; set; }        
        public ArrayList Children { get; set; }
        public Hashtable NamedChildren { get; set; }
        public ArrayList Metadata { get; set; }
        public string Date { get; set; }

        [JsonIgnoreAttribute]
        public string FILENAME { get { return File.FileName; } set { File.FileName = value; } }
        public string ID { get { return File.ID; } set { File.ID = value; } }
        public int FID { get { return File.FileId; } set { File.FileId = value; } }
        public string PATH { get { return File.FullPathStr; } set { File.FullPathStr = value; } }

        public EntryElement() 
        {
            this.Children = new ArrayList();
            this.Metadata = new ArrayList();
            this.NamedChildren = new Hashtable();
            Date = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
            
        }

        public EntryElement(EntryType type, FileData file)
        {
            this.Type = type;
            this.File = file;
            this.Children = new ArrayList();
            this.Metadata = new ArrayList();
            this.NamedChildren = new Hashtable();
            Date = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
        }

        public EntryElement(Entry entry, EntryType type, string file)
        {
            this.Type = type;

            string[] rootToken = entry.RootPath.Split('\\');
            string[] fileToken = file.Split('\\');
            string relativePath = "";
            FileData target;
            //EntryElement parentElement = entry.Root;
            for (int i = rootToken.Length; i < fileToken.Length - 1; i++)
            {
                relativePath = relativePath + "\\" + fileToken[i];
            }



            if (type == EntryType.File)
                target = new FileData
                {
                    FileName = fileToken.Last(),
                    FileId = entry.count--,
                    ID = FileManager.GetFileIDB(file).ToString(),
                    FullPathStr = relativePath
                    //Parent = parentElement.FID
                };
            else
                target = new FileData
                {
                    FileName = fileToken.Last(),
                    FileId = entry.count--,
                    ID = "-1",
                    FullPathStr = relativePath
                    //Parent = parentElement.FID
                };

            this.File = target;
            this.Children = new ArrayList();
            this.Metadata = new ArrayList();
            this.NamedChildren = new Hashtable();
            Date = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
        }

        public static EntryElement getChild(EntryElement target,string fileID)
        {
            EntryElement result = target.getChild(fileID);

            if (result == null)
            {
                EntryElement[] directories = target.getDirectories();
                foreach (EntryElement e in directories)
                {
                    result = getChild(e, fileID);
                    if (result != null)
                        break;
                }
            }
         
            return result;
        }

        public static EntryElement getChild(EntryElement target, int fileID)
        {
            EntryElement result = target.getChild(fileID);

            if (result == null)
            {
                EntryElement[] directories = target.getDirectories();
                foreach (EntryElement e in directories)
                {
                    result = getChild(e, fileID);
                    if (result != null)
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// 해당 노드의 부모 노드를 반환한다
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        
        public static EntryElement getParent(Entry entry, EntryElement e)
        {
            Queue<EntryElement> queue = new Queue<EntryElement>();
            queue.Enqueue(entry.Root);

            while (queue.Count > 0)
            {
                EntryElement target = queue.Dequeue();
                if (target.contains(e))
                {
                    return target;
                }
                else
                {
                    EntryElement[] Directories = target.getDirectories();
                    //EntryElement[] Files = target.getFiles();                    
                    foreach (EntryElement d in Directories)
                    {
                        queue.Enqueue(d);
                    }
                }
            }
            return null;
        }

        public string makeFileName(string filename, string ext, int count)
        {
            return filename + "(" + count.ToString() + ")" + ext;
        }

        /// <summary>
        /// EntryElement의 자식도느 추가
        /// </summary>
        /// <param name="entry">전체 Entry</param>
        /// <param name="e">추가 할 EntryElement</param>
        public History addChild(Entry entry, EntryElement e)
        {
            if (!contains(e))
            {
                int count = 1;
                string[] fileNameToken = e.FILENAME.Split('.');
                string resultFileName = e.FILENAME;
                if (fileNameToken.Length > 1)
                {
                    while (NamedChildren.Contains(resultFileName))
                    {
                        
                        resultFileName = makeFileName(fileNameToken[0], fileNameToken[1], count++);
                    }
                }
                else
                {
                    while (NamedChildren.Contains(resultFileName))
                    {
                        resultFileName = makeFileName(fileNameToken[0], "", count++);
                    }
                }

                if (count > 1)
                {
                    System.IO.File.Move(entry.RootPath + e.File.FullPathStr + "\\" + e.FILENAME, entry.RootPath + e.File.FullPathStr + "\\" + resultFileName);
                    e.File.FileName = resultFileName;
                }

                HistoryFileType ht;
                e.File.Parent = File.FileId;
                Children.Add(e);
                NamedChildren.Add(e.FILENAME, e);

                if (e.Type == EntryType.File)
                    ht = HistoryFileType.File;
                else
                    ht = HistoryFileType.Folder;

                string date = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);

                Date = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);

                return new History
                {
                    Type = HistoryType.Create,
                    FType = ht,
                    Data = e.File,
                    Sync = false,
                    Date = date
                };
            }
            return null;
        }

        /// <summary>
        /// EntryElement의 자식노드 제거
        /// </summary>
        /// <param name="e"></param>
        public History removeChild(EntryElement e)
        {
            if (contains(e))
            {
                HistoryFileType ht;

                Children.Remove(e);
                NamedChildren.Remove(e.FILENAME);

                if (e.Type == EntryType.File)
                    ht = HistoryFileType.File;
                else
                    ht = HistoryFileType.Folder;

                Date = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);

                return new History
                {
                    Type = HistoryType.Delete,
                    FType = ht,
                    Data = e.File,
                    Sync = false,
                    Date = Date
                };
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public History changeFilename(string filename)
        {
            string oldPath = File.FullPathStr + "\\" + FILENAME;
            File.FileName = filename;

            HistoryFileType ht;

            if (this.Type == EntryType.File)
                ht = HistoryFileType.File;
            else
                ht = HistoryFileType.Folder;

            string date = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);

            return new History
            {
                Type = HistoryType.Rename,
                FType = ht,
                OldPath = oldPath,
                Data = this.File,
                Sync = false,
                Date = date
            };
        }

        /// <summary>
        /// EntryElement의 자식노드에 해당 EntryElement가 포함되어 있는지 확인
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public bool contains(EntryElement e)
        {
            EntryElement[] children;

            if (e.Type == EntryType.File)
            {
                children = getFiles();
                foreach (EntryElement c in children)
                {
                    if ((c.FID == e.FID) || (!c.ID.Equals("-1")&&c.ID == e.ID))
                        return true;
                }
            }
            else
            {
                children = getDirectories();
                foreach (EntryElement c in children)
                {
                    if (e.FILENAME.Equals(c.FILENAME))
                        return true;
                }
            }
            
            return false;
            /*if (NamedChildren.Contains(e.FILENAME) && ((EntryElement)NamedChildren[e.FILENAME]).ID == e.ID)
                return true;
            else
                return false;*/
        }

        public EntryElement getChild(EntryElement e)
        {
            EntryElement[] children;

            if (e.Type == EntryType.File)
            {
                children = getFiles();
                foreach (EntryElement c in children)
                {
                    if (c.FID == e.FID || c.ID == e.ID)
                        return c;
                }
                return null;
            }
            else
            {
                children = getDirectories();
                foreach (EntryElement c in children)
                {
                    if (e.FILENAME.Equals(c.FILENAME))
                        return c;
                }
                return null;
            }
        }

        public EntryElement getChild(int fID)
        {
            EntryElement[] children = Children.ToArray(typeof(EntryElement)) as EntryElement[];

            foreach (EntryElement c in children)
            {
                if (c.FID == fID)
                    return c;
            }
            return null;
        }

        public EntryElement getChild(string ID)
        {
            EntryElement[] children = Children.ToArray(typeof(EntryElement)) as EntryElement[];

            foreach (EntryElement c in children)
            {
                if (c.ID == ID)
                    return c;
            }
            return null;
        }

        public EntryElement getChildFromName(string Name)
        {
            EntryElement[] children = Children.ToArray(typeof(EntryElement)) as EntryElement[];

            foreach (EntryElement c in children)
            {
                if (c.FILENAME.Equals(Name))
                    return c;
            }
            return null;
        }

        /// <summary>
        /// 입력된 EntryElement의 리스트에 포함된 EntryElement를 자식노드에 추가
        /// </summary>
        /// <param name="entryList"></param>
        public void addChildren(Entry entry, EntryElement[] entryList)
        {
            foreach (EntryElement e in entryList)
            {
                addChild(entry, e);
            }
        }

        /// <summary>
        /// File과 Directory를 입력받아 자식노드에 추가
        /// </summary>
        /// <param name="entry"></param>
        /// <param name="fileList"></param>
        /// <param name="folderList"></param>
        public void addChildren(Entry entry, string[] fileList, string[] folderList)
        {
            foreach (string file in fileList)
            {
                EntryElement newElement = new EntryElement(entry, EntryType.File, file);                
                addChild(entry, newElement);              

                string date = string.Format("{0:yyyy-MM-dd}", DateTime.Now);
                entry.addMetaData(date, newElement.File);
                string[] ext = file.Split('.');
                if (ext.Length > 1)
                    entry.addMetaData(ext.Last(), newElement.File);
            }
            foreach (string folder in folderList)
            {
                EntryElement newElement = new EntryElement(entry, EntryType.Folder, folder);                
                addChild(entry, newElement);
            }
        }

        /// <summary>
        /// 모든 자식노드의 목록 반환
        /// </summary>
        /// <returns></returns>
        public EntryElement[] getChildren()
        {
            return Children.ToArray(typeof(EntryElement)) as EntryElement[];
        }

        /// <summary>
        /// 자식노드들 중 Directory 반환
        /// </summary>
        /// <returns></returns>
        public EntryElement[] getDirectories()
        {
            ArrayList folderList = new ArrayList();
            foreach (EntryElement e in Children)
            {
                if (e.Type == EntryType.Folder)
                {
                    folderList.Add(e);
                }
            }
            return folderList.ToArray(typeof(EntryElement)) as EntryElement[];
        }

        /// <summary>
        /// 자식 노드들 중 File 반환
        /// </summary>
        /// <returns></returns>
        public EntryElement[] getFiles()
        {
            ArrayList fileList = new ArrayList();
            foreach (EntryElement e in Children)
            {
                if (e.Type == EntryType.File)
                {
                    fileList.Add(e);
                }
            }
            return fileList.ToArray(typeof(EntryElement)) as EntryElement[];
        }

        public void deleteMetadata(string key)
        {

        }

        public static EntryElement findParent(EntryElement root, EntryElement e)
        {
            if (root.FID == e.File.Parent)
                return root;
            else
            {
                EntryElement[] directories = root.getDirectories();
                EntryElement parent = null;
                foreach (EntryElement d in directories)
                {
                    parent = findParent(d, e);
                    if (parent != null)
                        break;
                }
                return parent;
            }           
        }

        public static EntryElement findParentByID(EntryElement root, EntryElement e)
        {
            EntryElement[] files = root.getFiles();
            EntryElement[] dirs = root.getDirectories();

            foreach (EntryElement f in files)
            {
                if (root.File.ID == f.File.ID)
                    return root;
            }

            EntryElement result = null;

            foreach (EntryElement d in dirs)
            {
                if (root.FILENAME != d.FILENAME)
                {
                    result = findParentByID(d, e);
                    if (result != null)
                        break;
                }else{
                    return root;
                }
            }
            return result;
        }
        
    }
    



}
