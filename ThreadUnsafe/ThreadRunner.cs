using System.Threading;

namespace ThreadUnsafe
{
    public class ThreadRunner
    {
        private readonly int numberToGenerate = 100;
        private int index = 0;
        private int[] orderedNumbers;

        public void Run()
        {
            orderedNumbers = new int[numberToGenerate];
            var threads = new Thread[numberToGenerate];

            for (int i = 0; i < numberToGenerate; i++)
            {
                var threadStart = new ParameterizedThreadStart(AddValue);
                var thread = new Thread(threadStart);
                threads[i] = thread;
            }
            
            //file:///C:/Users/kaylu/OneDrive/Documents/_Hull%20Uni/Final%20Year/T2/600089_Distributed_Systems_Programming/Lab/Threads%20Lab%207.pdf


        }
        public void AddValue(object value) {}
    }
}