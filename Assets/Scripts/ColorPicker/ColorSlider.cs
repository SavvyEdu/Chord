using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class ColorSlider : MonoBehaviour
{
    public Slider slider;
    public TMP_InputField input;
    public Image image;

    public UnityAction<float> onValueUpdated;

    private void Awake()
    {
        image.material = new Material(Shader.Find("Unlit/Gradient"));

        slider.onValueChanged.AddListener(SetValue);
        input.onValueChanged.AddListener(SetValueFromText);
    }

    public void SetValueFromText(string valueText)
    {
        if (int.TryParse(valueText, out int value255))
            SetValue(value255 / 255.0f);
    }

    public void SetValue(float value)
    {
        SetValueWithoutNotify(value);
        onValueUpdated?.Invoke(value);
    }

    public void SetValueWithoutNotify(float value)
    {
        slider.SetValueWithoutNotify(value);
        input.SetTextWithoutNotify(Mathf.FloorToInt(value * 255).ToString());
    }

    public void SetGradient(Color color0, Color color1)
    {
        image.materialForRendering.SetColor("_Color0", color0);
        image.materialForRendering.SetColor("_Color1", color1);
    }
}
