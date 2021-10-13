using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IModule
{
    bool editing { get; set; }
    string tooltipMessage { get; }
    void InputDown();
    void InputPressed();
    void InputReleased();
    void WhileEditing();
}

public abstract class Module<T> : IModule where T : ShapeData
{
    public bool editing { get; set; }
    public abstract T current { get; set; }
    public abstract string tooltipMessage { get; }

    public abstract void DrawShapes(List<T> data);
    public abstract void DrawEditing();

    public virtual void InputDown() { }
    public virtual void InputPressed() { }
    public virtual void InputReleased() { }
    public virtual void WhileEditing() { }
}
