using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using System;

public class ColorPicker : MonoBehaviour
{
    public Image preview;

    public ColorSlider inputRed;
    public ColorSlider inputGreen;
    public ColorSlider inputBlue;

    private Color displayColor = Color.white;

    /// <summary>
    /// begin edit with default color and onColorUpdated method
    /// </summary>
    public static UnityAction<Color, UnityAction<Color>> beginEdit;
    public static UnityAction<Color> onColorUpdated;

    private void Awake()
    {
        preview.color = displayColor;

        inputRed.onValueUpdated += SetRed;
        inputGreen.onValueUpdated += SetGreen;
        inputBlue.onValueUpdated += SetBlue;

        SetColor(displayColor);

        beginEdit += BeginEdit;
        gameObject.SetActive(false);
    }

    void BeginEdit(Color color, UnityAction<Color> listener)
    {
        onColorUpdated = listener;
        SetColor(color);
        gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        onColorUpdated = null;
    }

    private void SetRed(float r)
    {
        displayColor.r = r;
        SetColor(displayColor);
    }

    private void SetGreen(float g)
    {
        displayColor.g = g;
        SetColor(displayColor);
    }

    private void SetBlue(float b)
    {
        displayColor.b = b;
        SetColor(displayColor);
    }

    private void SetColor(Color color)
    {
        preview.color = displayColor = color;
        onColorUpdated?.Invoke(displayColor);

        inputRed.SetValueWithoutNotify(displayColor.r);
        inputRed.SetGradient(new Color(0, displayColor.g, displayColor.b),
                             new Color(1, displayColor.g, displayColor.b));

        inputGreen.SetValueWithoutNotify(displayColor.g);
        inputGreen.SetGradient(new Color(displayColor.r, 0, displayColor.b), 
                               new Color(displayColor.r, 1, displayColor.b));

        inputBlue.SetValueWithoutNotify(displayColor.b);
        inputBlue.SetGradient(new Color(displayColor.r, displayColor.g, 0), 
                              new Color(displayColor.r, displayColor.g, 1));
    }
}
