# Documentation

These files are the support manual for Stable Diffusion Unity Integration. The root [README](../README.md) is the short project page. The file [AI-SUPPORT.md](../AI-SUPPORT.md) at the repository root is the same manual concatenated into one document for an AI assistant.

## Read in this order

1. [Product](product.md) — what the tool is and what it does not do.
2. [Getting started](getting-started.md) — WebUI API, Unity project, first **Generate**.
3. [User guide](user-guide.md) — each component, the demo scene, save behaviour.
4. [Configuration](configuration.md) — `SDSettings`, paths, auth, fields that do nothing.
5. [Troubleshooting](troubleshooting.md) — symptoms, then [FAQ](FAQ.md).

Use [Reference](reference.md) when you need an exact Console string, JSON field, or limit. Use [Architecture](architecture.md) and [Development](development.md) when the question is about how the scripts are put together, player builds, or rebuilding the concatenated manual. [Roadmap](roadmap.md) separates current behaviour from things that are not scheduled.

[Instructions for AI assistants](ai-instructions.md) and the [question and symptom index](support-index.md) are written for the concatenated file. [Support evaluation](support-evaluation.md) is a maintainer checklist and is not copied into `AI-SUPPORT.md`.

## Rebuild the concatenated manual

From the repository root:

```text
python tools/build_ai_support.py
python tools/build_ai_support.py --check
```

The check compares output to the committed `AI-SUPPORT.md` and does not write it. Edit the chapter files, then run the build. Do not hand-edit `AI-SUPPORT.md`.

WebUI's own manual stays on https://github.com/AUTOMATIC1111/stable-diffusion-webui . These chapters only describe how this Unity project calls it.
