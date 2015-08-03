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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CloudUSB
{
    /// <summary>
    /// HistoryView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class HistoryView : MetroWindow
    {
        MainWindow mw;
        public HistoryView(MainWindow _mw)
        {
            InitializeComponent();
            mw = _mw;
        }

        private void History_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mw.Opacity = 1;
        }
    }
}
