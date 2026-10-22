# Product

## What this project is

Stable Diffusion Unity Integration is a Unity Editor tool that sends image-generation requests to an [AUTOMATIC1111 Stable Diffusion WebUI](https://github.com/AUTOMATIC1111/stable-diffusion-webui) HTTP API and applies the returned image inside the open Unity scene.

It is a design-time helper for level and UI mock-up work. It does not embed a Stable Diffusion runtime, and it does not ship any diffusion models.

The user-facing release described by the repository README changelog is **1.3.0** (2025-03-30). `ProjectSettings/ProjectSettings.asset` still records `bundleVersion: 1.2.1`. The repository has no git tags. Treat the README changelog as the feature history and the Unity bundle version as a stale Player Settings field.

Repository: https://github.com/dobrado76/Stable-Diffusion-Unity-Integration

## Who it is for

- A Unity developer who already runs, or can reach, an AUTOMATIC1111 WebUI with the API switched on.
- Someone texturing Built-in Render Pipeline meshes, or UI `Image` and `RawImage` objects, from the Inspector.
- Someone who wants the generated PNG kept in the Unity project.

It is not a general Stable Diffusion application, a model manager, or a runtime in-game generation framework.

## Primary workflows

1. Start WebUI with `--api`. Optionally protect it with `--api-auth username:password`.
2. Open a Unity scene that contains an active `StableDiffusionConfiguration`.
3. Assign an `SDSettings` asset. The repository includes `Assets/Stable-Diffusion-Unity-Integration/Settings/DefaultLocalSDSettings.asset`, pointed at `http://127.0.0.1:7860`.
4. Click **List Models** and choose a model on the generator component.
5. Click **Generate** on one of the generator components that has a custom inspector.
6. The tool writes a PNG, then assigns the pixels to a UI graphic or a mesh material.

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

## Design boundaries

- WebUI performs sampling, model loading, and image decoding on the server. This project builds the JSON request, waits for the response, and files the first returned image.
- One active configuration object is assumed. Generators call `FindAnyObjectByType<StableDiffusionConfiguration>()` and use whichever instance that returns.
- Only the first returned image is kept. Batch size and batch count are hardcoded to 1.
- The tool is operated from the Inspector. There is no game view, no runtime HUD, and no cancel button.
- Custom editors live under `Assets/Stable-Diffusion-Unity-Integration/Scripts/Editor/`. The generator scripts themselves live outside an `Editor` folder, so they are part of the player script compile. See [Player builds](development.md#player-builds).

## What it does not do

- It does not install, launch, or configure WebUI for you.
- It does not download checkpoints, LoRAs, VAEs, embeddings, or ControlNet models.
- It does not expose high-resolution fix, face restore, inpainting masks, styles, scripts, upscalers, or batch grids in the Inspector.
- It does not query the server for the sampler list. The sampler names are a serialized string array you edit by hand.
- It does not save a `.mat` asset or a normal-map PNG. It writes one colour PNG per component.
- It does not implement HDRP. URP is a shader-name switch on text-to-material only.
- It has no Unity Test Framework tests and no CI workflow.

## Licence texts in the repository

The file `LICENSE` at the repository root is the MIT License, copyright 2025 Dominic Branchaud. The README section titled "Licence" still describes LGPL v2.1 and links to an older revision of that file. Those two texts do not match. This documentation is not legal advice. Read `LICENSE` for the grant that is actually in the tree. Neither text grants rights to third-party models or to WebUI.
