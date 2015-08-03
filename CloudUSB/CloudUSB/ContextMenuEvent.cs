using System;
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
using System.Collections;
using System.Collections.ObjectModel;
using System.IO;
using System.Drawing;
using System.Windows.Controls.Primitives;
using ContentManager;
using System.Windows.Input;
using System.Net;
using System.Web;
using System.Collections.Specialized;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace CloudUSB
{
    public partial class MainWindow : MetroWindow
    {
        
        Boolean cutFlag = false;
        Boolean copyFlag = false;
        
        string cFilePath = "";
  
        string fileName = "";

        string fileExistCheck(string destFile)
        {
            if (!System.IO.Path.GetExtension(destFile).Equals(""))
            {
                string[] splitStr = destFile.Split('.');
                int splitStrLeng = splitStr.Length;
                string targetPath = destFile;
                string copyStr = "";

                for (int i = 0; i < splitStr.Length - 2; i++)
                {
                    copyStr += splitStr[i];
                    copyStr += '.';
                }
                copyStr += splitStr[splitStrLeng - 2];
                copyStr += " - 복사본";
                string fileNamePlus = copyStr;
                copyStr += '.';
                copyStr += splitStr[splitStrLeng - 1]; //확장자 붙이기

                //"파일명 - 복사본"이 존재하지 않으면
                if (!(File.Exists(copyStr)))
                {
                    return copyStr; //" - 복사본" 붙이기
                }
                else
                {
                    int count = 1;
                    string returnPath = "";
                    do
                    {
                        returnPath = fileNamePlus + "(" + count + ").";
                        returnPath += splitStr[splitStrLeng - 1];
                        count++;

                    } while (File.Exists(returnPath));

                    return returnPath;
                }
            }
            else
            {
                string targetPath = destFile;
                string copyStr = destFile + " - 복사본";
                //"파일명 - 복사본"이 존재하지 않으면
                if (!(File.Exists(copyStr)))
                {
                    return copyStr; //" - 복사본" 붙이기
                }
                else
                {
                    int count = 1;
                    string returnPath = "";
                    do
                    {
                        returnPath = copyStr + "(" + count + ")";
                        count++;

                    } while (File.Exists(returnPath));

                    return returnPath;
                }
            }
            
                
                
        }
        string folderExistCheck(string destFolder)
        {
            string targetPath = destFolder;
            string copyStr = targetPath + " - 복사본";
                        
            //"폴더명 - 복사본"이 존재하지 않으면
            if (!(Directory.Exists(copyStr)))
            {
                return copyStr; //" - 복사본" 붙이기
            }
            else
            {
                int count = 1;
                string returnPath = "";
                do
                {
                    returnPath = copyStr + "(" + count + ")";
                    count++;

                } while(Directory.Exists(returnPath));

                return returnPath;
            }
        }
        //복사 모듈
        void CopyModule(string _tagetFile, string _originFullPath, string _nowPathRoot)
        {
            string targetFile = _tagetFile;
            string originFullPath = _originFullPath;
            string nowPathRoot = _nowPathRoot;
            
            try
            {
                if (Directory.Exists(originFullPath) || File.Exists(originFullPath))
                {
                    //선택 대상이 폴더라면
                    if ((File.GetAttributes(originFullPath) & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                    
                        //폴더 만들기
                        if (!(Directory.Exists(targetFile)))
                        {
                            Directory.CreateDirectory(targetFile);
                            nowPathRoot = targetFile;
                        }
                        else
                        {
                            targetFile = folderExistCheck(targetFile);
                            Directory.CreateDirectory(targetFile);
                            nowPathRoot = targetFile;
                        }

                        //폴더 안의 파일 만들기

                        string[] files = Directory.GetFileSystemEntries(originFullPath);

                        foreach (string s in files)
                        {
                            string subTargetFile = "";
                            string subOrigincFullPath = "";
                            string subNowPathRoot = "";
                            //폴더
                            if ((File.GetAttributes(s) & FileAttributes.Directory) == FileAttributes.Directory)
                            {
                                //재귀
                                subTargetFile = nowPathRoot + "\\" + System.IO.Path.GetFileName(s);
                                subOrigincFullPath = s;
                                subNowPathRoot = nowPathRoot;

                                CopyModule(subTargetFile, subOrigincFullPath, subNowPathRoot);
                            }
                            //파일
                            else
                            {
                                subTargetFile = nowPathRoot + "\\" + System.IO.Path.GetFileName(s);
                                subOrigincFullPath = s;
                                subNowPathRoot = nowPathRoot;

                                CopyModule(subTargetFile, subOrigincFullPath, subNowPathRoot);
                            }
                        }                                                            
                    }
                    //선택 대상이 파일이라면
                    else
                    {
                        //목적지에 파일이 존재하지 않으면 파일 이름 그대로 복사
                        if (!File.Exists(targetFile))
                        {
                            System.IO.File.Copy(originFullPath, targetFile);
                        }
                        //파일 존재하면, 복사본 붙이기
                        else
                        {
                            string temp = "";
                            temp += fileExistCheck(targetFile);
                            System.IO.File.Copy(originFullPath, temp);
                        }
                    }
                }
                
            }
            catch (Exception eee)
            {
                MessageBox.Show(eee.Message);
            }
            return;
        }
        bool targetCheck(string origin, string target)
        {
            if (!(target.Equals(origin)) && target.Contains(origin))
                return true;
            return false;
        }
        //붙여넣기
        private async void ContextMenu_Paste(object sender, RoutedEventArgs e)
        {
            string desFileName = "";
            if (isClipboardData == true)
            {
                desFileName = nowPath + "\\" + clipboardFileName;
            }
            else
            {
                desFileName = nowPath + "\\" + fileName;
            }

            if (copyFlag == true || (isClipboardData == true && copyClipboard == true))
            {
                //타겟대상이 원본대상의 하위일때
                if (targetCheck(cFilePath, desFileName))
                {
                    await this.ShowMessageAsync("중단된 작업", "대상 폴더가 원본 폴더의 하위 폴더입니다");
                }
                else
                {
                    //(string _tagetFile, string _originFullPath, string _nowPathRoot)
                    CopyModule(desFileName, cFilePath, nowPath);
                    Refresh(nowPath);                    
                }
                
                copyFlag = false;
                copyClipboard = false;

                clipboardFileName = "";
                clipboardFullPath = "";
            }
            else if (cutFlag == true || (isClipboardData == true && cutClipboard == true))
            {
                try
                {
                    //폴더라면
                    if ((File.GetAttributes(cFilePath) & FileAttributes.Directory) == FileAttributes.Directory)
                    {
                        //타겟대상이 원본대상의 하위일때
                        if (targetCheck(cFilePath, desFileName))
                        {
                            await this.ShowMessageAsync("중단된 작업", "대상 폴더가 원본 폴더의 하위 폴더입니다");
                        }
                        else
                        {
                            if (Directory.Exists(desFileName))
                            {
                                MessageDialogResult result =
                                    await this.ShowMessageAsync
                                    ("파일이 이미 존재합니다", "덮어쓰시겠습니까?",
                                    MessageDialogStyle.AffirmativeAndNegative);

                                if (result.Equals(MessageDialogResult.Affirmative))
                                {
                                    System.IO.Directory.Delete(desFileName, true);                             
                                    System.IO.Directory.Move(cFilePath, desFileName);
                                }
                            }
                            else
                            {
                                System.IO.Directory.Move(cFilePath, desFileName);
                            }
                        }                       
                    }
                    //파일이라면 
                    else
                    {
                        if (File.Exists(desFileName))
                        {
                            MessageDialogResult result = await this.ShowMessageAsync("파일이 이미 존재합니다", "덮어쓰시겠습니까?", MessageDialogStyle.AffirmativeAndNegative);

                            if (result.Equals(MessageDialogResult.Affirmative))
                            {
                                System.IO.File.Delete(desFileName); 
                                System.IO.File.Move(cFilePath, desFileName);
                            }                                
                        }
                        else
                        {
                            System.IO.File.Move(cFilePath, desFileName);
                        }
                    }
                }
                catch (Exception eee)
                {
                    MessageBox.Show(eee.Message);
                    //Console.WriteLine(eee.Message);
                }
                finally
                {
                    Refresh(nowPath);
                }
                cutFlag = false;
                cutClipboard = false;
                Clipboard.Clear(); //
                isClipboardData = false;//
            }
            cFilePath = "";
            //isClipboardData = false;
            //Clipboard.Clear();            
        }
        

        //잘라내기
        private void ContextMenu_Cut(object sender, RoutedEventArgs e)
        {
            int selectedItemCnt = AllFileList.SelectedItems.Count;
            //타겟 파일을 클릭 했을 경우만 
            if (selectedItemCnt > 0)
            {
                try // FileNotFoundException
                {
                    cFilePath = ((FileData)AllFileList.SelectedItems[0]).FullPathStr;
                    fileName = ((FileData)AllFileList.SelectedItems[0]).FileName;
                    cutFlag = true;

                    StringCollection FileCollection = new StringCollection();
                    FileCollection.Add(((FileData)AllFileList.SelectedItems[0]).FullPathStr);

                    //클립보드에 데이터 셋팅
                    Clipboard.SetFileDropList(FileCollection);
                    cutClipboard = true;
                }
                catch (FileNotFoundException eee)
                {
                    System.Windows.MessageBox.Show(eee.Message);
                }
            }
            
        }

        //복사하기
        private void ContextMenu_Copy(object sender, RoutedEventArgs e)
        {

            int selectedItemCnt = AllFileList.SelectedItems.Count;
            //타겟 파일을 클릭 했을 경우만 
            if (selectedItemCnt > 0)
            {
                try // FileNotFoundException
                {
                   // MenuItem mi = sender as MenuItem;
                    cFilePath = ((FileData)AllFileList.SelectedItems[0]).FullPathStr;
                    copyFlag = true;
                    fileName = ((FileData)AllFileList.SelectedItems[0]).FileName;
                    //mi.Visibility = System.Windows.Visibility.Collapsed;
                    
                    StringCollection FileCollection = new StringCollection();
                    FileCollection.Add(((FileData)AllFileList.SelectedItems[0]).FullPathStr);
                    
                    
                    //클립보드에 데이터 셋팅
                    Clipboard.SetFileDropList(FileCollection);
                    copyClipboard = true;
                }
                catch (FileNotFoundException eee)
                {
                    System.Windows.MessageBox.Show(eee.Message);
                }

            }
        }

        

        //이름 수정
        private async void ContextMenu_Rename(object sender, RoutedEventArgs e)
        {
            
            int selectedItemCnt = AllFileList.SelectedItems.Count;
            if (selectedItemCnt > 0)
            {
                try
                {
                    var result = await this.ShowInputAsync("이름 수정", "파일 이름을 입력해주세요", new MetroDialogSettings() { 
                        ColorScheme = MetroDialogColorScheme.Accented
                    });

                    if (result != null)
                    { 
                        if(result.Equals(""))
                            MessageBox.Show("파일 이름을 입력해야 합니다");
                        else
                        {
                            string fileDataPath = ((FileData)AllFileList.SelectedItems[0]).FullPathStr;
                            string newPathToFile = "";
                            if (!((File.GetAttributes(fileDataPath) & FileAttributes.Directory) == FileAttributes.Directory))
                            {
                                string[] splitStr = fileDataPath.Split('.');
                                string extension = splitStr[splitStr.Length - 1];
                                newPathToFile = System.IO.Path.Combine(nowPath, result + "." + extension);
                                File.Move(fileDataPath, newPathToFile);
                            }
                            else
                            {
                                newPathToFile = System.IO.Path.Combine(nowPath, result);
                                Directory.Move(fileDataPath, newPathToFile);
                            }
                            Refresh(nowPath);
                        }
                    }
                }
                catch (Exception eee)
                {
                    System.Windows.MessageBox.Show(eee.Message);
                }

            }
           // this.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Accented;//블루
            //this.MetroDialogOptions.ColorScheme = MetroDialogColorScheme.Inverted; //블랙
                        
        }
        
        private void ContextMenu_TagCut(object sender, RoutedEventArgs e)
        {
            MenuItem cm = (MenuItem)sender as MenuItem;
            cm.Items.Clear();

            //string[] Keys = this.entry.getMetaKeys();

            if ((File.GetAttributes(((FileData)AllFileList.SelectedItems[0]).FullPathStr) & FileAttributes.Directory) != FileAttributes.Directory)
            {
                string fileID = FileManager.GetFileIDA(((FileData)AllFileList.SelectedItems[0]).FullPathStr).ToString();

                ContentManager.EntryElement target = ContentManager.EntryElement.getChild(entry.Root, fileID);

                string[] tags = target.Metadata.ToArray(typeof(string)) as string[];
                //entry.removeMetaData(fileID, )

                foreach (string k in tags)
                {
                    MenuItem newItem = new MenuItem();
                    newItem.Header = k;
                    newItem.Click += SubContextMenuClick2;
                    cm.Items.Add(newItem);
                }
            }
        }

        private void SubContextMenuClick2(object sender, RoutedEventArgs e)
        {
            MenuItem cm = (MenuItem)sender as MenuItem;
            if ((File.GetAttributes(((FileData)AllFileList.SelectedItems[0]).FullPathStr) & FileAttributes.Directory) != FileAttributes.Directory)
            {
                string fileID = FileManager.GetFileIDA(((FileData)AllFileList.SelectedItems[0]).FullPathStr).ToString();
                //EntryElement target = entry.CurrentRoot.getChild(fileID);
                //entry.addMetaData(cm.Header.ToString(), target.File);
                entry.removeMetaData(fileID, cm.Header.ToString());
            }
        }

        private void SubContextMenuClick(object sender, RoutedEventArgs e)
        {
            MenuItem cm = (MenuItem)sender as MenuItem;
            if ((File.GetAttributes(((FileData)AllFileList.SelectedItems[0]).FullPathStr) & FileAttributes.Directory) != FileAttributes.Directory)
            {
                string fileID = FileManager.GetFileIDA(((FileData)AllFileList.SelectedItems[0]).FullPathStr).ToString();
                EntryElement target = entry.CurrentRoot.getChild(fileID);
                entry.addMetaData(cm.Header.ToString(), target.File);                  
            }else if ((File.GetAttributes(((FileData)AllFileList.SelectedItems[0]).FullPathStr) & FileAttributes.Directory) == FileAttributes.Directory)
            {
                MessageBox.Show("파일만 태그 설정이 가능합니다");
            }
        }
        //삭제하기
        private async void ContextMenu_Delete(object sender, RoutedEventArgs e)
        {
            int selectedItemCnt = AllFileList.SelectedItems.Count;
            //타겟 파일을 클릭 했을 경우만 
            if (selectedItemCnt > 0)
            {
                try // FileNotFoundException
                {
                    for (int i = 0; i < selectedItemCnt; i++)
                    {
                        string path = ((FileData)AllFileList.SelectedItems[0]).FullPathStr;

                        MessageDialogResult result = await this.ShowMessageAsync("삭제", "이 파일을 휴지통에 버리시겠습니까?",
                            MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings {
                                ColorScheme = MetroDialogColorScheme.Inverted
                            });

                        if(result.Equals(MessageDialogResult.Affirmative))
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
                                        myFilesList.Remove(((FileData)AllFileList.SelectedItems[0]));
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
                                    // Use a try block to catch IOExceptions, to 
                                    // handle the case of the file already being 
                                    // opened by another process. 
                                    try
                                    {
                                        System.IO.File.Delete(path);
                                        myFilesList.Remove(((FileData)AllFileList.SelectedItems[0]));
                                    }
                                    catch (System.IO.IOException exception)
                                    {
                                        Console.WriteLine(exception.Message);
                                        return;
                                    }
                                }

                            }
                        }
                    }
                }
                catch (FileNotFoundException eee)
                {
                    System.Windows.MessageBox.Show(eee.Message);
                }

            }
        }
        //File Delete key press 파일 삭제 
        private async void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            int selectedItemCnt = AllFileList.SelectedItems.Count;
            //타겟 파일을 클릭 했을 경우만 
            if (selectedItemCnt > 0)
            {
                try // FileNotFoundException
                {
                    //Delete Key
                    if (e.Key == Key.Delete)
                    {
                        for (int i = 0; i < selectedItemCnt; i++)
                        {
                            string path = ((FileData)AllFileList.SelectedItems[0]).FullPathStr;

                            MessageDialogResult result = await this.ShowMessageAsync("삭제", "이 파일을 휴지통에 버리시겠습니까?",
                          MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
                          {
                              ColorScheme = MetroDialogColorScheme.Inverted
                          });
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

                                             myFilesList.Remove(((FileData)AllFileList.SelectedItems[0]));
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
                                         // Use a try block to catch IOExceptions, to 
                                         // handle the case of the file already being 
                                         // opened by another process. 
                                         try
                                         {
                                             System.IO.File.Delete(path);
                                             myFilesList.Remove(((FileData)AllFileList.SelectedItems[0]));
                                         }
                                         catch (System.IO.IOException exception)
                                         {
                                             Console.WriteLine(exception.Message);
                                             return;
                                         }
                                     }

                                 }
                             }

                        }
                    }

                }
                catch (FileNotFoundException eee)
                {
                    System.Windows.MessageBox.Show(eee.Message);
                }

            }
        }


    }
}