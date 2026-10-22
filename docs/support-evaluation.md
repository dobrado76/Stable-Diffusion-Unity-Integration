# Support evaluation cases

Maintainer checklist for `AI-SUPPORT.md`. These questions were written from the scripts, the demo scene YAML, Console strings, and `SDSettings` defaults. They are not a copy of the FAQ headings. This file is not bundled into `AI-SUPPORT.md`.

No separate AI assistant was given only `AI-SUPPORT.md` and scored against this list. A self-review while writing the manual is not that test. Do not treat this file as a measured accuracy rate.

Each case lists the section titles as they appear in the bundled manual.

## Answerable cases

### E1. First successful generation

**Question:** I cloned the repo and WebUI is running with `--api` on port 7860. What do I click, in order, to get one image onto the demo bison RawImage?

**Expected answer:** Open `Assets/Stable-Diffusion-Unity-Integration/Scenes/DemoScene.unity`. Select `StableDiffusionConfiguration`, confirm the URL is `http://127.0.0.1:7860` with no trailing slash, click **List Models**, re-select **Model** on `Text-2-RawImage` (its saved index is not trustworthy), leave the prompt `a bison in a field` or type one, click **Generate**. Expect a progress bar, a PNG under `Assets/StreamingAssets/SDImages/` named with that component's guid, and the RawImage updated.

**Sections:** Getting started; User guide → Text-to-image for UI; User guide → Demo scene contents.

**Avoid:** Telling the user to open `StableDiffusionIntegration/Scenes/DemoScene.unity`. Inventing a menu named Stable Diffusion.

**Pass:** Names **List Models** and **Generate**, the real scene path, and the PNG location. Mentions re-selecting the model.

**Uncertainty:** Unity 2019, and editors older than the `FindAnyObjectByType` backports (2021.3.18f1 on the 2021.3 line), fail to compile. Do not tell a 2021.3.33f1 user to upgrade to 2023.1 because of that API.

### E2. Empty click

**Question:** I press Generate and the Console stays empty. No progress bar.

**Expected answer:** Prompt is empty, UI image-to-image has no input texture, the object is inactive, or `generating` is stuck true after an earlier exception. The first three are silent. Ask which component, whether the prompt is empty, and whether a red exception appeared earlier. Do not start by deleting Library.

**Sections:** Troubleshooting → Generate does nothing; FAQ questions 39 and 84; User guide scene setup.

**Avoid:** Claiming a hidden error dialog exists.

**Pass:** Lists the silent cases and the Console line `Sending request to:` as the sign that a run actually started.

### E3. Readable texture

**Question:** The Console says `Input Image horse_input isn't readable. Go to texture import settings and tick the Read/Write box`.

**Expected answer:** This is `StableDiffusionImage2Image` refusing to call `EncodeToPNG`. Select the texture, enable Read/Write, Apply, generate again. The horse sample ships readable; this error means that import setting was turned off or a different texture is assigned. Do not change the prompt to "fix" it.

**Sections:** Troubleshooting → Input image errors; User guide → Image-to-image for UI; Reference exact string.

**Avoid:** Describing a project setting called "Make Readable" that is not the Unity texture importer.

**Pass:** Quotes the recovery as the texture importer Read/Write checkbox and says the request was not sent.

### E4. Auth wording

**Question:** List Models says `Authentication error: Server requires an API key. Please enable useAuth and set user/pass fields in your SDSettings.` I do not have an API key, I have a WebUI `--api-auth` user and password.

**Expected answer:** Those two fields are the user and password. Enable **Use Auth** and fill **User** and **Pass**. The header is HTTP Basic. There is no separate key field. The word "API key" in the log is this project's wording, not a different credential type.

**Sections:** Configuration → API Settings; FAQ questions 20 and 23; Reference message table.

**Avoid:** Inventing an API Key text box or a bearer token.

**Pass:** Maps the log sentence onto **Use Auth**, **User**, and **Pass**, and mentions both must be non-empty.

### E5. Demo Button index

**Question:** On DemoScene, Generate on the Button throws IndexOutOfRangeException. I have not changed anything.

**Expected answer:** The Button's stored model index is 92 and the scene's saved model name list has 17 entries. The PNG for that guid is already deleted. Click **List Models**, choose a model again, then generate. Do not tell the user to download the checkpoint names saved in the scene.

**Sections:** Troubleshooting → Wrong checkpoint or an out-of-range model; Getting started step 5; FAQ question 72.

**Avoid:** Claiming the tool ships those checkpoints, or that the index is clamped.

**Pass:** States 92 versus 17 and the delete-before-request behaviour.

### E6. Steps default

**Question:** I added a new StableDiffusionText2Image and my settings asset says 35 steps. The component shows 90. Is that a bug?

**Expected answer:** No. Text-to-image initializes steps to 90 and does not copy the asset, because width and height start at 512 and the copy only runs when one of them is negative. Material components do copy the asset (default 35) on first awake. Changing the asset later does not push into existing components.

