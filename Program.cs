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
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Reflection;

namespace CS2Cheat;

public class Program :
    Application,
    IDisposable
{
    // --- MAIN ENTRY POINT ---
    [STAThread]
    public static void Main()
    {
        EnsureDotNetRuntime();
        EnsureFontInstalled();

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
        var version = Assembly.GetExecutingAssembly().GetName().Version;
        string versionStr = version != null ? $"{version.Major}.{version.Minor}.{version.Build}" : "1.1.0";
        Console.Title = $"CS2Deniz External v{versionStr}";
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("========================================");
        Console.WriteLine($"      CS2Deniz External Cheat v{versionStr}      ");
        Console.WriteLine("========================================");
        Console.ResetColor();
        Console.WriteLine("[Reminder] Set CS2 to 'Fullscreen Windowed' or 'Windowed' mode.");
        Console.WriteLine();

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
    private static void EnsureDotNetRuntime()
    {
        const string targetVersion = "Microsoft.WindowsDesktop.App 8.0.22";
        bool isInstalled = false;

        try
        {
            var process = new System.Diagnostics.Process
            {
                StartInfo = new System.Diagnostics.ProcessStartInfo
                {
                    FileName = "dotnet",
                    Arguments = "--list-runtimes",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };
            process.Start();
            string output = process.StandardOutput.ReadToEnd();
            process.WaitForExit();

            if (output.Contains(targetVersion))
            {
                isInstalled = true;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"[System] {targetVersion} requirement fulfilled.");
                Console.ResetColor();
            }
        }
        catch
        {
            // dotnet command not found or other error
        }

        if (!isInstalled)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"[System] {targetVersion} is not installed. Downloading and installing...");
            Console.ResetColor();

            string url = "https://builds.dotnet.microsoft.com/dotnet/WindowsDesktop/8.0.22/windowsdesktop-runtime-8.0.22-win-x64.exe";
            string installerPath = Path.Combine(Path.GetTempPath(), "windowsdesktop-runtime-8.0.22-win-x64.exe");

            try
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    var response = client.GetAsync(url).GetAwaiter().GetResult();
                    response.EnsureSuccessStatusCode();
                    using (var fs = new FileStream(installerPath, FileMode.Create))
                    {
                        response.Content.CopyToAsync(fs).GetAwaiter().GetResult();
                    }
                }

                Console.WriteLine("[System] Installing runtime silently...");
                var installProcess = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = installerPath,
                    Arguments = "/install /quiet /norestart",
                    UseShellExecute = true, // Required for elevation
                    WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
                });

                installProcess?.WaitForExit();
                Console.WriteLine("[System] Runtime installation complete.");
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[Error] Failed to install runtime: {ex.Message}");
                Console.WriteLine("Please install .NET Desktop Runtime 8.0.22 manually.");
                Console.ResetColor();
            }
        }
    }

    private static void EnsureFontInstalled()
    {
        const string fontFileName = "undefeated.ttf";
        const string fontName = "Undefeated (TrueType)";
        string fontsPath = Environment.GetFolderPath(Environment.SpecialFolder.Fonts);
        string destPath = Path.Combine(fontsPath, fontFileName);

        // 1. Check Registry (Primary Method)
        try
        {
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts", false))
            {
                if (key != null)
                {
                    object value = key.GetValue(fontName);
                    if (value != null)
                    {
                         Console.ForegroundColor = ConsoleColor.Green;
                         Console.WriteLine("[System] Font requirement fulfilled.");
                         Console.ResetColor();
                         return;
                    }
                }
            }
        }
        catch
        {
            // Ignore registry access errors and fall back to file check
        }

        // 2. Check File (Fallback Method)
        if (File.Exists(destPath))
        {
             Console.ForegroundColor = ConsoleColor.Green;
             Console.WriteLine("[System] Font requirement fulfilled.");
             Console.ResetColor();
             return;
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[System] Font '{fontFileName}' not found. Installing...");
        Console.ResetColor();

        try
        {
            // 3. Extract font from Embedded Resources
            Console.WriteLine("[System] Extracting font resource...");
            var assembly = Assembly.GetExecutingAssembly();
            // Resource name format: Namespace.Folder.Filename
            string resourceName = "CS2Deniz.assets.undefeated.ttf";
            
            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[Error] Embedded resource '{resourceName}' not found.");
                    Console.WriteLine("Available resources:");
                    foreach (string res in assembly.GetManifestResourceNames())
                    {
                        Console.WriteLine($"- {res}");
                    }
                    Console.ResetColor();
                    return;
                }

                // Copy to Fonts folder
                Console.WriteLine($"[System] Copying font to {destPath}...");
                using (var fileStream = new FileStream(destPath, FileMode.Create))
                {
                    stream.CopyTo(fileStream);
                }
            }

            // 4. Add to Registry
            Console.WriteLine("[System] Updating Registry...");
            using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Fonts", true))
            {
                if (key != null)
                {
                    key.SetValue(fontName, fontFileName);
                }
            }

            // 5. Register font
            Console.WriteLine("[System] Registering font resource...");
            AddFontResource(destPath);
            
            // 6. Broadcast change
            Console.WriteLine("[System] Broadcasting font change...");
            const int WM_FONTCHANGE = 0x001D;
            const int HWND_BROADCAST = 0xFFFF;
            SendNotifyMessage((IntPtr)HWND_BROADCAST, WM_FONTCHANGE, IntPtr.Zero, IntPtr.Zero);

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("[System] Font installed successfully.");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Error] Failed to install font: {ex.Message}");
            Console.ResetColor();
        }
    }

    [DllImport("gdi32.dll", EntryPoint = "AddFontResourceW", SetLastError = true)]
    private static extern int AddFontResource([In][MarshalAs(UnmanagedType.LPWStr)] string lpFileName);

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern bool SendNotifyMessage(IntPtr hWnd, int Msg, IntPtr wParam, IntPtr lParam);

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