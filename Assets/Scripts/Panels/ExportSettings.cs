using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExportSettings : MonoBehaviour
{
    public TMP_InputField centerXInput;
    public TMP_InputField centerYInput;
    public Button centerEditButton;

    public TMP_InputField sizeXInput;
    public TMP_InputField sizeYInput;
    public Button sizeEditButton;

    public ToggleButton rectVisibleButton;
    public Button exportButton;

    public ExportCamera exportCamera;

    private void Start()
    {
        //set default values
        centerXInput.SetTextWithoutNotify(exportCamera.X.ToString());
        centerYInput.SetTextWithoutNotify(exportCamera.Y.ToString());
        sizeXInput.SetTextWithoutNotify(exportCamera.Width.ToString());
        sizeYInput.SetTextWithoutNotify(exportCamera.Height.ToString());

        //setup UI events
        centerXInput.onEndEdit.AddListener(SetX);
        centerYInput.onEndEdit.AddListener(SetY);
        centerEditButton.onClick.AddListener(EditCenter);

        sizeXInput.onEndEdit.AddListener(SetWidth);
        sizeYInput.onEndEdit.AddListener(SetHeight);
        sizeEditButton.onClick.AddListener(EditSize);

    }

    private void SetX(string xStr)
    {
        if (int.TryParse(xStr, out int x))
        {
            exportCamera.X = x;
            centerXInput.SetTextWithoutNotify(x.ToString());
        }
    }
    private void SetY(string yStr)
    {
        if (int.TryParse(yStr, out int y))
        {
            exportCamera.Y = y;
            centerYInput.SetTextWithoutNotify(y.ToString());
        }
    }

    private void SetWidth(string widthStr)
    {
        if (int.TryParse(widthStr, out int width))
        {
            exportCamera.Width = width;
            sizeXInput.SetTextWithoutNotify(width.ToString());
        }
    }
    private void SetHeight(string heightStr)
    {
        if (int.TryParse(heightStr, out int height))
        {
            exportCamera.Height = height;
            sizeYInput.SetTextWithoutNotify(height.ToString());
        }
    }

    private void EditCenter()
    {
        rectVisibleButton.IsOn = true;
        exportCamera.EditCenter();
    }

    private void EditSize()
    {
        rectVisibleButton.IsOn = true;
        exportCamera.EditSize();
    }
}
