# Data Models

All serialized values live in plain C# classes so they can be saved via PlayerPrefs, JSON, or any
other backend you add later.

## ResolutionData
```csharp
[Serializable]
public class ResolutionData {
    public int Width { get; set; }
    public int Height { get; set; }
}
```
Used inside `VideoValues` to describe the currently selected resolution. `GetResolutionVector2()` on
`VideoValues` converts the width/height pair into a `Vector2` for UI use.

## VideoValues
```csharp
[Serializable]
public class VideoValues {
    public ResolutionData Resolution { get; set; } = new();
    public bool VSync { get; set; }
    public bool DepthOfField { get; set; }
    public bool Fullscreen { get; set; }
}
```
Add more properties (e.g., `MotionBlur`, `Brightness`) as needed. Make sure to update
`OptionSettings.SaveOptionSettings()` and any persistence code.

## AudioValues
```csharp
[Serializable]
public class AudioValues {
    public float MasterVolume { get; set; }
    public float MusicVolume { get; set; }
    public float SfxVolume { get; set; }
}
```
Values are stored in decibels to match the AudioMixer API.

## SettingValues
```csharp
[Serializable]
public class SettingValues {
    public VideoValues videoValues { get; set; } = new();
    public AudioValues audioValues { get; set; } = new();
}
```
This is the object that gets saved/loaded. When you introduce new categories, extend
`SettingValues` (or nest new structs) so the persistence layer can handle them consistently.
