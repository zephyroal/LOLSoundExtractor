﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;

namespace SplashDemo
{
    /// <summary>
    /// 
    /// </summary>
    class App: Application
    {
        /// <summary>
        /// 
        /// </summary>
        [STAThread ( )]
        static void Main ( )
        {
            Splasher.Splash = new SplashScreen ( );
            Splasher.ShowSplash ( );

            for ( int i = 0; i < 5000; i++ )
            {
                MessageListener.Instance.ReceiveMessage ( string.Format ( "Load module {0}", i ) );
                Thread.Sleep ( 1 );
            }

            new App ( );
        }
         /// <summary>
        /// 
        /// </summary>
        public App ( )
        {         
            StartupUri = new System.Uri ( "MainWindow.xaml", UriKind.Relative );

            Run ( );            
        }
    }
}
