# Hapticlabs Player API Documentation

## Overview

This document describes the main public classes and methods provided by the Hapticlabs Player Unity package.

---

## `Hapticlabs.Player.HapticlabsService`

Singleton service for managing haptic playback.

### Properties

- `Instance` — Singleton instance.

### Delegates

- `public delegate void CompletionCallback();`
    - Called when haptic playback completes successfully.
- `public delegate void FailureCallback(string errorMessage);`
    - Called when haptic playback fails, providing an error message.

### Cross-Platform (Android and iOS) Methods

- `void Play(string androidPath, string iosPath, CompletionCallback onCompletion = null, FailureCallback onFailure = null)`
  - Plays a haptic pattern from the given paths for Android and iOS. Calls onCompletion once playback has completed, or onFailure if something went wrong.
- `void PlayBuiltIn(string androidEffect, string iosEffect)`
  - Plays a built-in haptic effect.

### Android only Methods

- `void PlayAndroid(string hacPath, CompletionCallback onCompletion = null, FailureCallback onFailure = null)`
    - Plays a .hac file on Android. Calls onCompletion once playback has completed, or onFailure if something went wrong.
- `void PlayPredefinedAndroidEffect(AndroidBuiltInEffect effect)`
    - Plays a built-in Android haptic effect.
- `void PreloadAndroid(string hacPath)`
    - Preloads a .hac file for Android for lower latency playback.
- `void PlayHLA(string hlaPath, CompletionCallback onCompletion = null, FailureCallback onFailure = null)`
    - Plays a .hla file on Android. Calls onCompletion once playback has completed, or onFailure if something went wrong.
- `void PlayHAC(string hacPath, CompletionCallback onCompletion = null, FailureCallback onFailure = null)`
    - Plays a .hac file on Android. Calls onCompletion once playback has completed, or onFailure if something went wrong.
- `void PlayOGG(string oggPath, CompletionCallback onCompletion = null, FailureCallback onFailure = null)`
    - Plays an .ogg file on Android. Calls onCompletion once playback has completed, or onFailure if something went wrong.
- `void PreloadOGG(string oggPath)`
    - Preloads an .ogg file for Android for lower latency playback.
- `void Unload(string directoryOrHacPath)`
    - Unloads a specific .hac file or directory from memory.
- `void UnloadOGG(string oggPath)`
    - Unloads a specific .ogg file from memory.
- `void UnloadAll()`
    - Unloads all preloaded haptic files from memory.

### iOS only Methods
- `void PlayAHAP(string ahapPath, CompletionCallback onCompletion = null, FailureCallback onFailure = null)`
    - Plays an .ahap file on iOS. Calls onCompletion once playback has completed, or onFailure if something went wrong.
- `void SetAHAPHapticsMuted(bool muted)`
    - Mutes or unmutes haptic playback for .ahap files on iOS.
- `bool IsAHAPHapticsMuted()`
    - Returns whether haptic playback is muted for .ahap files on iOS.
- `void SetAHAPAudioMuted(bool muted)`
    - Mutes or unmutes audio playback for .ahap files on iOS.
- `bool IsAHAPAudioMuted()`
    - Returns whether audio playback is muted for .ahap files on iOS.
- `void PlayPredefinedIOSEffect(IOSBuiltInEffect effect)`
    - Plays a built-in iOS haptic effect.