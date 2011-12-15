using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using System.Media;
using System.IO;

namespace LOLVoiceExtractor
{
    public class MusicPlayer
    {
        //private System.Windows.Media.MediaPlayer mPlayer;
        private string mSoundsDir;
        private string mVoiceDir;
        private FMOD.System mSystem = null;
        private FMOD.Channel channel = null;
        private uint ms = 0;
        private uint lenms = 0;
        private bool playing = false;
        private bool paused = false;
        private int channelsplaying = 0;
        private System.Windows.Threading.DispatcherTimer m_Timer;
        public  enum eVoice
        {
            VoiceAnni=0,
            VoiceFB,
            VoiceDK,
            VoiceTK,
            VoiceQK,
            VoiceHK,
            VoiceACE,
            VoiceEffort,
            VoiceEffort2,
            VoiceBeginWork,
            VoiceClose,
            VoiceOpen,
            VoiceBeginWork2,
            VoiceNum
        };
        private string[] m_sVoiceSource;
        private FMOD.Sound[] mVoice;
        private FMOD.Sound[] mSound;
        private int m_iSoundCount = 0;
        public MusicPlayer()
        {
             string location = System.Environment.CurrentDirectory;
            //mPlayer = new System.Windows.Media.MediaPlayer();
            mSoundsDir = location+"\\Sound\\";
            mVoiceDir = location+"\\Voice\\";
            IniSystem();
            Load();
             //构造一个DispatcherTimer类实例
            m_Timer= new System.Windows.Threading.DispatcherTimer();
            //设置事件处理函数
            m_Timer.Tick += new EventHandler(timer_Tick);
            //定时器时间间隔1s
            m_Timer.Interval = new System.TimeSpan(0, 0, 0,0,5);
            m_Timer.Start();
        }
        private void IniSystem()
        {
            uint version = 0;
            FMOD.RESULT result;
            /*
                Create a System object and initialize.
            */
            result = FMOD.Factory.System_Create(ref mSystem);
            ERRCHECK(result);
            result = mSystem.getVersion(ref version);
            ERRCHECK(result);
            if (version < FMOD.VERSION.number)
            {
               System.Windows.MessageBox.Show("Error!  You are using an old version of FMOD " + version.ToString("X") + ".  This program requires " + FMOD.VERSION.number.ToString("X") + ".");
                return;
            }
            result = mSystem.init(32, FMOD.INITFLAG.NORMAL, (IntPtr)null);
            ERRCHECK(result);
        }
        private void Load()
        {

            FMOD.RESULT result;
            //LoadVoice
            m_sVoiceSource = new string[(int)eVoice.VoiceNum];
            mVoice = new FMOD.Sound[(int)eVoice.VoiceNum];
            m_sVoiceSource[(int)eVoice.VoiceAnni] = "AnnieFun.mp3";
            m_sVoiceSource[(int)eVoice.VoiceFB] = "female1_OnFirstBlood_1.mp3";
            m_sVoiceSource[(int)eVoice.VoiceDK] = "female1_OnChampionKillYouHeroY.mp3";
            m_sVoiceSource[(int)eVoice.VoiceTK] = "female1_OnChampionTripleKillYo.mp3";
            m_sVoiceSource[(int)eVoice.VoiceQK] = "AnnieFun.mp3";
            m_sVoiceSource[(int)eVoice.VoiceHK] = "female1_OnChampionPentaKillYou_1.mp3";
            m_sVoiceSource[(int)eVoice.VoiceACE] = "female1_OnAce_1.mp3";
            m_sVoiceSource[(int)eVoice.VoiceEffort] = "BlindMonk.effort21.mp3";
            m_sVoiceSource[(int)eVoice.VoiceEffort2] = "Lux.ultimateeffort1.mp3";
            m_sVoiceSource[(int)eVoice.VoiceBeginWork] = "Jester.joke.mp3";
            m_sVoiceSource[(int)eVoice.VoiceClose] = "Twitch.dying3_1.mp3";
            m_sVoiceSource[(int)eVoice.VoiceOpen] = "sona_ariaofperserverance_melod_5.mp3";
            m_sVoiceSource[(int)eVoice.VoiceBeginWork2] = "Jester.laugh3.mp3";
            
            for (int i = 0; i < (int)eVoice.VoiceNum; ++i)
            {
                result = mSystem.createSound(mVoiceDir+m_sVoiceSource[i], FMOD.MODE.HARDWARE, ref mVoice[i]);
                ERRCHECK(result);
            }

            //LoadSounds
            DirectoryInfo dir = new DirectoryInfo(mSoundsDir); 
             //不是目录 
              if (dir == null) 
                  return;
            FileSystemInfo[] files = dir.GetFiles("*.mp3", SearchOption.AllDirectories);
            m_iSoundCount = (int)files.Length;
            mSound = new FMOD.Sound[m_iSoundCount];
            for (int i = 0; i < files.Length; i++)
            {
                FileInfo file = files[i] as FileInfo;
                //是文件 
                if (file != null)
                {
                    result = mSystem.createSound(file.FullName,FMOD.MODE.HARDWARE, ref mSound[i]);
                    ERRCHECK(result);
                }
            }
            /* var vSoundList= (from file in Directory.EnumerateFiles(mSoundsDir, "*.mp3", SearchOption.AllDirectories)
                        select file).ToArray;
           m_iSoundCount = (int)vSoundList.Count;
            mSound = new FMOD.Sound[m_iSoundCount];
            for (int i = 0; i < m_iSoundCount;++i )
            {
                result = mSystem.createSound(mSoundsDir+vSoundList[i], FMOD.MODE.HARDWARE, ref mSound[i]);
                ERRCHECK(result);
            }   */ 
        }
        ~MusicPlayer()
        {
            FMOD.RESULT result;
            /*
                Shut down
            */
            for(int i=0;i<m_iSoundCount;++i)
                if (mSound[i] != null)
                {
                    if(mSound[i]!=null)
                    {  
                        result = mSound[i].release();
                        ERRCHECK(result);
                    }
                }
            for (int i = 0; i < (int)eVoice.VoiceNum; ++i)
                if (mVoice[i] != null)
                {
                     if(mVoice[i]!=null)
                    {  
                        result = mVoice[i].release();
                        ERRCHECK(result);
                     }
                }
            if (mSystem != null)
            {
                result = mSystem.close();
                ERRCHECK(result);
                result = mSystem.release();
                ERRCHECK(result);
            }
        }
        public void Play(ref FMOD.Sound  snd)
        {
            FMOD.RESULT result;
            result = mSystem.playSound(FMOD.CHANNELINDEX.FREE, snd, false, ref channel);
            ERRCHECK(result);
            //string location = System.Environment.CurrentDirectory + "\\Sound2\\AnnieFun.wav";
            /*SoundPlayer player = new SoundPlayer(location);
             //player.SoundLocation=location;
            if(player.Stream!=null)
                 player.Play();
             //mPlayer.Stop();
             mPlayer.Open(new Uri(@"Sound\\AnnieFun.mp3", UriKind.Relative));
             mPlayer.Play();*/
            //mediaElement1.URL = @location;
            //mediaElement1.Ctlcontrols.play();        //播放
        }
        public void PlaySound()
        {
            Random rnd1 = new Random();
            int i = rnd1.Next(0, m_iSoundCount- 1);
            Play(ref mSound[(int)i]);
        }
        public void PlayVoice(eVoice iVoice)
        {
            if (iVoice < 0 || iVoice >= eVoice.VoiceNum)
                return;
            Play(ref mVoice[(int)iVoice]);
        }
        
