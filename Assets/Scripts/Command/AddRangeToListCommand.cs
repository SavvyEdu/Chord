using System.Collections.Generic;

/// <summary> Generic Add Multiple to List Command for Undo/Redo </summary>
/// <typeparam name="T">list type</typeparam>
public class AddRangeToListCommand<T> : ICommand
{
    private List<T> dataList;
    private T[] data;

    public AddRangeToListCommand(List<T> dataList, T[] data)
    {
        this.dataList = dataList;
        this.data = data;
    }

    public void Execute()
    {
        dataList.AddRange(data);
    }

    public void Undo()
    {
        dataList.RemoveRange(dataList.Count - data.Length, data.Length);
    }
}