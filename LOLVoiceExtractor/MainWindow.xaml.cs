using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using System.Windows.Resources;
using System.Runtime.InteropServices;
using System.IO;
using Microsoft.Win32;
using System.Threading;
using System.Media;

////2011213命运又跟你开了次玩笑 FSBDLL丢失，怎么办？？？
//无他 行者其无疆也！
namespace LOLVoiceExtractor
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        [DllImport("FSBDll.dll", EntryPoint = "AbortFSBMoudle")]
        public static extern void AbortFSBMoudle();
        [DllImport("FSBDll.dll", EntryPoint = "GetTotalNum", CharSet = CharSet.Ansi)]
        public static extern int GetTotalNum();
        [DllImport("FSBDll.dll", EntryPoint = "GetNowNum")]
        public static extern int GetNowNum();
        [DllImport("FSBDll.dll", EntryPoint = "ExtractFDBFile")]
        public static extern int ExtractFDBFile(int argc, byte[] strFrom,byte[] strTo);
        [DllImport("FSBDll.dll", EntryPoint = "GetOutBuffer")]
        public static extern IntPtr GetOutBuffer();
        [DllImport("FSBDll.dll", EntryPoint = "SetBuffer")]
        public static extern void SetBuffer(StringBuilder abuf);
        /*
        [DllImport("SimpleDLL.dll", EntryPoint = "add", ExactSpelling = true, CallingConvention = CallingConvention.Cdecl)]
　　  public static extern int add(int a, int b); //正常声明即可
        */
        private string m_sDefaultFilePath="";
        private string m_sDefaultFileDir = "";
        private string m_sDefaultTargetDir="";
        private bool  m_bFirstOpen = true;
        private System.Windows.Threading.DispatcherTimer m_Timer;
        private Thread m_Thread;
        public static MusicPlayer mMusicPlayer=new MusicPlayer();
        private StringBuilder mStringBuilder=null;
        private enum MyState
        {
            State_Normal = 0,
            State_Extracting,
            State_Over
        };
        MyState m_State = MyState.State_Normal;
        private string[] strCommend ={
		    "CallFSBDLL",
		    "-d",
		    "",
		    ""
	        };
        public MainWindow()
        {
            InitializeComponent();
            // 创建一个纯色brush
            Brush brush = new SolidColorBrush(Color.FromArgb(0xFF, 0xC3, 0xAD, 0xAD));
            this.textBox1.Foreground = brush;
          // StreamResourceInfo sri = Application.GetResourceStream(new Uri("Cursor_7.cur", UriKind.Relative));
            //Cursor customCursor = new Cursor(sri.Stream);
            //this.Cursor = customCursor;
            GetDefaultDir();
            //构造一个DispatcherTimer类实例
            m_Timer= new System.Windows.Threading.DispatcherTimer();
            //设置事件处理函数
            m_Timer.Tick += new EventHandler(WriteBuffer);
            //定时器时间间隔1s
            m_Timer.Interval = new System.TimeSpan(0, 0, 0,0,10);
            
            //通过Manifest读取相关资源
            //string str = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".images.SplashScreen1.png";
            //可以使用
            //string[] resourceNames = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();
            System.Reflection.Assembly tManifestAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream stream0 = tManifestAssembly.GetManifestResourceStream(typeof(App), "Images.MainBack.jpg");
            System.IO.Stream stream1 = tManifestAssembly.GetManifestResourceStream(typeof(App), "Images.Ann.ico");
            System.IO.Stream stream2 = tManifestAssembly.GetManifestResourceStream(typeof(App), "Images.HoverIcon.png");
            System.IO.Stream stream3 = tManifestAssembly.GetManifestResourceStream(typeof(App), "Images.Cursor_7.cur");
            ImageSourceConverter imgConverter = new ImageSourceConverter();

            this.image1.Source= (ImageSource)imgConverter.ConvertFrom(stream0);
            this.image2.Source = (ImageSource)imgConverter.ConvertFrom(stream2);
            this.Icon = (ImageSource)imgConverter.ConvertFrom(stream1);
           // System.Windows.Forms.Cursor cursor = new System.Windows.Forms.Cursor(stream3);
            this.Cursor = new System.Windows.Input.Cursor(stream3);

            m_Thread = new Thread(new ThreadStart(ExtractFile));
            mStringBuilder = new StringBuilder();
            mStringBuilder.Capacity = 20480;//设置字符串最大长度       
            Hide();

        }
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mMusicPlayer.PlayVoice(MusicPlayer.eVoice.VoiceClose);
           // AbortFSBMoudle();
            Thread.Sleep(900);
            if (m_Thread!=null)
                m_Thread.Abort();
            SaveDefaultDir();
        }
        private void GetDefaultDir()
        {
            try
            {
                RegistryKey testKey = Registry.CurrentUser.OpenSubKey("LolSoundsExtractor");
                if (testKey == null)
                {
                    testKey = Registry.CurrentUser.CreateSubKey("LolSoundsExtractor");
                    testKey.SetValue("OpenFolderPath", "");
                    testKey.SetValue("OpenFolderDir", "");
                    testKey.SetValue("OutFolderDir", "");
                    testKey.SetValue("OpenOutDir", "1");

                    testKey.Close();
                    Registry.CurrentUser.Close();

                    mMusicPlayer.PlayVoice(MusicPlayer.eVoice.VoiceFB);
                }
                else
                {
                    m_sDefaultFilePath = testKey.GetValue("OpenFolderPath").ToString();
                    m_sDefaultFileDir = testKey.GetValue("OpenFolderDir").ToString();
                    m_sDefaultTargetDir = testKey.GetValue("OutFolderDir").ToString();
                    if(testKey.GetValue("OpenOutDir").ToString()=="True")
                        this.checkBox1.IsChecked =true;
                    else
                        this.checkBox1.IsChecked = false;
                    testKey.Close();
                    Registry.CurrentUser.Close();
                    if(m_sDefaultFilePath!="")
                    {
                        //取消默认的暗色提示语句
                        this.textBox1.Text = m_sDefaultFilePath;
                        Brush brush = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x00, 0x00));
                        this.textBox1.Foreground = brush;
                        m_bFirstOpen = false;
                    }
                    this.textBox2.Text = m_sDefaultTargetDir;
                    mMusicPlayer.PlayVoice(MusicPlayer.eVoice.VoiceOpen);
                }
            }
            catch (Exception e)
            {

            }
        }
        private void SaveDefaultDir()
        {
            try
            {
                RegistryKey testKey = Registry.CurrentUser.OpenSubKey("LolSoundsExtractor");
                if (testKey != null)
                {
                    testKey = Registry.CurrentUser.CreateSubKey("LolSoundsExtractor");
                    testKey.SetValue("OpenFolderPath",m_sDefaultFilePath);
                    testKey.SetValue("OpenFolderDir", m_sDefaultFileDir);
                    testKey.SetValue("OutFolderDir", m_sDefaultTargetDir);
                    testKey.SetValue("OpenOutDir", this.checkBox1.IsChecked);
                    testKey.Close();
                    Registry.CurrentUser.Close();
                }
            }
            catch (Exception e)
            {

            }
        }
        private void image1_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (m_State == MyState.State_Normal)
            {
                strCommend[2] = this.textBox2.Text;
                strCommend[3] = this.textBox1.Text;
                if (strCommend[2] == ""||strCommend[3]== "")
                {
                    mMusicPlayer.PlayVoice(MusicPlayer.eVoice.VoiceClose);
                    System.Windows.MessageBox.Show("抱歉，请先选择好文件及目录", "喔，ms这样不行的！", MessageBoxButton.YesNo, MessageBoxImage.Information);
                    return;    
                }
                this.richTextBox1.Document.Blocks.Clear();

                this.label3.Visibility = System.Windows.Visibility.Visible;
                this.progressBar1.Visibility = System.Windows.Visibility.Visible;
                this.button1.Content = "暂停提取";
                // this.progressBar1.SetCurrentValue();
                //启动定时器
                m_Timer.Start();
                m_State = MyState.State_Extracting;
                if (m_Thread.ThreadState == ThreadState.Unstarted)
                {
                    mMusicPlayer.PlayVoice(MusicPlayer.eVoice.VoiceBeginWork);
                    m_Thread.Start();
                    
                }
                else if (m_Thread.ThreadState == ThreadState.Suspended || m_Thread.ThreadState == ThreadState.SuspendRequested)
                {
                    mMusicPlayer.PlayVoice(MusicPlayer.eVoice.VoiceBeginWork2);
                    m_Thread.Resume();
                }
                else
                {
                    mMusicPlayer.PlayVoice(MusicPlayer.eVoice.VoiceBeginWork);
                    m_Thread = new Thread(new ThreadStart(ExtractFile));
                    m_Thread.Start();

                }
                
            }
            else if (m_State == MyState.State_Extracting)
            {
                //this.label3.Visibility = System.Windows.Visibility.Hidden;
                //this.progressBar1.Visibility = System.Windows.Visibility.Hidden;
                m_Timer.Stop();
                m_State = MyState.State_Normal;
                this.button1.Content = "继续提取";
                mMusicPlayer.PlayVoice(MusicPlayer.eVoice.VoiceClose);
                //AbortFSBMoudle();
                //Thread.Sleep(300);
                //this.progressBar1.Value = 0;
                m_Thread.Suspend();
            }
            
	    }
        private void ExtractFile()
        {
            Encoding   e0   =   Encoding.GetEncoding(   936   );

            char[] charOri = strCommend[3].ToCharArray();
            byte[] byteFrom = e0.GetBytes(strCommend[3]);
            byte[] byteTo     = e0.GetBytes(strCommend[2]);
//             string strAfter = e0.GetString(byteFrom);
//             char[] charAfter = strAfter.ToCharArray();
            ExtractFDBFile(4, byteFrom,byteTo);
          /*  strCommend[3] = strAfter;
            //Encoding.Convert(Encoding.ASCII,Encoding.Unicode,)
            for(int i=0;i<4;++i)
                strCommend[i] = e0.GetString(e0.GetBytes(strCommend[i]));
            ExtractFDBFile(4, strCommend);*/
            m_State = MyState.State_Over;
        }
        private void OpenWindow()
        {
            System.Diagnostics.Process.Start("explorer.exe", @m_sDefaultTargetDir);
        }
        private void WriteBuffer(object sender, EventArgs e)
        {
            if (m_State == MyState.State_Normal)
                return;
            string Ini;
            SetBuffer(mStringBuilder);
            Ini = mStringBuilder.ToString(); ;
            /*IntPtr ptr = GetOutBuffer();         
            if (ptr != null)
                Ini = Marshal.PtrToStringAnsi(ptr);
            else
                Ini = "";*/
            if (Ini != "")
            {
                this.richTextBox1.AppendText(Ini);
                this.richTextBox1.ScrollToEnd();
            }
          //  if (this.progressBar1.Minimum <= 0)
         //   this.progressBar1.Minimum = GetTotalNum();
            string slablel = "进度";
            slablel += GetNowNum();
            slablel += "/";
            slablel += GetTotalNum();
            this.label3.Content = slablel;
            Double now=Convert.ToDouble(GetNowNum()) ;
            Double total = Convert.ToDouble(GetTotalNum());
            this.progressBar1.Value = (now/total) * 100.0f;
            if (m_State == MyState.State_Over)
            {
                //关闭定时器
                m_Timer.Stop();
                if (m_Thread.ThreadState!=ThreadState.Stopped)
                    m_Thread.Abort();
                this.button1.Content = "开始提取";
                m_State = MyState.State_Normal;
                // string Buffer=GetOutBuffer();
                this.label3.Visibility = System.Windows.Visibility.Hidden;
                this.progressBar1.Visibility = System.Windows.Visibility.Hidden;
                this.progressBar1.Value = 0;
                if (this.checkBox1.IsChecked == true)
                {
                    if (System.Windows.MessageBox.Show("是否打开解压目录", "恭喜，解压成功！", MessageBoxButton.YesNo, MessageBoxImage.Information) == MessageBoxResult.Yes)
                    {
                        OpenWindow();
                    }
                }
                else
                    System.Windows.MessageBox.Show("解压成功！", "恭喜~", MessageBoxButton.OK, MessageBoxImage.Information);             
            }
        }
        private void button2_Click(object sender, RoutedEventArgs e)
        {
            System.IO.Stream myStream;
            System.Windows.Forms.OpenFileDialog openFileDialog= new System.Windows.Forms.OpenFileDialog();
            
            //首次defaultfilePath为空，按FolderBrowserDialog默认设置（即桌面）选择  
            if (m_sDefaultFilePath != "")
            {
                //设置此次默认目录为上一次选中目录  
                openFileDialog.InitialDirectory = m_sDefaultFileDir;
            }
            else
                openFileDialog.InitialDirectory = "c:\\";
            openFileDialog.Filter = "All files (*.*)|*.*|fsb files (*.fsb)|*.fsb";
            openFileDialog.FilterIndex = 2;
            openFileDialog.RestoreDirectory = true;
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                //测试下打开的文件是否存在
                if ((myStream = openFileDialog.OpenFile()) != null)
                {
                    mMusicPlayer.PlayVoice(MusicPlayer.eVoice.VoiceEffort);
                    // Insert code to read the stream here.
                    myStream.Close();
                    this.textBox1.Text = openFileDialog.FileName;
                    //记录选中的目录  
                    m_sDefaultFileDir = openFileDialog.FileName.Replace(openFileDialog.SafeFileName, "");
                    m_sDefaultFilePath = openFileDialog.FileName;
                    if (m_bFirstOpen)
                    {
                        //取消默认的暗色提示语句
                        Brush brush = new SolidColorBrush(Color.FromArgb(0xFF, 0x00, 0x00, 0x00));
                        this.textBox1.Foreground = brush;
                        m_bFirstOpen = false;
                    }
                }
            }
        }
        private void button3_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Forms.FolderBrowserDialog fbd = new System.Windows.Forms.FolderBrowserDialog();
            //首次defaultfilePath为空，按FolderBrowserDialog默认设置（即桌面）选择  
            if (m_sDefaultTargetDir != "")
            {
                //设置此次默认目录为上一次选中目录  
                fbd.SelectedPath = m_sDefaultTargetDir;
            }
            if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                mMusicPlayer.PlayVoice(MusicPlayer.eVoice.VoiceEffort2);
                this.textBox2.Text = fbd.SelectedPath;
                //记录选中的目录  
                m_sDefaultTargetDir = fbd.SelectedPath;
            }  
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            this.richTextBox1.Document.Blocks.Clear();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }
        private void image2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            if (e.ClickCount == 2)
            {
                //双击时执行
                AboutMe AB=new AboutMe();
                mMusicPlayer.PlayVoice(MusicPlayer.eVoice.VoiceAnni);
                AB.InitializeComponent();
                AB.Show();

            }
        }

        private void image2_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
/*
            string location = System.Environment.CurrentDirectory + "\\Sound\\AnnieFun.mp3";
            this.mediaElement1.Source = new Uri(location, UriKind.Absolute);
            this.mediaElement1.Play();        //播放*/
            //mMusicPlayer.PlaySound("Evelynn.attack1.mp3");
            mMusicPlayer.PlaySound();
            this.image2.Opacity = 1.0f;
        }
        private void image2_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            
            this.image2.Opacity = 0.8f;
            //PlaySound();
        }
  /*      private void PlaySound()
        {
            System.Media.SoundPlayer player = new System.Media.SoundPlayer();
            string location = System.Environment.CurrentDirectory + "\\Sound\\AnnieFun.wav";
            player.SoundLocation = location;
            player.Play();
        }*/
    }
}
