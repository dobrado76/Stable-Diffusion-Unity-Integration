# Question and symptom index

Use this index to jump to the chapter that answers a support question. In the repository, the links open the separate files under `docs/`. In `AI-SUPPORT.md` the same links are anchors inside the one file.

## Start here

| Question | Chapter |
|---|---|
| What this tool is and what it refuses to do | [Product](product.md) |
| Install Unity content, start the API, generate once | [Getting started](getting-started.md) |
| Day-to-day Inspector workflow | [User guide](user-guide.md) |
| Settings asset, output paths, defaults | [Configuration](configuration.md) |
| Exact JSON, error strings, limits | [Reference](reference.md) |
| Something failed | [Troubleshooting](troubleshooting.md) |
| How the scripts call the server | [Architecture](architecture.md) |
| Compile, build, tests, docs commands | [Development](development.md) |
| Short answers | [FAQ](FAQ.md) |
| Implemented versus not scheduled | [Roadmap](roadmap.md) |

## Symptoms

| What you see | Go to |
|---|---|
| **Generate** does nothing | [Generate does nothing](troubleshooting.md#generate-does-nothing) |
| Popup exception on selecting a generator | [Model popup throws](troubleshooting.md#model-popup-throws-when-you-select-the-object) |
| **List Models** cannot reach the server | [List Models fails](troubleshooting.md#list-models-fails-immediately) |
| Wrong model, or `IndexOutOfRangeException` | [Out-of-range model](troubleshooting.md#wrong-checkpoint-or-an-out-of-range-model) |
| HTTP 401, 404, 405, 422, or 500 | [HTTP errors during Generate](troubleshooting.md#http-errors-during-generate) |
| `No image was returned by the server` | [No image in the response](troubleshooting.md#no-image-in-the-response) |
| Progress bar stuck or `Progress unknown...` | [Progress bar](troubleshooting.md#progress-bar-stuck-or-progress-unknown) |
| Texture is not readable | [Input image errors](troubleshooting.md#input-image-errors) |
| Pink material, flipped texture, children changed | [Material looks wrong](troubleshooting.md#material-looks-wrong) |
| PNG missing or deleted | [The PNG is missing](troubleshooting.md#the-png-is-missing) |
| Browser auth works, Unity gets 401 | [Authentication mismatch](troubleshooting.md#authentication-works-in-the-browser-and-fails-in-unity) |
| Editor freeze after Generate | [Editor freeze](troubleshooting.md#editor-freeze-while-setting-the-model) |
| Script compile error | [Compile errors](troubleshooting.md#compile-errors) |
| Missing script on the demo volume | [Demo scene missing script](troubleshooting.md#demo-scene-missing-script) |
| Play Mode result disappeared | [Play Mode reverted](troubleshooting.md#play-mode-looked-successful-and-then-reverted) |
| Empty prompt, seeds, batches, URP, files | [FAQ](FAQ.md) |

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
