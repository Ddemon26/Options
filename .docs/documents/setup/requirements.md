# Requirements

| Component | Minimum | Notes |
| --- | --- | --- |
| Unity Editor | 2021.3 LTS | Tested with UI Toolkit runtime support. Works on newer versions too. |
| Render Pipeline | Built-in or URP | Depth of Field toggle requires URP with a `VolumeProfile`. |
| Audio | Unity `AudioMixer` asset | Needed for `MASTER`, `MUSIC`, `SFX` exposed parameters. |
| Input | Any | Use your preferred system to call `OptionsMenu.OpenOptionMenu()`. |
| Platforms | Desktop (Win/macOS/Linux) | Fullscreen + resolution APIs are desktop focused. |

Optional:

- If you want JSON saves, ensure the target platform allows writing to
  `Application.persistentDataPath`.
- To theme the menu, include the provided USS files or author your own.
