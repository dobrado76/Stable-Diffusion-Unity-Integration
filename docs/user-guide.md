# User guide

Everything here is driven from the Unity Inspector while the Editor is open. Buttons exist only where a custom editor draws them.

## Where the pieces live

Scripts: `Assets/Stable-Diffusion-Unity-Integration/Scripts/`

Custom inspectors: `Assets/Stable-Diffusion-Unity-Integration/Scripts/Editor/`

| Class | How you add it | Inspector extras |
|---|---|---|
| `StableDiffusionConfiguration` | Add Component, search the class name | Button **List Models** |
| `SDSettings` | **Assets > Create > SDSettings** | Not a component |
| `StableDiffusionText2Image` | Add Component on a UI object | Popups **Sampler** and **Model**, button **Generate** |
| `StableDiffusionImage2Image` | Add Component on a UI object | Help box `Input textures should be uncompressed and readable`, then the same popups and **Generate** |
| `StableDiffusionText2Material` | Add Component on a mesh object | Same popups and **Generate** |
| `StableDiffusionImage2Material` | Add Component on a mesh object | Default inspector only. No popups and no **Generate** button |
| `StableDiffusionGenerator` | Add Component will offer the base class | No generation UI. Do not add it on its own |

There is no component named `StableDiffusionImage` or `StableDiffusionMaterial` in the current source. Those names were renamed in version 1.2.4, as recorded in the README changelog.

None of the generator scripts use `[RequireComponent]` or `[DisallowMultipleComponent]`. A text-to-image component can sit on an object with no `Image`, and two generators can sit on one object. Each has its own guid and its own PNG. The last one to finish is the one left on the shared `Image` or `MeshRenderer`.

The base class is not abstract, so Unity lists it. It only supplies the progress-bar helpers and a child search used by the material components.

## Scene setup that every generator needs

1. An active `StableDiffusionConfiguration` in a loaded scene.
2. Its **Settings** field pointing at an `SDSettings` asset.
3. **List Models** completed at least once so **Model Names** is a non-null array.
4. A **Model** index inside that array. Select it again after every refresh of the list.
5. A non-empty **Prompt**.
6. The generator's GameObject must be active and the component checkbox must be enabled. `Generate()` calls `StartCoroutine`. Unity does not start that coroutine on an inactive object or a disabled behaviour, and this project does not catch that failure. `generating` is set only after the coroutine begins, so a refused start leaves the flag false. The configuration object must also be active. The parameterless `FindAnyObjectByType` returns an active object; it will not discover a disabled one. A configuration that was found while active and later disabled can stay in the static cache and still be used.

Missing configuration: opening the generator inspector throws `NullReferenceException` because the sampler popup reads `sdc.samplers` with no null check.

`Model Names` still null: the **Model** popup throws because `EditorGUILayout.Popup` receives a null array. Click **List Models** first, or set the array size on the configuration so it is an empty array rather than null. An empty array avoids the popup exception and then fails at generate time when the index is read.

## Text-to-image for UI

**Use it** when a Canvas element should display a new picture from a prompt: a `RawImage`, an `Image`, or a `Button` (a Button uses an `Image`).

**Where:** add `StableDiffusionText2Image` to that GameObject.

**Prerequisite:** the shared scene setup above. An `Image` or `RawImage` on the same GameObject is required only to see the result. Without either, the PNG is still written and the sprite is not assigned.

**Steps**

1. Enter **Prompt** and, if you want, **Negative Prompt**.
2. Set **Width** and **Height**. The Editor clamps them to 128–2048.
3. Set **Steps**, **Cfg Scale**, and **Seed**. Seed `-1` lets the server choose. The component default for steps on a newly added text-to-image component is 90, not the settings asset's 35.
4. Choose **Sampler** and **Model**.
5. Click **Generate**.

**What happens**

