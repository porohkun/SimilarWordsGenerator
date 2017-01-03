using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SimWordsGenApp.Pages
{
    /// <summary>
    /// Interaction logic for GeneratorTestPage.xaml
    /// </summary>
    public partial class GeneratorTestPage : Page
    {
        public GeneratorTestPage()
        {
            InitializeComponent();
        }

        private void Generate_Click(object sender, RoutedEventArgs e)
        {
            if (ProfilePage.Profile==null)
            {
                resultBox.Text = "Profile must be loaded";
                return;
            }
            resultBox.Clear();
            for (int i = 0; i < 20; i++)
                resultBox.Text += ProfilePage.Profile.GetRandomWord((int)wordLength.Value) + "\r\n";
        }
    }
}
