# ULTRADistortion
ULTRADistortion is a mod to DESTROY your ears.
## How To Use
1. Download and install BepInEx to ULTRAKILL.
2. Go to the [Release page](https://github.com/main1108/ultradistortion/releases) and Download `ultradistortion.dll`
3. Put the `ultradistortion.dll` into the `gamefolder/BepInEx/plugins'.
4. Done.
## Config
There's 2 config values in `susinopo.ULTRADistortion.cfg`.
- `AudioDistortionLevel` Value of AudioDistortionFilter.distortionLevel
- `OnlyMusic` Only music to distorted.
## How it works
in default, it add the AudioDistortionFilter component in the gameobject who have AudioDistortionFilter
if you turns on the OnlyMusic, it attaches AudioDistortionFilter to audiosource which plays music.
