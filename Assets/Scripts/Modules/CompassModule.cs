using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class CompassModule : ArcModule
{
    public override string tooltipMessage => "Left Click to draw\nRight Click to resize";
    public override DrawMode drawMode { get => DrawMode.Guides; }

    public bool resizing = false;
    public CircleData compass = new CircleData(Vector2.zero, 2);

    public override void DrawEditing()
    {
        if (editing)
        {
            if (resizing)
            {
                using (Draw.DashedScope())
                {
                    Draw.Ring(compass.origin, compass.radius);
                    Draw.Line(compass.origin, ModuleControl.snapPos);
                    Draw.Disc(compass.origin, 0.1f);
                    Draw.Disc(ModuleControl.snapPos, 0.1f);
                }
            }
            else
            {
                Draw.Arc(current.origin, current.radius, current.startAngle, current.endAngle);
                using (Draw.DashedScope())
                {
                    Draw.Ring(compass.origin, compass.radius);
                    Draw.Line(current.origin, current.startPoint);
                    if (arcMode == ArcMode.end)
                        Draw.Line(current.origin, current.endPoint);
                }
                Draw.Disc(current.origin, 0.1f);
                Draw.Disc(current.startPoint, 0.1f);
                Draw.Disc(current.endPoint, 0.1f);
            }
        }
        else
        {
            //Draw Compass at Mouse position 
            using (Draw.DashedScope())
            {
                Draw.Ring(ModuleControl.snapPos, compass.radius);
            }
        }
    }


    public override void MainInputDown()
    {
        if(arcMode == ArcMode.origin)
        {
            editing = true;
            compass.origin = ModuleControl.snapPos;
            current = new ArcData(compass.origin, compass.radius);
            arcMode = ArcMode.start;
        }
        else if (arcMode == ArcMode.start)
        {
            current.startPoint = current.origin + (ModuleControl.snapPos - current.origin).normalized * current.radius;
            current.startAngle = Vector2.SignedAngle(Vector2.right, current.startPoint - current.origin) * Mathf.Deg2Rad;
            arcMode = ArcMode.end;
        }
        else if (arcMode == ArcMode.end)
        {
            current.endPoint = current.origin + (ModuleControl.snapPos - current.origin).normalized * current.radius;

            CommandHistory.AddCommand(
                new AddToListCommand<ArcData>(LayersData.selectedLayer.compassArcs, current));

            current = null;
            editing = false;
            arcMode = ArcMode.origin;
        }
    }

    public override void WhileEditing()
    {
        if (arcMode == ArcMode.start)
        {
            current.startPoint = current.origin + (ModuleControl.snapPos - current.origin).normalized * current.radius;
        }
        else if (arcMode == ArcMode.end)
        {
            current.endPoint = current.origin + (ModuleControl.snapPos - current.origin).normalized * current.radius;
            current.endAngle = current.startAngle + Vector2.SignedAngle(current.startPoint - current.origin, current.endPoint - current.origin) * Mathf.Deg2Rad;
        }
    }

    public override void AltInputDown()
    {
        resizing = true;
        editing = true;
        compass.origin = ModuleControl.snapPos;
    }

    public override void AltInputPressed()
    {
        compass.radius = Vector2.Distance(ModuleControl.snapPos, compass.origin);
    }

    public override void AltInputReleased()
    {
        resizing = false;
        editing = false;
    }
}
