using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorPicker : MonoBehaviour
{
    [SerializeField] private Image preview;

    private Color displayColor = Color.black;

    private Slider sliderR;
    private Slider sliderG;
    private Slider sliderB;

    private InputField numR;
    private InputField numG;
    private InputField numB;

    private Image bgR;
    private Image bgG;
    private Image bgB;

    private Image targetImage;

    private void Awake()
    {
        Slider[] sliders = GetComponentsInChildren<Slider>();
        InputField[] nums = GetComponentsInChildren<InputField>();
        if (sliders.Length >= 3 && nums.Length >= 3)
        {
            sliderR = sliders[0];
            numR = nums[0];
            bgR = sliderR.GetComponentsInChildren<Image>()[2];

            sliderG = sliders[1];
            numG = nums[1];
            bgG = sliderG.GetComponentsInChildren<Image>()[2];

            sliderB = sliders[2];
            numB = nums[2];
            bgB = sliderB.GetComponentsInChildren<Image>()[2];
        }

        preview.color = displayColor;
    }

    public void SetTargetImage(Image target)
    {
        targetImage = target;
        SetColor(targetImage.color);
    }

    private void SetColor(Color color)
    {
        displayColor = color;

        sliderR.value = color.r;
        sliderG.value = color.g;
        sliderB.value = color.b;

        numR.text = Mathf.FloorToInt(displayColor.r * 255).ToString();
        numG.text = Mathf.FloorToInt(displayColor.g * 255).ToString();
        numB.text = Mathf.FloorToInt(displayColor.b * 255).ToString();

        //link target image color 
        if (targetImage != null)
        {
            targetImage.color = displayColor;
        }

        bgR.materialForRendering.SetColor("_Color0", new Color(0, displayColor.g, displayColor.b));
        bgR.materialForRendering.SetColor("_Color1", new Color(1, displayColor.g, displayColor.b));

        bgG.materialForRendering.SetColor("_Color0", new Color(displayColor.r, 0, displayColor.b));
        bgG.materialForRendering.SetColor("_Color1", new Color(displayColor.r, 1, displayColor.b));

        bgB.materialForRendering.SetColor("_Color0", new Color(displayColor.r, displayColor.g, 0));
        bgB.materialForRendering.SetColor("_Color1", new Color(displayColor.r, displayColor.g, 1));

        preview.color = displayColor;
    }

    private void Update()
    {
        displayColor.r = sliderR.value;
        displayColor.g = sliderG.value;
        displayColor.b = sliderB.value;

        SetColor(displayColor);
    }


}
