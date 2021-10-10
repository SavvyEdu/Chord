using System.Collections.Generic;

/// <summary> Generic Add to List Command for Undo/Redo </summary>
/// <typeparam name="T">list type</typeparam>
public class AddToListCommand<T> : ICommand
{
    private List<T> dataList;
    private T data;

    public AddToListCommand(List<T> dataList, T data)
    {
        this.dataList = dataList;
        this.data = data;
        Execute();
    }

    public virtual void Execute()
    {
        dataList.Add(data);
    }

    public virtual void Undo()
    {
        dataList.RemoveAt(dataList.Count - 1);
    }
}

