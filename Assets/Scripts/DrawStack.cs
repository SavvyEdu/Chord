using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DrawStack
{
    public Stack<DrawAction> stack;

    public DrawStack()
    {
        stack = new Stack<DrawAction>();
    }

    public void Add(EditMode draw, int poiAdded)
    {
        stack.Push(new DrawAction(draw, poiAdded));
    }

    public void Undo()
    {
        if (stack.Count == 0)
            return;

        DrawAction prevDrawAction = stack.Pop();

        int removeIndex;
        switch (prevDrawAction.editMode)
        {
            case EditMode.Circle:
                removeIndex = ModuleControl.Circles.circles.Count - 1;
                ModuleControl.Circles.circles.RemoveAt(removeIndex);
                break;
            case EditMode.Line:
                removeIndex = ModuleControl.Lines.lines.Count - 1;
                ModuleControl.Lines.lines.RemoveAt(removeIndex);
                break;
            case EditMode.Arc:
                removeIndex = ModuleControl.Arcs.arcs.Count - 1;
                ModuleControl.Arcs.arcs.RemoveAt(removeIndex);
                break;
            case EditMode.Segment:
                removeIndex = ModuleControl.Segments.segments.Count - 1;
                ModuleControl.Segments.segments.RemoveAt(removeIndex);
                break;
            case EditMode.None:
            default:
                break;
        }

        if (prevDrawAction.poiAdded > 0)
        {
            removeIndex = ModuleControl.POI.points.Count - prevDrawAction.poiAdded;
            ModuleControl.POI.points.RemoveRange(removeIndex, prevDrawAction.poiAdded);
        }
    }


    public struct DrawAction
    {
        public EditMode editMode;
        public int poiAdded;

        public DrawAction(EditMode editMode, int poiAdded)
        {
            this.editMode = editMode;
            this.poiAdded = poiAdded;
        }
    }
}


