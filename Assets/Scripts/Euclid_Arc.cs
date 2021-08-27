using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public enum ArcDrawMode { start, origin, end}

public class Euclid_Arc
{
    public bool editing;

    private ArcDrawMode drawMode = ArcDrawMode.start;

    public List<ArcData> arcs = new List<ArcData>();
    private ArcData currentArc = null;

    public void DrawShapes()
    {
        //Draw Circles
        foreach (var arc in arcs)
        {
            Draw.Arc(arc.origin, arc.radius, arc.startAngle, arc.endAngle);
        }
    }

    public void DrawEditing()
    {
        if (editing)
        {
            Draw.Arc(currentArc.origin, currentArc.radius, currentArc.startAngle, currentArc.endAngle);
            using (Draw.DashedScope())
            {
                Draw.Line(currentArc.startPoint, currentArc.origin);
                if (drawMode == ArcDrawMode.end)
                    Draw.Line(currentArc.origin, currentArc.endPoint);
            }

            //Draw controls
            Draw.Disc(currentArc.origin, 0.1f);
            Draw.Disc(currentArc.startPoint, 0.1f);
            Draw.Disc(currentArc.endPoint, 0.1f);
        }
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if(drawMode == 0)
            {
                editing = true;
                currentArc = new ArcData(Euclid.snapPos, 0);

                drawMode = ArcDrawMode.origin;
            }
            else if (drawMode == ArcDrawMode.origin)
            {
                currentArc.origin = Euclid.snapPos;
                currentArc.radius = Vector2.Distance(currentArc.startPoint, Euclid.snapPos);
                currentArc.startAngle = Vector2.SignedAngle(Vector2.right, currentArc.startPoint - currentArc.origin) * Mathf.Deg2Rad;

                drawMode = ArcDrawMode.end;
            }
            else if (drawMode == ArcDrawMode.end)
            {
                currentArc.endPoint = currentArc.origin + (Euclid.snapPos - currentArc.origin).normalized * currentArc.radius;
                arcs.Add(currentArc);
                Euclid.drawStack.Add(EditMode.Arc, 0);

                currentArc = null;
                editing = false;
                drawMode = ArcDrawMode.start;
            }
        }

        if (editing)
        {
            if (drawMode == ArcDrawMode.origin)
            {
                currentArc.origin = Euclid.snapPos;
                currentArc.radius = Vector2.Distance(currentArc.startPoint, Euclid.snapPos);
            }
            else if (drawMode == ArcDrawMode.end)
            {
                currentArc.endPoint = currentArc.origin + (Euclid.snapPos - currentArc.origin).normalized * currentArc.radius;
                currentArc.endAngle = currentArc.startAngle + Vector2.SignedAngle(currentArc.startPoint - currentArc.origin, currentArc.endPoint - currentArc.origin) * Mathf.Deg2Rad;
            }
        }
    }
}

public class ArcData
{
    public float radius;
    public Vector2 origin;
    public Vector2 startPoint;
    public Vector2 endPoint;
    public float startAngle;
    public float endAngle;
    public ArcData(Vector2 s, float r)
    {
        this.startPoint = s;
        this.endPoint = s;
        this.origin = s;
        this.radius = r;
    }
}
