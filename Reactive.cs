using Godot;

namespace Reactive;

// Generic reactive class
public partial class Reactive<T> : Resource, IReactive<T>
{
    // Non-generic event for propagation
    public event Action<IReactive>? ReactiveChanged;
    public bool IsReactive { get; private set; } = true;
    public void Mute() => IsReactive = false;
    public void Unmute() => IsReactive = true;

    // Typed event for type-safe subscriptions
    public event Action<IReactive<T>>? TypedChanged;
    event Action<IReactive<T>>? IReactive<T>.ReactiveChanged
    {
        add => TypedChanged += value;
        remove => TypedChanged -= value;
    }

    // Direct event passing the contained value
    public event Action<T?>? ValueChanged;

    private IReactive? _owner;
    public IReactive? Owner
    {
        get => _owner;
        set
        {
            if (_owner != null)
            {
                // Unsubscribe the old owner from my change
                ReactiveChanged -= _owner.OwnerPropagate;
            }

            _owner = value;

            if (_owner != null)
            {
                // Subscribe new owner to my change
                ReactiveChanged += _owner.OwnerPropagate;
            }
        }
    }

    private T? _value;
    public T? Value
    {
        get => _value;
        set
        {
          if (Equals(_value, value))
          {
            return;
          }

          _value = value;
          Invoke();
        }
    }

    public void ChangeValueMuted(T? value) => _value = value;

    public object? UntypedValue => _value;
    
    // Propagate changes to owner
    public void OwnerPropagate(IReactive obj) => Invoke();

    // Invoke all events
    public void Invoke()
    {
      if (!IsReactive)
      {
        return;
      }
        // Invoke non-generic event
        ReactiveChanged?.Invoke(this);

        // Invoke type-safe generic event
        TypedChanged?.Invoke(this);

        // Invoke direct value event
        ValueChanged?.Invoke(_value);
    }

    #region Constructors

    

    // used by godot and other cases.
    protected Reactive() : this(null) { }

    public Reactive(IReactive? owner=null, T? value = default)
    {
      Owner = owner;

      if (value != null)
      {
        Value = value;
      }
      else if (this is T self) // if T is the subclass itself
      {
        Value = self; // automatically set Value to this
      }
      else
      {
        Value = default!;
      }
    }

    public Reactive(T? value = default, IReactive? owner = null) : this(owner, value) { }
    
    #endregion
    
    private bool Valid() => Value != null;

    public virtual bool IsValid() => Valid();
}
