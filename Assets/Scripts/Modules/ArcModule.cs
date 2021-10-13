﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public enum ArcDrawMode { start, origin, end}

public class ArcModule : Module<ArcData>
{
    public override string tooltipMessage { get => "Click to add start point, origin, end point"; }
    public override ArcData current { get; set; } = null;

    private ArcDrawMode drawMode = ArcDrawMode.start;

    public override void DrawShapes(List<ArcData> arcs)
    {
        //Draw Circles
        foreach (var arc in arcs)
        {
            Draw.Arc(arc.origin, arc.radius, arc.startAngle, arc.endAngle);
        }
    }

    public override void DrawEditing()
    {
        if (editing)
        {
            Draw.Arc(current.origin, current.radius, current.startAngle, current.endAngle);
            using (Draw.DashedScope())
            {
                Draw.Line(current.startPoint, current.origin);
                if (drawMode == ArcDrawMode.end)
                    Draw.Line(current.origin, current.endPoint);
            }

            //Draw controls
            Draw.Disc(current.origin, 0.1f);
            Draw.Disc(current.startPoint, 0.1f);
            Draw.Disc(current.endPoint, 0.1f);
        }
    }

    public override void InputDown()
    {
        if (drawMode == 0)
        {
            editing = true;
            current = new ArcData(ModuleControl.snapPos, 0);

            drawMode = ArcDrawMode.origin;
        }
        else if (drawMode == ArcDrawMode.origin)
        {
            current.origin = ModuleControl.snapPos;
            current.radius = Vector2.Distance(current.startPoint, ModuleControl.snapPos);
            current.startAngle = Vector2.SignedAngle(Vector2.right, current.startPoint - current.origin) * Mathf.Deg2Rad;

            drawMode = ArcDrawMode.end;
        }
        else if (drawMode == ArcDrawMode.end)
        {
            current.endPoint = current.origin + (ModuleControl.snapPos - current.origin).normalized * current.radius;

            CommandHistory.AddCommand(
                new AddToListCommand<ArcData>(LayersData.selectedLayer.arcs, current));

            current = null;
            editing = false;
            drawMode = ArcDrawMode.start;
        }
    }
    public override void WhileEditing()
    {
        if (editing)
        {
            if (drawMode == ArcDrawMode.origin)
            {
                current.origin = ModuleControl.snapPos;
                current.radius = Vector2.Distance(current.startPoint, ModuleControl.snapPos);
            }
            else if (drawMode == ArcDrawMode.end)
            {
                current.endPoint = current.origin + (ModuleControl.snapPos - current.origin).normalized * current.radius;
                current.endAngle = current.startAngle + Vector2.SignedAngle(current.startPoint - current.origin, current.endPoint - current.origin) * Mathf.Deg2Rad;
            }
        }
    }
}
