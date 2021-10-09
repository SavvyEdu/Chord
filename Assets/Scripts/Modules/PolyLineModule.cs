using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class PolyLineModule : Module
{
    public bool editing { get; set; }

    public List<PolyLineData> polyLines = new List<PolyLineData>();
    public PolyLineData currentPolyLine;

    public void DrawShapes()
    {
        foreach (PolyLineData polyLine in polyLines)
        {
            for (int i = 0; i < polyLine.path.Count -1; i++)
            {
                Draw.Line(polyLine.path[i], polyLine.path[i+1]);
            }
        }
    }

    public void DrawEditing()
    {
        if (editing)
        {
            for (int i = 0; i < currentPolyLine.path.Count - 1; i++)
            {
                Draw.Line(currentPolyLine.path[i], currentPolyLine.path[i + 1]);
            }
        }
    }

    public void InputDown()
    {
        editing = true;

        if (currentPolyLine == null)
            currentPolyLine = new PolyLineData(ModuleControl.snapPos);

        currentPolyLine.AddPoint(ModuleControl.snapPos);
    }

    public void InputPressed() { }
    public void InputReleased() { }
    public void WhileEditing()
    {
        if (editing)
        {
            currentPolyLine.UpdateEndPoint(ModuleControl.snapPos);

            if (Input.GetKeyDown(KeyCode.X))
            {
                editing = false;

                currentPolyLine.RemoveEndPoint();
                polyLines.Add(currentPolyLine);
                currentPolyLine = null;
            }

        }
    }
}
