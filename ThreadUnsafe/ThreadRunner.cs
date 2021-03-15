using System;
using System.Threading;

namespace ThreadUnsafe
{
    /* In general we need to be very careful whenever we use threads that need to:
       • Pass values between the threads
       • Use a common storage collection
       • Be synchronised (thread A must get to a certain point in its execution 
       before thread B performs some task)
    */
    public class ThreadRunner
    {
        const int NumberToGenerate = 100;
        private int _index = 0;
        private int[] _orderedNumbers;
        readonly Random _rng = new Random();

        public void Run()
        {
            _orderedNumbers = new int[NumberToGenerate];
            var threads = new Thread[NumberToGenerate]; // An array of threads.

            // Set up/create each thread with the method, its value.
            for (var i = 0; i < NumberToGenerate; i++)
            {
                var threadStart = new ParameterizedThreadStart(AddValue);
                var thread = new Thread(threadStart);
                threads[i] = thread;
            }
            
            // Start all the threads.
            for (var i = 0; i < NumberToGenerate; i++)
            {
                threads[i].Start(i + 1); // Pass in the param to the method/function (AddValue).
            }

            // Verify that the threads have finished running.
            while (true)
            {
                foreach (var t in threads)
                {
                    if (t.IsAlive)
                    {
                        continue;
                    }
                }
                break;
            }

            // Write out the integers inside the orderedNumbers array to the Console window.
            foreach (var i in _orderedNumbers)
            {
                Console.WriteLine(i);
            }
        }

        // Add the value it was given as a parameter to the next position in the array.
        public void AddValue(object value)
        {
            _orderedNumbers[_index] = (int) value;
            // Put the thread to sleep for a few milliseconds to simulate distributed system.
            Thread.Sleep(_rng.Next(10));
            _index++;
        }
    }
}