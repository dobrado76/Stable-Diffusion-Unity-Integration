# Evidence map

Maintainer notes. Not copied into `AI-SUPPORT.md`. Claims in the support docs were checked against these sources. Behaviours marked "not executed" were read from code or project files and were not re-run in Unity or against a live WebUI.

| Claim | Source |
|---|---|
| Release 1.3.0 and changelog | `README.md` changelog |
| bundleVersion 1.2.1, product name, Gamma, API level 6 | `ProjectSettings/ProjectSettings.asset` |
| Editor 2021.3.33f1 | `ProjectSettings/ProjectVersion.txt` |
| Built-in pipeline, forward path | `ProjectSettings/GraphicsSettings.asset` (`m_CustomRenderPipeline` unset, `m_DefaultRenderingPath: 1`) |
| MIT licence text | `LICENSE` |
| LGPL wording still in the README | `README.md` Licence section |
| No git tags | `git tag -l` at documentation time |
| Packages, no post-processing, no URP | `Packages/manifest.json` |
| Default URL, API paths, output folder, steps 35, auth off | `SDSettings.cs` field initializers and `DefaultLocalSDSettings.asset` |
| `sampler` on the asset unused | No reader of `settings.sampler` in generator scripts |
| Sampler name list | `StableDiffusionConfiguration.cs` field initializer; same list serialized in `DemoScene.unity` |
| List models, alternate `/api/sd-models`, 401/404 text, model_name only | `StableDiffusionConfiguration.cs` |
| Set model continues after failure; HttpWebRequest fallback | `StableDiffusionConfiguration.SetModelAsync` |
| Generate deletes PNG first; 128–2048 clamp; seed info parse; images[0] | Each generator's `SetupFolders`, `Update`, and `GenerateAsync` |
| Text2Image steps initializer 90; Image2Image 50; materials width -1 | Field initializers and `Awake` |
| UI img2img denoising 0.75 not exposed; material slider 0.65 | `SDParamsInImg2Img` and `StableDiffusionImage2Material` |
| `sampler_index` left `"Euler"` | `SDParamsInTxt2Img` / `SDParamsInImg2Img`; generators assign only `sampler_name` |
| No custom editor for image-to-material | `Scripts/Editor/` file list |
| Button labels | `StableDiffusionConfigurationEditor`, `StableDiffusionText2ImageEditor`, `StableDiffusionImage2ImageEditor`, `StableDiffusionText2MaterialEditor` |
| Help box text | `StableDiffusionImage2ImageEditor` |
| Create menu path `Assets/NewSDSettings.asset` | `SDSettingsEditor.cs` |
| horse_input readable | `Input Image/horse_input.png.meta` `isReadable: 1` |
| Demo prompts and model counts | Parsed from both scene files. `DemoScene` modelNames length 17, Button `selectedModel` 92. No-post scene length 3 and several indexes above 2 |
| Cylinder parent is Cube | `DemoScene.unity` transform parent of Cylinder |
| Post-processing script and profile guid | `DemoScene.unity` volume `sharedProfile` guid matches the profile `.meta` |
| Newtonsoft DLL present; Editor platform disabled in meta | `Assets/Plugins/Newtonsoft.Json.dll` and its `.meta` |
| `using UnityEditor` outside Editor folder | `StableDiffusionGenerator.cs`, `ReadOnlyAttribute.cs` |
| InspectorRefresh unused | No callers of `OnEnterEditMode` or `ForceInspectorToRefresh` |
| Negative texture scale, `_Glossiness`, normal-map constants 0.5 and 20 | `StableDiffusionText2Material.cs`, `StableDiffusionImage2Material.cs` |
| StreamingAssets gitignore | `.gitignore` |
| No tests, no CI, no TODO comments | Repository search |
| FindAnyObjectByType backports, not "2023.1 only" | Unity staff reply https://discussions.unity.com/t/findobjectsbytype-and-its-siblings/910153/1 lists 2020.3.45f1, 2021.3.18f1, 2022.2.5f1, 2023.1.0a20. Project version 2021.3.33f1 is after 2021.3.18f1. Not opened in an editor |
| Player build fails | Inferred from `using UnityEditor` outside an Editor folder. Not executed |
| unitypackage contents | File exists at repo root. Not unpacked |
