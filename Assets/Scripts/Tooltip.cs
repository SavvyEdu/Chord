using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Tooltip : MonoBehaviour
{
    public GameObject displayObject;
    public TextMeshProUGUI text;

    public static UnityAction<string> setMessage;

    private Vector3 prevMousePosition;
    private float timeSinceMouseMoved = 0;

    public const float WAIT_TIME = 3;

    private void Awake()
    {
        setMessage += (string message) => text.text = message;
    }

    private void Update()
    {
        if (EventSystem.current.IsPointerOverGameObject() || Input.mousePosition != prevMousePosition)
        {
            timeSinceMouseMoved = 0;
            displayObject.SetActive(false);
        }
        else
        {
            timeSinceMouseMoved += Time.deltaTime;
            if(timeSinceMouseMoved > WAIT_TIME)
            {
                displayObject.SetActive(true);
            }
        }
        prevMousePosition = Input.mousePosition;
    }
}
