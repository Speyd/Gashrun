using RayTracingLib;
using ControlLib.Buttons;
using ProtoRender.Object;


namespace UIFramework.Weapon.Bullets.Variants;
public class StandartBullet : Bullet
{
    public StandartBullet(float damage, ButtonBinding? hitObject)
        : base(damage, hitObject)
    { }
    public StandartBullet(ButtonBinding? hitObject)
       : this(IBullet.BaseDamage, hitObject)
    { }
    public StandartBullet(StandartBullet standartBullet)
        : base(standartBullet)
    { }

    public override void Update()
    { }

    private bool ShouldStopFlight(
    ProtoRender.Object.IUnit owner,
    ProtoRender.Object.IObject? hitObject,
    SFML.System.Vector3f hitPosition)
    {
        if (owner.Map is null) return true;
        if (hitObject is null) return true;

        bool tooFar = FlightDistance != IBullet.InfinityFlightDistance &&
                      DataPipes.MathUtils.CalculateDistance(hitPosition, PositionOwner) > FlightDistance;

        return tooFar;
    }

    public override void Flight(ProtoRender.Object.IUnit owner)
    {
        IsActive = true;

        Owner = owner;
        PositionOwner = new SFML.System.Vector3f((float)owner.X.Axis, (float)owner.Y.Axis, (float)owner.Z.Axis);


        var result = Raycast.RaycastFun(owner);
        var hitObject = result.Item1;
        var hitPosition = result.Item2;

        if (ShouldStopFlight(owner, hitObject, hitPosition))
        {
            IsActive = false;
            return;
        }

        OnHit(owner, owner, hitObject, hitPosition);
        IsActive = false;
    }
    public override IBullet GetCopy()
    {
        return new StandartBullet(this);
    }
}
