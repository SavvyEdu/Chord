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

    //called by button
    public void BeginEdit()
    {
        ColorPicker.onColorUpdated += UpdateColor;
        ColorPicker.beginEdit?.Invoke(image.color);
    }

    private void UpdateColor(Color color)
    {
        image.color = color;
        onColorUpdated?.Invoke(color);
    }
}
