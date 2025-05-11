

using AnimationLib;

namespace ObjectFramework.Death;
public interface IDamageable
{
    public float MaxHp { get; }
    public float Hp { get; set; }


    /// <summary>
    /// Delegate action that handles incoming damage.
    /// Accepts a <c>float</c> value representing the amount of damage to apply.
    /// </summary>
    public Action<float>? DamageAction { get; set; }

    public Action? DeathAction { get; set; }
    public DeathAnimation? DeathAnimation { get; set; }
}
