# CS2Deniz 🌊

**CS2Deniz** is a lightweight, external cheat and overlay for **Counter-Strike 2**. It provides visual assistance and automation features while running entirely external to the game process.

> **⚠️ DISCLAIMER:** This software is for **educational purposes only**. Using cheats in online games violates Terms of Service and can lead to account bans. The author assumes no responsibility for how this software is used or any consequences resulting from its use.

---

## 🛠️ Prerequisites

Before running or building the project, ensure you have the following installed on your system:

1.  **Undefeated Font**  
    The overlay uses a specific font for rendering text.  
    *   Locate the font file in the `assets/` folder or download it.
    *   Right-click the font file and select **Install**.

2.  **.NET Desktop Runtime 8.0.22 (x64)**  
    This application requires the .NET 8 Runtime to execute.  
    *   [Download .NET Desktop Runtime 8.0.22 (Windows x64)](https://dotnet.microsoft.com/en-us/download/dotnet/thank-you/runtime-desktop-8.0.22-windows-x64-installer)

3.  **Administrative Privileges**  
    The application requires Administrator rights to read the game's memory and draw the overlay on top of the game window.

---

## ⚙️ Building the Project

You can build the project using the .NET CLI or Visual Studio.

1.  Open your terminal (PowerShell, CMD, or VS Code Terminal) in the project root directory.
2.  Run the clean command to remove old artifacts:
    ```powershell
    dotnet clean
    ```
3.  Build the project:
    ```powershell
    dotnet build
    ```

Once the build is successful, you will find the executables in:
`.\bin\Debug\`

---

## 🚀 How to Run

To ensure the overlay renders correctly and interacts with the game, follow these steps strictly:

### 1. Configure Game Video Settings
External overlays cannot draw over "Exclusive Fullscreen" mode.
1.  Open **Counter-Strike 2**.
2.  Go to **Settings** -> **Video** -> **Video**.
3.  Set **Display Mode** to **Fullscreen Windowed** (Borderless) or **Windowed**.
4.  Apply changes.

### 2. Launch the Cheat
1.  Navigate to your output folder: `.\bin\Debug\`
2.  Locate **`CS2Deniz.exe`**.
3.  **Right-click** the file and select **Run as Administrator**.
4.  The console will display: `Waiting for CS2 to start...` (if the game is not open) or `Cheat Activated!`.

---

## 🔧 Configuration

The application generates a `config.json` file in the build directory after the first run. You can modify settings in two ways:

### Method A: Interactive Menu
The build process automatically generates a configuration tool.
1.  Go to `.\bin\Debug\`
2.  Run **`Configuration.exe`**.
3.  Use the console menu to toggle features (Aimbot, ESP, Triggerbot, etc.) and change keybinds.
4.  Select "Save & Exit" to apply changes.

### Method B: Manual Editing
1.  Open `config.json` with any text editor (Notepad, VS Code).
2.  Change values (e.g., set `"AimBot": true` or change key codes).
3.  Save the file and restart the cheat.

---

## ⌨️ Features

*   **ESP (Extra Sensory Perception):** Box ESP, Skeleton ESP, Health Bars.
*   **Aimbot:** Smooth aiming assistance with configurable keys.
*   **Triggerbot:** Automatically fires when crosshair is over an enemy.
*   **Bomb Site:** Shows location of the D4 planted site overlay.
*   **Team Check:** Avoids locking onto or drawing ESP for teammates.

---

## 🛑 Troubleshooting

*   **Overlay not showing?**
    Ensure the game is in **Fullscreen Windowed** mode and you are running the `.exe` as **Administrator**.
*   **"Prerequisites missing" error?**
    Install the .NET 8.0.22 Runtime linked in the Prerequisites section.
*   **ESP incorrect?**
    The cheat relies on memory offsets. If CS2 updates, the offsets must be updated. The application attempts to fetch the latest offsets automatically on startup.