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

    public void Add(EditMode draw, int poiAdded) { Add(draw, poiAdded, LayerController.SelectedIndex); }
    public void Add(EditMode draw, int poiAdded, int layer)
    {
        stack.Push(new DrawAction(draw, poiAdded, layer));
    }

    public void Undo()
    {
        if (stack.Count == 0)
            return;

        DrawAction prevDrawAction = stack.Pop();
        LayerModules layerModules = LayerController.layers[prevDrawAction.layer].modules;

        int removeIndex;
        switch (prevDrawAction.editMode)
        {
            case EditMode.Circle:
                removeIndex = layerModules.Circles.circles.Count - 1;
                layerModules.Circles.circles.RemoveAt(removeIndex);
                break;
            case EditMode.Line:
                removeIndex = layerModules.Lines.lines.Count - 1;
                layerModules.Lines.lines.RemoveAt(removeIndex);
                break;
            case EditMode.Arc:
                removeIndex = layerModules.Arcs.arcs.Count - 1;
                layerModules.Arcs.arcs.RemoveAt(removeIndex);
                break;
            case EditMode.Segment:
                removeIndex = layerModules.Segments.segments.Count - 1;
                layerModules.Segments.segments.RemoveAt(removeIndex);
                break;
            case EditMode.None:
            default:
                break;
        }

        if (prevDrawAction.poiAdded > 0)
        {
            removeIndex = layerModules.POI.points.Count - prevDrawAction.poiAdded;
            layerModules.POI.points.RemoveRange(removeIndex, prevDrawAction.poiAdded);
        }
    }


    public struct DrawAction
    {
        public EditMode editMode;
        public int poiAdded;
        public int layer;

        public DrawAction(EditMode editMode, int poiAdded, int layer)
        {
            this.editMode = editMode;
            this.poiAdded = poiAdded;
            this.layer = layer;
        }
    }
}


