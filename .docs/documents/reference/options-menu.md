# OptionsMenu API

`OptionsMenu` is the entry point MonoBehaviour that glues the UI, audio/video helpers, and persistence together.

## Serialized Fields
- `UIDocument m_uiDocument` – host document that contains `OptionsMenuElement` as its root.
- `AudioMixer m_audioMixer` – optional but required for live audio control.
- `VolumeProfile m_volumeProfile` – optional; needed to toggle URP Depth of Field.

## Lifecycle
1. **Awake**
   - Enforces a singleton via `Instance`.
   - Detaches from any parent and survives scene loads (`DontDestroyOnLoad`).
   - Caches the `OptionsMenuElement` from the assigned `UIDocument`.
   - Builds an `OptionElementComponents` struct that stores references to every control.
   - Creates `SettingValues` and `OptionSettings` instances.

2. **Start**
   - Calls `OptionsSettings.Init()` to sync UI with saved data.
   - Passes the saved resolution vector to `OptionsMenuElement.Init()` so the dropdown selects the correct entry.

3. **OnDestroy**
   - Disposes the UI element and option settings, which also triggers persistence.

## Public API
```csharp
public static OptionsMenu Instance { get; }
public void OpenOptionMenu();
```

Call `OptionsMenu.Instance.OpenOptionMenu()` whenever you need to display the options UI. The method simply
sets `CurrentOptionType = OptionType.Choices`, which makes the button stack visible.

## Error Handling
- If no `UIDocument` is assigned, the script logs an error and disables itself.
- If no `VolumeProfile` is set while Depth of Field is enabled, you will see an error but the rest of the menu keeps working.
