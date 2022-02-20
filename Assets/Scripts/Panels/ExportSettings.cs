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

    [SerializeField] private ExportCamera exportCamera;
    private CameraPreview CamPreview => exportCamera.cameraPreview;

    private void Start()
    {
        //set default values
        centerXInput.SetTextWithoutNotify(CamPreview.X.ToString());
        centerYInput.SetTextWithoutNotify(CamPreview.Y.ToString());
        sizeXInput.SetTextWithoutNotify(CamPreview.Width.ToString());
        sizeYInput.SetTextWithoutNotify(CamPreview.Height.ToString());

        //setup UI events
        centerXInput.onEndEdit.AddListener(SetX);
        centerYInput.onEndEdit.AddListener(SetY);
        centerEditButton.onClick.AddListener(EditCenter);

        sizeXInput.onEndEdit.AddListener(SetWidth);
        sizeYInput.onEndEdit.AddListener(SetHeight);
        sizeEditButton.onClick.AddListener(EditSize);

        rectVisibleButton.onValueChanged.AddListener(CamPreview.ToggleBounds);

        OnEdit(false);
        CameraPreview.OnEdit += OnEdit;
    }

    private string Format(float input) => string.Format("{0:0.##}", input);

    private void SetX(string xStr)
    {
        if (int.TryParse(xStr, out int x))
        {
            CamPreview.X = x;
            centerXInput.SetTextWithoutNotify(Format(CamPreview.X));
        }
    }
    private void SetY(string yStr)
    {
        if (int.TryParse(yStr, out int y))
        {
            CamPreview.Y = y;
            centerYInput.SetTextWithoutNotify(Format(CamPreview.Y));
        }
    }

    private void SetWidth(string widthStr)
    {
        if (int.TryParse(widthStr, out int width))
        {
            CamPreview.Width = width;
            sizeXInput.SetTextWithoutNotify(Format(CamPreview.Width));
        }
    }

    private void SetHeight(string heightStr)
    {
        if (int.TryParse(heightStr, out int height))
        {
            CamPreview.Height = height;
            sizeYInput.SetTextWithoutNotify(Format(CamPreview.Height));
        }
    }

    private void EditCenter()
    {
        rectVisibleButton.IsOn = true;
        CamPreview.EditCenter();
    }

    private void EditSize()
    {
        rectVisibleButton.IsOn = true;
        CamPreview.EditSize();
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
            centerXInput.SetTextWithoutNotify(Format(CamPreview.X));
            centerYInput.SetTextWithoutNotify(Format(CamPreview.Y));
            sizeXInput.SetTextWithoutNotify(Format(CamPreview.Width));
            sizeYInput.SetTextWithoutNotify(Format(CamPreview.Height));
        }
    }
}
