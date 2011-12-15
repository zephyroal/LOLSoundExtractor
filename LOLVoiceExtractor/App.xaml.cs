using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using System.Threading;
using System.Drawing;

namespace LOLVoiceExtractor
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public  partial class App: Application
    {
        public static LOLVoiceExtractor.App mApp;
        public LOLVoiceExtractor.MainWindow mMainWindow=null;
        /// <summary>
        /// Main
        /// </summary>
        [STAThread()]
        static void Main()
        {
            new App();
           //SplashModule 
            //MySpalashWindow Splash = new MySpalashWindow();
        }
        public App ( )
        {
            //这部分由系统自动执行 不必去理会
            InitializeComponent();
            mApp = this;
            Run ( );            
        }
        public void StartMainWindow()
        {
            mMainWindow=new MainWindow();
        }
        public void ShowMainWindow()
        {
            if(mMainWindow!=null)
                mMainWindow.Show();
        }
    } 
}
