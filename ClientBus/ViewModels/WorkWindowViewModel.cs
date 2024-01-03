using ClientBus.Services;
using ClientBus.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientBus.Services;
using ClientBus.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using ClientBus.Inafrac.Commands;
using ClientBus.Models;
using System.Windows;

namespace ClientBus.ViewModels
{
    internal class WorkWindowViewModel : ViewModel
    {
        private readonly ServiceUser _apiService;
        private readonly ServiceTicket _serviceTicket;
        private readonly ServiceUser _serviceUser;
        private string _emailBox;
        public string UserEmail
        {
            get => _emailBox;
            set => Set(ref _emailBox, value);

        }

        private ObservableCollection<TripModel> _trips;



        public ObservableCollection<TripModel> Trips
        {
            get { return _trips; }
            set { Set(ref _trips, value); }
        }

        private ObservableCollection<string> _startcity;

        private ObservableCollection<string> _finishcity;
        private ObservableCollection<string> _departuredate;

        public ObservableCollection<string> StartCity {
            get { return _startcity; }
            set
            {
                if (_startcity != value)
                {
                    Set(ref _startcity, value);
                }
            }

        }

        private string _startCitySelectedValue;
        public string StartCitySelectedValue
        {
            get { return _startCitySelectedValue; }
            set { Set(ref _startCitySelectedValue, value); }
        }

        private string _finishCitySelectedValue;
        public string FinishCitySelectedValue
        {
            get { return _finishCitySelectedValue; }
            set { Set(ref _finishCitySelectedValue, value); }
        }

        private string _departureDateSelectedValue;
        public string DepartureDateSelectedValue
        {
            get { return _departureDateSelectedValue; }
            set { Set(ref _departureDateSelectedValue, value); }
        }


        public ObservableCollection<string> FinishCity
        {
            get { return _finishcity; }
            set
            {
                if (_finishcity != value)
                {
                    Set(ref _finishcity, value);
                }
            }

        }


        public ObservableCollection<string> DepartureDate
        {
            get { return _departuredate; }
            set
            {
                if (_departuredate != value)
                {
                    Set(ref _departuredate, value);
                }
            }

        }

        public LambdaCommand ResearchButton_Click_Command { get; }
        private bool CanResearchButton_ClickExecuted(object p) => true;
        private async void OnResearchButton_ClickExecuted(object p)
        {

           
                // Перевірка наявності з'єднання з сервером
               

                // Перевірка на пусті значення
                {
                    // Перевірка на пусті значення
                    if (string.IsNullOrEmpty(StartCitySelectedValue) ||
                        string.IsNullOrEmpty(FinishCitySelectedValue) ||
                        DepartureDateSelectedValue == default)
                    {
                        // Обробка помилки або виведення повідомлення
                        MessageBox.Show("Будь ласка, заповніть всі поля перед пошуком.");
                        return;
                    }

                    var criteria = new TripSearchModel
                    {
                        StartCity = StartCitySelectedValue,
                        FinishCity = FinishCitySelectedValue,
                        DepartureDate = DepartureDateSelectedValue
                    };

                    try
                    {
                        // Викликати сервіс та отримати результати пошуку
                        var serviceTrip = new ServiceTrip("https://localhost:7283");
                        var trips = await serviceTrip.SearchTripsAsync(criteria);

                        // Перевірка на наявність результатів
                        if (trips == null || !trips.Any())
                        {
                            MessageBox.Show("Маршрутів не знайдено.");
                            Trips = new ObservableCollection<TripModel>(); // Оновити колекцію Trips пустою
                            return;
                        }

                        // Оновити колекцію Trips у вашій ViewModel
                        Trips = new ObservableCollection<TripModel>(trips);
                    }
                    catch (Exception ex)
                    {
                        // Обробка помилок
                        MessageBox.Show($"Помилка під час пошуку маршрутів: {ex.Message}");
                    }

                    // Звернення до сервісу користувача
                    try
                    {
                        string userEmail = await _apiService.GetUserEmailAsync();
                        UserEmail = userEmail;
                    }
                    catch (Exception ex)
                    {
                        // Обробка помилок
                        MessageBox.Show($"Помилка при отриманні електронної пошти користувача: {ex.Message}");
                    }
                }
            
           
        }


        public LambdaCommand CreateTicketCommand { get; }
        private bool CanCreateTicketExecuted(object parameter) => true;

        private async void OnCreateTicketExecuted(object parameter)
        {
            
               

                

                if (parameter is TripModel selectedTrip)
                {
                    try
                    {
                        // Викликати сервіс та отримати результати створення квитка
                        var serviceTicket = new ServiceTicket("https://localhost:7283");
                        var userEmail = await _apiService.GetUserEmailAsync();

                        // Вивести діалогове вікно для підтвердження
                        MessageBoxResult result = MessageBox.Show($"Ви впевнені, що хочете створити квиток для маршруту {selectedTrip.NumberBus}?", "Підтвердження створення квитка", MessageBoxButton.YesNo);

                        if (result == MessageBoxResult.Yes)
                        {

                            bool ticketCreated = await serviceTicket.CreateTicketAsync(selectedTrip.Id, userEmail);

                            if (ticketCreated)
                            {
                                MessageBox.Show("Квиток успішно створено.");
                            }
                            else
                            {
                                MessageBox.Show("Не вдалося створити квиток.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Помилка при створенні квитка: {ex.Message}");
                    }
                }
            

           
        }
        
    

        public LambdaCommand MenuTicketCommand { get; }
        private bool CanManuTicketExecuted(object p) => true;

        private async void OnManuTicketExecuted(object p)
        {
            TicketWindow ticketWindow = new TicketWindow ();
            ticketWindow.Show();

            if (p is Window window)
            {
                window.Close();
            }
        }

        public LambdaCommand ExitCommand { get; }
        private bool CanExitExecuted(object p) => true;

        private async void OnExitExecuted(object p)
        {
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();

            if (p is Window window)
            {
                window.Close();
            }
        }


        public WorkWindowViewModel()
        {

            StartCity = new ObservableCollection<string>
        {
            "Львів"
           
            // Додайте інші елементи, які вам потрібно
        };

            FinishCity = new ObservableCollection<string>
        {
            "Київ",
            "Рівне",
            "Тернопіль"
           
            // Додайте інші елементи, які вам потрібно
        };

            DepartureDate = new ObservableCollection<string>
        {
            "18.12.2023",
            "19.12.2023"
           
            // Додайте інші елементи, які вам потрібно
        };


            _serviceTicket = new ServiceTicket("https://localhost:7283");
            _apiService = new ServiceUser("https://localhost:7283");
            ResearchButton_Click_Command = new LambdaCommand(OnResearchButton_ClickExecuted, CanResearchButton_ClickExecuted);
            CreateTicketCommand = new LambdaCommand(OnCreateTicketExecuted, CanCreateTicketExecuted);
            MenuTicketCommand = new LambdaCommand(OnManuTicketExecuted, CanManuTicketExecuted);
            ExitCommand = new LambdaCommand(OnExitExecuted, CanExitExecuted);

        }
       
    }
    

    

    
}
