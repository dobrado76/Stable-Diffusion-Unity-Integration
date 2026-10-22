# Stable Diffusion Unity Integration — AI support knowledge base

This file is the self-contained support manual for **Stable Diffusion Unity Integration**.
It is generated from the chapter files under `docs/`. Do not edit it by hand.
Rebuild it with `python tools/build_ai_support.py`.

| Fact | Value |
|---|---|
| Repository | https://github.com/dobrado76/Stable-Diffusion-Unity-Integration |
| Issues | https://github.com/dobrado76/Stable-Diffusion-Unity-Integration/issues |
| Release name in the README | 1.3.0 (2025-03-30) |
| Unity Player Settings bundle version | 1.2.1 |
| `ProjectVersion.txt` | 2021.3.33f1 |
| Licence file in the tree | MIT (`LICENSE`). The README still describes LGPL. |

An AI assistant with only this file should be able to answer installation, Inspector, configuration, file, error, and recovery questions about this Unity project. It should not invent WebUI, RunPod, or Unity features that the chapters do not describe.

## How links work in this file

Chapters that live as separate files in the repository are copied below in full.
A repository link such as `configuration.md#output-folder` has been turned into an anchor in this file.
Screenshots named from the root README are not embedded here. WebUI's own documentation stays on https://github.com/AUTOMATIC1111/stable-diffusion-webui .

When the repository is available, the same text is under `docs/`. `docs/support-evaluation.md` and `docs/evidence-map.md` are maintainer notes and are not part of this file.

## Contents

