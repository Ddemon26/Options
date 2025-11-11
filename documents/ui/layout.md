# UI Layout

`OptionsMenuElement` is a custom `VisualElement` annotated with `[UxmlElement]`, which means it
shows up inside UI Builder. Internally it contains:

- **Button Container** – hosts the `Audio Settings`, `Video Settings`, and `Back` buttons.
- **AudioSettingsElement** – derives from `SettingsSectionElement` and exposes three sliders.
- **VideoSettingsElement** – same base class, but exposes dropdown/toggles plus an Accept button.
- **Return Buttons** – each pane includes its own `Back` button that routes through
  `HandleReturnBackPressed` on the menu element.

Display logic is handled by `OptionType` states. The element toggles `DisplayStyle` on each child so only
the active pane is visible. Feel free to add more panes by extending `OptionType` and mirroring the pattern.

The default UXML hierarchy is minimal:

```
OptionsMenuElement (root)
 ├── ButtonContainer
 └── ElementContainer
     ├── AudioSettingsElement
     └── VideoSettingsElement
```

Everything is classed using string constants from `GeneralSettingsNames`, keeping USS selectors predictable.
