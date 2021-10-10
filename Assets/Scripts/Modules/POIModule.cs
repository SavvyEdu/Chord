using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class POIModule : MonoBehaviour
{
    public List<Vector2> points = new List<Vector2>();

    public List<Vector2> GetNewPOI(List<Vector2> possiblePOI)
    {
        List<Vector2> newPOI = new List<Vector2>();
        foreach (var point in possiblePOI)
        {
            if (CanAddPoint(point))
                newPOI.Add(point);
        }
        return newPOI;
    }

    public bool CanAddPoint(Vector2 p)
    {
        foreach (var point in points)
        {
            Vector2 diff = p - point;
            if(diff.x * diff.x + diff.y * diff.y < 0.000005)
                return false;
        }
        return true;
    }

    public void DrawShapes()
    {
        foreach (var poi in points)
        {
            //Draw.Disc(poi, 0.05f, Color.white);
            //Draw.Ring(poi, 0.05f);

            Draw.Disc(poi, 0.02f);
        }
    }
}
