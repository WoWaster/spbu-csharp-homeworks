namespace LazyTests;

using System.Threading;
using Lazy;
using NUnit.Framework;

public class SafeLazyTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void RaceCondition()
    {
        var lazyObject = LazyFactory.CreateSafeLazy(Suppliers.InitThreadedLazyObject);

        var ids = new (int InitBy, int ThreadId)[2];
        var threads = new Thread[2];
        for (var i = 0; i < threads.Length; i++)
        {
            var localI = i;
            threads[i] = new Thread(
                () =>
                {
                    var threadedLazyObject = lazyObject.Get();
                    ids[localI] = (threadedLazyObject.InitializedBy, Thread.CurrentThread.ManagedThreadId);
                }
            );
            threads[i].Start();
        }

        foreach (var t in threads)
        {
            t.Join();
        }

        // If everything is correct and lock have occured, only one pair will be true, other must be false
        // If both pairs are true we have a problem
        var isNotCorrect = ids[0].InitBy == ids[0].ThreadId && ids[1].InitBy == ids[1].ThreadId;
        Assert.That(isNotCorrect, Is.False);
    }
}