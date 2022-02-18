using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

[RequireComponent(typeof(Camera))]
public class ExportCamera : MonoBehaviour
{
    //transparency calculation: https://gist.github.com/bitbutter/302da1c840b7c93bc789

    private const int PIXELS_PER_UNIT = 100;

    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private Camera cam;

    private PNGExporter pngExporter = new PNGExporter();

    private Rect worldSpaceRect = new Rect(0, 0, 10, 10);

    public int Width
    {
        get => (int)worldSpaceRect.width;
        set
        {
            renderTexture.Release();
            renderTexture.width = value * PIXELS_PER_UNIT;
            renderTexture.Create();

            worldSpaceRect.width = value;
            cam.aspect = (float)value / Height;
        }
    }

    public int Height
    {
        get => renderTexture.height / PIXELS_PER_UNIT;
        set
        {
            renderTexture.Release();
            renderTexture.height = value * PIXELS_PER_UNIT;
            renderTexture.Create();

            worldSpaceRect.height = value;
            cam.orthographicSize = value / 2.0f;
            cam.aspect = Width / (float)value;
        }
    }

    public bool ShowBounds { get; private set; } = false;

    public Vector2[] GetCorners()
    {
        Vector2 extents = worldSpaceRect.size / 2;

        return new Vector2[4] {
            worldSpaceRect.position + extents * new Vector2(-1, -1),
            worldSpaceRect.position + extents * new Vector2(-1,  1),
            worldSpaceRect.position + extents * new Vector2( 1,  1),
            worldSpaceRect.position + extents * new Vector2( 1, -1),
        };
    }           

    private void Awake()
    {
        //create a copy of Render Texture
        renderTexture = new RenderTexture(renderTexture);
        renderTexture.name = "Export Camera Render Texture";

        cam = GetComponent<Camera>();
        cam.targetTexture = renderTexture;

        Width = (int)worldSpaceRect.width;
        Height = (int)worldSpaceRect.width;
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
