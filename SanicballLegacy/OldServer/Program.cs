
namespace SanicballServer;

internal class Program
{
    private static void Main(string[] args)
    {
        CommandQueue commandQueue = new();
        bool serverClosed = false;

        while (!serverClosed)
        {
            using Server serv = new(commandQueue);
            serv.OnLog += args =>
            {
                Console.ForegroundColor = args.Type switch
                {
                    LogType.Normal => ConsoleColor.White,
                    LogType.Debug => ConsoleColor.Gray,
                    LogType.Warning => ConsoleColor.Yellow,
                    LogType.Error => ConsoleColor.Red,
                };
                Console.WriteLine(args.Message);

                //Reset console color to not mess with the color of input text
                Console.ForegroundColor = ConsoleColor.White;
            };

            Thread inputThread = new(() =>
            {
                string? input;
                while (true)
                {
                    input = Console.ReadLine();
                    if (!string.IsNullOrEmpty(input))
                    {
                        commandQueue.Add(new Command(input.ToString()));
                    }
                }
            });

            inputThread.Start();

            try
            {
                serv.Start();

                //Wait until server closes

                serverClosed = true;
            }
            catch (Exception ex)
            {
                serv.Log("Server encountered an exception and will restart.", LogType.Error);
                string exText = ex.GetType() + ": " + ex.Message + Environment.NewLine + ex.StackTrace;
                serv.Log(exText, LogType.Normal);
                Thread.Sleep(1000);
            }


            inputThread.Abort();
            inputThread.Join();
        }

        Console.WriteLine("Press any key to close this window.");
        Console.ReadKey(true);
    }
}
