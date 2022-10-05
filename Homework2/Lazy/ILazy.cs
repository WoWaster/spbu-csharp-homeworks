namespace Lazy;

/**
 * <summary>Interface for lazy calculations.</summary>
 */
public interface ILazy<T>
{
    /**
     * <summary>
     *     Returns lazily calculated value.
     *     Runs calculation only once.
     * </summary>
     */
    T? Get();
}