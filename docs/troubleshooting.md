# Troubleshooting

Match the Console string first. The exact messages are listed in [Reference](reference.md#response-handling). Prefer the smallest check that does not delete data. **Generate** deletes the existing `{guid}.png` before the server answers, so do not click it again as a test until you have copied any PNG you care about.

WebUI-internal faults (CUDA out of memory, a bad checkpoint file, a sampler crash inside the Python process) are reported here only as "the body WebUI returned". Fix those with WebUI's own issue tracker and docs, not by changing Unity sliders at random.

## Generate does nothing

**Symptom:** **Generate** clicks, no progress bar, no new Console error.

**Causes:**

- **Prompt** is empty or only whitespace is not the check; the check is `string.IsNullOrEmpty`. A prompt of spaces is sent. A truly empty prompt returns silently.
- UI image-to-image has no **Input Texture**. That also returns silently.
- `generating` is still true from an earlier run that threw. The flag is invisible in the default inspector.

**Checks:** Confirm the prompt length, the input texture, and whether a previous red exception mentioned this component.

**Resolution:** Fill the prompt or texture. Activate the GameObject and enable the component. If a previous run threw or you disabled the object mid-request, reload the scene or trigger a script recompile so the flag resets, then generate once.

**Expected result:** A progress bar appears within a second. Text-to-image, UI image-to-image, and text-to-material log `Sending request to:`. Image-to-material does not log that line.

## Model popup throws when you select the object

**Symptom:** Inspector exception as soon as you select a generator. Often `NullReferenceException` or `ArgumentNullException` from `EditorGUILayout.Popup`.

**Causes:** No active `StableDiffusionConfiguration` in loaded scenes, or `modelNames` is still null because **List Models** has never succeeded on this object.

**Checks:** Search the hierarchy for `StableDiffusionConfiguration`. Confirm it is active. Look at **Model Names**. Null and a size of 0 are different; null crashes the popup.

**Resolution:** Add or enable the configuration, assign **Settings**, start WebUI, click **List Models**. You can also set **Model Names** size to 0 in the Inspector to get past the null popup, then list models.

**Expected result:** **Sampler** and **Model** draw, and **Model** has one entry per checkpoint `model_name`.

## List Models fails immediately

**Symptom:** `Connection error`, `API endpoint not found`, `Authentication error`, or `Invalid response format`.

**Causes and checks:**

1. WebUI is not running or the port differs. Open `{Stable Diffusion Server URL}/sdapi/v1/sd-models` in a browser. You should see a JSON array.
2. The URL has a trailing slash, so the request path contains `//`.
3. `--api` was not set. The browser shows HTML. The Unity message for a non-JSON body is `Invalid response format`.
4. `--api-auth` is set but **Use Auth** is off or User/Pass do not match. Status 401 or 403. The log says "API key"; the fields to fill are **User** and **Pass**.
5. A firewall or a remote host blocked the port. The generic log still mentions port 7860. Trust the URL printed in `Requesting models from:` more than that sentence.
6. HTTP 404 tries `{server}/api/sd-models`. Stock WebUI will not implement that path. A 404 on the primary path means the primary URL is wrong or the API is off, not that you should depend on the fallback.

**Resolution:** Fix the server or the URL first. Change Unity settings only after the browser check works. Then click **List Models** again.

**Expected result:** `Successfully loaded N models` and a filled **Model Names** array.

This split matters: if the browser fails, the defect is outside this project. If the browser returns JSON and Unity still fails, compare the logged URL and whether Basic auth is required.

## Wrong checkpoint or an out-of-range model

**Symptom:** `IndexOutOfRangeException` after **Generate**, or an image that clearly used a different checkpoint than the one you picked. The demo `Button` stores model index 92 while `DemoScene` only has 17 saved names. The no-post-processing scene stores 3 names and several indexes above 2.

**Causes:** `selectedModel` is a raw integer. **List Models** replaces the name array and does not remap indexes. The set-checkpoint call sends `model_name`, not the WebUI title, and a failed set does not cancel the image POST.

**Checks:** Count **Model Names**. Compare **Model**'s stored index (switch the Inspector to Debug mode if the popup hides it). Read the Console for `Failed to set model` or `All attempts to set model failed`. Look at WebUI's own model indicator.

**Resolution:** Click **List Models**, then choose the model again on every generator so the index is inside the new array. If the index is valid and WebUI does not switch, this project has no further switch mechanism. Use WebUI's API docs for the options payload it expects, and compare that with the logged JSON `{"sd_model_checkpoint":"..."}`.

**Expected result:** No exception, and WebUI shows the checkpoint you intended before the image returns.

**Data note:** the exception happens after the old PNG is deleted.

## HTTP errors during Generate

**Symptom:** `Request error:` plus a code.

| Code or text | Likely side | What to do |
|---|---|---|
| Connection error, status 0 | Server down, wrong URL, TLS mismatch (`https` vs `http`), or the machine cannot route to the host | Repeat the browser check against the exact URL logged in `Sending request to:` |
| 401 or 403 | Auth | Fill **Use Auth**, **User**, and **Pass** to match `--api-auth`. Image-to-material never sends auth; a 401 from that component is expected when the server requires auth |
| 404 | Wrong API path | Restore the default `/sdapi/v1/...` paths unless you intentionally proxy them |
| 405 | Method or proxy rejection | The project logs extra 405 text aimed at hosted proxies. Confirm `--api` and the proxy's allowed methods in the host's docs. This repository does not configure RunPod |
| 422 or 500 with a JSON `detail` | WebUI rejected the payload (size, sampler name, missing init image, and similar) | Read the logged response body. Adjust the field WebUI names. Do not invent extra Unity controls for options this project does not send |
| HTML body | You hit a web page, not the API | Remove a trailing path that points at the Gradio UI instead of the API origin |

**Expected result:** status success, a body with an `images` array, a PNG on disk.

## No image in the response

**Symptom:** `No image was returned by the server` or the image-to-material wording `No image was return by the server`.

**Cause:** The HTTP call succeeded and the JSON parsed, but `images` was missing or empty. That is a server-side response shape this project does not repair.

**Checks:** Read `Full response:` in the Console. If the body is an error object, treat it as WebUI's message.

**Resolution:** Fix the condition WebUI reported. There is no second image slot to fall back to.

## Progress bar stuck or "Progress unknown..."

**Symptom:** The bar stays up after the click, or it sits on `Progress unknown...`.

**Causes:** The progress GET failed (the image request may still be running). An exception skipped `ClearProgressBar`. Image-to-material's auth-less poll failed the parse. Play Mode was stopped mid-request.

**Checks:** Look for `Failed to get progress` or `Error parsing progress`. See whether `Sending request to:` was followed later by `Response received` or `Request error`.

**Resolution:** Wait if the image request is still running; a large step count on a slow GPU can take minutes and the tool has no timeout. If the auth progress poll threw while parsing, that component stops updating the bar until a domain reload; the image request can still finish. If the request already ended and the bar remains, enter Play Mode with an active `StableDiffusionText2Material` or `StableDiffusionImage2Material` in the scene (their `Start` clears the bar), then stop Play Mode. You can also trigger another generation that reaches its cleanup. There is no cancel control. Disabling the component stops the coroutine and skips cleanup. It does not call `Abort` on the request, and `generating` stays true.

**Expected result:** The bar closes. A successful run also leaves a PNG.

## Input image errors

**Symptom:** `Input Image ... isn't readable` or `Texture ... is not readable` / `EncodeToPNG` exception from image-to-material.

**Resolution for UI image-to-image:** Select the texture, enable **Read/Write**, disable compression if the help box's "uncompressed" note matters for your file, click Apply.

**Resolution for image-to-material:** The source is the renderer's current main texture, not a field you can swap in the generator. It must be a `Texture2D` with Read/Write. Render textures and models' non-readable imports throw. This component also will not send auth.

**Expected result:** For the UI component, the readable error disappears and the next **Generate** logs `Sending request to:`.

## Material looks wrong

**Symptom:** Pink material, flat lighting, flipped texture, smoothness ignored, or children changed together.

**Causes:**

- **Use Universal Render Pipeline** is on but the URP shader is not installed, or it is off and the project is URP. Pink means the shader is missing. Image-to-material always assigns `Standard`, which is wrong for a URP project.
- Smoothness wrote `_Glossiness`. URP Lit will not follow that property.
- **Tiling X** / **Tiling Y** are applied as a negative scale, so the texture is mirrored.
- **Apply Recursively** assigned the parent's new material to children. The demo cube does this to its cylinder.
- **Normal Map Strength** only changes `_BumpScale`. It does not rebuild the normal map. Rebuild by generating again, or toggle **Generate Normal Map** only after a texture exists in memory; `GenerateNormalMap` returns immediately when `generatedTexture` is null, which it is after a domain reload.
- Normal maps are not saved as PNG files. After a restart the bump slot may be empty even though the colour PNG exists.

**Resolution:** Pick the shader path that matches the project. For URP, only text-to-material consults the checkbox, and smoothness still will not map. Assign the saved albedo PNG manually if the scene lost its texture reference. Turn off **Apply Recursively** before generating a parent if children should keep their materials.

**Expected result:** The renderer uses a Standard (or URP Lit) material whose main texture is the new image. `_BumpMap` is set only while that Editor session still holds `generatedTexture`.

## The PNG is missing

**Symptom:** Generation seemed to run, but there is no file, or the previous file vanished.

**Checks:**

1. Read the guid on the component.
2. Look in `Assets/StreamingAssets/SDImages` and `SDMaterials`, and also in `Assets/streamingAssets` if you created that folder.
3. Remember `.gitignore` hides `StreamingAssets` from git status. It does not hide it from the Project window. If the Project window does not refresh, right-click the folder and choose Refresh, or focus Unity so `AssetDatabase.Refresh` from a successful edit-mode run can finish.
4. If the Console shows an exception after `Sending request to:` never appeared, the delete step may already have removed the previous file and the new one was never written.

**Resolution:** Copy important PNGs before generating again. There is no trash folder and no restore command in this project. If the new bytes were written, the path logged indirectly by the guid is the location. The code does not log the full output path on success.

## Authentication works in the browser and fails in Unity

**Symptom:** Browser with credentials succeeds; Unity gets 401.

**Checks:** **Use Auth** enabled, both fields non-empty, no extra space, same asset assigned on the configuration you are actually hitting. Image-to-material cannot succeed against an authenticated server with the current code.

**Resolution:** Fix the three fields and use a component that sends the header (`StableDiffusionText2Image`, `StableDiffusionImage2Image`, or `StableDiffusionText2Material`).

## Editor freeze while setting the model

**Symptom:** Unity stops pumping the UI for a while after **Generate**, before the progress bar moves.

**Cause:** If the `UnityWebRequest` options call fails, `SetModelAsync` calls `HttpWebRequest.GetResponse()`, which blocks the Editor thread. A dead host can stall the Editor until the operating-system timeout.

**Resolution:** Make the options URL reachable. Avoid the fallback by keeping WebUI's `/sdapi/v1/options` working. There is no timeout field to lower.

## Compile errors

**Symptom:** `FindAnyObjectByType` does not exist, or `UnityWebRequest.Result` does not exist.

**Cause:** The editor is older than one of those APIs. `UnityWebRequest.Result` exists from Unity 2020.2. `FindAnyObjectByType` was backported to 2020.3.45f1, 2021.3.18f1, 2022.2.5f1, and 2023.1.0a20, according to Unity staff (https://discussions.unity.com/t/findobjectsbytype-and-its-siblings/910153/1). The checked-in project is **2021.3.33f1**, which is newer than 2021.3.18f1. A missing-method error on that exact editor was not reproduced here. Unity 2019 fails on both APIs. A 2021.3 patch older than 18f1 fails on `FindAnyObjectByType` only.

**Resolution:** Use 2021.3.33f1 or another editor at or after the backport for that release line. Unity 6 also has the method. This repository does not include a compatibility shim. Do not treat the README's Unity 2019 sentence as something the current scripts can compile.

**Symptom:** `Newtonsoft` namespace missing.

**Cause:** `Assets/Plugins/Newtonsoft.Json.dll` missing or disabled for Editor. The meta file sets the Editor platform entry to disabled.

**Resolution:** Restore the DLL from the repository and enable the Editor platform on the importer. This project does not vendor an alternative JSON library.

**Symptom:** Player build fails on `UnityEditor` inside `StableDiffusionGenerator.cs` or `ReadOnlyAttribute.cs`.

**Cause:** Those files sit outside an `Editor` folder and import `UnityEditor` without `#if UNITY_EDITOR`.

**Resolution:** This tool is an Editor workflow. The current layout does not support a player build of a project that contains these scripts unchanged. There is no asmdef that excludes them from the player. No supported build setting fixes that.

## Demo scene missing script

**Symptom:** `PostProcessingVolume` shows a missing script on `DemoScene`.

**Cause:** `com.unity.postprocessing` is not in `Packages/manifest.json`.

**Resolution:** Install the Post Processing package, reopen the scene. The volume is global and references `PostProcessingVolume Profile.asset`. If the profile link is lost, assign that asset to the volume's profile. Or open `DemoScene-NoPostProcessing` instead. Generation does not use the volume.

## Play Mode looked successful and then reverted

**Symptom:** The mesh or UI updated while playing, and stopping Play Mode restored the old look.

**Cause:** Scene changes during Play Mode are discarded by Unity. The PNG on disk is kept. `InspectorRefresh.ForceInspectorToRefresh` is never called.

**Resolution:** Generate in Edit Mode when you want the scene to keep the assignment. To use a PNG made during Play Mode, assign the file onto the material or graphic after you leave Play Mode.

## What to collect if you open an issue

Issues: https://github.com/dobrado76/Stable-Diffusion-Unity-Integration/issues

Include:

- Unity editor version (**Help > About Unity**).
- Whether you opened this repository or imported the unitypackage.
- The component class name (`StableDiffusionText2Image`, `StableDiffusionImage2Image`, `StableDiffusionText2Material`, or `StableDiffusionImage2Material`).
- The URL line from `Requesting models from:` or `Sending request to:` with any userinfo removed.
- The Console error text and HTTP status.
- What you expected and what happened.

Remove passwords, `Authorization` headers, private prompts, local usernames, and full absolute home-directory paths. A few dozen lines of Console output are enough. Do not attach the whole `Library` folder, a full `Editor.log` unless asked, or an `SDSettings` asset that still contains **Pass**.

This repository does not promise a reply. WebUI bugs belong on https://github.com/AUTOMATIC1111/stable-diffusion-webui/issues with the response body, once the logged request URL is the URL you intended.
