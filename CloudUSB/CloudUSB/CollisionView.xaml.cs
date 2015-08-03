using MahApps.Metro.Controls;
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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ContentManager;
using System.Collections;

namespace CloudUSB
{
    /// <summary>
    /// CollisionView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CollisionView : MetroWindow
    {
        MainWindow mw;

        Hashtable serverTable;
        Hashtable localTable;

        Hashtable serverTableIdx;
        Hashtable localTableIdx;
        public CollisionView(MainWindow _mw)
        {
            mw = _mw;
            InitializeComponent();
            serverTable = new Hashtable();
            localTable = new Hashtable();
            serverTableIdx = new Hashtable();
            localTableIdx = new Hashtable();

            ServerFileListBox_List();
            LocalFileListBox_List();
        }

        private void ServerFileListBox_List()
        {
            ArrayList arrayList = stringRetrun();

            for (int i = 0; i < arrayList.Count; i++)
            {
                ListBoxItem itm = new ListBoxItem();
                itm.Content = arrayList[i].ToString();
                itm.Selected += ServerListBoxitem_Event;
                ServerFileListBox.Items.Add(itm);
                serverTable.Add(itm.Content, false);
                serverTableIdx.Add(itm.Content, i);
            }            
        }
        private void LocalFileListBox_List()
        {
            ArrayList arrayList = stringRetrun();
            for (int i = 0; i < arrayList.Count; i++)
            {
                ListBoxItem itm = new ListBoxItem();
                itm.Content = arrayList[i].ToString();
                itm.Selected += LocalListBoxitem_Event;
                LocalFileListBox.Items.Add(itm);
                localTable.Add(itm.Content, false);
                localTableIdx.Add(itm.Content, i);
            }
        }

        private void ServerListBoxitem_Event(object sender, RoutedEventArgs e)
        {
            string key = (((ListBoxItem)sender).Content).ToString();

            //local table insert

            //server table find 
            //if find . 
            if (localTable.ContainsKey(key))
            {
                if ((bool)localTable[key])
                {

                    int i = (int)localTableIdx[key];
                    int idxNum = 0;
                    System.Console.WriteLine("1 " +i);
                    foreach (ListBoxItem lb in LocalFileListBox.SelectedItems)
                    {
                        System.Console.WriteLine("1 " + idxNum);
                        if (((string)(lb.Content)).Equals(key))
                        {
                            System.Console.WriteLine("1 " + idxNum);
                            LocalFileListBox.SelectedItems.RemoveAt(idxNum);
                            break;
                        }
                        idxNum++;
                    }

                    LocalFileListBox.InvalidateVisual();
                    localTable[key] = false;
                }

            }
            serverTable[key] = true;



            //for (int i = 0; i < ServerFileListBox.Items.Count; i++)
            //{
            //    if (ServerFileListBox.Items[i] == ((ListBoxItem)sender))
            //    {
            //        if (LocalFileListBox.SelectedItems.Contains(LocalFileListBox.Items[i]))
            //            LocalFileListBox.SelectedItems.RemoveAt(i);

            //        break;
            //    }
            //    //MessageBox.Show(i.ToString());
            //}
            
            //((ListBoxItem)sender)
        }

        private void LocalListBoxitem_Event(object sender, RoutedEventArgs e)
        {
            string key = (((ListBoxItem)sender).Content).ToString();

            if (serverTable.ContainsKey(key))
            {
                if ((bool)serverTable[key])
                {
                    
                    int i = (int)serverTableIdx[key];
                    int idxNum = 0;
                    foreach (ListBoxItem lb in ServerFileListBox.SelectedItems)
                    {
                        if(((string)(lb.Content)).Equals(key))
                        {
                            ServerFileListBox.SelectedItems.RemoveAt(idxNum);       
                            break;
                        }
                        idxNum++;
                    }
                   
                    LocalFileListBox.InvalidateVisual();
                    serverTable[key] = false;
                }

            }
            localTable[key] = true;
            //if (LocalFileListBox.SelectedIndex > -1)
             //MessageBox.Show(LocalFileListBox.SelectedIndex + "~");
            //for (int i = 0; i < LocalFileListBox.Items.Count; i++)
            //{
            //    if (LocalFileListBox.Items[i] == ((ListBoxItem)sender))
            //    {
            //        if (ServerFileListBox.SelectedItems.Contains(ServerFileListBox.Items[i]))
            //            ServerFileListBox.SelectedItems.RemoveAt(i);
                   
            //         break;
            //    }
            //    //MessageBox.Show(i.ToString());
            //}
        }

        private void SyncOKbutton_Click(object sender, RoutedEventArgs e)
        {
            //최신 목록을 만들어 ㄱㄱ
            mw.stop();
            Entry currentEntry = new Entry();
            currentEntry.setRoot(mw.defaultPath);
            currentEntry.buildEntry();

            currentEntry.Root.File.FileId = mw.entry.Root.File.FileId;

            Entry.compareEntry(mw.entry, currentEntry);

            foreach (DictionaryEntry k in Entry.metaTable)
            {
                if (!currentEntry.Meta.Contains(k.Key))
                    currentEntry.Meta.Add(k.Key, k.Value);
            }
            mw.Request.syncStart(mw.Request.userID, mw.Request.aKey);
            ContentManager.EntryElement[] fileList = mw.Request.getAllFileList(mw.userID, mw.userToken);
            Entry entry2 = Entry.buildEntryFromFileList(currentEntry, fileList);

            Entry.mergeEntry(entry2, currentEntry, mw.Request);
            mw.Request.syncEnd(mw.Request.userID, mw.Request.aKey);
            mw.entry = currentEntry;
            mw.play();
            this.Close();
            //((EntryElement)main.entry.Root.Children[0]).File.Parent = entry2.Root.FID;
            //main.Request.uploadFile(main.userID, main.userToken, main.entry, ((EntryElement)main.entry.Root.Children[0]).File);
        }
        

        //테스트를 위한 함수
        public ArrayList stringRetrun()
        {
            ArrayList strArr = new ArrayList();
            for (int i = 0; i < 40; i++)
            {
                strArr.Add(i + "번째 데이터");
            }
            return strArr;
        }

        private void Sync_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        //private void ServerFileListBox_Loaded(object sender, RoutedEventArgs e)
        //{
        //    ServerFileListBox.ItemContainerStyle.Setters.Add(new EventSetter()
        //    {
        //        Event = MouseDownEvent,
        //        Handler = new MouseButtonEventHandler(ServerListBoxitem_Event)
        //    });
            
        //    ServerFileListBox_List();
        //   // ServerFileListBox.SelectAll();
            
        //}

        //private void LocalFileListBox_Loaded(object sender, RoutedEventArgs e)
        //{
        //    LocalFileListBox.ItemContainerStyle.Setters.Add(new EventSetter()
        //    {
        //        Event = MouseDownEvent,
        //        Handler = new MouseButtonEventHandler(LocalListBoxitem_Event)
        //    });
        //    LocalFileListBox_List();
        //}
    }
}
