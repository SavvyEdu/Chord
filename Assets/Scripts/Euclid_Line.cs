using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class Euclid_Line
{
    private bool editing;

    public List<LineData> lines = new List<LineData>();
    private LineData currentLine = null;

    public void DrawShapes()
    {
        //Draw Lines
        foreach (var line in lines)
        {
            //Draw Line Segment
            Draw.Line(line.start, line.end);

            //using (Draw.DashedScope())
            //{
                Vector2 diffNormalized = line.Diff.normalized;
                Draw.Line(line.end, line.end + diffNormalized * 20, Draw.Color / 4);
                Draw.Line(line.start, line.start - diffNormalized * 20, Draw.Color / 4);
            //}
        }
    }

    public void DrawEditing()
    {
        if (editing)
        {
            Draw.Line(currentLine.start, currentLine.end);
            Draw.Disc(currentLine.start, 0.1f);
            Draw.Disc(currentLine.end, 0.1f);
        }
    }

    public void Update()
    {
        //Start a new Line
        if (Input.GetMouseButtonDown(0))
        {
            editing = true;
            currentLine = new LineData(Euclid.snapPos, Euclid.snapPos);
        }

        //Update the current line
        if (Input.GetMouseButton(0))
        {
            currentLine.end = Euclid.snapPos;
        }

        if (Input.GetMouseButtonUp(0))
        {
            editing = false;
            int poiAdded = UpdatePOI();
            lines.Add(currentLine);
            Euclid.drawStack.Add(EditMode.Line, poiAdded);
            currentLine = null;
        }
    }

    private int UpdatePOI()
    {
        if (currentLine == null)
            return 0;

        //add the line ends 
        int poiAdded = Euclid.POI.AddPoints(currentLine.start, currentLine.end);

        foreach (var line in lines)
        {
            bool intersect = Euclid_Intersection.TryLineLine(line, currentLine, out Vector2 p);
            if (intersect)
            {
                poiAdded += Euclid.POI.AddPoint(p);
            }
        }

        foreach (var circle in Euclid.Circles.circles)
        {
            int numIntersects = Euclid_Intersection.TryLineCircle(currentLine, circle, out Vector2 p1, out Vector2 p2);
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

public class LineData
{
    public Vector2 start;
    public Vector2 end;

    public Vector2 Diff { get { return end - start; } }

    public LineData(Vector2 s, Vector2 e)
    {
        this.start = s;
        this.end = e;
    }
}
