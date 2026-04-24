namespace Reactive;

public interface IReactive : INotifyChange
{
  event Action<IReactive>? ReactiveChanged;
  IReactive? Owner { get; set; }
  void OwnerPropagate(IReactive obj); 
  void Invoke();
  object? UntypedValue { get; }
  bool IsReactive { get; }
  void Mute();
  void Unmute();
}

// Generic interface for typed access
public interface IReactive<T> : INotifyChange<T>,IReactive
{
  new event Action<IReactive<T>>? ReactiveChanged;
  event Action<T?>? ValueChanged;
  event Action<IReactive<T>>? TypedChanged;
  T? Value { get; set; }
  void ChangeValueMuted(T? value);
}