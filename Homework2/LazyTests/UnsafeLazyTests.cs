namespace LazyTests;

using Lazy;
using NUnit.Framework;

public class UnsafeLazyTests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void SupplierReturnNull()
    {
        var lazyNull = LazyFactory.CreateUnsafeLazy(Suppliers.NullSupplier);
        Assert.That(lazyNull.Get(), Is.EqualTo(null));
    }

    [Test]
    public void FastSupplier()
    {
        const int a = 5;
        const int b = 7;
        var lazySum = LazyFactory.CreateUnsafeLazy(() => Suppliers.AdditionSupplier(a, b));

        // Lazy is quite unnecessary here since addition is very fast operation
        Assert.That(lazySum.Get(), Is.EqualTo(12));
    }

    [Test]
    public void SlowSupplier()
    {
        var lazyLongWait = LazyFactory.CreateUnsafeLazy(Suppliers.LongWaitSupplier);

        // Lazy is useful here since getting the result takes long time
        Assert.That(lazyLongWait.Get(), Is.EqualTo("Initialized!"));
    }

    [Test]
    public void ExceptionFromSupplier()
    {
        var lazyException = LazyFactory.CreateUnsafeLazy(Suppliers.ExceptionSupplier);
        Assert.That(() => lazyException.Get(), Throws.InvalidOperationException);
    }
}