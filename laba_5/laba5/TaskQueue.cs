using System;
using System.Collections.Generic;
using System.Threading;

namespace laba5
{
	public delegate void TaskDelegate();

    public class TaskQueue
    {
		static Dictionary<Thread, bool> threads = new Dictionary<Thread, bool>();
		static List<TaskDelegate> TaskDelegates = new List<TaskDelegate>();

		public TaskQueue(int threadsCount)
		{
			if (threadsCount == 0)
			{
				threadsCount = 1;
			}
            
			threadsCount = Math.Abs(threadsCount);

			for (int i = 0; i < threadsCount; i++)
			{
				Thread thread = new Thread(ThreadProc);
				thread.IsBackground = true;
				thread.Start();
				threads.Add(thread, false);
			}
		}

		public void EnqueueTask(TaskDelegate task)
		{
			TaskDelegates.Add(task);
		}
              
        public void ThreadProc()
		{
			TaskDelegate del = null;
			Thread.Sleep(50);

			while (true)
			{
				lock (this)
				{
					if (TaskDelegates.Count > 0)
					{                  
						del = TaskDelegates[0];
						TaskDelegates.RemoveAt(0);
      				}
				}

				if (del != null)
				{
					threads[Thread.CurrentThread] = true;
					del();
					del = null;
					threads[Thread.CurrentThread] = false;
				}
			}
		}

		public bool Stopped()
		{
			lock (this)
			{
				if (TaskDelegates.Count != 0)
				{
					return false;
				}

				foreach (var values in threads.Values)
				{
					if (values)
					{
						return false;
					}
				}
			}
			return true;
		}
    }
}