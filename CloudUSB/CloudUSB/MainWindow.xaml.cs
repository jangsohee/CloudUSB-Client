using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using System.IO;
using MahApps.Metro.Controls;
using System.Windows.Controls.Primitives;
using ContentManager;
using System.Windows.Input;
using MahApps.Metro.Controls.Dialogs;
using System.Net;
using System.Web;



namespace CloudUSB
{
    /// <summary>
    /// MainWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        ObservableCollection<FileData> myFilesList = new ObservableCollection<FileData>();
        //public string defaultPath = "C:\\Users\\Sohee\\Desktop\\Test"; 디폴트경로작업중
        public string defaultPath = "";
        public bool isLogin = false; //로그인<->로그아웃 버튼 변경을 위한 flag
        public bool defaultModeFlag = true;
        public bool isCategory_TagListLoaded = false;
        string nowPath = "";

        //동기화버튼 호출에 사용하기 위한
        public string userID = "";
        public string userToken = "";

        ImageSource folderIcon;
        public Entry entry;
        public RequestManager Request;
        FileSystemWatcher watcher;

        public static bool IsWindowOpened { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            //folder icon init
            ImageSourceConverter c = new ImageSourceConverter();
            folderIcon = (ImageSource)c.ConvertFrom("pack://application:,,,/img/folderImage.png");
            entry = new Entry();
            //Request = new RequestManager();
            //watcher = new FileSystemWatcher();
        }

        public void run()
        {
            
            watcher.Path = defaultPath;
            watcher.NotifyFilter = NotifyFilters.Attributes | NotifyFilters.CreationTime | NotifyFilters.FileName | NotifyFilters.DirectoryName;
            watcher.Filter = "*.*";

            watcher.Changed += new FileSystemEventHandler(onFileChanged);
            watcher.Created += new FileSystemEventHandler(onFileChanged);
            watcher.Deleted += new FileSystemEventHandler(onFileChanged);
            //watcher.Renamed += new RenamedEventHandler(onFileRenamed);
            watcher.IncludeSubdirectories = true;
            watcher.EnableRaisingEvents = true;
        }

        public void play()
        {
            watcher.EnableRaisingEvents = false;
        }

        public void stop()
        {
            watcher.EnableRaisingEvents = false;
        }

        public void onFileChanged(object source, FileSystemEventArgs e)
        {
            
            string[] rpath = defaultPath.Split('\\');
            string[] bpath = e.FullPath.Split('\\');
            string[] path = null;
                        
            string resultPath = "";

            if (rpath.Length - bpath.Length > 1)
            {
                path = new string[rpath.Length - bpath.Length - 1];
                for (int i = rpath.Length; i < bpath.Length - 1; i++)
                {
                    path[i] = bpath[i];
                    resultPath = resultPath + "\\" + bpath[i];
                }               
            }           

            

            if (e.ChangeType == WatcherChangeTypes.Created)
            {
                
            }
            else if (e.ChangeType == WatcherChangeTypes.Deleted)
            {
                MessageBox.Show("asdfaf");
            }
            else if (e.ChangeType == WatcherChangeTypes.Changed)
            {
                
            }
        }


        //private async void initLogin()
        //{ 
        //    //로그인창 띄우기
        //    if (isLogin == false)
        //    {
        //        try
        //        {
        //            LoginDialogData result = await this.ShowLoginAsync("로그인", "아이디와 비밀번호를 입력해주세요",
        //            new LoginDialogSettings
        //            {
        //                ColorScheme = MetroDialogColorScheme.Accented,
        //                UsernameWatermark = "ID",
        //                PasswordWatermark = "Password",
        //                NegativeButtonVisibility = Visibility.Visible,
        //               // NegativeButtonText = join
        //            });
        //        }
        //        catch (InvalidOperationException eee)
        //        {
        //            MessageBox.Show(eee.Message);
        //        }
        //    }
        //    else
        //    {
        //        isLogin = false;
        //        LoginBtn.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/img/login.png", UriKind.Absolute)));

