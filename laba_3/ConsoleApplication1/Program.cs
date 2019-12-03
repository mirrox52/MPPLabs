using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ConsoleApplication1
{
    class Mutex
    {
        private int _holderId = -1;                                            

        public void Lock()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            while ((Interlocked.CompareExchange(ref _holderId, id, -1) != -1))  // Сравнивает два(1,3) значения на равенство и, если они равны, заменяет первое значение
            {
                Thread.Sleep(100);
            }
        }

        public void Unlock()
        {
            var id = Thread.CurrentThread.ManagedThreadId;
            Interlocked.CompareExchange(ref _holderId, -1, id);
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var mutex = new Mutex();
            for (var i = 0; i < 10; i++)
            {
                new Thread(() =>
                {
                    mutex.Lock();
                    Console.WriteLine("Thread #" + Thread.CurrentThread.ManagedThreadId + " locked mutex.");
                    Thread.Sleep(400);
                    Console.WriteLine("Thread #" + Thread.CurrentThread.ManagedThreadId + " unlocked mutex.");
                    mutex.Unlock();
                }).Start();
            }
            Console.ReadLine();
        }
    }
}