**Sections:** Configuration → Per-generator fields; FAQ questions 27 and 28.

**Avoid:** Saying all components inherit the asset on every Generate.

**Pass:** Distinguishes text-to-image (90) from material components (copy once).

### E7. Output path and git

**Question:** Generate succeeded but git status does not show the PNG, and I cannot find it next to the script.

**Expected answer:** Look in `Assets/StreamingAssets/SDImages` or `SDMaterials` for `{guid}.png`. The default output folder string is `/streamingAssets`, and if that directory is absent the code uses `Assets/StreamingAssets`. `.gitignore` ignores `StreamingAssets`, so git stays quiet. The guid is the read-only field on the component. Success does not log the full path.

**Sections:** Configuration → Output folder; FAQ questions 60–62; Troubleshooting → The PNG is missing.

**Avoid:** Telling them to search the WebUI `outputs` folder for the Unity copy. WebUI may also have written its own file; this project does not read it.

**Pass:** Gives the StreamingAssets fallback and the gitignore reason.

### E8. Deleted PNG

**Question:** The server was down. Generate failed. The previous PNG for this object is gone. How do I restore it from the tool?

**Expected answer:** You cannot. `SetupFolders` deletes the file before the request. There is no trash and no undo registration. Restore from the user's own copy or from git if it had been committed. StreamingAssets files are gitignored by default, so git often will not have them.

**Sections:** User guide → Operations that destroy data; Troubleshooting → The PNG is missing; FAQ question 66.

**Avoid:** Inventing a Recycle Bin menu or an automatic `.bak`.

**Pass:** Says the tool cannot restore it, and says why the file was removed.

### E9. URP smoothness

**Question:** I turned on Use Universal Render Pipeline. The albedo appears but the Smoothness slider does nothing. Image-to-material objects are pink.

**Expected answer:** Text-to-material writes `_Glossiness`, while URP Lit uses `_Smoothness`. The flag only selects the shader name `Universal Render Pipeline/Lit` inside text-to-material. Image-to-material always creates a Standard material, which is pink when the project is on URP. HDRP is not implemented. This was not visually re-tested in a URP project; the property names are what the code sets.

**Sections:** User guide → Text-to-material; FAQ questions 52 and 53; Roadmap limitations.

**Avoid:** Claiming the checkbox converts the whole project or sets `_Smoothness`.

**Pass:** Separates the shader switch from the property that is actually written, and the image-to-material exception.

### E10. Player build

**Question:** I tried to build the demo as a Windows player and the compiler says `UnityEditor` does not exist in `StableDiffusionGenerator.cs`.

**Expected answer:** Those scripts sit outside an Editor folder and import `UnityEditor` without a scripting define. A player build fails. The intended workflow is Editor generation, then ordinary textures in the game. There is no Player Settings toggle that excludes them. This was inferred from the source and not reproduced by making a build.

**Sections:** Development → Player builds; FAQ question 77; Troubleshooting → Compile errors.

**Avoid:** Saying the README's "no runtime dependency" means the player compile succeeds.

**Pass:** Names the file and the folder layout, and does not pretend a build was run.

### E11. Sampler ignored

**Question:** I selected DPM++ 2M in the Sampler popup and the picture looks like Euler. Is there another sampler field?

**Expected answer:** The popup sets `sampler_name`. The JSON also sends `sampler_index` hardcoded as `Euler`, and this project never updates it. `SDSettings.sampler` is unused. Which field WebUI honours is WebUI's behaviour and can change; point at WebUI's API docs rather than asserting which field won. Suggest comparing a direct API call.

**Sections:** User guide → Samplers; Reference text-to-image JSON; FAQ question 35.

**Avoid:** Claiming Unity definitely sent only Euler, or debugging WebUI's scheduler source from memory.

**Pass:** States both JSON fields and stops at the boundary.

### E12. Play Mode revert

**Question:** I generated on the floor while in Play Mode. It looked right. I stopped Play Mode and the old material came back. The PNG is on disk.

**Expected answer:** Play Mode scene changes are discarded. The file remains. Generate in Edit Mode to keep the assignment, or assign the PNG by hand after exiting Play Mode. `InspectorRefresh.ForceInspectorToRefresh` is never called.

**Sections:** User guide → Play Mode; Troubleshooting → Play Mode looked successful and then reverted; FAQ question 64.

**Avoid:** Telling them the PNG was not written.

**Pass:** Separates the file (kept) from the scene assignment (reverted).

## Deliberately unanswerable or misleading cases

These eight cases pass only when the answer refuses to invent a feature, asks for the missing fact, or sends the user to an upstream project.

### U1. Cancel button

**Question:** Where is the Cancel Generation button?

**Expected answer:** There is no cancel control. The request is not aborted by disabling the component. Say that, and mention the stuck progress-bar recovery only if they already have a stuck bar.

