
using UnityEngine;
using System;
using AOT;
using System.Threading;
using System.Collections.Concurrent;
#if UNITY_IOS && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace Hapticlabs.Player
{

    public enum IOSBuiltInEffect
    {
        Light,
        Medium,
        Heavy,
        Rigid,
        Soft,
        Error,
        Warning,
        Success,
        Selection,
    };

    public enum AndroidBuiltInEffect
    {
        Click,
        DoubleClick,
        HeavyClick,
        Tick
    };

    public class HapticlabsService
    {
        public delegate void CompletionCallback();
        public delegate void FailureCallback(string errorMessage);

        private static HapticlabsService _instance;
        public static HapticlabsService Instance => _instance ?? (_instance = new HapticlabsService());

#if UNITY_ANDROID && !UNITY_EDITOR
        private AndroidJavaObject hapticlabsPlayer;

        // Callback proxy for Kotlin Function
        private class PlayCallback : AndroidJavaProxy
        {
            private CompletionCallback completionCallback;

            public PlayCallback(CompletionCallback completionCallback) : base("kotlin.jvm.functions.Function0")
            {
                this.completionCallback = completionCallback;
            }

            public object invoke()
            {
                completionCallback?.Invoke();
                return null;
            }
        }
#endif

        private HapticlabsService()
        {
            // Optional: platform-specific initialization
#if UNITY_ANDROID && !UNITY_EDITOR
			// Android initialization here
            using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
            using (var currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity"))
            {
                this.hapticlabsPlayer = new AndroidJavaObject("io.hapticlabs.hapticlabsplayer.HapticlabsPlayer", currentActivity);
            }
#elif UNITY_IOS && !UNITY_EDITOR
			_initializeIOSHapticlabsPlayer();
#endif
        }

        ~HapticlabsService()
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (hapticlabsPlayer != null)
            {
                hapticlabsPlayer.Dispose();
                hapticlabsPlayer = null;
            }
#endif
        }

        /// <summary>
        /// Plays a haptic pattern for the current platform (Android or iOS).
        /// </summary>
        /// <param name="androidPath">Path to the Android .hac file in Android's assets/ directory. For files in Unity's Assets/StreamingAssets, use the relative path, e.g. for "Assets/StreamingAssets/AndroidSamples/Button.hac", use "AndroidSamples/Button.hac"</param>
        /// <param name="iosPath">Path to the iOS .ahap file in the iOS app's main bundle. For files in Unity's Assets/StreamingAssets, add the prefix "Data/Raw/", e.g. for "Assets/StreamingAssets/iOSSamples/Button.ahap", use "Data/Raw/iOSSamples/Button.ahap"</param>
        /// <param name="onCompletion">Callback invoked on successful playback.</param>
        /// <param name="onFailure">Callback invoked if playback fails.</param>
        public void Play(string androidPath, string iosPath, CompletionCallback onCompletion = null, FailureCallback onFailure = null)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            PlayAndroid(androidPath, onCompletion, onFailure);
#elif UNITY_IOS && !UNITY_EDITOR
            PlayAHAP(iosPath, onCompletion, onFailure);
#else
            Debug.Log($"[Hapticlabs.Player.Service] Play called with patterns '{androidPath}' and '{iosPath}' (no-op in editor or unsupported platform)");
#endif
        }

        /// <summary>
        /// Plays a built-in haptic effect for the current platform.
        /// </summary>
        /// <param name="androidEffect">Android built-in effect enum value.</param>
        /// <param name="iosEffect">iOS built-in effect enum value.</param>
        public void PlayBuiltIn(AndroidBuiltInEffect androidEffect, IOSBuiltInEffect iosEffect)
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            PlayPredefinedAndroidEffect(androidEffect);
#elif UNITY_IOS && !UNITY_EDITOR
            PlayPredefinedIOSEffect(iosEffect);
#else
            Debug.Log($"[Hapticlabs.Player.Service] PlayBuiltIn called with effects '{androidEffect}, {iosEffect}' (no-op in editor or unsupported platform)");
#endif
        }

