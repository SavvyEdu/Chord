using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ToggleButton : MonoBehaviour
{
    public bool isOn;

    public GameObject graphicOn;
    public GameObject graphicOff;

    public UnityEvent<bool> onValueChanged;

    private void OnValidate()
    {
        if(graphicOff && graphicOn)
            UpdateGraphics();
    }

    public void SetIsOnWithoutNotify(bool isOn)
    {
        this.isOn = isOn;
        UpdateGraphics();
    }

    public void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            isOn = !isOn;
            UpdateGraphics();
            onValueChanged?.Invoke(isOn);
        });

        UpdateGraphics();
    }

    private void UpdateGraphics()
    {
        graphicOn?.SetActive(isOn);
        graphicOff?.SetActive(!isOn);
    }
}
