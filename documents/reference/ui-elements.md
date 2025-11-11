# UI Elements API Reference

This page documents the UI Toolkit-specific types defined in the `Scripts/AudioSettingsElement.cs`
and `Scripts/OptionsMenuElement.cs` files. Use it when customizing the markup or extending the UI.

---

## SettingsSectionElement
- **Namespace:** `TCS.Options`
- **Base Type:** `UnityEngine.UIElements.VisualElement`
- **Defined In:** `Scripts/AudioSettingsElement.cs`

`SettingsSectionElement` is an abstract helper that provides a consistent header, settings container,
and optional Back button for any settings pane.

### Constructor
```csharp
protected SettingsSectionElement(string sectionTitle, string ussRootClass)
```
| Parameter | Description |
| --- | --- |
| `sectionTitle` | Text displayed in the header label. |
| `ussRootClass` | Root USS class added via `AddToClassList` for easy styling. |

### Protected Members
| Member | Type | Description |
| --- | --- | --- |
| `VisualElement HeaderContainer` | Layout node that wraps the header label. |
| `Label HeaderLabel` | Displays the title passed to the constructor. |
| `VisualElement SettingsContainer` | Parent node where child controls are appended. |
| `Button BackButton` | Created via `AddBackButton`; exposed for click registration. |
| `void AddBackButton()` | Adds a "Back" button and container at the bottom of the section. |
| `void AddSetting<T>(T element, string className)` | Adds a control to the settings container and assigns a USS class. |

Derive from this class to get consistent markup and utility helpers for additional sections
(Gameplay, Controls, etc.).

---

## AudioSettingsElement
- **Namespace:** `TCS.Options`
- **Base Type:** `SettingsSectionElement`
- **Defined In:** `Scripts/AudioSettingsElement.cs`
- **UXML Attribute:** `[UxmlElement]`

`AudioSettingsElement` renders three sliders with labels and input fields. It is instantiated inside
`OptionsMenuElement` but can also be reused elsewhere in your UI Toolkit hierarchy.

### Public Properties
| Property | Type | Description |
| --- | --- | --- |
| `Slider MasterVolumeSlider` | `Slider` | Controls overall volume (0–100). Input field enabled. |
| `Slider MusicVolumeSlider` | `Slider` | Controls music channel volume (0–100). |
| `Slider sfxVolumeSlider` | `Slider` | Controls SFX channel volume (0–100). |
| `Button BackButton` | `Button` | Inherited field created via `AddBackButton()`. |

### Behavior
- Each slider is initialized with defaults (`75`, `60`, `80` respectively) and shares the
  `options-settings__slider` class.
- The constructor applies the shared label class, so labels match the rest of the menu.
- Sliders expose `showInputField = true`, letting players type exact values.

---

## VideoSettingsElement
- **Namespace:** `TCS.Options`
- **Base Type:** `SettingsSectionElement`
- **Defined In:** `Scripts/AudioSettingsElement.cs`
- **UXML Attribute:** `[UxmlElement]`

`VideoSettingsElement` provides dropdown/toggle controls plus an Accept button so resolution changes
are opt-in.

### Public Properties
| Property | Type | Description |
| --- | --- | --- |
| `DropdownField ResolutionDropdown` | `DropdownField` | Displays supported resolutions (`WxH`). |
| `Toggle FullscreenToggle` | `Toggle` | Toggles fullscreen/windowed state. |
| `Toggle VSyncToggle` | `Toggle` | Controls VSync flag. |
| `Toggle DepthOfFieldToggle` | `Toggle` | Enables URP Depth of Field override. |
| `Button AcceptChangesButton` | `Button` | Applies pending video settings. |
| `Button BackButton` | `Button` | Inherited from `SettingsSectionElement`. |

### Methods
| Method | Description |
| --- | --- |
| `void Init(Vector2[] resolutions, Vector2 currentResolution)` | Populates dropdown choices and selects the current resolution string if present. |

Labels are automatically assigned the shared `options-settings__label` class. The Accept button adds
an extra USS class `accept-changes-button` for styling.

---

## OptionType (enum)
- **Namespace:** `TCS.Options`
- **Defined In:** `Scripts/OptionsMenuElement.cs`

| Value | Description |
| --- | --- |
| `None` | Hides the entire options UI (default state). |
| `Choices` | Shows the top-level button stack (Audio / Video / Back). |
| `Audio` | Displays `AudioSettingsElement`. |
| `Video` | Displays `VideoSettingsElement`. |

`OptionsMenuElement.CurrentOptionType` uses this enum to toggle visibility. Extend it with new values
when you add more panes.

---

## GeneralSettingsNames
- **Namespace:** `TCS.Options`
- **Defined In:** `Scripts/OptionsMenuElement.cs`

Utility class containing the USS class/element name constants used throughout the UI Toolkit tree.
Reference these when styling or querying elements so you avoid typo-prone string literals.

| Constant | Value | Usage |
| --- | --- | --- |
| `OptionsName` | `"options-settings"` | Root class for `OptionsMenuElement`. |
| `HeaderContainerName` | `OptionsName + "__header-container"` | Wrapper for section headers. |
| `HeaderLabelName` | `OptionsName + "__header-label"` | Label class for headers. |
| `SettingsContainerName` | `OptionsName + "__settings-container"` | Parent for sliders/toggles/buttons. |
| `BackButtonContainerName` | `OptionsName + "__back-button-container"` | Container around the Back button. |
| `BackButtonName` | `OptionsName + "__back-button"` | Back button class. |
| `ElementContainerName` | `OptionsName + "__element-container"` | Holds Audio/Video panes. |
| `SettingsLabelName` | `OptionsName + "__label"` | Applied to all settings labels. |
| `SettingsButtonName` | `OptionsName + "__button"` | Shared button styling. |
| `SettingButtonName` | `OptionsName + "__setting-button"` | Additional button class for menu buttons. |
| `SettingsSliderName` | `OptionsName + "__slider"` | Slider styling hook. |
| `SettingsToggleName` | `OptionsName + "__toggle"` | Toggle styling hook. |
| `SettingsDropdownName` | `OptionsName + "__dropdown"` | Dropdown styling hook. |

Keep these constants synchronized with your USS files so code references continue to match the styles.
