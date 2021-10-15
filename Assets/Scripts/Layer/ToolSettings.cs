using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;



public class ToolSettings : MonoBehaviour
{
    public static UnityAction<IModule> onToolSelected;

    private void Awake()
    {
        onToolSelected += ShowToolSettings;
    }

    public void ShowToolSettings(IModule module)
    {
        
    }
}
