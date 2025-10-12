using System.Runtime.InteropServices;

namespace Itmo.ObjectOrientedProgramming.Lab2.Services.Alerts;

public class SoundAlertSystem : IAlertSystem
{
    public string Name => "Sound Alert System";

    public void Alert(string message)
    {
        Console.WriteLine($"🔊 SOUND ALERT: {message}");
        Console.WriteLine($"Timestamp: {DateTime.Now:HH:mm:ss}");

        try
        {
            if (IsWindows())
            {
                // windows-specific sound (commented out bc it doesn't work for me :( )
                // Console.Beep(1000, 500);
            }
            else
            {
                // linux/macOS fallback - visual representation
                Console.WriteLine("[BEEP! BEEP! BEEP!]");
                FlashConsole();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Sound failed: {ex.Message}]");
            Console.WriteLine("[VISUAL ALERT ACTIVATED]");
        }
    }

    private bool IsWindows()
    {
        return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
    }

    private void FlashConsole()
    {
        // visual alert for non-windows systems
        ConsoleColor originalColor = Console.ForegroundColor;

        for (int i = 0; i < 3; i++)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Thread.Sleep(200);
            Console.ForegroundColor = originalColor;
            Thread.Sleep(200);
        }
    }
}