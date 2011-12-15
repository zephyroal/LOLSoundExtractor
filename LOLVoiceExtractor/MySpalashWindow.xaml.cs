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
using System.Windows.Shapes;
using System.Threading;

namespace LOLVoiceExtractor
{
    /// <summary>
    /// Interaction logic for MySpalashWindow.xaml
    /// </summary>
    public partial class MySpalashWindow : Window
    {
        public static App GetApp()
        {
            return App.mApp;
        }
        private System.Windows.Threading.DispatcherTimer m_OpenTimer;
        private System.Windows.Threading.DispatcherTimer m_CloseTimer;
        public MySpalashWindow()
        {
            InitializeComponent();
            //this.Background
            System.Reflection.Assembly tManifestAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream stream0 = tManifestAssembly.GetManifestResourceStream(typeof(App), "Images.SplashScreen1.png");
            //此处图片从文件中读入用以模拟内存中的图片

            /*System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap("bg.jpg");
            MemoryStream stream = new MemoryStream();
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
            ImageBrush imageBrush = new ImageBrush();
            ImageSourceConverter imageSourceConverter = new ImageSourceConverter();
            imageBrush.ImageSource = (ImageSource)imageSourceConverter.ConvertFrom(stream);
            button.Background = imageBrush;*/
            ImageSourceConverter imgConverter=new ImageSourceConverter();
            this.image1.Source = (ImageSource)imgConverter.ConvertFrom(stream0);
            //设置淡入
            m_OpenTimer = new System.Windows.Threading.DispatcherTimer();
            m_OpenTimer.Tick += new EventHandler(FadeIn);
            m_OpenTimer.Interval = new System.TimeSpan(0, 0, 0, 0, 30);
            //设置淡出
            m_CloseTimer = new System.Windows.Threading.DispatcherTimer();
            m_CloseTimer.Tick += new EventHandler(FadeOut);
            m_CloseTimer.Interval = new System.TimeSpan(0, 0, 0, 0, 30);
            //开始显示界面
            BeginFadeIn();
            Show();
            GetApp().StartMainWindow();
            //加载线程建立
            Thread LoadThread = new Thread(new ThreadStart(MyLoadThread));
            LoadThread.Start();

        }
        void MyLoadThread()
        {
            //这里等待一小会
            //Thread.Sleep(1000);
            // 实际中 可以建立线程 监控模块读取进度
            /*for (int i = 0; i < 100; i++)
            {
                MessageListener.Instance.ReceiveMessage(string.Format("Load module {0}", i));
            }*/
            //等待Splasher关闭淡出时间 挂起
            Thread.Sleep(1300);
            BeginFadeOut();
        }
        public void BeginFadeIn()
        {
            //开始
            this.image1.Opacity = 0;
            m_OpenTimer.Start();
        }
        public void BeginFadeOut()
        {
            m_CloseTimer.Start();
        }
        public void FadeIn(object sender, EventArgs e)
        {
            this.image1.Opacity += 0.03f;
            if (this.image1.Opacity >= 1.0f)
            {
                this.image1.Opacity = 1.0f;
                m_OpenTimer.Stop();
            }
        }
        public void FadeOut(object sender, EventArgs e)
        {
            this.image1.Opacity -= 0.03f;
            if (this.image1.Opacity <= 0.0f)
            {
                m_CloseTimer.Stop();
                GetApp().ShowMainWindow();
                //
                Close();
            }
        }
    }
}