1. The component sets an internal running flag.
2. It deletes `{output}/SDImages/{guid}.png` if that file exists, then creates the folders.
3. It POSTs the selected `model_name` to the options API as `sd_model_checkpoint`. If that call fails, generation still continues with whatever checkpoint the server already had loaded.
4. It POSTs text-to-image JSON. `tiling` is always false on this component.
5. Every 0.5 seconds it polls the progress API and updates the Editor progress bar.
6. It decodes `images[0]` as raw base64 (not a `data:` URL), writes the PNG, and builds a `Texture2D` with `LoadImage`. If an `Image` is present, it creates a sprite whose pivot is `Vector2.zero` (bottom-left) and assigns that sprite. A `RawImage` gets the texture. Nothing else on the graphic changes: not the colour, not the material, not the Image type, not the RawImage UV rect, and not the RectTransform size. Width and height are the generated picture's pixel size, not the UI element's size.
7. It parses `info` for the seed when that field is a non-empty string. A null `info`, or an `info` string that does not match `SDParamsOutTxt2Img`, throws after the PNG is already written. The catch logs `Error processing response:` and leaves the file in place.
8. Outside Play Mode, `LoadIntoImage` refreshes the asset database, calls `EditorSceneManager.MarkAllScenesDirty()` (every loaded scene, not only the active one), and calls `EditorUtility.RequestScriptReload()`. That reload is requested even when the object has neither an `Image` nor a `RawImage`. The in-memory sprite is not an asset. The PNG is the copy that is still on disk after a domain reload. Whether Unity keeps the sprite across the reload the tool itself requests was not re-tested. The code then clears the progress bar; if the reload interrupts the coroutine first, the bar can stay up.

**Result:** one PNG on disk and, when a graphic is present, an updated sprite or texture. Further images in the response are ignored. The delete step removes the `.png` only, not a sibling `.meta`. What Unity's importer does with a leftover `.meta` was not re-tested.

**Undo:** there is no undo for the deleted or overwritten PNG. Duplicating the GameObject creates a new guid only after the duplicate's empty guid is replaced; duplicate the object and let `Update` assign a new guid, or copy the PNG yourself, before generating again if you need to keep the previous file.

**Limits:** no batch, no hi-res fix, no face restore. Dimensions outside 128–2048 do not stick in the Editor. Sizes WebUI dislikes (often anything not divisible by 8, depending on that WebUI build) fail on the server.

## Image-to-image for UI

**Use it** when an existing picture should be modified. The demo object `Image-2-RawImage` uses this component with `horse_input.png`.

**Where:** `StableDiffusionImage2Image` on a GameObject that has an `Image` or `RawImage` if you want the result displayed. The init image is the **Input Texture** field, not whatever the UI graphic currently shows.

**Prerequisite:** **Input Texture** assigned, and that texture's import settings have **Read/Write** enabled. The inspector shows `Input textures should be uncompressed and readable`. The code checks `isReadable` only. It does not read the compression setting. A readable texture proceeds to `EncodeToPNG` whether or not it is compressed. The horse sample is readable and its importer compression is none.

**Steps:** same as text-to-image, plus assign **Input Texture**, then **Generate**.

**If the texture is missing:** **Generate** returns without a Console message.

**If it is not readable:** the Console logs `Input Image {name} isn't readable. Go to texture import settings and tick the Read/Write box` and does not start the request.

**What is sent:** a base64 PNG of the input texture in `init_images`, plus prompt, negative prompt, steps, CFG, width, height, seed, `tiling` false, and sampler name. Denoising strength is the private default **0.75**. There is no slider. Resize mode stays 0. No mask is sent.

**Result:** same filing rules as text-to-image (`SDImages/{guid}.png`) and the same UI assignment. The init texture asset is not modified.

## Text-to-material for meshes

**Use it** to replace a mesh's material with a generated albedo, optional normal map, and metallic or smoothness values. Demo objects include `Floor`, `Walls`, `Ceiling`, `Table`, `Cube`, `Cylinder`, `Painting1`, and `Painting2`.

**Where:** `StableDiffusionText2Material` on the mesh, or on a parent when **Apply Recursively** is enabled.

**Prerequisite:** a `MeshRenderer` on this object, or on an active child when **Apply Recursively** is on. Inactive children are skipped. The search does not include the parent when looking at children; it starts at the children. If this object has a `MeshRenderer`, that one is used and children are not searched for the primary renderer. Children still receive the finished material when **Apply Recursively** is on.

