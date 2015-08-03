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
  
    public partial class MainWindow : MetroWindow
    {
        ObservableCollection<FileData> myFilesList2 = new ObservableCollection<FileData>();
        //ContentManager2 cm2 = new ContentManager2();

        public void displayCategory(ContentManager.FileData[] cm)
        {
            try
            {
                myFilesList2.Clear();
                AllFileList2.ItemsSource = null;
                AllFileList2.Items.Clear();

                //테스트를 위해 FileData 리스트 받아오기
                ContentManager.FileData[] fileDatas = cm;

                for (int i = 0; i < fileDatas.Length; i++)
                {
                    string filePath = defaultPath+ fileDatas[i].FullPathStr + "\\" + fileDatas[i].FileName;

                    if ((File.GetAttributes(filePath) & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        string fileName = filePath.Split('\\').Last();
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

                        myFilesList2.Add(new FileData { FileName = fileName, NickName = nickName, FileIcon = folderIcon, FullPathStr = filePath });
                    }
                    else
                    {
                        string fName = System.IO.Path.GetFileName(filePath);
                        FileToImageIconConverter some = new FileToImageIconConverter(filePath);
                        ImageSource imgSource = some.Icon;
                        string nName = "";
                        if (fName.Length > 25)
                        {
                            nName = fName.Substring(0, 22) + "..." + fName.Substring(fName.Length - 4, 4);
                        }
                        else
                        {
                            nName = fName;
                        }

                        myFilesList2.Add(new FileData { FileName = fName, NickName = nName, FileIcon = imgSource, FullPathStr = filePath });
                    }

                }
                AllFileList2.ItemsSource = myFilesList2;
            }
            catch (Exception e1)
            {
                MessageBox.Show(e1.Message);
            }
            
        }
        private void ItemRightClick2(object sender, ContextMenuEventArgs e)
        {
            ListView lv = sender as ListView;
            //선택된 아이템이 있을 경우

            Keys = this.entry.getMetaKeys();
            if (AllFileList2.SelectedItems.Count > 0)
            {
                ((MenuItem)lv.ContextMenu.Items[0]).IsEnabled = true;
                ((MenuItem)lv.ContextMenu.Items[1]).IsEnabled = true;

            }
            // 선택된 아이템이 없을 경우 (배경 우클릭)
            else
            {
                ((MenuItem)lv.ContextMenu.Items[0]).IsEnabled = false;
                ((MenuItem)lv.ContextMenu.Items[1]).IsEnabled = false;
            }
        }
        //삭제하기
        private async void ContextMenu_Delete2(object sender, RoutedEventArgs e)
        {
            int selectedItemCnt = AllFileList2.SelectedItems.Count;
            //타겟 파일을 클릭 했을 경우만 
            if (selectedItemCnt > 0)
            {
                try // FileNotFoundException
                {
                    for (int i = 0; i < selectedItemCnt; i++)
                    {
                        string path = ((FileData)AllFileList2.SelectedItems[0]).FullPathStr;

                        MessageDialogResult result = await this.ShowMessageAsync("삭제", "이 파일을 휴지통에 버리시겠습니까?", MessageDialogStyle.AffirmativeAndNegative);

                        if (result.Equals(MessageDialogResult.Affirmative))
                        {
                            //폴더라면
                            if ((File.GetAttributes(path) & FileAttributes.Directory) == FileAttributes.Directory)
                            {
                                // Delete a directory and all subdirectories with Directory static method
                                if (System.IO.Directory.Exists(path))
                                {
                                    try
                                    {
                                        System.IO.Directory.Delete(path, true);
                                        myFilesList2.Remove(((FileData)AllFileList2.SelectedItems[0]));
                                    }

                                    catch (System.IO.IOException exception)
                                    {
                                        Console.WriteLine(exception.Message);
                                    }
                                }
                            }
                            else//파일이라면 
                            {
                                if (System.IO.File.Exists(path))
                                {
                                    try
                                    {
                                        System.IO.File.Delete(path);
                                        Console.WriteLine(((FileData)AllFileList2.SelectedItems[0]));
                                        myFilesList2.Remove(((FileData)AllFileList2.SelectedItems[0]));
                                    }
                                    catch (Exception exception)
                                    {
                                        Console.WriteLine(exception.Message);
                                        return;
                                    }
                                }

                            }
                        }
                    }
                }
                catch (System.IO.IOException exception)
                {
                    System.Windows.MessageBox.Show(exception.Message);
                }

            }
        }
        //배경 클릭 했을 경우 아이템 선택 해제
        private void listViewBackgroundClick2(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (AllFileList2 != null)
            {
                AllFileList2.SelectedItem = null;
            }
        }

        //우클릭 태그 설정
        private void ContextMenu_TagSetting(object sender, MouseEventArgs e)
        {

        }   


    }
 
        
    //테스트(삭제)-------------------------------------------------
    public class ContentManager2
    {
        ArrayList arrayList;
        ContentManager.FileData[] f1;
        //생성자
        public ContentManager2()
        {
            arrayList = new ArrayList();
            f1 = new ContentManager.FileData[5];

            arrayList.Add("테스트");
        }

        public ContentManager.FileData[] addValue()
        {
            f1[0] = new ContentManager.FileData()
            {
                FileName = "(양식2)신청장비 리스트-복사본-복사본-복사본 - 복사본.xlsx",
                FullPathStr = @"C:\Users\Sohee\Desktop\Test\sss\sss - 복사본\(양식2)신청장비 리스트-복사본-복사본-복사본 - 복사본.xlsx",
                Metadata = arrayList
            };
            f1[1] = new ContentManager.FileData()
            {
                FileName = "d",
                FullPathStr = @"C:\Users\Sohee\Desktop\Test\sss\sss - 복사본\d",
                Metadata = arrayList
            };
            f1[2] = new ContentManager.FileData()
            {
                FileName = "c.txt",
                FullPathStr = @"C:\Users\Sohee\Desktop\Test\sss\sss - 복사본\c.txt",
                Metadata = arrayList
            };
            f1[3] = new ContentManager.FileData()
            {
                FileName = "b",
                FullPathStr = @"C:\Users\Sohee\Desktop\Test\sss\sss - 복사본\b.txt",
                Metadata = arrayList
            };
            f1[4] = new ContentManager.FileData()
            {
                FileName = "a",
                FullPathStr = @"C:\Users\Sohee\Desktop\Test\sss\sss - 복사본\a",
                Metadata = arrayList
            };

            return f1;
        }
        
    }//(삭제)-------------------------------------------------
}