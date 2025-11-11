# OptionSettings API

`OptionSettings` orchestrates the runtime helpers and persistence. It is created inside `OptionsMenu`
and is not a MonoBehaviour.

## Constructor
```csharp
public OptionSettings(OptionElementComponents components, SettingValues values)
```
- Loads persisted values through `SaveSettings.Load()`.
- Creates `AudioSettings`/`VideoSettings` helpers, passing in the UI references collected earlier.

## Methods
- `Init()` – pushes saved values into the UI and applies any video settings that must be active immediately.
- `Dispose()` – serializes the current UI state back to disk by calling `SaveOptionSettings()` and disposes the helpers.

## Internals
- `SaveSettings` is private and encapsulated so consumers only interact with `OptionSettings`.
- `SaveOptionSettings()` pulls the latest values from `AudioSettings.GetCurrentDecibelLevels()` and
  `VideoSettings.GetCurrentVideoValues()`.

### AudioSettings Helper
- Registers slider callbacks lazily to avoid double-registration when re-initialized.
- Converts slider percentages to decibels using `PercentToDecibels`.
- Provides utility methods that you can reuse when writing custom audio UI.

### VideoSettings Helper
- Stores callbacks for each control so they can be unregistered in `Dispose()`.
- Uses the Accept button to apply pending values, preventing sudden resolution switches while navigating the UI.
- If a `VolumeProfile` is missing or does not contain `DepthOfField`, the toggle safely does nothing.

Use `OptionSettings` as the integration point for any new settings category you add. Keep the UI logic
(UI Toolkit) and system logic (Unity APIs) separated by following the same pattern.
