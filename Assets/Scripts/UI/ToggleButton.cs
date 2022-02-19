using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ToggleButton : MonoBehaviour
{
    private bool isOn;
    public bool IsOn
    {
        get => isOn;
        set
        {
            isOn = value;
            onValueChanged?.Invoke(isOn);
            UpdateGraphics();
        }
    }

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
        button.onClick.AddListener(() => IsOn = !IsOn);

        UpdateGraphics();
    }

    private void UpdateGraphics()
    {
        graphicOn?.SetActive(IsOn);
        graphicOff?.SetActive(!IsOn);
    }
}
