using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using CS2Cheat.DTO.ClientDllDTO;
using CS2Cheat.Utils.DTO;
using Newtonsoft.Json;

namespace CS2Cheat.Utils;

public abstract class Offsets
{
    #region offsets

    public const float WeaponRecoilScale = 2f;
    
    public static int dwLocalPlayerPawn;
    public static int dwLocalPlayerController;
    public static int dwEntityList;
    public static int dwViewMatrix;
    public static int dwViewAngles;
    public static int dwPlantedC4;
    public static int dwGlobalVars;
    public static int dwBuildNumber;

    public static int m_vOldOrigin;
    public static int m_vecViewOffset;
    public static int m_AimPunchAngle;
    public static int m_modelState;
    public static int m_pGameSceneNode;
    public static int m_fFlags;
    public static int m_iIDEntIndex;
    public static int m_lifeState;
    public static int m_iHealth;
    public static int m_iTeamNum;
    public static int m_bDormant;
    public static int m_iShotsFired;
    public static int m_hPawn; 
    public static int m_entitySpottedState;
    public static int m_Item;
    public static int m_pClippingWeapon;
    public static int m_AttributeManager;
    public static int m_iItemDefinitionIndex;
    public static int m_bIsScoped;
    public static int m_flFlashDuration;
    public static int m_iszPlayerName;
    public static int m_nBombSite;
    public static int m_bBombDefused;
    public static int m_vecAbsVelocity;
    public static int m_flDefuseCountDown;
    public static int m_flC4Blow;
    public static int m_bBeingDefused;
    
    public const nint m_nCurrentTickThisFrame = 0x34;

    public static readonly Dictionary<string, int> Bones = new()
    {
        { "head", 6 }, { "neck_0", 5 }, { "spine_1", 4 }, { "spine_2", 2 }, { "pelvis", 0 },
        { "arm_upper_L", 8 }, { "arm_lower_L", 9 }, { "hand_L", 10 },
        { "arm_upper_R", 13 }, { "arm_lower_R", 14 }, { "hand_R", 15 },
        { "leg_upper_L", 22 }, { "leg_lower_L", 23 }, { "ankle_L", 24 },
        { "leg_upper_R", 25 }, { "leg_lower_R", 26 }, { "ankle_R", 27 }
    };

