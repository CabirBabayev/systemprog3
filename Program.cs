Semaphore semaphore = new Semaphore(2, 2);
ManualResetEvent reset = new ManualResetEvent(false);
Thread threadqlav = new Thread(() =>
{
    reset.WaitOne();
    Console.WriteLine("All orders have been processed!");
});
threadqlav.Start();
int c = 0;
Thread[] threads = new Thread[5];
for (int i = 0; i < 5; i++)
{
    int threadNum = i + 1;
    threads[i] = new Thread(() =>
    {
        for (int order = 1; order <= 100; order++)
        {
            semaphore.WaitOne();
            try
            {
                Thread.Sleep(10);
                Interlocked.Increment(ref c);
            }
            finally
            {
                semaphore.Release();
            }
        }
        Console.WriteLine($"Thread {threadNum} completd");
    });
    threads[i].Start();
}

foreach (Thread thread in threads)
{
    thread.Join();
}
Console.WriteLine($"Отработано заказов {c}");
reset.Set();
threadqlav.Join();