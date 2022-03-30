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

    private ImageExporter imageExporter = new PNGExporter();
    private AnimationExporter animationExporter = new GIFExporter();

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

        //StartCoroutine(ExportImage(callback));
        StartCoroutine(ExportAnimation(callback));
    }

    private IEnumerator ExportAnimation(Action<bool> callback)
    {
        cam.enabled = true;

        CommandHistory.UndoAll();
        int frameCount = CommandHistory.RedoCount + 1;
        Debug.Log(frameCount);

        Texture2D[] frames = new Texture2D[frameCount];
        for (int i = 0; i < frameCount; i++)
        {
            yield return new WaitForEndOfFrame(); //wait for camera to render
            
            frames[i] = RenderAsTexture2D();

            CommandHistory.Redo();

            if (Input.GetKeyDown(KeyCode.K))
            {
                break;
            }
        }


        cam.enabled = false;

        //EXPORT
        bool success = animationExporter.SaveData(Application.persistentDataPath, frames);
        callback?.Invoke(success);
    }


    private IEnumerator ExportImage(Action<bool> callback)
    {
        cam.enabled = true;

        yield return new WaitForEndOfFrame(); //wait for camera to render
        Texture2D image = RenderAsTexture2D();

        cam.enabled = false;

        //EXPORT
        bool success = imageExporter.SaveData(Application.persistentDataPath, image);
        callback?.Invoke(success);
    }

    /// <summary>
    /// Convert camera RenderTexture output to Texture2D
    /// </summary>
    /// <returns></returns>
    private Texture2D RenderAsTexture2D()
    {
        Texture2D tex2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        RenderTexture.active = renderTexture;
        tex2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        tex2D.Apply();
        return tex2D;
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
