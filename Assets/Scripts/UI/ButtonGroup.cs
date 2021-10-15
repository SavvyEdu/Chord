using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonGroup : MonoBehaviour
{
    [HideInInspector]
    public Button[] buttons;
    private Button selectedButton = null;

    public Color normalColor;
    public Color selectedColor;

    private void Awake()
    {
        buttons = GetComponentsInChildren<Button>();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => OnButtonSelected(button));
        }

        //select the first button
        if (buttons.Length > 0)
            selectedButton = buttons[0];
    }

    private void OnEnable()
    {
        if (selectedButton != null)
            selectedButton.onClick?.Invoke();
    }

    private void OnButtonSelected(Button selectedButton)
    {
        this.selectedButton = selectedButton;

        foreach (Button button in buttons)
        {
            button.image.color = normalColor;
        }
        selectedButton.image.color = selectedColor;
    }
}
