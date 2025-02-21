namespace AvaloniaTodoAPp.Memento;

public interface MCommand
{
    public void doCommand();
    public void undoCommand();
}