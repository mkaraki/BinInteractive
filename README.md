BinInteractive / BinView / BinPlayground        {#mainpage}
============

<!--

Dear editor.

`{#mainpage}` is magic for doxygen. Do not remove from `README.md`

-->

[![codecov](https://codecov.io/gh/mkaraki/BinInteractive/graph/badge.svg?token=pkmX7RJLg4)](https://codecov.io/gh/mkaraki/BinInteractive)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/01820668a7be41b3960d8d92fc211391)](https://app.codacy.com/gh/mkaraki/BinInteractive/dashboard?utm_source=gh&utm_medium=referral&utm_content=&utm_campaign=Badge_grade)

BinInteractive is a small binary inspection workspace built around a C# scripting. It is split into several related projects:

- BinInteractive: the console application that opens a file or an empty in-memory stream and lets you inspect it interactively.
- BinPlayground: the reusable library that provides byte helpers, stream utilities, bitmap views, search helpers, and stencil parsing.
- BinView: a small WASM application that uses the BinPlayground library to render a file in a browser.

## What BinInteractive can do

BinInteractive provides a simple interactive C# scripting environment with FileStream.
You can use familiar C# syntax/methods to inspect a file or an in-memory stream.
It also provides some additional commands to make it easier to inspect binary data.

See the [basic.md](docs/basic.md) for a list of commands and examples.

## What BinView can do

BinView is a Blazor WebAssembly application that uses the BinPlayground library to render a file in a browser.

It provides Bitmap view, Hex view, ASCII view, and a simple numeric/text decoder.

You can try it online at [https://binview.mkarakiapps.com/](https://binview.mkarakiapps.com/).

## Generate BinPlayground/BinInteractive documentation

Use Doxygen to generate the documentation for BinPlayground and BinInteractive.

```bash
$ pwd
/path/to/BinInteractive
$ doxygen
$ ls html
︙
index.html
︙
```
