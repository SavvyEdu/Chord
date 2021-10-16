using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class ColorSliderData
{
    public Slider slider;
    public TMP_InputField input;
    public Image image;
}

public class ColorPicker : MonoBehaviour
{
    [SerializeField] private Image preview;

    public ColorSliderData redSlider;
    public ColorSliderData greenSlider;
    public ColorSliderData blueSlider;

    private Color displayColor = Color.white;

    private void Awake()
    {
        preview.color = displayColor;
    }

    private void SetColor(Color color)
    {
        displayColor = color;

        redSlider.slider.SetValueWithoutNotify(color.r);
        greenSlider.slider.SetValueWithoutNotify(color.g);
        blueSlider.slider.SetValueWithoutNotify(color.b);

        redSlider.input.text = Mathf.FloorToInt(displayColor.r * 255).ToString();
        greenSlider.input.text = Mathf.FloorToInt(displayColor.g * 255).ToString();
        blueSlider.input.text = Mathf.FloorToInt(displayColor.b * 255).ToString();

        redSlider.image.materialForRendering.SetColor("_Color0", new Color(0, displayColor.g, displayColor.b));
        redSlider.image.materialForRendering.SetColor("_Color1", new Color(1, displayColor.g, displayColor.b));

        greenSlider.image.materialForRendering.SetColor("_Color0", new Color(displayColor.r, 0, displayColor.b));
        greenSlider.image.materialForRendering.SetColor("_Color1", new Color(displayColor.r, 1, displayColor.b));

        blueSlider.image.materialForRendering.SetColor("_Color0", new Color(displayColor.r, displayColor.g, 0));
        blueSlider.image.materialForRendering.SetColor("_Color1", new Color(displayColor.r, displayColor.g, 1));

        preview.color = displayColor;
    }


    public void OnSliderUpdated()
    {
        displayColor.r = redSlider.slider.value;
        displayColor.g = greenSlider.slider.value;
        displayColor.b = blueSlider.slider.value;

        SetColor(displayColor);
    }


}
