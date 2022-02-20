using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Camera))]
public class ExportCamera : MonoBehaviour
{
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private Camera cam;

    private PNGExporter pngExporter = new PNGExporter();

    public CameraPreview cameraPreview = null;

    private void Awake()
    {
        //create a copy of Render Texture
        renderTexture = new RenderTexture(renderTexture);
        renderTexture.name = "Export Camera Render Texture";

        cam = GetComponent<Camera>();
        cam.targetTexture = renderTexture; 
        cam.enabled = false;

        cameraPreview = new CameraPreview(Vector2.zero, Vector2.one * 10, renderTexture, cam);
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

    
    private void Update()
    {
        if (cameraPreview.editingCenter)
            cameraPreview.OnEditCenter();

        if (cameraPreview.editingSize)
            cameraPreview.OnEditSize();

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Input.GetMouseButtonDown(0))
                cameraPreview.OnEndEdit();
        }      
    }
}
