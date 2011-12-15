using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace LOLVoiceExtractor
{
    /// <summary>
    /// Helper to show or close given splash window
    /// </summary>
    public static class Splasher
    {
        /// <summary>
        /// 
        /// </summary>
        private static MySpalashWindow mSplash;
        /// <summary>
        /// Get or set the splash screen window
        /// </summary>
        public static MySpalashWindow Splash
        {
            get
            {
                return mSplash;
            }
            set
            {
                mSplash = value;
            }
        }
        public static System.TimeSpan m_FadeOutTime = new System.TimeSpan(0, 0, 0, 3);
        public static System.TimeSpan m_FadeInTime = new System.TimeSpan(0, 0, 0, 3);
        /// <summary>
        /// Show splash screen
        /// </summary>
        public static void ShowSplash(bool autoClose, bool topMost)
        {
            if (mSplash != null)
            {
                mSplash.Show();
            }
        }
        public static int GetCloseTime()
        {
            int iCloseTime = Convert.ToInt32(m_FadeOutTime.TotalMilliseconds);
            return iCloseTime;
        }
        /// <summary>
        /// Close splash screen
        /// </summary>
        public static void CloseSplash()
        {
            if (mSplash != null)
            {
                mSplash.BeginFadeOut();
/*
                if (mSplash is IDisposable)
                    (mSplash as IDisposable).Dispose();*/
            }
        }
    } 
}