        private void timer_Tick(object sender, System.EventArgs e)
        {
            FMOD.RESULT result;

            if (channel != null)
            {
                FMOD.Sound currentsound = null;
                result = channel.isPlaying(ref playing);
                if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE) && (result != FMOD.RESULT.ERR_CHANNEL_STOLEN))
                {
                    ERRCHECK(result);
                }
                result = channel.getPaused(ref paused);
                if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE) && (result != FMOD.RESULT.ERR_CHANNEL_STOLEN))
                {
                    ERRCHECK(result);
                }
                result = channel.getPosition(ref ms, FMOD.TIMEUNIT.MS);
                if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE) && (result != FMOD.RESULT.ERR_CHANNEL_STOLEN))
                {
                    ERRCHECK(result);
                }
                channel.getCurrentSound(ref currentsound);
                if (currentsound != null)
                {
                    result = currentsound.getLength(ref lenms, FMOD.TIMEUNIT.MS);
                    if ((result != FMOD.RESULT.OK) && (result != FMOD.RESULT.ERR_INVALID_HANDLE) && (result != FMOD.RESULT.ERR_CHANNEL_STOLEN))
                    {
                        ERRCHECK(result);
                    }
                }
            }
            if (mSystem != null)
            {
                mSystem.getChannelsPlaying(ref channelsplaying);
                mSystem.update();
            }
            //string statusBarText = "Time " + ms / 1000 / 60 + ":" + ms / 1000 % 60 + ":" + ms / 10 % 100 + "/" + lenms / 1000 / 60 + ":" + lenms / 1000 % 60 + ":" + lenms / 10 % 100+ " : " + (paused ? "Paused " : playing ? "Playing" : "Stopped") + " : Channels Playing " + channelsplaying;
        }

        private void ERRCHECK(FMOD.RESULT result)
        {
            if (result != FMOD.RESULT.OK)
            {
#if DEBUG
                m_Timer.Stop();
                System.Windows.MessageBox.Show("FMOD error! " + result + " - " + FMOD.Error.String(result));
               //Environment.Exit(-1);
#endif
                }
        }
    }
}