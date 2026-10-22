# Reference

Behaviour below is taken from the current C# sources. WebUI may add its own validation on top. When a rule here and a WebUI rule both apply, this project does the local half and then forwards the JSON.

## Versions

| Source | Value |
|---|---|
| README title and changelog | 1.3.0 on 2025-03-30 (RunPod-oriented HTTP changes from contributor TeoVibe) |
| Previous changelog entries | 1.2.6 image-to-material, 1.2.5 auth and URP, 1.2.4 image-to-image and the Text2/Image2 renames, 1.2.3 progress-bar fix, 1.2.2 async generation and progress bar, 1.2.1 comments, 1.2.0 play-mode refresh and guid display, 1.1.0 manual sampler list, 1.0.0 first public release |
| `ProjectSettings` `bundleVersion` | 1.2.1 |
| `ProjectVersion.txt` | 2021.3.33f1 (`ee5a2aa03ab2`) |
| Git tags | None |
| Product name / company | `StableDiffusionUnityIntegration` / `AIPower` |
| Colour space | Gamma (`m_ActiveColorSpace: 0`) |
| Scripting API compatibility value | `6`, which in Unity 2021.3 means .NET Standard 2.0 |
| Render pipeline | Built-in, forward rendering path. No URP/HDRP asset assigned |

There is no migration code between 1.x releases. Old scenes keep their serialized fields. Renames in 1.2.4 changed class names; scenes saved before that rename are not upgraded by a script in this repository.

## Endpoints this project calls

All URLs are `StableDiffusionServerURL + path` with no extra slash handling.

| Action | Method | Path field | Default path | Body |
|---|---|---|---|---|
| List models | GET | `ModelsAPI` | `/sdapi/v1/sd-models` | none |
| List models fallback | GET | hardcoded | `/api/sd-models` | none |
| Set checkpoint | POST | `OptionAPI` | `/sdapi/v1/options` | `{"sd_model_checkpoint":"<model_name>"}` |
| Set checkpoint fallback | POST | hardcoded | `/api/options` | same JSON |
| Text-to-image and text-to-material | POST | `TextToImageAPI` | `/sdapi/v1/txt2img` | `SDParamsInTxt2Img` JSON |
| Image-to-image UI | POST | `ImageToImageAPI` | `/sdapi/v1/img2img` | `SDParamsInImg2Img` JSON via `UnityWebRequest` |
| Image-to-material | POST | `ImageToImageAPI` | `/sdapi/v1/img2img` | same class via `HttpWebRequest`, no auth header |
| Progress | GET | `ProgressAPI` | `/sdapi/v1/progress` | none |

Headers on the UnityWebRequest calls: `Content-Type: application/json`. If auth applies: `Authorization: Basic` plus the base64 of `user + ":" + pass` in UTF-8.

The set-checkpoint call tries `UnityWebRequest` first. On failure it tries a blocking `HttpWebRequest` on the same URL. If that throws and the URL contains `/sdapi/v1/options`, it starts another coroutine POST to `/api/options`. A comment in the catch block says a retry cannot be yielded from inside `catch`; the code still starts that coroutine. Success of the image request is not tied to success of the checkpoint request.

## Text-to-image JSON fields the code sets

| JSON field | Value |
|---|---|
| `prompt` | Component prompt |
| `negative_prompt` | Component negative prompt |
| `steps` | Component steps |
| `cfg_scale` | Component CFG scale |
| `width`, `height` | Component width and height after the 128–2048 clamp |
| `seed` | Component seed |
| `tiling` | The **Tiling** checkbox on text-to-material; `false` on text-to-image |
| `sampler_name` | Sampler array entry at `selectedSampler`, when the index is inside the array. If the index is out of range, `sampler_name` stays at the class default `Euler a` |

Fields left at class defaults include `sampler_index` `"Euler"`, `batch_size` 1, `n_iter` 1, `enable_hr` false, `restore_faces` false, `denoising_strength` 0, `subseed` -1, `subseed_strength` 0, `eta` 0, `s_noise` 1, `hr_scale` 2, `override_settings_restore_afterwards` true, and `styles` `[""]`. CFG is sent only as `cfg_scale`.

## Image-to-image JSON fields the code sets

| JSON field | UI image-to-image | Image-to-material |
|---|---|---|
| `init_images` | One base64 PNG of **Input Texture** | One base64 PNG of the renderer's current main texture |
| `prompt`, `negative_prompt`, `steps`, `cfg_scale`, `width`, `height`, `seed` | From the component | From the component |
| `tiling` | false | false |
| `denoising_strength` | class default 0.75 | slider, default 0.65 |
| `sampler_name` | same index rule as text-to-image | same |

Also left at class defaults: `resize_mode` 0, `mask_blur` 4, `inpainting_fill` 0, `inpaint_full_res` true, `initial_noise_multiplier` 1, `include_init_images` false, `sampler_index` `"Euler"`. The `mask` field is commented out of the payload class. `script_name` is commented out.

## Response handling

The code deserializes text responses as `SDResponseTxt2Img` or, for UI image-to-image, `SDResponseImg2Img`. It uses:

