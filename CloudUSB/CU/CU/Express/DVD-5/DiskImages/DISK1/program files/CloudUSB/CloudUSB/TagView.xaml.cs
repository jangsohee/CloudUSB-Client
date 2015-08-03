using MahApps.Metro.Controls;
using System;
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

namespace CloudUSB
{
    /// <summary>
    /// TagView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class TagView : MetroWindow
    {
        Entry entry;
        MainWindow root;
        string[] Keys;

        public TagView()
        {
            InitializeComponent();
            tagListBox.ItemsSource = null;
            entry = null;
        }

        public TagView(Entry e, MainWindow _root)
        {
            InitializeComponent();
            this.root = _root;
            this.entry = e;     

            Keys = entry.Meta.getKeys();
            tagListBox.ItemsSource = this.entry.Meta.getKeys();
        }

        private void tagSaveBtn_Click(object sender, RoutedEventArgs e)
        {
            string tag = tagName.Text;
            bool existTag = false;
            //태그 리스트에 저장
            foreach (string key in Keys)
            {
                if (key.Equals(tag) == true)
                {
                    existTag = true;
                }
            }
            if (existTag == true)
            {
                MessageBox.Show("이미 태그가 존재합니다.");
            }
            else {
                this.entry.Meta.addKey(tag);
                Keys = entry.Meta.getKeys();
                tagListBox.ItemsSource = this.entry.Meta.getKeys();
            }
        }
        private void tagView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
           root.Opacity = 1;
           root.Category_TaglistBox.ItemsSource = this.entry.Meta.getKeys();
           
        }

        private void tagViewContextMenu(object sender, ContextMenuEventArgs e)
        {
            ListBox lb = sender as ListBox;

            if (tagListBox.SelectedItems.Count > 0)
            {
                lb.ContextMenu.Visibility = System.Windows.Visibility.Visible;
            }
            else {
                lb.ContextMenu.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        private void tagListBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (tagListBox != null)
            {
                tagListBox.SelectedItem = null;
            }
        }

        private void tagRemove_Click(object sender, RoutedEventArgs e)
        {
            //태그 삭제 함수 호출하기!!
           
        }

        
    }
}
