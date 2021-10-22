using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class PolyLineModule : Module<PolyLineData>
{
    public override string tooltipMessage { get => "Left Click to add points\nRight Click to end edit"; }
    public override DrawMode drawMode { get => DrawMode.Final; }
    public override PolyLineData current { get; set; } = null;

    public override void DrawShapes(List<PolyLineData> polyLineData)
    {
        foreach (PolyLineData polyLine in polyLineData)
        {
            for (int i = 0; i < polyLine.path.Count -1; i++)
            {
                Draw.Line(polyLine.path[i], polyLine.path[i+1]);
            }
        }
    }

    public override void DrawEditing()
    {
        if (editing)
        {
            for (int i = 0; i < current.path.Count - 1; i++)
            {
                Draw.Line(current.path[i], current.path[i + 1]);
            }
        }
    }

    public override void MainInputDown()
    {
        editing = true;
        ModuleControl.EnableLineLock();

        if (current == null)
            current = new PolyLineData(ModuleControl.snapPos);

        current.AddPoint(ModuleControl.snapPos);
    }

    public override void AltInputDown()
    {
        editing = false;
        current.RemoveEndPoint();

        CommandHistory.AddCommand(
            new AddToListCommand<PolyLineData>(LayersData.selectedLayer.polyLines, current));

        current = null;
    }

    public override void WhileEditing()
    {
        current.UpdateEndPoint(ModuleControl.snapPos);
    }
}
