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

        ExportCamera.OnEdit += OnEdit;
    }

    private string Format(float input) => string.Format("{0:0.##}", input);

    private void SetX(string xStr)
    {
        if (int.TryParse(xStr, out int x))
        {
            exportCamera.X = x;
            centerXInput.SetTextWithoutNotify(Format(exportCamera.X));
        }
    }
    private void SetY(string yStr)
    {
        if (int.TryParse(yStr, out int y))
        {
            exportCamera.Y = y;
            centerYInput.SetTextWithoutNotify(Format(exportCamera.Y));
        }
    }

    private void SetWidth(string widthStr)
    {
        if (int.TryParse(widthStr, out int width))
        {
            exportCamera.Width = width;
            sizeXInput.SetTextWithoutNotify(Format(exportCamera.Width));
        }
    }

    private void SetHeight(string heightStr)
    {
        if (int.TryParse(heightStr, out int height))
        {
            exportCamera.Height = height;
            sizeYInput.SetTextWithoutNotify(Format(exportCamera.Height));
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

    private void OnEdit(bool editing)
    {
        //don't allow input while Export camera is being edited
        centerXInput.interactable = !editing;
        centerYInput.interactable = !editing;
        centerEditButton.interactable = !editing;
        sizeXInput.interactable = !editing;
        sizeYInput.interactable = !editing;
        sizeEditButton.interactable = !editing;

        //when edit ends, update the input fields
        if (!editing)
        {
            centerXInput.SetTextWithoutNotify(Format(exportCamera.X));
            centerYInput.SetTextWithoutNotify(Format(exportCamera.Y));
            sizeXInput.SetTextWithoutNotify(Format(exportCamera.Width));
            sizeYInput.SetTextWithoutNotify(Format(exportCamera.Height));
        }
    }
}
