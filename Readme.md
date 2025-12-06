# CS2Deniz

<div align="center">
  <!-- LOGO -->
  <img src="assets/logo.png" alt="CS2Deniz Logo" width="200" />

  <br>
  <br>

  <!-- TAGS / BADGES -->
  <img src="https://img.shields.io/badge/Language-C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white" />
  <img src="https://img.shields.io/badge/.NET-8.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" />
  <img src="https://img.shields.io/badge/Platform-Windows-0078D6?style=for-the-badge&logo=windows&logoColor=white" />
  <img src="https://img.shields.io/badge/Type-External-FF0000?style=for-the-badge" />
  <img src="https://img.shields.io/badge/Game-CS2-orange?style=for-the-badge&logo=counter-strike&logoColor=white" />
</div>

<!-- LINE SEPARATOR -->
<hr>

**CS2Deniz** is a lightweight, external cheat and overlay for **Counter-Strike 2**. It provides visual assistance and automation features while running entirely external to the game process.

> **⚠️ DISCLAIMER:** This software is for **educational purposes only**. Using cheats in online games violates Terms of Service and can lead to account bans. The author assumes no responsibility for how this software is used or any consequences resulting from its use.

---

## 📥 Download

If you just want to use the tool without building it yourself, download the latest pre-compiled version here:

[**Download CS2Deniz.zip (Latest Release)**](https://github.com/anas1412/CS2Deniz/releases/latest/download/CS2Deniz.zip)

*(Unzip the file to a folder of your choice before running).*

---

## 🛠️ Prerequisites (For All Users)

Whether you downloaded the release or built it yourself, you **must** have these installed to run the application:

1.  **.NET Desktop Runtime 8.0.22 (x64)**  
    Required to run the application.  
    [Download Runtime Installer](https://builds.dotnet.microsoft.com/dotnet/WindowsDesktop/8.0.22/windowsdesktop-runtime-8.0.22-win-x64.exe)

2.  **Undefeated Font**  
    The overlay requires this specific font to render text correctly.  
    *   Locate the font file in the CS2Deniz.zip file or `assets/` folder.
    *   Right-click the font file and select **Install**.

3.  **Administrative Privileges**  
    The application must be run as **Administrator** to read game memory and draw the overlay.

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
1.  Navigate to the folder where you extracted or built the tool.
2.  Locate **`CS2Deniz.exe`**.
3.  **Right-click** the file and select **Run as Administrator**.
4.  The console will display: `Waiting for CS2 to start...` (if the game is not open) or `Cheat Activated!`.

---

## 🔧 Configuration

The application generates a `config.json` file after the first run. You can modify settings in two ways:

### Method A: Interactive Menu
1.  Run **`Configuration.exe`** (located in the same folder).
2.  Use the console menu to toggle features (Aimbot, ESP, Triggerbot, etc.) and change keybinds.
3.  Select "Save & Exit" to apply changes.

### Method B: Manual Editing
1.  Open `config.json` with any text editor (Notepad, VS Code).
2.  Change values (e.g., set `"AimBot": true` or change key codes).
3.  Save the file and restart the cheat.

---

## 👨‍💻 For Developers: Building from Source

If you want to modify the code or build the project yourself, follow these instructions to create a clean, single-file executable.

### Build Prerequisites
*   **.NET 8.0 SDK (v8.0.416)**  
    Required to compile the code.  
    [Download .NET 8.0 SDK (x64)](https://builds.dotnet.microsoft.com/dotnet/Sdk/8.0.416/dotnet-sdk-8.0.416-win-x64.exe)

### Build Instructions
1.  Clone the repository and open your terminal in the project root.
2.  Clean previous build artifacts:
    ```powershell
    dotnet clean
    ```
3.  Publish the project as a single file (Release mode):
    ```powershell
    dotnet publish -c Release -r win-x64 -p:PublishSingleFile=true --self-contained false
    ```
4.  **Locate the Output:**
    The executable will be located in:
    `.\bin\Release\net8.0-windows\win-x64\publish\`

5.  **Create the Config Tool:**
    *   Go to the publish folder above.
    *   Copy `CS2Deniz.exe`.
    *   Paste it in the same folder and rename it to **`Configuration.exe`**.
    *   *Note: Both files are required if you want to use the menu, but they share the same internal code.*

---

## ⌨️ Features

*   **ESP (Extra Sensory Perception):** Box ESP, Skeleton ESP, Health Bars.
*   **Aimbot:** Smooth aiming assistance with configurable keys.
*   **Triggerbot:** Automatically fires when crosshair is over an enemy.
*   **Bomb Location:** Accurate C4 site location overlay.
*   **Team Check:** Avoids locking onto or drawing ESP for teammates.

---

## 🛑 Troubleshooting

*   **Overlay not showing?**
    Ensure CS2 is in **Fullscreen Windowed** mode and you ran the `.exe` as **Administrator**.
*   **"Prerequisites missing" error?**
    Install the .NET Desktop Runtime linked in the Prerequisites section.
*   **ESP incorrect?**
    The cheat relies on memory offsets. If CS2 updates, the offsets must be updated. The application attempts to fetch the latest offsets automatically on startup.