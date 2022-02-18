using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ExportSettings : MonoBehaviour
{
    public TMP_InputField centerXInput;
    public TMP_InputField centerYInput;

    public ExportCamera exportCamera;

    private void Start()
    {
        //set default values
        centerXInput.SetTextWithoutNotify(exportCamera.Width.ToString());
        centerYInput.SetTextWithoutNotify(exportCamera.Height.ToString());

        //setup UI events
        centerXInput.onEndEdit.AddListener(SetGridX);
        centerYInput.onEndEdit.AddListener(SetGridY);
    }

    private void SetGridX(string xStr)
    {
        if (int.TryParse(xStr, out int x))
        {
            exportCamera.Width = x;
            centerXInput.SetTextWithoutNotify(x.ToString());
        }
    }
    private void SetGridY(string yStr)
    {
        if (int.TryParse(yStr, out int y))
        {
            exportCamera.Height = y;
            centerYInput.SetTextWithoutNotify(y.ToString());
        }
    }
}
