using System;
using UnityEngine;

public struct LayerUtil
{
    public static void ForeachVisibleLayer(Action<LayerData> action)
    {
        foreach (LayerData layerData in LayersData.layers)
        {
            if (layerData.visible)
                action?.Invoke(layerData);
        }
    }

    public static void ForeachVisibleLine(Action<LineData> action)
    {
        ForeachVisibleLayer((LayerData layerData) =>
        {
            foreach (LineData lineData in layerData.Lines.lines)
            {
                action?.Invoke(lineData);
            }
        });
    }

    public static void ForeachVisibleCircle(Action<CircleData> action)
    {
        ForeachVisibleLayer((LayerData layerData) =>
        {
            foreach (CircleData circleData in layerData.Circles.circles)
            {
                action?.Invoke(circleData);
            }
        });
    }

    public static void ForeachVisibePOI(Action<Vector2> action)
    {
        ForeachVisibleLayer((LayerData layer) =>
        {
            foreach (Vector2 poi in layer.POI.points)
            {
                action?.Invoke(poi);
            }
        });
    }

}
