namespace Lazy;

public static class LazyFactory
{
    /**
     * <summary>Creates thread safe (using locking) lazy calculations.</summary>
     * <param name="supplier">Method to run lazily.</param>
     */
    public static ILazy<T> CreateSafeLazy<T>(Func<T> supplier)
    {
        return new SafeLazy<T>(supplier);
    }

    /**
     * <summary>Creates thread unsafe (without locking) lazy calculations.</summary>
     * <param name="supplier">Method to run lazily.</param>
     */
    public static ILazy<T> CreateUnsafeLazy<T>(Func<T> supplier)
    {
        return new UnsafeLazy<T>(supplier);
    }
}