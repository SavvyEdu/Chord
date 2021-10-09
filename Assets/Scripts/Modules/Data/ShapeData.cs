﻿using UnityEngine;
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
    public ArcData(Vector2 startPoint, float radius) : base(startPoint, radius)
    {
        this.startPoint = startPoint;
        this.endPoint = startPoint;
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
    public Vector2 Diff => endPoint - startPoint; 
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
