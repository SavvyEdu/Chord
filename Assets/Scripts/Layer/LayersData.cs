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
    public static UnityAction<int, LayerData> onLayerInserted;
    public static UnityAction<int> onLayerSelected;
    public static UnityAction<int, int> onLayerSwap;
    public static UnityAction<int> onLayerRemoved;

    /// <summary> Methods that create the Undoable commands </summary>
    #region COMMAND CALLERS 
    public static void AddLayer()
    {
        //AddLayer(new LayerData());
        CommandHistory.AddCommand(new AddLayerCommand(new LayerData()));
    }
    public static void RemoveSelectedLayer()
    {
        CommandHistory.AddCommand(new RemoveLayerCommand(selectedLayer, selectedIndex));
    }

    public static void MoveLayerUp(LayerData layerData)
    {
        int index = layers.IndexOf(layerData);
        if (index != 0)
            CommandHistory.AddCommand(new SwapLayerCommand(index, index - 1));
    }

    public static void MoveLayerDown(LayerData layerData)
    {
        int index = layers.IndexOf(layerData);
        if (index != layers.Count - 1)
            CommandHistory.AddCommand(new SwapLayerCommand(index, index + 1));
    }
    #endregion

    /// <summary> Methods called by commands that update the list and call actions </summary>
    #region EVENT CALLERS
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

    public static void AddLayer(LayerData layerData)
    {
        layers.Add(layerData);
        onLayerAdded?.Invoke(layerData);

        UpdateSelection(layers.Count -1); //immediate selection
    }

    public static void InsertLayer(int addIndex, LayerData layerData)
    {
        layers.Insert(addIndex, layerData);
        onLayerInserted?.Invoke(addIndex, layerData);
    }

    public static void RemoveLayer(LayerData layerData) => RemoveLayerAt(layers.IndexOf(layerData));
    public static void RemoveLayerAt(int removeIndex)
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

    public static void SwapLayerOrder(int indexA, int indexB)
    {
        LayerData temp = layers[indexA];
        layers[indexA] = layers[indexB];
        layers[indexB] = temp;

        onLayerSwap?.Invoke(indexA, indexB);
    }
    #endregion
}

/// <summary> LayersData specific commands </summary>
#region COMMANDS
public class AddLayerCommand : ICommand
{
    private LayerData layerData;
    public AddLayerCommand(LayerData layerData)
    {
        this.layerData = layerData;
        Execute();
    }

    public void Execute() => LayersData.AddLayer(layerData);
    public void Undo() => LayersData.RemoveLayer(layerData);
}

public class SwapLayerCommand : ICommand
{
    private int indexA;
    private int indexB;
    public SwapLayerCommand(int indexA, int indexB)
    {
        this.indexA = indexA;
        this.indexB = indexB;
        Execute();
    }

    public void Execute() => LayersData.SwapLayerOrder(indexA, indexB);
    public void Undo() => LayersData.SwapLayerOrder(indexB, indexA);
}

public class RemoveLayerCommand : ICommand
{
    private LayerData layerData;
    private int removeIndex;
    public RemoveLayerCommand(LayerData layerData, int removeIndex)
    {
        this.layerData = layerData;
        this.removeIndex = removeIndex;
        Execute();
    }

    public void Execute() => LayersData.RemoveLayerAt(removeIndex);
    public void Undo() => LayersData.InsertLayer(removeIndex, layerData);
}
#endregion

public class LayerData
{
    public string name = $"Layer {LayersData.layers.Count + 1}"; 
    public bool visible = true;

    //TODO: should be the lists of things like CircleData here;

    public List<CircleData> circles = new List<CircleData>();
    public List<LineData> lines = new List<LineData>();
    public List<ArcData> arcs = new List<ArcData>();
    public List<SegmentData> segments = new List<SegmentData>();
    public List<PolyLineData> polyLines = new List<PolyLineData>();
    public List<Vector2> poi = new List<Vector2>();
}