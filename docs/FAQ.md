# FAQ

Answers describe the current scripts. WebUI features that have no Inspector control are answered as "not in this project".

## Basics

### 1. What is Stable Diffusion Unity Integration?

It is a Unity Editor tool that calls an AUTOMATIC1111 WebUI HTTP API and puts the returned image on a UI graphic or a mesh material. It does not run the diffusion model itself. Repository: https://github.com/dobrado76/Stable-Diffusion-Unity-Integration

### 2. Which version do these answers describe?

The README changelog calls the current user-facing release **1.3.0** (2025-03-30). Unity Player Settings still say bundle version **1.2.1**. There are no git tags. If you imported an older `unitypackage` and the inspector does not match this FAQ, the package is older than the scripts these answers describe.

### 3. Does the download include Stable Diffusion or any checkpoints?

No. You install [AUTOMATIC1111 WebUI](https://github.com/AUTOMATIC1111/stable-diffusion-webui) and its models separately. The Unity side only stores the server URL and the model names the server reports.

### 4. Can I use ComfyUI, Forge, or the Stability API?

Only if that server accepts the same requests this project sends: GET `/sdapi/v1/sd-models`, POST `/sdapi/v1/options` with `sd_model_checkpoint`, POST `/sdapi/v1/txt2img` and `/sdapi/v1/img2img`, and GET `/sdapi/v1/progress`. There is no ComfyUI graph, no Stability REST client, and no vendor SDK. A different API will fail at list or generate time. Use that product's docs for its own API, and use this FAQ for the JSON this Unity project actually posts.

### 5. Which Unity version should I open?

Use the editor in `ProjectVersion.txt`, **2021.3.33f1**, or another editor that has the APIs the scripts call. `UnityWebRequest.Result` exists from Unity 2020.2, so Unity 2019 does not compile this project. `FindAnyObjectByType` was backported; Unity staff listed 2020.3.45f1, 2021.3.18f1, 2022.2.5f1, and 2023.1.0a20 (https://discussions.unity.com/t/findobjectsbytype-and-its-siblings/910153/1). 2021.3.33f1 is newer than 2021.3.18f1, so that method is expected to exist there. This was not re-checked by launching an editor. The README also names Unity 2019; that claim does not match the source.

### 6. How do I install it?

Either open a clone of the repository as the Unity project, or import `Stable-Diffusion-Unity-Integration.unitypackage` from the repository root with **Assets > Import Package > Custom Package**. You still need a running WebUI with `--api`. Step-by-step checks are in [Getting started](getting-started.md).

### 7. Where is the demo scene? The README path does not exist.

Open `Assets/Stable-Diffusion-Unity-Integration/Scenes/DemoScene.unity` or `DemoScene-NoPostProcessing.unity`. The README path `StableDiffusionIntegration/Scenes/DemoScene.unity` is leftover wording.

### 8. Why does DemoScene say a script is missing?

The object `PostProcessingVolume` uses the Post Processing Stack v2 script. That package is not in `Packages/manifest.json`. Install Unity's Post Processing package and reopen the scene, or use the no-post-processing scene. Generation does not need the volume. The volume is global, weight 1, and its profile is `Assets/Stable-Diffusion-Unity-Integration/Scenes/PostProcessingVolume Profile.asset`.

### 9. Is this a runtime feature in my game?

The buttons are Editor inspectors. The generator scripts are not inside an `Editor` folder, so they are part of the player compile, and two of them import `UnityEditor` unconditionally, which fails a player build. Use the tool in the Editor, save the PNG or the scene, and ship ordinary textures. There is no in-game generate UI.

### 10. Where is the video and the project page?

The README links a demo video: https://youtu.be/8gf5GPzhgk8 . The source repository is https://github.com/dobrado76/Stable-Diffusion-Unity-Integration .

## Server connection

### 11. What URL should I put in?

`http://127.0.0.1:7860` for a default local WebUI. The field is **Stable Diffusion Server URL** on the `SDSettings` asset assigned to `StableDiffusionConfiguration`. Do not add a trailing slash. Do not append `/sdapi`; the path fields already contain `/sdapi/v1/...`.

### 12. How do I turn on the API?

Start WebUI with `--api`. On a typical Windows install that is `COMMANDLINE_ARGS` in `webui-user.bat`. Flag definitions are WebUI's: https://github.com/AUTOMATIC1111/stable-diffusion-webui/wiki/Command-Line-Arguments-and-Settings . Then browse to `http://127.0.0.1:7860/sdapi/v1/sd-models` and confirm you see a JSON array before you click anything in Unity.

### 13. Where is the List Model button?

The button text is **List Models**, on the `StableDiffusionConfiguration` inspector, under the default fields. It is not on the generator components.

### 14. What does a successful list look like?

The Console logs `Successfully loaded N models`, and **Model Names** on the configuration object fills with strings. Those strings are each JSON object's `model_name`. Titles, hashes, and filenames are discarded.

### 15. Why does the error mention port 7860 when I use another port?

The string `Make sure Stable Diffusion is running locally on port 7860, or update the URL in your settings.` is fixed text on some list-model failures. The URL that was actually called is the earlier line `Requesting models from:`.

### 16. What is the `/api/sd-models` request?

A fallback GET this project tries after a 404 or a JSON parse failure on the primary models URL. Stock WebUI's documented route is `/sdapi/v1/sd-models`. If the primary path 404s, fix `--api` or the base URL. Do not reconfigure WebUI to serve `/api/sd-models` unless you know you are behind a proxy that uses that prefix.

### 17. Can I change the API paths?

Yes. **Models API**, **Text To Image API**, **Image To Image API**, **Option API**, and **Progress API** on the settings asset are concatenated onto the server URL. Defaults match stock WebUI. Empty or wrong paths produce 404s. There is no UI to reset them other than typing the defaults back. The defaults are listed in [Configuration](configuration.md#server-and-api-paths).

### 18. Does it support HTTPS and a remote host?

The URL is a string. `https://` is sent if you type it. Certificate failures show up as connection errors from Unity. The project does not add a certificate bypass. Hosted WebUI, including a RunPod instance, is the same mechanism: put the origin in **Stable Diffusion Server URL**. There is no RunPod panel, pod template, or proxy login. Vendor docs live at https://www.runpod.io/ . HTTP 405 during generate logs extra sentences that mention RunPod; those sentences are hints, not a setup wizard.

### 19. Why did I get 405 Method Not Allowed?

The three `UnityWebRequest` generators log `405 Method Not Allowed - This is common with RunPod...` plus a line about CORS or an API proxy. The status itself comes from the server or the proxy in front of it. Check that `--api` is on and that the logged URL is the API origin, not a web UI page. CORS flags are a WebUI or proxy concern; this project does not send a CORS configuration.

## Authentication

### 20. How do I send the API password?

On the settings asset enable **Use Auth** and fill **User** and **Pass** with the same pair you passed to WebUI as `--api-auth user:password`. Both fields must be non-empty. The header is HTTP Basic, not a custom `X-API-Key`. The Console line `Using API key to authenticate` and the 401 text that says "API key" refer to these two fields.

### 21. I enabled Use Auth and it still returns 401.

One of the strings is empty, the pair does not match `--api-auth`, or you are using `StableDiffusionImage2Material`, which never sets the header. A password made only of spaces is sent, because the check is `Equals("")`, not a trim.

### 22. Where is the password saved?

In the `SDSettings` asset YAML, in plain text. It is not printed by the generator logs. Do not commit a real password, and delete it from any file you attach to a public issue.

### 23. Does List Models use the password?

Yes, when **Use Auth** is on and both fields are set. A 401 or 403 stops the list and does not try the alternate URL. The message is `Authentication error: Server requires an API key. Please enable useAuth and set user/pass fields in your SDSettings.`

## SDSettings and defaults

### 24. How do I create another settings asset?

**Assets > Create > SDSettings**. The file is always `Assets/NewSDSettings.asset`. Creating a second one fails until you rename or move the first. The new asset gets code defaults, not a duplicate of `DefaultLocalSDSettings`.

### 25. Which asset is the demo using?

`StableDiffusionConfiguration` in the demo scenes references `Assets/Stable-Diffusion-Unity-Integration/Settings/DefaultLocalSDSettings.asset` (asset guid `2f0a42d2a132f7144b35b4cabb576d34`).

### 26. I changed Sampler on the settings asset and the image still uses Euler a.

The field `sampler` on `SDSettings` is not read. The sampler that is sent is the **Sampler** popup on the generator, which indexes `StableDiffusionConfiguration.samplers`.

### 27. I changed width and steps on the settings asset and the UI component ignored them.

`StableDiffusionText2Image` starts at width 512 and height 512, and its `Awake` copies settings only when width or height is negative. A new one therefore keeps 512, 90 steps, CFG 7, and seed `-1`. `StableDiffusionImage2Image` is the same except steps start at 50. Material components start at width `-1`, so the first `Awake` does copy the asset (512, 512, 35 steps, CFG 7, seed `-1` on the default asset). After that, the component has its own numbers. Editing the asset does not push updates into objects already in the scene.

### 28. Why are brand-new text-to-image steps 90?

That is the field initializer on `StableDiffusionText2Image`. The settings asset default of 35 is a different field and is not applied to this component.

### 29. Do I need to press Apply or Save before Generate?

Generate reads the live objects. Save the scene to keep component edits, and save the project to keep the settings asset. There is no Apply button. Generation does not wait for a save.

## Models and samplers

### 30. The Model dropdown crashes as soon as I click the object.

`modelNames` is null until **List Models** succeeds, and the popup cannot take a null array. The sampler popup also throws if no `StableDiffusionConfiguration` is active in a loaded scene. Add the configuration, assign settings, and list models. Setting **Model Names** size to 0 avoids the null crash but leaves nothing to select.

### 31. I listed models, generated, and got IndexOutOfRangeException.

`selectedModel` is a stored integer. It is not clamped. `DemoScene` has 17 saved names and the `Button` object stores index 92. `DemoScene-NoPostProcessing` has 3 names and several objects store indexes well above 2. After every **List Models**, choose **Model** again on each generator. The exception happens after `{guid}.png` has already been deleted.

### 32. The model I chose is not the one WebUI used.

This project POSTs `{"sd_model_checkpoint":"<model_name>"}`. It does not send the checkpoint title or hash, and it does not cancel the image request when the options call fails. Check the Console for `Failed to set model using UnityWebRequest` or `All attempts to set model failed`. If the JSON you see does not match what your WebUI build expects, that difference is the integration boundary. WebUI's options behaviour is documented with its API: https://github.com/AUTOMATIC1111/stable-diffusion-webui/wiki/API

### 33. Does listing models switch the checkpoint?

No. Listing only fills **Model Names**. The options POST runs at the start of **Generate**.

### 34. Can the tool download the sampler list?

No. Edit the **Samplers** array on `StableDiffusionConfiguration`. The default names are Euler a, Euler, LMS, Heun, DPM2, DPM2 a, DPM++ 2S a, DPM++ 2M, DPM++ SDE, DPM fast, DPM adaptive, LMS Karras, DPM2 Karras, DPM2 a Karras, DPM++ 2S a Karras, DPM++ 2M Karras, DPM++ SDE Karras, DDIM, and PLMS. Unknown names fail on the server.

### 35. I picked DPM++ 2M and the result looks like Euler.

The popup writes `sampler_name`. The same JSON also contains `sampler_index` fixed as `Euler`, and the code never changes that second field. Which one your WebUI applies is up to that WebUI version. Compare with a direct API call before treating it as a Unity UI bug.

### 36. What if I have two StableDiffusionConfiguration objects?

Generators keep the first one `FindAnyObjectByType` returned, in a static field. Which object that is when several are active is not specified by this project. Use one active configuration.

## Generate buttons and prompts

### 37. Where is the Generate button?

At the bottom of the inspector for `StableDiffusionText2Image`, `StableDiffusionImage2Image`, and `StableDiffusionText2Material`, under the **Sampler** and **Model** popups. `StableDiffusionImage2Material` does not have one.

### 38. Why is there no Generate button on image-to-material?

`Scripts/Editor` has no custom editor for `StableDiffusionImage2Material`. The method `Generate()` exists and is public, and the default inspector does not draw a button for it. Sampler and model indexes stay at 0 unless you edit the hidden fields. The component also skips HTTP Basic and always creates a Standard material.

### 39. I click Generate and absolutely nothing happens.

The prompt is empty, UI image-to-image has no input texture, or an earlier run left `generating` true after an exception. None of those three cases logs a line. Reload the scene to clear a stuck flag, then try again with a non-empty prompt.

### 40. What are the old names StableDiffusionImage and StableDiffusionMaterial?

Version 1.2.4 renamed them to `StableDiffusionText2Image` and `StableDiffusionText2Material`. The current tree does not contain the old class names.

### 41. Unity offers a component called Stable Diffusion Generator. Should I add it?

No. That is the base class. It has no prompt and no **Generate** button. Add one of the four concrete components.

## Image-to-image

### 42. Which texture is uploaded for UI image-to-image?

The **Input Texture** field on `StableDiffusionImage2Image`, not the sprite currently shown on the `Image`. The demo `Image-2-RawImage` points at `Assets/Stable-Diffusion-Unity-Integration/Input Image/horse_input.png`.

### 43. Why must the texture be readable?

The code calls `EncodeToPNG()`. Unity only allows that when Read/Write is enabled. Otherwise UI image-to-image logs `Input Image {name} isn't readable. Go to texture import settings and tick the Read/Write box` and does not send a request. The inspector also shows `Input textures should be uncompressed and readable`.

### 44. What denoising strength is used?

UI image-to-image always sends **0.75**. There is no slider. Image-to-material sends its **Denoising strength** slider, default **0.65**. Text-to-image sends the class default 0, and WebUI ignores denoising for a normal text-to-image call.

### 45. Can I inpaint or pass a mask?

No. The mask field is commented out of the request class because the author saw HTTP 500 when it was included. There is no mask slot in the inspector.

### 46. The horse image is the init image. Will Generate modify that PNG?

No. The file is encoded into the request. The result is written to a different file, `SDImages/{guid}.png`.

## Materials, tiling, and normal maps

### 47. Which objects in the demo are materials?

`Floor`, `Ceiling`, `Walls`, `Table`, `Cube`, `Cylinder`, `Painting1`, and `Painting2` use `StableDiffusionText2Material`. Prompts are listed in the [user guide](user-guide.md#demo-scene-contents).

### 48. What is the difference between Tiling and Tiling X / Tiling Y?

**Tiling** is sent to WebUI as `tiling` so the sampler can attempt a seamless image. Only text-to-material forwards the checkbox. The other generators send false. **Tiling X** and **Tiling Y** are Unity `mainTextureScale`, and the code sets the scale to **negative** X and Y, which mirrors the texture. They do not change the WebUI request.

### 49. Why did generating the cube change the cylinder?

`Cylinder` is a child of `Cube`. `Cube`'s **Apply Recursively** is on, so the new shared material is copied onto child mesh renderers. `Cylinder` still has its own component and prompt, but its renderer now points at the cube's material. Turn **Apply Recursively** off on the parent if you do not want that.

### 50. Does Apply Recursively search inactive children?

No. Inactive children are skipped. If the host object has no `MeshRenderer` and the flag is off, generation still requests an image and then `LoadIntoMaterial` returns without assigning anything.

### 51. Where is the normal map file?

Nowhere. The normal map is an in-memory texture assigned to `_BumpMap`. Only the colour image is written to `{guid}.png`. **Normal Map Strength** changes `_BumpScale` and does not rebuild the map. The rebuild function is called with a fixed 0.5 (text-to-material) or 20 (image-to-material), not with the slider. After a domain reload the in-memory normal map is gone until you generate again and save the scene if Unity kept the embedded texture.

### 52. How do metallic and smoothness get applied?

They set `_Metallic` and `_Glossiness` on the current shared material, including when you move the sliders without generating. URP Lit uses `_Smoothness`, which this code does not set. Image-to-material always uses the Standard shader even if **Use Universal Render Pipeline** is enabled.

### 53. How do I enable URP?

Install URP using Unity's package docs, switch the project to that pipeline, and enable **Use Universal Render Pipeline** on the settings asset. Only `StableDiffusionText2Material` reads the flag, and it looks up the shader `Universal Render Pipeline/Lit`. If the shader is missing you get `Shader setup wrong: Please check if you're project uses 'Standard' or 'Universal Render Pipeline'`, and the material will be pink or absent. HDRP is not implemented.

### 54. Will this replace a material asset I already assigned?

Yes. On success the code does `sharedMaterial = new Material(...)`. The previous material asset on disk is not deleted, but the renderer no longer points at it. No new `.mat` file is created. Child renderers are repointed when **Apply Recursively** is on. This is not undoable through this project.

## Size, seed, and batches

### 55. Why can't I set width to 64 or 4096?

In the Editor, `Update` clamps width and height to the inclusive range 128–2048. The clamp does not run in a player, and this project does not support player generation anyway.

### 56. Does the size have to be a multiple of 8?

This project does not check. Many WebUI builds reject other sizes. If the response body says so, change **Width** and **Height**. That rule belongs to WebUI, and it can change between WebUI versions.

### 57. What does seed -1 do?

It is sent as `-1`. WebUI treats that as "pick a seed". The seed it picked is copied into **Generated Seed** when the response `info` string parses. **Generated Seed** is not copied back into **Seed**. Leave **Seed** at `-1` and every click is a new draw. Paste **Generated Seed** into **Seed** when you want another request with that number.

### 58. Can I generate a grid or four images at once?

No. `batch_size` and `n_iter` are hardcoded to 1. Only `images[0]` is saved. Extra images in the response are ignored.

### 59. Where are high-res fix, face restore, and styles?

They are not in the inspector. The JSON leaves `enable_hr` false, `restore_faces` false, and `styles` as a single empty string. Turning those on inside WebUI's own browser UI does not add controls to Unity.

## Files, save, and undo

### 60. Where is the PNG?

UI components write `{output}/SDImages/{guid}.png`. Material components write `{output}/SDMaterials/{guid}.png`. With the default settings, and without a pre-existing `Assets/streamingAssets` folder, `{output}` is `Assets/StreamingAssets`. The **Guid** field on the component is the file name. Success does not log the full path.

### 61. What is the difference between streamingAssets and StreamingAssets?

**Output Folder** defaults to `/streamingAssets`, which is concatenated onto the `Assets` path. If that exact directory does not exist, the code uses Unity's `Application.streamingAssetsPath`, the `Assets/StreamingAssets` folder, and creates it. The lowercase folder is used only when it already exists.

### 62. Why doesn't git show my new images?

`.gitignore` ignores `Assets/StreamingAssets*`. The files are still on disk. `Assets/streamingAssets` is not matched by that pattern.

### 63. Can I undo Generate?

Not with this tool. It never calls Unity's Undo API. The previous PNG is deleted at the start of the run, before the server responds. Copy the file first if you want to keep it. Duplicating the GameObject and letting it assign a new guid is how you keep a second result instead of overwriting the first.

### 64. I generated during Play Mode and the object reverted.

Unity discards scene edits when Play Mode ends. The PNG stays. Generate in Edit Mode when the scene should keep the new sprite or material. The helper `InspectorRefresh.ForceInspectorToRefresh` is never called, so leaving Play Mode does not reassign the new file for you.

### 65. The scene shows the picture, but I cannot find a material asset.

That is expected. The material and the UI sprite are created in memory. Save the scene if you want Unity to embed those references. The durable file you can reimport later is the PNG. Reassign it by hand if a reload loses the in-scene texture.

### 66. Generate failed and the old PNG is gone.

`SetupFolders` deletes `{guid}.png` before the HTTP call. A bad model index, a connection error, or an empty `images` array does not restore it. There is no recycle bin inside the project. Restore the picture from your own copy or from version control if it was committed.

## Progress, hangs, and errors

### 67. What does the progress bar mean?

The title is `Generation in progress`. About twice a second the Editor polls `/sdapi/v1/progress` and shows `progress * 100`. If that GET fails, the non-auth path shows `Progress unknown...` and the image request continues. The bar is not a cancel button. There is no timeout.

### 68. The Editor froze right after I clicked Generate.

If the UnityWebRequest options call fails, setting the model falls through to a blocking `HttpWebRequest`. A host that never answers can stall the Editor until the operating system gives up. Fix the options URL so the first request succeeds.

### 69. The bar is stuck after an exception.

Enter Play Mode in a scene that has an active text-to-material or image-to-material component. Their `Start` method clears the bar. Then stop Play Mode. Reloading the scene clears a stuck `generating` flag because that flag is not saved.

### 70. What does "No image was return by the server" mean?

That exact wording, including the grammar, comes from `StableDiffusionImage2Material` when the response has no `images` array. The other generators say `No image was returned by the server. Verify that the server is correctly setup.` The HTTP call itself succeeded. Read the following `Full response:` line and then WebUI's log. This project will not retry a different endpoint for that case.

### 71. The Console says the server must be on port 7860, but my browser can open the API.

Use the URL in `Requesting models from:`. A trailing slash, the wrong settings asset, or a second configuration object will call a different address than the one you opened in the browser.

## Demo scene

### 72. I opened the demo and Generate on the Button throws.

`Button` in `DemoScene` stores model index 92. The saved **Model Names** list on that scene has 17 entries. Click **List Models** against your server and pick a model again. Do this for every generator. Several indexes in `DemoScene-NoPostProcessing` are also past the end of its 3-name list.

### 73. Are the model names saved in the demo something I should download?

No. They are a snapshot of one developer's WebUI at save time. They are not distributed with this repository, and they will not match your install. Treat them as stale data.

### 74. What is Image-2-RawImage?

A `RawImage` with `StableDiffusionImage2Image`, prompt `a horse in a field, photorealistic, highly detailed, stock image`, and **Input Texture** set to `horse_input.png`.

## Platforms, builds, and the JSON plugin

### 75. The project does not compile: FindAnyObjectByType is missing.

Check the patch version, not only the year. Unity staff listed `FindAnyObjectByType` from 2020.3.45f1, 2021.3.18f1, 2022.2.5f1, and 2023.1.0a20 onward. This repository's project version is 2021.3.33f1, which is past that 2021.3 backport, so do not tell a 2021.3.33 user to move to 2023.1 for this error alone. Unity 2019, and 2021.3 builds older than 18f1, do fail here. There is no `#if` fallback. `UnityWebRequest.Result` is a separate requirement from Unity 2020.2.

### 76. The project does not compile: Newtonsoft could not be found.

The DLL should be at `Assets/Plugins/Newtonsoft.Json.dll`. Its importer meta currently has the Editor platform entry disabled (`enabled: 0`) while Any Platform is enabled. Select the DLL and enable **Editor**. If the file is missing, restore it from the repository. The scripts do not compile without `Newtonsoft.Json`.

### 77. Why did my player build fail on UnityEditor?

`StableDiffusionGenerator.cs` and `ReadOnlyAttribute.cs` import `UnityEditor` and are not in an Editor folder. That is a property of this tree, not a Player Settings switch. Keep this tool in the Editor.

### 78. Does the OS change the output path?

The code uses `Application.dataPath`, `Application.streamingAssetsPath`, and `Path.Combine` for the subfolder. Those Unity properties are correct on Windows, macOS, and Linux. The **Output Folder** value is still a raw string concat, so a missing leading slash is wrong on every OS. Paths were read from the code and were not re-run on each OS for this documentation.

## Licence, tests, and support

### 79. Is the licence LGPL or MIT?

The `LICENSE` file in the repository root is the MIT License, copyright 2025 Dominic Branchaud. The README section titled Licence still describes LGPL v2.1. They disagree. These docs are not legal advice. Model weights and WebUI have their own terms, which this repository does not grant.

### 80. Are there automated tests?

No. The Test Framework package is in the manifest and there is no Tests folder. Nobody has a green test run to cite.

### 81. Who do I contact, and what should I send?

Open https://github.com/dobrado76/Stable-Diffusion-Unity-Integration/issues with your Unity version, the component name, the logged request URL, the Console error, and what you expected. Strip passwords, private prompts, and home-directory paths. Do not send the whole project `Library` folder. There is no support desk and no promised reply. The Buy Me a Coffee link in the README is not a ticket system. WebUI faults, once the URL in the log is correct, belong on the WebUI issue tracker.

### 82. Is a feature that I see in WebUI's browser "planned" here?

No plan is published. If the inspector does not show the control, this project does not do that thing. See [Roadmap](roadmap.md).

### 83. Can these components live on a prefab?

The prompt, guid, sampler index, and model index are ordinary serialized fields, so a prefab asset can store them. Generating still deletes the PNG that belongs to that guid. Instances that share one guid share one output file. How Unity records a generation as a prefab override was not tested when these notes were written. Save the scene or apply the override the way you would for any other Inspector edit you want to keep.

### 84. Why does Generate fail when the object is inactive?

`Generate()` calls `StartCoroutine`. Unity does not start that coroutine when the GameObject is inactive or the component is disabled, so the click never reaches the server and `generating` stays false. Activate the generator, enable the component, and activate `StableDiffusionConfiguration`, then click **Generate** again. Text-to-image, UI image-to-image, and text-to-material then log `Sending request to:`. Image-to-material does not log that line. Disabling the object after the request has started is different: the coroutine stops, `generating` stays true, and the HTTP call is not aborted.

### 85. Why is the UI element still the old size?

**Width** and **Height** are the pixel size sent to WebUI and stored in the PNG. The code does not change the RectTransform. It only replaces the sprite or the RawImage texture. The new sprite uses pivot `(0, 0)`, so an `Image` can sit differently inside the same rectangle. Resize the RectTransform yourself if you want the widget to match 512 by 512 or whatever you generated.

### 86. Why did the Editor hitch, reload scripts, or move the Scene camera?

On a successful edit-mode assignment, UI and material generators call `EditorUtility.RequestScriptReload()` and `EditorSceneManager.MarkAllScenesDirty()`. Material generators also call `SceneView.FrameLastActiveSceneView()`, which frames the Scene view and can move the camera. UI generators do not move the camera. Every loaded scene is marked dirty, not only the one you generated in. The reload can make an in-memory sprite or material look missing afterward. The PNG under the guid file name is the copy to assign again. This reload behaviour was read from the source and not re-tested by watching the Editor.

### 87. The picture appeared and the Console still says Error processing response.

The PNG is written before `info` is parsed. If `info` is null, `info != ""` throws, and the catch logs `Error processing response:` plus the body. **Generated Seed** is left unchanged. The file is not rolled back. Copy the seed from WebUI's own result if you need it, or set **Seed** yourself for the next run.
