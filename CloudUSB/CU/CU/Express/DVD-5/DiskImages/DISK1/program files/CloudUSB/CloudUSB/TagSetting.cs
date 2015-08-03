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
using MahApps.Metro.Controls;
using System.Drawing;
using System.Windows.Controls.Primitives;
using ContentManager;

namespace CloudUSB
{

    public partial class MainWindow : MetroWindow
    {
        //TagButton Click Event
        private void TagBtn_Click(object sender, RoutedEventArgs e)
        {
            TagView tv = new TagView(this.e, this);
            this.Opacity = 0.5;
            tv.ShowDialog();
        }

       
    }
}
