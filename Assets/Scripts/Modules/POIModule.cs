using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class POIModule : MonoBehaviour
{
    public List<Vector2> points = new List<Vector2>();

    public int AddPoint(Vector2 p)
    {
        foreach (var point in points)
        {
            Vector2 diff = p - point;
            if(diff.x * diff.x + diff.y * diff.y < 0.000005)
                return 0;
        }
        points.Add(p);
        return 1;
    }

    public int AddPoints(params Vector2[] points)
    {
        int num = 0;
        foreach (Vector2 point in points)
            num += AddPoint(point);
        return num;
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
