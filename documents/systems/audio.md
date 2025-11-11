# Audio System

`AudioSettings` is created inside `OptionSettings` and owns three things:

1. The `AudioMixer` reference provided via `OptionElementComponents`.
2. The UI Toolkit sliders for Master, Music, and SFX.
3. Saved values loaded from `SaveSettings` (stored in decibels).

### Initialization Flow
- Saved dB values are converted back to percentages with `DecibelsToPercent` and assigned to sliders.
- Each slider registers a callback produced by `CreateVolumeCallback`, which calls `ApplyVolume`.
- `ApplyVolume` clamps the slider percent and feeds it into `PercentToDecibels` before setting the
  mixer parameter (`MASTER`, `MUSIC`, `SFX`).

### Getting Current Values
Call `GetCurrentDecibelLevels()` to retrieve whatever the mixer is currently using. This is what
`OptionSettings` serializes when the menu is destroyed.

### Customization Ideas
- Rename the mixer parameter constants to match your AudioMixer groups.
- Add mute toggles by appending `Toggle` controls and writing to the mixer or slider value.
- Use `AudioSettings.PercentToDecibels` anywhere else in your game you need the same conversion.
