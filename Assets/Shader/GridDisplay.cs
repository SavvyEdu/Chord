using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GridDisplay : MonoBehaviour
{
    [SerializeField] private SpriteRenderer grid;
    private Camera cam;

    public static bool IsVisible = false;
    public static Vector2 size = Vector2.one;

    public bool Enabled
    {
        get => grid.enabled;
        set => grid.enabled = IsVisible = value;
    }

    public float GridX
    {
        get => grid.material.GetFloat("_GridX");
        set => grid.material.SetFloat("_GridX", size.x = value);
    }

    public float GridY
    {
        get => grid.material.GetFloat("_GridY");
        set => grid.material.SetFloat("_GridY", size.y = value);
    }

    private void Awake()
    {
        cam = GetComponent<Camera>();

        IsVisible = Enabled;
        size = new Vector2(GridX, GridY);
    }

    private void LateUpdate()
    { 
        float height = cam.orthographicSize * 1.5f; //magic scaling number

        grid.transform.localScale = new Vector2(
            height * Screen.width / Screen.height,
            height);

        //adjust pixel density so gid is always visible
        grid.material.SetFloat("_Size", height * 4 / Screen.height);
    }
}
