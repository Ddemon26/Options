# Options Menu for Unity

`TCS.Options` is a UI Toolkit driven settings menu that drops into any Unity 2021.3+ project.
It exposes audio sliders, video toggles, and persistence helpers so players always land in
the same experience every time they boot your game.

The repo ships with:

- A ready-made `OptionsMenu` MonoBehaviour that owns the UI Document and dispatches changes.
- A composable `OptionsMenuElement` UI Toolkit control that flips between Audio/Video panes.
- Audio helpers that translate percent-based sliders to Audio Mixer dB values in real time.
- Video helpers that keep resolution, fullscreen, v-sync, and depth-of-field settings in sync
  with Unity's `Screen`, `QualitySettings`, and URP `VolumeProfile` APIs.
- Persistence providers (`PlayerPrefs` and JSON files) so you can decide where to store data.

Use this documentation as the living reference for wiring the menu into scenes,
customizing USS styling, and extending the system with extra settings categories.
