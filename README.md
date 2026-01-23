# Hapticlabs Player for Unity

Hapticlabs Player is a Unity package for adding advanced haptic feedback to your mobile games and apps. It supports both Android and iOS, allowing you to trigger custom haptic patterns from your Unity scripts.

## Features

- Simple API for triggering haptics
- Supports Android (.hac) and iOS (.ahap) haptic files
- Editor and runtime integration
- Sample scenes for quick start

## Installation

Add this package to your Unity project using the Unity Package Manager (UPM).

## Usage

0. [Optional, but recommended] Create custom haptic files with [Hapticlabs Studio](https://www.hapticlabs.io/mobile).
1. Add the provided scripts (e.g., `Custom`, `BuiltIn`) to your GameObjects.
2. Add your custom haptic files to `Assets/StreamingAssets`.
3. Reference your custom haptic files by their relative path in `Assets/StreamingAssets`.
4. Use the scripts' `TriggerHaptics` methods to play the haptics.

### Advanced

You can directly interface with the `Services` API for full control. See
[API documentation](Documentation/API.md).

## Samples

- Import sample scenes from the Package Manager window under the Hapticlabs Player package.
- Follow the instructions in each sample's README for setup (including copying StreamingAssets if required).

## Documentation

- See the [Documentation](Documentation/) folder for API details and advanced usage.
- Visit [hapticlabs.io/mobile](https://hapticlabs.io/mobile) for the latest guides and resources.

## Support

For help and feedback, visit [hapticlabs.io/mobile](https://hapticlabs.io/mobile) or contact support via the website.

---

Enjoy building with Hapticlabs Player!
