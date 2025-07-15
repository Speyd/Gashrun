using ProtoRender.Object;
using SFML.Audio;


namespace ObjectFramework;
public static class Camera
{
    public static IUnit? CurrentUnit { get; set; }
    public static Map? map { get; set; }


    public static void UpdateListener()
    {
        if (CurrentUnit is null)
            return;

        Listener.Position = new SFML.System.Vector3f(
            (float)CurrentUnit.X.Axis,
            (float)CurrentUnit.Z.Axis,
            (float)CurrentUnit.Y.Axis
        );


        float yaw = (float)CurrentUnit.Angle; 
        float pitch = (float)CurrentUnit.VerticalAngle;

        float dirX = (float)(Math.Cos(yaw));
        float dirY = (float)(Math.Sin(pitch));
        float dirZ = (float)(Math.Sin(yaw));

        Listener.Direction = new SFML.System.Vector3f(dirX, dirY, dirZ);
    }
}
