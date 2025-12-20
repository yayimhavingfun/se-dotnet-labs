namespace Itmo.ObjectOrientedProgramming.Lab4.Presentation;

public class Program
{
    public static void Main()
    {
        try
        {
            Console.WriteLine("======================================");
            Console.WriteLine("   File System Manager Application");
            Console.WriteLine("======================================\n");

            var app = new ConsoleApplication();
            app.Run();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fatal error: {ex.Message}");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}