- `images[0]` only. `Convert.FromBase64String` expects raw base64. A `data:` prefix throws. On the three `UnityWebRequest` generators that throw is inside the response `try`, so the Console shows `Error processing response:` and `generating` is cleared. On image-to-material the decode is outside the `try`, so the coroutine escapes and `generating` stays true.
- `info`, when it is a non-empty string, deserialized again as `SDParamsOutTxt2Img` (even for image-to-image) to read `seed` into `generatedSeed`. The check is `info != ""`. A null `info` throws `NullReferenceException` after the PNG has already been written. The picture remains; **Generated Seed** stays at its previous value; the log is `Error processing response:`.

Empty `images`, or a null array, logs:

- `No image was returned by the server. Verify that the server is correctly setup.` (text-to-image, text-to-material, UI image-to-image)
- `No image was return by the server. This should not happen. Verify that the server is correctly setup.` (image-to-material; the grammar is the actual string)

HTTP failure logs `Request error:`, `Response code:`, and `Response:` plus the body. Status 405 also logs:

`405 Method Not Allowed - This is common with RunPod. Make sure you launched with --api flag and check your firewall/proxy settings.`

and

`For RunPod, you might need to configure CORS settings or use their API proxy.`

Those 405 strings exist only on the three `UnityWebRequest` generators. Image-to-material does not have that branch.

Other exact Console strings:

| Situation | Message |
|---|---|
| List models URL | `Requesting models from: ` + URL |
| Auth header added while listing | `Using API key to authenticate` |
| List connection failure | `Connection error: ` + error, then `Response code:` and `Response:` |
| 404 list | `API endpoint not found. Trying alternate API endpoint...` |
| 401/403 list | `Authentication error: Server requires an API key. Please enable useAuth and set user/pass fields in your SDSettings.` |
| Other list failure | `Make sure Stable Diffusion is running locally on port 7860, or update the URL in your settings.` |
| Bad list body | `Invalid response format. Expected JSON array but got: ` + body |
| List parse error | `Error parsing models response: ` + exception message |
| Alternate list URL | `Trying alternate models API: ` + URL |
| Alternate list failure | `Alternate API also failed: ` + error, then `Please check if your Stable Diffusion WebUI is running at ` + server URL, then `If you're using AUTOMATIC1111's WebUI, make sure you launched it with the '--api' flag` |
| Alternate list parse failure | `Failed to parse models from alternate API: ` + message, then `Make sure your Stable Diffusion WebUI was launched with the '--api' flag`, then `Example launch command: './webui.sh --api'` |
| Set model | `Setting model to: ` + name + ` using URL: ` + URL |
| Model list empty inside set-model | `Failed to load model list. Cannot set model.` |
| Set model UnityWebRequest failed | `Failed to set model using UnityWebRequest: ` + error, then `Trying with HttpWebRequest as fallback...` |
| Set model WebException | `Error setting model: ` + message |
| All set-model attempts failed | `All attempts to set model failed. Make sure Stable Diffusion WebUI is running with '--api' flag.` |
| Progress GET failed | `Failed to get progress: ` + error |
| Progress parse failed | `Error parsing progress: ` + message |
| Unreadable init texture | `Input Image {name} isn't readable. Go to texture import settings and tick the Read/Write box` |
| PNG write failed | `Error writing image file: ` + message |
| Shader missing | `Shader setup wrong: Please check if you're project uses 'Standard' or 'Universal Render Pipeline'` |
| Image request URL | `Sending request to: ` + URL on text-to-image, UI image-to-image, and text-to-material. Image-to-material does not log this line |

`SetupFolders` failures log `exception.Message` and the stack trace. They do not stop `GenerateAsync`; a later write can then fail as well.

## Local validation

| Check | Rule |
|---|---|
| Prompt | Must be non-empty or `Generate()` returns. No message. |
| UI image-to-image texture | Must be non-null or `Generate()` returns. No message. Must be readable or the error above is logged and the coroutine is not started. |
| Running flag | If `generating` is already true, `Generate()` returns. No message. The flag is not serialized. It stays true if the coroutine exits by an uncaught exception. |
| Width and height | In the Editor, values below 128 become 128 and values above 2048 become 2048, every `Update`. No multiple-of-8 check. The clamp is inside `#if UNITY_EDITOR`, so a player build would not clamp. |
| Steps, CFG, seed, denoising | No clamp in code except the Inspector `[Range]` on denoising (0–1), metallic, smoothness, tiling repeats, and normal-map strength. `[Range]` is an Inspector hint; a script can still assign other values. |
| Sampler index | Used only when `0 <= index < samplers.Length`. Otherwise the payload keeps `sampler_name` `"Euler a"`. |
| Model index | Not checked. Out of range throws `IndexOutOfRangeException` after the old PNG has been deleted. |
| Model name | Not checked against the server. |
| Auth | Header only when `useAuth` is true and both strings are non-empty (`Equals("")`). Whitespace-only passwords count as present. |

## Files and Unity objects

