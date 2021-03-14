using System;

namespace ThreadUnsafe
{
    class Program
    {
        /*
           The typical reason for using threads is where you want to perform tasks that 
           take some time in the background, rather than allowing them to block the 
           execution of other tasks.
         */
        static void Main(string[] args)
        {
            var threadRunner = new ThreadRunner();
            threadRunner.Run();
        }
    }
}
