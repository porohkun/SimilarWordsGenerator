using Microsoft.Win32;
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
    /// Interaction logic for ProfilePage.xaml
    /// </summary>
    public partial class ProfilePage : Page
    {
        public static GenProfile Profile;

        Brush BrushZero = new SolidColorBrush(Color.FromRgb(124, 207, 194));
        Brush BrushZeroRow0 = new SolidColorBrush(Color.FromRgb(169, 208, 225));
        Brush BrushZeroRow1 = new SolidColorBrush(Color.FromRgb(148, 219, 210));
        Brush BrushRow0 = new SolidColorBrush(Color.FromRgb(201, 228, 242));
        Brush BrushZeroCol0 = new SolidColorBrush(Color.FromRgb(159, 223, 201));
        Brush BrushZeroCol1 = new SolidColorBrush(Color.FromRgb(140, 217, 202));
        Brush BrushCol0 = new SolidColorBrush(Color.FromRgb(184, 235, 217));
        Brush BrushCross = new SolidColorBrush(Color.FromRgb(164, 229, 218));

        public ProfilePage()
        {
            InitializeComponent();
        }

        private void New_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Multiselect = true;
            if (ofd.ShowDialog(MainWindow.Instance).Value)
            {
                var profile = Parser.Parse(ofd.FileNames.Select(f => System.IO.File.ReadAllText(f)), @"\b([a-zA-Z]+)\b");
                var sfd = new SaveFileDialog();
                if (sfd.ShowDialog(MainWindow.Instance).Value)
                {
                    profile.SaveToFile(sfd.FileName);
                    LoadProfile(sfd.FileName);
                }
            }
        }

        private void Open_Click(object sender, RoutedEventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            if (ofd.ShowDialog(MainWindow.Instance).Value)
            {
                LoadProfile(ofd.FileName);
            }
        }

        private void LoadProfile(string filename)
        {
            Profile = GenProfile.LoadFromFile(filename);

            _grid.Children.Clear();
            _grid.RowDefinitions.Clear();
            _grid.ColumnDefinitions.Clear();

            for (int i = 0; i <= Profile.Length; i++)
            {
                _grid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(25) });
                _grid.RowDefinitions.Add(new RowDefinition() { Height = new GridLength(25) });
            }

            for (int i = -1; i < Profile.Length; i++)
                for (int j = -1; j < Profile.Length; j++)
                {
                    var tt = new TextBlock();
                    if (i == -1 || j == -1)
                        tt.Text = Profile.GetChar(Math.Max(i, j));
                    else
                    {
                        var t = Profile.GetWeight(i, j);
                        tt.Text = t == 0 ? "" : t.ToString();
                    }
                    var i2 = i % 2 == 0;
                    var j2 = j % 2 == 0;
                    if (i == -1 && j == -1)
                        tt.Background = BrushZero;
                    else if (i == -1)
                        tt.Background = j2 ? BrushZeroRow0 : BrushZeroRow1;
                    else if (j == -1)
                        tt.Background = i2 ? BrushZeroCol0 : BrushZeroCol1;
                    else if (i2 && !j2)
                        tt.Background = BrushCol0;
                    else if (j2 && !i2)
                        tt.Background = BrushRow0;
                    else if (!i2 && !j2)
                        tt.Background = BrushCross;
                    _grid.Children.Add(tt);
                    tt.SetValue(Grid.RowProperty, i + 1);
                    tt.SetValue(Grid.ColumnProperty, j + 1);
                }
        }
    }
}
