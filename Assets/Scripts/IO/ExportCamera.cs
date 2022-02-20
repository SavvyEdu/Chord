using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shapes;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class ExportCamera : MonoBehaviour
{
    //transparency calculation: https://gist.github.com/bitbutter/302da1c840b7c93bc789

    private const int PIXELS_PER_UNIT = 100;
    private const int MIN_PIXEL_SIZE = 1; //export must be at least 1x1

    private Rect worldSpaceRect = new Rect(-5, -5, 10, 10);

    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private Camera cam;

    private PNGExporter pngExporter = new PNGExporter();

    public static Action<bool> OnEdit; 

    public Vector2 Center
    {
        get => worldSpaceRect.center;
        set => SetCenter(value);
    }
    public Vector2 Size
    {
        get => worldSpaceRect.size;
        set => SetSize(value);
    }

    public float X { get => Center.x; set => Center = new Vector2(value, Y); }
    public float Y { get => Center.y; set => Center = new Vector2(X, value); }
    public float Width { get => Size.x; set => Size = new Vector2(value, Height); }
    public float Height { get => Size.y; set => Size = new Vector2(Width, value); }
    public bool ShowBounds { get; private set; } = false;
    public bool editingCenter { get; private set; }
    public bool editingSize { get; private set; }

    private void Awake()
    {
        //create a copy of Render Texture
        renderTexture = new RenderTexture(renderTexture);
        renderTexture.name = "Export Camera Render Texture";

        cam = GetComponent<Camera>();
        cam.targetTexture = renderTexture; 
        cam.enabled = false;

        Center = worldSpaceRect.center;
        Size = worldSpaceRect.size;
    }

    private void SetCenter(Vector2 worldCenter)
    {
        //round to pixel size
        float x = (float)Mathf.RoundToInt(worldCenter.x * PIXELS_PER_UNIT) / PIXELS_PER_UNIT;
        float y = (float)Mathf.RoundToInt(worldCenter.y * PIXELS_PER_UNIT) / PIXELS_PER_UNIT;

        worldSpaceRect.center = new Vector2(x, y);
        cam.transform.position = new Vector3(x, y, cam.transform.position.z);
    }

    private void SetSize(Vector2 worldSize)
    {
        //rounded pixel size (also with min size)
        int pixelWidth = Mathf.RoundToInt(worldSize.x * PIXELS_PER_UNIT);
        int pixelHeight = Mathf.RoundToInt(worldSize.y * PIXELS_PER_UNIT);
        pixelWidth = Mathf.Max(pixelWidth, MIN_PIXEL_SIZE);
        pixelHeight = Mathf.Max(pixelHeight, MIN_PIXEL_SIZE);

        renderTexture.Release();
        renderTexture.width = pixelWidth;
        renderTexture.height = pixelHeight;
        renderTexture.Create();

        //pixel size in world space
        float width = (float)pixelWidth / PIXELS_PER_UNIT;
        float height = (float)pixelHeight / PIXELS_PER_UNIT;
        cam.aspect = width / height;
        cam.orthographicSize = height / 2.0f;

        Vector2 center = Center;
        worldSpaceRect.size = new Vector2(width, height);
        worldSpaceRect.center = center; //maintain original center of rect
    }

    public void ToggleBounds(bool enabled)
    {
        ShowBounds = enabled;
    }

    public void Export()
    {
        void callback (bool success)
        {
            if(success)
                OpenFileExplorer(Application.persistentDataPath);
        }

        StartCoroutine(ExportPNG(callback));
    }

    private IEnumerator ExportPNG(Action<bool> callback)
    {
        cam.enabled = true;
        yield return new WaitForEndOfFrame(); //wait for camera to render

        //Convert rt to tex2D
        Texture2D tex2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        tex2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex2D.Apply();

        cam.enabled = false;

        //EXPORT
        bool success = pngExporter.SaveData(Application.persistentDataPath, tex2D);
        callback?.Invoke(success);
    }


    private void OpenFileExplorer(string path)
    {
        //validate file path
        path = path.Replace("/", @"\");

        //open at path
        System.Diagnostics.Process p = new System.Diagnostics.Process();
        p.StartInfo = new System.Diagnostics.ProcessStartInfo("explorer.exe", @path);
        p.Start();
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

    public Vector2[] GetCorners()
    {
        return new Vector2[4] {
            new Vector2(worldSpaceRect.xMin, worldSpaceRect.yMin),
            new Vector2(worldSpaceRect.xMin, worldSpaceRect.yMax),
            new Vector2(worldSpaceRect.xMax, worldSpaceRect.yMax),
            new Vector2(worldSpaceRect.xMax, worldSpaceRect.yMin),
        };
    }

    public void EditCenter()
    {
        editingCenter = true;
        OnEdit?.Invoke(true);
    }
    public void EditSize()
    {
        editingSize = true;
        OnEdit?.Invoke(true);
    }

    private void OnEditCenter()
    {
        SetCenter(ModuleControl.snapPos);
    }

    private void OnEditSize()
    {
        Vector2 centerToSnap = ModuleControl.snapPos - Center;

        float width = Mathf.Abs(centerToSnap.x) * 2;
        float height = Mathf.Abs(centerToSnap.y) * 2;

        SetSize(new Vector2(width, height));
    }

    private void OnEndEdit()
    {
        editingCenter = false;
        editingSize = false;
        OnEdit?.Invoke(false);
    }

    private void Update()
    {
        if (editingCenter) 
            OnEditCenter();

        if (editingSize) 
            OnEditSize();

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
                OnEndEdit();
        }

       
    }
}
