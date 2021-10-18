using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TMPro;

public class ToolSettings : MonoBehaviour
{
    public static UnityAction<IModule> onToolSelected;

    [Header("DrawStylerSettings")]
    public DrawStyler drawStyle;
    [Space]
    public ColorButton guideColor;
    public ColorButton finalColor;

    private void Awake()
    {
        onToolSelected += ShowToolSettings;

        guideColor.image.color = drawStyle.guideColor;
        guideColor.onColorUpdated += (Color guideColor) => drawStyle.guideColor = guideColor;

        finalColor.image.color = drawStyle.finalColor;
        finalColor.onColorUpdated += (Color finalColor) => drawStyle.finalColor = finalColor;
    }

    public void ShowToolSettings(IModule module)
    {
        /* Reflection Experiment
        FieldInfo[] fieldInfo = settings.GetType().GetFields();
        foreach (var field in fieldInfo)
        {
            if (field.FieldType.Equals(typeof(Color)))
            {
            }
        }*/
    }
}
