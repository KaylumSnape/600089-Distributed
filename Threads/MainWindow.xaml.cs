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
using System.Threading;

namespace Threads
{
    /// <summary>
    /// Lab 7 - Threads.
    /// </summary>
    public partial class MainWindow : Window
    {
        public List<int> primeNumbers;
        public int[] parameters = new int[1];

        public MainWindow()
        {
            InitializeComponent();
            primeNumbers = new List<int>();
        }

        private void btn_click_1(object sender, RoutedEventArgs e)
        {
            /* Old code without threading.
            FindPrimeNumbers(20000);
            tb_output.Text = primeNumbers[9999].ToString();
            */

            /*
            Using threads is not the same as simply calling methods,
            there is no return value. When the thread completes, it simply disappears.

            Use Asynchronous Thread Callbacks.
            A Callback function is a method which the thread calls when it finishes.
            */

            // Creating a thread with parameters.
            // The thread encapsulates the method.
            // Can you do method calls within that method and it stays in that thread?
            ParameterizedThreadStart ts = new ParameterizedThreadStart(FindPrimeNumbers); 

            /* Threads without callback.
            Thread t = new Thread(ts);
            t.Start(20000); // Start the thread/method, pass in the methods parameter.
            */

            // Because we have a callback method with IAsyncResult parameter,
            // We can forgo the explicit thread and invoke it through a Task.
            parameters[0] = 0;
            parameters[1] = 2000;
            Task t = Task.Run(() => ts.Invoke(parameters));
            t.ContinueWith(FindPrimesFinished); // Run this callback method when the core task finishes.
        }

        // Changed method param to object.
        private void FindPrimeNumbers(int[] param)
        {
            var primeCount = param[0];
            var numberOfPrimesToFind = param[1];
            int currentPossiblePrime = 1;
            while (primeCount < numberOfPrimesToFind)
            {
                currentPossiblePrime++;
                int possibleFactor = 2;
                bool isPrime = true;
                while ((possibleFactor <= currentPossiblePrime / 2) && (isPrime == true))
                {
                    int possibleFactor2 = currentPossiblePrime / possibleFactor;
                    if (currentPossiblePrime == possibleFactor2 * possibleFactor)
                    {
                        isPrime = false;
                    }

                    possibleFactor++;
                }

                if (isPrime)
                {
                    primeCount++;
                    primeNumbers.Add(currentPossiblePrime);

                    // We can callback from the thread at any time we'd like, such as when we find a prime.
                    // I can use the UI as it keeps updating with these values.
                    this.Dispatcher.Invoke(
                    new Action<int>(UpdateTextBox),
                        new object[] { currentPossiblePrime });
                }
            }
        }
        // Callback method that runs when "FindPrimeNumbers" finishes.
        private void FindPrimesFinished(IAsyncResult iar) 
        { 
            /* Thread safe.
            You are not permitted to make changes to them from other threads.
            Need to get the thread associated wih this WPF form, which is the UI thread.
            Do that using this.Dispatcher.
             */
            // The method and arguments are defined separately.
            this.Dispatcher.Invoke(
                new Action<int>(UpdateTextBox),
                new object[] {primeNumbers[9999]});
        }

        // Needed action to be passed into dispatcher.
        void UpdateTextBox(int number)
        {
            tb_output.Text = number.ToString();
        }
    }
}