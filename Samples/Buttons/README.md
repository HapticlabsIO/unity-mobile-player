# Buttons Sample

This sample demonstrates how to use the Hapticlabs Player package to trigger haptic feedback in your Unity mobile project using button interactions.

## What the Sample Does

- Shows two interactive UI buttons.
- Each button triggers haptic feedback using the Hapticlabs Player runtime scripts.
  - One button uses Built-In haptic effects. One effect can be chosen for each iOS and Android.
  - The other button uses Custom haptic effects. A .hac file for Android and a .ahap file for iOS can be chosen from the Assets/StreamingAssets folder.

## Setup Instructions

Custom haptics are file-based. For Android and iOS to be able to play back these files, they need to be present on the mobile device. In Unity, the Assets/StreamingAssets folder is packaged with the application. This package searches this folder for custom haptic files.

### 1. Copy StreamingAssets

The custom haptics sample requires custom haptic files to be present in your project's `Assets/StreamingAssets` directory.

- After importing the sample, locate the `Samples/HapticlabsPlayer/<version>/Buttons/Assets/StreamingAssets` folder in your project.
- If `Assets/StreamingAssets` does not exist in your project, create it.
- Manually copy all files and folders from `Samples/HapticlabsPlayer/<version>/Buttons/Assets/StreamingAssets` to your project's `Assets/StreamingAssets` directory.

### 2. Reference StreamingAssets in Scripts

- In the sample scene, button scripts reference custom haptic files by their relative path within `Assets/StreamingAssets`.
- When you first import the sample, the Custom Haptics button does not reference any haptic files (so it won't create any haptics). Select `AndroidSamples/Button.hac` for Android, and `iosSamples/Button.ahap` for iOS.
- Feel free to try out the other provided haptic files. Once you are ready to create your own custom haptic effects, visit [https://www.hapticlabs.io/mobile](https://www.hapticlabs.io/mobile).

## Build Settings

- To run this sample on your device, add the sample scene to your project's Build Settings and set it as the first scene.

## Learn More

For more information and advanced usage, visit [hapticlabs.io/mobile](https://www.hapticlabs.io/mobile).

---

Enjoy exploring haptic feedback with Hapticlabs Player!
