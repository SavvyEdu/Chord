using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ColorPicker : MonoBehaviour
{
    public Image preview;

    public ColorSlider inputRed;
    public ColorSlider inputGreen;
    public ColorSlider inputBlue;

    private Color displayColor = Color.white;

    private void Awake()
    {
        preview.color = displayColor;

        inputRed.onValueUpdated += SetRed;
        inputGreen.onValueUpdated += SetGreen;
        inputBlue.onValueUpdated += SetBlue;

        SetColor(displayColor);
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
