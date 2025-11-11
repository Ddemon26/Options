# Persistence API Reference

This page covers the classes responsible for saving and loading settings data.

## SaveSettings
- **Namespace:** `TCS.Options`
- **Assembly:** `Scripts/SaveSettings.cs`

`SaveSettings` abstracts how `SettingValues` are persisted. Swap the `SaveType` enum to choose the
backend.

### Serialized Fields
| Field | Type | Default | Description |
| --- | --- | --- | --- |
| `m_saveType` | `SaveType` | `SaveType.PlayerPrefs` | Selects the persistence backend. |

### Properties
| Property | Type | Description |
| --- | --- | --- |
| `string SavePath` | `string` | Returns `<Application.persistentDataPath>/settings` (used for JSON mode). |

### Methods
| Method | Description |
| --- | --- |
| `void Save(SettingValues values)` | Dispatches to the active backend to persist the provided values. |
| `SettingValues Load()` | Reads settings using the configured backend; falls back to defaults if unavailable. |

#### PlayerPrefs Backend
- Stores mixer decibel values as floats.
- Stores resolution width/height as ints and toggles as 0/1 integers.
- Calls `PlayerPrefs.Save()` to flush to disk.

#### JSON Backend
- Writes to `SavePath + ".json"` using `JsonUtility.ToJson(values, true)`.
- Creates the file if missing; returns new `SettingValues` when no file exists.

### Usage Example
```csharp
var saveSettings = new SaveSettings { m_saveType = SaveType.JsonFile };
saveSettings.Save(currentValues);
var loaded = saveSettings.Load();
```

---

## SaveType (enum)
- **Namespace:** `TCS.Options`
- **Defined In:** `Scripts/OptionSettings.cs`

| Value | Description |
| --- | --- |
| `PlayerPrefs` | Default mode. Uses Unity's `PlayerPrefs` API. |
| `JsonFile` | Writes a JSON file to `Application.persistentDataPath`. |
| *(commented placeholder)* | `TextFile` is hinted but not implemented; extend the enum and `Save/Load` to add more modes. |

When you create a new backend:
1. Extend `SaveType` with a new value.
2. Add cases to `Save()` and `Load()` that call your implementation.
3. Consider exposing configuration fields (paths, cloud keys, etc.) via serialized fields on
   `SaveSettings`.
