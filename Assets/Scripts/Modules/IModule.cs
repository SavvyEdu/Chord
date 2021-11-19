using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> common proporties accross the drawing modules </summary>
public interface IModule
{
    bool editing { get; set; }
    string tooltipMessage { get; }
    DrawMode drawMode { get; }
    void MainInputDown();
    void MainInputPressed();
    void MainInputReleased();
    void AltInputDown();
    void AltInputPressed();
    void AltInputReleased();
    void WhileEditing();
    void DrawEditing();
    void CancelEditing();
}

/// <summary> Generic base module for data specific implementation </summary>
/// <typeparam name="T">module data type</typeparam>
public abstract class Module<T> : IModule where T : ShapeData
{
    //IModule abstract implementation
    public bool editing { get; set; }
    public abstract string tooltipMessage { get; }
    public abstract DrawMode drawMode { get; }
    public virtual void MainInputDown() { }
    public virtual void MainInputPressed() { }
    public virtual void MainInputReleased() { }
    public virtual void AltInputDown() { }
    public virtual void AltInputPressed() { }
    public virtual void AltInputReleased() { }
    public virtual void WhileEditing() { }
    public virtual void DrawEditing() { }
    public virtual void CancelEditing() 
    { 
        editing = false;
        current = null;
    }

    //Type specific
    public abstract T current { get; set; }
    public abstract void DrawShapes(List<T> data);
}
