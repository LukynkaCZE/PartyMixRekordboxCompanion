namespace PartyMixCompanionProxy;

static class Program
{
    
    public static Window WindowHost;

    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        WindowHost = new Window();
        Application.Run(WindowHost);
    }
}