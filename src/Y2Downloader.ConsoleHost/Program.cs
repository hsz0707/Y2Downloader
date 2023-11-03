namespace Y2Downloader.ConsoleHost;

using Common.Interfaces;

using IoC.MicrosoftDi;

internal class Program
{
    private static async Task Main()
    {
        try
        {
            IIoCManager container = new MicrosoftDiManager();
            container.RegisterTypes<AppSettings, ConsoleLogger>();

            var app = container.GetApp();
            app.Init();
            await app.RunAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine("(>-:[CRITICAL ERROR]:-<)");
            Console.WriteLine(e);
        }

        Console.WriteLine("Press 'Enter' to exit.");
        Console.ReadLine();
    }
}