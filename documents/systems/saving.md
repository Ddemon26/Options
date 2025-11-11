# Saving & Loading

`SaveSettings` decides where to persist `SettingValues`. The default is `PlayerPrefs`, but a JSON
file option is also provided for platforms where file access is preferred.

## PlayerPrefs Flow
1. `Save()` writes floats for the mixer dB values and ints/bools for video settings.
2. `Load()` pulls the stored data, falling back to sensible defaults (75/60/80 volumes, 1920x1080 resolution).
3. Everything is wrapped in DTOs so that nothing touches the UI layer directly.

## JSON Flow
1. When `m_saveType` is set to `JsonFile`, `Save()` writes a pretty-printed JSON file to
   `<persistentDataPath>/settings.json`.
2. `Load()` deserializes the file if it exists, otherwise it returns a new `SettingValues` instance.

## Switching Providers
Expose `SaveSettings.m_saveType` in the inspector or set it from code:

```csharp
[SerializeField] SaveSettings saveSettings;

void Awake() {
    saveSettings.m_saveType = SaveType.JsonFile;
}
```

You can add more providers (cloud saves, Steam, etc.) by extending the `SaveType` enum and
implementing the corresponding `Save/Load` branches.
