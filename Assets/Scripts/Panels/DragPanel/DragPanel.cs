using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class DragPanel : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    /// <summary> The RectTransform attached to this GameObject </summary>
    public RectTransform rectTransform => transform as RectTransform;

    /// <summary> offset from mouse to pivot </summary>
    private Vector2 clickOffset = Vector2.zero;

    /// <summary> the panel is selected </summary>
    private bool dragging = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
        clickOffset = rectTransform.position - Input.mousePosition; //get the initial click offset
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;
    }

    void Update()
    {
        if (dragging)
        {
            rectTransform.position = (Vector2)Input.mousePosition + clickOffset;
            if (DragArea.current)
            {
                ClampToArea(DragArea.current.rectTransform);
            }
            else
            {
                ClampToArea(transform.parent as RectTransform);
            }
        }
    }

    /// <summary>
    /// limit this rectTransform to stay within the draggable area
    /// </summary>
    private void ClampToArea(RectTransform draggableArea)
    {
        Vector2 pos = rectTransform.position;
        Vector2 min = (Vector2)draggableArea.position + draggableArea.rect.min - rectTransform.rect.min;
        Vector2 max = (Vector2)draggableArea.position + draggableArea.rect.max - rectTransform.rect.max;

        pos.x = Mathf.Clamp(pos.x, min.x, max.x);
        pos.y = Mathf.Clamp(pos.y, min.y, max.y);
        rectTransform.position = pos;
    }

    private void OnNextFrame(System.Action action)
    {
        StartCoroutine(WaitFrame());
        IEnumerator WaitFrame()
        {
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            action.Invoke();
        }
    }
}
