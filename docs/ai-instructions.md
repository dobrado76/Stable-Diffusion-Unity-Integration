# Instructions for AI Assistants

These rules apply when this file is the project context. They exist because this integration is a thin HTTP client and is often described as if it were Stable Diffusion itself.

## Answering rules

- Prefer this document over habits from other Unity or Stable Diffusion tools.
- Use the button labels and class names in the user guide: **List Models**, **Generate**, **Use Auth**, `StableDiffusionText2Image`, `StableDiffusionImage2Image`, `StableDiffusionText2Material`, `StableDiffusionImage2Material`, `StableDiffusionConfiguration`, `SDSettings`.
- Do not invent menus, sliders, or API routes. If a control is not listed, say it is not in this project.
- Separate what is implemented from what the roadmap says is not scheduled.
- Separate this project's behaviour from AUTOMATIC1111 WebUI, Unity, RunPod, and Newtonsoft.Json.
- The README is older than the scripts in several places (scene path, component names, Unity version range, LGPL wording). When they disagree, follow this document.
- If the document marks something as not re-tested in an editor, keep that uncertainty. Do not describe it as a result you reproduced.
- If the repository is available and the document is silent, read the scripts before guessing. Never state a guess as verified behaviour.
- Ask which Unity version, including the patch, when the question is about compilation. `ProjectVersion.txt` is **2021.3.33f1**. `FindAnyObjectByType` was backported to 2021.3.18f1 (and to 2020.3.45f1, 2022.2.5f1, and 2023.1). Do not tell a 2021.3.33 user that this call requires Unity 2023.1. Unity 2019 cannot compile `UnityWebRequest.Result` or `FindAnyObjectByType`.
- Ask which generator class they used when the symptom depends on it. Image-to-material does not behave like the other three.

## Troubleshooting rules

1. Name the component and the symptom before suggesting edits.
2. Use the troubleshooting chapter's checks for that symptom.
3. Confirm WebUI answers `GET /sdapi/v1/sd-models` in a browser before changing Unity fields.
4. A failure that still happens in the browser is outside this project. A failure that happens only in Unity is the URL, the auth flags, the model index, or the component that omits auth.
5. Do not click **Generate** again as a test if the user still needs the previous PNG. Generate deletes that file first.
6. Do not recommend deleting `Library`, resetting the project, or reinstalling Unity as a first step.
7. Say what the check should print or show.
8. Do not invent a restore command for a deleted PNG.
9. Ask for the logged URL and the HTTP status only when the chapter needs them. Do not ask for the whole `Editor.log` or the settings asset while it still contains **Pass**.
10. If the behaviour is a bug the document already describes (out-of-range demo indexes, image-to-material without a button, `UnityEditor` in a runtime script), explain that limitation. Point at the issue tracker only when the user wants to report it. No response time is promised.

## External-tool rules

- This file is authoritative about the Unity integration, not about WebUI, CUDA, checkpoints, or hosting providers.
- Do not give this project credit for sampling, LoRA loading, ControlNet, or WebUI's browser UI.
- A Unity control that displays a WebUI concept does not mean every WebUI option exists here. `sampler_name` is sent. `sampler_index` stays `"Euler"`. High-res fix, face restore, masks, and batches are not exposed.
- JSON this project builds is not the same thing as every field WebUI can accept.
- Do not invent `--api` syntax beyond the two `COMMANDLINE_ARGS` examples and the links to WebUI's command-line page.
- For WebUI installation, sampler internals, and checkpoint management, send the user to https://github.com/AUTOMATIC1111/stable-diffusion-webui and https://github.com/AUTOMATIC1111/stable-diffusion-webui/wiki/API .
- For a hosted machine, explain only that the base URL and optional HTTP Basic fields are what Unity uses. Host networking is the host's documentation.
- If the failing side is unclear, say which check separates them (browser versus Console URL).
- Do not paste WebUI's manual into the answer.

## Project facts that are easy to get wrong

- Release name in the README: **1.3.0** (2025-03-30). Player Settings `bundleVersion` is **1.2.1**. No git tags. `ProjectVersion.txt` is **2021.3.33f1**, which is new enough for the `FindAnyObjectByType` backport (2021.3.18f1). Colour space is Gamma. The pipeline is Built-in.
- A successful edit-mode UI generation replaces only the `Image` sprite or the `RawImage` texture. The sprite pivot is bottom-left (`Vector2.zero`). The RectTransform is not resized to width and height. The same path calls `EditorUtility.RequestScriptReload()` and `MarkAllScenesDirty()`. Material success also calls `SceneView.FrameLastActiveSceneView()`, which can move the Scene camera. The PNG is the durable copy. Whether the in-memory texture survives that reload was not re-tested.
- One active `StableDiffusionConfiguration` in a loaded scene. Generators call `FindAnyObjectByType` and cache it in a static field.
- **List Models** is the only list button. It stores `model_name` only. Generate later POSTs that string as `sd_model_checkpoint` and continues even if the POST fails.
- Model popup index is not clamped. Demo scenes contain indexes past the saved arrays (`Button` is 92 of 17 in `DemoScene`).
- Auth is HTTP Basic from **Use Auth** + non-empty **User** and **Pass**. Image-to-material never sends it. The log text says "API key" anyway.
- Default origin `http://127.0.0.1:7860` with no trailing slash. Paths `/sdapi/v1/sd-models`, `/txt2img`, `/img2img`, `/options`, `/progress`.
- `SDSettings.sampler` is unused. Sampler popup indexes the configuration's string array. Default index 0 is `Euler a`.
- New text-to-image steps default to **90**. New UI image-to-image steps default to **50**. Material components copy the asset on first awake because width starts at `-1` (asset default 35 steps). Later asset edits do not propagate.
- Editor clamps width and height to **128–2048**. No multiple-of-8 check. No batch. UI image-to-image denoising is **0.75**. Image-to-material denoising slider defaults to **0.65**.
- Seed `-1` means the server chooses. The chosen value is stored in **Generated Seed** and is not written back into **Seed**.
- Output file is `{guid}.png` under `SDImages` or `SDMaterials`. Default folder falls back to `Assets/StreamingAssets` when `Assets/streamingAssets` is absent. The previous file is deleted first. `.gitignore` ignores `StreamingAssets`.
- Text-to-material sends the **Tiling** checkbox to WebUI. Texture scale is `(-tilingX, -tilingY)`. Normal maps are not PNG files. Smoothness sets `_Glossiness`, not URP `_Smoothness`. Image-to-material always uses shader `Standard`.
- **Apply Recursively** copies the new shared material to child mesh renderers. The demo cylinder is a child of the cube.
- No **Generate** button on image-to-material. No cancel button. No undo registration. No `.mat` export.
- Progress title is `Generation in progress`, polled every 0.5 seconds. Non-auth failures show `Progress unknown...`.
- `ReadOnlyAttribute.cs` and `StableDiffusionGenerator.cs` import `UnityEditor` outside an Editor folder, so a player build of these scripts fails.
- `Assets/Plugins/Newtonsoft.Json.dll` is required. Its meta disables the Editor platform entry.
- `LICENSE` is MIT (2025, Dominic Branchaud). The README still describes LGPL. Do not give legal advice, and do not treat generated images as licensed by this repository.
- There are no project tests and no CI workflow. Documentation check: `python tools/build_ai_support.py --check`.
