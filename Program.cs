using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using CS2Cheat.Data.Game;
using CS2Cheat.Features;
using CS2Cheat.Graphics;
using CS2Cheat.Utils;
using static CS2Cheat.Core.User32;
using Application = System.Windows.Application;

namespace CS2Cheat;

public class Program :
    Application,
    IDisposable
{
    // --- MAIN ENTRY POINT ---
    [STAThread]
    public static void Main()
    {
        // 1. Detect if running as Configuration tool
        string fullPath = Environment.ProcessPath ?? "";
        string fileName = Path.GetFileName(fullPath);

        if (fileName.Contains("Config", StringComparison.OrdinalIgnoreCase))
        {
            Console.Title = "CS2Deniz Configuration";
            InteractiveConfigMenu.Run();
            return;
        }

        // 2. Cheat Mode - Introduction
        Console.Title = "CS2Deniz External";
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("========================================");
        Console.WriteLine("      CS2Deniz External Cheat v1.0      ");
        Console.WriteLine("========================================");
        Console.ResetColor();

        // 3. Wait for Counter-Strike 2
        Console.Write("[Status] Waiting for CS2 to start...");
        
        // FIX: Explicitly use System.Diagnostics to avoid conflicts with Process.NET
        while (System.Diagnostics.Process.GetProcessesByName("cs2").Length == 0)
        {
            // Wait 1 second and print a dot
            System.Threading.Thread.Sleep(1000);
            Console.Write(".");
        }

        Console.WriteLine(); // New line
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("[Status] CS2 Process Found!");
        Console.WriteLine("[Status] Cheat Activated Successfully.");
        Console.ResetColor();
        Console.WriteLine("----------------------------------------");

        // 4. Start the Application
        new Program().Run();
    }
    // ------------------------

    private Program()
    {
        Console.WriteLine("[Init] Updating Offsets...");
        // Only download offsets if we are running the cheat
        Offsets.UpdateOffsets().Wait();

        Startup += (_, _) => InitializeComponent();
        Exit += (_, _) => Dispose();
    }

    private GameProcess GameProcess { get; set; } = null!;
    private GameData GameData { get; set; } = null!;
    private WindowOverlay WindowOverlay { get; set; } = null!;
    private Graphics.Graphics Graphics { get; set; } = null!;
    private TriggerBot Trigger { get; set; } = null!;
    private AimBot AimBot { get; set; } = null!;
    private BombTimer BombTimer { get; set; } = null!;

    public void Dispose()
    {
        GameProcess?.Dispose();
        GameData?.Dispose();
        WindowOverlay?.Dispose();
        Graphics?.Dispose();
        Trigger?.Dispose();
        AimBot?.Dispose();
        BombTimer?.Dispose();
    }

    private void InitializeComponent()
    {
        // Load settings from config.json
        var features = ConfigManager.Load();

        GameProcess = new GameProcess();
        GameProcess.Start();

        GameData = new GameData(GameProcess);
        GameData.Start();

        WindowOverlay = new WindowOverlay(GameProcess);
        WindowOverlay.Start();

        Graphics = new Graphics.Graphics(GameProcess, GameData, WindowOverlay);
        Graphics.Start();

        Trigger = new TriggerBot(GameProcess, GameData);
        if (features.TriggerBot) Trigger.Start();

        AimBot = new AimBot(GameProcess, GameData);
        if (features.AimBot) AimBot.Start();

        BombTimer = new BombTimer(Graphics);
        if (features.BombTimer) BombTimer.Start();

        // OBS bypass
        SetWindowDisplayAffinity(WindowOverlay!.Window.Handle, 0x00000011);
    }
}