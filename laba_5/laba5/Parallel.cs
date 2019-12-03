using System.Threading;

namespace laba5
{
    public class Parallel
    {
		public static void WaitAll(TaskDelegate[] delegates)
        {
            TaskQueue queue = new TaskQueue(delegates.Length);

            foreach (var del in delegates)
            {
                queue.EnqueueTask(del);
            }

			Thread.CurrentThread.Priority = ThreadPriority.Lowest;

			do
			{
				Thread.Sleep(50); 
			} while (!queue.Stopped());
		}
    }
}
