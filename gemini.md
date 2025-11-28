# CS2 External Cheat - Verified Configuration Facts

## Memory Logic
*   **Entity List Traversal:** Must use **`+0`** offset for list entries. (Standard `+0x10` returns empty results on your system).
*   **Controller Identification:** Confirmed working via standard loop `0-64`.

## Offsets & Data Types
| Data Point | Offset | Type | Notes |
| :--- | :--- | :--- | :--- |
| **Entity List Base** | `0x1D11D78` | `IntPtr` | Confirmed via dump. |
| **Pawn Handle** | **`0x6B4`** | `int` | Found in `CBasePlayerController`. The CS2-specific `0x8FC` handle returned garbage/dead entities in your logs. |
| **Team ID** | **`0x3EB`** | **`byte`** | **CRITICAL:** Must be read as 1 byte. Reading as `int` results in value `256` (broken), reading as `byte` results in `2` or `3` (working). |
| **Health** | `0x34C` | `int` | Confirmed valid. |
| **Origin (Position)** | `0x15A0` | `Vector3` | Matches `m_vOldOrigin` in dump. |
| **GameSceneNode** | `0x330` | `IntPtr` | Fallback for position if Origin is 0. |

## Critical Fix Summary
1.  **Do not** skip 16 bytes (`0x10`) when reading the Entity List; read directly at offset `0`.
2.  **Do not** use handle `0x8FC`; use `0x6B4`.
3.  **Do not** read Team as `int`; read as `byte`.