using CS2Cheat.Utils;
using Color = SharpDX.Color;

namespace CS2Cheat.Features;

internal class BombTimer(Graphics.Graphics graphics) : ThreadedServiceBase
{
    // Static fields for the Draw method
    private static bool _isBombPlanted;
    private static string _bombSite = "";

    // Internal pointers
    private IntPtr _plantedC4;
    private IntPtr _tempC4;

    protected override void FrameAction()
    {
        // 1. Read C4 Pointers (Using the logic you confirmed works for Site)
        _tempC4 = graphics.GameProcess.ModuleClient.Read<IntPtr>(Offsets.dwPlantedC4);
        _plantedC4 = graphics.GameProcess.Process.Read<IntPtr>(_tempC4);

        // 2. Check if Planted (Using the logic you confirmed works)
        _isBombPlanted = graphics.GameProcess.ModuleClient.Read<bool>(Offsets.dwPlantedC4 - 0x8);

        // 3. Read Site Index only if planted
        if (_isBombPlanted)
        {
            int siteIndex = graphics.GameProcess.Process.Read<int>(_plantedC4 + Offsets.m_nBombSite);
            _bombSite = siteIndex == 1 ? "B" : "A";
        }
        else
        {
            _bombSite = "";
        }
    }

    public static void Draw(Graphics.Graphics graphics)
    {
        // Only draw if bomb is planted
        if (!_isBombPlanted) return;

        // Display simple text: "Bomb planted on site: A"
        graphics.FontAzonix64.DrawText(default,
            $"Bomb planted on site: {_bombSite}", 
            30, 500, Color.Red);
    }
}