using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class CircleModule : Module
{
    public bool editing { get; set; }
    public string tooltipMessage { get => "Click and Drag to draw circle"; }

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
        Vector2[] newPOI = GetNewPOI();

        //Register Add Line and Add POI for Undo/Redo
        CommandHistory.AddCommand(
            new MultiCommand(
                new AddToListCommand<CircleData>(circles, currentCircle),
                new AddRangeToListCommand<Vector2>(LayerController.selectedLayer.modules.POI.points, newPOI)
            ));

        currentCircle = null;
    }
    public void WhileEditing() { }

    private Vector2[] GetNewPOI()
    {
        if (currentCircle == null)
            return null;

        List<Vector2> possiblePOI = new List<Vector2>();
        possiblePOI.Add(currentCircle.origin);

        LayerUtil.ForeachVisibleCircle((CircleData circle) =>
        {
            int numIntersects = IntersectHelper.TryCircleCircle(circle, currentCircle, out Vector2 p1, out Vector2 p2);
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
            int numIntersects = IntersectHelper.TryLineCircle(line, currentCircle, out Vector2 p1, out Vector2 p2);
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

        return LayerController.selectedLayer.modules.POI.GetNewPOI(possiblePOI).ToArray();
    }   
}

