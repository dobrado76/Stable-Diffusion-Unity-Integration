# Development

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

## Open and compile

Open the repository root in the editor recorded by `ProjectVersion.txt`, **2021.3.33f1**, or another editor that contains `Object.FindAnyObjectByType`. Unity staff listed that method from 2020.3.45f1, 2021.3.18f1, 2022.2.5f1, and 2023.1.0a20 onward (https://discussions.unity.com/t/findobjectsbytype-and-its-siblings/910153/1). 2021.3.33f1 is past 2021.3.18f1. Unity 2019 is not. This documentation pass did not click Play or run a build.

Package dependencies in `Packages/manifest.json` are the stock modules of that 2021 project plus IDE integrations, uGUI, TextMeshPro, Timeline, Visual Scripting, and the Test Framework. None of those packages is called by the generator scripts except uGUI types `Image` and `RawImage`.

## Tests

There are no `EditMode` or `PlayMode` tests. `com.unity.test-framework` is installed and unused. Do not claim a test run for this project. A useful future test would mock HTTP; nothing in the tree does that today.

## Player builds

Do not expect a player build to succeed while these scripts are in `Assets`.

- `StableDiffusionGenerator.cs` has a top-level `using UnityEditor`.
- `ReadOnlyAttribute.cs` has a top-level `using UnityEditor`.

Both files are outside `Scripts/Editor`. The generator methods that draw the progress bar are wrapped in `#if UNITY_EDITOR`, but the using directive is not. Unity does not reference `UnityEditor` when compiling player scripts, so the player script compile fails. That failure was inferred from the source layout and was not reproduced by producing a player.

The README's statement that a game built with this tool need not be open source is about the licence, not about these scripts compiling into a player. The intended use is generation in the Editor, then ordinary materials and sprites in the game, without calling WebUI at runtime.

## Extending

There is no plugin API, no events, and no virtual `Generate` hook beyond the public methods already on the components. `Generate()` is public, so another Editor script can call it. `StableDiffusionImage2Material.Generate` is the only way to start that component, because it has no button.

If you add a user-visible field, button, error string, or request field, update the matching document under `docs/` and rebuild `AI-SUPPORT.md` in the same change. The project rule for that lives in `.cursor/rules/documentation-maintenance.mdc`.

Do not document WebUI features as if this project exposed them. Add a real Inspector control and a real JSON assignment before the user guide mentions them.

Private request classes in `SDSettings.cs` are the payload. `sampler_index` staying at `"Euler"` is easy to miss if you add samplers and only set `sampler_name`.

## Package

`Stable-Diffusion-Unity-Integration.unitypackage` is checked in at the repository root. It was not extracted while writing these docs. After you change scripts, rebuild the package from the Unity **Export Package** menu if you still ship that file, or the package and the scripts will diverge. The docs describe the scripts.

## Documentation commands

From the repository root, with Python 3:

```text
python tools/build_ai_support.py
python tools/build_ai_support.py --check
```

The first command rewrites `AI-SUPPORT.md`. The check command compares a fresh build with the committed file and does not write it. It also checks that the root `README.md` links to `AI-SUPPORT.md` and contains the AI-help heading. The repository has no CI workflow, so nothing runs this check automatically. Adding a new CI system was intentionally skipped.

## Contributing

The README asks contributors to clone the repository, keep a pull request to one change, avoid formatting-only diffs, and include reproduction steps for bug fixes. Pull requests go through GitHub on https://github.com/dobrado76/Stable-Diffusion-Unity-Integration . Issues go to https://github.com/dobrado76/Stable-Diffusion-Unity-Integration/issues .

There is no supported release branch, no tag, and no response-time commitment. The Buy Me a Coffee link in the README (https://www.buymeacoffee.com/dobrado76) is an appreciation link, not a support channel.

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
