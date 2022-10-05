namespace Lazy;

public class UnsafeLazy<T> : ILazy<T>
{
    private readonly Func<T> _supplier;
    private bool _isValueReady;
    private T? _value;

    internal UnsafeLazy(Func<T> supplier)
    {
        _supplier = supplier;
    }

    public T? Get()
    {
        if (_isValueReady)
        {
            return _value;
        }

        _value = _supplier();
        _isValueReady = true;
        return _value;
    }
}