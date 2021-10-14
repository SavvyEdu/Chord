using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(VerticalLayoutGroup))]
public abstract class ScrollExpand : MonoBehaviour
{
    [Header("Scroll Expander")]
    public GameObject template;
    [Space]
    public RectTransform parent;
    public ScrollRect scrollRect;
    public int maxItems = 0;

    private List<Transform> childObjects = null;
    private bool scrollOpen = false;

    public virtual void Awake()
    {
        template.SetActive(false);
        scrollRect.gameObject.SetActive(false);

        childObjects = new List<Transform>();
    }

    public GameObject InstantiateTemplate()
    {
        GameObject obj = Instantiate(template, scrollOpen ? scrollRect.content : parent);
        obj.SetActive(true);

        childObjects.Add(obj.transform);
        if (!scrollOpen && childObjects.Count >= maxItems)
            OpenScrollRect();

        return obj;
    }

    public void DestroyTemplate(GameObject obj)
    {
        childObjects.Remove(obj.transform);
        if (scrollOpen && childObjects.Count < maxItems)
            CloseScrollRect();

        Destroy(obj);
    }

    private void CloseScrollRect()
    {
        scrollOpen = false;
        scrollRect.gameObject.SetActive(false);
        foreach (Transform childObject in childObjects)
            childObject.SetParent(parent);
    }

    private void OpenScrollRect()
    {
        scrollOpen = true;
        scrollRect.gameObject.SetActive(true);
        foreach (Transform childObject in childObjects)
            childObject.SetParent(scrollRect.content);
    }
}
