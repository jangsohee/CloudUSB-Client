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
using MahApps.Metro.Controls;
using System.Drawing;
using System.Windows.Controls.Primitives;
using ContentManager;

namespace CloudUSB
{

    public partial class MainWindow : MetroWindow
    {
        private void SearchBtn_Click(object sender, RoutedEventArgs e)
        {            
            string tagStr = tagSearchBox.Text;
            tagSearchBox.Clear();

            //ContentManager.FileData[] history = (((ArrayList)entry.Meta[tagStr]).ToArray(typeof(ContentManager.FileData))
            //            as ContentManager.FileData[])
            string[] keys = entry.getMetaKeys();
            ArrayList tagList = new ArrayList();
            
            foreach (string key in keys)
            {
                if (key.Contains(tagStr.ToLower()))
                {
                    tagList.Add(key);
                }
            }

            if (isCategory_TagListLoaded)
            {
                Category_TaglistBox.ItemsSource = tagList;
            }
            if (tagList.Count > 0)
                Category_TaglistBox.SelectedIndex = 0;
            else
                Category_TaglistBox.SelectedIndex = -1;
        }
    }


}