using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class Tooltip : MonoBehaviour
{
    public GameObject displayObject;
    public TextMeshProUGUI text;

    public static UnityAction<string> setMessage;
    public static UnityAction<bool> setActive;

    private void Awake()
    {
        setMessage += (string message) => text.text = message;
        setActive += displayObject.SetActive;
    }
}
