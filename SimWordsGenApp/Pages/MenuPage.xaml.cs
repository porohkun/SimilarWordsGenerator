using System.Windows;
using System.Windows.Controls;

namespace SimWordsGenApp.Pages
{
    /// <summary>
    /// Interaction logic for MenuPage.xaml
    /// </summary>
    public partial class MenuPage : Page
    {
        public MenuPage()
        {
            InitializeComponent();
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            //MainWindow.SwitchPage<ProfilePage>();
        }

        private void Generator_Click(object sender, RoutedEventArgs e)
        {
            //MainWindow.SwitchPage<GeneratorTestPage>();
        }
    }
}
