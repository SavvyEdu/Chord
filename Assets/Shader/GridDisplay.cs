using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GridDisplay : MonoBehaviour
{
    [SerializeField] private SpriteRenderer grid;
    private Camera cam;

    public bool Enabled
    {
        get => grid.enabled;
        set => grid.enabled = value;
    }

    public float GridX
    {
        get => grid.material.GetFloat("_GridX");
        set => grid.material.SetFloat("_GridX", value);
    }

    public float GridY
    {
        get => grid.material.GetFloat("_GridY");
        set => grid.material.SetFloat("_GridY", value);
    }

    private void Awake()
    {
        cam = GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        //scale to fill camera 

        float height = cam.orthographicSize * 1.5f; //magic scaling number

        grid.transform.localScale = new Vector2(
            height * Screen.width / Screen.height,
            height);

        //adjust pixel density so gid is always visible
        grid.material.SetFloat("_Size", height * 2 / Screen.height);
    }
}
