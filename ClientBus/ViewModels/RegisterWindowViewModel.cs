using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ClientBus.Inafrac.Commands;
using ClientBus.ViewModels.Base;
using System.Windows;
using PhoneNumbers;
using ClientBus.Services;
using ClientBus.Models;

namespace ClientBus.ViewModels
{
    internal class RegisterWindowViewModel: ViewModel

    {


        private readonly ServiceUser _apiService;

        #region Титульний надпис
        private string _Title = "Реєстрація";

        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        #endregion

        private string _phoneNumber;
        private string _password;
        private string _confirmPassword;
        private string _email;

        public string PhoneNumber
        {
            get => _phoneNumber;
            set => Set(ref _phoneNumber, value);
        }

       public string Password
        {
            get => _password;
            set => Set(ref _password, value);
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set => Set(ref _confirmPassword, value);
        }
       
        public string Email
        {
            get => _email;
            set => Set(ref _email, value);
        }



        private bool IsUkrainePhoneNumberValid(string phoneNumber)
        {
            PhoneNumberUtil phoneNumberUtil = PhoneNumberUtil.GetInstance();
            try
            {
                PhoneNumber parsedPhoneNumber = phoneNumberUtil.Parse(phoneNumber, "UA");
                return phoneNumberUtil.IsValidNumber(parsedPhoneNumber);
            }
            catch (NumberParseException)
            {
                return false;
            }
        }

      

        private void SetValidationError(string propertyName, string errorMessage)
        {
            MessageBox.Show(errorMessage);
            
        }

        public LambdaCommand RegisterButton_Click_Command2 { get; }
        private bool CanRegisterButton_ClickExecuted2(object p) => true;
        private async void OnRegisterButton_ClickExecuted2(object p)
        {

            if (_apiService == null)
            {
                MessageBox.Show("Сервіс не ініціалізований. Перевірте підключення до сервера.");
                return;
            }

            bool isValid = true;

                if (string.IsNullOrEmpty(PhoneNumber) || !IsUkrainePhoneNumberValid(PhoneNumber))
                {
                    SetValidationError(nameof(PhoneNumber), "Номер телефону для України невірний.");
                    isValid = false;
                }

                if (string.IsNullOrEmpty(Password) || Password.Length < 6)
                {
                    SetValidationError(nameof(Password), "Пароль має містити щонайменше 6 символів.");
                    isValid = false;
                }

                if (Password != ConfirmPassword)
                {
                    SetValidationError(nameof(ConfirmPassword), "Паролі не співпадають.");
                    isValid = false;
                }

                if (string.IsNullOrEmpty(Email) || Email.Length < 5 || !Email.Contains("@") || !Email.Contains("."))
                {
                    SetValidationError(nameof(Email), "Email введено не коректно.");
                    isValid = false;
                }

                if (isValid)
                {
                    // Виклик методу RegisterUserAsync з ServiceUser
                    bool registrationSuccess = await _apiService.RegisterUserAsync(new User
                    {
                        PhoneNumber = PhoneNumber,
                        Password = Password,
                        Email = Email
                    });

                    if (registrationSuccess)
                    {
                        MessageBox.Show("Успішна реєстрація");
                        LoginWindow loginwindow = new LoginWindow();
                        loginwindow.Show();

                        if (p is Window window)
                        {
                            window.Close();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Не вдалося зареєструвати користувача.");
                    }
                }
            
           
        }
        


        public RegisterWindowViewModel()
        {
            _apiService = new ServiceUser("https://localhost:7283");
            RegisterButton_Click_Command2 = new LambdaCommand(OnRegisterButton_ClickExecuted2, CanRegisterButton_ClickExecuted2);

        }

    }
}
