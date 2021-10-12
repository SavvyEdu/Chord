using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class LayerUI : MonoBehaviour
{
    //UI references
    public TMP_InputField layerNameInput;
    public Image image;

    //Data
    public LayerData data;

    //UI event methods (called by buttons / toggles)
    public void Select() => LayersData.UpdateSelection(data);
    public void MoveUp() => LayersData.MoveLayerUp(data);
    public void MoveDown() => LayersData.MoveLayerDown(data);
    public void SetVisibility(bool visiblity)
    {
        data.visible = visiblity;
    }
}
