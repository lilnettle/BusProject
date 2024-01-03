using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ClientBus.Inafrac.Commands;
using ClientBus.ViewModels.Base;
using System.Windows;
using ClientBus.Services;

namespace ClientBus.ViewModels
{
    internal class MainWindowViewModel : ViewModel
    {
      
        private string _Title = "Реєстрація/авторизація";

        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }

     

        #region Клік по кнопці зареєструватись
        public ICommand RegisterButton_Click_Command { get; }
        private bool CanRegisterButton_ClickExecuted(object p) => true;
        private void OnRegisterButton_ClickExecuted(object p)
        {
            RegisterWindow registerWindow = new RegisterWindow();
            registerWindow.Show();

            if (p is Window window)
            {
                window.Close();
            }
        }
        #endregion

        #region Клік по кнопці увійти
        public ICommand GoTologin_Click_Command { get; }
        private bool CanGoTologin_ClickExecute(object p) => true;
        private void OnGoTologin_ClickExecuted(object p)
        {
            // Створюємо нове вікно реєстрації
            LoginWindow loginWindow = new LoginWindow();

            // Відкриваємо вікно реєстрації
            loginWindow.Show();

            if (p is Window window)
            {
                window.Close();
            }

        }
        #endregion


        public MainWindowViewModel()
        {
            RegisterButton_Click_Command = new RelayCommand(OnRegisterButton_ClickExecuted, CanRegisterButton_ClickExecuted);
            GoTologin_Click_Command = new LambdaCommand(OnGoTologin_ClickExecuted, CanGoTologin_ClickExecute);


            }

    }
}
