using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class SegmentModule : Module
{
    public bool editing { get; set; }
    public string tooltipMessage { get => "Click and Drag to draw segment"; }

    public List<SegmentData> segments = new List<SegmentData>();
    private SegmentData currentSegment = null;

    public void DrawShapes()
    {
        foreach (var segment in segments)
        {
            Draw.Line(segment.startPoint, segment.endPoint);
        }
    }

    public void DrawEditing()
    {
        if (editing)
        {
            Draw.Line(currentSegment.startPoint, currentSegment.endPoint);
        }
    }

    public void InputDown()
    {
        editing = true;
        currentSegment = new SegmentData(ModuleControl.snapPos, ModuleControl.snapPos);
    }
    public void InputPressed()
    {
        currentSegment.endPoint = ModuleControl.snapPos;
    }
    public void InputReleased()
    {
        editing = false;

        CommandHistory.AddCommand(
            new AddToListCommand<SegmentData>(segments, currentSegment));

        currentSegment = null;
    }
    public void WhileEditing() { }
}



