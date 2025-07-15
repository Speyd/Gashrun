
namespace InteractionFramework;
public class Stat
{
    public float Max { get; private set; }
    public float Value { get; private set; }

    public Action? OnDepleted;
    public Action? OnFilled;
    public Action? OnChanged;

    public Stat(float max)
    {
        Max = Math.Max(0, max);
        Value = Max;
    }
    public Stat(Stat stat)
    {
        Max = stat.Max;
        Value = stat.Value;
    }


    public void SetMax(float newMax)
    {
        Max = Math.Max(0, newMax);
        if (Value > Max)
            Value = Max;
    }

    public void SetValue(float val)
    {
        float clamped = Math.Clamp(val, 0, Max);
        if (clamped == Value) return;

        Value = clamped;
        OnChanged?.Invoke();

        if (Value <= 0)
            OnDepleted?.Invoke();
        else if (Value >= Max)
            OnFilled?.Invoke();
    }

    public void Decrease(float amount) => SetValue(Value - amount);
    public void Increase(float amount) => SetValue(Value + amount);
}
