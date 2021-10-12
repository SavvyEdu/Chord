using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class LayerController : MonoBehaviour
{
    public GameObject layerTemplate;
    public Color normalColor;
    public Color selectedColor;

    public static List<LayerUI> layersUI;
    public static LayerUI selectedLayer;

    private void Awake()
    {
        layerTemplate.SetActive(false);
        layersUI = new List<LayerUI>();

        LayersData.onLayerAdded += AddLayerUI;
        LayersData.onLayerSelected += SelectLayerUI;
        LayersData.onLayerSwap += SwapLayerUI;
        LayersData.onLayerRemoved += RemoveLayerUI;

        //once the events have been hooked up, add the initial layer
        AddLayer();
    }

    #region UI calls

    public void AddLayer() => LayersData.AddLayer();
    public void RemoveSelectedLayer() => LayersData.RemoveSelectedLayer();

    #endregion

    public void AddLayerUI(LayerData data)
    {
        GameObject obj = Instantiate(layerTemplate, transform);
        obj.SetActive(true);
        obj.name = $"Layer {layersUI.Count + 1}";

        if (obj.TryGetComponent(out LayerUI layer))
        {
            layer.layerNameInput.text = $"Layer {layersUI.Count + 1}";
            layer.data = data;

            layersUI.Add(layer);
        }
    }

    public void RemoveLayerUI(int index)
    {
        Destroy(layersUI[index].gameObject);
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
            Debug.Log(layer.gameObject.name);
            layer.transform.SetAsLastSibling();
        }
    }
}