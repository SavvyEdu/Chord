using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ColorButton : Button
{
    public UnityAction<Color> onColorUpdated;

    protected override void Awake()
    {
        base.Awake();
        onClick.AddListener(BeginEdit);
    }

    public void BeginEdit()
    {
        ColorPicker.beginEdit?.Invoke(image.color, UpdateColor);
    }

    private void UpdateColor(Color color)
    {
        image.color = color;
        onColorUpdated?.Invoke(color);
    }
}
