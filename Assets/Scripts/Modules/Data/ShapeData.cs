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
    public CircleData(Vector2 origin, float radius) : base()
    {
        this.origin = origin;
        this.radius = radius;
    }
}

public class ArcData : CircleData
{
    public Vector2 startPoint;
    public Vector2 endPoint;
    public float startAngle;
    public float endAngle;
    public ArcData(Vector2 origin, float radius) : base(origin, radius)
    {
        this.startPoint = origin;
        this.endPoint = origin;
    }

    /// <summary> Returns the signed angle in degrees of p from the right of the circle </summary>
    public float AngleFrom0(Vector2 p) => Vector2.SignedAngle(Vector2.right, p - origin) * Mathf.Deg2Rad;

    /// <summary> Returns the signed angle in degrees of p from the draw start point </summary>
    public float AngleFromStart(Vector2 p) => Vector2.SignedAngle(startPoint - origin, p - origin) * Mathf.Deg2Rad;

    /// <summary> Returns true when p is between the start and end angles </summary>
    public bool AngleContains(Vector2 p)
    {
        float startToP = AngleFromStart(p);
        float startToEnd = AngleFromStart(endPoint);
        return Mathf.Abs(startToP) < Mathf.Abs(startToEnd) && Mathf.Sign(startToP * startToEnd) > 0;
    }
}

public class SegmentData : ShapeData
{
    public Vector2 startPoint;
    public Vector2 endPoint;
    public SegmentData(Vector2 startPoint, Vector2 endPoint) : base()
    {
        this.startPoint = startPoint;
        this.endPoint = endPoint;
    }
}

public class LineData : SegmentData
{
    /// <summary> Returns the vector from the startPoint to the endPoint </summary>
    public Vector2 Diff => startPoint != endPoint ? endPoint - startPoint : Vector2.up; 
    public LineData(Vector2 startPoint, Vector2 endPoint) : base (startPoint, endPoint) { }
}

public class PolyLineData : ShapeData //TODO
{
    public List<Vector2> path;

    public PolyLineData(Vector2 startPoint) : base()
    {
        path = new List<Vector2>();
        AddPoint(startPoint);
    }

    public void AddPoint(Vector2 point) => path.Add(point);
    public void UpdateEndPoint(Vector2 point) => path[path.Count - 1] = point;
    public void RemoveEndPoint() => path.RemoveAt(path.Count - 1);
}
