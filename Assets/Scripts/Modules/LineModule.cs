using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class LineModule : Module
{
    public bool editing { get; set; }
    public string tooltipMessage { get => "Click and Drag to draw line"; }

    public List<LineData> lines = new List<LineData>();
    private LineData currentLine = null;

    public void DrawShapes()
    {
        //Draw Lines
        foreach (var line in lines)
        {
            //Draw Line Segment
            Draw.Line(line.startPoint, line.endPoint);

            //using (Draw.DashedScope())
            //{
                Vector2 diffNormalized = line.Diff.normalized;
                Draw.Line(line.endPoint, line.endPoint + diffNormalized * 20, Draw.Color / 4);
                Draw.Line(line.startPoint, line.startPoint - diffNormalized * 20, Draw.Color / 4);
            //}
        }
    }

    public void DrawEditing()
    {
        if (editing)
        {
            Draw.Line(currentLine.startPoint, currentLine.endPoint);
            Draw.Disc(currentLine.startPoint, 0.1f);
            Draw.Disc(currentLine.endPoint, 0.1f);
        }
    }

    public void InputDown()
    {
        editing = true;
        currentLine = new LineData(ModuleControl.snapPos, ModuleControl.snapPos);
    }
    public void InputPressed()
    {
        currentLine.endPoint = ModuleControl.snapPos;
    }
    public void InputReleased()
    {
        editing = false;
        Vector2[] newPOI = GetNewPOI();

        //Register Add Line and Add POI for Undo/Redo
        CommandHistory.AddCommand( 
            new MultiCommand(
                new AddToListCommand<LineData>(lines, currentLine),
                new AddRangeToListCommand<Vector2>(LayerController.selectedLayer.modules.POI.points, newPOI)
            ));

        currentLine = null;
    }

    public void WhileEditing() { }

    public Vector2[] GetNewPOI()
    {
        if (currentLine == null)
            return null;

        List<Vector2> possiblePOI = new List<Vector2>();

        possiblePOI.Add(currentLine.startPoint);
        possiblePOI.Add(currentLine.endPoint);

        LayerUtil.ForeachVisibleLine((LineData line) =>
        {
            bool intersect = IntersectHelper.TryLineLine(line, currentLine, out Vector2 p);
            if (intersect)
            {
                possiblePOI.Add(p);
            }
        });

        LayerUtil.ForeachVisibleCircle((CircleData circle) =>
        {
            int numIntersects = IntersectHelper.TryLineCircle(currentLine, circle, out Vector2 p1, out Vector2 p2);
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
        return LayerController.selectedLayer.modules.POI.GetNewPOI(possiblePOI).ToArray();
    }    
}
