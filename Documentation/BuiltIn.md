# BuiltIn Component

The `BuiltIn` MonoBehaviour provides a simple way to trigger built-in haptic effects using the Hapticlabs Player package.

## Overview

Attach this component to a GameObject to play predefined haptic effects for Android and iOS.

## Inspector Fields

- **androidEffect**: The name of the built-in Android haptic effect to play (see `AndroidBuiltInEffect` enum).
- **iosEffect**: The name of the built-in iOS haptic effect to play (see `IOSBuiltInEffect` enum).

## Usage

1. Add the `BuiltIn` component to a GameObject in your scene.
2. Set the `androidEffect` and `iosEffect` fields to the desired effect names.
3. Call `TriggerHaptics()` (e.g., from a button click) to play the built-in effect.

## Example

```csharp
using Hapticlabs.UnityAndroid;

public class MyButton : MonoBehaviour
{
    public BuiltIn hapticBuiltIn;

    public void OnButtonPressed()
    {
        hapticBuiltIn.TriggerHaptics();
    }
}
```

## Notes

- Refer to the enums in the API documentation for available effect names.

For more details, see the main [API documentation](./API.md) or visit [hapticlabs.io/mobile](https://hapticlabs.io/mobile).
