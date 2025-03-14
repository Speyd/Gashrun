using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoRender.Object;
using ScreenLib;
using SFML.Graphics;
using SFML.System;
using static System.Formats.Asn1.AsnWriter;
namespace UIFramework.Sights.Crosses;
public class CrossSight : Sight
{
    private int _amountCross = 4;
    public int AmountCross 
    {
        get => _amountCross;
        set
        {
            _amountCross = value < 0? -value: value;
            UpdateAmountCross();
        }
    }

    private float _startDegree = 0;
    public float StartDegree 
    {
        get => _startDegree;
        set
        {
            _startDegree = value;
            UpdatePositionDegree();
        }
    }

    private float _generalDegreePosition = 90;
    public float GeneralDegreePosition
    {
        get => _generalDegreePosition;
        set
        {
            _generalDegreePosition = value;
            UpdatePositionDegree();
        }
    }

    private float _generalDegreeObject = 0;
    public float GeneralDegreeObject
    {
        get => _generalDegreeObject;
        set
        {
            _generalDegreeObject = value;
            UpdateObjectDegree();
        }
    }


    private float _widthCross = 0;
    private float _originWidthCross = 0;
    public float WidthCross
    {
        get => _widthCross;
        set
        {
            _originWidthCross = value;
            _widthCross = value / Screen.ScreenRatio;
            UpdateVertexArray();
        }
    }
    private float _heightCross = 0;
    private float _originHeightCross = 0;
    public float HeightCross
    {
        get => _heightCross;
        set
        {
            _originHeightCross = value;
            _heightCross = value / Screen.ScreenRatio;
            UpdateVertexArray();
        }
    }

    private float _indentFromCenter = 0;
    private float _originIndentFromCenter = 0;
    public float IndentFromCenter 
    {
        get => _indentFromCenter;
        set
        {
            _originIndentFromCenter = value;
            _indentFromCenter = value / Screen.ScreenRatio;
            UpdateVertexArray();
        }
    }

    public RotationType RotationType { get; set; } = RotationType.AroundCertainPosition;
    private Cross[] Crosses { get; set; }

    #region Constructor
    void CreateCrosses()
    {
        for (int i = 0; i < _amountCross; i++)
        {
            Crosses[i] = new Cross(StartDegree + i * GeneralDegreePosition, GeneralDegreeObject)
            {
                IsReverse = (i + 1) % 2 != 0 ? false : true
            };

            Drawables.Add(Crosses[i].VertexArray);
        }
    }
    public CrossSight(int amountCross, Vector2f positionOnScreen, Color color)
    {
        _amountCross = amountCross;

        Crosses = new Cross[_amountCross];
        CreateCrosses();

        FillColor = color;
        PositionOnScreen = positionOnScreen;

        UpdateVertexArray();
        Screen.WidthChangesFun += UpdateScreenInfo;
        Screen.HeightChangesFun += UpdateScreenInfo;
    }
    public CrossSight(int amountCross, Color color)
       : this(amountCross, new Vector2f(Screen.Setting.HalfWidth, Screen.Setting.HalfHeight), color)
    { }
    #endregion

    private Vector2f SetCenterRotate(Cross cross)
    {
        switch(RotationType)
        {
            case RotationType.AroundItsAxis:
                return Cross.CalculateCenter(cross.PointPositions);
            case RotationType.AroundCertainPosition:
                return PositionOnScreen;
            default:
                return Cross.CalculateCenter(cross.PointPositions);
        }
    }
    public Cross? GetCross(int index)
    {
        if(index < 0 || index >= Crosses.Length)
            return null;

        return Crosses[index];
    }

    private void UpdateVertexArray()
    {
        foreach (var cross in Crosses)
        {
            cross.UpdatePosition(PositionOnScreen, IndentFromCenter, WidthCross, HeightCross);
            cross.UpdateRotationObject(SetCenterRotate(cross));
        }
    }
    private void UpdateObjectDegree()
    {
        foreach (var cross in Crosses) 
            cross.DegreeObject = GeneralDegreeObject;
        UpdateVertexArray();
    }
    private void UpdatePositionDegree()
    {
        for (int i = 0; i < Crosses.Length; i++)
        {
            Crosses[i].DegreePosition = StartDegree + i * GeneralDegreePosition;
        }
        UpdateVertexArray();
    }
    private void UpdateAmountCross()
    {
        Drawables.Clear();
        Cross[] newCrosses = new Cross[_amountCross];

        Array.Copy(Crosses, newCrosses, Crosses.Length);

        Crosses = newCrosses;
        foreach(var cross in Crosses)
            Drawables.Add(cross.VertexArray);

        UpdateVertexArray();
    }

    public override void UpdateInfo() { }
    public override void UpdateScreenInfo()
    {
        IndentFromCenter = _originIndentFromCenter;

        WidthCross = _originWidthCross;
        HeightCross = _originHeightCross;
    }
    public override void Hide()
    {
        if (Drawables.Count > 0)
            Drawables.Clear();
        else
        {
            foreach (var cross in Crosses)
                Drawables.Add(cross.VertexArray);
        }
    }
}
