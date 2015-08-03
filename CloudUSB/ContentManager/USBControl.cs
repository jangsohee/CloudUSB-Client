using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Management;
using System.Windows.Threading;
using System.Windows.Forms;
using System.Threading;

namespace ContentManager
{
    public delegate void AttachEvent();
    public delegate void DetachEvent();

    public class USBControl : IDisposable
    {
        // used for monitoring plugging and unplugging of USB devices.        
        private ManagementEventWatcher watcherAttach;
        private ManagementEventWatcher watcherDetach;

        public AttachEvent attached { get; set; }
        public DetachEvent detached { get; set; }

        public bool isAttached;

        private SynchronizationContext _uiThreadContext;

        public USBControl()
        {
            isAttached = false;

            _uiThreadContext = new WindowsFormsSynchronizationContext();

            // Add USB plugged event watching
            watcherAttach = new ManagementEventWatcher();
            watcherAttach.EventArrived += new EventArrivedEventHandler(Attaching);
            watcherAttach.Query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 2");
            watcherAttach.Start();


            // Add USB unplugged event watching
            watcherDetach = new ManagementEventWatcher();
            watcherDetach.EventArrived += new EventArrivedEventHandler(Detaching);
            watcherDetach.Query = new WqlEventQuery("SELECT * FROM Win32_DeviceChangeEvent WHERE EventType = 3");
            watcherDetach.Start();
        }

        public void Dispose()
        {
            watcherAttach.Stop();
            watcherDetach.Stop();
            //you may want to yield or Thread.Sleep
            watcherAttach.Dispose();
            watcherDetach.Dispose();
            //you may want to yield or Thread.Sleep
        }


        void Attaching(object sender, EventArrivedEventArgs e)
        {
            if (sender != watcherAttach && !isAttached) return;
            isAttached = true;
            //Dispatcher.Invoke(DispatcherPriority.Normal, attached);
            _uiThreadContext.Post(new SendOrPostCallback((o) =>
            {
                attached();
            }), null);
        }


        void Detaching(object sender, EventArrivedEventArgs e)
        {
            if (sender != watcherDetach && isAttached) return;
            isAttached = false;
            //detached();
            _uiThreadContext.Post(new SendOrPostCallback((o) =>
            {
                detached();
            }), null);

        }

        ~USBControl()
        {
            this.Dispose();// for ease of readability I left out the complete Dispose pattern
        }
    }
}

