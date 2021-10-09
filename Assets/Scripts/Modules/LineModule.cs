using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class LineModule : Module
{
    public bool editing { get; set; }

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
        int poiAdded = UpdatePOI();
        lines.Add(currentLine);
        ModuleControl.drawStack.Add(EditMode.Line, poiAdded);
        currentLine = null;
    }

    public void WhileEditing() { }

    public int UpdatePOI()
    {
        if (currentLine == null)
            return 0;

        POIModule POI = LayerController.selectedLayer.modules.POI;
        int poiAdded = POI.AddPoints(currentLine.startPoint, currentLine.endPoint);

        LayerUtil.ForeachVisibleLine((LineData line) =>
        {
            bool intersect = IntersectHelper.TryLineLine(line, currentLine, out Vector2 p);
            if (intersect)
            {
                poiAdded += POI.AddPoint(p);
            }
        });

        LayerUtil.ForeachVisibleCircle((CircleData circle) =>
        {
            int numIntersects = IntersectHelper.TryLineCircle(currentLine, circle, out Vector2 p1, out Vector2 p2);
            if (numIntersects == 1)
            {
                poiAdded += POI.AddPoint(p1);
            }
            else if (numIntersects == 2)
            {
                poiAdded += POI.AddPoints(p1, p2);
            }
        });

        return poiAdded;
    }    
}
