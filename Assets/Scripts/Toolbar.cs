using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public enum DrawMode { Guides, Final }
public class Toolbar : MonoBehaviour
{
    public TextMeshProUGUI drawModeText;
    public ToggleButton drawToggle;
    public ButtonGroup groupGuides;
    public ButtonGroup groupFinal;

    private DrawMode drawMode;

    private void Awake()
    {
        ToggleDrawMode(false);
    }

    public void ToggleDrawMode(bool final)
    {
        if (final) drawMode = DrawMode.Final;
        else drawMode = DrawMode.Guides;

        drawToggle.SetIsOnWithoutNotify(final); //for non UI-calls
        drawModeText.text = $"{drawMode}";

        groupGuides.gameObject.SetActive(drawMode == DrawMode.Guides);
        groupFinal.gameObject.SetActive(drawMode == DrawMode.Final);
    }
}
