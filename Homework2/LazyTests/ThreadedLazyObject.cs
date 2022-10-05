namespace LazyTests;

using System.Threading;

public class ThreadedLazyObject
{
    public int InitializedBy;

    public ThreadedLazyObject(int threadId)
    {
        InitializedBy = threadId;
        Thread.Sleep(1000);
    }
}