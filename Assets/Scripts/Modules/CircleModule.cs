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

    public override void InputDown()
    {
        editing = true;
        ModuleControl.EnableLineLock();
        current = new CircleData(ModuleControl.snapPos, 0);
    }

    public override void InputPressed()
    {
        current.radius = Vector2.Distance(current.origin, ModuleControl.snapPos);
    }

    public override void InputReleased()
    {
        editing = false;
        ModuleControl.DisableLineLock();

        Vector2[] newPOI = GetNewPOI();

        //Register Add Line and Add POI for Undo/Redo
        CommandHistory.AddCommand(
            new MultiCommand(
                new AddToListCommand<CircleData>(LayersData.selectedLayer.circles, current),
                new AddRangeToListCommand<Vector2>(LayersData.selectedLayer.poi, newPOI)
            ));

        current = null;
    }

    private Vector2[] GetNewPOI()
    {
        if (current == null)
            return null;

        List<Vector2> possiblePOI = new List<Vector2>();
        possiblePOI.Add(current.origin);

        LayerUtil.ForeachVisibleCircle((CircleData circle) =>
        {
            int numIntersects = IntersectHelper.TryCircleCircle(circle, current, out Vector2 p1, out Vector2 p2);
            if (numIntersects == 1)
            {
                possiblePOI.Add(p1);
            }
            else if (numIntersects == 2)
            {
                possiblePOI.Add(p1);
                possiblePOI.Add(p2);
            }
        });

        LayerUtil.ForeachVisibleLine((LineData line) =>
        {
            int numIntersects = IntersectHelper.TryLineCircle(line, current, out Vector2 p1, out Vector2 p2);
            if (numIntersects == 1)
            {
                possiblePOI.Add(p1);
            }
            else if (numIntersects == 2)
            {
                possiblePOI.Add(p1);
                possiblePOI.Add(p2);
            }
        });

        return ModuleControl.POI.GetNewPOI(possiblePOI, LayersData.selectedLayer.poi).ToArray();
    }   
}

