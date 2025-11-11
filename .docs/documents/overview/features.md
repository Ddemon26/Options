# Feature Highlights

## Built for UI Toolkit
Everything ships as UXML/USS friendly elements so you can drop the menu into any
`UIDocument`. Buttons, sliders, toggles, and dropdowns are regular `VisualElement`
controls so they play nicely with the UI Builder.

## Audio Mixer Integration
`AudioSettings` registers slider callbacks that convert percentages into decibels and
writes directly to the `AudioMixer` parameters (`MASTER`, `MUSIC`, `SFX`). You get
instant feedback plus serialization of the dB values for persistence.

## Video Stack Awareness
The `VideoSettings` helper keeps `Screen` resolution, fullscreen/windowed state,
`QualitySettings.vSyncCount`, and URP `DepthOfField` overrides in lockstep with
what the player selects.

## Safe Persistence Layer
`SaveSettings` currently supports PlayerPrefs and JSON files. The API is structured so
you can swap in different storage backends without touching the UI or setting logic.

## Extendable Data Models
`SettingValues`, `AudioValues`, and `VideoValues` are plain serializable classes.
Adding new toggles or sliders only requires extending these models and the matching UI element.

## Prefab-Friendly
`OptionsMenu` detaches itself from the scene hierarchy, survives scene loads via
`DontDestroyOnLoad`, and exposes a `OpenOptionMenu` helper that you can wire into any input flow.
