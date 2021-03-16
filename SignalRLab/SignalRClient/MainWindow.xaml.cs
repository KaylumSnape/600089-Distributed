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
using Microsoft.AspNetCore.SignalR.Client;

namespace SignalRClient
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        protected HubConnection Connection;
        public MainWindow()
        {
            InitializeComponent();

            // Configure the connection to our hub.
            // Matches the route specified in Startup.cs.
            Connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:58008/chathub")
                .Build();

            // When a connection is received from the hub with the method name "GetMessage" and two string parameters.
            // Create an action instance to handle the "GetMessage" procedure call.
            // The action has a delegate (method) that carries out the work.
            Connection.On<string, string>("GetMessage",
                new System.Action<string, string>((username, message) => // Create new action with params.
                    GetMessage(username, message))); // Delegate to this method, pass in params.
        }

        // Method to update ListBox (LbMessage).
        private void GetMessage(string username, string message)
        {
            // this.Dispatcher, we need THIS UI thread.
            this.Dispatcher.Invoke(() =>
            {
                var chat = $"{username}: {message}";
                LbMessage.Items.Add(chat);
            });
        }

        // Connect to the server.
        private async void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await Connection.StartAsync();
                LbMessage.Items.Add("Connection opened.");
            }
            catch
            {
                LbMessage.Items.Add("Connection failed.");
            }
        }

        // Send a message to the server.
        private async void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Invoke the async task on the chathub called "BroadcastMessage".
                await Connection.InvokeAsync("BroadcastMessage", TbUserName.Text, TbMessage.Text);
            }
            catch (Exception exception)
            {
                LbMessage.Items.Add(exception.Message);
            }
        }
    }
}
