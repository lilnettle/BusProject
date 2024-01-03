using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ClientBus.Inafrac.Commands;
using ClientBus.Services;
using ClientBus.ViewModels.Base;

namespace ClientBus.ViewModels
{
    internal class LoginWindowViewModel:ViewModel
    {

        private readonly ServiceUser _apiService;

        #region Титульний надпис
        private string _Title = "Авторизація";
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        #endregion
        private string _password;
        private string _email;
        public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }


        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }

        public LambdaCommand LoginButton_Click_Command { get; }
        private bool CanLoginButton_ClickExecuted(object p) => true;
        private async void OnLoginButton_ClickExecuted(object p)
        {

            // Перевірка наявності з'єднання з сервером
            if (_apiService == null)
            {
                MessageBox.Show("Сервіс не ініціалізований. Перевірте підключення до сервера.");
                return;
            }
            // Проверка наличия данных
            if (string.IsNullOrEmpty(Email) || string.IsNullOrEmpty(Password))
                {
                    // Выводите сообщение или выполняйте необходимые действия для обработки пустых данных
                    MessageBox.Show("Введіть електронну адресу та пароль");
                    return;
                }

                // Вызов асинхронного метода для авторизации
                bool isLoginSuccessful = await _apiService.LoginAsync(Email, Password);


                if (isLoginSuccessful)
                {
                    // Вход выполнен успешно, выполните необходимые действия
                    MessageBox.Show("Ви успішно увійшли");
                    WorkWindow workwindow = new WorkWindow();
                    workwindow.Show();

                    if (p is Window window)
                    {
                        window.Close();
                    }
                }
                else
                {
                    // Выводите сообщение или выполняйте необходимые действия для обработки неудачной авторизации
                    MessageBox.Show("Помилка входу. Перевірте ваші дані та спробуйте знову");
                }
            
            
        }


        public LambdaCommand RLoginButton_Click_Command { get; }
        private bool CanRLoginButton_ClickExecuted(object p) => true;
        private async void OnRLoginButton_ClickExecuted(object p)
        {
            RegisterWindow registerindow = new RegisterWindow();
            registerindow.Show();

            if (p is Window window)
            {
                window.Close();
            }
        }
    

        public LoginWindowViewModel()
        {
            _apiService = new ServiceUser("https://localhost:7283");
            LoginButton_Click_Command = new LambdaCommand(OnLoginButton_ClickExecuted, CanLoginButton_ClickExecuted);
            RLoginButton_Click_Command = new LambdaCommand(OnRLoginButton_ClickExecuted, CanLoginButton_ClickExecuted);
        }



    }
}
