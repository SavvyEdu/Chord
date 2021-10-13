using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class POIModule
{
    public List<Vector2> GetNewPOI(List<Vector2> possiblePOI, List<Vector2> points)
    {
        List<Vector2> newPOI = new List<Vector2>();
        foreach (var possiblePoint in possiblePOI)
        {
            if (CanAddPoint(possiblePoint, points))
                newPOI.Add(possiblePoint);
        }
        return newPOI;
    }

    public bool CanAddPoint(Vector2 p, List<Vector2> points)
    {
        foreach (var point in points)
        {
            Vector2 diff = p - point;
            if(diff.x * diff.x + diff.y * diff.y < 0.000005)
                return false;
        }
        return true;
    }

    public void DrawShapes(List<Vector2> points)
    {
        foreach (var poi in points)
        {
            //Draw.Disc(poi, 0.05f, Color.white);
            //Draw.Ring(poi, 0.05f);

            Draw.Disc(poi, 0.02f);
        }
    }
}
