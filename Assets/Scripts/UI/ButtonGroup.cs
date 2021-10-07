using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{
    [HideInInspector]
    public Button[] buttons;

    public Color normalColor;
    public Color selectedColor;

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => OnButtonSelected(button));
        }
    }

    private void Start()
    {
        //select the first button
        if (buttons.Length > 0)
            buttons[0].onClick?.Invoke();
    }

    private void OnButtonSelected(Button selectedButton)
    {
        foreach (Button button in buttons)
        {
            button.image.color = normalColor;
        }
        selectedButton.image.color = selectedColor;
    }
}
