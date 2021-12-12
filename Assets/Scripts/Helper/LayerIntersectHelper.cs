using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// extended methods for the IntersectHelper dealing with whole layer intersections
/// </summary>
public struct LayerIntersectHelper
{
    public static Vector2[] GetLineIntersects(LineData line, LayerData layer)
    {
        List<Vector2> possiblePOI = new List<Vector2>();
        possiblePOI.Add(line.startPoint, line.endPoint);

        foreach (LineData layerLine in layer.lines)
        {
            bool intersect = IntersectHelper.TryLineLine(line, layerLine, out Vector2 p);
            if (intersect)
                possiblePOI.Add(p);
        }

        foreach (CircleData layerCircle in layer.circles)
        {
            int numIntersects = IntersectHelper.TryLineCircle(line, layerCircle, out Vector2 p1, out Vector2 p2);
            if (numIntersects == 1)
                possiblePOI.Add(p1);
            else if (numIntersects == 2)
                possiblePOI.Add(p1, p2);
        }

        foreach (ArcData layerCompassArc in layer.compassArcs)
        {
            int numIntersects = IntersectHelper.TryLineArc(line, layerCompassArc, out Vector2 p1, out Vector2 p2);
            if (numIntersects == 1)
                possiblePOI.Add(p1);
            else if (numIntersects == 2)
                possiblePOI.Add(p1, p2);
        }

        return GetNewPOI(possiblePOI, layer);
    }

    public static Vector2[] GetCircleIntersects(CircleData circle, LayerData layer)
    {
        List<Vector2> possiblePOI = new List<Vector2>();
        possiblePOI.Add(circle.origin);

        foreach (LineData layerLine in layer.lines)
        {
            int numIntersects = IntersectHelper.TryCircleLine(circle, layerLine, out Vector2 p1, out Vector2 p2);
            if (numIntersects == 1)
                possiblePOI.Add(p1);
            else if (numIntersects == 2)
                possiblePOI.Add(p1, p2);
        }

        foreach (CircleData layerCircle in layer.circles)
        {
            int numIntersects = IntersectHelper.TryCircleCircle(circle, layerCircle, out Vector2 p1, out Vector2 p2);
            if (numIntersects == 1)
                possiblePOI.Add(p1);
            else if (numIntersects == 2)           
                possiblePOI.Add(p1, p2);
        }

        foreach (ArcData layerArc in layer.arcs)
        {
            int numIntersects = IntersectHelper.TryCircleArc(circle, layerArc, out Vector2 p1, out Vector2 p2);
            if (numIntersects == 1)
                possiblePOI.Add(p1);
            else if (numIntersects == 2)
                possiblePOI.Add(p1, p2);
        }

        return GetNewPOI(possiblePOI, layer);
    }

    public static Vector2[] GetArcIntersects(ArcData arc, LayerData layer)
    {
        List<Vector2> possiblePOI = new List<Vector2>();
        possiblePOI.Add(arc.origin, arc.startPoint, arc.endPoint);

        foreach (LineData layerLine in layer.lines)
        {
            int numIntersects = IntersectHelper.TryArcLine(arc, layerLine, out Vector2 p1, out Vector2 p2);
            if (numIntersects == 1)
                possiblePOI.Add(p1);
            else if (numIntersects == 2)
                possiblePOI.Add(p1, p2);
        }

        foreach (CircleData layerCircle in layer.circles)
        {
            int numIntersects = IntersectHelper.TryArcCircle(arc, layerCircle, out Vector2 p1, out Vector2 p2);
            if (numIntersects == 1)
                possiblePOI.Add(p1);
            else if (numIntersects == 2)
                possiblePOI.Add(p1, p2);
        }

        foreach (ArcData layerArc in layer.arcs)
        {
            int numIntersects = IntersectHelper.TryArcArc(arc, layerArc, out Vector2 p1, out Vector2 p2);
            if (numIntersects == 1)
                possiblePOI.Add(p1);
            else if (numIntersects == 2)
                possiblePOI.Add(p1, p2);
        }

        return GetNewPOI(possiblePOI, layer);
    }

    private static Vector2[] GetNewPOI(List<Vector2> newPoi, LayerData layer)
    {
        List<Vector2> newIntersects = new List<Vector2>();
        foreach (Vector2 n in newPoi)
        {
            if (CanAddPoint(n))
                newIntersects.Add(n);
        }
        return newIntersects.ToArray();

        bool CanAddPoint(Vector2 n)
        {
            foreach (Vector2 p in layer.poi)
            {
                Vector2 diff = n - p;
                if (diff.x * diff.x + diff.y * diff.y < 0.000005)
                    return false;
            }
            return true;
        }
    }
}

public static partial class ListExtensions
{
    /// <summary> Overload for Generic List Add method to add multiple items</summary>
    public static void Add<T>(this List<T> list, params T[] items)
    {
        foreach (T item in items)
            list.Add(item);
    }
}