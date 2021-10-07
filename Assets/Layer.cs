using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;
public class Layer : MonoBehaviour
{
    //UI events
    public UnityAction<Layer> onSelect;
    public UnityAction<Layer> onMoveUp;
    public UnityAction<Layer> onMoveDown;
    public UnityAction<bool> onSetVisible;

    //UI references
    public TMP_InputField layerNameInput;
    public Image image;

    public LayerModules modules = new LayerModules();
    public bool IsVisible { get; private set; } = true;

    //UI event methods (called by buttons / toggles)
    public void Select()
    {
        onSelect?.Invoke(this);
    }
    public void MoveUp()
    {
        onMoveUp?.Invoke(this);
    }
    public void MoveDown()
    {
        onMoveDown?.Invoke(this);
    }
    public void SetVisibility(bool visiblity)
    {
        this.IsVisible = visiblity;

        onSetVisible?.Invoke(visiblity);
    }
}
