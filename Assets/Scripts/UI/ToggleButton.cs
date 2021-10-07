using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ToggleButton : MonoBehaviour
{
    public bool isOn;

    public Image targetGraphic;
    public Sprite spriteOn;
    public Sprite spriteOff;

    public UnityEvent<bool> onValueChanged;

    public void Awake()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(() =>
        {
            isOn = !isOn;
            UpdateSprite();
            onValueChanged?.Invoke(isOn);
        });

        UpdateSprite();
    }

    private void UpdateSprite()
    {
        targetGraphic.sprite = isOn ? spriteOn : spriteOff;
    }
}
