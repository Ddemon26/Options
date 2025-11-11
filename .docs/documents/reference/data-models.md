# Data Models Reference

All persistence-ready objects live under the `TCS.Options` namespace and are plain serializable
classes so they can be stored via PlayerPrefs, JSON, or any custom backend. Use the tables below as a
contract when extending the system.

---

## ResolutionData
| Member | Type | Description |
| --- | --- | --- |
| `Width` | `int` | Horizontal resolution in pixels. |
| `Height` | `int` | Vertical resolution in pixels. |

Used inside `VideoValues`. When you need a UI-friendly representation, call
`VideoValues.GetResolutionVector2()` which returns `new Vector2(Width, Height)`.

---

## VideoValues
| Member | Type | Default | Description |
| --- | --- | --- | --- |
| `Resolution` | `ResolutionData` | `new ResolutionData()` | Selected resolution parsed from the dropdown. |
| `VSync` | `bool` | `false` | Mirrors the VSync toggle; applied to `QualitySettings.vSyncCount`. |
| `DepthOfField` | `bool` | `false` | Enables or disables the `DepthOfField` override in the assigned `VolumeProfile`. |
| `Fullscreen` | `bool` | `false` | Controls the fullscreen flag passed into `Screen.SetResolution`. |

### Helper Methods
- `Vector2 GetResolutionVector2()` – converts the nested `ResolutionData` into a `Vector2`, convenient
  for feeding into `OptionsMenuElement.Init`.

Add new properties (e.g., `MotionBlur`, `Brightness`, `HDR`) as needed. Remember to:
1. Extend `SettingValues`.
2. Update `OptionSettings.SaveOptionSettings()` to persist the new values.
3. Register corresponding UI callbacks.

---

## AudioValues
| Member | Type | Default | Description |
| --- | --- | --- | --- |
| `MasterVolume` | `float` | `0f` | Mixer value in decibels applied to `MASTER`. |
| `MusicVolume` | `float` | `0f` | Mixer value applied to `MUSIC`. |
| `SfxVolume` | `float` | `0f` | Mixer value applied to `SFX`. |

Values are stored in decibels because Unity’s `AudioMixer.SetFloat` API expects that format. Use
`AudioSettings.PercentToDecibels` / `DecibelsToPercent` when translating to/from sliders.

---

## SettingValues
| Member | Type | Description |
| --- | --- | --- |
| `videoValues` | `VideoValues` | All video-related settings. |
| `audioValues` | `AudioValues` | All audio-related settings. |

`SettingValues` is the top-level object written by `SaveSettings.Save` and returned by
`SaveSettings.Load`. When you introduce new categories (Gameplay, Controls, etc.), embed the new DTOs
here so the persistence layer stays centralized.
