# Options Menu for Unity

`TCS.Options` is a drop-in UI Toolkit settings menu for Unity 2021.3+ projects. It bundles the
UXML/USS, MonoBehaviours, and persistence helpers you need to expose audio sliders, video toggles,
and quality-of-life options without rebuilding common plumbing every project.

## Installation

### Unity Package Manager (recommended)
1. Open **Window › Package Manager**.
2. Click the ➕ icon → **Add package from git URL...**.
3. Enter the repository URL, for example:
   ```
   https://github.com/Ddemon26/Options.git
   ```
4. Unity pulls the package into `Packages/tcs.options` and imports the scripts, UXML, and USS files.

### Git submodule / manual clone
```bash
git submodule add https://github.com/Ddemon26/Options.git Packages/tcs.options
```
Unity will recognize the folder as a local package the next time the editor refreshes assets.

## How to Use

1. Drag `Uxml/OptionsMenu.uxml` into a `UIDocument`.
2. Add `Uss/OptionsMenu.uss`, `AudioSettings.uss`, and `VideoSettings.uss` to the document styles.
3. Attach the `OptionsMenu` MonoBehaviour to the same GameObject, assign your `AudioMixer`, and
   optionally a URP `VolumeProfile`.
4. Call `OptionsMenu.Instance.OpenOptionMenu()` from a pause menu, button, or input action.

Refer to `.docs/documents/setup/quick-start.md` if you need a step-by-step walkthrough.

## License

Distributed under the terms of the repository's `LICENSE`. Review the file for full details.
