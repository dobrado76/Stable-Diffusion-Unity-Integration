# Architecture

This file explains how the pieces interact so support answers stay consistent. It is not an invitation to treat private classes as a stable API.

## Components

```text
SDSettings (ScriptableObject asset)
        |
        v
StableDiffusionConfiguration  -- ListModels / SetModelAsync
        ^
        | FindAnyObjectByType
        |
StableDiffusionGenerator (base)
   |-- StableDiffusionText2Image        txt2img  -> SDImages   -> Image or RawImage
   |-- StableDiffusionImage2Image       img2img  -> SDImages   -> Image or RawImage
   |-- StableDiffusionText2Material     txt2img  -> SDMaterials -> new Material
   |-- StableDiffusionImage2Material    img2img  -> SDMaterials -> new Material
```

Custom editors only add buttons and popups. They are not in the player script set. `InspectorRefresh` is an editor static class. Its event is unused.

JSON types (`SDParamsInTxt2Img`, `SDResponseTxt2Img`, `SDOption`, `SDProgress`, and the image-to-image equivalents) are private classes in `SDSettings.cs`. They exist to shape HTTP bodies. They are not components.

`Newtonsoft.Json` serializes those classes. Unity's `JsonUtility` is not used. `JsonUtility` would not have been a drop-in replacement for the nested response shapes the code expects, but that is why the DLL is in `Assets/Plugins`.

## Request lifecycle

For the three `UnityWebRequest` generators:

1. `Generate()` rejects re-entry and empty prompts (and a missing init texture on UI image-to-image).
2. `SetupFolders()` resolves the directory, deletes the previous PNG, and remembers the full path in a private field.
3. `SetModelAsync(modelNames[selectedModel])` runs. Failures are logged. The caller does not inspect a status code.
4. A POST body is built and `SendWebRequest()` is started without yielding on that call directly.
5. A loop waits on `request.isDone`, polling progress every 0.5 seconds.
6. The body is parsed, `images[0]` is written, a texture is built with `LoadImage`, and the scene object is updated.
7. `generatedSeed` is set from `info` when possible.
8. The progress bar is cleared and `generating` is set false.

`StableDiffusionImage2Material` replaces steps 4–5 with a blocking-capable `HttpWebRequest` and `GetResponseAsync`, and it never attaches Basic auth.

`SetModelAsync` itself may block the Editor on the `HttpWebRequest` fallback.

## How objects are found

`FindAnyObjectByType<StableDiffusionConfiguration>()` is called from property getters when the static field `StableDiffusionGenerator.sdc` compares equal to null, and again before generation under the same check. Unity's overloaded `==` treats a destroyed object as null, so deleting the configuration causes the next lookup to search again. A disabled object is not null. If it was cached while active, later calls keep using it. The parameterless find returns an active loaded object, so a configuration that is already disabled is not found, and the getter then throws when it reads `samplers`. A domain reload clears the static. The code does not subscribe to scene changes.

Child mesh search (`FindInChildrenAll`) walks the transform's children, skips inactive GameObjects when `active` is true (the default), collects components of type `T`, and recurses. It does not include the GameObject it was called on. `GetMeshRenderer` checks the host first, then the children.

## Persistence model

- ScriptableObject: server profile.
- Scene component fields: prompts and parameters, including hidden `selectedSampler` and `selectedModel`.
- Disk PNG: the only file the generators create.
- In-memory `Material` and normal-map `Texture2D`: referenced by the renderer. Edit-mode success marks the scene dirty and, for materials, calls `EditorUtility.SetDirty` on the textures. There is no `AssetDatabase.CreateAsset`.
- `generating` and the cached configuration pointer: not serialized.

`WriteImageFile` wraps the write in `AssetDatabase.StartAssetEditing` / `StopAssetEditing` and `SaveAssets` in the Editor. UI and text-to-material paths do this. Image-to-material writes inside the coroutine with `WriteAsync` instead, also bracketed by asset editing in the Editor.

## What runs in Edit Mode

`StableDiffusionConfiguration` is `[ExecuteInEditMode]`. Generators are `[ExecuteAlways]`. That is what allows coroutines started from an Inspector button to keep running while the Editor is not in Play Mode. `Update` on the generators also runs in Edit Mode, which is how the dimension clamp, guid assignment, and material slider updates happen without entering Play Mode.

## External boundary

This process never loads a neural network. It never reads WebUI's `models` directory from disk. It only speaks HTTP to the configured origin. Output files that WebUI writes into its own folders are ignored. Conversely, WebUI does not know about Unity materials; applying pixels to a renderer is entirely local.

The normal-map function is local CPU code over the colour texture. It is not a WebUI script and not a second network.

## Editor UI map

| Control | Script |
|---|---|
| **List Models** | `StableDiffusionConfigurationEditor` |
| **Assets/Create/SDSettings** | `SDSettingsEditor` |
| **Sampler**, **Model**, **Generate** | One editor each for text-to-image, UI image-to-image, and text-to-material |
| Read-only guid | `ReadOnlyDrawer` |
| Help box on UI image-to-image | `StableDiffusionImage2ImageEditor` |

`selectedSampler` and `selectedModel` are assigned on the component instance inside `OnInspectorGUI`. The editors call `serializedObject.ApplyModifiedProperties()` but do not route those two popup values through `SerializedProperty`. They are public fields, and the demo scenes show them serialized, so saving the scene does store them. The editors do not call `Undo.RecordObject`.

## Assemblies

There is no asmdef. Runtime-folder scripts compile into `Assembly-CSharp`. `Scripts/Editor` compiles into `Assembly-CSharp-Editor`. `ReadOnlyAttribute.cs` is on the runtime side and references `UnityEditor`, which is the player-build problem described in the development notes.
