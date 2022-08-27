# TailEffect
Tail effect for Unity. Allows to create tails from prefab segments, 2D and 3D work modes.

![Tail Effect](https://media.giphy.com/media/EL5dXX61usfsWkYYYd/giphy.gif)

This is early version, changes is coming in future.

## How to install
You can install plugin via Unity Package Manager as git package from github repository:

```
https://github.com/OlegDzhuraev/TailEffect.git
```

You also can download it directly from github and place into Assets folder.

## How to use

Setup steps:
1. Add TailFx component to GameObject. Setup segment fields with prefabs of your segments (head/body/tail).
2. Optionally change other settings.
3. Press Play.

Check TailFxExample scene for more info.

## Other features
### Tail color tint
With **TailColorTint** component you can assign color gradient, which be applied to the mesh renderers of tail by length.
![Tail color tint](https://media.giphy.com/media/MfzAEWoceAnoM7TBKt/giphy.gif)

## License
MIT License.
