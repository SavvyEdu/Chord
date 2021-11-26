using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

//Set the Origin, then start, then end
public enum ArcMode {origin, start, end}

public class ArcModule : Module<ArcData>
{
    public override string tooltipMessage { get => "Click to add start point, origin, end point"; }
    public override DrawMode drawMode { get => DrawMode.Final; }
    public override ArcData current { get; set; } = null;

    protected ArcMode arcMode = ArcMode.origin;

    public override void DrawShapes(List<ArcData> arcs)
    {
        //Draw Arcs
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
                Draw.Line(current.origin, current.startPoint);
                if (arcMode == ArcMode.end)
                    Draw.Line(current.origin, current.endPoint);
            }

            //Draw controls
            Draw.Disc(current.origin, 0.1f);
            Draw.Disc(current.startPoint, 0.1f);
            Draw.Disc(current.endPoint, 0.1f);
        }
    }

    public override void MainInputDown()
    {

        if(arcMode == ArcMode.origin)
        {
            //Begin creating arc from the origin
            editing = true;
            current = new ArcData(ModuleControl.snapPos, 0);
            arcMode = ArcMode.start;
        }
        else if (arcMode == ArcMode.start)
        {
            //Set the start point of the arc
            current.startPoint = ModuleControl.snapPos;
            current.radius = Vector2.Distance(current.origin, current.startPoint);
            current.startAngle = current.AngleFrom0(current.startPoint);

            arcMode = ArcMode.end;
        }
        else if (arcMode == ArcMode.end)
        {
            current.endPoint = current.origin + (ModuleControl.snapPos - current.origin).normalized * current.radius;
            current.endAngle = current.startAngle + current.AngleFromStart(current.endPoint);

            CommandHistory.AddCommand(
                new AddToListCommand<ArcData>(LayersData.selectedLayer.arcs, current));

            current = null;
            editing = false;
            arcMode = ArcMode.origin;
        }
    }
    public override void WhileEditing()
    {
        if (arcMode == ArcMode.start)
        {
            current.startPoint = ModuleControl.snapPos;
            current.radius = Vector2.Distance(current.startPoint, ModuleControl.snapPos);
        }
        else if (arcMode == ArcMode.end)
        {
            current.endPoint = current.origin + (ModuleControl.snapPos - current.origin).normalized * current.radius;
            current.endAngle = current.startAngle + current.AngleFromStart(current.endPoint);

        }
    }
}
