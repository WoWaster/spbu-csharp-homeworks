namespace LazyTests;

using System;
using System.Threading;

public static class Suppliers
{
    public static int? NullSupplier()
    {
        return null;
    }

    public static int AdditionSupplier(int a, int b)
    {
        return a + b;
    }

    public static string LongWaitSupplier()
    {
        Thread.Sleep(1000);
        return "Initialized!";
    }

    public static int ExceptionSupplier()
    {
        throw new InvalidOperationException();
    }

    public static ThreadedLazyObject InitThreadedLazyObject()
    {
        var threadedLazyObject = new ThreadedLazyObject(Thread.CurrentThread.ManagedThreadId);
        return threadedLazyObject;
    }
}