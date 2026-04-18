# ULTRADistortion

[![English](https://img.shields.io/badge/English-a?style=flat-square&color=blue)](https://github.com/main1108/ultradistortion/blob/master/README.md) [![日本語](https://img.shields.io/badge/%E6%97%A5%E6%9C%AC%E8%AA%9E-a?style=flat-square)](https://github.com/main1108/ultradistortion/blob/master/README_JP.md)

ULTRADistortion is a mod that can make the game sound EXTREMELY LOUD.

## How To Use

1. Download and install BepInEx to ULTRAKILL.
2. Go to the [Release page](https://github.com/main1108/ultradistortion/releases) and Download `ultradistortion.dll`
3. Put the `ultradistortion.dll` into the `gamefolder/BepInEx/plugins'.
4. Done.

## Config(Standalone)

There's 2 config values in `susinopo.ULTRADistortion.cfg`.

- `AudioDistortionLevel` Value of AudioDistortionFilter.distortionLevel
- `OnlyMusic` Only music to distorted.

**Changes to these values are not applied until the game restart.**

## Config(PluginConfigurator)

This mod supports [PluginConfigurator](https://github.com/eternalUnion/UKPluginConfigurator)
If PluginConfigurator is installed to ULTRAKILL, it ignores the .cfg file.
config changes from PluginConfigurator are applied as the config menu closes.

## How it works

In default, it adds the AudioDistortionFilter component to GameObject have AudioListener component.
When `OnlyMusic` is true, it attaches the AudioDistortionFilter to corresponding AudioSource.

## acknowledgments

[@kazukazu123123](https://github.com/kazukazu123123) for Idea of this mod.
