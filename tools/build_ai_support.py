#!/usr/bin/env python3
"""Build or check the root AI-SUPPORT.md bundle.

    python tools/build_ai_support.py
    python tools/build_ai_support.py --check

--check compares a fresh bundle with the committed file and does not write it.
"""

from __future__ import annotations

import re
import sys
from pathlib import Path

ROOT = Path(__file__).resolve().parents[1]
OUTPUT = ROOT / "AI-SUPPORT.md"

# Order is the bundled document order. Maintainer-only files stay out.
SOURCES = [
    "docs/support-index.md",
    "docs/ai-instructions.md",
    "docs/product.md",
    "docs/getting-started.md",
    "docs/user-guide.md",
    "docs/configuration.md",
    "docs/reference.md",
    "docs/troubleshooting.md",
    "docs/architecture.md",
    "docs/development.md",
    "docs/FAQ.md",
    "docs/roadmap.md",
]

README_HEADING = "Need help? Ask an AI assistant"
README_LINK = "](AI-SUPPORT.md)"

HEADING_RE = re.compile(r"^(#{1,6})\s+(.+?)\s*$", re.MULTILINE)
LINK_RE = re.compile(r"\[([^\]]*)\]\(([^)]+)\)")
FENCE_RE = re.compile(r"^```.*?^```", re.MULTILINE | re.DOTALL)


def slug(text: str) -> str:
    text = text.strip().lower()
    text = re.sub(r"<[^>]+>", "", text)
    text = text.replace("`", "")
    text = re.sub(r"[^\w\s-]", "", text, flags=re.UNICODE)
    text = re.sub(r"\s+", "-", text)
    text = re.sub(r"-{2,}", "-", text)
    return text.strip("-")


def split_fences(text: str) -> list[tuple[bool, str]]:
    parts: list[tuple[bool, str]] = []
    last = 0
    for match in FENCE_RE.finditer(text):
        if match.start() > last:
            parts.append((False, text[last : match.start()]))
        parts.append((True, match.group(0)))
        last = match.end()
    if last < len(text):
        parts.append((False, text[last:]))
    return parts or [(False, text)]


def heading_ids(path: Path) -> dict[str, str]:
    """Map a heading slug to the anchor id used in the bundle."""
    stem = path.stem
    text = path.read_text(encoding="utf-8")
    prose = "".join(chunk for is_code, chunk in split_fences(text) if not is_code)
    counts: dict[str, int] = {}
    found: dict[str, str] = {}
    for match in HEADING_RE.finditer(prose):
        base = slug(match.group(2))
        if not base:
            continue
        n = counts.get(base, 0)
        counts[base] = n + 1
        heading_slug = base if n == 0 else f"{base}-{n}"
        found[heading_slug] = f"{stem}-{heading_slug}"
    return found


def anchor_headings(text: str, stem: str) -> str:
    counts: dict[str, int] = {}

    def repl(match: re.Match[str]) -> str:
        base = slug(match.group(2))
        if not base:
            return match.group(0)
        n = counts.get(base, 0)
        counts[base] = n + 1
        heading_slug = base if n == 0 else f"{base}-{n}"
        anchor = f"{stem}-{heading_slug}"
        return f'<a id="{anchor}"></a>\n\n{match.group(0)}'

    out = []
    for is_code, chunk in split_fences(text):
        if is_code:
            out.append(chunk)
        else:
            out.append(HEADING_RE.sub(repl, chunk))
    return "".join(out)


def rewrite_links(text: str, ids_by_file: dict[str, dict[str, str]]) -> tuple[str, list[str]]:
    errors: list[str] = []

    def repl(match: re.Match[str]) -> str:
        label, target = match.group(1), match.group(2).strip()
        if target.startswith(("http://", "https://", "mailto:")):
            return match.group(0)
        if target.startswith("#"):
            errors.append(f"bare anchor link is not rewritten: {target}")
            return match.group(0)
        path_part, _, frag = target.partition("#")
        path_part = path_part.replace("\\", "/")
        if path_part.startswith("./"):
            path_part = path_part[2:]
        name = Path(path_part).name
        stem = Path(name).stem
        if name not in ids_by_file and f"docs/{name}" not in {f"docs/{Path(p).name}" for p in SOURCES}:
            # Allow links whose file is one of the bundled sources.
            pass
        bundled = {Path(p).name: Path(p).stem for p in SOURCES}
        if name not in bundled:
            errors.append(f"link target is not a bundled chapter: {target}")
            return match.group(0)
        if not frag:
            return f"[{label}](#{bundled[name]})"
        anchor = ids_by_file.get(name, {}).get(frag)
        if anchor is None:
            errors.append(f"unknown heading '{frag}' in {name}")
            return f"[{label}](#{bundled[name]})"
        return f"[{label}](#{anchor})"

    out = []
    for is_code, chunk in split_fences(text):
        if is_code:
            out.append(chunk)
        else:
            out.append(LINK_RE.sub(repl, chunk))
    return "".join(out), errors


