using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Requires: RectTransform + any Selectable
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class DragArea : MonoBehaviour, IPointerEnterHandler
{
    /// <summary> Drag Area that the mouse was LAST over </summary>
    public static DragArea current = null;

    /// <summary> The RectTransform attached to this GameObject </summary>
    public RectTransform rectTransform => transform as RectTransform;

    public void OnPointerEnter(PointerEventData eventData) => current = this;

    private void Awake()
    {
        DragPanel.onDrag += gameObject.SetActive;
        gameObject.SetActive(false);
    }
    private void OnDestroy() 
    { 
        DragPanel.onDrag += gameObject.SetActive; 
    }

}
