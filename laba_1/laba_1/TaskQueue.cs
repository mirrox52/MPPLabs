using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;

namespace laba_1
{
    class TaskQueue : IDisposable
    {
        public delegate void TaskDelegate();        
        private ConcurrentQueue<TaskDelegate> tasks;                // очередь
        private List<Thread> threads;

        public TaskQueue(int count)                                 // конструктор
        {
            tasks = new ConcurrentQueue<TaskDelegate>();            // инициализация очереди
            threads = new List<Thread>();                           // создание списка потоков (пул)

            Thread thread;
            for (int i = 0; i < count; i++)                         // пул потоков
            {
                thread = new Thread(new ThreadStart(Work));
                thread.Name = "Thread " + i.ToString();
                threads.Add(thread);
                threads[i].Start();
            }
        }

        public void EnqueueTask(TaskDelegate task)
        {
            tasks.Enqueue(task);                                    // добавляет элемент в конец очереди
        }

        private void Work()
        {   
            while(true)                                           
            {
                try
                {
                    TaskDelegate task;
                    while (!tasks.TryDequeue(out task))             // если нет доступа к таске поток ожидает
                       Thread.Sleep(10);
                    task();
                }
                catch (Exception exc)
                {
                   
                }
            }         
        }

        public void Wait()
        {
            foreach (Thread thread in threads)
            {
                thread.Join();                                      
            }
        }

        public void CloseThreads()
        {
            foreach (Thread thread in threads)
            {
                thread.Interrupt();
            }
        }

        public void Dispose()
        {
            CloseThreads();
            GC.SuppressFinalize(this);
        }

        ~TaskQueue()
        {
            CloseThreads();
        }

    }
}
