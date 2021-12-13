using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class CircleModule : Module<CircleData>
{
    public override string tooltipMessage { get => "Click and Drag to draw circle"; }
    public override DrawMode drawMode { get => DrawMode.Guides; }
    public override CircleData current { get; set; } = null;

    public override void DrawShapes(List<CircleData> circleData)
    {
        //Draw Circles
        foreach (var circle in circleData)
        {
            Draw.Ring(circle.origin, circle.radius);
        }
    }

    public override void DrawEditing()
    {
        if (editing)
        {
            Draw.Ring(current.origin, current.radius);

            //Draw controls
            Draw.Disc(current.origin, 0.1f);
            Draw.Disc(ModuleControl.snapPos, 0.1f);

            using (Draw.DashedScope())
            {
                Draw.Line(current.origin, ModuleControl.snapPos);
            }
        }
    }

    public override void MainInputDown()
    {
        editing = true;
        current = new CircleData(ModuleControl.snapPos, 0);
    }

    public override void MainInputPressed()
    {
        current.radius = Vector2.Distance(current.origin, ModuleControl.snapPos);
    }

    public override void MainInputReleased()
    {
        editing = false;

        Vector2[] newPOI = LayerIntersectHelper.GetCircleIntersects(current, LayersData.selectedLayer);

        //Register Add Line and Add POI for Undo/Redo
        CommandHistory.AddCommand(
            new MultiCommand(
                new AddToListCommand<CircleData>(LayersData.selectedLayer.circles, current),
                new AddRangeToListCommand<Vector2>(LayersData.selectedLayer.poi, newPOI)
            ));

        current = null;
    }
}

