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
    public ToggleButton gridToggle;
    public TMP_InputField gridXInput;
    public TMP_InputField gridYInput;
    public ToggleButton poiToggle;

    private void Awake()
    {
        //set default values
        guideColor.image.color = drawStyle.guideColor;
        finalColor.image.color = drawStyle.finalColor;
        poiToggle.SetIsOnWithoutNotify(drawStyle.showPOI);
        gridXInput.SetTextWithoutNotify(gridDisplay.GridX.ToString());
        gridYInput.SetTextWithoutNotify(gridDisplay.GridY.ToString());
        gridToggle.SetIsOnWithoutNotify(gridDisplay.Enabled);
        gridXInput.interactable = gridDisplay.Enabled;
        gridYInput.interactable = gridDisplay.Enabled;

        //setup UI events
        guideColor.onColorUpdated += SetGuideColor;
        finalColor.onColorUpdated += SetFinalColor;
        poiToggle.onValueChanged.AddListener(TogglePOI);
        gridXInput.onEndEdit.AddListener(SetGridX);
        gridYInput.onEndEdit.AddListener(SetGridY);
        gridToggle.onValueChanged.AddListener(ToggleGrid);
    }

    private void SetGuideColor(Color guideColor) => drawStyle.guideColor = guideColor;
    private void SetFinalColor(Color finalColor) => drawStyle.finalColor = finalColor;
    private void TogglePOI(bool enabled) => drawStyle.showPOI = enabled;
    private void ToggleGrid(bool enabled)
    {
        gridDisplay.Enabled = enabled;
        gridXInput.interactable = enabled;
        gridYInput.interactable = enabled;
    }
    private void SetGridX(string xStr)
    {
        if(int.TryParse(xStr, out int x))
        {
            gridDisplay.GridX = Mathf.Clamp(x, 1, 10);
            gridXInput.SetTextWithoutNotify(gridDisplay.GridX.ToString());
        }
    }
    private void SetGridY(string yStr)
    {
        if (int.TryParse(yStr, out int y))
        {
            gridDisplay.GridY = Mathf.Clamp(y, 1, 10);
            gridYInput.SetTextWithoutNotify(gridDisplay.GridY.ToString());
        }
    }
}
