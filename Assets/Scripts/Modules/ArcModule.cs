using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public enum ArcDrawMode { start, origin, end}

public class ArcModule : Module
{
    public bool editing { get; set; }

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

    public void InputDown()
    {
        if (drawMode == 0)
        {
            editing = true;
            currentArc = new ArcData(ModuleControl.snapPos, 0);

            drawMode = ArcDrawMode.origin;
        }
        else if (drawMode == ArcDrawMode.origin)
        {
            currentArc.origin = ModuleControl.snapPos;
            currentArc.radius = Vector2.Distance(currentArc.startPoint, ModuleControl.snapPos);
            currentArc.startAngle = Vector2.SignedAngle(Vector2.right, currentArc.startPoint - currentArc.origin) * Mathf.Deg2Rad;

            drawMode = ArcDrawMode.end;
        }
        else if (drawMode == ArcDrawMode.end)
        {
            currentArc.endPoint = currentArc.origin + (ModuleControl.snapPos - currentArc.origin).normalized * currentArc.radius;
            arcs.Add(currentArc);
            ModuleControl.drawStack.Add(EditMode.Arc, 0);

            currentArc = null;
            editing = false;
            drawMode = ArcDrawMode.start;
        }
    }
    public void InputPressed() { }
    public void InputReleased() { }
    public void WhileEditing()
    {
        if (editing)
        {
            if (drawMode == ArcDrawMode.origin)
            {
                currentArc.origin = ModuleControl.snapPos;
                currentArc.radius = Vector2.Distance(currentArc.startPoint, ModuleControl.snapPos);
            }
            else if (drawMode == ArcDrawMode.end)
            {
                currentArc.endPoint = currentArc.origin + (ModuleControl.snapPos - currentArc.origin).normalized * currentArc.radius;
                currentArc.endAngle = currentArc.startAngle + Vector2.SignedAngle(currentArc.startPoint - currentArc.origin, currentArc.endPoint - currentArc.origin) * Mathf.Deg2Rad;
            }
        }
    }
}
