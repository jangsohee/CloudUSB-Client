using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Net.Http.Headers;
using Newtonsoft.Json;


namespace ContentManager
{
    public class RequestManager
    {

        public class compareFile : IComparer
        {
            int IComparer.Compare(Object x, Object y)
            {
                EntryElement a = (EntryElement)x;
                EntryElement b = (EntryElement)y;

                if (a.File.Parent < b.File.Parent)
                    return -1;
                else if (a.File.Parent == b.File.Parent)
                    return 0;
                else
                    return 1;
            }
        }

        public class compareDate : IComparer
        {
            int IComparer.Compare(Object x, Object y)
            {
                History a = (History)x;
                History b = (History)y;

                DateTime adate = DateTime.ParseExact(a.Date, "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);
                DateTime bdate = DateTime.ParseExact(b.Date, "yyyy-MM-dd HH:mm:ss",
                                       System.Globalization.CultureInfo.InvariantCulture);
                if (adate.Ticks < bdate.Ticks)
                    return -1;
                else if (bdate.Ticks == adate.Ticks)
                    return 0;
                else
                    return 1;
            }
        }
        
        public string LastResponse { protected set; get; }
        public string token { get; set; }
        public Hashtable LinearList { get; set; }
        
        CookieContainer cookies = new CookieContainer();

        public RequestManager()
        {
            LinearList = new Hashtable();
            
        }

        

        internal string GetCookieValue(Uri SiteUri, string name)
        {
            Cookie cookie = cookies.GetCookies(SiteUri)[name];
            return (cookie == null) ? null : cookie.Value;
        }

        

        public string GetResponseContent(HttpWebResponse response)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }
            Stream dataStream = null;
            StreamReader reader = null;
            string responseFromServer = null;


            try
            {
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                reader = new StreamReader(dataStream);
                // Read the content.
                responseFromServer = reader.ReadToEnd();
                
                // Cleanup the streams and the response.
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (dataStream != null)
                {
                    dataStream.Close();
                }
                response.Close();
            }
            LastResponse = responseFromServer;
            return responseFromServer;
        }

