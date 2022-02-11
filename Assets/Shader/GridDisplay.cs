using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class GridDisplay : MonoBehaviour
{
    [SerializeField] private SpriteRenderer grid;
    private Camera cam;

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
