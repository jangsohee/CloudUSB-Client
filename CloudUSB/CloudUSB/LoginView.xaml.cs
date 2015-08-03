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

using System.IO;
using System.Net;
using System.Web;
using MahApps.Metro.Controls;
using Newtonsoft.Json;

namespace CloudUSB
{
    /// <summary>
    /// LoginView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoginView : MetroWindow
    {
        MainWindow root;
        
        
        public LoginView(MainWindow _root)
        {
            root = _root;
            InitializeComponent();

            //if (isLoginSuccess == true)
            //{
            //    ImageBrush img = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/img/logout.png", UriKind.Absolute)));
            //    root.LoginBtn.Background = img;
            //}
            //else
            //{
            //    ImageBrush img = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/img/login.png", UriKind.Absolute)));
            //    root.LoginBtn.Background = img;
            //}

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
        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string id = id_box.Text;
            string pw = pw_box.Password;

            if (idValidationChk(id) == false && pwValidationChk(pw) == false)
            {
                MessageBox.Show("ID, PASSWORD는 5이상 20이하의 소문자, 숫자만 가능합니다");
                id_box.Clear();
                pw_box.Clear();
            }
            else if (idValidationChk(id) == false) {
                MessageBox.Show("ID는 5이상 20이하의 소문자, 숫자만 가능합니다 : " + id);
                id_box.Clear();
            }
            else if (pwValidationChk(pw) == false)
            {
                MessageBox.Show("PASSWORD는 5이상 20이하의 소문자, 숫자만 가능합니다 : " + pw);
                pw_box.Clear();
            }
            else {
                String callUrl = "http://210.118.74.120:8080/cu/api/login";
                String postData = String.Format("userId={0}&password={1}", id, pw);

                try
                {
                    HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(callUrl);

                    // 인코딩 UTF-8
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
                        MessageBox.Show("안녕하세요 " + id + "님");
                        
                        root.userID = id;
                        root.userToken = jsonStr["clientKey"];
                        root.Request.userID = id;
                        root.Request.aKey = root.userToken;
                        root.LoginBtn.Background = new ImageBrush(new BitmapImage(new Uri("pack://application:,,,/img/logout.png", UriKind.Absolute)));
                        root.isLogin = true;

                        this.Close();
                    }
                    else
                    {
                        root.isLogin = false;
                        MessageBox.Show("존재하지 않는 계정입니다 \n다시 입력해주세요");
                        id_box.Clear();
                        pw_box.Clear();
                    }
                }
                catch(Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
             }
        }

        private void LoginView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {  
            root.Opacity = 1;
        }
        
        private void signUp_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            JoinView jv = new JoinView(root);
            root.Opacity = 0.3;
            jv.ShowDialog();
        } 
        
    }
}
