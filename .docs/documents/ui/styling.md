# Styling & USS

All controls share the `options-settings` prefix so you can theme them without resorting to
brittle element names. Key classes:

| Class | Applies To | Source |
| --- | --- | --- |
| `options-settings` | Root element | `OptionsMenuElement` constructor |
| `options-settings__button` / `__setting-button` | Primary buttons | `GeneralSettingsNames` |
| `options-settings__slider` | Sliders inside Audio pane | `AudioSettingsElement` |
| `options-settings__toggle` | Toggles inside Video pane | `VideoSettingsElement` |
| `video-settings` / `audio-settings` | Section roots | Derived from `SettingsSectionElement` |

The repo ships with three USS files located in `Uss/`:

1. `OptionsMenu.uss` – shared styles for layout and transitions.
2. `AudioSettings.uss` – slider widths, header spacing, input field tweaks.
3. `VideoSettings.uss` – dropdown widths, Accept button styling, toggle spacing.

### Tips
- Use UI Builder's **Class List** to preview the shipped theme, then clone the USS file into your
  project and adjust colors/typography.
- Keep the structural classes (e.g., `options-settings__element-container`) intact so the
  runtime code can still query the elements.
- Add media queries to adjust layout when embedding in pause screens or overlay menus.
