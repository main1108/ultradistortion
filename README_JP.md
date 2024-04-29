# ULTRADistortion

[![English](https://img.shields.io/badge/English-a?style=flat-square&color=blue)](https://github.com/main1108/ultradistortion/blob/master/README.md) [![日本語](https://img.shields.io/badge/%E6%97%A5%E6%9C%AC%E8%AA%9E-a?style=flat-square)](https://github.com/main1108/ultradistortion/blob/master/README_JP.md)

ULTRADistortionはULTRAKILLを爆音にするmodです。

## 使い方

1. BepInExをULTRAKILLに導入します。
2. [リリースページ](https://github.com/main1108/ultradistortion/releases)へ行き、`ultradistortion.dll`をダウンロードします。
3. `ultradistortion.dll`を`gamefolder/BepInEx/plugins'の中に入れます。
4. 完了です。

## 設定(単体)

このmodにはconfigの値が2つあります。
これらのconfigは`susinopo.ULTRADistortion.cfg`から編集できます。

- `AudioDistortionLevel` AudioDistortionFilter.distortionLevelの値(大きいほど爆音になります。最大値:1)
- `OnlyMusic` 音楽以外に影響を与えないようにします(trueかfalse)

**config値の変更はゲームを再起動しない限り反映されません。**

## 設定(PluginConfigurator)

このmodは[PluginConfigurator](https://github.com/eternalUnion/UKPluginConfigurator)に対応しています。
PluginConfiguratorが導入されている場合、cfgファイルは無効化され、config値の変更はconfig画面を閉じた時に反映されるようになります。

## このmodの仕組み

初期状態では、`AudioListener`コンポーネントがアタッチされているゲームオブジェクトに`AudioDistortionFilter`を追加します。
`OnlyMusic`が`true`になっている場合は、音楽を流す`AudioSource`がアタッチされているゲームオブジェクトに`AudioDistortionFilter`を追加します。

## 謝辞

[@kazukazu123123](https://github.com/kazukazu123123) このmodのアイデア
