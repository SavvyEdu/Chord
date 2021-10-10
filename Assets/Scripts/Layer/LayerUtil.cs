using System;
using UnityEngine;

public struct LayerUtil
{
    public static void ForeachVisibleLayer(Action<Layer> action)
    {
        if (LayerController.layers == null)
            return;

        foreach (Layer layer in LayerController.layers)
        {
            if (layer.IsVisible)
                action?.Invoke(layer);
        }
    }

    public static void ForeachVisibleLine(Action<LineData> action)
    {
        ForeachVisibleLayer((Layer layer) =>
        {
            foreach (LineData line in layer.modules.Lines.lines)
            {
                action?.Invoke(line);
            }
        });
    }

    public static void ForeachVisibleCircle(Action<CircleData> action)
    {
        ForeachVisibleLayer((Layer layer) =>
        {
            foreach (CircleData circle in layer.modules.Circles.circles)
            {
                action?.Invoke(circle);
            }
        });
    }

    public static void ForeachVisibePOI(Action<Vector2> action)
    {
        ForeachVisibleLayer((Layer layer) =>
        {
            foreach (Vector2 poi in layer.modules.POI.points)
            {
                action?.Invoke(poi);
            }
        });
    }

}
