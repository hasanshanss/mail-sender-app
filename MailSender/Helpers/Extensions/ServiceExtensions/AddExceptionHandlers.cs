public static class ServiceExtensions
{
    public static void AddExceptionHandler()
    {
        AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionTrapper;
    }
    
    private static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e) {
        Console.WriteLine(e.ExceptionObject.ToString());
        Console.WriteLine("Press Enter to continue");
        Console.ReadLine();
        Environment.Exit(1);
    }
}