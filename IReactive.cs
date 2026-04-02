namespace Reactive;

public interface IReactive
{
  event Action<IReactive>? ReactiveChanged;
  IReactive? Owner { get; set; }
  void _OwnerPropagate(IReactive obj);
  void Invoke();
  object? UntypedValue { get; }
  void Reactivity(bool reactive = true);
}

// Generic interface for typed access
public interface IReactive<T> : IReactive
{
  new event Action<IReactive<T>>? ReactiveChanged;
  event Action<T?>? ValueChanged;
  event Action<IReactive<T>>? TypedChanged;
  T? Value { get; set; }
}