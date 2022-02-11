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
    public GridDisplay gridDisplay;
    [Space]
    public ColorButton guideColor;
    public ColorButton finalColor;
    public ToggleButton poiToggle;
    public ToggleButton gridToggle;

    private void Awake()
    {
        onToolSelected += ShowToolSettings;

        guideColor.image.color = drawStyle.guideColor;
        guideColor.onColorUpdated += SetGuideColor;

        finalColor.image.color = drawStyle.finalColor;
        finalColor.onColorUpdated += SetFinalColor;

        poiToggle.SetIsOnWithoutNotify(drawStyle.showPOI);
        poiToggle.onValueChanged.AddListener(TogglePOI);

        gridToggle.SetIsOnWithoutNotify(gridDisplay.Enabled);
        gridToggle.onValueChanged.AddListener(ToggleGrid);
    }

    private void SetGuideColor(Color guideColor) => drawStyle.guideColor = guideColor;
    private void SetFinalColor(Color finalColor) => drawStyle.finalColor = finalColor;
    private void TogglePOI(bool enabled) => drawStyle.showPOI = enabled;
    private void ToggleGrid(bool enabled) => gridDisplay.Enabled = enabled;

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
