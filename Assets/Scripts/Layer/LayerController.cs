using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

public class LayerController : MonoBehaviour
{
    public GameObject layerTemplate;
    public Color normalColor;
    public Color selectedColor;

    public static List<Layer> layers;
    public static Layer selectedLayer;

    public static UnityAction onLayerUpdated;

    public static UnityAction<int> removeLayer;

    public static int SelectedIndex { get; private set; }

    private void Awake()
    {
        removeLayer += RemoveLayer;


        layerTemplate.SetActive(false);

        layers = new List<Layer>();
        AddLayer();
    }

    public void AddLayer()
    {
        GameObject obj = Instantiate(layerTemplate, transform);
        obj.SetActive(true);
        obj.name = $"Layer {layers.Count + 1}";

        if (obj.TryGetComponent(out Layer layer))
        {
            layer.layerNameInput.text = $"Layer {layers.Count + 1}";
            layer.onSelect += UpdateSelection;
            layer.onMoveUp += MoveLayerUp;
            layer.onMoveDown += MoveLayerDown;

            layers.Add(layer);
            UpdateSelection(layer);
            DrawStack.Add(layers.Count - 1, true);
        }

        
    }

    public void RemoveSelectedLayer()
    {
        int index = layers.IndexOf(selectedLayer);
        Destroy(selectedLayer.gameObject);
        layers.RemoveAt(index);

        if(layers.Count == 0)
            AddLayer();

        if (index == layers.Count)
            index--;

        UpdateSelection(layers[index]);
    }

    public void RemoveLayer(int removeIndex)
    {
        Destroy(layers[removeIndex].gameObject);
        layers.RemoveAt(removeIndex);

        if (layers.Count == 0)
            AddLayer();

        if (removeIndex < SelectedIndex || SelectedIndex == layers.Count)
            UpdateSelection(layers[SelectedIndex - 1]);
        else
            UpdateSelection(layers[SelectedIndex]);
    }

    private void UpdateSelection(Layer layer) {

        if(selectedLayer != null)
            selectedLayer.image.color = normalColor;

        selectedLayer = layer;
        selectedLayer.image.color = selectedColor;

        SelectedIndex = layers.IndexOf(layer);

        onLayerUpdated?.Invoke();
    }

    private void MoveLayerUp(Layer layer)
    {
        int index = layers.IndexOf(layer);
        if(index != 0)
            SwapLayerOrder(index, index - 1);
    }

    private void MoveLayerDown(Layer layer)
    {
        int index = layers.IndexOf(layer);
        if (index != layers.Count -1)
            SwapLayerOrder(index, index + 1);
    }

    private void SwapLayerOrder(int indexA, int indexB)
    {
        Layer[] layersArr = layers.ToArray();

        Layer temp = layersArr[indexA];
        layersArr[indexA] = layersArr[indexB];
        layersArr[indexB] = temp;

        layers.Clear();
        layers.AddRange(layersArr); 
        UpdateTransformOrder();
    }

    private void UpdateTransformOrder()
    {
        foreach (Layer layer in layers)
        {
            layer.transform.SetAsLastSibling();
        }
    }
}