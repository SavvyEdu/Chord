using System.Collections.Generic;

public static class CommandHistory 
{
    private static List<ICommand> commands = new List<ICommand>();
    private static int currentIndex = -1;
    
    public static void AddCommand(ICommand command)
    {
        //remove any commands that have been undone
        if(currentIndex < commands.Count - 1)
            commands.RemoveRange(currentIndex + 1, commands.Count - (currentIndex + 1));

        commands.Add(command);
        currentIndex = commands.Count - 1;
    }

    public static void Undo()
    {
        if (currentIndex == -1) //nothing to Undo
            return;

        commands[currentIndex].Undo();
        currentIndex--;
    }

    public static void Redo()
    {
        if (currentIndex == commands.Count - 1) //nothing to Redo
            return;

        commands[currentIndex + 1].Execute();
        currentIndex++;
    }
}
