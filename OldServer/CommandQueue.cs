namespace SanicballServer;

/// <summary>
/// Thread-safe command queue
/// </summary>
internal class CommandQueue
{
    private readonly List<Command> commands = [];

    public void Add(Command command)
    {
        lock (commands)
        {
            commands.Add(command);
        }
    }

    public IEnumerable<Command> ReadNext()
    {
        lock (commands)
        {
            foreach (Command cmd in commands)
            {
                yield return cmd;
            }
            commands.Clear();
        }
    }
}

internal delegate void CommandHandler(Command cmd);

internal class Command
{
    public string Name { get; }
    public string Content { get; } = "";

    public Command(string text)
    {
        text = text.Trim();

        int split = text.IndexOf(' ');
        if (split > -1)
        {
            Name = text[..split];
            if (text.Length > split + 1)
            {
                Content = text[(split + 1)..];
            }
        }
        else
        {
            Name = text;
        }
    }
}