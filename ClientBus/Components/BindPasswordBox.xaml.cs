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

namespace ClientBus.Components
{
    /// <summary>
    /// Логика взаимодействия для BindPasswordBox.xaml
    /// </summary>
    public partial class BindPasswordBox : UserControl
    {
        public static DependencyProperty PasswordProperty =
            DependencyProperty.Register("Password", typeof(string), typeof(BindPasswordBox), new PropertyMetadata(string.Empty));

        public string Password
        {
            get { return (string)GetValue(PasswordProperty); }
            set { SetValue(PasswordProperty, value); }
        }
        public BindPasswordBox()
        {
            InitializeComponent();
        }
      

        private void PasswordBox1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            Password = PasswordBox.Password;
        }
    }
}
