using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class SegmentModule : Module<SegmentData>
{
    public override string tooltipMessage { get => "Click and Drag to draw segment"; }
    public override DrawMode drawMode { get => DrawMode.Final; }
    public override SegmentData current { get; set;} = null;

    public override void DrawShapes(List<SegmentData> segmentData)
    {
        foreach (var segment in segmentData)
        {
            Draw.Line(segment.startPoint, segment.endPoint);
        }
    }

    public override void DrawEditing()
    {
        if (editing)
        {
            Draw.Line(current.startPoint, current.endPoint);
        }
    }

    public override void MainInputDown()
    {
        editing = true;
        current = new SegmentData(ModuleControl.snapPos, ModuleControl.snapPos);
    }
    public override void MainInputPressed()
    {
        current.endPoint = ModuleControl.snapPos;
    }
    public override void MainInputReleased()
    {
        editing = false;

        CommandHistory.AddCommand(
            new AddToListCommand<SegmentData>(LayersData.selectedLayer.segments, current));

        current = null;
    }
}



