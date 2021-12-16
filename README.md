# Gabby Dialogue Sample
### An interactive sample for the Gabby Dialogue Language made with Unity.
### **This is version 1.0 of the sample game, using Gabby 0.2.**
### [**Play the sample in your browser on itch.io here!**](https://potassium-k.itch.io/gabby)

<br>

Gabby is a language made for game dialogue. It makes it easy to build games and visual novels with dialogue at their core.

<br>

Working with Gabby is easy:

```
[Gabby.HelloWorld]

    (Gabby) Hello, world!
    -       ...
    -       Hello?

    (Kay)   Hey Gabby.

    (Gabby) Hi!
```
<br>

Read more about Gabby at the [dialogue engine repository](https://github.com/zdawson/gabby-dialogue-engine), grab the [vscode plugin](https://marketplace.visualstudio.com/vscode), or check out the [language spec](https://github.com/zdawson/gabby)!

<br>

## About the Sample

This is the repository for the sample game built using Unity. It demonstrates some common use cases for Gabby, and shows how to implement a dialogue system using the Gabby Dialogue Engine for Unity.

- [Assets/GameSample/Dialogue/](Assets/GameSample/Dialogue/) - Contains the gabby dialogue scripts for the sample. Refer to these as you play the game sample to get an idea of how everything works.

- [Assets/GameSample/Scripts/](Assets/GameSample/Scripts/) - Contains reusable scripts that demonstrate use of the dialogue engine, along with features like portraits and a text typewriter effect. You could use these as a starting point in your own project.

- [Assets/GameSample/Scripts/Game/](Assets/GameSample/Scripts/Game/) - Contains game sample specific scripts.

- [Assets/GameSample/Dialogue/Characters](Assets/GameSample/Dialogue/Characters) - Contains game sample specific scriptable objects that define the characters and their portraits.

- [Packages/gabby-dialogue-engine/](Packages/) - Contains the dialogue engine project as a submodule.

<br>

## Cloning

This project uses git LFS - if you haven't worked with git LFS, you can [read about it here.](http://arfc.github.io/manual/guides/git-lfs).

Additionally, this project uses git submodules - include `--recurse-submodules -j8` (or `--recursive` for older versions of git) when cloning.

Alternatively, clone normally and then run `git submodule update --init --recursive`.

**TL;DR**

With LFS installed, either:
```
git clone --recurse-submodules -j8 https://github.com/zdawson/gabby-dialogue-sample GabbySample
```
or
```
git clone https://github.com/zdawson/gabby-dialogue-sample GabbySample
cd GabbySample
git submodule update --init --recursive
```

If you just want the dialogue engine without the sample assets, [clone the dialogue engine directly.](https://github.com/zdawson/gabby-dialogue-engine)

<br>

## License

This project is multi-licensed.

The code in this repository is released under the MIT license. [You can read about this license here.](https://choosealicense.com/licenses/mit/)

Specifically, the following files and directories are licensed under the MIT license:
- All Gabby dialogue scripts (.gab)
- All C# scripts (.cs)
- All Unity scenes (.unity), excluding any references to or dependencies on other files not licensed under MIT
- All Unity prefabs (.prefab), excluding any references to or dependencies on other files not licensed under MIT
- Unity project files, excluding any references to or dependencies on other files not licensed under MIT

<br>

The artwork in this repository is protected under the Creative Commons Attribution-NonCommercial 4.0 International Public License (CC-BY-NC 4.0). [You can read about this license here.](https://creativecommons.org/licenses/by-nc/4.0/legalcode)

Specifically, the following files and directories are licensed under the CC-BY-NC 4.0 license:
- All image files (including but not limited to .png, .jpg, .ase)

<br>

Lastly, this project uses TextMeshPro as well as the LiberationSans font included with it by default.
- TextMeshPro is licensed under the [Unity Companion License.](https://unity3d.com/legal/licenses/Unity_Companion_License)
- LiberationSans is licensed under the [Open Font License.](Assets/TestMesh Pro/LiberationSans - OFL.txt)

**TL;DR**

- The short version is that you may freely modify the sample project including its scripts and assets, but you may not use the art assets for a commercial project.
- You may use the scripts for any project including commercial, as outlined by the MIT license.
- If you're going to use the art assets in a non commercial project, please read carefully how a commercial project is defined in the CC-BY-NC 4.0 license - hosting a project with ads, for example, is not permitted.
- Refer to the links above for the most accurate and complete description of the licenses.

<br>

## Links

- [gabby-dialogue-spec](https://github.com/zdawson/gabby) - The language specification.

- [gabby-dialogue-engine](https://github.com/zdawson/gabby-dialogue-engine) - The dialogue engine repo, without any of the sample content. Ideal for use in a game.

- [gabby-dialogue-sample](https://github.com/zdawson/gabby-dialogue-sample) - This repository, the game sample project.

- [vscode plugin](https://marketplace.visualstudio.com/vscode) - The plugin for Visual Studio Code.

- [itch.io sample](https://potassium-k.itch.io/gabby) - This sample, hosted on itch.io.
