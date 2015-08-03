using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Drawing;
using System.ComponentModel;
using System.Collections;
using System.Collections.ObjectModel;
using MahApps.Metro.Controls;
using System.IO;

namespace CloudUSB
{
    /// <summary>
    /// SettingView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SettingView : MetroWindow
    {
        MainWindow mw;
        bool flag;
        public SettingView(MainWindow _mw)
        {            
            InitializeComponent();
            mw = _mw;
            if (!mw.initSuccess)
            {
                mw.WindowState = WindowState.Minimized;
            }            
            flag = false;
        }

        FolderBrowserDialog fbd = new FolderBrowserDialog();
        //OpenFileDialog ofd = new OpenFileDialog();

        private void settingBtn_Click(object sender, EventArgs e)
        {
            //settingTextBox.Text = "";
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                settingTextBox.Text = fbd.SelectedPath;
                flag = true;

                File.WriteAllText(@"./SaveDefaultPath.txt", fbd.SelectedPath, Encoding.UTF8);               
            }
            else
            {
                flag = false;
            }
        }

        private void okBtn_Click(object sender, EventArgs e)
        {  
            //OK 버튼은 선택된 경로가 있을 때 적용
            if (flag)
            {
                mw.initSuccess = true;
                mw.defaultPath = fbd.SelectedPath;
                mw.Refresh(fbd.SelectedPath);                
            }
            this.Close();
        }

        private void SettingView_Closing(object sender, CancelEventArgs e)
        {
            mw.Opacity = 1;
            if (flag)
            {                
                mw.Refresh(fbd.SelectedPath);
            }
            //if (!flag)
            //{
                
            //    //mw.Close();
            //}
            //else
            //{
            //    //mw.WindowState = WindowState.Maximized;
            //}     
        }
    }
}
