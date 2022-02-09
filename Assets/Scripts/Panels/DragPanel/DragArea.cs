using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Requires: RectTransform, some Graphic with a Raycast Target
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class DragArea : MonoBehaviour, IPointerEnterHandler
{
    /// <summary> Drag Area that the mouse was LAST over </summary>
    public static DragArea current = null;
    /// <summary> The RectTransform attached to this GameObject </summary>
    public RectTransform rectTransform => transform as RectTransform;  

    public void OnPointerEnter(PointerEventData eventData)
    { 
        current = this;
    }
}
