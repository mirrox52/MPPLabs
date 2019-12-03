using System;
using System.Threading;

namespace laba_1
{
    class Program
    {
        static void Main(string[] args)
        {
            TaskQueue _TaskQueue = null;
            try
            {
                Console.Write("Enter the number of threads: ");
                int countThreads = int.Parse(Console.ReadLine());
                _TaskQueue = new TaskQueue(countThreads);

                for (int i = 0; i < countThreads*10; i++)
                {
                    _TaskQueue.EnqueueTask(SomeTask);
                }
                _TaskQueue.Wait();
            }
            finally
            {
                if(_TaskQueue != null)
                {         
                   _TaskQueue.Dispose();
                }
            }
                                    
        }

        static public void SomeTask()
        {
            Console.WriteLine(Thread.CurrentThread.Name);
        }
        
    }
}
