using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class LineModule : Module<LineData>
{
    public override string tooltipMessage { get => "Click and Drag to draw line"; }
    public override DrawMode drawMode { get => DrawMode.Guides; }
    public override LineData current { get; set; } = null;

    public override void DrawShapes(List<LineData> lineData)
    {
        //Draw Lines
        foreach (var line in lineData)
        {
            //Draw visible line Segment
            if (IntersectHelper.TryLineRect(line, ModuleControl.cameraRect, out Vector2 p1, out Vector2 p2))
            {
                Draw.Line(p1, p2);
            }
        }
    }

    public override void DrawEditing()
    {
        if (editing)
        {
            Draw.Line(current.startPoint, current.endPoint);
            Draw.Disc(current.startPoint, 0.1f);
            Draw.Disc(current.endPoint, 0.1f);
        }
    }

    public override void MainInputDown()
    {
        editing = true;
        current = new LineData(ModuleControl.snapPos, ModuleControl.snapPos);
    }
    public override void MainInputPressed()
    {
        current.endPoint = ModuleControl.snapPos;
    }
    public override void MainInputReleased()
    {
        editing = false;

        Vector2[] newPOI = LayerIntersectHelper.GetLineIntersects(current, LayersData.selectedLayer);

        //Register Add Line and Add POI for Undo/Redo
        CommandHistory.AddCommand( 
            new MultiCommand(
                new AddToListCommand<LineData>(LayersData.selectedLayer.lines, current),
                new AddRangeToListCommand<Vector2>(LayersData.selectedLayer.poi, newPOI)
            ));

        current = null;
    }
}
