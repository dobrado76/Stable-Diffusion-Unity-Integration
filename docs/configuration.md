# Configuration

There is no settings window and no environment-variable layer. Configuration is Unity serialization: one `SDSettings` asset, plus fields on the configuration object and on each generator.

Changes apply to the next request as soon as they are on the live objects. Nothing is generated until you click **Generate** or **List Models**. Save the scene to keep component edits. Save the project so the `SDSettings` asset keeps edits. There is no separate Apply button.

## SDSettings asset

Create one with **Assets > Create > SDSettings**. The menu always writes `Assets/NewSDSettings.asset`. If that path already exists, Unity's `AssetDatabase.CreateAsset` fails. Rename or move the existing asset first. The menu does not copy `DefaultLocalSDSettings`; a new asset gets the field initializers below.

The included asset is `Assets/Stable-Diffusion-Unity-Integration/Settings/DefaultLocalSDSettings.asset`. Assign it to **Settings** on `StableDiffusionConfiguration`. Generators do not reference the asset themselves. They look up the configuration object in loaded scenes.

The Inspector groups fields under the headers **AUTOMATIC1111 Settings**, **API Settings**, and **URP Settings**.

### Server and API paths

| Inspector label | Field | Default | Effect |
|---|---|---|---|
| Stable Diffusion Server URL | `StableDiffusionServerURL` | `http://127.0.0.1:7860` | Origin concatenated in front of every API path. No trailing slash. |
| Models API | `ModelsAPI` | `/sdapi/v1/sd-models` | GET used by **List Models**. |
| Text To Image API | `TextToImageAPI` | `/sdapi/v1/txt2img` | POST used by text-to-image and text-to-material. |
| Image To Image API | `ImageToImageAPI` | `/sdapi/v1/img2img` | POST used by both image-to-image components. |
| Option API | `OptionAPI` | `/sdapi/v1/options` | POST used to set `sd_model_checkpoint`. |
| Progress API | `ProgressAPI` | `/sdapi/v1/progress` | GET polled during generation. |
| Output Folder | `OutputFolder` | `/streamingAssets` | Appended to `Application.dataPath`. See below. |
| Sampler | `sampler` | `Euler a` | Not read by any generator. Changing it does nothing. The sampler that is sent is the per-component **Sampler** popup. |
| Width | `width` | `512` | Copied only by material components, and only while their width or height is still negative. |
| Height | `height` | `512` | Same copy rule as width. |
| Steps | `steps` | `35` | Same copy rule. Not applied to `StableDiffusionText2Image` (that component's own default is 90) or `StableDiffusionImage2Image` (its own default is 50). |
| Cfg Scale | `cfgScale` | `7` | Same copy rule as steps. |
| Seed | `seed` | `-1` | Same copy rule. `-1` is sent through so the server can pick a seed. |

### API Settings

| Inspector label | Field | Default | Effect |
|---|---|---|---|
| Use Auth | `useAuth` | false | When true **and** user and pass are both non-empty, requests send `Authorization: Basic ...`. |
| User | `user` | empty | HTTP Basic user. Matches WebUI `--api-auth user:password`. Not a separate API-key header. |
| Pass | `pass` | empty | HTTP Basic password, stored in plain text in the asset YAML. |

If **Use Auth** is on but either field is empty, the header is omitted. The tool does not warn at request time. A later 401 or 403 from **List Models** logs `Authentication error: Server requires an API key. Please enable useAuth and set user/pass fields in your SDSettings.` The words "API key" in that sentence mean this username and password.

Auth is applied on **List Models**, on the model-switch POST, on text-to-image, on `StableDiffusionImage2Image`, and on the non-material progress poll when credentials are set. `StableDiffusionImage2Material` does not send the header and always uses the non-auth progress poll.

### URP Settings

| Inspector label | Field | Default | Effect |
|---|---|---|---|
| Use Universal Render Pipeline | `useUniversalRenderPipeline` | false | Read only by `StableDiffusionText2Material` when it creates a material. False selects shader `Standard`. True selects `Universal Render Pipeline/Lit`. Ignored by `StableDiffusionImage2Material`, which always uses `Standard`. |

Turning this on does not install URP and does not convert the project. The demo project's graphics settings are Built-in. URP install is Unity's documentation: https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@14.0/manual/InstallingAndConfiguringURP.html

## Output folder

The output root is computed as:

```text
Application.dataPath + OutputFolder
```

`Application.dataPath` is the project's `Assets` directory. It is not `Path.Combine`. The default `OutputFolder` value `/streamingAssets` therefore means `{project}/Assets/streamingAssets`.

If that string is empty or that directory does not exist, the root is replaced with `Application.streamingAssetsPath`, which is `{project}/Assets/StreamingAssets`.

Consequences:

- With the default asset, and without a folder literally named `Assets/streamingAssets`, files go to `Assets/StreamingAssets/SDImages/` or `Assets/StreamingAssets/SDMaterials/`.
- The existence check happens before the folder is created. The fallback `StreamingAssets` folder is created when missing. A folder named `streamingAssets` is used only when it already exists.
- `.gitignore` ignores `Assets/StreamingAssets*`. PNGs in the fallback location are untracked. A folder named `Assets/streamingAssets` does not match that pattern.
- An empty **Output Folder** makes the root `Application.dataPath` itself, which exists, so files are written to `Assets/SDImages` or `Assets/SDMaterials`.
- A value without a leading slash is glued onto `Assets` (`Assets` + `MyFolder` = `AssetsMyFolder`). That path usually does not exist, so the tool falls back to `StreamingAssets`.
- An absolute path is also glued onto `Assets` and will not be treated as absolute. Use a suffix that starts with `/`.

UI generators (`StableDiffusionText2Image`, `StableDiffusionImage2Image`) add a subfolder `SDImages`. Material generators add `SDMaterials`. The file name is the component `guid` plus `.png`.

Example, default settings, fallback path:

```text
{project}/Assets/StreamingAssets/SDImages/dc71e857-ed1d-44c3-997b-2d3bd7a4eb1f.png
```

The guid is a standard GUID string with hyphens, created once when the field is empty. The Inspector drawer marks it read-only. Regenerating uses the same path. `SetupFolders` deletes that file before the HTTP request is sent. A failed generation does not restore it.

## StableDiffusionConfiguration object

Add the component by searching for `StableDiffusionConfiguration` in **Add Component**. The demo scene already has a GameObject with that name.

| Inspector label | Field | Role |
|---|---|---|
| Settings | `settings` | The `SDSettings` asset. Required. A missing reference throws `NullReferenceException` when listing models or generating. |
| Samplers | `samplers` | Names shown in every generator's **Sampler** popup. Not fetched from the server. |
| Model Names | `modelNames` | Filled by **List Models**. A fresh component leaves this null until a list succeeds. |

The default sampler array, in order, is:

`Euler a`, `Euler`, `LMS`, `Heun`, `DPM2`, `DPM2 a`, `DPM++ 2S a`, `DPM++ 2M`, `DPM++ SDE`, `DPM fast`, `DPM adaptive`, `LMS Karras`, `DPM2 Karras`, `DPM2 a Karras`, `DPM++ 2S a Karras`, `DPM++ 2M Karras`, `DPM++ SDE Karras`, `DDIM`, `PLMS`.

You can delete rows you do not use or add names your WebUI actually provides. The string is sent as JSON `sampler_name`. If WebUI does not know the name, the failure comes back as a server error. This project does not validate sampler names locally. WebUI's current sampler list changes between releases; the array above is this project's default, not a promise about your WebUI.

**List Models** is drawn under the default inspector by `StableDiffusionConfigurationEditor`.

Keep one active configuration in the open scenes. Generators use `FindAnyObjectByType` with no scene argument. A disabled GameObject is not a reliable target for that search. Two active configurations make the choice arbitrary.

## Per-generator fields

These live on the generator component, not in `SDSettings`.

### Shared by the four generators

| Label | Field | Notes |
|---|---|---|
| Guid | `guid` | Read-only. File name. Editor `Update` fills it when it is empty. `Generate()` does not fill it. A call that runs before that `Update` writes a file named `.png`. |
| Prompt | `prompt` | Required. Empty prompt: **Generate** returns immediately and logs nothing. |
| Negative Prompt | `negativePrompt` | Sent as `negative_prompt`. May be empty. |
| Sampler | popup | Stored as `selectedSampler`, an index into the configuration's sampler array. Hidden from the default inspector and drawn by the custom editors. Default index 0 (`Euler a` on the default array). `StableDiffusionImage2Material` has no popup, so the index stays at 0 unless you show hidden fields. |
| Model | popup | Stored as `selectedModel`, an index into `modelNames`. Same editor limitation. Not clamped. |
| Width, Height | `width`, `height` | Clamped every Editor `Update` to 128–2048 inclusive. Not required to be a multiple of 8; WebUI may still reject sizes it does not support. |
| Steps | `steps` | No local range check. Sent as `steps`. |
| Cfg Scale | `cfgScale` | No local range check. Sent as `cfg_scale`. |
| Seed | `seed` | Sent as `seed`. `-1` asks the server to choose. Any other `long` is sent as-is. |
| Generated Seed | `generatedSeed` | Written from the response `info` JSON when that string is non-empty. Starts at `-1`. Not sent back on the next request unless you copy it into **Seed**. |

Initial width and height:

- `StableDiffusionText2Image` and `StableDiffusionImage2Image` start at 512, so their `Awake` does **not** copy `SDSettings`. A new text-to-image component starts at 90 steps. A new image-to-image component starts at 50 steps. CFG scale 7, seed `-1`.
- Both material components start at width `-1` and height `-1`. The first Editor `Awake` copies width, height, steps, CFG scale, and seed from `SDSettings` when that asset is assigned on a found configuration. If no configuration or no asset is found, the fallback is 512, 512, 50 steps, CFG 7, seed `-1`. After the copy, width is positive and is saved with the scene, so later edits to `SDSettings` are not pushed into existing components.

### Text-to-material and image-to-material only

| Label | Field | Default | Effect |
|---|---|---|---|
| Tiling | `tiling` | true | Sent as WebUI `tiling` only by `StableDiffusionText2Material`. `StableDiffusionImage2Material` ignores it and sends `tiling: false`. |
| Tiling X, Tiling Y | `tilingX`, `tilingY` | 1 | Range 1–100. Applied in Unity as `mainTextureScale = (-tilingX, -tilingY)`. The negative sign flips the texture. This is separate from WebUI's tiling flag. |
| Metallic | `metallic` | 0.1 | Range 0–1. Sets shader float `_Metallic`. |
| Smoothness | `smoothness` | 0.5 | Range 0–1. Sets shader float `_Glossiness` (Built-in Standard). Does not set URP `_Smoothness`. |
| Generate Normal Map | `generateNormalMap` | true | When true, builds an in-memory normal map after a successful generation. |
| Normal Map Strength | `normalMapStrength` | 0.5 | Sets `_BumpScale`. Range 0–10 on text-to-material, 0–100 on image-to-material. It is not the strength argument passed into the normal-map function. Text-to-material calls that function with `0.5`. Image-to-material calls it with `20`. |
| Apply Recursively | `applyRecursively` | true | If this object has no `MeshRenderer`, search active children. After a generation, copy the new shared material onto children's `MeshRenderer`s. |
| Denoising strength | `denoising_strength` | 0.65 | Image-to-material only. Range 0–1 in the Inspector. Sent as `denoising_strength`. |

`StableDiffusionImage2Image` does not show a denoising slider. Its request uses the private class default `0.75`.

Material sliders are pushed onto the current shared material from Editor `Update` when the value changes, without a new generation. That requires a `MeshRenderer` that already has a material. The code does not call `Undo.RecordObject`.

## What is not configurable

These are hardcoded on the private request classes in `SDSettings.cs` and are not Inspector fields:

- `batch_size` 1 and `n_iter` 1.
- `enable_hr` false. No high-resolution fix.
- `restore_faces` false.
- `styles` is a one-element array containing an empty string.
- Text-to-image `denoising_strength` stays 0.
- `sampler_index` stays `"Euler"` even when **Sampler** changes `sampler_name`.
- Image-to-image `resize_mode` stays 0. The mask string is not sent. A comment in the source says including `mask` caused HTTP 500, and including `script_name` caused HTTP 422, on the server the author used. There is no inpainting UI.
- No timeout, no retry count, and no cancel flag.

## Credentials and sharing

Do not commit an `SDSettings` asset that contains a real password. Do not paste the asset, or a full Console log of the JSON body, into a public issue. The generation log prints the text-to-image JSON, including the prompt. Image-to-image logs only the first 100 characters of the JSON. The password is not written to the Console by these scripts, but it is in the asset file.
