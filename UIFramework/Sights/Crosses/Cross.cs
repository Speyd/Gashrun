using ScreenLib;
using SFML.Graphics;
using SFML.System;


namespace UIFramework.Sights.Crosses;
public class Cross
{
    public SFML.Graphics.Color FillColor { get; set; } = Color.Red;

    private float _degreePosition = 0;
    public float DegreePosition
    {
        get => _degreePosition;
        set
        {
            _degreePosition = value;
            _radianPosition = value * MathF.PI / 180;

            DirectionPosition = new Vector2f(MathF.Cos(_radianPosition), MathF.Sin(_radianPosition));
        }
    }

    private float _radianPosition = 0;
    public float RadianPosition
    {
        get => _radianPosition;
        set
        {
            _radianPosition = value;
            _degreePosition = value / MathF.PI * 180;

            DirectionPosition = new Vector2f(MathF.Cos(value), MathF.Sin(value));
        }
    }
    public Vector2f DirectionPosition { get; private set; }



    private float _degreeObject = 0;
    public float DegreeObject
    {
        get => _degreeObject;
        set
        {
            _degreeObject = value;
            _radianObject = value * MathF.PI / 180;

            DirectionObject = new Vector2f(MathF.Cos(_radianObject), MathF.Sin(_radianObject));
        }
    }

    private float _radianObject = 0;
    public float RadiannObject
    {
        get => _radianObject;
        set
        {
            _radianObject = value;
            _degreeObject = value / MathF.PI * 180;

            DirectionObject = new Vector2f(MathF.Cos(value), MathF.Sin(value));
        }
    }
    public Vector2f DirectionObject { get; private set; }


    public bool IsReverse {  get; set; } = false;
    public Dictionary<PointType, Vector2f> PointPositions { get; init; }
    public VertexArray VertexArray { get; set; } = new VertexArray(PrimitiveType.Quads, 4u);



    public Cross(float degreesPosition, float degreesObject)
    {
        PointPositions = new();

        DegreeObject = degreesObject;
        DegreePosition = degreesPosition;
    }
    public Cross(float degreesPosition)
        :this(degreesPosition, 0)
    { }
    public Cross()
        : this(0, 0)
    { }

    private void ResetVertexArray()
    {
        VertexArray[0] = new Vertex(PointPositions[PointType.LeftBottom], FillColor);
        VertexArray[1] = new Vertex(PointPositions[PointType.LeftTop], FillColor);
        VertexArray[2] = new Vertex(PointPositions[PointType.RightTop], FillColor);
        VertexArray[3] = new Vertex(PointPositions[PointType.RightBottom], FillColor);
    }
    private void SetVertex(Vector2f indentPosition, float widthCross, float heightCross)
    {
        PointPositions[PointType.LeftTop] = new Vector2f(indentPosition.X - widthCross / 2, indentPosition.Y + heightCross / 2);
        var LeftTop = PointPositions[PointType.LeftTop];

        PointPositions[PointType.LeftBottom] = new Vector2f(LeftTop.X, LeftTop.Y - heightCross);
        PointPositions[PointType.RightTop] = new Vector2f(LeftTop.X + widthCross, LeftTop.Y);
        PointPositions[PointType.RightBottom] = new Vector2f(LeftTop.X + widthCross, LeftTop.Y - heightCross);
    }

    private void UpdateVertexArray(Vector2f indentPosition, float widthCross, float heightCross)
    {
        float finalWidth = IsReverse ? heightCross : widthCross;
        float finalHeight = IsReverse ? widthCross : heightCross;

        SetVertex(indentPosition, finalWidth, finalHeight);
    }
    public void UpdatePosition(Vector2f PositionCenterSight, float indentFromCenter, float widthCross, float heightCross)
    {
        float xOffset = DirectionPosition.X * indentFromCenter;
        float yOffset = DirectionPosition.Y * indentFromCenter;

        Vector2f indentPosition = new Vector2f(
            Screen.Setting.HalfWidth + xOffset,
            Screen.Setting.HalfHeight + yOffset
        );

        UpdateVertexArray(indentPosition, widthCross, heightCross);
        ResetVertexArray();
    }

    public static Vector2f Rotate(Vector2f point, Vector2f pivot, Vector2f direction)
    {
        float newX = pivot.X + (point.X - pivot.X) * direction.X - (point.Y - pivot.Y) * direction.Y;
        float newY = pivot.Y + (point.X - pivot.X) * direction.Y + (point.Y - pivot.Y) * direction.X;
        return new Vector2f(newX, newY);
    }
    public static Vector2f CalculateCenter(Dictionary<PointType, Vector2f> points)
    {
        float centerX = (points[PointType.LeftTop].X + points[PointType.LeftBottom].X + points[PointType.RightTop].X + points[PointType.RightBottom].X) / 4;
        float centerY = (points[PointType.LeftTop].Y + points[PointType.LeftBottom].Y + points[PointType.RightTop].Y + points[PointType.RightBottom].Y) / 4;
        return new Vector2f(centerX, centerY);
    }
    public void UpdateRotationObject(Vector2f position)
    {
        PointPositions[PointType.LeftTop] = Rotate(PointPositions[PointType.LeftTop], position, DirectionObject);
        PointPositions[PointType.LeftBottom] = Rotate(PointPositions[PointType.LeftBottom], position, DirectionObject);
        PointPositions[PointType.RightTop] = Rotate(PointPositions[PointType.RightTop], position, DirectionObject);
        PointPositions[PointType.RightBottom] = Rotate(PointPositions[PointType.RightBottom], position, DirectionObject);

        ResetVertexArray();
    }

}