| Path or object | Role |
|---|---|
| `Assets/Plugins/Newtonsoft.Json.dll` | JSON serializer. Meta: Any platform enabled, Editor platform entry `enabled: 0`. |
| `Assets/Stable-Diffusion-Unity-Integration/Settings/DefaultLocalSDSettings.asset` | Default local profile. Guid `2f0a42d2a132f7144b35b4cabb576d34`. |
| `Assets/Stable-Diffusion-Unity-Integration/Input Image/horse_input.png` | Demo init image. Importer `isReadable: 1`, compression none. Guid `ed309daa36c8f274488a29b3c95faa39`. |
| `Assets/Stable-Diffusion-Unity-Integration/Scenes/DemoScene.unity` | Demo with post-processing volume. |
| `Assets/Stable-Diffusion-Unity-Integration/Scenes/DemoScene-NoPostProcessing.unity` | Demo without that volume. |
| `Assets/Stable-Diffusion-Unity-Integration/Scenes/PostProcessingVolume Profile.asset` | Profile referenced by the volume. Guid `45ed0ce96d2961245a23e347424e5389`. |
| `Stable-Diffusion-Unity-Integration.unitypackage` | Package at the repository root. Not unpacked for this documentation. |

Output file name pattern: `{guid}.png` with a GUID such as `89cbd6ae-0e3e-485e-8cc5-5d381e7b05c3`. Subfolder `SDImages` or `SDMaterials`.

`.gitignore` entry `/[Aa]ssets/StreamingAssets*` ignores the fallback output tree. It does not ignore `Assets/streamingAssets`.

## Material property names written

On the material created after a successful material generation, and again when sliders change:

| Property | Value |
|---|---|
| `mainTexture` | Generated colour texture |
| `mainTextureScale` | `(-tilingX, -tilingY)` |
| `_Metallic` | Metallic slider |
| `_Glossiness` | Smoothness slider |
| `_BumpMap` | Generated normal texture, or null when the normal map is off |
| `_BumpScale` | Normal Map Strength slider |
| keyword `_NORMALMAP` | Enabled only when the normal map is on and the texture exists |

Shader selected by text-to-material: `Universal Render Pipeline/Lit` if `useUniversalRenderPipeline`, else `Standard`. Image-to-material: always `Standard`.

Normal-map texture format: `TextureFormat.RGB24`, no mip chain. The constructor's `linear` argument is false, so the texture is sRGB rather than a linear normal map. Each sample is `(R, G, G)`; the blue channel is not read. `GetPixel` is clamped by Unity at the edges. The source texture must be readable. A texture created with `LoadImage` is readable. The generators never assign `wrapMode` on the colour texture or the normal map.

## Progress JSON read

`SDProgress.progress` is the 0–1 bar value. The label is that value times 100. The non-auth poll formats the label with `F1`. The auth poll uses default number formatting and has no try/catch. `eta_relative`, `current_image`, `textinfo`, and the nested `state` object are deserialized and ignored. `current_image` is not shown.

## Editor-only versus compiled-everywhere

Inside `#if UNITY_EDITOR`: dimension clamp, guid creation, progress bar, asset database save, scene dirty flags, shader refresh.

Not inside that define: the HTTP request, file write, material creation, and `Generate()` itself. `using UnityEditor` appears unconditionally in `StableDiffusionGenerator.cs` and `ReadOnlyAttribute.cs`, which are outside an `Editor` folder. A player build's script compile does not reference the `UnityEditor` assembly, so those two files fail that compile. This was determined by reading the source, not by running a build.

`ReadOnlyAttribute` is a `PropertyAttribute`. `ReadOnlyDrawer` (in the Editor folder) sets `GUI.enabled = false` around the property. The field can still be changed in Debug mode or by editing the scene YAML.

## External responsibility

| Area | This project | AUTOMATIC1111 WebUI |
|---|---|---|
| Install | Documents the API flags it depends on | Installs Python, torch, and the UI. https://github.com/AUTOMATIC1111/stable-diffusion-webui |
| Checkpoints | Stores `model_name` strings and POSTs one as `sd_model_checkpoint` | Loads files, lists them on `/sdapi/v1/sd-models`, applies the options change |
| Sampling | Sends prompt, size, steps, CFG, seed, sampler name, tiling, and for img2img an init image and denoising | Runs the sampler and returns base64 PNG(s) plus an `info` string |
| Auth | Optional HTTP Basic header from User and Pass | `--api-auth` and `--listen`. https://github.com/AUTOMATIC1111/stable-diffusion-webui/wiki/Command-Line-Arguments-and-Settings |
| Progress | Polls `/sdapi/v1/progress` and draws an Editor bar | Computes `progress` |
| Errors after launch | Logs status code and body | Produces the status and the JSON or HTML body |
| Output files | Writes the first image into the Unity project | May also write its own outputs under its own output folder. This project does not read that folder |

RunPod, or any other host, is only a base URL plus whatever auth matches HTTP Basic. Vendor-specific proxies, pod templates, and CORS setup are not implemented here. Vendor site: https://www.runpod.io/ . The 405 Console text mentions RunPod; it is not a RunPod client.

Normal-map math inside this project is local. The algorithm's origin is https://github.com/unitycoder/NormalMapFromTexture . The two material components do not call that repository; they contain adapted copies with different strength constants.
