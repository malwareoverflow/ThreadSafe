using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Internetconnection
{
    public partial class ThreadSafe : Form
    {// This delegate enables asynchronous calls for setting  
        // the text property on a TextBox control.  
        delegate void StringArgReturningVoidDelegate(string text);
        private Thread demoThread = null;
       
        public int Progresscount = 0;
        static EventWaitHandle waithandler = new AutoResetEvent(false);
        public ThreadSafe()
        {
            InitializeComponent();
        }
        public static bool CheckForInternetConnection()
        {
            try
            {

                
                using (var client = new WebClient())
                {
                    using (var stream = client.OpenRead("http://www.google.com"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }

        public  void Progressincrement()
        {
           
            
            

            
            waithandler.WaitOne();
            while (CheckForInternetConnection()==true)
            {
                if (Progresscount==100)

                {
                    break;
                }
                SetLabel("Connected");
                Progresscount += 1;

           SetProgress(Progresscount.ToString());
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
            if (Progresscount <100)
            {
                Startthread();
            }
            SetLabel("Completed");
            

        }

      public  void Startthread ()
            {

       this.demoThread=   new Thread(new ThreadStart(Progressincrement));
            this.demoThread.Start();
         SetLabel("Waiting for connection");
            while (CheckForInternetConnection() == false) ;

            waithandler.Set();
        }
        private void SetLabel(string text)
        {
            // InvokeRequired required compares the thread ID of the  
            // calling thread to the thread ID of the creating thread.  
            // If these threads are different, it returns true.  
            if (this.label1.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetLabel);
                this.Invoke(d, new object[] { text });
            }
            else
            {
                this.label1.Text = text;
            }
        }
        private void SetProgress(string Value)
        {
            // InvokeRequired required compares the thread ID of the  
            // calling thread to the thread ID of the creating thread.  
            // If these threads are different, it returns true.  
            if (this.progressBar1.InvokeRequired)
            {
                StringArgReturningVoidDelegate d = new StringArgReturningVoidDelegate(SetProgress);
                this.Invoke(d, new object[] {Value});
            }
            else
            {
                this.progressBar1.Value = Convert.ToInt32(Value);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Startthread();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Responsive");
        }
    }
}
