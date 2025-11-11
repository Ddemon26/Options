# Extending the Menu

The `SettingsSectionElement` base class handles headers, containers, and back buttons. To add a
Gameplay pane:

1. Create a new `GameplaySettingsElement : SettingsSectionElement` and add your controls via `AddSetting`.
2. Add an entry to `OptionType` and update `OptionsMenuElement.HandleMenuSelection` to toggle visibility.
3. Extend `OptionElementComponents` with the extra UI references so the logic layer can access them.
4. Update `VideoValues`/`AudioValues` or introduce a new data model to store the settings.
5. Hook into `OptionSettings` to create the matching runtime helper that subscribes to UI events.

Because the UI is defined entirely in UXML/USS, you can also:

- Swap the default layout for a tabbed interface.
- Embed the audio sliders inside another screen by instantiating `AudioSettingsElement` directly.
- Bind tooltips, icons, or validation states using standard UI Toolkit APIs.