        public void GetResponseContentToFile(HttpWebResponse response, string path)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }
            Stream dataStream = null; 
            StreamReader reader = null;
            string responseFromServer = null;

            try
            {
                // Get the stream containing content returned by the server.
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.
                //reader = new StreamReader(dataStream);
                // Read the content.

                using (Stream output = File.OpenWrite(path))
                {
                    byte[] buffer = new byte[2024];                    
                    int bytesRead;
                    while ((bytesRead = dataStream.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        output.Write(buffer, 0, bytesRead);
                    }
                }

                // Cleanup the streams and the response.
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (reader != null)
                {
                    reader.Close();
                }
                if (dataStream != null)
                {
                    dataStream.Close();
                }
                response.Close();
            }
            LastResponse = responseFromServer;            
        }

        public HttpWebResponse SendPOSTRequest(string uri, string content, string login, string password, bool allowAutoRedirect)
        {
            HttpWebRequest request = GeneratePOSTRequest(uri, content, login, password, allowAutoRedirect);
            return GetResponse(request);
        }

        public HttpWebResponse SendGETRequest(string uri, string login, string password, bool allowAutoRedirect)
        {
            HttpWebRequest request = GenerateGETRequest(uri, login, password, allowAutoRedirect);
            return GetResponse(request);
        }

        public HttpWebResponse SendRequest(string uri, string content, string method, string login, string password, bool allowAutoRedirect)
        {
            HttpWebRequest request = GenerateRequest(uri, content, method, login, password, allowAutoRedirect);
            return GetResponse(request);
        }

        public HttpWebRequest GenerateGETRequest(string uri, string login, string password, bool allowAutoRedirect)
        {
            return GenerateRequest(uri, null, "GET", null, null, allowAutoRedirect);
        }

        public HttpWebRequest GeneratePOSTRequest(string uri, string content, string login, string password, bool allowAutoRedirect)
        {
            return GenerateRequest(uri, content, "POST", null, null, allowAutoRedirect);
        }

        internal HttpWebRequest GenerateRequest(string uri, string content, string method, string login, string password, bool allowAutoRedirect)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }
            // Create a request using a URL that can receive a post. 
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(uri);
            // Set the Method property of the request to POST.
            request.Method = method;
            // Set cookie container to maintain cookies
            request.CookieContainer = cookies;
            request.AllowAutoRedirect = allowAutoRedirect;
            // If login is empty use defaul credentials
            if (string.IsNullOrEmpty(login))
            {
                request.Credentials = CredentialCache.DefaultNetworkCredentials;
            }
            else
            {
                request.Credentials = new NetworkCredential(login, password);
            }
            if (method == "POST")
            {
                // Convert POST data to a byte array.
                byte[] byteArray = Encoding.UTF8.GetBytes(content);
                // Set the ContentType property of the WebRequest.
                request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                // Set the ContentLength property of the WebRequest.
                request.ContentLength = byteArray.Length;
                // Get the request stream.
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.
                dataStream.Close();
            }
            return request;
        }

        internal HttpWebResponse GetResponse(HttpWebRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            HttpWebResponse response = null;
            try
            {
                response = (HttpWebResponse)request.GetResponse();
                cookies.Add(response.Cookies);
                // Print the properties of each cookie.
                /*Console.WriteLine("\nCookies: ");
                foreach (Cookie cook in cookies.GetCookies(request.RequestUri))
                {
                    Console.WriteLine("Domain: {0}, String: {1}", cook.Domain, cook.ToString());
                }*/
            }
            catch (WebException ex)
            {
                Console.WriteLine("Web exception occurred. Status code: {0}", ex.Status);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return response;
        }


        public string aKey;
        public string userID;
        public string testLogin(string uId, string pwd)
        {
            RequestManager rq = new RequestManager();
            userID = uId;
            string req = string.Format("userId={0}&password={1}", userID, pwd);            
            HttpWebResponse test = rq.SendPOSTRequest("http://210.118.74.120:8080/cu/api/login", req, null, null, true);
            string result = rq.GetResponseContent(test);
            dynamic jsonStr = JsonConvert.DeserializeObject(result);            
            aKey = jsonStr["clientKey"];
            return aKey;
        }

        public bool syncStart(string uId, string key)
        {
            RequestManager rq = new RequestManager();
            string req = string.Format("userId={0}&clientKey={1}", uId, key);            
            HttpWebResponse test = rq.SendPOSTRequest("http://210.118.74.120:8080/cu/api/syncStart", req, null, null, true);
            string result = rq.GetResponseContent(test);
            dynamic jsonStr = JsonConvert.DeserializeObject(result);
            string r = jsonStr["result"];
            if (r.Equals("true"))
            {
                return true;
            }
            else
            {
                return false;
            }
            
        }

        public bool syncEnd(string uId, string key)
        {
            RequestManager rq = new RequestManager();
            string req = string.Format("userId={0}&clientKey={1}", uId, key);
            HttpWebResponse test = rq.SendPOSTRequest("http://210.118.74.120:8080/cu/api/syncEnd", req, null, null, true);
            string result = rq.GetResponseContent(test);
            dynamic jsonStr = JsonConvert.DeserializeObject(result);
            string r = jsonStr["result"];
            if (r.Equals("true"))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public EntryElement[] getAllFileList(string uId, string Key)
        {
            string req = string.Format("userId={0}&clientKey={1}", uId, Key);
            HttpWebResponse reqResult = SendPOSTRequest("http://210.118.74.120:8080/cu/api/getAllFileList", req, null, null, true);
            string result = GetResponseContent(reqResult);
            dynamic json = JsonConvert.DeserializeObject(result);
            dynamic files = json["fileList"];

            ArrayList flist = new ArrayList();
            compareFile cf = new compareFile();

            //getHistory(uId, Key);
            LinearList.Clear();
            foreach (dynamic f in files)
            {
                int fileId = f["fileId"];
                int parentId = f["parentId"];
                string name = f["name"];
                string type = f["type"];

                EntryType et;

                if (type.Equals("folder"))
                {
                    et = EntryType.Folder;
                }
                else
                {
                    et = EntryType.File;
                }
                EntryElement newElement = new EntryElement(et, new FileData
                {
                    FileName = name,
                    FileId = fileId,
                    Parent = parentId,
                    ID = "-1"
                });               
               
                flist.Add(newElement);
            }
            
            flist.Sort(cf);

            EntryElement[] fresult = (flist.ToArray(typeof(EntryElement))) as EntryElement[];

            return fresult;
        }

        public History[] getHistory(string uId, string Key)
        {            
            string req = string.Format("userId={0}&clientKey={1}", uId, Key);
            HttpWebResponse reqResult = SendPOSTRequest("http://210.118.74.120:8080/cu/api/getSyncList", req, null, null, true);
            string result = GetResponseContent(reqResult);
            dynamic json = JsonConvert.DeserializeObject(result);
            dynamic files = json["syncList"];
            ArrayList hlist = new ArrayList();
            

            foreach (dynamic f in files)
            {
                int fileId = f["fileId"];
                int parentId = f["parentId"];
                string name = f["fileName"];
                int historId = f["historyId"];
                string type = f["type"];
                string ftype = f["fileType"];
                string date = f["time"];
                HistoryType ht;
                HistoryFileType hft;

                if(type.Equals("Created"))
                {
                    ht = HistoryType.Create;
                }
                else if (type.Equals("Deleted"))
                {
                    ht = HistoryType.Delete;
                }
                else if (type.Equals("Changed name"))
                {
                    ht = HistoryType.Rename;
                }
                else
                {
                    //Moved
                    ht = HistoryType.Etc;
                }

                if (ftype.Equals("folder"))
                {
                    hft = HistoryFileType.Folder;
                }
                else
                {
                    hft = HistoryFileType.File;
                }

                FileData newFIle = new FileData
                {
                    FileName = name,
                    FileId = fileId,
                    Parent = parentId
                };
                

                hlist.Add(new History
                {
                    HistoryId = historId,
                    FType = hft,
                    Sync = true,
                    Type = ht,
                    Data = newFIle,
                    Date = date
                });                
            }
                        
            return (hlist.ToArray(typeof(History)) as History[]);
        }

        public void checkHistory(Entry entry, History[] historyList)
        {
            ArrayList sum = new ArrayList();
            compareDate cd = new compareDate();

            foreach (History h in historyList)
            {
                sum.Add(h);
            }
            foreach (History h in entry.HistoryQueue)
            {
                if(h.Sync == false)
                    sum.Add(h);
            }            
            sum.Sort(cd);

            History[] sumList = sum.ToArray(typeof(History)) as History[];
            for (int i = 0; i < sumList.Length; i++ )
            {
                if (sumList[i].Type == HistoryType.Delete)
                {
                    if (historyList.Contains(sumList[i]))
                    {
                        ArrayList check = new ArrayList();
                        for (int j = i + 1; j < sumList.Length; j++)
                        {
                            if (sumList[j].Data.FileId == sumList[i].Data.FileId)
                            {
                                if (sumList[j].Type == HistoryType.Delete)
                                {
                                    for (int k = 0; k < check.Count; k++)
                                    {
                                        ((History)check[k]).Sync = true;
                                    }
                                    
                                    check.Clear();
                                    break;
                                }
                                else
                                {
                                    check.Add(sumList[j]);
                                }
                            }
                                                            
                        }
                        if (check.Count > 0)
                        {
                            
                            entry.ConfilicHistory.Add(check);   
                         
                        }
                        else
                        {
                            entry.ServerHistroy.Add(sumList[i]);
                        }
                    }
                }
                else
                {

                }
            }

        }

        public void syncFromHistory(string uId, string Key, History[] historyList, string rootPath, Entry entry)
        {            
            try
            {
                foreach (History h in historyList)
                {
                    entry.HistoryQueue.Enqueue(h);
                    if (h.Type == HistoryType.Create)
                    {
                        if (h.FType == HistoryFileType.Folder)
                        {
                            makeFolder(entry.RootPath, entry, h);
                        }
                        else
                        {
                            fileDownload(uId, Key, h.Data.FileId, entry.RootPath, entry, h);
                        }
                    }
                    else if (h.Type == HistoryType.Delete)
                    {
                        deleteFIle(entry, h);
                    }
                    else if (h.Type == HistoryType.Rename)
                    {
                        EntryElement parent = findParentFolder(entry.Root, h);
                        string path = makePath(entry, h);

                        if (parent != null)
                        {
                            EntryElement[] cl = parent.Children.ToArray(typeof(EntryElement)) as EntryElement[];
                            foreach (EntryElement ee in cl)
                            {
                                if (ee.File.FileId == h.Data.FileId)
                                {
                                    if (h.FType == HistoryFileType.Folder)
                                    {
                                        Directory.Move(entry.RootPath + path + "\\" + ee.File.FileName, entry.RootPath + path + "\\" + h.Data.FileName);
                                    }
                                    else
                                    {
                                        File.Move(entry.RootPath + path + "\\" + ee.File.FileName, entry.RootPath + path + "\\" + h.Data.FileName);
                                    }
                                }
                            }
                        }
                    }
                    else if (h.Type == HistoryType.Etc)
                    {
                        /*EntryElement parent = findParentFolder(entry.Root, h);
                        EntryElement target = findTarget(entry.Root, h);
                        string path = makePath(entry, h);

                        if (parent != null)
                        {
                            EntryElement[] cl = parent.Children.ToArray(typeof(EntryElement)) as EntryElement[];
                            foreach (EntryElement ee in cl)
                            {
                                if (ee.File.FileId == h.Data.FileId)
                                {
                                    if (h.FType == HistoryFileType.Folder)
                                    {
                                        Directory.Move(entry.RootPath + path + "\\" + ee.File.FileName, entry.RootPath + path + "\\" + h.Data.FileName);
                                    }
                                    else
                                    {
                                        File.Move(entry.RootPath + path + "\\" + ee.File.FileName, entry.RootPath + path + "\\" + h.Data.FileName);
                                    }
                                }
                            }
                        }*/
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception e)
            {

            }
        }

        public void insertFullPath(Entry entry, History h)
        {            
            h.Data.FullPathStr = makePath(entry, h);            
        }

        public void moveFile(Entry entry, History h)
        {
            string[] result = makePathToken(entry, h);
            h.Data.FullPathStr = entry.RootPath;
            EntryElement targetFolder = entry.Root;
        }

        public void deleteFIle(Entry entry, History h)
        {
            
            EntryElement targetFolder = findParentFolder(entry.Root, h);
            if (targetFolder == null)
                targetFolder = entry.Root;

            EntryElement targetFile = (EntryElement)targetFolder.NamedChildren[h.Data.FileName];
            deleteTarget(entry, targetFolder, targetFile);

            if (h.FType == HistoryFileType.File)
            {                    
                File.Delete(entry.RootPath + h.Data.FullPathStr + "\\" + h.Data.FileName);
            }
            else
            {
                
                Directory.Delete(entry.RootPath + h.Data.FullPathStr + "\\" + h.Data.FileName, true);
            }
        }

        public void deleteTarget(Entry entry, EntryElement parent, EntryElement target)
        {
            if (target.Type == EntryType.Folder)
            {
                EntryElement[] children = target.Children.ToArray(typeof(EntryElement)) as EntryElement[];
                foreach (EntryElement e in children)
                {
                    deleteTarget(entry, target, e);
                }                
                parent.Children.Remove(target);
                parent.NamedChildren.Remove(target.File.FileName);                
            }
            else if (target.Type == EntryType.File)
            {                
                parent.Children.Remove(target);
                parent.NamedChildren.Remove(target.File.FileName);
                deleteFileFromMeta(entry, target);
            }
        }

        public void deleteFileFromMeta(Entry entry, EntryElement target)
        {

        }
        public EntryElement findTarget(EntryElement root, History h)
        {
            if (root == null)
            {
                return null;
            }
            EntryElement[] entrylist = root.Children.ToArray(typeof(EntryElement)) as EntryElement[];
            foreach (EntryElement e in entrylist)
            {
                if (e.File.FileId == h.Data.FileId)
                    return e;
                else
                {
                    if (h.FType == HistoryFileType.Folder)
                    {
                        EntryElement result = findParentFolder(e, h);

                        if (result != null)
                            return result;
                    }
                }
            }
            return null;
        }

        public EntryElement[] findParent(EntryElement root, History h)
        {
            if (root == null)
            {
                return null;
            }
            EntryElement[] entrylist = root.Children.ToArray(typeof(EntryElement)) as EntryElement[];
            if (entrylist != null)
            {
                foreach (EntryElement e in entrylist)
                {
                    if (e.File.FileId == h.Data.Parent)
                        return new EntryElement[] { e };
                    else
                    {
                        if (h.FType == HistoryFileType.Folder)
                        {
                            EntryElement[] el = findParent(e, h);

                            if (el != null)
                            {
                                ArrayList nn = new ArrayList(el);
                                nn.Add(e);
                                return nn.ToArray(typeof(EntryElement)) as EntryElement[];
                            }
                        }
                    }
                }
            }
            return null;
        }

        public EntryElement findParentFolder(EntryElement root, History h)
        {
            if (root == null)
            {
                return null;
            }
            EntryElement[] entrylist = root.Children.ToArray(typeof(EntryElement)) as EntryElement[];
            foreach (EntryElement e in entrylist)
            {
                if (e.File.FileId == h.Data.Parent)
                    return e;
                else
                {
                    if (h.FType == HistoryFileType.Folder)
                    {
                        EntryElement result = findParentFolder(e, h);

                        if (result != null)
                            return result;
                    }
                }
            }
            return null;
        }

        public string makePath(Entry entry, History h)
        {
            /*int current = h.Data.Parent;            
            string result = "";
            while (((FileData)LinearList[current]).Parent != 0)
            {                
                FileData cFile = (FileData)LinearList[current];
                result = "\\" + cFile.FileName + result;
                current = cFile.Parent;
            }            
            return result;*/
            EntryElement[] pathToken = findParent(entry.Root, h);
            string result = "";
            if (pathToken != null)
            {
                for (int i = pathToken.Length - 1; i >= 0; i--)
                {
                    result = "\\" + pathToken[i].File.FileName;
                }
            }
            return result;
        }

        public string[] makePathToken(Entry entry, History h)
        {
            Queue<string> p = new Queue<string>();
            EntryElement[] pathToken = findParent(entry.Root, h);
            if (pathToken != null)
            {
                for (int i = pathToken.Length - 1; i >= 0; i--)
                {
                    p.Enqueue(pathToken[i].File.FileName);
                }
                string[] result = p.ToArray();
                return result;
            }
            else
                return null;

            
        }

        public void makeFolder(string root, Entry entry, History h)
        {
            string rpath = makePath(entry, h);
            string fullPath = root + rpath + "\\" + h.Data.FileName; ;
            string[] path = makePathToken(entry, h);
            EntryElement newEntryElement;
            EntryElement targetFolder = entry.Root;
            if (path != null)
            {
                for (int i = 0; i < path.Length; i++)
                {
                    targetFolder = (EntryElement)targetFolder.NamedChildren[path[i]];
                }                
            }

            if (!targetFolder.NamedChildren.ContainsKey(h.Data.FileName))
            {

                h.Data.FullPathStr = rpath + "\\" + h.Data.FileName;
                newEntryElement = new EntryElement
                {
                    File = h.Data,
                    Type = EntryType.Folder
                };

                targetFolder.Children.Add(newEntryElement);
                targetFolder.NamedChildren.Add(h.Data.FileName, newEntryElement);

                System.IO.Directory.CreateDirectory(fullPath);
            }
            else
            {
                if (h.Data.FileId != ((EntryElement)targetFolder.NamedChildren[h.Data.FileName]).File.FileId)
                {
                    string prvFileName = h.Data.FileName;
                    h.Data.FileName = h.Data.FileName + "_";
                    h.Data.FullPathStr = rpath + "\\" + h.Data.FileName;
                    fullPath = root + rpath + "\\" + h.Data.FileName ;

                    System.IO.Directory.CreateDirectory(fullPath);

                    newEntryElement = new EntryElement
                    {
                        File = h.Data,
                        Type = EntryType.Folder
                    };

                    targetFolder.Children.Add(newEntryElement);
                    targetFolder.NamedChildren.Add(h.Data.FileName, newEntryElement);
                    string date = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
                    entry.HistoryQueue.Enqueue(new History(HistoryType.Rename, h.Data, rpath + "\\" + prvFileName,date, false));

                }
            }
        }

        public void downloadFile(string uId, string Key, Entry entry, EntryElement e)
        {
            string req = string.Format("fileId={0}&userId={1}&clientKey={2}", e.FID, uId, Key);            
            HttpWebResponse reqResult = SendPOSTRequest("http://210.118.74.120:8080/cu/api/download", req, null, null, true);
            GetResponseContentToFile(reqResult,entry.RootPath + e.File.FullPathStr + "\\" + e.FILENAME);
        }

        public void renameFile(string uId, string Key, EntryElement e)
        {
            string req = string.Format("fileId={0}&userId={1}&clientKey={2}&changedName={3}", e.FID, uId, Key, e.FILENAME);
            HttpWebResponse reqResult = SendPOSTRequest("http://210.118.74.120:8080/cu/api/download", req, null, null, true);
            string result = GetResponseContent(reqResult);
        }

        public void fileDownload(string uId, string Key, int fileId, string root, Entry entry, History h)
        {
            string req = string.Format("fileId={0}&userId={1}&clientKey={2}", fileId, uId, Key);
            HttpWebResponse reqResult = SendPOSTRequest("http://210.118.74.120:8080/cu/api/download", req, null, null, true);
            string rpath = makePath(entry, h);
            string fullPath = root + rpath + "\\" + h.Data.FileName;
            string[] path = makePathToken(entry, h);
            EntryElement targetFolder = entry.Root;
            EntryElement newEntryElement;

            if (path != null)
            {
                for (int i = 0; i < path.Length; i++)
                {
                    targetFolder = (EntryElement)targetFolder.NamedChildren[path[i]];
                }
            }

            if (!targetFolder.NamedChildren.ContainsKey(h.Data.FileName))
            {
                h.Data.FullPathStr = rpath;

                GetResponseContentToFile(reqResult, fullPath);

                newEntryElement = new EntryElement
                {
                    File = h.Data,
                    Type = EntryType.File
                };

                targetFolder.Children.Add(newEntryElement);
                targetFolder.NamedChildren.Add(h.Data.FileName, newEntryElement);
            }
            else
            {
                if (h.Data.FileId != ((EntryElement)targetFolder.NamedChildren[h.Data.FileName]).File.FileId)
                {
                    string prvFileName = h.Data.FileName;
                    h.Data.FileName = h.Data.FileName + "_";
                    h.Data.FullPathStr = rpath + "\\" + h.Data.FileName;
                    fullPath = root + rpath + "\\" + h.Data.FileName;
                    
                    GetResponseContentToFile(reqResult, fullPath);

                    newEntryElement = new EntryElement
                    {
                        File = h.Data,
                        Type = EntryType.File
                    };

                    targetFolder.Children.Add(newEntryElement);
                    targetFolder.NamedChildren.Add(h.Data.FileName, newEntryElement);
                    string date = string.Format("{0:yyyy-MM-dd HH:mm:ss}", DateTime.Now);
                    entry.HistoryQueue.Enqueue(new History(HistoryType.Rename, h.Data, rpath + "\\" + prvFileName, date, false));
                }
            }
        }

        public void uploadSynk(string uId, string key, Entry entry)
        {
            foreach (History h in entry.HistoryQueue)
            {
                if (h.Type == HistoryType.Create)
                {
                    uploadFile(uId, key, entry, h.Data);
                }
                else if (h.Type == HistoryType.Delete)
                {

                }
                else if (h.Type == HistoryType.Change)
                {

                }
                else if (h.Type == HistoryType.Rename)
                {

                }
                else
                {

                }
            }
        }

        public int generateFolder(string uId, string Key, FileData f)
        {
            string req = string.Format("folderName={0}&userId={1}&clientKey={2}&parentId={3}", f.FileName, uId, Key, f.Parent);
            HttpWebResponse reqResult = SendPOSTRequest("http://210.118.74.120:8080/cu/api/newfolder", req, null, null, true);
            string result = GetResponseContent(reqResult);
            dynamic jsonStr = JsonConvert.DeserializeObject(result);
            string r = jsonStr["result"];

            if (r.Equals("true"))
            {
                return jsonStr["fileId"];
            }
            else
            {
                return -1;
            }
        }

        public int uploadFile(string uId, string Key, Entry entry, FileData f)
        {
            using (HttpClient client = new HttpClient())
            {

                using (MultipartFormDataContent content = new MultipartFormDataContent())
                {
                    string fileId = f.FileName;
                    KeyValuePair<string, string>[] values = new KeyValuePair<string, string>[] {
                        new KeyValuePair<string, string>("parentId", HttpUtility.UrlEncode(f.Parent.ToString())),
                        new KeyValuePair<string, string>("userId", HttpUtility.UrlEncode(uId)),
                        new KeyValuePair<string, string>("clientKey", HttpUtility.UrlEncode(Key))
                    };

                    foreach (var keyValuePair in values)
                    {
                        content.Add(new StringContent(keyValuePair.Value, Encoding.UTF8), keyValuePair.Key);
                    }

                    ByteArrayContent fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(entry.RootPath + f.FullPathStr  + "\\" + f.FileName));
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
                    {
                        Name = "file",
                        FileName = HttpUtility.UrlEncode(f.FileName)
                    };

                    content.Add(fileContent);
                    var requestUri = "http://210.118.74.120:8080/cu/api/upload";

                    //client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("multipart/form-data"));
                    //var result2 = client.PostAsync(requestUri, content).Result;
                    System.Net.Http.HttpResponseMessage response = client.PostAsync(requestUri, content).Result;
                    dynamic jsonStr = JsonConvert.DeserializeObject(response.Content.ReadAsStringAsync().Result);
                    string r = jsonStr["result"];

                    if (r.Equals("true"))
                    {
                        return jsonStr["fileId"];
                    }
                    else
                    {
                        return -1;
                    }
                }
            }
        }

        public void insertMeta(string uId, string Key, FileData f, string val)
        {
            string req = string.Format("fileId={0}&userId={1}&clientKey={2}&metaValue={3}", f.FileId, uId, Key, val);
            HttpWebResponse reqResult = SendPOSTRequest("http://210.118.74.120:8080/cu/api/metaInsert", req, null, null, true);
            string result = GetResponseContent(reqResult);
            dynamic jsonStr = JsonConvert.DeserializeObject(result);
            string r = jsonStr["result"];
        }
    }
}
