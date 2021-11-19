using System.Collections.Generic;
using UnityEngine;

public static class CommandHistory 
{
    private static Stack<ICommand> undoCommands = new Stack<ICommand>();
    private static Stack<ICommand> redoCommands = new Stack<ICommand>();
    
    public static void AddCommand(ICommand command)
    {
        //remove any commands that have been undone
        redoCommands.Clear();
        undoCommands.Push(command);
        command.Execute();
    }

    public static void Undo()
    {
        if (undoCommands.Count > 0)
        {
            ICommand command = undoCommands.Pop();
            redoCommands.Push(command);
            command.Undo();
        }
    }

    public static void Redo()
    {
        if (redoCommands.Count > 0)
        {
            ICommand command = redoCommands.Pop();
            undoCommands.Push(command);
            command.Execute();
        }
    }
}
