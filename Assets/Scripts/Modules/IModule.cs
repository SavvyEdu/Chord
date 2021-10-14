using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> common proporties accross the drawing modules </summary>
public interface IModule
{
    bool editing { get; set; }
    string tooltipMessage { get; }
    DrawMode drawMode { get; }
    void InputDown();
    void InputPressed();
    void InputReleased();
    void WhileEditing();
}

/// <summary> Generic base module for data specific implementation </summary>
/// <typeparam name="T">module data type</typeparam>
public abstract class Module<T> : IModule where T : ShapeData
{
    //IModule abstract implementation
    public bool editing { get; set; }
    public abstract string tooltipMessage { get; }
    public abstract DrawMode drawMode { get; }
    public virtual void InputDown() { }
    public virtual void InputPressed() { }
    public virtual void InputReleased() { }
    public virtual void WhileEditing() { }

    //Type specific
    public abstract T current { get; set; }
    public abstract void DrawShapes(List<T> data);
    public abstract void DrawEditing();
}
