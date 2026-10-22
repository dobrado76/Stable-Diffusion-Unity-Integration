# Getting started

This is the installation and first-generation path for the current scripts. WebUI's own install steps belong to that project: https://github.com/AUTOMATIC1111/stable-diffusion-webui

## Prerequisites

- A Unity project on the Built-in Render Pipeline for the included demo scenes. `ProjectSettings/GraphicsSettings.asset` has no custom render pipeline asset, and the demo materials use the Standard shader. URP is optional and only partially supported.
- An editor that contains the APIs these scripts call. `UnityWebRequest.Result` replaced `isNetworkError` / `isHttpError` in Unity 2020.2, so Unity 2019 does not compile this code. `FindAnyObjectByType` is in the 2023.1 API and was backported. A Unity staff reply lists it from **2020.3.45f1**, **2021.3.18f1**, **2022.2.5f1**, and **2023.1.0a20** onward: https://discussions.unity.com/t/findobjectsbytype-and-its-siblings/910153/1 . `ProjectSettings/ProjectVersion.txt` records **2021.3.33f1**, which is newer than 2021.3.18f1, so that call is expected to compile in the editor named by the project file. This documentation pass did not launch Unity. The README's claim that 2019 works does not match the source. Patches older than the backports above, and Unity 2022.1 (not in that staff list), are expected to fail on `FindAnyObjectByType`. There is no compatibility shim.
- [AUTOMATIC1111 WebUI](https://github.com/AUTOMATIC1111/stable-diffusion-webui) reachable over HTTP, started with the API enabled. Official API overview: https://github.com/AUTOMATIC1111/stable-diffusion-webui/wiki/API . Command-line flags such as `--api` and `--api-auth` are documented by WebUI: https://github.com/AUTOMATIC1111/stable-diffusion-webui/wiki/Command-Line-Arguments-and-Settings
- At least one checkpoint installed in that WebUI. This repository does not provide one.
- `Assets/Plugins/Newtonsoft.Json.dll` (the file is in the repository, about 662 KB). The scripts deserialize JSON with `Newtonsoft.Json`.

## 1. Start the WebUI API

On a local Windows WebUI, the usual place for flags is `webui-user.bat` in the WebUI folder, as a `COMMANDLINE_ARGS` variable. Two patterns the README uses:

Without authentication:

```bat
set COMMANDLINE_ARGS=--api
```

With HTTP Basic authentication and listening beyond localhost:

```bat
set COMMANDLINE_ARGS=--listen --api --api-auth YOUR_USERNAME:YOUR_PASSWORD
```

`--listen` exposes the process on the network. Use a password you can rotate, and do not commit it. WebUI's own docs explain the security implications of `--listen`.

Confirm the API before involving Unity. In a browser on the same machine, open:

`http://127.0.0.1:7860/sdapi/v1/sd-models`

A working API returns a JSON array. An HTML page, a connection error, or 404 means Unity will fail the same way. The default port is 7860 only because that is WebUI's default and the value stored in `DefaultLocalSDSettings`. If you changed WebUI's port, use that port instead.

## 2. Get the Unity project

Two ways are described in the README. Both are present in the repository.

**Open this repository as the Unity project.** Clone https://github.com/dobrado76/Stable-Diffusion-Unity-Integration and open the repository root in Unity. The demo scenes and the default settings asset are included.

**Import the package into another project.** The file `Stable-Diffusion-Unity-Integration.unitypackage` is at the repository root. Use **Assets > Import Package > Custom Package**. Importing the package into a fresh project does not bring along every texture that was generated later inside this demo; the package itself was not unpacked while writing this documentation. If the package and the `Scripts` folder ever disagree, the `Scripts` folder is the source these docs describe. You still need `Newtonsoft.Json.dll` if the package does not include it. The plugin importer meta sets the Editor platform entry to `enabled: 0` while Any Platform is enabled. If the Console reports that `Newtonsoft` cannot be found, select `Assets/Plugins/Newtonsoft.Json.dll` and enable it for **Editor**.

`Packages/manifest.json` does not include Post Processing or URP. TextMeshPro, Timeline, and the Unity Test Framework are listed; the Test Framework is unused. There is no `Tests` folder.

## 3. Open a demo scene

The README path `StableDiffusionIntegration/Scenes/DemoScene.unity` is outdated. The scenes are:

- `Assets/Stable-Diffusion-Unity-Integration/Scenes/DemoScene.unity`
- `Assets/Stable-Diffusion-Unity-Integration/Scenes/DemoScene-NoPostProcessing.unity`

`DemoScene` contains a global Post Processing volume (`isGlobal: 1`, weight 1) whose script guid is the Post Processing Stack v2 volume. That package is not in `manifest.json`. Until you install **Post Processing** from Package Manager, that object shows a missing script. The profile asset is `Assets/Stable-Diffusion-Unity-Integration/Scenes/PostProcessingVolume Profile.asset` and the volume's `sharedProfile` points at it. Install steps for the package are Unity's: https://docs.unity3d.com/Packages/com.unity.postprocessing@3.4/manual/Installation.html . Reopen the scene after the package imports. The post-processing volume is not required to generate images. Use `DemoScene-NoPostProcessing` to avoid it.

The demo is a small room (floor, walls, ceiling, table, paintings, a canvas with UI examples) plus a GameObject named `StableDiffusionConfiguration`. Its Settings field references `DefaultLocalSDSettings`.

## 4. Point settings at your server

Select `StableDiffusionConfiguration`. In the Inspector, under the settings asset:

- **Stable Diffusion Server URL** should be the server origin with no trailing slash, for example `http://127.0.0.1:7860`. API paths in the same asset already start with `/`. A trailing slash produces a double slash in the request URL.
- Leave the API path fields at their defaults unless your proxy remaps them. Defaults are `/sdapi/v1/sd-models`, `/sdapi/v1/txt2img`, `/sdapi/v1/img2img`, `/sdapi/v1/options`, and `/sdapi/v1/progress`.
- For `--api-auth`, enable **Use Auth** and set **User** and **Pass** to the same pair. Both must be non-empty or the tool sends no `Authorization` header. The password is stored in plain text in the asset.

## 5. List models

Click **List Models** on the configuration Inspector. The button label is plural. The Console should log `Successfully loaded N models`. The **Model Names** array on the configuration object fills with each model's `model_name` from the JSON (not the filename and not the title).

The demo scenes already contain a saved model-name snapshot from a developer's server, and saved **Model** indexes on individual objects. Those names are not checkpoints shipped by this project. `DemoScene` stores 17 names, and the `Button` object's model index is `92`, which is outside 0–16. `DemoScene-NoPostProcessing` stores 3 names, and several objects store indexes such as 13, 26, and 234. After you click **List Models**, re-select **Model** on each generator before clicking **Generate**. `Generate()` does not clamp the index. An out-of-range index throws before a new image is written, and the previous PNG for that component has already been deleted.

## 6. Generate one image

Pick a generator that has a **Generate** button:

| Object in the demo | Component | Also requires |
|---|---|---|
| `Text-2-RawImage`, `Image`, `Button` | `StableDiffusionText2Image` | A non-empty **Prompt**. `Image` or `RawImage` on the same GameObject if you want the result shown |
| `Image-2-RawImage` | `StableDiffusionImage2Image` | A non-empty **Prompt** and a readable **Input Texture**. The demo assigns `Assets/Stable-Diffusion-Unity-Integration/Input Image/horse_input.png`, which is imported with Read/Write enabled |
| `Floor`, `Walls`, `Ceiling`, `Table`, `Cube`, `Cylinder`, `Painting1`, `Painting2` | `StableDiffusionText2Material` | A non-empty **Prompt**, and a `MeshRenderer` on this object or, when **Apply Recursively** is on, on a child |

`StableDiffusionImage2Material` is not used in the demo and has no **Generate** button.

Click **Generate**. A progress bar titled `Generation in progress` appears while the request runs. On success:

- A PNG named `{guid}.png` is written under `Assets/StreamingAssets/SDImages/` (UI generators) or `Assets/StreamingAssets/SDMaterials/` (material generators), unless you created `Assets/streamingAssets` or changed **Output Folder**. See [Output paths](configuration.md#output-folder).
- The UI graphic or the mesh material updates in the scene.
- **Generated Seed** updates when the response `info` string contains a seed.
- The scene is marked dirty. Save the scene if you want Unity to keep the in-scene texture reference.

The `guid` field on the component is the file name. It is read-only in the Inspector.

## 7. Confirm success

All of the following should be true:

- The Console has no `Request error`, `No image was returned by the server`, or exception stack from the generator.
- The progress bar closes.
- The PNG exists on disk next to the guid.
- The UI element or mesh shows the new image.

If the button appears to do nothing, the prompt is empty, a generation is already flagged as running, or (for image-to-image) **Input Texture** is empty. Those cases log nothing. See [Troubleshooting](troubleshooting.md).

## Expected first-run failures

- WebUI not running, wrong port, or missing `--api`: connection error or 404 in the Console when listing models.
- `--api-auth` without **Use Auth**: HTTP 401 or 403 and the message `Authentication error: Server requires an API key...`
- Demo **Model** index out of range: exception when generating, and the previous PNG for that guid is already gone.
- `DemoScene` missing-script warning on `PostProcessingVolume` until the Post Processing package is installed. Generation does not need that package.
