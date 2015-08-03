using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
using Newtonsoft.Json;

namespace CloudUSB
{
    /// <summary>
    /// JoinView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class JoinView : MetroWindow
    {
        
        MainWindow root;
        public JoinView(MainWindow _root)
        {
            root = _root;
            InitializeComponent();
        }

        private void joinView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            root.Opacity = 1;
        }

        public bool idValidationChk(string _id)
        {
            bool res = true;
            string id = _id;

            int idLength = id.Length;
            if (idLength < 5 || idLength > 20)
                res = false;

            for (int i = 0; i < idLength; i++)
            {
                char word = id[i];
                if (((word > '0' && word <= '9') || (word >= 'a' && word <= 'z')) == false)
                    res = false;
            }
            return res;
        }

        public bool pwValidationChk(String _pw)
        {
            string pw = _pw;
            bool res = true;
            int pwLength = pw.Length;
            if (pwLength < 5 || pwLength > 20)
            {
                res = false;
            }
            for (int i = 0; i < pwLength; i++)
            {
                char word = pw[i];
                if (!((word > '0' && word <= '9') || (word >= 'a' && word <= 'z')))
                    res = false;
            }
            return res;
        }
        public bool checkNAME(String name)
        {
            bool result = true;
            int length = name.Length;
            if (length < 1 || length > 20)
            {
                result = false;
            }
            for (int i = 0; i < length; i++)
            {
                char word = name[i];

                if (word == '<')
                {
                    result = false;
                }
                if (word == '>')
                {
                    result = false;
                }
                if (word == '\"')
                {
                    result = false;
                }
                if (word == '\'')
                {
                    result = false;
                }
                if (word == ' ')
                {
                    result = false;
                }
                if (word == '.')
                {
                    result = false;
                }
                if (word == '\\')
                {
                    result = false;
                }

            }
            return result;
        }
        private void joinBtn_Click(object sender, RoutedEventArgs e)
        {
            string id = joinIdBox.Text;
            string pw = joinPwBox.Password;
            string name = joinNameBox.Text;

            if (idValidationChk(id) == false)
            {
                MessageBox.Show("ID는 5이상 20이하의 소문자, 숫자만 가능합니다 : " + id);
                joinIdBox.Clear();
            }
            else if (pwValidationChk(pw) == false)
            {
                MessageBox.Show("PASSWORD는 5이상 20이하의 소문자, 숫자만 가능합니다 : " + pw);
                joinPwBox.Clear();
            }
            else if (checkNAME(name) == false)
            {
                MessageBox.Show("NAME는 1이상 20이하의 글자만 가능합니다 : " + pw);
                joinNameBox.Clear();
            }
            else
            {
                String callUrl = "http://210.118.74.120:8080/cu/api/join";
                String postData = String.Format("userId={0}&password={1}&name={2}", id, pw, name);

                try
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(callUrl);
                    
                    byte[] sendData = UTF8Encoding.UTF8.GetBytes(postData);
                    httpWebRequest.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.ContentLength = sendData.Length;

                    Stream requestStream = httpWebRequest.GetRequestStream();
                    requestStream.Write(sendData, 0, sendData.Length);
                    requestStream.Close();

                    HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    StreamReader streamReader = new StreamReader(httpWebResponse.GetResponseStream(), Encoding.GetEncoding("UTF-8"));

                    string returnData = streamReader.ReadToEnd();

                    streamReader.Close();
                    httpWebResponse.Close();

                    dynamic jsonStr = JsonConvert.DeserializeObject(returnData);

                    string joinResult = jsonStr["result"]; // jsonStr.result

                    if (joinResult.Equals("True"))
                    {
                        MessageBox.Show("가입을 축하드립니다");
                        //root.LoginBtn.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/img/logout.png", UriKind.Absolute)));
                        root.isLogin = false;
                        
                        this.Close();
                    }
                    else 
                    {
                        root.isLogin = false;
                        MessageBox.Show("이미 존재하는 아이디입니다");
                        joinIdBox.Clear();
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }
    }
}
