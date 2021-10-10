using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DrawStack
{
    public static Stack<UndoAction> stack = new Stack<UndoAction>();

    public static void Add(int layerIndex, bool added) 
    {
        stack.Push(new LayerAction(layerIndex, added));
    }
    public static void Add(EditMode draw, int poiAdded) => Add(draw, poiAdded, LayerController.SelectedIndex); 
    public static void Add(EditMode draw, int poiAdded, int layer)
    {
        stack.Push(new DrawAction(draw, poiAdded, layer));
    }

    public static void Undo()
    {
        if (stack.Count == 0)
            return;

        UndoAction undoAction = stack.Pop();
        if(undoAction is DrawAction)
        {
            DrawAction drawAction = undoAction as DrawAction;

            LayerModules layerModules = LayerController.layers[drawAction.layer].modules;

            int removeIndex;
            switch (drawAction.editMode)
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
                case EditMode.PolyLine:
                    removeIndex = layerModules.PolyLine.polyLines.Count - 1;
                    layerModules.PolyLine.polyLines.RemoveAt(removeIndex);
                    break;
                case EditMode.None:
                default:
                    break;
            }
            if (drawAction.poiAdded > 0)
            {
                removeIndex = layerModules.POI.points.Count - drawAction.poiAdded;
                layerModules.POI.points.RemoveRange(removeIndex, drawAction.poiAdded);
            }
        }
        if(undoAction is LayerAction)
        {
            LayerAction layerAction = undoAction as LayerAction;

            if (layerAction.added)
            {
                LayerController.removeLayer?.Invoke(layerAction.layerIndex);
            }
            else
            {
                LayerModules layerModules = LayerController.layers[layerAction.layerIndex].modules;
            }

        }

    }

    public class UndoAction { }

    public class LayerAction : UndoAction
    {
        public int layerIndex;
        public bool added;

        public LayerAction(int layerIndex, bool added)
        {
            this.layerIndex = layerIndex;
            this.added = added;
        }
    }

    public class DrawAction : UndoAction
    {
        public EditMode editMode;
        public int poiAdded;
        public int layer;

        public DrawAction(EditMode editMode, int poiAdded, int layerIndex)
        {
            this.editMode = editMode;
            this.poiAdded = poiAdded;
            this.layer = layerIndex;
        }
    }
}


