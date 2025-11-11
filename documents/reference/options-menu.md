# OptionsMenu API Reference

## Overview
- **Namespace:** `TCS.Options`
- **Assembly:** `Scripts/OptionsMenu.cs`
- **Base Type:** `UnityEngine.MonoBehaviour`

`OptionsMenu` owns the runtime lifecycle of the options UI. It coordinates the `UIDocument`,
collects all UI Toolkit elements, instantiates `OptionSettings`, and exposes a simple entrypoint for
opening the menu from gameplay code.

### Serialized Fields
| Field | Type | Description |
| --- | --- | --- |
| `m_uiDocument` | `UIDocument` | Source of the `OptionsMenuElement`. Auto-fetched on `Awake` if omitted. |
| `m_audioMixer` | `AudioMixer` | Mixer that contains `MASTER`, `MUSIC`, `SFX` parameters. Optional but required for audio control. |
| `m_volumeProfile` | `VolumeProfile` | URP profile looked up for a `DepthOfField` override. Optional. |

### Public Properties
| Property | Type | Description |
| --- | --- | --- |
| `Instance` | `OptionsMenu` | Singleton reference set in `Awake`. |
| `OptionsMenuElement` | `OptionsMenuElement` | Root UI Toolkit element queried from the document. |
| `OptionElementComponents` | `OptionElementComponents` | DTO that exposes all UI controls to the settings logic. |
| `OptionsSettings` | `OptionSettings` | Runtime orchestrator for audio/video subsystems. |
| `SettingValues` | `SettingValues` | In-memory representation of persisted settings. |

### Public Methods
| Method | Description |
| --- | --- |
| `void OpenOptionMenu()` | Makes the button chooser visible by setting `CurrentOptionType = OptionType.Choices`. |

### Lifecycle Notes
1. **Awake**
   - Enforces singleton, detaches from any parent, and calls `DontDestroyOnLoad`.
   - Ensures the `UIDocument` reference exists and queries the `OptionsMenuElement`.
   - Builds `OptionElementComponents`, assigning sliders, toggles, dropdowns, mixer, and volume profile.
   - Instantiates `SettingValues` + `OptionSettings`.
2. **Start**
   - Calls `OptionsSettings.Init()` to push saved values into the UI.
   - Passes the saved resolution into `OptionsMenuElement.Init()` so the dropdown highlights it.
3. **OnDestroy**
   - Disposes both `OptionsMenuElement` and `OptionSettings`, which in turn unregister callbacks and save data.

### Usage Example
```csharp
using TCS.Options;

public class PauseMenu : MonoBehaviour {
    public void OnOptionsPressed() {
        OptionsMenu.Instance.OpenOptionMenu();
    }
}
```

---

## OptionsMenuElement (UI Toolkit)
- **Namespace:** `TCS.Options`
- **Assembly:** `Scripts/OptionsMenuElement.cs`
- **Base Type:** `UnityEngine.UIElements.VisualElement`

`OptionsMenuElement` is the UXML root that shows the button chooser plus individual sections (Audio,
Video, or any extension you add).

### UXML Attributes
| Attribute | Type | Description |
| --- | --- | --- |
| `current-option-type` | `OptionType` | Serialized state backing `CurrentOptionType`. Changing it toggles panes. |

### Notable Members
| Member | Description |
| --- | --- |
| `OptionType CurrentOptionType { get; set; }` | Enum with values `None`, `Choices`, `Audio`, `Video`. Drives visibility via `HandleMenuSelection`. |
| `AudioSettingsElement AudioSettings { get; }` | Child element exposing audio sliders + Back button. |
| `VideoSettingsElement VideoSettings { get; }` | Child element for resolution, fullscreen, VSync, DOF, Accept button. |
| `void Init(Vector2 currentResolution)` | Registers callbacks for nav buttons and initializes video dropdown values. |
| `void HandleReturnBackPressed()` | Convenience method that flips back to the button chooser. |
| `void Dispose()` | Unregisters all callbacks to prevent leaks when the document is destroyed. |

### UI Events
- `HandleAudioPressed`, `HandleVideoPressed`, `HandleBackPressed` (private) update `CurrentOptionType`.
- Each pane’s Back button delegates to `HandleReturnBackPressed`.

---

## OptionElementComponents (DTO)
- **Namespace:** `TCS.Options`
- **Assembly:** `Scripts/OptionElementComponents.cs`

`OptionElementComponents` is a plain container that gathers every UI control the runtime helpers need.
It is built once in `OptionsMenu.Awake` and passed into `OptionSettings`.

### Static Members
| Member | Description |
| --- | --- |
| `Vector2[] SupportedResolutions` | Hardcoded list of dropdown entries: 1280×720, 1920×1080, 2560×1440, 3840×2160. |

### Instance Members
| Property | Type | Description |
| --- | --- | --- |
| `AudioMixer AudioMixer` | `AudioMixer` | Mixer reference used by `AudioSettings`. |
| `Slider MasterVolumeSlider` | `Slider` | UI Toolkit slider controlling the `MASTER` mixer parameter. |
| `Slider MusicVolumeSlider` | `Slider` | Controls `MUSIC`. |
| `Slider SfxVolumeSlider` | `Slider` | Controls `SFX`. |
| `VolumeProfile VolumeProfile` | `VolumeProfile` | Optional URP profile for Depth of Field toggling. |
| `DropdownField ResolutionDropdown` | `DropdownField` | Source of resolution choice strings. |
| `Toggle FullscreenToggle` | `Toggle` | Binds to `VideoValues.Fullscreen`. |
| `Toggle VSyncToggle` | `Toggle` | Binds to `VideoValues.VSync`. |
| `Toggle DepthOfFieldToggle` | `Toggle` | Enables or disables URP Depth of Field. |
| `Button AcceptChangesButton` | `Button` | Applies video settings when clicked. |

Use this object when extending the system with new UI controls. Add new properties, assign them in
`OptionsMenu.Awake`, and consume them inside your custom settings helper.
