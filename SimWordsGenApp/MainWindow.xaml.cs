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
using Microsoft.Win32;
using System.ComponentModel;
using System.Text.RegularExpressions;
using System.Globalization;
using SimWordsGenApp.Pages;

namespace SimWordsGenApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow Instance { get; private set; }
        private static Dictionary<Type, Page> _pages = new Dictionary<Type, Page>();

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
            _menuFrame.Navigate(new MenuPage());
            SwitchPage< ProfilePage>();
        }

        public static void SwitchPage<T>() where T:Page
        {
            var type = typeof(T);
            if (!_pages.ContainsKey(type))
                _pages.Add(type, (T)type.GetConstructor(Type.EmptyTypes).Invoke(null));
            Instance._mainFrame.Navigate(_pages[type]);
        }
    }
}