**Sections:** User guide → Progress bar; Roadmap → Known limitations.

**Hallucination to avoid:** A button, a keyboard shortcut, or `Escape` to cancel.

**Pass:** States the control does not exist.

**Uncertainty:** This is a documented absence, not an unknown. Do not hedge as "it might be in a menu."

### U2. LoRA weight

**Question:** Which slider sets the LoRA weight to 0.8?

**Expected answer:** This project has no LoRA control. It does not send extra network parameters. WebUI may support LoRAs in the prompt syntax; that syntax is WebUI's, and this tool just forwards the prompt string. Do not invent a Weight slider.

**Sections:** Product → What it does not do; Instructions for AI Assistants → External-tool rules.

**Hallucination to avoid:** An Inspector field for LoRA.

**Pass:** Refusal plus the prompt-forwarding boundary.

### U3. Export material

**Question:** Which menu writes the generated material to a `.mat` file?

**Expected answer:** None. The code assigns `new Material` in memory and writes only `{guid}.png`. No `.mat` is created.

**Sections:** User guide → Saving and what survives a restart; FAQ question 65.

**Hallucination to avoid:** Assets > Create > Material From Generation, or an automatic save beside the PNG.

**Pass:** Names the PNG as the only file and the missing `.mat`.

### U4. Ambiguous failure

**Question:** It doesn't work. Fix it.

**Expected answer:** Ask which component, whether **List Models** or **Generate** was clicked, the Unity version, and the Console line if one exists. Do not recommend reinstalling Unity, deleting `Library`, or changing CFG scale. Do not pick a single root cause.

**Sections:** Instructions for AI Assistants → Troubleshooting rules; Troubleshooting introduction.

**Hallucination to avoid:** A specific fix with no symptom.

**Pass:** Requests only those facts and withholds a fix.

### U5. Their checkpoint list

**Question:** Is `my-private-model-v3` installed and selected in my project?

**Expected answer:** This repository cannot know the user's WebUI disk. **List Models** shows `model_name` values from their server. The names saved in the demo scene are someone else's snapshot. Ask them to click **List Models** and read **Model Names**.

**Sections:** Getting started step 5; FAQ question 73.

**Hallucination to avoid:** Yes or no about a checkpoint that is not in the repo.

**Pass:** Refuses and points at **List Models**.

### U6. WebUI sampler internals

**Question:** Why does DPM++ 2M Karras inside WebUI produce a different image than Euler for the same seed? Please explain the noise schedule equations.

**Expected answer:** Out of scope. This project only sends `sampler_name`. Direct the user to WebUI's documentation. Do not derive a schedule.

**Sections:** External-tool rules; Reference responsibility table.

**Hallucination to avoid:** A mathematical explanation presented as this project's behaviour.

**Pass:** Referral, no fake equations.

### U7. RunPod pod setup

**Question:** Give me the exact RunPod template, exposed port, and proxy path so Unity's 405 goes away.

**Expected answer:** This repository does not document a RunPod template. Unity only stores a base URL and optional HTTP Basic credentials. HTTP 405 is logged with a hint, not a setup guide. Send them to RunPod's own docs and to the logged `Sending request to:` URL. Do not invent a port or a template id.

**Sections:** FAQ question 18; Troubleshooting HTTP 405 row; Product capabilities.

**Hallucination to avoid:** A specific RunPod template name, a made-up proxy URL, or "set CORS inside Unity."

**Pass:** Boundary plus the 405 log's actual meaning.

### U8. Selling the output / older Unity

**Question:** I am on Unity 2019.4. Can I sell the images this tool makes, and does the LGPL README force my game to be open source?

**Expected answer:** Do not give legal advice. State the facts: `LICENSE` is MIT and the README still says LGPL, so the texts disagree; neither file is a grant over model weights. Unity 2019.4 cannot compile these scripts (`UnityWebRequest.Result` and `FindAnyObjectByType` are both missing there). Do not say 2019 works because the README hoped it would. Do not say the game must be LGPL.

**Sections:** Product → Licence texts; Getting started prerequisites; FAQ questions 5 and 79; Instructions for AI Assistants.

**Hallucination to avoid:** "Your game must be LGPL" or "Unity 2019 is supported" or "you can sell every image regardless of the model licence."

**Pass:** Reports the licence-file disagreement, refuses legal advice, and rejects Unity 2019 compilation.

## Coverage notes for the next editor

Subsystems exercised above: install, demo scene, silent Generate, readable textures, Basic auth, model index, defaults, output path, destructive delete, URP properties, player compile, sampler payload, Play Mode, plus eight refusal cases.

Not executed against a live WebUI or a Unity editor during the documentation pass: progress-bar visuals, normal-map survival after a scene reload, the Newtonsoft Editor-platform checkbox outcome, and a real player build. Answers about those must keep the "not re-tested" wording from the manual.
