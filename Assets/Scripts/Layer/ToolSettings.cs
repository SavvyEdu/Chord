using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public enum DrawMode { Guide, Final }

public class ToolSettings : MonoBehaviour
{
    public TextMeshProUGUI drawModeText;
    public ToggleButton drawToggle;
    private DrawMode drawMode;
    public DrawMode DrawMode {
        get => drawMode;
        private set
        {
            drawMode = value;
            drawToggle.SetIsOnWithoutNotify(drawMode == DrawMode.Final);
            drawModeText.text = $"{drawMode} Mode";
        }
    }

    public static UnityAction<IModule> onToolSelected;

    private void Awake()
    {
        onToolSelected += ShowToolSettings;
    }

    public void ShowToolSettings(IModule module)
    {
        DrawMode = module.drawMode;
    }

    public void ToggleDrawMode(bool final)
    {
        if (final) DrawMode = DrawMode.Final;
        else       DrawMode = DrawMode.Guide;
    }
}