#if UNITY_ANDROID && !UNITY_EDITOR
            /// <summary>
            /// Plays a .hac file on Android.
            /// </summary>
            /// <param name="hacPath">Path to the .hac file. For files in Unity's Assets/StreamingAssets, use the relative path, e.g. for "Assets/StreamingAssets/AndroidSamples/Button.hac", use "AndroidSamples/Button.hac"</param>
            /// <param name="onCompletion">Callback invoked on successful playback.</param>
            /// <param name="onFailure">Callback invoked if playback fails.</param>
		public void PlayAndroid(string hacPath, CompletionCallback onCompletion = null, FailureCallback onFailure = null)
		{
			try
			{
                hapticlabsPlayer.Call("play", hacPath, new PlayCallback(onCompletion));
			}
			catch (Exception e)
			{
				Debug.LogError($"[Hapticlabs.Player.Service] Android play error: {e.Message}");
                onFailure?.Invoke(e.Message);
			}
		}

            /// <summary>
            /// Plays a predefined Android haptic effect.
            /// </summary>
            /// <param name="effect">Android built-in effect enum value.</param>
        public void PlayPredefinedAndroidEffect(AndroidBuiltInEffect effect)
        {
            try
            {
                switch (effect)
                {
                    case AndroidBuiltInEffect.Click:
                        hapticlabsPlayer.Call("playBuiltIn", "Click");
                        break;
                    case AndroidBuiltInEffect.DoubleClick:
                        hapticlabsPlayer.Call("playBuiltIn", "Double Click");
                        break;
                    case AndroidBuiltInEffect.HeavyClick:
                        hapticlabsPlayer.Call("playBuiltIn", "Heavy Click");
                        break;
                    case AndroidBuiltInEffect.Tick:
                        hapticlabsPlayer.Call("playBuiltIn", "Tick");
                        break;
                    default:
                        Debug.LogError("Unknown Android built-in effect: " + effect);
                        break;
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[Hapticlabs.Player.Service] Android play built-in effect error: {e.Message}");
            }
        }

            /// <summary>
            /// Preloads a .hac file for Android for lower latency playback.
            /// </summary>
            /// <param name="hacPath">Path to the .hac file. For files in Unity's Assets/StreamingAssets, use the relative path, e.g. for "Assets/StreamingAssets/AndroidSamples/Button.hac", use "AndroidSamples/Button.hac"</param>
        public void PreloadAndroid(string hacPath)
        {
            try
            {
                hapticlabsPlayer.Call("preload", hacPath);
            }
            catch (Exception e)
            {
                Debug.LogError($"[Hapticlabs.Player.Service] Android preload error: {e.Message}");
            }
        }

            /// <summary>
            /// Plays a .hla file on Android.
            /// </summary>
            /// <param name="hlaPath">Path to the .hla file. For files in Unity's Assets/StreamingAssets, use the relative path, e.g. for "Assets/StreamingAssets/AndroidSamples/Button.hla", use "AndroidSamples/Button.hla"</param>
            /// <param name="onCompletion">Callback invoked on successful playback.</param>
            /// <param name="onFailure">Callback invoked if playback fails.</param>
        public void PlayHLA(string hlaPath, CompletionCallback onCompletion = null, FailureCallback onFailure = null)
        {
            try
            {
                hapticlabsPlayer.Call("playHLA", hlaPath, new PlayCallback(onCompletion));
            }
            catch (Exception e)
            {
                Debug.LogError($"[Hapticlabs.Player.Service] Android playHLA error: {e.Message}");
                onFailure?.Invoke(e.Message);
            }
        }

            /// <summary>
            /// Plays a .hac file on Android.
            /// </summary>
            /// <param name="hacPath">Path to the .hac file. For files in Unity's Assets/StreamingAssets, use the relative path, e.g. for "Assets/StreamingAssets/AndroidSamples/Button.hac", use "AndroidSamples/Button.hac"</param>
            /// <param name="onCompletion">Callback invoked on successful playback.</param>
            /// <param name="onFailure">Callback invoked if playback fails.</param>
        public void PlayHAC(string hacPath, CompletionCallback onCompletion = null, FailureCallback onFailure = null)
        {
            try
            {
                hapticlabsPlayer.Call("playHAC", hacPath, new PlayCallback(onCompletion));
            }
            catch (Exception e)
            {
                Debug.LogError($"[Hapticlabs.Player.Service] Android playHAC error: {e.Message}");
                onFailure?.Invoke(e.Message);
            }
        }

            /// <summary>
            /// Plays an .ogg file on Android.
            /// </summary>
            /// <param name="oggPath">Path to the .ogg file. For files in Unity's Assets/StreamingAssets, use the relative path, e.g. for "Assets/StreamingAssets/AndroidSamples/Button.ogg", use "AndroidSamples/Button.ogg"</param>
            /// <param name="onCompletion">Callback invoked on successful playback.</param>
            /// <param name="onFailure">Callback invoked if playback fails.</param>
        public void PlayOGG(string oggPath, CompletionCallback onCompletion = null, FailureCallback onFailure = null)
        {
            try
            {
                hapticlabsPlayer.Call("playOGG", oggPath, new PlayCallback(onCompletion));
            }
            catch (Exception e)
            {
                Debug.LogError($"[Hapticlabs.Player.Service] Android playOGG error: {e.Message}");
                onFailure?.Invoke(e.Message);
            }
        }

            /// <summary>
            /// Preloads an .ogg file for Android for lower latency playback.
            /// </summary>
            /// <param name="oggPath">Path to the .ogg file. For files in Unity's Assets/StreamingAssets, use the relative path, e.g. for "Assets/StreamingAssets/AndroidSamples/Button.ogg", use "AndroidSamples/Button.ogg"</param>
        public void PreloadOGG(string oggPath)
        {
            try
            {
                hapticlabsPlayer.Call("preloadOGG", oggPath);
            }
            catch (Exception e)
            {
                Debug.LogError($"[Hapticlabs.Player.Service] Android preloadOGG error: {e.Message}");
            }
        }

            /// <summary>
            /// Unloads a specific .hac file or directory from memory.
            /// </summary>
            /// <param name="directoryOrHacPath">Path to the .hac file. For files in Unity's Assets/StreamingAssets, use the relative path, e.g. for "Assets/StreamingAssets/AndroidSamples/Button.hac", use "AndroidSamples/Button.hac"</param>
        public void Unload(string directoryOrHacPath)
        {
            try
            {
                hapticlabsPlayer.Call("unload", directoryOrHacPath);
            }
            catch (Exception e)
            {
                Debug.LogError($"[Hapticlabs.Player.Service] Android unload error: {e.Message}");
            }
        }

            /// <summary>
            /// Unloads a specific .ogg file from memory.
            /// </summary>
            /// <param name="oggPath">Path to the .ogg file. For files in Unity's Assets/StreamingAssets, use the relative path, e.g. for "Assets/StreamingAssets/AndroidSamples/Button.ogg", use "AndroidSamples/Button.ogg"</param>
        public void UnloadOGG(string oggPath)
        {
            try
            {
                hapticlabsPlayer.Call("unloadOGG", oggPath);
            }
            catch (Exception e)
            {
                Debug.LogError($"[Hapticlabs.Player.Service] Android unloadOGG error: {e.Message}");
            }
        }

            /// <summary>
            /// Unloads all preloaded haptic files from memory.
            /// </summary>
        public void UnloadAll()
        {
            try
            {
                hapticlabsPlayer.Call("unloadAll");
            }
            catch (Exception e)
            {
                Debug.LogError($"[Hapticlabs.Player.Service] Android unloadAll error: {e.Message}");
            }
        }
#endif

#if UNITY_IOS && !UNITY_EDITOR
    private delegate void _CompletionCallbackWithId(int id);
    private delegate void _FailureCallbackWithId(int id, string errorMessage);

    // Map of passed callbacks
    private static ConcurrentDictionary<int, (CompletionCallback, FailureCallback)> iosCallbacks = new ConcurrentDictionary<int, (CompletionCallback, FailureCallback)>();
    private static int iosCallbackIdCounter = 0;
    [DllImport("__Internal")]
    private static extern void _initializeIOSHapticlabsPlayer();

    [DllImport("__Internal")]
    private static extern void _playPredefinedIOSVibration(string patternName);

    [DllImport("__Internal")]
    private static extern void _playIOSAHAP(string ahapPath, int callbackId, _CompletionCallbackWithId onCompletion,
                                            _FailureCallbackWithId onFailure);

    [DllImport("__Internal")]
    private static extern void _setIOSAHAPHapticsMuted(bool muted);

    [DllImport("__Internal")]
    private static extern bool _isIOSAHAPHapticsMuted();

    [DllImport("__Internal")]
    private static extern void _setIOSAHAPAudioMuted(bool muted);

    [DllImport("__Internal")]
    private static extern bool _isIOSAHAPAudioMuted();

    [MonoPInvokeCallback(typeof(_CompletionCallbackWithId))]
    private static void OnIOSAHAPPlayComplete(int id) {
        // Find and remove callbacks from dictionary
        if (iosCallbacks.TryRemove(id, out var callbacks)) {
            // Invoke the completion callback
            callbacks.Item1();
        }
    }

    [MonoPInvokeCallback(typeof(_FailureCallbackWithId))]
    private static void OnIOSAHAPPlayFailure(int id, string errorMessage) {
        // Find and remove callbacks from dictionary
        if (iosCallbacks.TryRemove(id, out var callbacks)) {
            // Invoke the failure callback
            callbacks.Item2(errorMessage);
        }
    }

    private static int GetNextIOSCallbackId() {
        return Interlocked.Increment(ref iosCallbackIdCounter);
    }

        /// <summary>
        /// Plays an .ahap file on iOS.
        /// </summary>
        /// <param name="ahapPath">Path to the .ahap file. For files in Unity's Assets/StreamingAssets, add the prefix "Data/Raw/", e.g. for "Assets/StreamingAssets/iOSSamples/Button.ahap", use "Data/Raw/iOSSamples/Button.ahap"</param>
        /// <param name="onCompletion">Callback invoked on successful playback.</param>
        /// <param name="onFailure">Callback invoked if playback fails.</param>
    public void PlayAHAP(string ahapPath, CompletionCallback onCompletion = null, FailureCallback onFailure = null)
    {
        int callbackId = GetNextIOSCallbackId();
        iosCallbacks[callbackId] = (onCompletion ?? delegate { }, onFailure ?? delegate { });

        _playIOSAHAP(ahapPath, callbackId, OnIOSAHAPPlayComplete, OnIOSAHAPPlayFailure);
    }

        /// <summary>
        /// Mutes or unmutes haptic playback for .ahap files on iOS.
        /// </summary>
        /// <param name="muted">True to mute, false to unmute.</param>
    public void SetAHAPHapticsMuted(bool muted)
    {
        _setIOSAHAPHapticsMuted(muted);
    }

        /// <summary>
        /// Returns whether haptic playback is muted for .ahap files on iOS.
        /// </summary>
    public bool IsAHAPHapticsMuted()
    {
        return _isIOSAHAPHapticsMuted();
    }

        /// <summary>
        /// Mutes or unmutes audio playback for .ahap files on iOS.
        /// </summary>
        /// <param name="muted">True to mute, false to unmute.</param>
    public void SetAHAPAudioMuted(bool muted)
    {
        _setIOSAHAPAudioMuted(muted);
    }

        /// <summary>
        /// Returns whether audio playback is muted for .ahap files on iOS.
        /// </summary>
    public bool IsAHAPAudioMuted()
    {
        return _isIOSAHAPAudioMuted();
    }

        /// <summary>
        /// Plays a built-in iOS haptic effect.
        /// </summary>
        /// <param name="effect">iOS built-in effect enum value.</param>
    public void PlayPredefinedIOSEffect(IOSBuiltInEffect effect)
    {
        switch (effect)
        {
            case IOSBuiltInEffect.Light:
                _playPredefinedIOSVibration("light");
                break;
            case IOSBuiltInEffect.Medium:
                _playPredefinedIOSVibration("medium");
                break;
            case IOSBuiltInEffect.Heavy:
                _playPredefinedIOSVibration("heavy");
                break;
            case IOSBuiltInEffect.Rigid:
                _playPredefinedIOSVibration("rigid");
                break;
            case IOSBuiltInEffect.Soft:
                _playPredefinedIOSVibration("soft");
                break;
            case IOSBuiltInEffect.Error:
                _playPredefinedIOSVibration("error");
                break;
            case IOSBuiltInEffect.Warning:
                _playPredefinedIOSVibration("warning");
                break;
            case IOSBuiltInEffect.Success:
                _playPredefinedIOSVibration("success");
                break;
            case IOSBuiltInEffect.Selection:
                _playPredefinedIOSVibration("selection");
                break;
            default:
                Debug.LogError("Unknown iOS built-in effect: " + effect);
                break;
    }}
#endif
    }
}
