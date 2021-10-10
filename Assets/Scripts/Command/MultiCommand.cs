/// <summary> Groups together Undo/Redo commands </summary>
public class MultiCommand : ICommand
{
    private ICommand[] commands;

    public MultiCommand(params ICommand[] commands)
    {
        this.commands = commands;
    }

    public void Execute()
    {
        foreach (ICommand command in commands)
            command.Execute();
    }

    public void Undo()
    {
        foreach (ICommand command in commands)
            command.Undo();
    }
}
