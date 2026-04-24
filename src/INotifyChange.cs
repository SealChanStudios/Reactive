namespace Reactive;


public interface INotifyChange
{
    event Action? OnChange;
}

public interface INotifyChange<T> : INotifyChange
{
    event Action<T?>? NotifyChange;
}