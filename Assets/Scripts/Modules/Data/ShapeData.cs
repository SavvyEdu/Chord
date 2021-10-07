using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Data shared between drawn shapes
/// </summary>
public abstract class ShapeData
{
    public ShapeData() { }
}

public class CircleData : ShapeData
{
    public Vector2 origin;
    public float radius;
    public CircleData(Vector2 o, float r) : base()
    {
        this.origin = o;
        this.radius = r;
    }
}

public class ArcData : CircleData
{
    public Vector2 startPoint;
    public Vector2 endPoint;
    public float startAngle;
    public float endAngle;
    public ArcData(Vector2 s, float r) : base(s, r)
    {
        this.startPoint = s;
        this.endPoint = s;
    }
}

public class SegmentData : ShapeData
{
    public Vector2 start;
    public Vector2 end;
    public SegmentData(Vector2 s, Vector2 e) : base()
    {
        this.start = s;
        this.end = e;
    }
}

public class LineData : SegmentData
{
    public Vector2 Diff { get { return end - start; } }
    public LineData(Vector2 s, Vector2 e) : base (s, e) { }
}

public class PolyLineData : ShapeData //TODO
{
    public List<SegmentData> segments; 

    public PolyLineData() : base()
    {
        segments = new List<SegmentData>();
    }
}
