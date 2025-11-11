# Video System

`VideoSettings` reacts to dropdown/toggle events and updates a shared `VideoValues` instance. When the
player presses **Accept Changes**, it applies the pending values.

### Resolutions
- The dropdown is pre-populated with `OptionElementComponents.SupportedResolutions`.
- `SetResolution` parses the "WIDTH x HEIGHT" string into a `ResolutionData` object.
- At apply time, the code calls `Screen.SetResolution` using either fullscreen windowed or windowed mode.

### VSync & Fullscreen
- `QualitySettings.vSyncCount` is set to `1` when VSync is enabled, otherwise `0`.
- `VideoValues.Fullscreen` determines whether `Screen.SetResolution` uses `FullScreenMode.FullScreenWindow` or `FullScreenMode.Windowed`.

### Depth of Field
- If a `VolumeProfile` is assigned and it contains a `DepthOfField` override, the script toggles the
  effect's `active` flag to mirror the toggle state.

### Extending
Add more toggles/sliders with the same pattern:

```csharp
var motionBlurToggle = new Toggle("Motion Blur");
m_motionBlurCallback = CreateToggleCallback(value => VideoValues.MotionBlur = value);
motionBlurToggle.RegisterValueChangedCallback(m_motionBlurCallback);
```

Remember to dispose the callbacks and to persist the new value through `SettingValues` and `SaveSettings`.
