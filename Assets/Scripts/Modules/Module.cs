using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Module
{
    bool editing { get; set; }
    string tooltipMessage { get; }

    void DrawShapes();
    void DrawEditing();

    void InputDown();
    void InputPressed();
    void InputReleased();
    void WhileEditing();
}
