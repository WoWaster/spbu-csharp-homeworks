namespace Lazy;

public class SafeLazy<T> : ILazy<T>
{
    private readonly object _locker = new();
    private readonly Func<T> _supplier;
    private bool _isValueReady;
    private T? _value;

    internal SafeLazy(Func<T> supplier)
    {
        _supplier = supplier;
    }

    public T? Get()
    {
        if (_isValueReady)
        {
            return _value;
        }

        lock (_locker)
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
}