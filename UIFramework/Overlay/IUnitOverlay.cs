using ProtoRender.Object;


namespace UIFramework.Overlay;
public static class IUnitOverlay
{
    public static string GetInfo(IUnit unit)
    {
        return $"MoveSpeed: {unit.MoveSpeed:0.00}\n" +
               $"MinDistanceFromWall: {unit.MinDistanceFromWall:0.00}\n" +
               $"IgnoreCollision: {unit.IgnoreCollisionObjects.Count}";
    }
}