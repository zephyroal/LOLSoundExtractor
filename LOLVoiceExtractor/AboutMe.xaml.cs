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

namespace LOLVoiceExtractor
{
    /// <summary>
    /// Interaction logic for AboutMe.xaml
    /// </summary>
    public partial class AboutMe : Window
    {
        private static BitmapImage btnImage;
        private static BitmapImage btnImageDown;
        private static BitmapImage btnImageOver;
        System.IO.Stream streamBack;
        System.IO.Stream streamBackDown ;
        System.IO.Stream streamBackOver;
        ImageSourceConverter imgConverter;
        public AboutMe()
        {
            InitializeComponent();

            System.Reflection.Assembly tManifestAssembly = System.Reflection.Assembly.GetExecutingAssembly();
            System.IO.Stream stream0 = tManifestAssembly.GetManifestResourceStream(typeof(App), "Images.AboutMeBack.jpg");
            System.IO.Stream stream1 = tManifestAssembly.GetManifestResourceStream(typeof(App), "Images.3089_Banksys_Wizard_Hat.png");

            streamBack = tManifestAssembly.GetManifestResourceStream(typeof(App), "Images.Back.png");
            streamBackDown = tManifestAssembly.GetManifestResourceStream(typeof(App), "Images.Back_Down.png");
            streamBackOver = tManifestAssembly.GetManifestResourceStream(typeof(App), "Images.Back_Over.png");
           
            imgConverter = new ImageSourceConverter();
            this.image1.Source = (ImageSource)imgConverter.ConvertFrom(stream0);
            this.image2.Source = (ImageSource)imgConverter.ConvertFrom(stream1);
            this.image3.Source = (ImageSource)imgConverter.ConvertFrom(streamBack);

            btnImage = new BitmapImage();
            btnImage.BeginInit();
            btnImage.StreamSource = streamBack;
            //btnImage.UriSource = new Uri("/Images/Back.png", UriKind.Relative);
            btnImage.EndInit();

            btnImageDown = new BitmapImage();
            btnImageDown.BeginInit();
            btnImageDown.StreamSource = streamBackDown;
            //btnImageDown.UriSource = new Uri("/Images/Back_Down.png", UriKind.Relative);
            btnImageDown.EndInit();

            btnImageOver = new BitmapImage();
            btnImageOver.BeginInit();
            btnImageOver.StreamSource = streamBackOver;
            //btnImageOver.UriSource = new Uri("/Images/Back_Over.png", UriKind.Relative);
            btnImageOver.EndInit();
           /* 
            btnImage = new BitmapImage();
            btnImage.BeginInit();
            btnImage.UriSource = new Uri("/Images/Back.png", UriKind.Relative);
            btnImage.EndInit();
            */
            System.IO.Stream stream3 = tManifestAssembly.GetManifestResourceStream(typeof(App), "Images.Cursor_7.cur");
            this.Cursor = new System.Windows.Input.Cursor(stream3);
        }

        private void image3_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.Close();
            //NavigationService.Navigate(new Uri("MainWindow.xaml", UriKind.Relative));
        }

        private void image3_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.image3.Source =btnImageDown;

        }
        private void image3_MouseEnter(object sender, MouseEventArgs e)
        {
            this.image3.Source =btnImageOver;
        }

        private void image3_MouseLeave(object sender, MouseEventArgs e)
        {
            this.image3.Source =btnImage;
        }
    }
}
