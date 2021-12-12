using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

[RequireComponent(typeof(Camera))]
public class ExportCamera : MonoBehaviour
{
    //transparency calculation: https://gist.github.com/bitbutter/302da1c840b7c93bc789

    [SerializeField] private RenderTexture renderTexture;

    private Camera cam;
    private PNGExporter pngExporter = new PNGExporter();

    public bool ShowBounds { get; private set; } = false;

    public Vector2[] GetCorners()
    {
        return new Vector2[4] {
            cam.ViewportToWorldPoint(Vector2.zero),
            cam.ViewportToWorldPoint(Vector2.up),
            cam.ViewportToWorldPoint(Vector2.one),
            cam.ViewportToWorldPoint(Vector2.right),
        };
    }           

    private void Awake()
    {
        cam = GetComponent<Camera>();
        cam.targetTexture = renderTexture;
    }

    public void ToggleBounds(bool enabled)
    {
        ShowBounds = enabled;
    }

    public void Export()
    {
        //Convert rt to tex2D
        Texture2D tex2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        tex2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex2D.Apply();

        //EXPORT
        pngExporter.SaveData(Application.persistentDataPath, tex2D);
    }

    public void DrawOutline() 
    {
        if (ShowBounds)
        {
            Vector2[] corners = GetCorners();
            Draw.Line(corners[0], corners[1]);
            Draw.Line(corners[1], corners[2]);
            Draw.Line(corners[2], corners[3]);
            Draw.Line(corners[3], corners[0]);
        }  
    }
}
