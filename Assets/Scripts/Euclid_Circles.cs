using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class Euclid_Circles
{
    public bool editing;

    public List<CircleData> circles = new List<CircleData>();
    private CircleData currentCircle = null;

    public void DrawShapes()
    {
        //Draw Circles
        foreach (var circle in circles)
        {
            Draw.Ring(circle.origin, circle.radius);
        }
    }

    public void DrawEditing()
    {
        if (editing)
        {
            Draw.Ring(currentCircle.origin, currentCircle.radius);

            //Draw controls
            Draw.Disc(currentCircle.origin, 0.1f);
            Draw.Disc(Euclid.snapPos, 0.1f);

            using (Draw.DashedScope())
            {
                Draw.Line(currentCircle.origin, Euclid.snapPos);
            }
        }
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            editing = true;
            currentCircle = new CircleData(Euclid.snapPos, 0);
        }

        if (Input.GetMouseButton(0))
        {
            currentCircle.radius = Vector2.Distance(currentCircle.origin, Euclid.snapPos);
        }

        if (Input.GetMouseButtonUp(0))
        {
            editing = false;
            int poiAdded = UpdatePOI();
            circles.Add(currentCircle);
            Euclid.drawStack.Add(EditMode.Circle, poiAdded);
            currentCircle = null;
        }
    }

    private int UpdatePOI()
    {
        if (currentCircle == null)
            return 0;

        //Add the origin
        int poiAdded = Euclid.POI.AddPoint(currentCircle.origin);

        //Add circle POI
        foreach (var circle in circles)
        {
            int numIntersects = Euclid_Intersection.TryCircleCircle(circle, currentCircle, out Vector2 p1, out Vector2 p2);
            if (numIntersects == 1)
            {
                poiAdded += Euclid.POI.AddPoint(p1);
            }
            else if (numIntersects == 2)
            {
                poiAdded += Euclid.POI.AddPoints(p1, p2);
            }
        }

        //Add line POI
        foreach (var line in Euclid.Lines.lines)
        {
            int numIntersects = Euclid_Intersection.TryLineCircle(line, currentCircle, out Vector2 p1, out Vector2 p2);
            if (numIntersects == 1)
            {
                poiAdded += Euclid.POI.AddPoint(p1);
            }
            else if (numIntersects == 2)
            {
                poiAdded += Euclid.POI.AddPoints(p1, p2);
            }
        }

        return poiAdded;
    }   
}

public class CircleData
{
    public Vector2 origin;
    public float radius;
    public CircleData(Vector2 o, float r)
    {
        this.origin = o;
        this.radius = r;
    }
}
