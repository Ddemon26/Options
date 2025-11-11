# Troubleshooting

| Symptom | Cause | Fix |
| --- | --- | --- |
| `OptionsMenu` disables itself on play | `UIDocument` reference missing | Assign the document or add the component to the same GameObject so `GetComponent<UIDocument>()` succeeds. |
| Sliders do nothing | Audio mixer not assigned or parameter names differ | Drag in an `AudioMixer` and rename the constants or exposed parameters so they match. |
| Accept button does nothing | `VolumeProfile` missing `DepthOfField` or no resolution selected | Assign a profile with the override and ensure `SupportedResolutions` contains the desired entry. |
| Values reset every run | No save data written (Play Mode domain reload) | Switch to JSON saves or disable domain reload when testing repeated runs. |
| UI buttons never show | `OptionsMenu.OpenOptionMenu()` never called | Wire the method into your pause menu, or set the default `OptionType` to `Choices` during init for debugging. |

Still stuck? Check the console for logs from `OptionsMenu`, `AudioSettings`, or `VideoSettings`.
