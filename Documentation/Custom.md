# Custom Component

The `Custom` MonoBehaviour allows you to trigger custom haptic feedback patterns in your Unity project using the Hapticlabs Player package.

## Overview

Attach this component to a GameObject to reference and play haptic files for Android and iOS platforms.

## Inspector Fields

- **androidPath**: Path to the Android `.hac` file within `Assets/StreamingAssets` (e.g., `AndroidSamples/Button.hac`).
- **iosPath**: Path to the iOS `.ahap` file within `Assets/StreamingAssets` (e.g., `iOSSamples/Button.ahap`).

## Usage

1. Add the `Custom` component to a GameObject in your scene.
2. Set the `androidPath` and `iosPath` fields to the relative paths of your haptic files in `Assets/StreamingAssets`.
3. Call `TriggerHaptics()` (e.g., from a button click) to play the haptic feedback.

## Example

```csharp
using Hapticlabs.UnityAndroid;

public class MyButton : MonoBehaviour
{
    public Custom hapticCustom;

    public void OnButtonPressed()
    {
        hapticCustom.TriggerHaptics();
    }
}
```

## Notes

- Ensure the referenced haptic files are present in `Assets/StreamingAssets`.
- The component validates paths and warns if files are missing or invalid.

For more details, see the main [API documentation](./API.md) or visit [hapticlabs.io/mobile](https://hapticlabs.io/mobile).
