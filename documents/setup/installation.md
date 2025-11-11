# Installation

## 1. Clone or Submodule
Add the `Options` repository to your project (e.g. `Packages/tcs.options`). You can clone it
next to your project or add it as a Git submodule so Unity picks it up as a package.

```bash
git submodule add https://github.com/<your-org>/Options.git Packages/tcs.options
```

## 2. Add the UXML & USS
1. Drag `Uxml/OptionsMenu.uxml` into a `UIDocument`.
2. Add the USS files from `Uss/` to the document's style list.
3. Optionally tweak class names in UI Builder to match your art direction.

## 3. Drop the Prefab / Script
- Add the `OptionsMenu` MonoBehaviour to a GameObject that also owns the `UIDocument`.
- Assign the `AudioMixer` and (optionally) a URP `VolumeProfile` in the inspector.

## 4. Wire Input
Call `OptionsMenu.Instance.OpenOptionMenu()` from your pause screen, settings button, or input action.

You should now be able to hit Play and toggle the Audio/Video panes.
