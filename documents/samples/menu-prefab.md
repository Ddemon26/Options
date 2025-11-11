# Sample: Options Menu Prefab

Want to keep the menu reusable across scenes? Turn it into a prefab with these steps:

1. **Create Prefab**
   - Build the hierarchy inside a temporary scene, assign the `UIDocument` + `OptionsMenu` component.
   - Drag it into your project to create a prefab asset (e.g., `Prefabs/OptionsMenu.prefab`).

2. **Scene Bootstrap**
   ```csharp
   public class OptionsBootstrap : MonoBehaviour {
       [SerializeField] GameObject optionsMenuPrefab;

       void Awake() {
           if (OptionsMenu.Instance == null) {
               Instantiate(optionsMenuPrefab);
           }
       }
   }
   ```

3. **Input Hook**
   - From any UI button: assign `OptionsMenu.Instance.OpenOptionMenu` via the event binding system.
   - From code: see the Quick Start sample.

Because `OptionsMenu` calls `DontDestroyOnLoad`, you only need to spawn it once. Later scenes can
assume it already exists and simply open it when required.
