using System;
using System.IO;
using System.Text.Json;
using ProcessKeys = Process.NET.Native.Types.Keys; // Alias for Process.NET Keys
using CS2Cheat.Utils;

namespace CS2Cheat.Utils
{
    public static class InteractiveConfigMenu
    {
        public static void Run()
        {
            var config = ConfigManager.Load();

            Console.Clear();
            Console.WriteLine("=== CS2Cheat Interactive Config Menu ===\n");

            // Simple menu loop
            while (true)
            {
                Console.WriteLine("Current settings:");
                Console.WriteLine($"1. AimBot: {config.AimBot}");
                Console.WriteLine($"2. BombTimer: {config.BombTimer}");
                Console.WriteLine($"3. ESP Aim Crosshair: {config.EspAimCrosshair}");
                Console.WriteLine($"4. ESP Box: {config.EspBox}");
                Console.WriteLine($"5. Skeleton ESP: {config.SkeletonEsp}");
                Console.WriteLine($"6. TriggerBot: {config.TriggerBot}");
                Console.WriteLine($"7. TeamCheck: {config.TeamCheck}");
                Console.WriteLine($"8. AimBot Key: {config.AimBotKey}");
                Console.WriteLine($"9. TriggerBot Key: {config.TriggerBotKey}");
                Console.WriteLine("0. Save & Exit\n");

                Console.Write("Select option to toggle/change: ");
                var input = Console.ReadLine();

                if (input == null) continue;

                switch (input)
                {
                    case "1": config.AimBot = !config.AimBot; break;
                    case "2": config.BombTimer = !config.BombTimer; break;
                    case "3": config.EspAimCrosshair = !config.EspAimCrosshair; break;
                    case "4": config.EspBox = !config.EspBox; break;
                    case "5": config.SkeletonEsp = !config.SkeletonEsp; break;
                    case "6": config.TriggerBot = !config.TriggerBot; break;
                    case "7": config.TeamCheck = !config.TeamCheck; break;
                    case "8":
                        Console.WriteLine("\nCommon Keys: LButton, RButton, MButton, LMenu (Left Alt), RMenu (Right Alt), Capital (Caps Lock)");
                        Console.Write($"Enter new AimBot key (current: {config.AimBotKey}): ");
                        var aimInput = Console.ReadLine();
                        if (!string.IsNullOrEmpty(aimInput) && Enum.TryParse(aimInput, true, out ProcessKeys aimKey))
                        {
                            config.AimBotKey = aimKey;
                            Console.WriteLine($"AimBot key set to: {aimKey}");
                        }
                        else
                        {
                            Console.WriteLine("Invalid or empty key. No changes made.");
                        }
                        break;
                    case "9":
                        Console.WriteLine("\nCommon Keys: LButton, RButton, MButton, LMenu (Left Alt), RMenu (Right Alt), Capital (Caps Lock)");
                        Console.Write($"Enter new TriggerBot key (current: {config.TriggerBotKey}): ");
                        var triggerInput = Console.ReadLine();
                        if (!string.IsNullOrEmpty(triggerInput) && Enum.TryParse(triggerInput, true, out ProcessKeys triggerKey))
                        {
                            config.TriggerBotKey = triggerKey;
                            Console.WriteLine($"TriggerBot key set to: {triggerKey}");
                        }
                        else
                        {
                            Console.WriteLine("Invalid or empty key. No changes made.");
                        }
                        break;
                    case "0":
                        ConfigManager.Save(config);
                        Console.WriteLine("Config saved! Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid option!");
                        break;
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
                Console.Clear();
            }
        }
    }
}
