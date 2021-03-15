using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public double threadSum, taskSum = 0;
        //public int[] Parameters = new int[2];

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

            // Creating a thread with params.
            // The thread encapsulates the method.
            // Can you do method calls within that method and it stays in that thread?
            //var ts = new ParameterizedThreadStart(FindPrimeNumbers); 

            /* Threads without callback.
            Thread t = new Thread(ts);
            t.Start(20000); // Start the thread/method, pass in the methods parameter.
            */

            // Because we have a callback method with IAsyncResult parameter,
            // We can forgo the explicit thread and invoke it through a Task.
            //Parameters[0] = 0;
            //Parameters[1] = 2000;
            //var t = Task.Run(() => ts.Invoke(Parameters)); // Invoke the function that the thread delegates, with its parameters.
            // .ContinueWith operates on the returned task. When task completes, continue with is ran.
            // The task object will keep us updated with the status of the task, started, running, completed.
            //t.ContinueWith(FindPrimesFinished); // Run this callback method when the core task finishes.



            //for (var i = 0; i < 100; i++)
            //{
            //    // Delegate, here is the function we would like to run. Not the only way to do this.
            //    var pts = new ParameterizedThreadStart(FindPrimeNumbers);
            //    var sw = new Stopwatch();
            //    var toFind = 2000;
            //    var other = 0;
            //    sw.Start();
            //    pts += (param) => // Adding new anonymous function.
            //    {
            //        // Callback.
            //        sw = (Stopwatch)param;
            //        threadSum += sw.Elapsed.Ticks;
            //    };
            //    var thread = new Thread(pts);
            //    thread.Start(sw);
            //}

            // Make 100 tasks
            for (var i = 0; i < 100; i++)
            {
                // Delegate, here is the function we would like to run. Not the only way to do this.
                var ts = new ParameterizedThreadStart(FindPrimeNumbers); // This is better than in the video.
                var sw = new Stopwatch();
                sw.Start();
                Task.Run(() => ts.Invoke(new object[] { sw, 2000, 0 })) // WOOOOO, this is how you pass in params.
                    .ContinueWith(result =>
                    {
                        // Callback
                        taskSum += sw.Elapsed.Ticks;
                    });
            }
        }

        // Changed method param to object.
        private void FindPrimeNumbers(object param)
        {

            var sw = (Stopwatch) ((object[])param)[0]; // Extract stopwatch from param array.
            var primeCount = 0;
            var numberOfPrimesToFind = (int)((object[])param)[1];
            var currentPossiblePrime = (int)((object[])param)[2];
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
                    sw.Stop();
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
            tb_threadticks.Text = "Thread ticks: " + threadSum / 100; // Divide by 100 to get average number.
            tb_taskticks.Text = "Task ticks: " + taskSum / 100;
        }
    }
}