- [Question and symptom index](#support-index-question-and-symptom-index)
  - [Start here](#support-index-start-here)
  - [Symptoms](#support-index-symptoms)
  - [Common "where is it?" questions](#support-index-common-where-is-it-questions)
- [Instructions for AI Assistants](#ai-instructions-instructions-for-ai-assistants)
  - [Answering rules](#ai-instructions-answering-rules)
  - [Troubleshooting rules](#ai-instructions-troubleshooting-rules)
  - [External-tool rules](#ai-instructions-external-tool-rules)
  - [Project facts that are easy to get wrong](#ai-instructions-project-facts-that-are-easy-to-get-wrong)
- [Product](#product-product)
  - [What this project is](#product-what-this-project-is)
  - [Who it is for](#product-who-it-is-for)
  - [Primary workflows](#product-primary-workflows)
  - [Capabilities that are implemented](#product-capabilities-that-are-implemented)
  - [Design boundaries](#product-design-boundaries)
  - [What it does not do](#product-what-it-does-not-do)
  - [Licence texts in the repository](#product-licence-texts-in-the-repository)
- [Getting started](#getting-started-getting-started)
  - [Prerequisites](#getting-started-prerequisites)
  - [1. Start the WebUI API](#getting-started-1-start-the-webui-api)
  - [2. Get the Unity project](#getting-started-2-get-the-unity-project)
  - [3. Open a demo scene](#getting-started-3-open-a-demo-scene)
  - [4. Point settings at your server](#getting-started-4-point-settings-at-your-server)
  - [5. List models](#getting-started-5-list-models)
  - [6. Generate one image](#getting-started-6-generate-one-image)
  - [7. Confirm success](#getting-started-7-confirm-success)
  - [Expected first-run failures](#getting-started-expected-first-run-failures)
- [User guide](#user-guide-user-guide)
  - [Where the pieces live](#user-guide-where-the-pieces-live)
  - [Scene setup that every generator needs](#user-guide-scene-setup-that-every-generator-needs)
  - [Text-to-image for UI](#user-guide-text-to-image-for-ui)
  - [Image-to-image for UI](#user-guide-image-to-image-for-ui)
  - [Text-to-material for meshes](#user-guide-text-to-material-for-meshes)
  - [Image-to-material for meshes](#user-guide-image-to-material-for-meshes)
  - [Models](#user-guide-models)
  - [Samplers](#user-guide-samplers)
  - [Seeds](#user-guide-seeds)
  - [Progress bar](#user-guide-progress-bar)
  - [Play Mode](#user-guide-play-mode)
  - [Saving and what survives a restart](#user-guide-saving-and-what-survives-a-restart)
  - [Operations that destroy data](#user-guide-operations-that-destroy-data)
  - [Demo scene contents](#user-guide-demo-scene-contents)
- [Configuration](#configuration-configuration)
  - [SDSettings asset](#configuration-sdsettings-asset)
    - [Server and API paths](#configuration-server-and-api-paths)
    - [API Settings](#configuration-api-settings)
    - [URP Settings](#configuration-urp-settings)
  - [Output folder](#configuration-output-folder)
  - [StableDiffusionConfiguration object](#configuration-stablediffusionconfiguration-object)
  - [Per-generator fields](#configuration-per-generator-fields)
    - [Shared by the four generators](#configuration-shared-by-the-four-generators)
    - [Text-to-material and image-to-material only](#configuration-text-to-material-and-image-to-material-only)
  - [What is not configurable](#configuration-what-is-not-configurable)
  - [Credentials and sharing](#configuration-credentials-and-sharing)
- [Reference](#reference-reference)
  - [Versions](#reference-versions)
  - [Endpoints this project calls](#reference-endpoints-this-project-calls)
  - [Text-to-image JSON fields the code sets](#reference-text-to-image-json-fields-the-code-sets)
  - [Image-to-image JSON fields the code sets](#reference-image-to-image-json-fields-the-code-sets)
  - [Response handling](#reference-response-handling)
  - [Local validation](#reference-local-validation)
  - [Files and Unity objects](#reference-files-and-unity-objects)
  - [Material property names written](#reference-material-property-names-written)
  - [Progress JSON read](#reference-progress-json-read)
  - [Editor-only versus compiled-everywhere](#reference-editor-only-versus-compiled-everywhere)
  - [External responsibility](#reference-external-responsibility)
- [Troubleshooting](#troubleshooting-troubleshooting)
  - [Generate does nothing](#troubleshooting-generate-does-nothing)
  - [Model popup throws when you select the object](#troubleshooting-model-popup-throws-when-you-select-the-object)
  - [List Models fails immediately](#troubleshooting-list-models-fails-immediately)
  - [Wrong checkpoint or an out-of-range model](#troubleshooting-wrong-checkpoint-or-an-out-of-range-model)
  - [HTTP errors during Generate](#troubleshooting-http-errors-during-generate)
  - [No image in the response](#troubleshooting-no-image-in-the-response)
  - [Progress bar stuck or "Progress unknown..."](#troubleshooting-progress-bar-stuck-or-progress-unknown)
  - [Input image errors](#troubleshooting-input-image-errors)
  - [Material looks wrong](#troubleshooting-material-looks-wrong)
  - [The PNG is missing](#troubleshooting-the-png-is-missing)
  - [Authentication works in the browser and fails in Unity](#troubleshooting-authentication-works-in-the-browser-and-fails-in-unity)
  - [Editor freeze while setting the model](#troubleshooting-editor-freeze-while-setting-the-model)
  - [Compile errors](#troubleshooting-compile-errors)
  - [Demo scene missing script](#troubleshooting-demo-scene-missing-script)
  - [Play Mode looked successful and then reverted](#troubleshooting-play-mode-looked-successful-and-then-reverted)
  - [What to collect if you open an issue](#troubleshooting-what-to-collect-if-you-open-an-issue)
- [Architecture](#architecture-architecture)
  - [Components](#architecture-components)
  - [Request lifecycle](#architecture-request-lifecycle)
  - [How objects are found](#architecture-how-objects-are-found)
  - [Persistence model](#architecture-persistence-model)
  - [What runs in Edit Mode](#architecture-what-runs-in-edit-mode)
  - [External boundary](#architecture-external-boundary)
  - [Editor UI map](#architecture-editor-ui-map)
  - [Assemblies](#architecture-assemblies)
- [Development](#development-development)
  - [Layout](#development-layout)
  - [Open and compile](#development-open-and-compile)
  - [Tests](#development-tests)
  - [Player builds](#development-player-builds)
  - [Extending](#development-extending)
  - [Package](#development-package)
  - [Documentation commands](#development-documentation-commands)
  - [Contributing](#development-contributing)
  - [Source history that explains user-visible names](#development-source-history-that-explains-user-visible-names)
- [FAQ](#FAQ-faq)
  - [Basics](#FAQ-basics)
    - [1. What is Stable Diffusion Unity Integration?](#FAQ-1-what-is-stable-diffusion-unity-integration)
    - [2. Which version do these answers describe?](#FAQ-2-which-version-do-these-answers-describe)
    - [3. Does the download include Stable Diffusion or any checkpoints?](#FAQ-3-does-the-download-include-stable-diffusion-or-any-checkpoints)
    - [4. Can I use ComfyUI, Forge, or the Stability API?](#FAQ-4-can-i-use-comfyui-forge-or-the-stability-api)
    - [5. Which Unity version should I open?](#FAQ-5-which-unity-version-should-i-open)
    - [6. How do I install it?](#FAQ-6-how-do-i-install-it)
    - [7. Where is the demo scene? The README path does not exist.](#FAQ-7-where-is-the-demo-scene-the-readme-path-does-not-exist)
    - [8. Why does DemoScene say a script is missing?](#FAQ-8-why-does-demoscene-say-a-script-is-missing)
    - [9. Is this a runtime feature in my game?](#FAQ-9-is-this-a-runtime-feature-in-my-game)
    - [10. Where is the video and the project page?](#FAQ-10-where-is-the-video-and-the-project-page)
  - [Server connection](#FAQ-server-connection)
    - [11. What URL should I put in?](#FAQ-11-what-url-should-i-put-in)
    - [12. How do I turn on the API?](#FAQ-12-how-do-i-turn-on-the-api)
    - [13. Where is the List Model button?](#FAQ-13-where-is-the-list-model-button)
    - [14. What does a successful list look like?](#FAQ-14-what-does-a-successful-list-look-like)
    - [15. Why does the error mention port 7860 when I use another port?](#FAQ-15-why-does-the-error-mention-port-7860-when-i-use-another-port)
    - [16. What is the `/api/sd-models` request?](#FAQ-16-what-is-the-apisd-models-request)
    - [17. Can I change the API paths?](#FAQ-17-can-i-change-the-api-paths)
    - [18. Does it support HTTPS and a remote host?](#FAQ-18-does-it-support-https-and-a-remote-host)
    - [19. Why did I get 405 Method Not Allowed?](#FAQ-19-why-did-i-get-405-method-not-allowed)
  - [Authentication](#FAQ-authentication)
    - [20. How do I send the API password?](#FAQ-20-how-do-i-send-the-api-password)
    - [21. I enabled Use Auth and it still returns 401.](#FAQ-21-i-enabled-use-auth-and-it-still-returns-401)
    - [22. Where is the password saved?](#FAQ-22-where-is-the-password-saved)
    - [23. Does List Models use the password?](#FAQ-23-does-list-models-use-the-password)
  - [SDSettings and defaults](#FAQ-sdsettings-and-defaults)
    - [24. How do I create another settings asset?](#FAQ-24-how-do-i-create-another-settings-asset)
    - [25. Which asset is the demo using?](#FAQ-25-which-asset-is-the-demo-using)
    - [26. I changed Sampler on the settings asset and the image still uses Euler a.](#FAQ-26-i-changed-sampler-on-the-settings-asset-and-the-image-still-uses-euler-a)
    - [27. I changed width and steps on the settings asset and the UI component ignored them.](#FAQ-27-i-changed-width-and-steps-on-the-settings-asset-and-the-ui-component-ignored-them)
    - [28. Why are brand-new text-to-image steps 90?](#FAQ-28-why-are-brand-new-text-to-image-steps-90)
    - [29. Do I need to press Apply or Save before Generate?](#FAQ-29-do-i-need-to-press-apply-or-save-before-generate)
  - [Models and samplers](#FAQ-models-and-samplers)
    - [30. The Model dropdown crashes as soon as I click the object.](#FAQ-30-the-model-dropdown-crashes-as-soon-as-i-click-the-object)
    - [31. I listed models, generated, and got IndexOutOfRangeException.](#FAQ-31-i-listed-models-generated-and-got-indexoutofrangeexception)
    - [32. The model I chose is not the one WebUI used.](#FAQ-32-the-model-i-chose-is-not-the-one-webui-used)
    - [33. Does listing models switch the checkpoint?](#FAQ-33-does-listing-models-switch-the-checkpoint)
    - [34. Can the tool download the sampler list?](#FAQ-34-can-the-tool-download-the-sampler-list)
    - [35. I picked DPM++ 2M and the result looks like Euler.](#FAQ-35-i-picked-dpm-2m-and-the-result-looks-like-euler)
    - [36. What if I have two StableDiffusionConfiguration objects?](#FAQ-36-what-if-i-have-two-stablediffusionconfiguration-objects)
  - [Generate buttons and prompts](#FAQ-generate-buttons-and-prompts)
    - [37. Where is the Generate button?](#FAQ-37-where-is-the-generate-button)
    - [38. Why is there no Generate button on image-to-material?](#FAQ-38-why-is-there-no-generate-button-on-image-to-material)
    - [39. I click Generate and absolutely nothing happens.](#FAQ-39-i-click-generate-and-absolutely-nothing-happens)
    - [40. What are the old names StableDiffusionImage and StableDiffusionMaterial?](#FAQ-40-what-are-the-old-names-stablediffusionimage-and-stablediffusionmaterial)
    - [41. Unity offers a component called Stable Diffusion Generator. Should I add it?](#FAQ-41-unity-offers-a-component-called-stable-diffusion-generator-should-i-add-it)
  - [Image-to-image](#FAQ-image-to-image)
    - [42. Which texture is uploaded for UI image-to-image?](#FAQ-42-which-texture-is-uploaded-for-ui-image-to-image)
    - [43. Why must the texture be readable?](#FAQ-43-why-must-the-texture-be-readable)
    - [44. What denoising strength is used?](#FAQ-44-what-denoising-strength-is-used)
    - [45. Can I inpaint or pass a mask?](#FAQ-45-can-i-inpaint-or-pass-a-mask)
    - [46. The horse image is the init image. Will Generate modify that PNG?](#FAQ-46-the-horse-image-is-the-init-image-will-generate-modify-that-png)
  - [Materials, tiling, and normal maps](#FAQ-materials-tiling-and-normal-maps)
    - [47. Which objects in the demo are materials?](#FAQ-47-which-objects-in-the-demo-are-materials)
    - [48. What is the difference between Tiling and Tiling X / Tiling Y?](#FAQ-48-what-is-the-difference-between-tiling-and-tiling-x-tiling-y)
    - [49. Why did generating the cube change the cylinder?](#FAQ-49-why-did-generating-the-cube-change-the-cylinder)
    - [50. Does Apply Recursively search inactive children?](#FAQ-50-does-apply-recursively-search-inactive-children)
    - [51. Where is the normal map file?](#FAQ-51-where-is-the-normal-map-file)
    - [52. How do metallic and smoothness get applied?](#FAQ-52-how-do-metallic-and-smoothness-get-applied)
    - [53. How do I enable URP?](#FAQ-53-how-do-i-enable-urp)
    - [54. Will this replace a material asset I already assigned?](#FAQ-54-will-this-replace-a-material-asset-i-already-assigned)
  - [Size, seed, and batches](#FAQ-size-seed-and-batches)
    - [55. Why can't I set width to 64 or 4096?](#FAQ-55-why-cant-i-set-width-to-64-or-4096)
    - [56. Does the size have to be a multiple of 8?](#FAQ-56-does-the-size-have-to-be-a-multiple-of-8)
    - [57. What does seed -1 do?](#FAQ-57-what-does-seed-1-do)
    - [58. Can I generate a grid or four images at once?](#FAQ-58-can-i-generate-a-grid-or-four-images-at-once)
    - [59. Where are high-res fix, face restore, and styles?](#FAQ-59-where-are-high-res-fix-face-restore-and-styles)
  - [Files, save, and undo](#FAQ-files-save-and-undo)
    - [60. Where is the PNG?](#FAQ-60-where-is-the-png)
    - [61. What is the difference between streamingAssets and StreamingAssets?](#FAQ-61-what-is-the-difference-between-streamingassets-and-streamingassets)
    - [62. Why doesn't git show my new images?](#FAQ-62-why-doesnt-git-show-my-new-images)
    - [63. Can I undo Generate?](#FAQ-63-can-i-undo-generate)
    - [64. I generated during Play Mode and the object reverted.](#FAQ-64-i-generated-during-play-mode-and-the-object-reverted)
    - [65. The scene shows the picture, but I cannot find a material asset.](#FAQ-65-the-scene-shows-the-picture-but-i-cannot-find-a-material-asset)
    - [66. Generate failed and the old PNG is gone.](#FAQ-66-generate-failed-and-the-old-png-is-gone)
  - [Progress, hangs, and errors](#FAQ-progress-hangs-and-errors)
    - [67. What does the progress bar mean?](#FAQ-67-what-does-the-progress-bar-mean)
    - [68. The Editor froze right after I clicked Generate.](#FAQ-68-the-editor-froze-right-after-i-clicked-generate)
    - [69. The bar is stuck after an exception.](#FAQ-69-the-bar-is-stuck-after-an-exception)
    - [70. What does "No image was return by the server" mean?](#FAQ-70-what-does-no-image-was-return-by-the-server-mean)
    - [71. The Console says the server must be on port 7860, but my browser can open the API.](#FAQ-71-the-console-says-the-server-must-be-on-port-7860-but-my-browser-can-open-the-api)
  - [Demo scene](#FAQ-demo-scene)
    - [72. I opened the demo and Generate on the Button throws.](#FAQ-72-i-opened-the-demo-and-generate-on-the-button-throws)
    - [73. Are the model names saved in the demo something I should download?](#FAQ-73-are-the-model-names-saved-in-the-demo-something-i-should-download)
    - [74. What is Image-2-RawImage?](#FAQ-74-what-is-image-2-rawimage)
  - [Platforms, builds, and the JSON plugin](#FAQ-platforms-builds-and-the-json-plugin)
    - [75. The project does not compile: FindAnyObjectByType is missing.](#FAQ-75-the-project-does-not-compile-findanyobjectbytype-is-missing)
    - [76. The project does not compile: Newtonsoft could not be found.](#FAQ-76-the-project-does-not-compile-newtonsoft-could-not-be-found)
    - [77. Why did my player build fail on UnityEditor?](#FAQ-77-why-did-my-player-build-fail-on-unityeditor)
    - [78. Does the OS change the output path?](#FAQ-78-does-the-os-change-the-output-path)
  - [Licence, tests, and support](#FAQ-licence-tests-and-support)
    - [79. Is the licence LGPL or MIT?](#FAQ-79-is-the-licence-lgpl-or-mit)
    - [80. Are there automated tests?](#FAQ-80-are-there-automated-tests)
    - [81. Who do I contact, and what should I send?](#FAQ-81-who-do-i-contact-and-what-should-i-send)
    - [82. Is a feature that I see in WebUI's browser "planned" here?](#FAQ-82-is-a-feature-that-i-see-in-webuis-browser-planned-here)
    - [83. Can these components live on a prefab?](#FAQ-83-can-these-components-live-on-a-prefab)
    - [84. Why does Generate fail when the object is inactive?](#FAQ-84-why-does-generate-fail-when-the-object-is-inactive)
    - [85. Why is the UI element still the old size?](#FAQ-85-why-is-the-ui-element-still-the-old-size)
    - [86. Why did the Editor hitch, reload scripts, or move the Scene camera?](#FAQ-86-why-did-the-editor-hitch-reload-scripts-or-move-the-scene-camera)
    - [87. The picture appeared and the Console still says Error processing response.](#FAQ-87-the-picture-appeared-and-the-console-still-says-error-processing-response)
- [Roadmap](#roadmap-roadmap)
  - [Implemented](#roadmap-implemented)
  - [Known limitations of the current code](#roadmap-known-limitations-of-the-current-code)
  - [Not planned in this repository](#roadmap-not-planned-in-this-repository)

<a id="support-index"></a>

<!-- source: docs/support-index.md -->

<a id="support-index-question-and-symptom-index"></a>

# Question and symptom index

Use this index to jump to the chapter that answers a support question. In the repository, the links open the separate files under `docs/`. In `AI-SUPPORT.md` the same links are anchors inside the one file.

<a id="support-index-start-here"></a>

## Start here

| Question | Chapter |
|---|---|
| What this tool is and what it refuses to do | [Product](#product) |
| Install Unity content, start the API, generate once | [Getting started](#getting-started) |
| Day-to-day Inspector workflow | [User guide](#user-guide) |
| Settings asset, output paths, defaults | [Configuration](#configuration) |
| Exact JSON, error strings, limits | [Reference](#reference) |
| Something failed | [Troubleshooting](#troubleshooting) |
| How the scripts call the server | [Architecture](#architecture) |
| Compile, build, tests, docs commands | [Development](#development) |
| Short answers | [FAQ](#FAQ) |
| Implemented versus not scheduled | [Roadmap](#roadmap) |

<a id="support-index-symptoms"></a>

## Symptoms

| What you see | Go to |
|---|---|
| **Generate** does nothing | [Generate does nothing](#troubleshooting-generate-does-nothing) |
| Popup exception on selecting a generator | [Model popup throws](#troubleshooting-model-popup-throws-when-you-select-the-object) |
| **List Models** cannot reach the server | [List Models fails](#troubleshooting-list-models-fails-immediately) |
| Wrong model, or `IndexOutOfRangeException` | [Out-of-range model](#troubleshooting-wrong-checkpoint-or-an-out-of-range-model) |
| HTTP 401, 404, 405, 422, or 500 | [HTTP errors during Generate](#troubleshooting-http-errors-during-generate) |
| `No image was returned by the server` | [No image in the response](#troubleshooting-no-image-in-the-response) |
| Progress bar stuck or `Progress unknown...` | [Progress bar](#troubleshooting-progress-bar-stuck-or-progress-unknown) |
| Texture is not readable | [Input image errors](#troubleshooting-input-image-errors) |
| Pink material, flipped texture, children changed | [Material looks wrong](#troubleshooting-material-looks-wrong) |
| PNG missing or deleted | [The PNG is missing](#troubleshooting-the-png-is-missing) |
| Browser auth works, Unity gets 401 | [Authentication mismatch](#troubleshooting-authentication-works-in-the-browser-and-fails-in-unity) |
| Editor freeze after Generate | [Editor freeze](#troubleshooting-editor-freeze-while-setting-the-model) |
| Script compile error | [Compile errors](#troubleshooting-compile-errors) |
| Missing script on the demo volume | [Demo scene missing script](#troubleshooting-demo-scene-missing-script) |
| Play Mode result disappeared | [Play Mode reverted](#troubleshooting-play-mode-looked-successful-and-then-reverted) |
| Empty prompt, seeds, batches, URP, files | [FAQ](#FAQ) |

<a id="support-index-common-where-is-it-questions"></a>

## Common "where is it?" questions

| Looking for | Answer |
|---|---|
| **List Models** | `StableDiffusionConfiguration` inspector. The label is plural. |
| **Generate** | Text-to-image, UI image-to-image, and text-to-material inspectors only. |
| **Use Auth**, **User**, **Pass** | `SDSettings` asset, header **API Settings**. |
| **Stable Diffusion Server URL** | Same asset, header **AUTOMATIC1111 Settings**. |
| Demo scene | `Assets/Stable-Diffusion-Unity-Integration/Scenes/DemoScene.unity` |
| Default URL asset | `Assets/Stable-Diffusion-Unity-Integration/Settings/DefaultLocalSDSettings.asset` |
| Output PNG | `Assets/StreamingAssets/SDImages` or `SDMaterials`, file name equals the component **Guid**. |
| Issue tracker | https://github.com/dobrado76/Stable-Diffusion-Unity-Integration/issues |

<a id="ai-instructions"></a>

<!-- source: docs/ai-instructions.md -->

<a id="ai-instructions-instructions-for-ai-assistants"></a>

# Instructions for AI Assistants

These rules apply when this file is the project context. They exist because this integration is a thin HTTP client and is often described as if it were Stable Diffusion itself.

<a id="ai-instructions-answering-rules"></a>

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

<a id="ai-instructions-troubleshooting-rules"></a>

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

<a id="ai-instructions-external-tool-rules"></a>

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

<a id="ai-instructions-project-facts-that-are-easy-to-get-wrong"></a>

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

<a id="product"></a>

<!-- source: docs/product.md -->

<a id="product-product"></a>

# Product

<a id="product-what-this-project-is"></a>

## What this project is

Stable Diffusion Unity Integration is a Unity Editor tool that sends image-generation requests to an [AUTOMATIC1111 Stable Diffusion WebUI](https://github.com/AUTOMATIC1111/stable-diffusion-webui) HTTP API and applies the returned image inside the open Unity scene.

It is a design-time helper for level and UI mock-up work. It does not embed a Stable Diffusion runtime, and it does not ship any diffusion models.

The user-facing release described by the repository README changelog is **1.3.0** (2025-03-30). `ProjectSettings/ProjectSettings.asset` still records `bundleVersion: 1.2.1`. The repository has no git tags. Treat the README changelog as the feature history and the Unity bundle version as a stale Player Settings field.

Repository: https://github.com/dobrado76/Stable-Diffusion-Unity-Integration

<a id="product-who-it-is-for"></a>

## Who it is for

- A Unity developer who already runs, or can reach, an AUTOMATIC1111 WebUI with the API switched on.
- Someone texturing Built-in Render Pipeline meshes, or UI `Image` and `RawImage` objects, from the Inspector.
- Someone who wants the generated PNG kept in the Unity project.

It is not a general Stable Diffusion application, a model manager, or a runtime in-game generation framework.

<a id="product-primary-workflows"></a>

## Primary workflows

1. Start WebUI with `--api`. Optionally protect it with `--api-auth username:password`.
2. Open a Unity scene that contains an active `StableDiffusionConfiguration`.
3. Assign an `SDSettings` asset. The repository includes `Assets/Stable-Diffusion-Unity-Integration/Settings/DefaultLocalSDSettings.asset`, pointed at `http://127.0.0.1:7860`.
4. Click **List Models** and choose a model on the generator component.
5. Click **Generate** on one of the generator components that has a custom inspector.
6. The tool writes a PNG, then assigns the pixels to a UI graphic or a mesh material.

<a id="product-capabilities-that-are-implemented"></a>

## Capabilities that are implemented

- Text-to-image onto a UI `Image` or `RawImage` (`StableDiffusionText2Image`).
- Image-to-image onto a UI `Image` or `RawImage` (`StableDiffusionImage2Image`), using a readable `Texture2D` as the init image.
- Text-to-image onto a `MeshRenderer` material (`StableDiffusionText2Material`), with optional seamless tiling sent to WebUI, a normal map built in Unity, and metallic, smoothness, and texture-scale controls.
- Image-to-image onto a `MeshRenderer` material (`StableDiffusionImage2Material`). This component has generation code and no custom inspector. See the user guide before expecting a **Generate** button.
- A shared server profile (`SDSettings`) for the base URL, API paths, HTTP Basic credentials, output folder, URP shader switch, and default width, height, steps, CFG scale, and seed.
- A manually edited sampler list on `StableDiffusionConfiguration`.
- A model list fetched from the server and stored on the configuration object.
- An Editor progress bar polled from the WebUI progress endpoint about twice a second.
- HTTP Basic authentication when `Use Auth` is enabled and both user and password are non-empty.
- A remote base URL, which is how a hosted WebUI such as a RunPod instance is used. There is no RunPod-specific client.

<a id="product-design-boundaries"></a>

## Design boundaries

- WebUI performs sampling, model loading, and image decoding on the server. This project builds the JSON request, waits for the response, and files the first returned image.
- One active configuration object is assumed. Generators call `FindAnyObjectByType<StableDiffusionConfiguration>()` and use whichever instance that returns.
- Only the first returned image is kept. Batch size and batch count are hardcoded to 1.
- The tool is operated from the Inspector. There is no game view, no runtime HUD, and no cancel button.
- Custom editors live under `Assets/Stable-Diffusion-Unity-Integration/Scripts/Editor/`. The generator scripts themselves live outside an `Editor` folder, so they are part of the player script compile. See [Player builds](#development-player-builds).

<a id="product-what-it-does-not-do"></a>

## What it does not do

- It does not install, launch, or configure WebUI for you.
- It does not download checkpoints, LoRAs, VAEs, embeddings, or ControlNet models.
- It does not expose high-resolution fix, face restore, inpainting masks, styles, scripts, upscalers, or batch grids in the Inspector.
- It does not query the server for the sampler list. The sampler names are a serialized string array you edit by hand.
- It does not save a `.mat` asset or a normal-map PNG. It writes one colour PNG per component.
- It does not implement HDRP. URP is a shader-name switch on text-to-material only.
- It has no Unity Test Framework tests and no CI workflow.

<a id="product-licence-texts-in-the-repository"></a>

## Licence texts in the repository

The file `LICENSE` at the repository root is the MIT License, copyright 2025 Dominic Branchaud. The README section titled "Licence" still describes LGPL v2.1 and links to an older revision of that file. Those two texts do not match. This documentation is not legal advice. Read `LICENSE` for the grant that is actually in the tree. Neither text grants rights to third-party models or to WebUI.

<a id="getting-started"></a>

<!-- source: docs/getting-started.md -->

<a id="getting-started-getting-started"></a>

# Getting started

This is the installation and first-generation path for the current scripts. WebUI's own install steps belong to that project: https://github.com/AUTOMATIC1111/stable-diffusion-webui

<a id="getting-started-prerequisites"></a>

## Prerequisites

- A Unity project on the Built-in Render Pipeline for the included demo scenes. `ProjectSettings/GraphicsSettings.asset` has no custom render pipeline asset, and the demo materials use the Standard shader. URP is optional and only partially supported.
- An editor that contains the APIs these scripts call. `UnityWebRequest.Result` replaced `isNetworkError` / `isHttpError` in Unity 2020.2, so Unity 2019 does not compile this code. `FindAnyObjectByType` is in the 2023.1 API and was backported. A Unity staff reply lists it from **2020.3.45f1**, **2021.3.18f1**, **2022.2.5f1**, and **2023.1.0a20** onward: https://discussions.unity.com/t/findobjectsbytype-and-its-siblings/910153/1 . `ProjectSettings/ProjectVersion.txt` records **2021.3.33f1**, which is newer than 2021.3.18f1, so that call is expected to compile in the editor named by the project file. This documentation pass did not launch Unity. The README's claim that 2019 works does not match the source. Patches older than the backports above, and Unity 2022.1 (not in that staff list), are expected to fail on `FindAnyObjectByType`. There is no compatibility shim.
- [AUTOMATIC1111 WebUI](https://github.com/AUTOMATIC1111/stable-diffusion-webui) reachable over HTTP, started with the API enabled. Official API overview: https://github.com/AUTOMATIC1111/stable-diffusion-webui/wiki/API . Command-line flags such as `--api` and `--api-auth` are documented by WebUI: https://github.com/AUTOMATIC1111/stable-diffusion-webui/wiki/Command-Line-Arguments-and-Settings
- At least one checkpoint installed in that WebUI. This repository does not provide one.
- `Assets/Plugins/Newtonsoft.Json.dll` (the file is in the repository, about 662 KB). The scripts deserialize JSON with `Newtonsoft.Json`.

<a id="getting-started-1-start-the-webui-api"></a>

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

<a id="getting-started-2-get-the-unity-project"></a>

## 2. Get the Unity project

Two ways are described in the README. Both are present in the repository.

**Open this repository as the Unity project.** Clone https://github.com/dobrado76/Stable-Diffusion-Unity-Integration and open the repository root in Unity. The demo scenes and the default settings asset are included.

**Import the package into another project.** The file `Stable-Diffusion-Unity-Integration.unitypackage` is at the repository root. Use **Assets > Import Package > Custom Package**. Importing the package into a fresh project does not bring along every texture that was generated later inside this demo; the package itself was not unpacked while writing this documentation. If the package and the `Scripts` folder ever disagree, the `Scripts` folder is the source these docs describe. You still need `Newtonsoft.Json.dll` if the package does not include it. The plugin importer meta sets the Editor platform entry to `enabled: 0` while Any Platform is enabled. If the Console reports that `Newtonsoft` cannot be found, select `Assets/Plugins/Newtonsoft.Json.dll` and enable it for **Editor**.

`Packages/manifest.json` does not include Post Processing or URP. TextMeshPro, Timeline, and the Unity Test Framework are listed; the Test Framework is unused. There is no `Tests` folder.

<a id="getting-started-3-open-a-demo-scene"></a>

## 3. Open a demo scene

The README path `StableDiffusionIntegration/Scenes/DemoScene.unity` is outdated. The scenes are:

- `Assets/Stable-Diffusion-Unity-Integration/Scenes/DemoScene.unity`
- `Assets/Stable-Diffusion-Unity-Integration/Scenes/DemoScene-NoPostProcessing.unity`

`DemoScene` contains a global Post Processing volume (`isGlobal: 1`, weight 1) whose script guid is the Post Processing Stack v2 volume. That package is not in `manifest.json`. Until you install **Post Processing** from Package Manager, that object shows a missing script. The profile asset is `Assets/Stable-Diffusion-Unity-Integration/Scenes/PostProcessingVolume Profile.asset` and the volume's `sharedProfile` points at it. Install steps for the package are Unity's: https://docs.unity3d.com/Packages/com.unity.postprocessing@3.4/manual/Installation.html . Reopen the scene after the package imports. The post-processing volume is not required to generate images. Use `DemoScene-NoPostProcessing` to avoid it.

The demo is a small room (floor, walls, ceiling, table, paintings, a canvas with UI examples) plus a GameObject named `StableDiffusionConfiguration`. Its Settings field references `DefaultLocalSDSettings`.

<a id="getting-started-4-point-settings-at-your-server"></a>

## 4. Point settings at your server

Select `StableDiffusionConfiguration`. In the Inspector, under the settings asset:

- **Stable Diffusion Server URL** should be the server origin with no trailing slash, for example `http://127.0.0.1:7860`. API paths in the same asset already start with `/`. A trailing slash produces a double slash in the request URL.
- Leave the API path fields at their defaults unless your proxy remaps them. Defaults are `/sdapi/v1/sd-models`, `/sdapi/v1/txt2img`, `/sdapi/v1/img2img`, `/sdapi/v1/options`, and `/sdapi/v1/progress`.
- For `--api-auth`, enable **Use Auth** and set **User** and **Pass** to the same pair. Both must be non-empty or the tool sends no `Authorization` header. The password is stored in plain text in the asset.

<a id="getting-started-5-list-models"></a>

## 5. List models

Click **List Models** on the configuration Inspector. The button label is plural. The Console should log `Successfully loaded N models`. The **Model Names** array on the configuration object fills with each model's `model_name` from the JSON (not the filename and not the title).

The demo scenes already contain a saved model-name snapshot from a developer's server, and saved **Model** indexes on individual objects. Those names are not checkpoints shipped by this project. `DemoScene` stores 17 names, and the `Button` object's model index is `92`, which is outside 0–16. `DemoScene-NoPostProcessing` stores 3 names, and several objects store indexes such as 13, 26, and 234. After you click **List Models**, re-select **Model** on each generator before clicking **Generate**. `Generate()` does not clamp the index. An out-of-range index throws before a new image is written, and the previous PNG for that component has already been deleted.

<a id="getting-started-6-generate-one-image"></a>

## 6. Generate one image

Pick a generator that has a **Generate** button:

| Object in the demo | Component | Also requires |
|---|---|---|
| `Text-2-RawImage`, `Image`, `Button` | `StableDiffusionText2Image` | A non-empty **Prompt**. `Image` or `RawImage` on the same GameObject if you want the result shown |
| `Image-2-RawImage` | `StableDiffusionImage2Image` | A non-empty **Prompt** and a readable **Input Texture**. The demo assigns `Assets/Stable-Diffusion-Unity-Integration/Input Image/horse_input.png`, which is imported with Read/Write enabled |
| `Floor`, `Walls`, `Ceiling`, `Table`, `Cube`, `Cylinder`, `Painting1`, `Painting2` | `StableDiffusionText2Material` | A non-empty **Prompt**, and a `MeshRenderer` on this object or, when **Apply Recursively** is on, on a child |

`StableDiffusionImage2Material` is not used in the demo and has no **Generate** button.

Click **Generate**. A progress bar titled `Generation in progress` appears while the request runs. On success:

- A PNG named `{guid}.png` is written under `Assets/StreamingAssets/SDImages/` (UI generators) or `Assets/StreamingAssets/SDMaterials/` (material generators), unless you created `Assets/streamingAssets` or changed **Output Folder**. See [Output paths](#configuration-output-folder).
- The UI graphic or the mesh material updates in the scene.
- **Generated Seed** updates when the response `info` string contains a seed.
- The scene is marked dirty. Save the scene if you want Unity to keep the in-scene texture reference.

The `guid` field on the component is the file name. It is read-only in the Inspector.

<a id="getting-started-7-confirm-success"></a>

## 7. Confirm success

All of the following should be true:

- The Console has no `Request error`, `No image was returned by the server`, or exception stack from the generator.
- The progress bar closes.
- The PNG exists on disk next to the guid.
- The UI element or mesh shows the new image.

If the button appears to do nothing, the prompt is empty, a generation is already flagged as running, or (for image-to-image) **Input Texture** is empty. Those cases log nothing. See [Troubleshooting](#troubleshooting).

<a id="getting-started-expected-first-run-failures"></a>

## Expected first-run failures

- WebUI not running, wrong port, or missing `--api`: connection error or 404 in the Console when listing models.
- `--api-auth` without **Use Auth**: HTTP 401 or 403 and the message `Authentication error: Server requires an API key...`
- Demo **Model** index out of range: exception when generating, and the previous PNG for that guid is already gone.
- `DemoScene` missing-script warning on `PostProcessingVolume` until the Post Processing package is installed. Generation does not need that package.

<a id="user-guide"></a>

<!-- source: docs/user-guide.md -->

<a id="user-guide-user-guide"></a>

# User guide

Everything here is driven from the Unity Inspector while the Editor is open. Buttons exist only where a custom editor draws them.

<a id="user-guide-where-the-pieces-live"></a>

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

<a id="user-guide-scene-setup-that-every-generator-needs"></a>

## Scene setup that every generator needs

1. An active `StableDiffusionConfiguration` in a loaded scene.
2. Its **Settings** field pointing at an `SDSettings` asset.
3. **List Models** completed at least once so **Model Names** is a non-null array.
4. A **Model** index inside that array. Select it again after every refresh of the list.
5. A non-empty **Prompt**.
6. The generator's GameObject must be active and the component checkbox must be enabled. `Generate()` calls `StartCoroutine`. Unity does not start that coroutine on an inactive object or a disabled behaviour, and this project does not catch that failure. `generating` is set only after the coroutine begins, so a refused start leaves the flag false. The configuration object must also be active. The parameterless `FindAnyObjectByType` returns an active object; it will not discover a disabled one. A configuration that was found while active and later disabled can stay in the static cache and still be used.

Missing configuration: opening the generator inspector throws `NullReferenceException` because the sampler popup reads `sdc.samplers` with no null check.

`Model Names` still null: the **Model** popup throws because `EditorGUILayout.Popup` receives a null array. Click **List Models** first, or set the array size on the configuration so it is an empty array rather than null. An empty array avoids the popup exception and then fails at generate time when the index is read.

<a id="user-guide-text-to-image-for-ui"></a>

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

<a id="user-guide-image-to-image-for-ui"></a>

## Image-to-image for UI

**Use it** when an existing picture should be modified. The demo object `Image-2-RawImage` uses this component with `horse_input.png`.

**Where:** `StableDiffusionImage2Image` on a GameObject that has an `Image` or `RawImage` if you want the result displayed. The init image is the **Input Texture** field, not whatever the UI graphic currently shows.

**Prerequisite:** **Input Texture** assigned, and that texture's import settings have **Read/Write** enabled. The inspector shows `Input textures should be uncompressed and readable`. The code checks `isReadable` only. It does not read the compression setting. A readable texture proceeds to `EncodeToPNG` whether or not it is compressed. The horse sample is readable and its importer compression is none.

**Steps:** same as text-to-image, plus assign **Input Texture**, then **Generate**.

**If the texture is missing:** **Generate** returns without a Console message.

**If it is not readable:** the Console logs `Input Image {name} isn't readable. Go to texture import settings and tick the Read/Write box` and does not start the request.

**What is sent:** a base64 PNG of the input texture in `init_images`, plus prompt, negative prompt, steps, CFG, width, height, seed, `tiling` false, and sampler name. Denoising strength is the private default **0.75**. There is no slider. Resize mode stays 0. No mask is sent.

**Result:** same filing rules as text-to-image (`SDImages/{guid}.png`) and the same UI assignment. The init texture asset is not modified.

<a id="user-guide-text-to-material-for-meshes"></a>

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

<a id="user-guide-image-to-material-for-meshes"></a>

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

<a id="user-guide-models"></a>

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

<a id="user-guide-samplers"></a>

## Samplers

The **Sampler** popup lists `StableDiffusionConfiguration.samplers`. Index 0 is the initial selection. The chosen string is written to `sampler_name`. The payload also contains `sampler_index` with the class default `Euler`, and this project never updates that second field. Which field your WebUI honours is WebUI's behaviour; if the sampler you picked is ignored, that unused `sampler_index` value is the first project-side discrepancy to suspect. Compare with a direct API call using WebUI's API docs before reporting a sampler bug.

<a id="user-guide-seeds"></a>

## Seeds

- **Seed** `-1` means "server chooses". The server's choice is copied into **Generated Seed** when `info` parses.
- **Seed** any other value is sent unchanged. **Generated Seed** should come back as that value when the server honours it.
- **Generated Seed** is not automatically copied into **Seed**. A second click with **Seed** still at `-1` is another random result.
- To keep a result, set **Seed** to the number shown in **Generated Seed**, save the scene, and generate again with the same prompt and parameters. Identity of the result still depends on the checkpoint, sampler, and WebUI version, which this project does not pin.

<a id="user-guide-progress-bar"></a>

## Progress bar

The bar title is `Generation in progress`. The non-auth poll formats the fraction with one decimal place (`F1`). The auth poll prints `progress * 100` with default number formatting. If the non-auth poll fails, the bar shows `Progress unknown...` at half scale and the image request continues. The auth poll does not catch parse errors.

Each generator has its own poll flag. If a poll on that component is still running, later polls on it are skipped until the flag clears. The non-auth poll clears the flag after a parse error. The auth poll has no try/catch; a deserialize error leaves the flag set, and that component's bar stops updating until a domain reload. The image request keeps running.

The bar is cleared when a text-to-image, UI image-to-image, or text-to-material request reaches the end of its coroutine, including HTTP failure and the "no image" path. It is not cleared if an exception escapes before that line (for example a bad model index). `StableDiffusionText2Material` and `StableDiffusionImage2Material` call `EditorUtility.ClearProgressBar()` from `Start`, which runs when entering Play Mode. There is no cancel button. The code never calls `Abort` or `Dispose` on the request. Disabling the component or its GameObject stops the coroutine, which skips that cleanup, so `generating` stays true and the bar can remain. The HTTP request is not explicitly cancelled.

A bar left on screen after a failed click can be cleared by entering Play Mode on a scene that contains an active material generator (its `Start` clears the bar), or by a later generation that reaches its cleanup line.

<a id="user-guide-play-mode"></a>

## Play Mode

**Generate** is not disabled in Play Mode. Inside the Editor, Play Mode still defines `UNITY_EDITOR`, so the progress bar and the file write still run. `WriteImageFile` still calls `AssetDatabase.SaveAssets()` during Play Mode. The `ForceUpdate` refresh, `MarkAllScenesDirty`, script reload, and Scene-view framing are skipped while `Application.isPlaying` is true. Stopping Play Mode restores scene objects to their pre-play state. The PNG written during play remains on disk. The in-scene material or sprite assigned during play is discarded with the rest of the play session.

`InspectorRefresh` listens for a return to Edit Mode and exposes `ForceInspectorToRefresh`. Nothing in the project subscribes to that event or calls that method. Returning from Play Mode does not, by itself, reimport the new PNG onto the object.

<a id="user-guide-saving-and-what-survives-a-restart"></a>

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

<a id="user-guide-operations-that-destroy-data"></a>

## Operations that destroy data

**Generate** on a component that already has a PNG deletes that file first. Copy the PNG if you might want it. Generating a parent with **Apply Recursively** replaces child renderers' shared materials. Duplicate the object (new guid) when you want a second result beside the first.

Field edits and generation are not wrapped in Unity's Undo system.

<a id="user-guide-demo-scene-contents"></a>

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

<a id="configuration"></a>

<!-- source: docs/configuration.md -->

<a id="configuration-configuration"></a>

# Configuration

There is no settings window and no environment-variable layer. Configuration is Unity serialization: one `SDSettings` asset, plus fields on the configuration object and on each generator.

Changes apply to the next request as soon as they are on the live objects. Nothing is generated until you click **Generate** or **List Models**. Save the scene to keep component edits. Save the project so the `SDSettings` asset keeps edits. There is no separate Apply button.

<a id="configuration-sdsettings-asset"></a>

## SDSettings asset

Create one with **Assets > Create > SDSettings**. The menu always writes `Assets/NewSDSettings.asset`. If that path already exists, Unity's `AssetDatabase.CreateAsset` fails. Rename or move the existing asset first. The menu does not copy `DefaultLocalSDSettings`; a new asset gets the field initializers below.

The included asset is `Assets/Stable-Diffusion-Unity-Integration/Settings/DefaultLocalSDSettings.asset`. Assign it to **Settings** on `StableDiffusionConfiguration`. Generators do not reference the asset themselves. They look up the configuration object in loaded scenes.

The Inspector groups fields under the headers **AUTOMATIC1111 Settings**, **API Settings**, and **URP Settings**.

<a id="configuration-server-and-api-paths"></a>

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

<a id="configuration-api-settings"></a>

### API Settings

| Inspector label | Field | Default | Effect |
|---|---|---|---|
| Use Auth | `useAuth` | false | When true **and** user and pass are both non-empty, requests send `Authorization: Basic ...`. |
| User | `user` | empty | HTTP Basic user. Matches WebUI `--api-auth user:password`. Not a separate API-key header. |
| Pass | `pass` | empty | HTTP Basic password, stored in plain text in the asset YAML. |

If **Use Auth** is on but either field is empty, the header is omitted. The tool does not warn at request time. A later 401 or 403 from **List Models** logs `Authentication error: Server requires an API key. Please enable useAuth and set user/pass fields in your SDSettings.` The words "API key" in that sentence mean this username and password.

Auth is applied on **List Models**, on the model-switch POST, on text-to-image, on `StableDiffusionImage2Image`, and on the non-material progress poll when credentials are set. `StableDiffusionImage2Material` does not send the header and always uses the non-auth progress poll.

<a id="configuration-urp-settings"></a>

### URP Settings

| Inspector label | Field | Default | Effect |
|---|---|---|---|
| Use Universal Render Pipeline | `useUniversalRenderPipeline` | false | Read only by `StableDiffusionText2Material` when it creates a material. False selects shader `Standard`. True selects `Universal Render Pipeline/Lit`. Ignored by `StableDiffusionImage2Material`, which always uses `Standard`. |

Turning this on does not install URP and does not convert the project. The demo project's graphics settings are Built-in. URP install is Unity's documentation: https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@14.0/manual/InstallingAndConfiguringURP.html

<a id="configuration-output-folder"></a>

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

<a id="configuration-stablediffusionconfiguration-object"></a>

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

<a id="configuration-per-generator-fields"></a>

## Per-generator fields

These live on the generator component, not in `SDSettings`.

<a id="configuration-shared-by-the-four-generators"></a>

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

<a id="configuration-text-to-material-and-image-to-material-only"></a>

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

<a id="configuration-what-is-not-configurable"></a>

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

<a id="configuration-credentials-and-sharing"></a>

## Credentials and sharing

Do not commit an `SDSettings` asset that contains a real password. Do not paste the asset, or a full Console log of the JSON body, into a public issue. The generation log prints the text-to-image JSON, including the prompt. Image-to-image logs only the first 100 characters of the JSON. The password is not written to the Console by these scripts, but it is in the asset file.

<a id="reference"></a>

<!-- source: docs/reference.md -->

<a id="reference-reference"></a>

# Reference

Behaviour below is taken from the current C# sources. WebUI may add its own validation on top. When a rule here and a WebUI rule both apply, this project does the local half and then forwards the JSON.

<a id="reference-versions"></a>

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

<a id="reference-endpoints-this-project-calls"></a>

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

<a id="reference-text-to-image-json-fields-the-code-sets"></a>

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

<a id="reference-image-to-image-json-fields-the-code-sets"></a>

## Image-to-image JSON fields the code sets

| JSON field | UI image-to-image | Image-to-material |
|---|---|---|
| `init_images` | One base64 PNG of **Input Texture** | One base64 PNG of the renderer's current main texture |
| `prompt`, `negative_prompt`, `steps`, `cfg_scale`, `width`, `height`, `seed` | From the component | From the component |
| `tiling` | false | false |
| `denoising_strength` | class default 0.75 | slider, default 0.65 |
| `sampler_name` | same index rule as text-to-image | same |

Also left at class defaults: `resize_mode` 0, `mask_blur` 4, `inpainting_fill` 0, `inpaint_full_res` true, `initial_noise_multiplier` 1, `include_init_images` false, `sampler_index` `"Euler"`. The `mask` field is commented out of the payload class. `script_name` is commented out.

<a id="reference-response-handling"></a>

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

<a id="reference-local-validation"></a>

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

<a id="reference-files-and-unity-objects"></a>

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

<a id="reference-material-property-names-written"></a>

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

<a id="reference-progress-json-read"></a>

## Progress JSON read

`SDProgress.progress` is the 0–1 bar value. The label is that value times 100. The non-auth poll formats the label with `F1`. The auth poll uses default number formatting and has no try/catch. `eta_relative`, `current_image`, `textinfo`, and the nested `state` object are deserialized and ignored. `current_image` is not shown.

<a id="reference-editor-only-versus-compiled-everywhere"></a>

## Editor-only versus compiled-everywhere

Inside `#if UNITY_EDITOR`: dimension clamp, guid creation, progress bar, asset database save, scene dirty flags, shader refresh.

Not inside that define: the HTTP request, file write, material creation, and `Generate()` itself. `using UnityEditor` appears unconditionally in `StableDiffusionGenerator.cs` and `ReadOnlyAttribute.cs`, which are outside an `Editor` folder. A player build's script compile does not reference the `UnityEditor` assembly, so those two files fail that compile. This was determined by reading the source, not by running a build.

`ReadOnlyAttribute` is a `PropertyAttribute`. `ReadOnlyDrawer` (in the Editor folder) sets `GUI.enabled = false` around the property. The field can still be changed in Debug mode or by editing the scene YAML.

<a id="reference-external-responsibility"></a>

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

<a id="troubleshooting"></a>

<!-- source: docs/troubleshooting.md -->

<a id="troubleshooting-troubleshooting"></a>

# Troubleshooting

Match the Console string first. The exact messages are listed in [Reference](#reference-response-handling). Prefer the smallest check that does not delete data. **Generate** deletes the existing `{guid}.png` before the server answers, so do not click it again as a test until you have copied any PNG you care about.

WebUI-internal faults (CUDA out of memory, a bad checkpoint file, a sampler crash inside the Python process) are reported here only as "the body WebUI returned". Fix those with WebUI's own issue tracker and docs, not by changing Unity sliders at random.

<a id="troubleshooting-generate-does-nothing"></a>

## Generate does nothing

**Symptom:** **Generate** clicks, no progress bar, no new Console error.

**Causes:**

- **Prompt** is empty or only whitespace is not the check; the check is `string.IsNullOrEmpty`. A prompt of spaces is sent. A truly empty prompt returns silently.
- UI image-to-image has no **Input Texture**. That also returns silently.
- `generating` is still true from an earlier run that threw. The flag is invisible in the default inspector.

**Checks:** Confirm the prompt length, the input texture, and whether a previous red exception mentioned this component.

**Resolution:** Fill the prompt or texture. Activate the GameObject and enable the component. If a previous run threw or you disabled the object mid-request, reload the scene or trigger a script recompile so the flag resets, then generate once.

**Expected result:** A progress bar appears within a second. Text-to-image, UI image-to-image, and text-to-material log `Sending request to:`. Image-to-material does not log that line.

<a id="troubleshooting-model-popup-throws-when-you-select-the-object"></a>

## Model popup throws when you select the object

**Symptom:** Inspector exception as soon as you select a generator. Often `NullReferenceException` or `ArgumentNullException` from `EditorGUILayout.Popup`.

**Causes:** No active `StableDiffusionConfiguration` in loaded scenes, or `modelNames` is still null because **List Models** has never succeeded on this object.

**Checks:** Search the hierarchy for `StableDiffusionConfiguration`. Confirm it is active. Look at **Model Names**. Null and a size of 0 are different; null crashes the popup.

**Resolution:** Add or enable the configuration, assign **Settings**, start WebUI, click **List Models**. You can also set **Model Names** size to 0 in the Inspector to get past the null popup, then list models.

**Expected result:** **Sampler** and **Model** draw, and **Model** has one entry per checkpoint `model_name`.

<a id="troubleshooting-list-models-fails-immediately"></a>

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

<a id="troubleshooting-wrong-checkpoint-or-an-out-of-range-model"></a>

## Wrong checkpoint or an out-of-range model

**Symptom:** `IndexOutOfRangeException` after **Generate**, or an image that clearly used a different checkpoint than the one you picked. The demo `Button` stores model index 92 while `DemoScene` only has 17 saved names. The no-post-processing scene stores 3 names and several indexes above 2.

**Causes:** `selectedModel` is a raw integer. **List Models** replaces the name array and does not remap indexes. The set-checkpoint call sends `model_name`, not the WebUI title, and a failed set does not cancel the image POST.

**Checks:** Count **Model Names**. Compare **Model**'s stored index (switch the Inspector to Debug mode if the popup hides it). Read the Console for `Failed to set model` or `All attempts to set model failed`. Look at WebUI's own model indicator.

**Resolution:** Click **List Models**, then choose the model again on every generator so the index is inside the new array. If the index is valid and WebUI does not switch, this project has no further switch mechanism. Use WebUI's API docs for the options payload it expects, and compare that with the logged JSON `{"sd_model_checkpoint":"..."}`.

**Expected result:** No exception, and WebUI shows the checkpoint you intended before the image returns.

**Data note:** the exception happens after the old PNG is deleted.

<a id="troubleshooting-http-errors-during-generate"></a>

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

<a id="troubleshooting-no-image-in-the-response"></a>

## No image in the response

**Symptom:** `No image was returned by the server` or the image-to-material wording `No image was return by the server`.

**Cause:** The HTTP call succeeded and the JSON parsed, but `images` was missing or empty. That is a server-side response shape this project does not repair.

**Checks:** Read `Full response:` in the Console. If the body is an error object, treat it as WebUI's message.

**Resolution:** Fix the condition WebUI reported. There is no second image slot to fall back to.

<a id="troubleshooting-progress-bar-stuck-or-progress-unknown"></a>

## Progress bar stuck or "Progress unknown..."

**Symptom:** The bar stays up after the click, or it sits on `Progress unknown...`.

**Causes:** The progress GET failed (the image request may still be running). An exception skipped `ClearProgressBar`. Image-to-material's auth-less poll failed the parse. Play Mode was stopped mid-request.

**Checks:** Look for `Failed to get progress` or `Error parsing progress`. See whether `Sending request to:` was followed later by `Response received` or `Request error`.

**Resolution:** Wait if the image request is still running; a large step count on a slow GPU can take minutes and the tool has no timeout. If the auth progress poll threw while parsing, that component stops updating the bar until a domain reload; the image request can still finish. If the request already ended and the bar remains, enter Play Mode with an active `StableDiffusionText2Material` or `StableDiffusionImage2Material` in the scene (their `Start` clears the bar), then stop Play Mode. You can also trigger another generation that reaches its cleanup. There is no cancel control. Disabling the component stops the coroutine and skips cleanup. It does not call `Abort` on the request, and `generating` stays true.

**Expected result:** The bar closes. A successful run also leaves a PNG.

<a id="troubleshooting-input-image-errors"></a>

## Input image errors

**Symptom:** `Input Image ... isn't readable` or `Texture ... is not readable` / `EncodeToPNG` exception from image-to-material.

**Resolution for UI image-to-image:** Select the texture, enable **Read/Write**, disable compression if the help box's "uncompressed" note matters for your file, click Apply.

**Resolution for image-to-material:** The source is the renderer's current main texture, not a field you can swap in the generator. It must be a `Texture2D` with Read/Write. Render textures and models' non-readable imports throw. This component also will not send auth.

**Expected result:** For the UI component, the readable error disappears and the next **Generate** logs `Sending request to:`.

<a id="troubleshooting-material-looks-wrong"></a>

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

<a id="troubleshooting-the-png-is-missing"></a>

## The PNG is missing

**Symptom:** Generation seemed to run, but there is no file, or the previous file vanished.

**Checks:**

1. Read the guid on the component.
2. Look in `Assets/StreamingAssets/SDImages` and `SDMaterials`, and also in `Assets/streamingAssets` if you created that folder.
3. Remember `.gitignore` hides `StreamingAssets` from git status. It does not hide it from the Project window. If the Project window does not refresh, right-click the folder and choose Refresh, or focus Unity so `AssetDatabase.Refresh` from a successful edit-mode run can finish.
4. If the Console shows an exception after `Sending request to:` never appeared, the delete step may already have removed the previous file and the new one was never written.

**Resolution:** Copy important PNGs before generating again. There is no trash folder and no restore command in this project. If the new bytes were written, the path logged indirectly by the guid is the location. The code does not log the full output path on success.

<a id="troubleshooting-authentication-works-in-the-browser-and-fails-in-unity"></a>

## Authentication works in the browser and fails in Unity

**Symptom:** Browser with credentials succeeds; Unity gets 401.

**Checks:** **Use Auth** enabled, both fields non-empty, no extra space, same asset assigned on the configuration you are actually hitting. Image-to-material cannot succeed against an authenticated server with the current code.

**Resolution:** Fix the three fields and use a component that sends the header (`StableDiffusionText2Image`, `StableDiffusionImage2Image`, or `StableDiffusionText2Material`).

<a id="troubleshooting-editor-freeze-while-setting-the-model"></a>

## Editor freeze while setting the model

**Symptom:** Unity stops pumping the UI for a while after **Generate**, before the progress bar moves.

**Cause:** If the `UnityWebRequest` options call fails, `SetModelAsync` calls `HttpWebRequest.GetResponse()`, which blocks the Editor thread. A dead host can stall the Editor until the operating-system timeout.

**Resolution:** Make the options URL reachable. Avoid the fallback by keeping WebUI's `/sdapi/v1/options` working. There is no timeout field to lower.

<a id="troubleshooting-compile-errors"></a>

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

<a id="troubleshooting-demo-scene-missing-script"></a>

## Demo scene missing script

**Symptom:** `PostProcessingVolume` shows a missing script on `DemoScene`.

**Cause:** `com.unity.postprocessing` is not in `Packages/manifest.json`.

**Resolution:** Install the Post Processing package, reopen the scene. The volume is global and references `PostProcessingVolume Profile.asset`. If the profile link is lost, assign that asset to the volume's profile. Or open `DemoScene-NoPostProcessing` instead. Generation does not use the volume.

<a id="troubleshooting-play-mode-looked-successful-and-then-reverted"></a>

## Play Mode looked successful and then reverted

**Symptom:** The mesh or UI updated while playing, and stopping Play Mode restored the old look.

**Cause:** Scene changes during Play Mode are discarded by Unity. The PNG on disk is kept. `InspectorRefresh.ForceInspectorToRefresh` is never called.

**Resolution:** Generate in Edit Mode when you want the scene to keep the assignment. To use a PNG made during Play Mode, assign the file onto the material or graphic after you leave Play Mode.

<a id="troubleshooting-what-to-collect-if-you-open-an-issue"></a>

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

<a id="architecture"></a>

<!-- source: docs/architecture.md -->

<a id="architecture-architecture"></a>

# Architecture

This file explains how the pieces interact so support answers stay consistent. It is not an invitation to treat private classes as a stable API.

<a id="architecture-components"></a>

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

<a id="architecture-request-lifecycle"></a>

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

<a id="architecture-how-objects-are-found"></a>

## How objects are found

`FindAnyObjectByType<StableDiffusionConfiguration>()` is called from property getters when the static field `StableDiffusionGenerator.sdc` compares equal to null, and again before generation under the same check. Unity's overloaded `==` treats a destroyed object as null, so deleting the configuration causes the next lookup to search again. A disabled object is not null. If it was cached while active, later calls keep using it. The parameterless find returns an active loaded object, so a configuration that is already disabled is not found, and the getter then throws when it reads `samplers`. A domain reload clears the static. The code does not subscribe to scene changes.

Child mesh search (`FindInChildrenAll`) walks the transform's children, skips inactive GameObjects when `active` is true (the default), collects components of type `T`, and recurses. It does not include the GameObject it was called on. `GetMeshRenderer` checks the host first, then the children.

<a id="architecture-persistence-model"></a>

## Persistence model

- ScriptableObject: server profile.
- Scene component fields: prompts and parameters, including hidden `selectedSampler` and `selectedModel`.
- Disk PNG: the only file the generators create.
- In-memory `Material` and normal-map `Texture2D`: referenced by the renderer. Edit-mode success marks the scene dirty and, for materials, calls `EditorUtility.SetDirty` on the textures. There is no `AssetDatabase.CreateAsset`.
- `generating` and the cached configuration pointer: not serialized.

`WriteImageFile` wraps the write in `AssetDatabase.StartAssetEditing` / `StopAssetEditing` and `SaveAssets` in the Editor. UI and text-to-material paths do this. Image-to-material writes inside the coroutine with `WriteAsync` instead, also bracketed by asset editing in the Editor.

<a id="architecture-what-runs-in-edit-mode"></a>

## What runs in Edit Mode

`StableDiffusionConfiguration` is `[ExecuteInEditMode]`. Generators are `[ExecuteAlways]`. That is what allows coroutines started from an Inspector button to keep running while the Editor is not in Play Mode. `Update` on the generators also runs in Edit Mode, which is how the dimension clamp, guid assignment, and material slider updates happen without entering Play Mode.

<a id="architecture-external-boundary"></a>

## External boundary

This process never loads a neural network. It never reads WebUI's `models` directory from disk. It only speaks HTTP to the configured origin. Output files that WebUI writes into its own folders are ignored. Conversely, WebUI does not know about Unity materials; applying pixels to a renderer is entirely local.

The normal-map function is local CPU code over the colour texture. It is not a WebUI script and not a second network.

<a id="architecture-editor-ui-map"></a>

## Editor UI map

| Control | Script |
|---|---|
| **List Models** | `StableDiffusionConfigurationEditor` |
| **Assets/Create/SDSettings** | `SDSettingsEditor` |
| **Sampler**, **Model**, **Generate** | One editor each for text-to-image, UI image-to-image, and text-to-material |
| Read-only guid | `ReadOnlyDrawer` |
| Help box on UI image-to-image | `StableDiffusionImage2ImageEditor` |

`selectedSampler` and `selectedModel` are assigned on the component instance inside `OnInspectorGUI`. The editors call `serializedObject.ApplyModifiedProperties()` but do not route those two popup values through `SerializedProperty`. They are public fields, and the demo scenes show them serialized, so saving the scene does store them. The editors do not call `Undo.RecordObject`.

<a id="architecture-assemblies"></a>

## Assemblies

There is no asmdef. Runtime-folder scripts compile into `Assembly-CSharp`. `Scripts/Editor` compiles into `Assembly-CSharp-Editor`. `ReadOnlyAttribute.cs` is on the runtime side and references `UnityEditor`, which is the player-build problem described in the development notes.

<a id="development"></a>

<!-- source: docs/development.md -->

<a id="development-development"></a>

# Development

<a id="development-layout"></a>

## Layout

```text
Assets/Stable-Diffusion-Unity-Integration/Scripts/          generator and settings scripts
Assets/Stable-Diffusion-Unity-Integration/Scripts/Editor/   inspectors and ReadOnly drawer
Assets/Plugins/Newtonsoft.Json.dll                         JSON
Assets/Stable-Diffusion-Unity-Integration/Scenes/          two demo scenes and a post-processing profile
Assets/Stable-Diffusion-Unity-Integration/Settings/        DefaultLocalSDSettings.asset
Stable-Diffusion-Unity-Integration.unitypackage            packaged snapshot at the repo root
docs/                                                      support documents
tools/build_ai_support.py                                  builds and checks AI-SUPPORT.md
```

Namespaces are not used. Class names are global.

<a id="development-open-and-compile"></a>

## Open and compile

Open the repository root in the editor recorded by `ProjectVersion.txt`, **2021.3.33f1**, or another editor that contains `Object.FindAnyObjectByType`. Unity staff listed that method from 2020.3.45f1, 2021.3.18f1, 2022.2.5f1, and 2023.1.0a20 onward (https://discussions.unity.com/t/findobjectsbytype-and-its-siblings/910153/1). 2021.3.33f1 is past 2021.3.18f1. Unity 2019 is not. This documentation pass did not click Play or run a build.

Package dependencies in `Packages/manifest.json` are the stock modules of that 2021 project plus IDE integrations, uGUI, TextMeshPro, Timeline, Visual Scripting, and the Test Framework. None of those packages is called by the generator scripts except uGUI types `Image` and `RawImage`.

<a id="development-tests"></a>

## Tests

There are no `EditMode` or `PlayMode` tests. `com.unity.test-framework` is installed and unused. Do not claim a test run for this project. A useful future test would mock HTTP; nothing in the tree does that today.

<a id="development-player-builds"></a>

## Player builds

Do not expect a player build to succeed while these scripts are in `Assets`.

- `StableDiffusionGenerator.cs` has a top-level `using UnityEditor`.
- `ReadOnlyAttribute.cs` has a top-level `using UnityEditor`.

Both files are outside `Scripts/Editor`. The generator methods that draw the progress bar are wrapped in `#if UNITY_EDITOR`, but the using directive is not. Unity does not reference `UnityEditor` when compiling player scripts, so the player script compile fails. That failure was inferred from the source layout and was not reproduced by producing a player.

The README's statement that a game built with this tool need not be open source is about the licence, not about these scripts compiling into a player. The intended use is generation in the Editor, then ordinary materials and sprites in the game, without calling WebUI at runtime.

<a id="development-extending"></a>

## Extending

There is no plugin API, no events, and no virtual `Generate` hook beyond the public methods already on the components. `Generate()` is public, so another Editor script can call it. `StableDiffusionImage2Material.Generate` is the only way to start that component, because it has no button.

If you add a user-visible field, button, error string, or request field, update the matching document under `docs/` and rebuild `AI-SUPPORT.md` in the same change. The project rule for that lives in `.cursor/rules/documentation-maintenance.mdc`.

Do not document WebUI features as if this project exposed them. Add a real Inspector control and a real JSON assignment before the user guide mentions them.

Private request classes in `SDSettings.cs` are the payload. `sampler_index` staying at `"Euler"` is easy to miss if you add samplers and only set `sampler_name`.

<a id="development-package"></a>

## Package

`Stable-Diffusion-Unity-Integration.unitypackage` is checked in at the repository root. It was not extracted while writing these docs. After you change scripts, rebuild the package from the Unity **Export Package** menu if you still ship that file, or the package and the scripts will diverge. The docs describe the scripts.

<a id="development-documentation-commands"></a>

## Documentation commands

From the repository root, with Python 3:

```text
python tools/build_ai_support.py
python tools/build_ai_support.py --check
```

The first command rewrites `AI-SUPPORT.md`. The check command compares a fresh build with the committed file and does not write it. It also checks that the root `README.md` links to `AI-SUPPORT.md` and contains the AI-help heading. The repository has no CI workflow, so nothing runs this check automatically. Adding a new CI system was intentionally skipped.

<a id="development-contributing"></a>

## Contributing

The README asks contributors to clone the repository, keep a pull request to one change, avoid formatting-only diffs, and include reproduction steps for bug fixes. Pull requests go through GitHub on https://github.com/dobrado76/Stable-Diffusion-Unity-Integration . Issues go to https://github.com/dobrado76/Stable-Diffusion-Unity-Integration/issues .

There is no supported release branch, no tag, and no response-time commitment. The Buy Me a Coffee link in the README (https://www.buymeacoffee.com/dobrado76) is an appreciation link, not a support channel.

<a id="development-source-history-that-explains-user-visible-names"></a>

## Source history that explains user-visible names

The README changelog is the history of user-visible behaviour:

- 1.1.0 added the manual sampler list.
- 1.2.0 showed the guid and tried to limit asset refresh outside Play Mode. The refresh helper that remains (`InspectorRefresh`) is not called.
- 1.2.2 made generation asynchronous and added the progress bar.
- 1.2.4 renamed the old image and material components to the Text2 names and added `StableDiffusionImage2Image`.
- 1.2.5 added HTTP Basic fields and the URP shader switch.
- 1.2.6 added `StableDiffusionImage2Material`.
- 1.3.0 changed the HTTP client toward hosted servers and added the 405 log text that mentions RunPod.

When an old tutorial names `StableDiffusionImage` or `StableDiffusionMaterial`, translate those to `StableDiffusionText2Image` and `StableDiffusionText2Material`.

<a id="FAQ"></a>

<!-- source: docs/FAQ.md -->

<a id="FAQ-faq"></a>

# FAQ

Answers describe the current scripts. WebUI features that have no Inspector control are answered as "not in this project".

<a id="FAQ-basics"></a>

## Basics

<a id="FAQ-1-what-is-stable-diffusion-unity-integration"></a>

### 1. What is Stable Diffusion Unity Integration?

It is a Unity Editor tool that calls an AUTOMATIC1111 WebUI HTTP API and puts the returned image on a UI graphic or a mesh material. It does not run the diffusion model itself. Repository: https://github.com/dobrado76/Stable-Diffusion-Unity-Integration

<a id="FAQ-2-which-version-do-these-answers-describe"></a>

### 2. Which version do these answers describe?

The README changelog calls the current user-facing release **1.3.0** (2025-03-30). Unity Player Settings still say bundle version **1.2.1**. There are no git tags. If you imported an older `unitypackage` and the inspector does not match this FAQ, the package is older than the scripts these answers describe.

<a id="FAQ-3-does-the-download-include-stable-diffusion-or-any-checkpoints"></a>

### 3. Does the download include Stable Diffusion or any checkpoints?

No. You install [AUTOMATIC1111 WebUI](https://github.com/AUTOMATIC1111/stable-diffusion-webui) and its models separately. The Unity side only stores the server URL and the model names the server reports.

<a id="FAQ-4-can-i-use-comfyui-forge-or-the-stability-api"></a>

### 4. Can I use ComfyUI, Forge, or the Stability API?

Only if that server accepts the same requests this project sends: GET `/sdapi/v1/sd-models`, POST `/sdapi/v1/options` with `sd_model_checkpoint`, POST `/sdapi/v1/txt2img` and `/sdapi/v1/img2img`, and GET `/sdapi/v1/progress`. There is no ComfyUI graph, no Stability REST client, and no vendor SDK. A different API will fail at list or generate time. Use that product's docs for its own API, and use this FAQ for the JSON this Unity project actually posts.

<a id="FAQ-5-which-unity-version-should-i-open"></a>

### 5. Which Unity version should I open?

Use the editor in `ProjectVersion.txt`, **2021.3.33f1**, or another editor that has the APIs the scripts call. `UnityWebRequest.Result` exists from Unity 2020.2, so Unity 2019 does not compile this project. `FindAnyObjectByType` was backported; Unity staff listed 2020.3.45f1, 2021.3.18f1, 2022.2.5f1, and 2023.1.0a20 (https://discussions.unity.com/t/findobjectsbytype-and-its-siblings/910153/1). 2021.3.33f1 is newer than 2021.3.18f1, so that method is expected to exist there. This was not re-checked by launching an editor. The README also names Unity 2019; that claim does not match the source.

<a id="FAQ-6-how-do-i-install-it"></a>

### 6. How do I install it?

Either open a clone of the repository as the Unity project, or import `Stable-Diffusion-Unity-Integration.unitypackage` from the repository root with **Assets > Import Package > Custom Package**. You still need a running WebUI with `--api`. Step-by-step checks are in [Getting started](#getting-started).

<a id="FAQ-7-where-is-the-demo-scene-the-readme-path-does-not-exist"></a>

### 7. Where is the demo scene? The README path does not exist.

Open `Assets/Stable-Diffusion-Unity-Integration/Scenes/DemoScene.unity` or `DemoScene-NoPostProcessing.unity`. The README path `StableDiffusionIntegration/Scenes/DemoScene.unity` is leftover wording.

<a id="FAQ-8-why-does-demoscene-say-a-script-is-missing"></a>

### 8. Why does DemoScene say a script is missing?

The object `PostProcessingVolume` uses the Post Processing Stack v2 script. That package is not in `Packages/manifest.json`. Install Unity's Post Processing package and reopen the scene, or use the no-post-processing scene. Generation does not need the volume. The volume is global, weight 1, and its profile is `Assets/Stable-Diffusion-Unity-Integration/Scenes/PostProcessingVolume Profile.asset`.

<a id="FAQ-9-is-this-a-runtime-feature-in-my-game"></a>

### 9. Is this a runtime feature in my game?

The buttons are Editor inspectors. The generator scripts are not inside an `Editor` folder, so they are part of the player compile, and two of them import `UnityEditor` unconditionally, which fails a player build. Use the tool in the Editor, save the PNG or the scene, and ship ordinary textures. There is no in-game generate UI.

<a id="FAQ-10-where-is-the-video-and-the-project-page"></a>

### 10. Where is the video and the project page?

The README links a demo video: https://youtu.be/8gf5GPzhgk8 . The source repository is https://github.com/dobrado76/Stable-Diffusion-Unity-Integration .

<a id="FAQ-server-connection"></a>

## Server connection

<a id="FAQ-11-what-url-should-i-put-in"></a>

### 11. What URL should I put in?

`http://127.0.0.1:7860` for a default local WebUI. The field is **Stable Diffusion Server URL** on the `SDSettings` asset assigned to `StableDiffusionConfiguration`. Do not add a trailing slash. Do not append `/sdapi`; the path fields already contain `/sdapi/v1/...`.

<a id="FAQ-12-how-do-i-turn-on-the-api"></a>

### 12. How do I turn on the API?

Start WebUI with `--api`. On a typical Windows install that is `COMMANDLINE_ARGS` in `webui-user.bat`. Flag definitions are WebUI's: https://github.com/AUTOMATIC1111/stable-diffusion-webui/wiki/Command-Line-Arguments-and-Settings . Then browse to `http://127.0.0.1:7860/sdapi/v1/sd-models` and confirm you see a JSON array before you click anything in Unity.

<a id="FAQ-13-where-is-the-list-model-button"></a>

### 13. Where is the List Model button?

The button text is **List Models**, on the `StableDiffusionConfiguration` inspector, under the default fields. It is not on the generator components.

<a id="FAQ-14-what-does-a-successful-list-look-like"></a>

### 14. What does a successful list look like?

The Console logs `Successfully loaded N models`, and **Model Names** on the configuration object fills with strings. Those strings are each JSON object's `model_name`. Titles, hashes, and filenames are discarded.

<a id="FAQ-15-why-does-the-error-mention-port-7860-when-i-use-another-port"></a>

### 15. Why does the error mention port 7860 when I use another port?

The string `Make sure Stable Diffusion is running locally on port 7860, or update the URL in your settings.` is fixed text on some list-model failures. The URL that was actually called is the earlier line `Requesting models from:`.

<a id="FAQ-16-what-is-the-apisd-models-request"></a>

### 16. What is the `/api/sd-models` request?

A fallback GET this project tries after a 404 or a JSON parse failure on the primary models URL. Stock WebUI's documented route is `/sdapi/v1/sd-models`. If the primary path 404s, fix `--api` or the base URL. Do not reconfigure WebUI to serve `/api/sd-models` unless you know you are behind a proxy that uses that prefix.

<a id="FAQ-17-can-i-change-the-api-paths"></a>

### 17. Can I change the API paths?

Yes. **Models API**, **Text To Image API**, **Image To Image API**, **Option API**, and **Progress API** on the settings asset are concatenated onto the server URL. Defaults match stock WebUI. Empty or wrong paths produce 404s. There is no UI to reset them other than typing the defaults back. The defaults are listed in [Configuration](#configuration-server-and-api-paths).

<a id="FAQ-18-does-it-support-https-and-a-remote-host"></a>

### 18. Does it support HTTPS and a remote host?

The URL is a string. `https://` is sent if you type it. Certificate failures show up as connection errors from Unity. The project does not add a certificate bypass. Hosted WebUI, including a RunPod instance, is the same mechanism: put the origin in **Stable Diffusion Server URL**. There is no RunPod panel, pod template, or proxy login. Vendor docs live at https://www.runpod.io/ . HTTP 405 during generate logs extra sentences that mention RunPod; those sentences are hints, not a setup wizard.

<a id="FAQ-19-why-did-i-get-405-method-not-allowed"></a>

### 19. Why did I get 405 Method Not Allowed?

The three `UnityWebRequest` generators log `405 Method Not Allowed - This is common with RunPod...` plus a line about CORS or an API proxy. The status itself comes from the server or the proxy in front of it. Check that `--api` is on and that the logged URL is the API origin, not a web UI page. CORS flags are a WebUI or proxy concern; this project does not send a CORS configuration.

<a id="FAQ-authentication"></a>

## Authentication

<a id="FAQ-20-how-do-i-send-the-api-password"></a>

### 20. How do I send the API password?

On the settings asset enable **Use Auth** and fill **User** and **Pass** with the same pair you passed to WebUI as `--api-auth user:password`. Both fields must be non-empty. The header is HTTP Basic, not a custom `X-API-Key`. The Console line `Using API key to authenticate` and the 401 text that says "API key" refer to these two fields.

<a id="FAQ-21-i-enabled-use-auth-and-it-still-returns-401"></a>

### 21. I enabled Use Auth and it still returns 401.

One of the strings is empty, the pair does not match `--api-auth`, or you are using `StableDiffusionImage2Material`, which never sets the header. A password made only of spaces is sent, because the check is `Equals("")`, not a trim.

<a id="FAQ-22-where-is-the-password-saved"></a>

### 22. Where is the password saved?

In the `SDSettings` asset YAML, in plain text. It is not printed by the generator logs. Do not commit a real password, and delete it from any file you attach to a public issue.

<a id="FAQ-23-does-list-models-use-the-password"></a>

### 23. Does List Models use the password?

Yes, when **Use Auth** is on and both fields are set. A 401 or 403 stops the list and does not try the alternate URL. The message is `Authentication error: Server requires an API key. Please enable useAuth and set user/pass fields in your SDSettings.`

<a id="FAQ-sdsettings-and-defaults"></a>

## SDSettings and defaults

<a id="FAQ-24-how-do-i-create-another-settings-asset"></a>

### 24. How do I create another settings asset?

**Assets > Create > SDSettings**. The file is always `Assets/NewSDSettings.asset`. Creating a second one fails until you rename or move the first. The new asset gets code defaults, not a duplicate of `DefaultLocalSDSettings`.

<a id="FAQ-25-which-asset-is-the-demo-using"></a>

### 25. Which asset is the demo using?

`StableDiffusionConfiguration` in the demo scenes references `Assets/Stable-Diffusion-Unity-Integration/Settings/DefaultLocalSDSettings.asset` (asset guid `2f0a42d2a132f7144b35b4cabb576d34`).

<a id="FAQ-26-i-changed-sampler-on-the-settings-asset-and-the-image-still-uses-euler-a"></a>

### 26. I changed Sampler on the settings asset and the image still uses Euler a.

The field `sampler` on `SDSettings` is not read. The sampler that is sent is the **Sampler** popup on the generator, which indexes `StableDiffusionConfiguration.samplers`.

<a id="FAQ-27-i-changed-width-and-steps-on-the-settings-asset-and-the-ui-component-ignored-them"></a>

### 27. I changed width and steps on the settings asset and the UI component ignored them.

`StableDiffusionText2Image` starts at width 512 and height 512, and its `Awake` copies settings only when width or height is negative. A new one therefore keeps 512, 90 steps, CFG 7, and seed `-1`. `StableDiffusionImage2Image` is the same except steps start at 50. Material components start at width `-1`, so the first `Awake` does copy the asset (512, 512, 35 steps, CFG 7, seed `-1` on the default asset). After that, the component has its own numbers. Editing the asset does not push updates into objects already in the scene.

<a id="FAQ-28-why-are-brand-new-text-to-image-steps-90"></a>

### 28. Why are brand-new text-to-image steps 90?

That is the field initializer on `StableDiffusionText2Image`. The settings asset default of 35 is a different field and is not applied to this component.

<a id="FAQ-29-do-i-need-to-press-apply-or-save-before-generate"></a>

### 29. Do I need to press Apply or Save before Generate?

Generate reads the live objects. Save the scene to keep component edits, and save the project to keep the settings asset. There is no Apply button. Generation does not wait for a save.

<a id="FAQ-models-and-samplers"></a>

## Models and samplers

<a id="FAQ-30-the-model-dropdown-crashes-as-soon-as-i-click-the-object"></a>

### 30. The Model dropdown crashes as soon as I click the object.

`modelNames` is null until **List Models** succeeds, and the popup cannot take a null array. The sampler popup also throws if no `StableDiffusionConfiguration` is active in a loaded scene. Add the configuration, assign settings, and list models. Setting **Model Names** size to 0 avoids the null crash but leaves nothing to select.

<a id="FAQ-31-i-listed-models-generated-and-got-indexoutofrangeexception"></a>

### 31. I listed models, generated, and got IndexOutOfRangeException.

`selectedModel` is a stored integer. It is not clamped. `DemoScene` has 17 saved names and the `Button` object stores index 92. `DemoScene-NoPostProcessing` has 3 names and several objects store indexes well above 2. After every **List Models**, choose **Model** again on each generator. The exception happens after `{guid}.png` has already been deleted.

<a id="FAQ-32-the-model-i-chose-is-not-the-one-webui-used"></a>

### 32. The model I chose is not the one WebUI used.

This project POSTs `{"sd_model_checkpoint":"<model_name>"}`. It does not send the checkpoint title or hash, and it does not cancel the image request when the options call fails. Check the Console for `Failed to set model using UnityWebRequest` or `All attempts to set model failed`. If the JSON you see does not match what your WebUI build expects, that difference is the integration boundary. WebUI's options behaviour is documented with its API: https://github.com/AUTOMATIC1111/stable-diffusion-webui/wiki/API

<a id="FAQ-33-does-listing-models-switch-the-checkpoint"></a>

### 33. Does listing models switch the checkpoint?

No. Listing only fills **Model Names**. The options POST runs at the start of **Generate**.

<a id="FAQ-34-can-the-tool-download-the-sampler-list"></a>

### 34. Can the tool download the sampler list?

No. Edit the **Samplers** array on `StableDiffusionConfiguration`. The default names are Euler a, Euler, LMS, Heun, DPM2, DPM2 a, DPM++ 2S a, DPM++ 2M, DPM++ SDE, DPM fast, DPM adaptive, LMS Karras, DPM2 Karras, DPM2 a Karras, DPM++ 2S a Karras, DPM++ 2M Karras, DPM++ SDE Karras, DDIM, and PLMS. Unknown names fail on the server.

<a id="FAQ-35-i-picked-dpm-2m-and-the-result-looks-like-euler"></a>

### 35. I picked DPM++ 2M and the result looks like Euler.

The popup writes `sampler_name`. The same JSON also contains `sampler_index` fixed as `Euler`, and the code never changes that second field. Which one your WebUI applies is up to that WebUI version. Compare with a direct API call before treating it as a Unity UI bug.

<a id="FAQ-36-what-if-i-have-two-stablediffusionconfiguration-objects"></a>

### 36. What if I have two StableDiffusionConfiguration objects?

Generators keep the first one `FindAnyObjectByType` returned, in a static field. Which object that is when several are active is not specified by this project. Use one active configuration.

<a id="FAQ-generate-buttons-and-prompts"></a>

## Generate buttons and prompts

<a id="FAQ-37-where-is-the-generate-button"></a>

### 37. Where is the Generate button?

At the bottom of the inspector for `StableDiffusionText2Image`, `StableDiffusionImage2Image`, and `StableDiffusionText2Material`, under the **Sampler** and **Model** popups. `StableDiffusionImage2Material` does not have one.

<a id="FAQ-38-why-is-there-no-generate-button-on-image-to-material"></a>

### 38. Why is there no Generate button on image-to-material?

`Scripts/Editor` has no custom editor for `StableDiffusionImage2Material`. The method `Generate()` exists and is public, and the default inspector does not draw a button for it. Sampler and model indexes stay at 0 unless you edit the hidden fields. The component also skips HTTP Basic and always creates a Standard material.

<a id="FAQ-39-i-click-generate-and-absolutely-nothing-happens"></a>

### 39. I click Generate and absolutely nothing happens.

The prompt is empty, UI image-to-image has no input texture, or an earlier run left `generating` true after an exception. None of those three cases logs a line. Reload the scene to clear a stuck flag, then try again with a non-empty prompt.

<a id="FAQ-40-what-are-the-old-names-stablediffusionimage-and-stablediffusionmaterial"></a>

### 40. What are the old names StableDiffusionImage and StableDiffusionMaterial?

Version 1.2.4 renamed them to `StableDiffusionText2Image` and `StableDiffusionText2Material`. The current tree does not contain the old class names.

<a id="FAQ-41-unity-offers-a-component-called-stable-diffusion-generator-should-i-add-it"></a>

### 41. Unity offers a component called Stable Diffusion Generator. Should I add it?

No. That is the base class. It has no prompt and no **Generate** button. Add one of the four concrete components.

<a id="FAQ-image-to-image"></a>

## Image-to-image

<a id="FAQ-42-which-texture-is-uploaded-for-ui-image-to-image"></a>

### 42. Which texture is uploaded for UI image-to-image?

The **Input Texture** field on `StableDiffusionImage2Image`, not the sprite currently shown on the `Image`. The demo `Image-2-RawImage` points at `Assets/Stable-Diffusion-Unity-Integration/Input Image/horse_input.png`.

<a id="FAQ-43-why-must-the-texture-be-readable"></a>

### 43. Why must the texture be readable?

The code calls `EncodeToPNG()`. Unity only allows that when Read/Write is enabled. Otherwise UI image-to-image logs `Input Image {name} isn't readable. Go to texture import settings and tick the Read/Write box` and does not send a request. The inspector also shows `Input textures should be uncompressed and readable`.

<a id="FAQ-44-what-denoising-strength-is-used"></a>

### 44. What denoising strength is used?

UI image-to-image always sends **0.75**. There is no slider. Image-to-material sends its **Denoising strength** slider, default **0.65**. Text-to-image sends the class default 0, and WebUI ignores denoising for a normal text-to-image call.

<a id="FAQ-45-can-i-inpaint-or-pass-a-mask"></a>

### 45. Can I inpaint or pass a mask?

No. The mask field is commented out of the request class because the author saw HTTP 500 when it was included. There is no mask slot in the inspector.

<a id="FAQ-46-the-horse-image-is-the-init-image-will-generate-modify-that-png"></a>

### 46. The horse image is the init image. Will Generate modify that PNG?

No. The file is encoded into the request. The result is written to a different file, `SDImages/{guid}.png`.

<a id="FAQ-materials-tiling-and-normal-maps"></a>

## Materials, tiling, and normal maps

<a id="FAQ-47-which-objects-in-the-demo-are-materials"></a>

### 47. Which objects in the demo are materials?

`Floor`, `Ceiling`, `Walls`, `Table`, `Cube`, `Cylinder`, `Painting1`, and `Painting2` use `StableDiffusionText2Material`. Prompts are listed in the [user guide](#user-guide-demo-scene-contents).

<a id="FAQ-48-what-is-the-difference-between-tiling-and-tiling-x-tiling-y"></a>

### 48. What is the difference between Tiling and Tiling X / Tiling Y?

**Tiling** is sent to WebUI as `tiling` so the sampler can attempt a seamless image. Only text-to-material forwards the checkbox. The other generators send false. **Tiling X** and **Tiling Y** are Unity `mainTextureScale`, and the code sets the scale to **negative** X and Y, which mirrors the texture. They do not change the WebUI request.

<a id="FAQ-49-why-did-generating-the-cube-change-the-cylinder"></a>

### 49. Why did generating the cube change the cylinder?

`Cylinder` is a child of `Cube`. `Cube`'s **Apply Recursively** is on, so the new shared material is copied onto child mesh renderers. `Cylinder` still has its own component and prompt, but its renderer now points at the cube's material. Turn **Apply Recursively** off on the parent if you do not want that.

<a id="FAQ-50-does-apply-recursively-search-inactive-children"></a>

### 50. Does Apply Recursively search inactive children?

No. Inactive children are skipped. If the host object has no `MeshRenderer` and the flag is off, generation still requests an image and then `LoadIntoMaterial` returns without assigning anything.

<a id="FAQ-51-where-is-the-normal-map-file"></a>

### 51. Where is the normal map file?

Nowhere. The normal map is an in-memory texture assigned to `_BumpMap`. Only the colour image is written to `{guid}.png`. **Normal Map Strength** changes `_BumpScale` and does not rebuild the map. The rebuild function is called with a fixed 0.5 (text-to-material) or 20 (image-to-material), not with the slider. After a domain reload the in-memory normal map is gone until you generate again and save the scene if Unity kept the embedded texture.

<a id="FAQ-52-how-do-metallic-and-smoothness-get-applied"></a>

### 52. How do metallic and smoothness get applied?

They set `_Metallic` and `_Glossiness` on the current shared material, including when you move the sliders without generating. URP Lit uses `_Smoothness`, which this code does not set. Image-to-material always uses the Standard shader even if **Use Universal Render Pipeline** is enabled.

<a id="FAQ-53-how-do-i-enable-urp"></a>

### 53. How do I enable URP?

Install URP using Unity's package docs, switch the project to that pipeline, and enable **Use Universal Render Pipeline** on the settings asset. Only `StableDiffusionText2Material` reads the flag, and it looks up the shader `Universal Render Pipeline/Lit`. If the shader is missing you get `Shader setup wrong: Please check if you're project uses 'Standard' or 'Universal Render Pipeline'`, and the material will be pink or absent. HDRP is not implemented.

<a id="FAQ-54-will-this-replace-a-material-asset-i-already-assigned"></a>

### 54. Will this replace a material asset I already assigned?

Yes. On success the code does `sharedMaterial = new Material(...)`. The previous material asset on disk is not deleted, but the renderer no longer points at it. No new `.mat` file is created. Child renderers are repointed when **Apply Recursively** is on. This is not undoable through this project.

<a id="FAQ-size-seed-and-batches"></a>

## Size, seed, and batches

<a id="FAQ-55-why-cant-i-set-width-to-64-or-4096"></a>

### 55. Why can't I set width to 64 or 4096?

In the Editor, `Update` clamps width and height to the inclusive range 128–2048. The clamp does not run in a player, and this project does not support player generation anyway.

<a id="FAQ-56-does-the-size-have-to-be-a-multiple-of-8"></a>

### 56. Does the size have to be a multiple of 8?

This project does not check. Many WebUI builds reject other sizes. If the response body says so, change **Width** and **Height**. That rule belongs to WebUI, and it can change between WebUI versions.

<a id="FAQ-57-what-does-seed-1-do"></a>

### 57. What does seed -1 do?

It is sent as `-1`. WebUI treats that as "pick a seed". The seed it picked is copied into **Generated Seed** when the response `info` string parses. **Generated Seed** is not copied back into **Seed**. Leave **Seed** at `-1` and every click is a new draw. Paste **Generated Seed** into **Seed** when you want another request with that number.

<a id="FAQ-58-can-i-generate-a-grid-or-four-images-at-once"></a>

### 58. Can I generate a grid or four images at once?

No. `batch_size` and `n_iter` are hardcoded to 1. Only `images[0]` is saved. Extra images in the response are ignored.

<a id="FAQ-59-where-are-high-res-fix-face-restore-and-styles"></a>

### 59. Where are high-res fix, face restore, and styles?

They are not in the inspector. The JSON leaves `enable_hr` false, `restore_faces` false, and `styles` as a single empty string. Turning those on inside WebUI's own browser UI does not add controls to Unity.

<a id="FAQ-files-save-and-undo"></a>

## Files, save, and undo

<a id="FAQ-60-where-is-the-png"></a>

### 60. Where is the PNG?

UI components write `{output}/SDImages/{guid}.png`. Material components write `{output}/SDMaterials/{guid}.png`. With the default settings, and without a pre-existing `Assets/streamingAssets` folder, `{output}` is `Assets/StreamingAssets`. The **Guid** field on the component is the file name. Success does not log the full path.

<a id="FAQ-61-what-is-the-difference-between-streamingassets-and-streamingassets"></a>

### 61. What is the difference between streamingAssets and StreamingAssets?

**Output Folder** defaults to `/streamingAssets`, which is concatenated onto the `Assets` path. If that exact directory does not exist, the code uses Unity's `Application.streamingAssetsPath`, the `Assets/StreamingAssets` folder, and creates it. The lowercase folder is used only when it already exists.

<a id="FAQ-62-why-doesnt-git-show-my-new-images"></a>

### 62. Why doesn't git show my new images?

`.gitignore` ignores `Assets/StreamingAssets*`. The files are still on disk. `Assets/streamingAssets` is not matched by that pattern.

<a id="FAQ-63-can-i-undo-generate"></a>

### 63. Can I undo Generate?

Not with this tool. It never calls Unity's Undo API. The previous PNG is deleted at the start of the run, before the server responds. Copy the file first if you want to keep it. Duplicating the GameObject and letting it assign a new guid is how you keep a second result instead of overwriting the first.

<a id="FAQ-64-i-generated-during-play-mode-and-the-object-reverted"></a>

### 64. I generated during Play Mode and the object reverted.

Unity discards scene edits when Play Mode ends. The PNG stays. Generate in Edit Mode when the scene should keep the new sprite or material. The helper `InspectorRefresh.ForceInspectorToRefresh` is never called, so leaving Play Mode does not reassign the new file for you.

<a id="FAQ-65-the-scene-shows-the-picture-but-i-cannot-find-a-material-asset"></a>

### 65. The scene shows the picture, but I cannot find a material asset.

That is expected. The material and the UI sprite are created in memory. Save the scene if you want Unity to embed those references. The durable file you can reimport later is the PNG. Reassign it by hand if a reload loses the in-scene texture.

<a id="FAQ-66-generate-failed-and-the-old-png-is-gone"></a>

### 66. Generate failed and the old PNG is gone.

`SetupFolders` deletes `{guid}.png` before the HTTP call. A bad model index, a connection error, or an empty `images` array does not restore it. There is no recycle bin inside the project. Restore the picture from your own copy or from version control if it was committed.

<a id="FAQ-progress-hangs-and-errors"></a>

## Progress, hangs, and errors

<a id="FAQ-67-what-does-the-progress-bar-mean"></a>

### 67. What does the progress bar mean?

The title is `Generation in progress`. About twice a second the Editor polls `/sdapi/v1/progress` and shows `progress * 100`. If that GET fails, the non-auth path shows `Progress unknown...` and the image request continues. The bar is not a cancel button. There is no timeout.

<a id="FAQ-68-the-editor-froze-right-after-i-clicked-generate"></a>

### 68. The Editor froze right after I clicked Generate.

If the UnityWebRequest options call fails, setting the model falls through to a blocking `HttpWebRequest`. A host that never answers can stall the Editor until the operating system gives up. Fix the options URL so the first request succeeds.

<a id="FAQ-69-the-bar-is-stuck-after-an-exception"></a>

### 69. The bar is stuck after an exception.

Enter Play Mode in a scene that has an active text-to-material or image-to-material component. Their `Start` method clears the bar. Then stop Play Mode. Reloading the scene clears a stuck `generating` flag because that flag is not saved.

<a id="FAQ-70-what-does-no-image-was-return-by-the-server-mean"></a>

### 70. What does "No image was return by the server" mean?

That exact wording, including the grammar, comes from `StableDiffusionImage2Material` when the response has no `images` array. The other generators say `No image was returned by the server. Verify that the server is correctly setup.` The HTTP call itself succeeded. Read the following `Full response:` line and then WebUI's log. This project will not retry a different endpoint for that case.

<a id="FAQ-71-the-console-says-the-server-must-be-on-port-7860-but-my-browser-can-open-the-api"></a>

### 71. The Console says the server must be on port 7860, but my browser can open the API.

Use the URL in `Requesting models from:`. A trailing slash, the wrong settings asset, or a second configuration object will call a different address than the one you opened in the browser.

<a id="FAQ-demo-scene"></a>

## Demo scene

<a id="FAQ-72-i-opened-the-demo-and-generate-on-the-button-throws"></a>

### 72. I opened the demo and Generate on the Button throws.

`Button` in `DemoScene` stores model index 92. The saved **Model Names** list on that scene has 17 entries. Click **List Models** against your server and pick a model again. Do this for every generator. Several indexes in `DemoScene-NoPostProcessing` are also past the end of its 3-name list.

<a id="FAQ-73-are-the-model-names-saved-in-the-demo-something-i-should-download"></a>

### 73. Are the model names saved in the demo something I should download?

No. They are a snapshot of one developer's WebUI at save time. They are not distributed with this repository, and they will not match your install. Treat them as stale data.

<a id="FAQ-74-what-is-image-2-rawimage"></a>

### 74. What is Image-2-RawImage?

A `RawImage` with `StableDiffusionImage2Image`, prompt `a horse in a field, photorealistic, highly detailed, stock image`, and **Input Texture** set to `horse_input.png`.

<a id="FAQ-platforms-builds-and-the-json-plugin"></a>

## Platforms, builds, and the JSON plugin

<a id="FAQ-75-the-project-does-not-compile-findanyobjectbytype-is-missing"></a>

### 75. The project does not compile: FindAnyObjectByType is missing.

Check the patch version, not only the year. Unity staff listed `FindAnyObjectByType` from 2020.3.45f1, 2021.3.18f1, 2022.2.5f1, and 2023.1.0a20 onward. This repository's project version is 2021.3.33f1, which is past that 2021.3 backport, so do not tell a 2021.3.33 user to move to 2023.1 for this error alone. Unity 2019, and 2021.3 builds older than 18f1, do fail here. There is no `#if` fallback. `UnityWebRequest.Result` is a separate requirement from Unity 2020.2.

<a id="FAQ-76-the-project-does-not-compile-newtonsoft-could-not-be-found"></a>

### 76. The project does not compile: Newtonsoft could not be found.

The DLL should be at `Assets/Plugins/Newtonsoft.Json.dll`. Its importer meta currently has the Editor platform entry disabled (`enabled: 0`) while Any Platform is enabled. Select the DLL and enable **Editor**. If the file is missing, restore it from the repository. The scripts do not compile without `Newtonsoft.Json`.

<a id="FAQ-77-why-did-my-player-build-fail-on-unityeditor"></a>

### 77. Why did my player build fail on UnityEditor?

`StableDiffusionGenerator.cs` and `ReadOnlyAttribute.cs` import `UnityEditor` and are not in an Editor folder. That is a property of this tree, not a Player Settings switch. Keep this tool in the Editor.

<a id="FAQ-78-does-the-os-change-the-output-path"></a>

### 78. Does the OS change the output path?

The code uses `Application.dataPath`, `Application.streamingAssetsPath`, and `Path.Combine` for the subfolder. Those Unity properties are correct on Windows, macOS, and Linux. The **Output Folder** value is still a raw string concat, so a missing leading slash is wrong on every OS. Paths were read from the code and were not re-run on each OS for this documentation.

<a id="FAQ-licence-tests-and-support"></a>

## Licence, tests, and support

<a id="FAQ-79-is-the-licence-lgpl-or-mit"></a>

### 79. Is the licence LGPL or MIT?

The `LICENSE` file in the repository root is the MIT License, copyright 2025 Dominic Branchaud. The README section titled Licence still describes LGPL v2.1. They disagree. These docs are not legal advice. Model weights and WebUI have their own terms, which this repository does not grant.

<a id="FAQ-80-are-there-automated-tests"></a>

### 80. Are there automated tests?

No. The Test Framework package is in the manifest and there is no Tests folder. Nobody has a green test run to cite.

<a id="FAQ-81-who-do-i-contact-and-what-should-i-send"></a>

### 81. Who do I contact, and what should I send?

Open https://github.com/dobrado76/Stable-Diffusion-Unity-Integration/issues with your Unity version, the component name, the logged request URL, the Console error, and what you expected. Strip passwords, private prompts, and home-directory paths. Do not send the whole project `Library` folder. There is no support desk and no promised reply. The Buy Me a Coffee link in the README is not a ticket system. WebUI faults, once the URL in the log is correct, belong on the WebUI issue tracker.

<a id="FAQ-82-is-a-feature-that-i-see-in-webuis-browser-planned-here"></a>

### 82. Is a feature that I see in WebUI's browser "planned" here?

No plan is published. If the inspector does not show the control, this project does not do that thing. See [Roadmap](#roadmap).

<a id="FAQ-83-can-these-components-live-on-a-prefab"></a>

### 83. Can these components live on a prefab?

The prompt, guid, sampler index, and model index are ordinary serialized fields, so a prefab asset can store them. Generating still deletes the PNG that belongs to that guid. Instances that share one guid share one output file. How Unity records a generation as a prefab override was not tested when these notes were written. Save the scene or apply the override the way you would for any other Inspector edit you want to keep.

<a id="FAQ-84-why-does-generate-fail-when-the-object-is-inactive"></a>

### 84. Why does Generate fail when the object is inactive?

`Generate()` calls `StartCoroutine`. Unity does not start that coroutine when the GameObject is inactive or the component is disabled, so the click never reaches the server and `generating` stays false. Activate the generator, enable the component, and activate `StableDiffusionConfiguration`, then click **Generate** again. Text-to-image, UI image-to-image, and text-to-material then log `Sending request to:`. Image-to-material does not log that line. Disabling the object after the request has started is different: the coroutine stops, `generating` stays true, and the HTTP call is not aborted.

<a id="FAQ-85-why-is-the-ui-element-still-the-old-size"></a>

### 85. Why is the UI element still the old size?

**Width** and **Height** are the pixel size sent to WebUI and stored in the PNG. The code does not change the RectTransform. It only replaces the sprite or the RawImage texture. The new sprite uses pivot `(0, 0)`, so an `Image` can sit differently inside the same rectangle. Resize the RectTransform yourself if you want the widget to match 512 by 512 or whatever you generated.

<a id="FAQ-86-why-did-the-editor-hitch-reload-scripts-or-move-the-scene-camera"></a>

### 86. Why did the Editor hitch, reload scripts, or move the Scene camera?

On a successful edit-mode assignment, UI and material generators call `EditorUtility.RequestScriptReload()` and `EditorSceneManager.MarkAllScenesDirty()`. Material generators also call `SceneView.FrameLastActiveSceneView()`, which frames the Scene view and can move the camera. UI generators do not move the camera. Every loaded scene is marked dirty, not only the one you generated in. The reload can make an in-memory sprite or material look missing afterward. The PNG under the guid file name is the copy to assign again. This reload behaviour was read from the source and not re-tested by watching the Editor.

<a id="FAQ-87-the-picture-appeared-and-the-console-still-says-error-processing-response"></a>

### 87. The picture appeared and the Console still says Error processing response.

The PNG is written before `info` is parsed. If `info` is null, `info != ""` throws, and the catch logs `Error processing response:` plus the body. **Generated Seed** is left unchanged. The file is not rolled back. Copy the seed from WebUI's own result if you need it, or set **Seed** yourself for the next run.

<a id="roadmap"></a>

<!-- source: docs/roadmap.md -->

<a id="roadmap-roadmap"></a>

# Roadmap

There is no published roadmap, milestone list, or `TODO`/`FIXME` marker in the scripts. Nothing in this file is a commitment.

<a id="roadmap-implemented"></a>

## Implemented

- Text-to-image and image-to-image for UI `Image` and `RawImage`, with Inspector **Generate** buttons.
- Text-to-material with WebUI tiling, Unity-side repeat, metallic, smoothness, optional normal map, and apply-to-children.
- Image-to-material generation code without an Inspector button.
- Server URL, API path overrides, HTTP Basic fields, output-folder suffix, and a URP shader toggle used by text-to-material only.
- Model list fetch with one alternate URL, and a checkpoint POST that falls back to a second HTTP stack and a second URL.
- Editor progress bar.
- Demo scenes and a default settings asset aimed at `http://127.0.0.1:7860`.
- README changelog through 1.3.0.

<a id="roadmap-known-limitations-of-the-current-code"></a>

## Known limitations of the current code

These are gaps in what exists, not scheduled fixes.

- Unity 2019 does not compile these scripts (`UnityWebRequest.Result`, `FindAnyObjectByType`). The checked-in editor is 2021.3.33f1, which is newer than the 2021.3.18f1 backport of `FindAnyObjectByType`. Patches older than the backports named in the getting-started chapter are still expected to fail.
- Two scripts reference `UnityEditor` outside an Editor folder, which blocks player builds.
- `StableDiffusionImage2Material` has no custom inspector, sends no auth header, and always builds a Standard material.
- Model indexes are not updated when the model list is replaced. Demo scenes contain out-of-range indexes.
- The checkpoint payload sends `model_name`, not the WebUI title.
- `sampler_index` remains `"Euler"` while `sampler_name` changes.
- `SDSettings.sampler` is unused.
- Text-to-image and UI image-to-image do not copy `SDSettings` defaults, because their width starts at 512.
- The previous PNG is deleted before the request succeeds.
- Normal maps and materials are not saved as their own assets.
- URP support does not set `_Smoothness` and does not cover image-to-material or HDRP.
- Texture repeat is applied with a negative scale.
- No cancel, no timeout, no undo registration, no batch, no inpainting, no hi-res fix, no face restore.
- Progress-bar refresh on leaving Play Mode is not wired up.
- `Newtonsoft.Json.dll` importer meta disables the Editor platform entry.
- README licence text (LGPL) does not match the MIT `LICENSE` file.
- README scene path and older component names are stale.
- No automated tests and no CI.

<a id="roadmap-not-planned-in-this-repository"></a>

## Not planned in this repository

No statement in the README, commits messages reviewed for the changelog, or source comments schedules HDRP, a runtime API, ControlNet, LoRA controls, a cancel button, or a packaged asmdef. Do not tell users those features are coming. Someone can still propose them in a pull request; the contributing notes in the README are the process, not a delivery date.
