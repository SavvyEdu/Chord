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
                removeIndex = Euclid.Circles.circles.Count - 1;
                Euclid.Circles.circles.RemoveAt(removeIndex);
                break;
            case EditMode.Line:
                removeIndex = Euclid.Lines.lines.Count - 1;
                Euclid.Lines.lines.RemoveAt(removeIndex);
                break;
            case EditMode.Arc:
                removeIndex = Euclid.Arcs.arcs.Count - 1;
                Euclid.Arcs.arcs.RemoveAt(removeIndex);
                break;
            case EditMode.Segment:
                removeIndex = Euclid.Segments.segments.Count - 1;
                Euclid.Segments.segments.RemoveAt(removeIndex);
                break;
            case EditMode.None:
            default:
                break;
        }

        if (prevDrawAction.poiAdded > 0)
        {
            removeIndex = Euclid.POI.points.Count - prevDrawAction.poiAdded;
            Euclid.POI.points.RemoveRange(removeIndex, prevDrawAction.poiAdded);
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


