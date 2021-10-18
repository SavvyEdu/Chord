using UnityEngine;
using TMPro;

public class Version : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<TextMeshProUGUI>().text = "version " + Application.version;
    }
}