def bundle_sources() -> tuple[str, list[str]]:
    errors: list[str] = []
    missing = [rel for rel in SOURCES if not (ROOT / rel).is_file()]
    if missing:
        return "", [f"missing source: {rel}" for rel in missing]

    ids_by_file = {Path(rel).name: heading_ids(ROOT / rel) for rel in SOURCES}
    chunks: list[str] = []
    for rel in SOURCES:
        path = ROOT / rel
        stem = path.stem
        body, link_errors = rewrite_links(path.read_text(encoding="utf-8"), ids_by_file)
        for err in link_errors:
            errors.append(f"{rel}: {err}")
        body = anchor_headings(body, stem)
        if not body.endswith("\n"):
            body += "\n"
        chunks.append(
            f'<a id="{stem}"></a>\n\n'
            f"<!-- source: {rel} -->\n\n"
            f"{body}"
        )
    return "\n".join(chunks).rstrip() + "\n", errors


def table_of_contents(body: str) -> str:
    lines = ["## Contents", ""]
    for match in HEADING_RE.finditer(body):
        level = len(match.group(1))
        title = match.group(2).strip()
        # The anchor is the explicit <a id> injected on the previous lines.
        # Recover it from the stem-heading scheme by looking backward.
        lines.append(f"{'  ' * (level - 1)}- [{title}](#{slug_for_injected(body, match.start(), title)})")
    lines.append("")
    return "\n".join(lines)


def slug_for_injected(body: str, heading_start: int, title: str) -> str:
    window = body[max(0, heading_start - 200) : heading_start]
    ids = re.findall(r'<a id="([^"]+)"></a>', window)
    if ids:
        return ids[-1]
    return slug(title)


def header() -> str:
    return """# Stable Diffusion Unity Integration — AI support knowledge base

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

"""


def build_text() -> tuple[str, list[str]]:
    body, errors = bundle_sources()
    if errors:
        return "", errors
    toc = table_of_contents(body)
    text = header() + toc + "\n" + body
    if not text.endswith("\n"):
        text += "\n"
    # Reject duplicate explicit anchors.
    ids = re.findall(r'<a id="([^"]+)"></a>', text)
    seen = set()
    for anchor in ids:
        if anchor in seen:
            errors.append(f"duplicate anchor: {anchor}")
        seen.add(anchor)
    # Every rewritten in-page link must hit an anchor.
    for target in re.findall(r"\]\(#([^)]+)\)", text):
        if target not in seen:
            errors.append(f"bundled link has no anchor: #{target}")
    return text, errors


def check_readme() -> list[str]:
    readme = (ROOT / "README.md").read_text(encoding="utf-8")
    errors = []
    if README_HEADING not in readme:
        errors.append(f"README.md is missing the heading '{README_HEADING}'")
    if README_LINK not in readme:
        errors.append("README.md does not link to AI-SUPPORT.md")
    # The help section should appear before the long install steps.
    help_at = readme.find(README_HEADING)
    install_at = readme.find("# Getting Started")
    if help_at == -1 or install_at == -1 or help_at > install_at:
        errors.append("README AI-help section must appear before Getting Started")
    return errors


def main() -> int:
    check_only = "--check" in sys.argv
    text, errors = build_text()
    errors.extend(check_readme())
    if errors:
        for err in errors:
            print(f"error: {err}", file=sys.stderr)
        return 1
    if check_only:
        current = OUTPUT.read_text(encoding="utf-8") if OUTPUT.is_file() else ""
        if current != text:
            print(
                "error: AI-SUPPORT.md is stale or missing. "
                "Run: python tools/build_ai_support.py",
                file=sys.stderr,
            )
            return 1
        print("AI-SUPPORT.md matches the chapter files.")
        return 0
    OUTPUT.write_text(text, encoding="utf-8", newline="\n")
    print(f"Wrote {OUTPUT.relative_to(ROOT)} ({len(text.splitlines())} lines).")
    return 0


if __name__ == "__main__":
    sys.exit(main())
