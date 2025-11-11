# Quick Start

Follow these steps to get a functional menu in under five minutes:

1. **Create a Settings Scene Object**
   - Add an empty GameObject named `OptionsMenu`.
   - Attach `UIDocument` and point it to `OptionsMenu.uxml`.
   - Attach the `OptionsMenu` MonoBehaviour script.

2. **Assign Dependencies**
   - Drag in your `AudioMixer` asset and expose three float parameters: `MASTER`, `MUSIC`, `SFX`.
   - If you use URP, assign the shared `VolumeProfile` that contains a `DepthOfField` override.

3. **Hook Up Input**
   ```csharp
   using TCS.Options;

   public class PauseMenu : MonoBehaviour {
       void Update() {
           if (Input.GetKeyDown(KeyCode.F10)) {
               OptionsMenu.Instance.OpenOptionMenu();
           }
       }
   }
   ```

4. **Test in Play Mode**
   - Open the menu and move the sliders/toggles.
   - Click **Accept Changes** to push video options.
   - Exit Play Mode and confirm the values persist between runs.

That is all you need for the default experience. Continue to the UI pages for customization tips.
