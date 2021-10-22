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
            //Draw.Line(line.startPoint, line.endPoint);

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

        Vector2[] newPOI = GetNewPOI();

        //Register Add Line and Add POI for Undo/Redo
        CommandHistory.AddCommand( 
            new MultiCommand(
                new AddToListCommand<LineData>(LayersData.selectedLayer.lines, current),
                new AddRangeToListCommand<Vector2>(LayersData.selectedLayer.poi, newPOI)
            ));

        current = null;
    }

    public Vector2[] GetNewPOI()
    {
        if (current == null)
            return null;

        List<Vector2> possiblePOI = new List<Vector2>();

        possiblePOI.Add(current.startPoint);
        possiblePOI.Add(current.endPoint);

        LayerUtil.ForeachVisibleLine((LineData line) =>
        {
            bool intersect = IntersectHelper.TryLineLine(line, current, out Vector2 p);
            if (intersect)
            {
                possiblePOI.Add(p);
            }
        });

        LayerUtil.ForeachVisibleCircle((CircleData circle) =>
        {
            int numIntersects = IntersectHelper.TryLineCircle(current, circle, out Vector2 p1, out Vector2 p2);
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

        //return the new POI
        return ModuleControl.POI.GetNewPOI(possiblePOI, LayersData.selectedLayer.poi).ToArray();
    }  

}
