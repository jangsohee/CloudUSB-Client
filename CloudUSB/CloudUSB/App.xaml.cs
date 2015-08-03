using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


using System.Windows.Forms;
using System.Drawing;
using System.Windows.Threading;

using ContentManager;

using Microsoft.Win32;


namespace CloudUSB
{
    /// <summary>
    /// App.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class App : System.Windows.Application
    {
        NotifyIcon TrayIcon;
        MainWindow main;
        USBControl usb;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            TrayIcon = new NotifyIcon();
            TrayIcon.Icon = new Icon(@"./cuIcon.ico");
            TrayIcon.Visible = true;

            RegistryKey rk = Registry.CurrentUser;
            RegistryKey sk = rk.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", true);
            sk.SetValue("DisableAutoplay", 1, Microsoft.Win32.RegistryValueKind.DWord);

            usb = new USBControl();
            usb.attached = attach;
            usb.detached = detach;

            TrayIcon.DoubleClick += new System.EventHandler(TrayIcon_DoubleClick);
            TrayIcon.Click += new System.EventHandler(TrayIcon_Click);

            CloudUSB.MainWindow.IsWindowOpened = false;
        }

        private static void TrayIcon_DoubleClick(object sender, EventArgs e)
        {

            System.Windows.Application.Current.Shutdown();
        }


        public void attach()
        {
            if (!CloudUSB.MainWindow.IsWindowOpened)
            {
                CloudUSB.MainWindow.IsWindowOpened = true;
                main = new MainWindow();

                main.Show();
            }
        }


        public void detach()
        {
            if (CloudUSB.MainWindow.IsWindowOpened)
            {
                main.Close();
            }
        }

        private void TrayIcon_Click(object sender, EventArgs e)
        {
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == MouseButtons.Left)
            {
                //if (MainWindow.IsWindowOpend)
                if (!CloudUSB.MainWindow.IsWindowOpened)
                {
                    main = new MainWindow();
                    CloudUSB.MainWindow.IsWindowOpened = true;
                    main.Show();
                }
                else
                {
                    //System.Windows.MessageBox.Show("already opened");
                }
            }
            else
            {
                
            }
        }

        private void App_Shutdown()
        {
            usb.Dispose();
            RegistryKey rk = Registry.CurrentUser;
            RegistryKey sk = rk.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\AutoplayHandlers", true);
            sk.SetValue("DisableAutoplay", 0, Microsoft.Win32.RegistryValueKind.DWord);
            System.Windows.Application.Current.Shutdown();
        }
    }
}
