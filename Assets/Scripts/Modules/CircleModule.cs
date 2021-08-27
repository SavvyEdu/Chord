﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class CircleModule : Module
{
    public bool editing { get; set; }

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
            Draw.Disc(ModuleControl.snapPos, 0.1f);

            using (Draw.DashedScope())
            {
                Draw.Line(currentCircle.origin, ModuleControl.snapPos);
            }
        }
    }

    public void InputDown()
    {
        editing = true;
        currentCircle = new CircleData(ModuleControl.snapPos, 0);
    }
    public void InputPressed()
    {
        currentCircle.radius = Vector2.Distance(currentCircle.origin, ModuleControl.snapPos);
    }
    public void InputReleased()
    {
        editing = false;
        int poiAdded = UpdatePOI();
        circles.Add(currentCircle);
        ModuleControl.drawStack.Add(EditMode.Circle, poiAdded);
        currentCircle = null;
    }
    public void WhileEditing() { }

    private int UpdatePOI()
    {
        if (currentCircle == null)
            return 0;

        //Add the origin
        int poiAdded = ModuleControl.POI.AddPoint(currentCircle.origin);

        //Add circle POI
        foreach (var circle in circles)
        {
            int numIntersects = IntersectHelper.TryCircleCircle(circle, currentCircle, out Vector2 p1, out Vector2 p2);
            if (numIntersects == 1)
            {
                poiAdded += ModuleControl.POI.AddPoint(p1);
            }
            else if (numIntersects == 2)
            {
                poiAdded += ModuleControl.POI.AddPoints(p1, p2);
            }
        }

        //Add line POI
        foreach (var line in ModuleControl.Lines.lines)
        {
            int numIntersects = IntersectHelper.TryLineCircle(line, currentCircle, out Vector2 p1, out Vector2 p2);
            if (numIntersects == 1)
            {
                poiAdded += ModuleControl.POI.AddPoint(p1);
            }
            else if (numIntersects == 2)
            {
                poiAdded += ModuleControl.POI.AddPoints(p1, p2);
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