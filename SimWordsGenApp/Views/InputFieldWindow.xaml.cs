using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace SimWordsGenApp.Views
{
    /// <summary>
    /// Interaction logic for InputFieldWindow.xaml
    /// </summary>
    public partial class InputFieldWindow : Window, INotifyPropertyChanged
    {
        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(storage, value)) return false;

            storage = value;
            RaisePropertyChanged(propertyName);

            return true;
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            PropertyChanged?.Invoke(this, args);
        }
        #endregion INotifyPropertyChanged
        //public string Title
        //{
        //    get => _title;
        //    private set => SetProperty(ref _title, value);
        //}
        public string Description
        {
            get => _description;
            private set => SetProperty(ref _description, value);
        }
        public string Value
        {
            get => _value;
            set => SetProperty(ref _value, value);
        }

        //private string _title;
        private string _description;
        private string _value;

        public InputFieldWindow()
        {
            DataContext = this;
            InitializeComponent();
        }

        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void CancelButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        public static string Show(string title, string description, string defaultValue = "")
        {
            var window = new InputFieldWindow();

            window.Title = title;
            window.Description = description;
            window.Value = defaultValue;
            window.Owner = Application.Current.MainWindow;

            var result = window.ShowDialog();
            if (result.Value)
                return window.Value;
            return null;
        }
    }
}