        //        //테스트
        //        MessageBox.Show("로그아웃");

        //    }
            
        //}
        //GroupBox Refresh
        public void Refresh(string FullPath)
        {
            BackPath(FullPath);

            if (defaultPath.Equals(nowPath) == true)
            {
                backButton.Opacity = 0;
            }
            else if (defaultPath.Equals(nowPath) == false)
            {
                backButton.Opacity = 1;
            }

            string[] rtoken = defaultPath.Split('\\');
            string[] ftoken = FullPath.Split('\\');

            if (entry.Root != null)
            {
                EntryElement target = entry.Root;

                for (int i = rtoken.Length; i < ftoken.Length; i++)
                {
                    target = (EntryElement)target.NamedChildren[ftoken[i]];
                }

                entry.CurrentRoot = target;
            }
            //entry.moveDirectory(); //뒤로가기버튼 클릭 시, 널참조 에러발생

            string[] directoryEntries = System.IO.Directory.GetFileSystemEntries(FullPath, "*", System.IO.SearchOption.TopDirectoryOnly);
            myFilesList.Clear(); //myFileList를 안비워줘서 문제생겼음


            AllFileList.ItemsSource = null;
            AllFileList.Items.Clear();

            //Refresh()를 category에서도 사용할 때
            //if (defaultModeFlag)
            //{
            //    AllFileList.ItemsSource = null;
            //    AllFileList.Items.Clear();
            //}
            //else
            //{
            //    AllFileList2.ItemsSource = null;
            //    AllFileList2.Items.Clear();
            //}
            
            
            
            int cnt = directoryEntries.Length;
            for(int i=0; i< cnt; i++) //(string filePath in directoryEntries)
            {
                string filePath = directoryEntries[i];
                //Folder
                if ((File.GetAttributes(filePath) & FileAttributes.Directory) == FileAttributes.Directory)
                {
                    string fileName = filePath.Split('\\').Last();//filePath.Replace(System.IO.Path.GetDirectoryName(filePath) + "\\", "");
                    string nickName = "";
                   
                    //파일명 줄임표
                    if (fileName.Length > 25)
                    {
                        nickName = fileName.Substring(0, 22) + "...";
                    }
                    else 
                    {
                        nickName = fileName;
                    }
                    myFilesList.Add(new FileData { FileName = fileName, NickName = nickName, FileIcon = folderIcon, FullPathStr = filePath });
                }
                //File
                else
                {
                    string fName = System.IO.Path.GetFileName(filePath);
                    FileToImageIconConverter some = new FileToImageIconConverter(filePath);
                    ImageSource imgSource = some.Icon;
                    string nName = "";
                    if (fName.Length > 25) {
                        nName = fName.Substring(0, 18) + "..." + fName.Substring(fName.Length-4,4);
                    }
                    else
                    {
                        nName = fileName;
                    }
                    
                    myFilesList.Add(new FileData { FileName = fName, NickName=nName, FileIcon = imgSource, FullPathStr = filePath });
                }
            }
            
            AllFileList.ItemsSource = myFilesList;
            //if (defaultModeFlag)
            //{
            //    AllFileList.ItemsSource = myFilesList;
            //}
            //else 
            //{
            //    AllFileList2.ItemsSource = myFilesList;
            //} 
             
        }

        //경로 표시
        private void BackPath(string path)
        {
            nowPath = path;
            GroupBox1.Header = path;
        }

