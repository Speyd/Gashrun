

namespace InteractionFramework.Death;
public interface IDamageable
{
    Stat Hp { get; set; }
    public DeathEffect? DeathAnimation { get; set; }
}
