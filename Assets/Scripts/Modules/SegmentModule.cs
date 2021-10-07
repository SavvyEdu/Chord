using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class SegmentModule : Module
{
    public bool editing { get; set; }

    public List<SegmentData> segments = new List<SegmentData>();
    private SegmentData currentSegment = null;

    public void DrawShapes()
    {
        foreach (var segment in segments)
        {
            Draw.Line(segment.start, segment.end);
        }
    }

    public void DrawEditing()
    {
        if (editing)
        {
            Draw.Line(currentSegment.start, currentSegment.end);
        }
    }

    public void InputDown()
    {
        editing = true;
        currentSegment = new SegmentData(ModuleControl.snapPos, ModuleControl.snapPos);
    }
    public void InputPressed()
    {
        currentSegment.end = ModuleControl.snapPos;
    }
    public void InputReleased()
    {
        editing = false;
        segments.Add(currentSegment);
        ModuleControl.drawStack.Add(EditMode.Segment, 0);
        currentSegment = null;
    }
    public void WhileEditing() { }
}



