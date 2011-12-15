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

namespace WpfApplication1
{
    /// <summary>
    /// Page1.xaml 的交互逻辑
    /// NavigationService 封装了在浏览器样式的导航上下文中下载内容的能力
    /// 通过 URI 导航到内容时，NavigationService 将返回一个包含该内容的对象。 


    /// </summary>
    public partial class Page1 : Page
    {
        public Page1()
        {
            InitializeComponent();
        }
        private void Page_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new Uri("Index.xaml", UriKind.Relative));
        }
    }
}
