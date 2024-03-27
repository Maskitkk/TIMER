using System;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;



namespace Таймер

{
    public partial class Form1 : Form
    {

        
        private int remainingSeconds = 0;
        private int totalSeconds = 0;
        private Thread countdownThread;
        private NotifyIcon notifyIcon;
        public Form1()

        {
            InitializeComponent();
            InitializeNotifyIcon();
            this.FormClosing += Form1_FormClosing;

        }

        private void InitializeNotifyIcon()
        { 
        notifyIcon = new NotifyIcon();
        notifyIcon.Icon = this.Icon;
            notifyIcon.Visible = false;
            ContextMenuStrip contextMenuStrip = new ContextMenuStrip();
            ToolStripMenuItem showMenuItem = new ToolStripMenuItem("Открыть");
            showMenuItem.Click += ShowMenuItem_Click;
            contextMenuStrip.Items.Add(showMenuItem);

            ToolStripMenuItem exitMenuItem = new ToolStripMenuItem("Выход");
            exitMenuItem.Click += ExitMenuItem_Click;
            contextMenuStrip.Items.Add(exitMenuItem);

            notifyIcon.ContextMenuStrip = contextMenuStrip;

            notifyIcon.MouseDoubleClick += NotifyIcon_MouseDoubleClick;

        }

        private void NotifyIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
           if (e.Button == MouseButtons.Left)
            {
                this.Show();
                this.WindowState = FormWindowState.Normal;

            }
        }

        private void ExitMenuItem_Click(object sender, EventArgs e)
        {
            StopCountdown();    
            Application.Exit();
        }

        private void ShowMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }

      

        private void StartCountdown(int durationSeconds)
        {
            {
                remainingSeconds = durationSeconds;
                totalSeconds = durationSeconds;
                countdownThread = new Thread(new ThreadStart(CountdownThread));
                countdownThread.Start();
            }
        }

        private void CountdownThread()
        { while (remainingSeconds > 0)
            
            { if (IsStopRequested())
                {
                    break; 
                }


            Updatelabel1();
        
            Thread.Sleep(1000);
           
            remainingSeconds--;
            }
        if (remainingSeconds <= 0)
            { Process.Start("shutdown", "/s /t0"); 
            }

        }
        
        
        
        private void Updatelabel1()
        { 
        TimeSpan timeSpan = TimeSpan.FromSeconds(remainingSeconds);
            string formattedTime = string.Format("{0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds);
            if (label1.InvokeRequired)
            {
                label1.Invoke(new MethodInvoker(delegate { label1.Text = formattedTime; }));
            }
            else { label1.Text = formattedTime; }

            UpdateProgressBar();
        
        
        }

        private void button30mins_Click(object sender, EventArgs e)
        {
            Process.Start("shutdown", "/s /t 1800");
            timer1.Start();
            StartCountdown(1800);
        }
        
        
        private void Button1_Click(object sender, EventArgs e)
        {
            Process.Start("shutdown", "/s /t 3600");
            timer1.Start();
            StartCountdown(3600);
        }


        private void button2_Click(object sender, EventArgs e)
        {
            Process.Start("shutdown", "/s /t 7200");
            timer1.Start();
            StartCountdown(7200);

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Process.Start("shutdown", "/s /t 5400");
            timer1.Start();
            StartCountdown(5400);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Process.Start("shutdown", "-a");
            StopCountdown();
        }


        private void StopCountdown()
        {
            if (countdownThread != null && countdownThread.IsAlive)
            {
                countdownThread.Abort();
                remainingSeconds = 0;
                Updatelabel1();

            }

        }

        private bool IsStopRequested()
        { if (InvokeRequired)
            {
                return (bool)Invoke(new Func<bool>(IsStopRequested));
            }
            return remainingSeconds <= 0;

        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
       {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.Hide();
                notifyIcon.Visible = true;
            }
        }

        private void UpdateProgressBar()
        {
            if (InvokeRequired)
            {
                Invoke(new MethodInvoker(delegate {  UpdateProgressBar(); })); 
            }
            else
            {
                progressBar1.Maximum = totalSeconds;
                progressBar1.Value = totalSeconds - remainingSeconds;


            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }



}
