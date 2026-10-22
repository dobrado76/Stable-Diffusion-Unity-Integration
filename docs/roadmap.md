# Roadmap

There is no published roadmap, milestone list, or `TODO`/`FIXME` marker in the scripts. Nothing in this file is a commitment.

## Implemented

- Text-to-image and image-to-image for UI `Image` and `RawImage`, with Inspector **Generate** buttons.
- Text-to-material with WebUI tiling, Unity-side repeat, metallic, smoothness, optional normal map, and apply-to-children.
- Image-to-material generation code without an Inspector button.
- Server URL, API path overrides, HTTP Basic fields, output-folder suffix, and a URP shader toggle used by text-to-material only.
- Model list fetch with one alternate URL, and a checkpoint POST that falls back to a second HTTP stack and a second URL.
- Editor progress bar.
- Demo scenes and a default settings asset aimed at `http://127.0.0.1:7860`.
- README changelog through 1.3.0.

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

## Not planned in this repository

No statement in the README, commits messages reviewed for the changelog, or source comments schedules HDRP, a runtime API, ControlNet, LoRA controls, a cancel button, or a packaged asmdef. Do not tell users those features are coming. Someone can still propose them in a pull request; the contributing notes in the README are the process, not a delivery date.
