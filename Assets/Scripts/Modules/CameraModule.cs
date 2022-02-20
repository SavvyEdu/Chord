using System;
using System.Collections.Generic;
using UnityEngine;
using Shapes;

public class CameraPreview
{
    private RenderTexture renderTexture = null;
    private Camera cam = null;

    private const int PIXELS_PER_UNIT = 100;
    private const int MIN_PIXEL_SIZE = 1; //export must be at least 1x1

    private Rect worldRect = new Rect(-5, -5, 10, 10);

    public static Action<bool> OnEdit;

    public CameraPreview(Vector2 center, Vector2 size, RenderTexture refTexure, Camera refCamera)
    {
        renderTexture = refTexure;
        cam = refCamera;

        SetCenter(center);
        SetSize(size);
    }

    public Vector2 Center
    {
        get => worldRect.center;
        set => SetCenter(value);
    }
    public Vector2 Size
    {
        get => worldRect.size;
        set => SetSize(value);
    }

    public float X { get => Center.x; set => Center = new Vector2(value, Y); }
    public float Y { get => Center.y; set => Center = new Vector2(X, value); }
    public float Width { get => Size.x; set => Size = new Vector2(value, Height); }
    public float Height { get => Size.y; set => Size = new Vector2(Width, value); }
    public bool ShowBounds { get; private set; } = false;
    public bool editingCenter { get; private set; }
    public bool editingSize { get; private set; }

    public void ToggleBounds(bool enabled)
    {
        ShowBounds = enabled;
    }

    private void SetCenter(Vector2 worldCenter)
    {
        //round to pixel size
        float x = (float)Mathf.RoundToInt(worldCenter.x * PIXELS_PER_UNIT) / PIXELS_PER_UNIT;
        float y = (float)Mathf.RoundToInt(worldCenter.y * PIXELS_PER_UNIT) / PIXELS_PER_UNIT;

        worldRect.center = new Vector2(x, y);
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
        worldRect.size = new Vector2(width, height);
        worldRect.center = center; //maintain original center of rect
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
            new Vector2(worldRect.xMin, worldRect.yMin),
            new Vector2(worldRect.xMin, worldRect.yMax),
            new Vector2(worldRect.xMax, worldRect.yMax),
            new Vector2(worldRect.xMax, worldRect.yMin),
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

    public void OnEditCenter()
    {
        SetCenter(ModuleControl.snapPos);
    }

    public void OnEditSize()
    {
        Vector2 centerToSnap = ModuleControl.snapPos - Center;

        float width = Mathf.Abs(centerToSnap.x) * 2;
        float height = Mathf.Abs(centerToSnap.y) * 2;

        SetSize(new Vector2(width, height));
    }

    public void OnEndEdit()
    {
        editingCenter = false;
        editingSize = false;
        OnEdit?.Invoke(false);
    }
}
