using ClientBus.Inafrac.Commands;
using ClientBus.Models;
using ClientBus.Services;
using ClientBus.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ClientBus.ViewModels
{
    internal class TicketWindowViewModel : ViewModel
    {
        private readonly ServiceUser _serviceUser;
        private readonly ServiceTicket _serviceTicket;

        private ObservableCollection<TicketsModel> _tickets;



        public ObservableCollection<TicketsModel> Tickets
        {
            get { return _tickets; }
            set { Set(ref _tickets, value); }
        }


        public LambdaCommand TicketShowButton_Click_Command { get; }
        private bool CanTicketShowButton_ClickExecuted(object p) => true;
        private async void OnTicketShowButton_ClickExecuted(object p)
        {
            
                // Перевірка наявності з'єднання з сервером
              
                try
                {
                    // Викликайте метод з вашого сервісу
                    var userTickets = await _serviceUser.GetUserTicketsAsync();

                    // Обробка результатів, наприклад, виведення їх на екран
                    if (userTickets == null || !userTickets.Any())
                    {
                        MessageBox.Show("Квитків не знайдено");
                        Tickets = new ObservableCollection<TicketsModel>(); // Оновити колекцію Trips пустою
                        return;
                    }

                    // Оновити колекцію Trips у вашій ViewModel
                    Tickets = new ObservableCollection<TicketsModel>(userTickets);
                }
                catch (Exception ex)
                {
                    // Вивести виняток у MessageBox
                    MessageBox.Show($"Error while retrieving user tickets: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
           
        }

        public LambdaCommand TicketDelButton_Click_Command { get; }
        private bool CanTicketDelButton_ClickExecuted(object p) => true;
        private async void OnTicketDelButton_ClickExecuted(object p)
        {

           
                
                if (p is TicketsModel selectedTicket)
                {
                    try
                    {
                        string userEmail = await _serviceUser.GetUserEmailAsync();
                        bool isSuccess = await _serviceTicket.DeleteTicketAsync(selectedTicket.ID, userEmail);

                        if (isSuccess)
                        {
                            MessageBox.Show("Квиток успішно скасовано");
                        }
                        else
                        {
                            MessageBox.Show("Помилка при скасувані квитка");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Помилка при скасувані квитка: {ex.Message}");
                    }

                }
            

          

        }

        public LambdaCommand ExitCommand1 { get; }
        private bool CanExitExecuted1(object p) => true;

        private async void OnExitExecuted1(object p)
        {
            WorkWindow workWindow = new WorkWindow();
            workWindow.Show();

            if (p is Window window)
            {
                window.Close();
            }
        }



        public TicketWindowViewModel()
                {
                    _serviceTicket = new ServiceTicket("https://localhost:7283");
                    _serviceUser = new ServiceUser("https://localhost:7283");
                    TicketShowButton_Click_Command = new LambdaCommand(OnTicketShowButton_ClickExecuted, CanTicketShowButton_ClickExecuted);
                    TicketDelButton_Click_Command = new LambdaCommand(OnTicketDelButton_ClickExecuted, CanTicketDelButton_ClickExecuted);
            ExitCommand1 = new LambdaCommand(OnExitExecuted1, CanExitExecuted1);
        }
            }
        } 
