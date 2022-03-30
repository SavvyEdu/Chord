using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class LayerController : ScrollExpand
{
    [Header("Layer Colors")]
    public Color normalColor;
    public Color selectedColor;

    public static List<LayerUI> layersUI;
    public static LayerUI selectedLayer;

    public override void Awake() 
    {
        base.Awake();

        layersUI = new List<LayerUI>();

        LayersData.onLayerAdded += AddLayerUI;
        LayersData.onLayerInserted += InsertLayerUI;
        LayersData.onLayerSelected += SelectLayerUI;
        LayersData.onLayerSwap += SwapLayerUI;
        LayersData.onLayerRemoved += RemoveLayerUI;
    }

    private void Start()
    {
        LayersData.AddLayer(new LayerData()); //create the default layer
    }

    #region UI calls

    public void AddLayer() => LayersData.AddLayer();
    public void RemoveSelectedLayer() => LayersData.RemoveSelectedLayer();

    #endregion

    private LayerUI CreateLayer(LayerData data)
    {
        GameObject obj = InstantiateTemplate();
        obj.name = data.name;

        if (obj.TryGetComponent(out LayerUI layer))
        {
            layer.layerNameInput.text = data.name;
            layer.data = data;
        }
        return layer;
    }

    public void AddLayerUI(LayerData data)
    {
        LayerUI layerUI = CreateLayer(data);
        layersUI.Add(layerUI);
    }

    public void InsertLayerUI(int addIndex, LayerData data)
    {
        LayerUI layerUI = CreateLayer(data);
        layersUI.Insert(addIndex, layerUI);
        layerUI.transform.SetSiblingIndex(addIndex+1);
    }

    public void RemoveLayerUI(int index)
    {
        DestroyTemplate(layersUI[index].gameObject);
        layersUI.RemoveAt(index);
    }
   
    private void SelectLayerUI(int index) {

        if(selectedLayer != null)
            selectedLayer.image.color = normalColor;

        selectedLayer = layersUI[index];
        selectedLayer.image.color = selectedColor;
    }

    private void SwapLayerUI(int indexA, int indexB)
    {
        LayerUI temp = layersUI[indexA];
        layersUI[indexA] = layersUI[indexB];
        layersUI[indexB] = temp;

        foreach (LayerUI layer in layersUI)
        {
            layer.transform.SetAsLastSibling();
        }
    }
}