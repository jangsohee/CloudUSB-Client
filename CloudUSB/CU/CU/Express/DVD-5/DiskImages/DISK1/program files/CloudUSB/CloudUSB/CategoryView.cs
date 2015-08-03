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

            //철표오빠메타데이터리스트호출(실제함수로 수정하기)
            History[] history = pyo(tagStr);

            ArrayList tagList = new ArrayList();
            for(int i = 0; i < history.Length; i++)
            {
                //하나의 히스토리에 메타데이터 개수 알아내기 위함
                ArrayList arrayList  = history[i].Data.Metadata;
                for (int j = 0; j < arrayList.Count; j++)
                {
                    tagList.Add(arrayList[j]);
                }            
            }
            Category_TaglistBox.ItemsSource = tagList;
        }

        //테스트를 위한 히스토리 리턴 함수(지우기)
        public History[] pyo(string metaData)
        {
            string meta = metaData; //

            History[] h = new History[5];
            for (int i = 0; i < 5; i++)
            {
                h[i] = new History()
                {
                    Type = HistoryType.Create,
                    Data =
                        new ContentManager.FileData()
                        {
                            FileName = i + "",
                            FullPathStr = i + "",
                            Metadata = new ArrayList { i+"Hello", i+"Hi"}
                        },
                   OldPath = "경로",
                   Sync = true
                };
            }          
           
            return h;
        }
                
    }


}