**Steps**

1. Set the prompt and the numeric fields. On first add, width and height of `-1` cause `Awake` to copy the settings asset defaults (512, 512, 35 steps, CFG 7, seed `-1` on the included asset).
2. Leave **Tiling** on to ask WebUI for a tileable image (`tiling: true`). Turn it off for a picture that should not repeat, which is how the demo paintings are set.
3. Set **Tiling X** and **Tiling Y** for how many times the texture repeats on the mesh. The scale applied is negative, so the texture is flipped.
4. Set **Metallic**, **Smoothness**, **Generate Normal Map**, and **Normal Map Strength**.
5. Choose sampler and model. Click **Generate**.

**What happens after the PNG is written**

1. If no `MeshRenderer` is found, `LoadIntoMaterial` returns immediately. The PNG is already on disk. The scene is not marked dirty and scripts are not reloaded.
2. Otherwise the first `MeshRenderer` gets a **new** material. Previous colour, shader, and extra maps are not copied. The shader is `Standard`, or `Universal Render Pipeline/Lit` when **Use Universal Render Pipeline** is enabled on the settings asset. If `Shader.Find` fails, the Console logs `Shader setup wrong: Please check if you're project uses 'Standard' or 'Universal Render Pipeline'`.
3. `mainTexture` is the generated texture. The code never sets `wrapMode`. A `Texture2D` created in code defaults to repeat wrapping; a reimported PNG uses the importer's wrap mode instead.
4. If **Apply Recursively** is on, every child `MeshRenderer` has its `sharedMaterial` set to that same material. In the demo, `Cylinder` is a child of `Cube`, and `Cube` has **Apply Recursively** on. Generating `Cube` replaces `Cylinder`'s material even though `Cylinder` has its own text-to-material component and its own prompt.
5. If **Generate Normal Map** is on, a normal map is built in memory from a 3x3 neighbourhood of the albedo (the approach described by [UnityCoder/NormalMapFromTexture](https://github.com/unitycoder/NormalMapFromTexture)) and assigned to `_BumpMap`. The keyword `_NORMALMAP` is enabled. The map is not written to a PNG. `GenerateNormalMap()` is public and has no button. It returns immediately when the in-memory colour texture is null, which it is after a domain reload.
6. `_Metallic`, `_Glossiness`, and `_BumpScale` are set. Changing those sliders later updates the live material without generating again.
7. Outside Play Mode the code marks every loaded scene dirty, requests a script reload, and calls `SceneView.FrameLastActiveSceneView()`, which frames the Scene view and can move the camera. UI generators do not frame the Scene view.

**Result:** `SDMaterials/{guid}.png` on disk, and a new in-memory material when a renderer existed. No `.mat` file is created. Save the scene if you want Unity to try to keep the texture and material reference. The PNG remains either way. The code deletes the `.png` only, not a sibling `.meta`.

**URP:** the albedo goes through `mainTexture`. Smoothness is written to `_Glossiness`, which is the Built-in property. URP Lit reads `_Smoothness`. This code does not set `_Smoothness`. Expect the smoothness slider to affect Standard materials and not to drive URP smoothness. This was not re-tested inside a URP project while writing the docs.

## Image-to-material for meshes

**Use it** only if you are prepared to call `Generate()` yourself. `StableDiffusionImage2Material` implements image-to-image from the mesh's **current** `mainTexture`, but `Scripts/Editor/` has no custom editor for it. The default inspector does not draw **Generate**, **Sampler**, or **Model**.

The init image is not a separate field. At generate time the code takes `GetMeshRenderer().sharedMaterial.mainTexture`, casts it to `Texture2D`, and calls `EncodeToPNG()`. The texture must already be a readable `Texture2D`. A missing renderer, a missing material, a null texture, or a non-`Texture2D` throws. `generating` can stay true after an exception that escapes the coroutine, and later clicks would not start a new run even if a button existed. A domain reload (script recompile or restarting Unity) or reloading the scene clears that flag because it is not serialized.

Differences from text-to-material, verified in code:

- The HTTP call is `HttpWebRequest`, not `UnityWebRequest`. It does not set an `Authorization` header.
- Progress uses the non-auth poll only.
- `tiling` sent to WebUI is always false.
- Denoising uses the **Denoising strength** slider, default 0.65.
- The new material always uses shader `Standard`, ignoring **Use Universal Render Pipeline**. On a URP project that material will not match the URP shader.
- The normal-map helper is called with strength `20` and uses a normal Z component of `5` instead of `1`.
- It does not log `Sending request to:`. The other three generators do.
- `Convert.FromBase64String` sits outside the try block. A bad payload escapes the coroutine, leaves `generating` true, and skips `ClearProgressBar`.
- Connection failures on `GetResponseAsync` are not caught. The progress bar is cleared only if that call returns.

There is no supported Inspector workflow for this component in the current source. Treat it as an incomplete surface, not as a second **Generate** button that the docs forgot.

## Models

**List Models** sends GET `{server}{ModelsAPI}` with `Content-Type: application/json` and optional Basic auth.

On success it stores each object's `model_name` and logs `Successfully loaded N models`. Fields `title`, `hash`, `sha256`, `filename`, and `config` are deserialized and discarded. WebUI often expects `sd_model_checkpoint` to be the checkpoint **title** (name plus hash). This project sends `model_name` only. If the server ignores the switch, the image can still succeed using the previously loaded checkpoint. The tool does not read the options API back to prove which checkpoint is active.

HTTP behaviour while listing:

| Response | What the tool does |
|---|---|
| 404 | Logs that the endpoint was not found and GETs `{server}/api/sd-models`. |
| 401 or 403 | Logs the authentication error and stops. Does not try the alternate path. |
| Other connection or protocol error | Logs `Make sure Stable Diffusion is running locally on port 7860, or update the URL in your settings.` The port in that sentence is fixed text, even when your URL uses another port. |
| Body that is empty or does not start with `[` or `{` | Logs `Invalid response format. Expected JSON array but got:` plus the body. |
| JSON that fails to deserialize | Logs `Error parsing models response:` and also starts the alternate `/api/sd-models` request. |

The alternate path is a second GET. It is not part of stock WebUI's documented `/sdapi/v1/` API. Stock WebUI should succeed on the primary path when `--api` is on.

Choosing a model does not switch the server yet. The switch POST runs at the start of **Generate**. A failed switch logs an error and the image request still runs.

## Samplers

The **Sampler** popup lists `StableDiffusionConfiguration.samplers`. Index 0 is the initial selection. The chosen string is written to `sampler_name`. The payload also contains `sampler_index` with the class default `Euler`, and this project never updates that second field. Which field your WebUI honours is WebUI's behaviour; if the sampler you picked is ignored, that unused `sampler_index` value is the first project-side discrepancy to suspect. Compare with a direct API call using WebUI's API docs before reporting a sampler bug.

## Seeds

- **Seed** `-1` means "server chooses". The server's choice is copied into **Generated Seed** when `info` parses.
- **Seed** any other value is sent unchanged. **Generated Seed** should come back as that value when the server honours it.
- **Generated Seed** is not automatically copied into **Seed**. A second click with **Seed** still at `-1` is another random result.
- To keep a result, set **Seed** to the number shown in **Generated Seed**, save the scene, and generate again with the same prompt and parameters. Identity of the result still depends on the checkpoint, sampler, and WebUI version, which this project does not pin.

## Progress bar

The bar title is `Generation in progress`. The non-auth poll formats the fraction with one decimal place (`F1`). The auth poll prints `progress * 100` with default number formatting. If the non-auth poll fails, the bar shows `Progress unknown...` at half scale and the image request continues. The auth poll does not catch parse errors.

Each generator has its own poll flag. If a poll on that component is still running, later polls on it are skipped until the flag clears. The non-auth poll clears the flag after a parse error. The auth poll has no try/catch; a deserialize error leaves the flag set, and that component's bar stops updating until a domain reload. The image request keeps running.

The bar is cleared when a text-to-image, UI image-to-image, or text-to-material request reaches the end of its coroutine, including HTTP failure and the "no image" path. It is not cleared if an exception escapes before that line (for example a bad model index). `StableDiffusionText2Material` and `StableDiffusionImage2Material` call `EditorUtility.ClearProgressBar()` from `Start`, which runs when entering Play Mode. There is no cancel button. The code never calls `Abort` or `Dispose` on the request. Disabling the component or its GameObject stops the coroutine, which skips that cleanup, so `generating` stays true and the bar can remain. The HTTP request is not explicitly cancelled.

A bar left on screen after a failed click can be cleared by entering Play Mode on a scene that contains an active material generator (its `Start` clears the bar), or by a later generation that reaches its cleanup line.

## Play Mode

**Generate** is not disabled in Play Mode. Inside the Editor, Play Mode still defines `UNITY_EDITOR`, so the progress bar and the file write still run. `WriteImageFile` still calls `AssetDatabase.SaveAssets()` during Play Mode. The `ForceUpdate` refresh, `MarkAllScenesDirty`, script reload, and Scene-view framing are skipped while `Application.isPlaying` is true. Stopping Play Mode restores scene objects to their pre-play state. The PNG written during play remains on disk. The in-scene material or sprite assigned during play is discarded with the rest of the play session.

`InspectorRefresh` listens for a return to Edit Mode and exposes `ForceInspectorToRefresh`. Nothing in the project subscribes to that event or calls that method. Returning from Play Mode does not, by itself, reimport the new PNG onto the object.

## Saving and what survives a restart

| Data | Stored where | Survives restart |
|---|---|---|
| Prompts, seeds, indexes, guid, slider values | Scene or prefab YAML | Yes, if you save the scene or prefab |
| Server URL, auth, API paths | `SDSettings` asset | Yes, if you save the asset. Password included |
| Model name list | `modelNames` on the configuration object in the scene | Yes, as a snapshot. It goes stale when you add or remove checkpoints until you click **List Models** again |
| Colour PNG | Output folder | Yes. This is the durable copy |
| Material asset | Not created | No `.mat` file |
| Normal map PNG | Not created | Only if Unity serialized the in-memory texture into the saved scene. The code marks it dirty and does not write a file. Persistence of that embedded texture was not re-tested by reloading the project |
| UI sprite | Assigned in memory, scene marked dirty | Same as the normal map: save the scene; the reliable copy is the PNG |

To put a generated PNG back on an object after a reload, assign the PNG from the output folder onto the material or UI graphic yourself. The guid in the Inspector is the file name to look for.

## Operations that destroy data

**Generate** on a component that already has a PNG deletes that file first. Copy the PNG if you might want it. Generating a parent with **Apply Recursively** replaces child renderers' shared materials. Duplicate the object (new guid) when you want a second result beside the first.

Field edits and generation are not wrapped in Unity's Undo system.

## Demo scene contents

`DemoScene` objects and prompts (so you can regenerate the examples):

| Object | Component | Prompt stored in the scene |
|---|---|---|
| `Text-2-RawImage` | Text-to-image | `a bison in a field` |
| `Image` | Text-to-image | `a cat on an apple` |
| `Button` | Text-to-image | `save icon, white background , SKSKS app icon` |
| `Image-2-RawImage` | Image-to-image | `a horse in a field, photorealistic, highly detailed, stock image` |
| `Floor` | Text-to-material | `light green wood plank` |
| `Ceiling` | Text-to-material | `white stucco` (trailing space in the saved string) |
| `Walls` | Text-to-material | `red brick wall texture` |
| `Table` | Text-to-material | `flat beige wood plank` (trailing space in the saved string) |
| `Cube` | Text-to-material | `birch wood` |
| `Cylinder` | Text-to-material | `mapple tree bark` |
| `Painting1` | Text-to-material | `a oil painting of a vibe` |
| `Painting2` | Text-to-material | `a surrealistic oil painting of sunflower in the rain` |

`DemoScene-NoPostProcessing` uses the same room layout with different saved model indexes and a shorter model snapshot. `Painting1` there stores `a oil painting of a cat eating cheese`. Re-select models before generating, as described in the getting-started guide.
