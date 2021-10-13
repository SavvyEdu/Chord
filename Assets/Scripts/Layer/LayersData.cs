using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class LayersData 
{
    public static List<LayerData> layers = new List<LayerData>();
    public static int selectedIndex = 0;

    public static LayerData selectedLayer = null;

    public static UnityAction<LayerData> onLayerAdded;
    public static UnityAction<int> onLayerSelected;
    public static UnityAction<int, int> onLayerSwap;
    public static UnityAction<int> onLayerRemoved;

    public static void AddLayer()
    {
        AddLayer(new LayerData());
    }
    public static void AddLayer(LayerData layerData)
    {
        layers.Add(layerData);
        onLayerAdded?.Invoke(layerData);

        UpdateSelection(layers.Count -1);
    }

    public static void UpdateSelection(LayerData layerData)
    {
        selectedIndex = layers.IndexOf(layerData);
        selectedLayer = layerData;

        onLayerSelected?.Invoke(selectedIndex);
    }

    public static void UpdateSelection(int index)
    {
        selectedIndex = index;
        selectedLayer = layers[index];

        onLayerSelected?.Invoke(selectedIndex);
    }

    public static void RemoveSelectedLayer() => RemoveLayer(selectedIndex);
    public static void RemoveLayer(int removeIndex)
    {
        layers.RemoveAt(removeIndex);
        onLayerRemoved?.Invoke(removeIndex);

        if (layers.Count == 0)
            AddLayer();

        if (removeIndex < selectedIndex || selectedIndex == layers.Count)
            UpdateSelection(selectedIndex - 1);
        else
            UpdateSelection(selectedIndex);
    }

    public static void MoveLayerUp(LayerData layerData)
    {
        int index = layers.IndexOf(layerData);
        if (index != 0)
            SwapLayerOrder(index, index - 1);
    }

    public static void MoveLayerDown(LayerData layerData)
    {
        int index = layers.IndexOf(layerData);
        if (index != layers.Count - 1)
            SwapLayerOrder(index, index + 1);
    }

    public static void SwapLayerOrder(int indexA, int indexB)
    {
        LayerData temp = layers[indexA];
        layers[indexA] = layers[indexB];
        layers[indexB] = temp;

        onLayerSwap?.Invoke(indexA, indexB);
    }
}

public class LayerData
{
    public string name = ""; //TODO
    public bool visible = true;

    //TODO: should be the lists of things like CircleData here;

    public List<CircleData> circles = new List<CircleData>();
    public List<LineData> lines = new List<LineData>();
    public List<ArcData> arcs = new List<ArcData>();
    public List<SegmentData> segments = new List<SegmentData>();
    public List<PolyLineData> polyLines = new List<PolyLineData>();
    public List<Vector2> poi = new List<Vector2>();
}