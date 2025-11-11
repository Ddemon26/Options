# OptionSettings API Reference

## OptionSettings
- **Namespace:** `TCS.Options`
- **Assembly:** `Scripts/OptionSettings.cs`

`OptionSettings` connects the UI layer (`OptionElementComponents`) with implementation helpers
(`AudioSettings`, `VideoSettings`) and persistence (`SaveSettings`).

### Constructor
```csharp
public OptionSettings(OptionElementComponents elementComponents, SettingValues settingValues)
```
| Parameter | Description |
| --- | --- |
| `elementComponents` | DTO containing all UI controls created in `OptionsMenu`. |
| `settingValues` | Snapshot of saved audio/video data. The constructor immediately replaces it with `SaveSettings.Load()`. |

### Public Methods
| Method | Description |
| --- | --- |
| `void Init()` | Initializes audio + video helpers, synchronizing UI controls with saved values. |
| `void Dispose()` | Saves the latest settings and disposes helper callbacks. |

### Internal Flow
1. Load persisted values via `m_saveSettings.Load()`.
2. Instantiate `AudioSettings` and `VideoSettings` with UI references and loaded values.
3. `Init()` sends the saved numbers to the sliders/toggles and applies video state (resolution, VSync, DOF) immediately.
4. `Dispose()` captures current values (`GetCurrentDecibelLevels`, `GetCurrentVideoValues`) and calls `SaveOptionSettings()`.

Use this class as the orchestration point when you add new categories—extend the data models, instantiate
a new helper, and include it in `Init`/`Dispose`.

---

## AudioSettings
- **Namespace:** `TCS.Options`
- **Assembly:** `Scripts/OptionSettings.cs`

`AudioSettings` manages three UI sliders and an `AudioMixer`.

### Constructor
```csharp
public AudioSettings(OptionElementComponents elements, AudioValues values)
```

### Key Members
| Member | Description |
| --- | --- |
| `AudioValues AudioValues { get; }` | Current persisted values (in decibels). |
| `void Init()` | Registers slider callbacks, sets initial slider values, and applies them to the mixer. |
| `void Dispose()` | Unregisters callbacks; safe to call multiple times. |
| `AudioValues GetCurrentDecibelLevels()` | Reads current mixer parameters and returns them (used for saving). |
| `static float PercentToDecibels(float percent)` | Converts slider values (0–100%) to decibels. |
| `static float DecibelsToPercent(float dB)` | Converts stored mixer values back to slider percentages. |

### Mixer Parameters
- `MASTER_PARAM = "MASTER"`
- `MUSIC_PARAM = "MUSIC"`
- `SFX_PARAM = "SFX"`

Update these constants if your mixer uses different exposed parameter names.

---

## VideoSettings
- **Namespace:** `TCS.Options`
- **Assembly:** `Scripts/OptionSettings.cs`

`VideoSettings` binds dropdowns/toggles to a `VideoValues` instance and applies the selections when the
Accept button is clicked.

### Constructor
```csharp
public VideoSettings(OptionElementComponents elements, VideoValues values)
```

### Key Members
| Member | Description |
| --- | --- |
| `VideoValues VideoValues { get; }` | Mutable snapshot updated by callbacks. |
| `void HandleAcceptChangesPressed()` | Applies resolution/fullscreen via `Screen.SetResolution`, VSync via `QualitySettings`, and DOF via `VolumeProfile`. |
| `VideoValues GetCurrentVideoValues()` | Returns the latest snapshot for persistence. |
| `void Dispose()` | Unregisters dropdown/toggle callbacks and Accept button handler. |

### Callback Behavior
- Resolution strings are parsed as `"Width x Height"` inside `SetResolution`.
- Toggle callbacks are created through `CreateToggleCallback(Action<bool>)`, keeping the implementation compact.
- If the assigned `VolumeProfile` is null or missing a `DepthOfField` override, DOF toggling safely no-ops.

---

## SaveSettings
- **Namespace:** `TCS.Options`
- **Assembly:** `Scripts/SaveSettings.cs`

`SaveSettings` encapsulates persistence. It supports:

| Mode | Description |
| --- | --- |
| `SaveType.PlayerPrefs` (default) | Stores floats/ints/bools using Unity’s `PlayerPrefs`. |
| `SaveType.JsonFile` | Writes a human-readable JSON file to `<persistentDataPath>/settings.json`. |

### Members
| Member | Description |
| --- | --- |
| `SaveType m_saveType` | Serialized field choosing the persistence backend. |
| `string SavePath` | Read-only path used when writing JSON. |
| `void Save(SettingValues values)` | Dispatches to the selected backend. |
| `SettingValues Load()` | Returns saved values or defaults if no data exists. |

Extend the `SaveType` enum plus `Save/Load` switches to add cloud saves, encrypted blobs, etc.
