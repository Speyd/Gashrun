using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProtoRender.Object;
using ScreenLib;
using ScreenLib.Output;
using SFML.Graphics;
using SFML.System;


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
            _widthCross = value / ScreenLib.Screen.MultWidth;
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
            _heightCross = value / ScreenLib.Screen.MultHeight;
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
            _indentFromCenter = value / (ScreenLib.Screen.MultHeight < ScreenLib.Screen.MultWidth ? ScreenLib.Screen.MultHeight : ScreenLib.Screen.MultWidth);
            UpdateVertexArray();
        }
    }


    private bool _invertCrossParts = true;
    public bool InvertCrossParts 
    {
        get => _invertCrossParts;
        set
        {
            _invertCrossParts = value;
            UpdateInvertCrossParts();
        }
    }


    private Predicate<int> _invertPredicate = (index => index % 2 != 0 ? false : true);
    public Predicate<int> InvertPredicate
    {
        get => _invertPredicate;
        set
        {
            _invertPredicate = value;
            UpdateInvertCrossParts();
        }
    }

    public RotationType RotationObjectType { get; set; } = RotationType.AroundCertainPosition;

    private Cross[] Crosses { get; set; }



    #region Constructor
    void CreateCrosses()
    {
        for (int i = 0; i < _amountCross; i++)
        {
            Crosses[i] = new Cross(StartDegree + i * GeneralDegreePosition, GeneralDegreeObject)
            {
                IsReverse = InvertCrossParts ? InvertPredicate(i + 1) : false
            };

            Drawables.Add(Crosses[i].VertexArray);
        }
    }
    public CrossSight(int amountCross, Vector2f positionOnScreen, Color color, IUnit? owner = null)
         : base(owner)
    {
        if (amountCross <= 0)
            throw new ArgumentException("Amount of crosses must be greater than zero.", nameof(amountCross));

        _amountCross = amountCross;
        PositionOnScreen = positionOnScreen;
        FillColor = color;

        Crosses = new Cross[_amountCross];
        CreateCrosses();

        UpdateVertexArray();

        Screen.WidthChangesFun += UpdateScreenInfo;
        Screen.HeightChangesFun += UpdateScreenInfo;
    }
    public CrossSight(int amountCross, Color color)
       : this(amountCross, new Vector2f(Screen.Setting.HalfWidth, Screen.Setting.HalfHeight), color)
    { }
    #endregion



    private Vector2f SetCenterRotate(Cross cross) => RotationObjectType switch
    {
        RotationType.AroundItsAxis => Cross.CalculateCenter(cross.PointPositions),
        RotationType.AroundCertainPosition => PositionOnScreen,
        _ => Cross.CalculateCenter(cross.PointPositions)
    };
    public Cross? GetCross(int index)
    {
        if(index < 0 || index >= Crosses.Length)
            return null;

        return Crosses[index];
    }

    private void UpdateCross(Cross cross)
    {
        cross.UpdatePosition(PositionOnScreen, IndentFromCenter, WidthCross, HeightCross);
        cross.UpdateRotationObject(SetCenterRotate(cross));
    }
    private void UpdateVertexArray()
    {
        foreach (var cross in Crosses)
            UpdateCross(cross);
    }
    private void UpdateObjectDegree()
    {
        foreach (var cross in Crosses)
        {
            cross.DegreeObject = GeneralDegreeObject;
            UpdateCross(cross);
        }
    }
    private void UpdatePositionDegree()
    {
        for (int i = 0; i < Crosses.Length; i++)
        {
            Crosses[i].DegreePosition = StartDegree + i * GeneralDegreePosition;
            UpdateCross(Crosses[i]);
        }
    }
    private void UpdateAmountCross()
    {
        Drawables.Clear();

        int newSize = Math.Min(Crosses.Length, AmountCross);
        Cross[] newCrosses = new Cross[newSize];


        for (int i = 0; i < newSize; i++)
        {
            if (i <= Crosses.Length - 1)
                newCrosses[i] = Crosses[i];
            else
            {
                newCrosses[i] = new Cross(StartDegree + i * GeneralDegreePosition, GeneralDegreeObject)
                {
                    IsReverse = InvertCrossParts ? InvertPredicate(i + 1) : false
                };
                UpdateCross(newCrosses[i]);
            }

            Drawables.Add(newCrosses[i].VertexArray);
        }

        Crosses = newCrosses;
    }
    private void UpdateInvertCrossParts()
    {
        for (int i = 0; i < _amountCross; i++)
        {
            if(InvertCrossParts == false && Crosses[i].IsReverse)
                Crosses[i].IsReverse = false; 
            else if(InvertCrossParts == true)
                Crosses[i].IsReverse = InvertPredicate(i + 1);

            UpdateCross(Crosses[i]);
        }
    }

    public override void Render()
    {
        UpdateInfo();
        foreach (var draw in Drawables)
            Screen.OutputPriority?.AddToPriority(IUIElement.OutputPriorityType, draw);
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
        if (IsHide && Drawables.Count > 0)
            Drawables.Clear();
        else
        {
            foreach (var cross in Crosses)
            {
                if (!Drawables.Contains(cross.VertexArray))
                    Drawables.Add(cross.VertexArray);
            }
        }
    }
}