    public static async Task UpdateOffsets()
    {
        try
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("[*] Loading local offsets from 'output' folder...");

            // 1. Read Local Files
            string jsonOffsets = ReadLocalFile("offsets.json");
            string jsonClient = ReadLocalFile("client_dll.json");

            var sourceDataDw = JsonConvert.DeserializeObject<OffsetsDTO>(jsonOffsets);
            var sourceDataClient = JsonConvert.DeserializeObject<ClientDllDTO>(jsonClient);

            Console.WriteLine("[*] Files loaded. Parsing...");

            dynamic destData = new ExpandoObject();

            long GetOffset(Dictionary<string, long> dict, string key) 
            {
                if (dict != null && dict.TryGetValue(key, out var val)) return val;
                return 0;
            }

            long GetField(string className, string fieldName) 
            {
                if (sourceDataClient.client_dll.classes.TryGetValue(className, out var classData) &&
                    classData.fields.TryGetValue(fieldName, out var offset))
                {
                    return offset;
                }
                return 0;
            }

            // Global Offsets
            destData.dwLocalPlayerController = GetOffset(sourceDataDw.client_dll, "dwLocalPlayerController");
            destData.dwEntityList = GetOffset(sourceDataDw.client_dll, "dwEntityList");
            destData.dwViewMatrix = GetOffset(sourceDataDw.client_dll, "dwViewMatrix");
            destData.dwPlantedC4 = GetOffset(sourceDataDw.client_dll, "dwPlantedC4");
            destData.dwLocalPlayerPawn = GetOffset(sourceDataDw.client_dll, "dwLocalPlayerPawn");
            destData.dwViewAngles = GetOffset(sourceDataDw.client_dll, "dwViewAngles");
            destData.dwGlobalVars = GetOffset(sourceDataDw.client_dll, "dwGlobalVars");
            destData.dwBuildNumber = GetOffset(sourceDataDw.engine2_dll, "dwBuildNumber");

            // Schema Fields
            destData.m_fFlags = GetField("C_BaseEntity", "m_fFlags");
            destData.m_pGameSceneNode = GetField("C_BaseEntity", "m_pGameSceneNode");
            destData.m_lifeState = GetField("C_BaseEntity", "m_lifeState");
            destData.m_iHealth = GetField("C_BaseEntity", "m_iHealth");
            destData.m_iTeamNum = GetField("C_BaseEntity", "m_iTeamNum");
            destData.m_vecAbsVelocity = GetField("C_BaseEntity", "m_vecAbsVelocity");
            destData.m_vOldOrigin = GetField("C_BasePlayerPawn", "m_vOldOrigin");
            destData.m_vecViewOffset = GetField("C_BaseModelEntity", "m_vecViewOffset");
            destData.m_AimPunchAngle = GetField("C_CSPlayerPawn", "m_aimPunchAngle");
            destData.m_iIDEntIndex = GetField("C_CSPlayerPawn", "m_iIDEntIndex");
            destData.m_iShotsFired = GetField("C_CSPlayerPawn", "m_iShotsFired");
            destData.m_entitySpottedState = GetField("C_CSPlayerPawn", "m_entitySpottedState");
            destData.m_bIsScoped = GetField("C_CSPlayerPawn", "m_bIsScoped");
            destData.m_pClippingWeapon = GetField("C_CSPlayerPawn", "m_pClippingWeapon");
            destData.m_flFlashDuration = GetField("C_CSPlayerPawnBase", "m_flFlashDuration");
            destData.m_modelState = GetField("CSkeletonInstance", "m_modelState");
            destData.m_bDormant = GetField("CGameSceneNode", "m_bDormant");
            destData.m_iszPlayerName = GetField("CBasePlayerController", "m_iszPlayerName");
            destData.m_Item = GetField("C_AttributeContainer", "m_Item");
            destData.m_AttributeManager = GetField("C_EconEntity", "m_AttributeManager");
            destData.m_iItemDefinitionIndex = GetField("C_EconItemView", "m_iItemDefinitionIndex");
            destData.m_nBombSite = GetField("C_PlantedC4", "m_nBombSite");
            destData.m_bBombDefused = GetField("C_PlantedC4", "m_bBombDefused");
            destData.m_flDefuseCountDown = GetField("C_PlantedC4", "m_flDefuseCountDown");
            destData.m_flC4Blow = GetField("C_PlantedC4", "m_flC4Blow");
            destData.m_bBeingDefused = GetField("C_PlantedC4", "m_bBeingDefused");

            // --- SMART PAWN HANDLE LOOKUP ---
            // Try to find the pawn handle offset dynamically
            long hPawn = GetField("CBasePlayerController", "m_hPawn");
            if (hPawn == 0) hPawn = GetField("CCSPlayerController", "m_hPlayerPawn");
            destData.m_hPawn = hPawn;

            UpdateStaticFields(destData);
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[SUCCESS] EntityList: {destData.dwEntityList}");
            Console.WriteLine($"[SUCCESS] ViewMatrix: {destData.dwViewMatrix}");
            Console.WriteLine($"[SUCCESS] m_hPawn: 0x{destData.m_hPawn:X}");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[ERROR] Local File Load Failed: {ex.Message}");
            Console.WriteLine("Make sure 'output' folder with .json files is in the same folder as the .exe!");
            throw;
        }
    }

    private static string ReadLocalFile(string filename)
    {
        // 1. Check current directory (where .exe is)
        string basePath = AppDomain.CurrentDomain.BaseDirectory;
        string path = Path.Combine(basePath, "output", filename);
        
        if (File.Exists(path)) return File.ReadAllText(path);

        // 2. Check up to 4 levels back (useful when running from Visual Studio bin/debug/...)
        for (int i = 0; i < 4; i++)
        {
            basePath = Path.Combine(basePath, "..");
            path = Path.Combine(basePath, "output", filename);
            if (File.Exists(path)) return File.ReadAllText(path);
        }

        throw new FileNotFoundException($"Could not find '{filename}' in any 'output' folder.");
    }

    private static void UpdateStaticFields(dynamic data)
    {
        dwLocalPlayerPawn = (int)data.dwLocalPlayerPawn;
        m_vOldOrigin = (int)data.m_vOldOrigin;
        m_vecViewOffset = (int)data.m_vecViewOffset;
        m_AimPunchAngle = (int)data.m_AimPunchAngle;
        m_modelState = (int)data.m_modelState;
        m_pGameSceneNode = (int)data.m_pGameSceneNode;
        m_iIDEntIndex = (int)data.m_iIDEntIndex;
        m_lifeState = (int)data.m_lifeState;
        m_iHealth = (int)data.m_iHealth;
        m_iTeamNum = (int)data.m_iTeamNum;
        m_bDormant = (int)data.m_bDormant;
        m_iShotsFired = (int)data.m_iShotsFired;
        m_hPawn = (int)data.m_hPawn; 
        m_fFlags = (int)data.m_fFlags;
        dwLocalPlayerController = (int)data.dwLocalPlayerController;
        dwViewMatrix = (int)data.dwViewMatrix;
        dwViewAngles = (int)data.dwViewAngles;
        dwEntityList = (int)data.dwEntityList;
        m_entitySpottedState = (int)data.m_entitySpottedState;
        m_Item = (int)data.m_Item;
        m_pClippingWeapon = (int)data.m_pClippingWeapon;
        m_AttributeManager = (int)data.m_AttributeManager;
        m_iItemDefinitionIndex = (int)data.m_iItemDefinitionIndex;
        m_bIsScoped = (int)data.m_bIsScoped;
        m_flFlashDuration = (int)data.m_flFlashDuration;
        m_iszPlayerName = (int)data.m_iszPlayerName;
        dwPlantedC4 = (int)data.dwPlantedC4;
        dwGlobalVars = (int)data.dwGlobalVars;
        m_nBombSite = (int)data.m_nBombSite;
        m_bBombDefused = (int)data.m_bBombDefused;
        m_vecAbsVelocity = (int)data.m_vecAbsVelocity;
        m_flDefuseCountDown = (int)data.m_flDefuseCountDown;
        m_flC4Blow = (int)data.m_flC4Blow;
        m_bBeingDefused = (int)data.m_bBeingDefused;
    }

    #endregion
}