        //로그인 버튼 클릭시
        private void Login_Click(object sender, RoutedEventArgs e)
        {
           // initLogin();
            if (isLogin == false)
            {
                LoginView loginView = new LoginView(this);
                this.Opacity = 0.3;
                loginView.ShowDialog();
            }
            else
            {
                isLogin = false;
                LoginBtn.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/img/login.png", UriKind.Absolute)));

                //테스트
                MessageBox.Show("로그아웃");

            }
            
        }
        private void settingBtn_Click(object sender, RoutedEventArgs e)
        {
            SettingView settingView = new SettingView(this);
            this.Opacity = 0.3;
            settingView.ShowDialog();
        }
        
        
        private void AllFileList_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try {
                if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed && this.AllFileList.SelectedItems.Count != 0)
                {
                    string path = ((FileData)AllFileList.SelectedItems[0]).FullPathStr;

                    //Folder
                    if ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        //System.Diagnostics.Process.Start(path); //디렉토리 변경이 아닌 폴더 자체 열기
                        try
                        {
                            Refresh(path);
                        }
                        catch (UnauthorizedAccessException eee)
                        {
                            MessageBox.Show(eee.Message);
                        }
                    }
                    else //File
                    {
                        System.Diagnostics.Process.Start(path);
                    }
                }
            }catch(FileNotFoundException eee)
            {
                MessageBox.Show(eee.Message);
            }
            
        }


        private void AllFileList2_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed && this.AllFileList2.SelectedItems.Count != 0)
                {
                    string path = ((FileData)AllFileList2.SelectedItems[0]).FullPathStr;

                    System.Diagnostics.Process.Start(path);
                    
                }
            }
            catch (FileNotFoundException eee)
            {
                MessageBox.Show(eee.Message);
            }

        }

            
        private void toggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton btn = sender as ToggleButton;

            if (defaultModeFlag)
            {
                ImageBrush imgBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/img/on.png", UriKind.Absolute)));
                btn.Background = imgBrush;
                defaultModeFlag = false;

                AllFileList2.ItemsSource = null;
                AllFileList2.Items.Clear();
                //Category_TaglistBox.ItemsSource = this.e.Meta.getKeys();
               
                //Loaded문제 
                //string[] key = entry.getMetaKeys();
                //string selectedTag = (string)Category_TaglistBox.SelectedItem;
                //if (key.Length > 0 && Category_TaglistBox.SelectedIndex >= 0)
                //    displayCategory((((ArrayList)entry.Meta[selectedTag]).ToArray(typeof(ContentManager.FileData))
                //        as ContentManager.FileData[]));

                //displayCategory(cm2);
                //---
            }
            else {
                ImageBrush imgBrush = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/img/off.png", UriKind.Absolute)));
                btn.Background = imgBrush;
                defaultModeFlag = true;

                
                
            }
        }

        private void Back_Button_Click(object sender, RoutedEventArgs e)
        {
            if (defaultPath.Equals(nowPath) == false)
            {
                string backPathStr = "";
                string[] arr = nowPath.Split('\\');
                int arrSize = arr.Length;

                if (arrSize > 1 && arr[arrSize - 2].Length > 0)
                {
                    for (int i = 0; i < arrSize - 2; i++)
                    {
                        backPathStr += arr[i];
                        backPathStr += '\\';
                    }

                    backPathStr += arr[arrSize - 2];

                    if (arrSize - 2 == 0)
                        backPathStr += '\\';

                    this.Refresh(backPathStr);
                }
            }
            
        }

        string[] Keys = null;
        string clipboardFileName = "";
        string clipboardFullPath = "";
        bool isClipboardData = false;

        //붙여넣기 할 때, 클립보드 Cut/Copy 구별을 위한 flag
        bool cutClipboard = false;
        bool copyClipboard = false;

        //마우스 우클릭
        private void ItemRightClick(object sender, ContextMenuEventArgs e)
        {
            isClipboardData = Clipboard.ContainsData(DataFormats.FileDrop);
            if (isClipboardData)
            {
                string[] getDatas = (string[])Clipboard.GetData(DataFormats.FileDrop);
                clipboardFileName = getDatas[0].Split('\\').Last();
                clipboardFullPath = getDatas[0];
                cFilePath = getDatas[0];

            }

            ListView lv = sender as ListView;
            //선택된 아이템이 있을 경우
            
            //Keys = this.e.Meta.getKeys();
            if (AllFileList.SelectedItems.Count > 0)
            {
                if (cutFlag == true || copyFlag == true || isClipboardData == true)
                {
                    //붙여넣기 보여줌 
                    ((MenuItem)lv.ContextMenu.Items[0]).IsEnabled = true;
                    ((MenuItem)lv.ContextMenu.Items[1]).IsEnabled = true;
                    ((MenuItem)lv.ContextMenu.Items[2]).IsEnabled = true;
                    ((MenuItem)lv.ContextMenu.Items[3]).IsEnabled = true;
                    ((MenuItem)lv.ContextMenu.Items[4]).IsEnabled = true;
                    ((MenuItem)lv.ContextMenu.Items[5]).IsEnabled = true;

                }
                else
                {
                    ((MenuItem)lv.ContextMenu.Items[0]).IsEnabled = true;
                    ((MenuItem)lv.ContextMenu.Items[1]).IsEnabled = true;
                    ((MenuItem)lv.ContextMenu.Items[2]).IsEnabled = false;
                    ((MenuItem)lv.ContextMenu.Items[3]).IsEnabled = true;
                    ((MenuItem)lv.ContextMenu.Items[4]).IsEnabled = true;
                    ((MenuItem)lv.ContextMenu.Items[5]).IsEnabled = true;

                }
            }
            // 선택된 아이템이 없을 경우 (배경 우클릭)
            else
            {
                //복사 또는 잘라내기를 실행했다면
                if (cutFlag == true || copyFlag == true || isClipboardData == true)
                {
                    //붙여넣기 보여줌                     
                    ((MenuItem)lv.ContextMenu.Items[0]).IsEnabled = false;
                    ((MenuItem)lv.ContextMenu.Items[1]).IsEnabled = false;
                    ((MenuItem)lv.ContextMenu.Items[2]).IsEnabled = true;
                    ((MenuItem)lv.ContextMenu.Items[3]).IsEnabled = false;
                    ((MenuItem)lv.ContextMenu.Items[4]).IsEnabled = false;
                    ((MenuItem)lv.ContextMenu.Items[5]).IsEnabled = false;

                }
                else
                {
                    ((MenuItem)lv.ContextMenu.Items[0]).IsEnabled = false;
                    ((MenuItem)lv.ContextMenu.Items[1]).IsEnabled = false;
                    ((MenuItem)lv.ContextMenu.Items[2]).IsEnabled = false;
                    ((MenuItem)lv.ContextMenu.Items[3]).IsEnabled = false;
                    ((MenuItem)lv.ContextMenu.Items[4]).IsEnabled = false;
                    ((MenuItem)lv.ContextMenu.Items[5]).IsEnabled = false;
                }
            }
        }
        //배경 클릭 했을 경우 아이템 선택 해제
        private void listViewBackgroundClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (AllFileList != null)
            {
                AllFileList.SelectedItem = null;
            }
        }

        private void Category_TaglistBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }

        //Drag and drop
        private async void fileCopyPaste(string[] dragFilePath)
        {
            try
            {
                for (int i = 0; i < dragFilePath.Length; i++)
                {
                    string desPath = nowPath + "\\" + System.IO.Path.GetFileName(dragFilePath[i]);
                    //폴더라면
                    if ((File.GetAttributes(dragFilePath[i]) & FileAttributes.Directory) == FileAttributes.Directory)
                    {                        
                        //타겟대상이 원본대상의 하위일때
                        if (targetCheck(dragFilePath[i], desPath))
                        {
                            await this.ShowMessageAsync("중단된 작업", "대상 폴더가 원본 폴더의 하위 폴더입니다");
                        }
                        else
                        {
                            if (Directory.Exists(desPath))
                            {
                                MessageDialogResult result =
                                    await this.ShowMessageAsync
                                    ("파일이 이미 존재합니다", "덮어쓰시겠습니까?",
                                    MessageDialogStyle.AffirmativeAndNegative);

                                if (result.Equals(MessageDialogResult.Affirmative))
                                {
                                    System.IO.Directory.Delete(desPath, true); 
                                    System.IO.Directory.Move(dragFilePath[i], desPath);
                                }
                            }
                            else
                            {
                                System.IO.Directory.Move(dragFilePath[i], desPath);
                            }
                        } 
                    }
                    else//파일이라면 
                    {
                        if (File.Exists(desPath))
                        {
                            MessageDialogResult result = await this.ShowMessageAsync("파일이 이미 존재합니다", "덮어쓰시겠습니까?", MessageDialogStyle.AffirmativeAndNegative);

                            if (result.Equals(MessageDialogResult.Affirmative))
                            {
                                System.IO.File.Delete(desPath); 
                                System.IO.File.Move(dragFilePath[i], desPath);
                              
                            }
                        }
                        else
                        {
                            System.IO.File.Move(dragFilePath[i], desPath);
                        }
                    }
                }
            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.Message);
            }
            finally
            {
                Refresh(nowPath);
            }
        }
        private void fileDropEvent(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                string desPath = nowPath + "\\" + System.IO.Path.GetFileName(files[0]);

                if (desPath.Equals(files[0]) == false)
                {
                    fileCopyPaste(files);
                }
               


            } 
        }

        private Point start; 
        private void AllFileList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.start = e.GetPosition(null);  
        }

        private void AllFileList_MouseMove(object sender, MouseEventArgs e)
        {
            Point mpos = e.GetPosition(null);
            Vector diff = this.start - mpos;

            if (e.LeftButton == MouseButtonState.Pressed &&
                Math.Abs(diff.X) > SystemParameters.MinimumHorizontalDragDistance &&
                Math.Abs(diff.Y) > SystemParameters.MinimumVerticalDragDistance)
            {
                if (this.AllFileList.SelectedItems.Count == 0)
                {
                    return;
                }

                string dataFormat = DataFormats.FileDrop;
                string[] files = new[] { ((FileData)AllFileList.SelectedItems[0]).FullPathStr };
                DataObject dataObject = new DataObject(dataFormat, files);
                DragDrop.DoDragDrop(this.AllFileList, dataObject, DragDropEffects.Copy);
            }
        }

        private void syncBtn_click(object sender, RoutedEventArgs e)
        {
            if (isLogin)
            {
                CollisionView cv = new CollisionView(this);
                cv.ShowDialog();
                //동기화 모듈 호출 부분
                //호출 시, 유저아이디와 토큰 넘겨주기
                Refresh(nowPath);
            }
            else
            {
                MessageBox.Show("Login이 필요합니다!");
            }
        }
        public bool initLoded = false;
        public bool initSuccess = false;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (File.Exists(@"./SaveDefaultPath.txt"))
            {
                initLoded = true;
                initSuccess = true;

                
                defaultPath = File.ReadAllText(@"./SaveDefaultPath.txt");
                //run();
                Refresh(defaultPath);
            }
            else
            {
                SettingView settingView = new SettingView(this);
                settingView.ShowDialog();  
            }               
          
        }

        private void Window_Rendered(object sender, EventArgs e)
        {
            try
            {
                if (initSuccess == false)
                {
                    this.Close();
                }
                else 
                {
                    entry.setRoot(this.defaultPath);
                    entry.buildEntry();
                    Entry entry1 = null;
                    if (File.Exists("EntryJson.dat"))
                    {
                        string json = File.ReadAllText("EntryJson.dat");
                        entry1 = Entry.toEntry(json);
                    }

                    if (entry1 != null)
                    {
                        Entry.compareEntry(entry1, entry);

                        foreach (DictionaryEntry k in Entry.metaTable)
                        {
                            if (!entry.Meta.Contains(k.Key))
                                entry.Meta.Add(k.Key, k.Value);
                        }
                        //entry.Meta = Entry.metaTable;                        
                    }
                    //Lodded문제
                    //Category_TaglistBox.ItemsSource = entry.getMetaKeys();

                    //for (int i = 0; i < Category_TaglistBox.Items.Count; i++)
                    //{
                    //    ListBoxItem targetItem = (ListBoxItem) Category_TaglistBox.ItemContainerGenerator.ContainerFromItem(Category_TaglistBox.Items[i]);
                    //    targetItem.MouseDoubleClick += new MouseButtonEventHandler( ListBoxItem_MouseDoubleClick);
                    //}

                        //foreach (ListBoxItem i in Category_TaglistBox.Items)
                        //{
                        //    i.MouseDoubleClick += ListBoxItem_MouseDoubleClick;
                        //}
                    //Category_TaglistBox.Items += ListBoxItem_MouseDoubleClick;

                    string[] key = entry.getMetaKeys();
                    if(key.Length > 0)
                        displayCategory((((ArrayList)entry.Meta[key[0]]).ToArray(typeof(ContentManager.FileData)) 
                            as ContentManager.FileData[]));
                    this.WindowState = WindowState.Normal;
                }
            }
            catch (System.ComponentModel.Win32Exception eee)
            {
                Console.WriteLine(eee.Message);
            }
        }

        private void Category_TaglistBox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Category_TaglistBox.ItemContainerStyle.Setters.Add(new EventSetter()
                {
                    Event = MouseDoubleClickEvent,
                    Handler = new MouseButtonEventHandler(ListBoxItem_MouseDoubleClick)
                });
            }
            catch (Exception eee)
            {
                Console.WriteLine(eee.Message);
            }
           

            Category_TaglistBox.ItemsSource = entry.getMetaKeys();

            string[] key = entry.getMetaKeys();
            string selectedTag = (string)Category_TaglistBox.SelectedItem;
            if (key.Length > 0 && Category_TaglistBox.SelectedIndex >= 0) {            
            
                displayCategory((((ArrayList)entry.Meta[selectedTag]).ToArray(typeof(ContentManager.FileData))
                    as ContentManager.FileData[]));

                
            }
            Category_TaglistBox.ItemsSource = entry.getMetaKeys();
            isCategory_TagListLoaded = true;

            
        }

        private void Root_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            File.WriteAllText("EntryJson.dat", entry.toJSON());

        }
        private void ListBoxItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            
                displayCategory((((ArrayList)entry.Meta[Category_TaglistBox.SelectedItem]).ToArray(typeof(ContentManager.FileData))
                            as ContentManager.FileData[]));
          
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            HistoryView historView = new HistoryView(this);
            this.Opacity = 0.3;
            historView.ShowDialog();
        }


        private void ContextMenu_TagList(object sender, RoutedEventArgs e)
        {
            MenuItem cm = (MenuItem)sender as MenuItem;
            cm.Items.Clear();

            string[] Keys = this.entry.getMetaKeys();

            foreach (string k in Keys)
            {
                MenuItem newItem = new MenuItem();
                newItem.Header = k;
                newItem.Click += SubContextMenuClick;
                cm.Items.Add(newItem);
            }
        }


        
        //상대경로 추출
        //private string GetRelativePath(string _pullPath, string _defaultPath)
        //{
        //    string pullPathStr = _pullPath;
        //    string defaultPathStr = _defaultPath;

        //    return pullPathStr.Substring(0, defaultPathStr.Length);
        //}
       
    }

    //디폴트 디렉토리 셋팅
    public class MyPath
    {
        public string DefaultPath { get; set; }
    }

    //파일 타입
    public class FileData
    {
        public string NickName { get; set; }
        public string FileName { get; set; }
        public string FullPathStr { get; set; }
        public ImageSource FileIcon { get; set; }
    }    
  
    public class FileToImageIconConverter
    {
        private string filePath;
        private System.Windows.Media.ImageSource icon;

        public string FilePath { get { return filePath; } }
        public System.Windows.Media.ImageSource Icon
        {
            get
            {
                if (icon == null && System.IO.File.Exists(FilePath))
                {
                    using (System.Drawing.Icon sysicon = System.Drawing.Icon.ExtractAssociatedIcon(FilePath))
                    {
                        icon = System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
                                  sysicon.Handle,
                                  System.Windows.Int32Rect.Empty,
                                  System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());
                    }
                } 

                return icon;
            }
        }

        public FileToImageIconConverter(string filePath)
        {
            this.filePath = filePath;
        }
    }
}
